using System;

namespace Pren.Web.Business.DataAccess.CodePortal.Entities
{
    public class CodeListEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ResourceId { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public DateTime Created { get; set; }
    }
}
