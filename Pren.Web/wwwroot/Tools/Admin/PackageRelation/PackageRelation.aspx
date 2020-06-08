﻿<%@ Page Language="C#" AutoEventWireup="true" EnableViewState="true" CodeBehind="PackageRelation.aspx.cs" Inherits="Pren.Web.Tools.Admin.PackageRelation.PackageRelation" %>

<asp:content contentplaceholderid="FullRegion" runat="Server">
    <%: System.Web.Optimization.Scripts.Render("~/bundles/js/global") %>  

    <style type="text/css">
        table { max-width: 100% !important;width: auto !important;}
        table td{ vertical-align: top;}
        table th { font-weight: bold !important;}
        table td:nth-child(1){ width: 75px;}
        table td:nth-child(2), table th:nth-child(2){ display: none;}
        table td:nth-child(3),table td:nth-child(4),table td:nth-child(5){ width: 100px; word-wrap: break-word;max-width: 300px;}
        table td:nth-child(6){ width: 100px;}
        .epi-contentContainer{ max-width: 100% !important;}
        input[type=text], select{ width: 213px;height: 25px;margin-bottom: 10px;}
        .listbox{ height: auto;}
        /*input[type=submit]{ width: 100px;margin-right: 13px;}*/
        .left{ float: left;margin-right: 30px;min-width: 245px;}
        .clear{ clear: both;}
        .displaynone{ display: none;}
        #add-rel, #add-item{ padding: 10px;background-color: #FFF;width: 220px;border: 1px solid #eee;margin-bottom: 20px;}
        .info{ padding: 10px;background-color: #FFF;width: 500px;border: 1px solid #eee;margin-bottom: 20px;}
        label{ font-weight: bold;}
        a{ text-decoration: underline;}
        #BtnDeleteRel{ margin-left: 15px;}
        .error{ color: red;}
    </style>   

    <div class="epi-contentContainer epi-padding">
        
        <div class="epi-contentArea">
            <h1 class="EP-prefix">Packagerelationer</h1>

            <p class="EP-systemInfo">Verktyg för hantering av packagerelationer.</p>               
            
            <div class="left">
                <asp:Label runat="server" AssociatedControlID="DdlRelationTypes">Relationstyper:</asp:Label><br/>
                <asp:DropDownList runat="server" ID="DdlRelationTypes"  DataSourceID="OdsRelationTypes" DataTextField="Name" DataValueField="Id" AutoPostBack="True" OnSelectedIndexChanged="DdlRelationTypesChanged" /><br/>
                                                                                           
                <asp:Label runat="server" AssociatedControlID="LbRelationLists">Relationer:</asp:Label><br/>
                <asp:ListBox runat="server" ID="LbRelationLists" CssClass="listbox" DataTextField="Name" DataSourceID="OdsRelationLists" AutoPostBack="True" DataValueField="Id" Rows="10"  SelectionMode="Single" OnSelectedIndexChanged="LbRelationListsChanged" /><br/>

                <a href="#" id="btnaddrel" class="addbtn" data-element="add-rel">Lägg till</a>  <asp:LinkButton runat="server" ID="BtnDeleteRel" ClientIDMode="Static" Text="Ta bort" OnClick="BtnDeleteRelationListClick"></asp:LinkButton><br/><br/>
                
                <div id="add-rel" class="displaynone">
                    <asp:Panel runat="server" DefaultButton="BtnAddRelation">
                        <asp:Label runat="server" AssociatedControlID="TxtAddName">Namn</asp:Label><br/>
                        <asp:TextBox runat="server" ID="TxtAddName"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ValidationGroup="addrel" ControlToValidate="TxtAddName" CssClass="error" Display="Dynamic" ErrorMessage="* Du måste ange ett namn"></asp:RequiredFieldValidator><br/>
                        <asp:Button runat="server" ID="BtnAddRelation" ValidationGroup="addrel" Text="Lägg till relation" OnClick="BtnAddRelationListClick" />
                    </asp:Panel>
                </div>
            </div>
            <div class="left">
                <asp:PlaceHolder runat="server" ID="PhRelationItems" Visible="False">
                    <div class="info">
                        <asp:Label runat="server" ID="LblInfo"></asp:Label>
                    </div>
                    <asp:GridView runat="server" ID="GvRelationItems" DataKeyNames="Id" DataSourceID="OdsRelationItems" OnRowDeleted="GvRelationItemsDeleted" OnRowUpdated="GvRelationItemsUpdated" AutoGenerateColumns="False" AutoGenerateEditButton="True" AutoGenerateDeleteButton="True">
                        <Columns>
                            <asp:BoundField HeaderText="ID" DataField="Id"/>                   
                            <asp:TemplateField HeaderText="Wildcard före">                        
                                <ItemTemplate>
                                    <%# Eval("WildcardBefore")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="Namn" DataField="Name"/>                   
                            <asp:TemplateField HeaderText="Wildcard efter">                        
                                <ItemTemplate>
                                    <%# Eval("WildcardAfter")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Skapad">                        
                                <ItemTemplate>
                                    <%# DateTime.Parse(Eval("Created").ToString()).ToString("yyyy-MM-dd HH:mm")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                
                    <a href="#" id="btnadditem" class="addbtn" data-element="add-item">Lägg till</a><br/><br/>
                
                    <div id="add-item" class="displaynone">                        
                        <asp:Panel runat="server" DefaultButton="BtnAddRelationItem">
                            <asp:Label runat="server" AssociatedControlID="TxtAddItem">Namn</asp:Label><br/>
                            <asp:TextBox runat="server" ID="TxtAddItem"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ValidationGroup="additem" ControlToValidate="TxtAddItem" CssClass="error" Display="Dynamic" ErrorMessage="* Du måste ange ett namn"></asp:RequiredFieldValidator><br/>
                            <asp:Button runat="server" ValidationGroup="additem" ID="BtnAddRelationItem" Text="Lägg till" OnClick="BtnAddRelationItemClick" />
                        </asp:Panel>
                    </div>
                </asp:PlaceHolder>                                                         
                
                <asp:Label runat="server" EnableViewState="false" ID="LblError" CssClass="error"></asp:Label>
              
            </div>           
        </div>

    </div>      
                
    <asp:ObjectDataSource runat="server" ID="OdsRelationTypes" TypeName="Pren.Web.Tools.Admin.PackageRelation.RelationItemsRepository" SelectMethod="GetRelationTypes"  >

    </asp:ObjectDataSource>  
    
    <asp:ObjectDataSource runat="server" ID="OdsRelationLists" TypeName="Pren.Web.Tools.Admin.PackageRelation.RelationItemsRepository" SelectMethod="GetRelationLists"  >
        <SelectParameters>
            <asp:ControlParameter ControlID="DdlRelationTypes" Name="relationTypeId" PropertyName="SelectedValue" Type="Int16" />                     
        </SelectParameters>  
    </asp:ObjectDataSource>  
    
    <asp:ObjectDataSource runat="server" ID="OdsRelationItems" TypeName="Pren.Web.Tools.Admin.PackageRelation.RelationItemsRepository" SelectMethod="GetRelationItems" UpdateMethod="UpdateRelationItem" DeleteMethod="DeleteRelationItem" >
        <SelectParameters>
            <asp:ControlParameter ControlID="LbRelationLists" Name="relationListId" PropertyName="SelectedValue" Type="Int16" />                     
        </SelectParameters>  
        <UpdateParameters>
            <asp:Parameter Name="Id" Type="Int16" />
            <asp:Parameter Name="Name" Type="String" />
        </UpdateParameters>
        <DeleteParameters>
            <asp:Parameter Name="Id" Type="Int16" />
        </DeleteParameters>
    </asp:ObjectDataSource>   
    
    <script type="text/javascript">

        $(".addbtn").on("click", function (e) {
            e.preventDefault();
            $("#" + $(this).data("element")).toggle();
        });

        $('div').find('a').filter(':contains("Delete")').on("click", function(e) {
            if (!confirm("Är du säker på att du vill fimpa relationen?")) {
                e.preventDefault();
            }
        });

        $("#BtnDeleteRel").on("click", function (e) {
            if (!confirm("Är du säker på att du vill fimpa relationen?")) {
                e.preventDefault();
            }
        });




    </script>
</asp:content>
