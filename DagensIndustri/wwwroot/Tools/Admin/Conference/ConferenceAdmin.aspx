<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ConferenceAdmin.aspx.cs" Inherits="DagensIndustri.Tools.Admin.Conference.ConferenceAdmin" %>
<%@ Register TagPrefix="DI" TagName="Activities" Src="~/Tools/admin/conference/Activities.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Konferensadmin</title>
    <link href="/Tools/Admin/Styles/StyleSheetPublic.css" rel="stylesheet" type="text/css" />  
    <script src="/Tools/Admin/Styles/Scripts/jquery-1.4.2.min.js" type="text/javascript"></script>
    <script src="/Tools/Admin/Styles/Scripts/jquery-ui-1.8.2.custom.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $("#showhideinfo").click(function () {
                $("#advancedinfo").toggle("blind", 500);
            });
        });

        function jsSaveComment(cmtId, controlId) {
            $.ajax({
                type: "POST",
                url: "/Tools/Admin/Conference/PdfCommentSaver.ashx",
                data: { action: 'savePdfComment', commentId: cmtId, comment: $("#" + controlId).val() },
                success: function (response) {
                    alert(response);
                },
                error: function () {
                    alert('Tekniskt fel: kommentaren kunde inte sparas.');
                }
            });
            return false;
        }
    </script>       
    
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

        textarea {
            font-family:arial;
            font-size: 12px;
            line-height: 16px;
            height: 30px;
            width: 200px;
        }

        hr { margin-top:0px; margin-bottom:0px; }
    </style>    

</head>
<body class="white">
    <form id="form1" runat="server">
             
        <div id="MainBodyArea">

            <h1>Konferensadmin</h1>               
            
            <asp:Panel ID="Panel1" runat="server" DefaultButton="BtnShowPersons">
            
                <fieldset style="width:300px">
                    <legend style="color:#000;">Ange sökkriterier</legend>
                    <asp:DropDownList runat="server" ID="DdlConferences"></asp:DropDownList>
                    <br /><br />
                    <asp:ImageButton runat="server" ID="BtnShowPersons" ImageUrl="/tools/admin/styles/images/show_persons.png" OnClick="BtnSearchOnClick" />
                    <asp:ImageButton runat="server" ID="BtnShowPdf" ImageUrl="/tools/admin/styles/images/show_pdf_logg.png" OnClick="BtnShowPdfOnClick" />
                    <asp:Label runat="server" EnableViewState="false" ID="LblError" CssClass="error"></asp:Label>
                </fieldset>                                                          
                <br />
                
                <asp:PlaceHolder runat="server" ID="PhPdf" Visible="false">
                    <p>
                        <asp:ImageButton runat="server" ID="BtnExportPdfLog" ImageUrl="/tools/admin/styles/images/export_pdf_excel.png" OnClick="BtnExportPdfLogOnClick" />
                    </p>
                    <asp:PlaceHolder ID="PlaceHolerPdfTable" runat="server"></asp:PlaceHolder>
                </asp:PlaceHolder>
                
                <asp:PlaceHolder runat="server" ID="PhSeminar">
                    
                    <a id="showhideinfo" href="#">Visa/dölj detaljerad information</a>   
                    
                    <table id="advancedinfo">
                        <tr>
                            <td valign="top">
                                <asp:Label runat="server" ID="LblOverview"></asp:Label>
                            </td>
                            <td valign="top" style="padding-left:25px;">
                                
                                <asp:GridView 
                                    runat="server" 
                                    ID="GvInfoChannels" 
                                    AutoGenerateColumns="false" 
                                    CssClass="AdminTable" 
                                    AlternatingRowStyle-BackColor="#F9F8F8" 
                                    HeaderStyle-BackColor="#BCBCBC" 
                                    GridLines="Both">
                                    <Columns>
                                        <asp:BoundField HeaderText="Informationskanal" DataField="ictext" />
                                        <asp:BoundField HeaderText="Antal" DataField="counter" />
                                    </Columns>
                                </asp:GridView>

                            </td>
                        </tr>                                       
                    </table>
                                                                               
                    <div class="searchheading">
                        Alla personer för "<%= Request.QueryString["searchheading"] %>"<br />
                        Antal: <asp:Label runat="server" ID="LblPersonCount"></asp:Label>
                        <p><asp:ImageButton runat="server" ID="BtnExport" ImageUrl="/tools/admin/styles/images/export_person_excel.png" OnClick="BtnExportOnClick" /></p>
                    </div>
                    
                    <asp:GridView 
                        runat="server" 
                        EditRowStyle-BackColor="ActiveBorder" 
                        AllowPaging="true" 
                        PageSize="50" 
                        DataSourceID="PersonDataSource" 
                        AllowSorting="true" 
                        DataKeyNames="personid" 
                        EnableViewState="true" 
                        CssClass="AdminTable" 
                        AlternatingRowStyle-BackColor="#F9F8F8" 
                        HeaderStyle-BackColor="#BCBCBC" 
                        GridLines="Both" 
                        ID="GvPersons" 
                        AutoGenerateColumns="false">
                        <Columns>
                            <asp:TemplateField >
                                <ItemTemplate>
                                    <%# Container.DataItemIndex + 1 %> 
                                </ItemTemplate>
                            </asp:TemplateField>   
                            <asp:CommandField EditText="Redigera" UpdateText="Uppdatera" CancelText="Avbryt" ShowEditButton="true" /> 
                            <asp:TemplateField ShowHeader="False"> 
                                <ItemTemplate> 
                                    <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete" OnClientClick="return confirm ('Är du säker på att du vill ta bort personen? All information för personen kommer att tas bort')" Text="Ta bort"></asp:LinkButton>&nbsp; 
                                </ItemTemplate> 
                            </asp:TemplateField>                                                             
                            <asp:BoundField HeaderText="ID" SortExpression="personid" ReadOnly="true" DataField="personid" />
                            <asp:BoundField HeaderText="Förnamn" SortExpression="firstname" DataField="firstname" />
                            <asp:BoundField HeaderText="Efternamn" SortExpression="lastname" DataField="lastname" />
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    Namn
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%# Eval("firstname") + " " + Eval("lastname")%>
                                </ItemTemplate>
                            </asp:TemplateField>                                                              
                            <asp:BoundField HeaderText="Företag" SortExpression="company" DataField="company" />
                            <asp:BoundField HeaderText="Befattning" SortExpression="title" DataField="title" />
                            <asp:BoundField HeaderText="Organisationsnr." SortExpression="orgno" DataField="orgno" />
                            <asp:BoundField HeaderText="Telefon" SortExpression="phone" DataField="phone" />
                            <asp:BoundField HeaderText="E-post" SortExpression="email" DataField="email" />
                            <asp:BoundField HeaderText="Fakturaadress" SortExpression="invoiceaddress" DataField="invoiceaddress" />
                            <asp:BoundField HeaderText="Fakturareferens" SortExpression="invoicereference" DataField="invoicereference" />
                            <asp:BoundField HeaderText="Postnummer" SortExpression="zip" DataField="zip" />
                            <asp:BoundField HeaderText="Ort" SortExpression="city" DataField="city" />
                            <asp:BoundField HeaderText="Kod" SortExpression="code" DataField="code" />                        
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    Aktiviteter
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%#GetActivitiesForPersonTable(Eval("personid").ToString())%>
                                </ItemTemplate>
                                <EditItemTemplate>                                   
                                    <DI:Activities runat="server" ID="ActivitiesForPerson" PersonId='<%# Bind("personid") %>' />
                                </EditItemTemplate>
                            </asp:TemplateField>                                                      
                            <%--<asp:BoundField ReadOnly="true" HeaderText="Betalningsmetod" DataField="method" />--%>
                            <asp:BoundField HeaderText="Belopp" DataField="price" />
                            <asp:BoundField ReadOnly="true" SortExpression="created" HeaderText="Skapad" DataField="created" />
                        </Columns>
                    </asp:GridView>  
                    
                    <asp:ObjectDataSource 
                        runat="server" 
                        ID="PersonDataSource" 
                        TypeName="DagensIndustri.Tools.Classes.Conference.ConferenceODS"
                        SelectMethod="GetPersons" 
                        UpdateMethod="UpdatePerson" 
                        DeleteMethod="DeletePerson" 
                        OnSelected="PersonDataSourceOnSelected">
                        <SelectParameters>                            
                            <asp:QueryStringParameter Name="conferenceid" QueryStringField="conferenceid" Type="Int32" DefaultValue="0" />
                            <asp:QueryStringParameter Name="dayid" QueryStringField="dayid" Type="Int32" DefaultValue="0" />
                            <asp:QueryStringParameter Name="seminarid" QueryStringField="seminarid" Type="Int32" DefaultValue="0" />                        
                        </SelectParameters>

                        <UpdateParameters>
                            <asp:Parameter Name="personid" Type="Int32" />
                            <asp:Parameter Name="firstname" Type="String" />
                            <asp:Parameter Name="lastname" Type="String" />
                            <asp:Parameter Name="company" Type="String" />
                            <asp:Parameter Name="title" Type="String" />
                            <asp:Parameter Name="orgno" Type="String" />
                            <asp:Parameter Name="phone" Type="String" />
                            <asp:Parameter Name="email" Type="String" />
                            <asp:Parameter Name="invoiceaddress" Type="String" />
                            <asp:Parameter Name="invoicereference" Type="String" />
                            <asp:Parameter Name="zip" Type="String" />
                            <asp:Parameter Name="city" Type="String" />
                            <asp:Parameter Name="code" Type="String" />
                            <asp:Parameter Name="price" Type="String" />
                        </UpdateParameters>
                        
                        <DeleteParameters>
                            <asp:Parameter Name="personid" Type="Int32" />
                        </DeleteParameters>
                    </asp:ObjectDataSource>
                

                </asp:PlaceHolder>
            </asp:Panel>                                                          
	    </div>
    </form>
</body>
</html>

