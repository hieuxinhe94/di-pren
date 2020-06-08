using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DIClassLib.DbHandlers;
using System.Collections;
using System.IO;


namespace DIClassLib.BusinessCalendar
{
    public static class BusCalHandler
    {

        private static List<Group> _groups = new List<Group>();
        private static List<Company> _companys = new List<Company>();
        private static List<Subscription> _subscriptions = new List<Subscription>();

        private static List<CompanyEvent> _compEvsMillistream = new List<CompanyEvent>();
        private static List<CompanyEvent> _compEvsDiConference = new List<CompanyEvent>();
        private static List<CompanyEvent> _compEvsDiGasell = new List<CompanyEvent>();
        private static List<CompanyEvent> _compEvsDiEvents = new List<CompanyEvent>();

        private static int _hoursValidCompanys = 24;
        private static int _hoursValidCompanyEvents = 5;
        private static int _minutesValidSubscriptions = 10;

        private static DateTime _lastCallDiCompanyEvents = DateTime.Now.AddMinutes(-100);
        private static DateTime _lastUpdateCompanys = DateTime.Now;
        private static DateTime _lastUpdateCompanyEvents = DateTime.Now;
        private static DateTime _lastUpdateSubscriptions = DateTime.Now;
        


        public static List<Group> Groups
        {
            get
            {
                if (_groups == null || _groups.Count == 0)
                    _groups = BusCalDbHandler.GetGroups();

                return _groups;
            }
        }

        public static List<Company> Companys
        {
            get
            {
                DateTime dt = DateTime.Now;

                if (_companys == null || _companys.Count == 0 || _lastUpdateCompanys.AddHours(_hoursValidCompanys) < dt)
                {
                    _lastUpdateCompanys = dt;
                    _companys = BusCalDbHandler.GetCompanysFromMS();

                    if (_companys.Count > 0)
                    {
                        //update DiGuldCalendarData.js file
                        new CompanysInJsHandler(_companys);
                    }
                }

                return _companys;
            }
        }

        /// <summary>
        /// Millistream and DI company events
        /// </summary>
        public static List<CompanyEvent> CompanyEvents
        {
            get
            {
                //count=0 should not occur here
                if (_compEvsMillistream == null || _compEvsMillistream.Count == 0 || HasTimedOut("compEvs"))
                    _compEvsMillistream = BusCalDbHandler.GetCompEvsMillistream();

                //count=0 can occur for DI-events - make db-calls only every 10 minutes
                if (_lastCallDiCompanyEvents.AddMinutes(10) < DateTime.Now)
                {
                    if (_compEvsDiConference == null || _compEvsDiConference.Count == 0 || HasTimedOut("compEvs"))
                        _compEvsDiConference = BusCalDbHandler.GetCompEvsDiConference();

                    if (_compEvsDiGasell == null || _compEvsDiGasell.Count == 0 || HasTimedOut("compEvs"))
                        _compEvsDiGasell = BusCalDbHandler.GetCompEvsDiGasell();

                    //if (_compEvsDiEvents == null || _compEvsDiEvents.Count == 0 || HasTimedOut("compEvs"))
                    //    _compEvsDiEvents = BusCalDbHandler.GetCompEvsDiEvents();

                    _lastCallDiCompanyEvents = DateTime.Now;
                }


                if (HasTimedOut("compEvs"))
                    _lastUpdateCompanyEvents = DateTime.Now;

                
                List<CompanyEvent> evs = new List<CompanyEvent>();
                evs.AddRange(_compEvsMillistream);
                evs.AddRange(_compEvsDiConference);
                evs.AddRange(_compEvsDiGasell);
                evs.AddRange(_compEvsDiEvents);

                return evs;
            }
        }

        private static bool HasTimedOut(string obj)
        {
            DateTime dt = DateTime.Now;

            if (obj == "compEvs")
                if (_lastUpdateCompanyEvents.AddHours(_hoursValidCompanyEvents) < dt)
                    return true;

            return false;
        }


        public static List<Subscription> Subscriptions
        {
            get
            {
                DateTime dt = DateTime.Now;

                if (_lastUpdateSubscriptions.AddMinutes(_minutesValidSubscriptions) < dt)
                {
                    _lastUpdateSubscriptions = dt;
                    _subscriptions = new List<Subscription>();
                    BusCalDbHandler.DoLog("Life span for subscriptions list timed out. Num subs in list when cleared:" + _subscriptions.Count.ToString());
                }

                return _subscriptions;
            }
        }

        public static string GetGroupName(int groupId)
        {
            foreach (Group gr in Groups)
            {
                if(gr.GroupId == groupId)
                    return gr.GroupName;
            }

            return "Okänd grupp";
        }

        public static string GetGroupName(int? typeId, int? subTypeId)
        {
            if (typeId < 0)
                subTypeId = null;

            foreach (Group gr in Groups)
            {
                foreach (TypeSubType tSt in gr.TypesSubTypes)
                {
                    if (tSt.TypeId == typeId && tSt.SubTypeId == subTypeId)
                        return gr.GroupName;
                }
            }

            return "Okänd grupp";
        }


        public static string GetTypeName(int? typeId, int? subTypeId)
        {   
            foreach(Group gr in Groups)
            {
                foreach (TypeSubType t in gr.TypesSubTypes)
                {
                    if (t.TypeId == typeId && t.SubTypeId == subTypeId)
                        return t.TypeName;
                }
            }

            return "Okänd händelse";
        }


        public static Subscription GetSubscription(string userId)
        {
            ArrayList al = BusCalDbHandler.GetCusnoAndSubsId(userId);
            
            int cusno = int.Parse(al[0].ToString());
            Guid? subsId = null;
            if(al[1] != null)
                subsId = new Guid(al[1].ToString());
            
            //cust has no sub - add to db and memory
            if (subsId == null)
            {
                Subscription sub = new Subscription(Guid.NewGuid(), cusno);
                BusCalDbHandler.InsertSubscription(sub);
                Subscriptions.Add(sub);
                return sub;
            }

            return GetSubscription((Guid)subsId);
        }


        public static Subscription GetSubscription(Guid subsId)
        {

            //try get from memory
            foreach (Subscription sub in Subscriptions)
            {
                if (sub.SubsId == subsId)
                {
                    //BusCalDbHandler.DoLog("GetSubscription - read from memory");
                    return sub;
                }
            }

            //get from mssql + add to memory
            Subscription subNew = BusCalDbHandler.GetSubscription(subsId);
            if(subNew != null)
                Subscriptions.Add(subNew);

            //BusCalDbHandler.DoLog("GetSubscription - read from db");

            return subNew;  //might be null
        }


        public static void AddEventsToSubs(Subscription sub, List<EventInSubs> listAdd)
        {
            foreach (EventInSubs ev in listAdd)
                BusCalDbHandler.InsertEventInSubs(sub.SubsId, ev);  //insert in db

            sub.EventsInSubs.AddRange(listAdd);                     //add to memory
        }


        public static void DeleteEventInSubs(Subscription sub, EventInSubs ev)
        {
            BusCalDbHandler.DeleteEventInSubs(sub.SubsId, ev);  //delete from db
            RemoveEventFromMemory(sub, ev);                     //remove from memory
        }
        
        private static void RemoveEventFromMemory(Subscription sub, EventInSubs ev)
        {
            foreach (EventInSubs e in sub.EventsInSubs)
            {
                if (e.CompanyId == ev.CompanyId && e.TypeId == ev.TypeId && e.SubTypeId == ev.SubTypeId)
                {
                    sub.EventsInSubs.Remove(e);
                    return;
                }
            }
        }


        public static string GetCompanyName(int compId)
        {
            if (compId == BusCalSettings.DiCompanyId)
                return BusCalSettings.DiCompanyName;
            
            foreach (Company c in Companys)
                if (c.CompanyId == compId)
                    return c.Name;

            return "Okänt företag";
        }

        public static CompanyEvent GetCompanyEvent(EventInSubs ev)
        {
            foreach (CompanyEvent ce in CompanyEvents)
            {
                if (EvInSubMatchesCompEv(ev, ce))
                    return ce;
            }

            return null;
        }

        public static bool EvInSubMatchesCompEv(EventInSubs ev, CompanyEvent ce)
        {
            if (ev.CompanyId == ce.CompanyId &&
                ev.TypeId == ce.TypeId &&
                ev.SubTypeId == ce.SubTypeId)
                return true;

            return false;
        }


        #region old
        //public static void SaveSubscription(Subscription sub)
        //{
        //    DeleteSubscription(sub.SubsId);             //if exixts - delete
        //    BusCalDbHandler.InsertSubscription(sub);    //insert into db
        //    Subscriptions.Add(sub);                     //add to memory

        //    BusCalDbHandler.DoLog("Subscription saved to db and memory, subsId:" + sub.SubsId.ToString() + 
        //                          ", EventsInSubs.Count:" + sub.EventsInSubs.Count.ToString());
        //}

        //public static void DeleteSubscription(Guid subsId)
        //{
        //    //delete from db
        //    BusCalDbHandler.DeleteSubscription(subsId);

        //    //remove from memory
        //    foreach (Subscription sub in Subscriptions)
        //    {
        //        if (sub.SubsId == subsId)
        //        {
        //            Subscriptions.Remove(sub);
        //            return;
        //        }
        //    }
        //}
        #endregion


    }
}
