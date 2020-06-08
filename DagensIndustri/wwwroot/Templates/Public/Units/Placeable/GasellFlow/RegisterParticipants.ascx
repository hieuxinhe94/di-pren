<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RegisterParticipants.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.GasellFlow.RegisterParticipants" %>
<%@ Register TagPrefix="DI" TagName="Input" Src="~/Tools/Classes/WebControls/InputWithValidation.ascx" %>

<div class="form-box">
  							
	<!-- Participant -->
	<div class="section" id="form-street">
		<div class="row">
			<div class="col">
                <DI:Input ID="FirstNameInput" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" TypeOfInput="Text" Name="firstname" Title='<%# Translate("/gasell/flow/forms/firstname") %>' DisplayMessage='<%# Translate("/gasell/flow/forms/firstname.message") %>' runat="server" />
			</div>
			<div class="col">
				<DI:Input ID="LastNameInput" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" TypeOfInput="Text" Name="lastname" Title='<%# Translate("/gasell/flow/forms/lastname") %>' DisplayMessage='<%# Translate("/gasell/flow/forms/lastname.message") %>' runat="server" />
			</div>						
		</div>
					
		<div class="row">
			<div class="col">
				<DI:Input ID="TitleInput" CssClass="text" StripHtml="true" AutoComplete="true" TypeOfInput="Text" Name="title" Title='<%# Translate("/gasell/flow/forms/title") %>' runat="server" />
			</div>
			<div class="col">
				<DI:Input ID="CompanyInput" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" TypeOfInput="Text" Name="comapny" Title='<%# Translate("/gasell/flow/forms/company") %>'  DisplayMessage='<%# Translate("/gasell/flow/forms/company.message") %>' runat="server" />
			</div>
		</div>						
					
		<div class="row">
			<div class="col">
                <DI:Input ID="StreetInput" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" TypeOfInput="Text" Name="street" Title='<%# Translate("/gasell/flow/forms/address") %>'  DisplayMessage='<%# Translate("/gasell/flow/forms/address.message") %>' runat="server" />
			</div>
			<div class="col">
				<div class="small">
                    <DI:Input ID="StreetNumberInput" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" TypeOfInput="Text" Name="number" Title='<%# Translate("/gasell/flow/forms/addressnumber") %>'  DisplayMessage='<%# Translate("/gasell/flow/forms/addressnumber.message") %>' runat="server" />
				</div>
			</div>					
		</div>	
					
		<div class="row">
			<div class="col">
				<div class="small">
					<DI:Input ID="ZipCodeInput" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" Name="zip" TypeOfInput="Numeric" Title='<%# Translate("/gasell/flow/forms/zip") %>' MaxValue="6" DisplayMessage='<%# Translate("/gasell/flow/forms/zip.message") %>' runat="server" />
				</div>
				<div class="medium">
					<DI:Input ID="StateInput" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" Name="state" TypeOfInput="Text" Title='<%# Translate("/gasell/flow/forms/city") %>' DisplayMessage='<%# Translate("/gasell/flow/forms/city.message") %>' runat="server" />
				</div>							
			</div>	
			<div class="col">
				<DI:Input ID="TelephoneInput" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" Name="phone" TypeOfInput="Telephone" Title='<%# Translate("/gasell/flow/forms/phone") %>'  DisplayMessage='<%# Translate("/gasell/flow/forms/phone.message") %>'  runat="server" />
			</div>
		</div>
					
		<div class="row">
			<div class="col">
				<DI:Input ID="EmailInput" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" Name="mail" TypeOfInput="Email" Title='<%# Translate("/gasell/flow/forms/mail") %>'   DisplayMessage='<%# Translate("/gasell/flow/forms/mail.message") %>'  runat="server" />
			</div>
			<div class="col">
                <span class="checkbox"> 
                    <asp:CheckBox ID="GasellCompanyCheckBox" Text='<%# Translate("/gasell/flow/forms/gasellcompanystrong") %>' runat="server" />
                </span>
			</div>						
		</div>
					
		<div class="row">
			<div class="col">
                <DI:Input ID="BranchInput" CssClass="text" StripHtml="true" AutoComplete="true" Name="email" TypeOfInput="Text" Title='<%# Translate("/gasell/flow/forms/branch") %>' runat="server" />
			</div>
			<div class="col">
				<div class="small">
                    <DI:Input ID="EmployeesInput" CssClass="text" StripHtml="true" AutoComplete="true" Name="employees" TypeOfInput="Text" Title='<%# Translate("/gasell/flow/forms/employeesnumber") %>' runat="server" />
				</div>
			</div>
		</div>																	
	</div>
	<!-- // Participant -->			
</div>
