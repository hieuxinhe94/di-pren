using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace DagensIndustri.Tools.Telemarketing
{
    public static class TelemarketingHelper
    {
        public static IEnumerable<ProductConfig> GetProducts()
        {
            var productsAppsettingsString = ConfigurationManager.AppSettings["ProductsForTelemarketingAdmin"];
            var productDefinitions = productsAppsettingsString.Split(',');
            return productDefinitions.Select(productDefinition => productDefinition.Split('|')).Select(info => new ProductConfig() { PaperCode = info[0], ProductNo = info[1], TargetGroup = info[2] }).ToList();
        }
    }
}