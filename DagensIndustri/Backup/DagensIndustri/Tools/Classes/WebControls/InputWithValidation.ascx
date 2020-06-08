<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InputWithValidation.ascx.cs" Inherits="DagensIndustri.Tools.Classes.WebControls.InputWithValidation" %>

<asp:PlaceHolder ID="InputPlaceHolder" Visible="false" runat="server">
    <asp:Label AssociatedControlID="Input" Text='<%# Title %>' Visible="<%# TypeOfInput != InputType.CheckBox %>" runat="server" />
    <input id="Input" name='<%# Name %>' title='<%# DisplayMessage %>' class='<%# CssClass %>' pattern='<%# Pattern %>' autocomplete='<%# AutoComplete ? "on" : "off" %>' runat="server" />
    <asp:Label AssociatedControlID="Input" Text='<%# Title %>' Visible="<%# TypeOfInput == InputType.CheckBox %>" runat="server" />
</asp:PlaceHolder>

<asp:PlaceHolder ID="TextAreaPlaceHolder" Visible="false" runat="server">
    <asp:Label ID="TextAreaLabel" AssociatedControlID="TextArea" Text='<%# Title %>' runat="server" />
    <textarea id="TextArea" name='<%# Name %>' title='<%# DisplayMessage %>' class='<%# CssClass %>' pattern='<%# Pattern %>' autocomplete='<%# AutoComplete ? "on" : "off" %>' runat="server" />
</asp:PlaceHolder>
