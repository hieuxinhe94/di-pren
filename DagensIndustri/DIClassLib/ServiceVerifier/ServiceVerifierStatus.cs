using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DIClassLib.ServiceVerifier
{
    public class ServiceVerifierStatus
    {
        public bool IsValid { get; set; }
        public String Message { get; set; }
        public Exception Exception { get; set; }

        public String ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Status: ");
            
            sb.Append(IsValid);
            sb.Append("<br/>");
            sb.Append("Message: ");
            sb.Append(Message);

            return sb.ToString();
        }
    }
}
