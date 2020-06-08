using System.Collections.Generic;
using EPiServer.Shell.ObjectEditing;

namespace Pren.Web.Business.CustomProperties
{
    public class SubsKindSelectionFactory : ISelectionFactory
    {
        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            return new []
            {
                new SelectItem { Text = "Avgörs av Kayak", Value = "0" },
                new SelectItem { Text = "Tillsvidare (01)", Value = "01" },
                new SelectItem { Text = "Tidsbestämd (02)", Value = "02" }
            };
        }
    }
}
