using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EPiServer.PlugIn;
using EPiServer;
using EPiServer.Core;
using DagensIndustri.Tools.Classes;
using DIClassLib.Misc;
using DIClassLib.DbHandlers;
using System.Data;
using DIClassLib.DbHelpers;
using System.Text;

namespace DagensIndustri.Tools.Jobs.GasellMail
{
    [ScheduledPlugIn(DisplayName = "Send Gasell welcome mail", Description = "Send Gasell welcome mail to customers")]
    public class GasellMail
    {
        public static string Execute()
        {
            StringBuilder log = new StringBuilder();
            
            string propId_SendDate = "WelcomeMailSendDate";
            string propId_Subject  = "WelcomeMailSubject";
            string propId_Body     = "WelcomeMailBody";

            //get gaselMeeting pages in gasell meetings folder
            PageData gasellContainer = DataFactory.Instance.GetPage(EPiFunctions.SettingsPageSetting(null, "GasellMeetingsContainer") as PageReference);
            foreach (PageData pd in DataFactory.Instance.GetChildren(gasellContainer.PageLink))
            {
                if (!EPiFunctions.IsMatchingPageType(pd, pd.PageTypeID, "GasellMeetingPageType"))
                    continue;

                if (!EPiFunctions.HasValue(pd, propId_SendDate))
                    continue;

                //send date is today
                DateTime pdSendDate = DateTime.MinValue;
                DateTime.TryParse(pd[propId_SendDate].ToString(), out pdSendDate);
                if (pdSendDate.Date == DateTime.Now.Date)
                {
                    int epiPageId = pd.PageLink.ID;

                    //welcome mails has already been sent
                    DateTime dtAlreadySent = MsSqlHandler.GetGasellWelcomeMailDate(epiPageId);
                    if (dtAlreadySent > DateTime.MinValue)
                    {
                        log.Append("No new mails sent for EpiPageId:" + epiPageId + ". Reason: already sent:" + dtAlreadySent.ToString() + ". ");
                        continue;
                    }
                    else  //try send welcome mails
                    {
                        string subject = (EPiFunctions.HasValue(pd, propId_Subject)) ? pd[propId_Subject].ToString() : null;
                        string body = (EPiFunctions.HasValue(pd, propId_Body)) ? pd[propId_Body].ToString() : null;
                        if (string.IsNullOrEmpty(subject) || string.IsNullOrEmpty(body))
                        {
                            log.Append("Mails NOT sent for EpiPageId:" + epiPageId + ". Reason: subject/body was empty. ");
                            continue;
                        }

                        List<string> mailAddresses = GetValidMailAddresses(epiPageId);
                        SendEmails(mailAddresses, subject, body);

                        //insert row in DB table GasellWelcomeMail
                        MsSqlHandler.InsertGasellWelcomeMailRow(epiPageId);

                        log.Append("Mails sent for EpiPageId:" + epiPageId + ". Number of mails:" + mailAddresses.Count + ". ");
                    }
                }
            }

            return log.ToString();
        }


        private static List<string> GetValidMailAddresses(int epiPageId)
        {
            HashSet<string> mails = new HashSet<string>();
            DataSet ds = MsSqlHandler.GetGasellSignedUpMailAddresses(epiPageId);
            if (DbHelpMethods.DataSetHasRows(ds))
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string s = dr["mail"].ToString();
                    if (MiscFunctions.IsValidEmail(s))
                        mails.Add(s);
                }
            }

            return mails.ToList();
        }
        
        private static void SendEmails(List<string> mailAddresses, string subject, string body)
        {
            foreach (string mailAdr in mailAddresses)
            {
                try
                {
                    MiscFunctions.SendMail("gasell@di.se", mailAdr, subject, body, true);
                }
                catch (Exception ex)
                {
                    new Logger("SendEmails() failed for mail address: " + mailAdr, ex.ToString());
                }
            }
        }


    }
}