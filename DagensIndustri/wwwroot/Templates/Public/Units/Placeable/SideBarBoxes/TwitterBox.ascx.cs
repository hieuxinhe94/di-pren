using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DagensIndustri.Tools.Classes;
using EPiServer.Core;


namespace DagensIndustri.Templates.Public.Units.Placeable.SideBarBoxes
{
    public partial class TwitterBox : EPiServer.UserControlBase
    {

        //NB! - UserControl uses OutputCache cause Twitter only allows x calls / time unit


        private PageData _pd;
        public int PageID { get; set; }

        public string WidgetId
        {
            get
            {
                if (EPiFunctions.HasValue(_pd, "WidgetId"))
                    return _pd["WidgetId"].ToString();

                return string.Empty;
            }
        }

        //v1
        //public string TweetName
        //{
        //    get
        //    {
        //        if (EPiFunctions.HasValue(_pd, "TweetName"))
        //            return _pd["TweetName"].ToString();

        //        return string.Empty;
        //    }
        //}

        //public string ShowNumItems
        //{
        //    get
        //    {
        //        if (EPiFunctions.HasValue(_pd, "ShowNumItems"))
        //            return _pd["ShowNumItems"].ToString();

        //        return "3";
        //    }
        //}


        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            DataBind();

            if (PageID > 0)
                _pd = EPiServer.DataFactory.Instance.GetPage(new PageReference(PageID));
            else
                _pd = CurrentPage;

            if (string.IsNullOrEmpty(WidgetId))
                this.Visible = false;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }
    }
}