<%@ Page Language="C#" MasterPageFile="~/Templates/Public/MasterPages/MasterPage.Master" AutoEventWireup="true" CodeBehind="BusinessCalendar.aspx.cs" Inherits="DagensIndustri.Templates.Public.Pages.BusinessCalendar" %>
<%@ Register TagPrefix="DI" TagName="Heading" Src="~/Templates/Public/Units/Placeable/Heading.ascx" %>
<%@ Register tagname="UserMessage" tagprefix="uc1" src="../Units/Placeable/UserMessage.ascx" %>
<%@ Register tagname="MainIntro" tagprefix="uc3" src="../Units/Placeable/MainIntro.ascx" %>
<%@ Register tagname="MainBody" tagprefix="uc2" src="../Units/Placeable/MainBody.ascx" %>


<asp:Content ID="Content4" ContentPlaceHolderID="HeadingContentPlaceHolder" runat="server">
    <DI:Heading ID="HeadingControl" runat="server"/>
</asp:Content>


<asp:Content ID="Content6" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    

    <uc1:UserMessage ID="UserMessage1" runat="server" />

    
    
    <asp:PlaceHolder ID="PlaceHolderBusCal" runat="server">
   
        <script src="../js/DiGuldCalendar.js" type="text/javascript"></script>

	    <!-- Content -->
        <div id="content">
			
            <p class="intro">
	            <uc3:MainIntro ID="MainIntro1" runat="server" />
            </p>			
			
            <uc2:MainBody ID="MainBody1" runat="server" />

            <div class="form-nav">
	            <ul>
 		            <li class="current"><a href="#calendar-ordinary"><%# Translate("/buscal/regevs") %></a></li>
 		            <li><a href="#calendar-events"><%# Translate("/buscal/activities") %></a></li> 					
 	            </ul>
            </div>
			
            <div class="form-box">
				
	            <!-- Ordinary -->
	            <div class="section" id="calendar-ordinary">
					
		            <!-- Add -->
		            <div class="row">
			            <div class="col">
				            <label for="input-company"><%# Translate("/buscal/compnames") %></label>
				            <input type="text" class="text medium" name="company" id="input-company" autocomplete="off" />
			            </div>
		            </div>
					
		            <div class="row searchlist">
			            <h4><%# Translate("/buscal/searchresult") %></h4>
			            <ul></ul>
		            </div>
					
		            <div class="row noresults">
			            <p><%# Translate("/buscal/searchresultdetails") %></p>
		            </div>
					
	            </div>
	            <!-- // Ordinary -->
				
	            <!-- Events -->
	            <div class="section" id="calendar-events">

		            <div class="row searchlist">
			            <h4><%# Translate("/buscal/upcomingevs") %></h4>
			            <ul>

                            <%=GetLinksAllDiEvents()%>
														
			            </ul>
		            </div>
					
	            </div>
	            <!-- // Events -->								
	
            </div>
            <!-- // Add -->
			
            <!-- My calendar -->
            <div class="section section-visible my-calendar" id="my-calendar">
	            <h2>Min kalender</h2>
				
	            <!-- Ordinary -->
	            <h3 class="box-header"><%# Translate("/buscal/regevs") %></h3>
	            <div class="content">		
		            <table class="top-term">
		            <tbody>
                        
                        <%=GetLinksReuglarEvents()%>
			  						  						  						  			
		            </tbody>
	            </table>						
	            </div>
	            <!-- // Ordinary -->

	            <!-- Events -->
	            <h3 class="box-header"><%# Translate("/buscal/activities") %></h3>
	            <div class="content">		
		            <table class="top-term">
		            <tbody>
                        
                        <%=GetLinksDiEventsSubscribedOn()%>
			            	  						  						  						  			
		            </tbody>
	            </table>						
	            </div>
	            <!-- // Events -->
				
	            <!-- Export -->
	            <h3 class="box-header"><%# Translate("/buscal/linktocal") %></h3>
	            <div class="content">
		            <div class="button-wrapper">
			            <a href="#" class="btn" id="btn-showurl"><span><%# Translate("/buscal/showlinktocal") %></span></a>
		            </div>
		            <div class="sub-section hidden" id="input-url">
			            <input type="text" class="text large" value="<%=GetCalendarLink()%>" />
		            </div>
					
		            <!-- 
			            If a user already had a link previously generated the div.button-wrapper can be removed and 
			            the class "hidden" on div.sub-section can be removed so we reveal the url initialy.
		            -->
					
	            </div>
	            <!-- // Export -->												
				
            </div>			
            <!-- // My calendar -->			
						
        </div>
	    <!-- // Content -->
		
	    <br class="clear" />		

    </asp:PlaceHolder>

</asp:Content>


<%--<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>--%>
<%--<asp:Content ID="Content2" ContentPlaceHolderID="TopContentPlaceHolder" runat="server"></asp:Content>--%>
<%--<asp:Content ID="Content3" ContentPlaceHolderID="RSSPlaceHolder" runat="server"></asp:Content>--%>
<%--<asp:Content ID="Content5" ContentPlaceHolderID="WideMainContentPlaceHolder" runat="server"></asp:Content>--%>
<%--<asp:Content ID="Content7" ContentPlaceHolderID="RightColumnPlaceHolder" runat="server"></asp:Content>--%>
