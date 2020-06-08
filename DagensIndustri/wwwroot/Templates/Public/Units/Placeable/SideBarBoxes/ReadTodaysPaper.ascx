<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ReadTodaysPaper.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.SideBarBoxes.ReadTodaysPaper" %>

<!-- Banner - Read paper --> 
<div class="banner banner-read-paper"> 
	<div class="wrapper"> 
		<div class="content"> 
			<h2>
                <asp:Literal ID="ReadTodaysPaperHeadingLiteral" runat="server" />
            </h2> 
			    <asp:Literal ID="ReadTodaysPaperTextLiteral" runat="server" /> 
			        <a href='<%= ReadTodaysPaperLink %>' class="btn">
                <span>
                    <asp:Literal ID="ReadTodaysPaperLinkText" runat="server" />
                </span>
            </a> 
		</div> 
		<div class="image-wrapper"> 
			<img src='<%= ettansURL %>' /> 
		</div> 
	</div> 
</div> 
<!-- // Banner - Read paper -->