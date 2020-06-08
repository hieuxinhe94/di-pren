using System.Collections.Generic;

namespace DIClassLib.Kayak
{
    public class ProductPackage
    {
        public string PackageId { get; set; }
        public string PackageName { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string PaperName { get; set; }
        public string ProductName { get; set; }
        public string Total { get; set; }
        public List<PackageProduct> Products { get; set; }
    }
}