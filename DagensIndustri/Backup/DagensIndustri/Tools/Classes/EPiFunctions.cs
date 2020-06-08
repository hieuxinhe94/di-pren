using System;
using System.Data;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.Net;
using System.Net.Mail;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml;
using EPiServer;
using EPiServer.Core;
using EPiServer.Filters;
using EPiServer.Security;
using DagensIndustri.OneByOne;
using DagensIndustri.Tools.Classes.BaseClasses;
using DagensIndustri.Tools.Classes.Extras;
using DIClassLib.Membership;
using DIClassLib.DbHelpers;
using DIClassLib.Misc;

namespace DagensIndustri.Tools.Classes
{
    public class EPiFunctions
    {
        
        #region EPi
        
        public static PageData StartPage()
        {
            return EPiServer.DataFactory.Instance.GetPage(PageReference.StartPage);
        }
 
        public static PageData SettingsPage(PageData pd)
        {
            PageData pageData = pd;
            if (pageData == null || pageData["SettingsPage"] == null)
            {
                pageData = StartPage();
            }

            return EPiServer.DataFactory.Instance.GetPage(new PageReference(pageData["SettingsPage"].ToString()));
        }
        
        public static object SettingsPageSetting(PageData pd, string propertyName)
        {
            PageData settingsPage = SettingsPage(pd);
            return settingsPage[propertyName];
        }

        public static PageData GetDiGoldStartPage()
        {
            return EPiServer.DataFactory.Instance.GetPage(EPiFunctions.SettingsPageSetting(null, "DIGoldStartPage") as PageReference);
        }

        /// <summary>
        /// Get a property on a page that is referenced to from SettingsPage
        /// </summary>
        /// <param name="pd"></param>
        /// <param name="pagePropertyName"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static object GetPagePropertyValueBySettingsPage(PageData pd, string pagePropertyName, string propertyName)
        {
            object propertyValue = null;
            PageReference pr = SettingsPageSetting(pd, pagePropertyName) as PageReference;
            if (pr != null)
            {
                PageData pageData = EPiServer.DataFactory.Instance.GetPage(pr);
                propertyValue = pageData[propertyName];
            }
            return propertyValue;
        }

        /// <summary>
        /// Check if the page has a value in a certain property.
        /// </summary>
        /// <param name="pd"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static bool HasValue(PageData pd, string propertyName)
        {
            return pd.Property[propertyName] != null && pd.Property[propertyName].Value != null;
        }

        /// <summary>
        /// Get a friendly URL for the given PageData object
        /// </summary>
        /// <param name="pd">The page to get the Friendly URL from</param>
        /// <param name="Absolute">Return an absolute path</param>
        /// <returns>The friendly Url for the given PageData object</returns>
        public static string GetFriendlyUrl(PageData pd)
        {
            UrlBuilder url = new UrlBuilder(pd.LinkURL);
            //Call UrlRewriteProvider's ConvertToExternal method
            EPiServer.Global.UrlRewriteProvider.ConvertToExternal(url, pd.PageLink, System.Text.UTF8Encoding.UTF8);

            return MiscFunctions.RemoveWwwFromUrl(url.ToString());
        }

        public static string GetFriendlyAbsoluteUrl(string url)
        {
            return MiscFunctions.RemoveWwwFromUrl(EPiServer.Configuration.Settings.Instance.SiteUrl.ToString()) + url.TrimStart('/');
        }

        public static string GetFriendlyAbsoluteUrl(PageData pd)
        {
            return MiscFunctions.RemoveWwwFromUrl(EPiServer.Configuration.Settings.Instance.SiteUrl.ToString()) + GetFriendlyUrl(pd).TrimStart('/');
        }

        public static string GetFriendlyAbsoluteUrl(PageReference pageReference)
        {
            string friendlyAbsoluetUrl = string.Empty;
            if (pageReference != null)
            {
                PageData pd = EPiServer.DataFactory.Instance.GetPage(pageReference);
                friendlyAbsoluetUrl = GetFriendlyAbsoluteUrl(pd);
            }
            return friendlyAbsoluetUrl;
        }

        /// <summary>
        /// Check if current user has a certain access to the specified page.
        /// </summary>
        /// <param name="pd"></param>
        /// <param name="accessLevel"></param>
        /// <returns></returns>
        public static bool UserHasPageAccess(PageData pd, AccessLevel accessLevel)
        {
            if (pd == null)
                return false;

            return pd.ACL.QueryDistinctAccess(accessLevel);
        }

        /// <summary>
        /// Check if current user has a certain access to the specified page.
        /// </summary>
        /// <param name="pr"></param>
        /// <param name="accessLevel"></param>
        /// <returns></returns>
        public static bool UserHasPageAccess(PageReference pr, AccessLevel accessLevel)
        {
            if (pr == null)
                return false;

            PageData pd = DataFactory.Instance.GetPage(pr);
            return UserHasPageAccess(pd, accessLevel);
        }

        /// <summary>
        /// Check whether the user has DiGold role
        /// </summary>
        public static bool IsUserDIGoldMember()
        {
            return HttpContext.Current.User.Identity.IsAuthenticated && Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, DiRoleHandler.RoleDiGold);
        }
        
        /// <summary>
        /// Get all page data objects for the given page references. Filter on page type if needed.
        /// </summary>
        /// <param name="pageReferences"></param>
        /// <param name="pageTypeID"></param>
        /// <returns></returns>
        public static PageDataCollection GetPageDataCollection(List<PageReference> pageReferences, int? pageTypeID)
        {
            PageDataCollection pdColl = new PageDataCollection();

            if (pageReferences != null)
            {
                foreach (PageReference pr in pageReferences)
                {
                    PageData pd = DataFactory.Instance.GetPage(pr);
                    if (pageTypeID.HasValue)
                    {
                        if (pd.PageTypeID == pageTypeID.Value)
                        {
                            pdColl.Add(pd);
                        }
                    }
                    else
                    {
                        pdColl.Add(pd);
                    }
                }
            }

            return pdColl;
        }

        /// <summary>
        /// Returns if it the same pageTypeID or not
        /// </summary>
        /// <param name="pageTypeID"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static bool IsMatchingPageType(PageData pd, int pageTypeID, string propertyName)
        {
            if (EPiFunctions.SettingsPage(pd)[propertyName] != null)
            {
                if (pageTypeID.Equals((int)EPiFunctions.SettingsPage(pd)[propertyName]))
                    return true;
            }

            return false;
        }

        public static string GetXHTMLWithoutEpiDiv(PageData pd, string propertyname)
        {
            string prop = "";

            if (HasValue(pd, propertyname))
            {
                prop = pd.Property[propertyname].ToString();

                if (prop.StartsWith("<div>") && prop.EndsWith("</div>"))
                {
                    prop = prop.Remove(0, 5).Remove(prop.Length - 6, 6);
                }
            }

            return prop;
        }       

        public static string GetDate(PageData pd, string propertyname)
        { 
            string date = "";

            if (HasValue(pd, propertyname))
            {
                DateTime dt = Convert.ToDateTime(pd[propertyname].ToString());

                if (dt.ToString("MMMM").Length > 4)
                {
                    date = dt.ToString("dddd, d MMM");
                }
                else
                {
                    date = dt.ToString("dddd, d MMMM");
                }


            }
            return date;
        }

        //todo: if we have time make it better...
        public static string GetConferenceDateAndPlace(PageData pd, bool simple, string lang)
        {
            string dateAndPlace = "";

            DateTime startDate = new DateTime();
            DateTime endDate = new DateTime();

            if (simple)
            {
                if (EPiFunctions.HasValue(pd, "Date"))
                {
                    startDate = Convert.ToDateTime(pd["Date"].ToString());

                    dateAndPlace = String.Format("{0} - {1}", startDate.ToString("dd MMMM yyyy", new CultureInfo(lang)), pd["Place"].ToString());
                }
            }
            else
            {
                if (EPiFunctions.HasValue(pd, "Date") && EPiFunctions.HasValue(pd, "EndDate"))
                {
                    startDate = Convert.ToDateTime(pd["Date"].ToString());
                    endDate = Convert.ToDateTime(pd["EndDate"].ToString());

                    if (startDate.Month == endDate.Month)
                    {

                        dateAndPlace = String.Format("{0}-{1} - {2}", startDate.ToString("dd", new CultureInfo(lang)), endDate.ToString("dd MMMM yyyy", new CultureInfo(lang)), pd["Place"].ToString());
                    }
                    else
                    {
                        dateAndPlace = String.Format("{0}-{1} - {2}", startDate.ToString("dd MMMM", new CultureInfo(lang)), endDate.ToString("dd MMMM yyyy", new CultureInfo(lang)), pd["Place"].ToString());
                    }
                }
                else if (EPiFunctions.HasValue(pd, "Date"))
                {
                    startDate = Convert.ToDateTime(pd["Date"].ToString());

                    dateAndPlace = String.Format("{0} - {1}", startDate.ToString("dd MMMM yyyy", new CultureInfo(lang)), pd["Place"].ToString());
                }

            }


            return dateAndPlace;
        }

        public static PageDataCollection FilterMenu(PageDataCollection pdc)
        {
            PageDataCollection filteredPDC = new PageDataCollection();

            foreach (PageData pd in pdc)
            {
                if (pd.VisibleInMenu == true)
                    filteredPDC.Add(pd);
            }

            return filteredPDC;
        }

        /// <summary>
        /// Redirect to a certain page. Add ReturnUrl as parameter if exists
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageLink"></param>
        /// <param name="returnUrl"></param>
        public static void RedirectToPage(Page page, PageReference pageLink, string returnUrl)
        {
            string url = EPiFunctions.GetFriendlyAbsoluteUrl(pageLink);
            if (!string.IsNullOrEmpty(url))
            {
                if (!string.IsNullOrEmpty(returnUrl))
                {
                    url = string.Format("{0}?ReturnUrl={1}", url, returnUrl);
                }
                
                page.Response.Redirect(url);
            }
        }

        /// <summary>
        /// Get url of the subrscription flow
        /// </summary>
        /// <returns></returns>
        public static string GetSubscriptionPageUrl(PageData pd)
        {            
            PageReference pr = SettingsPageSetting(pd, "DiGoldSubscriptionPage") as PageReference;
            return pr != null ? EPiServer.DataFactory.Instance.GetPage(pr).LinkURL : string.Empty;
        }

        /// <summary>
        /// Get Di Gold membership flow page
        /// </summary>
        /// <param name="pd"></param>
        /// <returns></returns>
        public static PageData GetDiGoldFlowPage(PageData pd)
        {
            PageData diGoldFlowPage = null;
            PageReference diGoldMembershipFlowPageLink = EPiFunctions.SettingsPageSetting(pd, "DiGoldMembershipFlowPage") as PageReference;
            if (diGoldMembershipFlowPageLink != null)
                diGoldFlowPage = EPiServer.DataFactory.Instance.GetPage(diGoldMembershipFlowPageLink);

            return diGoldFlowPage;
        }

        //public static PageData GetLoginPage(PageData pd)
        //{
        //    PageData loginPage = null;
        //    PageReference loginPageLink = EPiFunctions.SettingsPageSetting(pd, "LinkToLoginPage") as PageReference;

        //    if (loginPageLink != null)
        //        loginPage = EPiServer.DataFactory.Instance.GetPage(loginPageLink);
        //    return loginPage;
        //}

        public static PageData GetLoginPage(PageData pd)
        {
            PageData loginPage = null;
            PageReference loginPageLink = GetLoginSettingsPage(pd)["LinkToLoginPage"] as PageReference; //EPiFunctions.SettingsPageSetting(pd, "LinkToLoginPage") as PageReference;
            
            if (loginPageLink != null)
                loginPage = EPiServer.DataFactory.Instance.GetPage(loginPageLink);

            return loginPage;
        }

        public static string GetLoginBanner(PageData pd)
        {
            string propName = "NotLoggedInLoginBanner";
            string result = string.Empty;

            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                if (System.Web.HttpContext.Current.User.IsInRole("DiGold"))
                    propName = "DiGoldInLoginBanner";
                else
                    propName = "LoggedInLoginBanner";
            }

            if (!string.IsNullOrEmpty(propName))
            {
                if (HasValue(GetLoginSettingsPage(pd), propName))
                {
                    result = GetLoginSettingsPage(pd)[propName].ToString();
                }
            }

            return result;
        }

        public static string GetLoginBannerLink(PageData pd)
        {
            string propName = "NotLoggedInLoginBannerLink";
            string result = string.Empty;

            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                if (System.Web.HttpContext.Current.User.IsInRole("DiGold"))
                    propName = "DiGoldInLoginBannerLink";
                else
                    propName = "LoggedInLoginBannerLink";
            }

            if (!string.IsNullOrEmpty(propName))
            {
                if (HasValue(GetLoginSettingsPage(pd), propName))
                {
                    result = GetLoginSettingsPage(pd)[propName].ToString();
                }
            }

            return result;
        }

        public static bool GetLoginBannerVisibility(PageData pd)
        {
            string propName = "NotLoggedInLoginBanner";

            bool visible = false;

            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                if (System.Web.HttpContext.Current.User.IsInRole("DiGold"))
                    propName = "DiGoldInLoginBanner";
                else
                    propName = "LoggedInLoginBanner";
            }

            if (!string.IsNullOrEmpty(propName))
            {
                if (HasValue(GetLoginSettingsPage(pd),propName))
                {
                    visible = true;
                }
            }

            return visible;
        }

        public static bool HideHyperLink(PageData pd)
        {
            string propName = "NotLoggedInLoginBannerLink";

            bool visible = false;

            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                if (System.Web.HttpContext.Current.User.IsInRole("DiGold"))
                    propName = "DiGoldInLoginBannerLink";
                else
                    propName = "LoggedInLoginBannerLink";
            }

            if (!string.IsNullOrEmpty(propName))
            {
                if (HasValue(GetLoginSettingsPage(pd), propName))
                {
                    visible = true;
                }
            }

            return visible;
        }

        public static PageData GetLoginSettingsPage(PageData pd)
        {
            PageData loginSettingsPage = null;

            if (HasValue(pd, "MainMenuContainer"))
            {
                loginSettingsPage = EPiServer.DataFactory.Instance.GetPage(pd["MainMenuContainer"] as PageReference);
            }
            else
            {
                loginSettingsPage = StartPage();
            }

            return loginSettingsPage;
        }

        public static string GetLoginPageUrl(PageData pd)
        {
            return string.Format("{0}?ReturnUrl={1}",
                                    GetFriendlyAbsoluteUrl(GetLoginPage(pd)),
                                    HttpUtility.UrlEncode(GetFriendlyAbsoluteUrl(pd)));
        }

        public static PageData GetLoginPage()
        {
            return GetLoginPage(StartPage());
        }

        public static string GetCardPayFailPageUrl()
        {
            if (SettingsPageSetting(null, "CardPayFailPage") != null)
            {
                PageData pd = EPiServer.DataFactory.Instance.GetPage(SettingsPageSetting(null, "CardPayFailPage") as PageReference);
                string p = GetFriendlyAbsoluteUrl(pd);
                if(p.EndsWith("/"))
                    p = p.Substring(0, p.Length - 1);   //remove '/' at end of path

                return p;
            }

            return string.Empty;
        }
        #endregion EPi

        
        #region UserControl methods
        
        /// <summary>
        /// Find DiGoldMembershipPopup control on the page
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public static UserControl FindDiGoldMembershipPopup(Page page)
        {
            return page.Master.FindControl("DiGoldMembershipPopup") as UserControl;
        }

        /// <summary>
        /// Given a user control, find a control on it and set an attribute with a certain value on that control
        /// </summary>
        /// <param name="userControl"></param>
        /// <param name="controlName"></param>
        /// <param name="attributeName"></param>
        /// <param name="attributeValue"></param>
        public static void SetAttributeOnControl(UserControl userControl, string controlName, string attributeName, string attributeValue)
        {
            Control control = userControl.FindControl(controlName);
            if (control != null)
            {
                if (control is HtmlControl)
                {
                    ((HtmlControl)control).Attributes.Add(attributeName, attributeValue);
                }
                else if (control is WebControl)
                {
                    ((WebControl)control).Attributes.Add(attributeName, attributeValue);
                }
            }
        }
        
        #endregion
        
        
        #region Show User Message Methods
        
        /// <summary>
        /// Add an message to the validation summary
        /// </summary>
        /// <param name="errorMsg"></param>
        public static void AddMessageOnPage(Page page, string message)
        {
            page.Validators.Add(new ValidationError(message));
        }

        /// <summary>
        /// Show a message on page by invoking the page's ShowMessage method
        /// </summary>
        /// <param name="page"></param>
        /// <param name="message"></param>
        /// <param name="isKey"></param>
        public static void ShowMessage(Page page, string message, bool isKey, bool isErrorMessage)
        {
            if (page.GetType().Equals(typeof(DiTemplatePage)))
            {
                ((DiTemplatePage)page).ShowMessage(message, isKey, isErrorMessage);
            }
            else 
            {
                System.Reflection.MethodInfo mi = page.GetType().GetMethod("ShowMessage");
                if (mi != null) 
                    mi.Invoke(page, new Object[] { message, isKey, isErrorMessage });
            }
        }
        
        #endregion

        
        #region Various methods

        public static Int32 TryParse(string parseThis)
        {
            Int32 tmpInt;

            if (Int32.TryParse(parseThis, out tmpInt))
                return tmpInt;

            return -1;
        }

        /// <summary>
        /// Get map url for the address
        /// </summary>
        /// <param name="address"></param>
        /// <param name="city"></param>
        /// <returns></returns>
        public static string GetMapsUrl(string address, string city)
        {            
            return string.Format("{0}{1},{2}", ConfigurationManager.AppSettings["MapsUrl"],
                                              HttpUtility.UrlEncode(address.Trim()),
                                              HttpUtility.UrlEncode(city.Trim()));
        }

        /// <summary>
        /// Redirect the user to a given page. If no return url is given, redirect user to start page
        /// </summary>
        /// <param name="page"></param>
        /// <param name="returnUrlKey"></param>
        public static void RedirectToReturnUrlOrGoldStartPage(Page page, string returnUrlKey)
        {
            if (page.Request.QueryString[returnUrlKey] != null)
                page.Response.Redirect(page.Request.QueryString[returnUrlKey]);
            else
                page.Response.Redirect(GetDiGoldStartPage().LinkURL);
                //page.Response.Redirect(EPiFunctions.StartPage().LinkURL);
        }

        /// <summary>
        /// The price retrieved from Adlibris is without decimals (such as 16700 instead of 167.00)
        /// Get the price from xml node and return with decimals
        /// </summary>
        /// <param name="priceStr"></param>
        /// <returns></returns>
        public static string GetAdlibrisPrice(string priceStr)
        {
            string formattedPrice = "-";
            int price;
            if (int.TryParse(priceStr, out price))
            {
                formattedPrice = (price / 100.0).ToString("F");
            }
            return formattedPrice;
        }

        /// <summary>
        /// Call Populate method of a certain control
        /// </summary>
        /// <param name="control"></param>
        public static void PopulateWithPaging(Control control)
        {
            System.Reflection.MethodInfo mi = control.GetType().GetMethod("Populate");
            if (mi != null)
                mi.Invoke(control, null);
        }
        
        #endregion
        
        
        #region Webservice methods
        
        /// <summary>
        /// Set security credentials for One By One's Xml webservice client authentication
        /// </summary>
        /// <returns></returns>
        public static DocumentFactoryService SetUpOneByOneDocumentFactory()
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
        
        #endregion


        #region DISE
        
        //DISE
        //returnerar datum i svenskt format
        public static string GetSwedishDateFormat(DateTime datetime)
        {
            return datetime.ToString("yyyy-MM-dd");
        }

        //DISE
        //Returnerar datumet för senaste utgåva av dagens industri på nätet
        public static DateTime GetCurrentEditionDateDI()
        {
            SqlDataReader aDR = null;
            bool bUpdate = false;
            HttpApplicationState theApplication = HttpContext.Current.Application;
            try
            {
                if (theApplication["CurrentEditionDateSet"] == null)
                    bUpdate = true;
                else
                    if (((DateTime)theApplication["CurrentEditionDateSet"]).CompareTo(DateTime.Today) != 0)
                        bUpdate = true;

                if (bUpdate)
                {
                    aDR = SqlHelper.ExecuteReader(null, "Editions_GetCurrent", new System.Data.SqlClient.SqlParameter("@Today", DateTime.Today));

                    if (aDR.Read())
                    {
                        theApplication.Set("CurrentEditionDate", aDR["EditionDate"]);
                        theApplication.Set("CurrentEditionDateSet", DateTime.Today);
                    }
                    else
                    {
                        return DateTime.Today;
                    }
                    aDR.Close();
                }
                return (DateTime)theApplication["CurrentEditionDate"];
            }
            catch (Exception ex)
            {
                new Logger("GetCurrentEditionDateDI() - failed", ex.ToString());
                return DateTime.Today;
            }
            finally
            {
                if ((aDR != null) && !aDR.IsClosed)
                    aDR.Close();
            }
        }

        /// <summary>
        /// If possible, cast object to datetime and return a ShortDateString
        /// </summary>
        /// <param name="dateTime">Input parameter</param>
        /// <returns>A ShortDateString representation of DateTime object, otherwise an empty string</returns>
        public static string ToShortDateString(object dateTime)
        {
            if (dateTime != null && dateTime is DateTime)
                return GetSwedishDateFormat((DateTime)dateTime);

            return string.Empty;
        }
        
        #endregion


        /// <summary>
        /// Returns: loginPagePath(?ReturnUrl=xxx)
        /// </summary>
        public static string GetSsoLoginPageUrl(PageData pd, string returnUrl)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(GetFriendlyAbsoluteUrl(GetLoginPage(pd)));

            if (!string.IsNullOrEmpty(returnUrl))
                sb.Append("?ReturnUrl=" +  HttpUtility.UrlEncode(returnUrl));

            return sb.ToString();
        }
    }
}