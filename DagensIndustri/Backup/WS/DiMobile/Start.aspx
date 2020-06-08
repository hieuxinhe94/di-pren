<%@ Page Title="" Language="C#" MasterPageFile="~/DiMobile/DiMobile.Master" AutoEventWireup="true" CodeBehind="Start.aspx.cs" Inherits="WS.DiMobile.Start" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <div class="container">

        <div class="row">
            <div class="span12" style="text-align:center;">
                <img src="images/logga_utan_skugga.png" style="max-width:200px; padding-top:5px; padding-bottom:5px;" />
            </div>
        </div>

        
        <div class="row" style="background-color:#f8f8f8;">
            <div class="span12">
                <div style="padding-left:10px; padding-right:10px;">
                    <h4>Läs dagens tidning</h4>
                    <p>Mobilen ger dig gränslös tillgång till Di:s nyheter och inspiration för att utveckla dina affärer. För att läsa i 
                    mobilen behöver du en prenumeration och ett Di-konto.</p>
                </div>
            </div>
        </div>
        

        <asp:Panel ID="PanelNotLoggedIn" Visible="false" runat="server">
            <div class="row">
                <div class="span12">
                    <br />
                    <div style="width:250px;">
                        <asp:HyperLink ID="HyperLinkToLogin" CssClass="btn btn-large btn-block btn-success" runat="server">Logga in</asp:HyperLink>
                        <br />
                        <asp:HyperLink ID="HyperLinkToCreateAccount" CssClass="btn btn-large btn-block btn-success" runat="server">Skapa Di-konto</asp:HyperLink>
                        <br />
                        <asp:HyperLink ID="HyperLinkToBuyDi" CssClass="btn btn-large btn-block btn-success" runat="server">Köp prenumeration</asp:HyperLink>
                    </div>
                </div>
            </div>
        </asp:Panel>


        <asp:Panel ID="PanelLoggedInNotActivated" Visible="true" runat="server">
            <div class="row">
                <div class="span12">
                    <br />
                    <asp:Literal ID="LiteralErrMess" Visible="false" runat="server"></asp:Literal>
                    <p>
                        <asp:Literal ID="LiteralLoggedInInfo" runat="server"></asp:Literal>
                    </p>
                </div>
            </div>

            <div class="row">
                <div class="span4">
                    <p>
                        <br />
                        För att aktivera dina digitala tjänster behöver du ange ditt kundnummer eller den e-postadress 
                        du uppgav när prenumerationen tecknades.
                        <br />
                        <br />
                        Kundnummer ELLER e-post<br />
                    </p>
                    <asp:TextBox ID="TextBoxCusnoOrEmail" class="input-big" runat="server"></asp:TextBox> 
                    <br />
                    <asp:Button ID="ButtonActivateAccount" CssClass="btn btn-large btn-success" Text="Aktivera ditt Di-konto" onclick="ButtonActivateAccount_Click" runat="server" />
                    <br />
                    <br />
                </div>
                <div class="span8" style="background-color:#f8f8f8;">
                    <div style="padding-left:10px; padding-right:10px;">
                        <h4>Kontakta kundtjänst</h4>
                        <p>
                            Ring vår kundtjänst på tel <a href="tel:0857365100">08-573 651 00</a> om du inte vet kundnummer eller e-postadress.<br />
                        </p>
                    </div>
                </div>
                <%--<div class="span6">
                    <br />
                    Kontakt
                </div>--%>
            </div>
        </asp:Panel>
        
        <asp:Panel ID="PanelNotAllowedToReadPaper" Visible="true" runat="server">
            <div class="row">
                <div class="span12">
                    <br />
                    <b>
                      <asp:Literal ID="LiteralInfoMakerReturnHeader" runat="server"></asp:Literal>
                    </b>
                    <p>
                      <asp:Literal ID="LiteralInfoMakerReturnBody" runat="server"></asp:Literal>
                    </p>
                </div>
            </div>    
        </asp:Panel>

    </div>
</asp:Content>
