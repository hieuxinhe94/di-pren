using System;

namespace Pren.Web.Business.DataAccess.Logging.Entities
{
    public class LogEmailEntity
    {
        public int Id { get; set; }
        public string ToAddress { get; set; }
        public string FromAddress { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public string Ip { get; set; }
        public string Result { get; set; }
        public DateTime SendDate { get; set; }
    }
}