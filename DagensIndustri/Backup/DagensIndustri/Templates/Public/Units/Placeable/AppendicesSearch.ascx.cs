using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;
using EPiServer.Core;
using System.Data.SqlClient;
using DIClassLib.DbHelpers;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Collections;

namespace DagensIndustri.Templates.Public.Units.Placeable
{
    public partial class AppendicesSearch : EPiServer.UserControlBase
    {
        #region Properties

        public int TotalItems { get; set; }

        public int FromItem { get; set; }

        public int ToItem { get; set; }

        public int PageNumber
        {
            get
            {
                if (ViewState["PageNumber"] != null)
                    return Convert.ToInt32(ViewState["PageNumber"]);
                else
                    return 0;
            }
            set
            {
                ViewState["PageNumber"] = value;
            }
        }

        public int maxValue = 400;

        #endregion

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PagingRepeater.ItemCommand +=
            new RepeaterCommandEventHandler(PagingRepeater_ItemCommand);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!IsPostBack)
            {
                PopulateAppendix();
            }
        }

        /// <summary>
        /// Show specific appendix
        /// </summary>
        protected void ShowAppendixWithId(object sender, System.EventArgs e)
        {
            LinkButton lnkBtn = ((LinkButton)sender);

            if (lnkBtn != null && !string.IsNullOrEmpty(lnkBtn.CommandArgument))
                ShowAppendix("DiseGetBilagor", new SqlParameter("@bilageId", lnkBtn.CommandArgument), true);
        }

        /// <summary>
        /// Search for appendix matching searchtext
        /// </summary>
        protected void SearchAppendix(object sender, System.EventArgs e)
        {
            PopulateAppendix();
        }

        protected void PopulateAppendix()
        {
            if (!string.IsNullOrEmpty(SearchInput.Text))
                ShowAppendix("searchBilaga", new SqlParameter("@searchWord", SearchInput.Text), true);
            else
            {
                if (IsValue("MaxValue") && Convert.ToInt32(CurrentPage["MaxValue"].ToString()) <= maxValue)
                {
                    maxValue = Convert.ToInt32(CurrentPage["MaxValue"].ToString());
                }
                ShowAppendix("DiseGetBilagor", new SqlParameter("@top", maxValue), false);
            }  
        }

        protected void ShowAppendix(string spName, SqlParameter sqlParameter, bool showHistoryLink)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(null, spName, sqlParameter);

                DataView dv = ds.Tables[0].DefaultView;

                TotalItems= dv.Count;

                var pagedDataSrc = new PagedDataSource();
                pagedDataSrc.AllowPaging = true;
                pagedDataSrc.DataSource = dv;
                pagedDataSrc.PageSize = 20;
                pagedDataSrc.CurrentPageIndex = PageNumber;

                int offset = PageNumber * pagedDataSrc.PageSize;

                FromItem = offset + 1;

                if(pagedDataSrc.Count < pagedDataSrc.PageSize)
                    ToItem = offset + pagedDataSrc.Count;
                else
                    ToItem = offset + pagedDataSrc.PageSize;

                if (TotalItems > 0)
                {
                    ResultsCounterPlaceHolder.Visible = true;
                }
                else
                {
                    ResultsCounterPlaceHolder.Visible = false;
                }

                if (pagedDataSrc.PageCount > 1)
                {
                    PagingRepeater.Visible = true;
                    ArrayList pages = new ArrayList();
                    for (int i = 0; i < pagedDataSrc.PageCount; i++)
                        pages.Add((i + 1).ToString());
                    PagingRepeater.DataSource = pages;
                    PagingRepeater.DataBind();
                }
                else
                {
                    PagingRepeater.Visible = false;
                }

                AppendixRepeater.DataSource = pagedDataSrc;
                AppendixRepeater.DataBind();

            }
            catch (Exception ex)
            {
                new Logger("ShowAppendix(string spName, SqlParameter sqlParameter, bool showHistoryLink) - failed", ex.ToString());
                //LblMessage.Text = Translate("/appendix/error");
            }
        }

        void PagingRepeater_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            PageNumber = Convert.ToInt32(e.CommandArgument) - 1;
            PopulateAppendix();
        }
    }

}