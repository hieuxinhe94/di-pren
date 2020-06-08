<%@ Page Language="C#" MasterPageFile="~/Templates/Public/MasterPages/MasterPage.Master" AutoEventWireup="true" CodeBehind="Complaint.aspx.cs" Inherits="DagensIndustri.Templates.Public.Pages.UserSettings.Complaint" %>
<%@ Register TagPrefix="di" TagName="UserMessage" Src="~/Templates/Public/Units/Placeable/UserMessage.ascx" %>
<%@ Register TagPrefix="di" TagName="MySettingsMenu" src="~/Templates/Public/Pages/UserSettings/MySettingsMenu.ascx" %>
<%@ Register TagPrefix="di" TagName="Input" Src="~/Tools/Classes/WebControls/InputWithValidation.ascx" %>
<%@ Register TagPrefix="di" TagName="Mainbody" Src="~/Templates/Public/Units/Placeable/MainBody.ascx" %>
<%@ Register TagPrefix="di" TagName="heading" Src="~/Templates/Public/Units/Placeable/Heading.ascx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadingContentPlaceHolder" runat="server">
    <%--<di:heading ID="Heading1" runat="server" />--%>
    <script type="text/javascript" src="/Templates/Public/js/PreventDoublePost.js"></script>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="TopContentPlaceHolder" runat="server" ></asp:Content>


<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server">

    <di:MySettingsMenu ID="MySettingsMenu1" runat="server" />

    <h2><%= CurrentPage["Heading"] ?? CurrentPage.PageName %></h2>
    <di:Mainbody ID="Mainbody1" runat="server" />


    <di:UserMessage ID="UserMessageControl" runat="server" />

    <asp:PlaceHolder ID="PlaceHolderForm" runat="server">
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
                    <asp:Button ID="ButtonSave" CssClass="btn" Text="<%$ Resources: EPiServer, common.send %>" OnClick="ButtonSave_Click" OnClientClick="return jsPreventDoublePost('divSubmitBtn', 'divFormSent');" runat="server" />
	            </div>
            
                <div id="divFormSent" style="float:right; visibility:hidden;">
                    <img src="/Templates/Public/Images/loader.gif" alt="" />
                    <i>&nbsp;<asp:Literal ID="Literal1" Text="<%$ Resources: EPiServer, common.sendingform %>" runat="server" /></i>
                </div>

            </div>

        </div>
    </asp:PlaceHolder>

</asp:Content>

