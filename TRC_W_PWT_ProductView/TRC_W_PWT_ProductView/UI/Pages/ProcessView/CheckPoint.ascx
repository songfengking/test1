<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CheckPoint.ascx.cs" Inherits="TRC_W_PWT_ProductView.UI.Pages.ProcessView.CheckPoint" %>
<%@ Import Namespace="TRC_W_PWT_ProductView.Common" %>

<%-- デザイン表示時使用 マスターページ使用時不要 --%>
<%--<link href="../../../CSS/cobalt/jquery-wijmo.css" rel="stylesheet" />
<link href="../../../CSS/Base.css" rel="stylesheet" />
<link href="../../../CSS/TRC.css" rel="stylesheet" />--%>

<%-- 画面CSS/スクリプト定義領域 --%>
<asp:PlaceHolder ID="SubScripts" runat="server" >
    <script src="<%: ResolveUrl( "~/Scripts/LibScript/scrollViewer.js") %>" ></script>
    <script src="<%: PageUtils.ResolveUrl( this.Page, "~/Scripts/Pages/ProcessView/CheckPoint.js") %>" ></script>
</asp:PlaceHolder>

<div class="div-detail-info-margin">
    <div id="divDetailBodyScroll" class="div-fix-scroll" >
        <KTCC:KTButton ID="btnOutputExcel" runat="server" Text="EXCEL出力" CssClass="btn-middle" OnCommand="btnOutputExcel_Click" OnClientClick="SubmitControl.SetLoadingType(SubmitControl.TYPE_NONE);" />
        <div class="div-detail-table-title">■工程情報</div>
        <div style="margin-top:10px;"/>
        <%-- 工程情報定義 --%>
        <asp:UpdatePanel ID="upnCondition" runat="server">
        <ContentTemplate>
        <div id="divProcessArea" class="div-y-scroll-flt2" >
            <table class="table-layout-fix" style="margin-left:10px">
                <tr>
                    <td>
                        <div id="divProcessLTScroll" class="div-fix-scroll div-left-grid">
                            <div id="divProcessLTHeader" runat="server">
                                <table id="solidProcessLTHeader" runat="server" class="grid-layout" style="width: 660px;">
                                    <tr id="processLTHeaderContent"     runat="server" class="listview-header_2r ui-state-default">
                                        <th id="stationCd"              runat="server" style="width:110px">ステーション</th>
                                        <th id="stationNm"              runat="server" style="width:250px">ステーション名</th>
                                        <th id="resultDifferenceFlg1"   runat="server" class="tr-fix-zero-height"></th>
                                        <th id="workStartDt"            runat="server" style="width:150px">作業開始時刻</th>
                                        <th id="workEndDt"              runat="server" style="width:150px">作業終了時刻</th>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </td>
                    <td>
                        <div id="divProcessRTScroll" class="div-scroll-right-top div-right-grid">
                            <div id="divProcessRTHeader" runat="server">
                                <table id="solidProcessRTHeader" runat="server" class="grid-layout" style="width: 480px;">
                                    <tr id="processRTHeaderContent"     runat="server" class="listview-header_2r ui-state-default">
                                        <th id="checkPoint"             runat="server" style="width:80px">関所</th>
                                        <th id="plcNextPage"            runat="server" style="width:80px">次ページ</th>
                                        <th id="plcPrev"                runat="server" style="width:80px">前データ</th>
                                        <th id="plcNext"                runat="server" style="width:80px">次データ</th>
                                        <th id="plcMissing"             runat="server" style="width:80px">歯抜け</th>
                                        <th id="resultDifferenceFlg2"   runat="server" class="tr-fix-zero-height"></th>
                                        <th id="plcRepair"              runat="server" style="width:80px">手直し</th>
                                    </tr>
                                </table>
                            </div>
                        </div>
                   </td>
                </tr>
                <tr>
                    <td>
                        <div id="divProcessLBScroll" class="div-scroll-left-bottom div-left-grid" style="height: 150px">
                            <div id="divProcessLB" runat="server">
                                <asp:ListView ID="listProcessLB" runat="server" OnItemDataBound="listProcessLB_ItemDataBound" OnSelectedIndexChanging="listProcessLB_SelectedIndexChanging" OnSelectedIndexChanged="listProcessLB_SelectedIndexChanged">
                                    <LayoutTemplate>
                                        <table id="itemPlaceholderContainerProcessLB" runat="server" class="grid-layout" style="width:660px">
                                            <tr id="processLBHeaderContent"     runat="server" class="listview-header_3r ui-state-default">
                                                <th id="stationCd"              runat="server" style="width:110px"/>
                                                <th id="stationNm"              runat="server" style="width:250px"/>
                                                <th id="resultDifferenceFlg1"   runat="server" class="tr-fix-zero-height"/>
                                                <th id="workStartDt"            runat="server" style="width:150px"/>
                                                <th id="workEndDt"              runat="server" style="width:150px"/>
                                            </tr>
                                            <tr id="itemPlaceholder" runat="server">
                                            </tr>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr id="trRowData" runat="server" class="listview-row ui-widget">
                                            <td>
                                                <asp:Button ID="btnSelect" runat="server" CommandName="Select" Text="" CssClass="btnSelect invisible" />
                                                <KTCC:KTTextBox ID="txtStationCd" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" ></KTCC:KTTextBox>
                                            </td>
                                            <td>
                                                <KTCC:KTTextBox ID="txtStationNm" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                                            </td>
                                            <td class="tr-fix-zero-height">
                                                <KTCC:KTTextBox ID="txtResultDifferenceFlg1" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                                            </td>
                                            <td>
                                                <KTCC:KTTextBox ID="txtWorkStartDt" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                            </td>
                                            <td>
                                                <KTCC:KTTextBox ID="txtWorkEndDt" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:ListView>
                            </div>
                        </div>
                    </td>
                    <td>
                        <div id="divProcessRBScroll" class="div-visible-scroll div-right-grid" style="height: 150px">
                            <div id="divProcessRB" runat="server">
                                <asp:ListView ID="listProcessRB" runat="server" OnItemDataBound="listProcessRB_ItemDataBound" OnSelectedIndexChanging="listProcessRB_SelectedIndexChanging" OnSelectedIndexChanged="listProcessRB_SelectedIndexChanged">
                                    <LayoutTemplate>
                                        <table id="itemPlaceholderContainerProcessRB" runat="server" class="grid-layout" style="width:480px">
                                            <tr id="processRBHeaderContent"     runat="server" class="listview-header_3r ui-state-default">
                                                <th id="checkPoint"             runat="server" style="width:80px"/>
                                                <th id="plcNextPage"            runat="server" style="width:80px"/>
                                                <th id="plcPrev"                runat="server" style="width:80px"/>
                                                <th id="plcNext"                runat="server" style="width:80px"/>
                                                <th id="plcMissing"             runat="server" style="width:80px"/>
                                                <th id="resultDifferenceFlg2"   runat="server" class="tr-fix-zero-height"/>
                                                <th id="plcRepair"              runat="server" style="width:80px"/>
                                            </tr>
                                            <tr id="itemPlaceholder" runat="server">
                                            </tr>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr id="trRowData" runat="server" class="listview-row ui-widget">
                                            <td>
                                                <asp:Button ID="btnSelect" runat="server" CommandName="Select" Text="" CssClass="btnSelect invisible" />
                                                <KTCC:KTTextBox ID="txtCheckPoint" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" ></KTCC:KTTextBox>
                                            </td>
                                            <td>
                                                <KTCC:KTTextBox ID="txtPlcNextPage" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                            </td>
                                            <td>
                                                <KTCC:KTTextBox ID="txtPlcPrev" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                            </td>
                                            <td>
                                                <KTCC:KTTextBox ID="txtPlcNext" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                            </td>
                                            <td>
                                                <KTCC:KTTextBox ID="txtPlcMissing" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                            </td>
                                            <td class="tr-fix-zero-height">
                                                <KTCC:KTTextBox ID="txtResultDifferenceFlg2" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                                            </td>
                                            <td>
                                                <KTCC:KTTextBox ID="txtPlcRepair" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
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
        </ContentTemplate>
        </asp:UpdatePanel>

        <%-- 工程作業実績定義 --%>
        <div class="div-detail-table-title">■工程作業実績</div>
        <div style="margin-top:10px;"></div>
        <div id="divWorkArea" class="div-fix-scroll-flt">
            <div id="divDetailViewBox" class="div-auto-scroll">
                <table class="table-layout-fix" style="margin-left:10px">
                    <tr>
                        <td>
                            <table id="solidWorkHeader" runat="server" class="grid-layout" style="width: 490px;">
                                <tr id="workHeaderContent"      runat="server" class="listview-header_2r ui-state-default">
                                    <th id="instructOrder"      runat="server" style="width: 80px">順序</th>
                                    <th id="instructContent"    runat="server" style="width: 550px">作業内容</th>
                                    <th id="toolNm"             runat="server" style="width: 200px">工具</th>
                                    <th id="instructCnt"        runat="server" style="width:100px">指示回数</th>
                                    <th id="resultCnt"          runat="server" style="width:100px">実績回数</th>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:ListView ID="listWork" runat="server" OnItemDataBound="listWork_ItemDataBound">
                                <LayoutTemplate>
                                    <table id="itemPlaceholderContainerWork" runat="server" class="grid-layout" style="width:490px">
                                        <tr id="workHeaderContent"      runat="server" class="listview-header_3r ui-state-default">
                                            <th id="instructOrder"      runat="server" style="width: 80px"/>
                                            <th id="instructContent"    runat="server" style="width: 550px"/>
                                            <th id="toolNm"             runat="server" style="width: 200px"/>
                                            <th id="instructCnt"        runat="server" style="width:100px"/>
                                            <th id="resultCnt"          runat="server" style="width:100px"/>
                                        </tr>
                                        <tr id="itemPlaceholder" runat="server">
                                        </tr>
                                    </table>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <tr id="trRowData" runat="server" class="listview-row ui-widget">
                                        <td>
                                            <KTCC:KTTextBox ID="txtInstructOrder"   runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-rg dmy-row" />
                                        </td>
                                        <td>
                                            <KTCC:KTTextBox ID="txtInstructContent" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf dmy-row" />
                                        </td>
                                        <td>
                                            <KTCC:KTTextBox ID="txtToolNm"          runat="server" ReadOnly="true" CssClass="font-default txt-width-full al-lf dmy-row" />
                                        </td>
                                        <td>
                                            <KTCC:KTTextBox ID="txtInstructCnt"     runat="server" ReadOnly="true" CssClass="font-default txt-width-full al-rg dmy-row" />
                                        </td>
                                        <td>
                                            <KTCC:KTTextBox ID="txtResultCnt"       runat="server" ReadOnly="true" CssClass="font-default txt-width-full al-rg dmy-row" />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:ListView>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
</div>