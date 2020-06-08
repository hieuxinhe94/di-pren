using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace Di.Common.EndPointBehaviours
{
    public class RequestInspectorBehavior : IEndpointBehavior
    {
        private readonly MessageInspector _messageInspector = new MessageInspector();

        public string LastRequestXml
        {
            get
            {
                return _messageInspector.LastRequestXml;
            }
        }

        public string LastResponseXml
        {
            get
            {
                return _messageInspector.LastResponseXml;
            }
        }

        public void AddBindingParameters(ServiceEndpoint endpoint, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {

        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {

        }

        public void Validate(ServiceEndpoint endpoint)
        {

        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            clientRuntime.MessageInspectors.Add(_messageInspector);
        }
    }
}
