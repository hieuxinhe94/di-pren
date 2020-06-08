using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Caching;
using System.Web.Script.Serialization;
using System.Web.UI;
using DIClassLib.BonnierDigital;
using DIClassLib.Campaign;
using DIClassLib.DbHandlers.KayakWrappers;
using DIClassLib.DbHelpers;
using DIClassLib.EPiJobs.Apsis;
using DIClassLib.Kayak;
using DIClassLib.Misc;
using DIClassLib.Subscriptions;
using DIClassLib.Subscriptions.CirixMappers;
using DIClassLib.Subscriptions.DiPlus;

using Microsoft.VisualBasic;

using Subscription = DIClassLib.Subscriptions.Subscription;
using System.Text.RegularExpressions;

namespace DIClassLib.DbHandlers
{
    public class KayakHandler : ISubscriptionHandler
    {
        private static IKayak Ws
        {
            get
            {
                bool useTestWs;
                if (!bool.TryParse(MiscFunctions.GetAppsettingsValue("UseKayakTestWS"), out useTestWs))
                    useTestWs = false;

                if (useTestWs)
                {
                    return new KayakServiceTestWrapper();    
                }
                return new KayakServiceProdWrapper();
            }
        }

        private const string ResponseOk = "OK";

        private const string KayakUserId = "DIWEB";
        
        private const int MaxGetCustomerResult = 99999999;

        public HashSet<string> GetTargetGroups(string paperCode)
        {
            var cacheKey = "KayakHandler_GetTargetGroups_" + paperCode;

            // If list is cached, return it
            var data = HttpRuntime.Cache.Get(cacheKey);
            if (data != null)
            {
                return (HashSet<string>)data;
            }

            var hs = new HashSet<string>();

            var ds = Ws.GetParameterValuesByGroup_(paperCode, "TARGETGRPS");
            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (DataTable dt in ds.Tables)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        var s = dr["CODEVALUE"].ToString();
                        if (!string.IsNullOrEmpty(s))
                            hs.Add(s);
                    }
                }
            }
            // Cache this
            HttpRuntime.Cache.Insert(
                cacheKey,
                hs,
                null,
                DateTime.Now.AddSeconds(Settings.CacheTimeSecondsMedium),
                Cache.NoSlidingExpiration);
            return hs;
        }

        public List<StringPair> GetAllTargetGroups()
        {
            var ret = new List<StringPair>();
            var di = GetSortedTargetGroups(Settings.PaperCode_DI);
            var dise = GetSortedTargetGroups(Settings.PaperCode_DISE);
            var ipad = GetSortedTargetGroups(Settings.PaperCode_IPAD);

            ret.Add(new StringPair(Settings.PaperCode_DI, ""));
            ret.Add(new StringPair("-----------------------", ""));
            foreach (string s in di)
                ret.Add(new StringPair(s, s));

            ret.Add(new StringPair("", ""));

            ret.Add(new StringPair(Settings.PaperCode_DISE, ""));
            ret.Add(new StringPair("-----------------------", ""));
            foreach (string s in dise)
                ret.Add(new StringPair(s, s));

            ret.Add(new StringPair("", ""));

            ret.Add(new StringPair(Settings.PaperCode_IPAD, ""));
            ret.Add(new StringPair("-----------------------", ""));
            foreach (string s in ipad)
                ret.Add(new StringPair(s, s));

            return ret;
        }

        public DataSet GetCampaign(long campNo)
        {
            return GetOneCampaign(campNo);
        }

        public DataSet GetCampaign(string campId)
        {
            return GetOneCampaign(GetCampno(campId));
        }

        public long GetCampno(string campId)
        {
            var ds = Ws.GetCampaignByCampOrGroupId_(campId, string.Empty);
            return !DbHelpMethods.DataSetHasRows(ds) ? 0 : DbHelpMethods.ValueIfColumnExist(ds.Tables[0], ds.Tables[0].Rows[0], "CAMPNO", 0);
        }

        public string GetProductName(string paperCode, string productNo)
        {
            var allPapersAndProducts = GetPapersAndProducts(string.Empty);
            var prod = allPapersAndProducts.FirstOrDefault(c => c.PaperCode == paperCode && c.ProductNo == productNo);
            return (prod != null) ? prod.ProductName : string.Empty;
        }

        //TODO: Rebuild this to take packageId as parameter instead or refactor in future to have GetActiveFreeCampaigns() as a Kayak Webservice instead
        public List<CampaignInfo> GetActiveFreeCampaigns(string paperCode, string productNo)
        {
            var cacheKey = string.Format("KayakHandler_GetActiveFreeCampaigns_{0}{1}", paperCode, productNo);
            // If list is cached, return it
            var data = HttpRuntime.Cache.Get(cacheKey);
            if (data != null)
            {
                return (List<CampaignInfo>)data;
            }
            var packageId = GetPackageId(paperCode, productNo);
            var activeCampNos = GetActiveCampNos(packageId);
            var allFree = new List<CampaignInfo>();
            var eventsToWaitFor = new List<ManualResetEvent>();
            
            //Now GetOneCampaignParsed is called in separate threads and then waits for them all, so they executes async to save time.
            foreach (var campNo in activeCampNos)
            {
                var resetEvent = new ManualResetEvent(false);
                ThreadPool.QueueUserWorkItem(arg =>
                {
                    allFree.AddRange(GetOneCampaignParsed(campNo));
                    resetEvent.Set();
                });
                eventsToWaitFor.Add(resetEvent);
            }
            while (eventsToWaitFor.Any())
            {
                var range = eventsToWaitFor.Count >= 64 ? 64 : eventsToWaitFor.Count; //Max 64 is allowed to wait for at a time
                var part = eventsToWaitFor.GetRange(0, range);
                eventsToWaitFor.RemoveRange(0, range);
                WaitHandle.WaitAll(part.ToArray());
            }

            var result = allFree.Where(l => l != null && l.TotalPrice == 0).ToList();
            // Cache this
            HttpRuntime.Cache.Insert(
                cacheKey,
                result,
                null,
                DateTime.Now.AddSeconds(Settings.CacheTimeSecondsMedium),
                Cache.NoSlidingExpiration);
            return result;
        }
        
        public string GetCommuneCode(string zipCode)
        {
            try
            {
                return Ws.GetCommuneCode_(zipCode, Settings.Country);
            }
            catch (Exception ex)
            {
                new Logger("KayakHandler.GetCommuneCode(string zipCode) failed, zipCode:" + zipCode, ex.ToString());
                return null;
            }
        }

        //TODO: No more needed?
        public long GetPriceListNo(string paperCode, string productNo, DateTime invStartDate, string communeCode, string priceGr, string campId)
        {
            return 0;
        }

        public string UpdateCustomerInformation(long lCusNo, string sEmailAddress, string sHPhone, string sWPhone, string sOPhone, string sSalesDen, string sOfferdenDir, string sOfferdenSal, string sOfferdenEmail, string sDenySmsMark, string sAccnoBank, string sAccnoAcc, string sNotes, long lEcusno, string sOtherCusno, string sWwwUserId, string sExpday, double dDiscPercent, string sTerms, string sSocialSecNo, string sCategory, long lMasterCusno, string companyId)
        {
            /* Source:
            01 Kund
            02 ASO, alltså kunden har gjort via webben
            03 Nix */
            const string source = "02";
            var cusType = string.IsNullOrEmpty(companyId) ? Settings.SubscriberPrivate : Settings.SubscriberCompany; // 01=private 02=company
            /*
            sSalesDen -> sCusMarkAllowed
            sOfferdenDir->sDirectMarkAllowed
            sOfferdenSal-> sSalesManMarkAllowed
            sOfferdenEmail->sEmailMarkAllowed
            oDenySmsMark->sSmsMarkAllowed

            Above properties have the opposite(!) meaning in Kayak.StoreCustomerPaperInformation() where it is ALLOWED instead of DENY!!!
            */
            var returnValue = Ws.StoreCustomerPaperInformation_(lCusNo, "ALL",
                OfferOpposite(sOfferdenDir),
                OfferOpposite(sOfferdenSal),
                OfferOpposite(sOfferdenEmail),
                OfferOpposite(sDenySmsMark),
                OfferOpposite(sSalesDen),
                DateTime.Now,
                source,
                DateTime.Now,
                source,
                DateTime.Now,
                source,
                DateTime.Now,
                source,
                DateTime.Now,
                source);
            return returnValue.ToUpper() != ResponseOk ?
                returnValue.ToUpper() :
                Ws.UpdateCustomerInformation_CII_(lCusNo, sEmailAddress, sHPhone, sWPhone, sOPhone, sAccnoBank, sAccnoAcc, false,
                    sNotes, lEcusno, sOtherCusno, sWwwUserId, sExpday, sTerms, sSocialSecNo, sCategory, lMasterCusno, string.Empty,
                    string.Empty, DateTime.MinValue, cusType, string.Empty, false, companyId, string.Empty, string.Empty, string.Empty);
        }
        
        public string GetWwwPassword(long cusNo)
        {
            try
            {
                return Ws.GetWwwPassword_CII_(cusNo);
            }
            catch (Exception ex)
            {
                new Logger("KayakHandler.GetWwwPassword() - failed for cusNo:" + cusNo, ex.ToString());
            }
            return string.Empty;
        }

        public string GetWwwUserId(long cusno)
        {
            var ds = GetCustomer(cusno);
            var ac = ParseCustomerDataSetToApsisCustomer(ds);
            return ac.UserName;
        }

        public DataSet GetCustomer(long cusNo)
        {
            try
            {
                return Ws.GetCustomer_CII_(cusNo);
            }
            catch (Exception ex)
            {
                new Logger("KayakHandler.GetGetCustomer() - failed for cusNo:" + cusNo.ToString(), ex.ToString());
                return null;
            }
        }

        public Person GetCustomerInfo(long cusNo)
        {
            var ds = GetCustomer(cusNo);
            return ParseCustomerDataSetToPerson(ds);
        }

        public string GetEmailAddress(long cusno)
        {
            var ds = GetCustomer(cusno);
            var p = ParseCustomerDataSetToPerson(ds);
            return p.Email;
        }

        public int InsertCustomerProperty(long cusno, string propCode, string propValue)
        {
            try
            {
                var s = Ws.InsertCustomerProperty_CII_(cusno, propCode, propValue, string.Empty, string.Empty);
                return (s.ToUpper() == ResponseOk) ? 0 : -1;
            }
            catch (Exception ex)
            {
                new Logger("KayakHandler.InsertCustomerProperty() failed. " + Environment.NewLine +
                           "cusno: " + cusno + Environment.NewLine +
                           "propCode: " + propCode + Environment.NewLine +
                           "propValue: " + propValue,
                    ex.ToString());
                return -1;
            }
        }

        public DataSet GetSubscriptions(long cusNo, bool showPassiveIfNoActive)
        {
            try
            {
                return Ws.GetSubscriptions_(cusNo, showPassiveIfNoActive, KayakUserId, string.Empty);
            }
            catch (Exception ex)
            {
                new Logger("KayakHandler.GetSubscriptions() - failed for cusNo:" + cusNo, ex.ToString());
                return null;
            }
        }

        public List<Subscription> GetSubscriptions2(long cusno)
        {
            var subs = new List<Subscription>();
            try
            {
                var ds = GetSubscriptions(cusno, true);
                if (ds != null)
                {
                    foreach (DataTable dt in ds.Tables)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            var sub = new Subscription();
                            sub.SubsNo = long.Parse(dr["SUBSNO"].ToString());
                            sub.SubsKind = dr["SUBSKIND"] as string;
                            var invStartDate = DbHelpMethods.SetDateFromDbFieldName(dr, "INVSTARTDATE");
                            if (invStartDate > DateTime.MinValue)
                            {
                                sub.InvStartDate = invStartDate;
                            }
                            sub.SubsStartDate = DbHelpMethods.SetDateFromDbFieldName(dr, "SUBSSTARTDATE");
                            sub.SubsEndDate = DbHelpMethods.SetDateFromDbFieldName(dr, "SUBSENDDATE");
                            sub.CampId = dr["CAMPID"] as string;
                            sub.SubsState = dr["SUBSSTATE"] as string;
                            sub.PackageId = dr["PACKAGEID"] as string;
                            sub.PaperCode = dr["PAPERCODE"] as string;
                            sub.ProductNo = dr["PRODUCTNO"] as string;
                            sub.Pricegroup = dr["PRICEGROUP"] as string;
                            sub.Substype = dr["SUBSTYPE"] as string;

                            long cno;
                            long.TryParse(dr["CAMPNO"].ToString(), out cno);
                            if (cno > 0)
                                sub.CampNo = cno;

                            sub.SuspendDate = DbHelpMethods.SetDateFromDbFieldName(dr, "SUSPENDDATE");
                            sub.Subscriber.Cusno = long.Parse(dr["SUBSCUSNO"].ToString());

                            int mons;
                            int.TryParse(dr["SUBSLEN_MONS"].ToString(), out mons);
                            if (mons > 0)
                                sub.SubsLenMons = mons;

                            int days;
                            int.TryParse(dr["SUBSLEN_DAYS"].ToString(), out days);
                            if (days > 0)
                                sub.SubsLenDays = days;

                            var payCusno = long.Parse(dr["PAYCUSNO"].ToString());
                            if (payCusno > 0)
                                sub.SubscriptionPayer.Cusno = payCusno;
                            else
                                sub.SubscriptionPayer = null;

                            int extno;
                            int.TryParse(dr["EXTNO"].ToString(), out extno);
                            if (extno > 0)
                                sub.ExtNo = extno;

                            sub.CancelReason = dr["CANCELREASON"].ToString();

                            subs.Add(sub);
                        }
                    }
                }
                subs.Sort(); //subs[0] - latest SubsRealEndDate

            }
            catch (Exception ex)
            {
                new Logger("KayakHandler.GetSubscriptions2() failed for cusno:" + cusno, ex.ToString());
            }

            return subs;
        }

        public List<ApsisCustomer> CirixGetNewCusts(DateTime? takeFrom = null)
        {
            var newCustomers = new List<ApsisCustomer>();
            var dsNewCustomers = Ws.GetSubsForConfirm_CII_(string.Empty, string.Empty, takeFrom ?? DateTime.Today, true);
            if (!DbHelpMethods.DataSetHasRows(dsNewCustomers))
            {
                return newCustomers;
            }
            var apsisCustomers = ParseSubsForConfirmDataSetToApsisCustomer(dsNewCustomers);
            var mailSenderDbHandler = new MailSenderDbHandler();
            var emailRules = mailSenderDbHandler.GetEmailRules();
			apsisCustomers.Add(new ApsisCustomer() { Email = "", f_PaperCode = "DI", f_ProductNo = "01" });
            foreach (var cust in apsisCustomers)
            {
                if (!mailSenderDbHandler.EmailNotEmptyAndPassesRules(cust.Email, emailRules))
                {
                    //return newCustomers;
                    continue;
                }
                if (CustHadDigSubThenGotPaperSub(cust.CustomerId))
                {
                    //return newCustomers;
                    continue;
                }
                var definitionFile = new DefinitionFile(); //to decide value of IsExtCustomer
                cust.SetIsExtCustomer(definitionFile);
                var freeSubsCampIds = GetFreeSubsCampIds();
                cust.SetApsisProjectGuid(freeSubsCampIds);
                newCustomers.Add(cust);
            }
            return newCustomers;
        }

        public List<ApsisCustomer> CirixGetCustsManuallyFromList(IEnumerable<string> customerIds)
        {
            var returnList = new List<ApsisCustomer>();
            foreach (var cusnoString in customerIds)
            {
                long cusno;
                if (!long.TryParse(cusnoString, out cusno))
                {
                    new Logger(string.Format("KayakHandler.CirixGetCustsManuallyFromList() failed parsing cusno {0}", cusnoString));
                    continue;
                }
                var subscriptionList = GetSubscriptions2(cusno);
                var customerLatestSub = subscriptionList
                    .Where(sub => sub.SubsState == "00" && sub.SubsState == "01" && sub.SubsState == "02" && string.IsNullOrEmpty(sub.CancelReason))
                    .OrderByDescending(s => s.SubsStartDate).FirstOrDefault();
                if (customerLatestSub == null)
                {
                    new Logger(string.Format("KayakHandler.CirixGetCustsManuallyFromList() failed fetching any subscription for cusno {0}", cusno));
                    continue;
                }
                var ds = GetCustomer(cusno);
                var apsisCustomer = ParseCustomerDataSetToApsisCustomer(ds);
                apsisCustomer.CustomerId = (int)cusno;
                apsisCustomer.PassWord = string.Empty;
                apsisCustomer.f_PaperCode = customerLatestSub.PaperCode;
                apsisCustomer.f_ProductNo = customerLatestSub.ProductNo;
                apsisCustomer.TargetGroup = customerLatestSub.TargetGroup;
                apsisCustomer.InvStartDate = customerLatestSub.InvStartDate;
                apsisCustomer.SubsEndDate = customerLatestSub.SubsEndDate;
                apsisCustomer.CampNo = (int)customerLatestSub.CampNo;
                //apsisCustomer.ReceiveType = string.Empty;
                apsisCustomer.SubsLenMonsFromCirix = customerLatestSub.SubsLenMons;

                returnList.Add(apsisCustomer);
            }
            return returnList;
        }

        //TODO: Here only for backward compatibility for S+, remove when Kayak is fully released.
        public List<long> GetUpdatedCusnosInDateInterval(DateTime dateMin, DateTime dateMax)
        {
            var changedCusnos = new List<long>();
            var dsChanged = GetUpdatedCustomersAsDataSet(dateMin, dateMax);
            foreach (var dr in from DataTable dt in dsChanged.Tables from DataRow dr in dt.Rows select dr)
            {
                long cusno;
                if (long.TryParse(dr["CUSNO"].ToString(), out cusno))
                {
                    changedCusnos.Add(cusno);
                }
                /*
                <CUSNO>3730529</CUSNO>
                <STAMP_DATE>2014-10-14T00:07:11+02:00</STAMP_DATE>
                <CUSTOMERMODIFIED>Y</CUSTOMERMODIFIED>
                <SUBSMODIFIED>N</SUBSMODIFIED>
                */
            }
            return changedCusnos;
        }

        public List<ApsisCustomer> GetUpdatedCustomers(DateTime startDate, DateTime endDate)
        {
            var changedCustomers = GetUpdatedCusnosInDateInterval(startDate, endDate);
            return changedCustomers.Select(GetCustomer).Select(ParseCustomerDataSetToApsisCustomer).ToList();
        }

        public void FlagCustsInLetter(List<ApsisCustomer> custs, string flag)
        {
            if (flag.ToUpper() != "Y") //TODO: No logic present yet to handle "send regular letter"-flag
            {
                return;
            }
            try
            {
                var customerString = new StringBuilder();
                foreach (var cust in custs)
                {
                    customerString.Append(string.Format("{0},{1};", cust.SubsNoForConfirm, cust.ExtNo));
                }
                if (!string.IsNullOrEmpty(customerString.ToString()))
                {
                    Ws.SubsIsConfirmed_CII_(KayakUserId, customerString.ToString());
                    new Logger(string.Format("KayakHandler SubsIsConfirmed_CII() called with {0}", customerString));
                }
            }
            catch (Exception ex)
            {
                new Logger("KayakHandler.FlagCustsInLetter(Y) failed", ex.ToString());
                throw; // Throw exception up so job will not send email
            }
        }

        //TODO: Here only for backward compatibility for S+, remove when Kayak is fully released.
        public void FlagCustsInExpCustomer(HashSet<int> cusnos, string flag)
        {
            return;
        }

        //TODO: Welcome email functionality!
        public void UpdateLetterInCirix(ApsisCustomer c, int subsno)
        {
            // TODO: NEED NEW WS
            throw new NotImplementedException();
        }

        public void UpdateCustomerEmail(int cusno, string email)
        {
            Ws.UpdateCustomerEmail_CII_(cusno, email);
        }

        public ApsisCustomer CirixGetCustomer(int customerId, int subsNo, bool isExtCustomer)
        {
            var apsisCustomer = new ApsisCustomer();
            try
            {
                var ds = GetCustomer(customerId);
                var ac = ParseCustomerDataSetToApsisCustomer(ds);
                if (ac == null)
                {
                    return apsisCustomer;
                }
                apsisCustomer.CustomerId = customerId;
                apsisCustomer.IsExtCustomer = isExtCustomer;

                var subs = GetSubscriptions(customerId, false);
                if (!DbHelpMethods.DataSetHasRows(subs))
                {
                    return null;
                }

                var dataRows = from DataTable dt in subs.Tables
                    from DataRow drow in dt.Rows
                    where drow["SUBSNO"].ToString() == subsNo.ToString()
                    select drow;
                var dr = dataRows.First();
                if (dr == null)
                {
                    return null;
                }

                apsisCustomer.f_PaperCode = dr["PAPERCODE"].ToString();
                apsisCustomer.f_ProductNo = dr["PRODUCTNO"].ToString();
                var invStartDate = SetDateFromString(dr["INVSTARTDATE"].ToString());
                if (invStartDate > DateTime.MinValue)
                {
                    apsisCustomer.InvStartDate = invStartDate;
                }
                
                var endDate = SetDateFromString(dr["SUBSENDDATE"].ToString());
                if (endDate > DateTime.MinValue)
                {
                    apsisCustomer.SubsEndDate = endDate;
                }

                int campNo;
                if (int.TryParse(dr["CAMPNO"].ToString(), out campNo))
                {
                    apsisCustomer.CampNo = campNo;
                }

                var freeSubsCampIds = GetFreeSubsCampIds();
                apsisCustomer.SetApsisProjectGuid(freeSubsCampIds);

                if (apsisCustomer.CampNo == null || !(apsisCustomer.CampNo > 0))
                {
                    return apsisCustomer;
                }

                var campaign = Ws.GetCampaignSubs_((long)apsisCustomer.CampNo);
                if (!DbHelpMethods.DataSetHasRows(campaign))
                {
                    return apsisCustomer;
                }

                var campaignRows = from DataTable dtCampaign in campaign.Tables
                    from DataRow campaignRow in dtCampaign.Rows
                    select campaignRow;
                var cr = campaignRows.First();
                apsisCustomer.CampId = cr["CAMPID"].ToString();
                apsisCustomer.ReceiveType = cr["RECEIVETYPE"].ToString();
                apsisCustomer.TargetGroup = cr["TARGETGROUP"].ToString();
                return apsisCustomer;
            }
            catch (Exception ex)
            {
                new Logger(string.Format("KayakHandler.CirixGetCustomer() failed for cusno: {0} and subsno: {1}", customerId, subsNo), ex.ToString());
            }
            return null;
        }

        public bool CustomerIsActive(int cusno, string paperCode, string productNo)
        {
            try
            {
                var ds = Ws.GetSubscriptions_(cusno, false, KayakUserId, paperCode.ToUpper());
                return DbHelpMethods.DataSetHasRows(ds);
            }
            catch (Exception ex)
            {
                new Logger("KayakHandler.CustomerIsActive() - failed for cusNo: " + cusno + " paperCode: " + paperCode + " productNo: " + productNo, ex.ToString());
            }
            return false;
        }

        public List<long> GetCusnosByEmail(string email)
        {
            var returnList = new List<long>();
            var ds = Ws.GetCusByEmail_(email);
            if (!DbHelpMethods.DataSetHasRows(ds))
            {
                return returnList;
            }
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                long cusno;
                if (long.TryParse(row["CUSNO"].ToString(), out cusno))
                {
                    returnList.Add(cusno);
                }
            }
            return returnList;
        }

        public List<string> GetCustomerXtraFields(long cusno)
        {
            var returnList = new List<string>();
            var ds = GetCustomer(cusno);
            if (!DbHelpMethods.DataSetHasRows(ds))
            {
                return returnList;
            }
            try
            {
                returnList.Add(ds.Tables[0].Rows[0]["XTRA01"].ToString());
                returnList.Add(ds.Tables[0].Rows[0]["XTRA02"].ToString());
                returnList.Add(ds.Tables[0].Rows[0]["XTRA03"].ToString());
            }
            catch (Exception ex)
            {
                new Logger(string.Format("KayakHandler.GetCustomerXtraFields() failed for cusno: {0}", cusno), ex.ToString());
            }
            return returnList;
        }

        //Cached long time
        public List<DateTime> GetPublDays(string sPaperCode, string sProductNo, DateTime dteFirstDate, DateTime dteLastDate)
        {
			var cacheKey = string.Format("KayakHandler_GetPublDays_{0}_{1}_{2}_{3}", sPaperCode, sProductNo, dteFirstDate, dteLastDate);
            // If list is cached, return it
            var data = HttpRuntime.Cache.Get(cacheKey);
            if (data != null)
            {
                return (List<DateTime>)data;
            }
            var publDates = new List<DateTime>();
            var ds = Ws.GetPublDays_(sPaperCode, sProductNo, dteFirstDate, dteLastDate);
            if (!DbHelpMethods.DataSetHasRows(ds))
            {
                return publDates;
            }
            publDates.AddRange(from DataTable dt in ds.Tables from DataRow dr in dt.Rows select DateTime.Parse(dr["ISSUEDATE"].ToString()));
            // Cache this
            HttpRuntime.Cache.Insert(
                cacheKey,
                publDates,
                null,
                DateTime.Now.AddMinutes(Settings.CacheTimeMinutesLong),
                Cache.NoSlidingExpiration);
            return publDates;
        }

        public List<DateTime> GetProductsIssueDatesInInterval(string paperCode, string productno, DateTime dateMin, DateTime dateMax)
        {
            return GetPublDays(paperCode, productno, dateMin, dateMax);
        }

        public DateTime GetIssueDate(string paperCode, string productno, DateTime date, EnumIssue.Issue issue)
        {
            DateTime fromDate;
            DateTime toDate;
            switch (issue)
            {
                case EnumIssue.Issue.FirstBeforeInDate:
                    fromDate = date.AddMonths(-1).Date; //Subtract at least a month to be sure to catch a couple of issueDates
                    toDate = date.AddDays(-1).Date;
                    break;
                case EnumIssue.Issue.FirstAfterInDate:
                    fromDate = date.AddDays(1).Date;
                    toDate = date.AddMonths(1).Date; //Add at least a month to be sure to catch a couple of issueDates
                    break;
                case EnumIssue.Issue.InDateOrFirstBeforeInDate:
                    fromDate = date.AddMonths(-1).Date; //Subtract at least a month to be sure to catch a couple of issueDates
                    toDate = date.Date;
                    break;
                case EnumIssue.Issue.InDateOrFirstAfterInDate:
                    fromDate = date.Date;
                    toDate = date.AddMonths(1).Date; //Add at least a month to be sure to catch a couple of issueDates
                    break;
                default:
                    fromDate = date.Date;
                    toDate = date.Date;
                    break;
            }
            var issueList = GetPublDays(paperCode, productno, fromDate, toDate);
            return (issue == EnumIssue.Issue.FirstBeforeInDate || issue == EnumIssue.Issue.InDateOrFirstBeforeInDate)
                ? issueList.Max()
                : issueList.Min();
        }

        public DateTime GetNextIssuedate(string papercode, string productno, DateTime minDate)
        {
            try
            {
                var dateString = Ws.GetNextIssuedate_CII_(papercode, productno, minDate);
                DateTime dt;
                if (DateTime.TryParse(dateString, out dt))
                {
                    return dt;
                }
                throw new Exception("TryParse(GetNextIssueDate) failed, dateString = " + dateString);
            }
            catch (Exception ex)
            {
                new Logger("KayakHandler.GetNextIssueDate() - failed for papercode:" + papercode + ", productno:" + productno + ", minDate:" + minDate, ex.ToString());
            }
            return DateTime.MinValue;
        }

        public DateTime GetNextIssueDateIncDiRules(DateTime wantedDate, string paperCode, string productNo)
        {
            wantedDate = wantedDate.Date;

            //unknown product
            if (string.IsNullOrEmpty(paperCode) || string.IsNullOrEmpty(productNo))
            {
                new Logger(string.Format("KayakHandler.GetNextIssueDateIncDiRules() empty call, PaperCode:{0}, ProductNo: {1}", paperCode, productNo));
                //return DateTime.MinValue.Date;
                return GetClosesIssueDateInTheory(paperCode, productNo);
            }

            //get minDate for subs start by di rules
            var minDate = GetClosesIssueDateInTheory(paperCode, productNo);

            //minDate 'earlier' then wantedDate
            if (minDate < wantedDate)
                minDate = wantedDate;

            return GetNextIssuedate(paperCode, productNo, minDate);
        }

        public DateTime GetClosesIssueDateInTheory(string paperCode, string productNo)
        {
            var dt = DateTime.Now.Date;

            if (paperCode != Settings.PaperCode_AGENDA && productNo == Settings.ProductNo_Weekend)
            {
                switch (dt.DayOfWeek)
                {
                    case DayOfWeek.Monday:
                        return dt.AddDays(4);
                    case DayOfWeek.Tuesday:
                        return dt.AddDays(3);
                    case DayOfWeek.Wednesday:
                        return dt.AddDays(9);
                    case DayOfWeek.Thursday:
                        return dt.AddDays(8);
                    case DayOfWeek.Friday:
                        return dt.AddDays(7);
                    case DayOfWeek.Saturday:
                        return dt.AddDays(6);
                    default:
                        return dt.AddDays(5);
                }
            }
            switch (dt.DayOfWeek)
            {
                case DayOfWeek.Monday:
                case DayOfWeek.Tuesday:
                case DayOfWeek.Wednesday:
                    return dt.AddDays(3);
                default:
                    return dt.AddDays(5);
            }
        }

        public string CreateRenewal_DI(long lSubscusno, long lSubsno, int iExtno, long lPricelistno, long lCampno, int iSubslenMons, int iSubslenDays,
            DateTime dateSubsStartdate, DateTime dateSubsEnddate, string sSubskind, double dblTotalPrice, double dblItemPrice, int iItemqty,
            string sSalesno, long lPaycusno, string sPackageId, string sPaperCode, string sProductno, string sReceiveType, string sTargetGroup, string sPriceAtStart, string sOtherSubsno,
            string sOrderId, string sAutogiro, string sPriceGroup, string sInvMode)
        {
            try
            {
                if (lPaycusno < 1)
                {
                    lPaycusno = lSubscusno;
                }
                return Ws.CreateRenewal_CII_(KayakUserId, lSubscusno, lPaycusno, lSubsno, iExtno, sSubskind, sPackageId, sPriceGroup, string.Empty, lCampno, iSubslenMons,
                    iSubslenDays, dateSubsStartdate, dateSubsStartdate, dateSubsEnddate, iItemqty, sSalesno, DateTime.Today, sTargetGroup, sReceiveType, sInvMode,
                    sOrderId, false, 0, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, dateSubsEnddate, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
            }
            catch (Exception ex)
            {
                new Logger("KayakHandler.CreateRenewal_DI() - failed", ex.ToString());
            }
            return string.Empty;
        }

        public string DefinitiveAddressChange(long cusno, string street, string houseNo, string stairCase, string apartment, string careOf, string zip, DateTime startDate)
        {
            var sb = new StringBuilder();

            try
            {
                sb.Append("cusno:" + cusno + ", ");
                sb.Append("street:" + street + ", ");
                sb.Append("houseNo:" + houseNo + ", ");
                sb.Append("stairCase:" + stairCase + ", ");
                sb.Append("apartment:" + apartment + ", ");
                sb.Append("careOf:" + careOf + ", ");
                sb.Append("zip:" + zip + ", ");
                sb.Append("startDate:" + startDate.ToShortDateString());

                var s = Ws.AddNewBasicAddress_CII_(KayakUserId, cusno, street, houseNo, stairCase, apartment, careOf, string.Empty, "SE", zip, startDate, string.Empty, string.Empty, Settings.sReceiveType, false);

                if (s != ResponseOk)
                {
                    new Logger("KayakHandler.DefinitiveAddressChange() failed, returned FALSE", sb + "<hr>" + s);
                }

                return s;
            }
            catch (Exception ex)
            {
                new Logger("KayakHandler.DefinitiveAddressChange() failed, cusno:" + cusno, string.Format("{0}, EXCEPTION: {1}", sb, ex));
            }
            return string.Empty;
        }

        public string DefinitiveAddressChangeRemove(long cusno, DateTime startDate)
        {
            var sb = new StringBuilder();
            try
            {
                sb.Append("cusno:" + cusno + ", ");
                sb.Append("startDate:" + startDate.ToShortDateString());

                var s = Ws.RemoveWaitingBasicAddress_CII_(KayakUserId, cusno, startDate);

                if (s != ResponseOk)
                {
                    new Logger("KayakHandler.DefinitiveAddressChangeRemove() failed", sb + "<hr>" + s);
                }

                return s;
            }
            catch (Exception ex)
            {
                new Logger("KayakHandler.DefinitiveAddressChangeRemove() failed, exception", sb + "<hr>" + ex);
            }
            return string.Empty;
        }

        // TODO: Obsolete! Should not be used!
        public List<StringPair> GetNumSubsForPayingCust(string customerRowText1SearchStr)
        {
            throw new NotImplementedException();
        }

        // TODO: Obsolete! Should not be used!
        public int GetNumSubsForPayingCustOnline(string customerRowText1SearchStr)
        {
            throw new NotImplementedException();
        }

        //TODO: Remove? As this was last used 2011-12-20 if look at last made page of template DiGoldFreePlusSubs.aspx that is the only place using this method!!!
        public bool AddSubsIpadSummer(long cusno)
        {
            var ret = "FAILED";
            try
            {
                var campNo = 1169;
                if (MiscFunctions.GetAppsettingsValue("UseCirixTestWS") == "true")
                {
                    campNo = 1113;
                }

                var dtStart = DateTime.Now;
                var dtEnd = new DateTime(2011, 8, 31);

                // TODO: Visual Basic DateAndTime?!
                var mons = int.Parse(DateAndTime.DateDiff(DateInterval.Month, dtStart, dtEnd).ToString());
                if (dtStart.Day > dtEnd.Day)
                    mons--;

                var days = int.Parse(DateAndTime.DateDiff(DateInterval.Day, dtStart.AddMonths(mons), dtEnd).ToString());

                ret = Ws.CreateNewSubs_CII_(KayakUserId, cusno, cusno, "02", string.Empty, "00", "01", campNo, mons, days, dtStart.Date,
                    dtStart.Date, dtEnd.Date, 1, string.Empty, "GULDPLUS", Settings.sReceiveType, 0, 0, string.Empty, string.Empty, false,
                    0, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                    string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
            }
            catch (Exception ex)
            {
                new Logger("KayakHandler.AddSubsIpadSummer() - failed", ex.ToString());
            }

            return !ret.StartsWith("FAILED");
        }
        
        public List<Person> FindCustomerByPerson(Person searchCriterias, bool includeEmailAsSearchCriteria)
        {
            var pers = new List<Person>();
            var tmpCn = new List<long>(); //avoid multiple identical cusnos in return list
            var ds = FindCustomers2(searchCriterias, includeEmailAsSearchCriteria);
            if (!DbHelpMethods.DataSetHasRows(ds))
            {
                return pers;
            }
            foreach (var cusno in from DataTable dt in ds.Tables from DataRow dr in dt.Rows select long.Parse(dr["CUSNO"].ToString()) into cusNos where !tmpCn.Contains(cusNos) select cusNos)
            {
                tmpCn.Add(cusno);
				var p = new Person() { Cusno = cusno };
                pers.Add(p);
            }
            return pers;
        }

        public long TryAddCustomer2(Subscription sub, bool isSubscriber)
        {
            long cusno = -10;
            try
            {
                var p = isSubscriber ? sub.Subscriber : sub.SubscriptionPayer;
                var pnum = (!string.IsNullOrEmpty(p.SocialSecurityNo)) ? p.SocialSecurityNo : string.Empty;
                var sCusType = string.IsNullOrEmpty(p.CirixName2) ? Settings.sCusType : Settings.sCusTypeCorp;
                if (isSubscriber && p.PhysicalAddressMissing)
                {
                    p.CirixName2 = p.Email; //make non-address-customer unique to avoid cirix search hits on "wrong" customer
                    p.ZipCode = "10000";    //cirix needs a zipcode to work 
                }

                var bCollectInv = (Settings.sCollectInv.ToUpper() == "Y" || Settings.sCollectInv.ToUpper() == "TRUE");
                
                var resultString = CreateNewCustomer(KayakUserId, p.CirixName1, p.CirixName2, string.Empty, p.FirstName, p.LastName,
                    p.StreetName, p.HouseNo, p.StairCase, p.Stairs, p.CirixStreet2, string.Empty, Settings.Country, p.ZipCode,
                    string.Empty, p.PhoneDayTime, p.MobilePhone, p.Email, Settings.Nets_CurrencyCode,
                    string.Empty, string.Empty, bCollectInv,
                    sCusType, string.Empty, string.Empty, Settings.sNotes, Settings.sExpDay,
                    Settings.sTerms, pnum, string.Empty, string.Empty, string.Empty,
                    Settings.sCategory, Settings.lMasterCusno, string.Empty, p.CompanyNo);

                if (!long.TryParse(resultString, out cusno))
                {
                    new Logger(string.Format("KayakHandler.TryAddCustomer2() - Webservice transaction failed for subsno: {0}", sub.SubsNo));
                }
                return cusno;
            }
            catch (Exception ex)
            {
                new Logger(string.Format("KayakHandler.TryAddCustomer2() casted an exception. SubsNo: {0}", sub.SubsNo), ex.ToString());
                return cusno;
            }
        }

        public string AddNewSubs2(Subscription sub, long cusNoPay, DiPlusSubscription plusSub)
        {
            var ret = "FAILED";
            try
            {
                //var communeCode = GetCommuneCode(sub.Subscriber.ZipCode);
                //var priceListNo = GetPriceListNo(sub.PaperCode, sub.ProductNo, sub.SubsStartDate, communeCode, sub.Pricegroup, sub.CampNo.ToString());
                var cusNoSub = sub.Subscriber.Cusno;
                var priceGroup = sub.Pricegroup;
                if (plusSub != null && plusSub.SubsType != DiPlusSubscriptionType.PlusSubsType.StandAlonePlusSubs)
                    priceGroup = MiscFunctions.GetAppsettingsValue("awdPriceGroupUpg");

                var wholeCampaign = GetOneCampaignParsed(sub.CampNo).FirstOrDefault();
                // If operation is successful returns subscription and package subscription number, like: 7000088;0
                // If operation is not successful returns FAILED
                var modifiedPayCusno = cusNoPay > 0 ? cusNoPay : cusNoSub; //Do not send in 0 (zero) as 3rd parameter lPaycusno as it will be extremly slow request!
                ret = Ws.CreateNewSubs_CII_(KayakUserId, cusNoSub, modifiedPayCusno, sub.SubsKind, wholeCampaign.PackageId, priceGroup, sub.Substype, sub.CampNo,
                    sub.SubsLenMons, sub.SubsLenDays, sub.SubsStartDate,
                    sub.SubsStartDate, sub.SubsEndDate, sub.ItemQty,
                    Settings.sSalesNo, sub.TargetGroup, Settings.sReceiveType,
                    Settings.dblDiscAmount, 0, sub.InvoiceMode, string.Empty, false,
                    0, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                    string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);

                if (!ret.ToUpper().StartsWith("FAILED")) //Kayak returns "7000046;0" on success, subscription- and package number
                {
                    var retParts = ret.Split(';');
                    var onlySubsNoPart = retParts[0];
                    sub.SubsNo = long.Parse(onlySubsNoPart);
                    TryAddCoProdIncl(sub, cusNoSub, cusNoPay, sub.SubsNo);
                    TryAddSubToBonDig(sub, plusSub);
                    return onlySubsNoPart;
                }
            }
            catch (Exception ex)
            {
                new Logger("KayakHandler.AddNewSubs2() - failed", ex.ToString());
            }
            return ret;
        }
        
        public void ChangeCusInvMode(long cusno, string newInvoiceMode, string oldInvoiceMode)
        {
            if (string.IsNullOrEmpty(newInvoiceMode) || string.IsNullOrEmpty(oldInvoiceMode) || newInvoiceMode == oldInvoiceMode)
                return;

            var sb = new StringBuilder();
            sb.Append("cusno:" + cusno + ", ");
            sb.Append("oldInvoiceMode:" + oldInvoiceMode + ", ");
            sb.Append("newInvoiceMode:" + newInvoiceMode);

            try
            {
                var oldPaperCode = oldInvoiceMode == "03" ? "DI" : "ALL";
                var newPaperCode = newInvoiceMode == "03" ? "DI" : "ALL";
                var ret = Ws.ChangeCusInvMode_CII_(cusno, oldInvoiceMode, oldPaperCode, newInvoiceMode, true,
                    string.Empty, string.Empty, string.Empty, newPaperCode, false);

                if (ret.StartsWith("FAILED"))
                    new Logger("KayakHandler.ChangeCusInvMode() failed. " + sb, "Return from Kayak: " + ret);
            }
            catch (Exception ex)
            {
                new Logger("KayakHandler.ChangeCusInvMode() failed, exception. " + sb, ex.ToString());
            }
        }

        public string TryGetDefaultCusInvMode(long cusno)
        {
            try
            {
                var ds = Ws.GetCusInvModes_CII_(cusno);
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (var tblRow in from DataTable dt in ds.Tables from dr in dt.Rows.Cast<DataRow>().Where(dr => dr["DEFAULT"].ToString().ToUpper() == "TRUE") select dr)
                    {
                        return tblRow["INVMODE"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                new Logger("KayakHandler.TryGetDefaultCusInvMode() failed for cusno:" + cusno, ex.ToString());
            }

            return string.Empty;
        }

        public void AddNewCusInvmode(long cusno, string invMode, bool isInvDefault)
        {
            var sb = new StringBuilder();
            sb.Append("cusno:" + cusno + ", ");
            sb.Append("invMode:" + invMode + ", ");
            sb.Append("isInvDefault:" + isInvDefault);

            try
            {
				var ret = Ws.AddNewCusInvMode_CII_(cusno, invMode, isInvDefault, string.Empty, string.Empty, string.Empty, "ALL", isInvDefault, "01");
                if (ret.StartsWith("FAILED"))
                    new Logger("KayakHandler.AddNewCusInvmode() failed. " + sb, "Kayak return: " + ret);
            }
            catch (Exception ex)
            {
                new Logger("KayakHandler.AddNewCusInvmode() failed, exception. " + sb, ex.ToString());
            }
        }

        public long GetNextInvno()
        {
            return Ws.GetNextInvno_();
        }

        public string BuildRefno2(long lInvno, string sInvType, string sPaperCode)
        {
            return Ws.BuildRefno2_(lInvno, sInvType, sPaperCode);
        }

        public long CreateImmediateInvoice(Subscription subscription, int iExtno, int iItemno, long lInvno,
            string sRefno)
        {
            var subscriber = GetSubscriptions2(subscription.Subscriber.Cusno);
            var selectedSub = subscriber.FirstOrDefault(s => s.SubsNo == subscription.SubsNo);
            if (selectedSub == null)
            {
                return 0;
            }
            var returnValue = Ws.CreateImmediateInvoice_CII_(subscription.SubsNo, iExtno, iItemno, 0,
                selectedSub.PackageId, 0, sRefno, lInvno, 0);
            long invoiceNumber;
            if (SubscriptionController.ActiveHandler == SubscriptionController.AvailableHandlers.Kayak)
            {
                //returnValue from Kayak is in format like: "OK REFNO: 11005168 INVNO: 11005163 OPENAMOUNT: 3095"
                var match = Regex.Match(returnValue, @"(INVNO:[\s]*)([\d]*[\s]*)");
                if (match.Success)
                {
                    returnValue = match.Groups[2].Value;
                }
            }

            if (!long.TryParse(returnValue, out invoiceNumber))
            {
                new Logger(string.Format("KayakHandler.CreateImmediateInvoice() failed for subsno: {0}",
                    subscription.SubsNo));
            }

            return invoiceNumber;
        }

        public string InsertElectronicPayment(long lCusno, long lInvno, string sRefno, double dAmount)
        {
            return Ws.CreatePaymentOnInvoice_CII_(lInvno, DateTime.Now.Date, DateTime.Now.Date, DateTime.Now.Date, string.Empty, string.Empty, "KORT", dAmount, "02");
        }

        public long CreateNewInvoice(Subscription subscription)
        {
            var invno = GetNextInvno();
            var refno = BuildRefno2(invno, "00", subscription.PaperCode);
            var immInv = CreateImmediateInvoice(subscription, 0, 1, invno, refno);
            return immInv;
        }

        public DataSet GetOpenInvoices(long lCusno)
        {
            return Ws.GetOpenInvoices_(lCusno, KayakUserId);
        }

        public DataSet GetInvArgItems(long subsno, int extno)
        {
            return Ws.GetInvArgItems_(subsno, extno);
        }

        public DataSet FindCustomers(long lCusno, long lInvno, long lSubsno, string sName1, string sName2, string sName3, string sPhone, string sEmail, string sStreet, 
            string sHouseNo, string sStaircase, string sApartment, string sCountry, string sZipcode, string sUserid, string sPostname)
        {
            return Ws.FindCustomers_CII_(lCusno, lInvno, lSubsno, sName1, sName2, sName3, sPhone, string.Empty, sEmail, sStreet,
                sHouseNo, sStaircase, sApartment, sCountry, sZipcode, sUserid, sPostname, string.Empty, string.Empty, string.Empty, string.Empty);
        }

        public DataSet GetCustomerProperties(long lCusno)
        {
            return Ws.GetCustomerProperties_CII_(lCusno);
        }

        public string CleanCustomerProperties(long lCusno)
        {
            return Ws.CleanCustomerProperties_(lCusno);
        }

        public string UpdateCustomerExtraInfo(long lCusno, string sExtra1, string sExtra2, string sExtra3)
        {
            return Ws.UpdateCustomerExtraInfo_(lCusno, sExtra1, sExtra2, sExtra3);
        }

        public DataSet GetSubsSleeps(long subscriptionNumber)
        {
            return Ws.GetSubsSleeps_CII_(KayakUserId, subscriptionNumber, 0);
        }

        public DataSet GetCurrentAndPendingAddressChanges(long cusno, long subsno)
        {
            return Ws.GetCurrentAndPendingAddressChanges_(cusno, subsno);
        }

        public DataSet GetCusTempAddresses(long customerNumber, string sOnlyThisCountry, string sOnlyValid)
        {
            return Ws.GetCusTempAddresses_(customerNumber, sOnlyThisCountry, sOnlyValid);
        }

        public long CreateSubssleep_CII(long lSubsno, DateTime dateSleepStartDate, DateTime dateSleepEndDate, string sSleepType, string sCreditType, string sAllowWebPaper, string sSleepReason, string sReceiveType, string sSleepLimit)
        {
            return Ws.CreateSubssleep_CII_(KayakUserId, lSubsno, dateSleepStartDate, dateSleepEndDate, sSleepType, sCreditType, sAllowWebPaper, sSleepReason, sReceiveType, sSleepLimit);
        }

        public string CreateHolidayStop(long lCusno, string sCusName, long lSubsno, DateTime dateSleepStartDate, DateTime dateSleepEndDate, string sSleepType, 
            string sAllowWebpaper, string sCreditType)
        {
            return Ws.CreateHolidayStop_CII_(KayakUserId, lSubsno, 0, dateSleepStartDate, dateSleepEndDate, sSleepType, sCreditType,
                sAllowWebpaper, false, string.Empty, Settings.sReceiveType, string.Empty);
        }

        public string DeleteHolidayStop(long lCusno, string sCusName, long lSubsno, DateTime dateSleepStartDate, DateTime dateSleepEndDate)
        {
            return Ws.DeleteHolidayStop_CII_(KayakUserId, lSubsno, 0, dateSleepStartDate);
        }

		public string TemporaryAddressChange(string sUserId, long lCusno, long lSubsno, int iExtno, AddressMap addressMap, DateTime dAddrStartDate, DateTime dAddrEndDate, string sNewName1, string sNewName2, string sInvToTempAddress)
        {
			return Ws.AddNewTemporaryAddress_CII_(sUserId, lCusno, lSubsno, iExtno, addressMap.StreetName, addressMap.Houseno, addressMap.Staircase, addressMap.Apartment,
                addressMap.Street2, addressMap.Street3, addressMap.CountryCode, addressMap.ZipCode, dAddrStartDate, dAddrEndDate, sInvToTempAddress, sNewName1, sNewName2,
                Settings.PaperCode_DI, Settings.sReceiveType, false);
        }

		//150417 - obsolete
		//public string TemporaryAddressChangeNewAddress(long lCusno, long lSubsno, string sStreetname, string sHouseno, string sStaircase, string sApartment, string sStreet2, string sStreet3, string sCountrycode, string sZipcode, string sNewName1, string sNewName2, DateTime dateStartdate, DateTime dateEnddate, string sInvToTempAddress)
		//{
		//	return Ws.TemporaryAddressChangeNewAddress_(KayakUserId, lCusno, lSubsno, sStreetname, sHouseno, sStaircase, sApartment, sStreet2, sStreet3, sCountrycode, sZipcode, sNewName1, sNewName2, dateStartdate, dateEnddate, sInvToTempAddress);
		//}

		public string TemporaryAddressChangeRemove(string sUserId, long lCusno, long lSubsno, int iExtno, int iAddrno, DateTime dOriginalStartDate)
        {
			return Ws.RemoveWaitingTemporaryAddress_CII_(sUserId, lCusno, lSubsno, iExtno, dOriginalStartDate);
        }

        public long IdentifyCustomer(long lCusno, long lSubsno, long lInvno, string sName1, string sName2, string sName3, string sPhone, string sEmail, string sStreet, string sHouseno, string sStaircase, string sApartment, string sCountry, string sZipcode, string sPostname, string sUserId)
        {
            return Ws.IdentifyCustomer_(lCusno, lSubsno, lInvno, sName1, sName2, sName3, sPhone, sEmail, sStreet, sHouseno, sStaircase, sApartment, sCountry, sZipcode, sPostname, sUserId);
        }

        public string UpdateWwwPassword(long lCusno, string sNewPassword1, string sNewPassword2, bool bCheckOldPassword, string sOldPassword)
        {
            return Ws.UpdateWwwPassword_CII_(lCusno, sNewPassword1, sNewPassword2, bCheckOldPassword, sOldPassword);
        }

        public bool IsHybridSubscriber(long cusno)
        {
            var dsSubs = GetSubscriptions(cusno, false);
            if (!DbHelpMethods.DataSetHasRows(dsSubs))
            {
                return false;
            }
            foreach (DataRow dr in dsSubs.Tables[0].Rows)
            {
                var papercode = dr["PAPERCODE"].ToString();
                var productno = dr["PRODUCTNO"].ToString();
                if (papercode != Settings.PaperCode_DI || productno != Settings.ProductNo_Weekend)
                {
                    continue;
                }
                long campno;
                long.TryParse(dr["CAMPNO"].ToString(), out campno);
                if (campno <= 0)
                {
                    continue;
                }
                if (IsHybridCampaign(campno))
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsHybridCampaign(long campNo)
        {
            var dsCamp = GetOneCampaign(campNo);
            if (!DbHelpMethods.DataSetHasRows(dsCamp))
            {
                return false;
            }
            long campgroupid;
            long.TryParse(dsCamp.Tables[0].Rows[0]["CAMPGROUPID"].ToString(), out campgroupid);
            if (Settings.HybridCampGroupIds.Contains(campgroupid))
            {
                return true;
            }
            return false;
        }

        public IEnumerable<DataSet> GetActiveCampaigns()
        {
            var productPackages = RequestAllProductPackages();
            foreach (var productPackage in productPackages) 
            {
                var dataSet = Ws.GetActiveCampaigns_CII_(productPackage.PackageId);
                if (dataSet == null || !DbHelpMethods.DataSetHasRows(dataSet))
                {
                    continue;
                }

                yield return dataSet;
            }            
        }

        public string GetPostName(string zipCode, string countryCode = "SE")
        {
            
            return Ws.GetPostName_CII(countryCode, zipCode);
        }

        public long GetCustomerByEcusno(long ecusno)
        {
            return Ws.GetCustomerByEcusno_CII(ecusno);
        }

        public long GetEcusnoByCustomer(long cusno)
        {
            return Ws.GetEcusnoByCustomer_CII(cusno);
        }

        public List<PackageProduct> GetPackageProducts(string packageId)
        {
            var serializer = new JavaScriptSerializer();
            var productJson = RequestJsonData(string.Format(GetWsUrl() + "/GetPackageProductsJSON?sPackageId={0}", packageId));
            var packageProducts = serializer.Deserialize<PackageProducts>(productJson);
            try
            {
                return packageProducts.Table.Select(packageProduct => new PackageProduct()
                {
                    AbcRule = packageProduct["ABC_RULE"],
                    AllowDigitalProductActive = packageProduct["ALLOW_DIGITAL_PRODUCT_ACTIVE"],
                    InvoicingProduct = packageProduct["INVOICINGPRODUCT"],
                    MainProduct = packageProduct["MAINPRODUCT"],
                    PackageId = packageProduct["PACKAGEID"],
                    PaperCode = packageProduct["PAPERCODE"],
                    PaperName = packageProduct["PAPERNAME"],
                    PeriodicDigiProdSleepRule = packageProduct["PERIODIC_DIGIPROD_SLEEPRULE"],
                    ProductName = packageProduct["PRODUCTNAME"],
                    ProductNo = packageProduct["PRODUCTNO"]
                }).ToList();
            }
            catch (Exception ex)
            {
                new Logger("KayakHandler.GetPackageProducts() failed, packageId: " + packageId, ex.ToString());
            }
            return new List<PackageProduct>();
        }

        #region *** Private methods ***
        private static string CreateNewCustomer(string sUserId, string sName1, string sName2, string sName3, string sFirstName, string sLastName, string sStreetname, 
            string sHouseno, string sStaircase, string sApartment, string sStreet2, string sStreet3, string sCountrycode, string sZipcode, string sHomePhone, 
            string sWorkPhone, string sOtherPhone, string sEmailAddress, string sCurrency, string sAccnoBank, string sAccnoAcc, bool bCollectInv, string sCusType, string sWwwUserId, 
            string sWwwPinCode, string sNotes, string sExpday, string sTerms, string sSocialSecNo, string sExtra1, string sExtra2, 
            string sExtra3, string sCategory, long lMasterCusno, string sOtherCusno, string sCompanyId)
        {
            return Ws.CreateNewCustomer_CII_(sUserId, sName1, sName2, sName3, sFirstName, sLastName,
                sStreetname, sHouseno, sStaircase, sApartment, sStreet2,
                sStreet3, sCountrycode, sZipcode,
                sHomePhone, sWorkPhone, sOtherPhone, sEmailAddress, sCurrency, sAccnoBank, sAccnoAcc,
                bCollectInv, sCusType, sWwwUserId, sWwwPinCode, sNotes, sExpday, sTerms, sSocialSecNo, sExtra1, sExtra2, sExtra3,
                sCategory, lMasterCusno, sOtherCusno, false, string.Empty, DateTime.MinValue, sCompanyId, string.Empty, false);
        }

        private static List<long> GetActiveCampNos(string packageId)
        {
            var cacheKey = string.Format("KayakHandler_GetActiveCampNos_{0}", packageId);
            
            // If list is cached, return it
            var data = HttpRuntime.Cache.Get(cacheKey);
            if (data != null)
            {
                return (List<long>)data;
            }
            var list = new List<long>();
            var dataSet = Ws.GetActiveCampaigns_CII_(packageId);
            if (dataSet == null || !DbHelpMethods.DataSetHasRows(dataSet))
            {
                return list;
            }
            foreach (DataRow dr in dataSet.Tables[0].Rows)
            {
                try
                {
                    list.Add(long.Parse(dr["CAMPNO"].ToString()));
                }
                catch (Exception ex)
                {
                    new Logger(string.Format("KayakHandler.GetActiveCampNos/( failed for packageId:{0}", packageId), ex.ToString());
                }
            }

            // Cache this
            HttpRuntime.Cache.Insert(
                cacheKey,
                list,
                null,
                DateTime.Now.AddSeconds(Settings.CacheTimeSecondsShort),
                Cache.NoSlidingExpiration);
            return list;
        }

        private static List<string> GetSortedTargetGroups(string paperCode)
        {
            var hs = new HashSet<string>();

            var ds = Ws.GetParameterValuesByGroup_(paperCode, "TARGETGRPS");
            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (var codeValue in from DataTable dt in ds.Tables from DataRow dr in dt.Rows select dr["CODEVALUE"].ToString() into codeValues where !string.IsNullOrEmpty(codeValues) select codeValues)
                {
                    hs.Add(codeValue);
                }
            }
            var ret = hs.ToList();
            ret.Sort();
            return ret;
        }

        private static List<CampaignInfo> ParseCampaignDataSet(DataSet ds)
        {
            var campaignList = new List<CampaignInfo>();
            if (ds == null || !DbHelpMethods.DataSetHasRows(ds))
            {
                return campaignList;
            }
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                try
                {
                    var packageId = dr["PACKAGEID"].ToString();
                    var campaignPackageProducts = SubscriptionController.GetPackageProducts(packageId);
                    campaignList.Add(new CampaignInfo()
                    {
                        CampNo = int.Parse(dr["CAMPNO"].ToString()),
                        CampId = dr["CAMPID"].ToString(),
                        CampName = dr["CAMPNAME"].ToString(),
                        PackageId = packageId,
                        PaperCode = campaignPackageProducts[0].PaperCode, //dr["PAPERCODE"].ToString(), DO NOT USE PAPERCODE FROM CAMPAIGN
                        ProductNo = campaignPackageProducts[0].ProductNo, //dr["PRODUCTNO"].ToString(), DO NOT USE PRODUCTNO FROM CAMPAIGN
                        CampStartDate = DateTime.Parse(dr["CAMPSTARTDATE"].ToString()),
                        CampEndDate = DateTime.Parse(dr["CAMPENDDATE"].ToString()),
                        PerDiscount = DbHelpMethods.ValueIfColumnExist<decimal>(ds.Tables[0], dr, "PERDISCOUNT", 0),
                        StandDiscount = DbHelpMethods.ValueIfColumnExist<decimal>(ds.Tables[0], dr, "STANDDISCOUNT", 0),
                        Discpercent = DbHelpMethods.ValueIfColumnExist<decimal>(ds.Tables[0], dr, "DISCPERCENT", 0),
                        TotalPrice = DbHelpMethods.ValueIfColumnExist<decimal>(ds.Tables[0], dr, "TOTALPRICE", -1)
                    });
                }
                catch (Exception ex)
                {
                    new Logger("KayakHandler.ParseCampaignDataSet - failed to parse one of the campaign properties.", ex.Message);
                }
            }
            return campaignList;
        }

        private static Person ParseCustomerDataSetToPerson(DataSet ds)
        {
            var person = new Person();
            if (ds == null || !DbHelpMethods.DataSetHasRows(ds))
            {
                return person;
            }
            try
            {
                person.FirstName = ds.Tables[0].Rows[0]["FIRSTNAME"] as string;
                person.LastName = ds.Tables[0].Rows[0]["LASTNAME"] as string;
                person.Email = ds.Tables[0].Rows[0]["EMAILADDRESS"] as string;
                person.MobilePhone = ds.Tables[0].Rows[0]["O_PHONE"] as string ?? string.Empty;
                var rt2 = ds.Tables[0].Rows[0]["ROWTEXT2"] as string;
                person.Company = (string.IsNullOrEmpty(rt2) || rt2.Contains("@")) ? string.Empty : ds.Tables[0].Rows[0]["ROWTEXT1"] as string;
            }
            catch (Exception ex)
            {
                new Logger("KayakHandler.ParseCustomerDataSetToPerson() - failed to parse one of the dataset values.", ex.Message);
            }
            return person;
        }

        private ApsisCustomer ParseCustomerDataSetToApsisCustomer(DataSet ds)
        {
            if (ds == null || !DbHelpMethods.DataSetHasRows(ds))
            {
                return null;
            }
            var apsisCustomer = new ApsisCustomer();
            try
            {
                apsisCustomer.Name = ds.Tables[0].Rows[0]["ROWTEXT1"].ToString();
                apsisCustomer.Email = ds.Tables[0].Rows[0]["EMAILADDRESS"].ToString();
                apsisCustomer.UserName = ds.Tables[0].Rows[0]["WWWUSERID"].ToString();
                apsisCustomer.f_OfferdenEmail = ds.Tables[0].Rows[0]["OFFERDEN_EMAIL"].ToString();
                apsisCustomer.f_CountryCode = ds.Tables[0].Rows[0]["COUNTRYCODE"].ToString();

                int.TryParse(ds.Tables[0].Rows[0]["CUSNO"].ToString(), out apsisCustomer.CustomerId);

                if (apsisCustomer.CustomerId == 0)
                {
                    return apsisCustomer;
                }

                var subscriptionList = GetSubscriptions2(apsisCustomer.CustomerId);

                var customerLatestSub = subscriptionList
                    .Where(sub => Settings.SubsStateActiveValues.Contains(sub.SubsState) && string.IsNullOrEmpty(sub.CancelReason.Trim()))
                    .OrderByDescending(s => s.SubsStartDate).FirstOrDefault();

                if (customerLatestSub == null)
                {
                    return apsisCustomer;
                }

                apsisCustomer.f_PaperCode = customerLatestSub.PaperCode;
                apsisCustomer.f_ProductNo = customerLatestSub.ProductNo;
                apsisCustomer.TargetGroup = customerLatestSub.TargetGroup;
                apsisCustomer.InvStartDate = customerLatestSub.InvStartDate;
                apsisCustomer.SubsEndDate = customerLatestSub.SubsEndDate;
                apsisCustomer.CampNo = (int)customerLatestSub.CampNo;
                apsisCustomer.SubsLenMonsFromCirix = customerLatestSub.SubsLenMons;
            }
            catch (Exception ex)
            {
                new Logger("KayakHandler.ParseCustomerDataSetToApsisCustomer() failed", ex.ToString());
            }
            return apsisCustomer;
        }

        
        private static bool SubShorterThen3Months(Subscription sub)
        {
            var subsLenDays = (sub.SubsLenMons * 30) + sub.SubsLenDays;
            return subsLenDays <= 90;
        }

        private static DataSet FindCustomers2(Person person, bool includeEmailAsSearchCriteria)
        {
            if (person == null)
            {
                return null;
            }
            //todo: find out how sensitive search is...
            //string sPhone = "";  //!string.IsNullOrEmpty(person.PhoneMobile) ? person.PhoneMobile : "";  //: person.PhoneDayTime;
            //string sPhone = string.IsNullOrEmpty(person.MobilePhone) ? "" : person.MobilePhone;

            var email = "";
            if (includeEmailAsSearchCriteria && !string.IsNullOrEmpty(person.Email))
            {
                email = person.Email.Trim(' ');
            }

            return Ws.FindCustomers_CII_(person.Cusno, //lCusNo
                0, //lInvNo
                0, //lSubsNo
                person.CirixName1.Trim(' ').ToUpper(), //sname1
                person.CirixName2.Trim(' ').ToUpper(), //sname2
                string.Empty, //sname3
                string.Empty, //sPhone.Trim(' '),                     //phone
                string.Empty, //SocialSecNo
                email, //email
                person.StreetName.Trim(' ').ToUpper(), //sStreet
                person.HouseNo.Trim(' '), //sHouseNo
                string.Empty, //sStairCase
                string.Empty, //sApartment
                string.Empty, //sCountry
                person.ZipCode.Trim(' '), //sZipCode
                string.Empty, //sUserId       WEBCIRIX or "" ?
                person.City.Trim(' ').ToUpper(), //sPostName
                string.Empty, //OtherCusno
                string.Empty, //OrderId
                string.Empty, //PaperCode
                string.Empty //sReferenceNo
                );
        }

        private static void TryAddCoProdIncl(Subscription sub, long cusNoSub, long cusNoPay, long subsNo)
        {
            if (sub.CoProdIncl != "Y")
            {
                return;
            }
            try
            {
                var ds = Ws.GetCampaignCoProducts_(sub.CampId);
                if (ds != null && ds.Tables[0].Rows != null && ds.Tables[0].Rows[0] != null)
                {
                    Ws.CreateExtraExpenseItem_CII_(KayakUserId, cusNoSub, cusNoPay, sub.PaperCode, sub.ProductNo, 0, sub.SubsStartDate, sub.SubsStartDate, "01", 1,
                        false, string.Empty, string.Empty, string.Empty, string.Empty, cusNoSub, sub.SubsStartDate, false, DateTime.Now, false,
                        subsNo, sub.ExtNo, int.Parse(ds.Tables[0].Rows[0]["EXPCODE"].ToString()), 0, 0, double.Parse(ds.Tables[0].Rows[0]["DISCPERCENT"].ToString()));
                }
            }
            catch (Exception ex)
            {
                new Logger("KayakHandler.TryAddCoProdIncl() - failed", ex.ToString());
            }
        }

        private static void TryAddSubToBonDig(Subscription sub, DiPlusSubscription plusSub)
        {
            //130410 - only subs from dagensindustri.se/lasplatta are added to bondig
            if (plusSub == null)
            {
                return;
            }
            var cusno = sub.Subscriber.Cusno;
            var subsno = sub.SubsNo;
            var email = sub.Subscriber.Email;
            //var passwd = (!string.IsNullOrEmpty(sub.Subscriber.PasswordBonDig)) ? sub.Subscriber.PasswordBonDig : sub.Subscriber.Password;
            //var passwd = sub.Subscriber.Password;
            //if (plusSub != null)
            var passwd = plusSub.Passwd;

            var ret = BonDigHandler.TryAddCustAndSubToBonDig(sub.PaperCode, sub.ProductNo, cusno, subsno, email, sub.Subscriber.FirstName,
                sub.Subscriber.LastName, sub.Subscriber.MobilePhone, passwd, true, SubscriptionController.IsHybridCampaign(sub.CampNo));
            if (ret > 0)
            {
                var userAddedToBonDig = (ret == 1) ? true : false;
                //BonDigHandler.SendBonDigWelcomeMail(userAddedToBonDig, email, passwd);   //130410 - only apsis welcome mail should be sent to cust
            }
            else
            {
                //SendFailedToCrateIpadLoginMail(sub);
                new Logger("KayakHandler.TryAddSubToBonDig() - failed. ret=" + ret + ", cusno=" + cusno + ", subsno=" + subsno + ", email=" + email + ", passwd=" + passwd, "not an exception, but subscription was not saved to Bonnier digital");
            }
        }

        private static List<Product> GetPapersAndProducts(string sUserId)
        {
            var returnList = new List<Product>();
            var allPapersAndProducts = Ws.GetPapersAndProducts_CII_(sUserId);
            if (!DbHelpMethods.DataSetHasRows(allPapersAndProducts))
            {
                return returnList;
            }
            returnList.AddRange(from DataRow dr in allPapersAndProducts.Tables[0].Rows
                select new Product()
                {
                    PackageId = dr["PACKAGEID"].ToString(),
                    PaperCode = dr["PAPERCODE"].ToString(),
                    PaperName = dr["PAPERNAME"].ToString(),
                    ProductNo = dr["PRODUCTNO"].ToString(),
                    ProductName = dr["PRODUCTNAME"].ToString()
                });
            return returnList;
        }
        
        private static DateTime SetDateFromString(string dateStr)
        {
            DateTime dt;
            return DateTime.TryParse(dateStr, out dt) ? dt : DateTime.MinValue;
        }

        private static List<string> GetFreeSubsCampIds()
        {
            var ids = Ws.GetParameterValue_("DI", "WELCOME", "FREE_CAMPAIGNS");
            var arr = ids.Split(',');
            return arr.Select(id => id.Trim()).ToList();
        }

        //TODO: Here only for backward compatibility for S+ (used by GetUpdatedCusnosInDateInterval()), remove when Kayak is fully released.
        private static DataSet GetUpdatedCustomersAsDataSet(DateTime dateMin, DateTime dateMax)
        {
            var returnDataSet = new DataSet();
            var allPapersAndProducts = GetPapersAndProducts(string.Empty);
            if (!allPapersAndProducts.Any())
            {
                return returnDataSet;
            }
            var paperCodesString = allPapersAndProducts.Select(p => p.PaperCode.ToUpper()).Aggregate((a, b) => a + ";" + b);

            return Ws.GetChangedCustomerSubs_CII_(dateMin, dateMax, MaxGetCustomerResult, paperCodesString);
        }

        //TODO: Welcome email functionality!
        private static void FlagCustsInLetterHelper(List<ApsisCustomer> custs, string flag)
        {
            // TODO: NEED NEW WS
            throw new NotImplementedException();
        }

        private static string OfferOpposite(string value)
        {
            return !string.IsNullOrEmpty(value) && value.ToUpper() == "Y" ? "N" : "Y";
        }

        private List<ApsisCustomer> ParseSubsForConfirmDataSetToApsisCustomer(DataSet ds)
        {
            if (ds == null || !DbHelpMethods.DataSetHasRows(ds))
            {
                return null;
            }
            var returnList = new List<ApsisCustomer>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                try
                {
                    var apsisCustomer = new ApsisCustomer();
                    apsisCustomer.CustomerId = int.Parse(dr["SUBSCUSNO"].ToString());
                    apsisCustomer.Name = dr["ROWTEXT1"].ToString();
                    apsisCustomer.Email = dr["EMAILADDRESS"].ToString();
                    apsisCustomer.UserName = dr["WWWUSERID"].ToString();
                    apsisCustomer.PassWord = dr["WWWPINCODE"].ToString();
                    apsisCustomer.SubsNoForConfirm = int.Parse(dr["SUBSNO"].ToString());
                    apsisCustomer.ExtNo = int.Parse(dr["EXTNO"].ToString());

                    // TODO: Is PaperCode and ProductNo needed, or can we use packageId?
                    var tempCustomer = CirixGetCustomer(apsisCustomer.CustomerId, apsisCustomer.SubsNoForConfirm, false);

                    if (tempCustomer == null)
                    {
                        new Logger("KayakHandler.ParseSubsForConfirmDataSetToApsisCustomer() - CirixGetCustomer returned null, cusno: " + apsisCustomer.CustomerId);
                        continue;
                    }

                    apsisCustomer.f_PaperCode = tempCustomer.f_PaperCode;
                    apsisCustomer.f_ProductNo = tempCustomer.f_ProductNo;
                    apsisCustomer.InvStartDate = tempCustomer.InvStartDate;

                    apsisCustomer.TargetGroup = dr["TARGETGROUP"].ToString();
                    apsisCustomer.CampId = string.Empty;
                    apsisCustomer.f_CountryCode = dr["COUNTRYCODE"].ToString();
                    apsisCustomer.f_OfferdenEmail = OfferOpposite(dr["EMAIL_MARKET_ALLOWED"].ToString());

                    var endDate = SetDateFromString(dr["SUBSENDDATE"].ToString());
                    if (endDate > DateTime.MinValue)
                    {
                        apsisCustomer.SubsEndDate = endDate;
                    }

                    int tempCampNo;
                    if (int.TryParse(dr["CAMPNO"].ToString(), out tempCampNo))
                    {
                        apsisCustomer.CampNo = tempCampNo;
                    }

                    apsisCustomer.ReceiveType = dr["RECEIVETYPE"].ToString();

                    int tmpSubsLenMons;
                    if (int.TryParse(dr["SUBSLEN_MONS"].ToString(), out tmpSubsLenMons))
                    {
                        apsisCustomer.SubsLenMonsFromCirix = tmpSubsLenMons;
                    }
                    returnList.Add(apsisCustomer);
                }
                catch (Exception ex)
                {
                    new Logger("KayakHandler.ParseSubsForConfirmDataSetToApsisCustomer() failed", ex.ToString());
                }                
            }

            return returnList;
        }

        private static bool CustHadDigSubThenGotPaperSub(long cusno)
        {
            var hasDigSub = false;
            var hasPaperSub = false;

            var minDateDigSub = DateTime.MaxValue;
            var maxDatePaperSub = DateTime.MinValue;

            var ds = Ws.GetSubscriptions_(cusno, true, KayakUserId, string.Empty);
            if (!DbHelpMethods.DataSetHasRows(ds))
            {
                return false;
            }
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                var paperCode = dr["PAPERCODE"].ToString();
                var dt = DateTime.Parse(dr["INVSTARTDATE"].ToString());

                if (paperCode == Settings.PaperCode_IPAD || paperCode == Settings.PaperCode_DISE)
                {
                    hasDigSub = true;
                    if (dt < minDateDigSub)
                        minDateDigSub = dt;
                }

                if (paperCode != Settings.PaperCode_DI)
                {
                    continue;
                }
                hasPaperSub = true;
                if (dt > maxDatePaperSub)
                    maxDatePaperSub = dt;
            }
            return (hasDigSub && hasPaperSub) && (minDateDigSub.Date < maxDatePaperSub.Date);
        }

        private string GetPackageId(string paperCode, string productNo)
        {
            var packageIdList = GetPackageIdList(paperCode, productNo);
            return packageIdList.FirstOrDefault();
        }

        private List<string> GetPackageIdList(string paperCode, string productNo)
        {
            var allProductPackages = RequestAllProductPackages();
            if (!allProductPackages.Any())
            {
                return new List<string>();
            }
            return (from productPackage in allProductPackages
                where productPackage.Products.Count == 1
                from product in productPackage.Products
                where product.PaperCode.ToUpper() == paperCode && product.ProductNo.ToUpper() == productNo
                select product.PackageId).Distinct().ToList();
        }

        private List<CampaignInfo> GetOneCampaignParsed(long campNo)
        {
            return ParseCampaignDataSet(GetOneCampaign(campNo));
        }

        private static DataSet GetOneCampaign(long campNo)
        {
            try
            {
                return Ws.GetCampaign_(campNo); // Do not use PaperCode and ProductNo from this result!

            }
            catch (Exception ex)
            {
                new Logger("KayakHandler.GetOneCampaign(long campNo) failed, campNo:" + campNo, ex.ToString());
                return null;
            }
        }
        
        private List<ProductPackage> RequestAllProductPackages()
        {
            var cacheKey = "KayakHandler.RequestAllProductPackages";
            // If list is cached, return it
            var data = HttpRuntime.Cache.Get(cacheKey);
            if (data != null)
            {
                return (List<ProductPackage>)data;
            }
            try
            {
                var json = RequestJsonData(GetWsUrl() + "/GetProductPackagesJSON");
                var serializer = new JavaScriptSerializer();
				var productPackages = serializer.Deserialize<ProductPackages>(json);
                var allProductPackages = new List<ProductPackage>();
                //Not able to deserialize nested classes. Refactor in future
                foreach (var p in productPackages.Table)
                {
                    try
                    {
                        var newProductPackage = new ProductPackage()
                        {
                            PackageId = p["PACKAGEID"],
                            PackageName = p["PACKAGENAME"],
                            StartDate = p["STARTDATE"],
                            EndDate = p["ENDDATE"],
                            PaperName = p["PAPERNAME"],
                            ProductName = p["PRODUCTNAME"],
                            Total = p["TOTAL"]
                        };
                        newProductPackage.Products = GetPackageProducts(newProductPackage.PackageId);
                        allProductPackages.Add(newProductPackage);
                    }
                    catch (Exception innerEx)
                    {
                        new Logger("KayakHandler.RequestAllProductPackages() inner exception on package", innerEx.ToString());
                    }
                }
                // Cache this
                HttpRuntime.Cache.Insert(
                    cacheKey,
                    allProductPackages,
                    null,
                    DateTime.Now.AddSeconds(Settings.CacheTimeSecondsShort),
                    Cache.NoSlidingExpiration);
                return allProductPackages;
            }
            catch (Exception ex)
            {
                new Logger("KayakHandler.RequestAllProductPackages() failed", ex.ToString());
            }
            return null;
        }
        
        private static string RequestJsonData(string url, string method = "GET")
        {
            var jsonString = string.Empty;
            try
            {
                var req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = method;
                var resp = (HttpWebResponse)req.GetResponse();
                var sr = new StreamReader(resp.GetResponseStream());
                jsonString = sr.ReadToEnd();
                sr.Close();
            }
            catch (Exception ex)
            {
                new Logger("KayakHandler.RequestJsonData() failed", ex.ToString());
            }

            return jsonString;
        }

        private static string GetWsUrl()
        {
            if (string.IsNullOrEmpty(MiscFunctions.GetAppsettingsValue("KayakTestWSUrl")))
                throw new Exception("Missing AppSetting KayakTestWSUrl");
            if (string.IsNullOrEmpty(MiscFunctions.GetAppsettingsValue("KayakProdWSUrl")))
                throw new Exception("Missing AppSetting KayakProdWSUrl");

            bool useTestWs;
            if (!bool.TryParse(MiscFunctions.GetAppsettingsValue("UseKayakTestWS"), out useTestWs))
                useTestWs = false;

            if (useTestWs)
            {
                return MiscFunctions.GetAppsettingsValue("KayakTestWSUrl");
            }
            return MiscFunctions.GetAppsettingsValue("KayakProdWSUrl");
        }
        #endregion

        public string TestOracleConnection()
        {
            return Ws.TestOracleConnection_();
        }

    }
}