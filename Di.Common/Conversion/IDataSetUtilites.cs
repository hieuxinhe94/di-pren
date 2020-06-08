using System.Data;

namespace Di.Common.Conversion
{
    public interface IDataSetUtilites
    {
        T GetDataRowValue<T>(DataRow dataRow, string columnName);
        bool HasRows(DataSet dataSet);
    }
}
