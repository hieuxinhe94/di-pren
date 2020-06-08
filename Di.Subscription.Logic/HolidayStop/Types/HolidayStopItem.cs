using System;

namespace Di.Subscription.Logic.HolidayStop.Types
{
    public class HolidayStopItem
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
    }
}
