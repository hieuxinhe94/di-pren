using System;
using System.Linq;

using EPiServer.Core;

namespace PrenDiSe.Tools.Properties
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