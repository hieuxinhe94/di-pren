using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;


namespace DIClassLib.Misc
{
    public class ValidationError : IValidator
    {
        public ValidationError(string message)
        {
            ErrorMessage = message;
        }

        public string ErrorMessage { get; set; }

        public bool IsValid
        {
            get { return false; }
            set { }
        }

        public void Validate() { }
    }
}
