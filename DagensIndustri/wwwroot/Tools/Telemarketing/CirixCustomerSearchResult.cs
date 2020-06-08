using System;
using System.Collections.Generic;
using System.Linq;

namespace DagensIndustri.Tools.Telemarketing
{
    public class CirixCustomerSearchResult
    {
        public int CirixCustomerSearchCount { get; set; }

        public CirixCustomer CirixCustomer { get; set; }

        public List<CirixCustomer> CirixCustomers { get; set; }
    }
}