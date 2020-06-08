using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

using DIClassLib.DbHandlers;
using DIClassLib.EPiJobs.Apsis;
using DIClassLib.Misc;

using EPiServer.PlugIn;

namespace DagensIndustri.Tools.Jobs.Apsis
{
    [ScheduledPlugIn(DisplayName = "Apsis send mail from list", Description = "Skickar välkomstmail utifrån lista med Cirix cusnos i en fil på disk. Aktivera endast jobbet när det ska användas!")]
    public class ApsisSendMailFromList
    {
        public static string Execute()
        {
            var db = new MailSenderDbHandler();
            var batch = new ApsisBatch();      //new batch each time the program executes

            if (batch.BatchId == 0)         //mssql db connection probably down
                return "Kunde ej skapa ny batch. MSSQL-databas förmodligen ej tillgänglig. Se logg.";

            //TODO: Read from file!
            var filePath = ConfigurationManager.AppSettings["ApsisSendMailFromList"];
            if (string.IsNullOrEmpty(filePath))
            {
                return "Har ingen filsökväg. Vänligen sätt den i appsettings.config som ApsisSendMailFromList";
            }
            var textFileContent = System.IO.File.ReadAllText(filePath);
            if (string.IsNullOrEmpty(textFileContent))
            {
                return "Filen innehöll inget.";
            }
            var customerList = textFileContent.Split(',');
            var customersForMail = CirixDbHandler.CirixGetCustsManuallyFromList(customerList);
            db.AddNewCusts(customersForMail);

            TrySendEmails(customersForMail, batch);
            return BuildLogMessage(batch, customersForMail.Count);
        }

        private static void TrySendEmails(List<ApsisCustomer> custs, ApsisBatch batch)
        {
            var db = new MailSenderDbHandler();
            var ws = new ApsisWsHandler();

            foreach (var c in custs)
            {
                //apsis does not send messages to badly formated addresses, so fake bounce
                if (!MiscFunctions.IsValidEmail(c.Email))
                {
                    db.FlagCustAsBounce(c.CustomerId);
                    continue;
                }

                //avoid same cust in several batches if ApsisSendEmail() reply is slow
                db.FlagCustAsSent(c.CustomerId); //isBounce=0, forceRetry=0

                var identifier = Guid.NewGuid().ToString();

                //TODO: Activate this on send!!!!!
                var mailId = ws.ApsisSendEmail(identifier, c);

                if (mailId > 0)
                    db.InsertCustomerInBatch(identifier, c, batch, mailId);
                else
                {
                    try
                    {
                        db.SetForceRetry(c.CustomerId, true, false);
                    } //send failure: include in next batch (forceRetry=1) 
                    catch
                    {
                        //silent EX - log is written in SetForceRetry() method
                    }
                }

            }
        }

        private static string BuildLogMessage(ApsisBatch batch, int numCustsNew)
        {
            var ret = new StringBuilder();
            ret.Append("BatchId: " + batch.BatchId + ", datum: " + batch.DateBatch);
            
            if (numCustsNew > 0)
                ret.Append("<br>Antal processade: " + numCustsNew);

            return ret.ToString();
        }
    }
}