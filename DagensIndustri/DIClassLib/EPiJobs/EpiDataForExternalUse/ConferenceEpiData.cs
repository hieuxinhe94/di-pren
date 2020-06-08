namespace DIClassLib.EPiJobs.EpiDataForExternalUse
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class ConferenceEpiData
    {
        public string Headline { get; set; }
        public string ShortIntroTextHtml { get; set; }
        public string Place { get; set; }
        public string PlaceMapUrl { get; set; }
        public string ConferencePageUrl { get; set; }
        public DateTime DateConferenceStart { get; set; }

        public ConferenceEpiData() {}

        public ConferenceEpiData(string headline, string shortIntroTextHtml, string place, string placeMapUrl, string conferencePageUrl, DateTime dateConferenceStart)
        {
            Headline = headline;
            ShortIntroTextHtml = shortIntroTextHtml;
            Place = place;
            PlaceMapUrl = placeMapUrl;
            ConferencePageUrl = conferencePageUrl;
            DateConferenceStart = dateConferenceStart;
        }
    }
}
