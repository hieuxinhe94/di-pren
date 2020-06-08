<%@ Page Language="C#" MasterPageFile="~/Templates/Public/MasterPages/MasterPage.Master" AutoEventWireup="true" CodeBehind="SubsSleeps.aspx.cs" Inherits="DagensIndustri.Templates.Public.Pages.UserSettings.SubsSleeps" %>
<%@ Register TagPrefix="di" TagName="UserMessage" Src="~/Templates/Public/Units/Placeable/UserMessage.ascx" %>
<%@ Register TagPrefix="di" TagName="MySettingsMenu" src="~/Templates/Public/Pages/UserSettings/MySettingsMenu.ascx" %>
<%@ Register TagPrefix="di" TagName="Input" Src="~/Tools/Classes/WebControls/InputWithValidation.ascx" %>
<%@ Register TagPrefix="di" TagName="Mainbody" Src="~/Templates/Public/Units/Placeable/MainBody.ascx" %>
<%@ Register TagPrefix="di" TagName="heading" Src="~/Templates/Public/Units/Placeable/Heading.ascx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadingContentPlaceHolder" runat="server">
    <%--<di:heading ID="Heading1" runat="server" />--%>
    <script type="text/javascript" src="/Templates/Public/js/PreventDoublePost.js"></script>
</asp:Content>


<asp:Content ContentPlaceHolderID="TopContentPlaceHolder" runat="server" ></asp:Content>


<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    
    <di:MySettingsMenu ID="MySettingsMenu1" runat="server" />

    <style type="text/css">
        .gvStyle { text-align:left; padding-right:10px; line-height:1.5em; }
    </style>



    <%--<h2>Uppehåll</h2>--%>
    <h2><%= CurrentPage["Heading"] ?? CurrentPage.PageName %></h2>
    <di:Mainbody ID="Mainbody1" runat="server" />


    <di:UserMessage ID="UserMessageControl" runat="server" />

    <asp:PlaceHolder ID="PlaceHolderFutureSubsSleepsAndForm" runat="server">

        <asp:PlaceHolder ID="PlaceHolderFutureSubsSleeps" runat="server">
            <h3>Framtida uppehåll</h3>
            
            <asp:GridView ID="gvSubsSleeps" DataKeyNames="Id" OnSelectedIndexChanged="gvSubsSleeps_SelectedIndexChanged" OnRowCommand="gvSubsSleeps_OnRowCommand" OnRowDataBound="gvSubsSleeps_RowDataBound" SelectedRowStyle-BackColor="#cccccc" AutoGenerateColumns="false" runat="server">
                <Columns>
                    <asp:BoundField DataField="SleepStartDate" HeaderText="Från och med" DataFormatString="{0:yyyy-MM-dd}" HeaderStyle-CssClass="gvStyle" ItemStyle-CssClass="gvStyle" />
                    <asp:BoundField DataField="SleepEndDate" HeaderText="Till och med" DataFormatString="{0:yyyy-MM-dd}" HeaderStyle-CssClass="gvStyle" ItemStyle-CssClass="gvStyle" />
            
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="lbtnEdit" runat="server" Text="Redigera" CommandName="Select" CommandArgument='<%# Eval("Id") %>' Visible='<%#(bool)Eval("CanBeEdited") %>' CssClass="gvStyle" />
                            <asp:LinkButton ID="lbtnDelete" runat="server" Text="Radera" CommandName="Delete" CommandArgument='<%# Eval("Id") %>' Visible='<%#(bool)Eval("CanBeDeleted") %>' CssClass="gvStyle" OnClientClick="return confirm ('Är du säker på att du vill radera uppehållet?')" />
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Literal ID="Literal1" runat="server" Text='<%#Eval("Comment") %>' Visible='<%#(bool)Eval("HasComment") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>

                </Columns>
            </asp:GridView>
            <br />
        </asp:PlaceHolder>

        <p><asp:LinkButton ID="LinkButtonNewAddress" runat="server" Text="Nytt uppehåll" onclick="LinkButtonNewAddress_Click" /></p>
        
        <asp:PlaceHolder ID="PlaceHolderForm" Visible="false" runat="server">
            <%--<h3>Uppehåll</h3>--%>
            <div class="form-box">    
                <div class="section" id="form-street">
                    <div class="row">
                        <div class="col">
				            <div class="medium">
					            <DI:Input ID="Date1" CssClass="text date" Required="true" Name="date-temp-from" TypeOfInput="Date" Title="Från och med <i>(YYYY-MM-DD)</i>" DisplayMessage="Ange datum" runat="server" />
				            </div>								
			            </div>
			            <div class="col">
				            <div class="medium">						
                                <DI:Input ID="Date2" CssClass="text date" Name="date-temp-to" TypeOfInput="Date" Title="Till och med <i>(YYYY-MM-DD)</i>" runat="server" />
				            </div>								
			            </div>								
		            </div>

                    <div class="row">
                        <div class="col">
                            <div class="medium">
                                <asp:PlaceHolder ID="PlaceHolderAllowWebPaper" runat="server">
                                    <asp:CheckBox ID="CheckBoxAllowWebPaper" runat="server" />Digital tillgång under uppehållet<br />
                                    <%--<br />(Vid digital tillgång förbrukas prenumerationsdagar som vanligt, men tidningen levereras inte fysiskt).--%>
                                </asp:PlaceHolder>
                            </div>
                        </div>
                        <div class="col">
                            <div class="medium">
                                (Lämna till-datumet tomt för att skapa ett tillsvidareuppehåll)
                            </div>
                        </div>
                    </div>

                    <%--<div class="row">
                        <div class="col"></div>
                    </div>--%>

                    <div class="button-wrapper">

                        <div id="divSubmitBtn">
                            <asp:Button ID="StreetFormButton" CssClass="btn" Text="Spara"  OnClick="DateFormButton_Click" OnClientClick="return jsPreventDoublePost('divSubmitBtn', 'divFormSent');" runat="server" />
	                    </div>
            
                        <div id="divFormSent" style="float:right; visibility:hidden;">
                            <img src="/Templates/Public/Images/loader.gif" alt="" />
                            <i>&nbsp;<asp:Literal ID="Literal1" Text="<%$ Resources: EPiServer, common.sendingform %>" runat="server" /></i>
                        </div>

	                </div>

                </div>
            </div>

            <br />
        </asp:PlaceHolder>

    </asp:PlaceHolder>


</asp:Content>
