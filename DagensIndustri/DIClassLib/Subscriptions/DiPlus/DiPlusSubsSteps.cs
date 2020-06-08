using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;


namespace DIClassLib.Subscriptions.DiPlus
{
    public class DiPlusSubsSteps
    {

        public enum Steps
        {
            [DescriptionAttribute("_1_SelectSubsType")]
            _1_SelectSubsType = 0,
            [DescriptionAttribute("_2_PersonForm")]
            _2_PersonForm,
            [DescriptionAttribute("_3_ThankYou")]
            _3_ThankYou,
            [DescriptionAttribute("_4_Error")]
            _4_Error
        }

    }
}
