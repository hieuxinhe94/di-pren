using System;
using System.Text;

namespace Di.Common.Logging
{
    public abstract class Loggable
    {
        public string ToLogString()
        {
            try
            {
                var sb = new StringBuilder();

                foreach (var propertyInfo in GetType().GetProperties())
                {
                    sb.AppendLine(String.Format("{0}: '{1}'", propertyInfo.Name, propertyInfo.GetValue(this, null) ?? "[NULL]"));
                }

                return sb.ToString();
            }
            catch
            {
                return "ToLogString() error";
            }
        }
    }
}
