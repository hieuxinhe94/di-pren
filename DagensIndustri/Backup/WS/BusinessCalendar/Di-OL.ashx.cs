using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using DIClassLib.BusinessCalendar;
using DIClassLib.DbHelpers;


namespace WS.BusinessCalendar
{
    /// <summary>
    /// Summary description for BusinessCalendar
    /// </summary>
    public class BusinessCalendar : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            //context.Response.Write("Hello World");
            //context.Response.ContentType = "text/x-vCalendar";

            string id = context.Request.QueryString["id"];

            context.Response.ContentType = "text/plain";
            //context.Response.Write("Content-Type: text/x-vCalendar" + Environment.NewLine);
            //context.Response.Write("Content-Disposition: inline; filename=BusinessCalendar.ics" + Environment.NewLine);
            context.Response.Write(PrintCalendar(id));
        }



        private string PrintCalendar(string id)
        {
            Guid g;
            
            try   { g = new Guid(id); }
            catch { return PrintCalendarError(); }
                 
            try
            {
                //Guid g = new Guid(id);
                Subscription sub = BusCalHandler.GetSubscription(g);

                if (sub != null)
                    return sub.Calendar.ToString();

            }
            catch (Exception ex)
            {
                new Logger("PrintCalendar() - failed for id=" + id, ex.ToString());
            }

            return PrintCalendarError();
        }


        private string PrintCalendarError()
        {
            StringBuilder sb = new StringBuilder();

            DateTime dtStart = DateTime.Now;
            DateTime dtEnd = dtStart.AddDays(1);

            sb.Append(@"BEGIN:VCALENDAR" + Environment.NewLine);
            sb.Append(@"VERSION:2.0" + Environment.NewLine);
            sb.Append(@"PRODID:-//Apple Computer\, Inc//iCal 1.0//EN" + Environment.NewLine);
            sb.Append(@"METHOD:REQUEST" + Environment.NewLine);
            sb.Append(@"CALSCALE:GREGORIAN" + Environment.NewLine);

            sb.Append(@"BEGIN:VEVENT" + Environment.NewLine);
            sb.Append(@"DTSTART;VALUE=DATE:" + dtStart.ToString("yyyyMMdd") + Environment.NewLine); //20020331
            sb.Append(@"DTEND;VALUE=DATE:" + dtEnd.ToString("yyyyMMdd") + Environment.NewLine);     //20020401
            sb.Append(@"SUMMARY:" + "DI kalender - tekniskt fel" + Environment.NewLine);
            sb.Append(@"DESCRIPTION:" + "Kalenderdata kunde inte hämtas." + Environment.NewLine);
            sb.Append(@"UID:" + Guid.NewGuid().ToString() + Environment.NewLine);
            sb.Append(@"DTSTAMP:" + dtStart.ToString("yyyyMMdd") + "T" + dtStart.ToString("hhmmss") + "Z" + Environment.NewLine);  //20041015T171054Z
            sb.Append(@"END:VEVENT" + Environment.NewLine);

            sb.Append(@"END:VCALENDAR");

            return sb.ToString();
        }


        public bool IsReusable
        {
            get { return false; }
        }
    }
}