using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;
using EPiServer.Core;
using DagensIndustri.Tools.Classes.BaseClasses;
using DIClassLib.Antipodes;

namespace DagensIndustri.Templates.Public.Pages.Antipodes
{
    public partial class Wine : DiTemplatePage
    {
        public string imageURL { get; set; }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (IsValue("WineId"))
            {
                int WineId = Convert.ToInt32(CurrentPage["WineId"].ToString());
                WineItem wi = AntipodesWinesHandler.GetWineItem(WineId);
                imageURL = wi.ImagePath;
                NameLiteral.Text = wi.Name;
                CountryRegionLiteral.Text = string.Format("{0}, {1}", wi.Country, wi.Region);
                PriceLiteral.Text = string.Format("{0}", wi.Type);
                QuantityLiteral.Text = string.Format("{0}{1}", wi.BottleSize, wi.Unit);
            }
        }
    }
}