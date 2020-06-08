<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ConferenceList.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.Conference.ConferenceList" %>

<EPiServer:PageList ID="MainConferencePageList" runat="server">
    <HeaderTemplate>
        
    </HeaderTemplate>
    <ItemTemplate>
        <div class="banner"> 
	        <div class="content"> 
                <img src='<%# Container.CurrentPage["PuffImage"] %>' alt='<%# Container.CurrentPage["Heading"] %>' /> 
		        <h2>
                    <%# Container.CurrentPage["PuffHeading"] ?? Container.CurrentPage["Heading"] %>
                </h2> 
		        <p class="date"><%# EPiFunctions.GetConferenceDateAndPlace(Container.CurrentPage, true, "sv-SE") %></p> 
		        <p><%# Container.CurrentPage["PuffText"] %></p> 
		        <a href='<%# Container.CurrentPage.LinkURL %>' class="more">
                    <%# Container.CurrentPage["PuffReadMoreText"] ?? "Läs mer" %>
                </a>   
            </div> 
        </div>	  
    </ItemTemplate>
    <FooterTemplate>
    	    
    </FooterTemplate>
</EPiServer:PageList>
 							
			
<!-- The rest of the conferences -->
<%--<asp:PlaceHolder ID="Heading2PlaceHolder" runat="server">
    <h2>
        <EPiServer:Translate Text="/conference/more" runat="server" />
    </h2> 
</asp:PlaceHolder>--%>
 

<%--<EPiServer:PageList ID="SecondaryConferencePageList" runat="server">
    <HeaderTemplate>
        <dl class="conferencelist"> 
    </HeaderTemplate>
    <ItemTemplate>
        <dt><%# Container.CurrentPage.PageName%></dt> 
	    <dd> 
		    <span class="date"><%# EPiFunctions.GetConferenceDateAndPlace(Container.CurrentPage, false, "sv-SE") %></span> 
		    <a href='<%# Container.CurrentPage.LinkURL %>'>
                <%# Container.CurrentPage["PuffReadMoreText"] ?? "Läs mer" %>
            </a> 
	    </dd> 
    </ItemTemplate>
    <FooterTemplate>
        </dl> 
    </FooterTemplate>
</EPiServer:PageList> --%>