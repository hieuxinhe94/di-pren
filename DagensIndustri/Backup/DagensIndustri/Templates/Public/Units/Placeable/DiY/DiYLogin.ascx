<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DiYLogin.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.DiY.DiYLogin" %>
<%@ Register TagPrefix="DI" TagName="UserMessage" Src="~/Templates/Public/Units/Placeable/UserMessage.ascx" %>
<%@ Register TagPrefix="DI" TagName="Input" Src="~/Tools/Classes/WebControls/InputWithValidation.ascx" %>


<DI:UserMessage ID="UserMessageControl" runat="server" />



<%--
<div class="form-nav">
    <ul>
        <li class="current"><a href="javascript:void(0);">Logga in</a></li>
    </ul>
</div>
--%>

<div class="form-box">
    <div class="row">
        <div class="col">
            <DI:Input ID="InputCusno" Title="Kundnummer" DisplayMessage="Ange kundnummer" MaxValue="10" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" Name="cn" TypeOfInput="Text" runat="server" />
        </div>
        <div class="col">
            <DI:Input ID="InputZip" Title="Postnummer" DisplayMessage="Ange postnummer" MaxValue="5" MinValue="5" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" Name="pn" TypeOfInput="Text" runat="server" />
        </div>
    </div>
    <div class="button-wrapper">
        <asp:Button ID="ButtonLogin" runat="server" Text="Logga in" onclick="ButtonLogin_Click" />
    </div>
</div>


