using System.Collections.Generic;
using Di.Subscription.Logic.Parameters.Retrievers;
using EPiServer.ServiceLocation;
using EPiServer.Shell.ObjectEditing;

namespace Pren.Web.Business.CustomProperties
{
    class ReceiveTypeListSelectionFactory : ISelectionFactory
    {
        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            var parameterRetriver = ServiceLocator.Current.GetInstance<IParametersRetriever>(); 

            var receiveTypes = parameterRetriver.GetAllReceiveTypes(); 

            foreach (var receiveType in receiveTypes)
            {
                yield return new SelectItem
                {
                    Text = receiveType.Name + " (" + receiveType.Id + ")",
                    Value = receiveType.Id
                };
            }
        }
    }
}
