using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DIClassLib.BusinessCalendar
{
    
    /// <summary>
    /// Most events are fetched from Millistream, but some are DI events.
    /// DI events: companyId=-1, typeId=-1, subTypeId=1 (konferens) OR subTypeId=2 (gasell) OR subTypeId=3 (events).
    /// </summary>
    public class CompanyEvent
    {
        public string EventId
        {
            get { return Guid.NewGuid().ToString(); }
        }
        
        public int CompanyId;
        public string CompanyName;
        public int? TypeId;
        public int? SubTypeId;
        
        public DateTime DateEvent;

        //displayed in outlook heading
        public string Summary               
        {
            get
            {
                if (CompanyId == BusCalSettings.DiCompanyId)
                    return BusCalHandler.GetTypeName(TypeId, null) + ", " + _diMetaInfo;

                return CompanyName + ", " + BusCalHandler.GetTypeName(TypeId, SubTypeId);
            }
        }
        
        public string Description = "";     //displayed in outlook detail view
        private string _diMetaInfo = "";    //conference=name, gasell=city, event=categorytext
        


        public CompanyEvent() { }

        public CompanyEvent(int companyId, string companyName, int? typeId, int? subTypeId, DateTime dateEvent, string description, string diMetaInfo)
        {
            CompanyId = companyId;
            CompanyName = companyName;
            TypeId = typeId;
            SubTypeId = subTypeId;
            DateEvent = dateEvent;
            Description = description;
            _diMetaInfo = diMetaInfo;
        }

    }
}
