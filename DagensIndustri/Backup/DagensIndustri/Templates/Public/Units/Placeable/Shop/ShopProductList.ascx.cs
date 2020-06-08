using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using EPiServer;
using EPiServer.Core;
using EPiServer.Filters;
using DagensIndustri.Tools.Classes;
using DagensIndustri.Tools.Classes.Shop;

namespace DagensIndustri.Templates.Public.Units.Placeable.Shop
{
    public partial class ShopProductList : UserControlBase
    {
        #region Events
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            PagingControl.ParentControl = this;
            int noOfProductsPerPage = Convert.ToInt32(EPiFunctions.SettingsPageSetting(CurrentPage, "NoOfProductsPerPage"));
            PagingControl.NoOfHitsPerPage = noOfProductsPerPage == 0 ? 6 : noOfProductsPerPage;

            if (!IsPostBack)
            {
                Populate(PagingControl.NoOfHitsPerPage, 0);
            }
            else
            {
                PagingControl.CreatePagingControl();
            }
        }
        
        protected void BuyLinkButton_Click(object sender, EventArgs e)
        {
            LinkButton buyLinkButton = sender as LinkButton;
            string productPageURL = buyLinkButton.CommandArgument;

            if (!string.IsNullOrEmpty(productPageURL))
                Response.Redirect(productPageURL);            
        }
        #endregion

        #region Methods
        /// <summary>
        /// Calculate the specified number of product pages according to the paging position and 
        /// populate according to that. OBS! Do NOT change the method name or its modifier since
        /// it is used with reflection.
        /// </summary>
        public void Populate()
        {
            int take = PagingControl.CurrentIndex * PagingControl.NoOfHitsPerPage;
            int skip = PagingControl.CurrentIndex == 1 ? 0 : take - PagingControl.NoOfHitsPerPage;

            Populate(take, skip);
        }

        /// <summary>
        /// Get the specified number of product pages according to the paging position.
        /// </summary>
        /// <param name="take"></param>
        /// <param name="skip"></param>
        private void Populate(int take, int skip)
        {
            List<Product> productList = GetProducts(take, skip);

            if (productList != null)
            {
                var pagedDataSrc = new PagedDataSource();
                pagedDataSrc.AllowCustomPaging = true;
                pagedDataSrc.AllowPaging = true;
                pagedDataSrc.DataSource = productList;
                pagedDataSrc.PageSize = PagingControl.NoOfHitsPerPage;

                ProductsRepeater.DataSource = pagedDataSrc;
                ProductsRepeater.DataBind();

                if (PagingControl.TotalNumberOfHits > PagingControl.NoOfHitsPerPage)
                {
                    PagingControl.CreatePagingControl();
                }
            }
        }

        /// <summary>
        /// Get the products for a certain product category (or all products available if no specific product category is specified).
        /// TODO: This method uses FindAllPagesWithCriteria(). If the performance is bad then we have to consider if it is better to use GetDesendant() and Filter.
        /// </summary>
        /// <returns></returns>
        private List<Product> GetProducts(int take, int skip)
        {
            List<Product> products = new List<Product>();

            //If the current shop list page is referring to a products root page then retrieve that specific product category's product pages.
            if (EPiFunctions.HasValue(CurrentPage, "ProductsPage"))
            {
                PropertyCriteriaCollection criteriaColl = new PropertyCriteriaCollection();

                //Set the critieria to get only pages of page type ShopProduct.
                PropertyCriteria criteria1 = new PropertyCriteria();
                criteria1.Condition = EPiServer.Filters.CompareCondition.Equal;
                criteria1.Name = "PageTypeID";
                criteria1.Value = EPiServer.DataAbstraction.PageType.Load((int)EPiFunctions.SettingsPageSetting(CurrentPage, "ProductPageType")).ID.ToString();
                criteria1.Type = PropertyDataType.PageType;
                criteria1.Required = true;
                criteriaColl.Add(criteria1);

                //If the list should only contain a certain product, remove all the products that don't belong to that product category.
                if (EPiFunctions.HasValue(CurrentPage, "ProductCategory"))
                {
                    PropertyCriteria criteria2 = new PropertyCriteria();
                    criteria2.Condition = EPiServer.Filters.CompareCondition.Equal;
                    criteria2.Name = "ProductCategory";
                    criteria2.Value = (string)CurrentPage["ProductCategory"];
                    criteria2.Type = PropertyDataType.String;
                    criteria2.Required = true;
                    criteriaColl.Add(criteria2);
                }

                PageDataCollection pdAllProductPages = EPiServer.DataFactory.Instance.FindAllPagesWithCriteria((PageReference)CurrentPage["ProductsPage"], criteriaColl, CurrentPage.LanguageBranch, new LanguageSelector(CurrentPage.LanguageBranch));

                //Create products from the retrieved pages. If the page has external source, add products from that source.
                List<Product> productList = new List<Product>();
                foreach (PageData pageData in pdAllProductPages)
                {
                    if (EPiFunctions.HasValue(pageData, "HasExternalSource") && (bool)pageData["HasExternalSource"])
                    {
                        productList.AddRange(Product.CreateAdlibrisShopProducts());
                    }
                    else
                    {
                        productList.Add(Product.CreateShopProduct(pageData));
                    }
                }
                
                products = (from product in productList
                                        .Take(take)
                                        .Skip(skip)
                                    select product).ToList<Product>();

                if (!IsPostBack)
                {
                    PagingControl.TotalNumberOfHits = productList.Count;
                    PagingControl.CurrentIndex = 1;
                }
            }

            return products;
        }             

        /// <summary>
        /// Get the product pages for a certain product category (or all products available if no specific product category is specified).
        /// TODO: This method uses GetDescendents() and filter. We have to consider if it is better to use this method than FindAllPagesWithCriteria.
        /// </summary>
        /// <returns></returns>
        //private PageDataCollection GetProductPages(int take, int skip)
        //{
        //    PageDataCollection pdProducts = null;

        //    //If the current shop list page is referring to a products root page then retrieve that specific product category's product pages.
        //    if (Functions.HasValue(CurrentPage, "ProductsPage"))
        //    {
        //        //Get all the descendents from a products page root.
        //        var productPageReferences = DataFactory.Instance.GetDescendents((PageReference)CurrentPage["ProductsPage"]) as List<PageReference>;

        //        PageDataCollection pdAllProductPages = Functions.GetPageDataCollection(productPageReferences, (int)Functions.SettingsPageSetting(CurrentPage, "ProductPageType"));

        //        //If the list should only contain a certain product, remove all the products that don't belong to that product category.
        //        if (Functions.HasValue(CurrentPage, "ProductCategory"))
        //        {
        //            new FilterCompareTo("ProductCategory", (string)CurrentPage["ProductCategory"]).Filter(pdAllProductPages);
        //        }

        //        //Select only the necessary product pages for the view.
        //        var pageDataList = (from pd in pdAllProductPages
        //                                .Take(take)
        //                                .Skip(skip)
        //                            select pd).ToList<PageData>();

        //        pdProducts = new PageDataCollection(pageDataList);

        //        if (!IsPostBack)
        //        {
        //            TotalNumberOfProducts = pdAllProductPages.ToList().Count;
        //            CurrentIndex = 1;
        //        }
        //    }

        //    return pdProducts;
        //}

        #endregion        
    }
}