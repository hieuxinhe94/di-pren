namespace Di.Subscription.Logic.Customer.Modifiers
{
    public interface ICustomerPropertyChanger
    {
        bool InsertUpdateCustomerProperty(long customerNumber, string propertyCode, string propertyValue); 
    }
}