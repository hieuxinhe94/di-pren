using System;
using System.Data;
using System.Linq;
using Di.Common.Conversion;
using Di.Common.Conversion.Attributes;
using Di.Common.Conversion.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Di.Common.UnitTests.Conversion
{
    [TestClass]
    public class ObjectConverterUnitTests
    {
        [TestMethod]
        public void ConvertFromDataSet_SupportedDataTypes_GetsConvertedToObjectProps()
        {
            var converter = new ObjectConverter(new DataSetUtilities(), new PropertyUtilities());

            var convertedObject = converter.ConvertFromDataSet<TypedDatasetObject>(CreateFakeDataSet()).First();

            Assert.AreEqual((decimal)1.1, convertedObject.DecimalValue);
            Assert.AreEqual(1, convertedObject.IntValue);
            Assert.AreEqual(1, convertedObject.LongValue);
            Assert.AreEqual("string", convertedObject.StringValue);
            Assert.AreEqual(true, convertedObject.BoolValue);
            Assert.AreEqual(new DateTime(2015, 01, 01), convertedObject.DateTimeValue);
        }

        private DataSet CreateFakeDataSet()
        {
            var dt = new DataTable();
            dt.Columns.Add("DECIMAL");
            dt.Columns.Add("INTEGER");
            dt.Columns.Add("LONG");
            dt.Columns.Add("STRING");
            dt.Columns.Add("BOOL");
            dt.Columns.Add("DATETIME");

            var ds = new DataSet();
            ds.Tables.Add(dt);

            var row = ds.Tables[0].NewRow();
            row["DECIMAL"] = 1.1;
            row["INTEGER"] = 1;
            row["LONG"] = 1;
            row["STRING"] = "string";
            row["BOOL"] = true;
            row["DATETIME"] = new DateTime(2015, 01, 01);
            ds.Tables[0].Rows.Add(row);

            return ds;
        }
    }

    internal class TypedDatasetObject : IDataSetObject
    {
        [DataSet(ColumnName = "DECIMAL")]
        public decimal DecimalValue { get; set; }

        [DataSet(ColumnName = "INTEGER")]
        public int IntValue { get; set; }

        [DataSet(ColumnName = "LONG")]
        public long LongValue { get; set; }

        [DataSet(ColumnName = "STRING")]
        public string StringValue { get; set; }

        [DataSet(ColumnName = "BOOL")]
        public bool BoolValue { get; set; }

        [DataSet(ColumnName = "DATETIME")]
        public DateTime DateTimeValue { get; set; }    
    }
}
