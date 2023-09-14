<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Injecter.ascx.cs" Inherits="TRC_W_PWT_ProductView.UI.Pages.PartsView.Injecter" %>
<%@ Import Namespace="TRC_W_PWT_ProductView.Common" %>

<%-- デザイン表示時使用 マスターページ使用時不要 --%>
<%--<link href="../../../CSS/cobalt/jquery-wijmo.css" rel="stylesheet" />
<link href="../../../CSS/Base.css" rel="stylesheet" />
<link href="../../../CSS/TRC.css" rel="stylesheet" />--%>

<%-- 画面CSS/スクリプト定義領域 --%>
<asp:PlaceHolder ID="SubScripts" runat="server">
    <script src="<%: PageUtils.ResolveUrl( this.Page, "~/Scripts/Pages/PartsView/Injecter.js") %>" ></script>
</asp:PlaceHolder>

<div class="div-detail-info-margin">
    <div id="divDetailBodyScroll" class="div-fix-scroll">
        <div class="div-detail-table-title">■組付情報</div>
        <div style="clear: both; height: 10px;width:auto"></div>
        <div id="divGrvDisplay" runat="server" style="height:166px;margin-left:10px">
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
                                        <asp:TemplateField HeaderText="installDt" />
                                        <asp:TemplateField HeaderText="stationNm" />
                                        <asp:TemplateField HeaderText="partsSerial" />
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
                                        <asp:TemplateField HeaderText="partsKubotaNum" />
                                        <asp:TemplateField HeaderText="partsMakerNum" />
                                        <asp:TemplateField HeaderText="historyIndex" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <%-- 
        <div id="divMainListArea" class="div-auto-scroll" style="height: 166px">
            <asp:ListView ID="lstMainList" runat="server" OnItemDataBound="lstMainList_ItemDataBound">
                <LayoutTemplate>
                    <table id="itemPlaceholderContainer" runat="server" class="grid-layout" style="width: 730px; height:auto">
                        <tr id="headerMainContent" runat="server" class="listview-header_2r ui-state-default">
                            <th id="Th01" style="width: 150px" runat="server">組付日時</th>
                            <th id="Th02" style="width: 160px" runat="server">ステーション名</th>
                            <th id="Th03" style="width: 180px" runat="server">部品機番</th>
                            <th id="Th04" style="width: 120px" runat="server">クボタ品番</th>
                            <th id="Th05" style="width: 120px" runat="server">メーカー品番</th>
                            <th id="Th06" style="width:  80px" runat="server">来歴No</th>
                        </tr>
                        <tr class="" id="itemPlaceholder" runat="server">
                        </tr>
                    </table>
                </LayoutTemplate>
                <ItemTemplate>
                    <tr id="trRowData" runat="server" class="listview-row ui-widget">
                        <td>
                            <KTCC:KTTextBox ID="txtInstallDt" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                        </td>
                        <td>
                            <KTCC:KTTextBox ID="txtStationNm" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                        </td>
                        <td>
                            <KTCC:KTTextBox ID="txtPartsSerial" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                        </td>
                        <td>
                            <KTCC:KTTextBox ID="txtPartsKubotaNum" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                        </td>
                        <td>
                            <KTCC:KTTextBox ID="txtPartsMakerNum" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                        </td>
                        <td>
                            <KTCC:KTNumericTextBox ID="ntbHistoryIndex" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full txt-numeric" />
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:ListView>
        </div>
        --%>


        <div class="div-detail-table-title">■補正情報</div>
        <div style="clear: both; height: 10px;width:auto"></div>
        <div id="divGrvSubDisplay" runat="server" style="height:166px;margin-left:10px">
            <table class="table-layout-fix">
                <tr>
                    <td>
                        <div id="divLTSubScroll" class="div-fix-scroll div-left-grid">
                            <div id="divGrvHeaderLTSub" runat="server">
                                <asp:GridView ID="grvHeaderLTSub" runat="server" CssClass="grid-layout2 ui-widget-content" AllowPaging="false" AllowSorting="false" PageSize="100" AutoGenerateColumns="false">
                                    <HeaderStyle CssClass="grid-header ui-state-default" />
                                </asp:GridView>
                            </div>
                        </div>
                    </td>
                    <td>
                        <div id="divRTSubScroll" class="div-scroll-right-top div-right-grid">
                            <div id="divGrvHeaderRTSub" runat="server">
                                <asp:GridView ID="grvHeaderRTSub" runat="server" CssClass="grid-layout2 ui-widget-content" AllowPaging="false" AllowSorting="false" PageSize="100" AutoGenerateColumns="false">
                                    <HeaderStyle CssClass="grid-header-newline ui-state-default" />
                                </asp:GridView>
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div id="divLBSubScroll" class="div-scroll-left-bottom div-left-grid">
                            <div id="divGrvLBSub" runat="server">
                                <asp:GridView ID="grvMainViewLBSub" runat="server" ShowHeader="false" CssClass="grid-layout2 ui-widget-content" AllowPaging="false" AllowSorting="false" PageSize="100" AutoGenerateColumns="false" OnRowDataBound="grvMainViewLBSub_RowDataBound">
                                    <HeaderStyle CssClass="grid-header ui-state-default" />
                                    <RowStyle CssClass="grid-row ui-widget" />
                                    <SelectedRowStyle CssClass="ui-state-highlight" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="ntbCylinderNum" />
                                        <asp:TemplateField HeaderText="txtInspectionDt" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </td>
                    <td>
                        <div id="divRBSubScroll" class="div-visible-scroll div-right-grid">
                            <div id="divGrvRBSub" runat="server">
                                <asp:GridView ID="grvMainViewRBSub" runat="server" ShowHeader="false" CssClass="grid-layout2 ui-widget-content" AllowPaging="false" AllowSorting="false" PageSize="1000" AutoGenerateColumns="false" OnRowDataBound="grvMainViewRBSub_RowDataBound">
                                    <HeaderStyle CssClass="grid-header ui-state-default" />
                                    <RowStyle CssClass="grid-row ui-widget" />
                                    <SelectedRowStyle CssClass="ui-state-highlight" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="injectionQuantityAdjustVal" />
                                        <asp:TemplateField HeaderText="makerNm" />
                                        <asp:TemplateField HeaderText="injecterSerial" />
                                        <asp:TemplateField HeaderText="pressureLv" />
                                        <asp:TemplateField HeaderText="terminalNm" />
                                        <asp:TemplateField HeaderText="historyIndex" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <%-- 
        <div id="divSubListArea" class="div-auto-scroll" style="height: 100px">
            <asp:ListView ID="lstSubList" runat="server" OnItemDataBound="lstSubList_ItemDataBound">
                <LayoutTemplate>
                    <table id="itemPlaceholderContainer" runat="server" class="grid-layout" style="width: 730px; height:auto">
                        <tr id="headerMainContent" runat="server" class="listview-header_2r ui-state-default">
                            <th id="Th01" style="width:  60px" runat="server">気筒No</th>
                            <th id="Th02" style="width: 150px" runat="server">測定日時</th>
                            <th id="Th03" style="width: 240px" runat="server">噴射量<br />補正データ</th>
                            <th id="Th04" style="width:  80px" runat="server">メーカー</th>
                            <th id="Th05" style="width: 100px" runat="server">インジェクタ<br />製番</th>
                            <th id="Th06" style="width:  80px" runat="server">圧力水準</th>
                            <th id="Th07" style="width: 100px" runat="server">測定端末</th>
                            <th id="Th08" style="width:  80px" runat="server">来歴No</th>
                        </tr>
                        <tr class="" id="itemPlaceholder" runat="server">
                        </tr>
                    </table>
                </LayoutTemplate>
                <ItemTemplate>
                    <tr id="trRowData" runat="server" class="listview-row ui-widget">
                        <td>
                            <KTCC:KTNumericTextBox ID="ntbCylinderNum" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full txt-numeric" />
                        </td>
                        <td>
                            <KTCC:KTTextBox ID="txtInspectionDt" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                        </td>
                        <td>
                            <KTCC:KTTextBox ID="txtInjectionQuantityAdjustVal" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                        </td>
                        <td>
                            <KTCC:KTTextBox ID="txtMakerNm" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                        </td>
                        <td>
                            <KTCC:KTTextBox ID="txtInjecterSerial" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                        </td>
                        <td>
                            <KTCC:KTTextBox ID="txtPressureLv" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                        </td>
                        <td>
                            <KTCC:KTTextBox ID="txtTerminalNm" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                        </td>
                        <td>
                            <KTCC:KTNumericTextBox ID="ntbHistoryIndex" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full" />
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:ListView>
        </div>
        --%>
    </div>
</div>
