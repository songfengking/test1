<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CriticalInspection.ascx.cs" Inherits="TRC_W_PWT_ProductView.UI.Pages.ProcessView.CriticalInspection" %>
<%@ Import Namespace="TRC_W_PWT_ProductView.Common" %>

<%-- デザイン表示時使用 マスターページ使用時不要 --%>
<%--<link href="../../../CSS/cobalt/jquery-wijmo.css" rel="stylesheet" />
<link href="../../../CSS/Base.css" rel="stylesheet" />
<link href="../../../CSS/TRC.css" rel="stylesheet" />--%>

<%-- 画面CSS/スクリプト定義領域 --%>
<asp:PlaceHolder ID="SubScripts" runat="server">
    <script src="<%: PageUtils.ResolveUrl( this.Page, "~/Scripts/Pages/ProcessView/CriticalInspection.js") %>"></script>
</asp:PlaceHolder>

<div class="div-detail-info-margin">
    <div id="divDetailBodyScroll" class="div-fix-scroll">
        <div class="div-detail-table-title">■検査情報</div>
        <div style="clear: both; width:auto;margin-top:10px"></div>
        <div id="divMainListArea" style="height: 150px">
            <table class="table-layout-fix" style="margin-left:10px">
                <tr>
                    <td>
                        <div id="divLTScroll" class="div-fix-scroll div-left-grid">
                            <div id="divHeaderLT" runat="server">
                                <table id="solidLTHeader" runat="server" class="grid-layout" style="width: 360px;">
                                    <tr id="headerLTMainContent"      runat="server" class="listview-header_2r ui-state-default">
                                        <th id="Th1"  runat="server" style="width: 150px;">検査日時</th>
                                        <th id="Th2"  runat="server" style="width: 160px;">部品名</th>
                                        <th id="Th3"  runat="server" style="width:  90px;">加工日</th>
                                        <th id="Th20" runat="server" style="width:  60px;">連番</th>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </td>
                    <td>
                        <div id="divRTScroll" class="div-scroll-right-top div-right-grid">
                            <div id="divHeaderRT" runat="server">
                                <table id="solidRTHeader" runat="server" class="grid-layout" style="width: 950px;">
                                    <tr id="headerRTMainContent"    runat="server" class="listview-header_2r ui-state-default">
                                        <th id="Th5" runat="server" style="width: 120px;">品番</th>
                                        <th id="Th6" runat="server" style="width:  80px;">作業区分</th>
                                        <th id="Th7" runat="server" style="width: 150px;">納入日時</th>
                                        <th id="Th8" runat="server" style="width: 200px;">結果</th>
                                        <th id="Th9" runat="server" style="width: 400px;">備考</th>
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
                                        <table id="itemPlaceholderContainerLB" runat="server" class="grid-layout" style="width:360px">
                                            <tr id="headerLBContent"   runat="server" class="listview-header_3r ui-state-default">
                                                <th id="Th1" 　runat="server" style="width:150px;"/>
                                                <th id="Th2"   runat="server" style="width:160px;"/>
                                                <th id="Th3"   runat="server" style="width: 90px;"/>
                                                <th id="Th4"   runat="server" style="width: 60px;"/>
                                            </tr>
                                            <tr id="itemPlaceholder" runat="server">
                                            </tr>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr id="trRowData" runat="server" class="listview-row ui-widget">
                                            <td>
                                                <asp:Button ID="btnSelect" runat="server" CommandName="Select" Text="" CssClass="btnSelect invisible" />
                                                <KTCC:KTTextBox ID="txtInspectionDt" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct"></KTCC:KTTextBox>
                                            </td>
                                            <td>
                                                <KTCC:KTTextBox ID="txtPartsNm" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full" />
                                            </td>
                                            <td>
                                                <KTCC:KTTextBox ID="txtProcessYmd" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full" />
                                            </td>
                                            <td>
                                                <KTCC:KTTextBox ID="txtProcessNum" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full" />
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
                                        <table id="itemPlaceholderContainerRB" runat="server" class="grid-layout" style="width:950px">
                                            <tr id="headerRBContent"   runat="server" class="listview-header_3r ui-state-default">
                                                <th id="Th5" runat="server" style="width: 120px;"/>
                                                <th id="Th6" runat="server" style="width:  80px;"/>
                                                <th id="Th7" runat="server" style="width: 150px;"/>
                                                <th id="Th8" runat="server" style="width: 200px;"/>
                                                <th id="Th9" runat="server" style="width: 400px;"/>
                                            </tr>
                                            <tr id="itemPlaceholder" runat="server">
                                            </tr>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr id="trRowData" runat="server" class="listview-row ui-widget">
                                            <td>
                                                <asp:Button ID="btnSelect" runat="server" CommandName="Select" Text="" CssClass="btnSelect invisible" />
                                                <KTCC:KTTextBox ID="txtPartsNum" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full" ></KTCC:KTTextBox>
                                            </td>
                                            <td>
                                                <KTCC:KTTextBox ID="txtOperationKind" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                            </td>
                                            <td>
                                                <KTCC:KTTextBox ID="txtDeliveryDt" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                            </td>
                                            <td>
                                                <KTCC:KTTextBox ID="txtResult" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full" />
                                            </td>
                                            <td>
                                                <KTCC:KTTextBox ID="txtNotes" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full" />
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
                    <table id="itemPlaceholderContainer" runat="server" class="grid-layout" style="width: 1438px; height: auto">
                        <tr id="headerMainContent" runat="server" class="listview-header_2r ui-state-default">
                            <th id="Th1" runat="server" style="width: 150px;">検査日時</th>
                            <th id="Th2" runat="server" style="width: 160px;">部品名</th>
                            <th id="Th3" runat="server" style="width:  90px;">加工日</th>
                            <th id="Th4" runat="server" style="width:  60px;">連番</th>
                            <th id="Th5" runat="server" style="width: 120px;">品番</th>
                            <th id="Th6" runat="server" style="width:  80px;">作業区分</th>
                            <th id="Th7" runat="server" style="width: 150px;">納入日時</th>
                            <th id="Th8" runat="server" style="width: 200px;">結果</th>
                            <th id="Th9" runat="server" style="width: 400px;">備考</th>
                        </tr>
                        <tr id="itemPlaceholder" runat="server">
                        </tr>
                    </table>
                </LayoutTemplate>
                <ItemTemplate>
                    <tr id="trRowData" runat="server" class="listview-row ui-widget">
                        <td>
                            <asp:Button ID="btnSelect" runat="server" CommandName="Select" Text="" CssClass="btnSelect invisible" />
                            <KTCC:KTTextBox ID="txtInspectionDt" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct"></KTCC:KTTextBox>
                        </td>
                        <td>
                            <KTCC:KTTextBox ID="txtPartsNm" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full" />
                        </td>
                        <td>
                            <KTCC:KTTextBox ID="txtProcessYmd" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full" />
                        </td>
                        <td>
                            <KTCC:KTTextBox ID="txtProcessNum" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full" />
                        </td>
                        <td>
                            <KTCC:KTTextBox ID="txtPartsNum" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full" />
                        </td>
                        <td>
                            <KTCC:KTTextBox ID="txtOperationKind" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                        </td>
                        <td>
                            <KTCC:KTTextBox ID="txtDeliveryDt" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                        </td>
                        <td>
                            <KTCC:KTTextBox ID="txtResult" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full" />
                        </td>
                        <td>
                            <KTCC:KTTextBox ID="txtNotes" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full" />
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:ListView>
            --%>
        </div>

        <div class="div-detail-table-title">■検査ファイル</div>
        <div style="clear: both; width:auto;margin-top:10px"></div>
        <div id="divSubListArea" class="div-auto-scroll" style="margin-left:10px">
            <asp:ListView ID="lstSubList" runat="server" OnItemDataBound="lstSubList_ItemDataBound">
                <LayoutTemplate>
                    <table id="itemPlaceholderContainer" runat="server" class="grid-layout" style="width: 721px; height: auto">
                        <tr id="headerMainContent" runat="server" class="listview-header_2r ui-state-default">
                            <th id="Th1" runat="server" style="width: 120px;">ファイルNo</th>
                            <th id="Th2" runat="server" style="width: 600px;">ファイル名</th>
                        </tr>
                        <tr class="" id="itemPlaceholder" runat="server">
                        </tr>
                    </table>
                </LayoutTemplate>
                <ItemTemplate>
                    <tr id="trRowData" runat="server" class="listview-row ui-widget">
                        <td>
                            <KTCC:KTNumericTextBox ID="ntbFileNum" runat="server" CssClass="font-default txt-default txt-numeric txt-width-full" ReadOnly="true" />
                        </td>
                        <td>
                            <KTCC:KTButton ID="btnFileNm" runat="server" ReadOnly="true" CssClass="link-style font-default" OnCommand="btnFileNm_Command" OnClientClick="SubmitControl.SetLoadingType(SubmitControl.TYPE_NONE);" />
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:ListView>
        </div>
    </div>
</div>
