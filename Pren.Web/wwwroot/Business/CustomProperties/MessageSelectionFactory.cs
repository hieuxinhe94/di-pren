using System.Collections.Generic;
using EPiServer.Shell.ObjectEditing;

namespace Pren.Web.Business.CustomProperties
{
    class MessageSelectionFactory : ISelectionFactory
    {       

        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {

            return new ISelectItem[] { new SelectItem { Text = "Vit", Value = "1" }, new SelectItem { Text = "Röd", Value = "3" }, new SelectItem { Text = "Grön", Value = "0" } };

        }
    }
}
