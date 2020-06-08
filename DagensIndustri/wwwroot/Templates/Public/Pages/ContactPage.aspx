<%@ Page Title="" Language="C#" MasterPageFile="~/Templates/Public/MasterPages/MasterPage.Master" AutoEventWireup="true" CodeBehind="ContactPage.aspx.cs" Inherits="DagensIndustri.Templates.Public.Pages.ContactPage" %>
<%@ Register TagPrefix="di" TagName="heading" Src="~/Templates/Public/Units/Placeable/Heading.ascx" %>
<%@ Register TagPrefix="di" TagName="mainintro" Src="~/Templates/Public/Units/Placeable/MainIntro.ascx" %>


<asp:Content ID="Content4" ContentPlaceHolderID="HeadingContentPlaceHolder" runat="server">
    <di:heading runat="server" />
</asp:Content>

<asp:Content ID="Content6" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <di:mainintro runat="server" />

	<ul class="contactlist"> 
		<li> 
		  	<div class="image-wrapper"><img width="140" height="210" src="/templates/public/images/placeholders/red-pf-small.jpg" class="attachment-large wp-post-image" alt="Peter Fellman" title="Peter Fellman" /></div> 
		  	<h2>Peter Fellman</h2> 
		  	<p class="title">Chefredaktör</p> 
		  	<p class="phone">08-573 650 86</p> 
		  	<p class="mail"><a href="mailto:peter.fellman@di.se">peter.fellman@di.se</a></p> 
		</li> 
		  		
		<li> 
		  	<div class="image-wrapper"><img width="140" height="210" src="/templates/public/images/placeholders/red-le-small.jpg" class="attachment-large wp-post-image" alt="red-le-small" title="red-le-small" /></div> 
		  	<h2>Lotta Edling</h2> 
		  	<p class="title">Redaktionschef</p> 
		  	<p class="phone">08-573 650 70</p> 
		  	<p class="mail"><a href="mailto:lotta.edling@di.se">lotta.edling@di.se</a></p> 
		</li> 
		  		
		<li> 
		  	<div class="image-wrapper"><img width="140" height="210" src="/templates/public/images/placeholders/red-mn-small.jpg" class="attachment-large wp-post-image" alt="red-mn-small" title="red-mn-small" /></div> 
		  	<h2>Maria Nilsson</h2> 
		  	<p class="title">Chef di.se</p> 
		  	<p class="phone">08-573 650 91</p> 
		  	<p class="mail"><a href="mailto:maria.nilsson@di.se">maria.nilsson@di.se</a></p> 
		</li> 
		  		
		<li> 
		  	<div class="image-wrapper"><img width="140" height="210" src="/templates/public/images/placeholders/red-jj-small.jpg" class="attachment-large wp-post-image" alt="red-jj-small" title="red-jj-small" /></div> 
		  	<h2>Jonas Jonsson</h2> 
		  	<p class="title">Nyhetschef</p> 
		  	<p class="phone">08-573 651 49</p> 
		  	<p class="mail"><a href="mailto:jonas.jonsson@di.se">jonas.jonsson@di.se</a></p> 
		</li>

        <li> 
		  	<div class="image-wrapper"><img width="140" height="210" src="/templates/public/images/placeholders/red-ae-small.jpg" class="attachment-large wp-post-image" alt="red-ae-small" title="red-ae-small" /></div>
		  	<h2>Anna Ekelund</h2> 
		  	<p class="title">Nyhetschef</p> 
		  	<p class="phone">08-573 650 75</p> 
		  	<p class="mail"><a href="mailto:anna.ekelund@di.se">anna.ekelund@di.se</a></p> 
		</li>

		<li> 
		  	<%--<div class="image-wrapper" style="width:140px; height:214px;"></div>--%>
            <div class="image-wrapper"><img width="140" height="210" src="/templates/public/images/placeholders/red-cs-small.jpg" class="attachment-large wp-post-image" alt="red-cs-small" title="red-cs-small" /></div>
            <h2>Cecilia Schramm</h2>
		  	<p class="title">Chef Analys</p> 
		  	<p class="phone">08-573 651 98</p> 
		  	<p class="mail"><a href="mailto:cecilia.schramm@di.se">cecilia.schramm@di.se</a></p> 
		</li>
		  		
		<li> 
		  	<div class="image-wrapper"><img width="140" height="210" src="/templates/public/images/placeholders/red-dimension-hw-small.jpg" class="attachment-large wp-post-image" alt="red-dimension-hw-small" title="red-dimension-hw-small" /></div> 
		  	<h2>Henrietta Westman</h2> 
		  	<p class="title">Redaktör Dimension</p> 
		  	<p class="phone">08-573 652 01</p> 
		  	<p class="mail"><a href="mailto:henrietta.westman@di.se">henrietta.westman@di.se</a></p> 
		</li>	     					
	</ul> 
</asp:Content>

