using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DIClassLib.DbHandlers;
using DIClassLib.Misc;
using System.Data;


namespace DIClassLib.WineTipSms
{
    public static class WineTipSmsHandler
    {

        public static void SendWineTipSms(int smsMessId)
        {
            List<int> cusnos = GetCusnos();
            if (cusnos.Count == 0)
                return;

            List<WineTipCustomer> wtCusts = MdbDbHandler.GetWineTipCustomers(cusnos);
            EPiJobs.Apsis.ApsisWsHandler ws = new EPiJobs.Apsis.ApsisWsHandler();
            string mess = GetSmsText(smsMessId);

            if (mess.Length > 0)
            {
                foreach (WineTipCustomer wc in wtCusts)
                {
                    if (!MiscFunctions.IsValidPhoneMobile(wc.PhoneMobile))
                    {
                        SmsDbHandler.InsertBadPhoneMobile(wc.Cusno, wc.PhoneMobile);
                        continue;
                    }
                    
                    string apsisKey = ws.ApsisSendSms(mess, wc.PhoneMobile);
                    SmsDbHandler.InsertCustInSmsMess(smsMessId, wc.Cusno, apsisKey);
                }

                SmsDbHandler.SetSmsMessDateSent(smsMessId);
            }
        }


        private static List<int> GetCusnos()
        {
            List<int> cusnos = new List<int>();
            DataSet ds = SmsDbHandler.GetCusnosInSmsGroup(WineTipSettings.SmsGroupId_WineTip);

            if (ds != null && ds.Tables != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                    cusnos.Add(int.Parse(dr["cusno"].ToString()));
            }

            return cusnos;
        }


        private static string GetSmsText(int smsMessId)
        {
            DataSet ds = SmsDbHandler.GetSmsMess(smsMessId);
            if (ds != null && ds.Tables != null && ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0]["smsText"].ToString();

            return string.Empty;
        }

        

    }
}
