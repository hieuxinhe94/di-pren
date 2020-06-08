using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DIClassLib.Subscriptions;

namespace DIClassLib.Customers
{
    public class Customer
    {
        public List<Addresses.Address> Addresses;
        public List<Subscription> Subscriptions;
        public List<Invoices.Invoice> Invoices;
        public SubscriberType SubscriberType;
    
        public bool IsSubscriber
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public bool IsPayer
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public long CusNo
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public string EmailAddressExpCust
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public string EmailAddressCirix
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public string PhoneHome
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }
    }
}
