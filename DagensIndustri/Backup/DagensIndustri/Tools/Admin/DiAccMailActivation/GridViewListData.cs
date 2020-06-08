using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApsisMailSort.ClassLibrary
{
    //Use this class to create a list with customers containing theese parameters.
    public class GridViewListData
    {
        public string customerId { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string userName { get; set; }
        public string dateSaved { get; set; }
        public string dateUpdated { get; set; }
        public string dateRegularLetter { get; set; }
        public string ApsisUpdateCheckServicePlus { get; set; }
        public string HaveServicePlusAccount { get; set; }
        public string PaperCode { get; set; }
        public string ProductNo { get; set; }
        public string SubsLen_Mons { get; set; }
        public string ProductName { get; set; }
    }
}