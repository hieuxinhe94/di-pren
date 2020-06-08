using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using EPiServer;
using EPiServer.PlugIn;
using EPiServer.ServiceLocation;
using Pren.Web.Business.DataAccess;
using Pren.Web.Business.DataAccess.Package;
using Pren.Web.Business.DataAccess.Package.Entities;
using Pren.Web.Business.Package;

namespace Pren.Web.Tools.Admin.PackageRelation
{
    [GuiPlugIn(Area = PlugInArea.AdminMenu, Description = "Administrera package relationer", RequiredAccess = EPiServer.Security.AccessLevel.Administer, DisplayName = "Package relationer", UrlFromUi = "/Tools/Admin/PackageRelation/PackageRelation.aspx", SortIndex = 1040)]
    public partial class PackageRelation : System.Web.UI.Page
    {
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);

            // Use EPiServer UI master page
            MasterPageFile = UriSupport.ResolveUrlFromUIBySettings("MasterPages/EPiServerUI.master");
        }

        #region Relation types

        protected void DdlRelationTypesChanged(object sender, EventArgs e)
        {
            PhRelationItems.Visible = false;
        }

        #endregion

        #region Relation list

        protected void BtnAddRelationListClick(object sender, EventArgs e)
        {
            var packageListHandler = ServiceLocator.Current.GetInstance<IPackageRelationListDataHandler>();

            packageListHandler.AddPackageRelationList(TxtAddName.Text, int.Parse(DdlRelationTypes.SelectedValue));

            TxtAddName.Text = string.Empty;

            LbRelationLists.DataBind();

            PhRelationItems.Visible = false;
        }

        protected void BtnDeleteRelationListClick(object sender, EventArgs e)
        {
            var packageListHandler = ServiceLocator.Current.GetInstance<IPackageRelationListDataHandler>();

            packageListHandler.DeletePackageRelationList(int.Parse(LbRelationLists.SelectedValue));

            LbRelationLists.DataBind();

            PhRelationItems.Visible = false;
        }

        protected void LbRelationListsChanged(object sender, EventArgs e)
        {
            PhRelationItems.Visible = true;
            SetInfoText();
        }

        #endregion

        #region Relation item

        protected void BtnAddRelationItemClick(object sender, EventArgs e)
        {
            var repos = new RelationItemsRepository();
            repos.AddRelationItem(int.Parse(LbRelationLists.SelectedValue), TxtAddItem.Text, true, string.Empty, true, string.Empty);

            TxtAddItem.Text = string.Empty;

            SetInfoText();

            GvRelationItems.DataBind();            
        }

        protected void GvRelationItemsDeleted(object sender, GridViewDeletedEventArgs e)
        {
            SetInfoText();
        }

        protected void GvRelationItemsUpdated(object sender, GridViewUpdatedEventArgs e)
        {
            SetInfoText();
        }

        #endregion

        private void SetInfoText()
        {
            var packageRelationManager = ServiceLocator.Current.GetInstance<IPackageRelationManager>();

            const string info = "Relationer för <strong>{0}</strong> och package <strong>{1}</strong><br/><br/>Parameter som skickas till S+ <strong>{2}</strong>";

            PackageRelationTypeEnum typeEnum;

            Enum.TryParse(DdlRelationTypes.SelectedValue, out typeEnum) ;

            LblInfo.Text = string.Format(info, DdlRelationTypes.SelectedItem.Text, LbRelationLists.SelectedItem.Text,
                packageRelationManager.GetParameters(typeEnum, LbRelationLists.SelectedItem.Text));
        }
    }

    public class RelationItemsRepository
    {
        private readonly IDataAccess _dataAccess;

        public RelationItemsRepository()
        {
            _dataAccess = ServiceLocator.Current.GetInstance<IDataAccess>();
        }

        public IEnumerable<PackageRelationItemEntity> GetRelationItems(int relationListId)
        {
            return _dataAccess.PackageRelationItemDataHandler.GetPackageRelationItems(relationListId);
        }

        public void UpdateRelationItem(int id, string name)
        {
            _dataAccess.PackageRelationItemDataHandler.UpdatePackageRelationItem(id, name);
        }

        public void DeleteRelationItem(int id)
        {
            _dataAccess.PackageRelationItemDataHandler.DeletePackageRelationItem(id);
        }

        public void AddRelationItem(int packageRelationListId, string name, bool wildCardBefore, string conditionBefore, bool wildCardAfter,
            string conditionAfter)
        {
            _dataAccess.PackageRelationItemDataHandler.AddPackageRelationItem(packageRelationListId, name, wildCardBefore, conditionBefore, wildCardAfter, conditionAfter);
        }

        public IEnumerable<PackageRelationEntity> GetRelationTypes()
        {
            return _dataAccess.PackageRelationDataHandler.GetAllPackageRelations().OrderBy(t => t.Name);            
        }

        public IEnumerable<PackageRelationListEntity> GetRelationLists(int relationTypeId)
        {
            var packageListHandler = ServiceLocator.Current.GetInstance<IPackageRelationListDataHandler>();

            return packageListHandler.GetPackageRelationLists(relationTypeId);
        }
    }
}