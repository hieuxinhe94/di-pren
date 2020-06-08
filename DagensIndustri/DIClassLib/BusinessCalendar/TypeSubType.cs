using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DIClassLib.BusinessCalendar
{
    public class TypeSubType
    {
        public int? TypeId;
        public int? SubTypeId;
        public string TypeName;

        public TypeSubType(int? typeId, int? subTypeId, string typeName)
        {
            TypeId = typeId;
            SubTypeId = subTypeId;
            TypeName = typeName;
        }
    }
}
