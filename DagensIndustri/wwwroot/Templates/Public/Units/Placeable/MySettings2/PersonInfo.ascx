<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PersonInfo.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.MySettings2.PersonInfo" %>
<%@ Register TagPrefix="DI" TagName="Input" Src="~/Tools/Classes/WebControls/InputWithValidation.ascx" %>


<h2>Personuppgifter</h2>

<p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed id nulla molestie massa lacinia aliquet rutrum sed felis. Duis eleifend dolor non nibh sollicitudin vulputate. Sed suscipit ultrices urna, id feugiat sem eleifend ac. Ut et purus nulla.</p>


<div class="form-box">
    <div class="row">
        <div class="col">
            <DI:Input ID="InputEmail" CssClass="text" Required="true" Name="email" TypeOfInput="Email" MaxValue="60" StripHtml="true" AutoComplete="true" Title="E-postadress" DisplayMessage="<%$ Resources: EPiServer, common.validation.emailrequired%>" runat="server" />
        </div>
    </div>
    <div class="row">
        <div class="col">
            <DI:Input ID="InputPhone" CssClass="text" Required="false" Name="phoneMob" TypeOfInput="Telephone" MinValue="7" MaxValue="20" StripHtml="true" AutoComplete="true" Title="Mobilnummer" DisplayMessage="Ange korrekt formaterat mobilnummer" runat="server" />
        </div>
    </div>
    <div class="button-wrapper">
        <asp:Button ID="ButtonSave" CssClass="btn" Text="<%$ Resources: EPiServer, common.save %>" OnClick="ButtonSave_Click" runat="server" />
    </div>
</div>

