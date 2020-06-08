using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DIClassLib.BusinessCalendar
{

    public class EventInSubs
    {
        public int CompanyId;
        public int? TypeId;
        public int? SubTypeId;


        public EventInSubs(int companyId, int? typeId, int? subTypeId)
        {
            CompanyId = companyId;
            TypeId = typeId;
            SubTypeId = subTypeId;
        }
    }


}
