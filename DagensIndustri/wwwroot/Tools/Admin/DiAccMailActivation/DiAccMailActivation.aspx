<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DiAccMailActivation.aspx.cs"
    Inherits="DagensIndustri.Tools.Admin.DiAccMailActivation.DiAccMailActivation" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>

    <script src="Scripts/jquery-1.9.1.js" type="text/javascript"></script>

    <link href="Bootstrap/bootstrap.css" rel="stylesheet" type="text/css" />
    <link href="Bootstrap/bootstrap-theme.min.css" rel="stylesheet" type="text/css" />
    <link href="Bootstrap/bootstrap.css" rel="stylesheet" type="text/css" />
    <link href="Bootstrap/bootstrap-theme.min.css" rel="stylesheet" type="text/css" />
    
    <script src="Scripts/bootstrap-datepicker.js" type="text/javascript"></script>
    <script src="Scripts/bootstrap.js" type="text/javascript"></script>

    <link href="../../../bootstrapDi/css/datepicker.css" rel="stylesheet" type="text/css" />
    <link href="../../../bootstrapDi/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="Main.css" rel="stylesheet" type="text/css" />



    <%--DATEPICKER--%>
    <script type="text/javascript">
        $(document).ready(function () {

            $("#tbxDateFrom, #tbxDateTo").datepicker();

            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function (evt, args) {
                $(".datepicker").datepicker();
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <center>
        <div class="headerDiv">
            <asp:Label ID="topic" runat="server" Text="DI-Konton"></asp:Label>
        </div>
    </center>
    <div class="contentWrapper">
        <div class="searchContainerDiv">
            <div class="dateBoxDiv">
                <asp:Label ID="lblDate" runat="server" Text="Ange datum (yyyy/mm/dd)"></asp:Label><br />
                <asp:TextBox ID="tbxDateFrom" runat="server"></asp:TextBox>
                -
                <asp:TextBox ID="tbxDateTo" runat="server"></asp:TextBox>
            </div>
            <div class="ddlDiv">
                <div style="float: left;">
                    <asp:ListBox CssClass="listBox" ID="ddlSubsLen_Mons" runat="server" SelectionMode="Multiple"
                        Rows="1"></asp:ListBox>
                </div>
                <asp:DropDownList CssClass="ddl" ID="ddlBetweenMail" runat="server">
                </asp:DropDownList>
                <br />
                <asp:DropDownList CssClass="ddl" ID="ddlPaperCode" runat="server">
                </asp:DropDownList>
                <br />
                <asp:DropDownList CssClass="ddl" ID="ddlProductNo" runat="server">
                </asp:DropDownList>
                <br />
                <br />
            </div>
            <asp:Button CssClass="btnSearch" ID="btnSearch" runat="server" Text="SÖK"
                OnClick="btnSearch_Click" /><br />
            <asp:Button CssClass="btnReset" ID="btnReset" runat="server" Text="Återställ" OnClick="btnReset_Click" />
        </div>
        <div class="tableDiv">
            <h3>
                Antal DI-konton som har skapats</h3>
            <table class="tableContent" id="tableContent" border="1">
                <tr>
                    <td>
                        Direkt vid första välkomstmailet
                    </td>
                    <td>
                        <asp:Literal ID="literalMailStart" runat="server"></asp:Literal>
                    </td>
                </tr>
                <tr>
                    <td>
                        Mellan steg 1 och 2
                    </td>
                    <td>
                        <asp:Literal ID="literalMail1" runat="server"></asp:Literal>
                    </td>
                </tr>
                <tr>
                    <td>
                        Mellan steg 2 och 3
                    </td>
                    <td>
                        <asp:Literal ID="literalMail2" runat="server"></asp:Literal>
                    </td>
                </tr>
                <tr>
                    <td>
                        Mellan steg 3 och 4
                    </td>
                    <td>
                        <asp:Literal ID="literalMail3" runat="server"></asp:Literal>
                    </td>
                </tr>
                <tr>
                    <td>
                        Vid en annan koll
                    </td>
                    <td>
                        <asp:Literal ID="literalOther" runat="server"></asp:Literal>
                    </td>
                </tr>
                <tr>
                    <td>
                        Totalt
                    </td>
                    <td>
                        <asp:Literal ID="literalTotal" runat="server"></asp:Literal>
                    </td>
                </tr>
            </table>
        </div>
        <div class="gridviewDiv">
            <div>
                <asp:Button ID="btnExport" runat="server" Text="Export to Excel" OnClick="btnExport_Click" /><br />
                <br />
            </div>
            <asp:GridView ID="GridView1" runat="server" CellPadding="3" AutoGenerateColumns="False"
                BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px">
                <Columns>
                    <asp:BoundField DataField="customerId" HeaderText="customerId" />
                    <asp:BoundField DataField="name" HeaderText="name" />
                    <asp:BoundField DataField="email" HeaderText="email" />
                    <asp:BoundField DataField="userName" HeaderText="userName" />
                    <asp:BoundField DataField="dateSaved" HeaderText="dateSaved" DataFormatString="{0:yyyy/MM/dd}" />
                    <asp:BoundField DataField="dateUpdated" HeaderText="dateUpdated" DataFormatString="{0:yyyy/MM/dd}" />
                    <asp:BoundField DataField="dateRegularLetter" HeaderText="dateRegularLetter" />
                    <asp:BoundField DataField="ApsisUpdateCheckServicePlus" HeaderText="ApsisUpdateCheckServicePlus" />
                    <asp:BoundField DataField="HaveServicePlusAccount" HeaderText="HaveServicePlusAccount" />
                    <asp:BoundField DataField="PaperCode" HeaderText="PaperCode" />
                    <asp:BoundField DataField="ProductNo" HeaderText="ProductNo" />
                    <asp:BoundField DataField="ProductName" HeaderText="ProductName" />
                    <asp:BoundField DataField="SubsLen_Mons" HeaderText="SubsLen_Mons"/>
                </Columns>
                <FooterStyle BackColor="White" ForeColor="#000066" />
                <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                <RowStyle ForeColor="#000066" BackColor="#f3f3e6" HorizontalAlign="Center"/>
                <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                <%-- <SortedAscendingCellStyle BackColor="#F1F1F1" />
                    <SortedAscendingHeaderStyle BackColor="#007DBB" />
                    <SortedDescendingCellStyle BackColor="#CAC9C9" />
                    <SortedDescendingHeaderStyle BackColor="#00547E" />--%>
            </asp:GridView>
        </div>
    </div>
    </form>
</body>
</html>

<%--<link href="Main.css" rel="stylesheet" type="text/css" />--%>