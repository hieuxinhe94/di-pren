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
using DIClassLib.DbHandlers;


namespace DagensIndustri.Tools.Admin.Apsis
{
    public partial class Rules : System.Web.UI.UserControl
    {
        MailSenderDbHandler _db;
        public MailSenderDbHandler Db
        {
            get
            {
                if (_db == null)
                    _db = new MailSenderDbHandler();

                return _db;
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                BindList();
        }


        protected void ButtonNew_Click(object sender, EventArgs e)
        {
            Db.InsertEmailRule(TextBoxNewRule.Text);
            TextBoxNewRule.Text = "";
            DataList1.EditItemIndex = -1;
            BindList();
        }

        public void Edit_Command(Object sender, DataListCommandEventArgs e)
        {
            DataList1.EditItemIndex = e.Item.ItemIndex;
            BindList();
        }

        public void Cancel_Command(Object sender, DataListCommandEventArgs e)
        {
            DataList1.EditItemIndex = -1;
            BindList();
        }

        public void Update_Command(Object sender, DataListCommandEventArgs e)
        {
            int id = int.Parse(e.CommandArgument.ToString());
            string rule = ((TextBox)e.Item.FindControl("TextBoxRule")).Text;
            Db.UpdateEmailRule(id, rule);

            DataList1.EditItemIndex = -1;
            BindList();
        }

        public void Delete_Command(Object sender, DataListCommandEventArgs e)
        {
            int id = int.Parse(e.CommandArgument.ToString());
            Db.DeleteEmailRule(id);

            DataList1.EditItemIndex = -1;
            BindList();
        }

        void BindList()
        {
            DataList1.DataSource = Db.GetEmailRules();
            DataList1.DataBind();
        }
    }
}