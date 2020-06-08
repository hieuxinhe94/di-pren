using System;

namespace Di.Subscription.Logic.Address.Types
{
    public class AddressChange
    {
        public bool IsOngoing
        {
            get
            {
                var now = DateTime.Now.Date;
                return (now >= StartDate && (now <= EndDate));
            }
        }

        public string Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string StreetAddress { get; set; }
        public string StreetNumber { get; set; }
        public string Street1 { get; set; }
        public string Street2 { get; set; }
        public string StairCase { get; set; }
        public string Apartment { get; set; }
        public string Zip { get; set; }
        public string City { get; set; }
    }
}
