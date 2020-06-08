using System.Collections.Generic;
using EPiServer.Shell.ObjectEditing;

namespace Pren.Web.Business.CustomProperties
{
    class MessageTypeSelectionFactory : ISelectionFactory
    {       

        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {

            return new ISelectItem[] { new SelectItem { Text = "Info", Value = "Info" }, new SelectItem { Text = "Warning", Value = "Warning" }, new SelectItem { Text = "Error", Value = "Error" } };

        }
    }
}
