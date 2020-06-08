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
using EPiServer.Core;
using System.Data.SqlClient;
using DIClassLib.DbHandlers;
using DagensIndustri.Tools.Classes.Campaign;

namespace DagensIndustri.Tools.Properties
{
    public partial class TypeControl : System.Web.UI.UserControl
    {
        string commentText = LanguageManager.Instance.Translate("/campaigns/various/comment");
        string chooseText = LanguageManager.Instance.Translate("/campaigns/choose/type");

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!IsPostBack)
            {

            }
        }

        public void FillTypes()
        {
            //DdlTypes.DataSource = MsSqlHandler.GetTypes();
            //DdlTypes.DataBind();

            //DdlTypes.Items.Insert(0, new ListItem(chooseText));
        }

        public void SetSelectedValue(PageData page)
        {
            //DataSet dsType = page.GetCampaignType();

            //if (dsType.Tables[0].Rows.Count > 0)
            //{
            //    DdlTypes.SelectedValue = dsType.Tables[0].Rows[0]["typeId"].ToString();

            //    string comment = dsType.Tables[0].Rows[0]["typeComment"].ToString();
            //    if (!string.IsNullOrEmpty(comment))
            //        TxtComment.Text = comment;
            //}
        }


        public string GetComment()
        {
            return TxtComment.Text != commentText ? TxtComment.Text : string.Empty;
        }

        public string GetValue()
        {
            return DdlTypes.SelectedIndex > 0 ? DdlTypes.SelectedValue : string.Empty;
        }
    }
}