<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FindCustomer.aspx.cs" Inherits="DagensIndustri.Tools.Admin.CustomerInfo.FindCustomer" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Kundinformation</title>
    <link href="/Tools/Admin/Styles/StyleSheetPublic.css" rel="stylesheet" type="text/css" />  
    <link href="/Tools/Admin/Styles/jquery-ui-1.8.custom.css" rel="stylesheet" type="text/css" />                
</head>
<body class="white">
    <form id="form1" runat="server">
    
            
        <script type="text/javascript">

            function showProgress() {
                //must reset src in image after postback, otherwise IE will stop animation
                ProgressImg = document.getElementById('progressimg');
                setTimeout("ProgressImg.src = ProgressImg.src", 100);
                showmodal("#progress", "200px", "100", false);
            }

            function showmodal(id, modalwidth, modalheight, showbutton) {

                $(id).dialog("destroy");

                if (showbutton) {
                    $(id).dialog({
                        width: modalwidth, height: modalheight, modal: true,
                        buttons: {
                            Ok: function () {
                                $(this).dialog('close');
                            }
                        }
                    });
                }
                else {
                    $(id).dialog({ width: modalwidth, height: modalheight, modal: true });
                }

            }	    	       
    	    
	    </script>        
        
        <div id="MainBodyArea">

            <h1>Kundsök</h1>   
                 
            <asp:Panel ID="Panel1" runat="server" DefaultButton="BtnSearch">
            
                <table border="0" cellpadding="0" cellspacing="0">
                <tr>
                <td valign="top">
                    <fieldset style="width:340px">
                        <legend style="color:#000;">Ange sökkriterier</legend>
                            <table>
                                <tr>
                                    <td>
                                        <strong>Företag:</strong><br />
                                        <asp:TextBox runat="server" ID="TxtCompany"></asp:TextBox>                                   
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <strong>Förnamn:</strong><br />
                                        <asp:TextBox runat="server" ID="TxtFirstName"></asp:TextBox>                      
                                    </td>       
                                    <td>
                                        <strong>Efternamn:</strong><br />
                                        <asp:TextBox runat="server" ID="TxtLastName"></asp:TextBox>                                      
                                    </td>                 
                                </tr>
                                <tr>
                                    <td>
                                        <strong>Gatuadress:</strong><br />
                                        <asp:TextBox runat="server" ID="TxtStreet"></asp:TextBox>                        
                                    </td>
                                    <td>
                                        <strong>Gatunummer:</strong><br />
                                        <asp:TextBox runat="server" ID="TxtStreetNo"></asp:TextBox>                        
                                    </td>
                                </tr>                         
                                <tr>
                                    <td>
                                        <strong>Postnummer:</strong><br />
                                        <asp:TextBox runat="server" ID="TxtZip"></asp:TextBox>                        
                                    </td>
                                    <td>
                                        <strong>Ort:</strong><br />
                                        <asp:TextBox runat="server" ID="TxtCity"></asp:TextBox>                        
                                    </td>
                                </tr>     
                                <tr>
                                    <td>
                                        <strong>E-post:</strong><br />
                                        <asp:TextBox runat="server" ID="TxtEmail"></asp:TextBox>                        
                                    </td>
                                    <td>
                                        <strong>Telefon:</strong><br />
                                        <asp:TextBox runat="server" ID="TxtPhone"></asp:TextBox>
                                    </td>
                                </tr>                                                                  
                            </table>   
                        <asp:Button runat="server" ID="BtnSearch" Text="Sök" OnClick="BtnSearchOnClick" OnClientClick="javascript:showProgress();" /><br />
                        <asp:Label runat="server" EnableViewState="false" ID="LblError" CssClass="error"></asp:Label>
                    </fieldset>
                </td>
                <td width="30">&nbsp;</td>
                <td valign="top">
                    <fieldset style="width:280px">
                        <legend style="color:#000;">Skicka e-post</legend>
                        
                        <div style="margin-bottom:5px;">
                            <b>E-postadress *</b><br />
                            <asp:TextBox ID="TextBoxSendMailEmail" Width="250" runat="server"></asp:TextBox>
                        </div>

                        <table border="0" cellpadding="0" cellspacing="0">
                        <tr>
                        <td>
                            <b>Kundnummer</b><br />
                            <asp:TextBox ID="TextBoxSendMailCusno" Width="90" Enabled="false" runat="server"></asp:TextBox>
                        </td>
                        <td width="20"></td>
                        <td>
                            <b>Kod</b><br />
                            <asp:TextBox ID="TextBoxSendMailCode" Width="90" Enabled="false" runat="server"></asp:TextBox>
                        </td>
                        </tr>
                        </table>
                        
                        <br />
                        
                        <asp:CheckBoxList ID="CheckBoxListSendMail" runat="server">
                            <asp:ListItem Text="Länk till kampanjsida" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Länk till aktivera digital prenumeration" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Länk till glömt lösenord" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Länk till Di-Guld" Value="1"></asp:ListItem>
                        </asp:CheckBoxList>
                        
                        <br />

                        <table border="0" cellpadding="0" cellspacing="0">
                        <tr>
                        <td><asp:Button ID="ButtonSendMail" runat="server" Text="Skicka e-post" OnClick="ButtonSendMail_OnClick"/></td>
                        <td width="10"></td>
                        <td><asp:Button ID="ButtonSendMailClearForm" runat="server" Text="Rensa formulär" OnClick="ButtonSendMailClearForm_OnClick"/></td>
                        </tr>
                        </table>

                        <br />
                        <asp:Label ID="LabelSendMailMess" ForeColor="Red" runat="server"></asp:Label>

                    </fieldset>

                    
                    <%--Gå till:<br />
                    - kampanjsida<br />
                    - skapa Di-konto<br />
                    - glömt lösenord<br />

                    <br />
                    Skicka mail med länk till:<br />
                    - kampanjsida<br />
                    - skapa Di-konto<br />
                    - glömt lösenord till Di-konto<br />
                    - Di guld startsida<br />--%>
                    

                </td>
                <td width="20">&nbsp;</td>
                <td width="280" valign="top">
                    <b>Länkar</b>
                    <br />
                    <br />
                    <a href='http://dagensindustri.se/kampanj/bastad' target='_blank'>Öppna kampanjsida</a>
                    <br />
                    <br />
                    <a href='https://login.di.se/password/forgot-password' target='_blank'>Öppna glömt lösenord</a>
                    <br />
                    <br />
                    <a href='http://dagensindustri.se/diguld' target='_blank'>Öppna Di Guld</a>
                    <br />
                    <br />
                    När en kund aktiverat sin digitala prenumeration måste kunden loggas ut från dagensindustri.se innan nästa kund får hjälp.<br />
                    <a href='http://dagensindustri.se' target='_blank'>Öppna dagensindustri.se</a>
                    <br />
                    <br />
                    Om det strular efter utloggning från dagensindustri.se - testa att logga ut även här.<br />
                    <a href='https://login.di.se' target='_blank'>Öppna login.di.se</a>
                </td>
                </tr>
                </table>
                                                                          
                                                         
                <br /><br />
                <asp:GridView runat="server" EnableViewState="true" CssClass="CustomerInfo" AlternatingRowStyle-BackColor="#F9F8F8" HeaderStyle-BackColor="#BCBCBC" GridLines="Both" ID="GvCustomers" AutoGenerateColumns="false" OnRowDataBound="GvCustomers_RowDataBound">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %> 
                            </ItemTemplate>
                        </asp:TemplateField>  
                        <asp:TemplateField>
                            <HeaderTemplate>
                                Kundnr
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="LinkButton1" runat="server" OnClick="LbShowSubscription" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CUSNO")%>'>
                                    <%# DataBinder.Eval(Container.DataItem, "CUSNO")%>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>                                      
                        <asp:BoundField HeaderText="Namn1" DataField="ROWTEXT1" />
                        <asp:BoundField HeaderText="Namn2" DataField="ROWTEXT2" />
                        <%--<asp:BoundField HeaderText="Namn3" DataField="ROWTEXT3" />--%>
                        <asp:BoundField HeaderText="Gata1" DataField="STREET1" />
                        <asp:BoundField HeaderText="Gata2" DataField="STREET2" />
                        <asp:BoundField HeaderText="Nr" DataField="HOUSENO" />
                        <%--<asp:BoundField HeaderText="Landskod" DataField="COUNTRYCODE" />--%>
                        <asp:BoundField HeaderText="Postnr" DataField="ZIPCODE" />
                        <asp:BoundField HeaderText="Ort" DataField="POSTNAME" />
                        <%--<asp:BoundField HeaderText="Telefon hem" DataField="H_PHONE" />
                        <asp:BoundField HeaderText="Telefon arbete" DataField="W_PHONE" />--%>
                        <asp:BoundField HeaderText="Telefon mobil" DataField="O_PHONE" />
                        <asp:BoundField HeaderText="E-post" DataField="EMAILADDRESS" />
                        <asp:TemplateField>
                            <HeaderTemplate>Di-konto</HeaderTemplate>
                            <ItemTemplate>
                                <%--<asp:LinkButton ID="LinkButton1" runat="server" OnClick="LbShowSubscription" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CUSNO")%>'>
                                    <%# DataBinder.Eval(Container.DataItem, "CUSNO")%>
                                </asp:LinkButton>--%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField>
                            <HeaderTemplate></HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="LinkButtonPopMailForm" runat="server" OnClick="LinkButtonPopMailForm_Click">  <%--CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CUSNO")%>'--%>
                                    Kopiera upp
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField> 
                         
                    </Columns>
                </asp:GridView>     
            </asp:Panel>                                                  
            
            <%-- Progress area, jquery popup --%>
            <div id="progress" class="hidden bold">
                <EPiServer:Translate ID="Translate1" runat="server" Text="/campaigns/various/progress" />&nbsp;
                <img id="progressimg" src="/tools/admin/styles/images/loader.gif" alt="Skickar ..." />
            </div>	            
            <%-- Subscription area, jquery popup --%>
            <div id="subscription" title="Prenumerationer" class="hidden">                
                <asp:GridView runat="server" ID="GvSubscription" CssClass="CustomerInfo" AlternatingRowStyle-BackColor="#F9F8F8" HeaderStyle-BackColor="#BCBCBC" GridLines="Both" AutoGenerateColumns="false">
                    <Columns>
                        <asp:BoundField HeaderText="Prennr." DataField="SUBSNO" />
                        <asp:BoundField HeaderText="Namn" DataField="ROWTEXT1" />                        
                        <asp:BoundField HeaderText="Produkt" DataField="PRODUCTNAME" />
                        <asp:BoundField HeaderText="Kampanjkod" DataField="CAMPID" />
                        <asp:TemplateField>
                            <HeaderTemplate>Prenstart</HeaderTemplate>
                            <ItemTemplate>                               
                                <%# DateTime.Parse(DataBinder.Eval(Container.DataItem, "SUBSSTARTDATE").ToString()).ToShortDateString()%> 
                            </ItemTemplate>
                        </asp:TemplateField>                         
                        <asp:TemplateField>
                            <HeaderTemplate>Prenslut</HeaderTemplate>
                            <ItemTemplate>                               
                                <%# DateTime.Parse(DataBinder.Eval(Container.DataItem, "SUBSENDDATE").ToString()).ToShortDateString()%> 
                            </ItemTemplate>
                        </asp:TemplateField>                          
                        <asp:BoundField HeaderText="Papperskod" DataField="PAPERCODE" />
                        <asp:BoundField HeaderText="Produktnr." DataField="PRODUCTNO" />
                        <asp:BoundField HeaderText="Subskind" DataField="SUBSKIND_CODVAL" />
                        <asp:BoundField HeaderText="Substate" DataField="SUBSSTATE_CODVAL" />                        
                    </Columns>
                </asp:GridView>
                <p class="underline">
                    <asp:Label runat="server" EnableViewState="false" ID="LbSubInfo"></asp:Label>
                    <strong>Saknar du information? Kontakta kundtjänst</strong><br />
                    Telefon: 08-573 651 00<br />
                    E-post: <a href="mailto:pren@di.se">pren@di.se</a>
                </p>
            </div>
	    </div>
    </form>
</body>
</html>
