<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TypeControl.ascx.cs" Inherits="PrenDiSe.Tools.Properties.TypeControl" %>

<asp:UpdatePanel runat="server" ID="updPanelTypes" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:DropDownList runat="server" ID="DdlTypes" DataTextField="typeName" DataValueField="typeId"></asp:DropDownList>
        <asp:TextBox runat="server" ID="TxtComment" onfocus="javascript:this.select()" Text="<%$ Resources: EPiServer, campaigns.various.comment %>"></asp:TextBox>
    </ContentTemplate>
</asp:UpdatePanel>