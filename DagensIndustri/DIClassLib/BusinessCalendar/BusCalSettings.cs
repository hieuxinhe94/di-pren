using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace DIClassLib.BusinessCalendar
{
    public static class BusCalSettings
    {
        public static int DaysBackMillistreamCompEvents { get { return -7; } }
        public static int DaysFwdMillistreamCompEvents  { get { return 90; } }    

        public static int DiCompanyId           { get { return -1; } }
        public static string DiCompanyIsin      { get { return "-1"; } }
        public static string DiCompanyName      { get { return "Dagens industri"; } }


        public static int GrIdUtdelningar   { get { return 1; } }
        public static int GrIdStammor       { get { return 2; } }
        public static int GrIdKapmarkndagar { get { return 3; } }
        public static int GrIdRapporter     { get { return 4; } }
        public static int GrIdEmissioner    { get { return 5; } }
        public static int GrIdSplit         { get { return 6; } }
        public static int GrIdDi            { get { return 7; } }

        public static int? DiTypeIdConferece { get { return -1; } }
        public static int? DiTypeIdGasell    { get { return -2; } }
        public static int? DiTypeIdEvent     { get { return -3; } }
    }
}
