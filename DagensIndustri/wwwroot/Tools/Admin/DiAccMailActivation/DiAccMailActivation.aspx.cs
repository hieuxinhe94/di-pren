using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;
using System.Collections;
using ApsisMailSort.ClassLibrary;
using ApsisMailSort.Handlers;
using EPiServer.PlugIn;


namespace DagensIndustri.Tools.Admin.DiAccMailActivation
{
    [GuiPlugIn(Area = PlugInArea.AdminMenu, Description = "DIKonton", RequiredAccess = EPiServer.Security.AccessLevel.Administer, DisplayName = "DIKonton", UrlFromUi = "/Tools/Admin/DiAccMailActivation/DiAccMailActivation.aspx", SortIndex = 1020)]

    public partial class DiAccMailActivation : System.Web.UI.Page
    {
        //Declarations
        #region Declarations
        //private List<GridViewListData> gridViewData = new List<GridViewListData>();
        private List<string> mailList = new List<string>();
        private ArrayList SubsLen_Mons = new ArrayList();

        private int iMailOne = 0;
        private int iMailTwo = 0;
        private int iMailThree = 0;
        private int iMailStart = 0;
        private int iOtherCheck = 0;
        private int iTotal = 0;

        private string sDateFrom;
        private string sDateTo;
        private string sBetweenMail;
        private string sPaperCode;
        private string sProductNo;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                PopulateDdls();

            }
        }

        #region Buttons
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Filter();
        }

        //Export to Excel
        protected void btnExport_Click(object sender, EventArgs e)
        {
            if (GridView1.Rows.Count == 0)
            {
                //grid is empty
            }

            if (GridView1.Rows.Count != 0)
            {
                Response.ClearContent();
                Response.AddHeader("content-disposition", "attachment; filename=" + DateTime.Now + ".xls");
                Response.ContentType = "application/excel";
                System.IO.StringWriter sw = new System.IO.StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                GridView1.RenderControl(htw);
                Response.Write(sw.ToString());
                Response.End();
            }
        }

        //Resets Search Parameters, table and gridview
        protected void btnReset_Click(object sender, EventArgs e)
        {
            tbxDateFrom.Text = "";
            tbxDateTo.Text = "";
            ddlBetweenMail.SelectedValue = "0";
            ddlPaperCode.SelectedValue = "0";
            ddlProductNo.SelectedValue = "0";
            ddlSubsLen_Mons.SelectedValue = "0";

            iMailOne = 0;
            iMailTwo = 0;
            iMailThree = 0;
            iMailStart = 0;
            iOtherCheck = 0;
            iTotal = 0;

            PopulateTable(iTotal, iMailOne, iMailTwo, iMailThree, iMailStart, iOtherCheck);

            GridView1.DataSource = null;
            GridView1.DataBind();
        }
        #endregion

        #region Functions
        public void PopulateDdls()
        {
            PopulateDdlPaperCode();
            PopulateDdlProductNo();
            PopulateDdlMail();
            PopulateDdlSubsLenMons();
        }

        //Populates ddl for PaperCode
        public void PopulateDdlPaperCode()
        {
            DbHandler db = new DbHandler();

            foreach (StringPair sp in db.GetPaperCodeList())
                ddlPaperCode.Items.Add(new ListItem(sp.Text, sp.Value));
        }

        //Populates ddl ProductNo
        public void PopulateDdlProductNo()
        {
            DbHandler db = new DbHandler();
            foreach (StringPair sp in db.GetProductNoList())
                ddlProductNo.Items.Add(new ListItem(sp.Text, sp.Value));
        }

        //Populates ddl for Mail
        public void PopulateDdlMail()
        {
            ddlBetweenMail.Items.Add(new ListItem() { Text = "Mail", Value = 0.ToString() });
            for (int i = 1; i < 5; i++)
            {
                ddlBetweenMail.Items.Add(
                    new ListItem()
                    {
                        Text = "Mail " + i,
                        Value = i.ToString()
                    });
            }
        }

        //Populates ddl SubsLen_Mons
        public void PopulateDdlSubsLenMons()
        {
            ddlSubsLen_Mons.Items.Add(new ListItem() { Text = "SubsLen_Mons", Value = 0.ToString() });
            for (int i = 1; i < 49; i++)
            {
                ddlSubsLen_Mons.Items.Add(new ListItem() { Text = i.ToString(), Value = i.ToString() });
            }
            ddlSubsLen_Mons.DataBind();
        }

        //Filters Registred Accounts
        public void Filter()
        {
            sDateFrom = tbxDateFrom.Text;
            sDateTo = tbxDateTo.Text;
            sBetweenMail = ddlBetweenMail.SelectedValue.ToString();
            sPaperCode = ddlPaperCode.SelectedValue.ToString();
            sProductNo = ddlProductNo.SelectedValue.ToString();

            for (int i = 1; i < ddlSubsLen_Mons.Items.Count; i++)
            {
                if (ddlSubsLen_Mons.Items[i].Selected == true)
                {
                    SubsLen_Mons.Add(int.Parse(ddlSubsLen_Mons.Items[i].Value));
                }
            }

            DbHandler db = new DbHandler();

            var gridViewData = new List<GridViewListData>();

            foreach (GridViewListData gld in db.GetGridViewData(sDateFrom, sDateTo, sBetweenMail, sPaperCode, SubsLen_Mons, sProductNo))
                gridViewData.Add(gld);

            GridView1.DataSource = gridViewData;
            GridView1.DataBind();

            CheckHowManyCustomerReggedAfterWhichMail(gridViewData);
        }

        //Populates Mail Table
        public void PopulateTable(int total, int betweenMailOneAndTwo, int betweenMailTwoAndThree, int betweenMailThreeAndFour, int beforeMailTwo, int onOtherCheck)
        {
            literalMail1.Text = betweenMailOneAndTwo.ToString();
            literalMail2.Text = betweenMailTwoAndThree.ToString();
            literalMail3.Text = betweenMailThreeAndFour.ToString();
            literalMailStart.Text = beforeMailTwo.ToString();
            literalOther.Text = onOtherCheck.ToString();
            literalTotal.Text = total.ToString();
        }

        //Checks how many customers regged after which mail
        public void CheckHowManyCustomerReggedAfterWhichMail(List<GridViewListData> gridViewData)
        {
            foreach (var mail in gridViewData)
                mailList.Add(mail.ApsisUpdateCheckServicePlus);

            foreach (var mail in mailList)
            {
                switch (mail)
                {
                    case "1":
                        iMailOne++;
                        break;
                    case "2":
                        iMailTwo++;
                        break;
                    case "3":
                        iMailThree++;
                        break;
                    case null:
                    case "":
                        iMailStart++;
                        break;
                    default:
                        iOtherCheck++;
                        break;
                }
            }

            iTotal = iMailOne + iMailTwo + iMailThree + iMailStart + iOtherCheck;

            PopulateTable(iTotal, iMailOne, iMailTwo, iMailThree, iMailStart, iOtherCheck);
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            //
        }
        #endregion
    }
}