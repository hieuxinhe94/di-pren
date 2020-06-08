using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DIClassLib.Subscriptions.CirixMappers;


namespace DagensIndustri.Templates.Public.Units.Placeable.AddressForm
{
    [Serializable]
    public class ObjHolderForAddressFormClick
    {
        /// <summary>
        /// values: insert update delete
        /// </summary>
        public string SqlCommand;

        public AddressMap AddressOrg;

        
        public ObjHolderForAddressFormClick() { }

        public ObjHolderForAddressFormClick(string sqlCommand, AddressMap addressOrg)
        {
            SqlCommand = sqlCommand;
            AddressOrg = addressOrg;
        }

    }
}