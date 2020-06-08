namespace Di.Subscription.Logic.Customer.Types
{
    /// <summary>
    /// When updating a customer in Kayak you need to provide current values on the customers otherwise they get overrided
    /// This object contains properties that we need for the update but don't want to expose in our public Customer POCO object
    /// </summary>
    internal class CustomerForUpdate : Customer
    {
        public CustomerForUpdate(Customer customer)
        {
            CustomerNumber = customer.CustomerNumber;
            ECustomerNumber = customer.ECustomerNumber;
            CompanyName = customer.CompanyName;
            CompanyNumber = customer.CompanyNumber;
            FullName = customer.FullName;
            FirstName = customer.FirstName;
            LastName = customer.LastName;
            PhoneOffice = customer.PhoneOffice;
            PhoneWork = customer.PhoneWork;
            Email = customer.Email;
            AddressStreetName = customer.AddressStreetName;
            AddressStreetNumber = customer.AddressStreetNumber;
            AddressStairCase = customer.AddressStairCase;
            AddressStairs = customer.AddressStairs;
            AddressCareOf = customer.AddressCareOf;
            AddressZip = customer.AddressZip;
            AddressCity = customer.AddressCity;            
        }

        public string PhoneHome { get; set; }
        public string AccountNumberBank { get; set; }
        public string AccountNumberAccount { get; set; }
        public string Notes { get; set; }
        public string OtherCustomerNumber { get; set; }
        public string WwwUserId { get; set; }
        public string ExpDay { get; set; }
        public string Terms { get; set; }
        public string SocialSecurityNumber { get; set; }
        public string Category { get; set; }
        public long MasterCustomerNumber { get; set; }
        public string CustomerType { get; set; }
    }
}
