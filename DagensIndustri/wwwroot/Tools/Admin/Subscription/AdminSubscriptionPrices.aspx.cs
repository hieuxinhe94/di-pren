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
using EPiServer;
using EPiServer.PlugIn;
using DagensIndustri.Tools.Classes.Subscription;
using DIClassLib.DbHandlers;
//using DagensIndustri.Tools.Classes.DbHandlers;

namespace DagensIndustri.Tools.Admin.Subscription
{
    [GuiPlugIn(Area = PlugInArea.AdminMenu, Description = "Prenumerationsadmin, prisintervall", RequiredAccess = EPiServer.Security.AccessLevel.Administer, DisplayName = "Prenumerationsadmin, prisintervall", UrlFromUi = "/Tools/Admin/Subscription/AdminSubscriptionPrices.aspx", SortIndex = 1030)]
    public partial class AdminSubscriptionPrices : System.Web.UI.Page
    {
        DbHandler _db = new DbHandler();

        string _urlDo;
        int _urlId = 0;

        string _cmdDoNew = "new";
        string _cmdDoEdit = "edit";
        string _cmdDoDelete = "delete";

        protected void Page_Load(object sender, EventArgs e)
        {
            _urlDo = Request.QueryString["do"];
            int.TryParse(Request.QueryString["aid"], out _urlId);

            PopulateRBLs();
            PopulateDDLs();

            if (!IsPostBack && !string.IsNullOrEmpty(_urlDo) && _urlId > 0)
                ExecuteDo();
            else
                HandlePanelVisibility(true, false);
        }


        private void PopulateRBLs()
        {
            RadioButtonListPrices1.Items.Clear();
            RadioButtonListPrices2.Items.Clear();
            RadioButtonListPrices3.Items.Clear();
            AddOffersToRBL(1);
            AddOffersToRBL(2);
            AddOffersToRBL(3);
        }

        private void PopulateDDLs()
        {
            if (!IsPostBack || DropDownListCamp1.Items.Count == 0)
            {
                //regular offers
                List<ListItem> tmp = new List<ListItem>();
                DataSet ds = CirixHandler.GetActiveCampaigs(Settings.SubProductNo_Regular);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string txt = dr.ItemArray[1].ToString() + " - " + dr.ItemArray[2].ToString();
                    tmp.Add(new ListItem(txt, dr.ItemArray[0].ToString()));
                }
                tmp = SortListItems(tmp, false);
                foreach (ListItem l in tmp)
                {
                    DropDownListCamp1.Items.Add(l);
                    DropDownListCamp2.Items.Add(l);
                }

                //weekend offers
                tmp = new List<ListItem>();
                ds = CirixHandler.GetActiveCampaigs(Settings.SubProductNo_Weekend);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string txt = dr.ItemArray[1].ToString() + " - " + dr.ItemArray[2].ToString();
                    tmp.Add(new ListItem(txt, dr.ItemArray[0].ToString()));
                }
                tmp = SortListItems(tmp, false);
                foreach (ListItem l in tmp)
                    DropDownListCamp3.Items.Add(l);


                ListItem li = new ListItem("Lägg till ny", "");
                DropDownListCamp1.Items.Insert(0, li);
                DropDownListCamp2.Items.Insert(0, li);
                DropDownListCamp3.Items.Insert(0, li);
            }
        }

        private List<ListItem> SortListItems(List<ListItem> list, bool desc)
        {
            if (desc)
                list.Sort(delegate(ListItem x, ListItem y) { return y.Text.CompareTo(x.Text); });
            else
                list.Sort(delegate(ListItem x, ListItem y) { return x.Text.CompareTo(y.Text); });

            return list;
        }


        private void ExecuteDo()
        {
            if (_urlDo == _cmdDoNew)
            {
                DataSet ds = CirixHandler.GetCampaign((long)_urlId);
                if (ds != null)
                    TextBoxOfferTxt.Text = ds.Tables[0].Rows[0].ItemArray[8].ToString();

                TextBoxSorting.Text = "100";
                CheckBoxIsActive.Checked = false;
                CheckBoxIsAutogiro.Checked = false;
                HiddenFieldId.Value = _urlId.ToString();
                HandlePanelVisibility(false, true);
            }

            if (_urlDo == _cmdDoEdit)
            {
                DataTable dt = _db.GetCampaignFromMSSQLById(_urlId);
                TextBoxOfferTxt.Text = dt.Rows[0].ItemArray[3].ToString();

                if (string.IsNullOrEmpty(TextBoxOfferTxt.Text))
                {
                    DataSet ds = CirixHandler.GetCampaign(long.Parse(dt.Rows[0].ItemArray[1].ToString()));
                    if (ds != null)
                        TextBoxOfferTxt.Text = ds.Tables[0].Rows[0].ItemArray[8].ToString();
                }

                TextBoxSorting.Text = dt.Rows[0].ItemArray[4].ToString();
                CheckBoxIsActive.Checked = bool.Parse(dt.Rows[0].ItemArray[5].ToString());
                CheckBoxIsAutogiro.Checked = bool.Parse(dt.Rows[0].ItemArray[6].ToString());
                HiddenFieldId.Value = _urlId.ToString();
                HandlePanelVisibility(false, true);
            }

            if (_urlDo == _cmdDoDelete)
            {
                _db.DeleteOffer(_urlId);
                CampObjHandler.Init(true);
                HandlePanelVisibility(true, false);
            }

            PopulateRBLs();
        }


        private void HandlePanelVisibility(bool prices, bool edit)
        {
            PanelPrices.Visible = prices;
            PanelEdit.Visible = edit;
        }


        protected void ButtonEditSave_Click(object sender, EventArgs e)
        {
            if (_urlDo == _cmdDoNew)
            {
                _db.InsertOffer(_urlId,
                                int.Parse(Request["offerGroup"]),
                                TextBoxOfferTxt.Text,
                                int.Parse(TextBoxSorting.Text),
                                CheckBoxIsAutogiro.Checked,
                                CheckBoxIsActive.Checked);
            }

            if (_urlDo == _cmdDoEdit)
            {
                _db.UpdateOffer(int.Parse(HiddenFieldId.Value),
                                TextBoxOfferTxt.Text,
                                int.Parse(TextBoxSorting.Text),
                                CheckBoxIsAutogiro.Checked,
                                CheckBoxIsActive.Checked);
            }
            CampObjHandler.Init(true);
            Response.Redirect(System.IO.Path.GetFileName(HttpContext.Current.Request.FilePath));
        }



        protected void DropDownListCamp_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList dl = (DropDownList)sender;

            if (dl.SelectedIndex > 0)
            {
                string off = dl.ID.Substring(dl.ID.Length - 1, 1);
                Response.Redirect(GetCurrentPage + "?do=new&offerGroup=" + off + "&aid=" + dl.SelectedValue);
            }
        }

        public string GetCurrentPage
        {
            get { return System.IO.Path.GetFileName(HttpContext.Current.Request.FilePath); }
        }




        private void AddOffersToRBL(int offerGr)
        {
            DataTable dt = _db.GetCampaignsFromMSSQL(offerGr, false);
            foreach (DataRow r in dt.Rows)
            {
                int id = int.Parse(r.ItemArray[0].ToString());
                long campNo = long.Parse(r.ItemArray[1].ToString());
                int offerGroup = offerGr;  //int.Parse(r.ItemArray[2].ToString());
                string offerTxt = r.ItemArray[3].ToString();
                int sortOrder = int.Parse(r.ItemArray[4].ToString());
                bool isActive = bool.Parse(r.ItemArray[5].ToString());
                bool isAutoGiro = bool.Parse(r.ItemArray[6].ToString());

                CampaignPriceRow ci = new CampaignPriceRow(id, campNo, offerGroup, offerTxt, sortOrder, isActive, isAutoGiro, true);
                ListItem li = new ListItem(ci.ListItemText, campNo.ToString());
                li.Enabled = false;

                if (offerGr == 1) RadioButtonListPrices1.Items.Add(li);
                if (offerGr == 2) RadioButtonListPrices2.Items.Add(li);
                if (offerGr == 3) RadioButtonListPrices3.Items.Add(li);
            }
        }

    }
}