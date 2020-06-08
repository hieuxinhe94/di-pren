<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="Campaign.aspx.cs"
    Inherits="PrenDiSe.Templates.Public.Pages.CampaignTemplates.CampaignPaperPrototyp.Campaign" MasterPageFile="~/Templates/Public/MasterPages/C-Campaign.Master" %>

<asp:Content ContentPlaceHolderID="MainBodyRegion" runat="server">
    
    <div class="header-wrapper">
        <div class="container">
            <header>
                <div class="row">
                    <div class="col-md-12">
                        <div class="logo-new pull-left"><img class="img-responsive" src="/templates/public/images/di_logo_refl.png" /></div>
                        <div class="h1-new pull-left"><h1><EPiServer:Property runat="server" PropertyName="CampaignHeading" /></h1></div>
                    </div>
                </div>
            </header>
        </div>
    </div>    

    <div class="hero">
        <div class="container">
            <asp:PlaceHolder runat="server" ID="PhSubHeader">
                <div class="row">
                    <div class="h2-new col-md-8 subheading"><h2><EPiServer:Property runat="server" PropertyName="CampaignSubHeading" /></h2><span>Alla priser är inkl. moms</span></div>
                </div>
            </asp:PlaceHolder>            
            <div runat="server" id="ThankYouHeading" visible="false" class="thankyou">
                <h1><EPiServer:Property runat="server" PropertyName="ThankYouHeading" /> <small><asp:Label runat="server" ID="LblConfirmText"></asp:Label></small></h1>             
            </div>
            <div class="row hero-inner">
                <asp:MultiView runat="server" ID="MvHeroView" ActiveViewIndex="0">
                    <asp:View ID="ViewUsp" runat="server">
                        <div class='col-md-8'>
                            <div class="jumbotron">                                              
                                <div class="campaign1 campw selected" style='background: <%=CurrentPage["Campaign1BgImage"] != null ? "url(" +  CurrentPage["Campaign1BgImage"] + ")" : "none" %> no-repeat scroll right 20px top transparent;'>
                                    <asp:RadioButton ID="RbPrimaryCampaign" Checked="true" GroupName="CampaignGroup" CssClass="RbPrimary" runat="server" />
                                    <asp:Label AssociatedControlID="RbPrimaryCampaign" runat="server">
                                        <span><%= CurrentPage["Campaign1Heading"]%></span><br />
                                        <small>
                                            <%= CurrentPage["Campaign1Sub1"]%><br />
                                            <strong><%= CurrentPage["Campaign1Sub2"]%></strong><br />
                                            <i><%= CurrentPage["Campaign1Sub3"]%></i>
                                        </small>
                                    </asp:Label>
                                    <%if (CurrentPage["Campaign1BgImage"] != null){ %>
                                        <img src='<%=CurrentPage["Campaign1BgImage"]%>' alt='<%=CurrentPage["Campaign1Heading"]%>' class="visible-xs" />
                                    <%} %>
                                </div>
                                <div class="campaign2 campw" style='background: <%=CurrentPage["Campaign2BgImage"] != null ? "url(" +  CurrentPage["Campaign2BgImage"] + ")" : "none" %> no-repeat scroll right 20px top transparent;'>
                                    <asp:RadioButton ID="RbSecondaryCampaign" GroupName="CampaignGroup" runat="server" CssClass="RbSecondary" />
                                    <asp:Label AssociatedControlID="RbSecondaryCampaign" runat="server">
                                        <span><%= CurrentPage["Campaign2Heading"]%></span><br />
                                        <small>
                                            <%= CurrentPage["Campaign2Sub1"]%><br />
                                            <strong><%= CurrentPage["Campaign2Sub2"]%></strong><br />
                                            <i><%= CurrentPage["Campaign2Sub3"]%></i>
                                        </small>
                                    </asp:Label>
                                    <%if (CurrentPage["Campaign2BgImage"] != null){ %>
                                        <img src='<%=CurrentPage["Campaign2BgImage"]%>' alt='<%=CurrentPage["Campaign2Heading"]%>' class="visible-xs" />
                                    <%} %>
                                </div>                                                 
                            </div>
                        </div>
                    </asp:View>
                    <asp:View ID="ViewConfirm" runat="server">
                        <div class='col-md-12'>
                            <div class="jumbotron jumbotron-thanks">   
                                <asp:MultiView runat="server" ID="MvConfirmView" ActiveViewIndex="0">
                                    <asp:View ID="ViewConfirmStep1" runat="server">
                                        <div class="row"> 
                                            <div class="col-md-12 dighead">
                                                <h3>Kom igång digitalt redan nu - skapa konto!</h3>
                                            </div>
                                        </div>
                                        <div class="row digarea">
                                            <div class="col-md-4 digform"> 
                                                <div class="form-group">
                                                    <asp:Label runat="server" AssociatedControlID="TextBoxDigUser">Användarnamn (e-post)</asp:Label>
                                                    <asp:TextBox ID="TextBoxDigUser" CssClass="form-control" required="required" runat="server"></asp:TextBox>

                                                    <asp:PlaceHolder ID="PlaceHolderDigPasswds" runat="server">
                                                        <asp:Label runat="server" AssociatedControlID="TextBoxDigPass1">Lösenord (minst 6 tecken)</asp:Label>
                                                        <asp:TextBox ID="TextBoxDigPass1" CssClass="form-control" TextMode="Password" runat="server"></asp:TextBox>                                                                                                                                            
                                                        <asp:Label runat="server" AssociatedControlID="TextBoxDigPass2">Repetera lösenord</asp:Label>
                                                        <asp:TextBox ID="TextBoxDigPass2" CssClass="form-control" TextMode="Password" runat="server"></asp:TextBox>  
                                                        <asp:CompareValidator runat="server" ValidationGroup="bondig" ControlToValidate="TextBoxDigPass1" Display="Dynamic" ControlToCompare="TextBoxDigPass2" Type="String">
                                                            <span class="alert alert-danger fade in alert-margin">Lösenorden måste vara identiska</span>
                                                        </asp:CompareValidator>
                                                    </asp:PlaceHolder>
                                                </div>
                                                <asp:Literal ID="LiteralDigErr" Visible="false" runat="server"></asp:Literal>
                                        
                                                <asp:Button ID="ButtonDigAcc" Text="Skapa Di-konto" class="btn btn-success" ValidationGroup="bondig" runat="server" OnClick="ButtonDigAcc_Click" />
                                        
                                                <div class="dig-group">
                                                    <asp:LinkButton ID="LinkButtonDigHaveAcc" runat="server" CssClass="link-red" onclick="LinkButtonDigHaveAcc_Click">Jag har redan ett Di-konto</asp:LinkButton>
                                                    <asp:LinkButton ID="LinkButtonDigNoAcc" runat="server" CssClass="link-red" onclick="LinkButtonDigNoAcc_Click" Visible="false">Jag har inget Di-konto</asp:LinkButton>
                                                </div>

                                            </div>
                                            <div class="col-md-6 col-md-offset-1">
                                                    <h4>Läs Di när, var och hur du vill!</h4>
                                                    Som prenumerant får du nu tillgång till dina digitala tjänster och kan läsa Dagens industri när och hur du vill och var du än är någonstans. 
                                                    Perfekt när du är på tjänsteresa, semester eller på annan plats där du inte har tillgång till papperstidningen. 
                                                    Aktivera dina digitala tjänster genom att fylla i formuläret till vänster. 
                                                    När du skapar ett nytt Di-konto så får du ett mail sänt till din e-postadress där du behöver bekräfta ditt Di-konto.
                                                    <br />
                                                    <br />
                                                    <img src="/Templates/Public/Pages/bscampaign/images/digitalProducts.jpg" alt="" border="0" />
                                            </div>
                                        </div>                                     
                                    </asp:View>
                                    <asp:View ID="ViewConfirmStep2" runat="server">
                                        <div class="row digarea">
                                            <div class="row"> 
                                                <div class="col-md-12">                                                                                    
                                                    <h1>Grattis!</h1>
                                                    <p>Nu har du full tillgång till tidningens innehåll digitalt. Nedan kan du välja på vilket 
                                                    sätt du vill läsa tidningen så hjälper vi dig – steg för steg – med att komma igång.</p>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-2">
                                                    <img src="http://dagensindustri.se/Global/Digitalt/sid3_mobil.png" alt="" border="0" />
                                                </div>
                                                <div class="col-md-9">
                                                    <h4>I mobilen</h4>
                                                    Ta del av hela tidningens innehåll i din smarta mobil redan vid midnatt. Här kan du läsa vår guide – som 
                                                    visar steg för steg – hur du kommer igång med att läsa Dagens industri i din mobil.<br>
                                                    <a target="_blank" href="/guide/mobil/">Läs guiden här</a>
                                                </div>
                                            </div>                                        
                                            <div class="row">
                                                <div class="col-md-2">
                                                    <img src="http://dagensindustri.se/Global/Digitalt/sid3_lasplatta.png" alt="" border="0" />
                                                </div>
                                                <div class="col-md-9">
                                                    <h4>I läsplattan</h4>
                                                    Läs Dagens industri, Di Weekend samt Di Idé, med extra material och mervärden som fördjupad grafik, bildspel och 
                                                    Twitterflöden, direkt i läsplattan. Vår guide visar dig – steg för steg – hur du kommer igång.<br>
                                                    <a target="_blank" href="/guide/lasplatta/">Läs guiden här</a>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-2">
                                                    <img src="http://dagensindustri.se/Global/Digitalt/sid3_online.png" alt="" border="0" />
                                                </div>
                                                <div class="col-md-9">
                                                    <h4>På nätet eller som PDF</h4>
                                                    Ta del av hela tidningens innehåll som pdf redan klockan 22.00 kvällen före utgivning. Vår guide visar dig – 
                                                    steg för steg – hur du gör för att läsa vårt utbud i din dator.<br>
                                                    <a target="_blank" href="/guide/dator/">Läs guiden här</a>
                                                </div>
                                            </div>                                                                                    
                                        </div>                                            
                                    </asp:View>
                                </asp:MultiView>
                            </div>
                            <footer id="footer">
                                <div class="container">
                                    <div class="col-md-12">                    
                                        <EPiServer:Property runat="server" PropertyName="CampaignFooterComplete" />
                                    </div>
                                </div>
                            </footer>
                        </div>
                    </asp:View>
                </asp:MultiView>

                <div class="col-md-4" id="Affix" runat="server" visible="true">
                    <div>
                        <div class="sidebar" data-spy="affix" data-offset-bottom="300" data-offset-top="50">
                            <!-- multistep form -->
                            <div id="msform" class="form-horizontal" role="form">
                                <!-- progressbar -->
                                <ul id="progressbar">
                                    <li class="active"></li>
                                    <li></li>
                                    <li></li>
                                </ul>
                                <!-- STEP1 -->
                                <fieldset>
                                    <h2 class="fs-title begin-here">
                                        Börja här!</h2>
                                    <div class="form-group" id="email">
                                        <asp:Label runat="server" AssociatedControlID="TxtEmail">Din e-post *</asp:Label>
                                        <asp:TextBox runat="server" ID="TxtEmail" CssClass="form-control txtemail" placeholder="E-post"></asp:TextBox>
                                        <asp:RequiredFieldValidator runat="server" Display="Dynamic" ValidationGroup="email" ControlToValidate="TxtEmail"><span class="alert alert-danger fade in alert-margin">Vänligen fyll i din e-post</span></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator runat="server" Display="Dynamic" ValidationGroup="email"
                                            ControlToValidate="TxtEmail" CssClass="valreg" ValidationExpression="^([_A-Za-z0-9-])+(\.[_A-Za-z0-9-]+)*@(([A-Za-z0-9]){1,}([-.])?([A-Za-z0-9]){1,})+((\.)?([A-Za-z0-9]){1,}([-])?([A-Za-z0-9]){1,})*(\.[A-Za-z]{2,4})$"><span class="alert alert-danger fade in alert-margin">Din epost tycks inte vara korrekt, försök igen</span></asp:RegularExpressionValidator>
                                    </div>
                                    <button type="button" name="next" id="step1-next" class="btn btn-lg btn-success">Gå vidare</button>
                                </fieldset>
                                <!-- //STEP1 -->
                                <!-- STEP2 -->
                                <fieldset>
                                    <a href="#" name="previous" class="previous"><i class="icon-arrow-left"></i></a>
                                    <h2 class="fs-title">
                                        Dina adressuppgifter</h2>
                                    <div class="form-group" id="auto">
                                        <p class="help-block">
                                            <a href="#">Hämta uppgifter automatiskt</a></p>
                                    </div>
                                    <div class="form-group" id="ssn">
                                        <asp:Label runat="server" AssociatedControlID="TxtSsn">Person- eller organisationsnummer&nbsp;<span data-toggle="tooltip" title="" data-original-title="Ange person- (ÅÅÅÅMMDDXXXX) eller organisationsnummer (XXXXXX-XXXX) för att hämta dina uppgifter automatiskt. "><i class="icon-info-sign icon-1-5x"></i></span></asp:Label>
                                        <!--[if lte IE 9]>
                                        <br /><i class="pull-left" style="font-weight: normal;">ÅÅMMDDXXXX eller XXXXXX-XXXX</i>
                                        <![endif]-->                                        
                                        <asp:TextBox runat="server" ID="TxtSsn" CssClass="form-control txtssn" placeholder="ÅÅÅÅMMDDXXXX eller XXXXXX-XXXX"></asp:TextBox>

                                        <div id="ssnerror" class="alert alert-danger fade in alert-margin" style="display: none;">
                                        </div>
                                        <p class="help-block">
                                            <a href="#" class="pull-right">Eller fyll i uppgifter manuellt <i class="icon-double-angle-right"></i></a>
                                        </p>
                                        <div class="clearfix">
                                        </div>
                                        <button class="btn btn-success" id="getAddress">
                                            Hämta mina adressuppgifter</button>
                                    </div>
                                    <!-- Wrapper for Extra info -->
                                    <div class="" id="extra-info">
                                        <div class="form-group" id="pulled-data">
                                            <div class="verify-box">
                                                <div class="icon-wrapper">
                                                    <span data-original-title="Ändra" data-toggle="tooltip" title="">
                                                        <a href="#"><i class="icon-edit" id="showform"></i></a>
                                                    </span>                                                
                                                </div>
                                                <span class="txtfname-ver">-</span> <span class="txtlname-ver">-</span><br />
                                                <span class="txtaddress-ver">-</span> <span class="txtaddresshouseno-ver">-</span>
                                                <span class="txtaddresstaircase-ver">-</span> <span class="txtaddresstairs-ver">-</span><br />
                                                <span class="txtaddresszipcode-ver">-</span> <span class="txtcity-ver">-</span><br />
                                                <span class="txtemail-ver">-</span><br />
                                                <span class="txtphone-ver">-</span><br />
                                                <span class="txtcompany-ver"></span>
                                            </div>
                                        </div>
                                        <h3 class="fs-title">Komplettera följande uppgifter</h3>
                                    </div>
                                    <!-- All form fields -->
                                    <div id="wform">
                                        <div class="form-group" id="name">
                                            <div class="row">
                                                <div class="col-xs-6" style="padding-right:0">
                                                    <asp:Label runat="server" AssociatedControlID="TxtFirstName">Förnamn *</asp:Label>
                                                    <asp:TextBox runat="server" ID="TxtFirstName" CssClass="form-control txtfname" placeholder="Förnamn"></asp:TextBox>
                                                    <asp:RequiredFieldValidator runat="server" Display="Dynamic" ValidationGroup="form"
                                                        ControlToValidate="TxtFirstName"><span class="alert alert-danger fade in alert-margin">Vänligen ange ditt förnamn</span></asp:RequiredFieldValidator>
                                                </div>
                                                <div class="col-xs-6" style="padding-right:5px">
                                                    <asp:Label runat="server" AssociatedControlID="TxtLastName">Efternamn *</asp:Label>
                                                    <asp:TextBox runat="server" ID="TxtLastName" CssClass="form-control txtlname" placeholder="Efternamn"></asp:TextBox>
                                                    <asp:RequiredFieldValidator runat="server" Display="Dynamic" ValidationGroup="form"
                                                        ControlToValidate="TxtLastName"><span class="alert alert-danger fade in alert-margin">Vänligen ange ditt efternamn</span></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group" id="extrainput" runat="server" visible='<%#IsValue("ExtraInfoHeading") %>'>
                                            <div class="row">
                                                <div class="col-xs-8">
                                                    <asp:Label runat="server" AssociatedControlID="TxtExtraInfo"><%=CurrentPage["ExtraInfoHeading"] %></asp:Label>
                                                    <asp:TextBox runat="server" ID="TxtExtraInfo" CssClass="form-control txtextrainput"></asp:TextBox>

                                                    <asp:RequiredFieldValidator 
                                                      runat="server" 
                                                      Display="Dynamic" 
                                                      CssClass="ExtraInputVal" 
                                                      Visible='<%#IsValue("ExtraInfoMandatory") %>' 
                                                      ValidationGroup="form"
                                                      ControlToValidate="TxtExtraInfo">
                                                        <span class="alert alert-danger fade in alert-margin">Detta fält är tvingande</span>
                                                    </asp:RequiredFieldValidator>
                                                        
                                                    <asp:RegularExpressionValidator 
                                                      runat="server" 
                                                      Display="Dynamic" 
                                                      CssClass="ExtraInputVal" 
                                                      ValidationGroup="form"
                                                      ControlToValidate="TxtExtraInfo" 
                                                      ValidationExpression="(^[\d]{9}$)">
                                                        <span class="alert alert-danger fade in alert-margin">EuroBonusnumret måste anges med 9 siffror.</span>
                                                    </asp:RegularExpressionValidator>

                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group" id="studssn">
                                            <div class="row">
                                                <div class="col-xs-8">
                                                    <asp:Label runat="server" AssociatedControlID="TxtStudSsn">Personnummer *</asp:Label>
                                                    <asp:TextBox runat="server" ID="TxtStudSsn" CssClass="form-control txtstudssn" placeholder="ÅÅÅÅMMDDXXXX"></asp:TextBox>
                                                    
                                                    <asp:RequiredFieldValidator 
                                                      runat="server" 
                                                      Display="Dynamic" 
                                                      CssClass="StudSsnVal" 
                                                      ValidationGroup="form"
                                                      ControlToValidate="TxtStudSsn">
                                                        <span class="alert alert-danger fade in alert-margin">Vänligen ange ditt personnummer</span>
                                                    </asp:RequiredFieldValidator>
                                                    
                                                    <asp:RegularExpressionValidator 
                                                      runat="server" 
                                                      Display="Dynamic" 
                                                      CssClass="StudSsnRegVal" 
                                                      ValidationGroup="form"
                                                      ControlToValidate="TxtStudSsn" 
                                                      ValidationExpression="(^[\d]{12}$)">
                                                        <span class="alert alert-danger fade in alert-margin">Ditt personnummer tycks inte vara korrekt, försök igen med 12 siffror ÅÅÅÅMMDDXXXX.</span>
                                                    </asp:RegularExpressionValidator>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="form-group" id="phone">
                                            <div class="row">
                                                <div class="col-xs-8">
                                                    <asp:Label runat="server" AssociatedControlID="TxtPhone">Mobilnummer *</asp:Label>
                                                    <asp:TextBox runat="server" ID="TxtPhone" CssClass="form-control txtphone" placeholder="Telefonnummer"></asp:TextBox>
                                                    <asp:RequiredFieldValidator runat="server" Display="Dynamic" CssClass="val" ValidationGroup="form"
                                                        ControlToValidate="TxtPhone"><span class="alert alert-danger fade in alert-margin">Vänligen ange ditt mobilnummer</span></asp:RequiredFieldValidator>
                                                    <asp:RegularExpressionValidator runat="server" Display="Dynamic" CssClass="val" ValidationGroup="form"
                                                        ControlToValidate="TxtPhone" ValidationExpression="(^0([0-9-\s\+]){7,}$)"><span class="alert alert-danger fade in alert-margin">Ditt mobilnummer tycks inte vara korrekt, försök igen med bara siffror</span></asp:RegularExpressionValidator>
                                                </div>
                                            </div>
                                        </div>
                                        <div id="addCompany" class="form-group">
                                            <div class="row">
                                            <div class="col-md-12">
                                                <a class="btn btn-link" href="#">
                                                    Lägg till företagsuppgifter <i class="icon-double-angle-right"></i>                                                    
                                                </a>&nbsp;<span data-original-title="Lägg till extra uppgifter för att få tidningen skickad till ditt företag" data-toggle="tooltip" title="" ><i class="icon-info-sign icon-1-5x"></i></span>
                                            </div>
                                            </div>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                        <div class="form-group" id="company">
                                            <asp:Label runat="server" AssociatedControlID="TxtCompany">Företagsuppgifter</asp:Label>
                                            <asp:TextBox runat="server" ID="TxtCompany" CssClass="form-control txtcompany" Rows="2"></asp:TextBox>
                                        </div>
                                        <div class="clearfix">
                                        </div>
                                        <div class="form-group" id="address">
                                            <div class="row">
                                                <div class="col-xs-8 float-left">
                                                    <asp:Label runat="server" AssociatedControlID="TxtStreetAddress">Gatuadress *</asp:Label>
                                                    <asp:TextBox runat="server" ID="TxtStreetAddress" CssClass="form-control txtaddress"
                                                        placeholder="Gatuadress"></asp:TextBox>
                                                    <asp:RequiredFieldValidator runat="server" Display="Dynamic" CssClass="val" ValidationGroup="form"
                                                        ControlToValidate="TxtStreetAddress"><span class="alert alert-danger fade in alert-margin">Vänligen ange din gatuadress</span></asp:RequiredFieldValidator>
                                                </div>
                                                <div class="col-xs-4 float-right">
                                                    <asp:Label runat="server" AssociatedControlID="TxtHouseNo">Nr</asp:Label>
                                                    <asp:TextBox runat="server" ID="TxtHouseNo" CssClass="form-control txtaddresshouseno"
                                                        placeholder="Nr"></asp:TextBox>
                                                </div>
                                            </div>
                                            <a href="#" class="pull-left" id="addAddressCo">Lägg till C/O-adress&nbsp;<i class="icon-double-angle-right"></i></a>
                                            <div id="addressCo">
                                                <br />
                                                <asp:Label runat="server" AssociatedControlID="TxtAddressCo">C/O-adress</asp:Label>
                                                <asp:TextBox runat="server" ID="TxtAddressCo" CssClass="form-control txtaddressco"
                                                    placeholder="C/O"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group" id="address-extra">
                                            <div class="row">
                                                <div class="col-xs-3 float-left" style="padding-right:0">
                                                    <asp:Label runat="server" AssociatedControlID="TxtStairCase">Uppg</asp:Label>
                                                    <asp:TextBox runat="server" ID="TxtStairCase" CssClass="form-control txtaddresstaircase"
                                                        placeholder="A"></asp:TextBox>
                                                </div>
                                                <div class="col-xs-3 float-left">
                                                    <asp:Label runat="server" AssociatedControlID="TxtStairs">Trp</asp:Label>
                                                    <asp:TextBox runat="server" ID="TxtStairs" CssClass="form-control txtaddresstairs"
                                                        placeholder="5"></asp:TextBox>
                                                </div>
                                                <div class="col-xs-5 float-left">
                                                    <asp:Label runat="server" AssociatedControlID="TxtAppartmentNo">Lgh Nr</asp:Label>
                                                    <asp:TextBox runat="server" ID="TxtAppartmentNo" CssClass="form-control txtaddressappno"
                                                        placeholder="1001"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group" id="zip">
                                            <div class="row form-inline">
                                                <div class="col-xs-5" style="padding-right: 0;width:55%">
                                                    <asp:Label runat="server" AssociatedControlID="TxtZipCode">
                                                    Postnr *&nbsp;<span data-toggle="tooltip" title="Postorten läggs till automatiskt."><i class="icon-info-sign icon-1-5x"></i></span>
                                                    </asp:Label>
                                                    <asp:TextBox runat="server" ID="TxtZipCode" CssClass="form-control txtaddresszipcode"
                                                        placeholder="12345"></asp:TextBox>
                                                    <asp:RequiredFieldValidator runat="server" Display="Dynamic" CssClass="val" ValidationGroup="form"
                                                        ControlToValidate="TxtZipCode"><span class="alert alert-danger fade in alert-margin">Vänligen ange ditt postnummer</span></asp:RequiredFieldValidator>
                                                    <asp:RegularExpressionValidator runat="server" Display="Dynamic" CssClass="var" ValidationGroup="form"
                                                        ControlToValidate="TxtZipCode" ValidationExpression="(^[\d]{5}$)"><span class="alert alert-danger fade in alert-margin">Ange ditt postnummer med 5 siffror</span></asp:RegularExpressionValidator>
                                                </div>
 
                                                <div class="col-xs-7">
                                                    <asp:Label runat="server" AssociatedControlID="TxtCity">Ort</asp:Label>
                                                    <asp:TextBox runat="server" ID="TxtCity" CssClass="form-control txtcity" Enabled="false"></asp:TextBox>
                                                </div>
                                            </div>  
                                        </div>
                                        <div class="form-group" id="pren">
                                            <div class="row">
                                                <div class="col-xs-8">
                                                    <asp:Label runat="server" AssociatedControlID="TxtStartDate">
                                                        Prenumerationsstart
                                                    </asp:Label>  
                                                  <br/>
                                                  <div class="input-group pull-left">
                                                    <asp:TextBox runat="server" ID="TxtStartDate" CssClass="form-control txtprenstart" data-date="" data-date-weekstart="1" data-date-format="yyyy-mm-dd"></asp:TextBox>
                                                    <span class="input-group-addon"><i class="icon-calendar"></i></span>
                                                  </div>
                                                  <asp:RegularExpressionValidator runat="server" ID="RegValStartDate" Display="Dynamic" CssClass="val" ValidationGroup="form" ControlToValidate="TxtStartDate" ErrorMessage="Felaktigt format på datum (åååå-dd-mm)" ValidationExpression="(^[\d]{4}-[\d]{2}-[\d]{2}$)">Fel format</asp:RegularExpressionValidator>
                                                </div>
                                            </div>
                                        </div>
                                        <br />
                                    </div>
                                    <button type="button" id="step2-next" name="next" class="btn btn-success">Nästa</button>
                                </fieldset>
                                <!-- //STEP2 -->
                                <!-- STEP3 -->
                                <fieldset>
                                    <a href="#" name="previous" class="previous"><i class="icon-arrow-left"></i></a>
                                    <h2 class="fs-title">
                                        Välj betalsätt & bekräfta</h2>
                                    <div class="form-group" id="verify">
                                        <div class="verify-box">
                                            <span class="txtfname-ver">-</span> <span class="txtlname-ver">-</span><br />
                                            <span class="txtaddress-ver">-</span> <span class="txtaddresshouseno-ver">-</span>
                                            <span class="txtaddresstaircase-ver">-</span> <span class="txtaddresstairs-ver">-</span><br />
                                            <span class="txtaddresszipcode-ver">-</span> <span class="txtcity-ver">-</span><br />
                                            <span class="txtemail-ver">-</span><br />
                                            <span class="txtphone-ver">-</span><br />
                                            <span class="txtcompany-ver"></span>
                                        </div>
                                    </div>         
                                    <div id="payinvoice">
                                        <asp:LinkButton runat="server" CssClass="btn btn-success btn-lg" OnClick="BtnSubmitInvoiceClick">
                                            <span>Betala med faktura</span> &nbsp;&nbsp;<i class="icon-file-text-alt icon-1-5x"></i>
                                        </asp:LinkButton>
                                    </div>               
                                    <div id="payinvoiceother">
                                        <a href="#" class="btn btn-link" data-toggle="modal" data-target="#company-invoice">Faktura men till företaget <i class="icon-double-angle-right"></i></a>
                                        <br />
                                    </div>            
                                    <div id="paycard">
                                        <asp:LinkButton runat="server" CssClass="btn btn-success btn-lg" OnClick="BtnSubmitCardClick">
                                            Betala med kort &nbsp;&nbsp;<i class="icon-credit-card icon-1-5x"></i>
                                        </asp:LinkButton>
                                        <p class="alert alert-diinfo amount" style="display:none">Årsbeloppet debiteras vid köptillfället</p>
                                        <br /><br />
                                    </div>
                                    <div id="payautopay">
                                        <asp:LinkButton runat="server" CssClass="btn btn-success btn-lg" OnClick="BtnSubmitAutopayClick">
                                            Betala med autogiro &nbsp;&nbsp;<i class="icon-credit-card icon-1-5x"></i>
                                        </asp:LinkButton>                                        
                                        <br /><br />
                                    </div>
                                    <div id="payautowithdrawal">
                                        <asp:LinkButton runat="server" CssClass="btn btn-success btn-lg" OnClick="BtnSubmitAutoWithdrawalClick">
                                            Autodragning &nbsp;&nbsp;<i class="icon-credit-card icon-1-5x"></i>
                                        </asp:LinkButton>                                        
                                    </div>

                                </fieldset>
                                <!-- //STEP3 -->
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    
    <div class="container benefits" id="Benefits" runat="server">
        <EPiServer:PageList ID="PlProducts" runat="server">
            <HeaderTemplate>
                <div class="row" id="listanchor">
                    <div class="col-md-12">
                        <h2><EPiServer:Property runat="server" PropertyName="CampaignIncludedHeading" /></h2>
                    </div>
                </div>  
                <div class="row">
                <div class="col-md-8">
            </HeaderTemplate>
            <ItemTemplate>
                <div class="row">
                    <div class="col-md-12">
                        <div class="well">
                            <div class="row">
                                <div class="col-md-3">
                                    <img src="<%# Container.CurrentPage.Property["CampaignSideBarImage"] %>" class="img-responsive" alt="<%# Container.CurrentPage.PageName %>" />
                                </div>
                                <div class="col-md-9">
                                    <h3><%# Container.CurrentPage.Property["CampaignSideBarHeading"] %> <small><%# Container.CurrentPage.Property["CampaignSideBarWeekdays"]%></small></h3>
                                    <p><%# Container.CurrentPage.Property["CampaignSideBarDescription"]%></p>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </ItemTemplate>
            <FooterTemplate>
                </div>
                <div class="col-md-4 uspw">
                    <h2><EPiServer:Property runat="server" PropertyName="CampaignUspHeading" /></h2>
                    <EPiServer:Property runat="server" PropertyName="CampaignUspBody" />
                </div>
                </div>
            </FooterTemplate>
        </EPiServer:PageList>
    </div>

    <footer id="Footer" style="min-height:800px" runat="server">
        <div class="container">
            <div class="col-md-8">
                <EPiServer:Property runat="server" PropertyName="CampaignFooter" />    
            </div>
        </div>
    </footer>

    <div class="modal fade" id="company-invoice" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title">
                        Faktura till företaget
                    </h4>
                </div>
                <div class="modal-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-sm-6">
                                <asp:Label runat="server" AssociatedControlID="TxtOpCompany" CssClass="control-label">Företagsnamn/namn *</asp:Label>
                                <asp:TextBox runat="server" ID="TxtOpCompany" CssClass="form-control" placeholder="Företagsnamn"></asp:TextBox>
                                <asp:RequiredFieldValidator runat="server" Display="Dynamic" ValidationGroup="opform"
                                    ControlToValidate="TxtOpCompany"><span class="alert alert-danger fade in alert-margin">Vänligen ange företagsnamn/namn</span></asp:RequiredFieldValidator>
                            </div>
                            <div class="col-sm-6">
                                <asp:Label runat="server" AssociatedControlID="TxtOpAttention" CssClass="control-label">Attention *</asp:Label>
                                <asp:TextBox runat="server" ID="TxtOpAttention" CssClass="form-control" placeholder="Förnamn Efternamn"></asp:TextBox>
                                <asp:RequiredFieldValidator runat="server" Display="Dynamic" ValidationGroup="opform"
                                    ControlToValidate="TxtOpAttention"><span class="alert alert-danger fade in alert-margin">Vänligen ange attention</span></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-sm-10">
                                <asp:Label runat="server" AssociatedControlID="TxtOpStreetAddress" CssClass="control-label">Gatuadress *</asp:Label>
                                <asp:TextBox runat="server" ID="TxtOpStreetAddress" CssClass="form-control" placeholder="Storgatan"></asp:TextBox>
                                <asp:RequiredFieldValidator runat="server" Display="Dynamic" ValidationGroup="opform"
                                    ControlToValidate="TxtOpStreetAddress"><span class="alert alert-danger fade in alert-margin">Vänligen ange din gatuadress</span></asp:RequiredFieldValidator>
                            </div>
                            <div class="col-sm-2">
                                <asp:Label runat="server" AssociatedControlID="TxtOpHouseNo" CssClass="control-label">Nummer</asp:Label>
                                <asp:TextBox runat="server" ID="TxtOpHouseNo" CssClass="form-control" placeholder="1"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-sm-4">
                                <asp:Label runat="server" AssociatedControlID="TxtOpZip" CssClass="control-label">
                                    Postnr * <span data-toggle="tooltip" title="Postorten läggs till automatiskt."><i class="icon-info-sign icon-1-5x"></i></span>
                                </asp:Label>
                                <asp:TextBox runat="server" ID="TxtOpZip" CssClass="form-control txtaddresszipcodeop" placeholder="12345"></asp:TextBox>
                                <asp:RequiredFieldValidator runat="server" Display="Dynamic" ValidationGroup="opform"
                                    ControlToValidate="TxtOpZip"><span class="alert alert-danger fade in alert-margin">Vänligen ange ditt postnummer</span></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator runat="server" Display="Dynamic" ValidationGroup="opform"
                                    ControlToValidate="TxtOpZip" ValidationExpression="(^[\d]{5}$)"><span class="alert alert-danger fade in alert-margin">Ange ditt postnummer med 5 siffror</span></asp:RegularExpressionValidator>
                            </div>
                            <div class="col-sm-4">
                                <asp:Label runat="server" AssociatedControlID="TxtOpCity">Ort</asp:Label>
                                <asp:TextBox runat="server" ID="TxtOpCity" CssClass="form-control txtcityop" Enabled="false"></asp:TextBox>
                            </div>
                            <div class="col-sm-4">
                                <asp:Label runat="server" AssociatedControlID="TxtOpPhone" CssClass="control-label">Telefonnummer</asp:Label>
                                <asp:TextBox runat="server" ID="TxtOpPhone" CssClass="form-control" placeholder="08-123 456"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <p>                        
                        <asp:LinkButton runat="server" CssClass="btn btn-success btn-lg" ValidationGroup="opform" OnClick="BtnSubmitInvoiceOtherPayerClick">
                            Bekräfta beställning &nbsp;&nbsp;<i class="icon-file-text-alt icon-1-5x"></i>
                        </asp:LinkButton>
                    </p>
                </div>
            </div>
        </div>
    </div>
        
    <div class="modal fade" id="modalError" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title" id="myModalLabel">Något gick fel</h4>
                </div>
                <div class="modal-body">
                    <div class="alert alert-danger">
                        <asp:Label runat="server" ID="LblError" EnableViewState="false"></asp:Label>                                                                                
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" data-dismiss="modal" class="btn btn-primary center-block">OK</button>
                </div>
            </div>
        </div>        
    </div>          
    
    <%--Hidden fields used by jquery logic--%>
    <input type="hidden" id="pageid" value="<%= CurrentPage.PageLink.ID %>" />
    <input type="hidden" id="campaign1FreeCamp" value="<%= CurrentPage["Campaign1FreeCamp"] %>" />
    <input type="hidden" id="campaign1StudentCamp" value="<%= CurrentPage["Campaign1IsStudent"] %>" />
    <input type="hidden" id="campaign1HideCard" value="<%= CurrentPage["Campaign1HideCard"] %>" />
    <input type="hidden" id="campaign1HideInvoice" value="<%= CurrentPage["Campaign1HideInvoice"] %>" />
    <input type="hidden" id="campaign1HideInvoiceOther" value="<%= CurrentPage["Campaign1HideInvoiceOther"] %>" />
    <input type="hidden" id="campaign1HideAutoPay" value="<%= CurrentPage["Campaign1HideAutoPay"] %>" />
    <input type="hidden" id="campaign1HideAutoWithdrawal" value="<%= CurrentPage["Campaign1HideAutoWithdrawal"] %>" />
    <input type="hidden" id="campaign2FreeCamp" value="<%= CurrentPage["Campaign2FreeCamp"] %>" />
    <input type="hidden" id="campaign2HideCard" value="<%= CurrentPage["Campaign2HideCard"] %>" />
    <input type="hidden" id="campaign2HideInvoice" value="<%= CurrentPage["Campaign2HideInvoice"] %>" />
    <input type="hidden" id="campaign2HideInvoiceOther" value="<%= CurrentPage["Campaign2HideInvoiceOther"] %>" />
    <input type="hidden" id="campaign2HideAutoPay" value="<%= CurrentPage["Campaign2HideAutoPay"] %>" />
    <input type="hidden" id="campaign2HideAutoWithdrawal" value="<%= CurrentPage["Campaign2HideAutoWithdrawal"] %>" />
    <input type="hidden" id="campaign2StudentCamp" value="<%= CurrentPage["Campaign2IsStudent"] %>" />
           
    <script src="/c-components/scripts/34ad1adc.jquery.js" type="text/javascript"></script>
    <script src="/c-components/scripts/05025d63.jquery.plugins.js" type="text/javascript"></script>
    <script type="text/javascript" src="/bootstrapDi/js/bootstrap-datepicker.js"></script>
    <script src="/c-components/scripts/main.js?v=11" type="text/javascript"></script>
    <!--[if !IE]><!-->
    <script src="/c-components/scripts/vendor/bootstrap.js?v=1" type="text/javascript"></script>
    <!--<![endif]-->
    <!--[if lte IE 9]>
    <script src="/c-components/scripts/vendor/bootstrapie9.js?v=1" type="text/javascript"></script>
    <![endif]-->    
    <script src="/c-components/scripts/vendor/respond.js" type="text/javascript"></script>
    <script src="/bootstrapDi/js/selectivizr-min.js" type="text/javascript"></script>

    <asp:Literal ID="LiteralScript" EnableViewState="false" runat="server"></asp:Literal>
    <asp:Literal ID="LiteralAdWordsScriptOnLoad" EnableViewState="false" runat="server"></asp:Literal>
    <asp:Literal ID="LiteralAdWordsScriptOnThankYou" EnableViewState="false" runat="server"></asp:Literal>
    
</asp:Content>
