<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="XForm.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.XFormControl" %>

<style type="text/css">

    #id_matrix{}
    #id_matrix .xformvalidator {display:none;}
    
</style>



<asp:Panel runat="server" ID="FormPanel" CssClass="xForm">
	<xforms:xformcontrol ID="FormControl" runat="server" EnableClientScript="false" ValidationGroup="XForm" />
</asp:Panel>
<asp:Panel runat="server" ID="StatisticsPanel" Visible="false">
		<asp:literal id="NumberOfVotes" runat="server" />
		<!-- Set StatisticsType to format output: N=numbers only, P=percentage -->
		<EPiServer:XFormStatistics StatisticsType="P" runat="server" id="Statistics" PropertyName="XForm" />
</asp:Panel>
<br />
<asp:Button runat="server" ID="SwitchButton" CssClass="button" OnClick="SwitchView" CausesValidation="false" Text="<%$ Resources: EPiServer, form.showstat %>" />

<asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="XForm" />