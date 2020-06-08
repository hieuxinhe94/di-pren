<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ConcurrentUsers.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.ConcurrentUsers" %>

  
<style type="text/css">
  .no-close .ui-dialog-titlebar-close {
    display: none;
  }
  .ui-dialog-buttonpane {
    display: none;
  }
  .padLeft {
    padding-left: 5px;
  }
  .white {
    color: #ffffff;
  }
</style>
  
<link rel="stylesheet" href="http://code.jquery.com/ui/1.10.3/themes/smoothness/jquery-ui.css" />
<script src="http://code.jquery.com/jquery-1.9.1.js" type="text/javascript"></script>
<script src="http://code.jquery.com/ui/1.10.3/jquery-ui.js" type="text/javascript"></script>

<script type="text/javascript">
  $(document).ready(function () {
    $("#dialog-message").dialog({
      modal: true,
      dialogClass: "no-close"
    });
  });
</script>

<div id="dialog-message" title="Meddelande" style="display: block;">
  <p>
    <asp:Literal ID="LiteralMessage" runat="server"></asp:Literal>
    <br/>
  </p>
  <%--<a href='#' id="btnLink" class="btn"><span style="color:#ffffff;">OK</span></a>--%>
  <asp:HyperLink ID="HyperLinkButton" CssClass="btn" runat="server"><span class="white">Logga in</span></asp:HyperLink>
  <asp:HyperLink ID="HyperLinkCancel" CssClass="btn padLeft" runat="server"><span class="white">Avbryt, gå till startsidan</span></asp:HyperLink>
</div>




<%--<script>
  $(document).ready(function () {
      //$("button").click(function () {
      //$("p").hide();
      //});
      //$("#dialog").dialog("open");

    alert('test');
    window.location = '<%=LogoutLoginRedirUrl%>';

//      $(function () {
//        $("#dialog-confirm").dialog({
//          resizable: false,
//          height: 140,
//          modal: true,
//          buttons: {
//            "Delete all items": function () {
//              $(this).dialog("close");
//            },
//            Cancel: function () {
//              $(this).dialog("close");
//            }
//          }
//        });
//      });


//  $(document).ready(function () {
////    $(function () {
//      $("#dialog-message").dialog({
//        modal: true,
//        dialogClass: "no-close",
////        buttons: {
////          Ok: function () {
////            $(this).dialog("close");
////          }
////        }
////        buttons: [{
////          "text": 'OK',
////          "click": function () {
////            $(this).dialog("close");
////          },
////          "class": 'btn'
////        }]
//      });
////    });
//  });

  });
</script>--%>