<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TwitterBox.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.SideBarBoxes.TwitterBox" %>
<%@ OutputCache Duration="60" VaryByControl="controlname" %>
<%-- Cache options:
OutputCache 
   Duration="NUM SEC"
   Location="Any | Client | Downstream | Server | None | ServerAndClient "
   Shared="True | False"
   VaryByControl="controlname"
   VaryByCustom="browser | customstring"
   VaryByHeader="headers"
   VaryByParam="parametername" 
   CacheProfile="cache profile name | ''"
   NoStore="true | false"
   SqlDependency="database/table name pair | CommandNotification"--%>


<%-- NB! - UserControl uses OutputCache cause Twitter only allows x calls / time unit --%>


<%--
<%= DateTime.Now.ToString() %>
<br />
--%>

<a class="twitter-timeline" data-widget-id="<%=WidgetId%>"></a>
<script>!function (d, s, id) { var js, fjs = d.getElementsByTagName(s)[0], p = /^http:/.test(d.location) ? 'http' : 'https'; if (!d.getElementById(id)) { js = d.createElement(s); js.id = id; js.src = p + "://platform.twitter.com/widgets.js"; fjs.parentNode.insertBefore(js, fjs); } } (document, "script", "twitter-wjs");</script>

<br />
<br />


<%-- V1 
<style type="text/css">
    .twtr-tweet-text p { font-family: Arial, Verdana, Sans-Serif; font-size:13px; line-height:15px; }
</style>

<div align="center">
    <script charset="utf-8" src="http://widgets.twimg.com/j/2/widget.js"></script>
    <script>
        new TWTR.Widget({
            version: 2,
            type: 'profile',
            rpp: <%=ShowNumItems%>,
            interval: 30000,
            width: 'auto',
            height: 591,
            theme: {
                shell: {
                    background: '#e4e4e4',
                    color: '#000000'
                },
                tweets: {
                    background: '#ffffff',
                    color: '#000000',
                    links: '#c8281e'
                }
            },
            features: {
                scrollbar: false,
                loop: false,
                live: false,
                behavior: 'all'
            }
        }).render().setUser('<%=TweetName%>').start();
    </script>
    <br />
    <br />
</div>--%>