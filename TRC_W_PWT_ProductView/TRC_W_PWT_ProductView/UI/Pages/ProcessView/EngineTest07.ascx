<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EngineTest07.ascx.cs" Inherits="TRC_W_PWT_ProductView.UI.Pages.ProcessView.EngineTest07" %>
<%@ Import Namespace="TRC_W_PWT_ProductView.Common" %>

<%-- デザイン表示時使用 マスターページ使用時不要 --%>
<%--<link href="../../../CSS/cobalt/jquery-wijmo.css" rel="stylesheet" />
<link href="../../../CSS/Base.css" rel="stylesheet" />
<link href="../../../CSS/TRC.css" rel="stylesheet" />--%>

<%-- 画面CSS/スクリプト定義領域 --%>
<asp:PlaceHolder ID="SubScripts" runat="server">
    <script src="<%: PageUtils.ResolveUrl( this.Page, "~/Scripts/Pages/ProcessView/EngineTest07.js") %>" ></script>
</asp:PlaceHolder>

<div class="div-detail-info-margin">
    <div id="divDetailBodyScroll" class="div-fix-scroll">
        <div class="div-detail-table-title">■来歴情報</div>
        <div style="clear: both; width:auto;margin-top:10px"></div>
        <div id="divMainListArea" style="height: 150px">
            <table class="table-layout-fix" style="margin-left:10px">
                <tr>
                    <td>
                        <div id="divLTScroll" class="div-fix-scroll div-left-grid">
                            <div id="divHeaderLT" runat="server">
                                <table id="solidLTHeader" runat="server" class="grid-layout" style="width: 290px;">
                                    <tr id="headerLTMainContent"      runat="server" class="listview-header_2r ui-state-default">
                                        <th id="Th1" 　runat="server" style="width:150px;">測定日時</th>
                                        <th id="Th2"   runat="server" style="width: 70px;">ベンチ<br />No</th>
                                        <th id="Th3"   runat="server" style="width: 70px;">結果</th>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </td>
                    <td>
                        <div id="divRTScroll" class="div-scroll-right-top div-right-grid">
                            <div id="divHeaderRT" runat="server">
                                <table id="solidRTHeader" runat="server" class="grid-layout" style="width: 1300px;">
                                    <tr id="headerRTMainContent"    runat="server" class="listview-header_2r ui-state-default">
                                        <th id="Th4"   runat="server" style="width:120px;">作業者名</th>
                                        <th id="Th5"   runat="server" style="width: 90px;">パレット<br />No</th>
                                        <th id="Th6"   runat="server" style="width: 70px;">EGR<br />有無</th>
                                        <th id="Th7"   runat="server" style="width:150px;">搬入日時</th>
                                        <th id="Th8"   runat="server" style="width: 90px;">始動</th>
                                        <th id="Th9"   runat="server" style="width: 90px;">調整開始</th>
                                        <th id="Th10"  runat="server" style="width: 90px;">調整終了</th>
                                        <th id="Th11"  runat="server" style="width: 80px;">排ガス<br />区分</th>
                                        <th id="Th12"  runat="server" style="width: 80px;">OEM<br />区分</th>
                                        <th id="Th13"  runat="server" style="width:120px;">CRS<br />区分</th>
                                        <th id="Th14"  runat="server" style="width: 80px;">定格点<br />Qランク</th>
                                        <th id="Th15"  runat="server" style="width: 80px;">トルク点<br />Qランク</th>
                                        <th id="Th16"  runat="server" style="width: 80px;">ｲﾝﾀｰｸｰﾗ<br />有無</th>
                                        <th id="Th17"  runat="server" style="width: 80px;">アクセル<br />電圧区分</th>
                                        <th id="Th18"  runat="server" style="width: 80px;">CAN通信<br />速度</th>
                                        <th id="Th19"  runat="server" style="width:120px;">ｱｸｾﾙ手動計測<br/>(実績/指示)</th>
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
                                        <table id="itemPlaceholderContainerLB" runat="server" class="grid-layout" style="width:290px">
                                            <tr id="headerLBContent"   runat="server" class="listview-header_3r ui-state-default">
                                                <th id="Th1" 　runat="server" style="width:150px;"/>
                                                <th id="Th2"   runat="server" style="width: 70px;"/>
                                                <th id="Th3"   runat="server" style="width: 70px;"/>
                                            </tr>
                                            <tr id="itemPlaceholder" runat="server">
                                            </tr>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr id="trRowData" runat="server" class="listview-row ui-widget">
                                            <td>
                                                <asp:Button ID="btnSelect" runat="server" CommandName="Select" Text="" CssClass="btnSelect invisible" />
                                                <KTCC:KTTextBox ID="txtInspectionDt" runat="server" CssClass="font-default txt-default txt-width-full al-ct" ReadOnly="true"></KTCC:KTTextBox>
                                            </td>
                                            <td>
                                                <KTCC:KTTextBox ID="txtBenchiNo" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
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
                    <td>
                        <div id="divRBScroll" class="div-visible-scroll div-right-grid">
                            <div id="divGrvRB" runat="server">
                                <asp:ListView ID="lstMainListRB" runat="server" OnItemDataBound="lstMainListRB_ItemDataBound" OnSelectedIndexChanging="lstMainListRB_SelectedIndexChanging" OnSelectedIndexChanged="lstMainListRB_SelectedIndexChanged">
                                    <LayoutTemplate>
                                        <table id="itemPlaceholderContainerRB" runat="server" class="grid-layout" style="width:1300px">
                                            <tr id="headerRBContent"   runat="server" class="listview-header_3r ui-state-default">
                                                <th id="Th4"   runat="server" style="width:120px;"/>
                                                <th id="Th5"   runat="server" style="width: 90px;"/>
                                                <th id="Th6"   runat="server" style="width: 70px;"/>
                                                <th id="Th7"   runat="server" style="width:150px;"/>
                                                <th id="Th8"   runat="server" style="width: 90px;"/>
                                                <th id="Th9"   runat="server" style="width: 90px;"/>
                                                <th id="Th10"  runat="server" style="width: 90px;"/>
                                                <th id="Th11"  runat="server" style="width: 80px;"/>
                                                <th id="Th12"  runat="server" style="width: 80px;"/>
                                                <th id="Th13"  runat="server" style="width:120px;"/>
                                                <th id="Th14"  runat="server" style="width: 80px;"/>
                                                <th id="Th15"  runat="server" style="width: 80px;"/>
                                                <th id="Th16"  runat="server" style="width: 80px;"/>
                                                <th id="Th17"  runat="server" style="width: 80px;"/>
                                                <th id="Th18"  runat="server" style="width: 80px;"/>
                                                <th id="Th19"  runat="server" style="width:120px"/>
                                            </tr>
                                            <tr id="itemPlaceholder" runat="server">
                                            </tr>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr id="trRowData" runat="server" class="listview-row ui-widget">
                                            <td>
                                                <asp:Button ID="btnSelect" runat="server" CommandName="Select" Text="" CssClass="btnSelect invisible" />
                                                <KTCC:KTTextBox ID="txtUserNm" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" ></KTCC:KTTextBox>
                                            </td>
                                            <td>
                                                <KTCC:KTTextBox ID="txtPaletNo" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                            </td>
                                            <td>
                                                <KTCC:KTTextBox ID="txtErgTyp" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                            </td>
                                            <td>
                                                <KTCC:KTTextBox ID="txtEngineCarryDt" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                            </td>
                                            <td>
                                                <KTCC:KTTextBox ID="txtEngineStartTime" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                            </td>
                                            <td>
                                                <KTCC:KTTextBox ID="txtMeasureStartTime" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                            </td>
                                            <td>
                                                <KTCC:KTTextBox ID="txtMeasureEndTime" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                            </td>
                                            <td>
                                                <KTCC:KTTextBox ID="txtTireTyp" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                            </td>
                                            <td>
                                                <KTCC:KTTextBox ID="txtOemTyp" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                            </td>
                                            <td>
                                                <KTCC:KTTextBox ID="txtEngineTyp" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                            </td>
                                            <td>
                                                <KTCC:KTTextBox ID="txtTeikakuRtfLjtVal" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                            </td>
                                            <td>
                                                <KTCC:KTTextBox ID="txtTorqueRtfLjtVal" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                            </td>
                                            <td>
                                                <KTCC:KTTextBox ID="txtInterCoolerTyp" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                            </td>
                                            <td>
                                                <KTCC:KTTextBox ID="txtAccelVoltageTyp" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                            </td>
                                            <td>
                                                <KTCC:KTTextBox ID="txtCanSpeedKbn" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                            </td>
                                            <td>
                                                <KTCC:KTTextBox ID="txtAccelManualCount" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                            </td>
                                        </tr>
                                    </ItemTemplate>

                                </asp:ListView>
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
            <%-- 
            <asp:ListView ID="lstMainList" runat="server" OnItemDataBound="lstMainList_ItemDataBound" OnSelectedIndexChanging="lstMainList_SelectedIndexChanging" OnSelectedIndexChanged="lstMainList_SelectedIndexChanged">
                <LayoutTemplate>
                    <table id="itemPlaceholderContainer" runat="server" class="grid-layout" style="width: 1590px; height:auto">
                        <tr id="headerMainContent" runat="server" class="listview-header_2r ui-state-default">
                            <th id="Th1" 　runat="server" style="width:150px;">測定日時</th>
                            <th id="Th2"   runat="server" style="width: 70px;">ベンチ<br />No</th>
                            <th id="Th3"   runat="server" style="width: 70px;">結果</th>
                            <th id="Th4"   runat="server" style="width:120px;">作業者名</th>
                            <th id="Th5"   runat="server" style="width: 90px;">パレット<br />No</th>
                            <th id="Th6"   runat="server" style="width: 70px;">EGR<br />有無</th>
                            <th id="Th7"   runat="server" style="width:150px;">搬入日時</th>
                            <th id="Th8"   runat="server" style="width: 90px;">始動</th>
                            <th id="Th9"   runat="server" style="width: 90px;">調整開始</th>
                            <th id="Th10"  runat="server" style="width: 90px;">調整終了</th>
                            <th id="Th11"  runat="server" style="width: 80px;">排ガス<br />区分</th>
                            <th id="Th12"  runat="server" style="width: 80px;">OEM<br />区分</th>
                            <th id="Th13"  runat="server" style="width:120px;">CRS<br />区分</th>
                            <th id="Th14"  runat="server" style="width: 80px;">定格点<br />Qランク</th>
                            <th id="Th15"  runat="server" style="width: 80px;">トルク点<br />Qランク</th>
                            <th id="Th16"  runat="server" style="width: 80px;">ｲﾝﾀｰｸｰﾗ<br />有無</th>
                            <th id="Th17"  runat="server" style="width: 80px;">アクセル<br />電圧区分</th>
                            <th id="Th18"  runat="server" style="width: 80px;">CAN通信<br />速度</th>
                            <th id="Th19"  runat="server" style="width:120px;">ｱｸｾﾙ手動計測<br/>(実績/指示)</th>
                        </tr>
                        <tr id="itemPlaceholder" runat="server">
                        </tr>
                    </table>
                </LayoutTemplate>
                <ItemTemplate>
                    <tr id="trRowData" runat="server" class="listview-row ui-widget">
                        <td>
                            <asp:Button ID="btnSelect" runat="server" CommandName="Select" Text="" CssClass="btnSelect invisible" />
                            <KTCC:KTTextBox ID="txtInspectionDt" runat="server" CssClass="font-default txt-default txt-width-full al-ct" ReadOnly="true"></KTCC:KTTextBox>
                        </td>
                        <td>
                            <KTCC:KTTextBox ID="txtBenchiNo" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                        </td>
                        <td>
                            <KTCC:KTTextBox ID="txtResult" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                        </td>
                        <td>
                            <KTCC:KTTextBox ID="txtUserNm" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                        </td>
                        <td>
                            <KTCC:KTTextBox ID="txtPaletNo" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                        </td>
                        <td>
                            <KTCC:KTTextBox ID="txtErgTyp" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                        </td>
                        <td>
                            <KTCC:KTTextBox ID="txtEngineCarryDt" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                        </td>
                        <td>
                            <KTCC:KTTextBox ID="txtEngineStartTime" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                        </td>
                        <td>
                            <KTCC:KTTextBox ID="txtMeasureStartTime" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                        </td>
                        <td>
                            <KTCC:KTTextBox ID="txtMeasureEndTime" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                        </td>
                        <td>
                            <KTCC:KTTextBox ID="txtTireTyp" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                        </td>
                        <td>
                            <KTCC:KTTextBox ID="txtOemTyp" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                        </td>
                        <td>
                            <KTCC:KTTextBox ID="txtEngineTyp" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                        </td>
                        <td>
                            <KTCC:KTTextBox ID="txtTeikakuRtfLjtVal" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                        </td>
                        <td>
                            <KTCC:KTTextBox ID="txtTorqueRtfLjtVal" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                        </td>
                        <td>
                            <KTCC:KTTextBox ID="txtInterCoolerTyp" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                        </td>
                        <td>
                            <KTCC:KTTextBox ID="txtAccelVoltageTyp" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                        </td>
                        <td>
                            <KTCC:KTTextBox ID="txtCanSpeedKbn" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                        </td>
                        <td>
                            <KTCC:KTTextBox ID="txtAccelManualCount" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:ListView>
            --%>
        </div>

        <div class="div-detail-table-title">■測定結果</div>
        <div style="clear: both; width:auto;margin-top:10px"></div>
            <div id="divSubListArea" style="margin-left:10px">
            <table class="table-layout-fix">
                <tr>
                    <td>
                        <div id="divLTSubScroll" class="div-fix-scroll div-left-grid">
                            <div id="divSubHeaderLT" runat="server">
                                <table id="solidSubLTHeader" runat="server" class="grid-layout" style="width: 410px;">
                                    <tr id="Tr1"      runat="server" class="listview-header_2r ui-state-default">
                                        <th id="Th61"  runat="server" style="width: 70px;">No</th>
                                        <th id="Th62"  runat="server" style="width:250px;">測定項目名</th>
                                        <th id="Th63"  runat="server" style="width: 90px;">測定時間</th>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </td>
                    <td>
                        <div id="divRTSubScroll" class="div-scroll-right-top div-right-grid">
                            <div id="divSubHeaderRT" runat="server">
                                <table id="solidSubRTHeader" runat="server" class="grid-layout" style="width: 2110px;">
                                    <tr id="Tr2"      runat="server" class="listview-header_2r ui-state-default">
                                        <th id="Th39"  runat="server" style="width: 90px;">回転数<br />(rpm)</th>
                                        <th id="Th40"  runat="server" style="width: 90px;">トルク<br />(N･m)</th>
                                        <th id="Th41"  runat="server" style="width: 90px;">冷却水<br />入口(℃)</th>
                                        <th id="Th42"  runat="server" style="width: 90px;">冷却水<br />出口(℃)</th>
                                        <th id="Th43"  runat="server" style="width: 90px;">潤滑油<br />温度(℃)</th>
                                        <th id="Th44"  runat="server" style="width: 90px;">潤滑油<br />圧力(kPa)</th>
                                        <th id="Th45" runat="server" style="width: 90px;">吸気<br />温度(℃)</th>
                                        <th id="Th46" runat="server" style="width: 90px;">吸気<br />湿度(%)</th>
                                        <th id="Th47" runat="server" style="width:110px;">燃料測定流量<br />(ml/min)</th>
                                        <th id="Th48" runat="server" style="width:110px;">燃料補正流量<br />(ml/min)</th>
                                        <th id="Th49" runat="server" style="width: 90px;">燃料<br />温度(℃)</th>
                                        <th id="Th50" runat="server" style="width: 90px;">燃料供給<br />温度(℃)</th>
                                        <th id="Th51" runat="server" style="width: 90px;">燃料供給<br />圧力(kPa)</th>
                                        <th id="Th52" runat="server" style="width: 90px;">大気圧<br />(kPa)</th>
                                        <th id="Th53" runat="server" style="width: 90px;">回転ﾊﾝﾁﾝｸﾞ<br />(rpm)</th>
                                        <th id="Th54" runat="server" style="width: 90px;">燃料噴射量<br />(mm3/st)</th>
                                        <th id="Th20" runat="server" style="width: 90px;">出力<br />(kW)</th>
                                        <th id="Th21" runat="server" style="width: 90px;">修正出力<br />(kW)</th>
                                        <th id="Th22" runat="server" style="width: 90px;">修正トルク<br />(N･m)</th>
                                        <th id="Th23" runat="server" style="width: 90px;">修正係数</th>
                                        <th id="Th24" runat="server" style="width: 90px;">ブースト<br />圧力(kPa)</th>
                                        <th id="Th25" runat="server" style="width: 90px;">EGR測定<br />温度(℃)</th>
                                        <th id="Th26" runat="server" style="width: 90px;">ECU吸気<br />温度(℃)</th>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div id="divLBSubScroll" class="div-scroll-left-bottom div-left-grid">
                            <div id="divSubLB" runat="server">
                                <asp:ListView ID="lstSubListLB" runat="server" OnItemDataBound="lstSubListLB_ItemDataBound" >
                                    <LayoutTemplate>
                                        <table id="itemPlaceholderContainerLB" runat="server" class="grid-layout" style="width:410px">
                                            <tr id="headerLBContent"   runat="server" class="listview-header_3r ui-state-default">
                                                <th id="Th1"  runat="server" style="width: 70px;"/>
                                                <th id="Th2"  runat="server" style="width:250px;"/>
                                                <th id="Th3"  runat="server" style="width: 90px;"/>
                                            </tr>
                                            <tr id="itemPlaceholder" runat="server">
                                            </tr>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr id="trRowData" runat="server" class="listview-row ui-widget">
                                            <td>
                                                <KTCC:KTTextBox ID="txtSeqNo" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                            </td>
                                            <td>
                                                <KTCC:KTTextBox ID="txtMeasureNm" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                                            </td>
                                            <td>
                                                <KTCC:KTTextBox ID="txtMeasureTime" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:ListView>
                            </div>
                        </div>
                    </td>
                    <td>
                        <div id="divRBSubScroll" class="div-visible-scroll div-right-grid">
                            <div id="divSubRB" runat="server">
                                <asp:ListView ID="lstSubListRB" runat="server" OnItemDataBound="lstSubListRB_ItemDataBound">
                                    <LayoutTemplate>
                                        <table id="itemPlaceholderContainerRB" runat="server" class="grid-layout" style="width:2110px">
                                            <tr id="headerRBContent"   runat="server" class="listview-header_3r ui-state-default">
                                                <th id="Th39"  runat="server" style="width: 90px;"/>
                                                <th id="Th40"  runat="server" style="width: 90px;"/>
                                                <th id="Th41"  runat="server" style="width: 90px;"/>
                                                <th id="Th42"  runat="server" style="width: 90px;"/>
                                                <th id="Th43"  runat="server" style="width: 90px;"/>
                                                <th id="Th44"  runat="server" style="width: 90px;"/>
                                                <th id="Th45"  runat="server" style="width: 90px;"/>
                                                <th id="Th46"  runat="server" style="width: 90px;"/>
                                                <th id="Th47"  runat="server" style="width:110px;"/>
                                                <th id="Th48"  runat="server" style="width:110px;"/>
                                                <th id="Th49"  runat="server" style="width: 90px;"/>
                                                <th id="Th50"  runat="server" style="width: 90px;"/>
                                                <th id="Th51"  runat="server" style="width: 90px;"/>
                                                <th id="Th52"  runat="server" style="width: 90px;"/>
                                                <th id="Th53"  runat="server" style="width: 90px;"/>
                                                <th id="Th54"  runat="server" style="width: 90px;"/>
                                                <th id="Th20"  runat="server" style="width: 90px;"/>
                                                <th id="Th21"  runat="server" style="width: 90px;"/>
                                                <th id="Th22"  runat="server" style="width: 90px;"/>
                                                <th id="Th23"  runat="server" style="width: 90px;"/>
                                                <th id="Th24"  runat="server" style="width: 90px;"/>
                                                <th id="Th25"  runat="server" style="width: 90px;"/>
                                                <th id="Th26"  runat="server" style="width: 90px;"/>
                                            </tr>
                                            <tr id="itemPlaceholder" runat="server">
                                            </tr>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr id="trRowData" runat="server" class="listview-row ui-widget">
                                            <td>
                                                <KTCC:KTDecimalTextBox ID="ntbPowerRpm" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                            </td>
                                            <td>
                                                <KTCC:KTDecimalTextBox ID="ntbPowerNm" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                            </td>
                                            <td>
                                                <KTCC:KTDecimalTextBox ID="ntbCoolTempIn" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                            </td>
                                            <td>
                                                <KTCC:KTDecimalTextBox ID="ntbCoolTempOut" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                            </td>
                                            <td>
                                                <KTCC:KTDecimalTextBox ID="ntbLTemp" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                            </td>
                                            <td>
                                                <KTCC:KTDecimalTextBox ID="ntbLPrs" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                            </td>
                                            <td>
                                                <KTCC:KTDecimalTextBox ID="ntbInTemp" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                            </td>
                                            <td>
                                                <KTCC:KTDecimalTextBox ID="ntbInHum" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                            </td>
                                            <td>
                                                <KTCC:KTDecimalTextBox ID="ntbFMl" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                            </td>
                                            <td>
                                                <KTCC:KTDecimalTextBox ID="ntbFRml" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                            </td>
                                            <td>
                                                <KTCC:KTDecimalTextBox ID="ntbFTemp" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                            </td>
                                            <td>
                                                <KTCC:KTDecimalTextBox ID="ntbFSupply" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                            </td>
                                            <td>
                                                <KTCC:KTDecimalTextBox ID="ntbFPrs" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                            </td>
                                            <td>
                                                <KTCC:KTDecimalTextBox ID="ntbPaHg" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                            </td>
                                            <td>
                                                <KTCC:KTDecimalTextBox ID="ntbTrnRpm" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                            </td>
                                            <td>
                                                <KTCC:KTDecimalTextBox ID="ntbQSt" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                            </td>
                                            <td>
                                                <KTCC:KTDecimalTextBox ID="ntbPsKw" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                            </td>
                                            <td>
                                                <KTCC:KTDecimalTextBox ID="ntbSkKw" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                            </td>
                                            <td>
                                                <KTCC:KTDecimalTextBox ID="ntbTkNm" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                            </td>
                                            <td>
                                                <KTCC:KTDecimalTextBox ID="ntbKeisuu" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="4" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                            </td>
                                            <td>
                                                <KTCC:KTDecimalTextBox ID="ntbPinPrs" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="2" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                            </td>
                                            <td>
                                                <KTCC:KTDecimalTextBox ID="ntbEgrTemp" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                            </td>
                                            <td>
                                                <KTCC:KTDecimalTextBox ID="ntbEcuInTemp" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
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

        <%--    ヘッダ固定化対応により、コメントアウト
        <div id="divSubListArea" class="div-auto-scroll">
            <asp:ListView ID="lstSubList" runat="server" OnItemDataBound="lstSubList_ItemDataBound">
                <LayoutTemplate>
                    <table id="itemPlaceholderContainer" runat="server" class="grid-layout" style="width: 2520px; height:auto">
                        <tr id="headerMainContent" runat="server" class="listview-header_2r ui-state-default">
                            <th id="Th1"  runat="server" style="width: 70px;">No</th>
                            <th id="Th2"  runat="server" style="width:250px;">測定項目名</th>
                            <th id="Th3"  runat="server" style="width: 90px;">測定時間</th>
                            <th id="Th4"  runat="server" style="width: 90px;">回転数<br />(rpm)</th>
                            <th id="Th5"  runat="server" style="width: 90px;">トルク<br />(N･m)</th>
                            <th id="Th6"  runat="server" style="width: 90px;">冷却水<br />入口(℃)</th>
                            <th id="Th7"  runat="server" style="width: 90px;">冷却水<br />出口(℃)</th>
                            <th id="Th8"  runat="server" style="width: 90px;">潤滑油<br />温度(℃)</th>
                            <th id="Th9"  runat="server" style="width: 90px;">潤滑油<br />圧力(kPa)</th>
                            <th id="Th10" runat="server" style="width: 90px;">吸気<br />温度(℃)</th>
                            <th id="Th11" runat="server" style="width: 90px;">吸気<br />湿度(%)</th>
                            <th id="Th12" runat="server" style="width:110px;">燃料測定流量<br />(ml/min)</th>
                            <th id="Th13" runat="server" style="width:110px;">燃料補正流量<br />(ml/min)</th>
                            <th id="Th14" runat="server" style="width: 90px;">燃料<br />温度(℃)</th>
                            <th id="Th15" runat="server" style="width: 90px;">燃料供給<br />温度(℃)</th>
                            <th id="Th16" runat="server" style="width: 90px;">燃料供給<br />圧力(kPa)</th>
                            <th id="Th17" runat="server" style="width: 90px;">大気圧<br />(kPa)</th>
                            <th id="Th18" runat="server" style="width: 90px;">回転ﾊﾝﾁﾝｸﾞ<br />(rpm)</th>
                            <th id="Th19" runat="server" style="width: 90px;">燃料噴射量<br />(mm3/st)</th>
                            <th id="Th20" runat="server" style="width: 90px;">出力<br />(kW)</th>
                            <th id="Th21" runat="server" style="width: 90px;">修正出力<br />(kW)</th>
                            <th id="Th22" runat="server" style="width: 90px;">修正トルク<br />(N･m)</th>
                            <th id="Th23" runat="server" style="width: 90px;">修正係数</th>
                            <th id="Th24" runat="server" style="width: 90px;">ブースト<br />圧力(kPa)</th>
                            <th id="Th25" runat="server" style="width: 90px;">EGR測定<br />温度(℃)</th>
                            <th id="Th26" runat="server" style="width: 90px;">ECU吸気<br />温度(℃)</th>
                        </tr>
                        <tr class="" id="itemPlaceholder" runat="server">
                        </tr>
                    </table>
                </LayoutTemplate>
                <ItemTemplate>
                    <tr id="trRowData" runat="server" class="listview-row ui-widget">
                        <td>
                            <KTCC:KTTextBox ID="txtSeqNo" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                        </td>
                        <td>
                            <KTCC:KTTextBox ID="txtMeasureNm" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                        </td>
                        <td>
                            <KTCC:KTTextBox ID="txtMeasureTime" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                        </td>
                        <td>
                            <KTCC:KTDecimalTextBox ID="ntbPowerRpm" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                        </td>
                        <td>
                            <KTCC:KTDecimalTextBox ID="ntbPowerNm" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                        </td>
                        <td>
                            <KTCC:KTDecimalTextBox ID="ntbCoolTempIn" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                        </td>
                        <td>
                            <KTCC:KTDecimalTextBox ID="ntbCoolTempOut" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                        </td>
                        <td>
                            <KTCC:KTDecimalTextBox ID="ntbLTemp" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                        </td>
                        <td>
                            <KTCC:KTDecimalTextBox ID="ntbLPrs" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                        </td>
                        <td>
                            <KTCC:KTDecimalTextBox ID="ntbInTemp" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                        </td>
                        <td>
                            <KTCC:KTDecimalTextBox ID="ntbInHum" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                        </td>
                        <td>
                            <KTCC:KTDecimalTextBox ID="ntbFMl" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                        </td>
                        <td>
                            <KTCC:KTDecimalTextBox ID="ntbFRml" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                        </td>
                        <td>
                            <KTCC:KTDecimalTextBox ID="ntbFTemp" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                        </td>
                        <td>
                            <KTCC:KTDecimalTextBox ID="ntbFSupply" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                        </td>
                        <td>
                            <KTCC:KTDecimalTextBox ID="ntbFPrs" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                        </td>
                        <td>
                            <KTCC:KTDecimalTextBox ID="ntbPaHg" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                        </td>
                        <td>
                            <KTCC:KTDecimalTextBox ID="ntbTrnRpm" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                        </td>
                        <td>
                            <KTCC:KTDecimalTextBox ID="ntbQSt" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                        </td>
                        <td>
                            <KTCC:KTDecimalTextBox ID="ntbPsKw" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                        </td>
                        <td>
                            <KTCC:KTDecimalTextBox ID="ntbSkKw" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                        </td>
                        <td>
                            <KTCC:KTDecimalTextBox ID="ntbTkNm" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                        </td>
                        <td>
                            <KTCC:KTDecimalTextBox ID="ntbKeisuu" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="4" CssClass="font-default txt-default txt-numeric txt-width-full" />
                        </td>
                        <td>
                            <KTCC:KTDecimalTextBox ID="ntbPinPrs" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="2" CssClass="font-default txt-default txt-numeric txt-width-full" />
                        </td>
                        <td>
                            <KTCC:KTDecimalTextBox ID="ntbEgrTemp" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                        </td>
                        <td>
                            <KTCC:KTDecimalTextBox ID="ntbEcuInTemp" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:ListView>
        </div>
            --%>
    </div>
</div>