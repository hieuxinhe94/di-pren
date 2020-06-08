using System.ServiceModel.Dispatcher;

namespace Di.Common.EndPointBehaviours
{
    public class MessageInspector : IClientMessageInspector
    {
        public string LastRequestXml { get; private set; }
        public string LastResponseXml { get; private set; }

        public void AfterReceiveReply(ref System.ServiceModel.Channels.Message reply, object correlationState)
        {
            LastResponseXml = reply.ToString();
        }

        public object BeforeSendRequest(ref System.ServiceModel.Channels.Message request, System.ServiceModel.IClientChannel channel)
        {
            LastRequestXml = request.ToString();
            return request;
        }
    }
}
