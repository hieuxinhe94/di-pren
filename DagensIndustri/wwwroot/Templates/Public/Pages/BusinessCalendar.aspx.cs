using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DIClassLib.BusinessCalendar;
using DIClassLib.Membership;
using DIClassLib.Misc;
using System.Text;
using DagensIndustri.Tools.Classes.BaseClasses;
using DIClassLib.DbHelpers;
using System.Configuration;
using DIClassLib.DbHandlers;
using DIClassLib.ServiceVerifier;


namespace DagensIndustri.Templates.Public.Pages
{
    public partial class BusinessCalendar : DiTemplatePage
    {
        Subscription _sub;
        
        #region posted variables
        
        bool _insert;                   //1==>insert, 0==>delete
        int _compId = -100;             //CompanyId (DI=-1)
        int _diEvTypeId = -100;         //-1==>conf, -2==>gasell, -3==>event
        int _diEvId = -100;             //DI-id (konf/gasell/event)

        bool _grUtdelningar = false;
        bool _grStammor = false;
        bool _grKapitalmarkndagar = false;
        bool _grRapporter = false;
        bool _grEmissioner = false;
        bool _grSplittar = false;
        
        #endregion

        private bool IsRegularEvent
        {
            get
            {
                if (_compId > -100) //&& (_gr1_utdelningar || _gr2_stammor || _gr3_kapitalmarkndagar || _gr4_rapporter || _gr5_emissioner || _gr6_splittar)
                    return true;

                return false;
            }
        }
        
        private bool IsDiEvent
        {
            get
            {
                if (_diEvId > -100 && _diEvTypeId > -100)
                    return true;

                return false;
            }
        }
                
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!MembershipFunctions.UserAllowedToSeePage(DiRoleHandler.RoleDiGold))
            {
                UserMessage1.ShowMessage("/common/message/onlypren", true, true);
                PlaceHolderBusCal.Visible = false;
                return;
            }

            //_sub = BusCalHandler.GetSubscription("nazgol"); 
            _sub = BusCalHandler.GetSubscription(HttpContext.Current.User.Identity.Name);

            if (TrySetMembers() && (IsRegularEvent || IsDiEvent))
            {
                if (_insert)
                    InsertEventsInSubs();
                else
                    DeleteEventsInSubs();

                Response.Redirect(CurrentPage.LinkURL);     //clean up url
            }

            if (!ServiceVerifier.BusinessCalendarIsValid)
            {
                PlaceHolderBusCal.Visible = false;
                UserMessage1.ShowMessage("/buscal/servicenotavailable", true, true);
            }
            else
            {
                PlaceHolderBusCal.Visible = true;
            }

            DataBind();
        }
        
        private bool TrySetMembers()
        {
            string ins = "";
            if(Request.QueryString["insert"] != null)
                ins = Request.QueryString["insert"].ToString();
            if (ins != "0" && ins != "1")
                return false;
            _insert = (ins == "1") ? true : false;


            if (Request.QueryString["compId"] == null || !int.TryParse(Request.QueryString["compId"].ToString(), out _compId))
                _compId = -100;

            if (Request.QueryString["diEvTypeId"] == null || !int.TryParse(Request.QueryString["diEvTypeId"], out _diEvTypeId))
                _diEvTypeId = -100;
            
            if (Request.QueryString["diEvId"] == null || !int.TryParse(Request.QueryString["diEvId"], out _diEvId))
                _diEvId = -100;


            if (_compId == -100 && (_diEvTypeId == -100 || _diEvId == -100))
                return false;

            _grUtdelningar       = IsUrlValueIsSet("cat_1");
            _grStammor           = IsUrlValueIsSet("cat_2");
            _grKapitalmarkndagar = IsUrlValueIsSet("cat_3");
            _grRapporter         = IsUrlValueIsSet("cat_4");
            _grEmissioner        = IsUrlValueIsSet("cat_5");
            _grSplittar          = IsUrlValueIsSet("cat_6");

            //if (!_gr1_utdelningar && !_gr2_stammor && !_gr3_kapitalmarkndagar && !_gr4_rapporter && !_gr5_emissioner &&
            //    !_gr6_splittar && !_gr7_diKonferens && !_gr8_diGasell && !_gr9_diEvent)
            //    return false;

            return true;
        }

        private bool IsUrlValueIsSet(string param)
        {
            if (Request.QueryString[param] != null && Request.QueryString[param] == "on")
                return true;

            return false;
        }


        private List<int> GetSelectedGroupIds()
        {
            List<int> groupIds = new List<int>();

            if (_grUtdelningar)       groupIds.Add(BusCalSettings.GrIdUtdelningar);
            if (_grStammor)           groupIds.Add(BusCalSettings.GrIdStammor);
            if (_grKapitalmarkndagar) groupIds.Add(BusCalSettings.GrIdKapmarkndagar);
            if (_grRapporter)         groupIds.Add(BusCalSettings.GrIdRapporter);
            if (_grEmissioner)        groupIds.Add(BusCalSettings.GrIdEmissioner);
            if (_grSplittar)          groupIds.Add(BusCalSettings.GrIdSplit);

            return groupIds;
        }

        private void InsertEventsInSubs()
        {            
            List<EventInSubs> evs = new List<EventInSubs>();

            if(IsRegularEvent)
            {
                DeleteEventsInSubs();

                foreach (int grId in GetSelectedGroupIds())
                {
                    foreach (EventInSubs eis in GetEventsInGroup(grId))
                    {
                        if(!EventExistsInSubs(eis))
                            evs.Add(eis);
                    }
                }
            }

            if (IsDiEvent)
            {
                EventInSubs e1 = new EventInSubs(BusCalSettings.DiCompanyId, _diEvTypeId, _diEvId);
                if(!EventExistsInSubs(e1))
                    evs.Add(e1);
            }

            BusCalHandler.AddEventsToSubs(_sub, evs);
        }

        private List<EventInSubs> GetEventsInGroup(int groupId)
        {
            List<EventInSubs> evs = new List<EventInSubs>();

            foreach (Group g in BusCalHandler.Groups)
            {
                if (g.GroupId == groupId)
                {
                    foreach (TypeSubType t in g.TypesSubTypes)
                        evs.Add(new EventInSubs(_compId, t.TypeId, t.SubTypeId));
                }
            }

            return evs;
        }


        private void DeleteEventsInSubs()
        {
            if (IsRegularEvent)
            {
                List<EventInSubs> evsToDel = new List<EventInSubs>();

                foreach (EventInSubs ev in _sub.EventsInSubs)
                {
                    if (ev.CompanyId == _compId)
                        evsToDel.Add(ev);
                }

                foreach (EventInSubs ev in evsToDel)
                    BusCalHandler.DeleteEventInSubs(_sub, ev);
            }

            if (IsDiEvent)
            {
                EventInSubs ev = new EventInSubs(BusCalSettings.DiCompanyId, _diEvTypeId, _diEvId);
                if(EventExistsInSubs(ev))
                    BusCalHandler.DeleteEventInSubs(_sub, ev);
            }
        }

        private bool EventExistsInSubs(EventInSubs ev)
        {
            foreach (EventInSubs e in _sub.EventsInSubs)
            { 
                if(EventInSubsAreEqual(e, ev))
                    return true;
            }

            return false;
        }

        private bool EventInSubsAreEqual(EventInSubs e1, EventInSubs e2)
        {
            return (e1.CompanyId == e2.CompanyId && e1.TypeId == e2.TypeId && e1.SubTypeId == e2.SubTypeId);
        }


        public string GetLinksReuglarEvents()
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                foreach (int compId in _sub.GetCompanyIds())
                {
                    if (compId == BusCalSettings.DiCompanyId)
                        continue;

                    sb.Append("<tr>");
                    sb.Append("<td>" + BusCalHandler.GetCompanyName(compId) + "</td>");
                    sb.Append("<td>");

                    int i = 0;
                    foreach (string s in _sub.GetGroupNames(compId))
                    {
                        sb.Append((i == 0) ? s : ", " + s);
                        i++;
                    }

                    sb.Append("<td><a href='?insert=0&compId=" + compId.ToString() + "' class='more delete'>Ta bort</a></td>");
                    sb.Append("</tr>");
                }
            }
            catch (Exception ex)
            {
                new Logger("GetLinksReuglarEvents() failed", ex.ToString());
            }

            return sb.ToString();
        }

        public string GetLinksDiEventsSubscribedOn()
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                foreach (EventInSubs ev in _sub.GetDiEvents())
                {
                    CompanyEvent ce = BusCalHandler.GetCompanyEvent(ev);
                    if (ce != null)
                    {
                        sb.Append("<tr>");
                        sb.Append("<td>" + ce.Summary + "</td>");
                        sb.Append("<td><a href='?insert=0&diEvTypeId=" + ce.TypeId.ToString() + "&diEvId=" + ev.SubTypeId.ToString() + "' class='more delete'>Ta bort</a></td>");
                        sb.Append("</tr>");
                    }
                }
            }
            catch (Exception ex)
            {
                new Logger("GetLinksDiEventsSubscribedOn() failed", ex.ToString());
            }

            return sb.ToString();
        }

        public string GetLinksAllDiEvents()
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                foreach (CompanyEvent ce in BusCalHandler.CompanyEvents)
                {
                    if (ce.CompanyId == BusCalSettings.DiCompanyId)
                    {
                        sb.Append("<li>");
                        sb.Append("<h5>" + ce.Summary + "</h5>");
                        sb.Append("<a href='?insert=1&diEvTypeId=" + ce.TypeId.ToString() + "&diEvId=" + ce.SubTypeId.ToString() + "' class='more add'>Lägg till</a>");
                        sb.Append("</li>");
                    }
                }
            }
            catch (Exception ex)
            {
                new Logger("GetLinksAllDiEvents() failed", ex.ToString());
            }

            return sb.ToString();
        }

        public string GetCalendarLink()
        {
            new Logger(Settings.LogEvent_CompanyCalendar, MembershipDbHandler.GetCusno(HttpContext.Current.User.Identity.Name), true);

            return ConfigurationManager.AppSettings["BusCalPathToCalendarFile"] + "?id=" + _sub.SubsId.ToString();
        }

    }
}