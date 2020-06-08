<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CampaignAdmin.aspx.cs" Inherits="DagensIndustri.Tools.Admin.Campaign.CampaignAdmin" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Kampanjadmin</title>
    <link href="/Tools/Admin/Styles/StyleSheetPublic.css" rel="stylesheet" type="text/css" />    
    
    <style type="text/css">

    </style>    
</head>
<body>
    <form id="form1" runat="server">
    
        <div id="MainBodyArea">
            <h1>Kampanjadmin</h1>
            Här anger du de erbjudandekoder och målgrupper som ska vara tillgängliga vid skapandet av en kampanj.
            <hr />
            <div id="offercodes">
                <h2>Erbjudandekoder</h2>            
                <table>
                    <tr>
                        <td colspan="4">
                            <asp:DropDownList runat="server" ID="DdlProduct" AutoPostBack="true" DataSourceID="ProductsDataSource" DataTextField="productName" DataValueField="productNo">
                                <asp:ListItem Text="--Välj produkt--"></asp:ListItem>
                                <asp:ListItem Text="DI" Value="01"></asp:ListItem>
                                <asp:ListItem Text="Weekend" Value="05"></asp:ListItem>
                            </asp:DropDownList>        
                            <asp:ObjectDataSource ID="ProductsDataSource" runat="server"                     
                                TypeName="DagensIndustri.Tools.Classes.Campaign.CampaignODS" 
                                SelectMethod="GetProducts">                           
                            </asp:ObjectDataSource>              
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:DropDownList runat="server" ID="DdlOfferCodes" DataSourceID="ActiveCampaignsDataSource" AutoPostBack="true" DataTextField="CAMPID" DataValueField="CAMPNO" OnSelectedIndexChanged="DdlOfferCodesOnSelectedIndexChanged"></asp:DropDownList>  
                            <asp:ObjectDataSource ID="ActiveCampaignsDataSource" runat="server"                     
                                TypeName="DagensIndustri.Tools.Classes.Campaign.CampaignODS" 
                                SelectMethod="GetActiveCampaigns">
                                <SelectParameters>
                                    <asp:Parameter Name="addChooseItem" DbType="Boolean" DefaultValue="true" />
                                    <asp:ControlParameter ControlID="DdlProduct" Name="productNo" PropertyName="SelectedValue" />
                                </SelectParameters>                            
                            </asp:ObjectDataSource>                                            
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <strong>Namn</strong>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="addcode" ControlToValidate="TxtCampName" ErrorMessage="* Du måste ange namn"></asp:RequiredFieldValidator>
                            <br />
                            <asp:TextBox runat="server" Width="300" ID="TxtCampName"></asp:TextBox>                            
                        </td>
                        <td>
                            <strong>Aktiv</strong><br />
                            <asp:CheckBox runat="server" ID="ChkActive" />                    
                        </td>
                        <td class="hidden">
                            <strong>Autogiro</strong><br />
                            <asp:CheckBox runat="server" ID="ChkAutogiro" />                        
                        </td>
                        <td class="hidden">
                            <strong>Pricegroup</strong><br />
                            <asp:TextBox runat="server" ID="TxtPriceGroup"></asp:TextBox>                            
                        </td>
                        <td class="hidden">
                            <strong>Subskind</strong><br />
                            <asp:TextBox runat="server" ID="TxtSubsKind"></asp:TextBox>                            
                        </td>                        
                        <td>
                            <strong>Student</strong><br />
                            <asp:CheckBox runat="server" ID="ChkStudent" />                        
                        </td>                             
                        <td>
                            <strong>Sorteringsordning</strong><br />
                            <asp:textbox runat="server" ID="TxtSortOrder" Width="50"></asp:textbox>                        
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button runat="server" ValidationGroup="addcode" ID="BtnAddOfferCode" Text="Lägg till erbjudandekod" OnClick="BtnAddOfferCodeOnClick" />                    
                        </td>
                    </tr>
                </table>                        
                <br />
                <asp:GridView runat="server" AlternatingRowStyle-BackColor="#E2BEB1" ID="GvOfferCodes" AllowSorting="true" AutoGenerateColumns="false" DataSourceID="OfferCodeDataSource" DataKeyNames="offercodeId">
                    <Columns>                        
                        <asp:BoundField ReadOnly="true" SortExpression="Campno" HeaderText="Campno" DataField="campNo"></asp:BoundField>
                        <asp:BoundField ReadOnly="true" SortExpression="Campid" HeaderText="Campid" DataField="campId"></asp:BoundField>
                        <asp:BoundField HeaderText="Beskrivning" SortExpression="offerText" DataField="offerText"></asp:BoundField>
                        <asp:BoundField HeaderText="Sortering" DataField="sortOrder"></asp:BoundField>
                        <asp:CheckBoxField HeaderText="Aktiv" SortExpression="isActive" DataField="isActive" />                        
                        <asp:CheckBoxField ReadOnly="true" HeaderText="Autogiro" SortExpression="isAutogiro" DataField="isAutogiro" />
                        <asp:CheckBoxField HeaderText="Student" SortExpression="isStudent" DataField="isStudent" />
                        <asp:BoundField ReadOnly="true" HeaderText="Produkt" SortExpression="productName" DataField="productName"></asp:BoundField>
                        <asp:CommandField ShowEditButton="true" ShowDeleteButton="true" />
                    </Columns>
                </asp:GridView>
                
                <asp:ObjectDataSource ID="OfferCodeDataSource" runat="server"                     
                    TypeName="DagensIndustri.Tools.Classes.Campaign.CampaignODS" OnDeleting="OfferCodeDeleting" 
                    SelectMethod="GetOfferCodes" UpdateMethod="UpdateOfferCode" InsertMethod="InsertOfferCode" DeleteMethod="DeleteOfferCode">
                    <SelectParameters>
                        <asp:Parameter Name="onlyActive" Type="Int32" DefaultValue="0" />
                    </SelectParameters>
                    <UpdateParameters>
                        <asp:Parameter Name="offercodeId" Type="Int32" />
                        <asp:Parameter Name="offerText" Type="String" />
                        <asp:Parameter Name="sortOrder" Type="Int32" />
                        <asp:Parameter Name="isActive" Type="Boolean" />
                        <asp:Parameter Name="isStudent" Type="Boolean" />
                    </UpdateParameters>
                    <InsertParameters>
                        <asp:ControlParameter ControlID="DdlOfferCodes" Name="campNo" PropertyName="SelectedValue" />
                        <asp:ControlParameter ControlID="DdlOfferCodes" Name="campId" PropertyName="SelectedItem.Text" />
                        <asp:ControlParameter ControlID="TxtCampName" Name="offerText" PropertyName="Text" />
                        <asp:ControlParameter ControlID="TxtSortOrder" Name="sortOrder" PropertyName="Text" />
                        <asp:ControlParameter ControlID="ChkActive" Name="isActive" PropertyName="Checked" />
                        <asp:ControlParameter ControlID="DdlProduct" Name="productNo" PropertyName="SelectedValue" />
                        <asp:ControlParameter ControlID="ChkStudent" Name="isStudent" PropertyName="Checked" />
                        <asp:ControlParameter ControlID="ChkAutogiro" Name="isAutogiro" PropertyName="Checked" />                        
                        <asp:ControlParameter ControlID="TxtPriceGroup" Name="priceGroup" PropertyName="Text" />
                        <asp:ControlParameter ControlID="TxtSubsKind" Name="subsKind" PropertyName="Text" />                        
                    </InsertParameters>
                    <DeleteParameters>
                        <asp:Parameter Name="offercodeId" Type="Int32" />
                    </DeleteParameters>
                </asp:ObjectDataSource>                
                <br />
                <asp:Label runat="server" ID="LblErrorOc" EnableViewState="false" CssClass="error"></asp:Label>

            </div>
            
            <hr />
            <div id="targetgroups">
                <h2>Målgrupper</h2>
                
                <asp:DropDownList runat="server" ID="DdlCirixTargetGroups" DataSourceID="CirixTargetGroupDataSource" DataTextField="CODEVALUE"></asp:DropDownList>
                <asp:ObjectDataSource ID="CirixTargetGroupDataSource" runat="server" TypeName="DagensIndustri.Tools.Classes.Campaign.CampaignODS" SelectMethod="GetParameterValuesByGroup">
                    <SelectParameters>
                        <asp:Parameter Name="paperCode" DbType="String" DefaultValue="DI" />
                    </SelectParameters>                           
                </asp:ObjectDataSource>
                                
                <asp:Button runat="server" ID="BtnAddTargetGroup" Text="Lägg till målgrupp" OnClick="BtnAddTargetGroupOnClick" />
                <br />                                                
                <asp:GridView runat="server" ID="GvTargetGroups" AlternatingRowStyle-BackColor="#E2BEB1" AutoGenerateColumns="false" 
                    DataSourceID="TargetGroupDataSource" DataKeyNames="targetGroupId">
                    <Columns>
                        <asp:BoundField ReadOnly="true" HeaderText="Målgrupp" DataField="targetGroupName" />
                        <asp:CommandField ShowDeleteButton="true" />
                    </Columns>
                </asp:GridView>
                <asp:ObjectDataSource ID="TargetGroupDataSource" runat="server" TypeName="DagensIndustri.Tools.Classes.Campaign.CampaignODS" 
                    OnDeleting="TargetGroupDeleting" SelectMethod="GetTargetGroups" InsertMethod="InsertTargetGroup" DeleteMethod="DeleteTargetGroup" >
                    <InsertParameters>
                        <asp:ControlParameter ControlID="DdlCirixTargetGroups" Name="targetGroupName" PropertyName="SelectedItem.Text" />
                    </InsertParameters> 
                    <DeleteParameters>
                        <asp:Parameter Name="targetGroupId" DbType="Int32" />
                    </DeleteParameters>
                </asp:ObjectDataSource>                 
                <br />
                <asp:Label runat="server" ID="LblErrorTg" EnableViewState="false" CssClass="error"></asp:Label>
            </div> 
        </div>
        
    </form>
</body>
</html>


