<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustomerAdmin.aspx.cs" Inherits="DagensIndustri.Tools.Admin.CustomerInfo.CustomerAdmin" %>
<%@ Register TagPrefix="di" Src="~/Tools/Admin/Student/StudentVerifierBox.ascx" TagName="StudentVerifierBox" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Kundinformation</title>
    <link href="/Tools/Admin/Styles/StyleSheetPublic.css" rel="stylesheet" type="text/css" />    
    
    <style type="text/css">
        .tblresult {border:1px solid #888;border-collapse:collapse;margin-top:10px;}
        .tblresult td {border:1px solid #888;padding:5px;}  
        a.unlock {font-weight:bold;white-space:nowrap;color:Blue;}     
        .tblresult .heading{background-color: #dec7bb;}
    </style>    
</head>
<body>
    <form id="form1" runat="server">
        
        <div id="MainBodyArea">

            <h1>Kundinformation</h1>            
            För att kunder ska kunna logga in måste de ha minst en aktiv 'inloggningsgiltig'-prenumeration i Cirix.<br />
            Ibland saknas viss information i webbdatabasen - synka sådana kunder nedan.
            <br />
            <br />
            <table border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td valign="top">
                        <b>Synka kund via kundnummer</b><br />
                        <table border="0" cellpadding="0" cellspacing="3">
                        <tr>
                        <td>Kundnummer</td>
                        <td><asp:TextBox ID="TextBoxCusno" Width="60" runat="server"></asp:TextBox></td>
                        <td><asp:Button ID="ButtonSyncByCusno" runat="server" Text="Synka från Cirix" onclick="ButtonSyncByCusno_Click" /></td>
                        </tr>
                        </table>

                        <br />

                        <asp:Panel ID="Panel1" runat="server" DefaultButton="BtnSearch">
                            <strong>Synka kund via sök</strong><br />
                            <asp:TextBox runat="server" ID="TxtSearchString"></asp:TextBox>
                            <asp:DropDownList runat="server" ID="DdlCriteria">
                                <asp:ListItem Text="E-post" Value="@email"></asp:ListItem>
                                <asp:ListItem Text="Kundnummer" Value="@cusno"></asp:ListItem>
                                <asp:ListItem Text="Användarid" Value="@userid"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:Button runat="server" ID="BtnSearch" Text="Sök" OnClick="BtnSearchOnClick" /><br />
                        </asp:Panel>
                        <i>Söktips: För att söka på alla som har en e-post som börjar<br />
                        på anna, skriv anna* i sökrutan. För att söka på alla med<br />
                        ett kundnummer som innehåller 15, skriv *15* i sökrutan.</i><br />

                        <br />

                        <asp:Label runat="server" EnableViewState="false" ID="LblError" CssClass="error"></asp:Label>
                                                     
                        <asp:Repeater runat="server" ID="RepSearchResult">
                                <HeaderTemplate>
                                    <table class="tblresult">
                                        <tr class="bold heading">
                                            <td>E-post</td>
                                            <td>Användarid</td>
                                            <td>Lösenord</td>
                                            <td>Kundnummer</td>
                                            <%--<td>Papperskod</td>--%>
                                            <td>Synka från Cirix</td>
                                            <%--<td>ProdNo</td>--%>
                                            <%--<td>Antal login</td>--%>
                                            <%--<td>Kopplad Trader anv.</td>--%>
                                        </tr>               
                                </HeaderTemplate>
                                <ItemTemplate>
                                        <tr class='<%# Container.ItemIndex % 2 == 0 ? "evenrow" : "oddrow" %>'>
                                            <td><%# DataBinder.Eval(Container.DataItem, "email")%></td>
                                            <td><%# DataBinder.Eval(Container.DataItem, "userid")%></td>
                                            <td><%# DataBinder.Eval(Container.DataItem, "passwd")%></td>
	                                        <td><%# DataBinder.Eval(Container.DataItem, "cusno")%></td>
                                            <%--<td><%# DataBinder.Eval(Container.DataItem, "paperCode")%></td>--%>
                                            <td>
                                                <%--<# (string)DataBinder.Eval(Container.DataItem, "subsactive") != "Y" ? "Nej" : "Ja">--%>
                                                <asp:LinkButton runat="server" ID="LbUnlock" OnClick="LbUnlockOnClick" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "cusno") %>' CssClass="unlock" OnClientClick="javascript:return confirm('Är du säker på att du synkronisera kund?');" Text="Synka från Cirix"></asp:LinkButton>
                                            </td>
                                            <%--<td><%# DataBinder.Eval(Container.DataItem, "productno")%></td>--%>
                                            <%--<td><%# DataBinder.Eval(Container.DataItem, "no_logins_total")%></td>--%>
                                            <%--<td><%# DataBinder.Eval(Container.DataItem, "DITUser")%></td>--%>
                                        </tr>                                
                                </ItemTemplate>
                                <FooterTemplate>
                                    </table>                 
                                </FooterTemplate>
                            </asp:Repeater>
                    </td>
                    <td valign="top">
                        <di:StudentVerifierBox ID="studentVerifierBox" runat="server" />
                    </td>
                    <td width="20"></td>
                    <td valign="top">
                        <b>Kod för Di-konto</b><br />
                        <table border="0" cellpadding="0" cellspacing="3">
                        <tr>
                        <td>Kundnummer</td>
                        <td><asp:TextBox ID="TextBoxSsoCusno" Width="60" runat="server"></asp:TextBox></td>
                        <td><asp:Button ID="ButtonSsoGetCode" runat="server" Text="Hämta kod" onclick="ButtonSsoGetCode_Click" /></td>
                        </tr>
                        </table>

                        <asp:Label ID="LabelSsoCodeMess" ForeColor="Red" runat="server"></asp:Label>
                        
                        <asp:PlaceHolder ID="PlaceHolderSsoCodeInfo" Visible="false" runat="server">
                            <table border="0" cellpadding="0" cellspacing="3">
                            <tr>
                            <td>Kundens kod</td>
                            <td><asp:TextBox ID="TextBoxSsoCode" Enabled="false" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr>
                            <td>Kundens mailadress</td>
                            <td><asp:TextBox ID="TextBoxSsoEmail" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr>
                            <td></td>
                            <td><asp:Button ID="ButtonSsoSendCodeMail" OnClick="ButtonSsoSendCodeMail_Click" runat="server" Text="Maila kod till kund" /></td>
                            </tr>
                            </table>
                        </asp:PlaceHolder>
                        
                        <br/>
                        <br/>
                        <b>SAS EuroBonus</b><br/>
                        <table border="0" cellpadding="3" cellspacing="0">
                          <tr>
                            <td>EuroBonusnummer (9 siffror)</td>
                            <td><asp:TextBox ID="tbEuroBonusNum" Width="80" runat="server" /></td>
                          </tr>
                          <tr>
                            <td>Kundnummer</td>
                            <td><asp:TextBox ID="tbEuroBonusCusno" Width="80" runat="server" /></td>
                          </tr>
                          <tr>
                            <td>&nbsp;</td>
                            <td><asp:Button ID="ButtonEuroBonus" OnClick="ButtonEuroBonus_Click" runat="server" Text="Ge poäng" /></td>
                          </tr>
                        </table>
                        <asp:Label ID="LabelEuroBonusMess" ForeColor="Red" Visible="False" runat="server"></asp:Label>

                    </td>

                </tr>
            </table>
            


	    </div>
    </form>
</body>
</html>
