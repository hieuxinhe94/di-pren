<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ParticipantExport.ascx.cs" Inherits="DagensIndustri.Templates.Public.Pages.Competition.ParticipantExport" %>


<div style="margin:10px;">

    <div style="margin-bottom:10px">
        <asp:Button runat="server" Text="Visa deltagare" OnClick="BtnGetParticipantsOnClick" />
        <asp:Button runat="server" Text="Exportera deltagare till Excel" OnClick="BtnExportOnClick" />
    </div>

    <asp:GridView 
        runat="server" 
        EnableViewState="true" 
        CssClass="AdminTable" 
        AlternatingRowStyle-BackColor="#F9F8F8" 
        HeaderStyle-BackColor="#BCBCBC" 
        GridLines="Both" 
        ID="GvParticipants" 
        AutoGenerateColumns="false" AllowPaging="true" PageSize="100" OnPageIndexChanging="GvParticipantsPageIndexChanging">    
        <Columns>    
            <asp:TemplateField>
                <ItemTemplate>
                    <div title='ID:<%#Eval("participantId") %>' style="text-decoration:underline;cursor:pointer;"><%# Container.DataItemIndex + 1%></div>
                </ItemTemplate>
            </asp:TemplateField>                                        
            <asp:BoundField HeaderText="Förnamn" DataField="firstname" />
            <asp:BoundField HeaderText="Efternamn" DataField="lastname" />
            <asp:BoundField HeaderText="Epost" DataField="email" />        
            <asp:TemplateField>
                <HeaderTemplate>
                    Telefon
                </HeaderTemplate>
                <ItemTemplate>
                    &nbsp;<%#Eval("phone") %>
                </ItemTemplate>
            </asp:TemplateField>  
            <asp:TemplateField>
                <HeaderTemplate>
                    Svar
                </HeaderTemplate>
                <ItemTemplate>
                    <%# string.IsNullOrEmpty(Eval("answers") as string) ? "saknas" : ((string)Eval("answers")).Replace(Environment.NewLine, "<br style='mso-data-placement:same-cell;'/>")%>                    
                </ItemTemplate>
            </asp:TemplateField>   
            <asp:TemplateField>
                <HeaderTemplate>
                    Rätt svar
                </HeaderTemplate>
                <ItemTemplate>
                    <%# ((bool)Eval("iscorrect")) ? "Ja" : "Nej"%>
                </ItemTemplate>
            </asp:TemplateField>                                      
            <asp:TemplateField>
                <HeaderTemplate>
                    Läser di idag
                </HeaderTemplate>
                <ItemTemplate>
                    <%# GetReaderValue(Eval("readerid").ToString()) %>
                </ItemTemplate>
            </asp:TemplateField>     
            <asp:TemplateField>
                <HeaderTemplate>
                    Erhåll info
                </HeaderTemplate>
                <ItemTemplate>
                    <%# ((bool)Eval("receiveinfo")) ? "Ja" : "Nej"%>
                </ItemTemplate>
            </asp:TemplateField>        
            <asp:TemplateField>
                <HeaderTemplate>
                    Blev prenumerant
                </HeaderTemplate>
                <ItemTemplate>
                    <%# ((bool)Eval("pren")) ? "Ja" : "Nej"%>
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