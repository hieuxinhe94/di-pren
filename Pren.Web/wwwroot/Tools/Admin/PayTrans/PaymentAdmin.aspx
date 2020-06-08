<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PaymentAdmin.aspx.cs" Inherits="Pren.Web.Tools.Admin.PayTrans.PaymentAdmin" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Betalningsadmin</title>
    
    <style type="text/css">
        .tbltop {border:1px solid #888;}
        .tbltop td{padding:5px;}
        table.info{background-color:#eee;border:1px solid #888;}
        
        .tblresult {width:95%;border:1px solid #888;border-collapse:collapse;margin-top:10px;}
        .tblresult td {border:1px solid #888;padding:5px;}
        .tblresult th {border:1px solid #888;background-color:#eee;font-family:Verdana;font-size:14px;padding:5px;}              
    </style>    
</head>
<body>
    <form id="form1" runat="server">
        
        <div id="MainBodyArea">
            <h1>Betalningsadmin</h1>
                
            <table cellspacing="0" cellpadding="3" border="0" class="tbltop">
              <tr>
                <td>
                    <strong>Datum (ååååmmdd):</strong><br />                
                    <asp:TextBox runat="server" Width="150" MaxLength="10" ID="TxtDate"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <strong>Referensnummer:</strong><br />
                    <asp:TextBox runat="server" Width="150" MaxLength="10" ID="TxtRefNo"></asp:TextBox>
                </td>
                <td>
                    <strong>Transaktionsnummer:</strong><br />
                    <asp:TextBox runat="server" Width="150" MaxLength="10" ID="TxtTransNo"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <strong>Namn:</strong><br />
                    <asp:TextBox runat="server" Width="150" MaxLength="100" ID="TxtName"></asp:TextBox>
                </td>
                <td>                    
                    <strong>E-post:</strong><br />
                    <asp:TextBox runat="server" Width="150" MaxLength="100" ID="TxtEmail"></asp:TextBox>
                </td>
              </tr>
                <tr>
                    <td><strong>Filter</strong></td>
                </tr>
              <tr>
                <td colspan="2">
                    <table class="info">                   
                        <tr>
                            <td>
                                <strong>Kommentar</strong><br />
                                <asp:TextBox runat="server" ID="TxtFilter"></asp:TextBox>    
                            </td>
                        
                            <td>
                                <strong>Status</strong><br />
                                <asp:DropDownList runat="server" ID="DdlStatus">
                                    <asp:ListItem Text="Alla status"></asp:ListItem>
                                    <asp:ListItem Value="p" Text="Påbörjad"></asp:ListItem>
                                    <asp:ListItem Value="a" Text="OK"></asp:ListItem>
                                    <asp:ListItem Value="e" Text="Avbruten"></asp:ListItem>
                                </asp:DropDownList>                                        
                            </td>
                        </tr>
                    </table>                   
                </td>
              </tr>
              <tr>
                <td>
                    <asp:Button ID="Button1" runat="server" Text="Sök" OnClick="BtnSearchOnClick" />
                    <asp:Button ID="Button2" runat="server" Text="Rensa" OnClick="BtnClearOnClick" />                    
                </td>
              </tr>
            </table>
            <p>
                <i>Söktips: För att söka efter alla transaktioner i Maj 2008, skriv 200805* i sökrutan<br /> 
                För att söka på alla transaktioner med en e-post som börjar på anna, skriv anna* i sökrutan.</i>
            </p>
            <table class="info">                   
                <tr>
                    <td>
                        <strong>Antal rader: </strong><%=Rows %>
                        <strong>Summa: </strong><%=TotalAmount %>:-
                        <strong>Varav moms: </strong><%=TotalVat %>:-
                    </td>
                </tr>
            </table>
            
            <asp:Label runat="server" EnableViewState="false" ID="LblError" CssClass="error"></asp:Label>
            
            <asp:GridView runat="server" AllowSorting="true" DataSourceID="GvDataSource" ID="GvResult" OnSorting="GvDataSourceOnSorting" AutoGenerateColumns="false" CssClass="tblresult" AlternatingRowStyle-CssClass="oddrow">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <%# Rows += 1%> 
                        </ItemTemplate>
                    </asp:TemplateField> 
                    <asp:BoundField HeaderText="Refnr" SortExpression="Customer_refno" DataField="Customer_refno" />
                    <asp:BoundField HeaderText="Transnr" SortExpression="Transaction_id" DataField="Transaction_id" />
                    <asp:BoundField HeaderText="Namn" SortExpression="Consumer_name" DataField="Consumer_name" />
                    <asp:BoundField HeaderText="Kommentar" SortExpression="Comment" DataField="Comment" />
                    <asp:BoundField HeaderText="Email" SortExpression="Email_address" DataField="Email_address" />
                    <asp:TemplateField SortExpression="Amount" HeaderText="Belopp">
                        <ItemTemplate><%# GetAmount(DataBinder.Eval(Container.DataItem, "Amount")).ToString("0.00") %>:-</ItemTemplate>
                    </asp:TemplateField>                    
                    <asp:TemplateField SortExpression="VAT" HeaderText="Varav moms">
                        <ItemTemplate><%# GetVat(DataBinder.Eval(Container.DataItem, "VAT")).ToString("0.00") %>:-</ItemTemplate>
                    </asp:TemplateField>           
                    <asp:BoundField HeaderText="Produkt" SortExpression="Goods_description" DataField="Goods_description" />
                    <asp:BoundField HeaderText="Korttyp" SortExpression="Card_type" DataField="Card_type" />
                    <asp:TemplateField SortExpression="Status" HeaderText="Status">                        
                        <ItemTemplate>
                            <%# GetStatus(DataBinder.Eval(Container.DataItem, "Status").ToString(), DataBinder.Eval(Container.DataItem, "Status_code").ToString())%>
                        </ItemTemplate>
                    </asp:TemplateField>
                     <asp:TemplateField SortExpression="Purchase_date" HeaderText="Datum">                        
                        <ItemTemplate>
                            <%# GetDate(DataBinder.Eval(Container.DataItem, "Purchase_date")).ToString("yyyy-MM-dd HH:mm") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--<asp:BoundField HeaderText="Datum" SortExpression="Purchase_date" DataField="Purchase_date" />--%>
                </Columns>
            </asp:GridView>
            
            <asp:ObjectDataSource runat="server" ID="GvDataSource" TypeName="Pren.Web.Tools.Admin.PayTrans.PaymentAdmin" SelectMethod="GetResult">
                <SelectParameters>
                    <asp:ControlParameter ControlID="TxtDate" Name="date" PropertyName="Text" />                     
                    <asp:ControlParameter ControlID="TxtRefNo" Name="refNo" PropertyName="Text" />
                    <asp:ControlParameter ControlID="TxtTransNo" Name="transNo" PropertyName="Text" />
                    <asp:ControlParameter ControlID="TxtName" Name="name" PropertyName="Text" />
                    <asp:ControlParameter ControlID="TxtEmail" Name="email" PropertyName="Text" />
                </SelectParameters>       
            </asp:ObjectDataSource>            
	    </div>
    </form>
</body>
</html>
