<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GoogleMaps.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.SideBarBoxes.GoogleMaps" %>

<!-- Map -->			
<div class="infobox"> 
	<div class="wrapper"> 
		<h2>
            <EPiServer:Translate Text="/confroom/showinmap" runat="server" />
        </h2> 
		<div class="content"> 
			<div id="map" class="gmap"> 
				<script type="text/javascript">
				    q.map.set('#map', '<%= address %>'); 
                </script> 
			</div> 
		</div> 
	</div> 
</div> 
<!-- // Map -->	