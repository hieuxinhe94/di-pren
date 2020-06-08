using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;
using EPiServer.Core;
using DagensIndustri.Tools.Classes.BaseClasses;
using DagensIndustri.Tools.Classes.Campaign;
using System.Data;
using DIClassLib.DbHelpers;
using DIClassLib.CardPayment;
using DIClassLib.DbHandlers;
using DagensIndustri.Tools.Classes;
using DIClassLib.Misc;


namespace DagensIndustri.Templates.Public.Pages.Campaign
{
    public partial class CampaignPage : CampaignTemplatePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            //Add javascript and css for Jquery Dialogue
            RegisterClientScriptFile("/Templates/Public/Styles/campaign/js/jquery-1.4.2.min.js");
            RegisterClientScriptFile("/Templates/Public/Styles/campaign/js/jquery-ui-1.8.2.custom.min.js");
            //Reqister css for tabs and dialogue. We don't use jquery for tabs, just using jquery css
            RegisterCssFile("/Templates/Public/Styles/campaign/jquery-ui-1.8.custom.css");

            //Register javascript for flash
            //only register js if property is set
            if (CurrentPage["FlashMovie"] != null)
            {
                //Register javascript for swfobject
                RegisterClientScriptFile("/Templates/Public/Styles/campaign/js/swfobject.js");
                //Show placeholder for script
                PhFlashScript.Visible = true;
            }
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (!IsPostBack)
            {
                SetUpCampaign();

                //MAC will be returned from Auriga after payment
                if (Request.QueryString["MAC"] != null)
                    HandleAurigaReturn();

                //Querystring complete when campaign is completed
                if (Request.QueryString["complete"] != null)
                {
                    string productNo = Request.QueryString["complete"];
                    ThanksDi.Visible = productNo.Equals("01");
                    ThanksWeekend.Visible = productNo.Equals("05");

                    HideForms();
                }
            }
        }

        /// <summary>
        /// Set up offercodes, image and flash
        /// </summary>
        protected void SetUpCampaign()
        {
            try
            {
                //**************
                //Set up TopImage based on theme
                //**************
                EPiServer.Core.PageData CampaignRootPage = GetPage((EPiServer.Core.PageReference)CurrentPage["CampaignRootPage"]);

                if (CurrentPage["CampaignTheme"] == null) //Theme DI
                {
                    TopImage.ImageUrl = CampaignRootPage["TopImageUrlDi"].ToString();
                    TopImage.AlternateText = Translate("/campaigns/various/diimagealt");
                }
                else if (CurrentPage["CampaignTheme"].ToString() == "CampThemeWeekend") //Theme Weekend
                {
                    TopImage.ImageUrl = CampaignRootPage["TopImageUrlWeekend"].ToString();
                    TopImage.AlternateText = Translate("/campaigns/various/weekendimagealt");
                }
                else //Theme dimension
                {
                    TopImage.ImageUrl = CampaignRootPage["TopImageUrlDimension"].ToString();
                    TopImage.AlternateText = Translate("/campaigns/various/dimensionimagealt");
                }

                //**************
                //Set up right image
                //**************

                if (CurrentPage["ImageUrl"] != null)
                {
                    ImgCampaign.Visible = true;
                    ImgCampaign.ImageUrl = CurrentPage["ImageUrl"] as string;
                    ImgCampaign.AlternateText = CurrentPage["ImageAltText"] as string;
                }

                //**************
                //Set up footer
                //**************

                if (CurrentPage["CampaignTheme"] != null)
                {
                    FooterDimension.Visible = CurrentPage["CampaignTheme"].Equals("CampThemeDimension");
                    FooterWeekend.Visible = CurrentPage["CampaignTheme"].Equals("CampThemeWeekend");
                }
                else
                {
                    FooterDi.Visible = true;
                }

                //Move
                //**************
                //Set up offercodes
                //**************

                List<OfferCode> offerCodeList = new List<OfferCode>();

                //Loop through all offercodes (from Campaign db)
                foreach (DataRow row in CurrentPage.GetOfferCodes().Tables[0].Rows)
                {
                    //Create new offercode object
                    OfferCode offerCode = new OfferCode(row);
                    //Add to list<>
                    offerCodeList.Add(offerCode);
                    //Add option listitem
                    string text;
                    if (offerCode.IsAutogiro)
                        text = offerCode.Text + ", " + offerCode.TotalPrice / offerCode.SubsLength + " :-/mån (exkl.moms)";
                    else
                        text = offerCode.Text + (offerCode.TotalPrice > 0 ? ", " + offerCode.TotalPrice + " :- (exkl.moms)" : string.Empty);

                    string value = offerCode.OfferCodeId.ToString();
                    RblOfferCodes.Items.Add(new ListItem(text, value));
                }

                //If only one offercode, select it and hide offercode area
                if (RblOfferCodes.Items.Count == 1)
                {
                    ShowOptions(offerCodeList[0]);
                    RblOfferCodes.Items[0].Selected = true;
                    PhOfferCodes.Visible = false;
                }

                //Save list<> in ViewState property
                OfferCodeList = offerCodeList;
            }
            catch (Exception ex)
            {
                new Logger("SetUpCampaign() - failed", ex.ToString());
                ShowError(string.Empty);
            }
        }

        /// <summary>
        /// Triggers when user click on offercode list
        /// </summary>
        protected void RblOfferCodesIndexChanged(object sender, EventArgs e)
        {
            OfferCode offerCode = GetSelectedOfferCode();
            ShowOptions(offerCode);
        }

        private void ShowOptions(OfferCode offerCode)
        {
            if (offerCode != null)
            {
                //show payment info if not autogiro and not for free
                PhPaymentOptions.Visible = !offerCode.IsAutogiro && offerCode.Price > 0;

                if (PhPaymentOptions.Visible && CurrentPage["HideOtherPayerOption"] != null)
                    RblPayMethod.Items.Remove(RblPayMethod.Items.FindByValue("invOther"));

                //show autogiro info
                PhAutogiroOption.Visible = offerCode.IsAutogiro;

                //if studentoffer, show field for birthno and hide option for other payer
                if (offerCode.IsStudent)
                {
                    RblPayMethod.Items.Remove(RblPayMethod.Items.FindByValue("invOther"));
                    CampaignForm.ShowBirthNo(true);
                }
                else
                    CampaignForm.ShowBirthNo(false);
            }
        }

        /// <summary>
        /// Collect data and do stuff
        /// </summary>
        protected void BtnSubmitOnClick(object sender, EventArgs e)
        {
            try
            {
                if (this.IsValid)
                {
                    //If other payer, show other payer form and return
                    if (RblPayMethod.SelectedValue.Equals("invOther") && !CampaignFormOtherPayer.Visible)
                    {
                        HandleFormVisibility(true, false, false);   //Show other payer form to collect more data
                        return;
                    }

                    string targetGroup = CurrentPage.GetTargetGroup().Tables[0].Rows[0]["targetGroupName"].ToString();
                    OfferCode offerCode = GetSelectedOfferCode();   //Get selected offercode from viewstate
                    CampaignUser campaignUser = new CampaignUser(CampaignForm.ActiveTab, CampaignForm.FirstName, CampaignForm.LastName, CampaignForm.StreetName, 
                                                                 CampaignForm.StreetNo, CampaignForm.Entrance, CampaignForm.Stairs, CampaignForm.Apartment, 
                                                                 CampaignForm.Zip, CampaignForm.City, CampaignForm.Phone, CampaignForm.Email, CampaignForm.Co,
                                                                 CampaignForm.BirthNo, CampaignForm.Company, CampaignForm.OrgNo, offerCode, targetGroup, true);

                    if (offerCode.IsStudent && !IsUserStudent(campaignUser.BirthNo)) //If student offer, verify that user really is a student
                    {
                        ShowModalWindow("nostudent", "350", "auto");
                        return;
                    }

                    if (CampaignFormOtherPayer.Visible) //other payer form visible, add payer
                        campaignUser.Payer = new CampaignUser(CampaignForm.ActiveTab, CampaignForm.FirstName, CampaignForm.LastName, CampaignForm.StreetName,
                                                            CampaignForm.StreetNo, CampaignForm.Entrance, CampaignForm.Stairs, CampaignForm.Apartment,
                                                            CampaignForm.Zip, CampaignForm.City, CampaignForm.Phone, CampaignForm.Email, CampaignForm.Co,
                                                            CampaignForm.BirthNo, CampaignForm.Company, CampaignForm.OrgNo, offerCode, targetGroup, false);

                    if (PhPaymentOptions.Visible && RblPayMethod.SelectedValue.Equals("card")) //card payment
                    {
                        AurigaPrepare auPrep = GetAurigaPerpareObj(offerCode, campaignUser);
                        StoreUser(campaignUser, auPrep.CustomerRefNo);      //Save CampaignUser
                        auPrep.SaveDataBeforePostensPage();                 //Save data to paytrans
                        Response.Redirect(auPrep.GetAurigaUrl(), false);    //Redirect to Auriga page 
                    }
                    else  //not card payment
                    {
                        string invMode = campaignUser.OfferCode.IsAutogiro ? Settings.InvoiceMode_AutoGiro : Settings.InvoiceMode_BankGiro;
                        string errMsg = CirixDbHandler.TryInsertSubs(campaignUser, invMode);

                        if (!string.IsNullOrEmpty(errMsg))
                        {
                            //Uncomment if you want silence even if customer already exists in Cirix

                            //if (errMsg.Equals(Translate("/campaigns/errors/alreadyincirix")))
                            //{
                            //    //Skriv till db
                            //    MsSqlHandler.InsertCampaignUser(campaignUser, false);                                
                            //    Complete(campaignUser);
                            //}
                            //else
                            //{
                            ShowError(errMsg);
                            //}
                        }
                        else
                        {
                            Complete(campaignUser);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new Logger("BtnSubmitOnClick() - failed", ex.ToString());
                ShowError(string.Empty);
            }
        }

        /// <summary>
        /// When customer and subsription are created.
        /// Check if premium item is associated with the subscription.
        /// Then redirect to prevent multiple insertion when refreshing page.
        /// Include productNo to show the right thank you text
        /// </summary>
        private void Complete(CampaignUser campaignUser)
        {
            try
            {
                campaignUser.CreateExtraExpenseItem(); //if offercode ends with "P", then add expenseitem
                CurrentPage.UpdateCampaignLeads(); //update counter in db
                Response.Redirect(EPiServer.UriSupport.AddQueryString(EPiFunctions.GetFriendlyAbsoluteUrl(CurrentPage), "complete", campaignUser.OfferCode.ProductNo), false);
            }
            catch (Exception ex)
            {
                new Logger("Complete() - failed", ex.ToString());
                ShowError(string.Empty);
            }

        }

        /// <summary>
        /// Handle result after Auriga payment
        /// </summary>
        protected void HandleAurigaReturn()
        {
            try
            {
                AurigaReturn auriga = new AurigaReturn();
                string status = auriga.GetAurigaPaymentStatus();

                //Get campaignUser from session
                CampaignUser campaignUser = GetStoredUser(auriga.CustomerRefNo);

                switch (status)
                {
                    case "backButtonPushedOnPostensPage":
                        FillForm(campaignUser);
                        break;
                    case "MACmismatch":
                        ShowError(MiscFunctions.GetDefaultErrMess("variableln 'MAC' fick ett felaktigt värde vid verifiering."));
                        break;
                    case "E":
                    default:
                        auriga.SaveDataAfterPostensPage();
                        if (status == "E") //Error on auriga page
                        {
                            FillForm(campaignUser);
                            string errMsg = string.Empty;
                            if (MiscFunctions.IsNumeric(auriga.StatusCode))
                            {
                                string[] arrErr = MiscFunctions.GetBankMessArr(status);    //Get auriga error message
                                errMsg = arrErr[int.Parse(auriga.StatusCode)];
                            }
                            ShowError(MiscFunctions.GetDefaultErrMess(errMsg));
                        }
                        else
                        {
                            string errMsg = CirixDbHandler.TryInsertSubs(campaignUser, Settings.InvoiceMode_KontoKort);

                            if (!string.IsNullOrEmpty(errMsg))
                            {
                                StaffMail_PayedButNotSavedToCirix(campaignUser);

                                //Uncomment if you want silence even if customer already exists in Cirix

                                //if(errMsg.Equals(Translate("/campaigns/errors/alreadyincirixpayed")))
                                //{
                                //    MsSqlHandler.InsertCampaignUser(campaignUser, true);  
                                //    Complete(campaignUser);
                                //}
                                //else
                                ShowError(errMsg);  //user completed payment, but insert in Cirix failed
                            }
                            else
                            {
                                DIClassLib.Misc.MiscFunctions.TrySendPaymentReceiptEmail(CurrentPage.PageName,
                                                                                       auriga.Amount,
                                                                                       auriga.VAT,
                                                                                       auriga.TransactionID,
                                                                                       false,
                                                                                       campaignUser.Email);
                                Complete(campaignUser);
                            }
                        }

                        break;
                }
            }
            catch (Exception ex)
            {
                new Logger("HandleAurigaReturn() - failed", ex.ToString());
                ShowError(string.Empty);
            }
        }

        #region Helpers

        private bool IsUserStudent(string birthNo)
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            string studentKortetUrl = MiscFunctions.GetAppsettingsValue("StudentcheckUrl");

            if (!string.IsNullOrEmpty(studentKortetUrl))
            {
                //TODO: what if service is down?
                doc.Load(studentKortetUrl + "?persnr=" + birthNo + "&type=3");
                if (doc.GetElementsByTagName("status")[0].InnerText == "0")
                {
                    //ShowModalWindow("nostudent", "350", "auto");
                    //return;
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Fillform with values from CampaignUser.
        /// </summary>
        /// <param name="campaignUser"></param>
        private void FillForm(CampaignUser campaignUser)
        {
            CampaignForm.FillForm(campaignUser);
            RblOfferCodes.SelectedValue = campaignUser.OfferCode.OfferCodeId.ToString();
            PhPaymentOptions.Visible = true;
            RblPayMethod.SelectedValue = "card";
        }

        private void StaffMail_PayedButNotSavedToCirix(CampaignUser campaignUser)
        {
            string body = "Meddelande skickat från www.dagensindustri.se<br><br>" +
                          "Följande person har betalat för prenumeration med kort, men kunde inte " +
                          "sparas i Cirix pga att kunduppgifterna redan finns där.<br>" +
                          "Kontakta personen omgående<br><br>" +
                          campaignUser.ToString();

            MiscFunctions.SendMail(MiscFunctions.GetAppsettingsValue("mailPrenDiSe"),
                                                 MiscFunctions.GetAppsettingsValue("mailPrenDiSe"),
                                                 "Kontakta kund - har betalat med kort, men ej sparats i Cirix",
                                                 body,
                                                 true);
        }

        /// <summary>
        /// Get selected offercode
        /// </summary>
        /// <returns>The selected offercode from viewstate</returns>
        private OfferCode GetSelectedOfferCode()
        {
            if (RblOfferCodes.SelectedValue != null)
                return OfferCodeList.Find(delegate(OfferCode oc) { return oc.OfferCodeId.ToString() == RblOfferCodes.SelectedValue; });

            return null;
        }

        /// <summary>
        /// Get AurigaPerpare object
        /// </summary>
        /// <param name="offerCode">The offercode instance</param>
        /// <param name="campaignUser">Tha campaignUser instance</param>
        /// <returns></returns>
        private AurigaPrepare GetAurigaPerpareObj(OfferCode offerCode, CampaignUser campaignUser)
        {
            return new AurigaPrepare(offerCode.TotalPriceIncVAT * 100,
                                    (offerCode.TotalPriceIncVAT - offerCode.TotalPrice) * 100,
                                    EPiFunctions.GetFriendlyAbsoluteUrl(CurrentPage),
                                    EPiFunctions.GetFriendlyAbsoluteUrl(CurrentPage),
                                    CurrentPage.PageLink.ID + "_" + offerCode.CampId + "_" + campaignUser.TargetGroup,
                                    CurrentPage.PageName,
                                    campaignUser.FirstName + " " + campaignUser.LastName,
                                    campaignUser.Email);
        }

        /// <summary>
        /// All offercodes for this campaign, saved in viewstate
        /// </summary>
        protected List<OfferCode> OfferCodeList
        {
            get
            {
                return (List<OfferCode>)ViewState["OfferCodeList"];
            }
            set
            {
                ViewState["OfferCodeList"] = value;
            }
        }

        /// <summary>
        /// Saves CampaignUser object in sessions and to file
        /// </summary>
        /// <param name="campaignUser">The object to be saved</param>
        /// <param name="id">Id of the file</param>
        private void StoreUser(CampaignUser campaignUser, int id)
        {
            //Save in session
            HttpContext.Current.Session["campaignUser"] = campaignUser;

            //Serialize object to file, if session dies
            Serializer serializer = new Serializer();
            serializer.SaveObjectToFile(id, campaignUser);
        }

        /// <summary>
        /// Get the CampaignUser object from session or file 
        /// </summary>
        /// <param name="id">Id of the file</param>
        /// <returns>A CampaignUser object</returns>
        private CampaignUser GetStoredUser(int id)
        {
            if (HttpContext.Current.Session["campaignUser"] != null)
            {
                return (CampaignUser)HttpContext.Current.Session["campaignUser"];
            }
            else
            {
                Serializer serializer = new Serializer();
                return (CampaignUser)serializer.GetObjectFromFile(id);
            }
        }

        private void HideForms()
        {
            //Hide forms, button and offers
            HandleFormVisibility(false, false, false);
            //PhInviteArea.Visible = false;
            BtnSubmit.Visible = false;
        }

        private void HandleFormVisibility(bool campFormOtherPayer, bool campForm, bool offerCodeArea)
        {
            CampaignFormOtherPayer.Visible = campFormOtherPayer;
            CampaignForm.Visible = campForm;
            PhOfferCodeArea.Visible = offerCodeArea;
        }

        private void ShowError(string errorMsg)
        {
            LblError.Text = !string.IsNullOrEmpty(errorMsg) ? errorMsg : MiscFunctions.GetDefaultErrMess(string.Empty); ;
            ShowModalWindow("error", "300", "auto");
        }

        private void ShowModalWindow(string id, string width, string height)
        {
            Page.ClientScript.RegisterClientScriptBlock(
                GetType(),
                id,
                "<script type='text/javascript'>$(function() {showmodal('#" + id + "','" + width + "px', '" + height + "', true);});</script>");
        }

        #endregion
    }
}