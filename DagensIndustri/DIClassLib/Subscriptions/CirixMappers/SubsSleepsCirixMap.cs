using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DIClassLib.DbHelpers;


namespace DIClassLib.Subscriptions.CirixMappers
{
    [Serializable]
    public class SubsSleepsCirixMap : IComparable<SubsSleepsCirixMap>
    {
        public string Id { get { return Subsno + "_" + SleepStartDate.ToString() + "_" + SleepEndDate.ToString(); } }

        public long Subsno { get; set; }
        public DateTime SleepStartDate { get; set; }
        public DateTime SleepEndDate { get; set; }
        public string CreditType { get; set; }
        public double CreditAmout { get; set; }
        public string SleepType { get; set; }
        public string Credited { get; set; }
        public long CreditInvNo { get; set; }
        public DateTime BookingDate { get; set; }
        public string AllowWebPaper { get; set; }

        public bool CanBeEdited
        {
            get
            {
                return !IsInProgress;
            }
        }
        public bool CanBeDeleted
        {
            get
            {
                return !IsInProgress;
            }
        }
        private bool IsInProgress
        {
            get
            {
                DateTime now = DateTime.Now.Date;
                return (now >= SleepStartDate && (now <= SleepEndDate || SleepEndDate == CirixDateNotSet));
            }
        }

        //"tillsvidare" subs sleeps has this end date in Cirix
        public DateTime CirixDateNotSet { get { return new DateTime(1800, 1, 1); } }

        public string Comment
        {
            get
            {
                string s = string.Empty;

                if (IsInProgress)
                    s = "Uppehåll påbörjat. Kontakta kundtjänst för ändringar.";

                return s;
            }
        }
        public bool HasComment
        {
            get { return (Comment.Length > 0); }
        }


        #region not used props
        //"STAMP_USER"
        //"STAMP_DATE"
        //"STAMP_PROGRAM"
        #endregion

        public SubsSleepsCirixMap() { }

        public SubsSleepsCirixMap(DataRow dr)
        {
            //try
            //{
                Subsno = long.Parse(dr["SUBSNO"].ToString());
                SleepStartDate = (DateTime)dr["SLEEPSTARTDATE"];
                SleepEndDate = (DateTime)dr["SLEEPENDDATE"];
                CreditType = dr["CREDITTYPE"].ToString();
                CreditAmout = double.Parse(dr["CREDITAMOUNT"].ToString());
                SleepType = dr["SLEEPTYPE"].ToString();
                Credited = dr["CREDITED"].ToString();
                CreditInvNo = long.Parse(dr["CREDIT_INVNO"].ToString());
                BookingDate = (DateTime)dr["BOOKINGDATE"];
                AllowWebPaper = dr["ALLOW_WEBPAPER"].ToString();
            //}
            //catch (Exception ex)
            //{
            //    new Logger("SubsSleepsCirixMap() failed for subsno:" + Subsno, ex.ToString());
            //}
        }


        /// <summary>
        /// used when List<SubsSleepsCirixMap>.Sort() is called
        /// </summary>
        public int CompareTo(SubsSleepsCirixMap other)
        {
            return SleepStartDate.CompareTo(other.SleepStartDate);  //ASC
            //return -xxx.CompareTo(other.xxx);   //DESC
        }

        public bool AreEqual(SubsSleepsCirixMap map)
        {
            if (map.Subsno == Subsno &&
                map.SleepStartDate == SleepStartDate &&
                map.SleepEndDate == SleepEndDate &&
                CompareStrings(map.CreditType, CreditType) &&
                map.CreditAmout == CreditAmout &&
                CompareStrings(map.SleepType, SleepType) &&
                CompareStrings(map.Credited, Credited) &&
                map.CreditInvNo == CreditInvNo &&
                map.BookingDate == BookingDate &&
                CompareStrings(map.AllowWebPaper, AllowWebPaper))
                return true;

            return false;
        }

        private bool CompareStrings(string s1, string s2)
        {
            if (string.IsNullOrEmpty(s1))
                s1 = "";

            if (string.IsNullOrEmpty(s2))
                s2 = "";

            return (s1.ToUpper().Trim() == s2.ToUpper().Trim());
        }

    }
}
