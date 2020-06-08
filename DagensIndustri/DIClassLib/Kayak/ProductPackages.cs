using System;
using System.Collections.Generic;

namespace DIClassLib.Kayak
{
    [Serializable]
    public class ProductPackages
    {
        //Not able to deserialize if Table is of type List<ProductPackage>?! Refactor in future .Net version
        public List<Dictionary<string, string>> Table { get; set; }

    }
}