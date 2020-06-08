<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="DagensIndustri.Aktiverad.Default" %>

<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8">
    <title>Dagens industri - Di-konto aktiverat</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <!-- Bootstrap -->
    <link href="/bootstrapDi/css/bootstrap.min.css" rel="stylesheet" />
    <link href="/bootstrapDi/css/bootstrap-responsive.min.css" rel="stylesheet" />
</head>
<body>
    <div class="container">
        <div class="hero-unit">
            <h1 class="text-center">
                Ditt Di-konto är aktiverat!</h1>
            <br>
            <p class="text-center">
                Som prenumerant har du nu full tillgång till Dagens industris tjänster digitalt.
                <br>
                För att komma igång kan du välja något utav följande alternativ:</p>
            <p>
            </p>
        </div>
        <ul class="thumbnails">
            <li class="span4">
                <div class="thumbnail">
                    <img src="img/prenumerera.jpg" alt="Prenumerera">
                    <div class="caption">
                        <h3 class="text-center">
                            Bli prenumerant</h3>
                        <p>
                            Få djuplodande analyser, värdefulla insikter och möjlighet att läsa tidningarnas
                            innehåll digitalt genom att teckna en prenumeration på Dagens industri.
                        </p>
                        <p align="center">
                            <a href="/prenumerera/" class="btn btn-success btn-block">Jag vill teckna en prenumeration</a></p>
                    </div>
                </div>
            </li>
            <li class="span4">
                <div class="thumbnail">
                    <img src="img/digitalt.jpg" alt="ALT NAME">
                    <div class="caption">
                        <h3 class="text-center">
                            Läs tidningen digitalt</h3>
                        <p>
                            Som prenumerant har du full tillgång till Dagens industris omfattande innehåll med
                            analyser och insikter digitalt – på webben, i läsplattan och i mobilen.</p>
                        <p align="center">
                            <%--<a href="/sso-login/?returnUrl=test2.dagensindustri.episerverhosting.com" class="btn btn-success btn-block">Jag vill läsa tidningen digitalt</a>--%>
                            <asp:HyperLink ID="HyperLinkDig" CssClass="btn btn-success btn-block" runat="server">Jag vill läsa tidningen digitalt</asp:HyperLink>
                        </p>
                    </div>
                </div>
            </li>
            <li class="span4">
                <div class="thumbnail">
                    <img src="img/dise.jpg" alt="ALT NAME">
                    <div class="caption">
                        <h3 class="text-center">
                            Läs nyheter på di.se</h3>
                        <p>
                            Håll dig uppdaterad och få snabba nyheter i nyhetsflödet på di.se. Debattera aktuella
                            artiklar och ta del av tjänster såsom Börssnack och Trader.
                        </p>
                        <p align="center">
                            <a href="http://www.di.se/" class="btn btn-success btn-block">Ta mig vidare till di.se</a></p>
                    </div>
                </div>
            </li>
        </ul>
    </div>

    <script src="/bootstrapDi/js/jquery.1.9.1.js" type="text/javascript"></script>
    <script src="/bootstrapDi/js/bootstrap.min.js" type="text/javascript"></script>

</body>
</html>

