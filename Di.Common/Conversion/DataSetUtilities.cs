using System;
using System.Data;

namespace Di.Common.Conversion
{
    public class DataSetUtilities : IDataSetUtilites
    {
        public T GetDataRowValue<T>(DataRow dataRow, string columnName)
        {
            if (dataRow == null || !dataRow.Table.Columns.Contains(columnName) || dataRow[columnName] == null || dataRow[columnName] == DBNull.Value)
            {
                return default(T);
            }

            if (typeof(T) == typeof(bool))
            {
                if (dataRow[columnName].ToString() == "Y")
                {
                    return (T)(object)(true);
                }
                if (dataRow[columnName].ToString() == "N")
                {
                    return (T)(object)(false);
                }
            }

            return (T)Convert.ChangeType(dataRow[columnName], typeof(T));
        }

        public bool HasRows(DataSet dataSet)
        {
            return dataSet != null
                   && dataSet.Tables[0] != null
                   && dataSet.Tables[0].Rows != null;
        }
    }
}
