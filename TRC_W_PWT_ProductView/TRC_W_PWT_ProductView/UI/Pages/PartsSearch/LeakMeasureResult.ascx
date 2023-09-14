<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LeakMeasureResult.ascx.cs" Inherits="TRC_W_PWT_ProductView.UI.Pages.PartsSearch.LeakMeasureResult" %>
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
                        <KTCC:KTTextBox ID="txtMeasureDt" runat="server" ReadOnly="true" CssClass="" />
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
                                    <KTCC:KTTextBox ID="txtStationCd" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                                </td>
                                <td class="ui-state-default detailtable-header">総合判定</td>
                                <td>
                                    <KTCC:KTTextBox ID="txtTotalJudge" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                                </td>
                            </tr>
                            <tr class="listview-header_2r">
                                <td class="ui-state-default detailtable-header">履歴NO</td>
                                <td>
                                    <KTCC:KTTextBox ID="txtRecordNum" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                                </td>
                                <td class="ui-state-default detailtable-header">計測器</td>
                                <td>
                                    <KTCC:KTTextBox ID="txtMeasureTester" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                                </td>
                            </tr>
                        </table>

                        <div class="div-detail-table-sub-item">[リーク圧力]</div>
                        <table class="table-border-layout grid-layout">
                            <colgroup>
                                <col style="width: 150px" />
                                <col style="width: 150px" />
                                <col style="width: 150px" />
                                <col style="width: 150px" />
                            </colgroup>
                            <tr class="listview-header_2r">
                                <td class="ui-state-default detailtable-header">実績<br />(kPa)</td>
                                <td>
                                    <KTCC:KTTextBox ID="txtLeakPressure" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                                </td>
                                <td class="ui-state-default detailtable-header">判定</td>
                                <td>
                                    <KTCC:KTTextBox ID="txtLeakPressureJudge" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                                </td>
                            </tr>
                        </table>
                        
                        <div class="div-detail-table-sub-item">[リーク流量]</div>
                        <table class="table-border-layout grid-layout">    
                            <colgroup>
                                <col style="width: 150px" />
                                <col style="width: 150px" />
                                <col style="width: 150px" />
                                <col style="width: 150px" />
                            </colgroup>                    
                            <tr class="listview-header_2r">
                                <td class="ui-state-default detailtable-header">流量<br />(cc/min)</td>
                                <td>
                                    <KTCC:KTTextBox ID="txtLeakFlowRate" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                                </td>
                                <td class="ui-state-default detailtable-header">判定</td>
                                <td>
                                    <KTCC:KTTextBox ID="txtLeakFlowJudge" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                                </td>
                            </tr>
                        </table>

                        <div class="div-detail-table-sub-item">[差圧センサー]</div>
                        <table class="table-border-layout grid-layout">
                            <colgroup>
                                <col style="width: 150px" />
                                <col style="width: 150px" />
                                <col style="width: 150px" />
                                <col style="width: 150px" />
                            </colgroup>

                            <tr class="listview-header_2r">
                                <td class="ui-state-default detailtable-header">判定時圧力<br />(kPa)</td>
                                <td>
                                    <KTCC:KTTextBox ID="txtDpPressure" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                                </td>
                                <td class="ui-state-default detailtable-header"></td>
                                <td>
                                    <KTCC:KTTextBox ID="KTTextBox7" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                                </td>
                            </tr>
                            <tr class="listview-header_2r">
                                <td class="ui-state-default detailtable-header">差圧なし実績<br />(kPa/時)</td>
                                <td>
                                    <KTCC:KTTextBox ID="txtDpSensorNothing" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                                </td>
                                <td class="ui-state-default detailtable-header">判定</td>
                                <td>
                                    <KTCC:KTTextBox ID="txtDpSensorNothingJudge" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                                </td>
                            </tr>
                            
                            <tr class="listview-header_2r">
                                <td class="ui-state-default detailtable-header">差圧あり実績<br />(kPa/時)</td>
                                <td>
                                    <KTCC:KTTextBox ID="txtDpSensorExists" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                                </td>
                                <td class="ui-state-default detailtable-header">判定</td>
                                <td>
                                    <KTCC:KTTextBox ID="txtDpSensorExistsJudge" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                                </td>
                            </tr>
                        </table>

                        
                        <div class="div-detail-table-sub-item">[温度センサー]</div>
                        <table class="table-border-layout grid-layout">
                            <colgroup>
                                <col style="width: 150px" />
                                <col style="width: 150px" />
                                <col style="width: 150px" />
                                <col style="width: 150px" />
                            </colgroup>
                            <tr class="listview-header_2r">
                                <td class="ui-state-default detailtable-header">室温<br />(℃)</td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="txtTempRoom" runat="server" ReadOnly="true" InputMode="FloatNumWithMinus" DecLen="2" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                                <td class="ui-state-default detailtable-header"></td>
                                <td>
                                    <KTCC:KTTextBox ID="KTTextBox9" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                                </td>
                            </tr>
                            <tr class="listview-header_2r">
                                <td class="ui-state-default detailtable-header">DOC入口実績</td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="txtTempSensorDocIn" runat="server" ReadOnly="true" InputMode="FloatNumWithMinus" DecLen="2" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                                <td class="ui-state-default detailtable-header">判定</td>
                                <td>
                                    <KTCC:KTTextBox ID="txtTempSensorDocInJudge" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                                </td>
                            </tr>
                            <tr class="listview-header_2r">
                                <td class="ui-state-default detailtable-header">DPF入口実績</td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="txtTempSensorDpfIn" runat="server" ReadOnly="true" InputMode="FloatNumWithMinus" DecLen="2" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                                <td class="ui-state-default detailtable-header">判定</td>
                                <td>
                                    <KTCC:KTTextBox ID="txtTempSensorDpfInJudge" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                                </td>
                            </tr>
                            <tr class="listview-header_2r">
                                <td class="ui-state-default detailtable-header">DPF出口実績</td>
                                <td>
                                    <KTCC:KTDecimalTextBox ID="txtTempSensorDpfOut" runat="server" ReadOnly="true" InputMode="FloatNumWithMinus" DecLen="2" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                </td>
                                <td class="ui-state-default detailtable-header">判定</td>
                                <td>
                                    <KTCC:KTTextBox ID="txtTempSensorDpfOutJudge" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
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
