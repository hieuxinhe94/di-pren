<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RegisterParticipants.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.SignUpFlow.RegisterParticipants" %>
<%@ Register TagPrefix="DI" TagName="Input" Src="~/Tools/Classes/WebControls/InputWithValidation.ascx" %>

<div class="form-box">
  							
	<!-- Participant -->
	<div class="section" id="form-street">
		<div class="row">
			<div class="col">
                <DI:Input ID="FirstNameInput" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" TypeOfInput="Text" Name="firstname" Title='<%# Translate("/signup/flow/forms/firstname") %>' DisplayMessage='<%# Translate("/signup/flow/forms/firstname.message") %>' runat="server" />
			</div>
			<div class="col">
				<DI:Input ID="LastNameInput" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" TypeOfInput="Text" Name="lastname" Title='<%# Translate("/signup/flow/forms/lastname") %>' DisplayMessage='<%# Translate("/signup/flow/forms/lastname.message") %>' runat="server" />
			</div>						
		</div>

        <div class="row">
            <div class="col">
                <DI:Input ID="StreetInput" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" TypeOfInput="Text" Name="street" Title='<%# Translate("/signup/flow/forms/address") %>'  DisplayMessage='<%# Translate("/signup/flow/forms/address.message") %>' runat="server" />
            </div>
            <div class="col">
                <div class="small">
					<DI:Input ID="ZipCodeInput" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" Name="zip" TypeOfInput="Numeric" Title='<%# Translate("/signup/flow/forms/zip") %>' MaxValue="6" DisplayMessage='<%# Translate("/signup/flow/forms/zip.message") %>' runat="server" />
				</div>
				<div class="medium">
					<DI:Input ID="StateInput" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" Name="state" TypeOfInput="Text" Title='<%# Translate("/signup/flow/forms/city") %>' DisplayMessage='<%# Translate("/signup/flow/forms/city.message") %>' runat="server" />
				</div>
			</div>					
		</div>

        <div class="row">
			<div class="col">
				<DI:Input ID="TelephoneInput" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" Name="phone" TypeOfInput="Telephone" Title='<%# Translate("/signup/flow/forms/phone") %>'  DisplayMessage='<%# Translate("/signup/flow/forms/phone.message") %>'  runat="server" />
			</div>
			<div class="col">
                <DI:Input ID="EmailInput" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" Name="mail" TypeOfInput="Email" Title='<%# Translate("/signup/flow/forms/mail") %>'   DisplayMessage='<%# Translate("/signup/flow/forms/mail.message") %>'  runat="server" />
			</div>
		</div>
					
		<%--<div class="row">
			<div class="col">
				<DI:Input ID="CompanyInput" CssClass="text" StripHtml="true" AutoComplete="true" TypeOfInput="Text" Name="comapny" Title='<%# Translate("/signup/flow/forms/company") %>'  DisplayMessage='<%# Translate("/signup/flow/forms/company.message") %>' runat="server" />
			</div>
			<div class="col">
                <DI:Input ID="TitleInput" CssClass="text" StripHtml="true" AutoComplete="true" TypeOfInput="Text" Name="title" Title='<%# Translate("/signup/flow/forms/title") %>' runat="server" />
			</div>
		</div>						--%>
					
			
					
<%--		<div class="row">
			<div class="col">
				<div class="small">
                    <DI:Input ID="StreetNumberInput" CssClass="text" StripHtml="true" AutoComplete="true" TypeOfInput="Text" Name="number" Title='<%# Translate("/signup/flow/forms/addressnumber") %>'  DisplayMessage='<%# Translate("/signup/flow/forms/addressnumber.message") %>' runat="server" />
			    </div>							
			</div>	
			<div class="col">
				<DI:Input ID="TelephoneInput" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" Name="phone" TypeOfInput="Telephone" Title='<%# Translate("/signup/flow/forms/phone") %>'  DisplayMessage='<%# Translate("/signup/flow/forms/phone.message") %>'  runat="server" />
			</div>
		</div>
--%>					
		<%--<div class="row">
			<div class="col">
				<DI:Input ID="EmailInput" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" Name="mail" TypeOfInput="Email" Title='<%# Translate("/signup/flow/forms/mail") %>'   DisplayMessage='<%# Translate("/signup/flow/forms/mail.message") %>'  runat="server" />
			</div>
			<div class="col">
                <span class="checkbox"> 
                    <asp:CheckBox ID="GasellCompanyCheckBox" Text='<%# Translate("/signup/flow/forms/gasellcompanystrong") %>' runat="server" />
                </span>
			</div>
		</div>--%>
					
		<%--<div class="row">
			<div class="col">
                <DI:Input ID="BranchInput" CssClass="text" StripHtml="true" AutoComplete="true" Name="email" TypeOfInput="Text" Title='<%# Translate("/signup/flow/forms/branch") %>' runat="server" />
			</div>
			<div class="col">
				<div class="small">
                    <DI:Input ID="EmployeesInput" CssClass="text" StripHtml="true" AutoComplete="true" Name="employees" TypeOfInput="Text" Title='<%# Translate("/signup/flow/forms/employeesnumber") %>' runat="server" />
				</div>
			</div>
		</div>--%>																	
	</div>
	<!-- // Participant -->			
</div>
