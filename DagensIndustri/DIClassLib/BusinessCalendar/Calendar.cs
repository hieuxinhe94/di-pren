using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DIClassLib.BusinessCalendar
{
    public class Calendar
    {

        private Subscription _subs;


        public Calendar(Subscription subs)
        {
            _subs = subs;
        }


        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(@"BEGIN:VCALENDAR" + Environment.NewLine);
            sb.Append(@"VERSION:2.0" + Environment.NewLine);
            sb.Append(@"PRODID:-//Apple Computer\, Inc//iCal 1.0//EN" + Environment.NewLine);
            sb.Append(@"CALSCALE:GREGORIAN" + Environment.NewLine);

            foreach (EventInSubs ev in _subs.EventsInSubs)
            {
                foreach (CompanyEvent ce in BusCalHandler.CompanyEvents)
                {
                    if (BusCalHandler.EvInSubMatchesCompEv(ev, ce))
                    {
                        DateTime dt = DateTime.Now;
                        sb.Append(@"BEGIN:VEVENT"           + Environment.NewLine);
                        sb.Append(@"DTSTART;VALUE=DATE:"    + ce.DateEvent.ToString("yyyyMMdd") + Environment.NewLine);             //20110302
                        sb.Append(@"DTEND;VALUE=DATE:"      + ce.DateEvent.AddDays(1).ToString("yyyyMMdd") + Environment.NewLine);  //20110303
                        sb.Append(@"SUMMARY:"               + ce.Summary + Environment.NewLine);
                        sb.Append(@"DESCRIPTION:"           + ce.Description + Environment.NewLine);
                        sb.Append(@"UID:"                   + ce.EventId.ToString() + Environment.NewLine);
                        sb.Append(@"DTSTAMP:"               + dt.ToString("yyyyMMdd") + "T" + dt.ToString("hhmmss") + "Z" + Environment.NewLine);  //20041015T171054Z
                        sb.Append(@"END:VEVENT"             + Environment.NewLine);
                        break;
                    }
                }
            }

            sb.Append(@"END:VCALENDAR");

            return sb.ToString();
        }


    }
}
