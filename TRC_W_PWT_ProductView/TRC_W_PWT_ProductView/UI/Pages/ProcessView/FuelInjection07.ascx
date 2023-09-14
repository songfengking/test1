<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FuelInjection07.ascx.cs" Inherits="TRC_W_PWT_ProductView.UI.Pages.ProcessView.FuelInjection07" %>
<%@ Import Namespace="TRC_W_PWT_ProductView.Common" %>

<%-- デザイン表示時使用 マスターページ使用時不要 --%>
<%--<link href="../../../CSS/cobalt/jquery-wijmo.css" rel="stylesheet" />
<link href="../../../CSS/Base.css" rel="stylesheet" />
<link href="../../../CSS/TRC.css" rel="stylesheet" />--%>

<%-- 画面CSS/スクリプト定義領域 --%>
<asp:PlaceHolder ID="SubScripts" runat="server">
    <script src="<%: PageUtils.ResolveUrl( this.Page, "~/Scripts/Pages/ContentScroll.js") %>" ></script>
</asp:PlaceHolder>

<div class="div-detail-info-margin">
    <div>
        <div class="div-detail-table-title">■来歴情報</div>
        <div id="divMainListArea" class="div-auto-scroll">
            <asp:ListView ID="lstMainList" runat="server" OnItemDataBound="lstMainList_ItemDataBound">
                <LayoutTemplate>
                    <div class="" id="itemPlaceholder" runat="server">
                    </div>
                </LayoutTemplate>
                <ItemTemplate>
                    <div class="ui-state-default div-detail-table-sub-title">
                        <span>【計測結果】</span>
                        <KTCC:KTTextBox ID="txtInspectionDt" runat="server" ReadOnly="true" CssClass="" />
                    </div>
                    <div class="div-detail-table-subarea">
                        <table class="table-border-layout grid-layout">
                            <colgroup>
                                <col style="width: 150px" />
                                <col style="width: 150px" />
                                <col style="width: 150px" />
                                <col style="width: 150px" />
                            </colgroup>
                            <tr class="listview-header_2r">
                                <td class="ui-state-default detailtable-header">ステーション</td>
                                <td>
                                    <KTCC:KTTextBox ID="txtStationNm" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                                </td>
                                <td class="ui-state-default detailtable-header">計測端末ホスト名</td>
                                <td>
                                    <KTCC:KTTextBox ID="txtMeasureTerminal" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                                </td>
                            </tr>
                            <tr class="listview-header_2r">
                                <td class="ui-state-default detailtable-header">検査担当<br />コード</td>
                                <td>
                                    <KTCC:KTTextBox ID="txtTesterCd" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                                </td>
                                <td class="ui-state-default detailtable-header">伝送判定</td>
                                <td>
                                    <KTCC:KTTextBox ID="txtTransferResult" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                                </td>
                            </tr>
                        </table>

                        <div class="div-detail-table-sub-item">[噴射条件]</div>
                        <table class="table-border-layout grid-layout">
                            <colgroup>
                                <col style="width: 150px" />
                                <col style="width: 150px" />
                                <col style="width: 150px" />
                                <col style="width: 150px" />
                            </colgroup>
                            <tr class="listview-header_2r">
                                <td class="ui-state-default detailtable-header">噴射ポンプ品番</td>
                                <td>
                                    <KTCC:KTTextBox ID="txtPumpNum" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                                </td>
                                <td class="ui-state-default detailtable-header">気温</td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbTemperature" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="1" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                            </tr>
                            <tr class="listview-header_2r">
                                <td class="ui-state-default detailtable-header">パルスタイミング<br />角度</td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbPulseTimingAngle" runat="server" ReadOnly="true" InputMode="FloatNumWithMinus" DecLen="1" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                                <td class="ui-state-default detailtable-header">噴射時期気筒間<br />誤差角度</td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbInjectionTimingVarianceAngle" runat="server" ReadOnly="true" InputMode="FloatNumWithMinus" DecLen="2" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                            </tr>
                        </table>

                        <div class="div-detail-table-sub-item">[噴射時期上下限値]</div>
                        <table class="table-border-layout grid-layout">
                            <colgroup>
                                <col style="width: 150px" />
                                <col style="width: 150px" />
                                <col style="width: 150px" />
                                <col style="width: 150px" />
                            </colgroup>
                            <tr class="listview-header_2r">
                                <td class="ui-state-default detailtable-header">上限</td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbInjectionTimingUpperLimit" runat="server" ReadOnly="true" InputMode="FloatNumWithMinus" DecLen="2" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                                <td class="ui-state-default detailtable-header">下限</td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbInjectionTimingLowerLimit" runat="server" ReadOnly="true" InputMode="FloatNumWithMinus" DecLen="2" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                            </tr>
                        </table>

                        <div class="div-detail-table-sub-item">[噴射時期実測値/判定]</div>
                        <table class="table-border-layout grid-layout">
                            <colgroup>
                                <col style="width: 150px" />
                                <col style="width: 150px" />
                                <col style="width: 150px" />
                                <col style="width: 150px" />
                            </colgroup>
                            <tr class="listview-header_2r">
                                <td class="ui-state-default detailtable-header">実測値1</td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbInjectionTimingVal1" runat="server" ReadOnly="true" InputMode="FloatNumWithMinus" DecLen="2" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                                <td class="ui-state-default detailtable-header">判定1</td>
                                <td>
                                    <KTCC:KTTextBox ID="txtInjectionTimingResult1" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                </td>
                            </tr>
                            <tr class="listview-header_2r">
                                <td class="ui-state-default detailtable-header">実測値2</td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbInjectionTimingVal2" runat="server" ReadOnly="true" InputMode="FloatNumWithMinus" DecLen="2" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                                <td class="ui-state-default detailtable-header">判定2</td>
                                <td>
                                    <KTCC:KTTextBox ID="txtInjectionTimingResult2" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                </td>
                            </tr>
                            <tr class="listview-header_2r">
                                <td class="ui-state-default detailtable-header">実測値3</td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbInjectionTimingVal3" runat="server" ReadOnly="true" InputMode="FloatNumWithMinus" DecLen="2" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                                <td class="ui-state-default detailtable-header">判定3</td>
                                <td>
                                    <KTCC:KTTextBox ID="txtInjectionTimingResult3" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                </td>
                            </tr>
                            <tr class="listview-header_2r">
                                <td class="ui-state-default detailtable-header">実測値4</td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbInjectionTimingVal4" runat="server" ReadOnly="true" InputMode="FloatNumWithMinus" DecLen="2" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                                <td class="ui-state-default detailtable-header">判定4</td>
                                <td>
                                    <KTCC:KTTextBox ID="txtInjectionTimingResult4" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                </td>
                            </tr>
                            <tr class="listview-header_2r">
                                <td class="ui-state-default detailtable-header">実測値平均</td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbInjectionTimingAveVal" runat="server" ReadOnly="true" InputMode="FloatNumWithMinus" DecLen="2" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                                <td class="ui-state-default detailtable-header">判定平均</td>
                                <td>
                                    <KTCC:KTTextBox ID="txtInjectionTimingAveResult" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                </td>
                            </tr>
                        </table>

                        <div class="div-detail-table-sub-item">[ピストン出代上下限値]</div>
                        <table class="table-border-layout grid-layout">
                            <colgroup>
                                <col style="width: 150px" />
                                <col style="width: 150px" />
                                <col style="width: 150px" />
                                <col style="width: 150px" />
                            </colgroup>
                            <tr class="listview-header_2r">
                                <td class="ui-state-default detailtable-header">ランク上</td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbPistonBumpRankUpper" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                                <td class="ui-state-default detailtable-header">ランク下</td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbPistonBumpRankLower" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                            </tr>
                            <tr class="listview-header_2r">
                                <td class="ui-state-default detailtable-header">実測値上限</td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbPistonBumpUpperLimit" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                                <td class="ui-state-default detailtable-header">実測値下限</td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbPistonBumpLowerLimit" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                            </tr>
                        </table>

                        <div class="div-detail-table-sub-item">[ピストン出代]</div>
                        <table class="table-border-layout grid-layout">
                            <colgroup>
                                <col style="width: 150px" />
                                <col style="width: 150px" />
                            </colgroup>
                            <tr class="listview-header_2r">
                                <td class="ui-state-default detailtable-header">ランク</td>
                                <td>
                                    <KTCC:KTTextBox ID="txtPistonBumpRank" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                </td>
                            </tr>
                            <tr class="listview-header_2r">
                                <td class="ui-state-default detailtable-header">実測値1</td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbPistonBumpVal1" runat="server" ReadOnly="true" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                            </tr>
                            <tr class="listview-header_2r">
                                <td class="ui-state-default detailtable-header">実測値2</td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbPistonBumpVal2" runat="server" ReadOnly="true" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                            </tr>
                            <tr class="listview-header_2r">
                                <td class="ui-state-default detailtable-header">実測値3</td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbPistonBumpVal3" runat="server" ReadOnly="true" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                            </tr>
                            <tr class="listview-header_2r">
                                <td class="ui-state-default detailtable-header">実測値4</td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbPistonBumpVal4" runat="server" ReadOnly="true" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                            </tr>
                            <tr class="listview-header_2r">
                                <td class="ui-state-default detailtable-header">実測値平均</td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbPistonBumpAveVal" runat="server" ReadOnly="true" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <br />
                </ItemTemplate>
            </asp:ListView>
        </div>
    </div>
</div>
