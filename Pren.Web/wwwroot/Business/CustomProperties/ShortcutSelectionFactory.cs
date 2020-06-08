using System.Collections.Generic;
using EPiServer.Shell.ObjectEditing;

namespace Pren.Web.Business.CustomProperties
{
    public class ShortcutSelectionFactory : ISelectionFactory
    {
        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            return new []
            {
                new SelectItem { Text = "Uppehåll", Value = "subssleep" }, 
                new SelectItem { Text = "Tillfällig adressändring", Value = "tmpaddresschange" },
                new SelectItem { Text = "Permanent adressändring", Value = "permaddresschange" },
                new SelectItem { Text = "Reklamation", Value = "reclaim" }
            };
        }
    }
}
