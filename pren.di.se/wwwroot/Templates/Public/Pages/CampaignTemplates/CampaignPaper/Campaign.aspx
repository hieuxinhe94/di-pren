<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="Campaign.aspx.cs" Inherits="PrenDiSe.Templates.Public.Pages.CampaignTemplates.CampaignPaper.Campaign" %>
<%@ Register TagPrefix="di" TagName="form" Src="~/Templates/Public/Units/CampaignTemplates/CampaignForm.ascx" %>
<%@ Register TagPrefix="di" TagName="header" Src="~/Templates/Public/Units/CampaignTemplates/CampaignHeader.ascx" %>
<%@ Register TagPrefix="di" TagName="papersidebar" Src="~/Templates/Public/Units/CampaignTemplates/CampaignPaperSideBar.ascx" %>
<%@ Register TagPrefix="di" TagName="googleanalytics" Src="~/Templates/Public/Units/Static/GoogleAnalytics.ascx" %>
<%@ Register src="~/Templates/Public/Units/CampaignTemplates/AddThisFieldset.ascx" tagname="AddThisFieldset" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <di:googleanalytics runat="server" />
    <di:header runat="server"></di:header>
    <title>
        <asp:Literal ID="LitPageTitle" runat="server" />
    </title>

    <!-- Start Visual Website Optimizer Asynchronous Code -->
    <script type='text/javascript'>
        var _vwo_code = (function () {
            var account_id = 54251,
            settings_tolerance = 2000,
            library_tolerance = 2500,
            use_existing_jquery = false,
            // DO NOT EDIT BELOW THIS LINE
            f = false, d = document; return { use_existing_jquery: function () { return use_existing_jquery; }, library_tolerance: function () { return library_tolerance; }, finish: function () { if (!f) { f = true; var a = d.getElementById('_vis_opt_path_hides'); if (a) a.parentNode.removeChild(a); } }, finished: function () { return f; }, load: function (a) { var b = d.createElement('script'); b.src = a; b.type = 'text/javascript'; b.innerText; b.onerror = function () { _vwo_code.finish(); }; d.getElementsByTagName('head')[0].appendChild(b); }, init: function () { settings_timer = setTimeout('_vwo_code.finish()', settings_tolerance); this.load('//dev.visualwebsiteoptimizer.com/j.php?a=' + account_id + '&u=' + encodeURIComponent(d.URL) + '&r=' + Math.random()); var a = d.createElement('style'), b = 'body{opacity:0 !important;filter:alpha(opacity=0) !important;background:none !important;}', h = d.getElementsByTagName('head')[0]; a.setAttribute('id', '_vis_opt_path_hides'); a.setAttribute('type', 'text/css'); if (a.styleSheet) a.styleSheet.cssText = b; else a.appendChild(d.createTextNode(b)); h.appendChild(a); return settings_timer; } };
        } ()); _vwo_settings_timer = _vwo_code.init();
    </script>
    <!-- End Visual Website Optimizer Asynchronous Code -->

</head>
<body id="body" runat="server">
    <form id="form1" runat="server">

    <asp:ScriptManager ID="ScriptManager" runat="server" />
    
    <div class="container">
        <div class="row marginbottom hidden-phone">
            <div class="span4">
                <img alt="Dagens industri" src="/templates/public/images/logo.png" />
            </div>
            <div class="span8">
                <div class="progresstracker">
                    <div class="step <%= GetProgressClass(2) %>">                                                                      
                        <div class="stepcontent">
                            <div class="arrow-in"></div>
                            <span class="steptext">3. Klar</span>
                        </div>                                              
                    </div>
                    <div class="step <%= GetProgressClass(1) %>">                        
                        <div class="stepcontent">
                            <div class="arrow-in"></div>
                            <span class="steptext">2. Dina uppgifter</span>
                            <div class="arrow-right"></div>
                        </div>                        
                    </div>
                    <div class="step <%= GetProgressClass(0) %>">                        
                        <div class="stepcontent">
                            <span class="steptext">1. Välj erbjudande</span>
                            <div class="arrow-right"></div>                                             
                        </div>                        
                    </div>
                </div>
            </div>
        </div>
        <div class="row marginbottom visible-phone">
            <div class="span12">
                <div class="progresstracker">
                    <div class="step <%= GetProgressClass(0) %>">                        
                        <div class="stepcontent">
                            <span class="steptext">1. Välj erbjudande</span>
                            <div class="arrow-right"></div> 
                        </div>                                                                    
                    </div>
                    <div class="step <%= GetProgressClass(1) %>">                        
                        <div class="stepcontent">
                            <div class="arrow-in"></div>
                            <span class="steptext">2. Dina uppgifter</span>
                            <div class="arrow-right"></div>
                        </div>                        
                    </div>
                    <div class="step <%= GetProgressClass(2) %>">                                                                      
                        <div class="stepcontent">
                            <div class="arrow-in"></div>
                            <span class="steptext">3. Klar</span>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <asp:MultiView runat="server" ID="MvSteps" ActiveViewIndex="0">
            <asp:View ID="Step1" runat="server">
                <%--Virtual page track for GA--%>
                <script type="text/javascript">
                    TrackPageview('/kampanj/step1');
                </script>
                <div class="row">
                    <div class="span12 white">
                        <div class="row">
                            <div class="span7">
                                <img alt='<%=CurrentPage["Campaign1Heading"] %>' src='<%=CurrentPage["Campaign1TopImage"] %>' />
                            </div>
                            <div class="span5">
                                <div class="diwrap">
                                    <h1><EPiServer:Property runat="server" PropertyName="Campaign1Heading" /></h1>
                                    <div class="size4">
                                        <EPiServer:Property runat="server" PropertyName="Campaign1SubHeading" />
                                    </div>
                                    <div>
                                        <small><i><EPiServer:Property runat="server" PropertyName="Campaign1ItalicText" /></i></small>
                                    </div>
                                    <div class="dicheckbox <%=JqueryLoadClass %>">  
                                        <asp:RadioButton ID="RbPrimaryCampaignStep1" Checked="true" GroupName="RbStep1" runat="server" />                                
                                        <asp:Label ID="Label1" runat="server" AssociatedControlID="RbPrimaryCampaignStep1">
                                            <div class="size4"><EPiServer:Property runat="server" PropertyName="Campaign1PrimHeading1" /></div>
                                            <div class="size6"><EPiServer:Property runat="server" PropertyName="Campaign1Price" /></div>
                                        </asp:Label>
                                    </div>
                                    <asp:PlaceHolder runat="server" ID="PhSecondaryArea1">
                                        <div class="dicheckbox <%=JqueryLoadClass %>">  
                                            <asp:RadioButton ID="RbSecondaryCampaignStep1" GroupName="RbStep1" runat="server" />                                
                                            <asp:Label ID="Label2" runat="server" AssociatedControlID="RbSecondaryCampaignStep1">
                                                <div class="size4"><EPiServer:Property ID="Property1" runat="server" PropertyName="Campaign2SecHeading" /></div>
                                                <div class="size6"><EPiServer:Property ID="Property2" runat="server" PropertyName="Campaign2Price" /></div>
                                            </asp:Label>
                                        </div>
                                    </asp:PlaceHolder>
                                    <asp:Button runat="server" CssClass="btn btn-large btn-success margintop20" ID="BtnSubmitStep1" OnClick="BtnSubmitStep1OnClick" data-loading-text="Laddar steg 2" Text="GÅ VIDARE TILL STEG 2" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="span12 papers">
                        <div id="loading-indicator" style="display: none;">
                            <img src="/bootstrapDi/img/ajax-loader2.gif" alt="Laddar..." />
                        </div>
                        <%--Placeholder for paperlist--%>
                        <div id="PaperList">
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="span12">
                        <EPiServer:Property runat="server" PropertyName="CampaignFooter" />
                    </div>
                </div>
            </asp:View>
            <asp:View ID="Step2" runat="server">
                <%--Virtual page track for GA--%>
                <script type="text/javascript">
                    TrackPageview('/kampanj/step2');
                </script>                
                <div class="row">
                    <div class="span4 hidden-phone">
                        <img alt='<%= CurrentPage[PrimaryCampaign + "PrimHeading"] %>' src='<%= CurrentPage[PrimaryCampaign + "PrimImage"] %>' />
                        <div class="diwrap">                            
                            <div class="borderbottom pink">                                
                                <div class="dicheckbox">  
                                    <asp:RadioButton ID="RbPrimaryCampaignStep2" onclick="<%#GetTrackScript(true) %>" AutoPostBack="true" Checked="true" GroupName="CampaignGroup" runat="server" />                                
                                    <asp:Label runat="server" AssociatedControlID="RbPrimaryCampaignStep2">
                                        <div class="size3"><%= CurrentPage[PrimaryCampaign + "PrimHeading"]%></div>
                                    </asp:Label>
                                </div>
                                <div class="text-center">
                                    <div class="size4"><%= CurrentPage[PrimaryCampaign + "Price"]%></div>
                                </div>
                            </div>
                            <div class="sidebarpapers">
                                <di:papersidebar runat="server" ID="SideBarPrimary"></di:papersidebar>
                            </div>
                        </div>
                        <asp:PlaceHolder runat="server" ID="PhSecondaryArea2">
                            <div class="diwrap margintop">
                                <div class="borderbottom pink">
                                    <div class="dicheckbox">    
                                        <asp:RadioButton ID="RbSecondaryCampaignStep2" onclick="<%#GetTrackScript(false) %>" AutoPostBack="true" GroupName="CampaignGroup" runat="server" />                                         
                                        <asp:Label runat="server" AssociatedControlID="RbSecondaryCampaignStep2">
                                            <div class="size3"><%= CurrentPage[SecondaryCampaign + "SecHeading"] %></div>
                                        </asp:Label>
                                    </div>
                                    <div class="text-center">
                                        <div class="size4"><%= CurrentPage[SecondaryCampaign + "Price"]%></div>
                                    </div>
                                </div>
                                <div class="sidebarpapers">
                                    <di:papersidebar runat="server" ID="SideBarSecondary"></di:papersidebar>
                                </div>
                            </div>
                        </asp:PlaceHolder>
                    </div>
                    <div class="span4 visible-phone">
                        <div class="dicheckbox marginbottom">  
                            <input type="checkbox" checked="checked" />                             
                            <label>
                                <span class="size3"><%= CurrentPage[PrimaryCampaign + "PrimHeading"]%></span>
                            </label>
                        </div>
                    </div>
                    <div class="span8">
                        <div class="diwrap">                            
                            <di:form runat="server" ID="UcCampaignForm" ValidationGroup="form" FormHeading="Ange dina uppgifter"></di:form>
                            <di:form runat="server" ID="UcCampaignFormOtherPayer" ValidationGroup="otherform" IsOtherPayerForm="true" Visible="false"  FormHeading="Ange fakturamottagarens uppgifter"></di:form>
                        </div>
                        
                        <EPiServer:Property runat="server" ID="PropFormEditor" CssClass="formeditor" />
                    </div>
                </div>
            </asp:View>
            <asp:View ID="Step3" runat="server">
                <%--Virtual page track for GA--%>
                <script type="text/javascript">
                    TrackPageview('/kampanj/step3');
                </script>
                <div class="row">
                    <div class="span4 hidden-phone">
                        <img alt='<%= CurrentPage[SelectedCampaign + "PrimHeading"] %>' src='<%= CurrentPage[SelectedCampaign + "PrimImage"] %>' />
                        <div class="diwrap">
                            <div class="borderbottom pink">                                
                                <div class="dicheckbox">  
                                    <input type="checkbox" checked="checked" />                             
                                    <label>
                                        <div class="size3"><%= CurrentPage[SelectedCampaign + "PrimHeading"]%></div>
                                    </label>
                                </div>
                                <div class="text-center">
                                    <div class="size4">
                                        <%= CurrentPage[SelectedCampaign + "Price"]%></div>
                                </div>
                            </div>
                            <div class="sidebarpapers">
                                <di:papersidebar runat="server" ID="SideBarStep3"></di:papersidebar>
                            </div>
                        </div>
                    </div>
                    <div class="span4 visible-phone">
                        <div class="dicheckbox">  
                            <input type="checkbox" checked="checked" />                             
                            <label>
                                <span class="size3"><%= CurrentPage[SelectedCampaign + "PrimHeading"]%></span>
                            </label>
                        </div>
                    </div>
                    <div class="span8">
                        
                        
                        <%--<%=DateTime.Now.ToString() %>--%>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <%--<%=DateTime.Now.ToString() %>--%>
                                
                                    <asp:PlaceHolder ID="PlaceHolderDigForm" runat="server">
                                        <div class="diwrap">
                                            <div class="size1"><EPiServer:Property ID="Property3" runat="server" PropertyName="ThankYouHeading" /></div>
                                            <EPiServer:Property runat="server" ID="PropThankYouText" />
                                        </div>
                                        <br />
                                        <div style="background-color: #ffffff;">
                                            <h3 style="padding-left:7px;">Kom igång digitalt redan nu!</h3>
                                            <div class="row-fluid">
                                                <div class="span4" style="background-color:#39392d; color:#ffffff; padding:15px; min-height:390px;">
                                                Användarnamn (e-post)<br />
                                                <asp:TextBox ID="TextBoxDigUser" CssClass="input-medium" required="required" runat="server"></asp:TextBox>
                                                <br />

                                                <asp:PlaceHolder ID="PlaceHolderDigPasswds" runat="server">
                                                    Lösenord (minst 6 tecken)<br />
                                                    <asp:TextBox ID="TextBoxDigPass1" CssClass="input-medium" TextMode="Password" required="required" runat="server"></asp:TextBox>
                                                    <br />
                                                    Repetera lösenord<br />
                                                    <asp:TextBox ID="TextBoxDigPass2" CssClass="input-medium" TextMode="Password" required="required" runat="server"></asp:TextBox>
                                                    <br />
                                                </asp:PlaceHolder>

                                                <asp:Literal ID="LiteralDigErr" Visible="false" runat="server"></asp:Literal>
                                                <asp:Button ID="ButtonDigAcc" Text="Skapa Di-konto" class="btn btn-success btn-large" runat="server" OnClick="ButtonDigAcc_Click" />
                                                <br />
                                                <br />
                                                <asp:LinkButton ID="LinkButtonDigHaveAcc" runat="server" onclick="LinkButtonDigHaveAcc_Click">Jag har redan ett Di-konto</asp:LinkButton>
                                                <asp:LinkButton ID="LinkButtonDigNoAcc" runat="server" onclick="LinkButtonDigNoAcc_Click" Visible="false">Jag har inget Di-konto</asp:LinkButton>
                                                <br />
                                                <br />
                                                Behöver du hjälp med ditt Di-konto? Kontakta kundtjänst: 08-573 651 00
                                            </div>
                        
                                                <div class="span8" style="padding-right:15px;">
                                                <h4>Läs Di när, var och hur du vill!</h4>
                                                Som prenumerant får du nu tillgång till dina digitala tjänster och kan läsa Dagens industri när och hur du vill och var du än är någonstans. 
                                                Perfekt när du är på tjänsteresa, semester eller på annan plats där du inte har tillgång till papperstidningen. 
                                                Aktivera dina digitala tjänster genom att fylla i formuläret till vänster. 
                                                <%--Ditt användarnamn blir detsamma som den e-postadress du valt när du registrerade din prenumeration. 
                                                Vill du byta ut e-postadressen till en annan e-postadress så går det såklart bra.--%>
                                                När du skapar ett nytt Di-konto så får du ett mail sänt till din e-postadress där du behöver bekräfta ditt Di-konto.
                                                <br />
                                                <br />
                                                <img src="/Templates/Public/Pages/CampaignTemplates/CampaignPaper/images/digitalProducts.jpg" alt="" border="0" />
                                            </div>
                                            </div>
                                        </div>
                                    </asp:PlaceHolder>

                                    <asp:PlaceHolder ID="PlaceHolderDigLinks" Visible="false" runat="server">
                                        <div class="row-fluid">
                                            <div class="span12 diwrap">
                                                <div class="size1">Grattis!</div>
                                                Nu har du full tillgång till tidningens innehåll digitalt. Nedan kan du välja på vilket 
                                                sätt du vill läsa tidningen så hjälper vi dig – steg för steg – med att komma igång. 
                                                <br />
                                                <br />
                                                <div class="row-fluid">
                                                    <div class="span3">
                                                        <img src="http://dagensindustri.se/Global/Digitalt/sid3_mobil.png" alt="" border="0" />
                                                    </div>
                                                    <div class="span9">
                                                        <h4>I mobilen</h4>
                                                        Ta del av hela tidningens innehåll i din smarta mobil redan vid midnatt. Här kan du läsa vår guide – som 
                                                        visar steg för steg – hur du kommer igång med att läsa Dagens industri i din mobil.<br>
                                                        <a target="_blank" href="/guide/mobil/">Läs guiden här</a>
                                                    </div>
                                                </div>
                                                <br />
                                                <div class="row-fluid">
                                                    <div class="span3">
                                                        <img src="http://dagensindustri.se/Global/Digitalt/sid3_lasplatta.png" alt="" border="0" />
                                                    </div>
                                                    <div class="span9">
                                                        <h4>I läsplattan</h4>
                                                        Läs Dagens industri, Di Weekend samt Di Idé, med extra material och mervärden som fördjupad grafik, bildspel och 
                                                        Twitterflöden, direkt i läsplattan. Vår guide visar dig – steg för steg – hur du kommer igång.<br>
                                                        <a target="_blank" href="/guide/lasplatta/">Läs guiden här</a>
                                                    </div>
                                                </div>
                                                <br />
                                                <br />
                                                <div class="row-fluid">
                                                    <div class="span3">
                                                        <img src="http://dagensindustri.se/Global/Digitalt/sid3_online.png" alt="" border="0" />
                                                    </div>
                                                    <div class="span9">
                                                        <h4>På nätet eller som PDF</h4>
                                                        Ta del av hela tidningens innehåll som pdf redan klockan 22.00 kvällen före utgivning. Vår guide visar dig – 
                                                        steg för steg – hur du gör för att läsa vårt utbud i din dator.<br>
                                                        <a target="_blank" href="/guide/dator/">Läs guiden här</a>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </asp:PlaceHolder>

                            </ContentTemplate>
                        </asp:UpdatePanel>


                        <br />
                        <div class="diwrap">
                            <uc1:AddThisFieldset ID="AddThisFieldset2" runat="server" />
                        </div>

                    </div>

                </div>
            </asp:View>
            <asp:View ID="ErrorView" runat="server">
                <div class="size3 error">
                    <asp:Label runat="server" ID="LblError" EnableViewState="false"></asp:Label>
                </div>
            </asp:View>
        </asp:MultiView>
    </div>
        
    <asp:Literal ID="LiteralAdWordsScriptOnLoad" EnableViewState="false" runat="server"></asp:Literal>
    <asp:Literal ID="LiteralAdWordsScriptOnThankYou" EnableViewState="false" runat="server"></asp:Literal>
        
    <div id="posreadmore<%=MvSteps.ActiveViewIndex %>" class="hidden-phone hide">
        <div class="rm">            
            <div class="rml">
                Läs mer<br /><i class="icon-chevron-down"></i>
            </div>
        </div>
    </div>    

    </form>
    <asp:PlaceHolder runat="server" ID="plhMaintenanceScript" Visible='<%#ConfigurationManager.AppSettings.Get("ShowMaintenanceMessage") == "true" %>'>
        <script type="text/javascript" src="<%=ResolveUrl("~/Templates/Public/js/maintenanceHandler/maintenanceHandler.js")%>"></script>
        <script type="text/javascript">
            maintenanceHandler.init();
        </script>
    </asp:PlaceHolder>
</body>
</html>
