<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ShopReceipt.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.ShopFlow.ShopReceipt" %>

<!-- Page primary content goes here -->
<p class="intro">
	<asp:Literal ID="GasellInfoLiteral" runat="server" />
</p>
			
<h2>
    <asp:Literal ID="RegisteredParticipantsLiteral" runat="server" />
</h2>

			
<div class="divider"><hr /></div>			

<%--<asp:Repeater ID="ParticipantsRepeater" runat="server">

    <ItemTemplate>
        <h3><%# DataBinder.Eval(Container.DataItem, "Firstname") %> <%# DataBinder.Eval(Container.DataItem, "Lastname") %></h3>
        <p><%# DataBinder.Eval(Container.DataItem, "Company") %></p>
        <div class="divider"><hr /></div>	
    </ItemTemplate>

</asp:Repeater>--%>

<p><a href="#" class="btn print"><span>Skriv ut</span></a></p>