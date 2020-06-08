<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InfoSearch.aspx.cs" Inherits="Pren.Web.Tools.Admin.InfoSearch.InfoSearch" %>

<asp:content contentplaceholderid="FullRegion" runat="Server">
    <link href="/Static/css/lib/datepicker3.css" rel="stylesheet" />
    <%: System.Web.Optimization.Scripts.Render("~/bundles/js/global") %>    

    <style type="text/css">
        table { width: 100% !important;}
        table td{ vertical-align: top;}
        table th { font-weight: bold !important;}
        table td:nth-child(1){ width: 20px;}
        table td:nth-child(2){ width: 20px;}
        table td:nth-child(3),table td:nth-child(4){ width: 40%; word-wrap: break-word;max-width: 300px;}
        table td:nth-child(5){ width: 50px;}
        .epi-contentContainer{ max-width: 100% !important;}
        input[type=text]{ width: 213px;height: 25px;margin-bottom: 10px;}
        input[type=submit]{ width: 100px;margin-right: 13px;}
        .dateinput{ width: 100px !important;}
        .left{ float: left;margin-right: 10px;}
        .clear{ clear: both;}
    </style>

    <div class="epi-contentContainer epi-padding">
        
        <div class="epi-contentArea">
            <h1 class="EP-prefix">Infosök</h1>

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
                <asp:Label runat="server" AssociatedControlID="TxtSource"><strong>Källa:</strong></asp:Label><br/>
                <asp:TextBox runat="server" ID="TxtSource"></asp:TextBox><br/>
                                                            
                <asp:Label runat="server" AssociatedControlID="TxtDescription"><strong>Beskrivning:</strong></asp:Label><br/>
                <asp:TextBox runat="server" ID="TxtDescription"></asp:TextBox><br/><br/>

                <asp:Button runat="server" Text="Sök"  />
                <asp:Button runat="server" Text="Rensa" OnClick="BtnClearOnClick" /><br/><br/>     

            
                <asp:Label runat="server" EnableViewState="false" ID="LblError" CssClass="error"></asp:Label>
            
                <asp:GridView runat="server"  DataSourceID="GvDataSource" ID="GvResult" AutoGenerateColumns="False">
                    <Columns>
                        <asp:TemplateField >                        
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="ID" DataField="id" />
                        <asp:TemplateField HeaderText="Källa">                        
                            <ItemTemplate>
                                <%# Eval("source")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Beskrivning">                        
                            <ItemTemplate>
                                <%# Eval("description")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Datum">                        
                            <ItemTemplate>
                                <%# DateTime.Parse(Eval("Date").ToString()).ToString("yyyy-MM-dd HH:mm")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            
                <asp:ObjectDataSource runat="server" ID="GvDataSource" TypeName="Pren.Web.Tools.Admin.InfoSearch.InfoSearch" SelectMethod="GetResult">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="TxtDateFrom" Name="fromdate" PropertyName="Text" Type="DateTime" />                     
                        <asp:ControlParameter ControlID="TxtDateTo" Name="todate" PropertyName="Text" Type="DateTime" />   
                        <asp:ControlParameter ControlID="TxtSource" Name="source" PropertyName="Text"  />
                        <asp:ControlParameter ControlID="TxtDescription" Name="description" PropertyName="Text"  />
                    </SelectParameters>       
                </asp:ObjectDataSource> 
            </div>           
        </div>

    </div>
    
    
    <script type="text/javascript">
        var dateHandler = new DateHandler($(".dateinput"));
        dateHandler.init();


    </script>

</asp:content>