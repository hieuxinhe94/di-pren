using System;
using System.Collections.Generic;
using Di.Subscription.Logic.PublicationDays.Types;

namespace Di.Subscription.Logic.PublicationDays
{
    public interface IPublicationDaysHandler
    {
        IEnumerable<PublicationDay> GetPublicationDays(string productNumber, DateTime startDate, DateTime endDate);
    }
}