using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;


namespace DIClassLib.DbHelpers
{
    public static class DbHelpMethods
    {

        public static DateTime SetDateFromDbFieldName(DataRow dr, string fieldName)
        {
            DateTime dt;
            if (DateTime.TryParse(dr[fieldName].ToString(), out dt))
                return dt;

            return DateTime.MinValue;
        }

        public static bool DataSetHasRows(DataSet ds)
        {
            try
            {
                if (ds.Tables[0].Rows.Count > 0)
                    return true;
            }
            catch
            {
            }

            return false;
        }

        public static T ValueIfColumnExist<T>(DataTable table, DataRow dr, string columnName, T defaultReturnValue)
        {
            try
            {
                return table.Columns.Contains(columnName) && dr[columnName] != DBNull.Value
                    ? (T) Convert.ChangeType(dr[columnName], typeof (T))
                    : defaultReturnValue;
            }
            catch (Exception ex)
            {
                new Logger("DbHelpMethods.ValueIfColumnExist() failed on columnName: " + columnName, ex.Message);
                return defaultReturnValue;
            }
        }
    }
}