using System;
using DIClassLib.Misc;

namespace DagensIndustri.Templates.Public.Pages
{
    public partial class DeviceRedirect : EPiServer.TemplatePage
    {
        private string UrlForRedirect { get; set; }

 
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            UrlForRedirect = CurrentPage["UrlForDesktop"] as string;

            if (CurrentPage["DependOnOS"] != null)
            {
                DependOnOs();
            }
            else
            {
                DependOnDevice();
            }
            DoRedirect();
        }

        private void DependOnOs()
        {
            var osName = Detection.GetOsName();
            switch (osName)
            {
                case Detection.OsStandardName.Android:
                    UrlForRedirect = CurrentPage["UrlForAndroid"] as string;
                    break;
                case Detection.OsStandardName.iOS:
                    UrlForRedirect = CurrentPage["UrlForiOS"] as string;
                    break;
            }
        }

        private void DependOnDevice()
        {
            var deviceName = Detection.GetDeviceName();
            switch (deviceName)
            {
                case Detection.DeviceName.Mobile:
                    UrlForRedirect = CurrentPage["UrlForMobile"] as string;
                    break;
                case Detection.DeviceName.Tablet:
                    UrlForRedirect = CurrentPage["UrlForTablet"] as string;
                    break;
            }
        }

        private void DoRedirect()
        {
            if (!string.IsNullOrEmpty(UrlForRedirect))
            {
                Response.Status = "301 Moved Permanently";
                Response.AddHeader("Location", UrlForRedirect);
                Response.End();
            }
            else
            {
                Response.Clear();
                Response.StatusCode = 404;
                Server.Transfer("~/Error.aspx");
            }
        }
    }
}