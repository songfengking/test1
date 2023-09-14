<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FuelInjection03.ascx.cs" Inherits="TRC_W_PWT_ProductView.UI.Pages.ProcessView.FuelInjection03" %>
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
                                <td class="ui-state-default detailtable-header">エンジン刻印名</td>
                                <td>
                                    <KTCC:KTTextBox ID="txtCarvedSealNm" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                                </td>
                                <td class="ui-state-default detailtable-header">総合判定</td>
                                <td>
                                    <KTCC:KTTextBox ID="txtResult" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                                </td>
                            </tr>
                            <tr class="listview-header_2r">
                                <td class="ui-state-default detailtable-header">気筒数</td>
                                <td>
                                    <KTCC:KTNumericTextBox ID="ntbCylinderQty" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-numeric txt-width-full al-lf" />
                                </td>
                                <td class="ui-state-default detailtable-header">ストローク</td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbStroke" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="1" CssClass="font-default txt-default txt-numeric txt-width-full al-lf" />
                                </td>
                            </tr>
                            <tr class="listview-header_2r">
                                <td class="ui-state-default detailtable-header">ボア</td>
                                <td>
                                    <KTCC:KTNumericTextBox ID="ntbBore" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-numeric txt-width-full al-lf" />
                                </td>
                                <td class="ui-state-default detailtable-header">燃焼方式</td>
                                <td>
                                    <KTCC:KTTextBox ID="txtCombustion" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                                </td>
                            </tr>
                            <tr class="listview-header_2r">
                                <td class="ui-state-default detailtable-header">進角調整</td>
                                <td>
                                    <KTCC:KTTextBox ID="txtAdvanceAdjust" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                                </td>
                                <td class="ui-state-default detailtable-header">ポンプネジ</td>
                                <td>
                                    <KTCC:KTTextBox ID="txtPumpScrew" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                                </td>
                            </tr>
                            <tr class="listview-header_2r">
                                <td class="ui-state-default detailtable-header">ラック位置</td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbRackPosition" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="1" CssClass="font-default txt-default txt-numeric txt-width-full al-lf" />
                                </td>
                                <td class="ui-state-default detailtable-header">ラック寸法差</td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbRackSizeDiff" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="1" CssClass="font-default txt-default txt-numeric txt-width-full al-lf" />
                                </td>
                            </tr>
                            <tr class="listview-header_2r">
                                <td class="ui-state-default detailtable-header">測定パス</td>
                                <td>
                                    <KTCC:KTTextBox ID="txtMeasurementPathTyp" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr class="listview-header_2r">
                                <td class="ui-state-default detailtable-header">ガスケット対象</td>
                                <td>
                                    <KTCC:KTTextBox ID="txtGasketTyp" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                                </td>
                                <td class="ui-state-default detailtable-header">標準ガスケット<br />品番</td>
                                <td>
                                    <KTCC:KTTextBox ID="txtGasketNum" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                                </td>
                            </tr>
                            <tr class="listview-header_2r">
                                <td class="ui-state-default detailtable-header">ガスケット寸法</td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbGasketSize" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="2" CssClass="font-default txt-default txt-numeric txt-width-full al-lf" />
                                </td>
                                <td class="ui-state-default detailtable-header">選定ガスケット<br />部品番号</td>
                                <td>
                                    <KTCC:KTTextBox ID="txtSelectedGasketNum" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                                </td>
                            </tr>
                            <tr class="listview-header_2r">
                                <td class="ui-state-default detailtable-header">燃焼室リセス</td>
                                <td>
                                    <KTCC:KTTextBox ID="txtCombustionRecess" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>
                        </table>

                       <div class="div-detail-table-sub-item">[エラーメッセージ]</div>
                         <table class="table-border-layout grid-layout">
                            <colgroup>
                                <col style="width: 150px" />
                                <col style="width: 450px" />
                            </colgroup>
                            <tr class="listview-header_2r">
                                <td class="ui-state-default detailtable-header">メッセージ1</td>
                                <td>
                                    <KTCC:KTTextBox ID="txtErrorMessage1" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full" />
                                </td>
                            </tr>
                             <tr class="listview-header_2r">
                                <td class="ui-state-default detailtable-header">メッセージ2</td>
                                <td>
                                    <KTCC:KTTextBox ID="txtErrorMessage2" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full" />
                                </td>
                            </tr>
                             <tr class="listview-header_2r">
                                <td class="ui-state-default detailtable-header">メッセージ3</td>
                                <td>
                                    <KTCC:KTTextBox ID="txtErrorMessage3" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full" />
                                </td>
                            </tr>
                        </table>

                        <div class="div-detail-table-sub-item">[ピストン出代]</div>
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
                                    <KTCC:KTDecimalTextBox ID="ntbPistonBumpUpperLimit" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="2" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                                <td class="ui-state-default detailtable-header">下限</td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbPistonBumpLowerLimit" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="2" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                            </tr>
                            <tr class="listview-header_2r">
                                <td class="ui-state-default detailtable-header">最大値</td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbBumpMaxVal" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                                <td class="ui-state-default detailtable-header">最小値</td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbBumpMinVal" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                            </tr>
                            <tr class="listview-header_2r">
                                <td class="ui-state-default detailtable-header">平均値</td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbBumpAveVal" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                                <td class="ui-state-default detailtable-header">寸法バラツキ<br />基準値</td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbBumpSizeBaseVal" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="2" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                            </tr>
                            <tr class="listview-header_2r">
                                <td class="ui-state-default detailtable-header">寸法誤差</td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbBumpSizeVariance" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="2" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr class="listview-header_2r">
                                <td class="ui-state-default detailtable-header">気筒1</td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbPiston01Bump" runat="server" ReadOnly="true" InputMode="FloatNumWithMinus" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr class="listview-header_2r">
                                <td class="ui-state-default detailtable-header">気筒2</td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbPiston02Bump" runat="server" ReadOnly="true" InputMode="FloatNumWithMinus" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr class="listview-header_2r">
                                <td class="ui-state-default detailtable-header">気筒3</td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbPiston03Bump" runat="server" ReadOnly="true" InputMode="FloatNumWithMinus" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr class="listview-header_2r">
                                <td class="ui-state-default detailtable-header">気筒4</td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbPiston04Bump" runat="server" ReadOnly="true" InputMode="FloatNumWithMinus" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>
                        </table>

                        <div class="div-detail-table-sub-item">[トップクリアランス]</div>
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
                                    <KTCC:KTDecimalTextBox ID="ntbTopClearanceUpperLimit" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="2" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                                <td class="ui-state-default detailtable-header">下限</td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbTopClearanceLowerLimit" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="2" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                            </tr>
                            <tr class="listview-header_2r">
                                <td class="ui-state-default detailtable-header">気筒1</td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbTopClearancePiston01" runat="server" ReadOnly="true" InputMode="FloatNumWithMinus" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr class="listview-header_2r">
                                <td class="ui-state-default detailtable-header">気筒2</td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbTopClearancePiston02" runat="server" ReadOnly="true" InputMode="FloatNumWithMinus" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr class="listview-header_2r">
                                <td class="ui-state-default detailtable-header">気筒3</td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbTopClearancePiston03" runat="server" ReadOnly="true" InputMode="FloatNumWithMinus" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr class="listview-header_2r">
                                <td class="ui-state-default detailtable-header">気筒4</td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbTopClearancePiston04" runat="server" ReadOnly="true" InputMode="FloatNumWithMinus" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>
                        </table>

                        <div class="div-detail-table-sub-item">[燃料噴射時期]</div>
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
                                    <KTCC:KTDecimalTextBox ID="ntbInjectionTimingUpperLimit" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="2" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                                <td class="ui-state-default detailtable-header">下限</td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbInjectionTimingLowerLimit" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="2" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                            </tr>
                            <tr class="listview-header_2r">
                                <td class="ui-state-default detailtable-header">噴射時期補正値</td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbInjectionTimingAdjustVal" runat="server" ReadOnly="true" InputMode="FloatNumWithMinus" DecLen="1" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr class="listview-header_2r">
                                <td class="ui-state-default detailtable-header">気筒1</td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbInjectionTimingPiston01" runat="server" ReadOnly="true" InputMode="FloatNumWithMinus" DecLen="2" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr class="listview-header_2r">
                                <td class="ui-state-default detailtable-header">気筒2</td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbInjectionTimingPiston02" runat="server" ReadOnly="true" InputMode="FloatNumWithMinus" DecLen="2" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr class="listview-header_2r">
                                <td class="ui-state-default detailtable-header">気筒3</td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbInjectionTimingPiston03" runat="server" ReadOnly="true" InputMode="FloatNumWithMinus" DecLen="2" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr class="listview-header_2r">
                                <td class="ui-state-default detailtable-header">気筒4</td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbInjectionTimingPiston04" runat="server" ReadOnly="true" InputMode="FloatNumWithMinus" DecLen="2" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>
                        </table>

                        <div class="div-detail-table-sub-item">[シム]</div>
                        <table class="table-border-layout grid-layout">
                            <colgroup>
                                <col style="width: 150px" />
                                <col style="width: 150px" />
                                <col style="width: 150px" />
                                <col style="width: 150px" />
                            </colgroup>
                            <tr class="listview-header_2r">
                                <td class="ui-state-default detailtable-header">基準ポンプシム<br />寸法</td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbPumpShimSize" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="2" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr class="listview-header_2r">
                                <td class="ui-state-default detailtable-header">セットシム量上限</td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbSymUpperLimit" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="2" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                                <td class="ui-state-default detailtable-header">セットシム量下限</td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbSymLowerLimit" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="2" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                            </tr>
                        </table>

                        <div class="div-detail-table-sub-item">[シム枚数]</div>
                        <table class="table-border-layout grid-layout">
                            <colgroup>
                                <col style="width: 150px" />
                                <col style="width: 150px" />
                            </colgroup>
                            <tr class="listview-header_2r">
                                <td class="ui-state-default detailtable-header">0.175mm</td>
                                <td>
                                    <KTCC:KTNumericTextBox ID="ntbSymQty175" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                            </tr>
                            <tr class="listview-header_2r">
                                <td class="ui-state-default detailtable-header">0.2mm</td>
                                <td>
                                    <KTCC:KTNumericTextBox ID="ntbSymQty200" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                            </tr>
                            <tr class="listview-header_2r">
                                <td class="ui-state-default detailtable-header">0.25mm</td>
                                <td>
                                    <KTCC:KTNumericTextBox ID="ntbSymQty250" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                            </tr>
                            <tr class="listview-header_2r">
                                <td class="ui-state-default detailtable-header">0.3mm</td>
                                <td>
                                    <KTCC:KTNumericTextBox ID="ntbSymQty300" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                            </tr>
                            <tr class="listview-header_2r">
                                <td class="ui-state-default detailtable-header">0.35mm</td>
                                <td>
                                    <KTCC:KTNumericTextBox ID="ntbSymQty350" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                            </tr>
                        </table>

                        <div class="div-detail-table-sub-item">[測定]</div>
                        <table class="table-border-layout grid-layout">
                            <colgroup>
                                <col style="width: 150px" />
                                <col style="width: 150px" />
                                <col style="width: 150px" />
                                <col style="width: 150px" />
                                <col style="width: 150px" />
                                <col style="width: 150px" />
                            </colgroup>
                            <tr class="listview-header_2r">
                                <td class="ui-state-default detailtable-header">測定点数</td>
                                <td>
                                    <KTCC:KTNumericTextBox ID="ntbMeasurementQty" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                                <td class="ui-state-default detailtable-header">測定号機</td>
                                <td>
                                    <KTCC:KTNumericTextBox ID="ntbMeasureTerminal" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr class="listview-header_2r">
                                <td class="ui-state-default detailtable-header">測定方法気筒</td>
                                <td>
                                    <KTCC:KTTextBox ID="txtMeasureMethodPiston" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                                <td class="ui-state-default detailtable-header">測定方法方向</td>
                                <td>
                                    <KTCC:KTTextBox ID="txtMeasureMethodDirection" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-numeric txt-width-full" /> 
                                </td>
                                <td class="ui-state-default detailtable-header">測定補正値</td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbMeasureAdjustVal" runat="server" ReadOnly="true" InputMode="FloatNumWithMinus" DecLen="2" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                            </tr>
                            <tr class="listview-header_2r">
                                <td class="ui-state-default detailtable-header">治具補正1</td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbJigAdjust1" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                                <td class="ui-state-default detailtable-header">治具補正2</td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbJigAdjust2" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                                <td class="ui-state-default detailtable-header">治具補正3</td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbJigAdjust3" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                            </tr>
                        </table>

                        <div class="div-detail-table-sub-item">[測定値]</div>
                        <table class="table-border-layout grid-layout">
                            <colgroup>
                                <col style="width: 150px" />
                                <col style="width: 150px" />
                                <col style="width: 150px" />
                                <col style="width: 150px" />
                            </colgroup>
                            <tr class="listview-header_2r">
                                <td class="ui-state-default detailtable-header">生1気筒1</td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbMeasurePiston01_1" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                                <td class="ui-state-default detailtable-header">補正値1気筒1</td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbAdjustPiston01_1" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                            </tr>
                            <tr class="listview-header_2r">
                                <td class="ui-state-default detailtable-header">生1気筒2</td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbMeasurePiston01_2" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                                <td class="ui-state-default detailtable-header">補正値1気筒2</td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbAdjustPiston01_2" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                            </tr>
                            <tr class="listview-header_2r">
                                <td class="ui-state-default detailtable-header">生1気筒3</td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbMeasurePiston01_3" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                                <td class="ui-state-default detailtable-header">補正値1気筒3</td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbAdjustPiston01_3" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                            </tr>
                            <tr class="listview-header_2r">
                                <td class="ui-state-default detailtable-header">生1気筒4</td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbMeasurePiston01_4" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                                <td class="ui-state-default detailtable-header">補正値1気筒4</td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbAdjustPiston01_4" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                            </tr>
                            <tr class="listview-header_2r">
                                <td class="ui-state-default detailtable-header">生2気筒1</td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbMeasurePiston02_1" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                                <td class="ui-state-default detailtable-header">補正値2気筒1</td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbAdjustPiston02_1" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                            </tr>
                            <tr class="listview-header_2r">
                                <td class="ui-state-default detailtable-header">生2気筒2</td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbMeasurePiston02_2" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                                <td class="ui-state-default detailtable-header">補正値2気筒2</td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbAdjustPiston02_2" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                            </tr>
                            <tr class="listview-header_2r">
                                <td class="ui-state-default detailtable-header">生2気筒3</td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbMeasurePiston02_3" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                                <td class="ui-state-default detailtable-header">補正値2気筒3</td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbAdjustPiston02_3" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                            </tr>
                            <tr class="listview-header_2r">
                                <td class="ui-state-default detailtable-header">生2気筒4</td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbMeasurePiston02_4" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                                <td class="ui-state-default detailtable-header">補正値2気筒4</td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbAdjustPiston02_4" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                            </tr>
                            <tr class="listview-header_2r">
                                <td class="ui-state-default detailtable-header">生3気筒1</td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbMeasurePiston03_1" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                                <td class="ui-state-default detailtable-header">補正値3気筒1</td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbAdjustPiston03_1" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                            </tr>
                            <tr class="listview-header_2r">
                                <td class="ui-state-default detailtable-header">生3気筒2</td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbMeasurePiston03_2" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                                <td class="ui-state-default detailtable-header">補正値3気筒2</td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbAdjustPiston03_2" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                            </tr>
                            <tr class="listview-header_2r">
                                <td class="ui-state-default detailtable-header">生3気筒3</td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbMeasurePiston03_3" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                                <td class="ui-state-default detailtable-header">補正値3気筒3</td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbAdjustPiston03_3" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                            </tr>
                            <tr class="listview-header_2r">
                                <td class="ui-state-default detailtable-header">生3気筒4</td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbMeasurePiston03_4" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                                <td class="ui-state-default detailtable-header">補正値3気筒4</td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbAdjustPiston03_4" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                            </tr>
                            <tr class="listview-header_2r">
                                <td class="ui-state-default detailtable-header">生4気筒1</td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbMeasurePiston04_1" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                                <td class="ui-state-default detailtable-header">補正値4気筒1</td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbAdjustPiston04_1" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                            </tr>
                            <tr class="listview-header_2r">
                                <td class="ui-state-default detailtable-header">生4気筒2</td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbMeasurePiston04_2" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                                <td class="ui-state-default detailtable-header">補正値4気筒2</td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbAdjustPiston04_2" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                            </tr>
                            <tr class="listview-header_2r">
                                <td class="ui-state-default detailtable-header">生4気筒3</td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbMeasurePiston04_3" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                                <td class="ui-state-default detailtable-header">補正値4気筒3</td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbAdjustPiston04_3" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                            </tr>
                            <tr class="listview-header_2r">
                                <td class="ui-state-default detailtable-header">生4気筒4</td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbMeasurePiston04_4" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                                <td class="ui-state-default detailtable-header">補正値4気筒4</td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbAdjustPiston04_4" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                            </tr>
                        </table>

                        <div class="div-detail-table-sub-item">[ＦＩＴ補正値]</div>
                        <table class="table-border-layout grid-layout">
                            <colgroup>
                                <col style="width: 150px" />
                                <col style="width: 150px" />
                            </colgroup>
                            <tr class="listview-header_2r">
                                <td class="ui-state-default detailtable-header">平均値</td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbFitAdjustAveVal" runat="server" ReadOnly="true" InputMode="FloatNumWithMinus" DecLen="2" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                            </tr>
                            <tr class="listview-header_2r">
                                <td class="ui-state-default detailtable-header">気筒1</td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbFitAdjustPiston01" runat="server" ReadOnly="true" InputMode="FloatNumWithMinus" DecLen="2" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                            </tr>
                            <tr class="listview-header_2r">
                                <td class="ui-state-default detailtable-header">気筒2</td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbFitAdjustPiston02" runat="server" ReadOnly="true" InputMode="FloatNumWithMinus" DecLen="2" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                            </tr>
                            <tr class="listview-header_2r">
                                <td class="ui-state-default detailtable-header">気筒3</td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbFitAdjustPiston03" runat="server" ReadOnly="true" InputMode="FloatNumWithMinus" DecLen="2" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                            </tr>
                            <tr class="listview-header_2r">
                                <td class="ui-state-default detailtable-header">気筒4</td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="ntbFitAdjustPiston04" runat="server" ReadOnly="true" InputMode="FloatNumWithMinus" DecLen="2" CssClass="font-default txt-default txt-numeric txt-width-full" />
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