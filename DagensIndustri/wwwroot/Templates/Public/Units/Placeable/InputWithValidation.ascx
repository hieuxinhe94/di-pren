<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InputWithValidation.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.InputWithValidation" %>

<asp:Label ID="Label1" runat="server" AssociatedControlID="TxtControl"><%= Translate(HeadingTranslateKey)%></asp:Label>

<asp:PlaceHolder runat="server" ID="PhReqVal" Visible="false">
    <asp:RequiredFieldValidator runat="server" ControlToValidate="TxtControl" ID="ReqVal" Display="Dynamic">
        <img src="/Templates/DI/Images/mandatory.png" title='<%= Translate("/various/validators/mandatory") %>' alt='<%= Translate("/various/validators/mandatory") %>' />
    </asp:RequiredFieldValidator>
</asp:PlaceHolder>

<asp:PlaceHolder runat="server" ID="PhRegExpVal" Visible="false">
    <asp:RegularExpressionValidator runat="server" ControlToValidate="TxtControl" ID="RegVal" Display="Dynamic" ValidationExpression="^([_A-Za-z0-9-])+(\.[_A-Za-z0-9-]+)*@(([A-Za-z0-9]){1,}([-.])?([A-Za-z0-9]){1,})+((\.)?([A-Za-z0-9]){1,}([-])?([A-Za-z0-9]){1,})*(\.[A-Za-z]{2,4})$">
        <img src="/Templates/DI/Images/mandatory.png" title='<%= Translate("/various/validators/mandatory") %>' alt='<%= Translate("/various/validators/mandatory") %>' />                                
        <EPiServer:Translate ID="Translate1" runat="server" Text="/various/validators/format" />
    </asp:RegularExpressionValidator>
</asp:PlaceHolder>
<br />
<asp:TextBox runat="server" ID="TxtControl"></asp:TextBox>