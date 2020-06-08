<%@ Page Title="" Language="C#" MasterPageFile="~/Templates/Public/MasterPages/MasterPage.Master" AutoEventWireup="true" CodeBehind="Vendors.aspx.cs" Inherits="DagensIndustri.Templates.Public.Pages.Vendors" %>
<%@ Register TagPrefix="di" TagName="heading" Src="~/Templates/Public/Units/Placeable/Heading.ascx" %>


<asp:Content ID="Content4" ContentPlaceHolderID="HeadingContentPlaceHolder" runat="server">
    <di:heading runat="server" />
</asp:Content>

<asp:Content ID="Content6" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <div class="search-form">
        <label for="input-areas">
            <EPiServer:Translate Text="/vendors/label" runat="server" />
        </label>	    
	    <asp:DropDownList runat="server" CssClass="input-areas" ID="DdlCities" OnSelectedIndexChanged="FillVendorList" AutoPostBack="true"></asp:DropDownList>
    </div>

    <div id="search-results"> 	
        <asp:Repeater runat="server" ID="RepVendorInfo" >
	        <HeaderTemplate>
                <ul>         
	        </HeaderTemplate>
	        <ItemTemplate>
                <li class="vcard"> 
	  				<h3 class="org fn"><%# DataBinder.Eval(Container.DataItem, "Vendor") %></h3> 
	  				<ul class="meta-list"> 
	  					<li class="adr">
                            <%# System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(DataBinder.Eval(Container.DataItem, "Address").ToString().ToLower())%>,
                            <%# DataBinder.Eval(Container.DataItem, "PostCode") %>, 
                            <%# System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(DataBinder.Eval(Container.DataItem, "City").ToString().ToLower()) %>
                        </li> 
	  					<li>
                            Tel: <a class="tel" href="callto:<%# DIClassLib.Misc.MiscFunctions.FormatPhoneNumber(DataBinder.Eval(Container.DataItem, "Phone").ToString()) %>"><%# DIClassLib.Misc.MiscFunctions.FormatPhoneNumber(DataBinder.Eval(Container.DataItem, "Phone").ToString()) %></a>
                        </li> 
	  				</ul> 
	  			</li> 
	        </ItemTemplate>
	        <FooterTemplate>
                </ul>
	        </FooterTemplate>
	    </asp:Repeater>	    
        <asp:Label runat="server" ID="LblMessage"></asp:Label>
    </div>
</asp:Content>