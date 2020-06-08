using System.Collections.Generic;
using EPiServer.ServiceLocation;
using EPiServer.Shell.ObjectEditing;
using Pren.Web.Business.DataAccess;

namespace Pren.Web.Business.CustomProperties
{
    class CodePortalListSelectionFactory : ISelectionFactory
    {
        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            var codeListHandler = ServiceLocator.Current.GetInstance<IDataAccess>().CodePortalListDataHandler;

            var codeLists = codeListHandler.GetAllCodeLists();

            foreach (var codeList in codeLists)
            {
                yield return new SelectItem
                {
                    Text = codeList.Name + " " + codeList.ValidFrom.ToString("yyyy-MM-dd") + " - " + codeList.ValidTo.ToString("yyyy-MM-dd"),
                    Value = codeList.Id
                };
            }
        }
    }
}
