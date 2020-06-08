<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SubTest.aspx.cs" Inherits="DagensIndustri.SubTest" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
<form id="form1" runat="server">
<asp:placeholder ID="PlaceholderTestGui" Visible="false" runat="server">
<table>
<tr>
<td valign="top" width="420">

    <b>Subscription</b><br/>

    * CampId: <asp:TextBox ID="TextBoxSCampId" runat="server"></asp:TextBox><br/>
    TargetGroup: <asp:TextBox ID="TextBoxSTargetGroup" runat="server"></asp:TextBox><br/>
    SubscriberAddressIsRequired: <asp:CheckBox ID="CheckBoxSSubscriberAddressIsRequired" runat="server" /><br/>
    * PayMethod: 
    <asp:DropDownList ID="DropDownListSPayMet" runat="server">
      <asp:ListItem Text="CreditCard" Value="CreditCard"></asp:ListItem>
      <asp:ListItem Text="CreditCardAutowithdrawal" Value="CreditCardAutowithdrawal"></asp:ListItem>
      <asp:ListItem Text="DirectDebit" Value="DirectDebit"></asp:ListItem>
      <asp:ListItem Text="DirectDebitOtherPayer" Value="DirectDebitOtherPayer"></asp:ListItem>
      <asp:ListItem Text="Invoice" Value="Invoice" Selected="True"></asp:ListItem>
      <asp:ListItem Text="InvoiceOtherPayer" Value="InvoiceOtherPayer"></asp:ListItem>
    </asp:DropDownList><br/>
    
    WantedStartDate (YYYY-MM-DD): <asp:TextBox ID="TextBoxSWantedSStartDate" runat="server"></asp:TextBox><br/>
    ServicePlusToken <asp:TextBox ID="TextBoxSServicePlusToken" runat="server"></asp:TextBox><br/>
    ServicePlusUserId: <asp:TextBox ID="TextBoxSServicePlusUserId" runat="server"></asp:TextBox><br/>
    CardPayCustRefno: <asp:TextBox ID="TextBoxSCardPayCustRefno" runat="server"></asp:TextBox><br/>
    
    <hr/>
    <b>Subscriber</b><br/>
    * FirstName: <asp:TextBox ID="TextBoxSubFirstName" runat="server"></asp:TextBox><br/>
    * LastName: <asp:TextBox ID="TextBoxSubLastName" runat="server"></asp:TextBox><br/>
    * Email: <asp:TextBox ID="TextBoxSubEmail" runat="server"></asp:TextBox><br/>
    MobilePhone: <asp:TextBox ID="TextBoxSubMobilePhone" runat="server"></asp:TextBox><br/>
    Company: <asp:TextBox ID="TextBoxSubCompany" runat="server"></asp:TextBox><br/>
    CareOf: <asp:TextBox ID="TextBoxSubCareOf" runat="server"></asp:TextBox><br/>
    StreetName: <asp:TextBox ID="TextBoxSubStreetName" runat="server"></asp:TextBox><br/>
    HouseNo: <asp:TextBox ID="TextBoxSubHouseNo" runat="server"></asp:TextBox><br/>
    StairCase: <asp:TextBox ID="TextBoxSubStairCase" runat="server"></asp:TextBox><br/>
    ZipCode: <asp:TextBox ID="TextBoxSubZipCode" runat="server"></asp:TextBox><br/>
    City: <asp:TextBox ID="TextBoxSubCity" runat="server"></asp:TextBox><br/>

    <hr/>
    <b>Payer (not mandatory)</b><br/>
    PhoneDayTime: <asp:TextBox ID="TextBoxPayPhoneDayTime" runat="server"></asp:TextBox><br/>
    Company: <asp:TextBox ID="TextBoxPayCompany" runat="server"></asp:TextBox><br/>
    CareOf: <asp:TextBox ID="TextBoxPayCareOf" runat="server"></asp:TextBox><br/>
    Attention: <asp:TextBox ID="TextBoxPayAttention" runat="server"></asp:TextBox><br/>
    CompanyNo: <asp:TextBox ID="TextBoxPayCompanyNo" runat="server"></asp:TextBox><br/>
    StreetName: <asp:TextBox ID="TextBoxPayStreetName" runat="server"></asp:TextBox><br/>
    HouseNo: <asp:TextBox ID="TextBoxPayHouseNo" runat="server"></asp:TextBox><br/>
    ZipCode: <asp:TextBox ID="TextBoxPayZipCode" runat="server"></asp:TextBox><br/>
    City: <asp:TextBox ID="TextBoxPayCity" runat="server"></asp:TextBox><br/>
    
    <hr/>
      <asp:Button ID="ButtonSubmit" runat="server" Text="Submit" onclick="ButtonSubmit_Click" />

</td>
<td valign="top">
  <asp:Label ID="LabelResult" runat="server"></asp:Label> 
</td>
</tr>
</table>
</asp:placeholder>
</form>
</body>
</html>
