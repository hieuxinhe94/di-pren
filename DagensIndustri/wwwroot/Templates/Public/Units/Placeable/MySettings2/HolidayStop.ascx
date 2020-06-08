<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HolidayStop.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.MySettings2.HolidayStop" %>
<%@ Register TagPrefix="DI" TagName="Input" Src="~/Tools/Classes/WebControls/InputWithValidation.ascx" %>

<div class="section" id="subscription">
			
    <!--Holiday stop-->
    <asp:PlaceHolder ID="HolidayStopPlaceHolder" runat="server">
	    <div class="row">
		    <h4><EPiServer:Translate Text="/mysettings/subscription/holidaystop/title" runat="server" /></h4>
            <asp:HyperLink ID="HolidayStopEditHyperLink" NavigateUrl="#" CssClass="edit" runat="server"><EPiServer:Translate Text="/common/change" runat="server" /></asp:HyperLink>
            <p class="description"><EPiServer:Translate Text="/mysettings/subscription/holidaystop/description" runat="server" /></p>
		    <p class="value"><%= NoOfHolidayStops > 0 ? Translate("/common/active") : Translate("/common/notactive") %></p>

            <!-- Dates -->
            <div ID="HolidayStopDiv" class="form-edit" runat="server">
                <asp:Repeater ID="HolidayStopRepeater" runat="server">   <%--OnItemDataBound="HolidayStopRepeater_ItemDataBound"--%>
                    <HeaderTemplate>
                        <div class="upcoming">
				            <h5><EPiServer:Translate Text="/mysettings/subscription/holidaystop/plannedholidaystops" runat="server" /></h5>
                            <table>
					            <thead>
						            <tr>
                                        <th><EPiServer:Translate Text="/common/from" runat="server" /></th>
							            <th><EPiServer:Translate Text="/common/to" runat="server" /></th>
							            <th></th>
						            </tr>
					            </thead>
					            <tbody>
                    </HeaderTemplate>

                    <ItemTemplate>
                        <tr>
						    <td><%# ((DateTime)DataBinder.Eval(Container.DataItem, "SLEEPSTARTDATE")).ToString("yyyy-MM-dd")%></td>
    	                    <td><%# ((DateTime)DataBinder.Eval(Container.DataItem, "SLEEPENDDATE")).ToString("yyyy-MM-dd")%></td>
                            <td>
                                <asp:LinkButton ID="DeleteHolidayStopLinkButton" Text="<%$ Resources: EPiServer, common.delete %>" OnClick="DeleteHolidayStop_Click" CommandArgument='<%# ((DateTime)DataBinder.Eval(Container.DataItem, "SLEEPSTARTDATE")).ToString("yyyy-MM-dd") + "|" + ((DateTime)DataBinder.Eval(Container.DataItem, "SLEEPENDDATE")).ToString("yyyy-MM-dd")%>' runat="server" />
                            </td>    	                    
					    </tr>
                    </ItemTemplate>

                    <FooterTemplate>
                                </tbody>
				            </table>
                        </div>
                    </FooterTemplate>
                </asp:Repeater>
                  
                <div class="row">
				    <div class="col">
					    <div class="medium">
                            <DI:Input ID="HolidayStopFromInput" CssClass="text date" Required="true" Name="date-from" TypeOfInput="Date" Title="<%$ Resources: EPiServer, mysettings.subscription.fromdateformat %>" DisplayMessage="<%$ Resources: EPiServer, mysettings.subscription.holidaystop.messagefillfromdate %>" runat="server" />
					    </div>
				    </div>

				    <div class="col">
					    <div class="medium">
                            <DI:Input ID="HolidayStopToInput" CssClass="text date" Required="true" Name="date-to" TypeOfInput="Date" Title="<%$ Resources: EPiServer, mysettings.subscription.todateformat %>" DisplayMessage="<%$ Resources: EPiServer, mysettings.subscription.holidaystop.messagefilltodate %>" runat="server" />
					    </div>
				    </div>
			    </div>
                            
			    <div class="button-wrapper">
                    <asp:Button ID="SaveHolidayStopButton" CssClass="btn" Text="<%$ Resources: EPiServer, common.save %>" OnClick="SaveHolidayStop_Click" runat="server" />
			    </div>
            
		    </div>
            <!-- // Dates -->
	    </div>
    </asp:PlaceHolder>
    <!--/Holiday stop-->

    <!--Temporary address-->
    <asp:PlaceHolder ID="TemporaryAddressPlaceHolder" runat="server">
	    <div class="row">
		    <h4><EPiServer:Translate Text="/mysettings/subscription/temporaryaddress/title" runat="server" /></h4>
            <asp:HyperLink ID="TemporaryAddressEditHyperLink" NavigateUrl="#" CssClass="edit" runat="server"><EPiServer:Translate Text="/common/change" runat="server" /></asp:HyperLink>
		    <p class="description"><EPiServer:Translate Text="/mysettings/subscription/temporaryaddress/description" runat="server" /></p>
		    <p class="value"><%= NoOfPendingAddresses > 0 ? Translate("/common/active") : Translate("/common/notactive")%></p>

            <!-- Previous temporary addresses -->
            <div ID="TemporaryAddressDiv" class="form-edit" runat="server">
                <div class="info">
                    <EPiServer:Translate Text="/mysettings/subscription/temporaryaddress/infoanothercountry" runat="server" />
			    </div>

                <div class="row">
				    <div class="col">
					    <div class="medium">
                            <asp:Label ID="PreviousTempAddressLabel" AssociatedControlID="PrevTempAddrDropDownList" Text="<%$ Resources: EPiServer, mysettings.subscription.temporaryaddress.previousaddresses %>" runat="server" />
                            <asp:DropDownList ID="PrevTempAddrDropDownList" AutoPostBack="true" DataTextField="STREET1" DataValueField="ADDRNOSTART" OnSelectedIndexChanged="PrevTempAddrDropDownList_SelectedIndexChanged" runat="server" />                       
					    </div>
				    </div>
			    </div>
        
                <div class="divider"><hr /></div>

                <asp:Repeater ID="PendingAddressRepeater" runat="server">  <%--OnItemDataBound="PendingAddressRepeater_ItemDataBound"--%>
    	            <HeaderTemplate>
                        <div class="upcoming">
				            <h5><EPiServer:Translate runat="server" Text="/mysettings/subscription/temporaryaddress/tempaddresses" /></h5>
                            <table>
					            <thead>
						            <tr>
                                        <th><EPiServer:Translate runat="server" Text="/mysettings/subscription/temporaryaddress/address" /></th>
							            <th><EPiServer:Translate runat="server" Text="/common/from" /></th>
							            <th><EPiServer:Translate runat="server" Text="/common/to" /></th>
                                        <th></th>
						            </tr>
					            </thead>
					            <tbody>
    	            </HeaderTemplate>
    	            <ItemTemplate>
                        <tr>
						    <td><%# DataBinder.Eval(Container.DataItem, "STREET1")%></td>
    	                    <td><%# ((DateTime)DataBinder.Eval(Container.DataItem, "STARTDATE")).ToString("yyyy-MM-dd")%></td>
    	                    <td><%# ((DateTime)DataBinder.Eval(Container.DataItem, "ENDDATE")).ToString("yyyy-MM-dd")%></td>
                            <td>
                                <asp:LinkButton ID="DeleteTemporaryAddressLinkButton" Text="<%$ Resources: EPiServer, common.delete %>" OnClick="DeleteTemporaryAddress_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ADDRNO") + "|" + DataBinder.Eval(Container.DataItem, "STARTDATE")%>' runat="server" />
                            </td>
				        </tr>
    	            </ItemTemplate>
    	            <FooterTemplate>
    	                        </tbody>
				            </table>
                        </div>
    	            </FooterTemplate>
    	        </asp:Repeater>            
							
			    <div class="row">

				    <div class="col">
                        <DI:Input ID="TempAddressCoInput" CssClass="text" Required="false" Name="temp-co" TypeOfInput="Text" MaxValue="21" AutoComplete="true" Title="<%$ Resources: EPiServer, common.address.careof %>" runat="server" />
				    </div>
			    </div>

                <div class="row">
				    <div class="col">
					    <DI:Input ID="TempAddressStreetInput" CssClass="text" Required="true" Name="temp-street" TypeOfInput="Text" MaxValue="27" AutoComplete="true" Title="<%$ Resources: EPiServer, common.address.streetaddress %>" DisplayMessage="<%$ Resources: EPiServer, common.validation.streetaddressrequired %>" runat="server" />
				    </div>
				    <div class="col">
					    <div class="small">
						    <DI:Input ID="TempAddressHouseNoInput" CssClass="text" Name="temp-number" TypeOfInput="Text" MaxValue="11" AutoComplete="true" Title="<%$ Resources: EPiServer, common.address.number %>" DisplayMessage="<%$ Resources: EPiServer, common.validation.streetnumberrequired %>" runat="server" />
					    </div>
					    <div class="small">
						    <DI:Input ID="TempAddressStairCaseInput" CssClass="text" Required="false" Name="temp-staircase" TypeOfInput="Text" MaxValue="10" AutoComplete="true" Title="<%$ Resources: EPiServer, common.address.staircase %>" runat="server" />
					    </div>							
				    </div>
			    </div>	
							
			    <div class="row">
				    <div class="col">
					    <div class="small">
						    <DI:Input ID="TempAddressStairsInput" CssClass="text" Required="false" Name="temp-stairs" TypeOfInput="Text" MaxValue="3" AutoComplete="true" Title="<%$ Resources: EPiServer, common.address.stairs %>" runat="server" />
					    </div>

					    <div class="small">
                            <DI:Input ID="TempAddressApartmentInput" CssClass="text" Required="false" Name="temp-appartment" TypeOfInput="Text" MaxValue="5" AutoComplete="true" Title="<%$ Resources: EPiServer, common.address.apartmentnumber %>" runat="server" />
					    </div>							
				    </div>
				    <div class="col">
					    <div class="small">						
                            <DI:Input ID="TempAddressZipInput" CssClass="text" Required="true" Name="temp-zip" TypeOfInput="ZipCode" MaxValue="5" AutoComplete="true" Title="<%$ Resources: EPiServer, common.address.zip %>" DisplayMessage="<%$ Resources: EPiServer, common.validation.zipcoderequired %>" runat="server" />
					    </div>				
				    </div>												
			    </div>

			    <div class="row">
				    <div class="col">
					    <div class="medium">
						    <DI:Input ID="TempAddressFromInput" CssClass="text date" Required="true" Name="date-temp-from" TypeOfInput="Date" Title="<%$ Resources: EPiServer, mysettings.subscription.fromdateformat %>" DisplayMessage="<%$ Resources: EPiServer, mysettings.subscription.address.messagefillfromdate %>" runat="server" />
					    </div>								
				    </div>
				    <div class="col">
					    <div class="medium">						
                            <DI:Input ID="TempAddressToInput" CssClass="text date" Required="true" Name="date-temp-to" TypeOfInput="Date" Title="<%$ Resources: EPiServer, mysettings.subscription.todateformat %>" DisplayMessage="<%$ Resources: EPiServer, mysettings.subscription.address.messagefilltodate %>" runat="server" />
					    </div>								
				    </div>								
			    </div>

			    <div class="button-wrapper">
                    <asp:Button ID="SaveTemporaryAddressButton" CssClass="btn" Text="<%$ Resources: EPiServer, common.save %>" OnClick="SaveTemporaryAddress_Click" runat="server" />
			    </div>
		    </div>
            <!-- //Previous temporary addresses -->
	    </div>
    </asp:PlaceHolder>
    <!--/Temporary address-->
			
    <!--Permanent address-->
	<div class="row">
		<h4><EPiServer:Translate Text="/mysettings/subscription/permanentaddress/title" runat="server" /></h4>
        <asp:HyperLink ID="PermanentAddressEditHyperLink" NavigateUrl="#" CssClass="edit" runat="server"><EPiServer:Translate Text="/common/change" runat="server" /></asp:HyperLink>
		<p class="description"><EPiServer:Translate Text="/mysettings/subscription/permanentaddress/description" runat="server" /></p>

        <!-- Address -->
        <div ID="PermanentAddressDiv" class="form-edit" runat="server">
            <div class="info">
                <EPiServer:Translate Text="/mysettings/subscription/permanentaddress/infoanothercountry" runat="server" />
			</div>

            <div class="row">
				<div class="col">
                    <div class="small">	
                        Företag
                    </div>
                    <div class="small">	
                        apa <asp:Literal ID="LiteralNowComp" runat="server"></asp:Literal>
                    </div>
				</div>
			</div>

            <div class="row">
				<div class="col">
					<DI:Input ID="PermanentAddressCoInput" CssClass="text" Required="false" Name="perm-co" TypeOfInput="Text" MaxValue="21" AutoComplete="true" Title="<%$ Resources: EPiServer, common.address.careof %>" runat="server" />
				</div>
			</div>
							
			<div class="row">
				<div class="col">
					<DI:Input ID="PermanentAddressStreetInput" CssClass="text" Required="true" Name="perm-street" TypeOfInput="Text" MaxValue="27" AutoComplete="true" Title="<%$ Resources: EPiServer, common.address.streetaddress %>" DisplayMessage="<%$ Resources: EPiServer, common.validation.streetaddressrequired %>" runat="server" />
				</div>
				<div class="col">
					<div class="small">					
                        <DI:Input ID="PermanentAddressHouseNoInput" CssClass="text" Name="perm-number" TypeOfInput="Text" MaxValue="11" AutoComplete="true" Title="<%$ Resources: EPiServer, common.address.number %>" DisplayMessage="<%$ Resources: EPiServer, common.validation.streetnumberrequired %>" runat="server" />					
					</div>
					<div class="small">
						<DI:Input ID="PermanentAddressStairCaseInput" CssClass="text" Required="false" Name="perm-staircase" TypeOfInput="Text" MaxValue="10" AutoComplete="true" Title="<%$ Resources: EPiServer, common.address.staircase %>" runat="server" />
					</div>							
				</div>						
			</div>	
							
			<div class="row">
				<div class="col">
					<div class="small">	
                        <DI:Input ID="PermanentAddressStairsInput" CssClass="text" Required="false" Name="perm-stairs" TypeOfInput="Text" MaxValue="3" AutoComplete="true" Title="<%$ Resources: EPiServer, common.address.stairs %>" runat="server" />
					</div>
					<div class="small">
						<DI:Input ID="PermanentAddressApartmentInput" CssClass="text" Required="false" Name="perm-appartment" TypeOfInput="Text" MaxValue="5" AutoComplete="true" Title="<%$ Resources: EPiServer, common.address.apartmentnumber %>" runat="server" />
					</div>							
				</div>

				<div class="col">
					<div class="small">
						<DI:Input ID="PermanentAddressZipInput" CssClass="text" Required="true" Name="perm-zip" TypeOfInput="ZipCode" MaxValue="5" AutoComplete="true" Title="<%$ Resources: EPiServer, common.address.zip %>" DisplayMessage="<%$ Resources: EPiServer, common.validation.zipcoderequired %>" runat="server" />
					</div>
					<div class="medium">
						<DI:Input ID="PermanentAddressFromInput" CssClass="text date" Required="true" Name="date-perm-from" TypeOfInput="Date" Title="<%$ Resources: EPiServer, mysettings.subscription.fromdateformat %>" DisplayMessage="<%$ Resources: EPiServer, mysettings.subscription.address.messagefillfromdate %>" runat="server" />
					</div>
				</div>
			</div>
                        
			<div class="button-wrapper">
                <asp:Button ID="SavePermanentAddressButton" CssClass="btn" Text="<%$ Resources: EPiServer, common.save %>" OnClick="SavePermanentAddress_Click" runat="server" />
			</div>
		</div>
        <!-- // Address -->
	</div>
    <!--/Permanent address-->

    <!--end DiY subs-->
    <%--<asp:PlaceHolder ID="PlaceHolderEndDiYSubs" runat="server">
	    <div class="row">
		    <h4>Avsluta DiY prenumeration</h4>
            <asp:HyperLink ID="HyperLink1" NavigateUrl="#" CssClass="edit" runat="server"><EPiServer:Translate ID="Translate2" Text="/common/change" runat="server" /></asp:HyperLink>
		    <p class="description">Här kan du avsluta din DiY prenumeration</p>

            <!-- Address -->
            <div ID="Div1" class="form-edit" runat="server">
                <div class="info">
                    Klicka på knappen nedan för att avsluta din DiY prenumeration.
			    </div>

                <div class="button-wrapper">
                    <asp:Button ID="ButtonEndDiYSubs" CssClass="btn" Text="Avsluta prenumeration" OnClick="ButtonEndDiYSubs_Click" runat="server" />
			    </div>
            </div>
        </div>
    </asp:PlaceHolder>--%>
    <!--/end DiY subs-->

    <!--Support-->
	<div class="row">
		<h4><EPiServer:Translate Text="/mysettings/subscription/support/title" runat="server" /></h4>
		<a href='<%= string.Format("mailto:{0}", CurrentPage["SubscriptionSupportEmail"]) %>' class="link"><%= CurrentPage["SubscriptionSupportEmail"]%></a>
		<p class="description"><EPiServer:Translate Text="/mysettings/subscription/support/description" runat="server" /></p>
	</div>
	<!--/Support-->

</div>