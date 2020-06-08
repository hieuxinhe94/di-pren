using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;
using EPiServer.Core;
using DagensIndustri.Tools.Classes.BaseClasses;
using DIClassLib.Subscriptions;
using System.Text;
using DIClassLib.Misc;
using DIClassLib.DbHandlers;
using DIClassLib.CardPayment;
using DagensIndustri.Tools.Classes;
using System.Data;
using DIClassLib.DbHelpers;


namespace DagensIndustri.Templates.Public.Pages.DiCampaign
{
    //TODO: Obsolete, should be deleted as page template!
    public partial class CampaignMobile : DiTemplatePage
    {
        //pageid=123 in url
        public int CampPageId
        {
            get
            {
                int pageid = -1;

                if (Request.QueryString["pageid"] != null)
                    int.TryParse(Request.QueryString["pageid"].ToString(), out pageid);

                return pageid;
            }
        }
        
        
        //cn=123 in url
        //public long UrlCampno
        //{
        //    get
        //    {
        //        long cn = -1;

        //        if (Request.QueryString["cn"] != null)
        //            long.TryParse(Request.QueryString["cn"].ToString(), out cn);

        //        return cn;
        //    }
        //}
        
        //tg=xxx in url
        //public string UrlTargetGroup
        //{
        //    get
        //    {
        //        string tg = "";

        //        if (Request.QueryString["tg"] != null)
        //            tg = Request.QueryString["cn"].ToString();

        //        return tg;
        //    }
        //}


        public Subscription Sub 
        {
            get 
            {
                if (ViewState["sub"] != null)
                    return (Subscription)ViewState["sub"];

                return null;
            }
            set
            {
                ViewState["sub"] = value;
            }
        }

        public bool IsWeekend
        {
            get
            {
                if (Sub != null && Sub.ProductNo == Settings.ProductNo_Weekend && Sub.PaperCode == Settings.PaperCode_DI)
                    return true;

                return false;
            }
        }



        protected void Page_Load(object sender, EventArgs e)
        {
            PlaceHolderIncludedTable.DataBind();
            HandlePhoneLinkVisibility();

            if (!IsPostBack)
            {
                string stdErr = "Tekniskt fel: kampanj hittades ej.";

                //get pageid from url
                if (CampPageId < 1)
                {
                    new Logger("Page_Load() - CampPageId=" + CampPageId.ToString(), "Bad pageid in url");
                    ShowMess(stdErr, false, true);
                    return;
                }

                //try find page in epi
                PageData pd = GetPageData();
                if (pd == null)
                {
                    ShowMess(stdErr, false, true);
                    return;
                }

                //epi page must have prop CampId
                if (!EPiFunctions.HasValue(pd, "CampId"))
                {
                    new Logger("Page_Load() - EPiFunctions.HasValue('CampId') false for CampPageId=" + CampPageId.ToString(), "");
                    ShowMess(stdErr, false, true);
                    return;
                }

                //try populate Sub from cirix
                string campId = pd["CampId"].ToString();
                long campNo = SubscriptionController.GetCampno(campId);
                Sub = new Subscription(string.Empty, campNo, PaymentMethod.TypeOfPaymentMethod.Invoice, DateTime.Now, false);
                if (string.IsNullOrEmpty(Sub.CampId))
                {
                    new Logger("Page_Load() - could not populate Sub for campId=" + campId + ", campNo=" + campNo, "");
                    ShowMess(stdErr, false, true);
                    return;
                }

                if (string.IsNullOrEmpty(Sub.TargetGroup))
                    Sub.TargetGroup = "IDAGENSINM";

                LabelOffer.Text = Sub.SubsLength.ToString() + " " + GetTimeUnit() + " för " + GetPriceIncVat().ToString() + " kr (ink moms)";
            }
        }

        private void HandlePhoneLinkVisibility()
        {
            DateTime now = DateTime.Now;

            if (now.DayOfWeek == DayOfWeek.Saturday || now.DayOfWeek == DayOfWeek.Sunday)
            {
                PlaceHolderPhone.Visible = false;
                return;
            }

            if (now.Hour < 8 || now.Hour > 17)
            {
                PlaceHolderPhone.Visible = false;
                return;
            }

            PlaceHolderPhone.Visible = true;
        }

        private PageData GetPageData()
        {
            try
            {
                PageData pd = GetPage(new PageReference(CampPageId));
                return pd;
            }
            catch (Exception ex)
            {
                new Logger("GetPageData() - failed for CampPageId=" + CampPageId.ToString(), ex.ToString());
                return null;
            }
        }

        private object GetTimeUnit()
        {
            string lenUnit = Sub.LengthUnit;
            int subsLen = Sub.SubsLength;

            if (lenUnit == "YY")
                return "år";

            if (lenUnit == "MM")
            {
                if (subsLen > 1)
                    return "månader";
                else
                    return "månad";
            }

            if (lenUnit == "WW")
            {
                if (subsLen > 1)
                    return "veckor";
                else
                    return "vecka";
            }

            if (lenUnit == "DD")
            {
                if (subsLen > 1)
                    return "dagar";
                else
                    return "dag";
            }

            return "";
        }

        private double GetPriceIncVat()
        {
            PriceCalculator pc = new PriceCalculator(Sub.TotalPriceExVat, null, Settings.VatPaper, false);
            return (double)pc.PriceIncVat;
        }


        protected void ButtonSend_Click(object sender, EventArgs e)
        {
            return;
            string err = ValidateForm();
            if (!string.IsNullOrEmpty(err))
            {
                ShowMess(err, true, true);
                return;
            }

            Person p = new Person(true, true, TextBoxName.Text.Trim(), TextBoxAddress.Text.Trim(), TextBoxZip.Text.Trim(), TextBoxPhone.Text.Trim(), TextBoxEmail.Text.Trim());
            Sub.Subscriber = p;
            Sub.TargetGroup = GetTargetGroup();

            //err = SubscriptionController.TryInsertSubscription2(Sub);
            if (!string.IsNullOrEmpty(err))
            {
                ShowMess(err, false, true);
                return;
            }

            PlaceHolderIncludedTable.Visible = false;
            ShowMess("Tack för din beställning!<br><br>Första tidningen levereras " + Sub.SubsStartDate.ToShortDateString() + ".<br><br>Med vänlig hälsning,<br>Dagens industri", false, false);
        }


        private string ValidateForm()
        {
            StringBuilder sb = new StringBuilder();

            if (string.IsNullOrEmpty(TextBoxName.Text.Trim()))
                sb.Append("- Namn<br>");

            if (string.IsNullOrEmpty(TextBoxAddress.Text.Trim()))
                sb.Append("- Adress<br>");

            string zip = TextBoxZip.Text.Trim();

            if (string.IsNullOrEmpty(zip) || zip.Length != 5 || !MiscFunctions.IsNumeric(zip))
                sb.Append("- Postnummer (5 siffror)<br>");

            if (string.IsNullOrEmpty(TextBoxPhone.Text.Trim()))
                sb.Append("- Mobilnummer<br>");

            if (!MiscFunctions.IsValidEmail(TextBoxEmail.Text.Trim()))
                sb.Append("- Giltig e-postadress<br>");


            if (!string.IsNullOrEmpty(sb.ToString()))
                return "Var god ange:<br>" + sb.ToString() + "<br>";

            return string.Empty;
        }

        private string GetTargetGroup()
        {
            string tgReg = "";
            string tgMob = "";

            DataSet ds = MsSqlHandler.GetCampaignTargetGroups(CampPageId);
            if (DbHelpMethods.DataSetHasRows(ds))
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    int id = int.Parse(dr["targetGroupTypeId"].ToString());
                    string tg = dr["targetGroup"].ToString();

                    if (id == Settings.TargetGroupType_Regular)
                        tgReg = tg;

                    if (id == Settings.TargetGroupType_Mobile)
                        tgMob = tg;
                }
            }

            if (!string.IsNullOrEmpty(tgMob))
                return tgMob;

            return tgReg;
        }

        private void ShowMess(string mess, bool showForm, bool isError)
        {

            if (isError)
                LabelMess.CssClass = "err";
            else
                LabelMess.CssClass = "";
            
            LabelMess.Text = mess;
            LabelMess.Visible = true;
            PlaceHolderForm.Visible = showForm;
        }



    }
}