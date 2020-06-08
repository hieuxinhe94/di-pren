<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DiMembers.aspx.cs" Inherits="DagensIndustri.Tools.Reports.DiMembers" %>

<asp:Content ContentPlaceHolderID="FullRegion" runat="server">
    <div class="epi-contentContainer epi-padding">
        <div class="epi-contentArea">
            <div class="EP-systemImage" style="background-image: url('/tools/reports/styles/images/apple-touch-icon-72x72.png');min-height: 6em;padding-left: 80px; background-position: 0 2px;background-repeat: no-repeat;">
                <h1>
                    Di Members          
                </h1>
                <p class="EP-systemInfo">
                    This report displays how many Di Gold and Di Regular members exists.
                </p>
            </div>
        </div>
        <div class="epi-formArea">
            <fieldset>
                <legend>
                    Di Gold Members
                </legend>
                <div class="epirowcontainer">
                    <label class="episize140">
                        <strong>Number of Di Gold Members:</strong>
                    </label>
                    <asp:Literal ID="DiGoldLiteral" runat="server" />
                </div>
            </fieldset>
            <fieldset>
                <legend>
                    Di Regular Members
                </legend>
                <div class="epirowcontainer">              
                    <label class="episize140">
                        <strong>Number of Di Regular Members:</strong>
                    </label>
                    <asp:Literal ID="DiRegularLiteral" runat="server" />
                </div>
            </fieldset>
        </div>
    </div>
</asp:Content>