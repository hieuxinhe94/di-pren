using System.Collections.Generic;
using EPiServer.Shell.ObjectEditing;

namespace Pren.Web.Business.CustomProperties
{
    public class IconSelectionFactory : ISelectionFactory
    {
        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            return new SelectItem[]  { new SelectItem { Text = "Ingen ikon", Value = "0" }, new SelectItem { Text = "Viktigt", Value = "1" } };
        }
    }

    public static class Icon
    {
        public enum IconType
        {
            None,
            Important
        }

        public static string IconCssClass(IconType iconType)
        {
            switch (iconType)
            {
                case IconType.Important:
                    return "fa fa-exclamation-circle icon-important";
                case IconType.None:
                default:
                    return "";
            }
        }
    }
}
