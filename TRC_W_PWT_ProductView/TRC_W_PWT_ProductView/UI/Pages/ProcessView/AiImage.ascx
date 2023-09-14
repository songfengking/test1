<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AiImage.ascx.cs" Inherits="TRC_W_PWT_ProductView.UI.Pages.ProcessView.AiImage" %>
<%@ Import Namespace="TRC_W_PWT_ProductView.Common" %>

<%-- デザイン表示時使用 マスターページ使用時不要 --%>
<%--<link href="../../../CSS/cobalt/jquery-wijmo.css" rel="stylesheet" />
<link href="../../../CSS/Base.css" rel="stylesheet" />
<link href="../../../CSS/TRC.css" rel="stylesheet" />--%>

<%-- 画面CSS/スクリプト定義領域 --%>
<asp:PlaceHolder ID="SubScripts" runat="server">
    <script src="<%: ResolveUrl( "~/Scripts/LibScript/scrollViewer.js") %>"></script>
    <script src="<%: PageUtils.ResolveUrl( this.Page, "~/Scripts/Pages/ProcessView/AiImage.js") %>"></script>
</asp:PlaceHolder>

<div class="div-detail-info-margin">
    <div id="tabResult" class="tabbox" style="margin-left: 10px;">
        <ul id="stationTabs" class="tabs" runat="server" />

        <asp:PlaceHolder ID="pnlTabDefines" runat="server" />
    </div>
</div>
