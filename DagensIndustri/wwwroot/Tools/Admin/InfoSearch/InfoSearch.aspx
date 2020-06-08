<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InfoSearch.aspx.cs" Inherits="DagensIndustri.Tools.Admin.InfoSearch.InfoSearch" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Infosök</title>
    
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
            <h1>Infosök</h1>
                
            <table cellspacing="0" cellpadding="3" border="0" class="tbltop">
              <tr>
                <td>
                    <strong>Fråndatum (ååååmmdd):</strong><br />                
                    <asp:TextBox runat="server" Width="150" MaxLength="10" ID="TxtDate"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <strong>Info:</strong><br />
                    <asp:TextBox runat="server" Width="150" MaxLength="10" ID="TxtInfo"></asp:TextBox>
                </td>
            </tr>

            <tr>
                <td>
             <em>Wildcard *</em>
                </td>
            </tr>
              <tr>
                <td>
                    <asp:Button ID="Button1" runat="server" Text="Sök" OnClick="BtnSearchOnClick" />
                    <asp:Button ID="Button2" runat="server" Text="Rensa" OnClick="BtnClearOnClick" />                    
                </td>
              </tr>
            </table>

            
            <asp:Label runat="server" EnableViewState="false" ID="LblError" CssClass="error"></asp:Label>
            
            <asp:GridView runat="server" AllowSorting="true" DataSourceID="GvDataSource" ID="GvResult" OnSorting="GvDataSourceOnSorting" AutoGenerateColumns="True" CssClass="tblresult" AlternatingRowStyle-CssClass="oddrow">

            </asp:GridView>
            
            <asp:ObjectDataSource runat="server" ID="GvDataSource" TypeName="DagensIndustri.Tools.Admin.InfoSearch.InfoSearch" SelectMethod="GetResult">
                <SelectParameters>
                    <asp:ControlParameter ControlID="TxtDate" Name="fromdate" PropertyName="Text" />                     
                    <asp:ControlParameter ControlID="TxtInfo" Name="info" PropertyName="Text" DefaultValue="*" />
                </SelectParameters>       
            </asp:ObjectDataSource>            
	    </div>
    </form>
</body>
</html>
