using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using Di.Common.Conversion.Attributes;
using Di.Common.Conversion.Types;

namespace Di.Common.Conversion
{
    public class ObjectConverter : IObjectConverter
    {
        private readonly IDataSetUtilites _dataSetUtilites;
        private readonly IPropertyUtilities _propertyUtilities;

        public ObjectConverter(IDataSetUtilites dataSetUtilites, IPropertyUtilities propertyUtilities)
        {
            _dataSetUtilites = dataSetUtilites;
            _propertyUtilities = propertyUtilities;
        }

        public T ConvertFromQueryString<T>(HttpRequestBase request) where T : IQueryStringObject, new()
        {
            var convertedObject = new T();

            if (request != null)
            {
                var propertyInfos = _propertyUtilities.GetPropertyInfos(new T());

                foreach (var propertyInfo in propertyInfos)
                {
                    var attr = (QueryStringAttribute)Attribute.GetCustomAttribute(propertyInfo, typeof(QueryStringAttribute), false);
                    if (attr == null)
                    {
                        continue;
                    }

                    switch (Type.GetTypeCode(propertyInfo.PropertyType))
                    {
                        case TypeCode.Decimal:
                            propertyInfo.SetValue(convertedObject, GetRequestQueryStringValue<decimal>(request, attr.Key));
                            break;
                        case TypeCode.Int32:
                            propertyInfo.SetValue(convertedObject, GetRequestQueryStringValue<int>(request, attr.Key));
                            break;
                        case TypeCode.Int64:
                            propertyInfo.SetValue(convertedObject, GetRequestQueryStringValue<long>(request, attr.Key));
                            break;
                        case TypeCode.String:
                            propertyInfo.SetValue(convertedObject, GetRequestQueryStringValue<string>(request, attr.Key));
                            break;
                        case TypeCode.Boolean:
                            propertyInfo.SetValue(convertedObject, GetRequestQueryStringValue<bool>(request, attr.Key));
                            break;
                        case TypeCode.DateTime:
                            propertyInfo.SetValue(convertedObject, GetRequestQueryStringValue<DateTime>(request, attr.Key));
                            break;
                    }
                }
            }

            return convertedObject;
        }

        public IEnumerable<T> ConvertFromDataSet<T>(DataSet dataSet) where T : IDataSetObject, new()
        {
            var list = new List<T>();

            if (_dataSetUtilites.HasRows(dataSet))
            {
                var properties = _propertyUtilities.GetPropertyInfos(new T());
                list.AddRange(from DataRow row in dataSet.Tables[0].Rows select ConvertFromDataRow<T>(row, properties));
            }

            return list;
        }

        private T ConvertFromDataRow<T>(DataRow dataRow, IEnumerable<PropertyInfo> propertyInfos) where T : IDataSetObject, new()
        {
            var convertedObject = new T();

            foreach (var propertyInfo in propertyInfos)
            {
                //todo: cache custom attr reflection
                var attr = (DataSetAttribute)Attribute.GetCustomAttribute(propertyInfo, typeof(DataSetAttribute), false);
                if (attr == null)
                {
                    continue;
                }

                switch (Type.GetTypeCode(propertyInfo.PropertyType))
                {
                    case TypeCode.Decimal:
                        propertyInfo.SetValue(convertedObject, _dataSetUtilites.GetDataRowValue<decimal>(dataRow, attr.ColumnName));
                        break;
                    case TypeCode.Int32:
                        propertyInfo.SetValue(convertedObject, _dataSetUtilites.GetDataRowValue<int>(dataRow, attr.ColumnName));
                        break;
                    case TypeCode.Int64:
                        propertyInfo.SetValue(convertedObject, _dataSetUtilites.GetDataRowValue<long>(dataRow, attr.ColumnName));
                        break;
                    case TypeCode.String:
                        propertyInfo.SetValue(convertedObject, _dataSetUtilites.GetDataRowValue<string>(dataRow, attr.ColumnName));
                        break;
                    case TypeCode.Boolean:
                        propertyInfo.SetValue(convertedObject, _dataSetUtilites.GetDataRowValue<bool>(dataRow, attr.ColumnName));
                        break;
                    case TypeCode.DateTime:
                        propertyInfo.SetValue(convertedObject, _dataSetUtilites.GetDataRowValue<DateTime>(dataRow, attr.ColumnName));
                        break;
                }
            }

            return convertedObject;
        }

        private T GetRequestQueryStringValue<T>(HttpRequestBase request, string key)
        {
            var value = request.QueryString[key];

            if (string.IsNullOrEmpty(value))
            {
                return default(T);
            }

            return (T)Convert.ChangeType(value, typeof(T));
        }

    }
}
