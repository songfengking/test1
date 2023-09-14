<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WiFiEcu.ascx.cs" Inherits="TRC_W_PWT_ProductView.UI.Pages.PartsView.WiFiEcu" %>
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
        <div style="clear: both; width:auto;margin-top:10px"></div>
        <div id="divMainListArea" class="div-auto-scroll" style="margin-left:10px">
            <asp:ListView ID="lstMainList" runat="server" OnItemDataBound="lstMainList_ItemDataBound">
                <LayoutTemplate>
                  <div class="" id="itemPlaceholder" runat="server">
                  </div>
                </LayoutTemplate>
                <ItemTemplate>
                    <div class="ui-state-default div-detail-table-sub-title">
                        <span>【測定結果】</span>
                        <KTCC:KTTextBox ID="txtInspectionDt" runat="server" ReadOnly="true" CssClass="" />
                    </div>
                    <div class="div-detail-table-subarea">
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
                                <td class="ui-state-default detailtable-header">ステータス</td>
                                <td>
                                    <KTCC:KTTextBox ID="txtStatus" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                                </td>
                                <td class="ui-state-default detailtable-header">検査員名</td>
                                <td>
                                    <KTCC:KTTextBox ID="txtEmployeeNm" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                                </td>
                                <td colspan="2"></td>
                            </tr>
                            <tr class="listview-header_2r">
                                <td class="ui-state-default detailtable-header">本機<br />ECU機番</td>
                                <td>
                                    <KTCC:KTTextBox ID="txtEcuSerial" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                                </td>
                                <td class="ui-state-default detailtable-header">WiFi<br />ECU品番</td>
                                <td>
                                    <KTCC:KTTextBox ID="txtHardNum" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                                </td>
                                <td class="ui-state-default detailtable-header">INI-W品番</td>
                                <td>
                                    <KTCC:KTTextBox ID="txtSoftNum" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                                </td>
                            </tr>
                        </table>
                        <asp:Panel runat="server" ID="pnlWfecu" Visible="true">
                            <div class="div-detail-table-sub-item">[WiFi ECU]</div>
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
                                    <td class="ui-state-default detailtable-header">アッシ品番</td>
                                    <td>
                                        <KTCC:KTTextBox ID="txtHardAssyNum" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                                    </td>
                                    <td class="ui-state-default detailtable-header">ソフト品番(WiFi)</td>
                                    <td>
                                        <KTCC:KTTextBox ID="txtHardSoftNumWifi" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                                    </td>
                                    <td class="ui-state-default detailtable-header">ソフト品番(KIND)</td>
                                    <td>
                                        <KTCC:KTTextBox ID="txtHardSoftNumKind" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                                    </td>
                                </tr>
                                <tr class="listview-header_2r">
                                    <td class="ui-state-default detailtable-header">WiFi IC<br />ソフト品番(WiFi)</td>
                                    <td>
                                        <KTCC:KTTextBox ID="txtIcSoftNumWifi" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                                    </td>
                                    <td class="ui-state-default detailtable-header">WiFi IC<br />ソフト品番(KIND)</td>
                                    <td>
                                        <KTCC:KTTextBox ID="txtIcSoftNumKind" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                                    </td>
                                    <td class="ui-state-default detailtable-header">機番</td>
                                    <td>
                                        <KTCC:KTTextBox ID="txtHardSerial" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                                    </td>
                                </tr>
                                <tr class="listview-header_2r">
                                    <td class="ui-state-default detailtable-header">アワーメータ<br />書込値</td>
                                    <td>
                                        <KTCC:KTNumericTextBox ID="ntbHourMeterWrite" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                                    </td>
                                    <td class="ui-state-default detailtable-header">アワーメータ<br />判定</td>
                                    <td>
                                        <KTCC:KTTextBox ID="txtHourMeterJud" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                                    </td>
                                    <td colspan="2"></td>
                                </tr>
                                <tr class="listview-header_2r">
                                    <td class="ui-state-default detailtable-header">アワーメータ<br />開始値</td>
                                    <td>
                                        <KTCC:KTNumericTextBox ID="ntbHourMeterStart" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                                    </td>
                                    <td class="ui-state-default detailtable-header">アワーメータ<br />終了値</td>
                                    <td>
                                        <KTCC:KTNumericTextBox ID="ntbHourMeterEnd" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                                    </td>
                                    <td class="ui-state-default detailtable-header">アワーメータ<br />誤差範囲</td>
                                    <td>
                                        <KTCC:KTNumericTextBox ID="ntbHourMeterCheck" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                                    </td>
                                </tr>
                                <tr class="listview-header_2r">
                                    <td class="ui-state-default detailtable-header">本機ECU<br />ペアリング</td>
                                    <td>
                                        <KTCC:KTTextBox ID="txtEcuPairing" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                                    </td>
                                    <td class="ui-state-default detailtable-header">稼働収集</td>
                                    <td>
                                        <KTCC:KTTextBox ID="txtCollection" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                                    </td>
                                    <td class="ui-state-default detailtable-header">検査モード</td>
                                    <td>
                                        <KTCC:KTTextBox ID="txtChkMode" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                                    </td>
                                </tr>
                                <tr class="listview-header_2r">
                                    <td class="ui-state-default detailtable-header">IGN状態</td>
                                    <td>
                                        <KTCC:KTTextBox ID="txtIgnState" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                                    </td>
                                    <td class="ui-state-default detailtable-header">汎用出力</td>
                                    <td>
                                        <KTCC:KTTextBox ID="txtOutput" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                                    </td>
                                    <td class="ui-state-default detailtable-header"></td>
                                    <td colspan="2"></td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel runat="server" ID="pnlDcu" Visible="false">
                            <div class="div-detail-table-sub-item">[DCU]</div>
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
                                    <td class="ui-state-default detailtable-header">高速収集条件<br/>エリア設定</td>
                                    <td>
                                        <KTCC:KTTextBox ID="txtHiCollectionState" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                                    </td>
                                    <td class="ui-state-default detailtable-header">サーバ設定0<br/>エリア設定</td>
                                    <td>
                                        <KTCC:KTTextBox ID="txtSvr0State" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                                    </td>
                                    <td class="ui-state-default detailtable-header">サーバ設定1<br/>エリア設定</td>
                                    <td>
                                        <KTCC:KTTextBox ID="txtSvr1State" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                                    </td>
                                </tr>
                                <tr class="listview-header_2r">
                                    <td class="ui-state-default detailtable-header">サーバ設定2<br/>エリア設定</td>
                                    <td>
                                        <KTCC:KTTextBox ID="txtSvr2State" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                                    </td>
                                    <td class="ui-state-default detailtable-header">サーバ設定3<br/>エリア設定</td>
                                    <td>
                                        <KTCC:KTTextBox ID="txtSvr3State" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                                    </td>
                                    <td class="ui-state-default detailtable-header">サーバ設定4<br/>エリア設定</td>
                                    <td>
                                        <KTCC:KTTextBox ID="txtSvr4State" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                                    </td>
                                </tr>
                                <tr class="listview-header_2r">
                                    <td class="ui-state-default detailtable-header">サーバ設定5<br/>エリア設定</td>
                                    <td>
                                        <KTCC:KTTextBox ID="txtSvr5State" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                                    </td>
                                    <td class="ui-state-default detailtable-header">サーバ設定6<br/>エリア設定</td>
                                    <td>
                                        <KTCC:KTTextBox ID="txtSvr6State" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                                    </td>
                                    <td class="ui-state-default detailtable-header">サーバ設定7<br/>エリア設定</td>
                                    <td>
                                        <KTCC:KTTextBox ID="txtSvr7State" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                                    </td>
                                </tr>
                                <tr class="listview-header_2r">
                                    <td class="ui-state-default detailtable-header">サーバ設定8<br/>エリア設定</td>
                                    <td>
                                        <KTCC:KTTextBox ID="txtSvr8State" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                                    </td>
                                    <td class="ui-state-default detailtable-header">サーバ設定9<br/>エリア設定</td>
                                    <td>
                                        <KTCC:KTTextBox ID="txtSvr9State" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                                    </td>
                                    <td class="ui-state-default detailtable-header">サーバ設定10<br/>エリア設定</td>
                                    <td>
                                        <KTCC:KTTextBox ID="txtSvr10State" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                                    </td>
                                </tr>
                                <tr class="listview-header_2r">
                                    <td class="ui-state-default detailtable-header">サーバ設定11<br/>エリア設定</td>
                                    <td>
                                        <KTCC:KTTextBox ID="txtSvr11State" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                                    </td>
                                    <td class="ui-state-default detailtable-header">3G通信結果</td>
                                    <td>
                                        <KTCC:KTTextBox ID="txtThreeGState" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                                    </td>
                                    <td class="ui-state-default detailtable-header">IMEI</td>
                                    <td>
                                        <KTCC:KTTextBox ID="txtImei" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                                    </td>
                                </tr>
                                <tr class="listview-header_2r">
                                    <td class="ui-state-default detailtable-header">ICCID</td>
                                    <td>
                                        <KTCC:KTTextBox ID="txtIccid" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                                    </td>
                                    <td class="ui-state-default detailtable-header">INI-XML品番</td>
                                    <td>
                                        <KTCC:KTTextBox ID="txtIniXmlHinban" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                                    </td>
                                    <td class="ui-state-default detailtable-header">認証ID</td>
                                    <td>
                                        <KTCC:KTTextBox ID="txtAuthenticationId" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <div class="div-detail-table-sub-item">[EEPROM]</div>
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
                                <td class="ui-state-default detailtable-header">製造工場<br />設定エリア状態</td>
                                <td>
                                    <KTCC:KTTextBox ID="txTeepromManufactureState" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                                </td>
                                <td class="ui-state-default detailtable-header">システム<br />設定エリア状態</td>
                                <td>
                                    <KTCC:KTTextBox ID="txtEepromSystemState" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                                </td>
                                <td class="ui-state-default detailtable-header">工場<br />設定エリア状態</td>
                                <td>
                                    <KTCC:KTTextBox ID="txTeepromKojoState" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                                </td>
                            </tr>
                            <tr class="listview-header_2r">
                                <td class="ui-state-default detailtable-header">収集条件<br />設定エリア状態</td>
                                <td>
                                    <KTCC:KTTextBox ID="txtEepromCollectionState" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                                </td>
                                <td class="ui-state-default detailtable-header">カルテデータ<br />エリア状態</td>
                                <td>
                                    <KTCC:KTTextBox ID="txTeepromKarteState" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                                </td>
                                <td class="ui-state-default detailtable-header">その他<br />エリア状態</td>
                                <td>
                                    <KTCC:KTTextBox ID="txtEepromEtcState" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                                </td>
                            </tr>
                            <tr class="listview-header_2r">
                                <td class="ui-state-default detailtable-header">動作オプション<br />エリア状態</td>
                                <td>
                                    <KTCC:KTTextBox ID="txtEepromOptionState" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                                </td>
                                <td class="ui-state-default detailtable-header">デフォルトパス<br />フレーズ状態</td>
                                <td>
                                    <KTCC:KTTextBox ID="txtEepromDefaultPassState" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                                </td>
                                <td class="ui-state-default detailtable-header">ユーザパス<br />フレーズ状態</td>
                                <td>
                                    <KTCC:KTTextBox ID="txtEepromUserPassState" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
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
