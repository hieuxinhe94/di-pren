using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DIClassLib.BusinessCalendar
{
    public class Group
    {
        public int GroupId;
        public string GroupName;
        public List<TypeSubType> TypesSubTypes;

        public Group(int groupId, string groupName)
        {
            GroupId = groupId;
            GroupName = groupName;
        }
    }
}
