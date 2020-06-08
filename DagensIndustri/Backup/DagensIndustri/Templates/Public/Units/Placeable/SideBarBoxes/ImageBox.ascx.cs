using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;
using EPiServer.Core;
using DagensIndustri.Tools.Classes;

namespace DagensIndustri.Templates.Public.Units.Placeable.SideBarBoxes
{
    public partial class ImageBox : EPiServer.UserControlBase
    {
        private int _pageID;

        public int PageID
        {
            get { return _pageID; }
            set { _pageID = value; }
        }

        public string imageURL { get; set; }
        public string imageAltText { get; set; }
        public string targetURL { get; set; }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            PageData pd = new PageData();

            if (PageID > 0)
                pd = EPiServer.DataFactory.Instance.GetPage(new PageReference(PageID));
            else
                pd = CurrentPage;

            if (pd != null)
            {

                if (EPiFunctions.HasValue(pd, "Image"))
                {
                    imageURL = pd["Image"].ToString();
                    imageAltText = pd.PageName.ToString();
                }
                else
                {
                    this.Visible = false;
                }

                if (EPiFunctions.HasValue(pd, "TargetURL"))
                {
                    targetURL = pd["TargetURL"].ToString();
                }

            }
            else
            {
                this.Visible = false;
            }
        }
    }
}