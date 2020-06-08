﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using DIClassLib.DbHelpers;
using EPiServer.PlugIn;

namespace DagensIndustri.Tools.Admin.LogSearch
{
    [GuiPlugIn(Area = PlugInArea.AdminMenu, Description = "Sök i logg", RequiredAccess = EPiServer.Security.AccessLevel.Administer, DisplayName = "Loggsök", UrlFromUi = "/Tools/Admin/LogSearch/LogSearch.aspx", SortIndex = 1030)]
    public partial class LogSearch : System.Web.UI.Page
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
            TxtDescription.Text = string.Empty;
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
            ////Clear filter parameters
            //GvDataSource.FilterParameters.Clear();

            ////Add filter parameters based on input
            //if (!string.IsNullOrEmpty(TxtFilter.Text) && DdlStatus.SelectedIndex > 0)
            //{
            //    GvDataSource.FilterExpression = "[Comment] LIKE '{0}' AND [Status]='{1}'";
            //    GvDataSource.FilterParameters.Add("Comment", TxtFilter.Text.Replace("*", "%"));
            //    GvDataSource.FilterParameters.Add("Status", DdlStatus.SelectedValue);
            //}
            //else if (!string.IsNullOrEmpty(TxtFilter.Text))
            //{
            //    GvDataSource.FilterExpression = "[Comment] LIKE '{0}'";
            //    GvDataSource.FilterParameters.Add("Comment", TxtFilter.Text.Replace("*", "%"));
            //}
            //else if (DdlStatus.SelectedIndex > 0)
            //{
            //    GvDataSource.FilterExpression = "[Status]='{0}'";
            //    GvDataSource.FilterParameters.Add("Status", DdlStatus.SelectedValue);
            //}
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
        public DataSet GetResult(string fromdate, string description)
        {
            List<SqlParameter> sqlParamList = new List<SqlParameter>();

  
            fromdate = string.IsNullOrEmpty(fromdate) ? DateTime.Now.ToString("yyyyMMdd") : fromdate;
            description = string.IsNullOrEmpty(description) ? " " : description;
            
            sqlParamList.Add(new SqlParameter("@fromdate", fromdate.Replace("*", "%")));

            sqlParamList.Add(new SqlParameter("@description", description));

            //Return result
            return SqlHelper.ExecuteDatasetParam("DagensIndustriMISC", "LogSearch", sqlParamList.ToArray());
        }
    }
}