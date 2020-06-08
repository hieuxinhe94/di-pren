using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;
using DagensIndustri.Tools.Classes;
using DIClassLib.Membership;
using DIClassLib.DbHelpers;
using DIClassLib.Misc;
using DIClassLib.Subscriptions;
using DIClassLib.DbHandlers;

namespace DagensIndustri.Templates.Public.Units.Placeable.MySettings2
{
    public partial class HolidayStop : UserControlBase
    {
        #region Constants
        private const string CHOOSE = "choose";
        private const string NEW = "new";
        #endregion 

        #region Properties
        /// <summary>
        /// Get the container page of this usercontrol
        /// </summary>
        //private Pages.MySettings2 MySettingsPage
        private DagensIndustri.Templates.Public.Pages.UserSettings.MySettings2 MySettingsPage
        {
            get
            {
                //return (Pages.MySettings2)Page;
                //return (DagensIndustri.Templates.Public.Pages.MySettings2)Page;
                return new DagensIndustri.Templates.Public.Pages.UserSettings.MySettings2();
            }
        }

        /// <summary>
        /// Get the Subscriber object from the container page of this usercontrol
        /// </summary>
        public SubscriptionUser2 Subscriber
        {
            get
            {
                return (SubscriptionUser2)MySettingsPage.Subscriber;
            }
        }

        public DataSet PendingAddresses
        {
            get
            {
                return ViewState["PendingAddresses"] as DataSet;
            }
            set
            {
                ViewState["PendingAddresses"] = value;
            }
        }

        /// <summary>
        /// Get number of existing holiday stops
        /// </summary>
        protected int NoOfHolidayStops
        {
            get
            {
                int noOfHolidayStops = 0;
                //if (Subscriber != null && Subscriber.HolidayStops != null)
                //{
                //    noOfHolidayStops = Subscriber.HolidayStops.Tables[0].Rows.Count;
                //}
                return noOfHolidayStops;
            }
        }

        /// <summary>
        /// Get number of pending temporary addresses
        /// </summary>
        protected int NoOfPendingAddresses
        {
            get
            {
                int noOfPendingAddresses = 0;
                if (PendingAddresses != null)
                {
                    noOfPendingAddresses = PendingAddresses.Tables[0].Rows.Count;
                }

                return noOfPendingAddresses;
            }
        }

        /// <summary>
        /// Get number of temporary addresses
        /// </summary>
        protected int NoOfTemporaryAddresses
        {
            get
            {
                int noOfTemporaryAddresses = 0;
                //if (Subscriber != null && Subscriber.TemporaryAddresses != null)
                //{
                //    noOfTemporaryAddresses = Subscriber.TemporaryAddresses.Tables[0].Rows.Count;
                //}
                return noOfTemporaryAddresses;
            }
        }
        #endregion

        #region General Events
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!IsPostBack)
            {
                if (MySettingsPage.UserIsAuthenticated)
                {
                    InitializeHolidayStops();
                    InitializeTemporaryAddresses();
                    InitializePermanentAddress();
                }
            }
            
            //RegisterScript();
        }        
        #endregion

        #region General Methods
        /// <summary>
        /// Set default values on inputfields
        /// </summary>
        public void SetDefaultValues()
        {
            //Holiday stop
            HolidayStopFromInput.Text = string.Empty;
            HolidayStopToInput.Text = string.Empty;

            ////Pending
            if (PrevTempAddrDropDownList.Items.Count > 0)
                PrevTempAddrDropDownList.SelectedIndex = 0;

            ClearTemporaryAddressFields();
            
            //Permanent 
            PermanentAddressCoInput.Text = string.Empty;
            PermanentAddressStreetInput.Text = string.Empty;
            PermanentAddressHouseNoInput.Text = string.Empty;
            PermanentAddressStairCaseInput.Text = string.Empty;
            PermanentAddressStairsInput.Text = string.Empty;
            PermanentAddressApartmentInput.Text = string.Empty;
            PermanentAddressZipInput.Text = string.Empty;
            PermanentAddressFromInput.Text = string.Empty;            
        }        

        /// <summary>
        /// Register clientscripts. When "Save buttons" are clicked, the id of the selected tab and section is saved in hidden fields.
        /// </summary>
//        private void RegisterScript()
//        {
//            HiddenField SelectedTabHiddenField = MySettingsPage.HiddenFieldSelectedTab;
//            HiddenField SelectedSectionHiddenField = MySettingsPage.HiddenFieldSelectedSection;
//            HyperLink SubscriptionHyperLink = MySettingsPage.HyperLinkSubscription;                       

//            // Create script for click on Save buttons where selected tab and section will be stored in hiddenfields
//            string script = string.Format(@"$(document).ready(function() {{
//                                                    $('#{0}').click(function () {{
//                                                    $('#{3}').val('{5}');
//                                                    $('#{4}').val('{6}');
//                                                }}),
//                                                    $('#{1}').click(function () {{
//                                                    $('#{3}').val('{5}');
//                                                    $('#{4}').val('{7}');
//                                                }}),
//                                                    $('#{2}').click(function () {{
//                                                    $('#{3}').val('{5}');
//                                                    $('#{4}').val('{8}');
//                                                }})                                                    
//                                            }});",

//                                            SaveHolidayStopButton.ClientID,
//                                            SaveTemporaryAddressButton.ClientID,
//                                            SavePermanentAddressButton.ClientID,

//                                            SelectedTabHiddenField.ClientID,
//                                            SelectedSectionHiddenField.ClientID,

//                                            SubscriptionHyperLink.NavigateUrl,
                                            
//                                            HolidayStopDiv.ClientID,
//                                            TemporaryAddressDiv.ClientID,
//                                            PermanentAddressDiv.ClientID
//                                        );

//            Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "ButtonClick_Subscription", script, true);
//        }
        
        /// <summary>
        /// Checks whether the given timespan is valid. 
        /// It checks so that from date is not after to date and earlier than earliest date.
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="earliestDate"></param>
        /// <returns></returns>
        private bool CheckTimeSpan(DateTime fromDate, DateTime toDate, DateTime earliestDate)
        {
            if (fromDate < earliestDate || fromDate > toDate)
                return false;

            return true;
        }

        /// <summary>
        /// Checks whether the given timespan is valid.
        /// It checks against existing dates so that selected period does not clash with other dates.
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="existingStartDate"></param>
        /// <param name="existingEndDate"></param>
        /// <returns></returns>
        private bool CheckTimeSpan(DateTime fromDate, DateTime toDate, DateTime existingStartDate, DateTime existingEndDate)
        {
            if ((fromDate >= existingStartDate && fromDate <= existingEndDate) || (toDate >= existingStartDate && toDate <= existingEndDate) || (fromDate < existingStartDate && toDate > existingEndDate))
                return false;

            return true;
        }

        /// <summary>
        /// Get earliest date an address can be changed
        /// </summary>
        /// <returns></returns>
        private DateTime EarliestDateAddress()
        {
            DateTime dt;
            switch (DateTime.Now.DayOfWeek)
            {
                case DayOfWeek.Monday:
                case DayOfWeek.Tuesday:
                case DayOfWeek.Wednesday:
                    dt = DateTime.Now.Date.AddDays(3);
                    break;
                default:
                    dt = DateTime.Now.Date.AddDays(5);
                    break;
            }

            return dt;
        }

        /// <summary>
        /// Send error mail to pren
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        private void MailErrorToPren(string subject, string body)
        {
            string emailAddressOnFailure = EPiFunctions.SettingsPageSetting(CurrentPage, "EmailAddressOnFailure") as string;
            MiscFunctions.SendMail(emailAddressOnFailure, emailAddressOnFailure, subject, body, true);
        }

        /// <summary>
        /// Send a confirmation mail to user
        /// </summary>
        private void SendConfirmationMail(string from, string subject, string body)
        {
            if (!string.IsNullOrEmpty(Subscriber.Email))
                MiscFunctions.SendMail(from, Subscriber.Email, subject, body, true);
            else
            {
                MySettingsPage.ShowMessage("/mysettings/errors/subscription/noconfirm", true, true);
            }
        }
        #endregion 

        #region HolidayStop

        #region Events
        /// <summary>
        /// Create client script to be run when delete holiday stop is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void HolidayStopRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        //{
        //    if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        //    {
        //        LinkButton deleteHolidayStopLinkButton = e.Item.FindControl("DeleteHolidayStopLinkButton") as LinkButton;
        //        deleteHolidayStopLinkButton.OnClientClick = MySettingsPage.CreateSelectedTabScript(MySettingsPage.HyperLinkSubscription.NavigateUrl, HolidayStopDiv.ClientID);
        //    }
        //}

        /// <summary>
        /// Remove a selected holiday stop.
        /// It can only be done if the holiday stop occur at least one week from now
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DeleteHolidayStop_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton deleteLinkButton = sender as LinkButton;
                string args = deleteLinkButton.CommandArgument;

                DateTime fromDate = DateTime.Parse(args.Split('|')[0]);
                DateTime toDate = DateTime.Parse(args.Split('|')[1]);

                //User can only delete holiday stops if they occur at least one week from now
                if (fromDate > DateTime.Now.AddDays(7))
                {
                    //if (Subscriber.DeleteHolidayStop(fromDate, toDate) != "OK")
                    //{
                        MySettingsPage.ShowMessage("/mysettings/errors/error", true, true);
                        new Logger("DeleteHolidayStop_Click() - failed to delete holidaystop in Cirix", CreateHolidayStopDetailsMessage(fromDate, toDate));
                    //}
                }
                else
                {
                    MySettingsPage.ShowMessage("/mysettings/errors/subscription/delete", true, true);
                }

                InitializeHolidayStops();
            }
            catch (Exception ex)
            {
                new Logger("DeleteHolidayStop_Click() - failed", ex.ToString());
                MySettingsPage.ShowMessage("/mysettings/errors/error", true, true);
            }
        }        

        /// <summary>
        /// Save a new holiday stop and send confirmation mail to user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SaveHolidayStop_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime fromDate;
                DateTime toDate;

                if (Page.IsValid && DateTime.TryParse(HolidayStopFromInput.Text, out fromDate) && DateTime.TryParse(HolidayStopToInput.Text, out toDate))
                {
                    //Get earliest date to stop. Parse to DateTime, to remove HH:MM:SS
                    DateTime earliestDateToStop = DateTime.Parse(EarliestDateHolidayStop().ToString("yyyy-MM-dd"));

                    //check input span, must be in range of subscription, and must be ahead of earliestDateToStop
                    if (!CheckTimeSpan(fromDate, toDate, earliestDateToStop))
                    {
                        //MySettingsPage.ShowMessage(string.Format(Translate("/mysettings/errors/subscription/date"), earliestDateToStop.ToString("yyyy-MM-dd"), Subscriber.SubEnd.ToString("yyyy-MM-dd")), false, true);
                        //return;
                    }

                    //Also, check that there's not already existing a holiday stop for selected period
                    if (NoOfHolidayStops > 0)
                    {
                        //foreach (DataRow dr in Subscriber.HolidayStops.Tables[0].Rows)
                        //{
                        //    DateTime existingStart = (DateTime)dr["SLEEPSTARTDATE"];
                        //    DateTime existingEnd = (DateTime)dr["SLEEPENDDATE"];

                        //    if (!CheckTimeSpan(fromDate, toDate, existingStart, existingEnd))
                        //    {
                        //        MySettingsPage.ShowMessage("/mysettings/errors/subscription/existing", true, true);
                        //        return;
                        //    }
                        //}
                    }

                    //if (Subscriber.CreateHolidayStop(fromDate, toDate) == "OK")
                    //{
                    //    InitializeHolidayStops();
                    //    SetDefaultValues();

                    //    //Send confirm mail to user
                    //    SendConfirmationMail((string)CurrentPage["MailFrom"], (string)CurrentPage["MailSubjectHB"], (string)CurrentPage["MailBodyHB"]);

                    //    MySettingsPage.ShowMessage("/mysettings/subscription/registered", true, false);                        
                    //}
                    else
                    {
                        //Send mail to pren
                        string message = CreateHolidayStopDetailsMessage(fromDate, toDate);
                        MailErrorToPren("WebService FAILED: tillfälligt uppehåll", message);
                        MySettingsPage.ShowMessage("/mysettings/errors/error", true, true);
                        new Logger("SaveHolidayStop_Click() - failed to create holidaystop in Cirix", message);
                    }
                }
                else
                {
                    MySettingsPage.ShowMessage("/mysettings/errors/subscription/dateformat", true, true);
                }
            }
            catch (Exception ex)
            {
                new Logger("SaveHolidayStop_Click() - failed", "Cusno: " + Subscriber.Cusno + " " + ex.ToString());
                //Send mail to Pren
                MailErrorToPren("WebService ERROR: tillfälligt uppehåll", "Kundnummer: " + Subscriber.Cusno + "<br />Start: " + HolidayStopFromInput.Text + "<br />Slut: " + HolidayStopToInput.Text + "<br />Felmeddelande:" + ex.ToString());
                MySettingsPage.ShowMessage("/mysettings/errors/error", true, true);
            }

            new Logger(Settings.LogEvent_HolidayStop, Subscriber.Cusno, true);
        }
        #endregion 
        
        #region Methods
        /// <summary>
        /// Initialize HolidayStop values with sleeping subscriptions
        /// </summary>
        protected void InitializeHolidayStops()
        {
            try
            {
                //If the subscriber has an IPAD subscription, then it is not possible to change holiday stop.
                //if (Subscriber.SubscriptionPaperCode == "IPAD")
                //{
                //    HolidayStopPlaceHolder.Visible = false;
                //}
                //else
                //{
                //    HolidayStopPlaceHolder.Visible = true;

                //    //Set min and max date on input controls
                //    string minDate = EarliestDateHolidayStop().ToString("yyyy-MM-dd");
                //    string maxDate = Subscriber.SubEnd.ToString("yyyy-MM-dd");

                //    HolidayStopFromInput.MinValue = minDate;
                //    HolidayStopFromInput.MaxValue = maxDate;

                //    HolidayStopToInput.MinValue = minDate;
                //    HolidayStopToInput.MaxValue = maxDate;

                //    //If result returned
                //    if (Subscriber.HolidayStops.Tables[0].Rows.Count > 0)
                //    {
                //        HolidayStopRepeater.Visible = true;
                //        HolidayStopRepeater.DataSource = Subscriber.HolidayStops;
                //        HolidayStopRepeater.DataBind();
                //    }
                //    else
                //    {
                //        //Hide repeater
                //        HolidayStopRepeater.Visible = false;
                //    }
                //}
            }
            catch (Exception ex)
            {
                new Logger("InitializeHolidayStops() - failed", ex.ToString());
                MySettingsPage.ShowMessage("/mysettings/errors/error", true, true);
            }
        }

        /// <summary>
        /// Gets the earliest date a holiday stop can start
        /// </summary>
        /// <returns></returns>
        protected DateTime EarliestDateHolidayStop()
        {
            //1 = monday, 7 = sunday
            int indexOfToday = GetTodayIndex();

            DateTime earliestDate;

            //if thursday - sunday, earliest on next tuesday
            if (indexOfToday > 3)
            {
                //Algorithm: 7 days in one week, index of tuesday is 2, 7 + 2 is 9 days, substract indexOfToday = days to tuesday
                earliestDate = DateTime.Now.AddDays(9 - indexOfToday);
            }
            else
            {
                //if monday - wednesday, add two days
                earliestDate = DateTime.Now.AddDays(2);
            }

            return earliestDate;
        }

        /// <summary>
        /// Get the index of today.
        /// </summary>
        /// <returns>index of today. Monday = 1 ... Sunday = 7</returns>
        private int GetTodayIndex()
        {
            //I could use (int)DateTime.Now.DayOfWeek
            //But that's locale-specific, then sunday can be 0

            int indexOfToday = 0;

            switch (DateTime.Now.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    indexOfToday = 1;
                    break;
                case DayOfWeek.Tuesday:
                    indexOfToday = 2;
                    break;
                case DayOfWeek.Wednesday:
                    indexOfToday = 3;
                    break;
                case DayOfWeek.Thursday:
                    indexOfToday = 4;
                    break;
                case DayOfWeek.Friday:
                    indexOfToday = 5;
                    break;
                case DayOfWeek.Saturday:
                    indexOfToday = 6;
                    break;
                case DayOfWeek.Sunday:
                    indexOfToday = 7;
                    break;
            }

            return indexOfToday;
        }

        /// <summary>
        /// Create a message with details about the holiday stop that is being saved.
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        private string CreateHolidayStopDetailsMessage(DateTime fromDate, DateTime toDate)
        {
            StringBuilder logMessageBuilder = new StringBuilder();
            logMessageBuilder.AppendFormat("UserName: {0} <br />", HttpContext.Current.User.Identity.Name);
            logMessageBuilder.AppendFormat("CustomerNumber: {0} <br />", Subscriber.Cusno);
            //logMessageBuilder.AppendFormat("SubscriptionNo: {0} <br />", Subscriber.SubscriptionNumber);
            logMessageBuilder.AppendFormat("FromDate: {0} <br />", fromDate);
            logMessageBuilder.AppendFormat("ToDate: {0} <br />", toDate);
            return logMessageBuilder.ToString();
        }
        #endregion
        #endregion

        #region Temporary Address

        #region Events
        /// <summary>
        /// If user selects a previous temporary address, show the address in the fields
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void PrevTempAddrDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //Clear all the fields
                ClearTemporaryAddressFields();

                if (PrevTempAddrDropDownList.SelectedValue != NEW && PrevTempAddrDropDownList.SelectedValue != CHOOSE)
                {
                    //If user has selected a previous temporary address, then set the address information in the field.
                    int addrno = int.Parse(PrevTempAddrDropDownList.SelectedValue.Split('|')[0]);
                    DateTime fromDate = DateTime.Parse(PrevTempAddrDropDownList.SelectedValue.Split('|')[1]);

                    //if (Subscriber.TemporaryAddresses != null)
                    //{
                    //    foreach (DataRow dr in Subscriber.TemporaryAddresses.Tables[0].Rows)
                    //    {
                    //        if (Convert.ToInt32(dr["ADDRNO"]) == addrno && Convert.ToDateTime(dr["STARTDATE"]).ToString("yyyy-MM-dd") == fromDate.ToString("yyyy-MM-dd"))
                    //        {
                    //            //street2 contains of = C/O and/or apartment number. 
                    //            int result;
                    //            string street2 = dr["STREET2"].ToString();
                    //            if (street2.Contains(" "))
                    //            {
                    //                //If a space is included, then split the text into C/O and apartment number. 
                    //                TempAddressCoInput.Text = street2.Substring(0, street2.LastIndexOf(" "));
                    //                TempAddressApartmentInput.Text = street2.Substring(street2.LastIndexOf(" ") + 1);
                    //            }
                    //            else if (int.TryParse(street2, out result))
                    //            {
                    //                //Otherwise, if street2 is numeric then it should be apartment number.
                    //                TempAddressApartmentInput.Text = street2;
                    //            }
                    //            else
                    //            {
                    //                //Otherwise it is C/O.
                    //                TempAddressCoInput.Text = street2;
                    //            }

                    //            TempAddressStreetInput.Text = dr["STREETNAME"].ToString();
                    //            TempAddressHouseNoInput.Text = dr["HOUSENO"].ToString();
                    //            TempAddressStairCaseInput.Text = dr["STAIRCASE"].ToString();
                    //            TempAddressStairsInput.Text = dr["APARTMENT"].ToString().Replace("TR", "");
                    //            TempAddressZipInput.Text = dr["ZIPCODE"].ToString();
                    //            TempAddressFromInput.Text = string.Empty;
                    //            TempAddressToInput.Text = string.Empty;

                    //            DisableTemporaryAddressFields(addrno);
                    //            break;
                    //        }
                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {
                new Logger("PrevTempAddrDropDownList_SelectedIndexChanged() - failed", ex.ToString());
                MySettingsPage.ShowMessage("/mysettings/errors/error", true, true);
                ClearTemporaryAddressFields();
            }
        }

        /// <summary>
        /// Create client script to be run when delete temporary address is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void PendingAddressRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        //{
        //    if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        //    {
        //        LinkButton deleteTemporaryAddressLinkButton = e.Item.FindControl("DeleteTemporaryAddressLinkButton") as LinkButton;
        //        deleteTemporaryAddressLinkButton.OnClientClick = MySettingsPage.CreateSelectedTabScript(MySettingsPage.HyperLinkSubscription.NavigateUrl, TemporaryAddressDiv.ClientID); 
        //    }
        //}

        /// <summary>
        /// Delete existing pending address
        /// It can only be done if the pending addresses occur at least one week from now
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DeleteTemporaryAddress_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton deleteTempAddrLinkButton = sender as LinkButton;
                string args = deleteTempAddrLinkButton.CommandArgument;

                int addrno = int.Parse(args.Split('|')[0]);
                DateTime fromDate = DateTime.Parse(args.Split('|')[1]);

                //User can only delete pending addresses if they occur at least one week from now
                if (fromDate > DateTime.Now.AddDays(7))
                {
                    //if (Subscriber.DeleteTemporaryAddress(addrno, fromDate) != "OK")
                    //{
                    //    MySettingsPage.ShowMessage("/mysettings/errors/error", true, true);
                    //    new Logger("DeleteTemporaryAddress_Click() - failed to delete temporary address in Cirix", CreateTempAddrDeleteDetailsMessage(addrno, fromDate));
                    //}
                }
                else
                {
                    MySettingsPage.ShowMessage("/mysettings/errors/subscription/delete", true, true);
                }

                //FillPendingAddresses(Subscriber.PendingAddresses);

                if (PrevTempAddrDropDownList.SelectedValue != NEW && PrevTempAddrDropDownList.SelectedValue != CHOOSE)
                {
                    int selectedAddrno = int.Parse(PrevTempAddrDropDownList.SelectedValue.Split('|')[0]);
                    DisableTemporaryAddressFields(selectedAddrno);
                }
            }
            catch (Exception ex)
            {
                new Logger("DeleteTemporaryAddress_Click() - failed", ex.ToString());
                MySettingsPage.ShowMessage("/mysettings/errors/error", true, true);
            }
        }

        /// <summary>
        /// Save a new Temporary address and send confirmation mail to user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SaveTemporaryAddress_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime fromDate;
                DateTime toDate;

                if (PrevTempAddrDropDownList.SelectedIndex > 0)
                {
                    if (Page.IsValid && DateTime.TryParse(TempAddressFromInput.Text, out fromDate) && DateTime.TryParse(TempAddressToInput.Text, out toDate))
                    {
                        //Get earliest date to stop. Parse to DateTime, to remove HH:MM:SS
                        DateTime earliestDateToStop = DateTime.Parse(EarliestDateAddress().ToString("yyyy-MM-dd"));

                        //check input span, must be in range of subscription, and must be ahead of earliestDateToStop
                        if (!CheckTimeSpan(fromDate, toDate, earliestDateToStop))
                        {
                            //MySettingsPage.ShowMessage(string.Format(Translate("/mysettings/errors/subscription/date"), earliestDateToStop.ToString("yyyy-MM-dd"), Subscriber.SubEnd.ToString("yyyy-MM-dd")), false, true);
                            //return;
                        }

                        //Also, check that there's not already existing a pending address for selected period
                        if (NoOfPendingAddresses > 0)
                        {
                            foreach (DataRow dr in PendingAddresses.Tables[0].Rows)
                            {
                                DateTime existingStart = (DateTime)dr["STARTDATE"];
                                DateTime existingEnd = (DateTime)dr["ENDDATE"];

                                if (!CheckTimeSpan(fromDate, toDate, existingStart, existingEnd))
                                {
                                    MySettingsPage.ShowMessage("/mysettings/errors/subscription/existing", true, true);
                                    return;
                                }
                            }
                        }

                        bool success = false;
                        string stairs = string.Empty;
                        string coAndApartment = string.Empty;

                        //if user selected an already existing address
                        if (PrevTempAddrDropDownList.SelectedValue != NEW)
                        {
                            //split selectedvalue in Dropdownlist to get addrno and org startdate
                            int addrNo = int.Parse(PrevTempAddrDropDownList.SelectedValue.Split('|')[0]);
                            DateTime addrStartDate = DateTime.Parse(PrevTempAddrDropDownList.SelectedValue.Split('|')[1]);

                            //Fromdate can't be less than selected address startDate
                            if (fromDate < addrStartDate)
                            {
                                //"Den valda adressen är giltig from "
                                MySettingsPage.ShowMessage(Translate("/mysettings/errors/subscription/validfrom") + " " + addrStartDate.ToString("yyyy-MM-dd"), false, true);
                                return;
                            }

                            bool createTemporaryNewAddress = false;
                            DateTime dateOriginalStartdate = DateTime.MinValue;

                            #region not used code
                            //DataSet tempAddressDataSet = Subscriber.TemporaryAddresses;
                            //if (tempAddressDataSet != null && tempAddressDataSet.Tables[0].Rows.Count > 0)
                            //{
                            //    foreach (DataRow tempAddress in tempAddressDataSet.Tables[0].Rows)
                            //    {
                            //        //check if user is trying to change an already existing address. 
                            //        if (Convert.ToInt32(tempAddress["ADDRNO"]) == addrNo)
                            //        {
                            //            if (tempAddress["STREETNAME"].ToString() != TempAddressStreetInput.Text.Trim() ||
                            //                tempAddress["HOUSENO"].ToString() != TempAddressHouseNoInput.Text.Trim() ||
                            //                tempAddress["STAIRCASE"].ToString() != TempAddressStairCaseInput.Text.Trim() ||
                            //                tempAddress["APARTMENT"].ToString().ToUpper().Replace("TR", "") != TempAddressStairsInput.Text.Trim().ToUpper().Replace("TR", "") ||
                            //                tempAddress["STAIRCASE"].ToString() != TempAddressStairCaseInput.Text.Trim() ||
                            //                tempAddress["STREET2"].ToString() != (string.Format("{0} {1}", TempAddressCoInput.Text.Trim(), TempAddressApartmentInput.Text.Trim())).Trim() ||
                            //                tempAddress["ZIPCODE"].ToString() != TempAddressZipInput.Text.Trim())
                            //            {
                            //                createTemporaryNewAddress = true;
                            //                dateOriginalStartdate = Convert.ToDateTime(tempAddress["STARTDATE"]);
                            //                break;
                            //            }
                            //        }
                            //    }
                            //}
                            #endregion

                            //If user is trying to change an already existing address, end the temporary address first and then create a new temporary address.
                            if (createTemporaryNewAddress)
                            {
                                success = Subscriber.EndAvailableTemporaryAddress(Subscriber.Cusno, addrNo, dateOriginalStartdate) == "OK";
                                if (success)
                                {
                                    success = CreateTemporaryNewAddress(fromDate, toDate, out stairs, out coAndApartment);
                                }

                                //If everything was stored successfully in Cirix, repopulate the temporary address dropdown control to get the new temporary address.
                                if (success)
                                    FillTemporaryAddress();
                            }
                            else
                            {
                                //User is not trying to change an already existing address. Set from and to date to the selected address.
                                //success = Subscriber.CreateTemporaryAddress(addrNo, fromDate, toDate) == "OK";
                            }
                        }
                        //if new address
                        else
                        {
                            success = CreateTemporaryNewAddress(fromDate, toDate, out stairs, out coAndApartment);
                        }

                        if (success)
                        {
                            if (PrevTempAddrDropDownList.SelectedValue == NEW)
                                FillTemporaryAddress();

                            //FillPendingAddresses(Subscriber.PendingAddresses);

                            SetDefaultValues();

                            //Send confirm mail to user
                            SendConfirmationMail((string)CurrentPage["MailFrom"], (string)CurrentPage["MailSubjectTA"], (string)CurrentPage["MailBodyTA"]);

                            MySettingsPage.ShowMessage("/mysettings/subscription/registered", true, false);                            
                        }
                        else
                        {
                            //Send mail to pren
                            MailErrorToPren("WebService FAILED: tillfällig adressändring", "Kundnummer: " + Subscriber.Cusno + "<br />Start: " + fromDate + "<br />Slut: " + toDate);
                            MySettingsPage.ShowMessage("/mysettings/errors/error", true, true);
                            new Logger("SaveTemporaryAddress_Click() - failed to create temporary address in Cirix", 
                                        CreateTempAddrSaveDetailsMessage(TempAddressStreetInput.Text, TempAddressHouseNoInput.Text, TempAddressStairCaseInput.Text,
                                                                            stairs, coAndApartment, TempAddressZipInput.Text, fromDate, toDate));
                            
                            new Logger(Settings.LogEvent_TempAddressChange, Subscriber.Cusno, false);
                        }
                    }
                    else
                    {
                        MySettingsPage.ShowMessage("/mysettings/errors/subscription/dateformat", true, true);
                    }
                }
                else
                {
                    MySettingsPage.ShowMessage("/mysettings/errors/subscription/noaddress", true, true);
                }
            }
            catch (Exception ex)
            {
                new Logger(Settings.LogEvent_TempAddressChange, Subscriber.Cusno, false);
                new Logger("SaveTemporaryAddress_Click() - failed", "Cusno: " + Subscriber.Cusno + " " + ex.ToString());
                //Send mail to Pren                
                MailErrorToPren("WebService ERROR: tillfällig adressändring", "Kundnummer: " + Subscriber.Cusno + "<br />Start: " + TempAddressFromInput.Text + "<br />Slut: " + TempAddressToInput.Text + "<br />Felmeddelande:" + ex.ToString());
                MySettingsPage.ShowMessage("/mysettings/errors/error", true, true);
            }

            new Logger(Settings.LogEvent_TempAddressChange, Subscriber.Cusno, true);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Fills PendingAddressRepeater and dropdownlist PrevTempAddrDropDownList with addresses and pending addresses
        /// Triggers on event OnPreRender on PhTemporaryAddressChange
        /// </summary>
        protected void InitializeTemporaryAddresses()
        {
            try
            {
                //If the subscriber does not have a DI subscription, it is not possible to change temporary address.
                //if (Subscriber.SubscriptionPaperCode != "DI")
                //{
                //    TemporaryAddressPlaceHolder.Visible = false;
                //}
                //else
                //{
                //    TemporaryAddressPlaceHolder.Visible = true;

                //    //Set min and max date on input controls
                //    string minDate = EarliestDateAddress().ToString("yyyy-MM-dd");
                //    string maxDate = Subscriber.SubEnd.ToString("yyyy-MM-dd");

                //    TempAddressFromInput.MinValue = minDate;
                //    TempAddressFromInput.MaxValue = maxDate;

                //    TempAddressToInput.MinValue = minDate;
                //    TempAddressToInput.MaxValue = maxDate;

                //    FillTemporaryAddress();
                //    PrevTempAddrDropDownList.Attributes.Add("onchange", MySettingsPage.CreateSelectedTabScript(MySettingsPage.HyperLinkSubscription.NavigateUrl, TemporaryAddressDiv.ClientID));
                //    PrevTempAddrDropDownList.SelectedIndexChanged += new EventHandler(PrevTempAddrDropDownList_SelectedIndexChanged);

                //    //Fill the pending address repeater
                //    FillPendingAddresses(Subscriber.PendingAddresses);
                //}
            }
            catch (Exception ex)
            {
                new Logger("InitializeTemporaryAddresses() - failed", ex.ToString());
                MySettingsPage.ShowMessage("/mysettings/errors/error", true, true);
            }
        }

        /// <summary>
        /// Fill the PendingAddressRepeater with pending addresses
        /// </summary>
        private void FillPendingAddresses(DataSet pendingAddresses)
        {
            PendingAddresses = pendingAddresses;

            if (NoOfPendingAddresses > 0)
            {
                //Delete rows that are not temporary address changes
                int i = 0;
                foreach (DataRow dr in PendingAddresses.Tables[0].Rows)
                {
                    if (dr["CHANGETYPE"].ToString() != "Temporary")
                        PendingAddresses.Tables[0].Rows[i].Delete();
                    i++;
                }
                //Accept changes
                PendingAddresses.AcceptChanges();
            }
            
            PendingAddressRepeater.DataSource = PendingAddresses;
            PendingAddressRepeater.DataBind();

            //This is done because when the data source of the Repeater is set but no data is returned, the control renders 
            //the HeaderTemplate and FooterTemplate with no items.
            PendingAddressRepeater.Visible = NoOfPendingAddresses > 0; 
        }

        /// <summary>
        /// Fill Temporary address dropdown list
        /// </summary>
        private void FillTemporaryAddress()
        {
            //Get customers addresses
            //DataSet tempAddressDataSet = Subscriber.TemporaryAddresses;

            ////Get selected value
            //string selectedValue = PrevTempAddrDropDownList.SelectedValue;

            //if (tempAddressDataSet != null && tempAddressDataSet.Tables[0].Rows.Count > 0)
            //{
            //    //Add column to dataset with merged ADDRNO and STARTDATE. 
            //    //This column is used as DataValueField on Dropdownlist
            //    tempAddressDataSet.Tables[0].Columns.Add("ADDRNOSTART", typeof(string));
            //    foreach (DataRow dr in tempAddressDataSet.Tables[0].Rows)
            //        dr["ADDRNOSTART"] = dr["ADDRNO"].ToString() + "|" + dr["STARTDATE"].ToString();

            //    PrevTempAddrDropDownList.DataSource = tempAddressDataSet;
            //    PrevTempAddrDropDownList.DataBind();
            //}

            //ListItem liChoose = new ListItem(Translate("/mysettings/subscription/temporaryaddress/chooseaddress"), CHOOSE);
            //ListItem liNew = new ListItem(Translate("/mysettings/subscription/temporaryaddress/createnewaddress"), NEW);

            ////Add items if not already in list
            //if (!PrevTempAddrDropDownList.Items.Contains(liChoose))
            //{
            //    //Insert two more options, first and last
            //    PrevTempAddrDropDownList.Items.Insert(0, liChoose);
            //    PrevTempAddrDropDownList.Items.Insert(PrevTempAddrDropDownList.Items.Count, liNew);
            //}

            ////set the selected value
            //selectedValue = NoOfTemporaryAddresses > 0 ? selectedValue : NEW;
            //PrevTempAddrDropDownList.SelectedIndex = PrevTempAddrDropDownList.Items.IndexOf(PrevTempAddrDropDownList.Items.FindByValue(selectedValue));            
        }

        /// <summary>
        /// Create a new temporary address
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="stairs"></param>
        /// <param name="coAndApartment"></param>
        /// <returns></returns>
        private bool CreateTemporaryNewAddress(DateTime fromDate, DateTime toDate, out string stairs, out string coAndApartment)
        {
            //Number of stairs must always be followed by TR without any space
            stairs = TempAddressStairsInput.Text;
            if (!string.IsNullOrEmpty(stairs) && !stairs.ToUpper().EndsWith("TR"))
                stairs = stairs + "TR";

            coAndApartment = string.Format("{0} {1}", TempAddressCoInput.Text, TempAddressApartmentInput.Text).Trim();

            //bool success = Subscriber.CreateTemporaryNewAddress(TempAddressStreetInput.Text, TempAddressHouseNoInput.Text, TempAddressStairCaseInput.Text,
            //                                                stairs, coAndApartment, TempAddressZipInput.Text, fromDate, toDate) == "OK";
            //return success;

            return true;
        }

        /// <summary>
        /// Clear all the temporary address fields
        /// </summary>
        private void ClearTemporaryAddressFields()
        {
            TempAddressCoInput.Text = string.Empty;
            TempAddressStreetInput.Text = string.Empty;
            TempAddressHouseNoInput.Text = string.Empty;
            TempAddressStairCaseInput.Text = string.Empty;
            TempAddressStairsInput.Text = string.Empty;
            TempAddressApartmentInput.Text = string.Empty;
            TempAddressZipInput.Text = string.Empty;
            TempAddressFromInput.Text = string.Empty;
            TempAddressToInput.Text = string.Empty;

            DisableTemporaryAddressFields(false);
        }
        
        /// <summary>
        /// Disable or enable fields in section temporary address
        /// </summary>
        /// <param name="disable"></param>
        private void DisableTemporaryAddressFields(bool disable)
        {
            TempAddressCoInput.Disabled = disable;
            TempAddressStreetInput.Disabled = disable;
            TempAddressHouseNoInput.Disabled = disable;
            TempAddressStairCaseInput.Disabled = disable;
            TempAddressStairsInput.Disabled = disable;
            TempAddressApartmentInput.Disabled = disable;
            TempAddressZipInput.Disabled = disable;
        }

        /// <summary>
        /// Disable or enable fields in section temporary address depending on selected addrno
        /// </summary>
        /// <param name="addrno"></param>
        private void DisableTemporaryAddressFields(int addrno)
        {
            bool disable = false;
            foreach (DataRow pendingAddrDataRow in PendingAddresses.Tables[0].Rows)
            {
                if (Convert.ToInt32(pendingAddrDataRow["ADDRNO"]) == addrno)
                {
                    disable = true;
                    break;
                }
            }

            DisableTemporaryAddressFields(disable);
        }

        /// <summary>
        /// Create a message with details about the temporary address that is being delete.
        /// </summary>
        /// <param name="addrNo"></param>
        /// <param name="fromDate"></param>
        /// <returns></returns>
        private string CreateTempAddrDeleteDetailsMessage(int addrNo, DateTime fromDate)
        {
            StringBuilder logMessageBuilder = new StringBuilder();
            logMessageBuilder.AppendFormat("UserName: {0} <br />", HttpContext.Current.User.Identity.Name);
            logMessageBuilder.AppendFormat("CustomerNumber: {0} <br />", Subscriber.Cusno);
            //logMessageBuilder.AppendFormat("SubscriptionNo: {0} <br />", Subscriber.SubscriptionNumber);
            logMessageBuilder.AppendFormat("FromDate: {0} <br />", fromDate);
            logMessageBuilder.AppendFormat("AddrNo: {0} <br />", addrNo);
            return logMessageBuilder.ToString();
        }

        /// <summary>
        /// Create a message with details about the temporary address that is being saved.
        /// </summary>
        /// <param name="street"></param>
        /// <param name="houseNo"></param>
        /// <param name="stairCase"></param>
        /// <param name="apartment"></param>
        /// <param name="co"></param>
        /// <param name="zip"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        private string CreateTempAddrSaveDetailsMessage(string street, string houseNo, string stairCase, string apartment, string co, string zip, DateTime fromDate, DateTime toDate)
        {
            StringBuilder logMessageBuilder = new StringBuilder();
            logMessageBuilder.AppendFormat("UserName: {0} <br />", HttpContext.Current.User.Identity.Name);
            logMessageBuilder.AppendFormat("CustomerNumber: {0} <br />", Subscriber.Cusno);
            //logMessageBuilder.AppendFormat("SubscriptionNo: {0} <br />", Subscriber.SubscriptionNumber);
            logMessageBuilder.AppendFormat("Streetname: {0} <br />", street);
            logMessageBuilder.AppendFormat("Houseno: {0} <br />", houseNo);
            logMessageBuilder.AppendFormat("Staircase: {0} <br />", stairCase);
            logMessageBuilder.AppendFormat("Apartment: {0} <br />", apartment);
            logMessageBuilder.AppendFormat("Street2: {0} <br />", co);
            logMessageBuilder.AppendFormat("Zipcode: {0} <br />", zip);
            logMessageBuilder.AppendFormat("FromDate: {0} <br />", fromDate);
            logMessageBuilder.AppendFormat("ToDate: {0} <br />", toDate);
            return logMessageBuilder.ToString();
        }
        #endregion

        #endregion

        #region Permanent Address
        
        #region Events
        protected void SavePermanentAddress_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    DateTime fromDate;
            //    string stairs = string.Empty;
            //    string coAndApartment = string.Empty;

            //    if (Page.IsValid && DateTime.TryParse(PermanentAddressFromInput.Text, out fromDate))
            //    {
            //        //Get earliest date to stop. Parse to DateTime, to remove HH:MM:SS
            //        DateTime earliestDateToStop = DateTime.Parse(EarliestDateAddress().ToString("yyyy-MM-dd"));

            //        //check input date, must be ahead of earliestDateToStop
            //        if (fromDate < earliestDateToStop)
            //        {
            //            //MySettingsPage.ShowMessage(string.Format(Translate("/mysettings/errors/subscription/date"), earliestDateToStop.ToString("yyyy-MM-dd"), Subscriber.SubEnd.ToString("yyyy-MM-dd")), false, true);
            //            //return;
            //        }


            //        //Number of stairs must always be followed by TR without any space
            //        stairs = PermanentAddressStairsInput.Text;
            //        if (!string.IsNullOrEmpty(stairs) && !stairs.ToUpper().EndsWith("TR"))
            //            stairs = stairs + "TR";

            //        coAndApartment = string.Format("{0} {1}", PermanentAddressCoInput.Text, PermanentAddressApartmentInput.Text).Trim();                    

            //        if (Subscriber.CreatePermanentAddress(PermanentAddressStreetInput.Text, PermanentAddressHouseNoInput.Text, PermanentAddressStairCaseInput.Text,
            //                                                stairs, coAndApartment, PermanentAddressZipInput.Text, fromDate) == "OK")
            //        {
            //            //Send confirmation mail to user
            //            SendConfirmationMail((string)CurrentPage["MailFrom"], (string)CurrentPage["MailSubjectPA"], (string)CurrentPage["MailBodyPA"]);

            //            MySettingsPage.ShowMessage("/mysettings/subscription/permanentaddress/registered", true, false);

            //            SetDefaultValues();

            //            List<string> xtra = CirixDbHandler.GetCustomerXtraFields(Subscriber.Cusno);
            //            if (!string.IsNullOrEmpty(xtra[0] + xtra[1] + xtra[2]))
            //                DIClassLib.OneByOne.Obo.OboUnsubscribe(Subscriber.Cusno, xtra[0], xtra[1], xtra[2]);
            //        }
            //        else
            //        {
            //            //Send mail to pren
            //            MailErrorToPren("WebService FAILED: permanent adressändring", "Kundnummer: " + Subscriber.Cusno + "<br />Start: " + fromDate);
            //            MySettingsPage.ShowMessage("/mysettings/errors/error", true, true);
            //            new Logger("SavePermanentAddress_Click() - failed to create permanent address in Cirix", 
            //                            CreatePermanentAddrSaveDetailsMessage(PermanentAddressStreetInput.Text, PermanentAddressHouseNoInput.Text, PermanentAddressStairCaseInput.Text, 
            //                                                                    stairs, coAndApartment, PermanentAddressZipInput.Text, fromDate));
            //        }
            //    }
            //    else
            //    {
            //        MySettingsPage.ShowMessage("/mysettings/errors/subscription/dateformat", true, true);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    new Logger("SavePermanentAddress_Click() - failed", "Cusno: " + Subscriber.Cusno + " " + ex.ToString());
            //    //Send mail to Pren
            //    MailErrorToPren("WebService ERROR: permanent adressändring", "Kundnummer: " + Subscriber.Cusno + "<br />Start: " + PermanentAddressFromInput.Text);
            //    MySettingsPage.ShowMessage("/mysettings/errors/error", true, true);
            //}

            //new Logger(Settings.LogEvent_PermAddressChange, Subscriber.Cusno, true);
        }
        #endregion

        #region Methods
        protected void InitializePermanentAddress()
        {
            try
            {
                //Set min and max date on input controls
                //PermanentAddressFromInput.MinValue = EarliestDateAddress().ToString("yyyy-MM-dd");
                //PermanentAddressFromInput.MaxValue = Subscriber.SubEnd.ToString("yyyy-MM-dd");
            }
            catch (Exception ex)
            {
                new Logger("InitializePermanentAddress() - failed", ex.ToString());
                MySettingsPage.ShowMessage("/mysettings/errors/error", true, true);
            }
        }

        /// <summary>
        /// Create a message with details about the permanent address that is being saved.
        /// </summary>
        /// <param name="street"></param>
        /// <param name="houseNo"></param>
        /// <param name="stairCase"></param>
        /// <param name="apartment"></param>
        /// <param name="co"></param>
        /// <param name="zip"></param>
        /// <param name="fromDate"></param>        
        /// <returns></returns>
        private string CreatePermanentAddrSaveDetailsMessage(string street, string houseNo, string stairCase, string apartment, string co, string zip, DateTime fromDate)
        {
            StringBuilder logMessageBuilder = new StringBuilder();
            logMessageBuilder.AppendFormat("UserName: {0} <br />", HttpContext.Current.User.Identity.Name);
            logMessageBuilder.AppendFormat("CustomerNumber: {0} <br />", Subscriber.Cusno);
            logMessageBuilder.AppendFormat("Streetname: {0} <br />", street);
            logMessageBuilder.AppendFormat("Houseno: {0} <br />", houseNo);
            logMessageBuilder.AppendFormat("Staircase: {0} <br />", stairCase);
            logMessageBuilder.AppendFormat("Apartment: {0} <br />", apartment);
            logMessageBuilder.AppendFormat("Street2: {0} <br />", co);
            logMessageBuilder.AppendFormat("Zipcode: {0} <br />", zip);
            logMessageBuilder.AppendFormat("FromDate: {0} <br />", fromDate);
            return logMessageBuilder.ToString();
        }
        #endregion

        #endregion


        #region end DiY subs
        //protected void ButtonEndDiYSubs_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        long subsno = 0;
        //        int extno = 0;

        //        CirixDbHandler.Ws.ChangeSubscriptionEnddate_(subsno, extno, DateTime.Now);
        //        CirixDbHandler.Ws.CancelSubscription_("WEBCIRIX", subsno, extno, "01");

        //    }
        //    catch (Exception ex)
        //    {
        //        new Logger("ButtonEndDiYSubs_Click() - failed", "Cusno: " + Subscriber.CustomerNumber + " " + ex.ToString());
        //        MailErrorToPren("WebService ERROR: misslyckades med att avsluta DiY prenumeration", "Kundnummer: " + Subscriber.CustomerNumber + "<br />");
        //        MySettingsPage.ShowMessage("/mysettings/errors/error", true, true);
        //    }
        //}
        #endregion

    }
}