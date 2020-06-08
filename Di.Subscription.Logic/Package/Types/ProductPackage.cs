using System;

namespace Di.Subscription.Logic.Package.Types
{
    public class ProductPackage
    {
        public string PackageId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string PaperCode { get; set; }

        public string ProductNumber { get; set; }

        public bool MainProduct { get; set; }
    }
}
