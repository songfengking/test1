<%@ Page Language="C#" MasterPageFile="~/UI/MasterForm/MasterSub.master" AutoEventWireup="true" CodeBehind="ProCodeIdnoDialog.aspx.cs" Inherits="TRC_W_PWT_ProductView.UI.Pages.Order.ProCodeIdnoDialog" %>

<%@ Import Namespace="TRC_W_PWT_ProductView.Common" %>
<asp:Content ID="MasterScript" runat="server" ContentPlaceHolderID="MasterScript">
    <script src="<%: PageUtils.ResolveUrl( this.Page, "~/Scripts/ControlCommon.js") %>"></script>
    <script src="<%: PageUtils.ResolveUrl( this.Page, "~/Scripts/Pages/Order/ProCodeIdnoDialog.js") %>"></script>
</asp:Content>
<asp:Content ID="MasterBodyTop" ContentPlaceHolderID="MasterBodyTop" runat="server">
    <%-- 画面タイトル --%>
    <div class="ui-widget-header header-title-area">
        <asp:Label ID="lblTitle" runat="server" Text=""></asp:Label>
    </div>
</asp:Content>
<asp:Content ID="MasterBody" ContentPlaceHolderID="MasterBody" runat="server">
    <div>
        <table class="table-layout">
            <tr>
                <td>
                    <div class="div-grid-results">
                        <div class="div-result-pager">
                            <asp:Panel ID="pnlPager" runat="server" EnableViewState="true"></asp:Panel>
                        </div>
                        <div id="divgrvCount" class="div-result-count">
                            <span>件数：</span>
                            <KTCC:KTNumericTextBox ID="ntbResultCount" runat="server" CssClass="txt-center-num" ReadOnly="true" />
                            <span>件</span>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <div style="clear: both; height: 0px;"></div>
    <div id="divGrvDisplay" runat="server">
        <table class="table-layout-fix">
            <tr>
                <td>
                    <div id="divLTScroll" class="div-fix-scroll div-left-grid">
                        <div id="divGrvHeaderLT" runat="server">
                            <asp:GridView ID="gvProCodeIdnoDialogHeader" runat="server" CssClass="grid-layout2 ui-widget-content" AllowPaging="false" PageSize="50" AutoGenerateColumns="false" OnRowDataBound="gvProCodeIdnoDialogBody_RowDataBound">
                                <HeaderStyle CssClass="grid-header ui-state-default" />
                            </asp:GridView>
                        </div>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div id="divLBScroll" class="div-scroll-left-bottom div-left-grid">
                        <div id="divGrvLB" runat="server">
                            <asp:GridView ID="gvProCodeIdnoDialogBody" runat="server" ShowHeader="false" CssClass="grid-layout grid-layout2 ui-widget-content" AllowPaging="true" AllowSorting="false" PageSize="50" AutoGenerateColumns="false" OnRowDataBound="gvProCodeIdnoDialogBody_RowDataBound">
                                <PagerSettings Position="TopAndBottom" Mode="NumericFirstLast" PageButtonCount="10" Visible="false" />
                                <HeaderStyle CssClass="grid-header ui-state-default" />
                                <RowStyle CssClass="grid-row ui-widget" />
                                <SelectedRowStyle CssClass="ui-state-highlight" />
                                <Columns>
                                    <asp:TemplateField HeaderText="DISP_ORDER" />
                                    <asp:TemplateField HeaderText="RITTAI_NAME" />
                                    <asp:TemplateField HeaderText="STOCK_KBN" />
                                    <asp:TemplateField HeaderText="STOCK_REN" />
                                    <asp:TemplateField HeaderText="STOCK_DAN" />
                                    <asp:TemplateField HeaderText="STOCK_RETSU" />
                                    <asp:TemplateField HeaderText="STOP_FLAG" />
                                    <asp:TemplateField HeaderText="LOCATION_FLAG" />
                                    <asp:TemplateField HeaderText="KUMITATE_PATTERN" />
                                    <asp:TemplateField HeaderText="ENGINE_SYUBETSU" />
                                    <asp:TemplateField HeaderText="TOUSAI_OEM" />
                                    <asp:TemplateField HeaderText="NAIGAISKAU" />
                                    <asp:TemplateField HeaderText="IDNO" />
                                    <asp:TemplateField HeaderText="KIBAN" />
                                    <asp:TemplateField HeaderText="TOKKI" />
                                    <asp:TemplateField HeaderText="UNTENFLAG" />
                                    <asp:TemplateField HeaderText="INSTOCK_DATE" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
