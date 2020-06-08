using System;
using EPiServer.Core;
using System.Collections.Generic;
using EPiServer.Filters;
using DagensIndustri.Tools.Classes.BaseClasses;

namespace DagensIndustri.Templates.Public.Units.Placeable.Gasellerna
{
    public partial class GasellAdList : GasellUserControlBase
    {
        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            PlAdList.Filter += new EPiServer.Web.WebControls.FilterEventHandler(RandomizeFilter);
            PlAdList.PageLink = ActualCurrentPage.PageLink;
        }


        /// <summary>
        /// Random sortorder on ads
        /// </summary>
        protected void RandomizeFilter(object sender, FilterEventArgs e)
        {
            PageDataCollection pagesToRandomize = e.Pages;

            for (int i = 0; i < pagesToRandomize.Count; i++)
            {
                PageData x = pagesToRandomize[i];
                Random rng = new Random();
                int index = rng.Next(pagesToRandomize.Count - i) + i;
                pagesToRandomize[i] = pagesToRandomize[index];
                pagesToRandomize[index] = x;
            }
        }
    }
}