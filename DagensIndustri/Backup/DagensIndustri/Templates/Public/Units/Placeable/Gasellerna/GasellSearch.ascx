<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GasellSearch.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.Gasellerna.GasellSearch" %>

<script type="text/javascript">

    function Redirect(selObj, list, e) {
        var targetUrl = '<%=GetMainTargetUrl()%>';
        var iframeSearch = '<%= GetUrlFromPageProperty("IframeSearch")%>';
        var query = iframeSearch;
        var redirect = false;

        if (list == true) {
            query += "?county=" + selObj.options[selObj.selectedIndex].value;
            redirect = true;
        }
        else {
            if (e.keyCode == 13) {
                query += "?search=" + selObj.value;
                redirect = true;
            }
        }

        if (redirect)
            location = targetUrl + "?iframeUrl=" + encodeURIComponent(query);
    }   
   
</script>

<div class="ColumnBox search">
    <table cellspacing="0" cellpadding="0">
        <tbody>
            <tr>
	            <td class="heading">
	                <%= HttpUtility.HtmlEncode(ActualCurrentPage.PageName)%>               
	            </td> 
            </tr>
            <tr>
                <td class="selectarea">
			        <table>
				        <tbody>
				            <tr>
					            <td>
						            <div class="options"> 
						                <select onchange="Redirect(this,true,event);">
								            <option value="0"><%=Translate("/gasellerna/search/optiondefaultvalue") %></option>
								            <option value="Blekinge">Blekinge</option>
                                            <option value="Dalarna">Dalarna</option>
                                            <option value="Gotland">Gotland</option>
                                            <option value="Gävleborg">Gävleborg</option>
                                            <option value="Halland">Halland</option>
                                            <option value="Jämtland">Jämtland</option>
                                            <option value="Jönköping">Jönköping</option>
                                            <option value="Kalmar">Kalmar</option>
                                            <option value="Kronoberg">Kronoberg</option>
                                            <option value="Norrbotten">Norrbotten</option>
                                            <option value="Skåne">Skåne</option>
                                            <option value="Stockholm">Stockholm</option>
                                            <option value="Södermanland">Södermanland</option>
                                            <option value="Uppsala">Uppsala</option>
                                            <option value="Värmland">Värmland</option>
                                            <option value="Västerbotten">Västerbotten</option>
                                            <option value="Västernorrland">Västernorrland</option>
                                            <option value="Västmanland">Västmanland</option>
                                            <option value="Västra Götaland">Västra Götaland</option>
                                            <option value="Örebro">Örebro</option>
                                            <option value="Östergötland">Östergötland</option>
								        </select>						           		            
						            </div>						           
					            </td>
				            </tr>
			            </tbody>
			        </table>                
                </td>
            </tr>            
	        <tr>
		        <td class="searcharea">
		            <EPiServer:Translate ID="Translate1" runat="server" Text="/gasellerna/search/quicksearchheading" /><br/>
				    <input type="text" onkeydown="Redirect(this,false,event)" onfocus="javascript:this.select();" value='<%=Translate("/gasellerna/search/inputdefaultvalue") %>' maxlength="50" />                    
		        </td> 
	        </tr>
	        
	        <EPiServer:PageList runat="server" ID="PlLinks">	            
	            <ItemTemplate>
	                <tr>
                        <td class="link redArr">
                            <a href='<%#GetMainTargetUrlWithQuery("iframeUrl", GetFriendlyAbsoluteUrl(Container.CurrentPage))%>'><EPiServer:Property ID="Property1" runat="server" PropertyName="PageName" /></a>
                        </td>
                    </tr>
	            </ItemTemplate>
	        </EPiServer:PageList>
	                 
        </tbody>
    </table>    
</div>