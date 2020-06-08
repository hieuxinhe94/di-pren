<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MailSearch.aspx.cs" Inherits="Pren.Web.Tools.Admin.MailSearch.MailSearch" %>


<asp:content contentplaceholderid="FullRegion" runat="Server">
    <link href="/Static/css/lib/datepicker3.css" rel="stylesheet" />
    <%: System.Web.Optimization.Scripts.Render("~/bundles/js/global") %>    

    <style type="text/css">
        table { width: 100% !important;}
            table th {font-weight: bold !important;}
            table td{ vertical-align: top;}
            table td:nth-child(1) {width: 20px;}
            table td:nth-child(2) {width: 20px;}
            table td:nth-child(6) { width: 30%;word-wrap: break-word; max-width: 300px;}
            table td:nth-child(8),table td:nth-child(9) {width: 50px;}
        .epi-contentContainer {max-width: 100% !important;}
        input[type=text] {width: 213px;height: 25px;margin-bottom: 10px;}
        input[type=submit] {width: 100px;margin-right: 13px;}
        .dateinput {width: 100px !important;}
        .left {float: left;margin-right: 10px;}
        .clear {clear: both;}
    </style>
    <div class="epi-contentContainer epi-padding">
        
        <div class="epi-contentArea">
            <h1 class="EP-prefix">Mailsök</h1>
    
            <p class="EP-systemInfo">Sökning i loggdatabas.</p>
            
            <div class="left">
                <asp:Label runat="server" AssociatedControlID="TxtDateFrom"><strong>Från:</strong></asp:Label><br/>
                <asp:TextBox runat="server" CssClass="dateinput" ClientIDMode="Static" ID="TxtDateFrom"></asp:TextBox><br/>
            </div>
            <div class="left">
                <asp:Label runat="server" AssociatedControlID="TxtDateTo"><strong>Till:</strong></asp:Label><br/>
                <asp:TextBox runat="server" CssClass="dateinput" ClientIDMode="Static" ID="TxtDateTo"></asp:TextBox><br/>
            </div>
            
            <div class="clear">
                <asp:Label runat="server" AssociatedControlID="TxtToAddress"><strong>Till adress:</strong></asp:Label><br/>
                <asp:TextBox runat="server" ID="TxtToAddress"></asp:TextBox><br/>
                
                <asp:Label runat="server" AssociatedControlID="TxtFromAddress"><strong>Från adress:</strong></asp:Label><br/>
                <asp:TextBox runat="server" ID="TxtFromAddress"></asp:TextBox><br/>
                  
                <asp:Label runat="server" AssociatedControlID="TxtSubject"><strong>Ämne:</strong></asp:Label><br/>
                <asp:TextBox runat="server" ID="TxtSubject"></asp:TextBox><br/>
                
                <asp:Label runat="server" AssociatedControlID="TxtBody"><strong>Meddelande:</strong></asp:Label><br/>
                <asp:TextBox runat="server" ID="TxtBody"></asp:TextBox><br/><br/>
                
                <asp:Button ID="Button1" runat="server" Text="Sök"  />
                <asp:Button ID="Button2" runat="server" Text="Rensa" OnClick="BtnClearOnClick" /><br/><br/>        

                <asp:Label runat="server" EnableViewState="false" ID="LblError" CssClass="error"></asp:Label>
            
                <asp:GridView runat="server" DataSourceID="GvDataSource" ID="GvResult" AutoGenerateColumns="False" >
                    <Columns>
                        <asp:TemplateField >                        
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="ID" DataField="id" />
                        <asp:TemplateField HeaderText="Till">                        
                            <ItemTemplate>
                                <%# Eval("ToAddress")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Från">                        
                            <ItemTemplate>
                                <%# Eval("FromAddress")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Ämne">                        
                            <ItemTemplate>
                                <%# Eval("Subject")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Meddelande">                        
                            <ItemTemplate>
                                <%# Eval("Message")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="IP">                        
                            <ItemTemplate>
                                <%# Eval("IP")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Resultat">                        
                            <ItemTemplate>
                                <%# Eval("Result")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Datum">                        
                            <ItemTemplate>
                                <%# DateTime.Parse(Eval("SendDate").ToString()).ToString("yyyy-MM-dd HH:mm")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
    <asp:ObjectDataSource runat="server" ID="GvDataSource" TypeName="Pren.Web.Tools.Admin.MailSearch.MailSearch" SelectMethod="GetResult">
        <SelectParameters>
            <asp:ControlParameter ControlID="TxtDateFrom" Name="fromdate" PropertyName="Text" Type="DateTime"/>                     
            <asp:ControlParameter ControlID="TxtDateTo" Name="todate" PropertyName="Text" Type="DateTime"/>    
            <asp:ControlParameter ControlID="TxtFromAddress" Name="fromaddress" PropertyName="Text" />   
            <asp:ControlParameter ControlID="TxtToAddress" Name="toaddress" PropertyName="Text" />    
            <asp:ControlParameter ControlID="TxtSubject" Name="subject" PropertyName="Text"  />
            <asp:ControlParameter ControlID="TxtBody" Name="body" PropertyName="Text" />
        </SelectParameters>       
    </asp:ObjectDataSource>            

    <script type="text/javascript">
        var dateHandler = new DateHandler($(".dateinput"));
        dateHandler.init();


    </script>

</asp:content>
