using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using DIClassLib.DbHelpers;
using DIClassLib.DbHandlers;
using System.Data;
using DIClassLib.EPiJobs.Apsis;
using DIClassLib.Misc;


namespace DagensIndustri.WebServicesPublic.S2
{
    /// <summary>
    /// Summary description for S2
    /// </summary>
    [WebService(Namespace = "http://ws.dagensindustri.se/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class S2 : System.Web.Services.WebService
    {

        int SUCCESS = 0;
        int FAILED = -1;


        /// <summary>
        /// Get CustomerInfo object. If object propertie IsBounce=true - highlight email address 
        /// in S2 GUI and try to update it while talking to customer over the phone.
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns>CustomerInfo object on success, NULL on fail</returns>
        [WebMethod]
        public CustomerInfo GetCustomerInfo(int customerId)
        {
            try
            {
                return TryGetCustomerInfoObject(customerId);
            }
            catch (Exception ex)
            {
                new Logger("GetCustomerInfo() failed for customerId: " + customerId.ToString(), ex.ToString());
                return null;
            }
        }


        /// <summary>
        /// Send welcome info to customer (email or regular letter). 
        /// Scenarios ----------------------------------------------
        /// 1. Email valid and has NOT changed - new welcome mail attempt to same address.
        /// 2. Email valid and HAS changed - welcome mail to new address and address updated in DI-databases.
        /// 3. Email NOT valid or does NOT pass "DI-rules" - regular letter is sent. 
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="email"></param>
        /// <returns>1 on new attempt to same email, 2 on email changed and valid, 3 on send regular letter, -1 on fail</returns>
        [WebMethod]
        public int SendWelcomeInfo(int customerId, string email)
        {
            try
            {
                email = email.Trim();
                MailSenderDbHandler db = new MailSenderDbHandler();
                DataSet dsEmailRules = db.GetEmailRules();
                ApsisSharedMethods sm = new ApsisSharedMethods();

                bool emailIsValid = MiscFunctions.IsValidEmail(email);
                bool emailPassesRules = db.EmailNotEmptyAndPassesRules(email, dsEmailRules);

                //send regular letter - email not valid / does not passe rules
                if (!emailIsValid || !emailPassesRules)
                {
                    sm.SendRegularLetter(customerId, email, true, dsEmailRules);
                    return 3;                                                       //return - regular letter
                }

                CustomerInfo ci = TryGetCustomerInfoObject(customerId);
                string emailOld = "";

                if (ci != null)
                    emailOld = ci.Email.Trim();

                if (email.ToLower() == emailOld.ToLower())
                {
                    db.SetForceRetry(customerId, true, true);
                    return 1;
                }
                else
                {
                    sm.UpdateAllEmailsAndSetForceRetry(customerId, email, true, true);
                    return 2;
                }
            }
            catch (Exception ex)
            {
                new Logger("SendWelcomeInfo() failed for customerId: " + customerId.ToString(), ex.ToString());
                return FAILED;
            }
        }


        /// <summary>
        /// Send regular letter to customer (could not find a valid email).
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns>0 on success, -1 on fail</returns>
        [WebMethod]
        public int SendRegularLetter(int customerId, string email)
        {
            try
            {
                ApsisSharedMethods sm = new ApsisSharedMethods();
                sm.SendRegularLetter(customerId, email.Trim(), true);
                return SUCCESS;
            }
            catch (Exception ex)
            {
                new Logger("SendRegularLetter() failed for customerId: " + customerId.ToString(), ex.ToString());
                return FAILED;
            }
        }


        /// <summary>
        /// If return value > 0 staff should ask customer for email address.
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns>
        /// 1 empty email, 2 not valid email, 3 email does not pass "DI-rules" (eg info@...), 
        /// 4 welcome mail could not be sent to email, 0 NO NEED TO UPDATE EMAIL, -1 method failed
        /// </returns>
        [WebMethod]
        public int AskCustomerForEmail(int customerId)
        {
            try
            {
                string emailCirix = GetCirixEmail(customerId);

                if (string.IsNullOrEmpty(emailCirix))
                    return 1;

                if (!MiscFunctions.IsValidEmail(emailCirix))
                    return 2;

                MailSenderDbHandler db = new MailSenderDbHandler();
                if (!db.EmailNotEmptyAndPassesRules(emailCirix, db.GetEmailRules()))
                    return 3;

                ApsisCustomer c = db.GetCustomer(customerId);
                if (c.CustomerId > 0)
                {
                    if (c.Email.ToLower().Trim() == emailCirix.ToLower())
                    {
                        if (!c.ForceRetry && (c.IsBounce || c.DateRegularLetter > DateTime.MinValue))
                            return 4;
                    }
                }

                return 0;
            }
            catch (Exception ex)
            {
                new Logger("AskCustomerForEmail() failed for customerId: " + customerId.ToString(), ex.ToString());
                return -1;
            }
        }

        private string GetCirixEmail(int customerId)
        {
            try
            {
                DataSet ds = SubscriptionController.GetCustomer(long.Parse(customerId.ToString()));
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                    return ds.Tables[0].Rows[0].ItemArray[10].ToString().Trim();

                return string.Empty;
            }
            catch
            {
                throw;
            }
        }


        /// <summary>
        /// Transform customers flagged as S2-bounces into DI-bounces. 
        /// After call to method customers will be displayed in DI:s bounce GUI.
        /// </summary>
        /// <param name="customerIds"></param>
        /// <returns>0 on success, -1 on fail</returns>
        [WebMethod]
        public int RejectCustomers(List<int> customerIds)
        {
            try
            {
                MailSenderDbHandler db = new MailSenderDbHandler();
                db.RejectCustsExt(customerIds);
                return SUCCESS;
            }
            catch (Exception ex)
            {
                new Logger("RejectCustomers() failed", ex.ToString());
                return FAILED;
            }
        }

        /// <summary>
        /// Call method for non paying subscribers that HAVE BOUNCED and 
        /// cannot be contacted OR does not want to buy a subscription.
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns>0 on success, -1 on fail</returns>
        [WebMethod]
        public int DeleteBounceInfo(int customerId)
        {
            try
            {
                MailSenderDbHandler db = new MailSenderDbHandler();
                //Customer c = db.GetCustomer(customerId);
                //if (c.IsBounce)

                db.DeleteCustomer(customerId, "Y");

                return SUCCESS;
            }
            catch (Exception ex)
            {
                new Logger("Deletebounceinfo() failed", ex.ToString());
                return FAILED;
            }
        }


        //todo 1 update in DiClassLib

        /// <summary>
        /// Call method for FREE SUBSCRIBERS that HAS BOUNCED and now starts a PAYMENT SUBSCRIPTION.
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns>
        /// 1 if welcome info sent by email, 2 if welcome info sent by regular letter, 
        /// 3 if customer has taken a weekend subs (no welcome info sent), 
        /// -1 if customer info could not be reterived from Cirix, -2 on exception
        /// </returns>
        [WebMethod]
        public int HandleBoncedFreeCustBuyingSubs(int customerId, int subsno)
        {
            try
            {
                //get updated cust info from cirix
                ApsisCustomer c = SubscriptionController.CirixGetCustomer(customerId, subsno, false);
                if (c.CustomerId < 1)
                {
                    //todo remove after testing
                    //new Logger("HandleBoncedFreeCustBuyingSubs() - return=-1 - cusno=" + customerId.ToString() + ", subsno=" + subsno.ToString());
                    return -1;
                }

                MailSenderDbHandler mailDb = new MailSenderDbHandler();
                mailDb.UpdateCustomer(c);   //updates all fields in customer table (isBounce=0 and forceRetry=0)

                //weekend - no mail or letter
                if (c.f_ProductNo == Settings.ProductNo_Weekend && c.f_PaperCode == Settings.PaperCode_DI)
                {
                    mailDb.DeleteCustomer(c.CustomerId, "Y");

                    //todo remove after testing
                    //new Logger("HandleBoncedFreeCustBuyingSubs() - return=3 (WE) - cusno=" + customerId.ToString() + ", subsno=" + subsno.ToString());

                    return 3;
                }

                DataSet dsEmailRules = mailDb.GetEmailRules();
                bool emailPassesRules = mailDb.EmailNotEmptyAndPassesRules(c.Email, dsEmailRules);
                bool emailIsValid = MiscFunctions.IsValidEmail(c.Email);

                //send email
                if (emailIsValid && emailPassesRules)
                {
                    mailDb.UpdateEmailInCustomer(customerId, c.Email);   //email now ok in all db:s
                    mailDb.SetForceRetry(customerId, true, true);

                    //todo remove after testing
                    //new Logger("HandleBoncedFreeCustBuyingSubs() - return=1 (email) - cusno=" + customerId.ToString() + ", subsno=" + subsno.ToString());

                    return 1;
                }
                else  //send regular letter
                {
                    SubscriptionController.UpdateLetterInCirix(c, subsno);
                    ApsisSharedMethods sm = new ApsisSharedMethods();
                    sm.SendRegularLetter(customerId, c.Email, true, dsEmailRules);

                    //todo remove after testing
                    //new Logger("HandleBoncedFreeCustBuyingSubs() - return=2 (regular letter) - cusno=" + customerId.ToString() + ", subsno=" + subsno.ToString());

                    return 2;
                }
            }
            catch (Exception ex)
            {
                new Logger("HandleBoncedFreeCustBuyingSubs() - failed", ex.ToString());
                return -2;
            }
        }


        //todo: remove after testing
        /// <summary>
        /// CAUTION! USE ONLY TEST CUSTOMERS ID:S, ELSE REAL DATA WILL BE AFFECTED!
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns>0 on success, -1 on fail</returns>
        [WebMethod]
        public int S2_ResetCustomer_FOR_TESTING_ONLY(int customerId, string email, bool isBounce)
        {
            try
            {
                email = email.Trim();

                ApsisSharedMethods sm = new ApsisSharedMethods();
                sm.UpdateAllEmailsAndSetForceRetry(customerId, email, false, false);

                MailSenderDbHandler db = new MailSenderDbHandler();
                db.S2_TEST_ResetCustomer(customerId, email, isBounce);

                ApsisCustomer c = new ApsisCustomer();
                c.CustomerId = customerId;
                List<ApsisCustomer> custs = new List<ApsisCustomer>();
                custs.Add(c);
                SubscriptionController.FlagCustsInLetter(custs, "P");

                return SUCCESS;
            }
            catch (Exception ex)
            {
                new Logger("S2_ResetCustomer_FOR_TESTING_ONLY() failed", ex.ToString());
                return FAILED;
            }
        }


        #region helpers

        private CustomerInfo TryGetCustomerInfoObject(int customerId)
        {
            try
            {
                MailSenderDbHandler db = new MailSenderDbHandler();
                ApsisCustomer c = db.GetCustomer(customerId);

                if (c.CustomerId == 0)
                    return null;

                return new CustomerInfo(c);
            }
            catch
            {
                throw;
            }
        }

        #endregion


    }

    /// <summary>
    /// Light weight customer class used in S2 webservice.
    /// </summary>
    public class CustomerInfo
    {
        public int CustomerId;
        public string Email;
        //public DateTime DateSubsStart;
        public DateTime DateSubsEnd;
        public bool HasBadEmail;
        public bool HasReceivedWelcomeInfo;


        public CustomerInfo() { }

        public CustomerInfo(ApsisCustomer c)
        {
            CustomerId = c.CustomerId;
            Email = c.Email;
            //DateSubsStart = c.SubsStartDate;
            DateSubsEnd = c.SubsEndDate;
            HasBadEmail = SetHasBadEmail(c);
            HasReceivedWelcomeInfo = SetHasReceivedWelcomeInfo(c);
        }

        private bool SetHasBadEmail(ApsisCustomer c)
        {
            bool bo = false;

            if (c.IsBounce || c.DateRegularLetter > DateTime.MinValue)
                bo = true;

            if (c.ForceRetry)
                bo = false;

            return bo;
        }

        private bool SetHasReceivedWelcomeInfo(ApsisCustomer c)
        {
            bool bo = false;

            if (c.ForceRetry || !c.IsBounce || c.DateRegularLetter > DateTime.MinValue)
                bo = true;

            return bo;
        }

    }
}
