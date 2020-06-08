<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LanguageLink.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.LanguageLink" %>

<asp:PlaceHolder ID="SwedishPlaceHolder" Visible="false" runat="server">
    <p class="language">
	    <a href='<%= languageURL %>' class="se">
            <%= EPiFunctions.HasValue(CurrentPage, "LanguageText") ? CurrentPage["LanguageText"] : "View this page in Swedish"%>
        </a>
    </p>
</asp:PlaceHolder>

<asp:PlaceHolder ID="EnglishPlaceHolder" Visible="false" runat="server">
    <p class="language">
        <a href='<%= languageURL %>' class="en">
            <%= EPiFunctions.HasValue(CurrentPage, "LanguageText") ? CurrentPage["LanguageText"] : "View this page in English"%>
        </a>
    </p>
</asp:PlaceHolder>