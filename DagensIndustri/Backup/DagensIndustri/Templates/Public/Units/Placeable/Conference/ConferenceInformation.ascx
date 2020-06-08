<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ConferenceInformation.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.Conference.ConferenceInformation" %>

<div class="infobox">
	<div class="wrapper">
		<h2><%= CurrentPage["LanguageEnglish"] != null ? "More Information" : "Mer information"%></h2>
		<div class="content">
			<p>
				<b><%= CurrentPage["LanguageEnglish"] != null ? "Date" : "När"%></b><br />						
				<%= EPiFunctions.HasValue(CurrentPage, "Date") != true ? "" : ((DateTime)CurrentPage["Date"]).ToString("d MMMM yyyy", new System.Globalization.CultureInfo(CurrentPage["LanguageEnglish"] != null ? "en-US" : "sv-SE"))%> , <%= CurrentPage["LanguageEnglish"] != null ? "at." : "kl." %> <%= EPiFunctions.HasValue(CurrentPage, "Date") != true ? "" : ((DateTime)CurrentPage["Date"]).ToString("HH.mm") %>
			</p>
			<p>
				<b><%= CurrentPage["LanguageEnglish"] != null ? "Venue" : "Plats"%></b><br />
				<%= CurrentPage["Place"] %><br />
				<a href='<%= CurrentPage["ShowInMapURL"] %>' target="_blank" class="more"><%= CurrentPage["LanguageEnglish"] != null ? "Show in map" : "Visa på karta"%></a>
			</p>
			<p>
				<b><%= CurrentPage["LanguageEnglish"] != null ? "Contact" : "Kontakt"%></b><br />
				<%= CurrentPage["ContactName"] %><br />
				Tel: <%= CurrentPage["ContactPhone"] %><br />
				<%= CurrentPage["LanguageEnglish"] != null ? "E-mail" : "E-post:"%>  <a href='mailto: <%= CurrentPage["ContactEmail"] %>'><%= CurrentPage["ContactEmail"] %></a>
			</p>
            <EPiServer:Property PropertyName="FreeTextMoreInformation" DisplayMissingMessage="false" runat="server" />
		</div>
	</div>
</div>