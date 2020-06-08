using EPiServer;
using EPiServer.Core;
using System.Web.UI;

namespace DagensIndustri.Tools.Classes.BaseClasses
{
    public class DiUserControlBase : EPiServer.UserControlBase
    {
        protected PageData ActualCurrentPage;

        protected override void OnInit(System.EventArgs e)
        {
            base.OnInit(e);

            SetActualPage(this.Parent);

        }

        /// <summary>
        /// If there is a control implementing the IPageSource 
        /// interface higher up in the control hiearchy 
        /// the actualPage will be set to the current page of that control. 
        /// If no such control is found the actualPage will be set to 
        /// the CurrentPage of this control. 
        /// 
        /// If a control is found this would imply that a templated control 
        /// is using this user control (for example a page list). Then we 
        /// are interested of the page currently being iterated in the 
        /// templated control, not the page containing 
        /// the templated control. 
        /// </summary>
        private void SetActualPage(Control control)
        {
            if ((control.GetType().GetInterface("IPageSource") != null))
                ActualCurrentPage = ((IPageSource)control).CurrentPage;
            else if (control.Parent == null)
                ActualCurrentPage = CurrentPage;
            else
                SetActualPage(control.Parent);
        }

        /// <summary>
        /// Gets the startpage pagadata object
        /// </summary>
        protected PageData StartPage
        {
            get
            {
                return GetPage(PageReference.StartPage);
            }
        }

        protected string GetFriendlyUrl(PageData pd)
        {
            return EPiFunctions.GetFriendlyUrl(pd);
        }

        protected string GetFriendlyAbsoluteUrl(PageData pd)
        {
            return EPiFunctions.GetFriendlyAbsoluteUrl(pd);
        }
    }
}