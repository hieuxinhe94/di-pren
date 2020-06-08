using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using EPiServer;
using DagensIndustri.Tools.Classes;

namespace DagensIndustri.Templates.Public.Units.Placeable
{
    public partial class Paging : UserControlBase
    {
        #region Properties
        public Control ParentControl { get; set; }

        public int TotalNumberOfHits
        {
            get
            {
                return Convert.ToInt32(ViewState["TotalNumberOfHits"]);
            }
            set
            {
                ViewState["TotalNumberOfHits"] = value;
            }
        }

        public int CurrentIndex
        {
            get
            {
                return Convert.ToInt32(ViewState["CurrentIndex"]);
            }
            set
            {
                ViewState["CurrentIndex"] = value;
            }
        }

        public int NoOfHitsPerPage { get; set; }
        #endregion

        #region Events
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        private void LinkButton_Click(object sender, EventArgs e)
        {
            LinkButton lnk = sender as LinkButton;

            if (lnk.ID.Contains("Previous"))
                CurrentIndex = CurrentIndex - 1;
            else if (lnk.ID.Contains("Next"))
                CurrentIndex = CurrentIndex + 1;
            else
                CurrentIndex = int.Parse(lnk.Text);

            EPiFunctions.PopulateWithPaging(ParentControl);
        }
    
        #endregion

        #region Methods
        /// <summary>
        /// Create a pagin control
        /// </summary>
        public void CreatePagingControl()
        {
            PagingPlaceHolder.Controls.Clear();

            //Create the prev item
            if (CurrentIndex > 1 && TotalNumberOfHits > 1)
            {
                var liPrevCtrl = new HtmlGenericControl("li");
                liPrevCtrl.Attributes.Add("class", "prev");

                var lnk = CreateLinkButton("Previous", "Föregående");

                liPrevCtrl.Controls.Add(lnk);
                PagingPlaceHolder.Controls.Add(liPrevCtrl);
            }

            //Create all the paging number items
            for (int i = 0; i < Math.Ceiling(TotalNumberOfHits / (float)NoOfHitsPerPage); i++)
            {
                var liCtrl = new HtmlGenericControl("li");

                //Current index should not be a link.
                if (i == CurrentIndex - 1)
                {
                    liCtrl.Attributes.Add("class", "current");
                    var spanCtrl = new HtmlGenericControl("span");
                    spanCtrl.InnerText = (i + 1).ToString();

                    liCtrl.Controls.Add(spanCtrl);
                }
                else
                {
                    var lnk = CreateLinkButton((i + 1).ToString(), (i + 1).ToString());
                    liCtrl.Controls.Add(lnk);
                }

                PagingPlaceHolder.Controls.Add(liCtrl);
            }

            //Create the "Next" item
            if (TotalNumberOfHits > 1 && CurrentIndex < Math.Ceiling(TotalNumberOfHits / (float)NoOfHitsPerPage))
            {
                var liNextCtrl = new HtmlGenericControl("li");
                liNextCtrl.Attributes.Add("class", "next");

                var lnk = CreateLinkButton("Next", "Nästa");

                liNextCtrl.Controls.Add(lnk);
                PagingPlaceHolder.Controls.Add(liNextCtrl);
            }
        }

        /// <summary>
        /// Create a link button.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        private Control CreateLinkButton(string id, string text)
        {
            var linkBtn = new LinkButton();
            linkBtn.Click += new EventHandler(LinkButton_Click);
            linkBtn.ID = "lnkPage" + id;
            linkBtn.Text = text;

            return linkBtn;
        }

        /// <summary>
        /// Clear the paging area from old results
        /// </summary>
        public void Clear()
        {
            PagingPlaceHolder.Controls.Clear();
        }
        #endregion
    }
}