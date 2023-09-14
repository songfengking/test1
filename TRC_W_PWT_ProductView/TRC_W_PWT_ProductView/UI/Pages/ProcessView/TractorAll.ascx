<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TractorAll.ascx.cs" Inherits="TRC_W_PWT_ProductView.UI.Pages.ProcessView.TractorAll" %>
<%@ Import Namespace="TRC_W_PWT_ProductView.Common" %>

<%-- 画面CSS/スクリプト定義領域 --%>
<asp:PlaceHolder ID="SubScripts" runat="server">
    <script src="<%: PageUtils.ResolveUrl( this.Page, "~/Scripts/Pages/ContentScroll.js") %>" ></script>
    <script src="<%: ResolveUrl( "~/Scripts/LibScript/scrollViewer.js") %>" ></script>
    <script src="<%: PageUtils.ResolveUrl( this.Page, "~/Scripts/Pages/ProcessView/TractorAll.js") %>" ></script>
</asp:PlaceHolder>

<div class="div-detail-info-margin">
    <div id="divDetailBodyScroll" class="div-fix-scroll" >
        <KTCC:KTButton ID="btnExcel" runat="server" Text="Excel出力" CssClass="btn-middle" OnClick="btnExcel_Click" OnClientClick="SubmitControl.SetLoadingType(SubmitControl.TYPE_NONE);"/>
        <div class="div-detail-table-title">■検査情報</div>
        <div style="margin-top:10px;"/>
        <%-- 検査情報定義 --%>
        <div id="divMainListArea" class="div-y-scroll-flt2" >
            <table class="table-layout-fix" style="margin-left:10px">
                <tr>
                    <td>
                        <div id="divLTMainScroll" class="div-fix-scroll div-left-grid">
                            <%-- 左ヘッダー --%>
                            <div id="divMainHeaderLT" runat="server">
                                <table id="solidLTMainHeader" runat="server" class="grid-layout" style="width: 100px;">
                                    <tr id="headerLTMainContent" runat="server" class="listview-header_2r ui-state-default">
                                        <th id="InspectionSeq"   runat="server" style="width:100px">検査連番</th>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </td>
                    <td>
                        <%-- 右ヘッダー --%>
                        <div id="divRTMainScroll" class="div-scroll-right-top div-right-grid">
                            <div id="divMainHeaderRT" runat="server">
                                <table id="solidMainRTHeader" runat="server" class="grid-layout" style="width: 250px;">
                                    <tr id="headerRTMainContent" runat="server" class="listview-header_2r ui-state-default">
                                        <th id="Result"          runat="server" style="width: 100px">完了フラグ</th>
                                        <th id="InspectionEmplCd" runat="server" style="width: 150px">最終検査従業員</th>
                                        <th id="InspectionDt" runat="server" style="width: 150px">最終検査日時</th>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <%-- 左データ行 --%>
                        <div id="divLBMainScroll" class="div-scroll-left-bottom div-left-grid" style="height: 80px">
                            <div id="divGrvLBMain" runat="server">
                                <asp:ListView ID="lstMainListLB" runat="server" OnItemDataBound="lstMainListLB_ItemDataBound" OnSelectedIndexChanging="lstMainListLB_SelectedIndexChanging" OnSelectedIndexChanged="lstMainListLB_SelectedIndexChanged">
                                    <LayoutTemplate>
                                        <table id="itemPlaceholderContainerLBMain" runat="server" class="grid-layout" style="width:100px">
                                            <tr id="headerLBMainContent"   runat="server" class="listview-header_3r ui-state-default">
                                                <th id="InspectionSeq"   runat="server" style="width:100px"/>
                                            </tr>
                                            <tr id="itemPlaceholder" runat="server">
                                            </tr>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr id="trRowData" runat="server" class="listview-row ui-widget">
                                            <td>
                                                <asp:Button ID="btnSelect" runat="server" CommandName="Select" Text="" CssClass="btnSelect invisible" />
                                                <KTCC:KTTextBox ID="txtInspectionSeq" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:ListView>
                            </div>
                        </div>
                    </td>
                    <td>
                        <%-- 右データ行 --%>
                        <div id="divRBMainScroll" class="div-visible-scroll div-right-grid" style="height: 80px">
                            <div id="divGrvRBMain" runat="server">
                                <asp:ListView ID="lstMainListRB" runat="server" OnItemDataBound="lstMainListRB_ItemDataBound" OnSelectedIndexChanging="lstMainListRB_SelectedIndexChanging" OnSelectedIndexChanged="lstMainListRB_SelectedIndexChanged">
                                    <LayoutTemplate>
                                        <table id="itemPlaceholderContainerRBMain" runat="server" class="grid-layout" style="width:100px">
                                            <tr id="headerRBMainContent"   runat="server" class="listview-header_3r ui-state-default">
                                                <th id="Result"          runat="server" style="width: 100px"/>
                                                <th id="InspectionEmplCd" runat="server" style="width: 150px"/>
                                                <th id="CreateDt" runat="server" style="width: 150px"/>
                                            </tr>
                                            <tr id="itemPlaceholder" runat="server">
                                            </tr>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr id="trRowData" runat="server" class="listview-row ui-widget">
                                            <td>
                                                <asp:Button ID="btnSelect" runat="server" CommandName="Select" Text="" CssClass="btnSelect invisible" />
                                                <KTCC:KTTextBox ID="txtResult" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                            </td>
                                            <td>
                                                <KTCC:KTTextBox ID="txtInspectionEmplCd" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                                            </td>
                                            <td>
                                                <KTCC:KTTextBox ID="txtInspectionDt" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
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
        <div class="ui-state-default div-detail-table-sub-title">
            <span>【計測結果】</span>
            <KTCC:KTTextBox ID="txtInspectionDt" runat="server" ReadOnly="true" CssClass="" />
        </div>
        <div id="divDetailViewArea" class="div-fix-scroll-flt">
            <div id="divDetailViewBox" class="div-auto-scroll">
                <div class="div-detail-table-subarea">
                    <table class="table-border-layout grid-layout">
                        <colgroup>
                            <col style="width: 180px" />
                            <col style="width: 180px" />
                            <col style="width: 180px" />
                            <col style="width: 180px" />
                        </colgroup>
                        <tr class="listview-header_2r">
                            <td class="ui-state-default detailtable-header">検査連番</td>
                            <td>
                                <KTCC:KTTextBox ID="txtInspectionSeq" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td>
                        </tr>
                        <tr class="listview-header_2r">
                            <td class="ui-state-default detailtable-header">月度連番</td>
                            <td>
                                <KTCC:KTTextBox ID="txtSeqenceNum" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td>
                            <td class="ui-state-default detailtable-header">PINコード</td>
                            <td>
                                <KTCC:KTTextBox ID="txtPinCd" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td>
                        </tr>
                        <tr class="listview-header_2r">
                            <td class="ui-state-default detailtable-header">検査グループ</td>
                            <td>
                                <KTCC:KTTextBox ID="txtInspectionGroup" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td>
                            <td class="ui-state-default detailtable-header">2WD4WD</td>
                            <td>
                                <KTCC:KTTextBox ID="txtWheelDrive" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td>
                        </tr>
                        <tr class="listview-header_2r">
                            <td class="ui-state-default detailtable-header">固縛フック_前</td>
                            <td>
                                <KTCC:KTTextBox ID="txtLashingHookF" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td>
                            <td class="ui-state-default detailtable-header">固縛フック_後</td>
                            <td>
                                <KTCC:KTTextBox ID="txtLashingHookR" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td>
                        </tr>
                        <tr class="listview-header_2r">
                            <td class="ui-state-default detailtable-header">治具タイヤ係数<br />共通_前</td>
                            <td>
                                <KTCC:KTTextBox ID="txtJigComF" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td>
                            <td class="ui-state-default detailtable-header">治具タイヤ係数<br />共通_後</td>
                            <td>
                                <KTCC:KTTextBox ID="txtJigComR" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td>
                        </tr>
                    </table>



            <div class="div-detail-table-sub-item">[最高速度検査]</div>

            <table class="table-border-layout grid-layout" style="margin-bottom:10px">
                <colgroup>
                    <col style="width: 180px" />
                    <col style="width: 180px" />
                    <col style="width: 180px" />
                    <col style="width: 180px" />
                </colgroup>
                <tr class="listview-header_2r">
                    <td class="ui-state-default detailtable-header">最高速<br />検査フラグ</td>
                    <td>
                        <KTCC:KTTextBox ID="txtMaxSpeedFlagDetail" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                    </td>
					<td class="ui-state-default detailtable-header">最高速検査<br />ステーション</td>
                    <td>
                        <KTCC:KTTextBox ID="txtMaxSpeedStationCdDetail" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                    </td>
                </tr>        
            </table>

            <table class="table-layout-fix" style="">
                <tr>
                    <td>
                        <div id="div1" class="div-fix-scroll div-left-grid">
                            <%-- ヘッダー --%>
                            <div id="div2" runat="server">
                                <table class="grid-layout">
                                    <tr id="Tr1" runat="server" class="listview-header_2r ui-state-default">
                                        
                                        <th id="Th1" runat="server" style="width: 150px">スピード検査順序</th>
                                        <th id="Th2" runat="server" style="width: 150px">検査終了時刻</th>
                                        <th id="Th3" runat="server" style="width: 150px">最高速度_前_下限</th>
                                        <th id="Th4" runat="server" style="width: 150px">最高速度_前_上限</th>
                                        <th id="Th5" runat="server" style="width: 150px">最高速度_後_下限</th>
                                        <th id="Th6" runat="server" style="width: 150px">最高速度_後_上限</th>
                                        <th id="Th7" runat="server" style="width: 150px">最高速度到達後<br />計測時間</th>
                                        <th id="Th8" runat="server" style="width: 150px">最高速度範囲外<br />打ち切り時間</th>
                                        <th id="Th9" runat="server" style="width: 150px">結果<br />最高速検査フラグ</th>
                                        <th id="Th19" runat="server" style="width: 150px">最高速検査<br />従業員番号_検査員</th>
                                        <th id="Th10" runat="server" style="width: 150px">最高速検査終了時刻</th>
                                        <th id="Th11" runat="server" style="width: 150px">結果_最高速度<br />前_左_直値</th>
                                        <th id="Th12" runat="server" style="width: 150px">結果_最高速度<br />前_右_直値</th>
                                        <th id="Th13" runat="server" style="width: 150px">結果_最高速度<br />後_左_直値</th>
                                        <th id="Th14" runat="server" style="width: 150px">結果_最高速度<br />後_右_直値</th>
                                        <th id="Th15" runat="server" style="width: 150px">結果_最高速度<br />前_左_補正値</th>
                                        <th id="Th16" runat="server" style="width: 150px">結果_最高速度<br />前_右_補正値</th>
                                        <th id="Th17" runat="server" style="width: 150px">結果_最高速度<br />後_左_補正値</th>
                                        <th id="Th18" runat="server" style="width: 150px">結果_最高速度<br />後_右_補正値</th>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <%-- データ行 --%>
                        <div id="div5" class="div-left-grid">
                            <div id="div6" runat="server">
                                <asp:ListView ID="lstSpeedIns" runat="server" OnItemDataBound="lstSpeedIns_ItemDataBound" >
                                    <LayoutTemplate>
                                        <table id="tblSpeed" runat="server" class="grid-layout" style="width:150px">
                                            <tr id="tr"   runat="server" class="listview-header_3r ui-state-default">
                                                
                                                <th id="th20"   runat="server" style="width:150px"/>
                                                <th id="th21"   runat="server" style="width:150px"/>
                                                <th id="th22"   runat="server" style="width:150px"/>
                                                <th id="th38"   runat="server" style="width:150px"/>
                                                <th id="th23"   runat="server" style="width:150px"/>
                                                <th id="th24"   runat="server" style="width:150px"/>
                                                <th id="th25"   runat="server" style="width:150px"/>
                                                <th id="th26"   runat="server" style="width:150px"/>
                                                <th id="th27"   runat="server" style="width:150px"/>
                                                <th id="th28"   runat="server" style="width:150px"/>
                                                <th id="th29"   runat="server" style="width:150px"/>
                                                <th id="th30"   runat="server" style="width:150px"/>
                                                <th id="th31"   runat="server" style="width:150px"/>
                                                <th id="th32"   runat="server" style="width:150px"/>
                                                <th id="th33"   runat="server" style="width:150px"/>
                                                <th id="th34"   runat="server" style="width:150px"/>
                                                <th id="th35"   runat="server" style="width:150px"/>
                                                <th id="th36"   runat="server" style="width:150px"/>
                                                <th id="th37"   runat="server" style="width:150px"/>
                                            </tr>
                                            <tr id="itemPlaceholder" runat="server">
                                            </tr>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr id="trRowData" runat="server" class="listview-row ui-widget">
                                  
                                            <td>
                                                <KTCC:KTTextBox ID="txtSpeedInspectionSeq" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                            </td>
                                            <td>
                                                <KTCC:KTTextBox ID="txtSpeedInspectionEndDatetime" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                            </td>
                                            <td>
                                                <KTCC:KTTextBox ID="txtFrontWheelMin" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                            </td>
                                            <td>
                                                <KTCC:KTTextBox ID="txtFrontWheelMax" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                            </td>
                                            <td>
                                                <KTCC:KTTextBox ID="txtRearWheelMin" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                            </td>
                                            <td>
                                                <KTCC:KTTextBox ID="txtRearWheelMax" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                            </td>
                                            <td>
                                                <KTCC:KTTextBox ID="txtRangeHoldTime" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                            </td>
                                            <td>
                                                <KTCC:KTTextBox ID="txtCloseTime" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                            </td>
                                            <td>
                                                <KTCC:KTTextBox ID="txtRtMaxSpeedFlag" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                            </td>
                                            <td>
                                                <KTCC:KTTextBox ID="txtRtMaxSpeedemplCd" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                            </td>
                                            <td>
                                                <KTCC:KTTextBox ID="txtRtMaxSpeedEndTime" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                            </td>
                                            <td>
                                                <KTCC:KTTextBox ID="txtRtFrontWheelL" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                            </td>
                                            <td>
                                                <KTCC:KTTextBox ID="txtRtFrontWheelR" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                            </td>
                                            <td>
                                                <KTCC:KTTextBox ID="txtRtRearWheelL" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                            </td>
                                            <td>
                                                <KTCC:KTTextBox ID="txtRtRearWheelR" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                            </td>
                                            <td>
                                                <KTCC:KTTextBox ID="txtRtFrontWheelLCv" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                            </td>
                                            <td>
                                                <KTCC:KTTextBox ID="txtRtFrontWheelRCv" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                            </td>
                                            <td>
                                                <KTCC:KTTextBox ID="txtRtRearWheelLCv" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                            </td>
                                            <td>
                                                <KTCC:KTTextBox ID="txtRtRearWheelRCv" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:ListView>
                            </div>
                        </div>
                    </td>
                </tr>
            </table>

                    <div class="div-detail-table-sub-item">[騒音検査]</div>
                    <table class="table-border-layout grid-layout">
                        <colgroup>
                            <col style="width: 180px" />
                            <col style="width: 180px" />
                            <col style="width: 180px" />
                            <col style="width: 180px" />
                        </colgroup>
                        <tr class="listview-header_2r">
                            <td class="ui-state-default detailtable-header">騒音検査フラグ</td>
                            <td>
                                <KTCC:KTTextBox ID="txtNoiseFlag" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td>
                        </tr>
                        <tr class="listview-header_2r">
                            <td class="ui-state-default detailtable-header">副変速 </td>
                            <td>
                                <KTCC:KTTextBox ID="txtSubTransmission" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td>
                            <td class="ui-state-default detailtable-header">主変速</td>
                            <td>
                                <KTCC:KTTextBox ID="txtMainTransmission" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td>
                        </tr>
                        <tr class="listview-header_2r">
                            <td class="ui-state-default detailtable-header">騒音速度_前輪_下限</td>
                            <td>
                                <KTCC:KTTextBox ID="txtNoiseFrontWheelMin" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td>
                            <td class="ui-state-default detailtable-header">騒音速度_前輪_上限</td>
                            <td>
                                <KTCC:KTTextBox ID="txtNoiseFrontWheelMax" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td>
                        </tr>
                        <tr class="listview-header_2r">
                            <td class="ui-state-default detailtable-header">騒音速度_後輪_下限</td>
                            <td>
                                <KTCC:KTTextBox ID="txtNoiseRearWheelMin" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td>
                            <td class="ui-state-default detailtable-header">騒音速度_後輪_上限</td>
                            <td>
                                <KTCC:KTTextBox ID="txtNoiseRearWheelMax" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td>
                        </tr>
                        <tr class="listview-header_2r">
                            <td class="ui-state-default detailtable-header">騒音速度到達後<br />計測時間</td>
                            <td>
                                <KTCC:KTTextBox ID="txtNoiseRangeHoldTime" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td>
                            <td class="ui-state-default detailtable-header">騒音速度範囲外<br />打ち切り時間</td>
                            <td>
                                <KTCC:KTTextBox ID="txtNoiseCloseTime" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td>
                        </tr>
                        <tr class="listview-header_2r">
                            <td class="ui-state-default detailtable-header">結果 騒音検査フラグ</td>
                            <td>
                                <KTCC:KTTextBox ID="txtRtNoiseFlag" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td>
                            <td class="ui-state-default detailtable-header">騒音検査終了時刻</td>
                            <td>
                                <KTCC:KTTextBox ID="txtRtNoiseEndTime" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td>
                        </tr>
                        <tr class="listview-header_2r">
                            <td class="ui-state-default detailtable-header">結果_騒音_前_<br />左_直値</td>
                            <td>
                                <KTCC:KTTextBox ID="txtRtNoiseFrontWheelL" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td>
                            <td class="ui-state-default detailtable-header">結果_騒音_前_<br />右_直値</td>
                            <td>
                                <KTCC:KTTextBox ID="txtRtNoiseFrontWheelR" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td>
                        </tr>
                        <tr class="listview-header_2r">
                            <td class="ui-state-default detailtable-header">結果_騒音_後_<br />左_直値</td>
                            <td>
                                <KTCC:KTTextBox ID="txtRtNoiseRearWheelL" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td>
                            <td class="ui-state-default detailtable-header">結果_騒音_後_<br />右_直値</td>
                            <td>
                                <KTCC:KTTextBox ID="txtRtNoiseRearWheelR" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td>
                        </tr>
                        <tr class="listview-header_2r">
                            <td class="ui-state-default detailtable-header">結果_騒音_前_<br />左_補正値</td>
                            <td>
                                <KTCC:KTTextBox ID="txtRtNoiseFrontWheelLCv" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td>
                            <td class="ui-state-default detailtable-header">結果_騒音_前_<br />右_補正値</td>
                            <td>
                                <KTCC:KTTextBox ID="txtRtNoiseFrontWheelRCv" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td>
                        </tr>
                        <tr class="listview-header_2r">
                            <td class="ui-state-default detailtable-header">結果_騒音_後_<br />左_補正値</td>
                            <td>
                                <KTCC:KTTextBox ID="txtRtNoiseRearWheelLCv" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td>
                            <td class="ui-state-default detailtable-header">結果_騒音_後_<br />右_補正値</td>
                            <td>
                                <KTCC:KTTextBox ID="txtRtNoiseRearWheelRCv" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td>
                        </tr>
                        <tr class="listview-header_2r">
                            <td class="ui-state-default detailtable-header">結果_最大騒音値</td>
                            <td>
                                <KTCC:KTTextBox ID="txtRtNoiseMax" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td>
                            <td class="ui-state-default detailtable-header">騒音判定閾値_下限</td>
                            <td>
                                <KTCC:KTTextBox ID="txtNoiseThresholdMin" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td>
                        </tr>
                        <tr class="listview-header_2r">
                            <td class="ui-state-default detailtable-header">騒音検査<br />ステーション</td>
                            <td>
                                <KTCC:KTTextBox ID="txtNoiseStationCd" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td>
                            <td class="ui-state-default detailtable-header">騒音判定閾値_上限</td>
                            <td>
                                <KTCC:KTTextBox ID="txtNoiseThreshold" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td>
                        </tr>
                    </table>

                    <div class="div-detail-table-sub-item">[電子チェックシート検査]</div>
                    <table class="table-border-layout grid-layout">
                        <colgroup>
                            <col style="width: 180px" />
                            <col style="width: 180px" />
                            <col style="width: 180px" />
                            <col style="width: 180px" />
                        </colgroup>
                        <tr class="listview-header_2r">
                            <td class="ui-state-default detailtable-header">電子チェックシート<br />検査フラグ</td>
                            <td>
                                <KTCC:KTTextBox ID="txtAbnormalNoiseFlag" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td>
                            <td class="ui-state-default detailtable-header">電子チェックシート<br />検査終了時刻</td>
                            <td>
                                <KTCC:KTTextBox ID="txtRtAbnormalNoiseEndTime" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td>
                        </tr>
                        <tr class="listview-header_2r">
                            <td class="ui-state-default detailtable-header">結果 電子チェック<br />シート<br /></td>
                            <td>
                                <KTCC:KTTextBox ID="txtRtAbnormalNoiseFlag" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td>
                            <td class="ui-state-default detailtable-header">電子チェックシート<br />ステーション</td>
                            <td>
                                <KTCC:KTTextBox ID="txtSisStationCd" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td>
                            
                        </tr>
                        
                    </table>

                    <div class="div-detail-table-sub-item">[ブレーキ検査]</div>
                    <table class="table-border-layout grid-layout">
                        <colgroup>
                            <col style="width: 180px" />
                            <col style="width: 180px" />
                            <col style="width: 180px" />
                            <col style="width: 180px" />
                        </colgroup>
                        <tr class="listview-header_2r">
                            <td class="ui-state-default detailtable-header">トラクタ重量</td>
                            <td>
                                <KTCC:KTTextBox ID="txtMaxAllowWeight" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td>
                        </tr>
                        <tr class="listview-header_2r">
                            <td class="ui-state-default detailtable-header">左右制動力差<br />NG閾値</td>
                            <td>
                                <KTCC:KTTextBox ID="txtBrakingForceLRDiff" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td>
                            <td class="ui-state-default detailtable-header">ブレーキ検査<br />打ち切り時間</td>
                            <td>
                                <KTCC:KTTextBox ID="txtBrakingComCloseTime" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td>
                        </tr>
                        <tr class="listview-header_2r">
                            <td class="ui-state-default detailtable-header">主ブレーキ<br />検査フラグ</td>
                            <td>
                                <KTCC:KTTextBox ID="txtBrakingBFlag" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td>
                            <td class="ui-state-default detailtable-header">踏力_下限</td>
                            <td>
                                <KTCC:KTTextBox ID="txtBrakingBForceMin" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td>
                        </tr>
                        <tr class="listview-header_2r">
                            <td class="ui-state-default detailtable-header">主ブレーキ判定閾値_下限</td>
                            <td>
                                <KTCC:KTTextBox ID="txtBrakingBThreshold" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td>
                            <td class="ui-state-default detailtable-header">踏力_上限</td>
                            <td>
                                <KTCC:KTTextBox ID="txtBrakingBForceMax" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td>
                        </tr>
                        <tr class="listview-header_2r">
                            <td class="ui-state-default detailtable-header">主ブレーキ判定閾値_上限</td>
                            <td>
                                <KTCC:KTTextBox ID="txtBrakingBThresholdMax" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td>
                            <td class="ui-state-default detailtable-header">主ブレーキ検査<br />ステーション</td>
                            <td>
                                <KTCC:KTTextBox ID="txtBrkbStationCd" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td>
                        </tr>
                        <tr class="listview-header_2r">
                            <td class="ui-state-default detailtable-header">駐車ブレーキ<br />検査フラグ</td>
                            <td>
                                <KTCC:KTTextBox ID="txtBrakingPFlag" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td>
                            <td class="ui-state-default detailtable-header">駐車ブレーキ<br />判定閾値_下限</td>
                            <td>
                                <KTCC:KTTextBox ID="txtBrakingPThreshold" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td>
                        </tr>
                        <tr class="listview-header_2r">
                            <td class="ui-state-default detailtable-header">駐車ブレーキ<br />ステーション</td>
                            <td>
                                <KTCC:KTTextBox ID="txtBrkpStationCd" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td>
                            <td class="ui-state-default detailtable-header">駐車ブレーキ<br />判定閾値_上限</td>
                            <td>
                                <KTCC:KTTextBox ID="txtBrakingPThresholdMax" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td>
                        </tr>
                        <tr class="listview-header_2r">
                            <td class="ui-state-default detailtable-header">非常ブレーキ<br />検査フラグ</td>
                            <td>
                                <KTCC:KTTextBox ID="txtBrakingEFlag" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td>
                            <td class="ui-state-default detailtable-header">非常ブレーキ<br />判定閾値_下限</td>
                            <td>
                                <KTCC:KTTextBox ID="txtBrakingEThreshold" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td>
                        </tr>
                        <tr class="listview-header_2r">
                            <td class="ui-state-default detailtable-header">非常ブレーキ<br />ステーション</td>
                            <td>
                                <KTCC:KTTextBox ID="txtBrkeStationCd" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td>
                            <td class="ui-state-default detailtable-header">非常ブレーキ<br />判定閾値_上限</td>
                            <td>
                                <KTCC:KTTextBox ID="txtBrakingEThresholdMax" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td>
                        </tr>
                        <tr class="listview-header_2r">
                            <td class="ui-state-default detailtable-header">結果_主B検査フラグ</td>
                            <td>
                                <KTCC:KTTextBox ID="txtRtBrkBFlag" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td>
                            <td class="ui-state-default detailtable-header">結果_主B検査<br />終了時刻</td>
                            <td>
                                <KTCC:KTTextBox ID="txtRtBrkBEndTime" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td>
                        </tr>
                        <tr class="listview-header_2r">
                            <td class="ui-state-default detailtable-header">結果_主B最大制動力<br />後_左_直値</td>
                            <td>
                                <KTCC:KTTextBox ID="txtRtBrkBMaxForceRearL" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td>
                            <td class="ui-state-default detailtable-header">結果_主B最大制動力<br />後_右_直値</td>
                            <td>
                                <KTCC:KTTextBox ID="txtRtBrkBMaxForceRearR" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td>
                        </tr>
                        <tr class="listview-header_2r">
                            <td class="ui-state-default detailtable-header">結果_主B最大制動力<br />後_左_補正値</td>
                            <td>
                                <KTCC:KTTextBox ID="txtRtBrkBMaxForceRearLCv" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td>
                            <td class="ui-state-default detailtable-header">結果_主B最大制動力<br />後_右_補正値</td>
                            <td>
                                <KTCC:KTTextBox ID="txtRtBrkBMaxForceRearRCv" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td>
                        </tr>
                        <tr class="listview-header_2r">
                            <td class="ui-state-default detailtable-header">結果_主B<br />最大制動力_踏力</td>
                            <td>
                                <KTCC:KTTextBox ID="txtRtBrkBMaxPedalForceMax" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td>
                            <td></td>
                            <td></td>
                        </tr>
                        <tr class="listview-header_2r">
                            <td class="ui-state-default detailtable-header">結果_駐B<br />検査フラグ</td>
                            <td>
                                <KTCC:KTTextBox ID="txtRtBrkPFlag" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td>
                            <td class="ui-state-default detailtable-header">結果_駐B<br />検査終了時刻</td>
                            <td>
                                <KTCC:KTTextBox ID="txtRtBrkPEndTime" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td>
                        </tr>
                        <tr class="listview-header_2r">
                            <td class="ui-state-default detailtable-header">結果_駐B最大制動力_<br />後_左_直値</td>
                            <td>
                                <KTCC:KTTextBox ID="txtRtBrkPMaxForceRearL" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td>
                            <td class="ui-state-default detailtable-header">結果_駐B最大制動力_<br />後_右_直値</td>
                            <td>
                                <KTCC:KTTextBox ID="txtRtBrkPMaxForceRearR" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td>
                        </tr>
                        <tr class="listview-header_2r">
                            <td class="ui-state-default detailtable-header">結果_駐B最大制動力_<br />後_左_補正値</td>
                            <td>
                                <KTCC:KTTextBox ID="txtRtBrkPMaxForceRearLCv" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td>
                            <td class="ui-state-default detailtable-header">結果_駐B最大制動力_<br />後_右_補正値</td>
                            <td>
                                <KTCC:KTTextBox ID="txtRtBrkPMaxForceRearRCv" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td>
                        </tr>
                        <tr class="listview-header_2r">
                            <td class="ui-state-default detailtable-header">結果_非B検査フラグ</td>
                            <td>
                                <KTCC:KTTextBox ID="txtRtBrkEFlag" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td>
                            <td class="ui-state-default detailtable-header">結果_非B検査<br />終了時刻</td>
                            <td>
                                <KTCC:KTTextBox ID="txtRtBrkEEndTime" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td>
                        </tr>
                        <tr class="listview-header_2r">
                            <td class="ui-state-default detailtable-header">結果_非B最大制動力_<br />後_左_直値</td>
                            <td>
                                <KTCC:KTTextBox ID="txtRtBrkEMaxForceRearL" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td>
                            <td class="ui-state-default detailtable-header">結果_非B最大制動力_<br />後_右_直値</td>
                            <td>
                                <KTCC:KTTextBox ID="txtRtBrkEMaxForceRearR" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td>
                        </tr>
                        <tr class="listview-header_2r">
                            <td class="ui-state-default detailtable-header">結果_非B最大制動力_<br />後_左_補正値</td>
                            <td>
                                <KTCC:KTTextBox ID="txtRtBrkEMaxForceRearLCv" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td>
                            <td class="ui-state-default detailtable-header">結果_非B最大制動力_<br />後_右_補正値</td>
                            <td>
                                <KTCC:KTTextBox ID="txtRtBrkEMaxForceRearRCv" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td>
                        </tr>
                    </table>

                    <div class="div-detail-table-sub-item">[モンロー検査]</div>
                    <table class="table-border-layout grid-layout">
                        <colgroup>
                            <col style="width: 180px" />
                            <col style="width: 180px" />
                            <col style="width: 180px" />
                            <col style="width: 180px" />
                        </colgroup>
                        <tr class="listview-header_2r">
                            <td class="ui-state-default detailtable-header">モンロー検査フラグ</td>
                            <td>
                                <KTCC:KTTextBox ID="txtMonroeFlag" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td>
                            <td class="ui-state-default detailtable-header">モンロー検査<br />終了時刻</td>
                            <td>
                                <KTCC:KTTextBox ID="txtRtMonroeEndTime" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td>
                        </tr>
                        <tr class="listview-header_2r">
                            <td class="ui-state-default detailtable-header">結果 モンロー<br />検査フラグ</td>
                            <td>
                                <KTCC:KTTextBox ID="txtRtMonroeFlag" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td>
                            <td class="ui-state-default detailtable-header">モンロー検査<br />ステーション</td>
                            <td>
                                <KTCC:KTTextBox ID="txtMonroeStationCd" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td>
                            
                        </tr>
                    </table>

                    <div class="div-detail-table-sub-item">[PTO検査]</div>
                    <table class="table-border-layout grid-layout">
                        <colgroup>
                            <col style="width: 180px" />
                            <col style="width: 180px" />
                            <col style="width: 180px" />
                            <col style="width: 180px" />
                        </colgroup>
                        <tr class="listview-header_2r">
                            <td class="ui-state-default detailtable-header">PTO検査フラグ</td>
                            <td>
                                <KTCC:KTTextBox ID="txtPtoFlag" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td>
                            <td class="ui-state-default detailtable-header">PTO検査<br />終了時刻</td>
                            <td>
                                <KTCC:KTTextBox ID="txtRtPtoEndTime" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td>
							
                        </tr>
                        <tr class="listview-header_2r">
                            <td class="ui-state-default detailtable-header">結果 PTO検査フラグ</td>
                            <td>
                                <KTCC:KTTextBox ID="txtRtPtoFlag" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td>
                            <td class="ui-state-default detailtable-header">PTO検査<br />ステーション</td>
                            <td>
                                <KTCC:KTTextBox ID="txtPtoStationCd" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td>
                        </tr>
                    </table>

                    <div class="div-detail-table-sub-item">[油圧検査]</div>
                    <table class="table-border-layout grid-layout">
                        <colgroup>
                            <col style="width: 180px" />
                            <col style="width: 180px" />
                            <col style="width: 180px" />
                            <col style="width: 180px" />
                        </colgroup>
                        <tr class="listview-header_2r">
                            <td class="ui-state-default detailtable-header">油圧検査フラグ</td>
                            <td>
                                <KTCC:KTTextBox ID="txtHydraulicFlag" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td>
                            <td class="ui-state-default detailtable-header">油圧検査用錘指示</td>
                            <td>
                                <KTCC:KTTextBox ID="txtHydraulicWeight" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td> 
                        </tr>
                        <tr class="listview-header_2r">
                            <td class="ui-state-default detailtable-header">油圧検査用スリング指示</td>
                            <td>
                                <KTCC:KTTextBox ID="txtHydraulicSling" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td>
                            <td class="ui-state-default detailtable-header">油圧検査用ロアリンクバー</td>
                            <td>
                                <KTCC:KTTextBox ID="txtHydraulicLowerLink" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td> 
                        </tr>
                        <tr class="listview-header_2r">
                            <td class="ui-state-default detailtable-header">結果 油圧検査フラグ</td>
                            <td>
                                <KTCC:KTTextBox ID="txtRtHydraulicFlag" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td>
                            <td class="ui-state-default detailtable-header">油圧検査終了時刻</td>
                            <td>
                                <KTCC:KTTextBox ID="txtRtHydraulicEndTime" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td>
                        </tr>
                        <tr class="listview-header_2r">
                            <td class="ui-state-default detailtable-header">油圧検査<br />ステーション</td>
                            <td>
                                <KTCC:KTTextBox ID="txtHydraulicStationCd" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td>
                        </tr>
                    </table>

                    <div class="div-detail-table-sub-item">[ライト検査]</div>
                    <table class="table-border-layout grid-layout">
                        <colgroup>
                            <col style="width: 180px" />
                            <col style="width: 180px" />
                            <col style="width: 180px" />
                            <col style="width: 180px" />
                        </colgroup>
                        <tr class="listview-header_2r">
                            <td class="ui-state-default detailtable-header">ライト検査フラグ</td>
                            <td>
                                <KTCC:KTTextBox ID="txtHeadlightFlag" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td>
                            <td class="ui-state-default detailtable-header">ライト検査<br />終了時刻</td>
                            <td>
                                <KTCC:KTTextBox ID="txtRtHeadlightEndTime" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td>
                        </tr>
                        <tr class="listview-header_2r">
                            <td class="ui-state-default detailtable-header">結果 ライト検査フラグ</td>
                            <td>
                                <KTCC:KTTextBox ID="txtRtHeadlightFlag" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td>
                            <td class="ui-state-default detailtable-header">ライト検査<br />ステーション</td>
                            <td>
                                <KTCC:KTTextBox ID="txtHeadlightStationCd" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td>
                        </tr>
                    </table>

                    <div class="div-detail-table-sub-item">[キーストップ検査]</div>
                    <table class="table-border-layout grid-layout">
                        <colgroup>
                            <col style="width: 180px" />
                            <col style="width: 180px" />
                            <col style="width: 180px" />
                            <col style="width: 180px" />
                        </colgroup>
                        <tr class="listview-header_2r">
                            <td class="ui-state-default detailtable-header">キーストップ<br />検査フラグ</td>
                            <td>
                                <KTCC:KTTextBox ID="txtKeyStopFlag" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td>
                            <td class="ui-state-default detailtable-header">キーストップ<br />検査終了時刻</td>
                            <td>
                                <KTCC:KTTextBox ID="txtRtKeyStopEmplEndTime" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td>
                        </tr>
                        <tr class="listview-header_2r">
                            <td class="ui-state-default detailtable-header">結果 キーストップ<br />検査フラグ</td>
                            <td>
                                <KTCC:KTTextBox ID="txtRtKeyStopFlag" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td>
                            <td class="ui-state-default detailtable-header">キーストップ<br />ステーション</td>
							<td>
                                <KTCC:KTTextBox ID="txtKeystopStationCd" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td>
                            
                        </tr>
                    </table>

                    <div class="div-detail-table-sub-item">[ハンドル締付検査]</div>
                    <table class="table-border-layout grid-layout">
                        <colgroup>
                            <col style="width: 180px" />
                            <col style="width: 180px" />
                            <col style="width: 180px" />
                            <col style="width: 180px" />
                        </colgroup>
                        <tr class="listview-header_2r">
                            <td class="ui-state-default detailtable-header">ハンドル締付検査フラグ</td>
                            <td>
                                <KTCC:KTTextBox ID="txtSteeringTightenFlag" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td>
                            <td class="ui-state-default detailtable-header">ハンドル締付検査<br />終了時刻</td>
                            <td>
                                <KTCC:KTTextBox ID="txtRtSteeringTightenEndTime" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td>
                        </tr>
                        <tr class="listview-header_2r">
                            <td class="ui-state-default detailtable-header">結果 ハンドル締付<br />検査フラグ</td>
                            <td>
                                <KTCC:KTTextBox ID="txtRtSteeringTightenFlag" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td>
                            <td class="ui-state-default detailtable-header">ハンドル締付検査<br />ステーション</td>
							<td>
                                <KTCC:KTTextBox ID="txtStgtightenStationCd" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </div>
</div>