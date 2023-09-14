<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GenericDetail.ascx.cs" Inherits="TRC_W_PWT_ProductView.UI.Pages.ProcessView.GenericDetail" %>
<%@ Import Namespace="TRC_W_PWT_ProductView.Common" %>
<asp:PlaceHolder ID="SubScripts" runat="server">
    <script src="<%: PageUtils.ResolveUrl( this.Page, "~/Scripts/Pages/ProcessView/GenericDetail.js") %>"></script>
</asp:PlaceHolder>
<div class="div-detail-info-margin">
    <div id="divDetailBodyScroll" class="div-fix-scroll">
        <div class="div-detail-table-title">■工程内作業</div>
        <div id="divMainListArea" style="height: 150px">
            <asp:GridView ID="grvHistory" runat="server" CssClass="grid-layout ui-widget-content" AutoGenerateColumns="False" OnSelectedIndexChanged="grvHistory_SelectedIndexChanged" OnRowDataBound="grvHistory_RowDataBound">
                <Columns>
                    <asp:BoundField HeaderText="DISPLAY_ODR" />
                    <asp:BoundField HeaderText="WORK_CD" />
                    <asp:BoundField HeaderText="WORK_NM" />
                    <asp:BoundField HeaderText="RECORD_NO" />
                    <asp:BoundField HeaderText="RESULT_NM" />
                    <asp:CommandField ShowSelectButton="True" />
                </Columns>
                <HeaderStyle CssClass="grid-header ui-state-default" />
                <RowStyle CssClass="grid-row ui-widget" />
                <SelectedRowStyle CssClass="grid-row ui-widget ui-state-highlight" />
            </asp:GridView>

        </div>
        <div class="div-detail-table-title">■測定結果</div>
        <div id="divSubListArea" class="div-auto-scroll">
            <asp:GridView ID="grvMeasuringResult" runat="server" CssClass="grid-layout ui-widget-content" AutoGenerateColumns="False">
                <HeaderStyle CssClass="grid-header ui-state-default" />
                <RowStyle CssClass="grid-row ui-widget" />
            </asp:GridView>
        </div>
</div>
</div>
