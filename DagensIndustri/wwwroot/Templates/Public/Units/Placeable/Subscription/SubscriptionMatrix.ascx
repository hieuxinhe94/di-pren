<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SubscriptionMatrix.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.Subscription.SubscriptionMatrix" %>
<%@ Register TagPrefix="di" TagName="MatrixSubscriptionType" Src="~/Templates/Public/Units/Placeable/Subscription/MatrixSubscriptionType.ascx" %>

<!-- Matrix goes here -->
<table>
	<thead>
		<tr>
			<th class="col_1">
				<div class="wrapper">
					<h1><EPiServer:Translate Text="/subscription/matrix/choosesubscription" runat="server" /></h1>
				</div>
			</th>

            <!--<th class="col_2"><di:MatrixSubscriptionType ID="DiPremiumSubscriptionType" SubscriptionType="DiPremium" CampaignNo1='<%# ConvertCampaignNo(CurrentPage["CampNoDiDiPremium"].ToString()) %>' CampaignNo2='<%# ConvertCampaignNo(CurrentPage["CampNoIpadDiPremium"].ToString()) %>' Name="<%$ Resources: EPiServer, subscription.matrix.typeofpaper.dipremium %>" PriceImageUrl='<%# (string)CurrentPage["PriceImageDiPremium"] %>' ShowDiGold="true" runat="server" /></th>-->
            <th class="col_3"><di:MatrixSubscriptionType ID="DagensIndustriSubscriptionType" SubscriptionType="Di" CampaignNo1='<%# ConvertCampaignNo(CurrentPage["CampNoDi"].ToString()) %>' Name="<%$ Resources: EPiServer, subscription.matrix.typeofpaper.di %>" PriceImageUrl='<%# (string)CurrentPage["PriceImageDi"]%>' ShowDiGold="true" runat="server" /></th>
            <th class="col_4"><di:MatrixSubscriptionType ID="DiDirectDebitSubscriptionType" SubscriptionType="DiDirectDebit" CampaignNo1='<%# ConvertCampaignNo(CurrentPage["CampNoDiDirectDebit"].ToString()) %>' Name="<%$ Resources: EPiServer, subscription.matrix.typeofpaper.directdebit %>" PriceImageUrl='<%# (string)CurrentPage["PriceImageDiDirectDebit"]%>' ShowDiGold="true" runat="server" /></th>
            <th class="col_5"><di:MatrixSubscriptionType ID="DiWeekendSubscriptionType" SubscriptionType="DiWeekend" CampaignNo1='<%# ConvertCampaignNo(CurrentPage["CampNoDiWeekend"].ToString()) %>' Name="<%$ Resources: EPiServer, subscription.matrix.typeofpaper.diweekend %>" PriceImageUrl='<%# (string)CurrentPage["PriceImageDiWeekend"] %>' ShowDiGold="false" runat="server" /></th>
            <th class="col_6"><di:MatrixSubscriptionType ID="DiPlusSubscriptionType" SubscriptionType="DiPlus" CampaignNo1='<%# ConvertCampaignNo(CurrentPage["CampNoDiPlus"].ToString()) %>' Name="<%$ Resources: EPiServer, subscription.matrix.typeofpaper.diplus %>" PriceImageUrl='<%# (string)CurrentPage["PriceImageDiPlus"] %>' ShowDiGold="true" runat="server" /></th>
		</tr>
	</thead>
	<tfoot>
		<tr>
			<td class="col_1"></td>
            <!--<td class="col_2"><di:MatrixSubscriptionType ID="DiPremiumFooterSubscriptionType" SubscriptionType="DiPremium" CampaignNo1='<%# ConvertCampaignNo(CurrentPage["CampNoDiDiPremium"].ToString()) %>' CampaignNo2='<%# ConvertCampaignNo(CurrentPage["CampNoIpadDiPremium"].ToString()) %>' Name="<%$ Resources: EPiServer, subscription.matrix.typeofpaper.dipremium %>" PriceImageUrl='<%# (string)CurrentPage["PriceImageDiPremium"] %>' ShowDiGold="true" runat="server" /></td>-->
            <td class="col_3"><di:MatrixSubscriptionType ID="DagensIndustriFooterSubscriptionType" SubscriptionType="Di" CampaignNo1='<%# ConvertCampaignNo(CurrentPage["CampNoDi"].ToString()) %>' Name="<%$ Resources: EPiServer, subscription.matrix.typeofpaper.di %>" PriceImageUrl='<%# (string)CurrentPage["PriceImageDi"] %>' ShowDiGold="true" runat="server" /></td>
            <td class="col_4"><di:MatrixSubscriptionType ID="DiDirectDebitFooterSubscriptionType" SubscriptionType="DiDirectDebit" CampaignNo1='<%# ConvertCampaignNo(CurrentPage["CampNoDiDirectDebit"].ToString()) %>' Name="<%$ Resources: EPiServer, subscription.matrix.typeofpaper.directdebit %>" PriceImageUrl='<%# (string)CurrentPage["PriceImageDiDirectDebit"] %>' ShowDiGold="true" runat="server" /></td>
            <td class="col_5"><di:MatrixSubscriptionType ID="DiWeekendFooterSubscriptionType" SubscriptionType="DiWeekend" CampaignNo1='<%# ConvertCampaignNo(CurrentPage["CampNoDiWeekend"].ToString()) %>' Name="<%$ Resources: EPiServer, subscription.matrix.typeofpaper.diweekend %>" PriceImageUrl='<%# (string)CurrentPage["PriceImageDiWeekend"] %>' ShowDiGold="false" runat="server" /></td>
            <td class="col_6"><di:MatrixSubscriptionType ID="DiPlusFooterSubscriptionType" SubscriptionType="DiPlus" CampaignNo1='<%# ConvertCampaignNo(CurrentPage["CampNoDiPlus"].ToString()) %>' Name="<%$ Resources: EPiServer, subscription.matrix.typeofpaper.diplus %>" PriceImageUrl='<%# (string)CurrentPage["PriceImageDiPlus"] %>' ShowDiGold="true" runat="server" /></td>
		</tr>
	</tfoot>
	<tbody>
             <tr>
                <td class="col_1">Dagens industri måndag-lördag</td>
                <!--<td class="col_2"><span class="yes">Ja</span></td>-->
                <td class="col_3"><span class="yes">Ja</span></td>
                <td class="col_4"><span class="yes">Ja</span></td>
                <td class="col_5"><span class="no">Nej</span></td>
                <td class="col_6"><span class="no">Nej</span></td>
            </tr>
            <tr>
                <td class="col_1">Dagens industri fredag-lördag</td>
                <!--<td class="col_2"><span class="yes">Ja</span></td>-->
                <td class="col_3"><span class="no">Nej</span></td>
                <td class="col_4"><span class="no">Nej</span></td>
                <td class="col_5"><span class="yes">Ja</span></td>
                <td class="col_6"><span class="no">Nej</span></td>
            </tr>
            <tr>
                <td class="col_1">Di Weekend varje fredag</td>
                <!--<td class="col_2"><span class="yes">Ja</span></td>-->
                <td class="col_3"><span class="yes">Ja</span></td>
                <td class="col_4"><span class="yes">Ja</span></td>
                <td class="col_5"><span class="yes">Ja</span></td>
                <td class="col_6"><span class="no">Nej</span></td>
            </tr>
            <tr>
                <td class="col_1">Di Dimension 10 gånger per år</td>
                <!--<td class="col_2"><span class="yes">Ja</span></td>-->
                <td class="col_3"><span class="yes">Ja</span></td>
                <td class="col_4"><span class="yes">Ja</span></td>
                <td class="col_5"><span class="no">Nej</span></td>
                <td class="col_6"><span class="no">Nej</span></td>
            </tr>
            <tr>
                <td class="col_1">Di online måndag-lördag <i>- tryckta tidningen som PDF</i></td>
                <!--<td class="col_2"><span class="yes">Ja</span></td>-->
                <td class="col_3"><span class="yes">Ja</span></td>
                <td class="col_4"><span class="yes">Ja</span></td>
                <td class="col_5"><span class="no">Nej</span></td>
                <td class="col_6"><span class="yes">Ja</span></td>
            </tr>
            
            <!--
            <tr>
                <td class="col_1">Di online fredag-lördag <i>- tryckta tidningen som PDF</i></td>
                <--<td class="col_2"><span class="yes">Ja</span></td>->
                <td class="col_3"><span class="no">Nej</span></td>
                <td class="col_4"><span class="no">Nej</span></td>
                <td class="col_5"><span class="yes">Ja</span></td>
                <td class="col_6"><span class="no">Nej</span></td>
            </tr>
            -->
            
            <tr>
                <td class="col_1">Dagens industris arkiv</td>
                <!--<td class="col_2"><span class="yes">Ja</span></td>-->
                <td class="col_3"><span class="yes">Ja</span></td>
                <td class="col_4"><span class="yes">Ja</span></td>
                <td class="col_5"><span class="no">Nej</span></td>
                <td class="col_6"><span class="yes">Ja</span></td>
            </tr>
            <tr>
                <td class="col_1">Dagens industri och Di Weekend för läsplatta måndag-lördag</td>
                <!--<td class="col_2"><span class="yes">Ja</span></td>-->
                <td class="col_3"><span class="yes">Ja</span></td>
                <td class="col_4"><span class="yes">Ja</span></td>
                <td class="col_5"><span class="no">Nej</span></td>
                <td class="col_6"><span class="yes">Ja</span></td>
            </tr>
            
            
            <tr>
                <td class="col_1">Dagens industri och Di Weekend för läsplatta fredag-lördag</td>
                <!--<td class="col_2"><span class="yes">Ja</span></td>-->
                <td class="col_3"><span class="no">Nej</span></td>
                <td class="col_4"><span class="no">Nej</span></td>
                <td class="col_5"><span class="yes">Ja</span></td>
                <td class="col_6"><span class="yes">Ja</span></td>
            </tr> 
            

            <!--
            <tr>
                <td class="col_1">Liveuppdateringar i läsplattan</td>
                <--<td class="col_2"><span class="yes">Ja</span></td>->
                <td class="col_3"><span class="no">Nej</span></td>
                <td class="col_4"><span class="no">Nej</span></td>
                <td class="col_5"><span class="no">Nej</span></td>
                <td class="col_6"><span class="yes">ja</span></td>
            </tr>
            -->                  
            <tr>
                <td colspan="6" class="col_all"><h3><EPiServer:Translate Text="/subscription/matrix/includingservices" runat="server" /></h3></td>
            </tr>
            <tr>
                <td class="col_1">Läs tidningen online kvällen innan</td>
                <!--<td class="col_2"><span class="yes">Ja</span></td>-->
                <td class="col_3"><span class="yes">Ja</span></td>
                <td class="col_4"><span class="yes">Ja</span></td>
                <td class="col_5"><span class="no">Nej</span></td>
                <td class="col_6"><span class="yes">Ja</span></td>
            </tr>
            <tr>
                <td class="col_1">Affärskontakter</td>
                <!--<td class="col_2"><span class="yes">Ja</span></td>-->
                <td class="col_3"><span class="yes">Ja</span></td>
                <td class="col_4"><span class="yes">Ja</span></td>
                <td class="col_5"><span class="no">Nej</span></td>
                <td class="col_6"><span class="yes">Ja</span></td>
            </tr>                  
            <tr>
 
                <td class="col_1">Affärskalender</td>
                <!--<td class="col_2"><span class="yes">Ja</span></td>-->
                <td class="col_3"><span class="yes">Ja</span></td>
                <td class="col_4"><span class="yes">Ja</span></td>
                <td class="col_5"><span class="no">Nej</span></td>
                <td class="col_6"><span class="yes">Ja</span></td>
            </tr>
            <tr>
                <td class="col_1">Mötesrum i Stockholm</td>
                <!--<td class="col_2"><span class="yes">Ja</span></td>-->
                <td class="col_3"><span class="yes">Ja</span></td>
                <td class="col_4"><span class="yes">Ja</span></td>
                <td class="col_5"><span class="no">Nej</span></td>
                <td class="col_6"><span class="yes">Ja</span></td>
            </tr>
            <tr>
                <td class="col_1">Personal Shopping Näringsliv</td>
                <!--<td class="col_2"><span class="yes">Ja</span></td>-->
                <td class="col_3"><span class="yes">Ja</span></td>
                <td class="col_4"><span class="yes">Ja</span></td>
                <td class="col_5"><span class="no">Nej</span></td>
                <td class="col_6"><span class="yes">Ja</span></td>
            </tr>
            <tr>
                <td class="col_1">Vintips via sms</td>
                <!--<td class="col_2"><span class="yes">Ja</span></td>-->
                <td class="col_3"><span class="yes">Ja</span></td>
                <td class="col_4"><span class="yes">Ja</span></td>
                <td class="col_5"><span class="no">Nej</span></td>
                <td class="col_6"><span class="yes">Ja</span></td>
            </tr>
            <tr>
                <td class="col_1">Exklusiva erbjudanden<br /><i>(visningar, vinprovningar, kurser, seminarier, föredrag mm)</i></td>
                <!--<td class="col_2"><span class="yes">Ja</span></td>-->
                <td class="col_3"><span class="yes">Ja</span></td>
                <td class="col_4"><span class="yes">Ja</span></td>
                <td class="col_5"><span class="no">Nej</span></td>
                <td class="col_6"><span class="yes">Ja</span></td>
            </tr>
</tbody>

</table>
<!-- // Matrix goes here -->
		
<!-- Agreements etc. -->
<div class="text">
    
	<div class="left" style="width:290px;margin-right:30px;">
		<h4><EPiServer:Translate Text="/subscription/matrix/digoldterms" runat="server" /></h4>
		<EPiServer:Property PropertyName="DiGoldTerms" runat="server" />
	</div>
    <div class="middle" style="float:left;width:290px;">
        <h4><EPiServer:Property PropertyName="FooterMidTitle" runat="server" /></h4>
		<EPiServer:Property PropertyName="FooterMidText" runat="server" />
    </div>
	<div class="right" style="width:290px;">

		<h4><EPiServer:Property PropertyName="FooterRightTitle" runat="server" /></h4>
        <EPiServer:Property PropertyName="FooterRightText" runat="server" />
	</div>
</div>
<!-- // Agreements etc. -->
