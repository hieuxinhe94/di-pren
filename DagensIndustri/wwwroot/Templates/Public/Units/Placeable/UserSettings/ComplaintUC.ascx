<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ComplaintUC.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.UserSettings.ComplaintUC" %>
<%@ Register TagPrefix="di" TagName="UserMessage" Src="~/Templates/Public/Units/Placeable/UserMessage.ascx" %>
<%@ Register TagPrefix="DI" TagName="Input" Src="~/Tools/Classes/WebControls/InputWithValidation.ascx" %>

<di:UserMessage ID="UserMessageControl" runat="server" />



<asp:PlaceHolder ID="PlaceHolderForm" runat="server">
    
<script type="text/javascript" src="/Templates/Public/js/PreventDoublePost.js"></script>

    <div class="form-box">
        <div class="row">
            <div class="col">
                <div class="medium">
                    <DI:Input ID="Date1" CssClass="text date" Required="true" Name="date-temp-from" TypeOfInput="Date" Title="Från och med <i>(YYYY-MM-DD)</i>" DisplayMessage="Ange datum" runat="server" />
                </div>
            </div>
            <div class="col">
                <div class="medium">
                    &nbsp;
                </div>
            </div>
        </div>
            
        <div class="row">
            <div class="col">
                <div class="medium">
                    <b>Antal dagar</b><br />
                    <asp:DropDownList ID="DropDownListNumDays" Width="150" runat="server">
                        <asp:ListItem Text="1" Value="1"></asp:ListItem>
                        <asp:ListItem Text="2" Value="2"></asp:ListItem>
                        <asp:ListItem Text="3" Value="3"></asp:ListItem>
                        <asp:ListItem Text="4" Value="4"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col">
                <div class="medium">
                    &nbsp;
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col">
                <div class="medium">
                    <b>Ange orsak</b><br />
                    <asp:DropDownList ID="DropDownListReason" Width="150" runat="server">
                        <asp:ListItem Text="Bilaga saknas"></asp:ListItem>
                        <asp:ListItem Text="Blöt tidning"></asp:ListItem>
                        <asp:ListItem Text="Fel tidning"></asp:ListItem>
                        <asp:ListItem Text="Sen leverans"></asp:ListItem>
                        <asp:ListItem Text="Trasig tidning"></asp:ListItem>
                        <asp:ListItem Text="Utebliven tidning"></asp:ListItem>
                        <asp:ListItem Text="Annan orsak"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col">
                <div class="medium">
                    &nbsp;
                </div>
            </div>
        </div>

        <div class="button-wrapper">

            <div id="divSubmitBtn">
                <asp:Button ID="ButtonSave" CssClass="btn" Text="Skicka" OnClick="ButtonSave_Click" OnClientClick="return jsPreventDoublePost('divSubmitBtn', 'divFormSent');" runat="server" />
	        </div>
            
            <div id="divFormSent" style="float:right; visibility:hidden;">
                <img src="/Templates/Public/Images/loader.gif" alt="" />
                <i>&nbsp;<asp:Literal ID="Literal1" Text="Formuläret skickas..." runat="server" /></i>
            </div>

        </div>

    </div>
</asp:PlaceHolder>