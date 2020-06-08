using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Xml;
using DIClassLib.OboWebReference;
using System.Net;
using DIClassLib.DbHelpers;
using DIClassLib.ApsisWs;
using DIClassLib.Misc;
using DIClassLib.BusinessCalendar;
using System.Web;
using System.Web.Caching;

namespace DIClassLib.EPiJobs.ServiceVerifier
{
    public class ServiceVerifier
    {
        /// <summary>
        /// Gets the specified life time in minutes of cache from AppSettings or default (60 min) if missing 
        /// </summary>
        public static int CacheLifetime
        {
            get
            {
                int cacheMinutes = 60;
                if (ConfigurationManager.AppSettings["serviceVerifierCacheMinutes"] != null)
                {
                    int tmp = 0;
                    if (Int32.TryParse(ConfigurationManager.AppSettings["serviceVerifierCacheMinutes"], out tmp))
                    {
                        cacheMinutes = tmp;
                    }
                }
                return cacheMinutes;
            }
        }

        public static bool AdlibrisIsValid 
        {
            get
            {
                if (HttpContext.Current.Cache["AdlibrisIsValid"] == null)
                {
                    ServiceVerifierStatus status = VerifyAdlibris();
                    HttpContext.Current.Cache.Insert("AdlibrisIsValid", status.IsValid, null, DateTime.UtcNow.AddMinutes(CacheLifetime), Cache.NoSlidingExpiration);
                }
                
                return (bool)HttpContext.Current.Cache["AdlibrisIsValid"];
            }            
        }

        public static bool OBOIsValid
        {
            get
            {
                if (HttpContext.Current.Cache["OBOIsValid"] == null)
                {
                    ServiceVerifierStatus status = VerifyOBO();
                    HttpContext.Current.Cache.Insert("OBOIsValid", status.IsValid, null, DateTime.UtcNow.AddMinutes(CacheLifetime), Cache.NoSlidingExpiration);
                }

                return (bool)HttpContext.Current.Cache["OBOIsValid"];
            }
        }

        
        public static bool ApsisIsValid
        {
            get
            {
                if (HttpContext.Current.Cache["ApsisIsValid"] == null)
                {
                    ServiceVerifierStatus status = VerifyApsis();
                    HttpContext.Current.Cache.Insert("ApsisIsValid", status.IsValid, null, DateTime.UtcNow.AddMinutes(CacheLifetime), Cache.NoSlidingExpiration);
                }

                return (bool)HttpContext.Current.Cache["ApsisIsValid"];
            }
        }
        
        public static bool DiRssIsValid
        {
            get
            {
                if (HttpContext.Current.Cache["DiRssIsValid"] == null)
                {
                    ServiceVerifierStatus status = VerifyDiRss();
                    HttpContext.Current.Cache.Insert("DiRssIsValid", status.IsValid, null, DateTime.UtcNow.AddMinutes(CacheLifetime), Cache.NoSlidingExpiration);
                }

                return (bool)HttpContext.Current.Cache["DiRssIsValid"];
            }
        }

        public static bool GoogleMapsIsValid
        {
            get
            {
                if (HttpContext.Current.Cache["GoogleMapsIsValid"] == null)
                {
                    ServiceVerifierStatus status = VerifyGoogleMaps();
                    HttpContext.Current.Cache.Insert("GoogleMapsIsValid", status.IsValid, null, DateTime.UtcNow.AddMinutes(CacheLifetime), Cache.NoSlidingExpiration);
                }

                return (bool)HttpContext.Current.Cache["GoogleMapsIsValid"];
            }
        }
        
        public static bool BusinessCalendarIsValid
        {
            get
            {
                if (HttpContext.Current.Cache["BusinessCalendarIsValid"] == null)
                {
                    ServiceVerifierStatus status = VerifyBusinessCalendar();
                    HttpContext.Current.Cache.Insert("BusinessCalendarIsValid", status.IsValid, null, DateTime.UtcNow.AddMinutes(CacheLifetime), Cache.NoSlidingExpiration);
                }

                return (bool)HttpContext.Current.Cache["BusinessCalendarIsValid"];
            }
        }
        
        // We wait with this one...
        // public static bool? GoogleAnalyticsIsValid { get; private set; }


        /// <summary>
        /// Test the connection and response from Adlibris
        /// </summary>
        /// <returns></returns>
        public static ServiceVerifierStatus VerifyAdlibris()
        {
            
            ServiceVerifierStatus status = new ServiceVerifierStatus();
            try
            {
                String adlibrisUrl = ConfigurationManager.AppSettings["AdlibrisUrl"];
                if (!string.IsNullOrEmpty(adlibrisUrl))
                {
                    XmlTextReader xmlReader = new XmlTextReader(adlibrisUrl);
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(xmlReader);

                    // check for valid elements
                    //
                    if (xmlDoc.SelectNodes("news_list/article").Count > 0)
                    {
                        status.IsValid = true;
                    }
                    else
                    {
                        throw new Exception("Articles missing in XML Document");
                    }

                }
            }
            catch (Exception ex)
            {
                status.Message = ex.Message;
                status.Exception = ex;
                new Logger("VerifyAdlibris() - failed", ex.ToString());
                SendVerifierFailedNotification("Adlibris", ex.Message, ex);
                
            }

             return status;
        }

        public static ServiceVerifierStatus VerifyOBO()
        {
            ServiceVerifierStatus status = new ServiceVerifierStatus();
            try
            {
                DocumentFactoryService factory = SetUpOneByOneDocumentFactory();

                if (factory == null)
                {
                    throw new Exception("Failed to setup OneByOneDocumentFactory");
                }

                RequestParameter[] parameters = new RequestParameter[5];
                parameters[0] = new RequestParameter() { name = "who", value = "" };
                parameters[1] = new RequestParameter() { name = "what", value = "dagens industri" };
                parameters[2] = new RequestParameter() { name = "where", value = "" };
                parameters[3] = new RequestParameter() { name = "maxSize", value = "1" };
                parameters[4] = new RequestParameter() { name = "offset", value = "0" };


                string documentName = ConfigurationManager.AppSettings["OBOSearchPerson"];
                String xml = factory.getDocument(documentName, parameters);

                // parse the document validate
                //
                //XmlTextReader xmlReader = new XmlTextReader(
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xml);
                if (xmlDoc.SelectNodes("Bizbook-Search-C/SearchResult").Count > 0)
                {
                    status.IsValid = true;
                }
                else
                {
                    throw new Exception("Failed to find SearchResult node");                    
                }

            }
            catch (Exception ex)
            {
                status.Message = ex.Message;
                status.Exception = ex;
                SendVerifierFailedNotification("OBO", ex.Message, ex);
            }

            return status;
        }

        /// <summary>
        /// Set security credentials for One By One's Xml webservice client authentication
        /// </summary>
        /// <returns></returns>
        private static DocumentFactoryService SetUpOneByOneDocumentFactory()
        {
            DocumentFactoryService DocumentFactory = null;
            try
            {
                DocumentFactory = new DocumentFactoryService();
                DocumentFactory.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["OBOUserName"], ConfigurationManager.AppSettings["OBOPassword"]);
            }
            catch (Exception ex)
            {
                new Logger("SetUpDocumentFactory() - failed", ex.ToString());                
                DocumentFactory = null;
            }

            return DocumentFactory;
        }

        public static ServiceVerifierStatus VerifyApsis()
        {
            ServiceVerifierStatus status = new ServiceVerifierStatus();
            string _apsisUser = ConfigurationManager.AppSettings["apsisWelcomeAccountUsername"];
            string _apsisPass = ConfigurationManager.AppSettings["apsisWelcomeAccountPassword"];
            DateTime dtStart = DateTime.Now.AddDays(-1);
            DateTime dtEnd = DateTime.Now;

            try
            {
                ApsisNewsletterProAPISoapClient _ws = new ApsisNewsletterProAPISoapClient();
                TransactionBounceResult[] bounces = _ws.GetTransactionBouncesByDateInterval(_apsisUser, _apsisPass, MiscFunctions.GetAppsettingsValue("apsisProjectGuid_regular"), dtStart, dtEnd);
                status.IsValid = true;
                     
            }
            catch (Exception ex)
            {
                status.Message = ex.Message;
                status.Exception = ex;
                SendVerifierFailedNotification("Apsis", ex.Message, ex);
            }

            return status;
        }

        public static ServiceVerifierStatus VerifyDiRss()
        {
            ServiceVerifierStatus status = new ServiceVerifierStatus();
            try
            {
                String diRssUrl = "http://www.di.se/rss";
                if (!string.IsNullOrEmpty(diRssUrl))
                {
                    XmlTextReader xmlReader = new XmlTextReader(diRssUrl);
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(xmlReader);

                    // check for valid elements
                    //
                    if (xmlDoc.SelectNodes("rss/channel").Count > 0)
                    {
                        status.IsValid = true;
                    }
                    else
                    {
                        throw new Exception("Channel is missing in RSS");
                    }

                }
            }
            catch (Exception ex)
            {
                status.Message = ex.Message;
                status.Exception = ex;
                SendVerifierFailedNotification("OBO", ex.Message, ex);
            }

            
            return status;
        }

        public static ServiceVerifierStatus VerifyGoogleMaps()
        {
            ServiceVerifierStatus status = new ServiceVerifierStatus();
            try
            {
                String url = string.Format("{0}{1}", ConfigurationManager.AppSettings["MapsUrl"], "Stockholm");
                HttpWebRequest req =(HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse res = (HttpWebResponse)req.GetResponse();
                if (res.StatusCode == HttpStatusCode.OK)
                {
                    status.IsValid = true;
                }
  
            }
            catch (Exception ex)
            {
                status.Message = ex.Message;                
                status.Exception = ex;
                SendVerifierFailedNotification("GoogleMaps", ex.Message, ex);
            }
            
            return status;
        }

        

        /// <summary>
        /// Verify Business Calendar service (Affärskalender)
        /// </summary>
        /// <returns></returns>
        public static ServiceVerifierStatus VerifyBusinessCalendar()
        {
            ServiceVerifierStatus status = new ServiceVerifierStatus();
            string milliStreamUrl  = ConfigurationManager.AppSettings["BusCalMilliStreamUrl"];
            string milliStreamUser = ConfigurationManager.AppSettings["BusCalMilliStreamUser"];
            string milliStreamPass = ConfigurationManager.AppSettings["BusCalMilliStreamPass"];

            List<Company> comps = new List<Company>();

            try
            {
                StringBuilder url = new StringBuilder();
                url.Append(milliStreamUrl);
                url.Append("?usr=" + milliStreamUser);
                url.Append("&pwd=" + milliStreamPass);
                url.Append("&cmd=quote&marketplace=35201&instrumenttype=4&fields=insref,name,isin");

                XmlTextReader xmlReader = new XmlTextReader(url.ToString());

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(xmlReader);

                // check for valid elements
                //
                if (xmlDoc.SelectNodes("milliresult/instrument").Count > 0)
                {
                    status.IsValid = true;
                }
                else
                {
                    throw new Exception("instrument element is missing in XML");
                }

                //object companyNode = reader.NameTable.Add("instrument");

                

            }
            catch (Exception ex)
            {
                status.Message = ex.Message;
                status.Exception = ex;
                SendVerifierFailedNotification("BusinessCalendar", ex.Message, ex);
            }

            return status;
        }

        /// <summary>
        /// Notify WebMaster by e-mail that there has been an exception while trying to access one of the external services.
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        private static void SendVerifierFailedNotification(String serviceName, String message, Exception ex)
        {
            // See if a notification for this service already has been sent
            //
            if(HttpContext.Current.Cache["ServiceVerifierErrorNotification_" + serviceName] != null){
                return;
            }

            String to = ConfigurationManager.AppSettings["mailWebmasterDiSe"];
            String from = to;
            StringBuilder sbBody = new StringBuilder();
            sbBody.Append(message + "\n");
            if (ex != null)
            {
                sbBody.Append(ex.StackTrace);
            }

            MiscFunctions.SendMail(from,to,"["+serviceName+"]Service Verifier Failed",sbBody.ToString(),false);

            // Add flag in Cache so that the webmaster is not spammed more than once per day
            //
            HttpContext.Current.Cache.Insert("ServiceVerifierErrorNotification_" + serviceName, "1", null, DateTime.UtcNow.AddDays(1), Cache.NoSlidingExpiration);
        }
    }
}
