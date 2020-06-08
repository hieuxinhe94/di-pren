using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DIClassLib.EPiJobs.Apsis
{
    //TODO: Merge this class and ApsisListSubscriber into one
    public class ApsisFields
    {
        public bool ActiveInCirix { get; set; }
        public bool CustomerHaveOptOut { get; set; }

        //Apsis demographic datafields
        public string UserName { get; set; }
        public string Status { get; set; }
        public string CustomerNumber { get; set; }
        public string StartDate { get; set; }
        public string MobileNumber { get; set; }
        public string PersonalCode { get; set; }
        public string PrenDi { get; set; }
        public string PrenPren { get; set; }
        public string PrenInfo { get; set; }
        public string PrenGasell { get; set; }
        public string PrenConference { get; set; }
        public string PrenGold { get; set; }
        public string PrenAdvertisment { get; set; }
        public string WelcomeEmailSentDate { get; set; }
        public string DiAccount { get; set; }
        public string PaperCode { get; set; }
        public string ProductNo { get; set; }
        public string SubsLength { get; set; }
        public string ProductDescription { get; set; }
        public string OutcomeS2 { get; set; }
        public string OutcomeS2Date { get; set; }
        public string ImportDate { get; set; }

        public string ToLogString()
        {
            try
            {
                var sb = new StringBuilder();

                foreach (var propertyInfo in typeof (ApsisFields).GetProperties())
                {
                    sb.AppendLine(String.Format("{0}: '{1}'", propertyInfo.Name,
                        propertyInfo.GetValue(this, null) ?? "[NULL]"));
                }

                return sb.ToString();
            }
            catch (Exception)
            {
                return "Could not build logstring for ApsisFields";
            }
        }
    }
}