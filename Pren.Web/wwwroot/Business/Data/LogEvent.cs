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
    
    public partial class LogEvent
    {
        public int id { get; set; }
        public int eventId { get; set; }
        public Nullable<long> cusno { get; set; }
        public bool isEventSuccess { get; set; }
        public string fileName { get; set; }
        public System.DateTime dateSaved { get; set; }
    }
}
