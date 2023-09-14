<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Optaxis.ascx.cs" Inherits="TRC_W_PWT_ProductView.UI.Pages.ProcessView.Optaxis" %>
<%@ Import Namespace="TRC_W_PWT_ProductView.Common" %>

<%-- 画面CSS/スクリプト定義領域 --%>
<asp:PlaceHolder ID="SubScripts" runat="server" >
    <script src="<%: ResolveUrl( "~/Scripts/LibScript/scrollViewer.js") %>" ></script>
    <script src="<%: PageUtils.ResolveUrl( this.Page, "~/Scripts/Pages/ProcessView/Optaxis.js") %>" ></script>
</asp:PlaceHolder>

<div class="div-detail-info-margin">
    <div id="divDetailBodyScroll" class="div-fix-scroll" >
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
                                <table id="solidMainRTHeader" runat="server" class="grid-layout" style="width: 500px;">
                                    <tr id="headerRTMainContent" runat="server" class="listview-header_2r ui-state-default">
                                        <th id="PinCd"           runat="server" style="width: 150px">PINｺｰﾄﾞ</th>
                                        <th id="Result"          runat="server" style="width: 100px">結果</th>
                                        <th id="InspectionGroup" runat="server" style="width: 100px">検査ｸﾞﾙｰﾌﾟ</th>
                                        <th id="OptaxisInsFlag"  runat="server" style="width: 100px">光軸検査<br />ﾌﾗｸﾞ</th>
                                        <th id="OptaxisLiftup"   runat="server" style="width: 100px">光軸検査<br />ﾘﾌﾀｰ上昇値</th>
                                        <th id="OptaxisScreen"   runat="server" style="width: 100px">光軸検査<br />ﾘﾌﾀｰ停止値</th>
                                        <th id="RtOptaxisInfFlagL" runat="server" style="width: 100px">結果光軸<br />検査ﾌﾗｸﾞL</th>
                                        <th id="RtOptaxisInfFlagR" runat="server" style="width: 100px">結果光軸<br />検査ﾌﾗｸﾞR</th>
										<th id="OptAxisStationCd" runat="server" style="width: 100px">光軸検査<br />ステーション</th>
                                        <th id="InspectionDt"    runat="server" style="width: 150px">作成日時</th>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <%-- 左データ行 --%>
                        <div id="divLBMainScroll" class="div-scroll-left-bottom div-left-grid" style="height: 120px">
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
                        <div id="divRBMainScroll" class="div-visible-scroll div-right-grid" style="height: 120px">
                            <div id="divGrvRBMain" runat="server">
                                <asp:ListView ID="lstMainListRB" runat="server" OnItemDataBound="lstMainListRB_ItemDataBound" OnSelectedIndexChanging="lstMainListRB_SelectedIndexChanging" OnSelectedIndexChanged="lstMainListRB_SelectedIndexChanged">
                                    <LayoutTemplate>
                                        <table id="itemPlaceholderContainerRBMain" runat="server" class="grid-layout" style="width:500px">
                                            <tr id="headerRBMainContent" runat="server" class="listview-header_3r ui-state-default">
                                                <th id="PinCd"           runat="server" style="width:150px"/>
                                                <th id="Result"          runat="server" style="width:100px"/>
                                                <th id="InspectionGroup" runat="server" style="width:100px"/>
                                                <th id="OptaxisInsFlag"  runat="server" style="width:100px"/>
                                                <th id="OptaxisLiftup"   runat="server" style="width:100px"/>
                                                <th id="OptaxisScreen"   runat="server" style="width:100px"/>
                                                <th id="RtOptaxisInsFlagL" runat="server" style="width:100px"/>
                                                <th id="RtOptaxisInsFlagR" runat="server" style="width:100px"/>
												<th id="OptAxisStationCd" runat="server" style="width:100px"/>
                                                <th id="InspectionDt"    runat="server" style="width:150px"/>
                                            </tr>
                                            <tr id="itemPlaceholder" runat="server">
                                            </tr>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr id="trRowData" runat="server" class="listview-row ui-widget">
                                            <td>
                                                <asp:Button ID="btnSelect" runat="server" CommandName="Select" Text="" CssClass="btnSelect invisible" />
                                                <KTCC:KTTextBox ID="txtPinCd"  runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                            </td>
                                            <td>
                                                <KTCC:KTTextBox ID="txtResult"  runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                            </td>
                                            <td>
                                                <KTCC:KTTextBox ID="txtInspectionGroup"  runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                            </td>
                                            <td>
                                                <KTCC:KTTextBox ID="txtOptaxisInsFlag"  runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                            </td>
                                            <td>
                                                <KTCC:KTTextBox ID="txtOptaxisLiftup"  runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                            </td>
                                            <td>
                                                <KTCC:KTTextBox ID="txtOptaxisScreen"  runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                            </td>
                                            <td>
                                                <KTCC:KTTextBox ID="txtRtOptaxisInsFlagL"  runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                            </td>
                                            <td>
                                                <KTCC:KTTextBox ID="txtRtOptaxisInsFlagR"  runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                            </td>
											<td>
                                                <KTCC:KTTextBox ID="txtOptAxisStationCd"  runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                            </td>
                                            <td>
                                                <KTCC:KTTextBox ID="txtInspectionDt"  runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
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

        <div id="divDetailViewArea" class="div-fix-scroll-flt">
            <div id="divDetailViewBox" class="div-auto-scroll">
                <div class="div-detail-table-title">■画像(左)</div>
                <asp:Image ID="imgMainArea" runat="server" AlternateText="" />
                <div class="div-detail-table-title">■画像(右)</div>
                <asp:Image ID="imgMainArea2" runat="server" AlternateText="" />
            </div>
        </div>
    </div>
</div>
