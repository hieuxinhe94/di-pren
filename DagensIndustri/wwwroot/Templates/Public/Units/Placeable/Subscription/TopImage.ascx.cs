using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using EPiServer;
using EPiCode.ImageList.Utils;
using EPiCode.ImageList.SpecializedProperties;
using DIClassLib.Subscriptions;
using DagensIndustri.Templates.Public.Pages;

namespace DagensIndustri.Templates.Public.Units.Placeable.Subscription
{
    public partial class TopImage : UserControlBase
    {
        #region Variables
        string[] ImageListItemCss = new string[] { "col_2", "col_2 gold", "col_3", "col_3 gold", "col_4", "col_4 gold", "col_5", "col_6", "col_6 gold" };
        #endregion

        #region Events
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!IsPostBack)
            {
                LoadAllImages();
            }
        }

        protected void ImageViewerRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                LinkItem linkItem = e.Item.DataItem as LinkItem;

                Image topItemImage = e.Item.FindControl("TopItemImage") as Image;
                if (topItemImage != null)
                {
                    topItemImage.ImageUrl = linkItem.ImageLink.URL;
                    topItemImage.AlternateText = linkItem.ImageLink.AltText;
                }

                HtmlGenericControl listItem = e.Item.FindControl("ListItem") as HtmlGenericControl;
                if (listItem != null && e.Item.ItemIndex < ImageListItemCss.Length)
                {
                    bool isActive = e.Item.ItemIndex == GetActiveIndex();
                    if (isActive)
                    {
                        listItem.Attributes.Add("class", string.Format("{0} active", ImageListItemCss[e.Item.ItemIndex]));
                    }
                    else
                    {
                        listItem.Attributes.Add("class", ImageListItemCss[e.Item.ItemIndex]);
                    }
                }
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Load all images from the image gallery
        /// </summary>
        public void LoadAllImages()
        {
            ImageViewerRepeater.DataSource = GetImageList();
            ImageViewerRepeater.DataBind();
        }


        /// <summary>
        /// Load only certain image based on selected subscription type and whether user has chosen to 
        /// become a DiGold member or not. 
        /// </summary>
        public void LoadSelectedSubscriptionTypeImage()
        {
            int activeIndex = GetActiveIndex();

            LinkItemCollection imgList = GetImageList();

            if (imgList.Count > activeIndex)
            {
                LinkItemCollection singleImageLinkItemColl = new LinkItemCollection();
                singleImageLinkItemColl.Add(imgList[activeIndex]);

                ImageViewerRepeater.DataSource = singleImageLinkItemColl;
                ImageViewerRepeater.DataBind();
            }
        }

        /// <summary>
        /// Get images from ImageListNotLoggedIn property. If no images were specified, try and get them from ImageListLoggedInNotGold.
        /// </summary>
        /// <returns></returns>
        private LinkItemCollection GetImageList()
        {           
            PropertyImageList imgList = CurrentPage.Property["ImageListNotLoggedIn"] as PropertyImageList;
            LinkItemCollection linkItemColl = imgList.SelectedLinkItems;

            if (linkItemColl.Count == 0)
            {
                imgList = CurrentPage.Property["ImageListLoggedInNotGold"] as PropertyImageList;
                linkItemColl = imgList.SelectedLinkItems;
            }

            return linkItemColl;
        }

        /// <summary>
        /// Gets the ImageListItemCss index for selected css based on subscription type and 
        /// whether user want's to become a Di gold member or not
        /// </summary>
        /// <returns></returns>
        private int GetActiveIndex()
        {
            int index = 0;
            bool becomeDiGoldMember = ((SubscriptionFlow)Page).BecomeDiGoldMember;
            switch (((SubscriptionFlow)Page).SubscriptionType)
            {
                case SubscriptionType.TypeOfSubscription.DiPremium:
                    index = !becomeDiGoldMember ? 0 : 1;
                    break;
                case SubscriptionType.TypeOfSubscription.Di:
                    index = !becomeDiGoldMember ? 2 : 3;
                    break;
                case SubscriptionType.TypeOfSubscription.DiDirectDebit:
                    index = !becomeDiGoldMember ? 4 : 5;
                    break;
                case SubscriptionType.TypeOfSubscription.DiWeekend:
                    index = 6;
                    break;
                case SubscriptionType.TypeOfSubscription.DiPlus:
                    index = !becomeDiGoldMember ? 7 : 8;
                    break;
                default:
                    index = 0;
                    break;
            }
            return index;
        }
       
        #endregion
    }
}