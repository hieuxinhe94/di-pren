using System;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using EPiServer;
using EPiServer.Core;
using DagensIndustri.Tools.Classes.BaseClasses;
using DIClassLib.DbHelpers;


namespace DagensIndustri.Templates.Public.Pages
{
    public partial class Vendors : DiTemplatePage
    {
        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (!IsPostBack)
                FillCities();
        }

        /// <summary>
        /// Fills dropdownlist with cities. Retrieved from database.
        /// </summary>
        protected void FillCities()
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset("DagensIndustriMISC", "GetCities", null);
                DdlCities.DataSource = ds.Tables[0].DefaultView;
                DdlCities.DataTextField = ds.Tables[0].Columns["City"].ColumnName;
                DdlCities.DataBind();
                //Insert first item in list
                DdlCities.Items.Insert(0, "Välj ort"); //Translate("/vendors/defaultitemtext"));
            }
            catch (Exception ex)
            {
                new Logger("FillCities() - failed", ex.ToString());
                ShowMessage("/vendors/error");
            }
        }

        /// <summary>
        /// Fill a repeater with vendors for selected city. Retrieved from database.
        /// </summary>
        protected void FillVendorList(object sender, System.EventArgs e)
        {
            try
            {
                if (DdlCities.SelectedIndex > 0)
                {
                    DataSet ds = SqlHelper.ExecuteDataset("DagensIndustriMISC", "GetVendors", new SqlParameter("@City", DdlCities.SelectedValue));
                    RepVendorInfo.DataSource = ds;
                    RepVendorInfo.DataBind();
                }
            }
            catch (Exception ex)
            {
                new Logger("FillVendorList() - failed", ex.ToString());
                ShowMessage("/vendors/error");
            }
        }

        private void ShowMessage(string translateKey)
        {
            DdlCities.Visible = false;
            LblMessage.Text = Translate(translateKey);
        }
    }
}