using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DIClassLib.DbHandlers;
using System.Data.OracleClient;
using System.Text;
using EPiServer.PlugIn;

namespace DagensIndustri.Tools.Admin.CustNameCounter
{
    [GuiPlugIn(Area = PlugInArea.AdminMenu, Description = "Kund-namns-räknare", RequiredAccess = EPiServer.Security.AccessLevel.Administer, DisplayName = "Kund-namns-räknare", UrlFromUi = "/Tools/Admin/CustNameCounter/CustNameCounter.aspx", SortIndex = 1010)]
    public partial class CustNameCounter : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            PlaceHolderSearchRes.Visible = false;
        }

        protected void ButtonDoSearch_Click(object sender, EventArgs e)
        {
            string searchStr = TextBoxName.Text.Trim();
            if (string.IsNullOrEmpty(searchStr))
                return;

            List<StringPair> strPairs = CirixDbHandler.GetNumSubsForPayingCust(searchStr);
            int numOnline = CirixDbHandler.GetNumSubsForPayingCustOnline(searchStr);
            
            if (strPairs.Count == 0)
            {
                PlaceHolderSearchRes.Visible = false;
                return;
            }

            int sum = 0;
            StringBuilder sb = new StringBuilder();
            foreach (StringPair sp in strPairs)
            {
                int num = int.Parse(sp.S1);
                string comp = sp.S2;
                sum += num;
                sb.Append(GetTableRow(num.ToString(), comp));                
            }

            sb.Insert(0, GetTableRow("<b>Antal</b>", "<b>Kundnamn</b>"));
            sb.Insert(0, GetTableRow("&nbsp;", ""));
            sb.Insert(0, GetTableRow("<b>(" + numOnline.ToString() + "</b>", "<b>summa online)</b>"));
            sb.Insert(0, GetTableRow("<b>" + sum.ToString() + "</b>", "<b>summa totalt</b>"));
            

            LiteralList.Text = sb.ToString();
            PlaceHolderSearchRes.Visible = true;
        }


        public string GetTableRow(string col1, string col2)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<tr>");
            sb.Append("<td>");
            sb.Append(col1);
            sb.Append("</td>");
            sb.Append("<td></td>");
            sb.Append("<td>");
            sb.Append(col2);
            sb.Append("</td>");
            sb.Append("</tr>");
            return sb.ToString();
        }

    }
}