<%@ Page Title="" Language="C#" MasterPageFile="~/Templates/Public/MasterPages/MasterPage.Master" AutoEventWireup="true" CodeBehind="TreePage.aspx.cs" Inherits="DagensIndustri.Templates.Public.Pages.TreePage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/Templates/Public/Styles/tree.css" rel="stylesheet" type="text/css" />
        <script type="text/javascript" src="/Templates/Public/js/Benton_Sans_DI_400.font.js"></script>
        <script type="text/javascript" src="/Templates/Public/js/tree.js"></script>
</asp:Content>

<asp:Content ID="MainContentPlaceHolder1" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <!-- list with all items -->
          <ul class="tree-item-list">

            <!-- Läsplatta -->
            <li id="tree-item-1" class="tree-item">
              <a href="<%=(string)CurrentPage["Item1Link"]%>">
              </a>
              <!-- list item popup -->
              <div class="tree-item-popup right">
                <!-- popup heading -->
                <strong><%=(string)CurrentPage["Item1Heading"]%></strong>
                <!-- popup text -->
                <p><%=(string)CurrentPage["Item1Text"]%></p>
              </div>
            </li>


            <!-- Di WEEKEND -->
            <li id="tree-item-2" class="tree-item">
              <a href="<%=(string)CurrentPage["Item2Link"]%>">
              </a>
              <!-- list item popup -->
              <div class="tree-item-popup top">
                <!-- popup heading -->
                <strong><%=(string)CurrentPage["Item2Heading"]%></strong>
                <!-- popup text -->
                <p><%=(string)CurrentPage["Item2Text"]%></p>
              </div>
            </li>

            <!-- DAGENS INDUSTRI -->
            <li id="tree-item-3" class="tree-item">
              <a href="<%=(string)CurrentPage["Item3Link"]%>">
              </a>
              <!-- list item popup -->
              <div class="tree-item-popup right">
                <!-- popup heading -->
                <strong><%=(string)CurrentPage["Item3Heading"]%></strong>
                <!-- popup text -->
                <p><%=(string)CurrentPage["Item3Text"]%></p>
              </div>
            </li>

            <!-- Di.SE -->
            <li id="tree-item-4" class="tree-item">
              <a href="<%=(string)CurrentPage["Item4Link"]%>">
              </a>
              <!-- list item popup -->
              <div class="tree-item-popup right">
                <!-- popup heading -->
                <strong><%=(string)CurrentPage["Item4Heading"]%></strong>
                <!-- popup text -->
                <p><%=(string)CurrentPage["Item4Text"]%></p>
              </div>
            </li>

            <!-- Di.SE-APPEN -->
            <li id="tree-item-5" class="tree-item">
              <a href="<%=(string)CurrentPage["Item5Link"]%>">
              </a>
              <!-- list item popup -->
              <div class="tree-item-popup left">
                <!-- popup heading -->
                <strong><%=(string)CurrentPage["Item5Heading"]%></strong>
                <!-- popup text -->
                <p><%=(string)CurrentPage["Item5Text"]%></p>
              </div>
            </li>

            <!-- Di DIMENSION -->
            <li id="tree-item-6" class="tree-item">
              <a href="<%=(string)CurrentPage["Item6Link"]%>">
              </a>
              <!-- list item popup -->
              <div class="tree-item-popup left">
                <!-- popup heading -->
                <strong><%=(string)CurrentPage["Item6Heading"]%></strong>
                <!-- popup text -->
                <p><%=(string)CurrentPage["Item6Text"]%></p>
              </div>
            </li>

            <!-- Di KONFERENS -->
            <li id="tree-item-7" class="tree-item">
              <a href="<%=(string)CurrentPage["Item7Link"]%>">
              </a>
              <!-- list item popup -->
              <div class="tree-item-popup top">
                <!-- popup heading -->
                <strong><%=(string)CurrentPage["Item7Heading"]%></strong>
                <!-- popup text -->
                <p><%=(string)CurrentPage["Item7Text"]%></p>
              </div>
            </li>

            <!-- Di GASELL -->
            <li id="tree-item-8" class="tree-item">
              <a href="<%=(string)CurrentPage["Item8Link"]%>">
              </a>
              <!-- list item popup -->
              <div class="tree-item-popup top">
                <!-- popup heading -->
                <strong><%=(string)CurrentPage["Item8Heading"]%></strong>
                <!-- popup text -->
                <p><%=(string)CurrentPage["Item8Text"]%></p>
              </div>
            </li>

            <!-- VINKLUBB -->
            <li id="tree-item-9" class="tree-item">
              <a href="<%=(string)CurrentPage["Item9Link"]%>">
              </a>
              <!-- list item popup -->
              <div class="tree-item-popup left">
                <!-- popup heading -->
                <strong><%=(string)CurrentPage["Item9Heading"]%></strong>
                <!-- popup text -->
                <p><%=(string)CurrentPage["Item9Text"]%></p>
              </div>
            </li>

            <!-- Di GULD -->
            <li id="tree-item-10" class="tree-item">
              <a href="<%=(string)CurrentPage["Item10Link"]%>">
              </a>
              <!-- list item popup -->
              <div class="tree-item-popup bottom">
                <!-- popup heading -->
                <strong><%=(string)CurrentPage["Item10Heading"]%></strong>
                <!-- popup text -->
                <p><%=(string)CurrentPage["Item10Text"]%></p>
              </div>
            </li>

            <!-- DiY -->
            <li id="tree-item-11" class="tree-item">
              <a href="<%=(string)CurrentPage["Item11Link"]%>">
              </a>
              <!-- list item popup -->
              <div class="tree-item-popup right">
                <!-- popup heading -->
                <strong><%=(string)CurrentPage["Item11Heading"]%></strong>
                <!-- popup text -->
                <p><%=(string)CurrentPage["Item11Text"]%></p>
              </div>
            </li>

          </ul>
</asp:Content>
