using System;
using System.Data;

namespace Di.Subscription.DataAccess.DataAccess
{
    internal interface ISubscriptionDataAccess
    {
        DataSet GetReclaimTypes(string paperCode);
        DataSet GetCustomerReclaims(long customerNumber);

        string CreateDeliveryReclaim(
            long customerNumber,
            long subscriptionNumber,
            int extNo, //todo: change to more descriptive variable name
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
            string doorCode);

        DataSet GetPapersAndProducts(string userId);

        DataSet GetActiveCampaigns(string packageId);

        DataSet GetCampaign(long campaignNumber);

        DataSet GetCampaignSimple(string campaignId, DateTime priceDate);

        #region HolidayStop

        long CreateHolidayStop(string userId, long subscriptionNumber, DateTime startDate, DateTime endDate,
            string sleepType, string creditType, string allowWebPaper, string sleepReason, string receiveType,
            string sleepLimit);

        string DeleteHolidayStop(string userId, long subscriptionNumber, int externalNumber, DateTime dateStart);

        string ChangeHolidayStop(string userId, long subscriptionNumber, int externalNumber, DateTime dateStartOld,
            DateTime dateStartNew, DateTime dateEndOld, DateTime dateEndNew, string sleepType, string creditType,
            string allowWebPaper, bool renewSubscription, string sleepReason, string receiveType, string sleepLimit);

        DataSet GetHolidayStops(string userId, long subscriptionNumber, int externalNumber);

        #endregion

        #region Address change

        string CreateTemporaryAddressChange(string userId, long customerNumber, long subscriptionNumber, int externalNumber,
            string streetAddress, string streetNo, string stairCase, string floor, string apartment, string street2, string street3,
            string countryCode, string zip,
            DateTime startDate, DateTime endDate, string name1, string name2, string invoiceToTemporaryAddress,
            string paperCode, string receiveType, bool saveAllPackageSubs);

        string ChangeTemporaryAddressChangeDates(string userId, long customerNumber, long subscriptionNumber,
            int externalNumber,
            DateTime oldStartDate, DateTime newStartDate, DateTime newEndDate, bool invoice, DateTime cusAddrEndDate,
            string receiveType);

        string DeleteTemporaryAddressChange(string userId, long customerNumber, long subscriptionNumber,
            int externalNumber, DateTime startDate);

        DataSet GetAllAddressChanges(long customerNumber, long subscriptionNumber);

        string CreatePermanentAddressChange(string userId, long customerNumber,
            string streetAddress, string streetNo, string stairCase, string floor, string apartment, string street2, string street3,
            string countryCode, string zip,
            DateTime startDate, string tempName1, string tempName2, string receiveType,
            bool changeImmediately);

        string DeletePermanentAddressChange(string userId, long customerNumber, DateTime startDate);

        DataSet GetCustomerTemporaryAddresses(long customerNumber, string onlyThisCountry, string onlyValid);

        string AddAddressChangeFee(
            long customerNumber,
            long subscriptionNumber,
            int externalNumber,
            bool basicAddressChange);

        #endregion

        long GetCustomerNumberByEcusno(long eCustomerNumber);

        long GetEcusnoByCustomerNumber(long customerNumber);

        DataSet GetCustomer(long customerNumber);

        DataSet FindCustomers(long customerNumber, long invoiceNumber, long subscriptionNumber, string sName1, string sName2,
            string sName3, string phone, string socialSecurityNumber, string email, string streetName, string streetNumber,
            string stairCase, string floor, string apartment, string country, string zipCode, string userId, string postName,
            string otherCustomerNumber, string orderId, string paperCode, string referenceNumber);

        string UpdateCustomerName(long customerNumber, string rowText1, string rowText2, string rowText3, string firstName, string lastName, DateTime activeDate);

        string UpdateCustomerInformation(
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
            string extra3);

        DataSet GetPublicationDays(string paperCode, string productNumber, DateTime firstDate, DateTime lastDate);

        DataSet GetExtraProducts(long subscriptionCustomerNumber, string paperCode, DateTime orderDate);

        DataSet GetSubscriptions(long customerNumber, string source, DateTime limitDate, string subscriptionNumber, string externalNumber);

        string GetPostName(string zipCode, string countryCode);

        DataSet GetParameterValuesByGroup(string paperCode, string codeGroupNumber);

        string InsertCustomerProperty(long customerNumber, string propertyCode, string propertyValue,
            string profileSourceGroup,
            string profileSourceSpecification);

        string GetNextIssuedate(string papercode, string productNumber, DateTime minDate);

        DataSet GetOpenInvoices(long customerNumber, string userId);

        DataSet GetCustomerProperties(long customerNumber);
    }
}