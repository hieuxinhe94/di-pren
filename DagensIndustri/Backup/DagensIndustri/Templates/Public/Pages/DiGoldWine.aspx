<%@ Page Title="" Language="C#" MasterPageFile="~/Templates/Public/MasterPages/MasterPage.Master" AutoEventWireup="true" CodeBehind="DiGoldWine.aspx.cs" Inherits="DagensIndustri.Templates.Public.Pages.DiGoldWine" %>
<%@ Register TagPrefix="di" TagName="heading" Src="~/Templates/Public/Units/Placeable/Heading.ascx" %>
<%@ Register TagPrefix="di" TagName="mainintro" Src="~/Templates/Public/Units/Placeable/MainIntro.ascx" %>
<%@ Register TagPrefix="di" TagName="mainbody" Src="~/Templates/Public/Units/Placeable/MainBody.ascx" %>
<%@ Register TagPrefix="di" TagName="sidebarboxlist" Src="~/Templates/Public/Units/Placeable/SideBarBoxes/SideBar.ascx" %>
<%@ Register TagPrefix="di" TagName="UserMessage" Src="~/Templates/Public/Units/Placeable/UserMessage.ascx" %>
<asp:Content ContentPlaceHolderID="head" runat="server">

        <script src="/Templates/Public/js/list.min.js" type="text/javascript"></script>
        <script src="/Templates/Public/js/list.paging.js" type="text/javascript"></script>
        <script src="/Templates/Public/js/wine.js" type="text/javascript"></script>
        <link href="/Templates/Public/Styles/wine/wine.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ContentPlaceHolderID="HeadingContentPlaceHolder" runat="server">
    <di:heading runat="server" />
</asp:Content>

<asp:Content ID="Content7" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
   <di:UserMessage ID="UserMessageControl" runat="server" />
    <% if (!HasAccess)
       { %>
       <%= CurrentPage["NoAccessIntro"]  %>
    <div class="btn-container">
        
       <asp:Button ID="btnNoAccessSubscribe" runat="server" Text="Abonnera" CssClass="btn-light" OnClick="btnNoAccessSubscribe_Click" />

        <%  if (!User.Identity.IsAuthenticated)
            { %>
                <asp:Button ID="btnNoAccessLogin" runat="server" Text="Logga in" CssClass="btn-light" OnClick="btnNoAccessLogin_Click" />
         <% } %>
                
    </div>
    <% } else { %>

<!-- SMS BOTTOM FIXED BAR -->
<div class="bottom-send-bar">
  <%if (!UserIsWineSubscriber)
    { %>
            <p>Abonnera på vintips via sms. <small>OBS! Du måste ha fyllt 20 år för att abonnera på tjänsten.</small></p>
            <asp:Button ID="btnStartSubscription" runat="server" CssClass="btn-light" Text="Abonnera" OnClick="btnStartSubscription_Click" />
            
      <%}else{ %>
          <p>
            Klicka på <em>Skicka</em> när du har kryssat för de vintips du vill få via SMS        
          </p>
          <asp:Button ID="btnSendSms" runat="server" CssClass="btn-light btn-send btn-disabled" Text="Skicka" OnClick="btnSendSms_Click" />    
  
  <%} %>
</div>
<!-- END SMS BOTTOM FIXED BAR -->

            <%= CurrentPage["MainIntro"]  %>
    <di:mainbody ID="MainBody" runat="server" />


    
<div class="form-nav">
    <ul>
        <li class="current">
            <a id="tag-filter" href="#">Filtrera</a>
        </li>
    </ul>
</div>
<div class="form-box">
    <div class="section" id="tag-cloud">
        <div class="row">
            <div class="col">
                <strong>Grupp:</strong>
                <ul class="tag-list tag-group-one">
                </ul>
                <strong>Passar till:</strong>
                <ul class="tag-list tag-group-two">
                </ul>
                <strong>Karaktär:</strong>
                <ul class="tag-list tag-group-three">
                </ul>
            </div>
            <div class="tag-cloud-reset">Ny sökning <span></span></div>
        </div>
    </div>
</div>

<div id="wine-container" class="wine-container">
    <h2 data-default="" data-selected="Dina val:"></h2>
    
    <script type="text/javascript">
      var wines = <%=WinesJson %>
    </script>

    <ul class="wine-list">
    </ul>
    <div class="wine-pagination">
		<ul class="paging"></ul>
	</div>

    <!-- Template for one list item. Used to render the list. -->
    <div style="display: none">
        <li id="wine-list-item" class="wine-list-item">
            <div class="wine-item-container">
                <div class="wine-content">
                    <div class="wine-map">
                        <div class="wine-corner"></div>
                        <div class="wine-google-map">
                            <a href="#" target="_blank">
                                <img src="" width="198" height="174" alt="">
                            </a>
                        </div>
                        <small class="wine-origin Origin"></small>
                    </div>
                    <small class="wine-week"><span class="Type"></span> Vecka <span class="Week"></span></small>
                    <h3 class="wine-name">
                        <span class="Name1"></span><br>
                        <span class="Name2"></span>
                    </h3>
                    <small class="wine-origin Origin2"></small>
                    <div class="wine-grape">
                        <p><strong>Druva</strong><br>
                            <span class="Grape"></span></p>
                    </div>
                    <div class="wine-character">
                        <p><strong>Karaktär</strong><br>
                            <span class="Character"></span></p>
                    </div>
                    <div class="wine-description">
                        <p class="About"></p>
                    </div>
                </div>
                <div class="wine-footer">
                    <div class="wine-notify">
                        <%if(UserIsWineSubscriber){ %>
                          <label><input type="checkbox" name="wine-notify"> Kryssa för ditt vinval här.<br />Klicka sedan på <em>Skicka</em><br />längst ned på sidan.</label>
                        <%} %>
                    </div>
                    <ul class="wine-info">
                        <li><strong>Varunummer:</strong> <span class="Varnummer"></span></li>
                        <li><strong>Årgång:</strong> <span class="Year"></span></li>
                        <li><strong>Alkoholhalt:</strong> <span class="Percentage"></span></li>
                    </ul>
                    <div class="wine-stock-info">
                        <a href="#">Lagersaldo och mer info</a>
                    </div>
                </div>
            </div>
        </li>
    </div>

</div>
<% } // if(HasAccess) %>
</asp:Content>
<asp:Content ID="Content8" ContentPlaceHolderID="RightColumnPlaceHolder" runat="server">
<%  if (HasAccess)
        { %>
            <% if (!UserIsWineSubscriber)
               { %>
                <div class="infobox">
                    <div class="wrapper">
                        <h2>Så funkar det</h2>
                        <div class="content">
                            <p>
                            Varje fredag skickar Per Styregård<br />
                            ett sms med sina personliga <br />
                            favoritviner: ett rött och ett vitt<br />
                            fynd han letat fram exklusivt för <br /> 
                            dig. Då har du alltid vintipsen <br />
                            till hands i din telefon inne <br />
                            på
                            Systembolaget – precis när <br />
                            du behöver dem.
                            </p>
                            <br />
                            <img style="position: absolute; bottom: 16px; right: 18px" src="/Templates/Public/Images/wine/di-per.jpg" alt="">
                        </div>
                    </div>
                </div>
                <div class="infobox infobox-dark">
                    <div class="wrapper">
                        <div class="wrapper-top"></div>
                        <h3>Abonnera på vintips via SMS</h3>
                        <div class="btn-wrapper">
                          <asp:Button ID="Button1" runat="server" CssClass="btn-light" Text="Abonnera" OnClick="btnStartSubscription_Click" />
                       </div>
                        <!--<div class="seperator"></div>-->
                        <small>OBS! Du måste ha fyllt 20 år för att abonnera på tjänsten.</small>
                        <div class="wrapper-bottom"></div>
                    </div>
                </div>

            
            <% }else{%>

                 <div class="infobox infobox-dark">
                    <div class="wrapper">
                        <div class="wrapper-top"></div>
                        <h3>Få vintips via SMS</h3>
                        <p>Kryssa för dina vinval till vänster. Klicka sedan på <em>Skicka</em> längst ner på sidan.</p>        
                        <!--<div class="seperator"></div>-->
                        <small>OBS! Du måste ha fyllt 20 år för att abonnera på tjänsten.</small>
                        <div class="wrapper-bottom"></div>
                    </div>
                </div>

             <%} 
}
%>

 <di:sidebarboxlist runat="server" />
</asp:Content>

