<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SignUpAdmin.aspx.cs" Inherits="DagensIndustri.Tools.Admin.SignUp.SignUpAdmin" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <style type="text/css">
        .AdminTable td {white-space:nowrap; height:20px;vertical-align:top;padding:10px;}
        .AdminTable th {text-align:left;padding:2px 10px 2px 10px;}
        .AdminTable tr.odd {background-color: rgb(249, 248, 248);}
        .AdminTable .sem {display:block;}
        .AdminTable .header {font-weight:bold;background-color:#BCBCBC;}
                
        .editactivities {background-color:#FFF;}
        .editactivities td {padding:5px;vertical-align:middle;}
        
        .searchheading {border:1px solid #BCBCBC;font-weight:bold;margin:10px 0 10px 0;padding:10px;width:300px;} 
        #advancedinfo {padding-top:10px;display:none;}
    </style>  

</head>
<body>
    <form id="form1" runat="server">
    <div>
        
        <h1>Sign-up admin</h1>
            
        <asp:Panel ID="Panel1" runat="server" DefaultButton="BtnShowPersons">
            
            <fieldset style="width:300px">
                <legend style="color:#000;">Välj event</legend>
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
                    DataKeyNames="Id"
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
                        <asp:BoundField HeaderText="Cusno" SortExpression="cusno" DataField="cusno" />
                        <asp:BoundField HeaderText="Förnamn" SortExpression="firstName" DataField="firstName" />
                        <asp:BoundField HeaderText="Efternamn" SortExpression="lastName" DataField="lastName" />
                        <%-- <asp:TemplateField HeaderText="För- och efternamn">
                            <ItemTemplate>
                                <%# Eval("firstName") + " " + Eval("lastName")%>
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                        <asp:BoundField HeaderText="Adress" SortExpression="address" DataField="address" />
                        <asp:BoundField HeaderText="Postnr" SortExpression="zip" DataField="zip" />
                        <asp:BoundField HeaderText="Ort" SortExpression="city" DataField="city" />
                        <asp:BoundField HeaderText="Telefon" SortExpression="phone" DataField="phone" />
                        <asp:BoundField HeaderText="E-post" SortExpression="email" DataField="email" />
                        <asp:TemplateField HeaderText="Avbokat" SortExpression="canceled">
                            <ItemTemplate>
                                <asp:CheckBox ID="cb1" runat="server" Enabled="false" Checked='<%# Bind("canceled")%>' />
                            </ItemTemplate>
                            <EditItemTemplate>                                   
                                <asp:CheckBox ID="cb2" runat="server" Checked='<%# Bind("canceled")%>' />
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Betalningsmetod" SortExpression="payMethod" DataField="payMethod" />
                        <asp:BoundField HeaderText="EpiPageId" SortExpression="epiPageId" DataField="epiPageId" />
                        <asp:BoundField HeaderText="ID" ReadOnly="true" SortExpression="Id" DataField="Id" />
                        <asp:BoundField HeaderText="Betalar ID" ReadOnly="true" SortExpression="payerId" DataField="payerId" />
                        <asp:BoundField HeaderText="Skapad" ReadOnly="true" SortExpression="dateSaved" DataField="dateSaved" dataformatstring="{0:yyyy-MM-dd HH:mm:ss}" HtmlEncode="false"/>
                    </Columns>
                </asp:GridView>  
                    
                <asp:ObjectDataSource 
                    runat="server" 
                    ID="PersonDataSource" 
                    TypeName="DagensIndustri.Tools.Admin.SignUp.SignUpObjDataSrc"
                    SelectMethod="GetSignUpPersons" 
                    UpdateMethod="UpdateSignUpPerson" 
                    DeleteMethod="DeleteSignUpPerson" 
                    OnSelected="PersonDataSourceOnSelected">
                    
                    <SelectParameters>                            
                        <asp:QueryStringParameter Name="epiPageId" QueryStringField="epiPageId" Type="Int32" DefaultValue="0" />
                    </SelectParameters>

                    <UpdateParameters>
                        <asp:Parameter Name="cusno" Type="String" />
                        <asp:Parameter Name="epiPageId" Type="String" />
                        <asp:Parameter Name="payMethod" Type="String" />
                        <asp:Parameter Name="firstName" Type="String" />
                        <asp:Parameter Name="lastName" Type="String" />
                        <asp:Parameter Name="address" Type="String" />
                        <asp:Parameter Name="zip" Type="String" />
                        <asp:Parameter Name="city" Type="String" />
                        <asp:Parameter Name="phone" Type="String" />
                        <asp:Parameter Name="email" Type="String" />
                        <asp:Parameter Name="canceled" Type="Boolean" />
                    </UpdateParameters>
                        
                    <DeleteParameters>
                        <asp:Parameter Name="Id" Type="Int32" />
                    </DeleteParameters>

                </asp:ObjectDataSource>

            </asp:PlaceHolder>
        </asp:Panel>         

    </div>
    </form>
</body>
</html>
