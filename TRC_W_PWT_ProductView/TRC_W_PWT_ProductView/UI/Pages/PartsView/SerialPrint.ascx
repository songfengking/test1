<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SerialPrint.ascx.cs" Inherits="TRC_W_PWT_ProductView.UI.Pages.PartsView.SerialPrint" %>
<%@ Import Namespace="TRC_W_PWT_ProductView.Common" %>

<%-- デザイン表示時使用 マスターページ使用時不要 --%>
<%--<link href="../../../CSS/cobalt/jquery-wijmo.css" rel="stylesheet" />
<link href="../../../CSS/Base.css" rel="stylesheet" />
<link href="../../../CSS/TRC.css" rel="stylesheet" />--%>

<%-- 画面CSS/スクリプト定義領域 --%>
<asp:PlaceHolder ID="SubScripts" runat="server">
    <script src="<%: PageUtils.ResolveUrl( this.Page, "~/Scripts/Pages/ContentSolidScroll.js") %>" ></script>
</asp:PlaceHolder>

<div class="div-detail-info-margin">
    <div id="divDetailBodyScroll" class="div-fix-scroll">
        <div class="div-detail-table-title">■機番ラベル印刷</div>
        <div style="clear: both; height: 10px;width:auto"></div>
        <div id="divGrvDisplay" runat="server" style="margin-left:10px">
            <table class="table-layout-fix">
                <tr>
                    <td>
                        <div id="divLTScroll" class="div-fix-scroll div-left-grid">
                            <div id="divGrvHeaderLT" runat="server">
                                <asp:GridView ID="grvHeaderLT" runat="server" CssClass="grid-layout2 ui-widget-content" AllowPaging="false" AllowSorting="true" PageSize="100" AutoGenerateColumns="false">
                                    <HeaderStyle CssClass="grid-header ui-state-default" />
                                </asp:GridView>
                            </div>
                        </div>
                    </td>
                    <td>
                        <div id="divRTScroll" class="div-scroll-right-top div-right-grid">
                            <div id="divGrvHeaderRT" runat="server">
                                <asp:GridView ID="grvHeaderRT" runat="server" CssClass="grid-layout2 ui-widget-content" AllowPaging="false" AllowSorting="true" PageSize="100" AutoGenerateColumns="false">
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
                                <asp:GridView ID="grvMainViewLB" runat="server" ShowHeader="false" CssClass="grid-layout2 ui-widget-content" AllowPaging="false" AllowSorting="false" PageSize="100" AutoGenerateColumns="false" OnRowDataBound="grvMainViewLB_RowDataBound">
                                    <HeaderStyle CssClass="grid-header ui-state-default" />
                                    <RowStyle CssClass="grid-row ui-widget" />
                                    <SelectedRowStyle CssClass="ui-state-highlight" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="stationNm" />
                                        <asp:TemplateField HeaderText="engineModelCd" />
                                        <asp:TemplateField HeaderText="engineSerial" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </td>
                    <td>
                        <div id="divRBScroll" class="div-visible-scroll div-right-grid">
                            <div id="divGrvRB" runat="server">
                                <asp:GridView ID="grvMainViewRB" runat="server" ShowHeader="false" CssClass="grid-layout2 ui-widget-content" AllowPaging="false" AllowSorting="false" PageSize="1000" AutoGenerateColumns="false" OnRowDataBound="grvMainViewRB_RowDataBound">
                                    <HeaderStyle CssClass="grid-header ui-state-default" />
                                    <RowStyle CssClass="grid-row ui-widget" />
                                    <SelectedRowStyle CssClass="ui-state-highlight" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="printCont" />
                                        <asp:TemplateField HeaderText="qrCd" />
                                        <asp:TemplateField HeaderText="autoFlag" />
                                        <asp:TemplateField HeaderText="printDt" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </div>

</div>
