<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Activities.ascx.cs" Inherits="DagensIndustri.Tools.Admin.Conference.Activities" %>

<asp:Repeater runat="server" ID="repAct">
    <HeaderTemplate>
        <table class="editactivities">
    </HeaderTemplate>
    <ItemTemplate>
        <tr>
            <%--Eventname--%>
            <td>                
                (<%#DataBinder.Eval(Container.DataItem, "eventname").ToString()%>)
            </td>
            <%--Activityname--%>
            <td>                
                <%#DataBinder.Eval(Container.DataItem, "name").ToString()%>
            </td>
            <td>
                <asp:ImageButton runat="server" ToolTip="Lägg till" ID="ImgAdd" OnPreRender="show" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "activityid").ToString() %>'   ImageUrl="/tools/admin/styles/images/add.png" OnClick="AddActivity" />
                <asp:ImageButton runat="server" ToolTip="Ta bort" ID="ImgDelete" OnPreRender="show" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "activityid").ToString() %>'  ImageUrl="/tools/admin/styles/images/delete.png" OnClick="DeleteActivity" />
            </td>
        </tr>        
    </ItemTemplate>
    <FooterTemplate>
        </table>
    </FooterTemplate>
</asp:Repeater>