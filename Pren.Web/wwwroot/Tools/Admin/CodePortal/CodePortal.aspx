<%@ Page Language="C#" AutoEventWireup="true" EnableViewState="true" CodeBehind="CodePortal.aspx.cs" Inherits="Pren.Web.Tools.Admin.CodePortal.CodePortal" %>

<asp:content ContentPlaceHolderID="FullRegion" runat="Server">
       
    <link href="<%:System.Web.Optimization.Styles.Url("~/bundles/css/global") %>" rel="stylesheet" />
    <%: System.Web.Optimization.Scripts.Render("~/bundles/js/global") %>

    <style type="text/css">
        .code-portal-admin label{ font-weight: bold;}
        .code-portal-admin .validator{ color: red;margin-left: 10px;}
        .code-portal-admin input{ width: 50%;}
        .code-portal-admin .bg-info{display: block;padding: 20px; margin: 20px 0; width: 50%;}
        .epi-contentArea ul{ margin-left: 0;}
        .margin-b-20 { margin-bottom: 20px;}
        .margin-t-20 { margin-top: 20px;}
        .displaynone{ display: none;}
    </style>


    <div class="epi-contentContainer epi-padding code-portal-admin">
        <asp:HiddenField runat="server" ID="ActiveTab" ClientIDMode="Static"/>     

        <div class="epi-contentArea">
            <h1 class="EP-prefix">Kodportaladmin</h1>
    
            <div role="tabpanel">
                <!-- Nav tabs -->
                <ul class="nav nav-tabs" role="tablist">
                    <li id="newlist-tab" role="presentation" class="active"><a href="#newlist" aria-controls="newlist" role="tab" data-toggle="tab">Ny lista</a></li>
                    <li id="managelists-tab" role="presentation"><a href="#managelists" aria-controls="managelists" role="tab" data-toggle="tab">Hantera befintliga listor</a></li>                                
                </ul>
                <!-- Tab panes -->
                <div class="tab-content">
                    <div role="tabpanel" class="tab-pane active" id="newlist">
                        <asp:Label runat="server" AssociatedControlID="tbListName">Listans namn:</asp:Label>            
                        <br/><i>Visas när man väljer lista på blocket</i><br/>
                        <asp:TextBox runat="server" ID="tbListName" EnableViewState="False" CssClass="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="tbListName" ErrorMessage="Obligatoriskt fält" ValidationGroup="addList" CssClass="validator"></asp:RequiredFieldValidator>
                        <br/>
                        <asp:Label runat="server" AssociatedControlID="tbResourceId">Service+ resurs:</asp:Label><br/><i>Endast kunder med denna resurs kommer att kunna få en kod (Exempel: di.se-cmore)</i><br/>
                        <asp:TextBox runat="server" ID="tbResourceId" EnableViewState="False" CssClass="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="tbResourceId" ErrorMessage="Obligatoriskt fält" ValidationGroup="addList" CssClass="validator"></asp:RequiredFieldValidator>
                        <br/>
                        <i>Datumen nedan styr inte när ett erbjudande visas på Mina koder, det måste styras manuellt från CMS:et</i><br/>
                        <asp:Label runat="server" AssociatedControlID="tbValidFrom">Giltig från:</asp:Label><br/><%--<i>Visas när man väljer lista på blocket</i><br/>--%>
                        <asp:TextBox runat="server" ID="tbValidFrom" EnableViewState="False" CssClass="dateinput form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="tbValidFrom" ErrorMessage="Obligatoriskt fält" ValidationGroup="addList" CssClass="validator"></asp:RequiredFieldValidator>
                        <br/>
                        <asp:Label runat="server" AssociatedControlID="tbValidTo">Giltig till:</asp:Label><br/><%--<i>Visas när man väljer lista på blocket</i><br/>--%>
                        <asp:TextBox runat="server" ID="tbValidTo" EnableViewState="False" CssClass="dateinput form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="tbValidTo" ErrorMessage="Obligatoriskt fält" ValidationGroup="addList" CssClass="validator"></asp:RequiredFieldValidator>
                        <br/>
            
                        <asp:Label runat="server" AssociatedControlID="fuCodeList">.csv lista med koder</asp:Label>
                        <br/><i>Filen ska innehålla en kolumn med koder och kolmnen ska ha en rubrik som heter Code.</i><br/>
                        <img src="/Tools/Admin/CodePortal/code-file-syntax.png"/>
                        <asp:FileUpload runat="server" ID="fuCodeList"></asp:FileUpload><br/><br/>
            
                        <asp:Label runat="server" ID="lblAddListFeedback" EnableViewState="False" Visible="False" CssClass="bg-info"></asp:Label>
            
                        <asp:Button runat="server" ID="btnAddList" CssClass="btn btn-primary" Text="Lägg till lista" OnClick="btnAddList_Click" ValidationGroup="addList"/>                                           

                    </div>
                    <div role="tabpanel" class="tab-pane" id="managelists">
                        <div class="row margin-b-20">
                            <div class="col-md-6">
                                <asp:Label runat="server"  AssociatedControlID="DdCodeLists">Befintliga listor</asp:Label>                        
                                <asp:DropDownList runat="server" ID="DdCodeLists" DataValueField="Id" DataTextField="Name" CssClass="form-control" ClientIDMode="Static" />  
                            </div>
                        </div>
                        
                        <asp:Label runat="server" EnableViewState="False" ID="lblImportListFeedback" CssClass="bg-info" Visible="False"></asp:Label>

                        <div id="editpanels" class="displaynone">
                            <div class="panel panel-default margintop20">
                                <div class="panel-heading">
                                    <h3 class="panel-title">Importera koder till befintlig lista</h3>
                                </div>
                                <div class="panel-body">
                                    <asp:Label runat="server" AssociatedControlID="FuCodeListImport">.csv lista med koder</asp:Label>
                                    <br/><i>Filen ska innehålla en kolumn med koder och kolumnen ska ha en rubrik som heter Code.</i><br/>
                                    <asp:FileUpload runat="server" ID="FuCodeListImport"></asp:FileUpload>                                    
                                    <asp:Button runat="server" ID="BtnImportToExistingList" CssClass="btn btn-primary margin-t-20" Text="Lägg till lista" OnClick="BtnImportToExistingList_Click"/>
                                </div>
                            </div>

                            <div class="panel panel-default">
                                <div class="panel-heading">
                                    <h3 class="panel-title">Exportera kodlista</h3>
                                </div>
                                <div class="panel-body">
                                    <asp:Label runat="server" AssociatedControlID="TxtCsvLimiter">Avgränsare</asp:Label>
                                    <asp:TextBox runat="server" ID="TxtCsvLimiter" Text="," CssClass="form-control" Width="100"></asp:TextBox>
                                    <asp:Button runat="server" ID="BtnExport" CssClass="btn btn-primary margin-t-20" Text="Exportera lista till Csv" OnClick="BtnExport_Click"/>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            
            


            
        </div>

    </div>
    
    <script type="text/javascript">
        var dateHandler = new DateHandler($(".dateinput"));
        dateHandler.init();



        // Set active tab
        var activeTab = $("#ActiveTab").val();
        if (activeTab.length) {
            $(".tab-pane, .nav-tabs li").removeClass("active");
            $("#" + activeTab + ", #" + activeTab + "-tab").addClass("active");
        }



        var feedback = $(".bg-info");
        var list = $("#DdCodeLists");
        // Attach event for list change
        list.on("change", function() {
            feedback.hide();
            setPanelVisibility();
        });

        var panels = $("#editpanels");        

        // Set default panel
        setPanelVisibility();

        function setPanelVisibility() {
            
            if (list.val() === "0") {
                panels.slideUp();
                return;
            }            
            panels.slideDown();
        }

    </script>

</asp:content>