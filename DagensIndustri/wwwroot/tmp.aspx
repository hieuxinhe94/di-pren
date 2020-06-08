<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="tmp.aspx.cs" Inherits="DagensIndustri.tmp" MasterPageFile="~/Templates/Public/MasterPages/MasterPage.Master" %>


<%--<asp:Content ContentPlaceHolderID="WideMainContentPlaceHolder" runat="server">--%>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
<asp:placeholder ID="PlaceholderTestGui" Visible="false" runat="server">

<%--<asp:Label ID="LabelRet" runat="server"></asp:Label>
<br />
<br />
<asp:Label ID="lblTestOutput" runat="server"></asp:Label>
<br />
<br />
<asp:Button ID="btnSendSMSTest" runat="server" Text="Testa SMS till Per L" onclick="btnSendSMSTest_Click" />
<br />
<asp:Button ID="btnPerTest" runat="server" Text="Testknapp för Apsis GetSubscribers" onclick="btnPerTest_Click" />--%>
    
<%--
    <br />
    <br />
    <b>apsisProjectGuids</b><br />
    Helg BF801EEC-6C24-4144-864E-531BEBBAE277<br />
    Digital korta perioder B64CDFD7-5B14-4DA0-8082-6AED0319A492<br />
    Ordinarie korta perioder 370A3C4A-B357-4BDE-8DDA-2139C8BF05D7<br />
    Ordinarie 910BEEE1-90EF-4980-9FB9-4606B4261DCC<br />
    Prov BE59FBDC-DC95-41B4-9C0D-410F8B59F7DC<br />
    Digital ordinarie BD6C0044-5A7A-4CAF-9F6D-8D63E6EDAAD6
    <br />
    <br />
    apsisProjectGuid: <asp:TextBox ID="TextBoxProjGuid" runat="server"></asp:TextBox><br />
    customerName: <asp:TextBox ID="TextBoxName" runat="server"></asp:TextBox><br />
    email: <asp:TextBox ID="TextBoxEmail" runat="server"></asp:TextBox><br />
    cusno: <asp:TextBox ID="TextBoxCusno" runat="server"></asp:TextBox><br />
    <asp:Button ID="Button1" runat="server" Text="Button" onclick="Button1_Click" />
--%>   

    <asp:Label ID="lblOutput" runat="server"></asp:Label>
    <asp:Button ID="Button1" runat="server" Text="Button" onclick="Button1_Click" />

    <br />
    
    
</asp:placeholder>
</asp:Content>


<asp:Content ContentPlaceHolderID="PlaceHolderOnTopOfSidebarBoxes" runat="server">
    
</asp:Content>


<%-- OLD CRAP

<script>
   window.twttr = (function (d,s,id) {
  var t, js, fjs = d.getElementsByTagName(s)[0];
  if (d.getElementById(id)) return; js=d.createElement(s); js.id=id;
  js.src="https://platform.twitter.com/widgets.js"; fjs.parentNode.insertBefore(js, fjs);
  return window.twttr || (t = { _e: [], ready: function(f){ t._e.push(f) } });
}(document, "script", "twitter-wjs"));
</script>
<div id="twitter-wjs"></div>



<script type="text/javascript"> 
    //<![CDATA[
    $(document).ready(function () {
        $('#Link1').click(function () {
            $('#ctl00_DiGoldMembershipPopup_NotLoggedInPopupControl_ReturnURLHiddenField').val('http://localhost/templates/public/pages/digoldstartpage.aspx?id=45');
        })
    });//]]>
    </script>
    <asp:LinkButton ID="LinkButton1" NavigateUrl="/templates/public/Units/Placeable/#membership-required" CssClass="ajax more" runat="server">LinkButton</asp:LinkButton>
    <br />
    <asp:HyperLink ID="HyperLink1" NavigateUrl="#membership-required" CssClass="ajax" runat="server" Text="test2"></asp:HyperLink>
    <br />
    <a id="Link1" class="ajax more" href="#membership-required">Läs mer</a>
    <input type="hidden" id="ctl00_DiGoldMembershipPopup_NotLoggedInPopupControl_ReturnURLHiddenField" />
    
--%>


<%--<EPiServer:MenuList ID="MenuList1" NumberOfLevels="3" PageLink="3" runat="server" >
    <HeaderTemplate></HeaderTemplate>
    <ItemTemplate></ItemTemplate>
    <FooterTemplate></FooterTemplate>
</EPiServer:MenuList>

<EPiServer:PageTree ID="PageTree1" NumberOfLevels="3" runat="server">
    <HeaderTemplate></HeaderTemplate>
    <ItemTemplate></ItemTemplate>
    <FooterTemplate></FooterTemplate>
</EPiServer:PageTree>

<EPiServer:PageList ID="PageList1" Paging="true" PagesPerPagingItem="5" MaxCount="20" runat="server">
    <HeaderTemplate></HeaderTemplate>
    <ItemTemplate></ItemTemplate>
    <FooterTemplate></FooterTemplate>
    </EPiServer:PageList>--%>
        
<%-- = CurrentPage.Created.ToString() --%>
<%--<EPiServer:Property PropertyName="PublishedBy" DisplayMissingMessage="false" runat="server" />--%>
<%--<EPiServer:Translate Text="/root/next" runat="server" />--%>
