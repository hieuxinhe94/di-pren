namespace DIClassLib.Misc
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using EPiServer.Core;

    public class ConferenceExposer
    {
        public ConferenceExposer() { }

        public List<Conference> GetUpcomingConferences(int numUpcomingConfrences = 3)
        {
            try
            {
                //PageData pd = EPiFunctions.StartPage();
                //var test = new DataFactory(new PageProviderMap());
                //EPiServer.Core.PageReferenceCollection
                //ConferenceContainerPage
                //PageData start = EPiServer.DataFactory.Instance.GetPage(PageReference.StartPage);
                //PageData settings = EPiServer.DataFactory.Instance.GetPage(new PageReference(start["SettingsPage"].ToString()));
                
                PageReference pr = new PageReference(30);
                //PageProviderBase pp = DataFactory.Instance.GetPageProvider(pr);
                PageData pd1 = EPiServer.DataFactory.Instance.GetPage(pr);
                
                //PageData pd = (PageData)settings["ConferenceContainerPage"];
                //PageData pd = EPiServer.DataFactory.Instance.GetPage(EPiFunctions.SettingsPageSetting(null, "ConferenceContainerPage") as PageReference);
            }
            catch (EPiServerException exEpi)
            {
                string s = exEpi.ToString();
            }
            catch (Exception ex)
            {
                string s = ex.ToString();
            }


            return new List<Conference> { new Conference() };
        }
    }

    public class Conference
    {
        public string Heading { get; set; }
        public string Introduction { get; set; }
        public string Place { get; set; }
        public string PlaceMapUrl { get; set; }
        public string ConfPageUrl { get; set; }
        public DateTime ConfDate { get; set; }

        public Conference() { }

        public Conference(string heading, string introduction, string place, string placeMapUrl, string confPageUrl, DateTime confDate)
        {
            Heading = heading;
            Introduction = introduction;
            Place = place;
            PlaceMapUrl = placeMapUrl;
            ConfPageUrl = confPageUrl;
            ConfDate = confDate;
        }
    }
}
