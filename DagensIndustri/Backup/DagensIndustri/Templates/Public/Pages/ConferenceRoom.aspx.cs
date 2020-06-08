using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DIClassLib.Misc;
using System.Text;
using DagensIndustri.Tools.Classes.BaseClasses;
using DIClassLib.DbHandlers;
using System.Data;
using System.Data.SqlClient;
using DIClassLib.DbHelpers;
using DIClassLib.Membership;
using DagensIndustri.Tools.Classes;
using DagensIndustri.Templates.Public.Units.Placeable.DiGoldMembershipPopup;


namespace DagensIndustri.Templates.Public.Pages
{
    public partial class ConferenceRoom : DiTemplatePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            //Register javascript for gmaps
            //this.Page.ClientScript.RegisterClientScriptInclude("javascript", " http://maps.google.com/maps?file=api&amp;v=2&amp;key=ABQIAAAArWnBSdNA2jqPuJO1ESOq-BT63K5vxdAm05G6TXbGMG4SHam5HRT7pxZbtWxf7SOCx9KbDDYEM6XBcQ");
            this.Page.ClientScript.RegisterClientScriptInclude("javascript", "/Templates/Public/js/jquery.gmap-1.1.0-min.js");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!MembershipFunctions.UserAllowedToSeePage(DiRoleHandler.RoleDiGold))
            {
                //UserMessage1.ShowMessage("/common/message/onlypren", true, true);
                //PlaceHolderForm.Visible = false;
                PlaceHolderBookingForm.Visible = false;
                ShowLoginButton();
                return;
            }


            if (!IsPostBack)
                HandleLoungeOffer();

            PlaceHolderThankYou.Visible = false;
            DataBind();
        }

        private void ShowLoginButton()
        {
            ButtonLogin.Visible = true;

            DiGoldMembershipPopup diGoldMembershipPopup = EPiFunctions.FindDiGoldMembershipPopup(Page) as DiGoldMembershipPopup;
            if (diGoldMembershipPopup != null)
            {
                
                string absoluteFriendlyURL = EPiFunctions.GetFriendlyAbsoluteUrl(CurrentPage);
                //diGoldMembershipPopup.RegisterSetReturnURLScript(ButtonLogin, absoluteFriendlyURL);
            }

        }


        private void HandleLoungeOffer()
        {
            ConfRoomRow cr = new ConfRoomRow(HttpContext.Current.User.Identity.Name);

            if (cr.DateCreated > DateTime.MinValue && cr.DateUsed == DateTime.MinValue)
            {
                TextBoxLoungeCode.Text = cr.Code.ToString().Substring(0, 8).ToUpper();
                DisplayLounge(true);
            }
            else
            {
                DisplayLounge(false);
            }
        }

        private void DisplayLounge(bool bo)
        {
            PlaceHolderLoungeTab.Visible = bo;
            PlaceHolderLoungeInfo.Visible = bo;
        }


        protected void Send_Click(object sender, EventArgs e)
        {
            String sDateTime = date.Text + " " + time.Text;
            DateTime dtTargetDate = DateTime.MinValue;
            if (!DateTime.TryParse(sDateTime, out dtTargetDate))
            {
                UserMessage1.ShowMessage("Ange korrekt datum och tid", true, true);
                return;
            }

            // check so that the booking is not within 24 hours
            if (DateTime.Now.AddHours(24) > dtTargetDate)
            {
                UserMessage1.ShowMessage("Bokningen måste ske minst 24 timmar innan.", true, true);
                return;
            }
            


            SendEmail();
            ClearForm();
            UserMessageControl1.ShowMessage("/confroom/mailsent", true, false);
            PlaceHolderThankYou.Visible = true;
        }

        private void SendEmail()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Förnamn: " + firstName.Text.ToString() + "<br>");
            sb.Append("Efternamn: " + lastName.Text.ToString() + "<br>");
            sb.Append("E-post: " + email.Text.ToString() + "<br>");
            sb.Append("Mobil: " + phone.Text.ToString() + "<br>");
            sb.Append("Önskar lokal: " + date.Text.ToString() + " " + time.Text.ToString() + "<br>");
            sb.Append("Ant timmar: " + numHours.Text.ToString() + "<br>");
            sb.Append("Ant personer: " + numPersons.Text.ToString() + "<br>");
            sb.Append("<br>");
            sb.Append("Skickat från dagensindustri.se  " + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "<br>");

            if (CurrentPage["MailSubject"] != null && CurrentPage["MailAddressDi"] != null && CurrentPage["MailAddressPartner"] != null)
            {
                string subj = CurrentPage["MailSubject"].ToString();
                string from = CurrentPage["MailAddressDi"].ToString();
                string to = CurrentPage["MailAddressPartner"].ToString();
                
                DIClassLib.Misc.MiscFunctions.SendMail(from, to, subj, sb.ToString(), true);
            }
        }

        private void ClearForm()
        {
            firstName.Text = "";
            lastName.Text = "";
            email.Text = "";
            phone.Text = "";
            date.Text = "";
            time.Text = "";
            numHours.Text = "";
            numPersons.Text = "";
        }


        public string GetMinDate()
        {
            return DateTime.Now.AddHours(24).ToString("yyyy-MM-dd");
        }

        public string GetMaxDate()
        {
            return DateTime.Now.AddHours(72).ToString("yyyy-MM-dd");
        }

    }


    public class ConfRoomRow
    {
        string _connStr = "DagensIndustriMISC";

        public string Userid;
        public int Cusno;
        public Guid Code;
        public DateTime DateCreated = DateTime.MinValue;
        public DateTime DateUsed = DateTime.MinValue;

        public ConfRoomRow(string userid)
        {
            Userid = userid;
            Cusno = MembershipDbHandler.GetCusno(userid);

            if (Cusno > -1)
            {
                TryGetConfRoomRow();
                if (DateCreated == DateTime.MinValue)   //no row in db
                    InsertConfRoomRow();                //add row
            }
        }

        private void TryGetConfRoomRow()
        {
            try
            {
                DataSet ds = null;
                ds = SqlHelper.ExecuteDataset(_connStr, "GetConfRoomRow", new SqlParameter("@cusno", Cusno));

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Code = new Guid(dr["code"].ToString());

                    DateTime dt1;
                    DateCreated = (DateTime.TryParse(dr["dateCreated"].ToString(), out dt1) ? dt1 : DateTime.MinValue);

                    DateTime dt2;
                    DateUsed = (DateTime.TryParse(dr["dateUsed"].ToString(), out dt2) ? dt2 : DateTime.MinValue);
                }
            }
            catch (Exception ex)
            {
                new Logger("TryGetConfRoomRow() - failed", ex.ToString());
            }
        }

        private void InsertConfRoomRow()
        {
            try
            {
                Code = Guid.NewGuid();
                DateCreated = DateTime.Now;

                SqlParameter[] sqlParameters = new SqlParameter[] {
                                            new SqlParameter("@cusno", Cusno),
                                            new SqlParameter("@code", Code) };

                SqlHelper.ExecuteNonQuery(_connStr, "InsertConfRoomRow", sqlParameters);
            }
            catch (Exception ex)
            {
                new Logger("InsertConfRoomRow() - failed", ex.ToString());
            }
        }
    }
}