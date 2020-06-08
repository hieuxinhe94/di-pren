using System;
using System.Collections.Generic;

namespace Di.Subscription.DataAccess.PublicationDays
{
    public interface IPublicationDaysRepository
    {
        IEnumerable<PublicationDay> GetPublicationDays(
            string paperCode, 
            string productNumber, 
            DateTime firstDate,
            DateTime lastDate);
    }
}