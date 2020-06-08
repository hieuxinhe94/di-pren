using System.Reflection;

namespace Di.Common.Conversion
{
    public class PropertyUtilities : IPropertyUtilities
    {
        public PropertyInfo[] GetPropertyInfos<T>(T obj)
        {
            return obj.GetType().GetProperties();
        }
    }
}
