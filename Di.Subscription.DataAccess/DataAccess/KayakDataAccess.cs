using System;
using System.Data;
using System.ServiceModel.Description;
using Di.Common.EndPointBehaviours;
using Di.Common.Logging;
using Di.Subscription.DataAccess.Kayak;

namespace Di.Subscription.DataAccess.DataAccess
{
    internal class KayakDataAccess : ISubscriptionDataAccess
    {
        private readonly ILogger _logger;
        private readonly KayakWebServiceSoapClient _client;
        private readonly IEndpointBehavior _endpointBehavior;

        public KayakDataAccess()
        {
            _logger = new Log4NetLogger();

            //Instatiate the KayakWebServiceSoapClient client and add the RequestInspectorBehavior to be able to log SOAP request and response
            _client = new KayakWebServiceSoapClient();
            _endpointBehavior = new RequestInspectorBehavior();
            _client.Endpoint.Behaviors.Add(_endpointBehavior);
        }

        public DataSet GetPapersAndProducts(string userId)
        {
            return CreateKayakRequest(kayakWsClient => kayakWsClient.GetPapersAndProducts_CII(userId));
        }

        public DataSet GetActiveCampaigns(string packageId)
        {
            return CreateKayakRequest(kayakWsClient => kayakWsClient.GetActiveCampaigns_CII(packageId));
        }

        public DataSet GetCampaign(long campaignNumber)
        {
            return CreateKayakRequest(kayakWsClient => kayakWsClient.GetCampaign(campaignNumber));
        }

        public DataSet GetCampaignSimple(string campaignId, DateTime priceDate)
        {
            return CreateKayakRequest(kayakWsClient => kayakWsClient.GetCampaignByCampOrGroupID(campaignId, string.Empty, priceDate));
        }

        #region Reclaims
        public DataSet GetReclaimTypes(string paperCode)
        {
            return CreateKayakRequest(kayakWsClient => kayakWsClient.GetReclaimHierarchy_CII(paperCode));
        }

        public DataSet GetCustomerReclaims(long customerNumber) 
        {
            return CreateKayakRequest(kayakWsClient => kayakWsClient.GetCustomerReclaims_CII(customerNumber));
        }

        public string CreateDeliveryReclaim(
            long customerNumber, 
            long subscriptionNumber, 
            int extNo, 
            string paperCode, 
            int reclaimItem,
            string reclaimChannel, 
            string reclaimKind, 
            DateTime publishDate, 
            bool creditSubscriber, 
            bool reclaimMessage,
            DateTime deliveryMessageDate, 
            string reclaimCode, 
            string reclaimText, 
            string responsiblePerson, 
            string language,
            string reclaimPaper, 
            string userId, 
            string doorCode)
        {
            return CreateKayakRequest(kayakWsClient => kayakWsClient.CreateDeliveryReclaim_CII(
                customerNumber,
                subscriptionNumber,
                extNo,
                paperCode,
                reclaimItem,
                reclaimChannel,
                reclaimKind,
                publishDate,
                creditSubscriber,
                reclaimMessage,
                deliveryMessageDate,
                reclaimCode,
                reclaimText,
                responsiblePerson,
                language,
                reclaimPaper,
                userId,
                doorCode));
        }

        #endregion

        #region HolidayStop

        public long CreateHolidayStop(
            string userId, 
            long subscriptionNumber, 
            DateTime startDate, 
            DateTime endDate,
            string sleepType, 
            string creditType, 
            string allowWebPaper, 
            string sleepReason, 
            string receiveType,
            string sleepLimit)
        {

            return CreateKayakRequest(kayakWsClient => kayakWsClient.CreateSubssleep_CII(
                    userId,
                    subscriptionNumber,
                    startDate,
                    endDate,
                    sleepType,
                    creditType,
                    allowWebPaper,
                    sleepReason,
                    receiveType,
                    sleepLimit));
        }

        public string DeleteHolidayStop(
            string userId, 
            long subscriptionNumber, 
            int externalNumber, 
            DateTime dateStart)
        {
            return CreateKayakRequest(kayakWsClient => kayakWsClient.DeleteHolidayStop_CII(userId, subscriptionNumber, externalNumber, dateStart));
        }

        public string ChangeHolidayStop(string userId, long subscriptionNumber, int externalNumber, DateTime dateStartOld, DateTime dateStartNew, DateTime dateEndOld, DateTime dateEndNew, string sleepType, string creditType, string allowWebPaper, bool renewSubscription, string sleepReason, string receiveType, string sleepLimit)
        {
            return CreateKayakRequest(kayakWsClient => kayakWsClient.ChangeHolidayStop_CII(userId, subscriptionNumber, externalNumber, dateStartOld, dateStartNew, dateEndOld, dateEndNew, sleepType, creditType, allowWebPaper, renewSubscription, sleepReason, receiveType, sleepLimit));
        }

        public DataSet GetHolidayStops(string userId, long subscriptionNumber, int externalNumber)
        {
            return CreateKayakRequest(kayakWsClient => kayakWsClient.GetSubsSleeps_CII(userId, subscriptionNumber, externalNumber));
        }

        #endregion

        #region AddressChange

        public string CreateTemporaryAddressChange(string userId, long customerNumber, long subscriptionNumber, int externalNumber,
            string streetAddress, string streetNo, string stairCase, string floor, string apartment, string street2, string street3, string countryCode, string zip,
            DateTime startDate, DateTime endDate, string name1, string name2, string invoiceToTemporaryAddress, string paperCode, string receiveType, bool saveAllPackageSubs)
        {
            return CreateKayakRequest(kayakWsClient => kayakWsClient.AddNewTemporaryAddress_CII(userId, customerNumber, subscriptionNumber, externalNumber,
                streetAddress, streetNo, stairCase, floor, apartment, street2, street3, countryCode, zip,
                startDate, endDate, invoiceToTemporaryAddress, name1, name2,
                paperCode, receiveType, saveAllPackageSubs));
        }

        public string ChangeTemporaryAddressChangeDates(string userId, long customerNumber, long subscriptionNumber, int externalNumber,
            DateTime oldStartDate, DateTime newStartDate, DateTime newEndDate, bool invoice, DateTime cusAddrEndDate,
            string receiveType)
        {
            return CreateKayakRequest(kayakWsClient => kayakWsClient.ChangeWaitingTemporaryAddressDates_CII(userId, customerNumber,subscriptionNumber, externalNumber, oldStartDate, newStartDate, newEndDate, invoice,cusAddrEndDate,receiveType));
        }

        public string DeleteTemporaryAddressChange(string userId, long customerNumber, long subscriptionNumber, int externalNumber, DateTime startDate)
        {
            return CreateKayakRequest(kayakWsClient => kayakWsClient.RemoveWaitingTemporaryAddress_CII(userId, customerNumber, subscriptionNumber, externalNumber, startDate));
        }

        public DataSet GetAllAddressChanges(long customerNumber, long subscriptionNumber)
        {
            return CreateKayakRequest(kayakWsClient => kayakWsClient.GetCurrentAndPendingAddressChanges(customerNumber, subscriptionNumber));
        }

        public string CreatePermanentAddressChange(string userId, long customerNumber,
            string streetAddress, string streetNo, string stairCase, string floor, string apartment, string street2, string street3, string countryCode, string zip,
            DateTime startDate, string tempName1, string tempName2, string receiveType, bool changeImmediately)
        {
            return CreateKayakRequest(kayakWsClient => kayakWsClient.AddNewBasicAddress_CII(userId, customerNumber, streetAddress, streetNo, stairCase, floor, apartment, street2,
                street3, countryCode, zip, startDate, tempName1, tempName2, receiveType, changeImmediately));
        }

        public string DeletePermanentAddressChange(string userId, long customerNumber, DateTime startDate)
        {
            return CreateKayakRequest(kayakWsClient => kayakWsClient.RemoveWaitingBasicAddress_CII(userId, customerNumber, startDate));
        }

        public DataSet GetCustomerTemporaryAddresses(long customerNumber, string onlyThisCountry, string onlyValid)
        {
            return CreateKayakRequest(kayakWsClient => kayakWsClient.GetCusTempAddresses(customerNumber, onlyThisCountry, onlyValid));
        }

        public string AddAddressChangeFee(
            long customerNumber, 
            long subscriptionNumber, 
            int externalNumber,
            bool basicAddressChange)
        {
            return
                CreateKayakRequest(
                    kayakWsClient =>
                        kayakWsClient.AddAddressChangeFee_CII(customerNumber, subscriptionNumber, externalNumber,
                            basicAddressChange));
        }

        #endregion

        #region Customer

        public DataSet GetCustomer(long customerNumber)
        {
            return CreateKayakRequest(kayakWsClient => kayakWsClient.GetCustomer_CII(customerNumber));
        }

        public DataSet FindCustomers(long customerNumber, long invoiceNumber, long subscriptionNumber, string sName1, string sName2,
            string sName3, string phone, string socialSecurityNumber, string email, string streetName, string streetNumber,
            string stairCase, string floor, string apartment, string country, string zipCode, string userId, string postName,
            string otherCustomerNumber, string orderId, string paperCode, string referenceNumber)
        {
            return CreateKayakRequest(kayakWsClient => kayakWsClient.FindCustomers_CII(customerNumber, invoiceNumber, subscriptionNumber, sName1, sName2, sName3, phone,
                socialSecurityNumber, email, streetName, streetNumber, stairCase, floor, apartment, country, zipCode,
                userId, postName, otherCustomerNumber, orderId, paperCode, referenceNumber));
        }

        public string UpdateCustomerName(long customerNumber, string rowText1, string rowText2, string rowText3, string firstName,
            string lastName, DateTime activeDate)
        {
            return
                CreateKayakRequest(
                    kayakWsClient =>
                        kayakWsClient.UpdateCustomerName_CII(customerNumber, rowText1, rowText2, rowText3, firstName,
                            lastName, activeDate));
        }

        public string UpdateCustomerInformation(
            long customerNumber,
            string email,
            string homePhone,
            string workPhone,
            string officePhone,
            string accountNumberBank,
            string accountNumberAccount,
            bool collectInv,
            string notes,
            long eCustomerNumber,
            string otherCustomerNumber,
            string wwwUserId,
            string expDay,
            string terms,
            string socialSecurityNumber,
            string category,
            long masterCustomerNumber,
            string marketingDenial,
            string marketingDenialReason,
            DateTime marketingDenialDate,
            string customerType,
            string title,
            bool protectedIdentity,
            string companyId,
            string extra1,
            string extra2,
            string extra3
            )
        {
            return CreateKayakRequest(kayakWsClient => kayakWsClient.UpdateCustomerInformation_CII(
                customerNumber,
                email,
                homePhone,
                workPhone,
                officePhone,
                accountNumberBank,
                accountNumberAccount,
                collectInv,
                notes,
                eCustomerNumber,
                otherCustomerNumber,
                wwwUserId,
                expDay,
                terms,
                socialSecurityNumber,
                category,
                masterCustomerNumber,
                marketingDenial,
                marketingDenialReason,
                marketingDenialDate,
                customerType,
                title,
                protectedIdentity,
                companyId,
                extra1,
                extra2,
                extra3));
        }

        public long GetCustomerNumberByEcusno(long eCustomerNumber)
        {
            return CreateKayakRequest(kayakWsClient => kayakWsClient.GetCustomerByEcusno_CII(eCustomerNumber));
        }

        public long GetEcusnoByCustomerNumber(long customerNumber)
        {
            return CreateKayakRequest(kayakWsClient => kayakWsClient.GetEcusnoByCustomer_CII(customerNumber));
        }

        #endregion

        #region Invoice

        public DataSet GetOpenInvoices(long customerNumber, string userId)
        {
            return CreateKayakRequest(kayakWsClient => kayakWsClient.GetOpenInvoices(customerNumber, userId));
        }

        #endregion

        public DataSet GetPublicationDays(string paperCode, string productNumber, DateTime firstDate, DateTime lastDate)
        {
            return CreateKayakRequest(kayakWsClient => kayakWsClient.GetPublDays(paperCode, productNumber, firstDate, lastDate));
        }

        public DataSet GetExtraProducts(long subscriptionCustomerNumber, string paperCode, DateTime orderDate)
        {
            return CreateKayakRequest(kayakWsClient => kayakWsClient.GetExtraProducts_CII(subscriptionCustomerNumber, paperCode, orderDate));
        }

        public DataSet GetSubscriptions(long customerNumber, string source, DateTime limitDate, string subscriptionNumber, string externalNumber)
        {
            return CreateKayakRequest(kayakWsClient => kayakWsClient.GetSubscriptions_CII(customerNumber, source, limitDate, subscriptionNumber, externalNumber));
        }

        public string GetPostName(string zipCode, string countryCode)
        {
            return CreateKayakRequest(kayakWsClient => kayakWsClient.GetPostName_CII(countryCode, zipCode));
        }

        public DataSet GetParameterValuesByGroup(string paperCode, string codeGroupNumber)
        {
            return CreateKayakRequest(kayakWsClient => kayakWsClient.GetParameterValuesByGroup(paperCode, codeGroupNumber));
        }

        public string InsertCustomerProperty(long customerNumber, string propertyCode, string propertyValue, string profileSourceGroup,
            string profileSourceSpecification)
        {
            return CreateKayakRequest(kayakWsClient => kayakWsClient.InsertCustomerProperty_CII(customerNumber, propertyCode, propertyValue, profileSourceGroup,
                profileSourceSpecification));
        }

        public string GetNextIssuedate(string papercode, string productNumber, DateTime minDate)
        {
            return CreateKayakRequest(kayakWsClient => kayakWsClient.GetNextIssuedate_CII(papercode, productNumber, minDate));
        }

        private TResponse CreateKayakRequest<TResponse>(Func<KayakWebServiceSoapClient, TResponse> kayakRequest)
        {
            try
            {
                var response = kayakRequest.Invoke(_client);

                LogRequestAndResponse(_endpointBehavior);

                return response;
            }
            catch (Exception exception)
            {
                _logger.Log(exception, "Kayak Request failed " + kayakRequest.Method + " " + kayakRequest.Target, LogLevel.Error, typeof(KayakDataAccess));
                throw;
            }
        }

        private void LogRequestAndResponse(IEndpointBehavior endpointBehavior)
        {
            var requestInspector = endpointBehavior as RequestInspectorBehavior;

            if (requestInspector == null)
            {
                _logger.Log("LogRequestAndResponse could not log, endpointBehavior could not be casted to RequestInspectorBehavior", LogLevel.Error, typeof(KayakDataAccess));
                return;
            }

            try
            {
                var requestXml = requestInspector.LastRequestXml;
                var responseXml = requestInspector.LastResponseXml;

                _logger.Log("Kayak request: " + requestXml, LogLevel.Debug, typeof(KayakDataAccess));
                _logger.Log("Kayak response: " + responseXml, LogLevel.Debug, typeof(KayakDataAccess));
            }
            catch (Exception exception)
            {
                _logger.Log(exception, "LogRequestAndResponse could not log", LogLevel.Error, typeof(KayakDataAccess));
            }
        }

        public DataSet GetCustomerProperties(long customerNumber)
        {
            return CreateKayakRequest(kayakWsClient => kayakWsClient.GetCustomerProperties_CII(customerNumber));
        }
    }
}
