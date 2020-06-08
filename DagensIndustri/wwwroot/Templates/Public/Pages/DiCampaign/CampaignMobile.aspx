<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CampaignMobile.aspx.cs" Inherits="DagensIndustri.Templates.Public.Pages.DiCampaign.CampaignMobile" %>
<%@ Register src="../../Units/Static/GoogleAnalytics.ascx" tagname="GoogleAnalytics" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Provläs Di</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href="../../Styles/campaign/mobile.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="wrapper">

        <img src="../../Styles/campaign/images/di-logo-120px.gif" width="120" height="51" alt="" border="0" />
            
        <h1>Provläs Di</h1>
        
        <p><asp:Label ID="LabelOffer" runat="server" /></p>
        
        <asp:Label ID="LabelMess" Visible="false" runat="server" />

        <asp:PlaceHolder ID="PlaceHolderIncludedTable" runat="server">
            <table border="0" cellpadding="0" cellspacing="0" style="border:1px solid #999999; padding:10px;">
            <tr>
            <td>
                <b>Det här ingår</b><br />
                <asp:Literal ID="Literal1" Visible="<%# !IsWeekend %>" runat="server">✓ Dagens industri, mån–lör<br /></asp:Literal>
                ✓ Di Weekend, varje fre<br />
                <asp:Literal ID="Literal2" Visible="<%# !IsWeekend %>" runat="server">✓ Di Dimension, 1 gång/månad<br /></asp:Literal>
                <asp:Literal ID="Literal3" Visible="<%# !IsWeekend %>" runat="server">✓ Dagens industri i läsplattan<br /></asp:Literal>
                ✓ Di Weekend i läsplattan<br />
                <!--✓ PDF-tidning 22.30 kvällen innan-->
            </td>
            </tr>
            </table>
            <br />
        </asp:PlaceHolder>

        
        <asp:PlaceHolder ID="PlaceHolderForm" runat="server">
            <div class="row">
                <div class="col1">
                    Namn
                </div>
                <div class="col2">
                    <asp:TextBox ID="TextBoxName" CssClass="formTextBox" runat="server" />
                </div>
            </div>

            <div class="row">
                <div class="col1">
                    Adress
                </div>
                <div class="col2">
                    <asp:TextBox ID="TextBoxAddress" CssClass="formTextBox" runat="server" />
                </div>
            </div>

            <div class="row">
                <div class="col1">
                    Postnr
                </div>
                <div class="col2">
                    <asp:TextBox ID="TextBoxZip" CssClass="formTextBox" runat="server" />
                </div>
            </div>

            <div class="row">
                <div class="col1">
                    Mobiltel
                </div>
                <div class="col2">
                    <asp:TextBox ID="TextBoxPhone" CssClass="formTextBox" runat="server" />
                </div>
            </div>

            <div class="row">
                <div class="col1">
                    E-post
                </div>
                <div class="col2">
                    <asp:TextBox ID="TextBoxEmail" CssClass="formTextBox" runat="server" />
                </div>
            </div>

            <div class="row">
                <div class="col1">
                    &nbsp;
                </div>
                <div class="col2">
                    
                    <asp:Button ID="ButtonSend" Text="Skicka" CssClass="btn" runat="server" onclick="ButtonSend_Click" />
                    <br />
                    <br />
                    
                    <asp:PlaceHolder ID="PlaceHolderPhone" runat="server">
                        <a href="tel:+46857365100">Ring oss: 08-573 651 00</a>
                        <br />
                        <br />
                    </asp:PlaceHolder>
                   
                    <a href="/System/Footer/Prenumerationsvillkor" target="_blank">Prenumerationsvillkor</a>
                    <br />

                </div>
            </div>
        </asp:PlaceHolder>


    </div>

    <uc1:GoogleAnalytics ID="GoogleAnalytics1" runat="server" />

    </form>
</body>
</html>
