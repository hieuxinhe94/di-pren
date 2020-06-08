using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace DIClassLib.Wine
{
    /// <summary>
    /// Summary description for IsoWeek
    /// </summary>
    [Serializable]
    public class IsoWeek
    {
        DateTime _firstDateOfWeek = DateTime.MinValue;
        int _year = -1;
        int _week = -1;

        public IsoWeek()
        {
            SetIsoWeek(DateTime.Now);
        }

        public IsoWeek(DateTime dt)
        {
            SetIsoWeek(dt);
        }

        public IsoWeek(int year, int week)
        {
            _year = year;
            _week = week;
            DateTime firstWeekOfYear = GetIsoWeekOne(_year);
            _firstDateOfWeek = firstWeekOfYear.AddDays(7 * (_week - 1));
        }

        public int Year
        {
            get { return _year; }
        }

        public int Week
        {
            get { return _week; }
        }

        public DateTime Monday
        {
            get { return Weekday(1); }
        }

        public DateTime Tuesday
        {
            get{ return Weekday(2); }
        }

        public DateTime Wednesday
        {
            get { return Weekday(3); }
        }

        public DateTime Thursday
        {
            get { return Weekday(4); }
        }

        public DateTime Friday
        {
            get { return Weekday(5); }
        }

        public DateTime Saturday
        {
            get { return Weekday(6); }
        }

        public DateTime Sunday
        {
            get { return Weekday(7); }
        }

        public DateTime Weekday(int dayOfWeek)
        {
            return _firstDateOfWeek.AddDays(dayOfWeek-1);
        }

        public IsoWeek NextWeek
        {
            get
            {
                IsoWeek nextWeek = new IsoWeek(_firstDateOfWeek.AddDays(7));
                return nextWeek;
            }
        }

        public IsoWeek PreviousWeek
        {
            get
            {
                IsoWeek previousWeek = new IsoWeek(_firstDateOfWeek.AddDays(-7));
                return previousWeek;
            }
        }

        public DateTime FirstDateOfWeek
        {
            get { return _firstDateOfWeek; }
        }

        public new String ToString()
        {
            return "" + Year + " / " + Week;
        }

        #region private Methods

        private void SetIsoWeek(DateTime dt)
        {
            DateTime firstWeekOfYear;
            int IsoYear = dt.Year;
            if (dt >= new DateTime(IsoYear, 12, 29))
            {
                firstWeekOfYear = GetIsoWeekOne(IsoYear + 1);
                if (dt < firstWeekOfYear)
                {
                    firstWeekOfYear = GetIsoWeekOne(IsoYear);
                }
                else
                {
                    IsoYear++;
                }
            }
            else
            {
                firstWeekOfYear = GetIsoWeekOne(IsoYear);
                if (dt < firstWeekOfYear)
                {
                    firstWeekOfYear = GetIsoWeekOne(--IsoYear);
                }
            }
            _year = IsoYear;
            _week = (dt - firstWeekOfYear).Days / 7 + 1;
            _firstDateOfWeek = firstWeekOfYear.AddDays(7 * (_week-1));
            //return (IsoYear * 100) + ((dt - week1).Days / 7 + 1);
        }

        private DateTime GetIsoWeekOne(int Year)
        {
            // get the date for the 4-Jan for this year
            DateTime dt = new DateTime(Year, 1, 4);

            // get the ISO day number for this date 1==Monday, 7==Sunday
            int dayNumber = (int)dt.DayOfWeek; // 0==Sunday, 6==Saturday
            if (dayNumber == 0)
            {
                dayNumber = 7;
            }

            // return the date of the Monday that is less than or equal
            // to this date
            return dt.AddDays(1 - dayNumber);
        }
        
        #endregion

    }
}
