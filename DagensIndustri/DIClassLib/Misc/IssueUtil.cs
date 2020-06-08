using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DIClassLib.DbHandlers;

namespace DIClassLib.Misc
{
    public class IssueUtil
    {
        /// <summary>
        /// The current user has pre access to the next edition (Gold members has access between 22 and 04 to the next edition)
        /// </summary>
        public bool HasNextIssuePreAccess { get; set; }

        public String PaperCode { get; set; }
        public String ProductNo { get; set; }

        DateTime _refTime = DateTime.Now;
        DateTime _nextIssueDate = DateTime.MinValue;
        DateTime _currentIssueDate = DateTime.MinValue;

        // The hour on the day before when di guld members have access
        private static readonly int PRE_NEXT_EDITION_HOUR = 21;

        // the hour when the next edition becomes the current edition
        private static readonly int NEXT_EDITION_RELEASE_HOUR = 4;

        public IssueUtil(bool hasNextissuePreAccess, String paperCode, String productNo)
        {
            HasNextIssuePreAccess = hasNextissuePreAccess;
            PaperCode = paperCode;
            ProductNo = productNo;
        }

        public IssueUtil(bool hasNextEditionPreAccess, String paperCode, String productNo, DateTime referenceTime)
        {
            HasNextIssuePreAccess = hasNextEditionPreAccess;
            PaperCode = paperCode;
            ProductNo = productNo;
            _refTime = referenceTime;

        }

        /// <summary>
        /// Gets the date to the next issue. The next issue is "released" at 4am
        /// </summary>
        public DateTime NextIssueDate
        {
            get
            {
                if (_nextIssueDate == DateTime.MinValue)
                {
                    DateTime dt = _refTime;

                    if (PaperCode == Settings.PaperCode_DI && _refTime.Hour < NEXT_EDITION_RELEASE_HOUR)
                    {
                        // between 0 and 4, use the day before the reference date for DI
                        //
                        dt = dt.AddDays(-1);
                    }

                    _nextIssueDate = NextIssueExistsInDigIssues();

                    if (_nextIssueDate == DateTime.MinValue)
                        _nextIssueDate = SubscriptionController.GetIssueDate(PaperCode, ProductNo, dt, EnumIssue.Issue.FirstAfterInDate);
                }

                return _nextIssueDate;
            }
        }

        private DateTime NextIssueExistsInDigIssues()
        {
            var dt = _refTime.AddDays(1);

            if (dt.Hour < NEXT_EDITION_RELEASE_HOUR)
                dt = dt.AddDays(-1);

            if (Settings.DigitalIssues.Contains(dt.Date))
                return dt.Date;

            return DateTime.MinValue;
        }

        public DateTime CurrentIssueDate
        {
            get
            {
                if (_currentIssueDate == DateTime.MinValue)
                {
                    DateTime dt = _refTime;
                    if (PaperCode == Settings.PaperCode_DI && _refTime.Hour < NEXT_EDITION_RELEASE_HOUR)
                    {
                        // between 0 and 4, use the day before the reference date for DI
                        //
                        dt = dt.AddDays(-1);
                    }

                    _currentIssueDate = IssueExistsInDigIssues();

                    if (_currentIssueDate == DateTime.MinValue)
                        _currentIssueDate = SubscriptionController.GetIssueDate(PaperCode, ProductNo, dt, EnumIssue.Issue.InDateOrFirstBeforeInDate);
                }
                return _currentIssueDate;
            }
        }

        private DateTime IssueExistsInDigIssues()
        {
            var dt = _refTime;

            if (dt.Hour < NEXT_EDITION_RELEASE_HOUR)
                dt = dt.AddDays(-1);

            if (Settings.DigitalIssues.Contains(dt.Date))
                return dt.Date;

            return DateTime.MinValue;
        }

        /// <summary>
        /// Does the user have access to the next issue
        /// </summary>
        public bool HasAccessToNextIssue
        {
            get
            {
                if (HasNextIssuePreAccess)
                {
                    TimeSpan ts = TimeToNextIssueAccess;
                    if (ts.TotalHours <= 0 && ts.Hours > -(24 - PRE_NEXT_EDITION_HOUR + NEXT_EDITION_RELEASE_HOUR))
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// Time left until di guld members have access to the issue
        /// </summary>
        public TimeSpan TimeToNextIssueAccess
        {
            get
            {
                return NextIssueDate.AddHours(PRE_NEXT_EDITION_HOUR-24).Subtract(_refTime);
            }
        }

        public DateTime CurrentIssueDateWeekend
        {
            get
            {
                return MsSqlHandler.GetCurrentEditonDateWeekend();
            }
        }

        /// <summary>
        /// Gets the current DI Dimension issue date
        /// </summary>
        public DateTime CurrentIssueDateDimension
        {
            get
            {
                return MsSqlHandler.GetCurrentEditonDateDimension();
            }
        }

        public DateTime CurrentIssueDateDiIdea
        {
            get
            {
                return MsSqlHandler.CurrentIssueDateDiIdea();
            }
        }
    }
}
