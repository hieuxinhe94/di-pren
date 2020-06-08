using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DIClassLib.StudentVerification;

namespace DagensIndustri.Tools.Admin.Student
{
    public partial class StudentVerifierBox : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnVerifyStudent_Click(object sender, EventArgs e)
        {
            if (tbSocialSecNo.Text.Length != 10)
                lbResult.Text = "Ange korrekt födelsenr";
            else
                lbResult.Text = VerifyStudent(tbSocialSecNo.Text);

            divResult.Visible = true;
        }

        private string VerifyStudent(string socialSecNo)
        {
            StudentVerifier sv = new StudentVerifier();
            var result = Regex.Replace(sv.VerifyByBirthNum(socialSecNo.Trim()), "<.*?>", string.Empty);
            var nResult = -1;
            if (Int32.TryParse(result, out nResult))
            {
                // 1=full time student, 2=part time student, 0=not student, 99=bad/missing input, -1=local exception
                // 1=full time student, 0=not full time student, 99=bad/missing input, -1=local exception
                switch (nResult)
                {
                    case 0:
                        return "Ej heltidsstudent";
                    case 1:
                        return "Heltidsstudent";
                    //case 2:
                    //    return "Deltidsstudent";
                    case 99:
                        return "Felaktigt indata";
                    default:
                        return "Systemfel (" + nResult + ")";
                }
            }
            else
            {
                return "Systemfel (" + result + ")";
            }
        }

    }
}