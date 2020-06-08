<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SeminarControl.ascx.cs" Inherits="DagensIndustri.Tools.Properties.SeminarControl" %>

<asp:UpdatePanel runat="server" ID="updPanelOfferCodes" UpdateMode="Conditional">
    <ContentTemplate>
    <style type="text/css">
        .selected-node a
        {
            background-color:#555555;
            color:#ffffff;
        }
        .conference-event-error
        {
            border:1px solid #ff0000;  
            background-color:#ffffff;          
            color:#ffffff;
            padding:10px 10px 10px 10px;
            font-weight:bold;
        }
        .add-new-event-btn
        {
            margin-bottom:10px;
        }

    </style>
        <div class="conference-event-error" runat="server" id="errorDiv" visible="false">
            <asp:Label runat="server" CssClass="error" ID="LblError" EnableViewState="false" Font-Bold="true"></asp:Label>
        </div>
        <div style="clear:both;"></div>
        <div style="float:left;">
            <asp:LinkButton ID="btnShowNewEventFields" runat="server" Text="Lägg till event" OnClick="btnShowNewEventFields_Click" Font-Bold="true" CssClass="add-new-event-btn" />
            <br /><br />
            <asp:TreeView ID="tvEvents" runat="server" ExpandDepth="3" OnSelectedNodeChanged="tvEvents_NodeChanged" 
                
                SelectedNodeStyle-CssClass="selected-node"
             >                
            </asp:TreeView>
        </div>
        <div style="float:left;">
        <asp:MultiView ID="multiEditFields" runat="server">
            <asp:View ID="viewAddEvent" runat="server">
                <h4>Lägg till Event</h4>
                <div>
                    <strong>Namn</strong><br />
                    <asp:TextBox ID="tbAddNewEventName" runat="server" />
                </div>
                <div>
                    <strong>Datum (åååå-mm-dd)</strong><br />
                    <asp:TextBox ID="tbAddNewEventDate" runat="server" />
                </div>
                <div style="float:left;">                    
                    <asp:Button ID="btnAddNewEvent" runat="server" Text="Spara" OnClick="btnAddNewEvent_Click" />
                </div>
            </asp:View>
            <asp:View ID="viewEditEvent" runat="server">
                <h4>Redigera Event</h4>
                <div>
                    <strong>Namn</strong><br />
                    <asp:TextBox ID="tbEventName" runat="server" />
                </div>
                <div>
                    <strong>Datum (åååå-mm-dd)</strong><br />
                    <asp:TextBox ID="tbEventDate" runat="server" />
                </div>
                <div style="float:left;">
                    <asp:LinkButton ID="btnDeleteEvent" runat="server" Text="Radera event" OnClick="btnDeleteEvent_Click" ForeColor="Red" OnClientClick="return confirm('Är du säker på att du vill radera event?');" />
                    <asp:Button ID="btnUpdateEvent" runat="server" Text="Spara" OnClick="btnUpdateEvent_Click" />
                </div>
                <div style="clear:both;"></div>
                <hr />
                <h4>Lägg till tidpunkt</h4>
                <div>
                    <strong>Start</strong><br />
                    <asp:DropDownList ID="ddlAddNewTimeStartHour" runat="server" DataSource='<%#GetHours() %>' AppendDataBoundItems="true">
                        <asp:ListItem Value="" Text="tt" />
                    </asp:DropDownList>
                    <asp:DropDownList ID="ddlAddNewTimeStartMinute" runat="server" DataSource='<%#GetMinutes() %>' AppendDataBoundItems="true">
                        <asp:ListItem Value="" Text="mm" />
                    </asp:DropDownList>
                </div>
                <div>
                    <strong>Slut</strong><br /> 
                    <asp:DropDownList ID="ddlAddNewTimeEndHour" runat="server" DataSource='<%#GetHours() %>' AppendDataBoundItems="true">
                        <asp:ListItem Value="" Text="tt" />
                    </asp:DropDownList>
                    <asp:DropDownList ID="ddlAddNewTimeEndMinute" runat="server" DataSource='<%#GetMinutes() %>' AppendDataBoundItems="true">
                        <asp:ListItem Value="" Text="mm" />
                    </asp:DropDownList>
                </div>
                <div style="float:left;">
                    <asp:Button ID="btnAddNewTime" runat="server" Text="Lägg till tidpunkt" OnClick="btnAddNewTime_Click"  />
                </div>
            </asp:View>
            <asp:View ID="viewEditTime" runat="server">
                <h4>Redigera Tid</h4>                
                <div>
                    <strong>Start</strong><br />
                    <asp:DropDownList ID="ddlTimeStartHour" runat="server" DataSource='<%#GetHours() %>' AppendDataBoundItems="true" >
                        <asp:ListItem Value="" Text="tt" />
                    </asp:DropDownList>
                    <asp:DropDownList ID="ddlTimeStartMinute" runat="server" DataSource='<%#GetMinutes() %>' AppendDataBoundItems="true">
                        <asp:ListItem Value="" Text="mm" />
                    </asp:DropDownList>
                </div>
                <div>
                    <strong>Slut</strong><br /> 
                    <asp:DropDownList ID="ddlTimeEndHour" runat="server" DataSource='<%#GetHours() %>' AppendDataBoundItems="true">
                        <asp:ListItem Value="" Text="tt" />
                    </asp:DropDownList>
                    <asp:DropDownList ID="ddlTimeEndMinute" runat="server" DataSource='<%#GetMinutes() %>' AppendDataBoundItems="true">
                        <asp:ListItem Value="" Text="mm" />
                    </asp:DropDownList>
                </div>
                <div>
                    <asp:LinkButton ID="lbtnDeleteTime" runat="server" Text="Radera tid" OnClick="btnDeleteTime_Click" ForeColor="Red" OnClientClick="return confirm('Är du säker på att du vill radera tiden?');" />
                    <asp:Button ID="btnUpdateTime" runat="server" Text="Spara" OnClick="btnUpdateTime_Click"  />
                </div>
                <div style="clear:both;"></div>
                <hr />
                <h4>Lägg till aktivitet</h4>
                <div>
                    <strong>Namn</strong><br />
                    <asp:TextBox ID="tbAddnewActivityName" runat="server" />
                </div>
                <div>
                    <strong>Max antal</strong><br />
                    <asp:TextBox ID="tbAddNewActivityMaxParticipants" runat="server" />
                </div>
                <div style="float:left;">
                    <asp:Button ID="btnAddNewActivity" runat="server" Text="Spara" OnClick="btnAddNewActivity_Click" />
                </div>
            </asp:View>
            <asp:View ID="viewEditActivity" runat="server">
                <h4>Redigera Aktivitet</h4>
                <div>
                    <strong>Namn</strong><br />
                    <asp:TextBox ID="tbActivityName" runat="server" />
                </div>
                <div>
                    <strong>Max antal</strong><br />
                    <asp:TextBox ID="tbMaxParticipants" runat="server" />
                </div>
                <div style="float:left;">
                    <asp:LinkButton ID="btnDeleteActivity" runat="server" Text="Radera aktivitet" OnClick="btnDeleteActivity_Click" ForeColor="Red" OnClientClick="return confirm('Är du säker på att du vill radera aktiviteten?');" />
                    <asp:Button ID="btnUpdateActivity" runat="server" Text="Spara" OnClick="btnUpdateActivity_Click" />
                </div>
            </asp:View>
        </asp:MultiView>
        </div>
        <div style="clear:both;"></div>
        <!--
        <table>
            <tr>
                <td colspan="3">
                    <div style="color:Red;font-weight:bold">
                        
                    </div>
                </td>
            </tr>         
            <tr>
                <td>
                    <strong>Event</strong><br />                    
                    <asp:PlaceHolder runat="server" ID="PhAddEvent" Visible="false">
                        <div style="border:solid 1px black;background-color:#FFF;padding:5px;margin-bottom:10px;">
                            <div>
                                <strong>Namn</strong>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="AddEvent" Display="Dynamic" ErrorMessage="* Får inte vara tomt" ControlToValidate="TxtEventName"></asp:RequiredFieldValidator>
                                <br />                        
                                <asp:TextBox runat="server" Width="150" ID="TxtEventName"></asp:TextBox>
                            </div>
                            <div>
                                <strong>Datum (åååå-mm-dd)</strong>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ValidationGroup="AddEvent" Display="Dynamic" ErrorMessage="* Får inte vara tomt" ControlToValidate="TxtEventDate"></asp:RequiredFieldValidator>                        
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ValidationGroup="AddEvent" Display="Dynamic" ErrorMessage="* Ej giltigt datumformat" ValidationExpression="\d\d\d\d-\d\d-\d\d" ControlToValidate="TxtEventDate"></asp:RegularExpressionValidator>                        
                                <br />
                                <asp:TextBox runat="server" Width="150" ID="TxtEventDate"></asp:TextBox>
                            </div>
                            <asp:Button runat="server" ID="BtnAddEvent" Text="Skapa" ValidationGroup="AddEvent" OnClick="BtnAddEventOnClick" />
                            <asp:Button runat="server" ID="BtnHideEvent" OnClick="BtnShowAddEventOnClick" Text="Dölj" />
                        </div>
                    </asp:PlaceHolder>                    
                    <asp:ListBox runat="server" ID="ListEvents" Width="250" AutoPostBack="true" OnSelectedIndexChanged="ShowTimesForEvent"></asp:ListBox><br />
                    <asp:LinkButton runat="server" ID="LbAddEvent" OnClick="BtnShowAddEventOnClick" Text="Lägg till"></asp:LinkButton>
                    <asp:LinkButton runat="server" ID="LbDeleteEvent" OnClick="BtnDeleteEventOnClick" Text="Ta bort"></asp:LinkButton>
                </td>
                <td>
                    <strong>Tidpunkter</strong><br />                   
                    <asp:PlaceHolder runat="server" ID="PhAddTime" Visible="false">
                        <div style="border:solid 1px black;background-color:#FFF;padding:5px;margin-bottom:10px;">
                            <div>
                                <strong>Start</strong><br />
                                <asp:DropDownList runat="server" ID="StartHour">
                                <asp:ListItem Value="00"></asp:ListItem>
                                <asp:ListItem Value="01"></asp:ListItem>
                                <asp:ListItem Value="02"></asp:ListItem>
                                <asp:ListItem Value="03"></asp:ListItem>
                                <asp:ListItem Value="04"></asp:ListItem>
                                <asp:ListItem Value="05"></asp:ListItem>
                                <asp:ListItem Value="06"></asp:ListItem>
                                <asp:ListItem Value="07"></asp:ListItem>
                                <asp:ListItem Value="08"></asp:ListItem>
                                <asp:ListItem Value="09"></asp:ListItem>
                                <asp:ListItem Value="10" Selected="True"></asp:ListItem>
                                <asp:ListItem Value="11"></asp:ListItem>
                                <asp:ListItem Value="12"></asp:ListItem>
                                <asp:ListItem Value="13"></asp:ListItem>
                                <asp:ListItem Value="14"></asp:ListItem>
                                <asp:ListItem Value="15"></asp:ListItem>
                                <asp:ListItem Value="16"></asp:ListItem>
                                <asp:ListItem Value="17"></asp:ListItem>
                                <asp:ListItem Value="18"></asp:ListItem>
                                <asp:ListItem Value="19"></asp:ListItem>
                                <asp:ListItem Value="20"></asp:ListItem>
                                <asp:ListItem Value="21"></asp:ListItem>
                                <asp:ListItem Value="22"></asp:ListItem>
                                <asp:ListItem Value="23"></asp:ListItem>
                            </asp:DropDownList>                        
                                <asp:DropDownList runat="server" ID="StartMinute">
                                <asp:ListItem Value="00"></asp:ListItem>
                                <asp:ListItem Value="05"></asp:ListItem>
                                <asp:ListItem Value="10"></asp:ListItem>
                                <asp:ListItem Value="15"></asp:ListItem>
                                <asp:ListItem Value="20"></asp:ListItem>
                                <asp:ListItem Value="25"></asp:ListItem>
                                <asp:ListItem Value="30"></asp:ListItem>
                                <asp:ListItem Value="35"></asp:ListItem>
                                <asp:ListItem Value="40"></asp:ListItem>
                                <asp:ListItem Value="45"></asp:ListItem>
                                <asp:ListItem Value="50"></asp:ListItem>
                                <asp:ListItem Value="55"></asp:ListItem>
                            </asp:DropDownList>
                            </div>
                            <div>
                                <strong>Slut</strong><br />
                                <asp:DropDownList runat="server" ID="EndHour">
                                <asp:ListItem Value="00"></asp:ListItem>
                                <asp:ListItem Value="01"></asp:ListItem>
                                <asp:ListItem Value="02"></asp:ListItem>
                                <asp:ListItem Value="03"></asp:ListItem>
                                <asp:ListItem Value="04"></asp:ListItem>
                                <asp:ListItem Value="05"></asp:ListItem>
                                <asp:ListItem Value="06"></asp:ListItem>
                                <asp:ListItem Value="07"></asp:ListItem>
                                <asp:ListItem Value="08"></asp:ListItem>
                                <asp:ListItem Value="09"></asp:ListItem>
                                <asp:ListItem Value="10"></asp:ListItem>
                                <asp:ListItem Value="11" Selected="True"></asp:ListItem>
                                <asp:ListItem Value="12"></asp:ListItem>
                                <asp:ListItem Value="13"></asp:ListItem>
                                <asp:ListItem Value="14"></asp:ListItem>
                                <asp:ListItem Value="15"></asp:ListItem>
                                <asp:ListItem Value="16"></asp:ListItem>
                                <asp:ListItem Value="17"></asp:ListItem>
                                <asp:ListItem Value="18"></asp:ListItem>
                                <asp:ListItem Value="19"></asp:ListItem>
                                <asp:ListItem Value="20"></asp:ListItem>
                                <asp:ListItem Value="21"></asp:ListItem>
                                <asp:ListItem Value="22"></asp:ListItem>
                                <asp:ListItem Value="23"></asp:ListItem>
                            </asp:DropDownList>                        
                                <asp:DropDownList runat="server" ID="EndMinute">
                                <asp:ListItem Value="00"></asp:ListItem>
                                <asp:ListItem Value="05"></asp:ListItem>
                                <asp:ListItem Value="10"></asp:ListItem>
                                <asp:ListItem Value="15"></asp:ListItem>
                                <asp:ListItem Value="20"></asp:ListItem>
                                <asp:ListItem Value="25"></asp:ListItem>
                                <asp:ListItem Value="30"></asp:ListItem>
                                <asp:ListItem Value="35"></asp:ListItem>
                                <asp:ListItem Value="40"></asp:ListItem>
                                <asp:ListItem Value="45"></asp:ListItem>
                                <asp:ListItem Value="50"></asp:ListItem>
                                <asp:ListItem Value="55"></asp:ListItem>
                            </asp:DropDownList>
                            </div>
                            <asp:Button runat="server" ID="BtnAddTime" Text="Skapa" OnClick="BtnAddTimeOnClick" />
                            <asp:Button runat="server" ID="BtnHideTime" OnClick="BtnShowAddTimeOnClick" Text="Dölj" />
                        </div>
                    </asp:PlaceHolder>    
                    <asp:ListBox runat="server" ID="ListTime" Width="100" AutoPostBack="true" OnSelectedIndexChanged="ShowActivitiesForTime"></asp:ListBox><br />                               
                    <asp:LinkButton runat="server" ID="LbAddTime" OnClick="BtnShowAddTimeOnClick" Text="Lägg till"></asp:LinkButton>
                    <asp:LinkButton runat="server" ID="LbDeleteTime" OnClick="BtnDeleteTimeOnClick" Text="Ta bort"></asp:LinkButton>                    
                </td>                
                <td>
                    <strong>Eventaktiviteter</strong><br />
                    <asp:PlaceHolder runat="server" ID="PhAddActivity" Visible="false">
                        <div style="border:solid 1px black;background-color:#FFF;padding:5px;margin-bottom:10px;">
                            <div>
                                <strong>Namn</strong>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ValidationGroup="AddActivity" Display="Dynamic" ErrorMessage="* Får inte vara tomt" ControlToValidate="TxtActivityName"></asp:RequiredFieldValidator>                        
                                <br />
                                <asp:TextBox runat="server" Width="150" ID="TxtActivityName"></asp:TextBox>
                            </div>
                            <div>
                                <strong>Max antal</strong>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ValidationGroup="AddActivity" Display="Dynamic" ErrorMessage="* Får inte vara tomt" ControlToValidate="TxtActivityMax"></asp:RequiredFieldValidator>                        
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ValidationGroup="AddActivity" Display="Dynamic" ErrorMessage="* Värde måste vara numeriskt" ValidationExpression="\d+" ControlToValidate="TxtActivityMax"></asp:RegularExpressionValidator>
                                <br />
                                <asp:TextBox runat="server" Width="150" ID="TxtActivityMax"></asp:TextBox>
                            </div>
                            <asp:Button runat="server" ID="BtnAddActivity" ValidationGroup="AddActivity" Text="Skapa" OnClick="BtnAddActivityOnClick" />
                            <asp:Button runat="server" ID="BtnHideActivity" Text="Dölj" OnClick="BtnShowAddActivityOnClick" />
                        </div>
                    </asp:PlaceHolder>                                                         
                    <asp:ListBox runat="server" ID="ListSeminars" Width="250"></asp:ListBox><br />
                    <asp:LinkButton runat="server" ID="LbAddActivity" OnClick="BtnShowAddActivityOnClick" Text="Lägg till"></asp:LinkButton>
                    <asp:LinkButton runat="server" ID="LbDeleteActivity" OnClick="BtnDeleteActivityOnClick" Text="Ta bort"></asp:LinkButton>                                         
                </td>
            </tr>      
        </table>
        -->
    </ContentTemplate>
</asp:UpdatePanel>