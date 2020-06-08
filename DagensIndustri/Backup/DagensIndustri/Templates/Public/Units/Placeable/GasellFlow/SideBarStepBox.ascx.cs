using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;
using EPiServer.Core;

namespace DagensIndustri.Templates.Public.Units.Placeable.GasellFlow
{
    public partial class SideBarStepBox : EPiServer.UserControlBase
    {
        #region Properties

        private Pages.DiGasell.GasellFlow GasellFlow
        {
            get
            {
                return (Pages.DiGasell.GasellFlow)Page;
            }
        }

        public string step { get; set; }

        #endregion

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            int numberOfParticipants = Convert.ToInt32(Request.QueryString["nop"]);
            step = String.Empty;
            string steps = Translate("/gasell/flow/sidebar/totalsteps");

            MultiView FlowMultiView = GasellFlow.MyFlowMultiView;

            if (FlowMultiView.ActiveViewIndex <= Convert.ToInt32(numberOfParticipants) - 1)
            {
                step = Translate("/gasell/flow/sidebar/step")+  " " + Translate("/gasell/flow/sidebar/step1") + " " + Translate("/gasell/flow/sidebar/of") + " " + Translate("/gasell/flow/sidebar/totalsteps");
                AddListItems(4, 1);
            }
            else if (FlowMultiView.ActiveViewIndex == Convert.ToInt32(numberOfParticipants))
            {
                step = Translate("/gasell/flow/sidebar/step")+  " " + Translate("/gasell/flow/sidebar/step2") + " " + Translate("/gasell/flow/sidebar/of") + " " + Translate("/gasell/flow/sidebar/totalsteps");
                AddListItems(4, 2);
            }
            else if (FlowMultiView.ActiveViewIndex == Convert.ToInt32(numberOfParticipants) + 1)
            {
                step = Translate("/gasell/flow/sidebar/step")+  " " + Translate("/gasell/flow/sidebar/step3") + " " + Translate("/gasell/flow/sidebar/of") + " " + Translate("/gasell/flow/sidebar/totalsteps");
                AddListItems(4, 3);
            }
            else if (FlowMultiView.ActiveViewIndex == Convert.ToInt32(numberOfParticipants) + 2)
            {
                step = Translate("/gasell/flow/sidebar/step") + " " + Translate("/gasell/flow/sidebar/step4") + " " + Translate("/gasell/flow/sidebar/of") + " " + Translate("/gasell/flow/sidebar/totalsteps");
                AddListItems(4, 4);
            }
        }

        protected void AddListItems(int items, int strongItem)
        {
            int i;
            for (i = 1; i <= items; i++)
            {
                string langStep = "step" + i.ToString() + ".text";
                System.Web.UI.HtmlControls.HtmlGenericControl li = new System.Web.UI.HtmlControls.HtmlGenericControl("li");

                if (i == strongItem)
                {
                    li.Attributes.Add("class", "active");
                }

                li.InnerText = Translate("/gasell/flow/sidebar/" + langStep);
                checkoutsteps.Controls.Add(li);
            }
        }
    }
}