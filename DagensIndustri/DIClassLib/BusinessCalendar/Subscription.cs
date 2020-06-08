using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DIClassLib.DbHandlers;


namespace DIClassLib.BusinessCalendar
{
    public class Subscription
    {
        public Guid SubsId;
        public int Cusno = 0;
        public DateTime DateSaved = DateTime.MinValue;
        public bool Disabled = true;
        public List<EventInSubs> EventsInSubs = new List<EventInSubs>();
        public Calendar Calendar
        {
            get { return new Calendar(this); }
        }


        public Subscription() { }

        public Subscription(Guid subsId, int cusno)
        {
            SubsId = subsId;
            Cusno = cusno;
            Disabled = false;
        }


        public HashSet<string> GetGroupNames(int companyId)
        {
            HashSet<string> uniqueGroups = new HashSet<string>();

            foreach (EventInSubs ev in EventsInSubs)
            {
                if (ev.CompanyId == companyId)
                    uniqueGroups.Add(BusCalHandler.GetGroupName(ev.TypeId, ev.SubTypeId));
            }

            return uniqueGroups;
        }

        public IEnumerable<int> GetCompanyIds()
        {
            HashSet<int> uniqueIds = new HashSet<int>();
            
            foreach (EventInSubs ev in EventsInSubs)
                uniqueIds.Add(ev.CompanyId);

            return uniqueIds;
        }

        public IEnumerable<EventInSubs> GetDiEvents()
        {
            List<EventInSubs> evs = new List<EventInSubs>();
            
            foreach (EventInSubs ev in EventsInSubs)
            {
                if (ev.CompanyId == BusCalSettings.DiCompanyId)
                    evs.Add(ev);
            }

            return evs;
        }
    }
}
