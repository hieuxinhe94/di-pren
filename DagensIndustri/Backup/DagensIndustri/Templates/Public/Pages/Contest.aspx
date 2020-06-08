<%@ Page Title="" Language="C#" MasterPageFile="~/Templates/Public/MasterPages/MasterPage.Master" AutoEventWireup="true" CodeBehind="Contest.aspx.cs" Inherits="DagensIndustri.Templates.Public.Pages.Contest" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="HeadingContentPlaceHolder" runat="server">
<h1><EPiServer:Property ID="PageName" PropertyName="PageName" runat="server" /></h1>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">


    <asp:MultiView ID="ContestMultiView" runat="server" ActiveViewIndex="0">

        <asp:View ID="QuestionsView" runat="server">
            <p class="intro">
                <%#CurrentPage["ContestIntro"] %>
            </p>

            <div class="form-box">
            	<!-- Server errors -->
				<div class="server-error" runat="server" id="server_error_div" visible='<%#!String.IsNullOrEmpty(ErrorMessage) %>'>
					<%#ErrorMessage %>
				</div>
				<!-- // Server errors -->


                <asp:Repeater ID="QuestionRepeater" runat="server" DataSource='<%#Questions %>'>
                <ItemTemplate>
                    <div class="contest_question">
                        <asp:PlaceHolder ID="AlternativePlaceHolder" runat="server" Visible='<%#((DIClassLib.Contest.QuestionType)Eval("QuestionType")) == DIClassLib.Contest.QuestionType.Alternatives %>'>
                            <div class="row radiolist">
                                <h4><%#Eval("QuestionText") %></h4>
                                <ul>
                                    <asp:Repeater ID="AlternativesRepeater" runat="server" DataSource='<%#((DIClassLib.Contest.ContestQuestion)Container.DataItem).Alternatives %>'>
                                        <ItemTemplate>  
                                            <li>
								  		        <label for="alt_<%#Eval("Id") %>">
								  			        <input type="radio" name="<%#Eval("FormInputName") %>" id="alt_<%#Eval("Id") %>" value="<%#Container.ItemIndex+1 %>" <asp:PlaceHolder ID="phSelected" runat="server" Visible='<%#(bool)Eval("IsSelected") %>'>checked="checked"</asp:PlaceHolder> />
								  			        <span><%#Eval("Text") %></span>
								  		        </label>
								  	        </li>         
                                        </ItemTemplate>                             
                                    </asp:Repeater>
                                </ul>
                            </div>
                        </asp:PlaceHolder>
                        <asp:PlaceHolder ID="TextPlaceHolder" runat="server" Visible='<%#((DIClassLib.Contest.QuestionType)Eval("QuestionType")) == DIClassLib.Contest.QuestionType.Text %>'>
                            <div class="row textarea">
                                <label for="text_<%#Eval("Id") %>"><%#Eval("QuestionText") %></label>
								<textarea required="required" name="<%#Eval("FormInputName") %>" id="text_<%#Eval("Id") %>"><%#Eval("Answer") %></textarea>
                            </div>
                        </asp:PlaceHolder>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
             <div class="button-wrapper">
                <asp:Button ID="SubmitContestButton" runat="server" Text="Skicka" OnClick="SubmitContestButton_Click" />
             </div>
            </div>
        </asp:View>
        <asp:View ID="ThankYouView" runat="server">
            <div class="contest_thankyou_wrapper">
                <%#CurrentPage["ContestThankYou"] %>
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>


