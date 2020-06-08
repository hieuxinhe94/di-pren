using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DIClassLib.EPiJobs.Apsis;
using System.Data;
using DIClassLib.DbHelpers;
using System.Data.SqlClient;
using DIClassLib.Misc;
using System.Configuration;


namespace DIClassLib.DbHandlers
{
    public class MailSenderDbHandler
    {
        private string _connStrMailSender = "ApsisMailSender";

        ///<returns>newly created batchId</returns>
        public int InsertBatch()
        {
            try
            {
                return int.Parse(SqlHelper.ExecuteScalar(_connStrMailSender, "insertBatch", null).ToString());
            }
            catch (Exception ex)
            {
                new Logger("InsertBatch() failed", ex.ToString());
            }

            return 0;
        }


        public int AddBouncesFromApsis()
        {
            int ret = 0;
            var aw = new ApsisWsHandler();
            var dsBounces = aw.ApsisGetBouncesV3();
            if (!DbHelpMethods.DataSetHasRows(dsBounces))
            {
                return ret;
            }
            foreach (DataTable dt in dsBounces.Tables)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    // AddBounce(int.Parse(dr[0].ToString()), dr[3].ToString(), dr[4].ToString(), DateTime.Parse(dr[2].ToString()));
                    // TODO: First parameter for mailId is probably wrong from Apsis! 
                    // So we add email to at least know which email bounced, as a first step to solve this problem
                    AddBounceWithEmail(int.Parse(dr[0].ToString()), dr[3].ToString(), dr[4].ToString(), DateTime.Parse(dr[2].ToString()), dr[1].ToString());
                    ret++;
                }
            }
            return ret;
        }

        public int UpdateEmailAndSetForceRetry(int custId, string newEmail)
        {
            var forceRetry = (MiscFunctions.IsValidEmail(newEmail) && EmailNotEmptyAndPassesRules(newEmail, GetEmailRules()));
            var sm = new ApsisSharedMethods();
            sm.UpdateAllEmailsAndSetForceRetry(custId, newEmail, forceRetry, false);
            return forceRetry ? 1 : 2;
        }

        public List<ApsisCustomer> GetRetryCusts()
        {
            var custs = new List<ApsisCustomer>();
            
            try
            {
                var ds = SqlHelper.ExecuteDataset(_connStrMailSender, "getRetryCusts", null);
                if (DbHelpMethods.DataSetHasRows(ds))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        var c = new ApsisCustomer();
                        SetCustomerProps(c, dr);

                        if (!CustomerInList(c, custs))
                            custs.Add(c);
                    }
                }
            }
            catch (Exception ex)
            {
                new Logger("GetRetryCusts() failed", ex.ToString());
            }
            
            return custs;
        }

        private static bool CustomerInList(ApsisCustomer c, List<ApsisCustomer> custs)
        {
            return custs.Any(cu => c.CustomerId == cu.CustomerId);
        }

        //sendSuccessCusts: mail sent X days ago, has not bounced
        public List<ApsisCustomer> GetNewSendSuccessCusts()
        {
            int daysBack;
            var custs = new List<ApsisCustomer>();
            if (!int.TryParse(MiscFunctions.GetAppsettingsValue("apsisDaysSendSuccess"), out daysBack))
            {
                daysBack = 5;
            }

            var minusDaysBack = -daysBack;
            try
            {
                var ds = SqlHelper.ExecuteDataset(_connStrMailSender, "getNewSendSuccessCusts", new SqlParameter("dtBack", DateTime.Now.AddDays(minusDaysBack)));
                if (DbHelpMethods.DataSetHasRows(ds))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        var c = new ApsisCustomer();
                        SetCustomerProps(c, dr);
                        custs.Add(c);
                    }
                }
            }
            catch (Exception ex)
            {
                new Logger("GetNewSendSuccessCusts() failed", ex.ToString());
            }

            return custs;
        }

        public List<ApsisCustomer> GetCustomersForApsisStep(int step)
        {
            var custs = new List<ApsisCustomer>();
            try
            {
                var ds = SqlHelper.ExecuteDataset(_connStrMailSender, "getCustomersForApsisStep", new SqlParameter("@apsisStep", step));
                if (DbHelpMethods.DataSetHasRows(ds))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        var c = new ApsisCustomer();
                        SetCustomerProps(c, dr);
                        custs.Add(c);
                    }
                }
            }
            catch (Exception ex)
            {
                new Logger("GetCustomersForApsisStep() failed", ex.ToString());
            }
            return custs;
        }

        public void MarkCustomerForApsisUpdateJob(List<int> customerIds)
        {
            if (!customerIds.Any())
            {
                return;
            }
            var stringCustomerIds = new StringBuilder();
            foreach (var id in customerIds)
            {
                stringCustomerIds.Append(id + ",");

            }
            var sql = string.Format(@"update customer SET ApsisUpdateDone = NULL, ApsisUpdateCheckServicePlus = 11 WHERE customerId IN ({0})",
                stringCustomerIds.ToString().TrimEnd(','));
            try
            {
                SqlHelper.ExecuteNonQuerySql(_connStrMailSender, sql);
            }
            catch (Exception ex)
            {
                new Logger("MarkCustomerForApsisUpdateJob(List<int>) failed", ex.ToString());
            }
        }

        //Related to GetCustomersForApsisUpdateJob()
        public void MarkCustomerForApsisUpdateJob(List<ApsisCustomer> customerInfo)
        {
            if (!customerInfo.Any())
            {
                return;
            }
            var customerUpdateStatements = new StringBuilder();
            //TODO: Due to that we're still using SQL 2005 that not support Table Valued Parameters, so easier to do like this:
            //ApsisUpdateCheckServicePlus = 11 small hack to indicate on users that previous didn't have a S+ account (ApsisUpdateCheckServicePlus = 10) now will be rechecked!
            foreach (var customer in customerInfo)
            {
                //Only touch ApsisUpdateCheckServicePlus if it is already 10, which means user had earlier no account, but now we want to recheck that
                customerUpdateStatements.Append(string.Format("update customer SET ApsisUpdateDone = NULL, ApsisUpdateCheckServicePlus = CASE WHEN ApsisUpdateCheckServicePlus = 10 THEN 11 ELSE ApsisUpdateCheckServicePlus END, PaperCode = '{0}', ProductNo = '{1}', SubsLen_Mons={2}  WHERE customerId = {3} ;",
                    customer.f_PaperCode, customer.f_ProductNo, customer.SubsLenMonsFromCirix, customer.CustomerId));
            }
           
            try
            {
                SqlHelper.ExecuteNonQuerySql(_connStrMailSender, customerUpdateStatements.ToString());
            }
            catch (Exception ex)
            {
                new Logger("MarkCustomerForApsisUpdateJob(List<ApsisCustomer>) failed", ex.ToString());
            }
        }

        public void SetHaveServicePlusAccountField(int customerId,bool haveServicePlusAccount)
        {
            try
            {
                var sql = string.Format("update customer SET HaveServicePlusAccount = '{0}' WHERE customerId = {1}", haveServicePlusAccount, customerId);
                SqlHelper.ExecuteNonQuerySql(_connStrMailSender, sql);
            }
            catch (Exception ex)
            {
                new Logger(string.Format("SetHaveServicePlusAccountField() failed for cusno: {0}", customerId), ex.ToString());
            }
        }

        //Related to MarkCustomerForApsisUpdateJob()
        public List<ApsisCustomer> GetCustomersForApsisUpdateJob()
        {
            var custs = new List<ApsisCustomer>();
            try
            {
                var ds = SqlHelper.ExecuteDataset(_connStrMailSender, "getCustomersForApsisUpdateJob", null);
                if (DbHelpMethods.DataSetHasRows(ds))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        var c = new ApsisCustomer();
                        SetCustomerProps(c, dr);
                        custs.Add(c);
                    }
                }
            }
            catch (Exception ex)
            {
                new Logger("GetCustomersForApsisUpdateJob() failed", ex.ToString());
            }
            return custs;
        }

        public void UpdateApsisCustomerStep(ApsisCustomer customer, int stepToSet)
        {
            try
            {
                var sqlparams = new SqlParameter[]
                {
                    new SqlParameter("@custId", customer.CustomerId), 
                    new SqlParameter("@stepToSet", stepToSet),
                    new SqlParameter("@haveServicePlusAccount", customer.HaveServicePlusAccount) 
                };
                SqlHelper.ExecuteNonQuery(_connStrMailSender, "UpdateApsisCustomerStep", sqlparams);
            }
            catch (Exception ex)
            {
                new Logger("UpdateApsisCustomerCheck() failed", ex.ToString());
            }
        }

        public void SetCustomerNotAvailableInApsis(int customerId)
        {
            try
            {
                var sqlparams = new SqlParameter[]
                {
                    new SqlParameter("@customerId", customerId)
                };
                SqlHelper.ExecuteNonQuery(_connStrMailSender, "SetCustomerNotAvailableInApsis", sqlparams);
            }
            catch (Exception ex)
            {
                new Logger("SetCustomerNotAvailableInApsis() failed", ex.ToString());
            }
        }

        public void SetIsSendSuccess(List<ApsisCustomer> custs, bool isSendSuccess)
        {
            if (custs.Count == 0)
                return;

            //using inline SQL to avoid multiple SP-calls
            var sql = new StringBuilder();
            sql.Append("update customer set isSendSuccess = '" + isSendSuccess + "' where customerId in (");

            int i = -1;
            foreach (var c in custs)
            {
                i++;

                if (i == 0)
                    sql.Append(c.CustomerId.ToString());
                else
                    sql.Append("," + c.CustomerId.ToString());
            }
            sql.Append(")");

            try
            {
                var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApsisMailSender"].ToString());
                conn.Open();
                var sqlComm = new SqlCommand(sql.ToString(), conn);
                sqlComm.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception ex)
            {
                new Logger("SetIsSendSuccess() failed", ex.ToString());
            }

        }


        public void AddNewCusts(List<ApsisCustomer> custs)
        {
            foreach (var c in custs)
                AddNewCustomer(c);
        }

        /// <summary>
        /// adds new row in customerInBatch table
        /// sets isBounce=0 and forceRetry=0 in customer table
        /// also updates email + dateUpdated in customer table if email updated in cirix
        /// </summary>
        public void InsertCustomerInBatch(string identifier, ApsisCustomer cus, ApsisBatch ba, int apsisMailId)
        {
            try
            {
                var arr = new SqlParameter[] { 
                                                        new SqlParameter("@identifier", identifier), 
                                                        new SqlParameter("@customerId", cus.CustomerId), 
                                                        new SqlParameter("@batchId", ba.BatchId),
                                                        new SqlParameter("@apsisMailId", apsisMailId), 
                                                        new SqlParameter("@email", cus.Email)
                                                     };

                SqlHelper.ExecuteNonQuery(_connStrMailSender, "insertCustomerInBatch", arr);
            }
            catch (Exception ex)
            {
                new Logger("InsertCustomerInBatch() failed", ex.ToString());
            }
        }

        public void SetForceRetry(int customerId, bool forceRetry, bool updateByExt)
        {
            try
            {
                var arr = new SqlParameter[] 
                { 
                    new SqlParameter("@customerId", customerId), 
                    new SqlParameter("@forceRetry", forceRetry) 
                };

                SqlHelper.ExecuteNonQuery(_connStrMailSender, !updateByExt ? "setForceRetry" : "setForceRetryExt", arr);
            }
            catch (Exception ex)
            {
                new Logger("SetForceRetry() failed", ex.ToString());
                throw;
            }
        }

        public void SetDateRegularLetter(int customerId)
        {
            try
            {
                var arr = new SqlParameter[] 
                { 
                    new SqlParameter("@customerId", customerId), 
                    new SqlParameter("@dateRegularLetter", DateTime.Now) 
                };
                SqlHelper.ExecuteNonQuery(_connStrMailSender, "setDateRegularLetter", arr);
            }
            catch (Exception ex)
            {
                new Logger("SetDateRegularLetter() failed", ex.ToString());
                throw;
            }
        }

        //2015-02-23 Changing to use cus.InvStartDate instead of cus.SubStartDate
        private void AddNewCustomer(ApsisCustomer cus)
        {
            try
            {
                var arr = new SqlParameter[] { 
                                                        new SqlParameter("@customerId", cus.CustomerId), 
                                                        new SqlParameter("@apsisProjectGuid", cus.ApsisProjectGuid),
                                                        new SqlParameter("@name", cus.Name), 
                                                        new SqlParameter("@email", cus.Email), 
                                                        new SqlParameter("@userName", cus.UserName), 
                                                        new SqlParameter("@passWord", cus.PassWord),
                                                        new SqlParameter("@targetGroup", cus.TargetGroup),
                                                        new SqlParameter("@campId", cus.CampId),
                                                        new SqlParameter("@subsStartDate", cus.InvStartDate),
                                                        new SqlParameter("@subsEndDate", cus.SubsEndDate),
                                                        new SqlParameter("@campNo", cus.CampNo),
                                                        new SqlParameter("@receiveType", cus.ReceiveType),
                                                        new SqlParameter("@isExtCustomer", cus.IsExtCustomer),
                                                        new SqlParameter("@PaperCode", cus.f_PaperCode),
                                                        new SqlParameter("@ProductNo", cus.f_ProductNo),
                                                        new SqlParameter("@SubsLen_Mons", cus.SubsLenMonsFromCirix)
                                                     };

                if (cus.InvStartDate == DateTime.MinValue)
                    arr[9] = new SqlParameter("@subsStartDate", DBNull.Value);

                if (cus.SubsEndDate == DateTime.MinValue)
                    arr[10] = new SqlParameter("@subsEndDate", DBNull.Value);

                if (cus.CampNo == null)
                    arr[11] = new SqlParameter("@campNo", DBNull.Value);

                if (string.IsNullOrEmpty(cus.ReceiveType))
                    arr[12] = new SqlParameter("@receiveType", DBNull.Value);


                SqlHelper.ExecuteNonQuery(_connStrMailSender, "insertUpdateCustomer", arr);
            }
            catch (Exception ex)
            {
                new Logger("AddNewCustomer() failed", ex.ToString());
            }
        }

        public List<ApsisCustomer> GetBouncedCusts()
        {
            var custs = new List<ApsisCustomer>();
            var ds = GetBouncedCustsDS();
            if (DbHelpMethods.DataSetHasRows(ds))
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    var c = new ApsisCustomer();
                    SetCustomerProps(c, dr);
                    custs.Add(c);
                }
            }

            return custs;
        }

        public DataSet GetBouncedCustsDS()
        {
            try { return SqlHelper.ExecuteDataset(_connStrMailSender, "getBouncedCusts", null); }
            catch (Exception ex) { new Logger("GetBouncedCusts() failed", ex.ToString()); }

            return new DataSet();
        }

        public ApsisCustomer GetCustomer(int customerId)
        {
            try
            {
                var c = new ApsisCustomer();
                var ds = SqlHelper.ExecuteDataset(_connStrMailSender, "getCustomer", new SqlParameter("customerId", customerId));
                if (DbHelpMethods.DataSetHasRows(ds))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                        SetCustomerProps(c, dr);
                }

                return c;
            }
            catch (Exception ex)
            {
                new Logger("GetCustomer() failed", ex.ToString());
                throw;
            }
        }

        public void DeleteCustomer(int customerId, string cirixLetterFlag)
        {
            try
            {
                if (!string.IsNullOrEmpty(cirixLetterFlag))
                {
                    var c = new ApsisCustomer();
                    c.CustomerId = customerId;
                    var custs = new List<ApsisCustomer>();
                    custs.Add(c);
                    SubscriptionController.FlagCustsInLetter(custs, cirixLetterFlag);
                }

                var arr = new SqlParameter[] { new SqlParameter("customerId", customerId) };
                SqlHelper.ExecuteNonQuery(_connStrMailSender, "deleteCustomer", arr);

            }
            catch (Exception ex)
            {
                new Logger("DeleteCustomer() failed", ex.ToString());
            }
        }

        // Look at AddBounceWithEmail() below
        private void AddBounce(int apsisMailId, string bounceCategory, string bounceReason, DateTime dateBounce)
        {
            try
            {
                var arr = new SqlParameter[] { 
                                                        new SqlParameter("@apsisMailId", apsisMailId), 
                                                        new SqlParameter("@bounceCategory", bounceCategory),
                                                        new SqlParameter("@bounceReason", bounceReason), 
                                                        new SqlParameter("@dateBounce", dateBounce)
                                                     };

                SqlHelper.ExecuteNonQuery(_connStrMailSender, "addBounce", arr);
            }
            catch (Exception ex)
            {
                new Logger("AddBounce() failed", ex.ToString());
            }
        }

        /// <summary>
        /// To be replacing AddBounce in future maybe
        /// </summary>
        /// <param name="apsisMailId"></param>
        /// <param name="bounceCategory"></param>
        /// <param name="bounceReason"></param>
        /// <param name="dateBounce"></param>
        /// <param name="email"></param>
        private void AddBounceWithEmail(int apsisMailId, string bounceCategory, string bounceReason, DateTime dateBounce, string email)
        {
            try
            {
                var arr = new SqlParameter[] { 
                                                        new SqlParameter("@apsisMailId", apsisMailId), 
                                                        new SqlParameter("@bounceCategory", bounceCategory),
                                                        new SqlParameter("@bounceReason", bounceReason), 
                                                        new SqlParameter("@dateBounce", dateBounce),
                                                        new SqlParameter("@email", email)
                                                     };

                SqlHelper.ExecuteNonQuery(_connStrMailSender, "addBounceWithEmail", arr);
            }
            catch (Exception ex)
            {
                new Logger("AddBounceWithEmail() failed", ex.ToString());
            }
        }

        private void SetCustomerProps(ApsisCustomer c, DataRow dr)
        {
            int custId = -1;

            try
            {
                custId = int.Parse(dr["customerId"].ToString());
                c.CustomerId = custId;
                c.ApsisProjectGuid = dr["apsisProjectGuid"].ToString();
                c.Name = dr["name"].ToString();
                c.Email = dr["email"].ToString();
                c.UserName = dr["userName"].ToString();
                c.PassWord = dr["passWord"].ToString();
                c.DateSaved = SetDateFromFieldName(dr, "dateSaved");
                c.DateUpdated = SetDateFromFieldName(dr, "dateUpdated");
                c.DateRegularLetter = SetDateFromFieldName(dr, "dateRegularLetter");
                c.IsBounce = bool.Parse(dr["isBounce"].ToString());
                c.ForceRetry = bool.Parse(dr["forceRetry"].ToString());
                c.IsSendSuccess = bool.Parse(dr["isSendSuccess"].ToString());
                c.TargetGroup = dr["targetGroup"].ToString();
                c.CampId = dr["campId"].ToString();
                c.SubsEndDate = SetDateFromFieldName(dr, "subsEndDate");
                c.ContactStatus = dr["contactStatus"].ToString();

                int tmpCN;
                if (int.TryParse(dr["campNo"].ToString(), out tmpCN))
                    c.CampNo = tmpCN;

                c.ReceiveType = dr["receiveType"].ToString();
                c.IsExtCustomer = bool.Parse(dr["isExtCustomer"].ToString());
                c.DateEmailUpdatedByExt = SetDateFromFieldName(dr, "dateEmailUpdatedByExt");
                c.DateRejectedByExt = SetDateFromFieldName(dr, "dateRejectedByExt");
                c.f_PaperCode = dr["PaperCode"] != null ? dr["PaperCode"].ToString() : string.Empty;
                c.f_ProductNo = dr["ProductNo"] != null ? dr["ProductNo"].ToString() : string.Empty;
                int tmpLen;
                if (int.TryParse(dr["SubsLen_Mons"].ToString(), out tmpLen))
                    c.SubsLenMonsFromCirix = tmpLen;
            }
            catch (Exception ex)
            {
                new Logger("SetCustomerProps() failed for customerId: " + custId.ToString(), ex.ToString());
            }
        }

        private static DateTime SetDateFromFieldName(DataRow dr, string fieldName)
        {
            DateTime dt;
            return DateTime.TryParse(dr[fieldName].ToString(), out dt) ? dt : DateTime.MinValue;
        }


        /// <summary>
        /// Updates email in ApsisMailSender database and sets forceRetry. 
        /// Argument updateByExt: ext is short for external (S2 or other partner).
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="email"></param>
        /// <param name="forceRetry"></param>
        /// <param name="updateByExt"></param>
        public void UpdateEmailInMailSenderDb(int customerId, string email, bool forceRetry, bool updateByExt)
        {
            try
            {
                var arr = new SqlParameter[] 
                { 
                    new SqlParameter("@customerId", customerId), 
                    new SqlParameter("@email", email),
                    new SqlParameter("@forceRetry", forceRetry)
                };

                SqlHelper.ExecuteNonQuery(_connStrMailSender, !updateByExt ? "updateEmail" : "updateEmailExt", arr);
            }
            catch (Exception ex)
            {
                new Logger("UpdateEmail() failed", ex.ToString());
                throw;
            }
        }

        public DataSet GetEmailRules()
        {
            try
            {
                var ds = SqlHelper.ExecuteDataset(_connStrMailSender, "getEmailRules", null);
                return ds;
            }
            catch (Exception ex)
            {
                new Logger("GetEmailRules() failed", ex.ToString());
            }

            return null;
        }

        public bool EmailNotEmptyAndPassesRules(string email, DataSet dsRules)
        {
            if (string.IsNullOrEmpty(email) || email.ToLower() == "null")
                return false;

            var mailPrefix = email;
            if (email.Contains('@'))
                mailPrefix = email.Substring(0, email.IndexOf('@'));   //info@xxx.se ==> info

            //loop rules
            if (DbHelpMethods.DataSetHasRows(dsRules))
            {
                foreach (DataRow dr in dsRules.Tables[0].Rows)
                {
                    if (mailPrefix.ToLower() == dr.ItemArray[1].ToString().ToLower())
                        return false;
                }
            }

            //does not match rules
            return true;
        }

        public void InsertEmailRule(string rule)
        {
            try
            {
                var arr = new SqlParameter[] { new SqlParameter("@emailRule", rule) };
                SqlHelper.ExecuteNonQuery(_connStrMailSender, "insertEmailRule", arr);
            }
            catch (Exception ex)
            {
                new Logger("InsertEmailRule() failed", ex.ToString());
            }
        }

        public void UpdateEmailRule(int ruleId, string rule)
        {
            try
            {
                var arr = new SqlParameter[] { 
                                                            new SqlParameter("@emailRuleId", ruleId),
                                                            new SqlParameter("@emailRule", rule)
                                                        };
                SqlHelper.ExecuteNonQuery(_connStrMailSender, "updateEmailRule", arr);
            }
            catch (Exception ex)
            {
                new Logger("UpdateEmailRule() failed", ex.ToString());
            }
        }

        public void DeleteEmailRule(int ruleId)
        {
            try
            {
                var arr = new SqlParameter[] { new SqlParameter("@emailRuleId", ruleId) };
                SqlHelper.ExecuteNonQuery(_connStrMailSender, "deleteEmailRule", arr);
            }
            catch (Exception ex)
            {
                new Logger("DeleteEmailRule() failed", ex.ToString());
            }
        }

        public bool IsFirstBatchInHour(DateTime dt)
        {
            var numBatches = 0;

            SqlDataReader rdr = null;
            var dtStart = new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, 0, 0);
            var dtEnd = new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour + 1, 0, 0);

            try
            {
                SqlParameter[] sqlParams = { new SqlParameter("@dateStart", dtStart), 
                                             new SqlParameter("@dateEnd", dtEnd) };

                rdr = SqlHelper.ExecuteReader(_connStrMailSender,
                                                                "getNumBatchesForInterval",
                                                                sqlParams);

                while (rdr.Read())
                    numBatches = int.Parse(rdr[0].ToString());

            }
            catch (Exception ex)
            {
                new Logger("IsFirstBatchInHour() failed", ex.ToString());
            }
            finally
            {
                if (rdr != null)
                    rdr = null;
            }

            if (numBatches == 1)
                return true;

            return false;
        }

        public DateTime GetLatestDateBounce()
        {
            try
            {
                var dt = new DateTime(1900, 1, 1);
                var o = SqlHelper.ExecuteScalar(_connStrMailSender, "getLatestDateBounce", null);

                if (o == null)       //bounce table has no posts, return '1900-01-01'
                    return dt;

                DateTime.TryParse(o.ToString(), out dt);
                return dt;
            }
            catch (Exception ex)
            {
                new Logger("GetLatestDateBounce() failed", ex.ToString());
                return DateTime.Now.AddHours(1);   //dont know latestDateBouce - return future date
            }
        }

        public void FlagCustAsBounce(int custId)
        {
            try
            {
                var arr = new SqlParameter[] { new SqlParameter("@customerId", custId) };
                SqlHelper.ExecuteNonQuery(_connStrMailSender, "flagCustAsBounce", arr);
            }
            catch (Exception ex)
            {
                new Logger("FlagCustAsBounce() failed", ex.ToString());
            }
        }

        public DataSet GetLatestBounceForCust(int custId)
        {
            try
            {
                return SqlHelper.ExecuteDataset(_connStrMailSender, "getLatestBounceInfoForCust", new SqlParameter("customerId", custId));
            }
            catch (Exception ex)
            {
                new Logger("GetLatestBounceForCust() failed", ex.ToString());
            }

            return null;
        }

        public void UpdateEmailInCustomer(int cusNo, string email)
        {
            try
            {
                SqlParameter[] arr = new SqlParameter[] 
                                    { 
                                        new SqlParameter("@cusno", cusNo),
                                        new SqlParameter("@email", email) 
                                    };
                SqlHelper.ExecuteNonQuery("DisePren", "UpdateEmailInCustomer", arr);
            }
            catch (Exception ex)
            {
                new Logger("UpdateEmailInCustomer() failed", ex.ToString());
                throw;
            }
        }

        public void UpdateContactStatus(int customerId, string contactStatus)
        {
            try
            {
                var arr = new SqlParameter[] { 
                                                            new SqlParameter("@customerId", customerId),
                                                            new SqlParameter("@contactStatus", contactStatus)
                                                        };
                SqlHelper.ExecuteNonQuery(_connStrMailSender, "updateContactStatus", arr);
            }
            catch (Exception ex)
            {
                new Logger("UpdateContactStatus() failed", ex.ToString());
            }
        }

        public void FlagCustAsSent(int custId)
        {
            try
            {
                var arr = new SqlParameter[] { new SqlParameter("@customerId", custId) };
                SqlHelper.ExecuteNonQuery(_connStrMailSender, "flagCustAsSent", arr);
            }
            catch (Exception ex)
            {
                new Logger("FlagCustAsSent() failed", ex.ToString());
            }
        }


        public void RejectCustsExt(List<int> custIds)
        {
            try
            {
                if (custIds.Count == 0)
                    return;

                string dt = DateTime.Now.ToString();

                //using inline SQL to avoid multiple SP-calls
                var sql = new StringBuilder();
                sql.Append("update customer set ");
                sql.Append("dateUpdated='" + dt + "', ");
                sql.Append("dateRejectedByExt='" + dt + "', ");
                sql.Append("isExtCustomer=0 ");
                sql.Append("where customerId in (");

                var i = -1;
                foreach (var id in custIds)
                {
                    i++;

                    if (i == 0)
                        sql.Append(id.ToString());
                    else
                        sql.Append("," + id.ToString());
                }

                sql.Append(")");


                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApsisMailSender"].ToString());
                conn.Open();
                SqlCommand sqlComm = new SqlCommand(sql.ToString(), conn);
                sqlComm.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception ex)
            {
                new Logger("ExtRejectCusts() failed", ex.ToString());
                throw;
            }
        }

        // TODO: Can be removed??
        public void S2_TEST_ResetCustomer(int custId, string email, bool isBounce)
        {
            try
            {
                var arr = new SqlParameter[] { 
                    new SqlParameter("@customerId", custId),
                    new SqlParameter("@email", email),
                    new SqlParameter("@isBounce", isBounce)
                };

                SqlHelper.ExecuteNonQuery(_connStrMailSender, "S2_TEST_ResetCustomer", arr);
            }
            catch (Exception ex)
            {
                new Logger("S2_TEST_ResetCustomer() failed", ex.ToString());
                throw;
            }
        }


        public void UpdateCustomer(ApsisCustomer cus)
        {
            if (cus == null || cus.CustomerId < 1)
            {
                new Logger("UpdateCustomer() failed - cus=null or cus.id<1", "");
                return;
            }

            try
            {
                var arr = new SqlParameter[] { 
                                                            new SqlParameter("@customerId", cus.CustomerId),
                                                            new SqlParameter("@apsisProjectGuid", cus.ApsisProjectGuid),
                                                            new SqlParameter("@name", cus.Name),
                                                            new SqlParameter("@email", cus.Email),
                                                            new SqlParameter("@userName", cus.UserName),
                                                            new SqlParameter("@passWord", cus.PassWord),
                                                            new SqlParameter("@targetGroup", cus.TargetGroup),
                                                            new SqlParameter("@campId", cus.CampId),
                                                            new SqlParameter("@subsStartDate", cus.InvStartDate),
                                                            new SqlParameter("@subsEndDate", cus.SubsEndDate),
                                                            new SqlParameter("@campNo", cus.CampNo),
                                                            new SqlParameter("@receiveType", cus.ReceiveType),
                                                            new SqlParameter("@isExtCustomer", cus.IsExtCustomer)
                                                        };
                SqlHelper.ExecuteNonQuery(_connStrMailSender, "updateCustomer", arr);
            }
            catch (Exception ex)
            {
                new Logger("UpdateCustomer() failed", ex.ToString());
            }
        }


        public void UpdateScheduledJobRunLastGetDate(int jobId, DateTime lastGetDate)
        {
            try
            {
                var arr = new SqlParameter[]
                {
                    new SqlParameter("@JobId", jobId),
                    new SqlParameter("@LastGetDate", lastGetDate)
                };
                SqlHelper.ExecuteNonQuery(_connStrMailSender, "insertUpdateScheduledJobRunLastGetDate", arr);
            }
            catch (Exception ex)
            {
                new Logger("UpdateScheduledJobRunLastGetDate() failed", ex.ToString());
            }
        }

        /// <summary>
        /// Defaults to datetime now -1 hour
        /// </summary>
        public DateTime GetScheduledJobRunLastGetDate(int jobId)
        {
            try
            {
                var ds = SqlHelper.ExecuteDataset(_connStrMailSender, "getScheduledJobRunLastGetDate", new SqlParameter("@JobId", jobId));
                if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows != null && ds.Tables[0].Rows[0] != null && ds.Tables[0].Rows[0][0] != null)
                {
                    return DateTime.Parse(ds.Tables[0].Rows[0][0].ToString());
                }                
            }
            catch (Exception ex)
            {
                new Logger("GetScheduledJobRunLastGetDate() failed", ex.ToString());                
            }

            return DateTime.Now.AddHours(-1);
        }

    }
}
