using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace DIClassLib.Antipodes
{
    public class WineItem
    {
        public int Id = -1;
        public bool IsActive = false;
        public DateTime DateModified { get; set; }
        public string Name { get; set; }
        public string Descr { get; set; }
        public string Type { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
        public int BottleSize { get; set; }
        public string Unit { get; set; }
        public string Alcohol { get; set; }
        public string ImagePath { get; set; }

        public WineItem() {}

    }
}
