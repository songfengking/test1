<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FrictionLoss.ascx.cs" Inherits="TRC_W_PWT_ProductView.UI.Pages.ProcessView.FrictionLoss" %>
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
    <div>
        <div class="div-detail-table-title">■来歴情報</div>
        <div style="clear: both; height: 10px;width:auto"></div>
        <div id="divGrvDisplay" runat="server" style="margin-left:10px">
            <table class="table-layout-fix">
                <tr>
                    <td>
                        <div id="divLTScroll" class="div-fix-scroll div-left-grid">
                            <div id="divGrvHeaderLT" runat="server">
                                <asp:GridView ID="grvHeaderLT" runat="server" CssClass="grid-layout2 ui-widget-content" AllowPaging="false" AllowSorting="false" PageSize="100" AutoGenerateColumns="false">
                                    <HeaderStyle CssClass="grid-header ui-state-default" />
                                </asp:GridView>
                            </div>
                        </div>
                    </td>
                    <td>
                        <div id="divRTScroll" class="div-scroll-right-top div-right-grid">
                            <div id="divGrvHeaderRT" runat="server">
                                <asp:GridView ID="grvHeaderRT" runat="server" CssClass="grid-layout2 ui-widget-content" AllowPaging="false" AllowSorting="false" PageSize="100" AutoGenerateColumns="false">
                                    <HeaderStyle CssClass="grid-header-newline ui-state-default" />
                                </asp:GridView>
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div id="divLBScroll" class="div-scroll-left-bottom div-left-grid">
                            <div id="divGrvLB" runat="server">
                                <asp:GridView ID="grvMainViewLB" runat="server" ShowHeader="false" CssClass="grid-layout2 ui-widget-content" AllowPaging="false" AllowSorting="false" PageSize="1000" AutoGenerateColumns="false" OnRowDataBound="grvMainViewLB_RowDataBound">
                                    <HeaderStyle CssClass="grid-header ui-state-default" />
                                    <RowStyle CssClass="grid-row ui-widget" />
                                    <SelectedRowStyle CssClass="ui-state-highlight" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="inspectionDt" />
                                        <asp:TemplateField HeaderText="mesureResult" />
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
                                        <asp:TemplateField HeaderText="standardRpm" />
                                        <asp:TemplateField HeaderText="mesureMaxVal" />
                                        <asp:TemplateField HeaderText="mesureMinVal" />
                                        <asp:TemplateField HeaderText="mesureAvgVal" />
                                        <asp:TemplateField HeaderText="mesureTemperature" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <%--    ヘッダ固定化に伴いコメントアウト
        <div id="divMainListArea" class="div-auto-scroll" >
            <asp:ListView ID="lstMainList" runat="server" OnItemDataBound="lstMainList_ItemDataBound">
                <LayoutTemplate>
                    <table id="itemPlaceholderContainer" runat="server" class="grid-layout" style="width: 720px; height:auto">
                        <tr id="headerMainContent" runat="server" class="listview-header_2r ui-state-default">
                            <th id="Th01" style="width: 150px" runat="server">計測日時</th>
                            <th id="Th02" style="width:  70px" runat="server">判定</th>
                            <th id="Th03" style="width: 100px" runat="server">回転数<br />(rpm)</th>
                            <th id="Th04" style="width: 100px" runat="server">最大値<br />(N・m)</th>
                            <th id="Th05" style="width: 100px" runat="server">最小値<br />(N・m)</th>
                            <th id="Th06" style="width: 100px" runat="server">平均値<br />(N・m)</th>
                            <th id="Th07" style="width: 100px" runat="server">気温</th>
                        </tr>
                        <tr class="" id="itemPlaceholder" runat="server">
                        </tr>
                    </table>
                </LayoutTemplate>
                <ItemTemplate>
                    <tr id="trRowData" runat="server" class="listview-row ui-widget">
                        <td>
                            <KTCC:KTTextBox ID="txtInspectionDt" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                        </td>
                        <td>
                            <KTCC:KTTextBox ID="txtMesureResult" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                        </td>
                        <td>
                            <KTCC:KTNumericTextBox ID="ntbStandardRpm" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-numeric txt-width-full" />
                        </td>
                        <td>
                            <KTCC:KTDecimalTextBox ID="ntbMesureMaxVal" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="2" CssClass="font-default txt-default txt-numeric txt-width-full" />
                        </td>
                        <td>
                            <KTCC:KTDecimalTextBox ID="ntbMesureMinVal" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="2" CssClass="font-default txt-default txt-numeric txt-width-full" />
                        </td>
                        <td>
                            <KTCC:KTDecimalTextBox ID="ntbMesureAvgVal" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="2" CssClass="font-default txt-default txt-numeric txt-width-full" />
                        </td>
                        <td>
                            <KTCC:KTDecimalTextBox ID="ntbMesureTemperature" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="1" CssClass="font-default txt-default txt-numeric txt-width-full" />
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:ListView>
        </div>
        --%>
    </div>
</div>
