<%@ Page Language="C#" MasterPageFile="~/Templates/Public/MasterPages/MasterPage.Master" AutoEventWireup="false" CodeBehind="AddressTempByEmail.aspx.cs"
  Inherits="DagensIndustri.Templates.Public.Pages.UserSettings.AddressTempByEmail" %>
<%@ Register TagPrefix="di" TagName="Input" Src="~/Tools/Classes/WebControls/InputWithValidation.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
  <h1><EPiServer:Property PropertyName="Heading" runat="server"/></h1>
  <EPiServer:Property PropertyName="MainBody" CssClass="intro" runat="server"/>

  <asp:PlaceHolder ID="PlaceHolderForm" runat="server">
    <div class="form-box">
      <div class="row">
        <div class="col">
          <%-- Info: InputWithValidation.ascx "mysteriously" needs master.js to validate. Should be refactored. --%>
          <di:Input ID="InputEmail" CssClass="text" Required="true" Name="email" TypeOfInput="Email" MaxValue="60" StripHtml="true"
            AutoComplete="true" Title="E-postadress" DisplayMessage="Vänligen ange din e-postadress"
            runat="server" />
        </div>
      </div>
      <div class="row">
        <div class="col">
          <di:Input ID="InputCusno" CssClass="text" Required="true" Name="cusno" TypeOfInput="Numeric" MaxValue="15" StripHtml="true"
            AutoComplete="true" Title="Kundnummer" DisplayMessage="Vänligen ange ditt kundnummer"
            runat="server" />
        </div>
      </div>
      
      <div class="button-wrapper">
                <div id="divSubmitBtn">
                    <asp:Button ID="ButtonSave" CssClass="btn" Text="<%$ Resources: EPiServer, common.save %>" OnClientClick="return jsPreventDoublePost('divSubmitBtn', 'divFormSent');" runat="server" />
	            </div>
            
                <div id="divFormSent" style="float:right; visibility:hidden;">
                    <img src="/Templates/Public/Images/loader.gif" alt="" />
                    <i>&nbsp;<asp:Literal ID="Literal1" Text="<%$ Resources: EPiServer, common.sendingform %>" runat="server" /></i>
                </div>

            </div>

    </div>
  </asp:PlaceHolder>
  
  <asp:PlaceHolder ID="phError" Visible="False" runat="server">
    <asp:Literal ID="litErrorMessage" runat="server"></asp:Literal>
  </asp:PlaceHolder>
</asp:Content>
