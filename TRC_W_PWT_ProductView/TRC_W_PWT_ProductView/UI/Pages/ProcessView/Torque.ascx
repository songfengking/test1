<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Torque.ascx.cs" Inherits="TRC_W_PWT_ProductView.UI.Pages.ProcessView.Torque" %>
<%@ Import Namespace="TRC_W_PWT_ProductView.Common" %>

<%-- デザイン表示時使用 マスターページ使用時不要 --%>
<%--<link href="../../../CSS/cobalt/jquery-wijmo.css" rel="stylesheet" />
<link href="../../../CSS/Base.css" rel="stylesheet" />
<link href="../../../CSS/TRC.css" rel="stylesheet" />--%>

<%-- 画面CSS/スクリプト定義領域 --%>
<asp:PlaceHolder ID="SubScripts" runat="server">
    <script src="<%: PageUtils.ResolveUrl( this.Page, "~/Scripts/Pages/ProcessView/Torque.js") %>" ></script>
</asp:PlaceHolder>

<div class="div-detail-info-margin">
    <div id="divDetailBodyScroll" class="div-fix-scroll">
        <div class="div-detail-table-title">■来歴情報</div>
        <div style="clear: both; height: 10px;width:auto"></div>

        <%-- 来歴情報定義 --%>
        <div id="divMainListArea" style="height: 150px">
            <table class="table-layout-fix" style="margin-left:10px">
                <tr>
                    <td>
                        <div id="divLTScroll" class="div-fix-scroll div-left-grid">
                            <div id="divHeaderLT" runat="server">
                                <table id="solidLTHeader" runat="server" class="grid-layout" style="width: 0px;">
                                    <tr id="headerLTMainContent" runat="server" class="listview-header_2r ui-state-default">
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </td>
                    <td>
                        <div id="divRTScroll" class="div-scroll-right-top div-right-grid">
                            <div id="divHeaderRT" runat="server">
                                <table id="solidRTHeader" runat="server" class="grid-layout" style="width: 720px;">
                                    <tr id="headerRTMainContent"    runat="server" class="listview-header_2r ui-state-default">
                                        <th id="partsNm"      style="width: 200px" runat="server">部品名</th>
                                        <th id="inspectionDt" style="width: 150px" runat="server">計測日時</th>
                                        <th id="terminalNm"   style="width: 120px" runat="server">端末名</th>
                                        <th id="historyIndex" style="width:  80px" runat="server">最終来歴</th>
                                        <th id="result"       style="width:  80px" runat="server">結果</th>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div id="divLBScroll" class="div-scroll-left-bottom div-left-grid">
                            <div id="divGrvLB" runat="server">
                                <asp:ListView ID="lstMainListLB" runat="server" OnItemDataBound="lstMainListLB_ItemDataBound" OnSelectedIndexChanging="lstMainListLB_SelectedIndexChanging" OnSelectedIndexChanged="lstMainListLB_SelectedIndexChanged">
                                    <LayoutTemplate>
                                        <table id="itemPlaceholderContainerLB" runat="server" class="grid-layout" style="width:0px">
                                            <tr id="headerLBContent" runat="server" class="listview-header_3r ui-state-default">
                                            </tr>
                                            <tr id="itemPlaceholder" runat="server">
                                            </tr>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr id="trRowData" runat="server" class="listview-row ui-widget">
                                        </tr>
                                    </ItemTemplate>
                                </asp:ListView>
                            </div>
                        </div>
                    </td>
                    <td>
                        <div id="divRBScroll" class="div-visible-scroll div-right-grid">
                            <div id="divGrvRB" runat="server" style="height: 90px">
                                <asp:ListView ID="lstMainListRB" runat="server" OnItemDataBound="lstMainListRB_ItemDataBound" OnSelectedIndexChanging="lstMainListRB_SelectedIndexChanging" OnSelectedIndexChanged="lstMainListRB_SelectedIndexChanged">
                                    <LayoutTemplate>
                                        <table id="itemPlaceholderContainerRB" runat="server" class="grid-layout" style="width: 720px">
                                            <tr id="headerRBContent"   runat="server" class="listview-header_3r ui-state-default">
                                                <th id="partsNm"         runat="server" style="width:200px"/>
                                                <th id="inspectionDt"    runat="server" style="width:150px"/>
                                                <th id="terminalNm"      runat="server" style="width:120px"/>
                                                <th id="historyIndex"    runat="server" style="width: 80px"/>
                                                <th id="result"          runat="server" style="width: 80px"/>
                                            </tr>
                                            <tr id="itemPlaceholder" runat="server">
                                            </tr>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr id="trRowData" runat="server" class="listview-row ui-widget">
                                            <td>
                                                <asp:Button ID="btnSelect" runat="server" CommandName="Select" Text="" CssClass="btnSelect invisible" />
                                                <KTCC:KTTextBox ID="txtPartsNm" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" ></KTCC:KTTextBox>
                                            </td>
                                            <td>
                                                <KTCC:KTTextBox ID="txtInspectionDt" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                            </td>
                                            <td>
                                                <KTCC:KTTextBox ID="txtTerminalNm" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                                            </td>
                                            <td>
                                                <KTCC:KTNumericTextBox ID="ntbHistoryIndex" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                            </td>
                                            <td>
                                                <KTCC:KTTextBox ID="txtResult" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:ListView>
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
        </div>

        <div id="divSubListArea" class="div-auto-scroll">
            <asp:ListView ID="lstSubList" runat="server" OnItemDataBound="lstSubList_ItemDataBound">
                <LayoutTemplate>
                    <div class="" id="itemPlaceholder" runat="server">
                    </div>
                </LayoutTemplate>
                <ItemTemplate>
                    <div class="ui-state-default div-detail-table-sub-title">
                        <span>【計測結果</span><KTCC:KTNumericTextBox ID="ntbHistoryIndex" runat="server" ReadOnly="true" CssClass="" /><span>】</span>
                        　    <KTCC:KTTextBox ID="txtInspectionDt" runat="server" ReadOnly="true" CssClass="" />
                        　    <KTCC:KTTextBox ID="txtTerminalNm" runat="server" ReadOnly="true" CssClass="" />
                    </div>
                    <div style="clear: both; width:auto;margin-top:10px"></div>
                    <div class="div-detail-table-subarea">
                        <table class="table-border-layout grid-layout">
                            <colgroup>
                                <col style="width: 200px" />
                                <col style="width:  80px" />
                                <col style="width:  80px" span="12" />
                            </colgroup>
                            <%-- ヘッダ --%>
                            <tr class="listview-header_2r ui-state-default">
                                <th colspan="2">部品名</th>
                                <th>上限値</th>
                                <th>下限値</th>
                                <th>計測値1</th>
                                <th>計測値2</th>
                                <th>計測値3</th>
                                <th>計測値4</th>
                                <th>計測値5</th>
                                <th>計測値6</th>
                                <th>計測値7</th>
                                <th>計測値8</th>
                                <th>計測値9</th>
                                <th>計測値10</th>
                            </tr>
                            <%-- 計測値[トルク] --%>
                            <tr class="listview-row ui-widget">
                                <td rowspan="2">
                                    <KTCC:KTTextBox ID="txtPartsNm" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                                </td>
                                <td class="font-default txt-default txt-width-short al-lf">トルク(N･m)</td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbUpperLimit"   runat="server" ReadOnly="true" DecLen="2" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbLowerLimit"   runat="server" ReadOnly="true" DecLen="2" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbMeasureVal1"  runat="server" ReadOnly="true" DecLen="2" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbMeasureVal2"  runat="server" ReadOnly="true" DecLen="2" CssClass="font-default txt-default txt-width-full txt-numeric" />
                                </td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbMeasureVal3"  runat="server" ReadOnly="true" DecLen="2" CssClass="font-default txt-default txt-width-full txt-numeric" />
                                </td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbMeasureVal4"  runat="server" ReadOnly="true" DecLen="2" CssClass="font-default txt-default txt-width-full txt-numeric" />
                                </td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbMeasureVal5"  runat="server" ReadOnly="true" DecLen="2" CssClass="font-default txt-default txt-width-full txt-numeric" />
                                </td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbMeasureVal6"  runat="server" ReadOnly="true" DecLen="2" CssClass="font-default txt-default txt-width-full txt-numeric" />
                                </td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbMeasureVal7"  runat="server" ReadOnly="true" DecLen="2" CssClass="font-default txt-default txt-width-full txt-numeric" />
                                </td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbMeasureVal8"  runat="server" ReadOnly="true" DecLen="2" CssClass="font-default txt-default txt-width-full txt-numeric" />
                                </td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbMeasureVal9"  runat="server" ReadOnly="true" DecLen="2" CssClass="font-default txt-default txt-width-full txt-numeric" />
                                </td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbMeasureVal10" runat="server" ReadOnly="true" DecLen="2" CssClass="font-default txt-default txt-width-full txt-numeric" />
                                </td>
                            </tr>
                            <%-- 計測値[角度] --%>
                            <tr class="listview-row ui-widget">
                                <td class="font-default txt-default txt-width-short al-lf">角度(°)</td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbAngleUpperLimit"   runat="server" ReadOnly="true" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbAngleLowerLimit"   runat="server" ReadOnly="true" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbAngleMeasureVal1"  runat="server" ReadOnly="true" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbAngleMeasureVal2"  runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full txt-numeric" />
                                </td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbAngleMeasureVal3"  runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full txt-numeric" />
                                </td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbAngleMeasureVal4"  runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full txt-numeric" />
                                </td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbAngleMeasureVal5"  runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full txt-numeric" />
                                </td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbAngleMeasureVal6"  runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full txt-numeric" />
                                </td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbAngleMeasureVal7"  runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full txt-numeric" />
                                </td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbAngleMeasureVal8"  runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full txt-numeric" />
                                </td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbAngleMeasureVal9"  runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full txt-numeric" />
                                </td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbAngleMeasureVal10" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full txt-numeric" />
                                </td>
                            </tr>
                            <%-- 2度締め確認[トルク] --%>
                            <tr class="listview-row ui-widget">
                                <td class="font-default txt-default al-lf" rowspan="2">2度締め確認</td>
                                <td class="font-default txt-default txt-width-short al-lf">トルク(N･m)</td>
                                <td id="tdTwiceUpperLimit" runat="server">
                                    <KTCC:KTDecimalTextBox ID="ntbTwiceUpperLimit"   runat="server" ReadOnly="true" DecLen="2" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                                <td id="tdTwiceLowerLimit" runat="server">
                                    <KTCC:KTDecimalTextBox ID="ntbTwiceLowerLimit"   runat="server" ReadOnly="true" DecLen="2" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                                <td id="tdTwiceMeasureVal1" runat="server">
                                    <KTCC:KTDecimalTextBox ID="ntbTwiceMeasureVal1"  runat="server" ReadOnly="true" DecLen="2" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                                <td id="tdTwiceMeasureVal2" runat="server">
                                    <KTCC:KTDecimalTextBox ID="ntbTwiceMeasureVal2"  runat="server" ReadOnly="true" DecLen="2" CssClass="font-default txt-default txt-width-full txt-numeric" />
                                </td>
                                <td id="tdTwiceMeasureVal3" runat="server">
                                    <KTCC:KTDecimalTextBox ID="ntbTwiceMeasureVal3"  runat="server" ReadOnly="true" DecLen="2" CssClass="font-default txt-default txt-width-full txt-numeric" />
                                </td>
                                <td id="tdTwiceMeasureVal4" runat="server">
                                    <KTCC:KTDecimalTextBox ID="ntbTwiceMeasureVal4"  runat="server" ReadOnly="true" DecLen="2" CssClass="font-default txt-default txt-width-full txt-numeric" />
                                </td>
                                <td id="tdTwiceMeasureVal5" runat="server">
                                    <KTCC:KTDecimalTextBox ID="ntbTwiceMeasureVal5"  runat="server" ReadOnly="true" DecLen="2" CssClass="font-default txt-default txt-width-full txt-numeric" />
                                </td>
                                <td id="tdTwiceMeasureVal6" runat="server">
                                    <KTCC:KTDecimalTextBox ID="ntbTwiceMeasureVal6"  runat="server" ReadOnly="true" DecLen="2" CssClass="font-default txt-default txt-width-full txt-numeric" />
                                </td>
                                <td id="tdTwiceMeasureVal7" runat="server">
                                    <KTCC:KTDecimalTextBox ID="ntbTwiceMeasureVal7"  runat="server" ReadOnly="true" DecLen="2" CssClass="font-default txt-default txt-width-full txt-numeric" />
                                </td>
                                <td id="tdTwiceMeasureVal8" runat="server">
                                    <KTCC:KTDecimalTextBox ID="ntbTwiceMeasureVal8"  runat="server" ReadOnly="true" DecLen="2" CssClass="font-default txt-default txt-width-full txt-numeric" />
                                </td>
                                <td id="tdTwiceMeasureVal9" runat="server">
                                    <KTCC:KTDecimalTextBox ID="ntbTwiceMeasureVal9"  runat="server" ReadOnly="true" DecLen="2" CssClass="font-default txt-default txt-width-full txt-numeric" />
                                </td>
                                <td id="tdTwiceMeasureVal10" runat="server">
                                    <KTCC:KTDecimalTextBox ID="ntbTwiceMeasureVal10" runat="server" ReadOnly="true" DecLen="2" CssClass="font-default txt-default txt-width-full txt-numeric" />
                                </td>
                            </tr>
                            <%-- 2度締め確認[角度] --%>
                            <tr class="listview-row ui-widget">
                                <td class="font-default txt-default txt-width-short al-lf">角度(°)</td>
                                <td id="tdTwiceAngleUpperLimit" runat="server">
                                    <KTCC:KTDecimalTextBox ID="ntbTwiceAngleUpperLimit"   runat="server" ReadOnly="true" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                                <td id="tdTwiceAngleLowerLimit" runat="server">
                                    <KTCC:KTDecimalTextBox ID="ntbTwiceAngleLowerLimit"   runat="server" ReadOnly="true" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                                <td id="tdTwiceAngleMeasureVal1" runat="server">
                                    <KTCC:KTDecimalTextBox ID="ntbTwiceAngleMeasureVal1"  runat="server" ReadOnly="true" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                                <td id="tdTwiceAngleMeasureVal2" runat="server">
                                    <KTCC:KTDecimalTextBox ID="ntbTwiceAngleMeasureVal2"  runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full txt-numeric" />
                                </td>
                                <td id="tdTwiceAngleMeasureVal3" runat="server">
                                    <KTCC:KTDecimalTextBox ID="ntbTwiceAngleMeasureVal3"  runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full txt-numeric" />
                                </td>
                                <td id="tdTwiceAngleMeasureVal4" runat="server">
                                    <KTCC:KTDecimalTextBox ID="ntbTwiceAngleMeasureVal4"  runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full txt-numeric" />
                                </td>
                                <td id="tdTwiceAngleMeasureVal5" runat="server">
                                    <KTCC:KTDecimalTextBox ID="ntbTwiceAngleMeasureVal5"  runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full txt-numeric" />
                                </td>
                                <td id="tdTwiceAngleMeasureVal6" runat="server">
                                    <KTCC:KTDecimalTextBox ID="ntbTwiceAngleMeasureVal6"  runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full txt-numeric" />
                                </td>
                                <td id="tdTwiceAngleMeasureVal7" runat="server">
                                    <KTCC:KTDecimalTextBox ID="ntbTwiceAngleMeasureVal7"  runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full txt-numeric" />
                                </td>
                                <td id="tdTwiceAngleMeasureVal8" runat="server">
                                    <KTCC:KTDecimalTextBox ID="ntbTwiceAngleMeasureVal8"  runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full txt-numeric" />
                                </td>
                                <td id="tdTwiceAngleMeasureVal9" runat="server">
                                    <KTCC:KTDecimalTextBox ID="ntbTwiceAngleMeasureVal9"  runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full txt-numeric" />
                                </td>
                                <td id="tdTwiceAngleMeasureVal10" runat="server">
                                    <KTCC:KTDecimalTextBox ID="ntbTwiceAngleMeasureVal10" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full txt-numeric" />
                                </td>
                            </tr>
                        </table>
                    </div>

                </ItemTemplate>
            </asp:ListView>
        </div>
    </div>
</div>
