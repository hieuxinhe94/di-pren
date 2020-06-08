namespace Pren.Web.Business.Messaging
{
    public class Message
    {
        public Message()
        {
        }

        public Message(string body, MessageType type = MessageType.Info)
        {
            Body = body;
            Type = type;
        }

        public string Body { get; set; }

        public MessageType Type { get; set; }

        public string CssClass
        {
            get
            {
                switch (Type)
                {
                    case MessageType.Success:
                        return "alert-success";                        
                    case MessageType.Warning:
                        return "alert-warning";
                    case MessageType.Danger:
                        return "alert-danger";
                    default:
                        return "alert-info";
                }
            }
        }
    }

    public enum MessageType
    {
        Success,
        Info,
        Warning,
        Danger
    }

}