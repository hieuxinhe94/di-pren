<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AppendixAdmin.aspx.cs" Inherits="DagensIndustri.Tools.Admin.Appendix.AppendixAdmin" %>
<%@ Register TagPrefix="DI" TagName="NotProcessed" Src="~/Tools/Admin/Appendix/NotProcessed.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Bilageadmin</title>
    <link href="/Tools/Admin/Styles/StyleSheetPublic.css" rel="stylesheet" type="text/css" />    
</head>
<body>
    <form id="form1" runat="server">
    
        <div id="MainBodyArea">
            <h1>Bilageadmin</h1>
            <div class="left">
            
                <asp:label id="LblError" EnableViewState="false" CssClass="error" runat="server"></asp:label>
                
                <asp:PlaceHolder runat="server" ID="PhAppendixList" Visible="true">
                            
                    <p class="blkArr">	                
	                    <asp:LinkButton runat="server" ID="LbShowAppendixAdd" OnClick="LbShowAppendixAddOnClick" Text="Lägg till ny bilaga"></asp:LinkButton>
	                </p>
        	        	        	        	       	        
	                <table class="appendixadminlist">
		                <asp:Repeater id="repeatBilagor" runat="server">
			                <HeaderTemplate>
				                <tr> 
					                <th>Rubrik</th>
					                <th>Utgivare</th>
					                <th>Ämne</th>
					                <th>Övrigt</th>
					                <th colspan="2">Publicerad</th>
				                </tr>
			                </HeaderTemplate>
			                <ItemTemplate>
				                <tr class='<%# Container.ItemIndex % 2 == 0 ? "evenrow" : "oddrow" %>'>
					                <td>
					                    <asp:LinkButton runat="server" ID="LbShowAppendixDetails" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' Text='<%# DataBinder.Eval(Container.DataItem, "headLine") %>' OnClick="LbShowAppendixDetailsOnClick"></asp:LinkButton>
					                </td>
					                <td><%# DataBinder.Eval(Container.DataItem, "publisher") %></td>
					                <td><%# DataBinder.Eval(Container.DataItem, "subject") %></td>
					                <td><%# DataBinder.Eval(Container.DataItem, "other") %></td>						
					                <td><%# DateTime.Parse(DataBinder.Eval(Container.DataItem, "created").ToString()).ToString("yyyy-MM-dd") %></td>
					                <td>
					                    <asp:LinkButton runat="server" ID="LbDeleteAppendix" OnClientClick="javascript:return confirm('Är du säker att du vill ta bort denna bilaga?');" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") + "|" + DateTime.Parse(DataBinder.Eval(Container.DataItem, "created").ToString()).ToString("yyyy-MM-dd") %>' OnClick="LbDeleteAppendixOnClick" Text="Ta bort"></asp:LinkButton>
					                </td>
				                </tr>
			                </ItemTemplate>
		                </asp:Repeater>
	                </table>
        	        	                	    	       
	            </asp:PlaceHolder>
    	        
	            <asp:PlaceHolder runat="server" ID="PhAppendixDetails" Visible="false">
    	        
	        	    <table class="appendixadminlist">
				        <tr>
					        <td>
					            Rubrik(namn)<br />
						        <asp:textbox id="txtHeadLine" runat="server" MaxLength="255"></asp:textbox>						        
						    </td>
				        </tr>
				        <tr>
					        <td>
					            Medieproducent<br />
						        <asp:textbox id="txtUtgivare" runat="server" MaxLength="255"></asp:textbox>
						    </td>
				        </tr>
				        <tr>
					        <td>
					            Ämne<br />
						        <asp:textbox id="txtAmne" runat="server" MaxLength="255"></asp:textbox>
						    </td>
				        </tr>
				        <tr>
					        <td>
					            Övrigt <i>(ytterligare sökord för arkivet)</i><br />
						        <asp:textbox id="txtOvrigt" runat="server" MaxLength="255"></asp:textbox>
						    </td>
				        </tr>
				        <tr>
					        <td>
					            Välj pdf<br />
					            <asp:HyperLink runat="server" ID="HlPdf" Target="_blank"></asp:HyperLink>
						        <asp:radiobuttonlist id="RadioButtonListPDF" runat="server"></asp:radiobuttonlist>
						    </td>
				        </tr>
				        <tr>
					        <td>
					            Välj bild (gif)<br />
					            <asp:HyperLink runat="server" ID="HlGif" Target="_blank"></asp:HyperLink>
						        <asp:radiobuttonlist id="RadiobuttonlistGIF" runat="server"></asp:radiobuttonlist>
						    </td>
				        </tr>
				        <tr>
					        <td>
					            Publiceringsdatum (YYYY-MM-DD)<br />
						        <asp:textbox id="txtPublishDate" runat="server" MaxLength="10"></asp:textbox>
						        <asp:TextBox id="txtId" runat="server" Visible="False"></asp:TextBox>
						    </td>
				        </tr>
				        <tr>
					        <td>
					            <asp:button id="BtnAdd" OnClick="BtnAddOnClick" runat="server" Text="Ladda upp"></asp:button><asp:button id="BtnUpdate" OnClick="BtnUpdateOnClick" runat="server" Text="Uppdatera"></asp:button>
					        </td>
				        </tr>
			        </table>			        
    			    
    			    <p class="blkArr">	
    			        <asp:LinkButton ID="LinkButton1" runat="server" OnClick="Show" CommandArgument="PhAppendixList" Text="Tillbaka"></asp:LinkButton>
    			    </p>
	            </asp:PlaceHolder>
	        </div>
	        
	        <asp:PlaceHolder runat="server" ID="PhNotProcessed" Visible="false">
	            <DI:NotProcessed runat="server" />	               
	        </asp:PlaceHolder>
	    </div>
    </form>
</body>
</html>

