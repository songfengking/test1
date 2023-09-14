<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EngineTest03.ascx.cs" Inherits="TRC_W_PWT_ProductView.UI.Pages.ProcessView.EngineTest03" %>
<%@ Import Namespace="TRC_W_PWT_ProductView.Common" %>

<%-- デザイン表示時使用 マスターページ使用時不要 --%>
<%--<link href="../../../CSS/cobalt/jquery-wijmo.css" rel="stylesheet" />
<link href="../../../CSS/Base.css" rel="stylesheet" />
<link href="../../../CSS/TRC.css" rel="stylesheet" />--%>

<%-- 画面CSS/スクリプト定義領域 --%>
<asp:PlaceHolder ID="SubScripts" runat="server">
    <script src="<%: PageUtils.ResolveUrl( this.Page, "~/Scripts/Pages/ProcessView/EngineTest03.js") %>" ></script>
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
                                    <table id="solidLTHeader" runat="server" class="grid-layout" style="width: 410px;">
                                        <tr id="headerLTMainContent"      runat="server" class="listview-header_2r ui-state-default">
                                            <th id="Th1" 　runat="server" style="width:150px;">測定日時</th>
                                            <th id="Th65" 　runat="server" style="width:120px;">生産型式</th>
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
                                    <table id="solidRTHeader" runat="server" class="grid-layout" style="width: 1740px;">
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
                                            <th id="Th61"  runat="server" style="width: 80px;">ﾌﾙ電子<br />ｶﾞﾊﾞﾅ区分</th>
                                            <th id="Th14"  runat="server" style="width: 80px;">定格点<br />Qランク</th>
                                            <th id="Th15"  runat="server" style="width: 80px;">トルク点<br />Qランク</th>
                                            <th id="Th62"  runat="server" style="width: 80px;">第2定格点<br />Qランク</th>
                                            <th id="Th63"  runat="server" style="width: 80px;">ﾗｯｸ位置<br />ｵﾌｾｯﾄ</th>
                                            <th id="Th16"  runat="server" style="width: 80px;">ｲﾝﾀｰｸｰﾗ<br />有無</th>
                                            <th id="Th17"  runat="server" style="width: 80px;">アクセル<br />電圧区分</th>
                                            <th id="Th18"  runat="server" style="width:120px;">ECU<br />区分</th>
                                            <th id="Th19"  runat="server" style="width: 80px;">ﾊｰﾈｽﾘﾚｰ<br />有無</th>
                                            <th id="Th20"  runat="server" style="width: 80px;">燃料区分</th>
                                            <th id="Th21"  runat="server" style="width: 80px;">出力<br />モード</th>
                                            <th id="Th22"  runat="server" style="width: 80px;">TSC1 SA<br />SELECT</th>
                                            <th id="Th67"  runat="server" style="width: 80px;">TCO1送信</th>
                                            <th id="Th66"  runat="server" style="width: 80px;">TCO1 SA<br />SELECT</th>
                                            <th id="Th64"  runat="server" style="width: 80px;">CAN<br />通信速度</th>
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
                                                    <th id="Th65"  runat="server" style="width:120px;"/>
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
                                                    <KTCC:KTTextBox ID="txtProductModelCd" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
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
                                            <table id="itemPlaceholderContainerRB" runat="server" class="grid-layout" style="width:1740px">
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
                                                    <th id="Th61"  runat="server" style="width: 80px;"/>
                                                    <th id="Th14"  runat="server" style="width: 80px;"/>
                                                    <th id="Th15"  runat="server" style="width: 80px;"/>
                                                    <th id="Th62"  runat="server" style="width: 80px;"/>
                                                    <th id="Th63"  runat="server" style="width: 80px;"/>
                                                    <th id="Th16"  runat="server" style="width: 80px;"/>
                                                    <th id="Th17"  runat="server" style="width: 80px;"/>
                                                    <th id="Th18"  runat="server" style="width:120px;"/>
                                                    <th id="Th19"  runat="server" style="width: 80px;"/>
                                                    <th id="Th20"  runat="server" style="width: 80px;"/>
                                                    <th id="Th21"  runat="server" style="width: 80px;"/>
                                                    <th id="Th22"  runat="server" style="width: 80px;"/>
                                                    <th id="Th68"  runat="server" style="width: 80px;"/>
                                                    <th id="Th69"  runat="server" style="width: 80px;"/>
                                                    <th id="Th64"  runat="server" style="width: 80px;"/>
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
                                                    <KTCC:KTTextBox ID="txtGovernorTyp" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                                </td>
                                                <td>
                                                    <KTCC:KTTextBox ID="txtTeikakuRtfLjtVal" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                                </td>
                                                <td>
                                                    <KTCC:KTTextBox ID="txtTorqueRtfLjtVal" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                                </td>
                                                <td>
                                                    <KTCC:KTTextBox ID="txtTeikaku2RtfLjtVal" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                                </td>
                                                <td>
                                                    <KTCC:KTTextBox ID="txtRackOffsetVal" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                                </td>
                                                <td>
                                                    <KTCC:KTTextBox ID="txtInterCoolerTyp" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                                </td>
                                                <td>
                                                    <KTCC:KTTextBox ID="txtAccelVoltageTyp" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                                </td>
                                                <td>
                                                    <KTCC:KTTextBox ID="txtEcuTyp" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                                </td>
                                                <td>
                                                    <KTCC:KTTextBox ID="txtHarnesRelayFlag" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                                </td>
                                                <td>
                                                    <KTCC:KTTextBox ID="txtFuelTyp" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                                </td>
                                                <td>
                                                    <KTCC:KTTextBox ID="txtEngineMode" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                                </td>
                                                <td>
                                                    <KTCC:KTTextBox ID="txtSaSelect" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                                </td>
                                                <td>
                                                    <KTCC:KTTextBox ID="txtTco1Send" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                                </td>
                                                <td>
                                                    <KTCC:KTTextBox ID="txtTco1SaSelect" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                                </td>
                                                <td>
                                                    <KTCC:KTTextBox ID="txtCanSpeedTyp" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
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

        <div class="div-detail-table-title">■測定結果</div>
        <div style="clear: both; width:auto;margin-top:10px"></div>
        <div id="divSubListArea" style="margin-left:10px">
        <div id="div1" style="margin-left:10px">
            <table class="table-layout-fix">
                <tr>
                    <td>
                        <div id="divLTSubScroll" class="div-fix-scroll div-left-grid">
                            <div id="divSubHeaderLT" runat="server">
                                <table id="solidSubLTHeader" runat="server" class="grid-layout" style="width: 290px;">
                                    <tr id="Tr1"      runat="server" class="listview-header_2r ui-state-default">
                                        <th id="Th23"  runat="server" style="width: 70px;">No</th>
                                        <th id="Th24"  runat="server" style="width:250px;">測定項目名</th>
                                        <th id="Th25"  runat="server" style="width: 90px;">測定時間</th>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </td>
                    <td>
                        <div id="divRTSubScroll" class="div-scroll-right-top div-right-grid">
                            <div id="divSubHeaderRT" runat="server">
                                <table id="solidSubRTHeader" runat="server" class="grid-layout" style="width: 290px;">
                                    <tr id="Tr2"      runat="server" class="listview-header_2r ui-state-default">
                                        <th id="Th26"  runat="server" style="width: 90px;">回転数<br />(rpm)</th>
                                        <th id="Th27"  runat="server" style="width: 90px;">トルク<br />(N･m)</th>
                                        <th id="Th28"  runat="server" style="width: 90px;">冷却水<br />入口(℃)</th>
                                        <th id="Th29"  runat="server" style="width: 90px;">冷却水<br />出口(℃)</th>
                                        <th id="Th30"  runat="server" style="width: 90px;">潤滑油<br />温度(℃)</th>
                                        <th id="Th31"  runat="server" style="width: 90px;">潤滑油<br />圧力(kPa)</th>
                                        <th id="Th32" runat="server" style="width: 90px;">吸気<br />温度(℃)</th>
                                        <th id="Th33" runat="server" style="width: 90px;">吸気<br />湿度(%)</th>
                                        <th id="Th34" runat="server" style="width:110px;">燃料測定流量<br />(ml/min)</th>
                                        <th id="Th35" runat="server" style="width:110px;">燃料補正流量<br />(ml/min)</th>
                                        <th id="Th36" runat="server" style="width: 90px;">燃料<br />温度(℃)</th>
                                        <th id="Th37" runat="server" style="width: 90px;">燃料供給<br />温度(℃)</th>
                                        <th id="Th38" runat="server" style="width: 90px;">燃料供給<br />圧力(kPa)</th>
                                        <th id="Th39" runat="server" style="width: 90px;">大気圧<br />(kPa)</th>
                                        <th id="Th40" runat="server" style="width: 90px;">回転ﾊﾝﾁﾝｸﾞ<br />(rpm)</th>
                                        <th id="Th41" runat="server" style="width: 90px;">燃料噴射量<br />(mm3/st)</th>
                                        <th id="Th42" runat="server" style="width: 90px;">出力<br />(kW)</th>
                                        <th id="Th43" runat="server" style="width: 90px;">修正出力<br />(kW)</th>
                                        <th id="Th44" runat="server" style="width: 90px;">修正トルク<br />(N･m)</th>
                                        <th id="Th45" runat="server" style="width: 90px;">修正係数</th>
                                        <th id="Th46" runat="server" style="width: 90px;">ブースト<br />圧力(kPa)</th>
                                        <th id="Th47" runat="server" style="width: 90px;">EGR測定<br />温度(℃)</th>
                                        <th id="Th48" runat="server" style="width: 90px;">ECU吸気<br />温度(℃)</th>
                                        <th id="Th49" runat="server" style="width: 90px;">ECU回転数<br />(rpm)</th>
                                        <th id="Th50" runat="server" style="width: 90px;">スロットル<br />開度(%)</th>
                                        <th id="Th51" runat="server" style="width: 90px;">吸気圧<br />(kPa)</th>
                                        <th id="Th52" runat="server" style="width: 90px;">O2センサ<br />Pre-cat</th>
                                        <th id="Th53" runat="server" style="width: 90px;">O2センサ<br />Post-cat</th>
                                        <th id="Th54" runat="server" style="width: 90px;">補正値<br />(%)</th>
                                        <th id="Th55" runat="server" style="width: 90px;">燃費<br />(g/kWh)</th>
                                        <th id="Th56" runat="server" style="width: 90px;">ECU燃費率<br />(g/s)</th>
                                        <th id="Th57" runat="server" style="width: 90px;">空気過剰率<br />(%)</th>
                                        <th id="Th58" runat="server" style="width: 90px;">ｱｸｾﾙ開度<br />(%)</th>
                                        <th id="Th59" runat="server" style="width: 90px;">安定時間<br />(sec)</th>
                                        <th id="Th60" runat="server" style="width: 90px;">測定時間<br />(sec)</th>
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
                                        <table id="itemPlaceholderContainerLB" runat="server" class="grid-layout" style="width:290px">
                                            <tr id="headerLBContent"   runat="server" class="listview-header_3r ui-state-default">
                                                <th id="Th23"  runat="server" style="width: 70px;"/>
                                                <th id="Th24"  runat="server" style="width:250px;"/>
                                                <th id="Th25"  runat="server" style="width: 90px;"/>
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
                                        <table id="itemPlaceholderContainerRB" runat="server" class="grid-layout" style="width:1740px">
                                            <tr id="headerRBContent"   runat="server" class="listview-header_3r ui-state-default">
                                                <th id="Th4"  runat="server" style="width: 90px;"/>
                                                <th id="Th5"  runat="server" style="width: 90px;"/>
                                                <th id="Th6"  runat="server" style="width: 90px;"/>
                                                <th id="Th7"  runat="server" style="width: 90px;"/>
                                                <th id="Th8"  runat="server" style="width: 90px;"/>
                                                <th id="Th9"  runat="server" style="width: 90px;"/>
                                                <th id="Th10" runat="server" style="width: 90px;"/>
                                                <th id="Th11" runat="server" style="width: 90px;"/>
                                                <th id="Th12" runat="server" style="width:110px;"/>
                                                <th id="Th13" runat="server" style="width:110px;"/>
                                                <th id="Th14" runat="server" style="width: 90px;"/>
                                                <th id="Th15" runat="server" style="width: 90px;"/>
                                                <th id="Th16" runat="server" style="width: 90px;"/>
                                                <th id="Th17" runat="server" style="width: 90px;"/>
                                                <th id="Th18" runat="server" style="width: 90px;"/>
                                                <th id="Th19" runat="server" style="width: 90px;"/>
                                                <th id="Th20" runat="server" style="width: 90px;"/>
                                                <th id="Th21" runat="server" style="width: 90px;"/>
                                                <th id="Th22" runat="server" style="width: 90px;"/>
                                                <th id="Th23" runat="server" style="width: 90px;"/>
                                                <th id="Th24" runat="server" style="width: 90px;"/>
                                                <th id="Th25" runat="server" style="width: 90px;"/>
                                                <th id="Th26" runat="server" style="width: 90px;"/>
                                                <th id="Th27" runat="server" style="width: 90px;"/>
                                                <th id="Th28" runat="server" style="width: 90px;"/>
                                                <th id="Th29" runat="server" style="width: 90px;"/>
                                                <th id="Th30" runat="server" style="width: 90px;"/>
                                                <th id="Th31" runat="server" style="width: 90px;"/>
                                                <th id="Th32" runat="server" style="width: 90px;"/>
                                                <th id="Th33" runat="server" style="width: 90px;"/>
                                                <th id="Th34" runat="server" style="width: 90px;"/>
                                                <th id="Th35" runat="server" style="width: 90px;"/>
                                                <th id="Th36" runat="server" style="width: 90px;"/>
                                                <th id="Th37" runat="server" style="width: 90px;"/>
                                                <th id="Th38" runat="server" style="width: 90px;"/>
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
                                            <td>
                                                <KTCC:KTDecimalTextBox ID="ntbEcuRpmVal" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                            </td>
                                            <td>
                                                <KTCC:KTDecimalTextBox ID="ntbEcuThrottleVal" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                            </td>
                                            <td>
                                                <KTCC:KTDecimalTextBox ID="ntbEcuIntakePrs" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                            </td>
                                            <td>
                                                <KTCC:KTTextBox ID="txtO2Precat" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full" />
                                            </td>
                                            <td>
                                                <KTCC:KTTextBox ID="txtO2Postcat" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full" />
                                            </td>
                                            <td>
                                                <KTCC:KTDecimalTextBox ID="ntbBeVal" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                            </td>
                                            <td>
                                                <KTCC:KTDecimalTextBox ID="ntbMileageVal" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                            </td>
                                            <td>
                                                <KTCC:KTDecimalTextBox ID="ntbEcuMileageVal" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                            </td>
                                            <td>
                                                <KTCC:KTDecimalTextBox ID="ntbAfVal" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                            </td>
                                            <td>
                                                <KTCC:KTDecimalTextBox ID="ntbEcuAccelVal" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                                            </td>
                                            <td>
                                                <KTCC:KTTextBox ID="txtStabilityTime" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-rg" />
                                            </td>
                                            <td>
                                                <KTCC:KTTextBox ID="txtMeasureAverage" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-rg" />
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

    </div>
</div>