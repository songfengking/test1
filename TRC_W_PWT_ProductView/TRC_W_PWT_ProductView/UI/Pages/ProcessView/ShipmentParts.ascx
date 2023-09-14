<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ShipmentParts.ascx.cs" Inherits="TRC_W_PWT_ProductView.UI.Pages.ProcessView.ShipmentParts" %>
<%@ Import Namespace="TRC_W_PWT_ProductView.Common" %>

<%-- 画面CSS/スクリプト定義領域 --%>
<asp:PlaceHolder ID="SubScripts" runat="server">
    <script src="<%: ResolveUrl( "~/Scripts/LibScript/scrollViewer.js") %>"></script>
    <script src="<%: PageUtils.ResolveUrl( this.Page, "~/Scripts/Pages/PartsView/ShipmentParts.js") %>"></script>
    <script src="<%: PageUtils.ResolveUrl( this.Page, "~/Scripts/ControlCommon.js") %>"></script>
</asp:PlaceHolder>
<div class="div-detail-info-margin">
    <div id="divDetailBodyScroll" class="div-fix-scroll">
        <div class="div-detail-table-title">■出荷部品</div>
        <table id="tblPackingOrderTime" class="table-border-layout grid-layout" style="width: 400px; height: auto;" runat="server">
            <colgroup>
                <col style="width: 150px" />
                <col style="width: 250px" />
            </colgroup>
            <tr class="detailtable-header">
                <td class="ui-state-default detailtable-header">梱包実績日時</td>
                <td>
                    <KTCC:KTTextBox ID="txtPackingOrderTime" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full" />
                </td>
            </tr>
        </table>
        <div style="clear: both; width: auto; margin-top: 5px"></div>
        <div id="divPackingOrderTime" class="div-auto-scroll">
            <div id="divMainListArea" style="height: 80px">
                <asp:GridView ID="grvShipmentParts" runat="server" CssClass="grid-layout ui-widget-content" AutoGenerateColumns="False" OnRowDataBound="grvShipmentParts_RowDataBound">
                    <Columns>
                        <asp:BoundField HeaderText="PARTS_NUM" />
                    </Columns>
                    <HeaderStyle CssClass="grid-header ui-state-default" />
                    <RowStyle CssClass="grid-row ui-widget" />
                </asp:GridView>
            </div>
        </div>
        <div class="div-detail-table-title">■梱包作業指示書</div>
        <asp:Button ID="btnPrint" runat="server" Text="印刷" CssClass="btn-middle" />
        <div id="divDetailListArea" style="width: 231px" class="div-y-scroll-flt">
            <asp:ListView ID="lstOrderSheetList" runat="server" OnItemDataBound="lstOrderSheetList_ItemDataBound">
                <LayoutTemplate>
                    <div class="" id="itemPlaceholder" runat="server" />
                </LayoutTemplate>
                <ItemTemplate>
                    <div id="divRowData" runat="server" class="div-list-view-item" style="width: 208px; height: auto; margin-bottom: 3px;">
                        <table class="table-border-layout" style="margin-left: 0px; margin-right: 1px">
                            <colgroup>
                                <col style="width: 202px" />
                            </colgroup>
                            <tr>
                                <td>
                                    <div>
                                        <asp:Image ID="imgOrderSheet" runat="server" CssClass="thumbnail-area-a4" />
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                </ItemTemplate>
            </asp:ListView>
        </div>
        <div id="divDetailViewArea" class="div-fix-scroll-flt">
            <div id="divDetailViewBox" class="div-auto-scroll">
                <asp:Image ID="imgMainArea" runat="server" AlternateText="" class="box-in-center" />
            </div>
        </div>
    </div>
</div>
