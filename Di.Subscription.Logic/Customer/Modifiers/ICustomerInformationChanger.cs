namespace Di.Subscription.Logic.Customer.Modifiers
{
    internal interface ICustomerInformationChanger
    {
        string UpdateCustomerInformation(
            long customerNumber,
            string email,
            string homePhone,
            string workPhone,
            string officePhone,
            string accountNumberBank,
            string accountNumberAccount,
            string notes,
            long eCustomerNumber,
            string otherCustomerNumber,
            string wwwUserId,
            string expDay,
            string terms,
            string socialSecurityNumber,
            string category,
            long masterCustomerNumber,
            string customerType,
            string companyId);

        string UpdateCustomerName(long customerNumber, string firstName, string lastName);
        string UpdateCustomerEmail(long customerNumber, string email);
        string UpdateCustomerPhone(long customerNumber, string phone);
    }
}
