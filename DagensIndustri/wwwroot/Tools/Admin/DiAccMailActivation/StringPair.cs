using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApsisMailSort.ClassLibrary
{
    //Use this class to populate ddl's.
    public class StringPair
    {
        public string Text;
        public string Value;

        public StringPair(string text, string value)
        {
            Text = text;
            Value = value;
        }
    }
}