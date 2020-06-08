using System.Collections.Generic;
using EPiServer.ServiceLocation;
using EPiServer.Shell.ObjectEditing;
using Pren.Web.Business.DataAccess;

namespace Pren.Web.Business.CustomProperties
{
    class CampaignUspSelectionFactory : ISelectionFactory
    {       
        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            var uspHandler = ServiceLocator.Current.GetInstance<IDataAccess>().UspHandler;

            var uspProducts = uspHandler.GetUspProducts();

            foreach (var uspProduct in uspProducts)
            {
                yield return new SelectItem
                {
                    Text = uspProduct.Text,
                    Value = uspProduct.Id
                };
            }
        }
    }
}
