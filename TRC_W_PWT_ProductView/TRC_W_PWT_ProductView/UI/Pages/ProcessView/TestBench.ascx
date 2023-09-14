<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TestBench.ascx.cs" Inherits="TRC_W_PWT_ProductView.UI.Pages.ProcessView.TestBench" %>
<%@ Import Namespace="TRC_W_PWT_ProductView.Common" %>

<%-- 画面CSS/スクリプト定義領域 --%>
<asp:PlaceHolder ID="SubScripts" runat="server">
    <script src="<%: PageUtils.ResolveUrl( this.Page, "~/Scripts/Pages/ContentScroll.js") %>"></script>
    <script src="<%: PageUtils.ResolveUrl( this.Page, "~/Scripts/Pages/ProcessView/TestBench.js") %>" ></script>
</asp:PlaceHolder>

<div class="div-detail-info-margin">
    <div id="divDetailBodyScroll" class="div-fix-scroll">
        <div id="divMainListArea" class="div-auto-scroll" style="margin-left: 10px; height: 490px">
            <asp:PlaceHolder ID="pnlTestResult" runat="server"></asp:PlaceHolder>
        </div>
        <div class="div-detail-table-title">■検査結果詳細</div>
        <div id="divSubListArea" class="div-auto-scroll" style="margin-left: 10px; height: 110px">
            <asp:GridView ID="grvTestResultDetail" runat="server" CssClass="grid-layout ui-widget-content" AutoGenerateColumns="False" OnRowDataBound="grvTestResultDetail_RowDataBound" OnRowCommand="grvTestResultDetail_RowCommand">
                <Columns>
                    <asp:BoundField HeaderText="TEST_NO" />
                    <asp:BoundField HeaderText="BENCH_NO" />
                    <asp:BoundField HeaderText="HISTORY_NO" />
                    <asp:ButtonField HeaderText="FILE_NAME" CommandName="Download" />
                    <asp:BoundField HeaderText="UNIQUE_NAME" />
                </Columns>
                <HeaderStyle CssClass="grid-header ui-state-default" />
                <RowStyle CssClass="grid-row ui-widget" />
            </asp:GridView>
        </div>
    </div>
</div>
