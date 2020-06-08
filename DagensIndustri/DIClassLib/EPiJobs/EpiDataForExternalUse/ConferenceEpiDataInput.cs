namespace DIClassLib.EPiJobs.EpiDataForExternalUse
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class ConferenceEpiDataInput : ConferenceEpiData
    {
        public int BatchId { get; set; }
        public int EpiPageId { get; set; }
        //public string Headline { get; set; }
        //public string ShortIntroTextHtml { get; set; }
        //public string Place { get; set; }
        //public string PlaceMapUrl { get; set; }
        //public string ConferencePageUrl { get; set; }
        //public DateTime DateConferenceStart { get; set; }

        public ConferenceEpiDataInput(int batchId, int epiPageId, string headline, string shortIntroTextHtml, string place, string placeMapUrl, string conferencePageUrl, DateTime dateConferenceStart)
        {
            BatchId = batchId;
            EpiPageId = epiPageId;
            Headline = headline;
            ShortIntroTextHtml = shortIntroTextHtml;
            Place = place;
            PlaceMapUrl = placeMapUrl;
            ConferencePageUrl = conferencePageUrl;
            DateConferenceStart = dateConferenceStart;
        }
    }
}
