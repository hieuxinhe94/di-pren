using System.Reflection;

namespace Di.Common.Conversion
{
    public interface IPropertyUtilities
    {
        PropertyInfo[] GetPropertyInfos<T>(T obj);
    }
}
