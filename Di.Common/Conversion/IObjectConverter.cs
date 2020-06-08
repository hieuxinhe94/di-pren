using System.Collections.Generic;
using System.Data;
using System.Web;
using Di.Common.Conversion.Types;

namespace Di.Common.Conversion
{
    public interface IObjectConverter
    {
        T ConvertFromQueryString<T>(HttpRequestBase request) where T : IQueryStringObject, new();
        IEnumerable<T> ConvertFromDataSet<T>(DataSet dataSet) where T : IDataSetObject, new();
    }
}
