using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;

using DIClassLib.BonnierDigital;
using DIClassLib.Misc;
using DIClassLib.DbHandlers;
using DIClassLib.ApsisWs;
using DIClassLib.ApsisWs_v3;
using DIClassLib.DbHelpers;
using System.Configuration;
using System.Data;
using DIClassLib.SingleSignOn;


namespace DIClassLib.EPiJobs.Apsis
{
    public class ApsisWsHandler
    {
        private string _apsisUser = ConfigurationManager.AppSettings["apsisWelcomeAccountUsername"];
        private string _apsisPass = ConfigurationManager.AppSettings["apsisWelcomeAccountPassword"];
        private string _giftTargetCode = ConfigurationManager.AppSettings["giftTargetCode"];
        private string _apsisSenderNameSMS = ConfigurationManager.AppSettings["apsisSenderNameSMS"];
        private string _apsisWelcomeEmailListId = ConfigurationManager.AppSettings["apsisWelcomeEmailList"];
        private ApsisNewsletterProAPISoapClient _ws = null;
        private ApsisNewsletterProv3APISoapClient _wsV3 = null;

        public ApsisNewsletterProAPISoapClient Ws
        {
            get
            {
                if (_ws == null)
                    _ws = new ApsisNewsletterProAPISoapClient();

                return _ws;
            }
        }

        /// <summary>
        /// To be used with version 3 of Apsis webservice instead of property Ws!
        /// </summary>
        public ApsisNewsletterProv3APISoapClient WsV3
        {
            get
            {
                if (_wsV3 == null)
                    _wsV3 = new ApsisNewsletterProv3APISoapClient();

                return _wsV3;
            }
        }

        #region email

        public int ApsisSendEmail(string identifier, ApsisCustomer c)
        {
            //test mode: return -10 to -1000
            if (MiscFunctions.ApsisMailerIsInTestMode)
            {
                var random = new Random();
                return random.Next(-1000, -10);
            }
             
            var apsisMailId = 0;
            long addToListResult = 0;
            try
            {
                var apsisData = ApsisSharedMethods.SetApsisFields(c);

                //if (apsisData != null)
                //{
                //    new Logger("SetApsisFields returned this: " + apsisData.ToLogString());
                //}
                //else
                //{
                //    new Logger("SetApsisFields returned null");
                //}

                //If user already exist in Apsis, set (=keep) or override values that was/was not set in SetApsisFields() above
                var apsisListSubscriber = GetSubscriberDetails(c.Email);

                //if (apsisListSubscriber != null)
                //{
                //    new Logger("GetSubscriberDetails returned this: " + apsisListSubscriber.ToLogString());
                //}
                //else
                //{
                //    new Logger("GetSubscriberDetails returned null");
                //}

                var apsisCustomerExist = (apsisListSubscriber != null && !string.IsNullOrEmpty(apsisListSubscriber.SubscriberID));
                //TODO: Merge this ApsisFields and ApsisListSubscriber into one
                if (apsisCustomerExist)
                {
                    apsisData.UserName = apsisListSubscriber.UserName;
                    apsisData.Status = apsisListSubscriber.Status;
                    apsisData.MobileNumber = apsisListSubscriber.MobileNumber;
                    apsisData.PrenDi = apsisListSubscriber.PrenDi;
                    apsisData.PrenPren = apsisListSubscriber.PrenPren;
                    apsisData.PrenInfo = apsisListSubscriber.PrenInfo;
                    apsisData.PrenGasell = apsisListSubscriber.PrenGasell;
                    apsisData.PrenConference = apsisListSubscriber.PrenConference;
                    apsisData.PrenGold = apsisListSubscriber.PrenGold;
                    apsisData.PrenAdvertisment = apsisListSubscriber.PrenAdvertisment;
                    apsisData.OutcomeS2 = apsisListSubscriber.OutcomeS2;
                    apsisData.OutcomeS2Date = apsisListSubscriber.OutcomeS2Date;
                    apsisData.ImportDate = apsisListSubscriber.ImportDate;
                }

                //new Logger("apsisCustomerExist = " + apsisCustomerExist);

                //new Logger("apsisData before ws call: " + apsisData.ToLogString());

                //sub is a gift, send the mail at 3 pm on subs start date
                var sendDate = string.Empty;
                if (!String.IsNullOrEmpty(_giftTargetCode) && !String.IsNullOrEmpty(c.TargetGroup) && c.TargetGroup.StartsWith(_giftTargetCode))
                {
                    sendDate = c.InvStartDate.Date.AddHours(15).ToString("yyyy-MM-dd HH:mm");
                }
                
                //Queue the welcome email at Apsis
                apsisMailId = WsV3.InsertTransaction(_apsisUser,
                    _apsisPass,
                    c.ApsisProjectGuid, //strId
                    identifier, //strIdentifier
                    c.Email, //strEmail
                    c.Name, //strName
                    "HTML", //strFormat
                    sendDate, //strSendTime
                    ApsisSharedMethods.GetApsisDemographicKeysValues(),
                    ApsisSharedMethods.GetApsisDemographicKeysValues(apsisData));

                //var logStringInsertTransaction = String.Format(
                //    "strUsername: '{0}', " +
                //    "strPassword: '{1}', " +
                //    "strId: '{2}'," +
                //    "strIdentifier: '{3}, " +
                //    "strEmail: '{4}, " +
                //    "strName: '{5}, " +
                //    "strFormat: '{6}, " +
                //    "strSendTime: '{7}, " +
                //    "strDataFields: '{8}, " +
                //    "strDataValues: '{9}, ",
                //    _apsisUser,
                //    "PASSWORD NOT LOGGED",
                //    c.ApsisProjectGuid,
                //    identifier,
                //    c.Email,
                //    c.Name,
                //    "HTML",
                //    sendDate,
                //    ApsisSharedMethods.GetApsisDemographicKeysValues(),
                //    ApsisSharedMethods.GetApsisDemographicKeysValues(apsisData));

                //new Logger("Apsis WS call WsV3.InsertTransaction " + logStringInsertTransaction);

                //Below is a way to UPDATE an existing Apsis-customer and make sure it gets added into list _apsisWelcomeEmailListId 
                //if not there, and to keep all current fields, otherwise they will be overwritten with 'empty' 
                //Otherwise Apsis-method like UpdateSubscriberData() would ofcourse have been suitable
                //Keep current standard datafields:
                var emailField = apsisCustomerExist ? apsisListSubscriber.Email : c.Email;
                var nameField = apsisCustomerExist ? apsisListSubscriber.Name : c.Name;
                var phoneField = apsisCustomerExist ? apsisListSubscriber.PhoneMob : string.Empty; //TODO: Get phone from Cirix on new customer
                
                addToListResult = WsV3.InsertSubscriberWithDemData(_apsisUser,
                    _apsisPass,
                    _apsisWelcomeEmailListId,
                    emailField,
                    nameField,
                    "46",
                    phoneField,
                    "HTML",
                    ApsisSharedMethods.GetApsisDemographicKeysValues(),
                    ApsisSharedMethods.GetApsisDemographicKeysValues(apsisData));
            }
            catch (Exception ex)
            {
                new Logger("ApsisSendEmail() failed: " + c.Email, ex.ToString());
            }

            if (apsisMailId == 0)
            {
                new Logger("ApsisSendEmail() failed: " + c.Email, "no ID received from Apsis");
            }

            if (apsisMailId < 0)
            {
                new Logger("ApsisSendEmail() failed: " + c.Email, "Apsis error code: " + apsisMailId);
            }

            if (addToListResult <= 0)
            {
                new Logger("ApsisSendEmail InsertSubscriberWithDemData() failed email:" + c.Email, "Result from InsertSubscriberWithDemData()" + addToListResult);
            }

            return apsisMailId;
        }

        /// <summary>
        /// For new Apsis webservice version 3 as it returns Dataset instead of TransactionBounceResult
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet ApsisGetBouncesV3()
        {
            // Test mode - return empty list
            if (MiscFunctions.ApsisMailerIsInTestMode)
                return new DataSet();

            var ms = new MailSenderDbHandler();
            DateTime dtStart = ms.GetLatestDateBounce(); //date for last saved bounce in MSSQL
            return ApsisGetBouncesV3(dtStart);
        }

        public DataSet ApsisGetBouncesV3(DateTime startFrom)
        {
            // Test mode - return empty list
            if (MiscFunctions.ApsisMailerIsInTestMode)
                return new DataSet();

            const string apsisDateTimeFormat = "yyyy-MM-ddT00:00:00";
            var dtStart = startFrom.AddSeconds(1); //add 1 sec to get only newer bounces from Apsis
            var dtEnd = DateTime.Now.AddDays(1); //add 1 day if time diff compared to apsis server

            try
            {
                DataSet we = WsV3.GetTransactionBouncesByDateInterval(_apsisUser, _apsisPass, MiscFunctions.GetAppsettingsValue("apsisProjectGuid2013_weekend"), dtStart.ToString(apsisDateTimeFormat), dtEnd.ToString(apsisDateTimeFormat));
                DataSet tr = WsV3.GetTransactionBouncesByDateInterval(_apsisUser, _apsisPass, MiscFunctions.GetAppsettingsValue("apsisProjectGuid2013_trial"), dtStart.ToString(apsisDateTimeFormat), dtEnd.ToString(apsisDateTimeFormat));
                DataSet ds = WsV3.GetTransactionBouncesByDateInterval(_apsisUser, _apsisPass, MiscFunctions.GetAppsettingsValue("apsisProjectGuid2013_digitalShort"), dtStart.ToString(apsisDateTimeFormat), dtEnd.ToString(apsisDateTimeFormat));
                DataSet dl = WsV3.GetTransactionBouncesByDateInterval(_apsisUser, _apsisPass, MiscFunctions.GetAppsettingsValue("apsisProjectGuid2013_digitalLong"), dtStart.ToString(apsisDateTimeFormat), dtEnd.ToString(apsisDateTimeFormat));
                DataSet ps = WsV3.GetTransactionBouncesByDateInterval(_apsisUser, _apsisPass, MiscFunctions.GetAppsettingsValue("apsisProjectGuid2013_paperShort"), dtStart.ToString(apsisDateTimeFormat), dtEnd.ToString(apsisDateTimeFormat));
                DataSet pl = WsV3.GetTransactionBouncesByDateInterval(_apsisUser, _apsisPass, MiscFunctions.GetAppsettingsValue("apsisProjectGuid2013_paperLong"), dtStart.ToString(apsisDateTimeFormat), dtEnd.ToString(apsisDateTimeFormat));

                DataSet ag = WsV3.GetTransactionBouncesByDateInterval(_apsisUser, _apsisPass, MiscFunctions.GetAppsettingsValue("apsisProjectGuid2014_Agenda"), dtStart.ToString(apsisDateTimeFormat), dtEnd.ToString(apsisDateTimeFormat));
                DataSet we2014 = WsV3.GetTransactionBouncesByDateInterval(_apsisUser, _apsisPass, MiscFunctions.GetAppsettingsValue("apsisProjectGuid2014_weekend"), dtStart.ToString(apsisDateTimeFormat), dtEnd.ToString(apsisDateTimeFormat));
                DataSet dig2014 = WsV3.GetTransactionBouncesByDateInterval(_apsisUser, _apsisPass, MiscFunctions.GetAppsettingsValue("apsisProjectGuid2014_digital"), dtStart.ToString(apsisDateTimeFormat), dtEnd.ToString(apsisDateTimeFormat));
                DataSet pap2014 = WsV3.GetTransactionBouncesByDateInterval(_apsisUser, _apsisPass, MiscFunctions.GetAppsettingsValue("apsisProjectGuid2014_paper"), dtStart.ToString(apsisDateTimeFormat), dtEnd.ToString(apsisDateTimeFormat));

                //Merge all datasets into dsAll
                var dsAll = new DataSet();
                dsAll.Merge(we);
                dsAll.Merge(tr);
                dsAll.Merge(ds);
                dsAll.Merge(dl);
                dsAll.Merge(ps);
                dsAll.Merge(pl);
                dsAll.Merge(ag);
                dsAll.Merge(we2014);
                dsAll.Merge(dig2014);
                dsAll.Merge(pap2014);

                return dsAll;

            }
            catch (Exception ex)
            {
                new Logger("ApsisGetBouncesV3() failed", ex.Message);
            }

            return new DataSet();
        }

        //public int ApsisSendMailTEMP()
        //{
        //    Random random = new Random();
        //    string id = random.Next(-1000, -10).ToString();


        //    return Ws.InsertTransaction(_apsisUser,
        //                                _apsisPass,
        //                                MiscFunctions.GetAppsettingsValue("apsisProjectGuid_regular"),    //strId
        //                                id,                                                 //strIdentifier
        //                                "petter.luotsinen@di.se",                           //strEmail
        //                                "Petter Luo",                                       //strName
        //                                "HTML",                                             //strFormat
        //                                "",                                                 //strSendTime
        //                                "DD1;DD2;DD3;DD4",                                  //strDataFields
        //                                "aaaaa;bbbb;123456;2011-03-28"); 
        //}

        //public DataSet GetNewsletterDetails(string id)
        //{
        //    return Ws.GetNewsletterDetails(_apsisUser, _apsisPass, id);
        //}

        #endregion


        #region Apsis lists

        public bool AddCustToApsisList(ApsisListSubscriber sub)
        {
            try
            {
                //csvData is based on order of demographic data
                //csvData: email, name, [HTML/TEXT], username, passwd, cusno, date, phoneMob
                var csv = new StringBuilder();
                csv.Append(sub.Email);
                csv.Append(",");
                csv.Append(sub.Name);
                csv.Append(",");
                csv.Append("HTML");
                csv.Append(",");
                csv.Append(""); //username
                csv.Append(",");
                csv.Append(""); //passwd
                csv.Append(",");
                csv.Append(sub.SubscriberID);
                csv.Append(",");
                csv.Append(DateTime.Now.ToString());
                csv.Append(",");
                csv.Append(sub.PhoneMob);

                WsV3.InsertCsvSubscribersWithDemData(_apsisUser, _apsisPass, sub.ApsisListId, csv.ToString());
                string[] phone = MiscFunctions.GetSeparatedCountryCodePhoneNumber(sub.PhoneMob);
                WsV3.UpdateSubscriber(_apsisUser, _apsisPass, string.Empty, sub.Email, sub.Name, string.Empty, phone[0], phone[1]);
                return true;
            }
            catch (Exception ex)
            {
                new Logger("AddCustToApsisList failed - " + sub.ToString(), ex.ToString());
                return false;
            }
        }

        public bool DeleteCustFromApsisList(ApsisListSubscriber sub)
        {
            try
            {
                WsV3.DeleteSubscription(_apsisUser, _apsisPass, sub.SubscriberID, sub.Email, sub.ApsisListId);
                return true;
            }
            catch (Exception ex)
            {
                new Logger("DeleteCustFromApsisList failed for: " + sub.ToString(), ex.ToString());
                return false;
            }
        }

        public ApsisListSubscriber TryGetApsisListSubscriber(ApsisListSubscriber sub)
        {
            if (sub == null || string.IsNullOrEmpty(sub.ApsisListId))
                return null;

            try
            {
                DataSet ds = GetSubscribersDataSet(_apsisUser, _apsisPass, sub.ApsisListId);

                if (DbHelpMethods.DataSetHasRows(ds))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        string mail = dr["Email"].ToString();
                        string cusno = dr["dd3"].ToString(); //Kundnummer
                        string mob = dr["dd5"].ToString(); //Mobilnummer
                        string subId = dr["SubscriberID"].ToString();
                        string name = dr["Name"].ToString();

                        if (mail == sub.Email || cusno == sub.SubscriberID || mob == sub.PhoneMob)
                            return new ApsisListSubscriber(sub.ApsisListId, subId, name, mail, mob);
                    }
                }
            }
            catch (Exception ex)
            {
                new Logger("TryGetApsisListSubscriber failed for: " + sub.ToString(), ex.ToString());
            }

            return null;
        }

        public ApsisListSubscriber TryGetApsisListSubscriberByCusno(String apsisListId, String cusno)
        {
            try
            {
                DataSet ds = GetSubscribersDataSet(_apsisUser, _apsisPass, apsisListId);

                if (DbHelpMethods.DataSetHasRows(ds))
                {
                    DataRow[] rows = ds.Tables[0].Select("dd3 = '" + cusno + "'"); //Kundnummer
                    if (rows != null && rows.Length > 0)
                    {
                        DataRow dr = rows[0];
                        string mail = dr["Email"].ToString();
                        //string cn = dr["dd3"].ToString();   //Kunddnummer
                        string mob = dr["dd5"].ToString(); //Mobilnummer
                        string subId = dr["SubscriberID"].ToString();
                        string name = dr["Name"].ToString();

                        mob = MiscFunctions.FormatPhoneNumber(mob, Settings.PhoneMaxNoOfDigits, true);
                        return new ApsisListSubscriber(apsisListId, subId, name, mail, mob);
                    }
                }
            }
            catch (Exception ex)
            {
                new Logger("TryGetApsisListSubscriberByCusno failed", ex.ToString());
            }

            return null;
        }
        
        public ApsisListSubscriber TryGetApsisListSubscriberByPhonenumbers(String apsisListId, String[] phoneNumbers)
        {
            try
            {
                DataSet ds = GetSubscribersDataSet(_apsisUser, _apsisPass, apsisListId);

                if (DbHelpMethods.DataSetHasRows(ds))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        string mail = dr["Email"].ToString();
                        //string cusno = dr["dd3"].ToString();    //Kundnummer
                        string mob = dr["dd5"].ToString(); //Mobilnummer
                        string subId = dr["SubscriberID"].ToString();
                        string name = dr["Name"].ToString();

                        mob = MiscFunctions.FormatPhoneNumber(mob, Settings.PhoneMaxNoOfDigits, true);

                        if (phoneNumbers.Contains(mob))
                            return new ApsisListSubscriber(apsisListId, subId, name, mail, mob);
                    }
                }
            }
            catch (Exception ex)
            {
                new Logger("TryGetApsisListSubscriberByPhonenumbers failed", ex.ToString());
            }

            return null;
        }

        private DataSet GetSubscribersDataSet(string apsisUser, string apsisPass, string apsisListId)
        {
            var ds = WsV3.GetSubscribers(apsisUser, apsisPass, apsisListId);
            return ds;
        }

        public string ApsisSendSms(string mess, string countryCode, string phoneMob)
        {
            try
            {
                var ssmr = WsV3.SendSmsWithSenderName(_apsisUser, _apsisPass, _apsisSenderNameSMS, countryCode, phoneMob, mess);
                return (ssmr != null) ? ssmr.ToString() : string.Empty;
            }
            catch (Exception ex)
            {
                new Logger("ApsisSendSms() - failed", ex.ToString());
            }
            return string.Empty;
        }

        public int UpdateSubscriberData(string subscriberId, string userEmail, string demographicDataKey, string demographicDataValue)
        {
            var result = WsV3.UpdateSubscriberData(_apsisUser, _apsisPass, subscriberId, userEmail, demographicDataKey, demographicDataValue);

            //new Logger(string.Format(
            //        "Apsis WsV3.UpdateSubscriberData called with: strUsername: '{0}', strPassword: '{1}', strSubscriberId: '{2}', 'strEmail: '{3}', strDataField: '{4}', straDataValue: '{5}'",
            //        _apsisUser, "PASSWORD NOT LOGGED", subscriberId, userEmail, demographicDataKey, demographicDataValue));

            if (result != 1)
            {
                new Logger(string.Format("UpdateSubscriberData() failed for {0}, DD-key {1}, DD-value {2}", userEmail, demographicDataKey, demographicDataValue), "Apsis errorcode: " + result);
            }
            return result;
        }

        public int MoveSubscriberToOptOutAll(string email)
        {
            var result = WsV3.MoveSubscriberToOptOutAll(_apsisUser, _apsisPass, email, string.Empty, string.Empty, string.Empty);
            if (result != 1)
            {
                new Logger(string.Format("MoveSubscriberToOptOutAll() failed for {0}", email), "Apsis errorcode: " + result);
            }
            return result;
        }

        /// <summary>
        /// Get the current optout list from Apsis. List is cached after fetched.
        /// </summary>
        /// <returns></returns>
        public List<string> GetCurrentOptOutAllList()
        {
            var cacheKey = "ApsisWsHandler_GetCurrentOptOutAllList";
            // If list is cached, return it
            var data = HttpRuntime.Cache.Get(cacheKey);
            if (data != null)
            {
                return (List<string>)data;
            }
            var ds = WsV3.GetOptOutAll(_apsisUser, _apsisPass);
            var list = ds.Tables[0].AsEnumerable().Select(dataRow => dataRow.Field<string>("Email")).ToList();
            // Cache list if it contains any data
            if (list.Any())
            {
                HttpRuntime.Cache.Insert(
                    cacheKey,
                    list,
                    null,
                    DateTime.Now.AddSeconds(Settings.CacheTimeSecondsLong),
                    Cache.NoSlidingExpiration);
            }
            return list;
        }

        public ApsisListSubscriber GetSubscriberDetails(string email)
        {
            try
            {
                var apsisUserData = WsV3.GetSubscriberDetails(_apsisUser, _apsisPass, string.Empty, email);
                if (DbHelpMethods.DataSetHasRows(apsisUserData))
                {
                    //try
                    //{
                    //    var logString = new StringBuilder();
                    //    foreach (DataColumn column in apsisUserData.Tables[0].Columns)
                    //    {
                    //        logString.Append(column.ColumnName + ", ");
                    //    }

                    //    new Logger("WsV3.GetSubscriberDetails Column names: " + logString.ToString().Trim().TrimEnd(','));
                    //}
                    //catch (Exception)
                    //{
                        
                    //}

                    var als = new ApsisListSubscriber(string.Empty, apsisUserData.Tables[0].Rows[0]["Id"].ToString(),
                        apsisUserData.Tables[0].Rows[0]["Name"].ToString(),
                        email,
                        apsisUserData.Tables[0].Rows[0]["PhoneNumber"].ToString()
                        )
                    {
                        UserName = apsisUserData.Tables[0].Rows[0]["Anvandarnamn"].ToString(),
                        Status = apsisUserData.Tables[0].Rows[0]["Statusx"].ToString(),
                        CustomerNumber = apsisUserData.Tables[0].Rows[0]["Kundnummer"].ToString(),
                        StartDate = apsisUserData.Tables[0].Rows[0]["Startdatum"].ToString(),
                        MobileNumber = apsisUserData.Tables[0].Rows[0]["Mobilnummer"].ToString(),
                        PersonalCode = apsisUserData.Tables[0].Rows[0]["Personligkod"].ToString(),
                        PrenDi = apsisUserData.Tables[0].Rows[0]["Pren_DI"].ToString(),
                        PrenPren = apsisUserData.Tables[0].Rows[0]["Pren_Pren"].ToString(),
                        PrenInfo = apsisUserData.Tables[0].Rows[0]["Pren_Info"].ToString(),
                        PrenGasell = apsisUserData.Tables[0].Rows[0]["Pren_Gasell"].ToString(),
                        PrenConference = apsisUserData.Tables[0].Rows[0]["Pren_Konferens"].ToString(),
                        PrenGold = apsisUserData.Tables[0].Rows[0]["Pren_Guld"].ToString(),
                        PrenAdvertisment = apsisUserData.Tables[0].Rows[0]["Pren_Annons"].ToString(),
                        WelcomeEmailSentDate = apsisUserData.Tables[0].Rows[0]["ValkomstmailSkickatDatum"].ToString(),
                        DiAccount = apsisUserData.Tables[0].Rows[0]["Di_Konto"].ToString(),
                        PaperCode = apsisUserData.Tables[0].Rows[0]["Papercode"].ToString(),
                        ProductNo = apsisUserData.Tables[0].Rows[0]["Productno"].ToString(),
                        SubsLength = apsisUserData.Tables[0].Rows[0]["Subslength"].ToString(),
                        ProductDescription = apsisUserData.Tables[0].Rows[0]["Product_Description"].ToString(),
                        OutcomeS2 = apsisUserData.Tables[0].Rows[0]["Utfall_S2"].ToString(),
                        OutcomeS2Date = apsisUserData.Tables[0].Rows[0]["Datum_Utfall_S2"].ToString(),
                        ImportDate = apsisUserData.Tables[0].Rows[0]["ImportDatum"].ToString()
                    };
                    return als;
                }
            }
            catch (Exception ex)
            {
                new Logger("ApsisWsHandler.GetSubscriberDetails() failed", ex.ToString());
            }
            return null;
        }

        #endregion
    }


    [Serializable]
    //TODO: Merge this class and ApsisFields into one
    public class ApsisListSubscriber
    {
        public string ApsisListId { get; set; }
        public string SubscriberID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public string PhoneMob
        {
            get
            {
                if (!string.IsNullOrEmpty(_ph) && _ph.StartsWith("+46"))
                    _ph = _ph.Replace("+46", "0");

                return _ph;
            }
            set { _ph = value; }
        }

        //Apsis demographic datafields
        public string UserName { get; set; }
        public string Status { get; set; }
        public string CustomerNumber { get; set; }
        public string StartDate { get; set; }
        public string MobileNumber { get; set; }
        public string PersonalCode { get; set; }
        public string PrenDi { get; set; }
        public string PrenPren { get; set; }
        public string PrenInfo { get; set; }
        public string PrenGasell { get; set; }
        public string PrenConference { get; set; }
        public string PrenGold { get; set; }
        public string PrenAdvertisment { get; set; }
        public string WelcomeEmailSentDate { get; set; }
        public string DiAccount { get; set; }
        public string PaperCode { get; set; }
        public string ProductNo { get; set; }
        public string SubsLength { get; set; }
        public string ProductDescription { get; set; }
        public string OutcomeS2 { get; set; }
        public string OutcomeS2Date { get; set; }
        public string ImportDate { get; set; }

        string _ph = string.Empty;
        
        public ApsisListSubscriber(string apsisListId, string subscriberId, string name, string email, string phoneMob)
        {
            ApsisListId = apsisListId;
            SubscriberID = subscriberId;
            Name = name;
            Email = email;
            PhoneMob = phoneMob;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("ApsisListId:" + ApsisListId + ", ");
            sb.Append("SubscriberID:" + SubscriberID + ", ");
            sb.Append("Name:" + Name + ", ");
            sb.Append("Email:" + Email + ", ");
            sb.Append("PhoneMob:" + PhoneMob);
            return sb.ToString();
        }

        public string ToLogString()
        {

            try
            {
                var sb = new StringBuilder();

                foreach (var propertyInfo in typeof (ApsisListSubscriber).GetProperties())
                {
                    sb.AppendLine(String.Format("{0}: '{1}'", propertyInfo.Name,
                        propertyInfo.GetValue(this, null) ?? "[NULL]"));
                }

                return sb.ToString();
            }
            catch (Exception)
            {
                return "Could not build logstring for ApsisListSubscriber";
            }

        }

    }

}
