//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Pren.Web.Business.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class PackageRelation
    {
        public PackageRelation()
        {
            this.PackageRelationList = new HashSet<PackageRelationList>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public System.DateTime Created { get; set; }
    
        public virtual ICollection<PackageRelationList> PackageRelationList { get; set; }
    }
}
