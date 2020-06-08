using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Data.OracleClient;
using System.Data;
using System.Web;
using System.Web.Caching;

using DIClassLib.Campaign;
using DIClassLib.DbHelpers;
using DIClassLib.EPiJobs.Apsis;
//using DIClassLib.CirixTest;
using DIClassLib.Misc;
using DIClassLib.Subscriptions;
using DIClassLib.Subscriptions.DiPlus;
using Microsoft.Contracts;
using DIClassLib.DbHandlers.CirixWrappers;
using Microsoft.VisualBasic;
using DIClassLib.BonnierDigital;
//using System.ComponentModel;


namespace DIClassLib.DbHandlers
{

    public static class CirixDbHandler
    {
        private static string _connStrCirix = ConfigurationManager.ConnectionStrings["Cirix"].ToString();

        private static ICirix Ws
        {
            get
            {
                bool useTestWs;
                if (!bool.TryParse(MiscFunctions.GetAppsettingsValue("UseCirixTestWS"), out useTestWs))
                    useTestWs = false;

                if (useTestWs)
                    return new CirixTestWrapper();
                else
                    return new CirixProdWrapper();
            }
        }

        public static ICirix CirixWebServiceForAdapter
        {
            get { return Ws; }
        }

        public static HashSet<string> GetTargetGroups(string paperCode)
        {
            var cacheKey = "CirixDbHandler_GetTargetGroups_" + paperCode;

            // If list is cached, return it
            var data = HttpRuntime.Cache.Get(cacheKey);
            if (data != null)
            {
                return (HashSet<string>)data;
            }

            HashSet<string> hs = new HashSet<string>();

            DataSet ds = Ws.GetParameterValuesByGroup_(paperCode, "TARGETGRPS");
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                foreach (DataTable dt in ds.Tables)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        string s = dr["CODEVALUE"].ToString();
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

        /// <summary>
        /// use List<StringPair> to populate DDL
        /// </summary>
        public static List<StringPair> GetAllTargetGroups()
        {
            List<StringPair> ret = new List<StringPair>();

            List<string> di = GetSortedTargetGroups(Settings.PaperCode_DI);
            List<string> dise = GetSortedTargetGroups(Settings.PaperCode_DISE);
            List<string> ipad = GetSortedTargetGroups(Settings.PaperCode_IPAD);
            
            //List<string> ipad = GetTargetGroups(Settings.PaperCode_IPAD);         //same resultset as DI
            
            //ret.Add(new StringPair(Settings.PaperCode_DI + " / " + Settings.PaperCode_IPAD, ""));
            ret.Add(new StringPair(Settings.PaperCode_DI, ""));
            ret.Add(new StringPair("-----------------------", ""));
            foreach(string s in di)
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

        private static List<string> GetSortedTargetGroups(string paperCode)
        {
            HashSet<string> hs = new HashSet<string>();
            
            DataSet ds = Ws.GetParameterValuesByGroup_(paperCode, "TARGETGRPS");
            if(ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                foreach (DataTable dt in ds.Tables)
                { 
                    foreach (DataRow dr in dt.Rows)
                    {
                        string s = dr["CODEVALUE"].ToString();
                        if (!string.IsNullOrEmpty(s))
                            hs.Add(s);
                    }
                }
            }

            List<string> ret = hs.ToList();
            ret.Sort();
            return ret;
        }


        //public static Cirix.CirixWebServiceSoapClient Ws
        //{
        //    get { return new Cirix.CirixWebServiceSoapClient(); }
        //}

        //public static CirixTest.CirixWebServiceSoapClient Ws
        //{
        //    get { return new CirixTest.CirixWebServiceSoapClient(); }
        //}


        //public static DataSet GetSubsChoices2(string pageId)
        //{
        //    try
        //    {
        //        return Ws.GetSubsChoices2_("DI", DateTime.Now, pageId);
        //    }
        //    catch (Exception ex)
        //    {
        //        new DIClassLib.DbHelpers.Logger("GetSubsChoices2() failed", ex.ToString());
        //        return null;
        //    }
        //}

        //public static DataSet GetSubsChoices2(string paperCode, DateTime targetDate, string pageId)
        //{
        //    try
        //    {
        //        return Ws.GetSubsChoices2_(paperCode, targetDate, pageId);
        //    }
        //    catch (Exception ex)
        //    {
        //        new DIClassLib.DbHelpers.Logger("GetSubsChoices2() failed", ex.ToString());
        //        return null;
        //    }
        //}

                

        public static DataSet GetCampaign(long campNo)
        {
            try
            {
                return Ws.GetCampaign_(campNo);
            }
            catch (Exception ex)
            {
                new DIClassLib.DbHelpers.Logger("GetCampaign() failed for campNo:" + campNo, ex.ToString());
                return null;
            }
        }

        public static DataSet GetCampaign(string campId)
        {
            return GetCampaign(GetCampno(campId));
        }

        public static long GetCampno(string campId)
        {
            var cacheKey = "CirixDbHandler_GetCampno_" + campId;

            // If list is cached, return it
            var data = HttpRuntime.Cache.Get(cacheKey);
            if (data != null)
            {
                return (long)data;
            }

            List<long> campNos = new List<long>();
            OracleConnection conn = null;
            OracleCommand cmd = null;
            OracleDataReader rdr = null;

            try
            {
                conn = new OracleConnection(_connStrCirix);
                conn.Open();
                cmd = new OracleCommand("select CAMPNO from campaign where CAMPID='" + campId + "'", conn);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                    campNos.Add(long.Parse(rdr[0].ToString()));
            }
            catch (Exception ex)
            {
                new Logger("GetCampno(campId) failed", ex.Message);
            }
            finally
            {
                KillOracleDbObjects(conn, cmd, rdr);
            }

            int num = campNos.Count;
            if (num != 1)
            {
                new Logger("GetCampno(campId) - " + num.ToString() + " campNos in list for campId=" + campId, "");
                return -1;
            }
            // Cache this 
            HttpRuntime.Cache.Insert(
                cacheKey,
                campNos[0],
                null,
                DateTime.Now.AddSeconds(Settings.CacheTimeSecondsMedium),
                Cache.NoSlidingExpiration);
            return campNos[0];
        }

        public static string GetProductName(string paperCode, string productNo)
        {
            var productName = string.Empty;
            OracleConnection conn = null;
            OracleCommand cmd = null;

            try
            {
                conn = new OracleConnection(_connStrCirix);
                conn.Open();
                var commandString = string.Format("SELECT PRODUCTNAME FROM product WHERE PAPERCODE='{0}' AND PRODUCTNO='{1}'", paperCode, productNo);
                cmd = new OracleCommand(commandString, conn);
                var result = cmd.ExecuteOracleScalar();

                if (result != null)
                {
                    productName = result.ToString();
                }
            }
            catch (Exception ex)
            {
                new Logger(string.Format("GetProductName({0}, {1}) failed", paperCode, productNo), ex.Message);
            }
            finally
            {
                KillOracleDbObjects(conn, cmd, null);
            }
            return productName;
        }

        public static long GetCusnoByLogin(string userid, string passwd)
        {
            List<long> cusnos = new List<long>();
            OracleConnection conn = null;
            OracleCommand cmd = null;
            OracleDataReader rdr = null;

            try
            {
                conn = new OracleConnection(_connStrCirix);
                conn.Open();
                cmd = new OracleCommand("select cusno from customer where wwwuserid='" + userid + "' and passwd='" + passwd + "'", conn);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                    cusnos.Add(long.Parse(rdr[0].ToString()));
            }
            catch (Exception ex)
            {
                new Logger("GetCusnoByLogin() failed", ex.Message);
            }
            finally
            {
                KillOracleDbObjects(conn, cmd, rdr);
            }

            int num = cusnos.Count;
            if (num != 1)
            {
                new Logger("GetCusnoByLogin() - " + num.ToString() + " cusnos in list for userid=" + userid + ", passwd=" + passwd, "");
                return -1;
            }

            return cusnos[0];
        }
        
        public static List<CampaignInfo> GetActiveCampaigns(string paperCode, string productNo)
        {
            var cacheKey = string.Format("CirixDbHandler_GetActiveCampaigns_{0}{1}", paperCode, productNo);

            // If list is cached, return it
            var data = HttpRuntime.Cache.Get(cacheKey);
            if (data != null)
            {
                return (List<CampaignInfo>)data;
            }

            var dataSet = Ws.GetActiveCampaigns_(paperCode, DateTime.Now, productNo);
            var list = ParseCampaignDataSet(dataSet);
            // Cache this
            HttpRuntime.Cache.Insert(
                cacheKey,
                list,
                null,
                DateTime.Now.AddSeconds(Settings.CacheTimeSecondsLong),
                Cache.NoSlidingExpiration);
            return list;
        }

        public static List<CampaignInfo> GetActiveFreeCampaigns(string paperCode, string productNo)
        {
            var listAll = GetActiveCampaigns(paperCode, productNo);
            return listAll.Where(l => l.TotalPrice == 0).ToList();
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
                    campaignList.Add(new CampaignInfo()
                    {
                        CampNo = int.Parse(dr["CAMPNO"].ToString()),
                        CampId = dr["CAMPID"].ToString(),
                        CampName = dr["CAMPNAME"].ToString(),
                        PaperCode = dr["PAPERCODE"].ToString(),
                        ProductNo = dr["PRODUCTNO"].ToString(),
                        CampStartDate = DateTime.Parse(dr["CAMPSTARTDATE"].ToString()),
                        CampEndDate = DateTime.Parse(dr["CAMPENDDATE"].ToString()),
                        PerDiscount = decimal.Parse(dr["PERDISCOUNT"].ToString()),
                        StandDiscount = decimal.Parse(dr["STANDDISCOUNT"].ToString()),
                        Discpercent = decimal.Parse(dr["DISCPERCENT"].ToString()),
                        TotalPrice = decimal.Parse(dr["TOTALPRICE"].ToString())
                    });
                }
                catch (Exception ex)
                {
                    new Logger("GetActiveCampaigns - failed to parse one of the campaign properties.", ex.Message);
                }
            }
            return campaignList;
        }

        public static string GetCommuneCode(string zipCode)
        {
            try
            {
                return Ws.GetCommuneCode_(zipCode, Settings.Country);
            }
            catch
            {
                throw;
            }
        }


        public static long GetPriceListNo(string paperCode, string productNo, DateTime invStartDate, string communeCode,
                                            string priceGr, string campId)
        {
            try
            {
                return Ws.GetPricelistno_(paperCode, productNo, invStartDate, communeCode, priceGr, campId);
            }
            catch
            {
                throw;
            }
        }


        public static long CreateNewCustomer(string sUserID, string sName1, string sName2, string sName3, string sFirstName, string sLastName,
                                              string sStreetname, string sHouseno, string sStaircase, string sApartment, string sStreet2,
                                              string sStreet3, string sCountrycode, string sZipcode, string sHomePhone, string sWorkPhone,
                                              string sOtherPhone, string sEmailAddress, string sOfferden_Dir, string sOfferden_Sal,
                                              string sOfferden_Email, string sSalesDen, string sOfferden_SMS, string sCurrency,
                                              string sInvMode, string sAccnoBank, string sAccnoAcc, string sCollectInv, string sCusType,
                                              string sWWWUserID, string sWWWPinCode, string sNotes, string sExpday, double dDiscPercent,
                                              string sTerms, string sSocialSecNo, string sExtra1, string sExtra2, string sExtra3,
                                              string sCategory, long lMasterCusno, string sGenUidAndPin, string sOtherCusno, string sCompanyId)
        {
            try
            {
                return Ws.CreateNewCustomer_(sUserID, sName1, sName2, sName3, sFirstName, sLastName,
                                             sStreetname, sHouseno, sStaircase, sApartment, sStreet2,
                                             sStreet3, sCountrycode, sZipcode, sHomePhone, sWorkPhone,
                                             sOtherPhone, sEmailAddress, sOfferden_Dir, sOfferden_Sal,
                                             sOfferden_Email, sSalesDen, sOfferden_SMS, sCurrency,
                                             sInvMode, sAccnoBank, sAccnoAcc, sCollectInv, sCusType,
                                             sWWWUserID, sWWWPinCode, sNotes, sExpday, dDiscPercent,
                                             sTerms, sSocialSecNo, sExtra1, sExtra2, sExtra3, sCategory, 
                                             lMasterCusno, sGenUidAndPin, sOtherCusno, sCompanyId);
            }
            catch
            {
                throw;
            }
        }

        public static string UpdateCustomerInformation(long lCusNo, string sEmailAddress, string sHPhone, string sWPhone, string sOPhone, string sSalesDen,
                                                       string sOfferdenDir, string sOfferdenSal, string sOfferdenEmail, string sDenySmsMark, string sAccnoBank,
                                                       string sAccnoAcc, string sNotes, long lEcusno, string sOtherCusno, string sWWWUserID, string sExpday,
                                                       double dDiscPercent, string sTerms, string sSocialSecNo, string sCategory, long lMasterCusno, string companyId)
        {
            try
            {
                return Ws.UpdateCustomerInformation_(lCusNo, sEmailAddress, sHPhone, sWPhone, sOPhone, sSalesDen,
                                                    sOfferdenDir, sOfferdenSal, sOfferdenEmail, sDenySmsMark, sAccnoBank,
                                                    sAccnoAcc, sNotes, lEcusno, sOtherCusno, sWWWUserID, sExpday,
                                                    dDiscPercent, sTerms, sSocialSecNo, sCategory, lMasterCusno, companyId);
            }
            catch
            {
                throw;
            }
        }

        public static string GetWWWPassword(long cusNo)
        {
            try
            {
                return Ws.GetWWWPassword_(cusNo);
            }
            catch (Exception ex)
            {
                new DIClassLib.DbHelpers.Logger("GetWWWPassword() - failed for cusNo:" + cusNo.ToString(), ex.ToString());
            }

            return string.Empty;
        }

        public static string GetWWWUserId(long cusno)
        {
            try
            {
                DataSet ds = GetCustomer(cusno);
                if (ds != null && ds.Tables != null && ds.Tables[0].Rows != null && ds.Tables[0].Rows.Count > 0)
                    return ds.Tables[0].Rows[0]["WWWUSERID"].ToString();
            }
            catch (Exception ex)
            {
                new Logger("GetWWWUserId() failed for cusno=" + cusno, ex.ToString());
            }

            return string.Empty;
        }

        public static DataSet GetCustomer(long cusNo)
        {
            try
            {
                return Ws.GetCustomer_(cusNo);
            }
            catch (Exception ex)
            {
                new DIClassLib.DbHelpers.Logger("GetGetCustomer() - failed for cusNo:" + cusNo.ToString(), ex.ToString());
                //throw;
                return null;
            }
        }

        public static Person GetCustomerInfo(long cusNo)
        {
            var ds = GetCustomer(cusNo);
            return ParseCustomerDataSet(ds);
        }

        private static Person ParseCustomerDataSet(DataSet ds)
        {
            var customerInfo = new Person();
            if (ds == null || !DbHelpMethods.DataSetHasRows(ds))
            {
                return customerInfo;
            }
            /*
<ROWTEXT1>ST7PER LUNDKVIST</ROWTEXT1>
<ROWTEXT2>ST7@PER-LUNDKVIST.COM</ROWTEXT2>
<BOOKINGDATE>2014-05-12T00:00:00+02:00</BOOKINGDATE>
<COLLECTINV>Y</COLLECTINV>
<CUSTYPE>01</CUSTYPE>
<CUSSTATE>01</CUSSTATE>
<INVMODE>04</INVMODE>
<FIRSTNAME>LUNDKVIST</FIRSTNAME>
<LASTNAME>ST7PER</LASTNAME>
<EMAILADDRESS>st7@per-lundkvist.com</EMAILADDRESS>
<WWWUSERID>3695889</WWWUSERID>
<COUNTRYCODE>SE</COUNTRYCODE>
<ZIPCODE>10000</ZIPCODE>
<POSTNAME>STOCKHOLM</POSTNAME>
<SALESDEN>N</SALESDEN>
<OFFERDEN_DIR>N</OFFERDEN_DIR>
<OFFERDEN_SAL>N</OFFERDEN_SAL>
<OFFERDEN_EMAIL>N</OFFERDEN_EMAIL>
<DENYSMSMARK>N</DENYSMSMARK>
<ECUSNO>0</ECUSNO>
<DISCPERCENT>0</DISCPERCENT>
<MASTERCUSNO>0</MASTERCUSNO>
             */
           
                try
                {
                    customerInfo.FirstName = ds.Tables[0].Rows[0]["FIRSTNAME"] as string;
                    customerInfo.LastName = ds.Tables[0].Rows[0]["LASTNAME"] as string;
                    customerInfo.Email = ds.Tables[0].Rows[0]["EMAILADDRESS"] as string;
                    customerInfo.MobilePhone = ds.Tables[0].Rows[0]["O_PHONE"] as string ?? string.Empty;
                    var rt2 = ds.Tables[0].Rows[0]["ROWTEXT2"] as string;
                    customerInfo.Company = (string.IsNullOrEmpty(rt2) || rt2.Contains("@")) ? string.Empty : ds.Tables[0].Rows[0]["ROWTEXT1"] as string;
                }
                catch (Exception ex)
                {
                    new Logger("ParseCustomerDataSet - failed to parse one of the dataset values.", ex.Message);
                }
            
            return customerInfo;
        }

        public static string GetEmailAddress(long cusno)
        {
            string email = "";
            DataSet ds = GetCustomer(cusno);
            if (DbHelpMethods.DataSetHasRows(ds))
            {
                DataRow dr = ds.Tables[0].Rows[0];
                email = dr["EMAILADDRESS"].ToString();
            }
            return email.Trim();
        }


        //todo: 2 last arguments are new - find out values...
        public static string CreateNewSubs_DI(string sUserID, long lSubscusno, long lPaycusno, string sSubskind, string sPapercode,
                                              string sPriceGroup, string sProductno, string sSubstype, string sAutogiro, long lPricelistno,
                                              long lCampno, int iSubslenMons, int iSubslenDays, DateTime dateSubsStartDate,
                                              DateTime dateSubsEndDate, double dblTotalPrice, double dblGrossPrice, double dblItemPrice,
                                              int iItemqty, string sSalesno, string sTargetgroup, string sReceiveType, double dblDiscAmount,
                                              string sPriceAtStart, string sOtherSubsno, string sOrderID, string sInvMode)
        {
            try
            {
                return Ws.CreateNewSubs_DI_(sUserID, lSubscusno, lPaycusno, sSubskind, sPapercode,
                                           sPriceGroup, sProductno, sSubstype, sAutogiro, lPricelistno,
                                           lCampno, iSubslenMons, iSubslenDays, dateSubsStartDate,
                                           dateSubsEndDate, dblTotalPrice, dblGrossPrice, dblItemPrice,
                                           iItemqty, sSalesno, sTargetgroup, sReceiveType, dblDiscAmount,
                                           sPriceAtStart, sOtherSubsno, sOrderID, sInvMode);
            }
            catch
            {
                throw;
            }

            #region old call
            //return Ws.CreateNewSubs(sUserID, lSubscusno, lPaycusno, sSubskind, sPapercode,
            //                        sPriceGroup, sProductno, sSubstype, sAutogiro, lPricelistno,
            //                        lCampno, iSubslenMons, iSubslenDays, dateSubsStartDate,
            //                        dateSubsEndDate, dblTotalPrice, dblGrossPrice, dblItemPrice,
            //                        iItemqty, sSalesno, sTargetgroup, sReceiveType, dblDiscAmount);
            #endregion
        }


        /// <summary>
        /// Returns 0 on success, -1 on fail
        /// </summary>
        public static int InsertCustomerProperty(long cusno, string propCode, string propValue)
        {
            int ret = 0;

            try
            {
                string s = Ws.InsertCustomerProperty_DI_(cusno, propCode, propValue);

                if (s.ToUpper() != "OK")
                    ret = -1;
            }
            catch (Exception ex)
            {
                //throw;
                new DIClassLib.DbHelpers.Logger("InsertCustomerProperty() failed. " + Environment.NewLine +
                                             "cusno: " + cusno.ToString() + Environment.NewLine +
                                             "propCode: " + propCode + Environment.NewLine +
                                             "propValue: " + propValue,
                                             ex.ToString());
                ret = -1;
            }

            return ret;
        }


        public static DataSet GetSubscriptions(long cusNo, bool showPassiveIfNoActive, string sUserId)
        {
            try
            {
                string bo = showPassiveIfNoActive.ToString().ToUpper();
                return Ws.GetSubscriptions_(cusNo, bo, sUserId);

            }
            catch (Exception ex)
            {
                new Logger("GetSubscriptions() - failed for cusNo:" + cusNo, ex.ToString());
                return null;
            }
        }


        public static List<ApsisCustomer> CirixGetNewCusts(string sUserId)
        {
            List<ApsisCustomer> custs = new List<ApsisCustomer>();
            DefinitionFile defFile = new DefinitionFile();                  //to decide value of IsExtCustomer
            List<string> freeSubsCampIds = CirixGetFreeSubsCampIds();
            //List<string> companySubsCampIds = CirixGetCompanySubsCampIds();
            MailSenderDbHandler ms = new MailSenderDbHandler();
            DataSet dsRules = ms.GetEmailRules();

            OracleConnection conn = null;
            OracleCommand cmd = null;
            OracleDataReader rdr = null;

            try
            {
                #region sql
                StringBuilder sql = new StringBuilder();
                sql.Append("select l.CUSNO, l.rowtext1, l.EMAILADDRESS, l.USERID, l.PASSWD, l.PAPERCODE,");
                sql.Append("l.PRODUCTNO, l.TARGETGROUP, l.CAMPID, l.COUNTRYCODE, l.OFFERDEN_EMAIL,");
                sql.Append("l.SUBSSTARTDATE, l.SUBSENDDATE, l.CAMPNO, l.RECEIVETYPE, l.SUBSLEN_MONS,c.campgroupid ");
                sql.Append("from CIRIX.EXPCUSTOMER_LETTER l ");
                sql.Append("LEFT JOIN CIRIX.CAMPAIGN c on l.campid = c.campid WHERE ");
                sql.Append("l.UPDATED NOT IN ('Y', 'P') ");

                if (MiscFunctions.GetAppsettingsValue("WelcMailExcludeTargetGroupsFlag").ToUpper() == "TRUE")
                    sql.Append("and (l.TARGETGROUP IS NULL OR l.TARGETGROUP not in (" + MiscFunctions.GetAppsettingsValue("WelcMailExcludedTargetGroups") + ")) ");

                sql.Append("and (");
                sql.Append("(l.PAPERCODE IN ('" + Settings.PaperCode_DI + "', '" + Settings.PaperCode_DISE + "', '" + Settings.PaperCode_IPAD + "') ");
                sql.Append("and l.PRODUCTNO IN ('01', '05')) or ");
                sql.Append("l.PAPERCODE IN ('" + Settings.PaperCode_AGENDA + "')");
                sql.Append(")");
                //sql.Append("and COUNTRYCODE = 'SE'"

                conn = new OracleConnection(_connStrCirix);
                conn.Open();
                cmd = new OracleCommand(sql.ToString(), conn);
                rdr = cmd.ExecuteReader();
                #endregion
                while (rdr.Read())
                {
                    ApsisCustomer c = new ApsisCustomer();
                    SetCustomerProperties(c, rdr);

                    //add non empty strings that passes rules (most often email addresses)
                    //if (ms.EmailNotEmptyAndPassesRules(c.Email, dsRules))
                    if (ms.EmailNotEmptyAndPassesRules(c.Email, dsRules))
                    {
                        if (CustHadDigSubThenGotPaperSub(c.CustomerId, sUserId))
                        {
                            ApsisCustomer ac = new ApsisCustomer() { CustomerId = c.CustomerId };
                            FlagCustsInLetterHelper(new List<ApsisCustomer>() { ac }, "Y");
                        }
                        else
                        {
                            c.SetIsExtCustomer(defFile);
                            c.SetApsisProjectGuid(freeSubsCampIds);
                            custs.Add(c);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new Logger("CirixGetNewCusts() failed", ex.Message);
            }
            finally
            {
                KillOracleDbObjects(conn, cmd, rdr);
            }
            
            if (custs.Count >= 950)
            {
                List<ApsisCustomer> newList = new List<ApsisCustomer>();
                int i = 0;
                foreach(ApsisCustomer ac in custs)
                {
                    if (i == 950)
                        return newList;

                    newList.Add(ac);
                    i++;
                }

            }

            return custs;
        }

        /// <summary>
        /// 2014-04-15 Per Lundkvist, for manually get customer list from a list of customerIds
        /// </summary>
        /// <returns></returns>
        public static List<ApsisCustomer> CirixGetCustsManuallyFromList(IEnumerable<string> customerIds, string sUserid)
        {
            var custs = new List<ApsisCustomer>();
            var defFile = new DefinitionFile();                  //to decide value of IsExtCustomer
            var freeSubsCampIds = CirixGetFreeSubsCampIds();
            var ms = new MailSenderDbHandler();
            var dsRules = ms.GetEmailRules();

            OracleConnection conn = null;
            OracleCommand cmd = null;
            OracleDataReader rdr = null;

            try
            {
                #region sql
                var sql = new StringBuilder();

                sql.Append(@"select 
                            customer.cusno,
                            customer.rowtext1,
                            customer.emailaddress,
                            customer.passwd,
                            subs.papercode,
                            subs.productno,
                            subs.targetgroup,
                            subs.countrycode,
                            customer.OFFERDEN_EMAIL,
                            subs.subsstartdate, 
                            subs.subsenddate, 
                            subs.campno, 
                            subs.receivetype, 
                            subs.subslen_mons
                            from customer
                            INNER JOIN subs ON customer.cusno = subs.subscusno
                            WHERE
                            subs.subsno 
                            IN(
                            select MAX(subsno) from subs 
                            where ROWNUM = 1 
                            AND subscusno = customer.cusno 
                            and subsstate in ('00', '01', '02')
                            and (CANCELREASON IS NULL or CANCELREASON = '')
                            )
                            AND
                            customer.cusno IN(" + string.Join(",", customerIds.ToArray()) + @")
                            order by subs.stamp_date desc");
    

                conn = new OracleConnection(_connStrCirix);
                conn.Open();
                cmd = new OracleCommand(sql.ToString(), conn);
                rdr = cmd.ExecuteReader();
                #endregion
                while (rdr.Read())
                {
                    var c = new ApsisCustomer();
                    c.CustomerId = int.Parse(rdr[0].ToString());         
                    c.Name = rdr[1].ToString();                    
                    c.Email = rdr[2].ToString();                 
                    c.UserName = "";
                    c.PassWord = rdr[3].ToString();                  
                    c.f_PaperCode = rdr[4].ToString();                  
                    c.f_ProductNo = rdr[5].ToString();                  
                    c.TargetGroup = rdr[6].ToString();               
                    c.CampId = "";                   
                    c.f_CountryCode = rdr[7].ToString();                  
                    c.f_OfferdenEmail = rdr[8].ToString();              

                    DateTime dt = SetDateFromString(rdr[9].ToString());
                    if (dt > DateTime.MinValue)
                        //c.SubsStartDate = dt;

                    dt = SetDateFromString(rdr[10].ToString());
                    if (dt > DateTime.MinValue)
                        c.SubsEndDate = dt;

                    int tmpCN;
                    if (int.TryParse(rdr[11].ToString(), out tmpCN))
                        c.CampNo = tmpCN;

                    c.ReceiveType = rdr[12].ToString();

                    int tmpSubsLenMons;
                    if (int.TryParse(rdr[13].ToString(), out tmpSubsLenMons))
                        c.SubsLenMonsFromCirix = tmpSubsLenMons;


                    if (ms.EmailNotEmptyAndPassesRules(c.Email, dsRules))
                    {
                        if (!CustHadDigSubThenGotPaperSub(c.CustomerId, sUserid))
                        {
                            c.SetIsExtCustomer(defFile);
                            c.SetApsisProjectGuid(freeSubsCampIds);
                            custs.Add(c);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new Logger("CirixGetNewCusts() failed", ex.Message);
            }
            finally
            {
                KillOracleDbObjects(conn, cmd, rdr);
            }
            return custs;
        }

        //ONLY FOR TEST
        /*
        public static List<ApsisCustomer> CirixGetNewCustsTest()
        {
            var custs = new List<ApsisCustomer>();
            var defFile = new DefinitionFile();                  //to decide value of IsExtCustomer
            var freeSubsCampIds = CirixGetFreeSubsCampIds();
            //List<string> companySubsCampIds = CirixGetCompanySubsCampIds();
            var ms = new MailSenderDbHandler();
            var dsRules = ms.GetEmailRules();

            OracleConnection conn = null;
            OracleCommand cmd = null;
            OracleDataReader rdr = null;

            try
            {
                #region sql
                var sql = new StringBuilder();
                sql.Append("select CUSNO, rowtext1, EMAILADDRESS, USERID, PASSWD, PAPERCODE, ");
                sql.Append("PRODUCTNO, TARGETGROUP, CAMPID, COUNTRYCODE, OFFERDEN_EMAIL, ");
                sql.Append("SUBSSTARTDATE, SUBSENDDATE, CAMPNO, RECEIVETYPE ");
                sql.Append("from CIRIX.EXPCUSTOMER_LETTER ");
                sql.Append("WHERE ");
                sql.Append("UPDATED NOT IN ('Y', 'P') ");
                sql.Append("and (");
                sql.Append("(PAPERCODE IN ('" + Settings.PaperCode_DI + "', '" + Settings.PaperCode_DISE + "', '" + Settings.PaperCode_IPAD + "') ");
                sql.Append("and PRODUCTNO IN ('01', '05')) or ");
                sql.Append("PAPERCODE IN ('" + Settings.PaperCode_AGENDA + "')");
                sql.Append(")");
                //sql.Append("and COUNTRYCODE = 'SE'"

                conn = new OracleConnection(_connStrCirix);
                conn.Open();
                cmd = new OracleCommand(sql.ToString(), conn);
                rdr = cmd.ExecuteReader();
                #endregion
                while (rdr.Read())
                {
                    var c = new ApsisCustomer();
                    SetCustomerProperties(c, rdr);

                    //add non empty strings that passes rules (most often email addresses)
                    //if (ms.EmailNotEmptyAndPassesRules(c.Email, dsRules))
                    if (ms.EmailNotEmptyAndPassesRules(c.Email, dsRules))
                    {
                        if (CustHadDigSubThenGotPaperSub(c.CustomerId))
                        {
                            var ac = new ApsisCustomer() { CustomerId = c.CustomerId };
                            FlagCustsInLetterHelper(new List<ApsisCustomer>() { ac }, "Y");
                        }
                        else
                        {
                            c.SetIsExtCustomer(defFile);
                            c.SetApsisProjectGuid(freeSubsCampIds);
                            custs.Add(c);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new Logger("CirixGetNewCusts() failed", ex.Message);
            }
            finally
            {
                KillOracleDbObjects(conn, cmd, rdr);
            }

            if (custs.Count >= 950)
            {
                var newList = new List<ApsisCustomer>();
                int i = 0;
                foreach (var ac in custs)
                {
                    if (i == 950)
                        return newList;

                    newList.Add(ac);
                    i++;
                }
            }
            return custs;
        }
        */

        public static bool CustHadDigSubThenGotPaperSub(long cusno, string sUserId)
        {
            bool hasDigSub = false;
            bool hasPaperSub = false;
            
            DateTime minDateDigSub = DateTime.MaxValue;
            DateTime maxDatePaperSub = DateTime.MinValue;

            DataSet ds = Ws.GetSubscriptions_(cusno, "TRUE", sUserId);
            if (DbHelpMethods.DataSetHasRows(ds))
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                { 
                    string paperCode = dr["PAPERCODE"].ToString();
                    DateTime dt = DateTime.Parse(dr["INVSTARTDATE"].ToString());

                    if (paperCode == Settings.PaperCode_IPAD || paperCode == Settings.PaperCode_DISE)
                    {
                        hasDigSub = true;
                        if (dt < minDateDigSub)
                            minDateDigSub = dt;
                    }

                    if (paperCode == Settings.PaperCode_DI)
                    {
                        hasPaperSub = true;
                        if (dt > maxDatePaperSub)
                            maxDatePaperSub = dt;
                    }
                }
            }

            if ((hasDigSub && hasPaperSub) && (minDateDigSub.Date < maxDatePaperSub.Date))
                return true;

            return false;
        }

        /// <summary>
        /// Returns list of customer numbers on success [1,2,3] and on failed execution [-1].
        /// TODO: Check and compare query with GetUpdatedCustomers() it they should use same query-logic maybe?
        /// </summary>
        public static List<long> GetUpdatedCusnosInDateInterval(DateTime dateMin, DateTime dateMax)
        {
            List<long> cusnos = new List<long>();
            StringBuilder sql = new StringBuilder();
            OracleConnection conn = null;
            OracleCommand cmd = null;
            OracleDataReader rdr = null;

            try
            {
                sql.Append("select distinct cusno from expcustomer WHERE ");
                sql.Append("STAMP_PROGRAM <> 'SUBSBAL' and ");
                sql.Append("stamp_date >= to_date('" + dateMin.ToString() + "','yyyy-mm-dd HH24:MI:SS') and ");
                sql.Append("stamp_date <= to_date('" + dateMax.ToString() + "','yyyy-mm-dd HH24:MI:SS') and ");
                sql.Append("PAPERCODE IN ('" + Settings.PaperCode_DI + "', '" + Settings.PaperCode_IPAD + "', '" + Settings.PaperCode_DISE + "', '" + Settings.PaperCode_AGENDA + "')");

                new Logger("GetUpdatedCusnosInDateInterval() SQL:" + sql.ToString());

                conn = new OracleConnection(_connStrCirix);
                conn.Open();
                cmd = new OracleCommand(sql.ToString(), conn);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                    cusnos.Add(long.Parse(rdr[0].ToString()));
            }
            catch (Exception ex)
            {
                new Logger("GetUpdatedCusnosInDateInterval() failed for dateMin:" + dateMin.ToString() + ", dateMax:" + dateMax.ToString(), "sql: " + sql.ToString() + Environment.NewLine + ex.ToString());
                cusnos.Add(-1);
            }
            finally
            {
                KillOracleDbObjects(conn, cmd, rdr);
            }

            return cusnos;
        }

        private static void SetCustomerProperties(ApsisCustomer c, OracleDataReader rdr)
        {
            c.CustomerId = int.Parse(rdr[0].ToString());         //"3451192"
            c.Name = rdr[1].ToString();                    //"PALM KRISTOFER"
            c.Email = rdr[2].ToString();                    //"kristofer@queensbridge.se"
            c.UserName = rdr[3].ToString();                    //"3451192"
            c.PassWord = rdr[4].ToString();                    //"xxxxxxxxx"
            c.f_PaperCode = rdr[5].ToString();                    //"DISE"
            c.f_ProductNo = rdr[6].ToString();                    //"01"
            c.TargetGroup = rdr[7].ToString();                    //""
            c.CampId = rdr[8].ToString();                    //""
            c.f_CountryCode = rdr[9].ToString();                    //"SE"
            c.f_OfferdenEmail = rdr[10].ToString();                 //""

            DateTime dt = SetDateFromString(rdr[11].ToString());
            if (dt > DateTime.MinValue)
                //c.SubsStartDate = dt;

            dt = SetDateFromString(rdr[12].ToString());
            if (dt > DateTime.MinValue)
                c.SubsEndDate = dt;

            int tmpCN;
            if (int.TryParse(rdr[13].ToString(), out tmpCN))
                c.CampNo = tmpCN;

            c.ReceiveType = rdr[14].ToString();

            int tmpSubsLenMons;
            if (int.TryParse(rdr[15].ToString(), out tmpSubsLenMons))
                c.SubsLenMonsFromCirix = tmpSubsLenMons;
            try
            {
                int campGroupId;
                if (int.TryParse(rdr[16].ToString(), out campGroupId))
                {
                    c.CampGroupId = campGroupId;
                }
            }
            catch (Exception ex)
            {
                c.CampGroupId = -1;
                new Logger("CirixDbHandler.SetCustomerProperties() called with reader without campgroupid column!");
            }
        }

        private static DateTime SetDateFromString(string dateStr)
        {
            DateTime dt;
            if (DateTime.TryParse(dateStr, out dt))
                return dt;

            return DateTime.MinValue;
        }


        /// <summary>
        /// flags:
        /// N - not processed. 
        /// P - in process. 
        /// Y - processed (ok to delete post in cirix).
        /// </summary>
        public static void FlagCustsInLetter(List<ApsisCustomer> custs, string flag)
        {
            //cirix can only handle 1000 items in an sql command, so we need to batch
            
            List<ApsisCustomer> tmpList = new List<ApsisCustomer>();

            foreach (ApsisCustomer cu in custs)
            {
                tmpList.Add(cu);
                
                if (tmpList.Count == 950)
                {
                    FlagCustsInLetterHelper(tmpList, flag);
                    tmpList = new List<ApsisCustomer>();
                }
            }

            if (tmpList.Count > 0)
                FlagCustsInLetterHelper(tmpList, flag);
        }

        private static void FlagCustsInLetterHelper(List<ApsisCustomer> custs, string flag)
        {
            OracleConnection conn = null;
            OracleCommand cmd = null;
            OracleDataReader rdr = null;

            try
            {
                if (custs.Count == 0)
                    return;

                StringBuilder sql = new StringBuilder();
                sql.Append("update CIRIX.EXPCUSTOMER_LETTER ");
                sql.Append("set UPDATED = '" + flag + "' ");
                sql.Append("where cusno in (");

                int i = -1;
                foreach (ApsisCustomer c in custs)
                {
                    i++;

                    if (i == 0)
                        sql.Append(c.CustomerId.ToString());
                    else
                        sql.Append("," + c.CustomerId.ToString());
                }

                sql.Append(")");


                conn = new OracleConnection(_connStrCirix);
                conn.Open();
                cmd = new OracleCommand(sql.ToString(), conn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                new Logger("CirixFlagCusts() failed", ex.Message);
                throw;
            }
            finally
            {
                KillOracleDbObjects(conn, cmd, rdr);
            }
        }


        /// <summary>
        /// flags:
        /// Y - updated 
        /// N - not updated
        /// </summary>
        public static void FlagCustsInExpCustomer(HashSet<int> cusnos, string flag)
        {
            //cirix can only handle 1000 items in an sql command, so we need to batch

            HashSet<int> tmpSet = new HashSet<int>();

            foreach (int i in cusnos)
            {
                tmpSet.Add(i);

                if (tmpSet.Count == 950)
                {
                    FlagCustsInExpCustomerHelper(tmpSet, flag);
                    tmpSet = new HashSet<int>();
                }
            }

            if (tmpSet.Count > 0)
                FlagCustsInExpCustomerHelper(tmpSet, flag);
        }

        private static void FlagCustsInExpCustomerHelper(HashSet<int> cusnos, string flag)
        {
            OracleConnection conn = null;
            OracleCommand cmd = null;
            OracleDataReader rdr = null;

            try
            {
                if (cusnos.Count == 0)
                    return;

                StringBuilder sql = new StringBuilder();
                sql.Append("update CIRIX.EXPCUSTOMER ");
                sql.Append("set UPDATED = '" + flag + "' ");
                sql.Append("where cusno in (");

                int i = -1;
                foreach (int cusno in cusnos)
                {
                    i++;

                    if (i == 0)
                        sql.Append(cusno.ToString());
                    else
                        sql.Append("," + cusno.ToString());
                }

                sql.Append(")");

                conn = new OracleConnection(_connStrCirix);
                conn.Open();
                cmd = new OracleCommand(sql.ToString(), conn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                new Logger("CirixFlagCusts() failed", ex.Message);
                throw;
            }
            finally
            {
                KillOracleDbObjects(conn, cmd, rdr);
            }
        }


        private static List<string> CirixGetFreeSubsCampIds()
        {
            List<string> campIds = new List<string>();

            OracleConnection conn = null;
            OracleCommand cmd = null;
            OracleDataReader rdr = null;

            try
            {
                string sql = "select codevalue from PARROW where CODEGROUPNO = 'WELCOME' and papercode = 'DI' and codeno = 'FREE_CAMPAIGNS'";
                conn = new OracleConnection(_connStrCirix);
                conn.Open();
                cmd = new OracleCommand(sql, conn);
                string s = cmd.ExecuteOracleScalar().ToString();
                string[] arr = s.Split(',');
                foreach (string id in arr)
                    campIds.Add(id.Trim());
            }
            catch (Exception ex) { new Logger("CirixGetFreeSubsCampIds() failed", ex.Message); }
            finally { KillOracleDbObjects(conn, cmd, rdr); }

            return campIds;
        }


        private static List<string> CirixGetCompanySubsCampIds()
        {
            List<string> campIds = new List<string>();

            OracleConnection conn = null;
            OracleCommand cmd = null;
            OracleDataReader rdr = null;

            try
            {
                string sql = "select codevalue from PARROW where CODEGROUPNO = 'RENEWING' and papercode = 'DI' and codeno = 'FORET_CAMPS'";
                conn = new OracleConnection(_connStrCirix);
                conn.Open();
                cmd = new OracleCommand(sql, conn);
                string s = cmd.ExecuteOracleScalar().ToString();
                string[] arr = s.Split(',');
                foreach (string id in arr)
                    campIds.Add(id.Trim());
            }
            catch (Exception ex) { new Logger("CirixGetCompanySubsCampIds() failed", ex.Message); }
            finally { KillOracleDbObjects(conn, cmd, rdr); }

            return campIds;
        }


        public static void UpdateEmailInCirix(int custId, string mail)
        {
            OracleConnection conn = null;
            OracleCommand cmd = null;
            OracleDataReader rdr = null;

            #region customer
            try
            {
                string sql = "update CIRIX.customer set EMAILADDRESS='" + mail + "' where cusno=" + custId;
                conn = new OracleConnection(_connStrCirix);
                conn.Open();
                cmd = new OracleCommand(sql, conn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                new Logger("CirixUpdateEmail() (CUSTOMER) failed for custId=" + custId.ToString(), ex.Message);
                throw;
            }
            finally
            {
                KillOracleDbObjects(conn, cmd, rdr);
            }
            #endregion

            #region letter
            try
            {
                string sql = "update CIRIX.EXPCUSTOMER_LETTER set EMAILADDRESS='" + mail + "' where cusno=" + custId;
                conn = new OracleConnection(_connStrCirix);
                conn.Open();
                cmd = new OracleCommand(sql, conn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                new Logger("CirixUpdateEmail() (LETTER) failed for custId=" + custId.ToString(), ex.Message);
                throw;
            }
            finally
            {
                KillOracleDbObjects(conn, cmd, rdr);
            }
            #endregion


            new CustomerPropertyHandler(custId, mail, null, null, null, null);
        }


        public static ApsisCustomer CirixGetCustomer(int customerId, int subsNo, bool isExtCustomer)
        {
            ApsisCustomer c = new ApsisCustomer();
            //DefinitionFile defFile = new DefinitionFile();                  //to decide value of IsExtCustomer
            List<string> freeSubsCampIds = CirixGetFreeSubsCampIds();
            //List<string> companySubsCampIds = CirixGetCompanySubsCampIds();
            MailSenderDbHandler ms = new MailSenderDbHandler();
            DataSet dsRules = ms.GetEmailRules();

            OracleConnection conn = null;
            OracleCommand cmd = null;
            OracleDataReader rdr = null;

            try
            {
                #region sql
                StringBuilder sql = new StringBuilder();
                sql.Append("select c.cusno, c.rowtext1, c.emailaddress, c.wwwuserid, c.passwd, s.papercode, ");
                sql.Append("s.productno, s.targetgroup, ca.campid, d.countrycode, c.offerden_email, ");
                sql.Append("s.subsstartdate, s.subsenddate, s.campno, s.receivetype ");
                sql.Append("from customer c join subs s on c.cusno = s.subscusno ");
                sql.Append("join doororder n on c.cusno = n.cusno ");
                sql.Append("join door d on n.doorno = d.doorno ");
                sql.Append("left join campaign ca on s.campno = ca.campno ");
                //sb.Append("where s.productno = '01' ");
                //sb.Append("and s.papercode in ('DI', 'DISE') ");
                sql.Append("where ");
                sql.Append("c.cusno='" + customerId.ToString() + "' ");
                sql.Append("and s.subsno='" + subsNo.ToString() + "' ");
                sql.Append("and s.extno in (select MAX(extno) from subs where subscusno = '" + customerId.ToString() + "')");

                //todo remove after testing
                //new Logger("sql CirixGetCustomer(): " + sql.ToString());

                #endregion

                conn = new OracleConnection(_connStrCirix);
                conn.Open();
                cmd = new OracleCommand(sql.ToString(), conn);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    SetCustomerProperties(c, rdr);
                    c.IsExtCustomer = isExtCustomer;
                    c.SetApsisProjectGuid(freeSubsCampIds);
                }
            }
            catch (Exception ex)
            {
                new Logger("CirixGetCustomer() failed", ex.Message);
            }
            finally
            {
                KillOracleDbObjects(conn, cmd, rdr);
            }

            return c;
        }

        public static void UpdateLetterInCirix(ApsisCustomer c, int subsno)
        {
            OracleConnection conn = null;
            OracleCommand cmd = null;
            OracleDataReader rdr = null;

            try
            {
                List<string> s = UpdateLetterInCirixHelper(c, subsno);
                string invMode = s[0];
                string subskind = s[1];
                string subsLenMons = s[2];

                #region SQL

                string subsStart = (c.InvStartDate > DateTime.MinValue) ? c.InvStartDate.ToString("yyyy-MM-dd") : "";
                string subsEnd = (c.SubsEndDate > DateTime.MinValue) ? c.SubsEndDate.ToString("yyyy-MM-dd") : "";

                StringBuilder sql = new StringBuilder();
                sql.Append("update CIRIX.expcustomer_letter set ");
                sql.Append("papercode='" + c.f_PaperCode + "', ");
                sql.Append("productno='" + c.f_ProductNo + "', ");
                sql.Append("campno='" + c.CampNo + "', ");
                sql.Append("targetgroup='" + c.TargetGroup + "', ");
                sql.Append("emailaddress='" + c.Email + "', ");
                sql.Append("receivetype='" + c.ReceiveType + "', ");
                sql.Append("SubsStartDate='" + subsStart + "', ");
                sql.Append("SubsEndDate='" + subsEnd + "', ");
                sql.Append("CampId='" + c.CampId + "', ");
                sql.Append("invMode='" + invMode + "', ");
                sql.Append("subskind='" + subskind + "', ");
                sql.Append("subslen_mons='" + subsLenMons + "', ");
                sql.Append("updated='N' ");
                sql.Append("where ");
                sql.Append("cusno='" + c.CustomerId.ToString() + "' and ");
                sql.Append("subsno='" + subsno.ToString() + "'");

                //todo remove after testing
                //new Logger("sql UpdateLetterInCirix(): " + sql.ToString());

                #endregion

                conn = new OracleConnection(_connStrCirix);
                conn.Open();
                cmd = new OracleCommand(sql.ToString(), conn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                new Logger("UpdateLetterInCirix() failed", ex.Message);
                //throw;
            }
            finally
            {
                KillOracleDbObjects(conn, cmd, rdr);
            }
        }

        private static List<string> UpdateLetterInCirixHelper(ApsisCustomer c, int subsno)
        {
            List<string> ret = new List<string>();
            OracleConnection conn = null;
            OracleCommand cmd = null;
            OracleDataReader rdr = null;

            try
            {
                //string sql = "select invmode, subskind, subslen_mons from subs where subsno='" + subsno + "'";  
                StringBuilder sql = new StringBuilder();
                sql.Append("select invmode, subskind, subslen_mons ");
                sql.Append("from subs where ");
                sql.Append("subsno='" + subsno.ToString() + "' and ");
                sql.Append("extno in (select MAX(extno) from subs where subscusno='" + c.CustomerId.ToString() + "')");

                conn = new OracleConnection(_connStrCirix);
                conn.Open();
                cmd = new OracleCommand(sql.ToString(), conn);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    ret.Add(rdr[0].ToString());
                    ret.Add(rdr[1].ToString());
                    ret.Add(rdr[2].ToString());

                    //todo remove after testing
                    //new Logger("UpdateLetterInCirixHelper() values - 0=" + rdr[0].ToString() + ", 1=" + rdr[1].ToString() + ", 2=" + rdr[2].ToString());
                }
            }
            catch (Exception ex)
            {
                new Logger("UpdateLetterInCirixHelper() failed", ex.Message);
            }
            finally
            {
                KillOracleDbObjects(conn, cmd, rdr);
            }

            return ret;
        }

        public static bool CustomerIsActive(int cusno, string paperCode, string productNo)
        {
            bool customerIsActive = false;
            using (var conn = new OracleConnection(_connStrCirix))
            {
                conn.Open();
                var sql = string.Format(@"SELECT COUNT(*) as antal
                            from subs 
                            where 
                            SUBSCUSNO = {0}
                            and subsstate in ('00', '01', '02')
                            and (CANCELREASON IS NULL or CANCELREASON = '') 
                            and PAPERCODE = '{1}'
                            and PRODUCTNO = '{2}'", cusno, paperCode, productNo);
                using (var cmd = new OracleCommand(sql, conn))
                {
                    int result;
                    int.TryParse(cmd.ExecuteOracleScalar().ToString(), out result);
                    customerIsActive = result > 0;
                }
            }
            return customerIsActive;
        }

        //TODO: Check and compare query with GetUpdatedCusnosInDateInterval() it they should use same query-logic maybe?
        public static List<ApsisCustomer> GetUpdatedCustomers()
        {
            const string cacheKey = "CirixDbHandler_GetUpdatedCustomers";

            // If list is cached, return it
            var data = HttpRuntime.Cache.Get(cacheKey);
            if (data != null)
            {
                return (List<ApsisCustomer>)data;
            }

            var customers = new List<ApsisCustomer>();
            using (var conn = new OracleConnection(_connStrCirix))
            {
                conn.Open();
                //SQL that retrieves ALL today's edited customers and its latest subscription:
                const string sql = @"select 
                            customer.cusno,
                            --customer.rowtext1,
                            customer.emailaddress,
                            --customer.passwd,
                            subs.papercode,
                            subs.productno,
                            --subs.targetgroup,
                            --subs.countrycode,
                            customer.OFFERDEN_EMAIL,
                            --subs.subsstartdate, 
                            --subs.subsenddate, 
                            --subs.campno, 
                            --subs.receivetype, 
                            subs.subslen_mons
                            from customer
                            INNER JOIN subs ON customer.cusno = subs.subscusno
                            WHERE
                            subs.subsno 
                            IN(
                            select MAX(subsno) from subs 
                            where ROWNUM = 1 
                            AND subscusno = customer.cusno 
                            and subsstate in ('00', '01', '02')
                            and (CANCELREASON IS NULL or CANCELREASON = '')  
                            )
                            AND
                            customer.cusno IN(
                              select distinct cusno from expcustomer where trunc(stamp_date) = trunc(sysdate)
                            )
                            order by subs.stamp_date desc";

                using (var cmd = new OracleCommand(sql, conn))
                {
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        var tmpCustomer = new ApsisCustomer();
                        if (int.TryParse(reader["cusno"].ToString(), out tmpCustomer.CustomerId))
                        {
                            tmpCustomer.Email = reader["emailaddress"].ToString();
                            tmpCustomer.f_PaperCode = reader["papercode"].ToString();
                            tmpCustomer.f_ProductNo = reader["productno"].ToString();
                            tmpCustomer.f_OfferdenEmail = reader["OFFERDEN_EMAIL"].ToString();
                            if (int.TryParse(reader["subslen_mons"].ToString(), out tmpCustomer.SubsLenMonsFromCirix))
                            {
                                customers.Add(tmpCustomer);
                            }
                        }
                    }
                }
            }
            // Cache this
            HttpRuntime.Cache.Insert(
                cacheKey,
                customers,
                null,
                DateTime.Now.AddSeconds(Settings.CacheTimeSecondsMedium),
                Cache.NoSlidingExpiration);
            return customers;
        } 

        public static bool EmailIsUnique(string email)
        {
            int numAdr = 0;
            OracleConnection conn = null;
            OracleCommand cmd = null;
            OracleDataReader rdr = null;

            try
            {
                string sql = "SELECT count(*) from customer c join subs s on (c.cusno = s.subscusno) where s.subsstate in ('00', '01', '02') and c.EMAILADDRESS = '" + email + "'";
                conn = new OracleConnection(_connStrCirix);
                conn.Open();
                cmd = new OracleCommand(sql, conn);
                int.TryParse(cmd.ExecuteOracleScalar().ToString(), out numAdr);
            }
            catch (Exception ex) { new Logger("CirixEmailIsUnique() failed", ex.Message); }
            finally { KillOracleDbObjects(conn, cmd, rdr); }

            if (numAdr == 1)
                return true;

            return false;
        }

        public static List<long> GetCusnosByEmail(string email)
        {
            List<long> cusnos = new List<long>();
            OracleConnection conn = null;
            OracleCommand cmd = null;
            OracleDataReader rdr = null;
            
            try
            {
                string sql = "SELECT distinct cusno from customer c join subs s on (c.cusno = s.subscusno) where s.subsstate in ('00', '01', '02') and c.EMAILADDRESS = '" + email + "'";
                conn = new OracleConnection(_connStrCirix);
                conn.Open();
                cmd = new OracleCommand(sql, conn);
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                    cusnos.Add(long.Parse(rdr[0].ToString()));
            }
            catch (Exception ex) { new Logger("GetCusnosByEmail() failed for email: " + email, ex.Message); }
            finally { KillOracleDbObjects(conn, cmd, rdr); }

            return cusnos;
        }


        //public static HashSet<int> GetChangedCusnosFromExpCustomer(int highId, int lowId)
        public static HashSet<int> GetChangedCusnosFromExpCustomer()
        {
            HashSet<int> cusnos = new HashSet<int>();
            OracleConnection conn = null;
            OracleCommand cmd = null;
            OracleDataReader rdr = null;

            try 
            {
                //sql.Append("and cusno <= " + highId.ToString() + " and cusno >= "+ lowId.ToString() +" ");
                //string sql = "select distinct CUSNO from CIRIX.EXPCUSTOMER WHERE UPDATED='N'";
                //string sql = "select distinct CUSNO from CIRIX.EXPCUSTOMER WHERE nvl(updated, 'N') <> 'Y'";
                //sql.Append("nvl(updated, 'N') <> 'Y' ");

                StringBuilder sql = new StringBuilder();
                sql.Append("select distinct cusno from expcustomer ");
                sql.Append("where ");
                sql.Append("nvl(updated, 'N') not in ('P', 'Y', 'D') ");
                sql.Append("order by cusno desc");

                conn = new OracleConnection(_connStrCirix);
                conn.Open();
                cmd = new OracleCommand(sql.ToString(), conn);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                    cusnos.Add(int.Parse(rdr[0].ToString()));
            }
            catch (Exception ex)
            {
                new Logger("GetChangedCusnosFromExpCustomer() failed", ex.Message);
            }
            finally
            {
                KillOracleDbObjects(conn, cmd, rdr);
            }

            return cusnos;
        }


        /// <summary>
        /// get xtra fields: ret[0]=xtra01, ret[1]=xtra02, ret[2]=xtra03
        /// </summary>
        public static List<string> GetCustomerXtraFields(long cusno)
        {
            List<string> ret = new List<string>();
            ret.Add("");
            ret.Add("");
            ret.Add("");

            OracleConnection conn = null;
            OracleCommand cmd = null;
            OracleDataReader rdr = null;

            try
            {
                string sql = "select xtra01, xtra02, xtra03 from customer where cusno=" + cusno.ToString();

                conn = new OracleConnection(_connStrCirix);
                conn.Open();
                cmd = new OracleCommand(sql, conn);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    ret[0] = rdr[0].ToString();
                    ret[1] = rdr[1].ToString();
                    ret[2] = rdr[2].ToString();
                }

                return ret;
            }
            catch (Exception ex)
            {
                new Logger("GetCustomerXtraFields() failed", ex.Message);
                ret[0] = "";
                ret[1] = "";
                ret[2] = "";
                return ret;
            }
            finally
            {
                KillOracleDbObjects(conn, cmd, rdr);
            }
        }


        public static List<DateTime> GetProductsIssueDatesInInterval(string paperCode, string productno, DateTime dateMin, DateTime dateMax)
        {
            List<DateTime> ret = new List<DateTime>();
            OracleConnection conn = null;
            OracleCommand cmd = null;
            OracleDataReader rdr = null;

            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("select issuedate from PUBLDAYS where ");
                sql.Append("deliveryno='" + paperCode + "' and ");
                sql.Append("productno='" + productno + "' and ");
                sql.Append("issuedate>=to_date('" + dateMin.ToShortDateString() + "','yyyy-mm-dd') and ");
                sql.Append("issuedate<=to_date('" + dateMax.ToShortDateString() + "','yyyy-mm-dd') ");
                sql.Append("order by issuedate desc");

                conn = new OracleConnection(_connStrCirix);
                conn.Open();
                cmd = new OracleCommand(sql.ToString(), conn);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                    ret.Add(SetDateFromString(rdr[0].ToString()));
            
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("paperCode:" + paperCode + ", ");
                sb.Append("productno:" + productno + ", ");
                sb.Append("dateMin:" + dateMin.ToShortDateString() + ", ");
                sb.Append("dateMax:" + dateMax.ToShortDateString());
                new Logger("GetProductsIssueDatesInInterval() failed for args: " + sb.ToString(), ex.Message);
            }
            finally
            {
                KillOracleDbObjects(conn, cmd, rdr);
            }

            return ret;
        }


        public static DateTime GetIssueDate(string paperCode, string productno, DateTime date, EnumIssue.Issue issue)
        {
            DateTime returnDate;
            var cacheKey = string.Format("CirixDbHandler_GetIssueDate_{0}_{1}_{2}_{3}", paperCode, productno, date, issue);

            // If SQL-dateresult is cached, return it
            var cachedObject = HttpRuntime.Cache.Get(cacheKey);
            if (cachedObject != null)
            {
                return (DateTime)cachedObject;
            }

            try
            {
                var sql = new StringBuilder();
                sql.Append("select ");

                if (issue == EnumIssue.Issue.FirstBeforeInDate || issue == EnumIssue.Issue.InDateOrFirstBeforeInDate)
                    sql.Append("MAX");

                if (issue == EnumIssue.Issue.FirstAfterInDate || issue == EnumIssue.Issue.InDateOrFirstAfterInDate)
                    sql.Append("MIN");

                sql.Append("(issuedate) from PUBLDAYS ");
                sql.Append("where deliveryno='" + paperCode + "' and PRODUCTNO='" + productno + "' and ");
                sql.Append("ISSUEDATE ");

                if (issue == EnumIssue.Issue.FirstBeforeInDate)
                    sql.Append("<");

                if (issue == EnumIssue.Issue.InDateOrFirstBeforeInDate)
                    sql.Append("<=");

                if (issue == EnumIssue.Issue.FirstAfterInDate)
                    sql.Append(">");

                if (issue == EnumIssue.Issue.InDateOrFirstAfterInDate)
                    sql.Append(">=");

                //sql.Append(" to_date('" + date.ToShortDateString() + "','yyyy-mm-dd')");
                sql.Append(" to_date('" + date.ToString("yyyy-MM-dd") + "','yyyy-mm-dd')");

                using (var conn = new OracleConnection(_connStrCirix))
                {
                    conn.Open();
                    using (var cmd = new OracleCommand(sql.ToString(), conn))
                    {
                        var issueDateString = cmd.ExecuteOracleScalar().ToString();
                        returnDate = SetDateFromString(issueDateString);

                        // PUBLDAYS in Cirix rarely get changed/updated, so we can cache this result long time
                        HttpRuntime.Cache.Insert(
                            cacheKey,
                            returnDate,
                            null,
                            DateTime.Now.AddDays(1),
                            Cache.NoSlidingExpiration);
                    }
                }
            }
            catch (Exception ex)
            {
                var sb = new StringBuilder();
                sb.Append("paperCode: " + paperCode + ", ");
                sb.Append("productno: " + productno + ", ");
                sb.Append("date.ToShortDateString(): " + date.ToShortDateString() + ", ");
                sb.Append("date.ToString('yyyy-MM-dd'): " + date.ToString("yyyy-MM-dd") + ", ");
                sb.Append("issue: " + issue);
                new Logger("GetIssueDate() failed " + sb, ex.Message);
                returnDate = DateTime.MinValue;
            }
            return returnDate;
        }

        /// <summary>
        /// Cannot start subs on same day as signed, so days are added to wantedDate according to Di rules.
        /// </summary>
        public static DateTime GetNextIssueDateIncDiRules(DateTime wantedDate, string paperCode, string productNo)
        {
            wantedDate = wantedDate.Date;

            //unknown product
            if (string.IsNullOrEmpty(paperCode) || string.IsNullOrEmpty(productNo))
                return DateTime.MinValue.Date;

            //get minDate for subs start by di rules
            DateTime minDate = GetClosesIssueDateInTheory(paperCode, productNo);

            //minDate 'earlier' then wantedDate
            if (minDate < wantedDate)
                minDate = wantedDate;

            return CirixDbHandler.GetIssueDate(paperCode, productNo, minDate, DbHandlers.EnumIssue.Issue.InDateOrFirstAfterInDate);
        }

        /// <summary>
        /// closest issue date in theory - cirix needs a few days to prepare addresses
        /// </summary>
        public static DateTime GetClosesIssueDateInTheory(string paperCode, string productNo)
        {
            DateTime dt = DateTime.Now.Date;

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
            else
            {
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
        }


        public static DateTime GetNextIssuedate(string papercode, string productno, DateTime minDate)
        {
            try
            {
                return Ws.GetNextIssuedate_(papercode, productno, minDate);
            }
            catch (Exception ex)
            {
                new Logger("GetNextIssuedate() - failed for papercode:" + papercode + ", productno:" + productno + ", minDate:" + minDate.ToString(), ex.ToString());
            }

            return DateTime.MinValue;
        }


        
        public static string CreateRenewal_DI(string sUserID, long lSubsno, int iExtno, long lPricelistno, long lCampno, int iSubslenMons, int iSubslenDays, DateTime dateSubsStartdate,
                                              DateTime dateSubsEnddate, string sSubskind, double dblTotalPrice, double dblItemPrice, int iItemqty, string sSalesno, long lPaycusno, string sProductno,
                                              string sReceiveType, string sTargetGroup, string sPriceAtStart, string sOtherSubsno, string sOrderID, string sAutogiro, string sPriceGroup, string sInvMode)
        {
            try
            {
                //dblGrossPrice, double dblDiscAmount not used in prod?!
                return Ws.CreateRenewal_DI_(sUserID, lSubsno, iExtno, lPricelistno, lCampno, iSubslenMons, iSubslenDays, dateSubsStartdate,
                                        dateSubsEnddate, sSubskind, dblTotalPrice, dblItemPrice, iItemqty, sSalesno, lPaycusno, sProductno,
                                        sReceiveType, sTargetGroup, sPriceAtStart, dblTotalPrice, Settings.dblDiscAmount, sOtherSubsno, sOrderID, sAutogiro, sPriceGroup, sInvMode);
            }
            catch (Exception ex)
            {
                new Logger("CreateRenewal_DI() - failed", ex.ToString());
            }

            return string.Empty;
        }



        public static string DefinitiveAddressChange(long cusno, string street, string houseNo, string stairCase, string apartment, string careOf, string zip, DateTime startDate)
        {
            StringBuilder sb = new StringBuilder();

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

                string s = Ws.DefinitiveAddressChange_(cusno, street, houseNo, stairCase, apartment, careOf, "", "SE", zip, startDate);

                if (s != "OK")
                    new Logger("DefinitiveAddressChange() failed, returned FALSE", sb.ToString() + "<hr>" + s);

                return s;
            }
            catch (Exception ex)
            {
                new Logger("DefinitiveAddressChange() failed, exception", sb.ToString() + "<hr>" + ex.ToString());
            }

            return string.Empty;
        }



        public static string DefinitiveAddressChangeRemove(long cusno, DateTime startDate)
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                sb.Append("cusno:" + cusno + ", ");
                sb.Append("startDate:" + startDate.ToShortDateString());

                string s = Ws.DefinitiveAddressChangeRemove_(cusno, startDate);

                if (s != "OK")
                    new Logger("DefinitiveAddressChangeRemove() failed, returned FALSE", sb.ToString() + "<hr>" + s);

                return s;
            }
            catch (Exception ex)
            {
                new Logger("DefinitiveAddressChangeRemove() failed, exception", sb.ToString() + "<hr>" + ex.ToString());
            }

            return string.Empty;
        }



        public static void CirixInsertInFreeSubs(long subsno)
        {
            OracleConnection conn = null;
            OracleCommand cmd = null;
            OracleDataReader rdr = null;

            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("INSERT INTO FREESUBS ");
                sql.Append("(STAMP_USER,STAMP_DATE,STAMP_PROGRAM,SUBSNO,EXTNO,FREESUBSGRP) ");
                sql.Append("VALUES ");
                sql.Append("('SQL','" + DateTime.Now.ToShortDateString() + "','SUBSIX 4.5.29'," + subsno + ",0,'71')");

                #region dbNull (not working)
                //sql.Append("(STAMP_USER,STAMP_DATE,STAMP_PROGRAM,SUBSNO,EXTNO,GRANTERNO,FREESUBSGRP,DEPTNO,UNITNO,GRANTARG) ");
                //sql.Append("VALUES ");
                //sql.Append("('SQL','" + DateTime.Now + "','SUBSIX 4.5.29'," + subsno + ",0," + DBNull.Value + ",'71'," + DBNull.Value + "," + DBNull.Value + "," + DBNull.Value + ")");

                //TO_DATE('26.10.12 12:31:50', 'DD.MM.YY HH24:MI:SS')
                #endregion

                conn = new OracleConnection(_connStrCirix);
                conn.Open();
                cmd = new OracleCommand(sql.ToString(), conn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                new Logger("CirixInsertInFreeSubs() failed", ex.Message);
                throw;
            }
            finally
            {
                KillOracleDbObjects(conn, cmd, rdr);
            }
        }


        public static List<StringPair> GetNumSubsForPayingCust(string customerRowText1SearchStr)
        {
            List<StringPair> ret = new List<StringPair>();
            StringBuilder sql = new StringBuilder();
            OracleConnection conn = null;
            OracleCommand cmd = null;
            OracleDataReader rdr = null;

            try
            {
                sql.Append("select count(A.Rowtext1), A.Rowtext1 ");
                sql.Append("FROM CIRIX.CUSTOMER A ");
                sql.Append("LEFT JOIN CIRIX.SUBS B ON B.SUBSCUSNO = A.CUSNO ");
                sql.Append("WHERE ");
                sql.Append("B.SUBSSTATE IN ('00','01','02') AND ");
                sql.Append("A.ROWTEXT1 LIKE ");
                sql.Append("'" + customerRowText1SearchStr.ToUpper() + "' ");
                sql.Append("group by A.Rowtext1 ");
                sql.Append("order by count(A.Rowtext1) desc");

                //todo: change to this SQL. Returns 3 params, not 2
                //sql.Append("select b.rowtext1 as betalare, ");
                //sql.Append("c.rowtext1 as mottagare, ");
                //sql.Append("count(a.subsno) as antal ");
                //sql.Append("from subs a ");
                //sql.Append("left join customer b ");
                //sql.Append("on b.cusno = a.paycusno ");
                //sql.Append("left join customer c ");
                //sql.Append("on c.cusno = a.subscusno ");
                //sql.Append("where subsstate in ('00','01','02') and ");
                //sql.Append("(b.rowtext1 like ");
                //sql.Append("'" + customerRowText1SearchStr.ToUpper() + "' ");
                //sql.Append("or c.rowtext1 like ");
                //sql.Append("'" + customerRowText1SearchStr.ToUpper() + "'");
                //sql.Append(") ");
                //sql.Append("group by b.rowtext1, c.rowtext1 ");
                //sql.Append("order by count(a.subsno) desc");

                conn = new OracleConnection(_connStrCirix);
                conn.Open();
                cmd = new OracleCommand(sql.ToString(), conn);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    ret.Add(new StringPair(rdr[0].ToString(), rdr[1].ToString()));
                }

                return ret;
            }
            catch (Exception ex)
            {
                new Logger("GetNumSubsForPayingCust() failed for indata:" + customerRowText1SearchStr, "sql: " + sql.ToString() + Environment.NewLine + ex.ToString());
            }
            finally
            {
                KillOracleDbObjects(conn, cmd, rdr);
            }

            return ret;
        }




        public static int GetNumSubsForPayingCustOnline(string customerRowText1SearchStr)
        {
            int ret = 0;
            StringBuilder sql = new StringBuilder();  
            OracleConnection conn = null;
            OracleCommand cmd = null;
            OracleDataReader rdr = null;

            try
            {
                sql.Append("SELECT DISTINCT COUNT(*) ");
                sql.Append("FROM CIRIX.CUSTOMER A ");
                sql.Append("LEFT JOIN CIRIX.SUBS B ");
                sql.Append("ON B.SUBSCUSNO = A.CUSNO ");
                sql.Append("WHERE B.CRITER6 = 'K' AND B.SUBSSTATE IN ('00','01','02') AND A.ROWTEXT1 LIKE ");
                sql.Append("'" + customerRowText1SearchStr.ToUpper() + "'");

                conn = new OracleConnection(_connStrCirix);
                conn.Open();
                cmd = new OracleCommand(sql.ToString(), conn);
                int.TryParse(cmd.ExecuteOracleScalar().ToString(), out ret);
            }
            catch (Exception ex) { new Logger("GetNumSubsForPayingCustOnline() failed for indata: " + customerRowText1SearchStr, "sql: " + sql.ToString() + Environment.NewLine + ex.Message); }
            finally { KillOracleDbObjects(conn, cmd, rdr); }

            return ret;
        }


        private static void KillOracleDbObjects(OracleConnection conn, OracleCommand cmd, OracleDataReader rdr)
        {
            if (conn != null)
            {
                conn.Close();
                conn.Dispose();
                conn = null;
            }

            if (cmd != null)
            {
                cmd.Dispose();
                cmd = null;
            }

            if (rdr != null)
                rdr = null;
        }

        #region summer deal specific (delete when summerdeal is over)

        /// <summary>
        /// Create a new subscription. Create new subscriber and subscriptionPayer only if createNewSubscribers = true. 
        /// Otherwise, take cusno from subscription
        /// </summary>
        /// <param name="subscription"></param>
        /// <returns></returns>
        //public static string TryInsertSubscriptionTmp(Subscription subscription, string targetGroup)
        //{
        //    string metName = "TryInsertSubscription()";
        //    string stdErr = MiscFunctions.GetDefaultErrMess("");

        //    subscription.InvoiceMode = Settings.InvoiceMode_BankGiro;


        //    long cusNoSub = 0;
        //    long cusNoPay = 0;

        //    try
        //    {
        //        //If no subscriber cusno was set, then new customer has to be created in Cirix / existing customer cusno has to be retrieved from Cirix
        //        if (subscription.Subscriber.Cusno <= 0)
        //        {
        //            DataSet dsFoundSubscriberCustomer;
        //            DataSet dsFoundPayerCustomer;

        //            //If customer already existed in Cirix with that data, and has active subscriptions already then user cannot start a subscription
        //            string canAddSubsMessage = HasActiveSubs(subscription, out dsFoundSubscriberCustomer, out dsFoundPayerCustomer);
        //            if (!string.IsNullOrEmpty(canAddSubsMessage))
        //                return canAddSubsMessage;

        //            //If subscriber did not exist in Cirix or didn't have active subscriptions
        //            if (dsFoundSubscriberCustomer == null || dsFoundSubscriberCustomer.Tables[0].Rows.Count == 0)
        //            {
        //                cusNoSub = TryAddCustomer(subscription, true);
        //            }
        //            else
        //            {
        //                cusNoSub = Convert.ToInt64(dsFoundSubscriberCustomer.Tables[0].Rows[0]["CUSNO"]);
        //                UpdateCustomerInformationIfNeeded(subscription.Subscriber, dsFoundSubscriberCustomer);
        //            }

        //            subscription.Subscriber.Cusno = cusNoSub;

        //            //payer does not always exist
        //            if (subscription.SubscriptionPayer != null)
        //            {
        //                if (dsFoundPayerCustomer == null || dsFoundPayerCustomer.Tables[0].Rows.Count == 0)
        //                {
        //                    cusNoPay = TryAddCustomer(subscription, false);
        //                }
        //                else
        //                {
        //                    cusNoPay = Convert.ToInt64(dsFoundPayerCustomer.Tables[0].Rows[0]["CUSNO"]);
        //                    UpdateCustomerInformationIfNeeded(subscription.SubscriptionPayer, dsFoundPayerCustomer);
        //                }

        //                //subscription.SubscriptionPayerCusNo = cusNoPay;
        //                subscription.SubscriptionPayer.Cusno = cusNoPay;
        //            }

        //            //Delete all the propValues before adding new ones. If customer has had subscription before, he/she has provided newer data and the old one should be removed.
        //            CustomerPropertyHandler.DeleteCusProps(cusNoSub);

        //            //need to save propValues before AddNewSubs
        //            CustomerPropertyHandler.HandleCusProps(cusNoSub, subscription.Subscriber.Email, subscription.Subscriber.MobilePhone, MiscFunctions.FormatCustPropBirthNo(subscription.Subscriber.SocialSecurityNo), null);

        //            //if (subscription.SubscriptionPayer != null)
        //            //    HandlePropertyValue(cusNoPay, subscription.SubscriptionPayer.Email, subscription.SubscriptionPayer.MobilePhone, Functions.FormatFinnishBirthNo(subscription.SubscriptionPayer.SocialSecurityNo));
        //        }
        //        else
        //        {
        //            cusNoSub = subscription.Subscriber.Cusno;
        //            //cusNoPay = subscription.SubscriptionPayerCusNo > 0 ? subscription.SubscriptionPayerCusNo : 0;
        //            cusNoPay = subscription.SubscriptionPayer != null ? subscription.SubscriptionPayer.Cusno : 0;
        //        }

        //        //all good - add new subscription
        //        string subcriptionNo = AddNewSubsTmp(subscription, cusNoSub, cusNoPay, targetGroup);
        //        if (subcriptionNo.StartsWith("FAILED"))
        //        {
        //            CustomerPropertyHandler.DeleteCusProps(cusNoSub);
        //            new Logger(metName + "/AddNewSubsTmp() failed", "Details: " + subscription.ToString());
        //            return stdErr;
        //        }
        //        else
        //        {
        //            subscription.SubsNo = Convert.ToInt64(subcriptionNo);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        new Logger(metName + " failed", ex.ToString());
        //        return stdErr;
        //    }

        //    return string.Empty;
        //}

        //private static string AddNewSubsTmp(Subscription subscription, long cusNoSub, long cusNoPay, string targetGroup)
        //{
        //    string ret = "FAILED";

        //    try
        //    {
        //        //string communeCode = GetCommuneCode(subscription.Subscriber.ZipCode);
        //        //long priceListNo = GetPriceListNo(subscription.Papercode, subscription.ProductNo, subscription.SubsStartDate, communeCode, subscription.Pricegroup, subscription.CampaignNo.ToString());

        //        long campNo = 1168;
        //        if (MiscFunctions.GetAppsettingsValue("UseCirixTestWS") == "true")
        //            campNo = 1112;

        //        double price = 188;

        //        DateTime dtStart = subscription.GetWeekendSubsDate(DateTime.Now); //subscription.SubsStartDate;
        //        DateTime dtEnd = new DateTime(2011, 8, 27);

        //        subscription.SubsEndDate = dtEnd;
        //        subscription.ProductNo = "05";
        //        subscription.PaperCode = "DI";


        //        int mons = int.Parse(DateAndTime.DateDiff(DateInterval.Month, dtStart, dtEnd).ToString());
        //        if (dtStart.Day > dtEnd.Day)
        //            mons--;

        //        int days = int.Parse(DateAndTime.DateDiff(DateInterval.Day, dtStart.AddMonths(mons), dtEnd).ToString());


        //        ret = CreateNewSubs_DI("WEBCIRIX", cusNoSub, cusNoPay, "02", "DI", "00", "05", "01", "N", 0, campNo,
        //                               mons, days, dtStart, dtEnd, price, price, price, 1, string.Empty,
        //                               targetGroup, "04", 0, string.Empty, string.Empty, string.Empty);
        //    }
        //    catch (Exception ex)
        //    {
        //        new Logger("AddNewSubsTmp() - failed", ex.ToString());
        //    }

        //    return ret;
        //}

        public static bool AddSubsIpadSummer(long cusno, string sUserId)
        {
            string ret = "FAILED";

            try
            {
                long campNo = 1169;
                if (MiscFunctions.GetAppsettingsValue("UseCirixTestWS") == "true")
                    campNo = 1113;

                double price = 0;

                DateTime dtStart = DateTime.Now;
                DateTime dtEnd = new DateTime(2011, 8, 31);

                int mons = int.Parse(DateAndTime.DateDiff(DateInterval.Month, dtStart, dtEnd).ToString());
                if (dtStart.Day > dtEnd.Day)
                    mons--;

                int days = int.Parse(DateAndTime.DateDiff(DateInterval.Day, dtStart.AddMonths(mons), dtEnd).ToString());

                //subsKind=02(tidsbestämd) - receiveType=04(säljkanal internet)
                ret = CreateNewSubs_DI(sUserId, cusno, 0, "02", "IPAD", "00", "01", "01", "N", 0, campNo,
                                       mons, days, dtStart.Date, dtEnd.Date, price, price, price, 1, string.Empty,
                                       "GULDPLUS", "04", 0, string.Empty, string.Empty, string.Empty, string.Empty);
            }
            catch (Exception ex)
            {
                new Logger("AddSubsIpadSummer() - failed", ex.ToString());
            }

            if (ret.StartsWith("FAILED"))
                return false;

            return true;
        }

        #endregion

        #region add subs to existing cust
        
        public static List<Person> FindCustomerByPerson(Person searchCriterias, bool includeEmailAsSearchCriteria)
        {
            List<Person> pers = new List<Person>();
            List<long> tmpCn = new List<long>();            //avoid multiple identical cusnos in return list
            DataSet ds = FindCustomers2(searchCriterias, includeEmailAsSearchCriteria);
            if (DbHelpMethods.DataSetHasRows(ds))
            {
                foreach (DataTable dt in ds.Tables)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        long cn = long.Parse(dr["CUSNO"].ToString());
                        
                        if (!tmpCn.Contains(cn))
                        {
                            tmpCn.Add(cn);
                            Person p = new Person() {Cusno = cn};
                            #region not used props
                            //p.PhoneMobile = dr["O_PHONE"] as string;
                            //p.Email = dr["EMAILADDRESS"] as string;

                            //DataSet ds2 = GetCustomer(p.Cusno);
                            //if(ds2 != null && ds2.Tables != null && ds2.Tables[0].Rows != null)
                            //    p.SocialSecurityNo = ds2.Tables[0].Rows[0]["SOCIALSECNO"] as string;


                            //p.CusState = dr["CUSSTATE"] as string;
                            //p.CirixName1 = dr["ROWTEXT1"] as string;
                            //p.CirixName2 = dr["ROWTEXT2"] as string;
                            //ROWTEXT3
                            //STREET1
                            //p.HouseNo = dr["HOUSENO"] as string;
                            //p.CirixStreet2 = dr["STREET2"] as string;
                            //STREET3
                            //COUNTRYCODE
                            //p.ZipCode = dr["ZIPCODE"] as string;
                            //POSTNAME
                            //H_PHONE
                            //p.PhoneDayTime = dr["W_PHONE"] as string;
                            #endregion
                            pers.Add(p);
                        }
                    }
                }
            }

            return pers;
        }

        private static DataSet FindCustomers2(Person person, bool includeEmailAsSearchCriteria)
        {
            if (person == null)
                return null;

            //todo: find out how sensitive search is...
            //string sPhone = "";  //!string.IsNullOrEmpty(person.PhoneMobile) ? person.PhoneMobile : "";  //: person.PhoneDayTime;
            //string sPhone = string.IsNullOrEmpty(person.MobilePhone) ? "" : person.MobilePhone;

            string email = "";
            if (includeEmailAsSearchCriteria && !string.IsNullOrEmpty(person.Email))
                email = person.Email.Trim(' ');

            return Ws.FindCustomers_(person.Cusno,                                         //lCusNo
                                    0,                                          //lInvNo
                                    0,                                          //lSubsNo
                                    person.CirixName1.Trim(' ').ToUpper(),      //sname1
                                    person.CirixName2.Trim(' ').ToUpper(),      //sname2
                                    "",                                         //sname3
                                    "", //sPhone.Trim(' '),                     //phone
                                    email,                                      //email
                                    person.StreetName.Trim(' ').ToUpper(),      //sStreet
                                    person.HouseNo.Trim(' '),                   //sHouseNo
                                    "",                                         //sStairCase
                                    "",                                         //sApartment
                                    "",                                         //sCountry
                                    person.ZipCode.Trim(' '),                   //sZipCode
                                    "",                                 //sUserId       WEBCIRIX or "" ?
                                    person.City.Trim(' ').ToUpper()             //sPostName
                                    );
        }

        /// <summary>
        /// Adds customer (subscriber or payer) to cirix
        /// </summary>
        /// <returns>
        /// >0=new customers cusno, -10=exception, -2=webservice transaction failed, -1=identical user(s) found i cirix
        /// </returns>
        public static long TryAddCustomer2(Subscription sub, bool isSubscriber, string sUserId)
        {
            string methodName = "TryAddCustomer2()";
            long cusno = -10;

            try
            {
                Person p = isSubscriber ? sub.Subscriber : sub.SubscriptionPayer;
                string pnum = (!string.IsNullOrEmpty(p.SocialSecurityNo)) ? p.SocialSecurityNo : string.Empty;
                string sCusType = string.IsNullOrEmpty(p.CirixName2) ? Settings.sCusType : Settings.sCusTypeCorp;
                if (isSubscriber && p.PhysicalAddressMissing)
                {
                    p.CirixName2 = p.Email;   //make non-address-customer unique to avoid cirix search hits on "wrong" customer
                    p.ZipCode = "10000";      //cirix needs a zipcode to work
                }
                cusno = CreateNewCustomer(sUserId, p.CirixName1, p.CirixName2, string.Empty, p.FirstName, p.LastName,
                                            p.StreetName, p.HouseNo, p.StairCase, p.Stairs, p.CirixStreet2, string.Empty, Settings.Country, p.ZipCode,
                                            string.Empty, p.PhoneDayTime, p.MobilePhone, p.Email, "N", "N", "N", "N", "N", Settings.Nets_CurrencyCode, sub.InvoiceMode,
                                            string.Empty, string.Empty, Settings.sCollectInv, sCusType, string.Empty, string.Empty, Settings.sNotes, Settings.sExpDay,
                                            sub.DiscPercent, Settings.sTerms, pnum, string.Empty, string.Empty, string.Empty,
                                            Settings.sCategory, Settings.lMasterCusno, "Y", "", p.CompanyNo);

                if (cusno == -1)
                    new Logger(methodName + " - cusNo=-1 - multiple identical users in db", "");

                if (cusno == -2)
                    new Logger(methodName + " - cusNo=-2 - webservice transaction failed", "");

                
                return cusno;
            }
            catch (Exception ex)
            {
                new Logger(methodName + " - failed", ex.ToString());
                return cusno;
            }
        }

        public static string AddNewSubs2(Subscription sub, long cusNoPay, DiPlusSubscription plusSub, string sUserId)
        {
            string ret = "FAILED";

            try
            {
                string communeCode = GetCommuneCode(sub.Subscriber.ZipCode);
                long priceListNo = GetPriceListNo(sub.PaperCode, sub.ProductNo, sub.SubsStartDate, communeCode, sub.Pricegroup, sub.CampNo.ToString());
                long cusNoSub = sub.Subscriber.Cusno;
                
                string priceGroup = sub.Pricegroup;
                if (plusSub != null && plusSub.SubsType != DiPlusSubscriptionType.PlusSubsType.StandAlonePlusSubs)
                    priceGroup = MiscFunctions.GetAppsettingsValue("awdPriceGroupUpg");

                ret = CreateNewSubs_DI(sUserId, cusNoSub, cusNoPay, sub.SubsKind, sub.PaperCode, priceGroup,
                                        sub.ProductNo, sub.Substype, sub.DirectDebit, priceListNo, sub.CampNo,
                                        sub.SubsLenMons, sub.SubsLenDays, sub.SubsStartDate, sub.SubsEndDate, sub.TotalPriceExVat,
                                        sub.TotalPriceExVat, sub.ItemPrice, sub.ItemQty, Settings.sSalesNo, sub.TargetGroup,
                                        Settings.sReceiveType, Settings.dblDiscAmount, string.Empty, string.Empty, string.Empty, sub.InvoiceMode);

                if (MiscFunctions.IsNumeric(ret))
                {
                    sub.SubsNo = long.Parse(ret);
                    TryAddCoProdIncl(sub, cusNoSub, cusNoPay, sub.SubsNo);
                    TryAddSubToBonDig(sub, plusSub);
                }
            }
            catch (Exception ex)
            {
                new Logger("AddNewSubs2() - failed", ex.ToString());
            }

            return ret;
        }

        

        /// <summary>
        /// will add product (knife etc) if camp includes this
        /// </summary>
        private static void TryAddCoProdIncl(Subscription sub, long cusNoSub, long cusNoPay, long subsNo)
        {
            if (sub.CoProdIncl == "Y")
            {
                try
                {
                    DataSet ds = Ws.GetCampaignCoProducts_(sub.CampId);
                    if (ds != null && ds.Tables != null && ds.Tables[0].Rows != null && ds.Tables[0].Rows[0] != null)
                    {
                        Ws.CreateExtraExpenseItem_(cusNoSub, cusNoPay, subsNo, 0, sub.PaperCode, sub.ProductNo,
                                                    ds.Tables[0].Rows[0]["EXPCODE"].ToString(),
                                                    1, sub.SubsStartDate, sub.SubsStartDate, "01",
                                                    double.Parse(ds.Tables[0].Rows[0]["DISCPERCENT"].ToString()));
                    }
                }
                catch (Exception ex)
                {
                    new Logger("TryAddCoProdIncl() - failed", ex.ToString());
                }
            }
        }

        private static void TryAddSubToBonDig(Subscription sub, DiPlusSubscription plusSub)
        {
            //130410 - only subs from dagensindustri.se/lasplatta are added to bondig
            if (plusSub != null)
            {
                long cusno = sub.Subscriber.Cusno;
                long subsno = sub.SubsNo;
                string email = sub.Subscriber.Email;
                //string passwd = (!string.IsNullOrEmpty(sub.Subscriber.PasswordBonDig)) ? sub.Subscriber.PasswordBonDig : sub.Subscriber.Password;
                //string passwd = sub.Subscriber.Password;
                //if (plusSub != null)
                string passwd = plusSub.Passwd;

                int ret = BonDigHandler.TryAddCustAndSubToBonDig(sub.PaperCode, sub.ProductNo, cusno, subsno, email, sub.Subscriber.FirstName,
                    sub.Subscriber.LastName, sub.Subscriber.MobilePhone, passwd, true, IsHybridCampaign(sub.CampNo));
                if (ret > 0)
                {
                    bool userAddedToBonDig = (ret == 1) ? true : false;
                    //BonDigHandler.SendBonDigWelcomeMail(userAddedToBonDig, email, passwd);   //130410 - only apsis welcome mail should be sent to cust
                }
                else
                {
                    //SendFailedToCrateIpadLoginMail(sub);
                    new Logger("TryAddSubToBonDig() - failed. ret=" + ret + ", cusno=" + cusno + ", subsno=" + subsno + ", email=" + email + ", passwd=" + passwd, "not an exception, but subscription was not saved to Bonnier digital");
                }
            }
        }

        

        //private static void SendFailedToCrateIpadLoginMail(Subscription sub)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append("Föjlande prenumeration kunde inte sparas i Bonnier digitals system.<br>");
        //    sb.Append("Det betyder förmodligen att kunden inte kan logga in via sin läsplatta.<br><br>");
        //    sb.Append("Undersök om kunden har ett fungerande inlogg.<br>");
        //    sb.Append("(2). Om inte: skapa ett fungerande inlogg.<br>");
        //    sb.Append("3. Skicka ett mail med inloggningsuppgifter till kunden.<br><br>");
        //    sb.Append(sub.ToString());

        //    MiscFunctions.SendMail("no-reply@di.se", MiscFunctions.GetAppsettingsValue("mailPrenFelDiSe"), "Inlogg sparades inte i S+", sb.ToString(), true);
        //}



        /// <summary>
        /// returns customers subsctiptions order by SubsRealEndDate DESC (ret[0] is most recent subs)
        /// </summary>
        public static List<Subscription> GetSubscriptions2(long cusno, string sUserId)
        {
            List<Subscription> subs = new List<Subscription>();

            try
            {
                DataSet ds = CirixDbHandler.Ws.GetSubscriptions_(cusno, "TRUE", sUserId);

                if (ds != null)
                {
                    foreach (DataTable dt in ds.Tables)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            Subscription sub = new Subscription();
                            sub.SubsNo = long.Parse(dr["SUBSNO"].ToString());
                            sub.SubsKind = dr["SUBSKIND"] as string;
                            sub.SubsStartDate = DbHelpMethods.SetDateFromDbFieldName(dr, "SUBSSTARTDATE");
                            sub.SubsEndDate = DbHelpMethods.SetDateFromDbFieldName(dr, "SUBSENDDATE");
                            sub.CampId = dr["CAMPID"] as string;
                            sub.SubsState = dr["SUBSSTATE"] as string;
                            sub.PaperCode = dr["PAPERCODE"] as string;
                            sub.ProductNo = dr["PRODUCTNO"] as string;
                            sub.Pricegroup = dr["PRICEGROUP"] as string;
                            sub.Substype = dr["SUBSTYPE"] as string;

                            long cno = 0;
                            long.TryParse(dr["CAMPNO"].ToString(), out cno);
                            if (cno > 0)
                                sub.CampNo = cno;
                            
                            sub.SuspendDate = DbHelpMethods.SetDateFromDbFieldName(dr, "SUSPENDDATE");
                            sub.Subscriber.Cusno = long.Parse(dr["SUBSCUSNO"].ToString());

                            int mons = 0;
                            int.TryParse(dr["SUBSLEN_MONS"].ToString(), out mons);
                            if (mons > 0)
                                sub.SubsLenMons = mons;

                            int days = 0;
                            int.TryParse(dr["SUBSLEN_DAYS"].ToString(), out days);
                            if (days > 0)
                                sub.SubsLenDays = days;

                            long payCusno = long.Parse(dr["PAYCUSNO"].ToString());
                            if (payCusno > 0)
                                sub.SubscriptionPayer.Cusno = payCusno;
                            else
                                sub.SubscriptionPayer = null;

                            int extno = 0;
                            int.TryParse(dr["EXTNO"].ToString(), out extno);
                            if (extno > 0)
                                sub.ExtNo = extno;

                            #region not used props
                            
                            //sub.ProductName = dr["PRODUCTNAME"] as string;
                            //sub.InvStartDate = DbHelpMethods.SetDateFromDbFieldName(dr, "INVSTARTDATE");
                            //sub.PaperName = dr["PAPERNAME"] as string;
                            //sub.UnpBreakDate = DbHelpMethods.SetDateFromDbFieldName(dr, "UNPBREAKDATE");
                            //sub.CancelReason = dr["CANCELREASON"] as string;
                            //sub.SubsKind_Codval = dr["SUBSKIND_CODVAL"] as string;
                            //sub.SubsState_Codval = dr["SUBSSTATE_CODVAL"] as string;
                            //sub.CancelReason_Codval = dr["CANCELREASON_CODVAL"] as string;
                            //sub.Pricegroup_Codval = dr["PRICEGROUP_CODVAL"] as string;
                            //sub.Substype_Codval = dr["SUBSTYPE_CODVAL"] as string;
                            #endregion

                            subs.Add(sub);
                        }
                    }
                }

                subs.Sort();  //subs[0] - latest SubsRealEndDate
                
            }
            catch (Exception ex)
            {
                new Logger("GetSubscriptions2() failed for cusno:" + cusno.ToString(), ex.ToString());
            }

            return subs;
        }

        public static void ChangeCusInvMode(long cusno, string newInvoiceMode, string oldInvoiceMode)
        {
            if (string.IsNullOrEmpty(newInvoiceMode) || string.IsNullOrEmpty(oldInvoiceMode) || newInvoiceMode == oldInvoiceMode)
                return;

            StringBuilder sb = new StringBuilder();
            sb.Append("cusno:" + cusno.ToString() + ", ");
            sb.Append("oldInvoiceMode:" + oldInvoiceMode + ", ");
            sb.Append("newInvoiceMode:" + newInvoiceMode);
            
            try
            {
                string ret = Ws.ChangeCusInvMode_(cusno, oldInvoiceMode, "Y", newInvoiceMode, "Y", "", "", "");
                
                if (ret.StartsWith("FAILED"))
                    new Logger("ChangeCusInvMode() failed. " + sb.ToString(), "Return from cirix: " + ret);
            }
            catch (Exception ex)
            {
                new Logger("ChangeCusInvMode() failed, exception. " + sb.ToString(), ex.ToString());
            }
        }

        public static string TryGetDefaultCusInvMode(long cusno)
        {
            try
            {
                DataSet ds = CirixDbHandler.Ws.GetCusInvModes_(cusno);
                if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
                {
                    foreach (DataTable dt in ds.Tables)
                    { 
                        foreach (DataRow dr in dt.Rows)
                        {
                            if (dr["INV_DEFAULT"].ToString() == "Y")
                                return dr["INVMODE"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new Logger("GetDefaultCusInvMode() failed for cusno:" + cusno.ToString(), ex.ToString());
            }

            return string.Empty;
        }

        /// <summary>
        /// invDefault = Y-is default / N-not default
        /// </summary>
        public static void AddNewCusInvmode(long cusno, string invMode, bool isInvDefault)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("cusno:" + cusno + ", ");
            sb.Append("invMode:" + invMode + ", ");
            sb.Append("isInvDefault:" + isInvDefault);

            try
            {
                string invDefault = isInvDefault ? "Y" : "N";
                string ret = Ws.AddNewCusInvmode_(cusno, invMode, invDefault, "", "", "");
                if (ret.StartsWith("FAILED"))
                    new Logger("AddNewCusInvmode() failed. " + sb.ToString(), "Cirix return: " + ret);
            }
            catch (Exception ex)
            {
                new Logger("AddNewCusInvmode() failed, exception. " + sb.ToString(), ex.ToString());
            }
        }


        public static long GetNextInvno()
        {
            return Ws.GetNextInvno_();
        }

        public static string BuildRefno2(long lInvno, string sInvType, string sPaperCode)
        {
            return Ws.BuildRefno2_(lInvno, sInvType, sPaperCode);
        }

        public static long CreateImmediateInvoice(Subscription subscription, int iExtno, int iItemno, long lInvno, string sRefno)
        {
            return Ws.CreateImmediateInvoice_(subscription.SubsNo, iExtno, iItemno, lInvno, sRefno);
        }

        public static string InsertElectronicPayment(long lCusno, long lInvno, string sRefno, double dAmount)
        {
            return Ws.InsertElectronicPayment_(lCusno, lInvno, sRefno, dAmount);
        }



        #endregion

        public static void CreateNewInvoice(Subscription subscription)
        {
            long invno = GetNextInvno();
            string refno = BuildRefno2(invno, "00", subscription.PaperCode);
            long immInv = CreateImmediateInvoice(subscription, 0, 1, invno, refno);
        }

        internal static bool IsHybridSubscriber(long cusno, string sUserId)
        {
            var dsSubs = GetSubscriptions(cusno, false, sUserId);
            if (DbHelpMethods.DataSetHasRows(dsSubs))
            {
                foreach (DataRow dr in dsSubs.Tables[0].Rows)
                {
                    var papercode = dr["papercode"].ToString();
                    var productno = dr["productno"].ToString();
                    if (papercode == Settings.PaperCode_DI && productno == Settings.ProductNo_Weekend)
                    {
                        long campno = 0;
                        long.TryParse(dr["campno"].ToString(), out campno);
                        return IsHybridCampaign(campno);
                    }
                }
            }

            return false;
        }

        public static bool IsHybridCampaign(long campNo)
        {
            if (campNo <= 0)
            {
                return false;
            }
            var dsCamp = GetCampaign(campNo);
            if (DbHelpMethods.DataSetHasRows(dsCamp))
            {
                long campgroupid = 0;
                long.TryParse(dsCamp.Tables[0].Rows[0]["campgroupid"].ToString(), out campgroupid);
                if (Settings.HybridCampGroupIds.Contains(campgroupid))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
