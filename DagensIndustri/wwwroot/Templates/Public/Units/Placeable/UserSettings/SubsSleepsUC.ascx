<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SubsSleepsUC.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.UserSettings.SubsSleepsUC" %>
<%@ Register TagPrefix="DI" TagName="Input_1" Src="~/Tools/Classes/WebControls/InputWithValidation.ascx" %>
<%@ Register TagPrefix="di" TagName="UserMessage" Src="~/Templates/Public/Units/Placeable/UserMessage.ascx" %>

<script type="text/javascript" src="/Templates/Public/js/PreventDoublePost.js"></script>

<style type="text/css">
    .gvStyle { text-align:left; padding-right:10px; line-height:1.5em; }
</style>

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
        
<%--<script>

    var isAutoGiro = <%=SubIsAutoGiro.ToString().ToLower()%>;

    $(document).ready(function() {
    
         if (isAutoGiro) {
            $("#<%=CheckBoxAllowWebPaper.ClientID%>").attr('checked','checked');
            $("#<%=CheckBoxAllowWebPaper.ClientID%>").attr("disabled", true);
        } 

        if (!isAutoGiro) {
            $("#<%=Date2.ClientID%>_Input").blur(function() {
                if ($("#<%=Date2.ClientID%>_Input").val() == "") {
                    $("#<%=CheckBoxAllowWebPaper.ClientID%>").removeAttr('checked');
                }
            });
        }

    });

    function jsCheckBoxClicked() {
        if ($("#<%=Date2.ClientID%>_Input").val() == "") {
            $("#<%=CheckBoxAllowWebPaper.ClientID%>").removeAttr('checked');
            alert("För digital tillgång under uppehållet krävs ett slutdatum.");
        }
    }

    function jsValidateForm() {
        if (isAutoGiro && $("#<%=Date2.ClientID%>_Input").val() == "") {
            alert("Vänligen ange ett slutdatum.");
            return false;
        }

        return jsPreventDoublePost('divSubmitBtn', 'divFormSent');
    }

</script>--%>


        <%--<h3>Uppehåll</h3>--%>
        <div class="form-box">    
            <div class="section" id="form-street">
                <div class="row">
                    <div class="col">
				        <div class="medium">
					        <DI:Input_1 ID="Date1" CssClass="text date" Required="true" Name="date-temp-from" TypeOfInput="Date" Title="Från och med <i>(YYYY-MM-DD)</i>" DisplayMessage="Ange startdatum" runat="server" />
				        </div>								
			        </div>
			        <div class="col">
				        <div class="medium">						
                            <DI:Input_1 ID="Date2" CssClass="text date" Required="true" Name="date-temp-to" TypeOfInput="Date" Title="Till och med <i>(YYYY-MM-DD)</i>" DisplayMessage="Ange slutdatum" runat="server" />
				        </div>								
			        </div>								
		        </div>

                <div class="row">
                    <div class="col">
                        <div class="medium">
                            <asp:PlaceHolder ID="PlaceHolderAllowWebPaper" runat="server">
                                <%--onclick="jsCheckBoxClicked();"--%>
                                <asp:CheckBox ID="CheckBoxAllowWebPaper" Checked="true" runat="server" />Digital tillgång under uppehållet<br />  <%--Checked="true"--%>
                            </asp:PlaceHolder>
                            &nbsp;
                        </div>
                    </div>
                    <div class="col">
                        <div class="medium">
                            <%--<asp:PlaceHolder ID="PlaceHolderInfoReg" Visible="True" runat="server">
                                Lämna slutdatumet tomt för att skapa ett tillsvidareuppehåll.<br/><br/>
                                Digital tillgång tillåts ej vid tillsvidareuppehåll.
                            </asp:PlaceHolder>--%>
                            
                            <asp:PlaceHolder ID="PlaceHolderInfoAutogiro" Visible="False" runat="server">
                                Som autogiroprenumerant har du tillgång till din tidning digitalt under ditt uppehåll.
                            </asp:PlaceHolder>
                        </div>
                    </div>
                </div>

                <%--<div class="row">
                    <div class="col"></div>
                </div>--%>

                <div class="button-wrapper" style="margin-top:40px;">

                    <div id="divSubmitBtn">     <%--return jsValidateForm();--%>
                        <asp:Button ID="StreetFormButton" CssClass="btn" Text="Spara" OnClick="DateFormButton_Click" OnClientClick="return jsPreventDoublePost('divSubmitBtn', 'divFormSent');" runat="server" />
	                </div>
            
                    <div id="divFormSent" style="float:right; visibility:hidden;">
                        <img src="/Templates/Public/Images/loader.gif" alt="" />
                        <i>&nbsp;<asp:Literal ID="Literal1" Text="Formuläret skickas..." runat="server" /></i>
                    </div>

	            </div>

            </div>
        </div>

        <br />
    </asp:PlaceHolder>

</asp:PlaceHolder>