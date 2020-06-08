<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LoginInfo_OLD.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.MySettings2.LoginInfo_OLD" %>
<%@ Register TagPrefix="DI" TagName="Input" Src="~/Tools/Classes/WebControls/InputWithValidation.ascx" %>


<h2>Inloggningsuppgifter</h2>

<p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed id nulla molestie massa lacinia aliquet rutrum sed felis. Duis eleifend dolor non nibh sollicitudin vulputate. Sed suscipit ultrices urna, id feugiat sem eleifend ac. Ut et purus nulla.</p>


<div class="form-box">
    <div class="row">
        <div class="col">
            <DI:Input ID="InputUsername" CssClass="text" Required="true" Name="username" TypeOfInput="Text" MaxValue="60" StripHtml="true" AutoComplete="true" Title="Användarnamn" DisplayMessage="Ange användarnamn" runat="server" />
        </div>
    </div>

    <div class="row">
        <div class="col">
            <DI:Input ID="InputPassOld" CssClass="text" Required="true" Name="passOld" TypeOfInput="Password" MaxValue="20" StripHtml="true" AutoComplete="true" Title="Nuvarande lösenord" DisplayMessage="Ange nuvarande lösenord för att kunna spara" runat="server" />
        </div>
        <div class="col">
            <br /><br />(Säkerhetskrav för att kunna spara)
        </div>
    </div>

    <div class="row">
        <div class="col">
            <DI:Input ID="InputPassNew1" CssClass="text" Required="false" Name="passNew1" TypeOfInput="HardPassword" MaxValue="20" StripHtml="true" AutoComplete="true" Title="Nytt lösenord" DisplayMessage="Ange giltigt lösenord" runat="server" />
        </div>
        <div class="col">
            <br /><br />(Lämna tomt för att behålla befintligt lösenord)
        </div>
    </div>

    <div class="row">
        <div class="col">
            <DI:Input ID="InputPassNew2" CssClass="text" Required="false" Name="passNew2" TypeOfInput="HardPassword" MaxValue="20" StripHtml="true" AutoComplete="true" Title="Nytt lösenord (repetera)" DisplayMessage="Ange giltigt lösenord" runat="server" />
        </div>
    </div>

    <div class="button-wrapper">
        <asp:Button ID="ButtonSave" CssClass="btn" Text="<%$ Resources: EPiServer, common.save %>" OnClick="ButtonSave_Click" runat="server" />
    </div>
</div>

