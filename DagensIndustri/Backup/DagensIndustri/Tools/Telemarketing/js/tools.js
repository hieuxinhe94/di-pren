var Telemarketing = function() {

  // Validate email method
  this.isValidEmailAddress = function(emailEntered) {
    var pattern = new RegExp(/^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/);
    return emailEntered != '' && pattern.test(emailEntered);
  };


  // Check email at Service Plus
  this.CheckEmailAtServicePlus = function(email, methodOnSuccess, methodOnError) {
    $.ajax({
      dataType: "json",
      url: '/Handlers/Tools/AddSubscriptionsHandler.axd',
      data: { methodName: 'checkemailatserviceplus', email: email },
      type: "POST",
      success: function(telemarketingData, txtStatus, jqXHR) {
        methodOnSuccess(telemarketingData);
      },
      error: function(jqXHR, txtStatus) {
        methodOnError(jqXHR, txtStatus);
      }
    });
  };


  // Find/search customer in Cirix
  this.FindCustomerInCirix = function(cusno, firstName, lastName, company, email, methodOnSuccess, methodOnError) {
    if (cusno == '') {
      cusno = 0;
    }
    $.ajax({
      dataType: "json",
      url: '/Handlers/Tools/AddSubscriptionsHandler.axd',
      data: { methodName: 'findcustomer', cusno: cusno, firstname: firstName, lastname: lastName, company: company, email: email },
      type: "POST",
      success: function(telemarketingData, txtStatus, jqXHR) {
        methodOnSuccess(telemarketingData);
      },
      error: function(jqXHR, txtStatus) {
        methodOnError(jqXHR, txtStatus);
      }
    });

  };


  //Get product configuration
  this.GetProducts = function(methodOnSuccess, methodOnError) {
    $.ajax({
      dataType: "json",
      url: '/Handlers/Tools/AddSubscriptionsHandler.axd',
      data: { methodName: 'getproducts' },
      type: "POST",
      success: function(telemarketingData, txtStatus, jqXHR) {
        methodOnSuccess(telemarketingData);
      },
      error: function(jqXHR, txtStatus) {
        methodOnError(jqXHR, txtStatus);
      }
    });
  };

}

