using System;
using System.Globalization;
using System.Text;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Web.UI.WebControls;
using EPiServer.Core.Html;
using DIClassLib.DbHelpers;
using DagensIndustri.Tools.Classes;
using DIClassLib.Misc;


namespace DagensIndustri.Templates.Public.Pages.Gasellerna
{
    public partial class GasellConference : EPiServer.TemplatePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            //Register javascript for functions
            RegisterClientScriptFile("/Templates/Public/js/Functions.js");
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (!IsPostBack)
            {
                FillConferences();
                //if direct link to a conference (from email etc.)
                if (Request.QueryString["conferenceId"] != null)
                {
                    string conferenceId = Request.QueryString["conferenceId"].ToString();
                    ShowForm(conferenceId);
                }
            }
        }

        private void FillConferences()
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(null, "getGasellConferences", null);
                GasellConfRepeater.DataSource = ds.Tables[0].DefaultView;
                GasellConfRepeater.DataBind();
            }
            catch (Exception ex)
            {
                new Logger("FillConferences() - failed", ex.ToString());
                ShowMessage("/gasellerna/conference/messages/error", false);
            }
        }

        /// <summary>
        /// Handler for linkbutton in conference list
        /// </summary>
        protected void ShowForm(object sender, EventArgs e)
        {
            LinkButton lnkBtn = ((LinkButton)sender);
            if (lnkBtn != null && !string.IsNullOrEmpty(lnkBtn.CommandArgument))
            {
                ClearInputFields();
                ShowForm(lnkBtn.CommandArgument);
            }
        }

        private void ShowForm(string conferenceId)
        {

            SqlDataReader DR = null;
            try
            {
                //show sign up
                PhSignUp.Visible = true;
                //hide message
                PhMessage.Visible = false;
                //show or hide the free pren placeholder
                PhFreePren.Visible = CurrentPage["FreePrenHide"] == null;

                string strCity = string.Empty;

                DR = SqlHelper.ExecuteReader(null, "getGasellConfDetail", new SqlParameter("@confid", conferenceId));
                if (DR.Read())
                {
                    //check if closed
                    if (DR["closed"] != System.DBNull.Value && DR["closed"].ToString() == "1")
                    {
                        ShowMessage("/gasellerna/conference/messages/closed", false);
                    }
                    else
                    {
                        LblConfText.Text = DR["text"] + Translate("/gasellerna/conference/messages/registerheading") + DR["city"];
                        strCity = DR["city"] as string;
                    }
                }
                else
                {
                    //conferense does not exist
                    ShowMessage("/gasellerna/conference/messages/closed", false);
                }

                //pass commandargument to submit button. conferenceID and city
                BtnSubmit.CommandArgument = conferenceId + "|" + strCity;
            }
            catch (Exception ex)
            {
                new Logger("ShowForm(string conferenceId) - failed", ex.ToString());
                ShowMessage("/gasellerna/conference/messages/error", false);
            }
            finally
            {
                //SqlConnecion will be closed when SqlDataReader is closed
                if (DR != null)
                    DR.Close();
            }
        }

        /// <summary>
        /// Handler for Submit button
        /// </summary>
        protected void OrderConferences(object sender, EventArgs e)
        {
            Button button = ((Button)sender);

            if (button != null && !string.IsNullOrEmpty(button.CommandArgument) && Page.IsValid)
            {
                //split commandargument to get conferenceID and city, 0 = id, 1 = city
                string[] args = button.CommandArgument.Split('|');
                OrderConferences(args[0], args[1]);
            }
        }

        private void OrderConferences(string conferenceID, string city)
        {

            string code = InputCode.Value;
            string confCode = CurrentPage["ConferenceCode"] as string ?? string.Empty;

            //clear code if no match
            if (Array.IndexOf(confCode.ToLower().Split(';'), code.ToLower()) < 0)
                code = string.Empty;

            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[] {
                    new SqlParameter("@firstName", InputFirstName.Value),
                    new SqlParameter("@lastName", InputLastName.Value),
                    new SqlParameter("@title", InputTitle.Value),
                    new SqlParameter("@company", InputCompany.Value),
                    new SqlParameter("@coAddress", InputCoAddress.Value),
                    new SqlParameter("@address", InputAddress.Value),
                    new SqlParameter("@stairs", InputStairs.Value),
                    new SqlParameter("@zipcode", InputZipCode.Value),
                    new SqlParameter("@city", InputCity.Value),
                    new SqlParameter("@phone", InputPhone.Value),
                    new SqlParameter("@mail", InputEmail.Value),
                    new SqlParameter("@bransch", InputIndustry.Value),
                    new SqlParameter("@employees", InputEmployees.Value),
                    new SqlParameter("@gasellCompany", RblGasellCompany.SelectedValue),
                    new SqlParameter("@code", code),
                    new SqlParameter("@invoiceAddress", InputInvoiceAddress.Value),
                    new SqlParameter("@invoiceZipcode", InputInvoiceZipCode.Value),
                    new SqlParameter("@invoiceCity", InputInvoiceCity.Value),
                    new SqlParameter("@invoiceRef", InputInvoiceReference.Value),
                    new SqlParameter("@orgNo", InputCorpIdNumber.Value),
                    new SqlParameter("@gasellConfId", conferenceID)
                };

                SqlHelper.ExecuteNonQuery(null, "insertGasellConference", sqlParameters);

                ShowMessage("/gasellerna/conference/messages/thanks", false);
                SendConfirmationMail(InputEmail.Value, city);

                //clear form
                ClearInputFields();

                //if freepren, redirect
                if (RblSubscribe.SelectedValue == "yes" && CurrentPage["FreePrenUrl"] != null && CurrentPage["FreePrenHide"] == null)
                {
                    string RedirUrl = CurrentPage["FreePrenUrl"] as string;
                    //Response.Redirect(RedirUrl, false);
                    //ClientScript.RegisterClientScriptBlock(GetType(), "redir", "<script type='text/javascript'>top.location.href = '" + RedirUrl + "';</script>");
                    LblMailText.Text += "<br /><br /><strong><a href='" + RedirUrl + "' target='_blank'>Klicka här för att prova DI gratis under 3 veckor</a></strong><br /><br />";
                }

            }
            catch (Exception ex)
            {
                new Logger("OrderConferences(string conferenceID) - failed", ex.ToString());
                ShowMessage("/gasellerna/conference/messages/error", false);
            }
        }

        private void SendConfirmationMail(string mailTo, string city)
        {
            try
            {
                string mailFrom = CurrentPage["MailFrom"] as string;
                string subject = CurrentPage["MailSubject"] as string;
                MiscFunctions.SendMail(mailFrom, mailTo, subject, GetConfirmMessage().Replace("[city]", city), true);
                ShowMessage("/gasellerna/conference/messages/mail", mailTo, true);
            }
            catch (Exception ex)
            {
                new Logger("SendConfirmationMail(string mailTo) - failed", ex.ToString());
                ShowMessage("/gasellerna/conference/messages/mailerror", true);
            }
        }

        #region Help functions and voids

        private void ShowMessage(string translateKey, bool isEmailText)
        {
            ShowMessage(translateKey, string.Empty, isEmailText);
        }

        private void ShowMessage(string translateKey, string addText, bool isEmailText)
        {
            PhSignUp.Visible = false;
            PhMessage.Visible = true;

            string message = Translate(translateKey) + addText;

            if (isEmailText)
                LblMailText.Text = message;
            else
                LblMessage.Text = message;
        }

        private string GetConfirmMessage()
        {
            return "<HTML><head><style type=text/css>" +
                        "TD{font-weight: bold;font-size: 12px;color: #000000;font-family: Arial, Verdana, Sans-Serif;}" +
                        "</style></head>" + "<body bgcolor=\"#FEE1D2\">" +
                        CurrentPage["MailBody"] +
                        "</BODY></HTML>";
        }

        protected string getDate(DateTime ThisDate)
        {
            return ThisDate.ToString("dddd d MMMM", CultureInfo.CreateSpecificCulture("sv-SE"));
        }

        /// <summary>
        /// Clear input controls of type InputWithValidation in placeholder PhSignUp
        /// </summary>
        private void ClearInputFields()
        {
            foreach (object control in PhSignUp.Controls)
            {
                if (control.GetType().BaseType == typeof(DagensIndustri.Templates.Public.Units.Placeable.InputWithValidation))
                {
                    DagensIndustri.Templates.Public.Units.Placeable.InputWithValidation iwv = (DagensIndustri.Templates.Public.Units.Placeable.InputWithValidation)control;
                    iwv.Value = string.Empty;
                }
            }
        }

        #endregion
    }
}