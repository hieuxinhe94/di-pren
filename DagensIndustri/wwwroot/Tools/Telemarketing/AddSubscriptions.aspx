<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddSubscriptions.aspx.cs" Inherits="DagensIndustri.Tools.Telemarketing.AddSubscriptions" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Telemarketing - Add subscriptions</title>
  <link href="/Templates/Public/js/datatables/css/jquery.dataTables.min.css" rel="stylesheet" type="text/css" />
  <link href="css/telemarketing.css" rel="stylesheet" type="text/css" />

  <script src="/Templates/Public/js/jquery/jquery-1.11.1.min.js" type="text/javascript"></script>
  <script src="/Templates/Public/js/datatables/js/jquery.dataTables.min.js" type="text/javascript"></script>
  <script src="/Tools/Telemarketing/js/tools.js" type="text/javascript"></script>
</head>
<body>
<div id="waitOverlay" class="wait-overlay"><img src="/Tools/Telemarketing/img/loading.gif" alt="Arbetar..." /></div>
    <form id="subscriptionForm" runat="server">
    <div class="content-container">
      
      <table class="mainTable">
      <tr>
        <td>
        <h1>Prova-på-prenumeration</h1>
        <p>Undersök om kunden redan finns i våra databaser.</p>
        
        <table class="inputfields">
        <tr>
          <td>
            <b>E-postadress:</b><br />
            <asp:TextBox ID="txtEmail" TabIndex="1" runat="server"></asp:TextBox>
            <br />
            <span id="emailMessage"></span><span id="cusnoForSearchCriteriaInfo"></span>
            <br />
            <input id="btnEmailControl" type="button" TabIndex="5" value="Sök kund" disabled="disabled"/> <a href="/Tools/Telemarketing/AddSubscriptions.aspx">[Ny sökning]</a>
          </td>
          <td>
            <span id="extendedInputFields">
            <b>Förnamn:</b><br />
            <asp:TextBox ID="txtFirstName" TabIndex="2" runat="server"></asp:TextBox> <span id="infoFirstName" class="msg-warning">*</span>
            <br />
            <b>Efternamn:</b><br />
            <asp:TextBox ID="txtLastName" TabIndex="3" runat="server"></asp:TextBox> <span id="infoLastName" class="msg-warning">*</span>
            <br />
              <b>Mobiltelefon:</b><br />
              <asp:TextBox ID="txtMobilePhone" TabIndex="4" runat="server"></asp:TextBox> <span id="infoMobilePhone" class="msg-warning">*</span>
            <!--
            <b>Företag:</b><br />
            <%-- <asp:TextBox ID="txtCompany" TabIndex="4" runat="server"></asp:TextBox>--%>
            -->
            </span>
          </td>
        </tr>
        </table>
       </td>
       <td>
         <h1>Info</h1>
          <p id="saveInfoMessage"></p>
          <p id="serverMessageText"><asp:Literal ID="litServerMessage" runat="server"></asp:Literal></p>
          
          <!-- TODO: List all found customers here if found more than 1 -->
          <div class="foundcustomers-wrapper">
            <table id="foundCustomers" class="display">
              <thead><tr><th>Kundnummer</th><th>Förnamn</th><th>Efternamn</th><th>Företag</th><th>E-post</th><th>Mobil</th></tr></thead>
              <tbody>
              </tbody>
            </table>
          </div>
       </td>
        </tr>
        </table>
        <!-- Chosen values -->
        <asp:HiddenField ID="hiddenCusnoForSearchCriteria" runat="server"/>
        <asp:HiddenField ID="txtServicePlusExternalSubscriberId" runat="server"/>
        <asp:HiddenField ID="txtServicePlusId" runat="server"/>
        <asp:HiddenField ID="txtCampId" runat="server"/>
        <asp:HiddenField ID="txtPaperCode" runat="server"/>
        <asp:HiddenField ID="txtProductNo" runat="server"/>
        <asp:HiddenField ID="txtCusno" runat="server"/>
        <asp:HiddenField ID="txtTargetGroup" runat="server"/>
        
        <div id="campaignlistContainer">
        <br /><br />
        <h1>Välj en av nedanstående "Prova-på"-kampanjer</h1>
        <asp:Repeater ID="rptCampaigns" runat="server">
          <HeaderTemplate>
            <table id="campaignTable" class="display">
          <thead>
        <tr>
            <th>CampNo</th>
            <th>CampId</th>
            <th>Name</th>
            <th>PaperCode</th>
            <th>ProductNo</th>
            <th>StartDate</th>
            <th>EndDate</th>
            <!--
            <th>PerDisc</th>
            <th>StandDisc</th>
            <th>Disc %</th>
            <th>Tot price</th>
            -->
        </tr>
        </thead>
        <tbody>
          </HeaderTemplate>
          <ItemTemplate>
            <tr class="tr-pointer">
              <td><%#Eval("CampNo")%></td>
              <td class="CampId"><%#Eval("CampId")%></td>
              <td><%#Eval("CampName")%></td>
              <td class="PaperCode"><%#Eval("PaperCode")%></td>
              <td class="ProductNo"><%#Eval("ProductNo")%></td>
              <td><%#string.Format("{0:yyyy-MM-dd}", Eval("CampStartDate"))%></td>
              <td><%#string.Format("{0:yyyy-MM-dd}", Eval("CampEndDate"))%></td>
              <!--
              <td><%#Eval("PerDiscount")%></td>
              <td><%#Eval("StandDiscount")%></td>
              <td><%#Eval("Discpercent")%></td>
              <td><%#Eval("TotalPrice")%></td>
              -->
            </tr>
          </ItemTemplate>
          <FooterTemplate>
            </tbody>
            </table>
          </FooterTemplate>
        </asp:Repeater>
        
        </div>
        <div style="text-align:center;"><asp:Button ID="btnSubmit" Text=" SLUTFÖR " runat="server"/></div>
      </div>
    </form>
    
<script type="text/javascript">
  $(document).ready(function() {
    // Init variables
    var mobilePhoneInputFieldMandatory = false;
    var servicePlusSearchRunningState = 0;
    var cirixSearchRunningState = 0;
    var emailIsValid = false;
    var foundCustomersDataTable;
    var emailCheckDone = false; //Set to false as soon as you want force user to re-check email, before saving subscription!
    var telemarketingHelper = new Telemarketing('waitOverlay');
    var productConfig;
    telemarketingHelper.GetProducts(function(returnData) {
      productConfig = returnData.ProductConfig;
      },
      function(jqXHR, txtStatus) {
        $waitOverlay.attr('style', 'display:none');
        alert('Målgruppskoderna kunde ej laddas in. Vänligen kontakta utvecklingsteamet.');
        console.log('Ett fel uppstod. ' + jqXHR + ' ' + txtStatus);
      });

    var $waitOverlay = $('#waitOverlay');
    var $emailMessage = $('#emailMessage');
    var $saveInfoMessage = $('#saveInfoMessage');
    var $serverMessageText = $('#serverMessageText');
    var $btnEmailControl = $('#btnEmailControl');
    var $btnCheckCirix = $('#btnCheckCirix');
    var $firstNameInputField = $('#<%=txtFirstName.ClientID  %>');
    var $lastNameInputField = $('#<%=txtLastName.ClientID  %>');
    //var $companyInputField = $('#<%--txtCompany.ClientID --%>');
    var $mobilePhoneInputField = $('#<%=txtMobilePhone.ClientID %>');
    var $emailInputField = $('#<%=txtEmail.ClientID %>');
    var $hiddenCusnoForSearchCriteria = $('#<%=hiddenCusnoForSearchCriteria.ClientID %>');
    var $cusnoForSearchCriteriaInfo = $('#cusnoForSearchCriteriaInfo');
    var $servicePlusIdChosen = $('#<%=txtServicePlusId.ClientID %>');
    var $servicePlusExternalSubscriberId = $('#<%=txtServicePlusExternalSubscriberId.ClientID %>');
    var $campIdChosen = $('#<%=txtCampId.ClientID %>');
    var $paperCodeChosen = $('#<%=txtPaperCode.ClientID %>');
    var $productNoChosen = $('#<%=txtProductNo.ClientID %>');
    var $cusNoChosen = $('#<%=txtCusno.ClientID %>');
    var $targetGroupChosen = $('#<%=txtTargetGroup.ClientID %>');
    var $btnSubmit = $('#<%=btnSubmit.ClientID %>');
    var $foundCustomersTable = $('#foundCustomers');
    var $foundcustomersWrapper = $('.foundcustomers-wrapper');
    var $campaignlistContainer = $('#campaignlistContainer');
    var $extendedInputFields = $('#extendedInputFields');
    var $infoFirstName = $('#infoFirstName');
    var $infoLastName = $('#infoLastName');
    var $infoMobilePhone = $('#infoMobilePhone');

    $emailInputField.focus();
    $btnSubmit.attr('disabled', 'disabled'); //Init submit button as disabled on load. Enables through method setSubmitButtonState()

    //------ Page methods ----------------------------------------------------
    var writeSaveInfo = function() {
      var message = "";
      if (isValidSearchCusno() && emailCheckDone && emailIsValid) {
        if ($cusNoChosen.val()) {
          message += "<div>Kund med kundnummer <b>" + $cusNoChosen.val() + "</b> i prenumerationsdatabasen kommer uppdateras.</div>";
        } else {
          message += "<div>Ny kund kommer skapas i prenumerationsdatabasen.</div>";
        }
        if ($servicePlusIdChosen.val()) {
          message += "<div>Kund med Di-konto kommer uppdateras. (UserId:<b>" + $servicePlusIdChosen.val() + "</b>)</div>";
        } else {
          message += "<div>Nytt Di-konto för kund kommer skapas.</div>";
        }
      }
      $saveInfoMessage.append(message);
    }

    var setSubmitButtonState = function (forceDisabled) {
      var check = compareCusnos();
      var mobileCheck = !mobilePhoneInputFieldMandatory || $mobilePhoneInputField.val();
      var nameAndCompanyOk = ($firstNameInputField.val() && $lastNameInputField.val() && mobileCheck) ? true : false;
      if (forceDisabled == undefined && check && emailCheckDone && $emailInputField.val() && nameAndCompanyOk && $campIdChosen.val() && $paperCodeChosen.val() && $productNoChosen.val()) {
        $btnSubmit.removeAttr('disabled');
      } else {
        $btnSubmit.attr('disabled', 'disabled');
      }
      writeNameFieldRequiredInfo();
    }

    var writeNameFieldRequiredInfo = function() {
      //Only for new customer, and not if updating a customer that does not have first/last name filled
      if ($cusNoChosen.val()) {
        $infoFirstName.hide();
        $infoLastName.hide();
        $infoMobilePhone.hide();
        return;
      }
      if ($firstNameInputField.val()) {
        $infoFirstName.hide();
      } else {
        $infoFirstName.show();
      }
      if ($lastNameInputField.val()) {
        $infoLastName.hide();
      } else {
        $infoLastName.show();
      }
      //$infoMobilePhone
      if (!mobilePhoneInputFieldMandatory || $mobilePhoneInputField.val()) {
        $infoMobilePhone.hide();
      } else {
        $infoMobilePhone.show();
      }
    };
    
    var setFieldsReadOnlyState = function(clear) {
      if (clear) {
        $firstNameInputField.removeAttr('readonly');
        $lastNameInputField.removeAttr('readonly');
        //$companyInputField.removeAttr('readonly');
        $mobilePhoneInputField.removeAttr('readonly');
        mobilePhoneInputFieldMandatory = true;
      } else {
        $firstNameInputField.attr('readonly', 'readonly');
        $lastNameInputField.attr('readonly', 'readonly');
        //$companyInputField.attr('readonly', 'readonly');
        $mobilePhoneInputField.attr('readonly', 'readonly');
        mobilePhoneInputFieldMandatory = false;
      }
    };

    var writeCusnoForSearchCriteriaInfo = function () {
      if (!isValidSearchCusno()) {
        $cusnoForSearchCriteriaInfo.html('Fel bland kundnumren i Di-kontot! Kan ej söka kundnummer i prenumerationsdatabasen.');
      } else if ($hiddenCusnoForSearchCriteria.val() != '0') {
        $cusnoForSearchCriteriaInfo.html('Sökning i prenumerationsdatabasen med kundnummer:<b>' + $hiddenCusnoForSearchCriteria.val() + '</b>.');
      }
    };

    var isValidSearchCusno = function() {
      return $hiddenCusnoForSearchCriteria.val() != '-1' && $servicePlusExternalSubscriberId.val() != '-1';
    };

    var startCirixSearch = function () {
      cirixSearchRunningState = 1;
      console.log('START CIRIX SEARCH WITH: ' + $hiddenCusnoForSearchCriteria.val());
      if (isValidSearchCusno()) {
        telemarketingHelper.FindCustomerInCirix($hiddenCusnoForSearchCriteria.val(), '', '', '', $emailInputField.val(), findCustomerInCirixReturn, errorResponse);
      } else {
        cirixSearchRunningState = 0;
        $extendedInputFields.fadeOut('slow');
        doAfterAllSearch(); //Extra call needed to remove overlay
      }
    };

    var startSearch = function(searchOnlyCirix) {
      emailCheckDone = true;
      $emailMessage.html('');
      $campaignlistContainer.fadeOut('slow');
      $foundcustomersWrapper.fadeOut('slow');
      $waitOverlay.attr('style', 'display:block');
      if (searchOnlyCirix) {
        startCirixSearch();
      } else {
        servicePlusSearchRunningState = 1;
        telemarketingHelper.CheckEmailAtServicePlus($emailInputField.val(), checkEmailAtServicePlusReturn, errorResponse);
      }
      //$extendedInputFields.fadeIn('slow');
    };

    //Accept parameter of type returnData.CirixCustomerSearchResult.CirixCustomers
    var buildFoundCirixCustomersTable = function(cirixCustomers) {
      if (foundCustomersDataTable != undefined) {
        foundCustomersDataTable.fnClearTable();
        foundCustomersDataTable.fnDestroy();
      }
      $.each(cirixCustomers, function(idx, cirixCustomerObj) {
        $foundcustomersWrapper.fadeIn('slow');
        $foundCustomersTable.find('tbody').append('<tr class="tr-pointer"><td class="js-Cusno">' + cirixCustomerObj.Cusno + '</td><td class="js-FirstName">' + cirixCustomerObj.FirstName + '</td><td class="js-LastName">' + cirixCustomerObj.LastName + '</td><td class="js-Company">' + cirixCustomerObj.Company + '</td><td class="js-Email">' + cirixCustomerObj.Email + '</td><td class="js-MobilePhone">' + cirixCustomerObj.MobilePhone + '</td></tr>');
      });
      activateFoundCustomersTable();
    }

    var activateFoundCustomersTable = function() {
      foundCustomersDataTable = $foundCustomersTable.dataTable({
        "paginate": true,
        "language": {
          "lengthMenu": "Visa _MENU_ poster per sida",
          "zeroRecords": "Ingen träff",
          "info": "Visar sida _PAGE_ av _PAGES_",
          "infoEmpty": "Inga rader",
          "infoFiltered": "(filtrerad från _MAX_ antal poster)",
          "search": "Sök",
          "paginate": {
            "first": "Första",
            "last": "Sista",
            "next": "Nästa",
            "previous": "Föregående"
          }
        }
      });
      $foundCustomersTable.find('tbody').on('click', 'tr', function() {
        $serverMessageText.html('');
        if ($(this).hasClass('selected')) {
          $(this).removeClass('selected');
          setSubmitButtonState();
        } else {
          foundCustomersDataTable.$('tr.selected').removeClass('selected');
          $(this).addClass('selected');
          // Set selected customer!
          $hiddenCusnoForSearchCriteria.val($(this).find('.js-Cusno').text());
          writeCusnoForSearchCriteriaInfo();
          $firstNameInputField.val($(this).find('.js-FirstName').text());
          $lastNameInputField.val($(this).find('.js-LastName').text());
          //$companyInputField.val($(this).find('.js-Company').text());
          $mobilePhoneInputField.val($(this).find('.js-MobilePhone').text());
          setSubmitButtonState();
          $saveInfoMessage.html('');
          startSearch(true);
        }
      });
    };

    var setTargetGroup = function(paperCode, productNo) {
      //Find chosen product's targetgroup code in config
      $.each(productConfig, function (idx, obj) {
        //console.log('Kolla prodConf ' + obj.PaperCode + ' ' + obj.ProductNo);
        if (obj.PaperCode == paperCode && obj.ProductNo == productNo) {
          $targetGroupChosen.val(obj.TargetGroup);
          return false;//Break out of loop
        }
        return true; //Continue loop
      });
      
    }

    /* -----------------------------------------------------------------------------
    *  --- Binding events to objects ----------------------------------------------------
    *  -----------------------------------------------------------------------------
    */
    $btnSubmit.bind('click', function() {
      $waitOverlay.attr('style', 'display:block');
    });

    // Init button to find customer first in S+ and try use cusno from there to search in Cirix
    $btnEmailControl.click(function() {
      $saveInfoMessage.html('');
      $serverMessageText.html('');
      startSearch(false);
    });

    // Init email input field check
    $emailInputField.bind('keyup', function() {
      $servicePlusIdChosen.val('');
      $cusNoChosen.val('');
      $saveInfoMessage.html('');
      $hiddenCusnoForSearchCriteria.val('0');
      $cusnoForSearchCriteriaInfo.html('');
      $firstNameInputField.val('');
      $lastNameInputField.val('');
      //$companyInputField.val('');
      $mobilePhoneInputField.val('');
      $extendedInputFields.fadeOut('slow');
      emailCheckDone = false;
      setFieldsReadOnlyState(true);
      $foundCustomersTable.find('tbody').empty();
      $foundcustomersWrapper.fadeOut('slow');
      if (telemarketingHelper.isValidEmailAddress($(this).val())) {
        emailIsValid = true;
        $emailMessage.html('');
        $btnEmailControl.removeAttr('disabled');
        $btnCheckCirix.removeAttr('disabled');
        setSubmitButtonState();
      } else {
        emailIsValid = false;
        $emailMessage.html('<div class="msg-warning">E-postadressen felformatterad</div>');
        $btnEmailControl.attr('disabled', 'disabled');
        $btnCheckCirix.attr('disabled', 'disabled');
        setSubmitButtonState(true);
      }
    });

    $firstNameInputField.bind('keyup blur click', function() {
      setSubmitButtonState();
    });
    $lastNameInputField.bind('keyup blur click', function() {
      setSubmitButtonState();
    });
//    $companyInputField.bind('keyup blur click', function() {
//      setSubmitButtonState();
    //    });
    $mobilePhoneInputField.bind('keyup blur click', function () {
      setSubmitButtonState();
    });


    /* -----------------------------------------------------------------------------
     * Page Ajax-callback methods
     * -----------------------------------------------------------------------------
     */
    var checkEmailAtServicePlusReturn = function(returnData) {
      servicePlusSearchRunningState = 0;
      $servicePlusExternalSubscriberId.val(returnData.ExternalSubscriberId);
      $hiddenCusnoForSearchCriteria.val(returnData.ExternalSubscriberId); //Include found cusno in FindCustomerInCirix() call below

      if (returnData.EmailExistAtServicePlus) {
        $saveInfoMessage.append('<div>Di-konto med e-postadressen fanns.</div>');
        $servicePlusIdChosen.val(returnData.ServicePlusUserId);
        writeCusnoForSearchCriteriaInfo();
      } else {
        $saveInfoMessage.append('<div>Inget Di-konto med den e-postadressen hittades.</div>');
        $servicePlusIdChosen.val('');
      }
      startCirixSearch();
      doAfterAllSearch();
      setSubmitButtonState();
    }

    var findCustomerInCirixReturn = function(returnData) {
      cirixSearchRunningState = 0;
      $extendedInputFields.fadeIn('slow');
      if (returnData.CirixCustomerSearchResult.CirixCustomerSearchCount > 1) {
        emailCheckDone = false;
        setFieldsReadOnlyState();
        $campaignlistContainer.fadeOut('slow');
        $saveInfoMessage.append('<div class="msg-warning">' + returnData.CirixCustomerSearchResult.CirixCustomerSearchCount + ' träffar i prenumerationsdatabasen.<br />Markera korrekt kund i listan.</div>');
        $cusNoChosen.val('');
        setSubmitButtonState(true);
        buildFoundCirixCustomersTable(returnData.CirixCustomerSearchResult.CirixCustomers);

      } else if (returnData.CirixCustomerSearchResult.CirixCustomerSearchCount == 0) {
        $saveInfoMessage.append('<div>Ingen träff i prenumerationsdatabasen!</div>');
        $cusNoChosen.val('');
        $campaignlistContainer.fadeIn('slow');

      } else {
        var diffEmailMess = returnData.CirixCustomerSearchResult.CirixCustomer.Email.toLowerCase() != $emailInputField.val().toLowerCase() ? '<br />Kund har e-postadress <b>' + returnData.CirixCustomerSearchResult.CirixCustomer.Email.toLowerCase() + '</b> i prenumerationsdatabasen.' : '';
        $saveInfoMessage.append('<div class="msg-ok">Kund "' + returnData.CirixCustomerSearchResult.CirixCustomer.FirstName + ' ' + returnData.CirixCustomerSearchResult.CirixCustomer.LastName + '" matchad mot prenumerationsdatabasen! ' + diffEmailMess + '</div>');
        $firstNameInputField.val(returnData.CirixCustomerSearchResult.CirixCustomer.FirstName);
        $lastNameInputField.val(returnData.CirixCustomerSearchResult.CirixCustomer.LastName);
        //$companyInputField.val(returnData.CirixCustomerSearchResult.CirixCustomer.Company);
        $mobilePhoneInputField.val(returnData.CirixCustomerSearchResult.CirixCustomer.MobilePhone);
        $cusNoChosen.val(returnData.CirixCustomerSearchResult.CirixCustomer.Cusno);
        setFieldsReadOnlyState();
        $campaignlistContainer.fadeIn('slow');
      }
      doAfterAllSearch();
      setSubmitButtonState();
    };

    var doAfterAllSearch = function() {
      if (servicePlusSearchRunningState == 0 && cirixSearchRunningState == 0) {
        $waitOverlay.attr('style', 'display:none');
        writeSaveInfo();
      }
    }

    var compareCusnos = function() {
      var check = true;
      if ($servicePlusExternalSubscriberId.val() && $cusNoChosen.val() && $servicePlusExternalSubscriberId.val() != '0' && $servicePlusExternalSubscriberId.val() != $cusNoChosen.val()) {
        check = false;
        alert('OBS! Kundnumret ifrån Di-kontot stämmer ej med det valda kundnumret i prenumerationsdatabasen!');
      }
      return check;
    };

    var errorResponse = function(jqXHR, txtStatus) {
      $waitOverlay.attr('style', 'display:none');
      alert('Ett tekniskt fel uppstod. Kontakta utvecklingsteamet.');
      console.log('Ett fel uppstod. ' + jqXHR + ' ' + txtStatus);
      servicePlusSearchRunningState = 0;
      cirixSearchRunningState = 0;
    };
    // --- END Page Ajax-callback methods -----


    // Init table designs with datatables.net script
    var campaignTable = $('#campaignTable').dataTable({
      paginate: true,
      language: {
        "lengthMenu": "Visa _MENU_ poster per sida",
        "zeroRecords": "Ingen träff",
        "info": "Visar sida _PAGE_ av _PAGES_",
        "infoEmpty": "Inga rader",
        "infoFiltered": "(filtrerad från _MAX_ antal poster)",
        "search": "Sök",
        "paginate": {
          "first": "Första",
          "last": "Sista",
          "next": "Nästa",
          "previous": "Föregående"
        }
      }
    });
    $('#campaignTable tbody').on('click', 'tr', function() {
      if ($(this).hasClass('selected')) {
        $(this).removeClass('selected');
        $campIdChosen.val('');
        $paperCodeChosen.val('');
        $productNoChosen.val('');
        setSubmitButtonState();
      } else {
        campaignTable.$('tr.selected').removeClass('selected');
        $(this).addClass('selected');
        // Set selected campId!
        $campIdChosen.val($(this).find('.CampId').text());
        $paperCodeChosen.val($(this).find('.PaperCode').text());
        $productNoChosen.val($(this).find('.ProductNo').text());
        setTargetGroup($(this).find('.PaperCode').text(), $(this).find('.ProductNo').text());
        setSubmitButtonState();
      }
    });

    activateFoundCustomersTable();
  });
</script>

</body>
</html>
