using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using DIClassLib.DbHelpers;
using DIClassLib.Misc;
using EPiServer.PlugIn;

namespace Pren.Web.Tools.Admin.PayTrans
{
    [GuiPlugIn(Area = PlugInArea.AdminMenu, Description = "Betalningsadmin", RequiredAccess = EPiServer.Security.AccessLevel.Administer, DisplayName = "Betalningsadmin", UrlFromUi = "/Tools/Admin/PayTrans/PaymentAdmin.aspx", SortIndex = 1020)]
    public partial class PaymentAdmin : System.Web.UI.Page
    {
        public double TotalAmount { get; set; }
        public double TotalVat { get; set; }
        public double Rows { get; set; }

        /// <summary>
        /// Clear all input fields
        /// </summary>
        protected void BtnClearOnClick(object sender, EventArgs e)
        {
            TxtDate.Text = string.Empty;
            TxtRefNo.Text = string.Empty;
            TxtTransNo.Text = string.Empty;
            TxtName.Text = string.Empty;
            TxtEmail.Text = string.Empty;
            TxtFilter.Text = string.Empty;
            DdlStatus.SelectedIndex = 0;
        }

        protected void BtnSearchOnClick(object sender, EventArgs e)
        {
            FilterDataSource();
        }

        protected void GvDataSourceOnSorting(object sender, EventArgs e)
        {
            FilterDataSource();
        }

        /// <summary>
        /// Add filter to ObjectDataSource
        /// </summary>
        private void FilterDataSource()
        {
            //Clear filter parameters
            GvDataSource.FilterParameters.Clear();

            //Add filter parameters based on input
            if (!string.IsNullOrEmpty(TxtFilter.Text) && DdlStatus.SelectedIndex > 0)
            {
                GvDataSource.FilterExpression = "[Comment] LIKE '{0}' AND [Status]='{1}'";
                GvDataSource.FilterParameters.Add("Comment", TxtFilter.Text.Replace("*", "%"));
                GvDataSource.FilterParameters.Add("Status", DdlStatus.SelectedValue);
            }
            else if (!string.IsNullOrEmpty(TxtFilter.Text))
            {
                GvDataSource.FilterExpression = "[Comment] LIKE '{0}'";
                GvDataSource.FilterParameters.Add("Comment", TxtFilter.Text.Replace("*", "%"));
            }
            else if (DdlStatus.SelectedIndex > 0)
            {
                GvDataSource.FilterExpression = "[Status]='{0}'";
                GvDataSource.FilterParameters.Add("Status", DdlStatus.SelectedValue);
            }
        }

        /// <summary>
        /// Get amount and add amount to totalamount
        /// </summary>
        public double GetAmount(object amount)
        {
            double iamount = 0;

            if (amount != DBNull.Value)
            {
                iamount = double.Parse(amount.ToString()) / 100;
                TotalAmount += iamount;
            }

            return iamount;
        }

        /// <summary>
        /// Get vat and add amount to totalvat
        /// </summary>
        public double GetVat(object amount)
        {
            double ivat = 0;

            if (amount != DBNull.Value)
            {
                ivat = double.Parse(amount.ToString()) / 100;
                TotalVat += ivat;
            }

            return ivat;
        }

        public DateTime GetDate(object date)
        {

            DateTime dt = new DateTime();

            if (date != DBNull.Value)
            {
                dt = Convert.ToDateTime(date);
            }

            return dt;  
        }

        /// <summary>
        /// Select method on ObjectDataSource
        /// </summary>
        public DataSet GetResult(string date, string refNo, string transNo, string name, string email)
        {
            List<SqlParameter> sqlParamList = new List<SqlParameter>();

            //Add parameters based on input
            if (!string.IsNullOrEmpty(date))
                sqlParamList.Add(new SqlParameter("@date", date.Replace("*", "%")));
            if (!string.IsNullOrEmpty(refNo))
                sqlParamList.Add(new SqlParameter("@refNo", refNo.Replace("*", "%")));
            if (!string.IsNullOrEmpty(transNo))
                sqlParamList.Add(new SqlParameter("@transno", transNo.Replace("*", "%")));
            if (!string.IsNullOrEmpty(name))
                sqlParamList.Add(new SqlParameter("@name", name.Replace("*", "%")));
            if (!string.IsNullOrEmpty(email))
                sqlParamList.Add(new SqlParameter("@email", email.Replace("*", "%")));

            //The notorious search strangler to avoid massive data
            //If no params, search todays payment
            if (sqlParamList.Count < 1)
                sqlParamList.Add(new SqlParameter("@date", DateTime.Now.ToString("yyyyMMdd")));

            //Return result
            return SqlHelper.ExecuteDatasetParam("DISEPren", "GetPayTransRows", sqlParamList.ToArray());
        }

        protected string GetStatus(string status, string statusCode)
        {
            return MiscFunctions.GetNetsCardPayStatus(status, statusCode);
        }
    }
}