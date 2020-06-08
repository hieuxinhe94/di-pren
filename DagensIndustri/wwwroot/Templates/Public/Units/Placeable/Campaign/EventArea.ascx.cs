using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;
using DIClassLib.CardPayment;
using System.Collections.Generic;


namespace DagensIndustri.Templates.Public.Units.Placeable.Campaign
{
    public partial class EventArea : EPiServer.UserControlBase
    {
        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (!IsPostBack)
            {
                FillOptionLists();

                //MAC will be returned from Auriga after payment
                if (Request.QueryString["MAC"] != null)
                    HandleAurigaReturn();

            }
        }

        /// <summary>
        /// Fill optionlists with items
        /// </summary>
        private void FillOptionLists()
        {
            SqlDataReader DR = null;

            try
            {
                DR = DIClassLib.DbHelpers.SqlHelper.ExecuteReader("Campaign", "GetAllEvents", new SqlParameter("@category", CurrentPage["EventCategory"]));
                bool okToAddOneMorePerson = false;

                while (DR.Read())
                {
                    string dateText = "<strong>" + DateTime.Parse(DR["dateStart"].ToString()).ToString("dddd d MMMM", new System.Globalization.CultureInfo("sv-SE")) + "</strong>";
                    string timeText = ", kl. " + DateTime.Parse(DR["dateStart"].ToString()).ToString("HH.mm") + "-" + DateTime.Parse(DR["dateEnd"].ToString()).ToString("HH.mm") + ". ";

                    //If all tickets are gone, disable option
                    okToAddOneMorePerson = int.Parse(DR["nrOfParticipants"].ToString()) < int.Parse(DR["maxNumParticipants"].ToString());

                    //Add item to event option list
                    RblOptions.Items.Add(new ListItem(dateText + timeText + DR["eventText"].ToString(), DR["eventId"].ToString(), okToAddOneMorePerson));
                }

                //only one event - select and hide it
                if (RblOptions.Items.Count == 1)
                {
                    RblOptions.Items[0].Selected = true;
                    RblOptions.Visible = false;

                    if (!okToAddOneMorePerson)
                    {
                        LabelEventFull.Visible = true;
                        PanelEventForm.Visible = false;
                    }
                }

                //Add items to subscriber option list
                RblSubscriber.Items.Add(new ListItem("Jag är prenumerant (anmälningsavgift " + CurrentPage["PriceSubscriber"] + " kr)", CurrentPage["PriceSubscriber"].ToString()));
                RblSubscriber.Items.Add(new ListItem("Jag är inte prenumerant (anmälningsavgift " + CurrentPage["PriceNotSubscriber"] + " kr)", CurrentPage["PriceNotSubscriber"].ToString()));

                //select first and hide
                RblSubscriber.Items[0].Selected = true;
                RblSubscriber.Visible = false;

            }
            catch (Exception ex)
            {
                new DIClassLib.DbHelpers.Logger("FillOptionLists() - failed", ex.ToString());
                LblError.Text = Translate("/various/errors/error");
            }
            finally
            {
                if (DR != null)
                    DR.Close();
            }
        }

        #region Before payment

        protected void BtnSubmitOnClick(object sender, EventArgs e)
        {
            try
            {
                int amount = int.Parse(RblSubscriber.SelectedValue) * 100;

                //Instanciate auriga class                
                AurigaPrepare auriga = new AurigaPrepare(
                        amount,                                                           //amount
                        amount * int.Parse(CurrentPage["PriceVAT"].ToString()) / 100,     //vat
                        DagensIndustri.Tools.Classes.EPiFunctions.GetFriendlyAbsoluteUrl(CurrentPage),  //responseurl
                        DagensIndustri.Tools.Classes.EPiFunctions.GetFriendlyAbsoluteUrl(CurrentPage),  //cancelurl
                        "EventID: " + RblOptions.SelectedValue,                           //Goods description  
                        CurrentPage.PageName,                                             //comment
                        InputFirstName.Value + " " + InputLastName.Value,                 //consumer name
                        InputEmail.Value                                                  //email address
                    );

                //Save data to Paytrans
                auriga.SaveDataBeforePostensPage();

                //Insert person to campaign db
                InsertPersonInEvent(auriga.CustomerRefNo);

                //Redirect to Auriga
                Response.Redirect(auriga.GetAurigaUrl(true), false);
            }
            catch (Exception ex)
            {
                new DIClassLib.DbHelpers.Logger("BtnSubmitOnClick() - failed", ex.ToString());
                LblError.Text = Translate("/various/errors/error");
            }
        }

        /// <summary>
        /// Inster person to Event db.
        /// </summary>
        /// <param name="CustomerRefNo">Id of row i Paytrans</param>
        public void InsertPersonInEvent(int CustomerRefNo)
        {
            try
            {
                //Insert person to Campaign db
                SqlParameter[] sqlParameters = new SqlParameter[] {
                    new SqlParameter("@firstName", InputFirstName.Value),
                    new SqlParameter("@lastName", InputLastName.Value),
                    new SqlParameter("@company", InputCompany.Value),
                    new SqlParameter("@address", InputAddress.Value),
                    new SqlParameter("@zip", InputZip.Value),
                    new SqlParameter("@city", InputCity.Value),
                    new SqlParameter("@email", InputEmail.Value),
                    new SqlParameter("@phone", InputPhone.Value),
                    new SqlParameter("@isSubscriber", RblSubscriber.SelectedIndex == 0 ? "1" : "0"),
                    new SqlParameter("@dateSaved", DateTime.Now),
                    new SqlParameter("@eventId", RblOptions.SelectedValue),
                    new SqlParameter("@refNo", CustomerRefNo)
                };

                DIClassLib.DbHelpers.SqlHelper.ExecuteNonQuery("Campaign", "InsertPersonInEvent", sqlParameters);
            }
            catch (Exception ex)
            {
                new DIClassLib.DbHelpers.Logger("InsertPersonInEvent() - failed", ex.ToString());
                throw ex;
            }
        }

        #endregion

        #region After payment

        /// <summary>
        /// Handle result after Auriga payment
        /// </summary>
        protected void HandleAurigaReturn()
        {
            try
            {
                PhForm.Visible = false;

                AurigaReturn auriga = new AurigaReturn();
                string status = auriga.GetAurigaPaymentStatus();

                switch (status)
                {
                    case "backButtonPushedOnPostensPage":
                        //do nothing for now
                        break;
                    case "MACmismatch":
                        LblError.Text = "MAC stämde inte";
                        break;
                    case "E":
                    default:
                        auriga.SaveDataAfterPostensPage();
                        UpdatePersonInEventStatus(auriga.CustomerRefNo, status);
                        if (status == "E")
                            LblError.Text = Translate("/various/errors/error");
                        else
                            LblMessage.Text = CurrentPage["ThankText"].ToString();
                        break;
                }
            }
            catch (Exception ex)
            {
                new DIClassLib.DbHelpers.Logger("HandleAurigaReturn() - failed", ex.ToString());
                LblError.Text = Translate("/various/errors/error");
            }
        }

        /// <summary>
        /// Update status in table PersonInEvent
        /// </summary>
        /// <param name="refNo">Id of row i Paytrans</param>
        /// <param name="status">Status on payment</param>
        public void UpdatePersonInEventStatus(int refNo, string status)
        {
            try
            {
                //Insert PersonInEvent status in Campaign db
                SqlParameter[] sqlParameters = new SqlParameter[] {
                    new SqlParameter("@refNo", refNo),
                    new SqlParameter("@status", status)
                };

                DIClassLib.DbHelpers.SqlHelper.ExecuteNonQuery("Campaign", "UpdatePersonInEventStatus", sqlParameters);
            }
            catch (Exception ex)
            {
                new DIClassLib.DbHelpers.Logger("InsertPersonInEvent() - failed", ex.ToString());
                throw ex;
            }
        }

        #endregion

    }
}