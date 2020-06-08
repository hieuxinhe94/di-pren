using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DagensIndustri.Templates.Public.Pages.UserSettings
{
    interface IUserSettingsPage
    {
        void ShowMessageFromChildCtrl(string mess, bool isKey, bool isError);
    }
}
