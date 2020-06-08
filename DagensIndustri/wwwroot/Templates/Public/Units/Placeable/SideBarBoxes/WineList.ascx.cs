using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;
using EPiServer.Core;
using DagensIndustri.Tools.Classes.WebControls;
using DagensIndustri.Tools.Classes;
using DIClassLib.Antipodes;

namespace DagensIndustri.Templates.Public.Units.Placeable.SideBarBoxes
{
    public partial class WineList : EPiServer.UserControlBase
    {
        private int _pageID;

        public int PageID
        {
            get { return _pageID; }
            set { _pageID = value; }
        }

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
                if (EPiFunctions.HasValue(pd, "Heading"))
                {
                    HeadingLiteral.Text = pd["Heading"].ToString();

                    WineListRepeater.DataSource = AntipodesWinesHandler.GetAllWines();
                    WineListRepeater.DataBind();
                }
                else
                {
                    this.Visible = false;
                }
            }
            else
            {
                this.Visible = false;
            }    
        }

        public string WineURL()
        {
            return "http://www.divinklubb.se/di_guld_136.php";
        }
    }
}