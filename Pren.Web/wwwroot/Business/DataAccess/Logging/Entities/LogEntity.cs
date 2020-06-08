using System;

namespace Pren.Web.Business.DataAccess.Logging.Entities
{
    public class LogEntity
    {
        public int Id { get; set; }
        public string Source { get; set; }
        public string Description { get; set; }
        public string Exception { get; set; }
        public DateTime Date { get; set; }
    }
}