using System;

namespace Di.Common.Conversion.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DataSetAttribute : Attribute
    {
        public string ColumnName { get; set; } 
    }
}
