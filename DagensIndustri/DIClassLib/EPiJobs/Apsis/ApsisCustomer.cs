using System;
using System.Linq;
using System.Collections.Generic;
using DIClassLib.Misc;

namespace DIClassLib.EPiJobs.Apsis
{
    public class ApsisCustomer
    {
        //values saved in mssql customer
        public int CustomerId;
        public string ApsisProjectGuid;     //customer categoriy (decides what mail template to use)
        public string Name;
        public string Email;
        public string UserName;
        public string PassWord;
        public DateTime DateSaved;
        public DateTime DateUpdated;
        public DateTime DateRegularLetter;  //not NULL: regular letter sent, customer not displayed as bounce and won´t be added to batch
        public bool IsBounce;               //true: apsis reported mail as bounce
        public bool ForceRetry;             //true: will include customer in batch
        public bool IsSendSuccess;
        public string TargetGroup;
        public string CampId;
        public int CampGroupId;
        //public DateTime SubsStartDate;
        public DateTime InvStartDate;
        public DateTime SubsEndDate;
        public string ContactStatus;
        public int ExtNo;
        public int? CampNo;
        public string ReceiveType;
        public bool IsExtCustomer;          //true: bounce handled by external resource (not displayed on EPi bounce page)
        public DateTime DateEmailUpdatedByExt;
        public DateTime DateRejectedByExt;
        public bool HaveServicePlusAccount;
        public int SubsLenMonsFromCirix; //TODO: SubsLenMons could probably be replaced by this?
        public int SubsNoForConfirm;

        private int SubsLenMons 
        {
            get
            {
                if (InvStartDate != null && InvStartDate > DateTime.MinValue && SubsEndDate != null && SubsEndDate > DateTime.MinValue)
                    return (int)Microsoft.VisualBasic.DateAndTime.DateDiff(Microsoft.VisualBasic.DateInterval.Month, InvStartDate, SubsEndDate);

                return 0;
            }
        }

        private bool IsDigitalPaperSub
        {
            get
            {
                return (f_PaperCode.ToUpper() == Settings.PaperCode_DISE ||
                        f_PaperCode.ToUpper() == Settings.PaperCode_IPAD ||
                        f_ProductNo.ToUpper() == Settings.PaperCode_IPAD ||
                        f_PaperCode.ToUpper() == Settings.PaperCode_AGENDA);
            }
        }

        //flag properties - "f_" prefix - initially not saved in mssql but nowadays some of them are
        public string f_PaperCode;
        public string f_ProductNo;
        public string f_CountryCode;
        public string f_OfferdenEmail;

        public void SetApsisProjectGuid(List<string> freeSubsCampIds)
        {
            // New fewer "dynamic" templates introduced in beginning of February 2014. Use them or not is an application setting
            bool useApsis2014Templates;
            bool.TryParse(MiscFunctions.GetAppsettingsValue("UseApsis2014Templates"), out useApsis2014Templates);
            if (useApsis2014Templates)
            {
                SetApsisProjectGuid2014();
                return;
            }
            // Use old Apsis templates
            #region Apsis Templates before 2014
            // If Agenda...
            if (f_PaperCode.ToUpper() == Settings.PaperCode_AGENDA)
            {
                ApsisProjectGuid = MiscFunctions.GetAppsettingsValue("apsisProjectGuid2014_Agenda");
                return;
            }

            //weekend subs, 'H' in cirix
            if (f_ProductNo == Settings.ProductNo_Weekend && f_PaperCode == Settings.PaperCode_DI)
            {
                ApsisProjectGuid = MiscFunctions.GetAppsettingsValue("apsisProjectGuid2013_weekend");
                return;
            }

            //trial subs, 'G' in cirix
            if (freeSubsCampIds.Contains(CampId) || CampIdStartsWithL() || CampIdIsN1101000E())
            {
                if (IsDigitalPaperSub)
                {
                    ApsisProjectGuid = GetDigitalApsisProjId();
                    return;
                }

                ApsisProjectGuid = MiscFunctions.GetAppsettingsValue("apsisProjectGuid2013_trial");
                return;
            }

            if (IsDigitalPaperSub)
            {
                ApsisProjectGuid = GetDigitalApsisProjId();
                return;
            }
            
            if (f_PaperCode.ToUpper() == Settings.PaperCode_DI)
            {
                ApsisProjectGuid = MiscFunctions.GetAppsettingsValue(SubsLenMons < 9 ? "apsisProjectGuid2013_paperShort" : "apsisProjectGuid2013_paperLong");
                return;
            }

            //fallback (should never happen)
            ApsisProjectGuid = MiscFunctions.GetAppsettingsValue("apsisProjectGuid2013_paperShort");
            #endregion
        }

        private void SetApsisProjectGuid2014()
        {
            //"Hybrid" hösten 2014
            var hybridCampGroupIdString = MiscFunctions.GetAppsettingsValue("HybridCampGroupId");
            int hybridCampGroupId;
            var parseHybrid = int.TryParse(hybridCampGroupIdString, out hybridCampGroupId);
            if (parseHybrid && CampGroupId == hybridCampGroupId)
            {
                ApsisProjectGuid = MiscFunctions.GetAppsettingsValue("apsisProjectGuid2014_hybrid");
                return;
            }

            // Agenda
            if (f_PaperCode.ToUpper() == Settings.PaperCode_AGENDA)
            {
                ApsisProjectGuid = MiscFunctions.GetAppsettingsValue("apsisProjectGuid2014_Agenda");
                return;
            }

            //Di Weekend subs, 'H' in Cirix
            if (f_ProductNo == Settings.ProductNo_Weekend && f_PaperCode == Settings.PaperCode_DI)
            {
                ApsisProjectGuid = MiscFunctions.GetAppsettingsValue("apsisProjectGuid2014_weekend");
                return;
            }

            //Any other digital product
            if (IsDigitalPaperSub)
            {
                ApsisProjectGuid = MiscFunctions.GetAppsettingsValue("apsisProjectGuid2014_digital");
                return;
            }

            //The rest, like regular paper
            ApsisProjectGuid = MiscFunctions.GetAppsettingsValue("apsisProjectGuid2014_paper");
        }

        /// <summary>
        /// digital short subs='K' in cirix AND digital long subs='P' in cirix
        /// </summary>
        private string GetDigitalApsisProjId()
        {
            if (SubsLenMons < 9)    
                return MiscFunctions.GetAppsettingsValue("apsisProjectGuid2013_digitalShort");
            
            return MiscFunctions.GetAppsettingsValue("apsisProjectGuid2013_digitalLong");
        }

        public void SetIsExtCustomer(DefinitionFile defFile)
        {
            IsExtCustomer = false;

            if (CampNo == null || CampNo < 1 || string.IsNullOrEmpty(ReceiveType))
                return;

            try
            {
                IsExtCustomer = !defFile.IsEpiBounce(ReceiveType, (int)CampNo);
            }
            catch (Exception ex)
            {
                new DIClassLib.DbHelpers.Logger("SetIsExtCustomer() failed", ex.Message);
            }
        }

        private bool CampIdStartsWithL()
        {
            return (!string.IsNullOrEmpty(CampId) && CampId.ToUpper().StartsWith("L"));
        }

        private bool CampIdIsN1101000E()
        {
            return (!string.IsNullOrEmpty(CampId) && CampId.ToUpper() == "N1101000E");
        }
    }
}
