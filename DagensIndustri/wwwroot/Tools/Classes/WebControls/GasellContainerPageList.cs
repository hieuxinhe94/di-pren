using System;
using System.Collections.Generic;
using System.Text;
using EPiServer.Web.WebControls;
using EPiServer.Core;
using System.Web.UI;
using System.ComponentModel;
using DagensIndustri.Tools.Classes;

namespace DagensIndustri.Tools.Classes.WebControls
{
    public class GasellContainerPageList : EPiServer.Web.WebControls.PageList
    {
        protected override void CreateChildControls()
        {
            PageDataCollection pages = base.GetPages();
            if (pages.Count != 0)
            {
                PageData page = null;
                if (!PageReference.IsNullOrEmpty(this.PageLink))
                {
                    page = this.GetPage(this.PageLink);
                }
                if (HeaderTemplate != null)
                {
                    Control container = new PageTemplateContainer(page);
                    HeaderTemplate.InstantiateIn(container);
                    this.Controls.Add(container);
                }
                this.PreparePagingControls(pages);

                for (int i = 0; i < pages.Count; i++)
                {
                    Control control2 = new PageTemplateContainer(pages[i]);

                    int pageTypeID = pages[i].PageTypeID;

                    if (GasellPuffTemplate != null && isMatchingPageType(pageTypeID, "GasellPuffPageType"))
                        GasellPuffTemplate.InstantiateIn(control2);
                    else if (GasellMovieListTemplate != null && isMatchingPageType(pageTypeID, "GasellMovieListPageType"))
                        GasellMovieListTemplate.InstantiateIn(control2);
                    else if (GasellSearchTemplate != null && isMatchingPageType(pageTypeID, "GasellSearchPageType"))
                        GasellSearchTemplate.InstantiateIn(control2);
                    else if (GasellAdPuffTemplate != null && isMatchingPageType(pageTypeID, "GasellAdPuffPageType"))
                        GasellAdPuffTemplate.InstantiateIn(control2);
                    else if (GasellAdListTemplate != null && isMatchingPageType(pageTypeID, "GasellAdListPageType"))
                        GasellAdListTemplate.InstantiateIn(control2);
                    else if (ItemTemplate != null)
                        ItemTemplate.InstantiateIn(control2);

                    this.Controls.Add(control2);
                }
                if (FooterTemplate != null)
                {
                    Control control3 = new PageTemplateContainer(page);
                    FooterTemplate.InstantiateIn(control3);
                    this.Controls.Add(control3);
                }
                this.CreatePagingControls(pages);
            }
        }

        /// <summary>
        /// Match the pagetypeID with pagetype-property on StartPage
        /// </summary>
        /// <param name="pageTypeID">The pageTypeID to match</param>
        /// <param name="propertyName">The property on StartPage containing the pagetypeId to match against</param>
        /// <returns>true if match</returns>
        private bool isMatchingPageType(int pageTypeID, string propertyName)
        {
            if (EPiFunctions.SettingsPage(CurrentPage)[propertyName] != null)
            {
                if (pageTypeID.Equals((int)EPiFunctions.SettingsPage(CurrentPage)[propertyName]))
                    return true;
            }

            return false;
        }

        //private PageData StartPage
        //{
        //    get
        //    {
        //        return GetPage(PageReference.StartPage);
        //    }
        //}

        [TemplateContainer(typeof(PageTemplateContainer)), Browsable(false), PersistenceMode(PersistenceMode.InnerProperty)]
        public ITemplate GasellPuffTemplate { get; set; }

        [TemplateContainer(typeof(PageTemplateContainer)), Browsable(false), PersistenceMode(PersistenceMode.InnerProperty)]
        public ITemplate GasellMovieListTemplate { get; set; }

        [TemplateContainer(typeof(PageTemplateContainer)), Browsable(false), PersistenceMode(PersistenceMode.InnerProperty)]
        public ITemplate GasellSearchTemplate { get; set; }

        [TemplateContainer(typeof(PageTemplateContainer)), Browsable(false), PersistenceMode(PersistenceMode.InnerProperty)]
        public ITemplate GasellAdPuffTemplate { get; set; }

        [TemplateContainer(typeof(PageTemplateContainer)), Browsable(false), PersistenceMode(PersistenceMode.InnerProperty)]
        public ITemplate GasellAdListTemplate { get; set; }
    }
}