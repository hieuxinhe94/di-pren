<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GasellAdmin.aspx.cs" Inherits="DagensIndustri.Tools.Admin.Gasell.GasellAdmin" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <style type="text/css">
        .AdminTable td {white-space:nowrap; height:20px;vertical-align:top;padding:10px;}
        .AdminTable th {text-align:left;padding:2px 10px 2px 10px;}
        .AdminTable tr.odd {background-color: rgb(249, 248, 248);}
        .AdminTable .sem{display:block;}
        .AdminTable .header{font-weight:bold;background-color:#BCBCBC;}
                
        .editactivities {background-color:#FFF;}
        .editactivities td{padding:5px;vertical-align:middle;}
        
        .searchheading{border:1px solid #BCBCBC;font-weight:bold;margin:10px 0 10px 0;padding:10px;width:300px;} 
        #advancedinfo{padding-top:10px;display:none;}
    </style>  

</head>
<body>
    <form id="form1" runat="server">
    <div>
        
        <h1>Gaselladmin</h1>
            
        <asp:Panel ID="Panel1" runat="server" DefaultButton="BtnShowPersons">
            
            <fieldset style="width:300px">
                <legend style="color:#000;">Välj Gasell</legend>
                <table border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td><asp:DropDownList ID="DdlConferences" DataTextField="text" DataValueField="value" runat="server"></asp:DropDownList></td>
                    <td width="10"></td>
                    <td><asp:ImageButton runat="server" ID="BtnShowPersons" ImageUrl="/tools/admin/styles/images/show_persons.png" OnClick="BtnSearchOnClick" /></td>
                </tr>
                </table>
            </fieldset>
            
            <br />

            <asp:PlaceHolder runat="server" ID="PhSeminar">

                <asp:PlaceHolder ID="PlaceHolderExport" runat="server">
                    <div class="searchheading">
                        Anmälda till "<%= Request.QueryString["searchheading"] %>" <asp:Label runat="server" ID="LblPersonCount"></asp:Label> st
                        <p><asp:ImageButton runat="server" ID="BtnExport" ImageUrl="/tools/admin/styles/images/export_person_excel.png" OnClick="BtnExportOnClick" /></p>
                    </div>
                </asp:PlaceHolder>

                <asp:GridView 
                    runat="server" 
                    DataKeyNames="id"
                    EditRowStyle-BackColor="ActiveBorder" 
                    AllowPaging="true" 
                    PageSize="20" 
                    DataSourceID="PersonDataSource" 
                    AllowSorting="true" 
                    EnableViewState="true" 
                    CssClass="AdminTable" 
                    AlternatingRowStyle-BackColor="#F9F8F8" 
                    HeaderStyle-BackColor="#BCBCBC" 
                    GridLines="Both" 
                    ID="GvPersons" 
                    AutoGenerateColumns="false">
                    <Columns>
                        <%--<asp:TemplateField >
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %> 
                            </ItemTemplate>
                        </asp:TemplateField> --%>  
                        <asp:CommandField EditText="Redigera" UpdateText="Uppdatera" CancelText="Avbryt" ShowEditButton="true" /> 
                        <asp:TemplateField ShowHeader="False"> 
                            <ItemTemplate> 
                                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete" OnClientClick="return confirm ('Är du säker på att du vill ta bort personen? All information för personen kommer att tas bort')" Text="Ta bort"></asp:LinkButton>&nbsp; 
                            </ItemTemplate> 
                        </asp:TemplateField>                                                             
                        <asp:BoundField HeaderText="Förnamn" SortExpression="firstname" DataField="firstname" />
                        <asp:BoundField HeaderText="Efternamn" SortExpression="lastname" DataField="lastname" />
                        <%-- <asp:TemplateField HeaderText="För- och efternamn">
                            <ItemTemplate>
                                <%# Eval("firstname") + " " + Eval("lastname")%>
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                        <asp:BoundField HeaderText="Befattning" SortExpression="title" DataField="title" />
                        <asp:BoundField HeaderText="Företag" SortExpression="company" DataField="company" />
                        <asp:BoundField HeaderText="Adress" SortExpression="address" DataField="address" />
                        <asp:BoundField HeaderText="Postnr" SortExpression="zipcode" DataField="zipcode" />
                        <asp:BoundField HeaderText="Ort" SortExpression="city" DataField="city" />
                        <asp:BoundField HeaderText="Telefon" SortExpression="phone" DataField="phone" />
                        <asp:BoundField HeaderText="E-post" SortExpression="mail" DataField="mail" />
                        <asp:BoundField HeaderText="Bransch" SortExpression="bransch" DataField="bransch" />
                        <asp:BoundField HeaderText="Anställda" SortExpression="employees" DataField="employees" />
                        <asp:TemplateField HeaderText="Gasellföretag" SortExpression="gasellCompany">
                            <%--<HeaderTemplate>Gasellföretag</HeaderTemplate>--%>
                            <ItemTemplate>
                                <asp:CheckBox ID="cb3" runat="server" Enabled="false" Checked='<%# Bind("gasellCompany")%>' />
                            </ItemTemplate>
                            <EditItemTemplate>                                   
                                <asp:CheckBox ID="cb4" runat="server" Checked='<%# Bind("gasellCompany")%>' />
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Kod" SortExpression="code" DataField="code" />
                        <asp:BoundField HeaderText="Fakturaadress" SortExpression="invoiceAddress" DataField="invoiceAddress" />
                        <asp:BoundField HeaderText="Faktura postnr" SortExpression="invoiceZipCode" DataField="invoiceZipCode" />
                        <asp:BoundField HeaderText="Faktura ort" SortExpression="invoiceCity" DataField="invoiceCity" />
                        <asp:BoundField HeaderText="Fakturareferens" SortExpression="invoiceRef" DataField="invoiceRef" />
                        <asp:BoundField HeaderText="Orgnr" SortExpression="orgNo" DataField="orgNo" />
                        <asp:BoundField HeaderText="Betalningsinfo" SortExpression="payInfo" DataField="payInfo" />
                        <asp:TemplateField HeaderText="Avbokat" SortExpression="canceled">
                            <ItemTemplate>
                                <asp:CheckBox ID="cb1" runat="server" Enabled="false" Checked='<%# Bind("canceled")%>' />
                            </ItemTemplate>
                            <EditItemTemplate>                                   
                                <asp:CheckBox ID="cb2" runat="server" Checked='<%# Bind("canceled")%>' />
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField ReadOnly="true" HeaderText="Skapad" SortExpression="createTime" DataField="createTime" dataformatstring="{0:yyyy-MM-dd HH:mm:ss}" HtmlEncode="false"/>
                        <asp:BoundField HeaderText="ID" ReadOnly="true" SortExpression="id" DataField="id" />
                    </Columns>
                </asp:GridView>  
                    
                <asp:ObjectDataSource 
                    runat="server" 
                    ID="PersonDataSource" 
                    TypeName="DagensIndustri.Tools.Admin.Gasell.GasellObjDataSrc"
                    SelectMethod="GetGasellPersons" 
                    UpdateMethod="UpdateGasellPerson" 
                    DeleteMethod="DeleteGasellPerson" 
                    OnSelected="PersonDataSourceOnSelected">
                    
                    <SelectParameters>                            
                        <asp:QueryStringParameter Name="epiPageId" QueryStringField="epiPageId" Type="Int32" DefaultValue="0" />
                    </SelectParameters>

                    <UpdateParameters>
                        <asp:Parameter Name="firstname" Type="String" />
                        <asp:Parameter Name="lastname" Type="String" />
                        <asp:Parameter Name="title" Type="String" />
                        <asp:Parameter Name="company" Type="String" />
                        <asp:Parameter Name="address" Type="String" />
                        <asp:Parameter Name="zipcode" Type="String" />
                        <asp:Parameter Name="city" Type="String" />
                        <asp:Parameter Name="phone" Type="String" />
                        <asp:Parameter Name="mail" Type="String" />
                        <asp:Parameter Name="bransch" Type="String" />
                        <asp:Parameter Name="employees" Type="String" />
                        <asp:Parameter Name="gasellCompany" Type="Boolean" />
                        <asp:Parameter Name="code" Type="String" />
                        <asp:Parameter Name="invoiceAddress" Type="String" />
                        <asp:Parameter Name="invoiceZipCode" Type="String" />
                        <asp:Parameter Name="invoiceCity" Type="String" />
                        <asp:Parameter Name="invoiceRef" Type="String" />
                        <asp:Parameter Name="orgno" Type="String" />
                        <asp:Parameter Name="payInfo" Type="String" />
                        <asp:Parameter Name="canceled" Type="Boolean" />
                    </UpdateParameters>
                        
                    <DeleteParameters>
                        <%--<asp:Parameter Name="personId" Type="Int32" />--%>
                    </DeleteParameters>

                </asp:ObjectDataSource>

            </asp:PlaceHolder>
        </asp:Panel>         

    </div>
    </form>
</body>
</html>
