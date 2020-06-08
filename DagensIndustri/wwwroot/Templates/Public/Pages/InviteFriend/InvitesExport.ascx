<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InvitesExport.ascx.cs" Inherits="DagensIndustri.Templates.Public.Pages.InviteFriend.InvitesExport" %>


<div style="margin:10px;">

    <div style="margin-bottom:10px">
        <strong>Kundnummer:</strong><br />
        <asp:TextBox runat="server" ID="TxtCusno"></asp:TextBox>
        <asp:Button runat="server" Text="Visa tips" OnClick="BtnGetInvitesOnClick" />
        <asp:Button runat="server" Text="Exportera tips till Excel" OnClick="BtnInvitesOnClick" />
    </div>

    <asp:GridView 
        runat="server" 
        EnableViewState="true" 
        CssClass="AdminTable" 
        AlternatingRowStyle-BackColor="#F9F8F8" 
        HeaderStyle-BackColor="#BCBCBC" 
        GridLines="Both" 
        ID="GvInvites" 
        AutoGenerateColumns="false" AllowPaging="true" PageSize="100" OnPageIndexChanging="GvInvitesPageIndexChanging">    
        <Columns>    
            <asp:TemplateField>
                <ItemTemplate>
                    <div title='ID:<%#Eval("id") %>' style="text-decoration:underline;cursor:pointer;"><%# Container.DataItemIndex + 1%></div>
                </ItemTemplate>
            </asp:TemplateField>                                        
            <asp:BoundField HeaderText="Cusno" DataField="sendercusno" />
            <asp:BoundField HeaderText="Avsändare förnamn" DataField="senderfirstname" />
            <asp:BoundField HeaderText="Avsändare efternamn" DataField="senderlastname" />
            <asp:BoundField HeaderText="Avsändare meddelande" DataField="sendermessage" />                       
            <asp:BoundField HeaderText="Mottagare förnamn" DataField="receiverfirstname" />       
            <asp:BoundField HeaderText="Mottagare efternamn" DataField="receiverlastname" />       
            <asp:BoundField HeaderText="Mottagare e-post" DataField="receiveremail" />       
            <asp:BoundField HeaderText="Mottagare telefonnummer" DataField="receiverphone" />    
            <asp:TemplateField>
                <HeaderTemplate>
                    Prenumerant
                </HeaderTemplate>
                <ItemTemplate>
                    <div title='PrenGuid: <%#Eval("prenguid") %>' style="text-decoration:underline;cursor:pointer;"><%# ((bool)Eval("pren")) ? "Ja" : "Nej"%></div>
                </ItemTemplate>
            </asp:TemplateField>                       
            <asp:TemplateField>
                <HeaderTemplate>
                    Datum
                </HeaderTemplate>
                <ItemTemplate>
                    <%# DateTime.Parse(Eval("created").ToString()).ToString("yyyy-MM-dd HH:mm")%>
                </ItemTemplate>
            </asp:TemplateField>                                                                          
        </Columns>
    </asp:GridView>

</div>