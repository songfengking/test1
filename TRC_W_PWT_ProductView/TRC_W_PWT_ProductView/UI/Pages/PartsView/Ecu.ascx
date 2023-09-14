<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Ecu.ascx.cs" Inherits="TRC_W_PWT_ProductView.UI.Pages.PartsView.Ecu" %>
<%@ Import Namespace="TRC_W_PWT_ProductView.Common" %>

<%-- デザイン表示時使用 マスターページ使用時不要 --%>
<%--<link href="../../../CSS/cobalt/jquery-wijmo.css" rel="stylesheet" />
<link href="../../../CSS/Base.css" rel="stylesheet" />
<link href="../../../CSS/TRC.css" rel="stylesheet" />--%>

<%-- 画面CSS/スクリプト定義領域 --%>
<asp:PlaceHolder ID="SubScripts" runat="server">
    <script src="<%: PageUtils.ResolveUrl( this.Page, "~/Scripts/Pages/ContentScroll.js") %>"></script>
</asp:PlaceHolder>

<div class="div-detail-info-margin">
    <div id="divMainListArea" class="div-auto-scroll">
        <div class="div-detail-table-title">■最終組付情報</div>
        <div style="clear: both; height: 10px; width: auto"></div>
        <table id="tblMain" class="table-border-layout grid-layout" style="width: 900px; height: auto; margin-left: 10px" runat="server">
            <colgroup>
                <col style="width: 200px" />
                <col style="width: 250px" />
                <col style="width: 200px" />
                <col style="width: 250px" />
            </colgroup>
            <tr class="detailtable-header">
                <td class="ui-state-default detailtable-header">組付日時</td>
                <td>
                    <KTCC:KTTextBox ID="txtInstallDt" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full" />
                </td>
                <td class="ui-state-default detailtable-header">ステーション</td>
                <td>
                    <KTCC:KTTextBox ID="txtStationNm" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full" />
                </td>
            </tr>
            <tr class="detailtable-header">
                <td class="ui-state-default detailtable-header">部品機番</td>
                <td>
                    <KTCC:KTTextBox ID="txtPartsSerial" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full" />
                </td>
                <td class="ui-state-default detailtable-header">クボタ品番</td>
                <td>
                    <KTCC:KTTextBox ID="txtPartsKubotaNum" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full" />
                </td>
            </tr>
            <tr class="detailtable-header">
                <td class="ui-state-default detailtable-header">メーカー品番</td>
                <td>
                    <KTCC:KTTextBox ID="txtPartsMakerNum" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full" />
                </td>
                <td colspan="2"></td>
            </tr>
            <tr class="detailtable-header">
                <td class="ui-state-default detailtable-header">来歴No</td>
                <td>
                    <KTCC:KTNumericTextBox ID="ntbHistoryIndex" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full txt-numeric" />
                </td>
                <td colspan="2"></td>
            </tr>
        </table>

        <div style="clear: both; height: 20px; width: auto"></div>
        <div class="div-detail-table-title">■来歴情報</div>
        <div style="clear: both; height: 10px; width: auto"></div>
        <div id="divGrvDisplay" runat="server" style="margin-left: 10px">
            <table class="table-layout-fix">
                <tr>
                    <td>
                        <div id="divGrvHeaderLT" runat="server" class="div-left-grid">
                            <asp:GridView ID="grvHeaderLT" runat="server" CssClass="grid-layout2 ui-widget-content" AllowPaging="false" AllowSorting="true" PageSize="100" AutoGenerateColumns="false">
                                <HeaderStyle CssClass="grid-header-newline ui-state-default" />
                            </asp:GridView>
                        </div>
                    </td>
                    <td>
                        <div id="divGrvHeaderRT" runat="server" class="div-right-grid">
                            <asp:GridView ID="grvHeaderRT" runat="server" CssClass="grid-layout2 ui-widget-content" AllowPaging="false" AllowSorting="true" PageSize="100" AutoGenerateColumns="false">
                                <HeaderStyle CssClass="grid-header-newline ui-state-default" />
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div id="divGrvLB" runat="server" class="div-left-grid">
                            <asp:GridView ID="grvMainViewLB" runat="server" ShowHeader="false" CssClass="grid-layout2 ui-widget-content" AllowPaging="true" AllowSorting="false" PageSize="100" AutoGenerateColumns="false" OnRowDataBound="grvMainViewLB_RowDataBound">
                                <HeaderStyle CssClass="grid-header ui-state-default" />
                                <RowStyle CssClass="grid-row ui-widget" />
                                <SelectedRowStyle CssClass="ui-state-highlight" />
                                <Columns>
                                    <asp:TemplateField HeaderText="inspectionDt" />
                                    <asp:TemplateField HeaderText="ecuEngineNum" />
                                    <asp:TemplateField HeaderText="ecuHardNum" />
                                    <asp:TemplateField HeaderText="ecuSoftNum" />
                                    <asp:TemplateField HeaderText="ecuSerial" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </td>
                    <td>
                        <div id="divGrvRB" runat="server" class="div-right-grid">
                            <asp:GridView ID="grvMainViewRB" runat="server" ShowHeader="false" CssClass="grid-layout2 ui-widget-content" AllowPaging="true" AllowSorting="false" PageSize="100" AutoGenerateColumns="false" OnRowDataBound="grvMainViewRB_RowDataBound">
                                <HeaderStyle CssClass="grid-header ui-state-default" />
                                <RowStyle CssClass="grid-row ui-widget" />
                                <SelectedRowStyle CssClass="ui-state-highlight" />
                                <Columns>
                                    <asp:TemplateField HeaderText="densoHardNum" />
                                    <asp:TemplateField HeaderText="densoSoftNum" />
                                    <asp:TemplateField HeaderText="enginePrt" />
                                    <asp:TemplateField HeaderText="ecuId" />
                                    <asp:TemplateField HeaderText="ecuCheckCd" />
                                    <asp:TemplateField HeaderText="terminalNm" />
                                    <asp:TemplateField HeaderText="prcJdg" />
                                    <asp:TemplateField HeaderText="oemIDecu" />
                                    <asp:TemplateField HeaderText="oemIDrev" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
            </table>
        </div>

        <div id="divInhouseEcuInfo" runat="server">
            <div style="clear: both; height: 20px; width: auto"></div>
            <div class="div-detail-table-title">■部品情報</div>
            <div style="clear: both; height: 10px; width: auto"></div>
            <table class="table-border-layout grid-layout" style="margin-left: 10px">
                <colgroup>
                    <col style="width: 200px" />
                    <col style="width: 250px" />
                    <col style="width: 200px" />
                    <col style="width: 250px" />
                </colgroup>
                <tr class="detailtable-header">
                    <td class="ui-state-default detailtable-header">機種</td>
                    <td>
                        <KTCC:KTTextBox ID="txtModelType" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                    </td>
                    <td class="ui-state-default detailtable-header">品番</td>
                    <td>
                        <KTCC:KTTextBox ID="txtPartsNumber" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                    </td>
                </tr>
                <tr class="detailtable-header">
                    <td class="ui-state-default detailtable-header">基板シリアルNo</td>
                    <td>
                        <KTCC:KTTextBox ID="txtCircuitBoardSerial" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                    </td>
                    <td class="ui-state-default detailtable-header">最終検査シリアルNo</td>
                    <td>
                        <KTCC:KTTextBox ID="txtEcuSerialHeader" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                        <asp:HiddenField ID="hdnEcuSerial" runat="server" />
                    </td>
                </tr>
            </table>

            <div style="clear: both; height: 20px; width: auto"></div>
            <div class="div-detail-table-title">
                ■検査情報&nbsp;<KTCC:KTButton ID="btnOutputCsv" runat="server" Text="検査データ出力" CssClass="btn-middle" OnClick="btnOutputCsv_Click" OnClientClick="SubmitControl.SetLoadingType(SubmitControl.TYPE_NONE);" />
            </div>
            <div class="ui-state-default div-detail-table-sub-title">
                <span>【完了日時】</span>
                <KTCC:KTTextBox ID="txtCompletionDt" runat="server" ReadOnly="true"/>
            </div>
            <div style="clear: both; height: 10px; width: auto" />
            <div class="div-detail-table-sub-item">[コネクタ組付け]</div>
                <table class="table-border-layout grid-layout" style="margin-left: 10px">
                    <colgroup>
                        <col style="width: 200px" />
                        <col style="width: 100px" />
                        <col style="width: 200px" />
                        <col style="width: 100px" />
                    </colgroup>
                    <tr class="detailtable-header">
                        <td class="ui-state-default detailtable-header">締付本数</td>
                        <td>
                            <KTCC:KTNumericTextBox ID="ntbConnectorAsmNum" runat="server" ReadOnly="true" InputMode="IntNum" CssClass="font-default txt-default txt-numeric txt-width-full" />
                        </td>
                        <td colspan="2"></td>
                    </tr>
                    <tr class="detailtable-header">
                        <td class="ui-state-default detailtable-header">トルク上限値[N･m]</td>
                        <td>
                            <KTCC:KTDecimalTextBox ID="ntbConnectorAsmMax" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="2" CssClass="font-default txt-default txt-numeric txt-width-full" />
                        </td>
                        <td class="ui-state-default detailtable-header">トルク下限値[N･m]</td>
                        <td>
                            <KTCC:KTDecimalTextBox ID="ntbConnectorAsmMin" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="2" CssClass="font-default txt-default txt-numeric txt-width-full" />
                        </td>
                    </tr>
                    <tr class="detailtable-header">
                        <td class="ui-state-default detailtable-header">トルク実測値1[N･m]</td>
                        <td>
                            <KTCC:KTDecimalTextBox ID="ntbConnectorAsmMeasurement1" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="2" CssClass="font-default txt-default txt-numeric txt-width-full" />
                        </td>
                        <td class="ui-state-default detailtable-header">トルク実測値2[N･m]</td>
                        <td>
                            <KTCC:KTDecimalTextBox ID="ntbConnectorAsmMeasurement2" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="2" CssClass="font-default txt-default txt-numeric txt-width-full" />
                        </td>
                    </tr>
                    <tr class="detailtable-header">
                        <td class="ui-state-default detailtable-header">トルク実測値3[N･m]</td>
                        <td>
                            <KTCC:KTDecimalTextBox ID="ntbConnectorAsmMeasurement3" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="2" CssClass="font-default txt-default txt-numeric txt-width-full" />
                        </td>
                        <td class="ui-state-default detailtable-header">トルク実測値4[N･m]</td>
                        <td>
                            <KTCC:KTDecimalTextBox ID="ntbConnectorAsmMeasurement4" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="2" CssClass="font-default txt-default txt-numeric txt-width-full" />
                        </td>
                    </tr>
                    <tr class="detailtable-header">
                        <td class="ui-state-default detailtable-header">締付結果</td>
                        <td>
                            <KTCC:KTTextBox ID="txtConnectorAsmResult" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                        </td>
                        <td colspan="2"></td>
                    </tr>
                </table>
            <div class="div-detail-table-sub-item">[防湿剤塗布]</div>
            <table class="table-border-layout grid-layout" style="margin-left: 10px">
                <colgroup>
                    <col style="width: 200px" />
                    <col style="width: 100px" />
                    <col style="width: 200px" />
                    <col style="width: 200px" />
                </colgroup>
                <tr class="detailtable-header">
                    <td class="ui-state-default detailtable-header">目視検査結果</td>
                    <td>
                        <KTCC:KTTextBox ID="txtDesiccantCoatInspxResult" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                    </td>
                    <td class="ui-state-default detailtable-header">目視検査作業者</td>
                    <td>
                        <KTCC:KTTextBox ID="txtDesiccantCoatOperator" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                    </td>
                </tr>
            </table>

            <div class="div-detail-table-sub-item">[放熱剤塗布]</div>
            <table class="table-border-layout grid-layout" style="margin-left: 10px">
                <colgroup>
                    <col style="width: 200px" />
                    <col style="width: 100px" />
                </colgroup>
                <tr class="detailtable-header">
                    <td class="ui-state-default detailtable-header">塗布欠け検査結果</td>
                    <td>
                        <KTCC:KTTextBox ID="txtHeatRadiatCoatInspxResult" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                    </td>
                </tr>
            </table>

            <div class="div-detail-table-sub-item">[基板・ケース組付け]</div>
            <table class="table-border-layout grid-layout" style="margin-left: 10px">
                <colgroup>
                    <col style="width: 200px" />
                    <col style="width: 100px" />
                    <col style="width: 200px" />
                    <col style="width: 100px" />
                </colgroup>
                <tr class="detailtable-header">
                    <td class="ui-state-default detailtable-header">締付本数</td>
                    <td>
                        <KTCC:KTNumericTextBox ID="ntbCircuitBoardAsmNum" runat="server" ReadOnly="true" InputMode="IntNum" CssClass="font-default txt-default txt-numeric txt-width-full" />
                    </td>
                    <td colspan="2"></td>
                </tr>
                <tr class="detailtable-header">
                    <td class="ui-state-default detailtable-header">トルク上限値[N･m]</td>
                    <td>
                        <KTCC:KTDecimalTextBox ID="ntbCircuitBoardAsmMax" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                    </td>
                    <td class="ui-state-default detailtable-header">トルク下限値[N･m]</td>
                    <td>
                        <KTCC:KTDecimalTextBox ID="ntbCircuitBoardAsmMin" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                    </td>
                </tr>
                <tr class="detailtable-header">
                    <td class="ui-state-default detailtable-header">トルク実測値1[N･m]</td>
                    <td>
                        <KTCC:KTDecimalTextBox ID="ntbCircuitBoardAsmMeasurement1" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                    </td>
                    <td class="ui-state-default detailtable-header">トルク実測値2[N･m]</td>
                    <td>
                        <KTCC:KTDecimalTextBox ID="ntbCircuitBoardAsmMeasurement2" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                    </td>
                </tr>
                <tr class="detailtable-header">
                    <td class="ui-state-default detailtable-header">トルク実測値3[N･m]</td>
                    <td>
                        <KTCC:KTDecimalTextBox ID="ntbCircuitBoardAsmMeasurement3" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                    </td>
                    <td class="ui-state-default detailtable-header">トルク実測値4[N･m]</td>
                    <td>
                        <KTCC:KTDecimalTextBox ID="ntbCircuitBoardAsmMeasurement4" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                    </td>
                </tr>
                <tr class="detailtable-header">
                    <td class="ui-state-default detailtable-header">トルク実測値5[N･m]</td>
                    <td>
                        <KTCC:KTDecimalTextBox ID="ntbCircuitBoardAsmMeasurement5" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                    </td>
                    <td class="ui-state-default detailtable-header">トルク実測値6[N･m]</td>
                    <td>
                        <KTCC:KTDecimalTextBox ID="ntbCircuitBoardAsmMeasurement6" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                    </td>
                </tr>
                <tr class="detailtable-header">
                    <td class="ui-state-default detailtable-header">締付結果</td>
                    <td>
                        <KTCC:KTTextBox ID="txtCircuitBoardAsmResult" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                    </td>
                    <td colspan="2"></td>
                </tr>
            </table>

            <div class="div-detail-table-sub-item">[シール剤塗布]</div>
            <table class="table-border-layout grid-layout" style="margin-left: 10px">
                <colgroup>
                    <col style="width: 200px" />
                    <col style="width: 100px" />
                </colgroup>
                <tr class="detailtable-header">
                    <td class="ui-state-default detailtable-header">塗布欠け検査結果</td>
                    <td>
                        <KTCC:KTTextBox ID="txtSealMaterialInspxResult" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                    </td>
                </tr>
            </table>

            <div class="div-detail-table-sub-item">[上下ケース組付け]</div>
            <table class="table-border-layout grid-layout" style="margin-left: 10px">
                <colgroup>
                    <col style="width: 200px" />
                    <col style="width: 100px" />
                    <col style="width: 200px" />
                    <col style="width: 100px" />
                </colgroup>
                <tr class="detailtable-header">
                    <td class="ui-state-default detailtable-header">締付本数</td>
                    <td>
                        <KTCC:KTNumericTextBox ID="ntbCaseAsmNum" runat="server" ReadOnly="true" InputMode="IntNum" CssClass="font-default txt-default txt-numeric txt-width-full" />
                    </td>
                    <td colspan="2"></td>
                </tr>
                <tr class="detailtable-header">
                    <td class="ui-state-default detailtable-header">トルク上限値[N･m]</td>
                    <td>
                        <KTCC:KTDecimalTextBox ID="ntbCaseAsmMax" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="2" CssClass="font-default txt-default txt-numeric txt-width-full" />
                    </td>
                    <td class="ui-state-default detailtable-header">トルク下限値[N･m]</td>
                    <td>
                        <KTCC:KTDecimalTextBox ID="ntbCaseAsmMin" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="2" CssClass="font-default txt-default txt-numeric txt-width-full" />
                    </td>
                </tr>
                <tr class="detailtable-header">
                    <td class="ui-state-default detailtable-header">トルク実測値1[N･m]</td>
                    <td>
                        <KTCC:KTDecimalTextBox ID="ntbCaseAsmMeasurement1" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="2" CssClass="font-default txt-default txt-numeric txt-width-full" />
                    </td>
                    <td class="ui-state-default detailtable-header">トルク実測値2[N･m]</td>
                    <td>
                        <KTCC:KTDecimalTextBox ID="ntbCaseAsmMeasurement2" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="2" CssClass="font-default txt-default txt-numeric txt-width-full" />
                    </td>
                </tr>
                <tr class="detailtable-header">
                    <td class="ui-state-default detailtable-header">トルク実測値3[N･m]</td>
                    <td>
                        <KTCC:KTDecimalTextBox ID="ntbCaseAsmMeasurement3" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="2" CssClass="font-default txt-default txt-numeric txt-width-full" />
                    </td>
                    <td class="ui-state-default detailtable-header">トルク実測値4[N･m]</td>
                    <td>
                        <KTCC:KTDecimalTextBox ID="ntbCaseAsmMeasurement4" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="2" CssClass="font-default txt-default txt-numeric txt-width-full" />
                    </td>
                </tr>
                <tr class="detailtable-header">
                    <td class="ui-state-default detailtable-header">締付結果</td>
                    <td>
                        <KTCC:KTTextBox ID="txtCaseAsmResult" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                    </td>
                    <td colspan="2"></td>
                </tr>
            </table>

            <div class="div-detail-table-sub-item">[リークテスト]</div>
            <table class="table-border-layout grid-layout" style="margin-left: 10px">
                <colgroup>
                    <col style="width: 200px" />
                    <col style="width: 100px" />
                    <col style="width: 200px" />
                    <col style="width: 100px" />
                </colgroup>
                <tr class="detailtable-header">
                    <td class="ui-state-default detailtable-header">加圧値[kPa]</td>
                    <td>
                        <KTCC:KTDecimalTextBox ID="ntbLeakPressureValue" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="1" CssClass="font-default txt-default txt-numeric txt-width-full" />
                    </td>
                    <td colspan="2"></td>
                </tr>
                <tr class="detailtable-header">
                    <td class="ui-state-default detailtable-header">漏れ量 上限値 [ml/min]</td>
                    <td>
                        <KTCC:KTDecimalTextBox ID="ntbLeakAmountMax" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                    </td>
                    <td class="ui-state-default detailtable-header">漏れ量 下限値 [ml/min]</td>
                    <td>
                        <KTCC:KTDecimalTextBox ID="ntbLeakAmountMin" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                    </td>
                </tr>
                <tr class="detailtable-header">
                    <td class="ui-state-default detailtable-header">漏れ量 [ml/min]</td>
                    <td>
                        <KTCC:KTDecimalTextBox ID="ntbLeakAmount" runat="server" ReadOnly="true" InputMode="FloatNum" DecLen="3" CssClass="font-default txt-default txt-numeric txt-width-full" />
                    </td>
                    <td class="ui-state-default detailtable-header">検査結果</td>
                    <td>
                        <KTCC:KTTextBox ID="txtLeakInspectionResult" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                    </td>
                </tr>
            </table>

            <div class="div-detail-table-sub-item">[最終検査]</div>
            <table class="table-border-layout grid-layout" style="margin-left: 10px">
                <colgroup>
                    <col style="width: 200px" />
                    <col style="width: 100px" />
                    <col style="width: 200px" />
                    <col style="width: 100px" />
                </colgroup>
                <tr class="detailtable-header">
                    <td class="ui-state-default detailtable-header">INJ1・4ピーク電流補正</td>
                    <td>
                        <KTCC:KTTextBox ID="txtLastInspxInj14peakCorrect" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                    </td>
                    <td class="ui-state-default detailtable-header">INJ1・4定電流補正</td>
                    <td>
                        <KTCC:KTTextBox ID="txtLastInspxInj14constCorrect" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                    </td>
                </tr>
                <tr class="detailtable-header">
                    <td class="ui-state-default detailtable-header">INJ2・3ピーク電流補正</td>
                    <td>
                        <KTCC:KTTextBox ID="txtLastInspxInj23peakCorrect" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                    </td>
                    <td class="ui-state-default detailtable-header">INJ2・3定電流補正</td>
                    <td>
                        <KTCC:KTTextBox ID="txtLastInspxInj23constCorrect" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                    </td>
                </tr>
                <tr class="detailtable-header">
                    <td class="ui-state-default detailtable-header">SCVゲイン補正</td>
                    <td>
                        <KTCC:KTTextBox ID="txtLastInspxScvGainCorrect" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                    </td>
                    <td class="ui-state-default detailtable-header">SCVオフセット補正</td>
                    <td>
                        <KTCC:KTTextBox ID="txtLastInspxScvOffsetCorrect" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                    </td>
                </tr>
                <tr class="detailtable-header">
                    <td class="ui-state-default detailtable-header">検査結果</td>
                    <td>
                        <KTCC:KTTextBox ID="txtLastInspxResult" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                    </td>
                    <td class="ui-state-default detailtable-header">シリアルNo</td>
                    <td>
                        <KTCC:KTTextBox ID="txtEcuSerial" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
</div>
