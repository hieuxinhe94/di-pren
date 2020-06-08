<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SiteStatistics.aspx.cs" Inherits="DagensIndustri.Tools.Reports.SiteStatistics" %>

<asp:Content ContentPlaceHolderID="FullRegion" runat="server">
    <div class="epi-contentContainer epi-padding">
        <div class="epi-contentArea">
            <div class="EP-systemImage" style="background-image: url('/tools/reports/styles/images/apple-touch-icon-72x72.png');min-height: 6em;padding-left: 80px; background-position: 0 2px;background-repeat: no-repeat;">
                <h1>
                    Di Site Statistics          
                </h1>
                <p class="EP-systemInfo">
                    This report displays site statistics.
                </p>
            </div>
        </div>
        <div class="epi-formArea">
            <fieldset>
                <legend>
                    Sök (YYYY-MM-DD)
                </legend>
                <div class="epirowcontainer">
                    <asp:Label Text="From " runat="server" />
                    <asp:TextBox ID="FromDateTextBox" runat="server"/>
                    <asp:Label Text="To " runat="server" />
                    <asp:TextBox ID="ToDateTextBox" runat="server"/>
                    <asp:Button ID="SearchButton" OnClick="SearchButton_Click" Text="Sök" runat="server" />
                </div>
            </fieldset>
        </div>
        <div class="epi-formArea">
            <fieldset>
                <legend>
                    Unika besökare
                </legend>
                <div class="epirowcontainer">
                    <label class="episize140">
                        <strong>Dagensindustri.se:</strong>
                    </label>
                    <asp:Literal ID="DagensindustriVisitorLiteral" runat="server" />
                </div>
                <div class="epirowcontainer">
                    <label class="episize140">
                        <strong>Di Guld:</strong>
                    </label>
                    <asp:Literal ID="DiGoldVisitorLiteral" runat="server" />
                </div>
            </fieldset>
            <fieldset>
                <legend>
                    Affärskontakter 
                </legend>
                <div class="epirowcontainer">
                    <label class="episize140">
                        <strong>Antal besökare:</strong>
                    </label>
                    <asp:Literal ID="ContactsVisitorsLiteral" runat="server" />      
                </div>
                <div class="epirowcontainer">
                    <label class="episize140">
                        <strong>Antal unika besökare:</strong>
                    </label>
                    <asp:Literal ID="ContactsUniqueVisitorsLiteral" runat="server" />      
                </div>
                <div class="epirowcontainer">
                    <label class="episize140">
                        <strong>Antal sidvisningar:</strong>
                    </label>
                    <asp:Literal ID="ContactsPageviewsLiteral" runat="server" />      
                </div>
            </fieldset>
            <fieldset>
                <legend>
                    Mötesrum 
                </legend>
                <div class="epirowcontainer">
                    <label class="episize140">
                        <strong>Antal besökare:</strong>
                    </label>
                    <asp:Literal ID="MeetRoomVisitorsLiteral" runat="server" />
                </div>
                <div class="epirowcontainer">
                    <label class="episize140">
                        <strong>Antal unika besökare:</strong>
                    </label>
                    <asp:Literal ID="MeetRoomUniqueVisitorsLiteral" runat="server" />      
                </div>
                <div class="epirowcontainer">
                    <label class="episize140">
                        <strong>Antal sidvisningar:</strong>
                    </label>
                    <asp:Literal ID="MeetRoomPageviewsLiteral" runat="server" />      
                </div>
            </fieldset>
            <fieldset>
                <legend>
                    Vinklubben 
                </legend>
                <div class="epirowcontainer">
                    <label class="episize140">
                        <strong>Antal besökare:</strong>
                    </label>
                    <asp:Literal ID="WineClubVisitorsLiteral" runat="server" />
                </div>
                <div class="epirowcontainer">
                    <label class="episize140">
                        <strong>Antal unika besökare:</strong>
                    </label>
                    <asp:Literal ID="WineClubUniqueVisitorsLiteral" runat="server" />      
                </div>
                <div class="epirowcontainer">
                    <label class="episize140">
                        <strong>Antal sidvisningar:</strong>
                    </label>
                    <asp:Literal ID="WineClubPageviewsLiteral" runat="server" />      
                </div>
            </fieldset>
            <fieldset>
                <legend>
                    Läs online 
                </legend>
                <div class="epirowcontainer">
                    <label class="episize140">
                        <strong>Antal besökare:</strong>
                    </label>
                    <asp:Literal ID="ReadOnlineVisitorsLiteral" runat="server" />
                </div>
                <div class="epirowcontainer">
                    <label class="episize140">
                        <strong>Antal unika besökare:</strong>
                    </label>
                    <asp:Literal ID="ReadOnlineUniqueVisitorsLiteral" runat="server" />      
                </div>
                <div class="epirowcontainer">
                    <label class="episize140">
                        <strong>Antal sidvisningar:</strong>
                    </label>
                    <asp:Literal ID="ReadOnlinePageviewsLiteral" runat="server" />      
                </div>
            </fieldset>
            <fieldset>
                <legend>
                    Arkivet  
                </legend>
                <div class="epirowcontainer">
                    <label class="episize140">
                        <strong>Antal besökare:</strong>
                    </label>
                    <asp:Literal ID="ArchiveVisitorsLiteral" runat="server" />
                </div>
                <div class="epirowcontainer">
                    <label class="episize140">
                        <strong>Antal unika besökare:</strong>
                    </label>
                    <asp:Literal ID="ArchiveUniqueVisitorsLiteral" runat="server" />      
                </div>
                <div class="epirowcontainer">
                    <label class="episize140">
                        <strong>Antal sidvisningar:</strong>
                    </label>
                    <asp:Literal ID="ArchivePageviewsLiteral" runat="server" />      
                </div>
            </fieldset>
            <fieldset>
                <legend>
                    Di Gasell 
                </legend>
                <div class="epirowcontainer">
                    <label class="episize140">
                        <strong>Antal besökare:</strong>
                    </label>
                    <asp:Literal ID="GasellVisitorsLiteral" runat="server" />
                </div>
                <div class="epirowcontainer">
                    <label class="episize140">
                        <strong>Antal unika besökare:</strong>
                    </label>
                    <asp:Literal ID="GasellUniqueVisitorsLiteral" runat="server" />      
                </div>
                <div class="epirowcontainer">
                    <label class="episize140">
                        <strong>Antal sidvisningar:</strong>
                    </label>
                    <asp:Literal ID="GasellPageviewsLiteral" runat="server" />      
                </div>
            </fieldset>
            <fieldset>
                <legend>
                    Di Konferens 
                </legend>
                <div class="epirowcontainer">
                    <label class="episize140">
                        <strong>Antal besökare:</strong>
                    </label>
                    <asp:Literal ID="ConferenceVisitorsLiteral" runat="server" />
                </div>
                <div class="epirowcontainer">
                    <label class="episize140">
                        <strong>Antal unika besökare:</strong>
                    </label>
                    <asp:Literal ID="ConferenceUniqueVisitorsLiteral" runat="server" />      
                </div>
                <div class="epirowcontainer">
                    <label class="episize140">
                        <strong>Antal sidvisningar:</strong>
                    </label>
                    <asp:Literal ID="ConferencePageviewsLiteral" runat="server" />      
                </div>
            </fieldset>
            <fieldset>
                <legend>
                    Events Log
                </legend>
                <div class="epirowcontainer">
                    <label class="episize140">
                        <strong>Temporary address change:</strong>
                    </label>
                    <asp:Literal ID="TempAddressLiteral" runat="server" />
                </div>
                <div class="epirowcontainer">
                    <label class="episize140">
                        <strong>Holiday stop:</strong>
                    </label>
                    <asp:Literal ID="HolidayStopLiteral" runat="server" />      
                </div>
                <div class="epirowcontainer">
                    <label class="episize140">
                        <strong>Permanent address change:</strong>
                    </label>
                    <asp:Literal ID="PermanentAddressLiteral" runat="server" />      
                </div>
                <div class="epirowcontainer">
                    <label class="episize140">
                        <strong>Free DI + Summer 2011:</strong>
                    </label>
                    <asp:Literal ID="FreeDiLiteral" runat="server" />      
                </div>
                <div class="epirowcontainer">
                    <label class="episize140">
                        <strong>Antal skapade URL (Affärskalender):</strong>
                    </label>
                    <asp:Literal ID="CreatedURLLiteral" runat="server" />      
                </div>
                <div class="epirowcontainer">
                    <label class="episize140">
                        <strong>Antal sök (Affärskontakter):</strong>
                    </label>
                    <asp:Literal ID="CompanySearchesLiteral" runat="server" />      
                </div>
                <div class="epirowcontainer">
                    <label class="episize140">
                        <strong>Intresseanmälan (Vinklubb):</strong>
                    </label>
                    <asp:Literal ID="WineClubMailLiteral" runat="server" />      
                <div class="epirowcontainer">
                    <label class="episize140">
                        <strong>Antal skickade mail (Mötesrum):</strong>
                    </label>
                    <asp:Literal ID="UnitedSpacesLiteral" runat="server" />      
                </div>
                </div>
            </fieldset>
            <fieldset>
                <legend>
                    Båstad
                </legend>
                <div class="epirowcontainer">
                    <label class="episize140">
                        <strong>Frukost med Christer Gardell:</strong>
                    </label>
                    <asp:Literal ID="BreakfastOneLiteral" runat="server" />
                </div>
                <div class="epirowcontainer">
                    <label class="episize140">
                        <strong>Frukost med Øystein Løseth:</strong>
                    </label>
                    <asp:Literal ID="BreakfastTwoLiteral" runat="server" />      
                </div>
                <div class="epirowcontainer">
                    <label class="episize140">
                        <strong>Frukost med Stefan Edberg:</strong>
                    </label>
                    <asp:Literal ID="BreakfastThreeLiteral" runat="server" />      
                </div>
                <div class="epirowcontainer">
                    <label class="episize140">
                        <strong>Tennisclinic:</strong>
                    </label>
                    <asp:Literal ID="TennisClinicLiteral" runat="server" />      
                </div>
            </fieldset>
        </div>
    </div>
</asp:Content>