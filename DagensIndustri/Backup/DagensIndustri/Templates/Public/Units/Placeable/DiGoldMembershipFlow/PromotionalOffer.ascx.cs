using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;
using DagensIndustri.Tools.Classes;
using DagensIndustri.Tools.Classes.BaseClasses;
using DIClassLib.DbHandlers;
using DIClassLib.DbHelpers;
using DagensIndustri.Templates.Public.Pages;
using System.Data;

namespace DagensIndustri.Templates.Public.Units.Placeable.DiGoldMembershipFlow
{
    public partial class PromotionalOffer : UserControlBase
    {
        #region Properties
        /// <summary>
        /// Page heading
        /// </summary>
        public string Heading
        {
            get
            {
                string heading = (string)EPiFunctions.GetPagePropertyValueBySettingsPage(CurrentPage, "DiGoldPromotionalOfferPage", "Heading");
                return !string.IsNullOrEmpty(heading) ? heading : (string)CurrentPage["Heading"];
            }
        }

        /// <summary>
        /// Name of the promotional offer
        /// </summary>
        public string PromotionalOfferName
        {
            get
            {
                string promotionalOfferName = (string)EPiFunctions.GetPagePropertyValueBySettingsPage(CurrentPage, "DiGoldPromotionalOfferPage", "PromotionalOfferName");
                return !string.IsNullOrEmpty(promotionalOfferName) ? promotionalOfferName : (string)CurrentPage["PromotionalOfferName"];
            }
        }

        /// <summary>
        /// Current subscriber user's customer number
        /// </summary>
        public long CustomerNumber
        {
            get
            {
                return (long)ViewState["CustomerNumber"];
            }
            set
            {
                ViewState["CustomerNumber"] = value;
            }
        }
        #endregion

        #region Events
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Check if data is valid
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            return Page.IsValid;
        }

        /// <summary>
        /// Save data to db
        /// </summary>
        /// <returns></returns>
        public bool SaveData()
        {
            bool saved = false;

            string logMessage = string.Format("CustomerNumber: {0}, PromotionalOfferName: {1}, StreetAddress: {2}, HouseNo: {3}, ZipCode: {4}, City: {5}, CareOf: {6}, UserName: {7}, Company: {8}, Name: {9} \n ",
                                               CustomerNumber, PromotionalOfferName, StreetAddressInput.Text.Trim(), HouseNoInput.Text.Trim(), ZipCodeInput.Text.Trim(), CityInput.Text.Trim(), CareOfInput.Text.Trim(), HttpContext.Current.User.Identity.Name, CompanyInput.Text.Trim(), NameInput.Text.Trim());

            try
            {
                if (string.IsNullOrEmpty(NameInput.Text.Trim()) || string.IsNullOrEmpty(StreetAddressInput.Text.Trim()) || 
                    string.IsNullOrEmpty(ZipCodeInput.Text.Trim()) || string.IsNullOrEmpty(CityInput.Text.Trim()))
                {
                    ((DiTemplatePage)Page).ShowMessage("/promotionaloffer/validation/missingdata", true, true);
                    saved = false;
                }
                else if (!HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    ((DiTemplatePage)Page).ShowMessage(string.Format(Translate("/common/message/loginforpromotionaloffer"), EPiFunctions.GetLoginPageUrl(CurrentPage), PromotionalOfferName.ToLower()), false, true);
                    saved = false;
                }
                else if (!CanGetPromotionalOffer())
                {
                    ((DiTemplatePage)Page).ShowMessage(string.Format(Translate("/common/message/tempreceiveoffer"), PromotionalOfferName.ToLower()), false, false);
                    saved = false;

                    new Logger("SaveData() - logging to detect if this is double post problem", logMessage);
                }
                else
                {
                    MsSqlHandler.InsertDiGoldOffer(CustomerNumber, CurrentPage.PageLink.ID, PromotionalOfferName, CompanyInput.Text.Trim(), NameInput.Text.Trim(), StreetAddressInput.Text.Trim(), HouseNoInput.Text.Trim(), ZipCodeInput.Text.Trim(), CityInput.Text.Trim(), CareOfInput.Text.Trim());
                    saved = true;
                }
            }
            catch (Exception ex)
            {
                new Logger("SaveData() - failed", logMessage + ex.ToString());
                ((DiTemplatePage)Page).ShowMessage("/common/errors/error", true, true);
            }
            return saved;
        }

        /// <summary>
        /// Get all the input values
        /// </summary>
        /// <returns></returns>
        public OfferRecieverDetails GetDetails()
        {
            OfferRecieverDetails details = new OfferRecieverDetails()
            {
                StreetAddress = StreetAddressInput.Text.Trim(),
                HouseNo = HouseNoInput.Text.Trim(),
                ZipCode = ZipCodeInput.Text.Trim(),
                City = CityInput.Text.Trim(),
                CareOf = CareOfInput.Text.Trim()
            };

            return details;
        }

        /// <summary>
        /// Fill the input controls with proper values
        /// </summary>
        ///<param name="details"
        public void FillControl(OfferRecieverDetails details)
        {
            StreetAddressInput.Text = details.StreetAddress;
            HouseNoInput.Text = details.HouseNo;
            ZipCodeInput.Text = details.ZipCode;
            CityInput.Text = details.City;
            CareOfInput.Text = details.CareOf;
        }

        /// <summary>
        /// If user is authenticated and has never received the promotional offer, then he/she can order it.
        /// </summary>
        /// <returns></returns>
        public bool CanGetPromotionalOffer()
        {
            bool canGetOffer = false;

            if (HttpContext.Current.User.Identity.IsAuthenticated && !string.IsNullOrEmpty(PromotionalOfferName))
            {
                int customerNumber = MembershipDbHandler.GetCusno(HttpContext.Current.User.Identity.Name);
                if (customerNumber > 0)
                {
                    DataSet diGoldOfferDs = MsSqlHandler.GetDiGoldOfferByCusno(customerNumber, PromotionalOfferName);

                    //If no rows where returned from the db, then user has not used the offer yet.
                    if (diGoldOfferDs == null || diGoldOfferDs.Tables[0].Rows.Count == 0)
                    {
                        CustomerNumber = customerNumber;
                        canGetOffer = true;
                    }
                    else
                    {
                        CustomerNumber = -1;
                    }
                }
            }

            return canGetOffer;
        }

        #endregion
    }
}