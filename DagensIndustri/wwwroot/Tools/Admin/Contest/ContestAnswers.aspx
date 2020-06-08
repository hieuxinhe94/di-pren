<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ContestAnswers.aspx.cs" Inherits="DagensIndustri.Tools.Admin.Contest.ContestAnswers" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Tävlingssvar</title>

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
        
        .error { color:#FF0000; }
    </style>  
    <link href="/Tools/Admin/Styles/StyleSheetPublic.css" rel="stylesheet" type="text/css" />    
</head>
<body>
    <form id="ContestAnswersForm" runat="server">
        <div id="MainBodyArea">
            <h1>Tävlingssvar</h1>
            <div class="left">
                <div id="error_message" runat="server" class="error">
                    <%#ErrorMessage %>
                </div>
                <div>
        
                    Filtrera på sida:
                    <asp:DropDownList ID="ContestsDropDownList" runat="server" DataSource='<%#ContestPages %>' DataTextField="PageName" DataValueField="PageLink">
                    </asp:DropDownList>
                    <asp:Button ID="FilterButton" runat="server" Text="Uppdatera" OnClick="FilterButton_Click" />
                    <asp:LinkButton ID="ExportToExcelButton" runat="server" Text="Exportera till excel" OnClick="ExportToExcelButton_Click" />
                </div>
                <div>
                    <asp:GridView ID="AnswersGrid" runat="server" DataSource='<%#Answers %>' AutoGenerateColumns="false" CssClass="AdminTable">
                        <Columns>
                            <asp:BoundField HeaderText="ID" DataField="id" />
                            <asp:BoundField HeaderText="Kundnr" DataField="cusno" />
                            <asp:BoundField HeaderText="Namn" DataField="name" />
                            <asp:BoundField HeaderText="Epost" DataField="mail" />
                            <asp:BoundField HeaderText="Tel" DataField="phone" />
                            <asp:BoundField HeaderText="Svar" DataField="answerData" />
                            <asp:BoundField HeaderText="Datum" DataField="date" />
                        </Columns>
                
                    </asp:GridView>

                </div>
            </div>
        </div>
    </form>
</body>
</html>
