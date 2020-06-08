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
using System.Collections.Generic;
using DIClassLib.EPiJobs.Apsis;
using System.Text;
using DagensIndustri.Tools.Classes;
using DIClassLib.DbHandlers;
using DIClassLib.DbHelpers;
using DIClassLib.Misc;


namespace DagensIndustri.Tools.Admin.Apsis
{
    public partial class BouncesListView : System.Web.UI.UserControl
    {
        MailSenderDbHandler _db = new MailSenderDbHandler();
        string _errMess = "Ett tekniskt fel uppstod. Var god försök igen.";


        protected void Page_Load(object sender, EventArgs e)
        {
            ListView1.ItemDataBound += new EventHandler<ListViewItemEventArgs>(ListView1_ItemDataBound);

            LabelMess.Text = "";

            if (!IsPostBack)
                BindList();
        }

        private void BindList()
        {
            DataSet ds = _db.GetBouncedCustsDS();

            if (DbHelpMethods.DataSetHasRows(ds))
            {
                ListView1.DataSource = ds;
                ListView1.DataBind();
            }
        }


        protected void PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {
            this.DataPager1.SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
            BindList();
        }

        protected void ListView_Update_Command(Object sender, ListViewUpdateEventArgs e)
        {
            ListView lv = (ListView)sender;
            Button b = (Button)lv.Items[e.ItemIndex].FindControl("UpdateButton");
            int custId = int.Parse(b.CommandArgument.ToString());
            CheckBox cbRetry = (CheckBox)lv.Items[e.ItemIndex].FindControl("CheckBoxForceRetry");
            CheckBox cbRegLett = (CheckBox)lv.Items[e.ItemIndex].FindControl("CheckBoxRegularLetter");
            TextBox tbEmail = (TextBox)lv.Items[e.ItemIndex].FindControl("TextBoxEmail");
            HiddenField hfEmail = (HiddenField)lv.Items[e.ItemIndex].FindControl("HiddenFieldEmail");
            TextBox tbStatus = (TextBox)lv.Items[e.ItemIndex].FindControl("TextBoxContactStatus");
            HiddenField hfStatus = (HiddenField)lv.Items[e.ItemIndex].FindControl("HiddenFieldContactStatus");

            string emailNew = tbEmail.Text;
            string emailOld = hfEmail.Value;
            bool emailChanged = false;
            if (emailNew != emailOld)
                emailChanged = true;

            bool newTryChecked = cbRetry.Checked;
            bool regLettChecked = cbRegLett.Checked;

            bool statusChanged = false;
            if (tbStatus.Text != hfStatus.Value)
                statusChanged = true;

            string err = ValidateForm(emailNew, emailChanged, newTryChecked, regLettChecked);

            if (err.Length > 0)
            {
                LabelMess.Text = err;
                return;
            }

            if (statusChanged)
                _db.UpdateContactStatus(custId, tbStatus.Text);

            if (regLettChecked)
            {
                DoSetDateRegularLetter(custId, emailNew);
                return;
            }

            if (emailChanged)
            {
                DoUpdateEmail(custId, emailNew);
                return;
            }

            if (newTryChecked)
            {
                DoSetForceRetry(custId);
                return;
            }


            if (statusChanged)
                LabelMess.Text = "Kontaktstatus sparad";
            else
                LabelMess.Text = "Ingen åtgärd vald";

            BindList();
        }

        protected void ListView_Delete_Command(Object sender, ListViewDeleteEventArgs e)
        {
            ListView lv = (ListView)sender;
            Button b = (Button)lv.Items[e.ItemIndex].FindControl("UpdateButton");
            int custId = int.Parse(b.CommandArgument.ToString());

            _db.DeleteCustomer(custId, "Y");

            //_db.DeleteCustomer(custId);
            //ApsisCustomer c = new ApsisCustomer();
            //c.CustomerId = custId;
            //List<ApsisCustomer> custs = new List<ApsisCustomer>();
            //custs.Add(c);
            //string tmp = "";
            //try { CirixDbHandler.CirixFlagCusts(custs, "Y"); }
            //catch { tmp = "<br>Fel: kunden kunde inte flaggas om till 'Y' i CIRIX.EXPCUSTOMER_LETTER."; }

            BindList();
            LabelMess.Text = "Kunden har raderats.<br>" +
                             "Får därmed varken välkomstmail eller välkomstbrev.";  //+tmp;
        }


        void ListView1_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            Button b = (Button)e.Item.FindControl("UpdateButton");
            int cusNo = int.Parse(b.CommandArgument.ToString());

            #region set phone numbers
            try
            {
                DataSet ds = SubscriptionController.GetCustomer(cusNo);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    Literal litPhHome = (Literal)e.Item.FindControl("LiteralPhoneHome");
                    litPhHome.Text = ds.Tables[0].Rows[0].ItemArray[12].ToString();

                    Literal litPhWork = (Literal)e.Item.FindControl("LiteralPhoneWork");
                    litPhWork.Text = ds.Tables[0].Rows[0].ItemArray[13].ToString();

                    Literal litPhOther = (Literal)e.Item.FindControl("LiteralPhoneOther");
                    litPhOther.Text = ds.Tables[0].Rows[0].ItemArray[14].ToString();
                }
            }
            catch (Exception ex)
            {
                new Logger("Item_Bound() set phone numbers failed for cusNo:" + cusNo.ToString(), ex.ToString());
            }
            #endregion

            #region set bounce details
            try
            {
                Panel panBounceDetails = (Panel)e.Item.FindControl("PanelBounceDetails");
                panBounceDetails.Visible = false;

                DataSet ds = _db.GetLatestBounceForCust(int.Parse(cusNo.ToString()));

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    panBounceDetails.Visible = true;

                    Literal litDateSaved = (Literal)e.Item.FindControl("LiteralDateSaved");
                    litDateSaved.Text = ds.Tables[0].Rows[0]["dateSaved"].ToString();

                    Literal litDateBounce = (Literal)e.Item.FindControl("LiteralDateBounce");
                    litDateBounce.Text = ds.Tables[0].Rows[0]["dateBounce"].ToString();

                    Literal litEmailRec = (Literal)e.Item.FindControl("LiteralEmailRec");
                    litEmailRec.Text = ds.Tables[0].Rows[0]["email"].ToString();

                    Literal litBounceCat = (Literal)e.Item.FindControl("LiteralBounceCat");
                    litBounceCat.Text = ds.Tables[0].Rows[0]["bounceCategory"].ToString();

                    Literal litBounceReason = (Literal)e.Item.FindControl("LiteralBounceReason");
                    litBounceReason.Text = ds.Tables[0].Rows[0]["bounceReason"].ToString();

                    Literal litApsisMailId = (Literal)e.Item.FindControl("LiteralApsisMailId");
                    litApsisMailId.Text = ds.Tables[0].Rows[0]["apsisMailId"].ToString();

                    Literal litApsisIdentifier = (Literal)e.Item.FindControl("LiteralApsisIdentifier");
                    litApsisIdentifier.Text = ds.Tables[0].Rows[0]["identifier"].ToString();
                }
            }
            catch (Exception ex)
            {
                new Logger("Item_Bound() set bounce details failed for cusNo:" + cusNo.ToString(), ex.ToString());
            }
            #endregion
        }

        public string GetDate(Object o, string colName)
        {
            string ret = "";
            DataRowView drv = (DataRowView)o;
            if (drv.Row.Table.Columns.Contains(colName))
            {
                int index = drv.Row.Table.Columns.IndexOf(colName);
                string s = drv.Row.ItemArray.GetValue(index).ToString();
                if (s.Length > 0 && s.Length >= 10)
                    ret = s.Substring(0, 10);
            }

            return ret;
        }


        private string ValidateForm(string emailNew, bool emailChanged, bool newTryChecked, bool regLettChecked)
        {
            StringBuilder sb = new StringBuilder();

            if (emailChanged && emailNew.Length > 0 && !MiscFunctions.IsValidEmail(emailNew))
                sb.Append("Angiven e-postadress är inte korrekt formaterad.<br><br>");

            if (newTryChecked && regLettChecked)
            {
                sb.Append("'Gör nytt försök' OCH 'Skicka vanligt brev'<br>");
                sb.Append("kan inte vara valda samtidigt.<br><br>");
            }

            return sb.ToString();
        }

        private void DoSetDateRegularLetter(int custId, string email)
        {
            try
            {
                ApsisSharedMethods sm = new ApsisSharedMethods();
                sm.SendRegularLetter(custId, email, false);
                LabelMess.Text = "Ett vanligt brev kommer att skickas till kunden.<br>" +
                                 "Inga nya e-postutskick kommer därmed att göras.";
            }
            catch
            {
                LabelMess.Text = _errMess;
            }

            BindList();
        }

        private void DoUpdateEmail(int custId, string emailNew)
        {
            try
            {
                int ret = _db.UpdateEmailAndSetForceRetry(custId, emailNew);

                if (ret == 1)
                    LabelMess.Text = "E-postadressen har uppdaterats i Cirix customertabell<br>" +
                                     "och i detta system. Ett nytt utskicksförsök kommer<br>" +
                                     "att göras till kunden.";

                if (ret == 2)
                    LabelMess.Text = "E-postadressen har uppdaterats i Cirix customertabell<br>" +
                                     "och i detta system. Dock kommer inget nytt utskicksförsök<br>" +
                                     "att göras till kunden eftersom e-postadressen inte passerade<br>" +
                                     "gällande regler (se 'Administrera regler').";
            }
            catch
            {
                LabelMess.Text = _errMess;
            }

            BindList();
        }

        private void DoSetForceRetry(int custId)
        {
            try
            {
                _db.SetForceRetry(custId, true, false);
                LabelMess.Text = "Ett nytt utskicksförsök kommer att göras till<br>" +
                                 "kunden nästa gång det schemalagda jobbet körs.";
            }
            catch
            {
                LabelMess.Text = _errMess;
            }

            BindList();
        }

    }
}