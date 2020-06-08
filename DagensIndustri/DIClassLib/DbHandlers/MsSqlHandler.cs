using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DIClassLib.CardPayment.Nets;
using DIClassLib.Misc;
using System.Data.SqlClient;
using DIClassLib.DbHelpers;
using DIClassLib.CardPayment;
using DIClassLib.Subscriptions;
using DIClassLib.SingleSignOn;
using DIClassLib.EPiJobs.EpiDataForExternalUse;


namespace DIClassLib.DbHandlers
{
    public static class MsSqlHandler
    {

        #region InviteFriend

        public static DataSet GetInviteFriend(int epiPageId, string senderCusNo)
        {
            SqlParameter[] sqlParameters = new SqlParameter[]{
                new SqlParameter("@pageid", epiPageId), 
                new SqlParameter("@sendercusno", senderCusNo)
            };

            return DIClassLib.DbHelpers.SqlHelper.ExecuteDatasetParam ("DagensIndustriMISC", "GetInviteFriend", sqlParameters);
        }

        public static string InsertInviteFriend(int epiPageId, long senderCusNo, string senderFirstName, string senderLastName, string senderMessage, string receiverFirstName, string receiverLastName, string receiverEmail, string receiverPhone, bool pren, Guid prenGuid)
        {
            SqlParameter[] sqlParameters = new SqlParameter[]{
                new SqlParameter("@pageid", epiPageId), 
                new SqlParameter("@sendercusno", senderCusNo), 
                new SqlParameter("@senderfirstname", senderFirstName),
                new SqlParameter("@senderlastname", senderLastName),
                new SqlParameter("@sendermessage", senderMessage),
                new SqlParameter("@receiverfirstname", receiverFirstName),
                new SqlParameter("@receiverlastname", receiverLastName),
                new SqlParameter("@receiveremail", receiverEmail),
                new SqlParameter("@receiverphone", receiverPhone),
                new SqlParameter("@pren", pren),
                new SqlParameter("@prenguid", prenGuid)
            };

            var inviteId = DIClassLib.DbHelpers.SqlHelper.ExecuteScalarParam("DagensIndustriMISC", "InsertInviteFriend", sqlParameters);

            return inviteId.ToString();
        }

        public static void UpdateInviteFriend(Guid prenGuid, bool pren)
        {
            SqlParameter[] sqlParameters = new SqlParameter[] 
                    {
                       new SqlParameter("@prenguid", prenGuid ),  
                       new SqlParameter("@pren", pren)
                    };

            DIClassLib.DbHelpers.SqlHelper.ExecuteNonQuery("DagensIndustriMISC", "UpdateInviteFriend", sqlParameters);
        }

        #endregion


        #region Competition

        public static DataSet GetParticipants(int epiPageId)
        {
            return DIClassLib.DbHelpers.SqlHelper.ExecuteDataset("DagensIndustriMISC", "GetCompetitionParticipants", new SqlParameter("@pageid", epiPageId));
        }

        public static string InsertParticipant(int epiPageId, string firstName, string lastName, string email, string phone, string answers, bool isCorrect, int readerId, bool pren, bool receiveinfo)
        {
            SqlParameter[] sqlParameters = new SqlParameter[]{
                new SqlParameter("@pageid", epiPageId), 
                new SqlParameter("@firstname", firstName), 
                new SqlParameter("@lastname", lastName),
                new SqlParameter("@email", email),
                new SqlParameter("@phone", phone),
                new SqlParameter("@answers", answers),
                new SqlParameter("@iscorrect", isCorrect),
                new SqlParameter("@readerid", readerId),
                new SqlParameter("@pren", pren),
                new SqlParameter("@receiveinfo", receiveinfo)
            };

            var participantId = DIClassLib.DbHelpers.SqlHelper.ExecuteScalarParam("DagensIndustriMISC", "InsertCompetitionParticipant", sqlParameters);

            return participantId.ToString();
        }

        public static void UpdateParticipant(string participantId, bool pren)
        {
            SqlParameter[] sqlParameters = new SqlParameter[] 
                    {
                       new SqlParameter("@participantId", participantId ),  
                       new SqlParameter("@pren", pren)
                    };

            DIClassLib.DbHelpers.SqlHelper.ExecuteNonQuery("DagensIndustriMISC", "UpdateCompetitionParticipant", sqlParameters);
        }

        #endregion


        #region Campaign v3

        static string CAMP3 = "Campaign_v3";

        public static DataSet GetCampaignTargetGroups(int epiPageId)
        {
            return DIClassLib.DbHelpers.SqlHelper.ExecuteDataset(CAMP3, "getTargGrsInCamp", new SqlParameter("@epiPageId", epiPageId));
        }

        public static void SaveCamp(int epiPageId, string campId, string targetGroupReg, string targetGroupEmail, string targetGroupPostal, string targetGroupMobile)
        {
            SqlParameter[] sqlParameters = new SqlParameter[]{
                    new SqlParameter("@epiPageId", epiPageId), 
                    new SqlParameter("@campId", campId), 
                    new SqlParameter("@targetGroupReg", targetGroupReg),
                    new SqlParameter("@targetGroupEmail", targetGroupEmail),
                    new SqlParameter("@targetGroupPostal", targetGroupPostal),
                    new SqlParameter("@targetGroupMobile", targetGroupMobile)
                };

            DIClassLib.DbHelpers.SqlHelper.ExecuteNonQuery(CAMP3, "saveCamp2", sqlParameters);
        }

        public static DataSet GetCustByCode(string code)
        {
            return DIClassLib.DbHelpers.SqlHelper.ExecuteDataset(CAMP3, "getCustByCode", new SqlParameter("@code", code));
        }

        //public static DataSet GetCust(string code, int epiPageId)
        //{
        //    SqlParameter[] sqlParameters = new SqlParameter[] 
        //    { 
        //        new SqlParameter("@code", code), 
        //        new SqlParameter("@epiPageId", epiPageId) 
        //    };

        //    return DIClassLib.DbHelpers.SqlHelper.ExecuteDatasetParam(CAMP3, "getCust", sqlParameters);
        //}

        public static void SetDateVisitedCamp(string code, int epiPageId)
        {
            SqlParameter[] sqlParameters = new SqlParameter[] 
            { 
                new SqlParameter("@code", code),
                new SqlParameter("@epiPageId", epiPageId)
            };

            DIClassLib.DbHelpers.SqlHelper.ExecuteScalarParam(CAMP3, "setDateVisitedCamp2", sqlParameters);
        }

        public static void SetDateBoughtCamp(string code, int epiPageId, long cusno)
        {
            SqlParameter[] sqlParameters = new SqlParameter[] 
            { 
                new SqlParameter("@code", code),
                new SqlParameter("@epiPageId", epiPageId),
                new SqlParameter("@cusno",cusno)
            };

            DIClassLib.DbHelpers.SqlHelper.ExecuteScalarParam(CAMP3, "setDateBoughtCamp3", sqlParameters);
        }


        #region campTip

        public static void InsertTipCustomer(int epiPageId, string code, string codeTipper, string email, string firstName, string lastName)
        {
            SqlParameter[] sqlParameters = new SqlParameter[] 
            {
                new SqlParameter("@epiPageId", epiPageId),
                new SqlParameter("@code", code),
                new SqlParameter("@codeTipper", codeTipper),
                new SqlParameter("@email", email),
                new SqlParameter("@firstName", firstName),
                new SqlParameter("@lastName", lastName)
            };

            DIClassLib.DbHelpers.SqlHelper.ExecuteNonQuery(CAMP3, "insertTipCustomer", sqlParameters);
        }

        public static int GetNumTipped(string codeTipper)
        {
            return int.Parse(DIClassLib.DbHelpers.SqlHelper.ExecuteScalar(CAMP3, "getNumTipped", new SqlParameter("@codeTipper", codeTipper)).ToString());
        }

        public static DataSet GetTipper(string codeTipper)
        {
            return DIClassLib.DbHelpers.SqlHelper.ExecuteDataset(CAMP3, "getTipper", new SqlParameter("@codeTipper", codeTipper));
        }
        
        #endregion
        #endregion


        #region Conference

        public static DataSet GetInfoChannels()
        {
            return DIClassLib.DbHelpers.SqlHelper.ExecuteDataset("Conference", "GetInfoChannels", null);
        }

        public static DataSet GetInfoChannelsEn()
        {
            return DIClassLib.DbHelpers.SqlHelper.ExecuteDataset("Conference", "GetInfoChannelsEn", null);
        }

        public static DataSet GetInfoChannelsForConference(int conferenceId)
        {
            return DIClassLib.DbHelpers.SqlHelper.ExecuteDataset("Conference", "GetInfoChannelsForConference", new SqlParameter("@conferenceid", conferenceId));
        }

        public static DataSet GetConference(int pageId)
        {
            return DIClassLib.DbHelpers.SqlHelper.ExecuteDataset("Conference", "GetConference", new SqlParameter("@epiPageId", pageId));
        }

        public static int InsertConference(int pageId, string name, int price)
        {
            SqlParameter[] sqlParameters = new SqlParameter[] 
                    {
                        new SqlParameter("@epipageid", pageId),
                        new SqlParameter("@name", name),
                        new SqlParameter("@price", price)
                    };

            return int.Parse(DIClassLib.DbHelpers.SqlHelper.ExecuteScalarParam("Conference", "InsertConference", sqlParameters).ToString());
        }

        public static void UpdateConferencePrice(int conferenceId, int price)
        {
            SqlParameter[] sqlParameters = new SqlParameter[] 
                    {
                        new SqlParameter("@conferenceid", conferenceId),
                        new SqlParameter("@price", price)
                    };

            DIClassLib.DbHelpers.SqlHelper.ExecuteNonQuery("Conference", "UpdateConferencePrice", sqlParameters);
        }

        public static DataSet GetEvents(int conferenceId)
        {
            return DIClassLib.DbHelpers.SqlHelper.ExecuteDataset("Conference", "GetEvents", new SqlParameter("@conferenceid", conferenceId));
        }

        public static void InsertEvent(int conferenceId, string name, string price, DateTime date)
        {
            SqlParameter[] sqlParameters = new SqlParameter[] 
                    {
                        new SqlParameter("@conferenceid", conferenceId),
                        new SqlParameter("@name", name),
                        new SqlParameter("@price", price),
                        new SqlParameter("@eventdate", date)
                    };

            DIClassLib.DbHelpers.SqlHelper.ExecuteNonQuery("Conference", "InsertEvent", sqlParameters);
        }

        public static void UpdateEvent(int eventId, string name,DateTime date)
        {
            SqlParameter[] sqlParameters = new SqlParameter[] 
                    {
                        new SqlParameter("@eventid", eventId),
                        new SqlParameter("@name", name),
                        new SqlParameter("@eventdate", date)
                    };

            DIClassLib.DbHelpers.SqlHelper.ExecuteNonQuery("Conference", "UpdateEvent", sqlParameters);
        }

        public static void DeleteEvent(int eventId)
        {
            SqlParameter[] sqlParameters = new SqlParameter[] 
                    {
                        new SqlParameter("@eventid", eventId)
                    };

            DIClassLib.DbHelpers.SqlHelper.ExecuteNonQuery("Conference", "DeleteEvent", sqlParameters);
        }

        public static DataSet GetEventTimes(int eventId)
        {
            return DIClassLib.DbHelpers.SqlHelper.ExecuteDataset("Conference", "GetEventTimes", new SqlParameter("@eventid", eventId));
        }

        public static void InsertTime(int eventId, string timeStart, string timeEnd)
        {
            SqlParameter[] sqlParameters = new SqlParameter[] 
                    {
                        new SqlParameter("@eventid", eventId),
                        new SqlParameter("@timeStart", timeStart),
                        new SqlParameter("@timeEnd", timeEnd)
                    };

            DIClassLib.DbHelpers.SqlHelper.ExecuteNonQuery("Conference", "InsertTime", sqlParameters);
        }

        public static void UpdateTime(int timeId, string timeStart, string timeEnd)
        {
            SqlParameter[] sqlParameters = new SqlParameter[] 
                    {
                        new SqlParameter("@timeid", timeId),
                        new SqlParameter("@timeStart", timeStart),
                        new SqlParameter("@timeEnd", timeEnd)
                    };

            DIClassLib.DbHelpers.SqlHelper.ExecuteNonQuery("Conference", "UpdateTime", sqlParameters);
        }

        public static void DeleteTime(int timeId)
        {
            SqlParameter[] sqlParameters = new SqlParameter[]
                    {
                        new SqlParameter("@timeid", timeId)
                    };

            DIClassLib.DbHelpers.SqlHelper.ExecuteNonQuery("Conference", "DeleteTime", sqlParameters);
        }

        public static DataSet GetEventActivities(int timeId)
        {
            return DIClassLib.DbHelpers.SqlHelper.ExecuteDataset("Conference", "GetEventActivities", new SqlParameter("@timeid", timeId));
        }

        /*
        public static DataSet GetEventActivitiesWithCount(int conferenceId)
        {
            return DIClassLib.DbHelpers.SqlHelper.ExecuteDataset("Conference", "GetEventActivitiesWithCount", new SqlParameter("@conferenceId", conferenceId));
        }
        */
        public static DataSet GetEventActivitiesForPerson(string personId)
        {
            return DIClassLib.DbHelpers.SqlHelper.ExecuteDataset("Conference", "GetEventActivitiesForPerson", new SqlParameter("@personId", personId));
        }

        public static void InsertActivity(int timeId, string name, string maxparticipants)
        {
            SqlParameter[] sqlParameters = new SqlParameter[] 
                    {
                        new SqlParameter("@timeid", timeId),
                        new SqlParameter("@name", name),
                        new SqlParameter("@maxparticipants", maxparticipants)
                    };

            DIClassLib.DbHelpers.SqlHelper.ExecuteNonQuery("Conference", "InsertActivity", sqlParameters);
        }

        public static void UpdateActivity(int activityId, string name, string maxparticipants)
        {
            SqlParameter[] sqlParameters = new SqlParameter[] 
                    {
                        new SqlParameter("@activityid", activityId),
                        new SqlParameter("@name", name),
                        new SqlParameter("@maxparticipants", maxparticipants)
                    };

            DIClassLib.DbHelpers.SqlHelper.ExecuteNonQuery("Conference", "UpdateActivity", sqlParameters);
        }

        public static void DeleteActivity(int activityId)
        {
            SqlParameter[] sqlParameters = new SqlParameter[] 
                    {
                        new SqlParameter("@activityid", activityId)
                    };

            DIClassLib.DbHelpers.SqlHelper.ExecuteNonQuery("Conference", "DeleteActivity", sqlParameters);
        }

        public static int InsertPerson(string firstName, string lastName, string company, string title, string orgNo, string phone, string email, string invoiceAddress, string invoiceReference, string zip, string city, string code, string infoChannel,int conferenceId,Guid personGuid)
        {
            SqlParameter[] sqlParameters = new SqlParameter[] 
            {
                new SqlParameter("@firstName", firstName),
                new SqlParameter("@lastName", lastName),
                new SqlParameter("@company", company),
                new SqlParameter("@title", title),
                new SqlParameter("@orgno", orgNo),
                new SqlParameter("@phone", phone),
                new SqlParameter("@email", email),
                new SqlParameter("@invoiceaddress", invoiceAddress),
                new SqlParameter("@invoicereference", invoiceReference),
                new SqlParameter("@zip", zip),
                new SqlParameter("@city", city),
                new SqlParameter("@code", code),
                new SqlParameter("@infochannel", infoChannel),
                new SqlParameter("@conferenceid", conferenceId),
                new SqlParameter("@personguid", personGuid),
            };

            return int.Parse(DIClassLib.DbHelpers.SqlHelper.ExecuteScalarParam("Conference", "InsertPerson2", sqlParameters).ToString());
        }
        public static DataSet GetPersonByGuid(Guid personGuid)
        {
            return DIClassLib.DbHelpers.SqlHelper.ExecuteDataset("Conference", "GetPersonByGuid", new SqlParameter("@personGuid", personGuid));
        }

        public static void InsertPersonInActivity(int personId, int activityId)
        {
            SqlParameter[] sqlParameters = new SqlParameter[] 
                {
                    new SqlParameter("@personid", personId),
                    new SqlParameter("@activityid", activityId)
                };

            DIClassLib.DbHelpers.SqlHelper.ExecuteNonQuery("Conference", "InsertPersonInActivity", sqlParameters);
        }

        public static void InsertPersonInConferenceMail(int personId, int conferenceId)
        {
            SqlParameter[] sqlParameters = new SqlParameter[] 
                {
                    new SqlParameter("@personid", personId),
                    new SqlParameter("@conferenceid", conferenceId)
                };

            DIClassLib.DbHelpers.SqlHelper.ExecuteNonQuery("Conference", "InsertPersonInConferenceMail", sqlParameters);
        }

        public static void DeletePersonInActivity(int personId, int activityId)
        {
            SqlParameter[] sqlParameters = new SqlParameter[] 
                {
                    new SqlParameter("@personid", personId),
                    new SqlParameter("@activityid", activityId)
                };

            DIClassLib.DbHelpers.SqlHelper.ExecuteNonQuery("Conference", "DeletePersonInActivity", sqlParameters);
        }



        public static void InsertToPayTrans(int personid, string paymentmethod, int amountToPay)
        {
            SqlParameter[] sqlParameters = new SqlParameter[] 
                {
                    new SqlParameter("@personid", personid),
                    new SqlParameter("@method", paymentmethod),
                    new SqlParameter("@price", amountToPay),
                    new SqlParameter("@paymentreceived", false)
                };

            DIClassLib.DbHelpers.SqlHelper.ExecuteNonQuery("Conference", "InsertPayTrans", sqlParameters);
        }

        public static void InsertPdfDownloadLog(int conferenceId, string name, string email, string phone)
        {
            SqlParameter[] sqlParameters = new SqlParameter[] 
                    {
                        new SqlParameter("@conferenceid", conferenceId),
                        new SqlParameter("@name", name),
                        new SqlParameter("@email", email),
                        new SqlParameter("@phone", phone)
                    };
            DIClassLib.DbHelpers.SqlHelper.ExecuteNonQuery("Conference", "InsertPdfDownloadLog", sqlParameters);
        }

        public static void DeletePerson(int personId)
        {
            SqlParameter[] sqlParameters = new SqlParameter[] 
            {
                new SqlParameter("@personid", personId)
            };

            DIClassLib.DbHelpers.SqlHelper.ExecuteNonQuery("Conference", "DeletePerson", sqlParameters);
        }

        public static DataSet GetActivitiesForConference(int conferenceId)
        {
            return DIClassLib.DbHelpers.SqlHelper.ExecuteDataset("Conference", "GetEventActivitiesForConference", new SqlParameter("@conferenceid", conferenceId));
        }

        public static DataSet GetAllConferences()
        {
            return DIClassLib.DbHelpers.SqlHelper.ExecuteDataset("Conference", "GetAllConferences", null);
        }

        public static DataSet GetPdfDownloadLog(int conferenceId)
        {
            return DIClassLib.DbHelpers.SqlHelper.ExecuteDataset("Conference", "GetPdfDownloadLog", new SqlParameter("conferenceid", conferenceId));
        }

        public static void UpdateConfPdfComment(int pdfDownloadLogId, string comment)
        {
            var sqlParameters = new SqlParameter[] 
            {
                new SqlParameter("@pdfdownloadlogid", pdfDownloadLogId),
                new SqlParameter("@comment", comment)
            };

            SqlHelper.ExecuteNonQuery("Conference", "UpdatePdfComment", sqlParameters);
        }

        public static DataSet GetPersonsForConference(int conferenceId, int eventId, int activityId)
        {
            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@conferenceid",conferenceId),
                new SqlParameter("@eventid", eventId),
                new SqlParameter("@activityid", activityId)
            };

            return DIClassLib.DbHelpers.SqlHelper.ExecuteDatasetParam("Conference", "GetPersons2", sqlParameters);
        }

        public static void UpdatePerson(int personid, string firstname, string lastname, string company, string title, string orgno, string phone, string email, string invoiceaddress, string invoicereference, string zip, string city, string code, string price)
        {
            SqlParameter[] sqlParameters = new SqlParameter[] 
            {
                new SqlParameter("@personid", personid),
                new SqlParameter("@firstname",firstname),
                new SqlParameter("@lastname",lastname),
                new SqlParameter("@company",company),
                new SqlParameter("@title",title),
                new SqlParameter("@orgno",orgno),
                new SqlParameter("@phone",phone),
                new SqlParameter("@email",email),
                new SqlParameter("@invoiceaddress",invoiceaddress),
                new SqlParameter("@invoicereference",invoicereference),
                new SqlParameter("@zip",zip),
                new SqlParameter("@city",city),
                new SqlParameter("@code",code),
                new SqlParameter("@price",price)
            };

            DIClassLib.DbHelpers.SqlHelper.ExecuteNonQuery("Conference", "UpdatePerson", sqlParameters);
        }


        public static int EpiJob_Conf_GetNewBatchId()
        {
            return int.Parse(SqlHelper.ExecuteScalarParam("Conference", "EpiJob_GetNewBatchId", null).ToString());
        }

        public static void EpiJob_Conf_InsertConfItem(ConferenceEpiDataInput conf)
        {
            SqlParameter[] sqlParameters = new SqlParameter[] 
                {
                    new SqlParameter("@batchId", conf.BatchId),
                    new SqlParameter("@epiPageId", conf.EpiPageId),
                    new SqlParameter("@headline", conf.Headline),
                    new SqlParameter("@shortIntroTextHtml", conf.ShortIntroTextHtml),
                    new SqlParameter("@place", conf.Place),
                    new SqlParameter("@placeMapUrl", conf.PlaceMapUrl),
                    new SqlParameter("@conferencePageUrl", conf.ConferencePageUrl),
                    new SqlParameter("@dateConferenceStart", conf.DateConferenceStart)
                };

            SqlHelper.ExecuteNonQuery("Conference", "EpiJob_InsertConfItem", sqlParameters);
        }

        public static DataSet EpiJob_Conf_GetUpcomingConferences(int numTopItems)
        {
            return SqlHelper.ExecuteDataset("Conference", "EpiJob_GetUpcomingConferences", new SqlParameter("@topItems", numTopItems));
        }
        #endregion


        #region Gasell

        public static void InsertAndUpdateGasellObject(int epiPageId, string city, DateTime date)
        {
            SqlParameter[] sqlParameters = new SqlParameter[] 
                    {
                       new SqlParameter("@epiPageId ", epiPageId ), 
                       new SqlParameter("@city ", city ), 
                       new SqlParameter("@startDate ", date )
                    };

            DIClassLib.DbHelpers.SqlHelper.ExecuteNonQuery("DISEMiscDB", "EpiSaveGasellConference", sqlParameters);
        }

        
        public static void UpdateGasellUserPayInfo(int gasellOrderID, string payInfo)
        {
            SqlParameter[] sqlParameters = new SqlParameter[] 
                    {
                       new SqlParameter("@id ", gasellOrderID ), 
                       new SqlParameter("@payInfo ", payInfo )
                    };

            DIClassLib.DbHelpers.SqlHelper.ExecuteNonQuery("DISEMiscDB", "EpiUpdateOrderGasellConfPayInfo", sqlParameters);
        }




        public static DataSet GetGasellPersons(int epiPageId)
        {
            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@epiPageId", epiPageId)
            };

            return DIClassLib.DbHelpers.SqlHelper.ExecuteDatasetParam("DISEMiscDB", "EpiGetGasellPersons", sqlParameters);
        }

        
        public static int InsertGasellPerson(int epiPageId, string firstname, string lastname, string title, string company, string address,
                                            string zip, string city, string phone, string email, string bransch, string employees,
                                            bool isGasellComp, string code, string invoiceAddress, string invoiceZip, string invoiceCity,
                                            string invoiceRef, string orgno, string payInfo)
        {
            SqlParameter[] sqlParameters = new SqlParameter[] 
                    {
                        new SqlParameter("@epiPageId", epiPageId),
                        new SqlParameter("@firstname", firstname),
                        new SqlParameter("@lastname", lastname),
                        new SqlParameter("@title", title),    
                        new SqlParameter("@company", company),
                        new SqlParameter("@address", address),
                        new SqlParameter("@zipcode", zip),
                        new SqlParameter("@city", city),
                        new SqlParameter("@phone", phone),
                        new SqlParameter("@mail", email),
                        new SqlParameter("@bransch", bransch),
                        new SqlParameter("@employees", employees),
                        new SqlParameter("@gasellCompany", isGasellComp),
                        new SqlParameter("@code", code),
                        new SqlParameter("@invoiceAddress", invoiceAddress),
                        new SqlParameter("@invoiceZipcode", invoiceZip),
                        new SqlParameter("@invoiceCity", invoiceCity),
                        new SqlParameter("@invoiceRef", invoiceRef),
                        new SqlParameter("@orgno", orgno),
                        new SqlParameter("@payInfo", payInfo)
                    };

            return int.Parse(DIClassLib.DbHelpers.SqlHelper.ExecuteScalarParam("DISEMiscDB", "EpiInsertGasellPerson", sqlParameters).ToString());
        }

        public static void UpdateGasellPerson(int personid, string firstname, string lastname, string title, string company, string address,
                                            string zip, string city, string phone, string email, string bransch, string employees,
                                            bool isGasellComp, string code, string invoiceAddress, string invoiceZip, string invoiceCity,
                                            string invoiceRef, string orgno, string payInfo, bool isCanceled)
        {
            SqlParameter[] sqlParameters = new SqlParameter[] 
            {
                new SqlParameter("@id", personid),
                new SqlParameter("@firstname", firstname),
                new SqlParameter("@lastname", lastname),
                new SqlParameter("@title", title),    
                new SqlParameter("@company", company),
                new SqlParameter("@address", address),
                new SqlParameter("@zipcode", zip),
                new SqlParameter("@city", city),
                new SqlParameter("@phone", phone),
                new SqlParameter("@mail", email),
                new SqlParameter("@bransch", bransch),
                new SqlParameter("@employees", employees),
                new SqlParameter("@gasellCompany", isGasellComp),
                new SqlParameter("@code", code),
                new SqlParameter("@invoiceAddress", invoiceAddress),
                new SqlParameter("@invoiceZipcode", invoiceZip),
                new SqlParameter("@invoiceCity", invoiceCity),
                new SqlParameter("@invoiceRef", invoiceRef),
                new SqlParameter("@orgno", orgno),
                new SqlParameter("@payInfo", payInfo),    
                new SqlParameter("@canceled", isCanceled)
            };

            DIClassLib.DbHelpers.SqlHelper.ExecuteNonQuery("DISEMiscDB", "EpiUpdateGasellPerson", sqlParameters);
        }

        public static void DeleteGasellPerson(int id)
        {
            SqlParameter[] sqlParameters = new SqlParameter[] 
            {
                new SqlParameter("@id", id)
            };

            DIClassLib.DbHelpers.SqlHelper.ExecuteNonQuery("DISEMiscDB", "EpiDeleteGasellPerson", sqlParameters);
        }

        public static DateTime GetGasellWelcomeMailDate(int epiPageId)
        {
            DateTime dt = DateTime.MinValue;

            try
            {
                SqlParameter prm = new SqlParameter("@epiPageId", epiPageId);
                object obj = SqlHelper.ExecuteScalar("DISEMiscDB", "GetGasellWelcomeMailDate", prm);

                if (obj != null)
                    DateTime.TryParse(obj.ToString(), out dt);

                return dt;
            }
            catch (Exception ex)
            {
                new Logger("GetGasellWelcomeMailDate() - failed for epiPageId:" + epiPageId, ex.ToString());
            }

            return dt;
        }

        public static DataSet GetGasellSignedUpMailAddresses(int epiPageId)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]{
                    new System.Data.SqlClient.SqlParameter("@epiPageId", epiPageId)
                    };

                return SqlHelper.ExecuteDatasetParam("DISEMiscDB", "GetGasellSignedUpMailAddresses", sqlParameters);
            }
            catch (Exception ex)
            {
                new Logger("GetGasellSignedUpMailAddresses() failed for epiPageId:" + epiPageId, ex.ToString());
            }

            return null;
        }

        public static void InsertGasellWelcomeMailRow(int epiPageId)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]{
                    new SqlParameter("@epiPageid", epiPageId)
                };

                DIClassLib.DbHelpers.SqlHelper.ExecuteNonQuery("DISEMiscDB", "InsertGasellWelcomeMailRow", sqlParameters);
            }
            catch (Exception ex)
            {
                new Logger("InsertGasellWelcomeMailRow() - failed for epiPageId:" + epiPageId, ex.ToString());
            }
        }

        #endregion


        #region card payment

        public static int GetPayTransCusRefNo()
        {
            try
            {
                return int.Parse(SqlHelper.ExecuteScalar("DisePren", "CardPayCreateCustomerRefNo", null).ToString());
            }
            catch (Exception ex)
            {
                new Logger("GetPayTransCusRefNo() failed", ex.ToString());
                throw;
            }
        }

        public static void SaveDataBeforeCardPaymentNets(int customerRefNo, string merchantId, string currency, int amountInOren,
                                                         int vatInOren, string paymentMethod, string goodsDescription,
                                                         string comment, string consumerName, string email, string transactionId,
                                                         long? invoiceNumber)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]{
                    new SqlParameter("@Customer_refno", customerRefNo), 
                    new SqlParameter("@Merchant_id", merchantId), 
                    new SqlParameter("@Currency", currency), 
                    new SqlParameter("@Amount", amountInOren), 
                    new SqlParameter("@VAT", vatInOren),
                    new SqlParameter("@Payment_method", paymentMethod ?? ""), 
                    new SqlParameter("@Goods_description", goodsDescription ?? ""), 
                    new SqlParameter("@Comment", comment ?? ""), 
                    new SqlParameter("@Consumer_name", consumerName ?? ""), 
                    new SqlParameter("@Email_address", email ?? ""),
                    new SqlParameter("@Transaction_id", transactionId ?? ""),
                    new SqlParameter("@Invoice_number", invoiceNumber)
                    };

                DIClassLib.DbHelpers.SqlHelper.ExecuteNonQuery("DisePren", "CardPaySaveBeforeNetsPage", sqlParameters);
            }
            catch (Exception ex)
            {
                new DIClassLib.DbHelpers.Logger("SaveDataBeforeCardPaymentNets() - failed", ex.ToString());
                throw ex;
            }
        }

        public static void SaveDataAfterCardPaymentNets(int customerRefNo, string status, string statusCode, string panHash, string issuer)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]{
                    new SqlParameter("@Customer_refno", customerRefNo), 
                    new SqlParameter("@Status", status), 
                    new SqlParameter("@Status_code", statusCode),
                    new SqlParameter("@AurigaSubsId", panHash),
                    new SqlParameter("@Card_type", issuer)
                    };

                if (string.IsNullOrEmpty(panHash))
                    sqlParameters[3] = new SqlParameter("@AurigaSubsId", DBNull.Value);

                DIClassLib.DbHelpers.SqlHelper.ExecuteNonQuery("DisePren", "CardPaySaveAfterNetsPage2", sqlParameters);
            }
            catch (Exception ex)
            {
                new DIClassLib.DbHelpers.Logger("SaveDataAfterCardPaymentNets() - failed", ex.ToString());
                throw ex;
            }
        }

        public static bool InsertPayTransFromDi(int diPayTransRefNo, int merchantId, string currency, int amountInOren,
            int vatInOren, string paymentMethod, DateTime purchaseDate, string goodsDescription,
            string comment, string consumerName, string email, string cardType, string transactionId,
            string status, string statusCode, DateTime finishDate)
        {
            try
            {
                var generatedCustomerRefno = GetPayTransCusRefNo();
                var sqlParameters = new SqlParameter[]{
                    new SqlParameter("@Customer_refno", generatedCustomerRefno), 
                    new SqlParameter("@diPayTransRefNo", diPayTransRefNo),
                    new SqlParameter("@merchantId", merchantId),
                    new SqlParameter("@currency", currency),
                    new SqlParameter("@amountInOren", amountInOren),
                    new SqlParameter("@vatInOren", vatInOren),
                    new SqlParameter("@paymentMethod", paymentMethod),
                    new SqlParameter("@purchaseDate", purchaseDate),
                    new SqlParameter("@goodsDescription", goodsDescription),
                    new SqlParameter("@comment", comment),
                    new SqlParameter("@consumerName", consumerName),
                    new SqlParameter("@email", email),
                    new SqlParameter("@cardType", cardType),
                    new SqlParameter("@transactionId", transactionId),
                    new SqlParameter("@Status", status), 
                    new SqlParameter("@Status_code", statusCode),
                    new SqlParameter("@finishDate", finishDate)
                    };

                SqlHelper.ExecuteNonQuery("DisePren", "InsertPayTransFromDi", sqlParameters);
            }
            catch (Exception ex)
            {
                new Logger("InsertPayTransFromDi() - failed", ex.ToString());
                return false;
            }
            return true;
        }

        /// <summary>
        /// Update info in DisePren / PayTrans
        /// </summary>
        public static void SaveDataBeforeCardPayment(int customerRefNo, string merchantId, string currency, int amountInOren,
                                                     int vatInOren, string paymentMethod, string mac, string goodsDescription,
                                                     string comment, string consumerName, string email, long? invoiceNumber, 
                                                     string aurigaSubsId)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]{
                    new System.Data.SqlClient.SqlParameter("@Customer_refno", customerRefNo), 
                    new System.Data.SqlClient.SqlParameter("@Merchant_id", merchantId), 
                    new System.Data.SqlClient.SqlParameter("@Currency", currency), 
                    new System.Data.SqlClient.SqlParameter("@Amount", amountInOren), 
                    new System.Data.SqlClient.SqlParameter("@VAT", vatInOren),
                    new System.Data.SqlClient.SqlParameter("@Payment_method", paymentMethod ?? ""), 
                    new System.Data.SqlClient.SqlParameter("@MAC", mac),
                    new System.Data.SqlClient.SqlParameter("@Goods_description", goodsDescription ?? ""), 
                    new System.Data.SqlClient.SqlParameter("@Comment", comment ?? ""), 
                    new System.Data.SqlClient.SqlParameter("@Consumer_name", consumerName ?? ""), 
                    new System.Data.SqlClient.SqlParameter("@Email_address", email ?? ""),
                    new System.Data.SqlClient.SqlParameter("@Invoice_number", invoiceNumber),
                    new System.Data.SqlClient.SqlParameter("@AurigaSubsId", aurigaSubsId)
                    };

                if (aurigaSubsId == null)
                    sqlParameters[12] = new SqlParameter("@AurigaSubsId", DBNull.Value);

                DIClassLib.DbHelpers.SqlHelper.ExecuteNonQuery("DisePren", "CardPaySaveBeforePostensPage3", sqlParameters);
            }
            catch (Exception ex)
            {
                new DIClassLib.DbHelpers.Logger("SaveDataBeforeCardPayment() - failed", ex.ToString());
                throw ex;
            }
        }

        /// <summary>
        /// Update info in DisePren / PayTrans
        /// </summary>
        public static void SaveDataAfterCardPayment(int customerRefNo, string transactionId, string status, string statusCode, string cardType)
        {
            try
            {
                //long transId = 0;
                //long.TryParse(transactionID, out transId);

                SqlParameter[] sqlParameters = new SqlParameter[]{
                    new SqlParameter("@Customer_refno", customerRefNo), 
                    new SqlParameter("@Transaction_id", transactionId), 
                    new SqlParameter("@Status", status), 
                    new SqlParameter("@Status_code", statusCode), 
                    new SqlParameter("@Card_type", cardType)
                    };

                if(cardType == null)
                    sqlParameters[4] = new SqlParameter("@Card_type", DBNull.Value);

                DIClassLib.DbHelpers.SqlHelper.ExecuteNonQuery("DisePren", "CardPaySaveAfterNetsPage", sqlParameters);
            }
            catch (Exception ex)
            {
                new DIClassLib.DbHelpers.Logger("SaveDataAfterCardPayment() - failed", ex.ToString());
                throw ex;
            }
        }

        
        public static void AppendToPayTransComment(int customerRefNo, string info)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]{
                    new SqlParameter("@Customer_refno", customerRefNo), 
                    new SqlParameter("@Info", info)
                    };

                DIClassLib.DbHelpers.SqlHelper.ExecuteNonQuery("DisePren", "AppendToPayTransComment", sqlParameters);
            }
            catch (Exception ex)
            {
                new DIClassLib.DbHelpers.Logger("AppendToPayTransComment() - failed for cusRefNo:" + customerRefNo + ", info:" + info, ex.ToString());
                throw ex;
            }
        }


        public static DataSet GetPayTransAmountAndVat(string transactionId)
        {
            try
            {
                return DIClassLib.DbHelpers.SqlHelper.ExecuteDataset("DISEPren", "getPayTransAmountAndVat", new SqlParameter("Transaction_id", transactionId));
            }
            catch (Exception ex)
            {
                new DIClassLib.DbHelpers.Logger("GetPayTransAmountAndVat() failed for transactionID: " + transactionId, ex.ToString());
            }

            return null;
        }


        
        public static void SaveCardPayment(int customerRefNo, CardPaymentDataHolder dh)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[] 
                {
                    new SqlParameter("@customerRefno", customerRefNo),
                    new SqlParameter("@epiPageId", dh.EpiPageId),
                    new SqlParameter("@diDepartment", dh.DiDepartment),
                    new SqlParameter("@itemDescr", dh.ItemDescr),
                    new SqlParameter("@numItems", dh.NumItems),
                    new SqlParameter("@priceIncVatTotal", dh.PriceCalc.PriceIncVat),
                    new SqlParameter("@priceExVatTotal", dh.PriceCalc.PriceExVat),
                    new SqlParameter("@vatPct", dh.PriceCalc.VatPct),
                    new SqlParameter("@firstName", dh.FirstName),
                    new SqlParameter("@lastName", dh.LastName),
                    new SqlParameter("@phoneMobile", dh.PhoneMobile),
                    new SqlParameter("@email", dh.Email),
                    new SqlParameter("@isStreetAddress", dh.IsStreetAddress),
                    new SqlParameter("@company", (dh.Company == null) ? "" : dh.Company),
                    new SqlParameter("@companyNum", (dh.CompanyNum == null) ? "" : dh.CompanyNum),
                    new SqlParameter("@street", (dh.Street == null) ? "" : dh.Street),
                    new SqlParameter("@streetNum", (dh.StreetNum == null) ? "" : dh.StreetNum),
                    new SqlParameter("@entrance", (dh.Entrance == null) ? "" : dh.Entrance),
                    new SqlParameter("@stairsNum", (dh.StairsNum == null) ? "" : dh.StairsNum),
                    new SqlParameter("@apartmentNum", (dh.ApartmentNum == null) ? "" : dh.ApartmentNum),
                    new SqlParameter("@zip", (dh.Zip == null) ? "" : dh.Zip),
                    new SqlParameter("@city", (dh.City == null) ? "" : dh.City),
                    new SqlParameter("@careOf", (dh.CareOf == null) ? "" : dh.CareOf),
                    new SqlParameter("@stopOrBox", (dh.StopOrBox == null) ? "" : dh.StopOrBox),
                    new SqlParameter("@stopOrBoxNum", (dh.StopOrBoxNum == null) ? "" : dh.StopOrBoxNum)
                };

                DIClassLib.DbHelpers.SqlHelper.ExecuteNonQuery("DagensIndustriMISC", "InsertCardPayment", sqlParameters);
            }
            catch (Exception ex)
            {
                new Logger("SaveCardPayment() failed", ex.ToString());
            }
        }

        public static int GetNumSoldCardPayItems(int epiPageId)
        {
            try
            {
                return int.Parse(DIClassLib.DbHelpers.SqlHelper.ExecuteScalar("DagensIndustriMISC", "GetNumSoldCardPayItems", new SqlParameter("@epiPageId", epiPageId)).ToString());
            }
            catch (Exception ex)
            {
                //new Logger("GetNumSoldCardPayItems() failed", ex.ToString());
                return 0;
            }
        }

        #endregion


        #region DiGold
        public static void UpdateUserJoinGuld(long cusno, string email, string birthNo)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[] {
                            new System.Data.SqlClient.SqlParameter("@cusno", cusno), 
                            new System.Data.SqlClient.SqlParameter("@birthNo ", birthNo),
                            new System.Data.SqlClient.SqlParameter("@email ", email)
                    };

                DIClassLib.DbHelpers.SqlHelper.ExecuteNonQuery("DisePren", "UpdateCustomerJoinGold", sqlParameters);

            }
            catch (Exception ex)
            {
                new DIClassLib.DbHelpers.Logger("UpdateUserJoinGuld() - failed", ex.ToString());
                throw ex;
            }
        }

        public static void SaveDiGoldNameChange(long cusno, string firstName, string lastName)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[] {
                            new System.Data.SqlClient.SqlParameter("@cusno", cusno), 
                            new System.Data.SqlClient.SqlParameter("@firstName", firstName),
                            new System.Data.SqlClient.SqlParameter("@lastName ", lastName)
                    };

                DIClassLib.DbHelpers.SqlHelper.ExecuteNonQuery("DagensIndustriMISC", "SaveDiGoldNameChange", sqlParameters);
            }
            catch (Exception ex)
            {
                new DIClassLib.DbHelpers.Logger("SaveDiGoldNameChange() - failed", ex.ToString());
                throw ex;
            }
        }

        public static void InsertDiGoldOffer(long cusno, int epiPageId, string @offerName, string company, string name, string @streetAddress, string houseNo, string zipCode, string city, string careOf)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[] {
                            new System.Data.SqlClient.SqlParameter("@cusno", cusno), 
                            new System.Data.SqlClient.SqlParameter("@epiPageId", epiPageId), 
                            new System.Data.SqlClient.SqlParameter("@offerName", offerName),
                            new System.Data.SqlClient.SqlParameter("@company", company),
                            new System.Data.SqlClient.SqlParameter("@name", name),
                            new System.Data.SqlClient.SqlParameter("@streetAddress", streetAddress),
                            new System.Data.SqlClient.SqlParameter("@houseNo", houseNo),
                            new System.Data.SqlClient.SqlParameter("@zipCode", zipCode),
                            new System.Data.SqlClient.SqlParameter("@city", city),
                            new System.Data.SqlClient.SqlParameter("@careOf", careOf)
                    };

                DIClassLib.DbHelpers.SqlHelper.ExecuteNonQuery("DagensIndustriMISC", "InsertDiGoldOffer3", sqlParameters);
            }
            catch (Exception ex)
            {
                new DIClassLib.DbHelpers.Logger("InsertDiGoldOffer() - failed", ex.ToString());
                throw ex;
            }
        }

        public static DataSet GetDiGoldOfferByCusno(long cusno, string offerName)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[] {
                            new System.Data.SqlClient.SqlParameter("@cusno", cusno), 
                            new System.Data.SqlClient.SqlParameter("@offerName", offerName)
                    };

                return DIClassLib.DbHelpers.SqlHelper.ExecuteDatasetParam("DagensIndustriMISC", "GetDiGoldOfferByCusno", sqlParameters);
            }
            catch (Exception ex)
            {
                new DIClassLib.DbHelpers.Logger("GetDiGoldOfferByCusno() - failed", ex.ToString());
                throw ex;
            }
        }
        #endregion

        
        #region DiGoldOfferCust (trying to get custs to join gold by nice offer)
        public static DataSet GetDiGoldOfferCust(Guid code)
        {
            SqlParameter[] sqlParameters = new SqlParameter[] { new SqlParameter("@code", code) };

            return DIClassLib.DbHelpers.SqlHelper.ExecuteDatasetParam("DagensIndustriMISC", "GetDiGoldOfferCustByCode", sqlParameters);
        }

        public static void SetDiGoldOfferCustDate(string code, bool setDateVisitedGoldOffer, bool setDateTookGoldOffer)
        {
            SqlParameter[] sqlParameters = new SqlParameter[]{
                    new SqlParameter("@code", code), 
                    new SqlParameter("@setDateVisited", setDateVisitedGoldOffer),
                    new SqlParameter("@setDateTook", setDateTookGoldOffer)
                };

            DIClassLib.DbHelpers.SqlHelper.ExecuteNonQuery("DagensIndustriMISC", "SetDiGoldOfferCustDate", sqlParameters);
        }

        public static DataSet GetDateTookDiGoldOffer(long cusno, int epiPageId)
        {
            SqlParameter[] sqlParameters = new SqlParameter[] 
            { 
                new SqlParameter("@cusno", cusno),
                new SqlParameter("@epiPageId", epiPageId)
            };

            return DIClassLib.DbHelpers.SqlHelper.ExecuteDatasetParam("DagensIndustriMISC", "GetDateTookDiGoldOffer", sqlParameters);
        }
        
        #endregion


        #region Customer

        public static DataSet GetCustomer(string userName)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[] { new System.Data.SqlClient.SqlParameter("@Login", userName) };
                return DIClassLib.DbHelpers.SqlHelper.ExecuteDatasetParam("DisePren", "getUserInfo", sqlParameters);
            }
            catch (Exception ex)
            {
                new DIClassLib.DbHelpers.Logger("GetUserInfo() - failed", ex.ToString());
                throw ex;
            }
        }

        public static void InsertCustomer(long cusno, string email, string birthNo)
        {
            string bn = (string.IsNullOrEmpty(birthNo)) ? "" : birthNo;

            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new SqlParameter("@cusno", cusno),
                    new SqlParameter("@userid", cusno.ToString()),
                    new SqlParameter("@passwd", string.Format("{0}_{1:HHmmss}", cusno, DateTime.Now)),
                    new SqlParameter("@email", email),
                    new SqlParameter("@birthNo", bn)
                    };

                SqlHelper.ExecuteNonQuery("DisePren", "InsertCustomer", sqlParameters);
            }
            catch (Exception ex)
            {
                string parameters = string.Format("cusno: {0}, email: {1}, birthNo: {2}", cusno, email, birthNo);
                new Logger("MsSqlHandler.DisePren.InsertCustomer() - failed for: " + parameters, ex.ToString());
                throw ex;
            }
        }

        public static void UpdateCustomer(long cusno, string userName, string password, string email, string birthNo)
        {
            string bn = (string.IsNullOrEmpty(birthNo)) ? "" : birthNo;

            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[] {
                            new SqlParameter("@cusno", cusno), 
                            new SqlParameter("@userid", userName),
                            new SqlParameter("@passwd", password),
                            new SqlParameter("@email", email),
                            new SqlParameter("@birthNo", bn)
                    };

                DIClassLib.DbHelpers.SqlHelper.ExecuteNonQuery("DisePren", "UpdateCustomer", sqlParameters);
            }
            catch (Exception ex)
            {
                string parameters = string.Format("cusno: {0}, userName: {1}, password: {2}, email: {3}, birthNo: {4}",
                                                    cusno, userName, password, email, birthNo);
                new DIClassLib.DbHelpers.Logger("UpdateCustomer() - failed for: " + parameters, ex.ToString());
                throw ex;
            }
        }

        /// <summary>
        /// if in-parameter is null or empty old value will be kept
        /// </summary>
        public static void UpdateCustomer(string userName, string newUserName, string newPassword, string newEmail)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[] {
                            new System.Data.SqlClient.SqlParameter("@Login", userName), 
                            new System.Data.SqlClient.SqlParameter("@NewUserid", newUserName),
                            new System.Data.SqlClient.SqlParameter("@NewPassword", newPassword),
                            new System.Data.SqlClient.SqlParameter("@NewEmail", newEmail)
                    };

                SqlHelper.ExecuteNonQuery("DisePren", "SetLogin", sqlParameters);
            }
            catch (Exception ex)
            {
                string parameters = string.Format("userName: {0}, newUserName: {1}, newPassword: {2}, newEmail: {3}", 
                                                   userName,      newUserName,      newPassword,      newEmail);
                new DIClassLib.DbHelpers.Logger("UpdateCustomer() - failed for: " + parameters, ex.ToString());
            }
        }



        public static void DeleteCustomerAndSubs(long cusno)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[] { new System.Data.SqlClient.SqlParameter("@cusno", cusno) };
                DIClassLib.DbHelpers.SqlHelper.ExecuteNonQuery("DisePren", "DeleteCustomerAndSubs", sqlParameters);
            }
            catch (Exception ex)
            {
                new DIClassLib.DbHelpers.Logger("DeleteCustomerAndSubs() - failed for: " + cusno.ToString(), ex.ToString());
                throw ex;
            }
        }

        public static string GetCustomerBirthNo(int cusno)
        {
            try
            {
                object o = new object();
                o = DIClassLib.DbHelpers.SqlHelper.ExecuteScalar("DisePren", "GetCustomerBirthNo", new System.Data.SqlClient.SqlParameter("@cusno", cusno));
                if (o != null)
                    return o.ToString();
            }
            catch (Exception ex)
            {
                new DIClassLib.DbHelpers.Logger("GetUserInfo() - failed for cusno: " + cusno.ToString(), ex.ToString());
            }

            return string.Empty;
        }

        #endregion


        #region Subscription
        public static void InsertSubscription(long subscriptionNo, long cusno, string productNo, string paperCode, DateTime expireDate, bool subsActive)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[] {
                            new System.Data.SqlClient.SqlParameter("@subsno", subscriptionNo), 
                            new System.Data.SqlClient.SqlParameter("@cusno", cusno),
                            new System.Data.SqlClient.SqlParameter("@productNo", productNo),
                            new System.Data.SqlClient.SqlParameter("@paperCode", paperCode),
                            new System.Data.SqlClient.SqlParameter("@expireDate", expireDate),
                            new System.Data.SqlClient.SqlParameter("@subsActive", subsActive ? 1 : 0)
                    };

                DIClassLib.DbHelpers.SqlHelper.ExecuteNonQuery("DisePren", "InsertSubscription", sqlParameters);
            }
            catch (Exception ex)
            {
                string parameters = string.Format("subsno: {0}, cusno: {1}, productNo: {2}, paperCode: {3}, expireDate: {4}, subsActive: {5}",
                                                    subscriptionNo, cusno, productNo, paperCode, expireDate, subsActive);
                new DIClassLib.DbHelpers.Logger("InsertSubscription() - failed for: " + parameters, ex.ToString());
                throw ex;
            }
        }

        internal static void InsertDeniedTrialSub(Subscription sub)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[] {
                        new SqlParameter("@sub", sub.ToString())
                    };

                SqlHelper.ExecuteNonQuery("DagensIndustriMISC", "InsertDeniedTrialSub", sqlParameters);
            }
            catch (Exception ex)
            {
                new Logger("InsertDeniedTrialSub() - failed", ex.ToString() + Environment.NewLine + Environment.NewLine + sub.ToString());
            }
        }
        #endregion


        #region Editons

        public static DateTime GetCurrentEditonDateWeekend()
        {
            return DateTime.Parse(SqlHelper.ExecuteScalar("DISEMiscDB", "Editions_GetCurrent_Weekend", null).ToString());
        }

        public static DateTime GetCurrentEditonDateDimension()
        {
            return DateTime.Parse(SqlHelper.ExecuteScalar("DISEMiscDB", "Editions_GetCurrent_Dimension", null).ToString());
        }

        public static DateTime CurrentIssueDateDiIdea()
        {
            DateTime dt = DateTime.MinValue;

            try { dt = DateTime.Parse(SqlHelper.ExecuteScalar("DISEMiscDB", "Editions_GetCurrent_DiIdea", null).ToString()); }
            catch { }
            
            return dt;
        }
        #endregion


        #region Reports

        public static int GetNumCustsInRole(int roleId)
        {
            try
            {
                return int.Parse(DIClassLib.DbHelpers.SqlHelper.ExecuteScalar("DisePren", "GetNumCustsInRole",  new System.Data.SqlClient.SqlParameter("@roleId", roleId)).ToString());
            }
            catch (Exception ex)
            {
                new DIClassLib.DbHelpers.Logger("GetNumCustsInRole() - failed", ex.ToString());
                throw ex;
            }
        }

        public static int GetEventsEntriesById(int eventId)
        {
            try
            {
                return int.Parse(DIClassLib.DbHelpers.SqlHelper.ExecuteScalar("DagensIndustriMISC", "CountEventEntries", new System.Data.SqlClient.SqlParameter("@eventId", eventId)).ToString());
            }
            catch (Exception ex)
            {
                new DIClassLib.DbHelpers.Logger("GetEventsEntriesById() - failed", ex.ToString());
                throw ex;
            }
        }

        public static int GetSimpleFormEntriesById(int epiPageID)
        {
            try
            {
                return int.Parse(DIClassLib.DbHelpers.SqlHelper.ExecuteScalar("DagensIndustriMISC", "CountSimpleFormEntries", new System.Data.SqlClient.SqlParameter("@epiPageID", epiPageID)).ToString());
            }
            catch (Exception ex)
            {
                new DIClassLib.DbHelpers.Logger("GetSimpleFormEntriesById() - failed", ex.ToString());
                throw ex;
            }
        }

        public static int GetSignUpEntriesById(int epiPageID)
        {
            try
            {
                return int.Parse(DIClassLib.DbHelpers.SqlHelper.ExecuteScalar("DagensIndustriMISC", "CountSignUpEntries", new System.Data.SqlClient.SqlParameter("@epiPageID", epiPageID)).ToString());
            }
            catch (Exception ex)
            {
                new DIClassLib.DbHelpers.Logger("GetSignUpEntriesById() - failed", ex.ToString());
                throw ex;
            }
        }

        public static int GetEmailSentToAddress(string toAddress)
        {
            try
            {
                return int.Parse(DIClassLib.DbHelpers.SqlHelper.ExecuteScalar("DagensIndustriMISC", "CountEmailSentToAddress", new System.Data.SqlClient.SqlParameter("@toAddress", toAddress)).ToString());
            }
            catch (Exception ex)
            {
                new DIClassLib.DbHelpers.Logger("GetEmailSentToAddress() - failed", ex.ToString());
                throw ex;
            }
        }
        #endregion


        #region SignUpUsers


        /// <summary>
        /// Set payer=0 to make new row in db payerId. Returns @@identity.
        /// </summary>
        public static int InsertSignUpUser(long cusno, int epiPageId, int payerId, string payMethod, string firstname, string lastsname, string address, string zip, string city, string phone, string email)
        {
            SqlParameter[] sqlParameters = new SqlParameter[] 
                    {
                        new SqlParameter("@cusno", cusno),
                        new SqlParameter("@epiPageId", epiPageId),
                        new SqlParameter("@payerId", payerId),
                        new SqlParameter("@payMethod", payMethod), 
                        new SqlParameter("@firstName", firstname),
                        new SqlParameter("@lastName", lastsname),
                        new SqlParameter("@address", address),
                        new SqlParameter("@zip", zip),
                        new SqlParameter("@city", city),
                        new SqlParameter("@phone", phone), 
                        new SqlParameter("@email", email)
                    };

            return int.Parse(DIClassLib.DbHelpers.SqlHelper.ExecuteScalarParam("DagensIndustriMISC", "InsertSignUpPerson", sqlParameters).ToString());
        }

        /// <summary>
        /// Set payer=0 to make new row in db payerId. Returns DataSet with @@identity(id) and created unique code
        /// </summary>
        public static DataSet InsertSignUpUserGuid(long cusno, int epiPageId, int payerId, string payMethod, string firstname, string lastsname, string address, string zip, string city, string phone, string email, Guid? code)
        {

            SqlParameter[] sqlParameters = new SqlParameter[] 
                    {
                        new SqlParameter("@cusno", cusno),
                        new SqlParameter("@epiPageId", epiPageId),
                        new SqlParameter("@payerId", payerId),
                        new SqlParameter("@payMethod", payMethod), 
                        new SqlParameter("@firstName", firstname),
                        new SqlParameter("@lastName", lastsname),
                        new SqlParameter("@address", address),
                        new SqlParameter("@zip", zip),
                        new SqlParameter("@city", city),
                        new SqlParameter("@phone", phone), 
                        new SqlParameter("@email", email),                     
                        new SqlParameter("@code", code)  
                    };

            if (!code.HasValue)
                sqlParameters[11] = new SqlParameter("@code", DBNull.Value);

            return DIClassLib.DbHelpers.SqlHelper.ExecuteDatasetParam("DagensIndustriMISC", "InsertSignUpPerson2", sqlParameters);
        }

        /// <summary>
        /// returns signUpPersonId OR 0 if no hit on cusno, epiPageId in db
        /// </summary>
        public static int TryUpdateSignUpUser(long cusno, int epiPageId, string payMethod, string firstname, string lastsname, string address, string zip, string city, string phone, string email)
        {
            SqlParameter[] sqlParameters = new SqlParameter[] 
                    {
                        new SqlParameter("@cusno", cusno),
                        new SqlParameter("@epiPageId", epiPageId),
                        new SqlParameter("@payMethod", payMethod), 
                        new SqlParameter("@firstName", firstname),
                        new SqlParameter("@lastName", lastsname),
                        new SqlParameter("@address", address),
                        new SqlParameter("@zip", zip),
                        new SqlParameter("@city", city),
                        new SqlParameter("@phone", phone), 
                        new SqlParameter("@email", email)
                    };

            return int.Parse(DIClassLib.DbHelpers.SqlHelper.ExecuteScalarParam("DagensIndustriMISC", "UpdateSignUpPerson", sqlParameters).ToString());
        }

        public static int GetSignUpNumParticipants(int epiPageId)
        {
            return int.Parse(DIClassLib.DbHelpers.SqlHelper.ExecuteScalar("DagensIndustriMISC", "GetSignUpNumParticipants2", new SqlParameter("@epiPageId", epiPageId)).ToString());
        }

        public static void UpdateSignUpPaymentStatus(int id, string status)
        {
            SqlParameter[] sqlParameters = new SqlParameter[] 
                    {
                       new SqlParameter("@id", id), 
                       new SqlParameter("@payMethod", status)
                    };

            DIClassLib.DbHelpers.SqlHelper.ExecuteNonQuery("DagensIndustriMISC", "UpdateSignUpPayMethod", sqlParameters);
        }

        /// <summary>
        /// Get registered person by unique code
        /// </summary>
        /// <param name="code">unique code</param>
        /// <returns></returns>
        public static DataSet GetSignUpPerson(Guid code)
        {
            SqlParameter[] sqlParameters = new SqlParameter[] { new SqlParameter("@code", code) };

            return DIClassLib.DbHelpers.SqlHelper.ExecuteDatasetParam("DagensIndustriMISC", "GetSignUpPersonByCode", sqlParameters);
        }

        /// <summary>
        /// Get registered person by cusno
        /// </summary>
        /// <param name="code">The cusno</param>
        /// <returns></returns>
        public static DataSet GetSignUpPerson(long cusno)
        {
            SqlParameter[] sqlParameters = new SqlParameter[] { new SqlParameter("@cusno", cusno) };

            return DIClassLib.DbHelpers.SqlHelper.ExecuteDatasetParam("DagensIndustriMISC", "GetSignUpPersonByCusno", sqlParameters);
        }

        
        /// <summary>
        /// Get a signed up persons friends
        /// </summary>
        /// <param name="payerId"></param>
        /// <returns>DataSet with all friends to this payerId which are not canceled</returns>
        public static DataSet GetSignUpPersonFriends(int payerId)
        {
            SqlParameter[] sqlParameters = new SqlParameter[] { new SqlParameter("@payerId", payerId) };

            return DIClassLib.DbHelpers.SqlHelper.ExecuteDatasetParam("DagensIndustriMISC", "GetSignUpPersonFriends", sqlParameters);
        }

        public static DataSet GetSignUpPersonCust(Guid code)
        {
            SqlParameter[] sqlParameters = new SqlParameter[] { new SqlParameter("@code", code) };

            return DIClassLib.DbHelpers.SqlHelper.ExecuteDatasetParam("DagensIndustriMISC", "GetSignUpPersonCustByCode", sqlParameters);
        }

        public static DataSet GetPersonalUrlCusno(Guid code, int epiPageId)
        {
            SqlParameter[] sqlParameters = new SqlParameter[] { new SqlParameter("@code", code), new SqlParameter("@epiPageId", epiPageId) };
            
            return DIClassLib.DbHelpers.SqlHelper.ExecuteDatasetParam("DagensIndustriMISC", "GetPersonalUrlCusno", sqlParameters);
        }
        
        

        public static int GetNumSignUpPersonsForCust(long cusno, int epiPageId)
        {
            SqlParameter[] sqlParameters = new SqlParameter[] { 
                new SqlParameter("@cusno", cusno),  
                new SqlParameter("@epiPageId", epiPageId)
            };

            return int.Parse(DIClassLib.DbHelpers.SqlHelper.ExecuteScalarParam("DagensIndustriMISC", "GetNumSignUpPersonsForCust", sqlParameters).ToString());
        }


        #region epi sign up admin page

        public static DataSet GetSignUpPersons(int epiPageId)
        {
            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@epiPageId", epiPageId)
            };

            return DIClassLib.DbHelpers.SqlHelper.ExecuteDatasetParam("DagensIndustriMISC", "GetSignUpPersons", sqlParameters);
        }

        public static void UpdateSignUpPerson(int id, string cusno, string epiPageId, string payMethod, string firstName, string lastName,
                                              string address, string zip, string city, string phone, string email, bool canceled)
        {
            SqlParameter[] sqlParameters = new SqlParameter[] 
            {
                new SqlParameter("@Id", id),
                new SqlParameter("@cusno", cusno),
                new SqlParameter("@epiPageId", epiPageId),
                new SqlParameter("@payMethod", payMethod),
                new SqlParameter("@firstName", firstName),
                new SqlParameter("@lastName", lastName),
                new SqlParameter("@address", address),
                new SqlParameter("@zip", zip),
                new SqlParameter("@city", city),
                new SqlParameter("@phone", phone),
                new SqlParameter("@email", email),
                new SqlParameter("@canceled", canceled)
            };

            if (string.IsNullOrEmpty(cusno))
                sqlParameters[1] = new SqlParameter("@cusno", DBNull.Value);

            if (string.IsNullOrEmpty(epiPageId))
                sqlParameters[2] = new SqlParameter("@epiPageId", DBNull.Value);


            DIClassLib.DbHelpers.SqlHelper.ExecuteNonQuery("DagensIndustriMISC", "UpdateSignUpPersonAdmin", sqlParameters);
        }

        public static void DeleteSignUpPerson(int id)
        {
            SqlParameter[] sqlParameters = new SqlParameter[] 
            {
                new SqlParameter("@Id", id)
            };

            DIClassLib.DbHelpers.SqlHelper.ExecuteNonQuery("DagensIndustriMISC", "DeleteSignUpPerson", sqlParameters);
        }

        public static void CancelSignUpPerson(int payerId)
        {
            SqlParameter[] sqlParameters = new SqlParameter[] 
            {
                new SqlParameter("@payerId", payerId)
            };

            DIClassLib.DbHelpers.SqlHelper.ExecuteNonQuery("DagensIndustriMISC", "CancelSignUpPerson", sqlParameters);
        }

        #endregion

        #endregion


        #region Various

        public static void InsertTicketEntry(long cusno)
        {
            SqlParameter[] sqlParameters = new SqlParameter[] 
            {
                new SqlParameter("@cusno", cusno)
            };

            DIClassLib.DbHelpers.SqlHelper.ExecuteNonQuery("DagensIndustriMISC", "InsertTicketEntry", sqlParameters);
        }

        public static bool IsCustomerInTicketEntries(long cusno)
        {
            SqlParameter[] sqlParameters = new SqlParameter[] 
            { 
                new SqlParameter("@cusno", cusno)
            };

            int IsInTicketEntries = int.Parse(SqlHelper.ExecuteScalarParam("DagensIndustriMISC", "IsInTicketEntries", sqlParameters).ToString());

            if (IsInTicketEntries > 0)
                return true;
            else
                return false;
        }
        
        public static void InsertSimpleFormEntry(int epiPageId, string firstname, string lastname, string email, string telephone, string messageBody)
        {
            SqlParameter[] sqlParameters = new SqlParameter[] 
            {
                new SqlParameter("@epiPageId", epiPageId),
                new SqlParameter("@firstName",firstname),
                new SqlParameter("@lastName",lastname),
                new SqlParameter("@email",email),
                new SqlParameter("@telephone",telephone),
                new SqlParameter("@messageBody",messageBody)
            };

            DIClassLib.DbHelpers.SqlHelper.ExecuteNonQuery("DagensIndustriMISC", "InsertSimpleFormEntry", sqlParameters);
        }

        public static bool InsertMbaApplication(long cusno, bool isPersonalApplication, string birthDate, string firstname, string lastname, string streetName, string zip, string city,
                                                string phoneMob, string email, string linkedInUrl, string company, string jobTitle, string education, string educationPoints,
                                                string numWorkingYears, int englishLevel, string motivation)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]{
                    new System.Data.SqlClient.SqlParameter("@cusno", cusno), 
                    new System.Data.SqlClient.SqlParameter("@isPersonalApplication", isPersonalApplication), 
                    new System.Data.SqlClient.SqlParameter("@birthDate", birthDate), 
                    new System.Data.SqlClient.SqlParameter("@firstname", firstname), 
                    new System.Data.SqlClient.SqlParameter("@lastname", lastname), 
                    new System.Data.SqlClient.SqlParameter("@streetName", streetName), 
                    new System.Data.SqlClient.SqlParameter("@zip", zip), 
                    new System.Data.SqlClient.SqlParameter("@city", city), 
                    new System.Data.SqlClient.SqlParameter("@phoneMob", phoneMob), 
                    new System.Data.SqlClient.SqlParameter("@email", email), 
                    new System.Data.SqlClient.SqlParameter("@linkedInUrl", linkedInUrl), 
                    new System.Data.SqlClient.SqlParameter("@company", company), 
                    new System.Data.SqlClient.SqlParameter("@jobTitle", jobTitle), 
                    new System.Data.SqlClient.SqlParameter("@education", education), 
                    new System.Data.SqlClient.SqlParameter("@educationPoints", educationPoints), 
                    new System.Data.SqlClient.SqlParameter("@numWorkingYears", numWorkingYears), 
                    new System.Data.SqlClient.SqlParameter("@englishLevel", englishLevel), 
                    new System.Data.SqlClient.SqlParameter("@motivation", motivation)
                    };

                DIClassLib.DbHelpers.SqlHelper.ExecuteNonQuery("DagensIndustriMISC", "Mba_InsertApplication", sqlParameters);
                return true;
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("cusno:" + cusno + ", ");
                sb.Append("isPersonalApplication:" + isPersonalApplication.ToString() + ", ");
                sb.Append("birthDate:" + birthDate + ", ");
                sb.Append("firstname:" + firstname + ", ");
                sb.Append("lastname:" + lastname + ", ");
                sb.Append("streetName:" + streetName + ", ");
                sb.Append("zip:" + zip + ", ");
                sb.Append("city:" + city + ", ");
                sb.Append("phoneMob:" + phoneMob + ", ");
                sb.Append("email:" + email + ", ");
                sb.Append("linkedInUrl:" + linkedInUrl + ", ");
                sb.Append("company:" + company + ", ");
                sb.Append("jobTitle:" + jobTitle + ", ");
                sb.Append("education:" + education + ", ");
                sb.Append("educationPoints:" + educationPoints + ", ");
                sb.Append("numWorkingYears:" + numWorkingYears + ", ");
                sb.Append("englishLevel:" + englishLevel + ", ");
                sb.Append("motivation:" + motivation);

                new Logger("InsertMbaApplication() - failed for cusno:" + cusno, sb.ToString() + Environment.NewLine + ex.ToString());
                return false;
            }
        }

        public static bool MdbInsertExtraInfo(int epiPageId, long cusno, string heading, string value)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[] 
                {
                    new SqlParameter("@pageid ", epiPageId),
                    new SqlParameter("@cusno",cusno),
                    new SqlParameter("@rubrik ", heading),
                    new SqlParameter("@value ",value)
                };

                SqlHelper.ExecuteNonQuery("MDB", "PostMemberCusno", sqlParameters);

                return true;
            }
            catch (Exception ex)
            {
                new Logger("MdbInsertExtraInfo() - failed for cusno :" + cusno + ", pageId: " + epiPageId, ex.ToString());
            }

            return false;
        }

        public static long MdbGetCusnoByCode(Guid code)
        {
            try
            {
                return long.Parse(SqlHelper.ExecuteScalar("MDB", "GetCusnoByCode", new SqlParameter("@uniqueId", code)).ToString());
            }
            catch (Exception ex)
            {
                new Logger("MdbGetCusnoByCode() - failed for code :" + code, ex.ToString());
            }

            return 0;
        }

        public static Guid MdbGetCodeByCusno(int cusno)
        {
            try
            {
                var guidString = SqlHelper.ExecuteScalar("MDB", "GetCodeByCusno", new SqlParameter("@cusno", cusno.ToString()));
                return guidString != null ? new Guid(guidString.ToString()) : Guid.Empty;
            }
            catch (Exception ex)
            {
                new Logger("MdbGetCodeByCusno() - failed for cusno :" + cusno, ex.ToString());
            }
            return Guid.Empty;
        }

        public static string MdbGetPostalNameByZip(string zipCode)
        {
            try
            {
                return SqlHelper.ExecuteScalar("MDB", "GetPostnameByZip", new SqlParameter("@uniqueZip", zipCode)).ToString();
            }
            catch (Exception ex)
            {
                //new Logger("MdbGetPostalNameByZip() - failed for zip :" + zipCode, ex.ToString());
            }

            return string.Empty;
        }

        public static void InsertToLogPar(int epiPageId, string campaign, string personno, string orgno, string firstName, string lastName, string company, string address, string zip, string city, string phone)
        {
            try
            {
                if (string.IsNullOrEmpty(phone))
                    phone = "";
                
                SqlParameter[] sqlParameters = new SqlParameter[]{
                    new SqlParameter("@pageid", epiPageId), 
                    new SqlParameter("@campaign", campaign), 
                    new SqlParameter("@personno", personno), 
                    new SqlParameter("@orgno", orgno), 
                    new SqlParameter("@firstname", firstName), 
                    new SqlParameter("@lastname", lastName),
                    new SqlParameter("@company", company),
                    new SqlParameter("@address", address),
                    new SqlParameter("@zip", zip),
                    new SqlParameter("@city", city),
                    new SqlParameter("@phone", phone)
                };

                DIClassLib.DbHelpers.SqlHelper.ExecuteNonQuery("DagensIndustriMISC", "InsertToLogPar", sqlParameters);
            }
            catch (Exception ex)
            {
                new Logger("InsertToLogPar() - failed on page:" + epiPageId, ex.ToString());
            }
        }

        public static DataSet GetCustomerInRoleRow(long cusno, int roleId)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]{
                    new System.Data.SqlClient.SqlParameter("@cusno", cusno), 
                    new System.Data.SqlClient.SqlParameter("@roleId", roleId)
                    };

                return SqlHelper.ExecuteDatasetParam("DisePren", "GetCustomerInRoleRow", sqlParameters);
            }
            catch (Exception ex)
            {
                new Logger("GetCustomerInRoleRow() failed for cusno:" + cusno + ", roleId:" + roleId, ex.ToString());
            }

            return null;
        }

        #endregion Various

        
        #region Campaign OLD

        // diCampagin ----
        //public static DataSet GetExtCustByCode(string code)
        //{
        //    return DIClassLib.DbHelpers.SqlHelper.ExecuteDataset("DagensIndustriMISC", "GetExtCustByCode", new SqlParameter("@code", code));
        //}

        //public static void SetExtCustDateVisitedByCode(string code)
        //{
        //    SqlParameter[] sqlParameters = new SqlParameter[] 
        //            {
        //                new SqlParameter("@code", code)
        //            };

        //    DIClassLib.DbHelpers.SqlHelper.ExecuteScalarParam("DagensIndustriMISC", "SetExtCustDateVisitedByCode", sqlParameters);
        //}

        //public static DataSet GetCampaignBasics(int epiPageId)
        //{
        //    return DIClassLib.DbHelpers.SqlHelper.ExecuteDataset("Campaign", "GetCampaignBasics", new SqlParameter("@epiPageId", epiPageId));
        //}

        // /diCampagin -----


        //public static bool ReminderSent(int EpiPageId)
        //{
        //    //return MsSqlHandler.GetCampaignTargetGroup(page.PageLink.ID);
        //    DataSet ds = DIClassLib.DbHelpers.SqlHelper.ExecuteDataset("Campaign", "GetCampaignReminderSent", new SqlParameter("@EpiPageId", EpiPageId));

        //    if (ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows[0]["reminderSent"].ToString() == true.ToString())
        //    {
        //        return true;
        //    }
        //    else
        //        return false;
        //}

        ///// <summary>
        ///// GetProducts from db Campaign
        ///// </summary>
        ///// <returns>A dataset with all products in table product</returns>
        //public static DataSet GetProducts()
        //{
        //    return DIClassLib.DbHelpers.SqlHelper.ExecuteDataset("Campaign", "GetAllProducts", null);
        //}

        //public static DataSet GetTypes()
        //{
        //    return DIClassLib.DbHelpers.SqlHelper.ExecuteDataset("Campaign", "GetAllTypes", null);
        //}

        ///// <summary>
        ///// GetOfferCodes from db Campaign
        ///// </summary>
        ///// <param name="onlyActive">If 1, only active offercodes will be returned</param>
        ///// <returns>A dataset with offercodes in table offerCode</returns>
        //public static DataSet GetOfferCodes(int onlyActive)
        //{
        //    return DIClassLib.DbHelpers.SqlHelper.ExecuteDataset("Campaign", "GetAllOfferCodes", new SqlParameter("@onlyActive", onlyActive));
        //}

        ///// <summary>
        ///// Get offer codes for specific campaign from db Campaign
        ///// </summary>
        ///// <param name="EpiPageId">The id of EpiServer PageData object</param>
        ///// <returns>A dataset with offercodes for campaign</returns>
        //public static DataSet GetCampaignOfferCodes(int EpiPageId)
        //{
        //    return DIClassLib.DbHelpers.SqlHelper.ExecuteDataset("Campaign", "GetCampaignOfferCodes", new SqlParameter("@EpiPageId", EpiPageId));
        //}

        ///// <summary>
        ///// Get costs for specific campaign from db Campaign
        ///// </summary>
        ///// <param name="EpiPageId">The id of EpiServer PageData object</param>
        ///// <returns>A dataset with costs for campaign</returns>
        //public static DataSet GetCampaignCosts(int EpiPageId)
        //{
        //    return DIClassLib.DbHelpers.SqlHelper.ExecuteDataset("Campaign", "GetCampaignCosts", new SqlParameter("@EpiPageId", EpiPageId));
        //}

        ///// <summary>
        ///// Get target group for specific campaign from db Campaign
        ///// </summary>
        ///// <param name="EpiPageId">The id of EpiServer PageData object</param>
        ///// <returns>A dataset with targetgroup for campaign</returns>
        //public static DataSet GetCampaignTargetGroup(int EpiPageId)
        //{
        //    return DIClassLib.DbHelpers.SqlHelper.ExecuteDataset("Campaign", "GetCampaignTargetGroup", new SqlParameter("@EpiPageId", EpiPageId));
        //}

        //public static DataSet GetCampaignType(int EpiPageId)
        //{
        //    return DIClassLib.DbHelpers.SqlHelper.ExecuteDataset("Campaign", "GetCampaignType", new SqlParameter("@EpiPageId", EpiPageId));
        //}

        ///// <summary>
        ///// GetTargetGroups from db Campaign
        ///// </summary>
        ///// <returns>A dataset with targetsgroups in table targetGroup</returns>
        //public static DataSet GetTargetGroups()
        //{
        //    return DIClassLib.DbHelpers.SqlHelper.ExecuteDataset("Campaign", "GetAllTargetGroups", null);
        //}

        ///// <summary>
        ///// Get all Campaigns with specified targetgroup from db Campaign
        ///// </summary>
        ///// <returns>A dataset with EpiPageIds for all campaigns in targetGroup</returns>
        //public static DataSet GetCampaignsWithTargetGroup(int targetGroupId)
        //{
        //    return DIClassLib.DbHelpers.SqlHelper.ExecuteDataset("Campaign", "GetCampaignsWithTargetGroup", new SqlParameter("@targetGroupId", targetGroupId));
        //}

        ///// <summary>
        ///// Get all Campaigns with specified offercde from db Campaign
        ///// </summary>
        ///// <returns>A dataset with EpiPageIds for all campaigns with offercode</returns>
        //public static DataSet GetCampaignsWithOfferCode(int offerCodeId)
        //{
        //    return DIClassLib.DbHelpers.SqlHelper.ExecuteDataset("Campaign", "GetCampaignsWithOfferCode", new SqlParameter("@offerCodeId", offerCodeId));
        //}

        //public static void UpdateCampaignReminderSent(int EpiPageId, bool sent)
        //{
        //    SqlParameter[] sqlParameters = new SqlParameter[]{
        //                    new SqlParameter("@EpiPageId", EpiPageId),
        //                    new SqlParameter("@sent", sent)
        //                };

        //    DIClassLib.DbHelpers.SqlHelper.ExecuteNonQuery("Campaign", "UpdateCampaignReminderSent", sqlParameters);
        //}

        //public static void UpdateCampaignLeads(int EpiPageId)
        //{
        //    SqlParameter[] sqlParameters = new SqlParameter[]{
        //                    new SqlParameter("@EpiPageId", EpiPageId)
        //                };

        //    DIClassLib.DbHelpers.SqlHelper.ExecuteNonQuery("Campaign", "UpdateCampaignLeads", sqlParameters);
        //}

        ///// <summary>
        ///// UpdateOfferCode in db Campaign
        ///// </summary>
        ///// <param name="offercodeId"></param>
        ///// <param name="offerText"></param>
        ///// <param name="sortOrder"></param>
        ///// <param name="isAutogiro"></param>
        ///// <param name="isActive"></param>
        //public static void UpdateOfferCode(int offercodeId, string offerText, int sortOrder, bool isActive, bool isStudent)
        //{
        //    SqlParameter[] sqlParameters = new SqlParameter[]{
        //            new SqlParameter("@offercodeid", offercodeId), 
        //            new SqlParameter("@offerText", offerText), 
        //            new SqlParameter("@sortOrder", sortOrder), 
        //            new SqlParameter("@isActive", isActive),
        //            new SqlParameter("@isStudent", isStudent)
        //    };

        //    DIClassLib.DbHelpers.SqlHelper.ExecuteNonQuery("Campaign", "UpdateOfferCode", sqlParameters);
        //}

        ///// <summary>
        ///// DeleteOffercode in db Campaign
        ///// </summary>
        ///// <param name="offerCodeId"></param>
        //public static void DeleteOfferCode(int offerCodeId)
        //{
        //    SqlParameter[] sqlParameters = new SqlParameter[]{
        //            new SqlParameter("@offercodeid", offerCodeId), 
        //    };

        //    DIClassLib.DbHelpers.SqlHelper.ExecuteNonQuery("Campaign", "DeleteOfferCode", sqlParameters);

        //}

        ///// <summary>
        ///// DeleteTargetGroup in db Campaign
        ///// </summary>
        ///// <param name="targetGroupId"></param>
        //public static void DeleteTargetGroup(int targetGroupId)
        //{
        //    SqlParameter[] sqlParameters = new SqlParameter[]{
        //            new SqlParameter("@targetGroupId", targetGroupId), 
        //    };

        //    DIClassLib.DbHelpers.SqlHelper.ExecuteNonQuery("Campaign", "DeleteTargetGroup", sqlParameters);
        //}

        ///// <summary>
        ///// Clear all offercodes for campaign in db Campaign
        ///// </summary>
        ///// <param name="EpiPageId"></param>
        //public static void ClearCampaignOfferCodes(int EpiPageId)
        //{
        //    SqlParameter[] sqlParameters = new SqlParameter[]{
        //                    new SqlParameter("@EpiPageId", EpiPageId)
        //                };

        //    DIClassLib.DbHelpers.SqlHelper.ExecuteNonQuery("Campaign", "ClearCampaignOfferCode", sqlParameters);
        //}

        ///// <summary>
        ///// Clear all costs for campaign in db Campaign
        ///// </summary>
        ///// <param name="EpiPageId"></param>
        //public static void ClearCampaignCosts(int EpiPageId)
        //{
        //    SqlParameter[] sqlParameters = new SqlParameter[]{
        //                    new SqlParameter("@EpiPageId", EpiPageId)
        //                };

        //    DIClassLib.DbHelpers.SqlHelper.ExecuteNonQuery("Campaign", "ClearCampaignCosts", sqlParameters);
        //}

        ///// <summary>
        ///// Insert OfferCode in db Campaign
        ///// </summary>
        ///// <param name="campNo"></param>
        ///// <param name="campId"></param>
        ///// <param name="offerText"></param>
        ///// <param name="sortOrder"></param>
        ///// <param name="isAutogiro"></param>
        ///// <param name="isActive"></param>
        ///// <param name="productNo"></param>
        //public static void InsertOfferCode(string campNo, string campId, string offerText, int sortOrder, bool isAutogiro, bool isActive, string productNo, bool isStudent, string priceGroup, string subsKind)
        //{
        //    SqlParameter[] sqlParameters = new SqlParameter[]{
        //            new SqlParameter("@campNo", campNo), 
        //            new SqlParameter("@campId", campId), 
        //            new SqlParameter("@offerText", offerText), 
        //            new SqlParameter("@sortOrder", sortOrder), 
        //            new SqlParameter("@isAutogiro", isAutogiro), 
        //            new SqlParameter("@isActive", isActive),
        //            new SqlParameter("@productNo", productNo),
        //            new SqlParameter("@isStudent", isStudent),
        //            new SqlParameter("@priceGroup", priceGroup),
        //            new SqlParameter("@subsKind", subsKind)
        //    };

        //    DIClassLib.DbHelpers.SqlHelper.ExecuteNonQuery("Campaign", "InsertOfferCode", sqlParameters);
        //}

        ///// <summary>
        ///// Insert TargetGroup in db Campaign
        ///// </summary>
        ///// <param name="targetGroupName"></param>
        //public static void InsertTargetGroup(string targetGroupName)
        //{
        //    SqlParameter[] sqlParameters = new SqlParameter[]{
        //            new SqlParameter("@targetGroupName", targetGroupName), 
        //    };

        //    DIClassLib.DbHelpers.SqlHelper.ExecuteNonQuery("Campaign", "InsertTargetGroup", sqlParameters);
        //}

        ///// <summary>
        ///// Insert campaign offercode in db Campaign
        ///// </summary>
        ///// <param name="epiPageId"></param>
        ///// <param name="offerCodeId"></param>
        //public static void InsertCampaignOfferCode(int epiPageId, string offerCodeId)
        //{
        //    SqlParameter[] sqlParameters = new SqlParameter[]{
        //                    new SqlParameter("@EpiPageId", epiPageId), 
        //                    new SqlParameter("@offerCodeId", offerCodeId)
        //                };

        //    DIClassLib.DbHelpers.SqlHelper.ExecuteNonQuery("Campaign", "InsertCampaignOfferCode", sqlParameters);
        //}

        ///// <summary>
        ///// Insert campaign targetgroup in db Campaign
        ///// </summary>
        ///// <param name="epiPageId"></param>
        ///// <param name="targetGroupId"></param>
        //public static void InsertCampaignTargetGroup(int epiPageId, string targetGroupId)
        //{
        //    SqlParameter[] sqlParameters = new SqlParameter[]{
        //            new SqlParameter("@EpiPageId", epiPageId), 
        //            new SqlParameter("@TargetGroupId", targetGroupId)
        //        };

        //    DIClassLib.DbHelpers.SqlHelper.ExecuteNonQuery("Campaign", "InsertCampaignTargetGroup", sqlParameters);
        //}

        ///// <summary>
        ///// Insert campaign type in db Campaign
        ///// </summary>
        ///// <param name="epiPageId"></param>
        ///// <param name="typeId"></param>
        ///// <param name="comment"></param>
        //public static void InsertCampaignType(int epiPageId, int typeId, string comment)
        //{
        //    SqlParameter[] sqlParameters = new SqlParameter[]{
        //            new SqlParameter("@EpiPageId", epiPageId), 
        //            new SqlParameter("@typeId", typeId),
        //            new SqlParameter("@typeComment", comment)
        //        };

        //    DIClassLib.DbHelpers.SqlHelper.ExecuteNonQuery("Campaign", "InsertCampaignType", sqlParameters);
        //}

        ///// <summary>
        ///// Insert campaign cost in db Campaign
        ///// </summary>
        ///// <param name="epiPageId"></param>
        ///// <param name="description"></param>
        ///// <param name="amount"></param>
        //public static void InsertCampaignCost(int epiPageId, string description, int amount)
        //{
        //    SqlParameter[] sqlParameters = new SqlParameter[]{
        //            new SqlParameter("@EpiPageId", epiPageId), 
        //            new SqlParameter("@description", description),
        //            new SqlParameter("@amount", amount)
        //        };

        //    DIClassLib.DbHelpers.SqlHelper.ExecuteNonQuery("Campaign", "InsertCampaignCost", sqlParameters);
        //}


        ///// <summary>
        ///// Insert CampaignUser in DagensIndustriMISC db
        ///// </summary>
        //public static void InsertCampaignUser(CampaignUser cu, bool isPayed)
        //{
        //    try
        //    {
        //        SqlParameter[] sqlParameters = new SqlParameter[]{
        //            new SqlParameter("@cusno", cu.CusNo), 
        //            new SqlParameter("@subsNo", cu.SubsNo), 
        //            new SqlParameter("@firstName", cu.FirstName), 
        //            new SqlParameter("@lastName", cu.LastName), 
        //            new SqlParameter("@streetName", cu.StreetName), 
        //            new SqlParameter("@houseNo", cu.HouseNo), 
        //            new SqlParameter("@staircase", cu.Staircase), 
        //            new SqlParameter("@apartment", cu.Apartment), 
        //            new SqlParameter("@apartmentNo", cu.ApartmentNo), 
        //            new SqlParameter("@zipCode", cu.ZipCode), 
        //            new SqlParameter("@city", cu.City), 
        //            new SqlParameter("@otherPhone", cu.OtherPhone), 
        //            new SqlParameter("@email", cu.Email), 
        //            new SqlParameter("@careOf", cu.CareOf), 
        //            new SqlParameter("@birthNo", cu.BirthNo), 
        //            new SqlParameter("@company", cu.Company), 
        //            new SqlParameter("@orgNo", cu.OrgNo), 
        //            new SqlParameter("@isSubscriber", cu.IsSubscriber), 
        //            new SqlParameter("@cirixName1", cu.CirixName1), 
        //            new SqlParameter("@cirixName2", cu.CirixName2), 
        //            new SqlParameter("@cirixStreet2", cu.CirixStreet2), 
        //            new SqlParameter("@targetGroup", cu.TargetGroup), 
        //            new SqlParameter("@dateStart", cu.DateStart), 
        //            new SqlParameter("@dateEnd", cu.DateEnd), 
        //            new SqlParameter("@price", cu.OfferCode.Price), 
        //            new SqlParameter("@paperCode", cu.OfferCode.PaperCode), 
        //            new SqlParameter("@subsLength", cu.OfferCode.SubsLength), 
        //            new SqlParameter("@subsLengthUnit", cu.OfferCode.SubsLengthUnit), 
        //            new SqlParameter("@discPercent", cu.OfferCode.DiscPercent), 
        //            new SqlParameter("@subsEndDate", cu.OfferCode.SubsEndDate), 
        //            new SqlParameter("@offerCodeId", cu.OfferCode.OfferCodeId), 
        //            new SqlParameter("@ocText", cu.OfferCode.Text), 
        //            new SqlParameter("@campNo", cu.OfferCode.CampNo), 
        //            new SqlParameter("@campId", cu.OfferCode.CampId),
        //            new SqlParameter("@isAutogiro", cu.OfferCode.IsAutogiro),
        //            new SqlParameter("@isStudent", cu.OfferCode.IsStudent),
        //            new SqlParameter("@productName", cu.OfferCode.ProcuctName),
        //            new SqlParameter("@productNo", cu.OfferCode.ProductNo),
        //            new SqlParameter("@subskind", cu.OfferCode.Subskind),
        //            new SqlParameter("@priceGroup", cu.OfferCode.PriceGroup),
        //            new SqlParameter("@subsType", cu.OfferCode.Substype),
        //            new SqlParameter("@grossPrice", cu.OfferCode.GrossPrice),
        //            new SqlParameter("@totalPrice", cu.OfferCode.TotalPrice),
        //            new SqlParameter("@totalPriceIncVat", cu.OfferCode.TotalPriceIncVAT),        
        //            new SqlParameter("@itemPrice", cu.OfferCode.ItemPrice),
        //            new SqlParameter("@itemQuantity", cu.OfferCode.ItemQuantity),
        //            new SqlParameter("@isPayed", isPayed)
        //        };

        //        DIClassLib.DbHelpers.SqlHelper.ExecuteNonQuery("DagensIndustriMISC", "InsertCampaignCustomer", sqlParameters);
        //    }
        //    catch (Exception ex)
        //    {
        //        new DIClassLib.DbHelpers.Logger("InsertCampaignUser() - failed", ex.ToString());
        //    }
        //}

        #endregion


        #region Autowithdrawal

 
        public static void InsertAwdNets(NetsCardPayReturn ret, long cusno, long subsno, long campno, DateTime subsEndDate)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]{
                    new SqlParameter("@aurigaSubsId", ret.QueryPayObj.PanHash), 
                    new SqlParameter("@cusno", cusno), 
                    new SqlParameter("@subsno", subsno), 
                    new SqlParameter("@campno", campno), 
                    new SqlParameter("@dateSubsEnd", subsEndDate),
                    };

                SqlHelper.ExecuteNonQuery("DisePren", "awd_InsertAwd2", sqlParameters);
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("aurigaSubsId:" + ret.QueryPayObj.PanHash);
                sb.Append(", cusno:" + cusno);
                sb.Append(", subsno:" + subsno);
                sb.Append(", dateSubsEnd:" + subsEndDate.ToString());

                new Logger("InsertAwdNets failed for " + sb.ToString(), ret.ToString() + "<hr>" + ex.ToString());
            }
        }

        public static void InsertAwd2(NetsCardPayReturn ret, long cusno, long subsno, long campno, DateTime subsEndDate)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]{
                    new SqlParameter("@aurigaSubsId", ret.QueryPayObj.PanHash), 
                    new SqlParameter("@cusno", cusno), 
                    new SqlParameter("@subsno", subsno), 
                    new SqlParameter("@campno", campno), 
                    new SqlParameter("@dateSubsEnd", subsEndDate),
                    };

                DIClassLib.DbHelpers.SqlHelper.ExecuteNonQuery("DisePren", "awd_InsertAwd2", sqlParameters);
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("aurigaSubsId:" + ret.QueryPayObj.PanHash);
                sb.Append(", cusno:" + cusno);
                sb.Append(", subsno:" + subsno);
                sb.Append(", dateSubsEnd:" + subsEndDate.ToString());

                new Logger("InsertAwd2 failed for " + sb.ToString(), ret.ToString() + "<hr>" + ex.ToString());
            }
        }

        public static DataSet GeAwdsForCust(long cusno)
        {
            try
            {
                return DIClassLib.DbHelpers.SqlHelper.ExecuteDataset("DisePren", "awd_GetAutowithdrawals", new SqlParameter("cusno", cusno));
            }
            catch (Exception ex)
            {
                new DIClassLib.DbHelpers.Logger("GeAwdsForCust() failed for cusno: " + cusno, ex.ToString());
            }

            return null;
        }

        public static int InsertAwdBatch()
        {
            try
            {
                return int.Parse(SqlHelper.ExecuteScalar("DisePren", "awd_InsertBatch", null).ToString());
            }
            catch (Exception ex)
            {
                new Logger("InsertAwdBatch() failed", ex.ToString());
                throw;
            }
        }

        public static DataSet GetAwdsForPayment()
        {
            try
            {
                return SqlHelper.ExecuteDataset("DisePren", "awd_GetAutowithdrawalsForPayment", null);

            }
            catch (Exception ex)
            {
                new Logger("GetAwdsForPayment failed", ex.ToString());
            }

            return null;
        }

        public static void SetAwdIncludeInBatchFlag(string aurigaSubsId, bool includeInBatch)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]{
                    new SqlParameter("@aurigaSubsId", aurigaSubsId), 
                    new SqlParameter("@includeInBatch", includeInBatch)
                    };

                DIClassLib.DbHelpers.SqlHelper.ExecuteNonQuery("DisePren", "awd_UpdateIncludeInBatch", sqlParameters);
            }
            catch (Exception ex)
            {
                new Logger("SetAwdIncludeInBatchFlag failed for aurigaSubsId:" + aurigaSubsId.ToString() + ", includeInBatch:" + includeInBatch.ToString(), ex.ToString());
            }
        }

        public static void InsertAwdCustInBatch(string aurigaSubsId, int batchId, int cusRefno)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]{
                    new System.Data.SqlClient.SqlParameter("@aurigaSubsId", aurigaSubsId), 
                    new System.Data.SqlClient.SqlParameter("@batchId", batchId),
                    new System.Data.SqlClient.SqlParameter("@Customer_Refno", cusRefno)
                    };

                DIClassLib.DbHelpers.SqlHelper.ExecuteNonQuery("DisePren", "awd_InsertCustInBatch", sqlParameters);
            }
            catch (Exception ex)
            {
                new Logger("InsertAwdCustInBatch failed for aurigaSubsId:" + aurigaSubsId.ToString() + 
                                                         ", batchId:" + batchId.ToString() + 
                                                         ", cusRefno:" + cusRefno.ToString(), ex.ToString());
            }
        }

        public static void UpdateAwdDateSubsEnd(string aurigaSubsId, DateTime dateSubsEnd)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]{
                    new System.Data.SqlClient.SqlParameter("@aurigaSubsId", aurigaSubsId), 
                    new System.Data.SqlClient.SqlParameter("@dateSubsEnd", dateSubsEnd)
                    };

                DIClassLib.DbHelpers.SqlHelper.ExecuteNonQuery("DisePren", "awd_UpdateDateSubsEnd", sqlParameters);
            }
            catch (Exception ex)
            {
                new Logger("UpdateAwdDateSubsEnd failed for aurigaSubsId:" + aurigaSubsId.ToString() + ", dateSubsEnd:" + dateSubsEnd.ToString(), ex.ToString());
            }
        }

        public static DataSet GetAwdFailedCustPayments(string aurigaSubsId)
        {
            try
            {
                return SqlHelper.ExecuteDataset("DisePren", "awd_GetFailedCustPayments", new SqlParameter("@aurigaSubsId", aurigaSubsId));
            }
            catch (Exception ex)
            {
                new Logger("GetAwdFailedCustPayments failed", ex.ToString());
            }

            return null;
        }

        public static DataSet GetAwdByPageGuid(Guid pageGuid)
        {
            try
            {
                return SqlHelper.ExecuteDataset("DisePren", "awd_GetAwdByPageGuid", new SqlParameter("@pageGuid", pageGuid));
            }
            catch (Exception ex)
            {
                new Logger("GetAwdByPageGuid failed", ex.ToString());
            }

            return null;
        }

        public static void InsertAwdPayPagePayment(Guid pageGuid, int customerRefNo)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]{
                    new System.Data.SqlClient.SqlParameter("@pageGuid", pageGuid), 
                    new System.Data.SqlClient.SqlParameter("@Customer_Refno", customerRefNo)
                    };

                DIClassLib.DbHelpers.SqlHelper.ExecuteNonQuery("DisePren", "awd_InsertPayPagePayment", sqlParameters);
            }
            catch (Exception ex)
            {
                new Logger("InsertAwdCustInBatch failed for pageGuid:" + pageGuid.ToString() + ", customerRefNo:" + customerRefNo.ToString(), ex.ToString());
            }
        }

        public static void CancelAwdSubscription(long subsno)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]{
                    new System.Data.SqlClient.SqlParameter("@subsno", subsno)
                    };

                DIClassLib.DbHelpers.SqlHelper.ExecuteNonQuery("DisePren", "awd_CancelSubs", sqlParameters);
            }
            catch (Exception ex)
            {
                new Logger("CancelAwdSubscription failed for subsno:" + subsno.ToString() , ex.ToString());
            }
        }

        public static DataSet GetAwdBySubsno(long subsno)
        {
            try
            {
                return SqlHelper.ExecuteDataset("DisePren", "awd_GetAwdBySubsno", new SqlParameter("@subsno", subsno));
            }
            catch (Exception ex)
            {
                new Logger("GetAwdBySubsno failed for subsno=" + subsno, ex.ToString());
            }

            return null;
        }

        public static bool IsActiveAwdSubs(long subsno)
        {
            DataSet ds = MsSqlHandler.GetAwdBySubsno(subsno);
            if (DbHelpMethods.DataSetHasRows(ds))
                return bool.Parse(ds.Tables[0].Rows[0]["includeInBatch"].ToString());

            return false;
        }

        public static DataSet GetAwdPayHistoryBySubsno(long subsno)
        {
            try
            {
                return SqlHelper.ExecuteDataset("DisePren", "awd_GetPayHistoryBySubsno", new SqlParameter("@subsno", subsno));
            }
            catch (Exception ex)
            {
                new Logger("GetAwdPayHistoryBySubsno failed for subsno=" + subsno, ex.ToString());
            }

            return null;
        }

        //public static int SubsnoInActiveAwd(long subsno)
        //{
        //    try
        //    {

        //        return int.Parse(
        //            SqlHelper.ExecuteScalar("DisePren", "awd_SubsnoInActiveAwd", 
        //                new System.Data.SqlClient.SqlParameter("@subsno", subsno)
        //            ).ToString());
        //    }
        //    catch (Exception ex)
        //    {
        //        new Logger("SubsnoInActiveAwd() failed", ex.ToString());
        //        throw;
        //    }
        //}

        #endregion


        #region SSO

        public static DataSet SsoGetCustRow(int id, string codeFirstFour)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]{
                    new System.Data.SqlClient.SqlParameter("@id", id), 
                    new System.Data.SqlClient.SqlParameter("@codeFirstFour", codeFirstFour)
                    };

                return SqlHelper.ExecuteDatasetParam("DisePren", "sso_GetRowByIdAndCode", sqlParameters);
            }
            catch (Exception ex)
            {
                new Logger("SsoGetCustRow(id, codeFirstFour) failed for id:" + id + ", codeFirstFour:" + codeFirstFour, ex.ToString());
            }

            return null;
        }

        /// <summary>
        /// Returns ssoConnect row. Will insert row for cusno in ssoConnect table if it does not exist already.
        /// </summary>
        /// <param name="cirixCusno"></param>
        /// <returns></returns>
        public static DataSet SsoGetCustRow(long cirixCusno)
        {
            try
            {
                return SqlHelper.ExecuteDataset("DisePren", "sso_GetRowByCusno", new System.Data.SqlClient.SqlParameter("@cirixCusno", cirixCusno));
            }
            catch (Exception ex)
            {
                new Logger("SsoGetCustRow(cirixCusno) failed for cirixCusno:" + cirixCusno, ex.ToString());
            }

            return null;
        }

        public static void SsoUpdateCustRow(long cirixCusno, string token, string bonDigUserId, string remembered)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]{
                    new System.Data.SqlClient.SqlParameter("@cirixCusno", cirixCusno), 
                    new System.Data.SqlClient.SqlParameter("@plusToken", token),
                    new System.Data.SqlClient.SqlParameter("@plusUserId", bonDigUserId),
                    new System.Data.SqlClient.SqlParameter("@plusFirstName", ""),
                    new System.Data.SqlClient.SqlParameter("@plusLastName", ""),
                    new System.Data.SqlClient.SqlParameter("@plusRemembered", remembered)
                    };

                DIClassLib.DbHelpers.SqlHelper.ExecuteNonQuery("DisePren", "sso_UpdateRow", sqlParameters);
            }
            catch (Exception ex)
            {
                new Logger("SsoUpdateCustRow failed()", ex.ToString());
            }
        }

        public static List<string> GetUsernameAndPasswd(long cusno)
        {
            List<string> ret = new List<string>();
            ret.Add("");    //user
            ret.Add("");    //pass
            
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset("DisePren", "GetUserAndPasswdByCusno", new SqlParameter("@cusno", cusno));
                if (DbHelpMethods.DataSetHasRows(ds))
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    ret[0] = dr["userid"].ToString();
                    ret[1] = dr["passwd"].ToString();
                }
            }
            catch (Exception ex)
            {
                new Logger("GetUsernamePasswd() - failed for cusno:" + cusno, ex.ToString());
            }

            return ret;
        }

        public static string GetSsoCustomerCode(long cusno)
        {
            DataSet ds = SsoGetCustRow(cusno);
            if (DbHelpMethods.DataSetHasRows(ds))
            {
                SsoConnectRow row = new SsoConnectRow(ds);
                return row.CustomerCode;
            }

            return string.Empty;
        }


        public static DataSet GetDiAccountsByExtSubId(string extSubId)
        {
            try
            {
                return SqlHelper.ExecuteDataset("DisePren", "DiAcc_getDiAccByExtSubId", new System.Data.SqlClient.SqlParameter("@extSubId", extSubId));
            }
            catch (Exception ex)
            {
                new Logger("GetDiAccountsByExtSubId() failed for extSubId:" + extSubId, ex.ToString());
            }

            return null;
        }

        #endregion

        
        #region Wine

        public static int InsertWine(int varnummer, string about, DateTime date,string longitude,string latitude)
        {
            int id = 0;
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]{
                    new System.Data.SqlClient.SqlParameter("@varnummer", varnummer),
                    new System.Data.SqlClient.SqlParameter("@about", about),
                    new System.Data.SqlClient.SqlParameter("@date", date),
                    new System.Data.SqlClient.SqlParameter("@longitude", longitude),
                    new System.Data.SqlClient.SqlParameter("@latitude", latitude)
                    };

                object oId = DIClassLib.DbHelpers.SqlHelper.ExecuteScalarParam("DagensIndustriMISC", "Wine_InsertWine", sqlParameters);
                if (oId != null)
                    id = Int32.Parse(oId.ToString());
            }
            catch (Exception ex)
            {
                new Logger("InsertWine failed", ex.ToString());
            }
            return id;
        }

        public static int InsertWineCharacter(string name)
        {
            int id = 0;
            try
            {


                SqlParameter returnValue = new SqlParameter();
                returnValue.Direction = ParameterDirection.ReturnValue;

                SqlParameter[] sqlParameters = new SqlParameter[]{
                    new System.Data.SqlClient.SqlParameter("@name", name),
                    returnValue
                    };
                object oId = DIClassLib.DbHelpers.SqlHelper.ExecuteScalarParam("DagensIndustriMISC", "Wine_InsertWineCharacter", sqlParameters);
                if (oId != null)
                    id = Int32.Parse(oId.ToString());
            }
            catch (Exception ex)
            {
                new Logger("InsertWineCharacter failed", ex.ToString());
            }

            return id;
        }

        // 
        public static DataTable GetWineCharacters()
        {
            try
            {

                DataSet ds = DIClassLib.DbHelpers.SqlHelper.ExecuteDatasetParam("DagensIndustriMISC", "Wine_GetAllWineCharacters", null);
                if (ds != null && ds.Tables != null)
                    return ds.Tables[0];
            }
            catch (Exception ex)
            {
                new Logger("GetWineCharacters failed", ex.ToString());
            }
            return null;
        }

        public static DataTable GetSystembolagetArticleByVarnummer(int varnummer)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]{
                    new System.Data.SqlClient.SqlParameter("@varnummer", varnummer),

                    };
                DataSet ds = DIClassLib.DbHelpers.SqlHelper.ExecuteDatasetParam("DagensIndustriMISC", "Wine_GetSystembolagetArticleByVarnummer", sqlParameters);
                if (ds != null && ds.Tables != null)
                    return ds.Tables[0];
            }
            catch (Exception ex)
            {
                new Logger("GetSystembolagetArticleByVarnummer failed", ex.ToString());
            }
            return null;
        }

        internal static void InsertCharacterInWine(int wineId, int characterId)
        {
            int result = 0;
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]{
                    new System.Data.SqlClient.SqlParameter("@wineId", wineId),
                    new System.Data.SqlClient.SqlParameter("@characterId", characterId)
                    };

                result = DIClassLib.DbHelpers.SqlHelper.ExecuteNonQuery("DagensIndustriMISC", "Wine_InsertCharactersInWine", sqlParameters);
            }
            catch (Exception ex)
            {
                new Logger("InsertCharactersInWine failed", ex.ToString());
            }

        }

        internal static DataTable GetWine(int id)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]{
                    new System.Data.SqlClient.SqlParameter("@id", id),

                    };
                DataSet ds = DIClassLib.DbHelpers.SqlHelper.ExecuteDatasetParam("DagensIndustriMISC", "Wine_GetWineById", sqlParameters);
                if (ds != null && ds.Tables != null)
                    return ds.Tables[0];
            }
            catch (Exception ex)
            {
                new Logger("GetWine failed", ex.ToString());
            }
            return null;
        }

        internal static void UpdateWine(int wineId, int varnummer, string about, DateTime date,string longitude,string latitude)
        {

            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]{
                    new System.Data.SqlClient.SqlParameter("@id", wineId),
                    new System.Data.SqlClient.SqlParameter("@varnummer", varnummer),
                    new System.Data.SqlClient.SqlParameter("@about", about),
                    new System.Data.SqlClient.SqlParameter("@date", date),
                    new System.Data.SqlClient.SqlParameter("@longitude", longitude),
                    new System.Data.SqlClient.SqlParameter("@latitude", latitude)
                    };

                DIClassLib.DbHelpers.SqlHelper.ExecuteScalarParam("DagensIndustriMISC", "Wine_UpdateWine", sqlParameters);

            }
            catch (Exception ex)
            {
                new Logger("UpdateWine failed", ex.ToString());
            }

        }

        internal static void DeleteCharacterInWine(int wineId, int characterId)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]{
                    new System.Data.SqlClient.SqlParameter("@wineId", wineId),
                    new System.Data.SqlClient.SqlParameter("@characterId", characterId)
                    };

                DIClassLib.DbHelpers.SqlHelper.ExecuteNonQuery("DagensIndustriMISC", "Wine_DeleteCharactersInWine", sqlParameters);

            }
            catch (Exception ex)
            {
                new Logger("DeleteCharacterInWine failed", ex.ToString());
            }

        }

        internal static DataTable GetAllWines()
        {
            try
            {

                DataSet ds = DIClassLib.DbHelpers.SqlHelper.ExecuteDatasetParam("DagensIndustriMISC", "Wine_GetAllWines", null);
                if (ds != null && ds.Tables != null)
                    return ds.Tables[0];
            }
            catch (Exception ex)
            {
                new Logger("GetWineCharacters failed", ex.ToString());
            }
            return null;
        }

        internal static void DeleteWine(int id)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]{
                    new System.Data.SqlClient.SqlParameter("@id", id)
                    };

                DIClassLib.DbHelpers.SqlHelper.ExecuteNonQuery("DagensIndustriMISC", "Wine_DeleteWine", sqlParameters);

            }
            catch (Exception ex)
            {
                new Logger("DeleteWine failed", ex.ToString());
            }
        }


        internal static DataTable GetWinesWithCharacters()
        {
            try
            {

                DataSet ds = DIClassLib.DbHelpers.SqlHelper.ExecuteDatasetParam("DagensIndustriMISC", "Wine_GetWinesWithCharacters", null);
                if (ds != null && ds.Tables != null)
                    return ds.Tables[0];
            }
            catch (Exception ex)
            {
                new Logger("GetWinesWithCharacters failed", ex.ToString());
            }
            return null;
        }

        internal static void DeleteWineCharacter(int id)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]{
                    new System.Data.SqlClient.SqlParameter("@id", id)
                    };

                DIClassLib.DbHelpers.SqlHelper.ExecuteNonQuery("DagensIndustriMISC", "Wine_DeleteWineCharacter", sqlParameters);

            }
            catch (Exception ex)
            {
                new Logger("DeleteWineCharacter failed", ex.ToString());
            }
        }

        internal static void UpdateWineCharacter(int id,String name)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]{
                    new System.Data.SqlClient.SqlParameter("@id", id),
                     new System.Data.SqlClient.SqlParameter("@name", name)
                    };

                DIClassLib.DbHelpers.SqlHelper.ExecuteNonQuery("DagensIndustriMISC", "Wine_UpdateWineCharacter", sqlParameters);

            }
            catch (Exception ex)
            {
                new Logger("UpdateWineCharacter failed", ex.ToString());
            }
        }

        #endregion


        #region docTrackr
        
        /// <summary>
        /// Returns a trackable doc name '102-YYYYMMDD.pdf' on success, else an empty string.
        /// </summary>
        //internal static string DocTrackr_TryGetTrackableDocName(DateTime dateIssue, string servicePlusUserId, string ipNumber, string siteProvidedDownload)
        //{
        //    try
        //    {
        //        SqlParameter[] sqlParameters = new SqlParameter[]
        //        {
        //            new SqlParameter("@dateIssue", dateIssue.Date), 
        //            new SqlParameter("@servicePlusUserId", servicePlusUserId),
        //            new SqlParameter("@ipNumber", ipNumber),
        //            new SqlParameter("@siteProvidedDownload", siteProvidedDownload)
        //        };

        //        object o = SqlHelper.ExecuteScalarParam("DocTrackr", "InsertCustomerDownload", sqlParameters);
        //        if (o != null)
        //            return o.ToString();
        //    }
        //    catch (Exception ex)
        //    {
        //        new Logger("DocTrackr_TryGetTrackableDocName failed - dateIssue: " + dateIssue.Date.ToString() + 
        //                    ", servicePlusUserId: " + servicePlusUserId + 
        //                    ", ipNumber: " + ipNumber + 
        //                    ", siteProvidedDownload: " + siteProvidedDownload, ex.ToString());
        //    }

        //    return string.Empty;
        //}

        #endregion

    }
}
