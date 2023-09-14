<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ELCheckSheet.ascx.cs" Inherits="TRC_W_PWT_ProductView.UI.Pages.ProcessView.ELCheckSheet" %>
<%@ Import Namespace="TRC_W_PWT_ProductView.Common" %>

<%-- デザイン表示時使用 マスターページ使用時不要 --%>
<%--<link href="../../../CSS/cobalt/jquery-wijmo.css" rel="stylesheet" />
<link href="../../../CSS/Base.css" rel="stylesheet" />
<link href="../../../CSS/TRC.css" rel="stylesheet" />--%>

<%-- 画面CSS/スクリプト定義領域 --%>
<asp:PlaceHolder ID="SubScripts" runat="server" >
    <script src="<%: ResolveUrl( "~/Scripts/LibScript/scrollViewer.js") %>" ></script>
    <script src="<%: PageUtils.ResolveUrl( this.Page, "~/Scripts/Pages/ProcessView/ELCheckSheet.js") %>" ></script>
</asp:PlaceHolder>

<div class="div-detail-info-margin">
    <div id="divDetailBodyScroll" class="div-fix-scroll" >
        <KTCC:KTButton ID="btnPrint" runat="server" Text="印刷" CssClass="btn-middle" OnCommand="btnPrint_Click" OnClientClick="SubmitControl.SetLoadingType(SubmitControl.TYPE_NONE);" />
        <div class="div-detail-table-title">■検査情報</div>
        <div style="margin-top:10px;"/>

        <%-- 検査情報定義 --%>
            <div id="divMainListArea" class="div-y-scroll-flt2" >
                <table class="table-layout-fix" style="margin-left:10px">
                    <tr>
                        <td>
                            <div id="divLTMainScroll" class="div-fix-scroll div-left-grid">
                                <div id="divMainHeaderLT" runat="server">
                                    <table id="solidLTMainHeader" runat="server" class="grid-layout" style="width: 720px;">
                                        <tr id="headerLTMainContent"      runat="server" class="listview-header_2r ui-state-default">
                                            <th id="OrderNo"    runat="server" style="width:110px">確定順序連番</th>
                                            <th id="LineCd"     runat="server" class="tr-fix-zero-height"></th>
                                            <th id="LineNm"     runat="server" style="width:250px">ライン名</th>
                                            <th id="StartDt"    runat="server" style="width:150px">検査開始日時</th>
                                            <th id="EndDt"      runat="server" style="width:150px">検査終了日時</th>
                                            <th id="Pass"       runat="server" style="width: 60px">結果</th>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </td>
                        <td>
                            <div id="divRTMainScroll" class="div-scroll-right-top div-right-grid">
                                <div id="divMainHeaderRT" runat="server">
                                    <table id="solidMainRTHeader" runat="server" class="grid-layout" style="width: 500px;">
                                        <tr id="headerRTMainContent"    runat="server" class="listview-header_2r ui-state-default">
                                            <th id="lastProc"    runat="server" style="width:140px">最終検査工程</th>
                                            <th id="Th23"       runat="server" style="width:160px">合格判定社員名</th>
                                            <th id="Remark"     runat="server" style="width:200px">特記事項</th>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                       </td>
                    </tr>
                    <tr>
                        <td>
                            <div id="divLBMainScroll" class="div-scroll-left-bottom div-left-grid" style="height: 70px">
                                <div id="divGrvLBMain" runat="server">
                                    <asp:ListView ID="lstMainListLB" runat="server" OnItemDataBound="lstMainListLB_ItemDataBound" OnSelectedIndexChanging="lstMainListLB_SelectedIndexChanging" OnSelectedIndexChanged="lstMainListLB_SelectedIndexChanged">
                                        <LayoutTemplate>
                                            <table id="itemPlaceholderContainerLBMain" runat="server" class="grid-layout" style="width:720px">
                                                <tr id="headerLBMainContent"   runat="server" class="listview-header_3r ui-state-default">
                                                    <th id="OrderNo"    runat="server" style="width:110px"/>
                                                    <th id="LineCd"     runat="server" class="tr-fix-zero-height"/>
                                                    <th id="LineNm"     runat="server" style="width:250px"/>
                                                    <th id="StartDt"    runat="server" style="width:150px"/>
                                                    <th id="EndDt"      runat="server" style="width:150px"/>
                                                    <th id="Pass"       runat="server" style="width: 60px"/>
                                                </tr>
                                                <tr id="itemPlaceholder" runat="server">
                                                </tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr id="trRowData" runat="server" class="listview-row ui-widget">
                                                <td>
                                                    <asp:Button ID="btnSelect" runat="server" CommandName="Select" Text="" CssClass="btnSelect invisible" />
                                                    <KTCC:KTTextBox ID="txtOrderNo" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" ></KTCC:KTTextBox>
                                                </td>
                                                <td class="tr-fix-zero-height">
                                                    <KTCC:KTTextBox ID="txtLineCd"  runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                                                </td>
                                                <td>
                                                    <KTCC:KTTextBox ID="txtLineNm"  runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                                                </td>
                                                <td>
                                                    <KTCC:KTTextBox ID="txtStartDt" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                                </td>
                                                <td>
                                                    <KTCC:KTTextBox ID="txtEndDt"   runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                                </td>
                                                <td>
                                                    <KTCC:KTTextBox ID="txtPass"    runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:ListView>
                                </div>
                            </div>
                        </td>
                        <td>
                            <div id="divRBMainScroll" class="div-visible-scroll div-right-grid" style="height: 70px">
                                <div id="divGrvRBMain" runat="server">
                                    <asp:ListView ID="lstMainListRB" runat="server" OnItemDataBound="lstMainListRB_ItemDataBound" OnSelectedIndexChanging="lstMainListRB_SelectedIndexChanging" OnSelectedIndexChanged="lstMainListRB_SelectedIndexChanged">
                                        <LayoutTemplate>
                                            <table id="itemPlaceholderContainerRBMain" runat="server" class="grid-layout" style="width:500px">
                                                <tr id="headerRBMainContent"   runat="server" class="listview-header_3r ui-state-default">
                                                    <th id="lastProc"    runat="server" style="width:140px"/>
                                                    <th id="EmpCd"      runat="server" style="width:160px"/>
                                                    <th id="Remark"     runat="server" style="width:200px"/>
                                                </tr>
                                                <tr id="itemPlaceholder" runat="server">
                                                </tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr id="trRowData" runat="server" class="listview-row ui-widget">
                                                <td>
                                                    <asp:Button ID="btnSelect" runat="server" CommandName="Select" Text="" CssClass="btnSelect invisible" />
                                                    <KTCC:KTTextBox ID="txtLastProc" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" ></KTCC:KTTextBox>
                                                </td>
                                                <td>
                                                    <KTCC:KTTextBox ID="txtEmpCd"   runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                                                </td>
                                                <td>
                                                    <KTCC:KTTextBox ID="txtRemark"  runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
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

        <div class="div-detail-table-title">■検査結果</div>
        <div style="margin-top:10px;"></div>
        <%-- 検査結果定義 --%>
        <div id="tabResult" class ="tabbox" style="margin-left:10px">
            <ul class="tabs">
                <li class="tabResult"><a href="#tabChkInfo" class="tabChkInfo tabColor"  onclick="ELCheckSheetDef.ChangeTab('tabChkInfo'); return false" style="width:100px;">検査情報</a></li>
                <li class="tabResult"><a href="#tabChkImg" class="tabChkImg tabColor" onclick="ELCheckSheetDef.ChangeTab('tabChkImg'); return false;" style="width:100px;">検査画像</a></li>
                <li class="tabResult"><a href="#tabNGList" class="tabNGList tabColor" onclick="ELCheckSheetDef.ChangeTab('tabNGList'); return false;" style="width:100px;">不具合一覧</a></li>
                <li class="tabResult"><a href="#tabNGImg" class="tabNGImg tabColor" onclick="ELCheckSheetDef.ChangeTab('tabNGImg'); return false;" style="width:100px;">不具合画像</a></li>
            </ul>

            <%-- 検査情報タブ定義 --%>

            <div id="tabChkInfo" class="tabResult tabDiv">
                <div style="clear: both; width:auto;margin-top:10px"></div>
                <table class="table-layout-fix" style="margin-left:10px">
                    <tr>
                        <td>
                            <div id="divLTScroll" class="div-fix-scroll div-left-grid">
                                <div id="divHeaderLT" runat="server">
                                    <table id="solidChkHeader" runat="server" class="grid-layout" style="width: 490px;">
                                        <tr id="headerMainContent"      runat="server" class="listview-header_2r ui-state-default">
                                            <th id="Proc"               runat="server" style="width: 80px">工程</th>
                                            <th id="GuaranteeNo"        runat="server" style="width: 40px">№</th>
                                            <th id="CritKind"           runat="server" style="width: 50px">区分</th>
                                            <th id="GuaranteeNm"        runat="server" style="width:320px">保証項目</th>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </td>
                        <td>
                            <div id="divRTScroll" class="div-scroll-right-top div-right-grid">
                                <div id="divHeaderRT" runat="server">
                                    <table id="solidChkHeaderRT" runat="server" class="grid-layout" style="width: 690px;">
                                        <tr id="headerMainContentRT"    runat="server" class="listview-header_2r ui-state-default">
                                            <th id="Input1"             runat="server" style="width:150px">入力値1</th>
                                            <th id="Input2"             runat="server" style="width:150px">入力値2</th>
                                            <th id="PassStatus"         runat="server" style="width: 80px">検査結果</th>
                                            <th id="InsDt"              runat="server" style="width:150px">検査日時</th>
                                            <th id="EmpCd"              runat="server" style="width:160px">検査社員名</th>
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
                                    <asp:ListView ID="lstChkInfo" runat="server" OnItemDataBound="lstChkInfo_ItemDataBound">
                                        <LayoutTemplate>
                                            <table id="itemPlaceholderContainer_Info" runat="server" class="grid-layout" style="width:490px">
                                                <tr id="headerChkInfoContent"   runat="server" class="listview-header_3r ui-state-default">
                                                    <th id="Proc"               runat="server" style="width: 80px"/>
                                                    <th id="GuaranteeNo"        runat="server" style="width: 40px"/>
                                                    <th id="CritKind"           runat="server" style="width: 50px"/>
                                                    <th id="GuaranteeNm"        runat="server" style="width:320px"/>
                                                </tr>
                                                <tr id="itemPlaceholder" runat="server">
                                                </tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr id="trRowData" runat="server" class="listview-row ui-widget" style="height:42px">
                                                <td>
                                                    <KTCC:KTTextBox ID="txtProc"        runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf dmy-row" />
                                                </td>
                                                <td>
                                                    <KTCC:KTTextBox ID="txtGuaranteeNo" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct dmy-row" />
                                                </td>
                                                <td>
                                                    <KTCC:KTTextBox ID="txtCritKind"    runat="server" ReadOnly="true" CssClass="font-default txt-width-full al-ct dmy-row" TextMode="MultiLine"/>
                                                </td>
                                                <td>
                                                    <KTCC:KTTextBox ID="txtGuaranteeNm" runat="server" ReadOnly="true" CssClass="font-default txt-width-full al-lf dmy-row" TextMode="MultiLine" />
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
                                    <asp:ListView ID="lstChkInfo2" runat="server" OnItemDataBound="lstChkInfo2_ItemDataBound">
                                        <LayoutTemplate>
                                            <table id="itemPlaceholderContainer_Info2" runat="server" class="grid-layout" style="width:690px">
                                                <tr id="headerChkInfoContent2"   runat="server" class="listview-header_3r ui-state-default">
                                                    <th id="Input1"             runat="server" style="width:150px"/>
                                                    <th id="Input2"             runat="server" style="width:150px"/>
                                                    <th id="PassStatus"         runat="server" style="width: 80px"/>
                                                    <th id="InsDt"              runat="server" style="width:150px"/>
                                                    <th id="EmpCd"              runat="server" style="width:160px"/>
                                                </tr>
                                                <tr id="itemPlaceholder" runat="server">
                                                </tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr id="trRowData" runat="server" class="listview-row ui-widget" style="height:42px">
                                                <td>
                                                    <KTCC:KTTextBox  ID="txtInput1"     runat="server" ReadOnly="true" CssClass="font-default txt-width-full al-lf dmy-row" TextMode="MultiLine" />
                                                </td>
                                                <td>
                                                    <KTCC:KTTextBox ID="txtInput2"      runat="server" ReadOnly="true" CssClass="font-default txt-width-full al-lf dmy-row" TextMode="MultiLine" />
                                                </td>
                                                <td>
                                                    <KTCC:KTTextBox ID="txtPassStatus"  runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct dmy-row" />
                                                </td>
                                                <td>
                                                    <KTCC:KTTextBox ID="txtInsDt"       runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf dmy-row" />
                                                </td>
                                                <td>
                                                    <KTCC:KTTextBox ID="txtEmpCd"       runat="server" ReadOnly="true" CssClass="font-default txt-width-full al-lf dmy-row" TextMode="MultiLine"/>
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
            
            <%-- 検査画像タブ定義 --%>
            <div id="tabChkImg" class="tabResult tabDiv" style="overflow:hidden">
                <div id="divChkListArea" class="div-y-scroll-flt" style="width: 232px">
                    <asp:ListView ID="lstChkImageList" runat="server" OnItemDataBound="lstChkImageList_ItemDataBound">
                        <LayoutTemplate>
                            <div class="" id="itemPlaceholder" runat="server" />
                        </LayoutTemplate>
                        <ItemTemplate>
                            <div id="divRowData" runat="server" class="div-list-view-item" style="width: 208px; height: auto">
                                <table class="table-border-layout" style="margin-left: 0px; margin-right:1px">
                                    <colgroup>
		                                <col style="width: 202px" />
		                            </colgroup>
                                    <tr>
                                        <td>
                                            <div>
                                                <asp:Image ID="imgCameraImage" runat="server" CssClass="thumbnail-area" />
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <KTCC:KTTextBox ID="txtProcInfo"  runat="server" ReadOnly="true"  CssClass="font-default  txt-width-full al-lf" TextMode="MultiLine" ></KTCC:KTTextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <KTCC:KTTextBox ID="txtImgTitle"  runat="server" ReadOnly="true" CssClass="font-default  txt-width-full al-lf" TextMode="MultiLine" ></KTCC:KTTextBox>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </ItemTemplate>
                    </asp:ListView>
                </div>
                <div id="divChkViewArea">
                    <div id="divChkViewBox" class="div-auto-scroll" style="padding: 3px">
                        <asp:Image ID="imgMainArea2" runat="server" AlternateText="" />
                    </div>
                </div>
            </div>

            <%-- 不具合一覧タブ定義 --%>
            <div id="tabNGList" class="tabResult tabDiv">
                <div style="clear: both; width:auto;margin-top:10px"></div>
                <table class="table-layout-fix" style="margin-left:10px">
                    <tr>
                        <td>
                            <div id="divNGLTScroll" class="div-fix-scroll div-left-grid">
                                <div id="divNGHeaderLT" runat="server">
                                    <table id="solidNGListHeaderLT" runat="server" class="grid-layout" style="width: 530px;">
                                        <tr id="headerNGContentLT"  runat="server" class="listview-header_2r ui-state-default">
                                            <th id="Th1"            runat="server" style="width: 80px">工程</th>
                                            <th id="Th2"            runat="server" style="width: 40px">№</th>
                                            <th id="RecNo"          runat="server" style="width: 40px">連番</th>
                                            <th id="Th3"            runat="server" style="width: 50px">区分</th>
                                            <th id="Guarant"        runat="server" style="width:320px">保証項目</th>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </td>
                        <td>
                            <div id="divNGRTScroll" class="div-scroll-right-top div-right-grid">
                                <div id="divNGHeaderRT" runat="server">
                                    <table id="solidNGListHeaderRT" runat="server" class="grid-layout" style="width:3090px;">
                                        <tr id="Tr3"    runat="server" class="listview-header_2r ui-state-default">
                                            <th id="ConfStatus"     runat="server" style="width:90px">ステータス</th>
                                            <th id="NGPrts"         runat="server" style="width:250px">不具合部品</th>
                                            <th id="NGDtl"          runat="server" style="width:470px">不具合内容</th>
                                            <th id="NGQty"          runat="server" style="width:250px">不具合<br />場所・数量</th>
                                            <th id="NGValue"        runat="server" style="width:250px">不具合値</th>
                                            <th id="NGDt"           runat="server" style="width:150px">不具合記入日時</th>
                                            <th id="NGEmpCd"        runat="server" style="width:160px">不具合記入社員名</th>
                                            <th id="NGImgCnt"       runat="server" style="width: 90px">不具合写真<br />枚数</th>
                                            <th id="AdjStaus"       runat="server" style="width:150px">手直し有無</th>
                                            <th id="AdjDtl"         runat="server" style="width:470px">手直し内容</th>
                                            <th id="CauseLine"      runat="server" style="width:250px">起因ライン</th>
                                            <th id="CauseLineDtl"   runat="server" style="width:250px">起因ライン特記事項</th>
                                            <th id="AdjDt"          runat="server" style="width:150px">手直し対応日時</th>
                                            <th id="AdjEmpCd"       runat="server" style="width:160px">手直し対応社員名</th>
                                            <th id="AdjImgCnt"      runat="server" style="width: 90px">手直し写真<br/>枚数</th>
                                            <th id="ConfDt"         runat="server" style="width:150px">確認日時</th>
                                            <th id="ConfEmpCd"      runat="server" style="width:160px">確認社員名</th>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div id="divNGLBScroll" class="div-scroll-left-bottom div-left-grid">
                                <div id="divNGList" runat="server">
                                    <asp:ListView ID="lstNG" runat="server" OnItemDataBound="lstNG_ItemDataBound">
                                        <LayoutTemplate>
                                            <table id="itemPlaceholderContainer_NGList" runat="server" class="grid-layout" style="width:530px">
                                                <tr id="headerNGListContent"   runat="server" class="listview-header_3r ui-state-default">
                                                    <th id="Proc"           runat="server" style="width: 80px"/>
                                                    <th id="GuaranteeNo"    runat="server" style="width: 40px"/>
                                                    <th id="RecNo"          runat="server" style="width: 40px"/>
                                                    <th id="CritKind"       runat="server" style="width: 50px"/>
                                                    <th id="Guarant"        runat="server" style="width:320px"/>
                                                </tr>
                                                <tr id="itemPlaceholder" runat="server">
                                                </tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr id="trRowData" runat="server" class="listview-row ui-widget" style="height:42px">
                                                <td>
                                                    <KTCC:KTTextBox ID="txtProc"        runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                                                </td>
                                                <td>
                                                    <KTCC:KTTextBox ID="txtGuaranteeNo" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                                </td>
                                                <td>
                                                    <KTCC:KTTextBox ID="txtRecNo"       runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                                </td>
                                                <td>
                                                    <KTCC:KTTextBox ID="txtCritKind"    runat="server" ReadOnly="true" CssClass="font-default txt-width-full al-ct" TextMode="MultiLine"/>
                                                </td>
                                                <td>
                                                    <KTCC:KTTextBox ID="txtGuarant"     runat="server" ReadOnly="true" CssClass="font-default txt-width-full al-lf" TextMode="MultiLine" />
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:ListView>
                                </div>
                            </div>
                        </td>
                        <td>
                            <div id="divNGRBScroll" class="div-visible-scroll div-right-grid">
                                <div id="divNGRBList" runat="server">
                                    <asp:ListView ID="lstNGRB" runat="server" OnItemDataBound="lstNGRB_ItemDataBound">
                                        <LayoutTemplate>
                                            <table id="itemPlaceholderContainer_NGRBList" runat="server" class="grid-layout" style="width:3090px">
                                                <tr id="headerNGRBListContent"   runat="server" class="listview-header_3r ui-state-default">
                                                    <th id="ConfStatus"     runat="server" style="width: 90px"/>
                                                    <th id="NGPrts"         runat="server" style="width:250px"/>
                                                    <th id="NGDtl"          runat="server" style="width:470px"/>
                                                    <th id="NGQty"          runat="server" style="width:250px"/>
                                                    <th id="NGValue"        runat="server" style="width:250px"/>
                                                    <th id="NGDt"           runat="server" style="width:150px"/>
                                                    <th id="NGEmpCd"        runat="server" style="width:160px"/>
                                                    <th id="NGImgCnt"       runat="server" style="width: 90px"/>
                                                    <th id="AdjStaus"       runat="server" style="width:150px"/>
                                                    <th id="AdjDtl"         runat="server" style="width:470px"/>
                                                    <th id="CauseLine"      runat="server" style="width:250px"/>
                                                    <th id="CauseLineDtl"   runat="server" style="width:250px"/>
                                                    <th id="AdjDt"          runat="server" style="width:150px"/>
                                                    <th id="AdjEmpCd"       runat="server" style="width:160px"/>
                                                    <th id="AdjImgCnt"      runat="server" style="width: 90px"/>
                                                    <th id="ConfDt"         runat="server" style="width:150px"/>
                                                    <th id="ConfEmpCd"      runat="server" style="width:160px"/>
                                                </tr>
                                                <tr id="itemPlaceholder" runat="server">
                                                </tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr id="trRowData" runat="server" class="listview-row ui-widget" style="height:42px">
                                                <td>
                                                    <KTCC:KTTextBox ID="txtConfStatus"  runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                                </td>
                                                <td>
                                                    <KTCC:KTTextBox ID="txtNGPrts"      runat="server" ReadOnly="true" CssClass="font-default txt-width-full al-lf" TextMode="MultiLine" />
                                                </td>
                                                <td>
                                                    <KTCC:KTTextBox ID="txtNGDtl"       runat="server" ReadOnly="true" CssClass="font-default txt-width-full al-lf" TextMode="MultiLine" />
                                                </td>
                                                <td>
                                                    <KTCC:KTTextBox ID="txtNGQty"       runat="server" ReadOnly="true" CssClass="font-default txt-width-full al-lf" TextMode="MultiLine" />
                                                </td>
                                                <td>
                                                    <KTCC:KTTextBox ID="txtNGValue"     runat="server" ReadOnly="true" CssClass="font-default txt-width-full al-lf" TextMode="MultiLine" />
                                                </td>
                                                <td>
                                                    <KTCC:KTTextBox ID="txtNGDt"        runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                                </td>
                                                <td>
                                                    <KTCC:KTTextBox ID="txtNGEmpCd"     runat="server" ReadOnly="true" CssClass="font-default txt-width-full al-lf" TextMode="MultiLine" />
                                                </td>
                                                <td>
                                                    <KTCC:KTTextBox ID="txtNGImgCnt"    runat="server" ReadOnly="true" CssClass="font-default txt-default  txt-width-full al-ct" />
                                                </td>
                                                <td>
                                                    <KTCC:KTTextBox ID="txtAdjStaus"    runat="server" ReadOnly="true" CssClass="font-default txt-default  txt-width-full al-ct" />
                                                </td>
                                                <td>
                                                    <KTCC:KTTextBox ID="txtAdjDtl"      runat="server" ReadOnly="true" CssClass="font-default txt-width-full al-lf" TextMode="MultiLine" />
                                                </td>
                                                <td>
                                                    <KTCC:KTTextBox ID="txtCauseLine"   runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                                                </td>
                                                <td>
                                                    <KTCC:KTTextBox ID="txtCauseLineDtl" runat="server" ReadOnly="true" CssClass="font-default  txt-width-full al-lf" TextMode="MultiLine" />
                                                </td>
                                                <td>
                                                    <KTCC:KTTextBox ID="txtAdjDt"       runat="server" ReadOnly="true" CssClass="font-default txt-default  txt-width-full al-ct" />
                                                </td>
                                                <td>
                                                    <KTCC:KTTextBox ID="txtAdjEmpCd"    runat="server" ReadOnly="true" CssClass="font-default txt-width-full al-lf" TextMode="MultiLine"/>
                                                </td>
                                                <td>
                                                    <KTCC:KTTextBox ID="txtAdjImgCnt"   runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                                </td>
                                                <td>
                                                    <KTCC:KTTextBox ID="txtConfDt"      runat="server" ReadOnly="true" CssClass="font-default txt-default  txt-width-full al-ct" />
                                                </td>
                                                <td>
                                                    <KTCC:KTTextBox ID="txtConfEmpCd"   runat="server" ReadOnly="true" CssClass="font-default txt-width-full al-lf" TextMode="MultiLine" />
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

            <%-- 不具合画像タブ定義 --%>
            <div id="tabNGImg" class="tabResult tabDiv" style="overflow:hidden">
                <div id="divNGListArea" class="div-y-scroll-flt" style="width: 232px">
                    <asp:ListView ID="lstNGImageList" runat="server" OnItemDataBound="lstNGImageList_ItemDataBound">
                        <LayoutTemplate>
                            <div class="" id="itemPlaceholder" runat="server" />
                        </LayoutTemplate>
                        <ItemTemplate>
                            <div id="divRowData" runat="server" class="div-list-view-item" style="width: 208px; height: auto">
                                <table class="table-border-layout" style="margin-left: 0px; margin-right:1px">
                                    <colgroup>
		                                <col style="width: 202px" />
		                            </colgroup>
                                    <tr>
                                        <td>
                                            <div>
                                                <asp:Image ID="imgCameraImage" runat="server" CssClass="thumbnail-area" />
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <KTCC:KTTextBox ID="txtProcInfo" runat="server" ReadOnly="true"  CssClass="font-default  al-lf" TextMode="MultiLine"></KTCC:KTTextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <KTCC:KTTextBox ID="txtNGPrts"  runat="server" ReadOnly="true" CssClass="font-default  txt-width-full al-lf" TextMode="MultiLine" ></KTCC:KTTextBox>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <KTCC:KTTextBox ID="txtNGDtl"   runat="server" ReadOnly="true" CssClass="font-default  txt-width-full al-lf" TextMode="MultiLine" ></KTCC:KTTextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <KTCC:KTTextBox ID="txtNGQty"   runat="server" ReadOnly="true" CssClass="font-default  txt-width-full al-lf" TextMode="MultiLine" ></KTCC:KTTextBox>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </ItemTemplate>
                    </asp:ListView>
                </div>
                <div id="divNGViewArea">
                    <div id="divNGViewBox" class="div-auto-scroll" style="padding: 3px">
                        <asp:Image ID="imgMainArea" runat="server" AlternateText="" />
                    </div>
                </div>
            </div>

        </div>
        <%-- タブアクション定義 --%>
        <script type="text/javascript">
        <!--
        //デフォルトタブ設定
            ELCheckSheetDef.ChangeTab('tabChkInfo');
        // --></script> 
    </div>
</div>