using System;

namespace Pren.Web.Business.DataAccess.CodePortal.Entities
{
    public class CodeEntity
    {
        public int Id { get; set; }
        public int ListId { get; set; }
        public string Code { get; set; }
        public DateTime? UsedTime { get; set; }
        public string UsedById { get; set; }
        public string UsedByEmail { get; set; }
    }
}
