using System;

namespace Di.Common.Conversion.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class QueryStringAttribute : Attribute
    {
        public string Key { get; set; } 
    }
}
