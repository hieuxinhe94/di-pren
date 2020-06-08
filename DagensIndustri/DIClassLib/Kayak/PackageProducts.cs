using System.Collections.Generic;

namespace DIClassLib.Kayak
{
    public class PackageProducts
    {
        //Not able to deserialize if Table is of type List<PackageProduct>?! Refactor in future .Net version
        public List<Dictionary<string, string>> Table { get; set; }
    }
}