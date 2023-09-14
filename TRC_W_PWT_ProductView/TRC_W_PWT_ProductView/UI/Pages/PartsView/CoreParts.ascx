<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CoreParts.ascx.cs" Inherits="TRC_W_PWT_ProductView.UI.Pages.PartsView.CoreParts" %>
<%@ Import Namespace="TRC_W_PWT_ProductView.Common" %>

<%-- 画面CSS/スクリプト定義領域 --%>
<asp:PlaceHolder ID="SubScripts" runat="server">
    <script src="<%: PageUtils.ResolveUrl( this.Page, "~/Scripts/Pages/ContentScroll.js") %>"></script>
</asp:PlaceHolder>

<div class="div-detail-info-margin">
    <div>
        <div class="div-detail-table-title">■最終組付情報</div>
        <div style="clear: both; height: 10px; width: auto"></div>
        <div id="divMainListArea" class="div-auto-scroll" style="margin-left: 10px; height: 490px">
            <asp:PlaceHolder ID="pnlInstallInfo" runat="server"></asp:PlaceHolder>
        </div>
    </div>
</div>
