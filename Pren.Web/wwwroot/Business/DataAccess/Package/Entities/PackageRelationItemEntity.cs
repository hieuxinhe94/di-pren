using System;

namespace Pren.Web.Business.DataAccess.Package.Entities
{
    public class PackageRelationItemEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool WildcardBefore { get; set; }
        public string ConditionBefore{ get; set; }
        public bool WildcardAfter{ get; set; }
        public string ConditionAfter { get; set; }
        public DateTime Created { get; set; }
    }
}