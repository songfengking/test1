<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Harnes.ascx.cs" Inherits="TRC_W_PWT_ProductView.UI.Pages.ProcessView.Harnes" %>
<%@ Import Namespace="TRC_W_PWT_ProductView.Common" %>

<%-- デザイン表示時使用 マスターページ使用時不要 --%>
<%--<link href="../../../CSS/cobalt/jquery-wijmo.css" rel="stylesheet" />
<link href="../../../CSS/Base.css" rel="stylesheet" />
<link href="../../../CSS/TRC.css" rel="stylesheet" />--%>

<%-- 画面CSS/スクリプト定義領域 --%>
<asp:PlaceHolder ID="SubScripts" runat="server">
    <script src="<%: PageUtils.ResolveUrl( this.Page, "~/Scripts/Pages/ContentSolidScroll.js") %>" ></script>
</asp:PlaceHolder>

<div class="div-detail-info-margin">
    <div>
        <div class="div-detail-table-title">■来歴情報</div>
        <div style="clear: both; width:auto;margin-top:10px"></div>
            <div id="divGrvDisplay" runat="server">
                <table class="table-layout-fix" style="margin-left:10px">
                    <tr>
                        <td>
                            <div id="divLTScroll" class="div-fix-scroll div-left-grid">
                                <div id="divGrvHeaderLT" runat="server">
                                    <asp:GridView ID="grvHeaderLT" runat="server" CssClass="grid-layout2 ui-widget-content" AllowPaging="false" AllowSorting="true" PageSize="100" AutoGenerateColumns="false">
                                        <HeaderStyle CssClass="grid-header ui-state-default" />
                                    </asp:GridView>
                                </div>
                            </div>
                        </td>
                        <td>
                            <div id="divRTScroll" class="div-scroll-right-top div-right-grid">
                                <div id="divGrvHeaderRT" runat="server">
                                    <asp:GridView ID="grvHeaderRT" runat="server" CssClass="grid-layout2 ui-widget-content" AllowPaging="false" AllowSorting="true" PageSize="100" AutoGenerateColumns="false">
                                        <HeaderStyle CssClass="grid-header-newline ui-state-default"/>
                                    </asp:GridView>
                                </div>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div id="divLBScroll" class="div-scroll-left-bottom div-left-grid">
                                <div id="divGrvLB" runat="server">
                                    <asp:GridView ID="grvMainViewLB" runat="server" ShowHeader="false" CssClass="grid-layout2 ui-widget-content" AllowPaging="true" AllowSorting="false" PageSize="100" AutoGenerateColumns="false" OnRowDataBound="grvMainViewLB_RowDataBound">
                                        <HeaderStyle CssClass="grid-header ui-state-default" />
                                        <RowStyle CssClass="grid-row ui-widget" />
                                        <SelectedRowStyle CssClass="ui-state-highlight" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="inspectionDt" />
                                            <asp:TemplateField HeaderText="txtResult" />
                                            <asp:TemplateField HeaderText="historyIndex" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </td>
                        <td>
                            <div id="divRBScroll" class="div-visible-scroll div-right-grid">
                                <div id="divGrvRB" runat="server">
                                    <asp:GridView ID="grvMainViewRB" runat="server" ShowHeader="false" CssClass="grid-layout2 ui-widget-content" AllowPaging="true" AllowSorting="false" PageSize="100" AutoGenerateColumns="false" OnRowDataBound="grvMainViewRB_RowDataBound">
                                        <HeaderStyle CssClass="grid-header ui-state-default" />
                                        <RowStyle CssClass="grid-row ui-widget" />
                                        <SelectedRowStyle CssClass="ui-state-highlight" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="resultCrs" />
                                            <asp:TemplateField HeaderText="resultInj" />
                                            <asp:TemplateField HeaderText="resultDpf" />
                                            <asp:TemplateField HeaderText="resultG" />
                                            <asp:TemplateField HeaderText="resultReil" />
                                            <asp:TemplateField HeaderText="errorCd1" />
                                            <asp:TemplateField HeaderText="errorCd2" />
                                            <asp:TemplateField HeaderText="errorCd3" />
                                            <asp:TemplateField HeaderText="errorCd4" />
                                            <asp:TemplateField HeaderText="errorCd5" />
                                            <asp:TemplateField HeaderText="errorCd6" />
                                            <asp:TemplateField HeaderText="errorCd7" />
                                            <asp:TemplateField HeaderText="errorCd8" />
                                            <asp:TemplateField HeaderText="errorCd9" />
                                            <asp:TemplateField HeaderText="errorCd10" />
                                            <asp:TemplateField HeaderText="errorCd11" />
                                            <asp:TemplateField HeaderText="errorCd12" />
                                            <asp:TemplateField HeaderText="errorCd13" />
                                            <asp:TemplateField HeaderText="errorCd14" />
                                            <asp:TemplateField HeaderText="errorCd15" />
                                            <asp:TemplateField HeaderText="errorCd16" />
                                            <asp:TemplateField HeaderText="errorCd17" />
                                            <asp:TemplateField HeaderText="errorCd18" />
                                            <asp:TemplateField HeaderText="errorCd19" />
                                            <asp:TemplateField HeaderText="errorCd20" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        <%-- 
        <div id="divMainListArea" class="div-auto-scroll" >
            <asp:ListView ID="lstMainList" runat="server" OnItemDataBound="lstMainList_ItemDataBound">
                <LayoutTemplate>
                    <table id="itemPlaceholderContainer" runat="server" class="grid-layout" style="width: 2250px; height:auto">
                        <tr id="headerMainContent" runat="server" class="listview-header_2r ui-state-default">
                            <th id="Th01" style="width: 150px" runat="server">検査日時</th>
                            <th id="Th02" style="width:  70px" runat="server">判定</th>
                            <th id="Th03" style="width:  80px" runat="server">試験連番</th>
                            <th id="Th04" style="width: 110px" runat="server">コモンレール</th>
                            <th id="Th05" style="width: 110px" runat="server">インジェクタ</th>
                            <th id="Th06" style="width: 110px" runat="server">DPF</th>
                            <th id="Th07" style="width: 110px" runat="server">Gセンタ</th>
                            <th id="Th08" style="width: 110px" runat="server">レール圧</th>
                            <th id="ThE01" style="width: 70px" runat="server">エラー<br />コード1</th>
                            <th id="ThE02" style="width: 70px" runat="server">エラー<br />コード2</th>
                            <th id="ThE03" style="width: 70px" runat="server">エラー<br />コード3</th>
                            <th id="ThE04" style="width: 70px" runat="server">エラー<br />コード4</th>
                            <th id="ThE05" style="width: 70px" runat="server">エラー<br />コード5</th>
                            <th id="ThE06" style="width: 70px" runat="server">エラー<br />コード6</th>
                            <th id="ThE07" style="width: 70px" runat="server">エラー<br />コード7</th>
                            <th id="ThE08" style="width: 70px" runat="server">エラー<br />コード8</th>
                            <th id="ThE09" style="width: 70px" runat="server">エラー<br />コード9</th>
                            <th id="ThE10" style="width: 70px" runat="server">エラー<br />コード10</th>
                            <th id="ThE11" style="width: 70px" runat="server">エラー<br />コード11</th>
                            <th id="ThE12" style="width: 70px" runat="server">エラー<br />コード12</th>
                            <th id="ThE13" style="width: 70px" runat="server">エラー<br />コード13</th>
                            <th id="ThE14" style="width: 70px" runat="server">エラー<br />コード14</th>
                            <th id="ThE15" style="width: 70px" runat="server">エラー<br />コード15</th>
                            <th id="ThE16" style="width: 70px" runat="server">エラー<br />コード16</th>
                            <th id="ThE17" style="width: 70px" runat="server">エラー<br />コード17</th>
                            <th id="ThE18" style="width: 70px" runat="server">エラー<br />コード18</th>
                            <th id="ThE19" style="width: 70px" runat="server">エラー<br />コード19</th>
                            <th id="ThE20" style="width: 70px" runat="server">エラー<br />コード20</th>
                        </tr>
                        <tr class="" id="itemPlaceholder" runat="server">
                        </tr>
                    </table>
                </LayoutTemplate>
                <ItemTemplate>
                    <tr id="trRowData" runat="server" class="listview-row ui-widget">
                        <td>
                            <KTCC:KTTextBox ID="txtInspectionDt" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                        </td>
                        <td>
                            <KTCC:KTTextBox ID="txtResult" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                        </td>
                        <td>
                            <KTCC:KTNumericTextBox ID="ntbHistoryIndex" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-numeric txt-width-full" />
                        </td>
                        <td>
                            <KTCC:KTTextBox ID="txtResultCrs" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                        </td>
                        <td>
                            <KTCC:KTTextBox ID="txtResultInj" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                        </td>
                        <td>
                            <KTCC:KTTextBox ID="txtResultDpf" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                        </td>
                        <td>
                            <KTCC:KTTextBox ID="txtResultG" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                        </td>
                        <td>
                            <KTCC:KTTextBox ID="txtResultReil" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                        </td>
                        <td>
                            <KTCC:KTTextBox ID="txtErrorCd1" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                        </td>
                        <td>
                            <KTCC:KTTextBox ID="txtErrorCd2" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                        </td>
                        <td>
                            <KTCC:KTTextBox ID="txtErrorCd3" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                        </td>
                        <td>
                            <KTCC:KTTextBox ID="txtErrorCd4" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                        </td>
                        <td>
                            <KTCC:KTTextBox ID="txtErrorCd5" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                        </td>
                        <td>
                            <KTCC:KTTextBox ID="txtErrorCd6" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                        </td>
                        <td>
                            <KTCC:KTTextBox ID="txtErrorCd7" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                        </td>
                        <td>
                            <KTCC:KTTextBox ID="txtErrorCd8" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                        </td>
                        <td>
                            <KTCC:KTTextBox ID="txtErrorCd9" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                        </td>
                        <td>
                            <KTCC:KTTextBox ID="txtErrorCd10" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                        </td>
                        <td>
                            <KTCC:KTTextBox ID="txtErrorCd11" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                        </td>
                        <td>
                            <KTCC:KTTextBox ID="txtErrorCd12" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                        </td>
                        <td>
                            <KTCC:KTTextBox ID="txtErrorCd13" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                        </td>
                        <td>
                            <KTCC:KTTextBox ID="txtErrorCd14" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                        </td>
                        <td>
                            <KTCC:KTTextBox ID="txtErrorCd15" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                        </td>
                        <td>
                            <KTCC:KTTextBox ID="txtErrorCd16" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                        </td>
                        <td>
                            <KTCC:KTTextBox ID="txtErrorCd17" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                        </td>
                        <td>
                            <KTCC:KTTextBox ID="txtErrorCd18" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                        </td>
                        <td>
                            <KTCC:KTTextBox ID="txtErrorCd19" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                        </td>
                        <td>
                            <KTCC:KTTextBox ID="txtErrorCd20" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:ListView>
        </div>
        --%>
    </div>
</div>