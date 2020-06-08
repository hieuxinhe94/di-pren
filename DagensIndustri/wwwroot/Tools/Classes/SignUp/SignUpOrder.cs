using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DagensIndustri.Tools.Classes.SignUp;
using DagensIndustri.Templates.Public.Pages;
using DIClassLib.DbHandlers;
using DIClassLib.SignUp;

namespace DagensIndustri.Tools.Classes.SignUp
{
    [Serializable]
    public class SignUpOrder
    {
        #region Properties

        public List<int> ListOrderIds;
        public List<SignUpUser> ListUsers = new List<SignUpUser>();
        public int EpiPageId { get; set; }
        public int PayMethod { get; set; }

        #endregion

        
        public SignUpOrder(List<SignUpUser> listUsers, int epiPageId, int payMethod)
        {
            ListUsers = listUsers;
            EpiPageId = epiPageId;
            PayMethod = payMethod;

            string paymentMethodName = string.Empty;

            if (PayMethod == 1)
                paymentMethodName = "Credit Card - NOT OK";
            else if (PayMethod == 2)
                paymentMethodName = "Invoice";


            ListOrderIds = new List<int>();

            int counter = 0;
            int payerId = 0;
            foreach (SignUpUser user in ListUsers)
            {
                Guid code = Guid.NewGuid();
                if (counter == 0)
                {
                    payerId = SignUpPerson.InsertSignUpPerson(0, EpiPageId, 0, paymentMethodName, user.FirstName, user.LastName, user.Address, user.Zip, user.City, user.Phone, user.Email, code);
                    ListOrderIds.Add(payerId);
                }
                else
                    ListOrderIds.Add(SignUpPerson.InsertSignUpPerson(0, EpiPageId, payerId, paymentMethodName, user.FirstName, user.LastName, user.Address, user.Zip, user.City, user.Phone, user.Email, code));

                counter++;
            }

        }
    }
}