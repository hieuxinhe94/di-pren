using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DIClassLib.Misc;
using DIClassLib.Subscriptions;


namespace DagensIndustri.Templates.Public.Units.Placeable.AddressForm
{
    public interface IAddress
    {
        void HandleAddressButtonClick(AddressDataHolder dh);
    }
}
