<%@ Page Language="C#" AutoEventWireup="False" CodeBehind="CompetitionQuestion.aspx.cs"
    Inherits="DagensIndustri.Templates.Public.Pages.Competition.CompetitionQuestion"  MasterPageFile="~/Templates/Public/MasterPages/MasterPage.Master"%>

<%@ Register TagPrefix="DI" TagName="Input" Src="~/Tools/Classes/WebControls/InputWithValidation.ascx" %>

<%@ Import Namespace="EPiServer.Core" %>
<%@ Import Namespace="DIClassLib.Competition" %>


<asp:Content ContentPlaceHolderID="head" runat="server">

    <link rel="stylesheet" type="text/css" media="screen" href="/Templates/Public/Styles/competition/competition.css"  />    

</asp:Content>


<asp:Content  ContentPlaceHolderID="FullRegion" runat="server">
                       
    <div id="compwrapper" class="clearfix">

        <div id="leftarea">
            
            <EPiServer:Property runat="server" PropertyName="Question" />
            
            <asp:PlaceHolder runat="server" Visible='<%# CurrentPage["Answers"] != null %>'>
                <asp:RadioButtonList runat="server" ID="RblAnswers" RepeatDirection="Horizontal" DataSource="<%# CompetitionUtility.GetAnswers(CurrentPage) %>" ></asp:RadioButtonList>
            </asp:PlaceHolder>
            <asp:PlaceHolder runat="server" Visible='<%# CurrentPage["Answers"] == null %>'>
                <asp:TextBox runat="server" ID="TxtAnswer"></asp:TextBox>
            </asp:PlaceHolder>     
        </div>
    </div>
        
</asp:Content>
