<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CampaignThankYou.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.DiCampaign.CampaignThankYou" %>

<style type="text/css">
    .space { padding-right:20px; padding-bottom:5px; }
</style>


<div id="content">
    <h1 class="big"><asp:Literal ID="LiteralThankYouHeader" runat="server">Tack för din beställning!</asp:Literal></h1>
    <p>
        Du kommer inom kort få ett mail med information om hur du går tillväga.<br />
        Trevlig läsning!
    </p>
 
        <asp:PlaceHolder ID="PlaceHolderNewSubscriptions" runat="server" Visible="false">
            <table>
                <thead>
                    <tr>
                        <td class="space"><b>Produkt</b></td>
                        <td><b>Prenumerationsnr</b></td>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="rptSubscriptions" runat="server" DataSource='<%#NewSubscriptions %>'>
                        <ItemTemplate>
                            <tr>
                                <td class="space"><%#Eval("ProductName")%></td>
                                <td><%#Eval("SubsNo")%></td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>
            <div>
                <br />Vid frågor går det bra att kontakta vår kundtjänst via mejl <a href="mailto:pren@di.se">pren@di.se</a> eller tel 08-573 651 00.
            </div>
        </asp:PlaceHolder>

        <asp:PlaceHolder ID="PlaceHolderCampaign1" runat="server" Visible="false">
            <div class="form-box">
			<!-- Offer -->
			<div class="section" id="form-street">
					
				<div class="row row-offer">					
                    <%=(string)CurrentPage["ThankYouCampaign1Html"]%>
				</div>
					
				<div class="divider"><hr /></div>					
					
				<div class="button-wrapper">					
					<asp:Button ID="ButtonThankYouCampaign1" runat="server" Text="Beställ" OnClick="ButtonThankYouCampaign1_Click" />
				</div>					
			</div>            
			<!-- // Offer -->
            </div>
        </asp:PlaceHolder>

        <asp:PlaceHolder ID="PlaceHolderCampaign2" runat="server" Visible="false">
            <div class="form-box">
            <!-- Offer -->
			<div class="section" id="form-street-2">
					
				<div class="row row-offer">					
                    <%=(string)CurrentPage["ThankYouCampaign2Html"]%>
				</div>
					
				<div class="divider"><hr /></div>					
					
				<div class="button-wrapper">					
					<asp:Button ID="ButtonThankYouCampaign2" runat="server" Text="Beställ" OnClick="ButtonThankYouCampaign2_Click" />
				</div>					
			</div>
			<!-- // Offer -->
            </div>
        </asp:PlaceHolder>

        <asp:PlaceHolder ID="PlaceHolderComplete" runat="server" Visible="false">

		    <!-- Main header and form -->
            <asp:PlaceHolder ID="PlaceHolderCompleteAutogiro" runat="server">
  		        <p class="intro">Du har valt att betala via autogiro vilket innebär att du måste lämna ditt medgivande.</p>

			    <h4 class="large">Gör så här för att slutföra beställningen:</h4>
			    <ol>
				    <li><a href="http://dagensindustri.se/Documents/Medgivande_Autogiro_Privat.pdf" target="_blank">Klicka här för att skriva ut autogiromedgivande</a></li>
				    <li>Fyll i autogiromedgivandet och skriv under</li>
				    <li>Posta autogiromedgivandet (adress är förmärkt och porto betalt)</li>
			    </ol>
		    </asp:PlaceHolder>

            <asp:PlaceHolder ID="PlaceHolderCompleteNotAutogiro" runat="server">
                <%--<p class="intro"><%=Translate("/campaigns/thankyou/completenotautogiro")%></p>--%>
            </asp:PlaceHolder>
		    <!-- // Main header and form -->

        </asp:PlaceHolder>
  	
</div>
