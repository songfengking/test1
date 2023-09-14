<%@ Page Language="C#" MasterPageFile="~/UI/MasterForm/MasterMain.master" AutoEventWireup="true" CodeBehind="KanbanShortageView.aspx.cs" Inherits="TRC_W_PWT_ProductView.UI.Pages.Kanban.KanbanShortageView" %>

<%@ Register Src="~/UI/Pages/UserControl/InputModal.ascx" TagName="InputModal" TagPrefix="UC" %>
<%@ Import Namespace="TRC_W_PWT_ProductView.Common" %>

<asp:Content ID="MasterScript" ContentPlaceHolderID="MasterScript" runat="server">
    <script src="<%: PageUtils.ResolveUrl( this.Page, "~/Scripts/Pages/Kanban/KanbanShortageView.js") %>"></script>
</asp:Content>

<asp:Content ID="MasterBody" ContentPlaceHolderID="MasterBody" runat="server">
    <%-- 検索条件 --%>
    <div>
        <table class="table-layout">
            <tr>
                <td>
                    <div class="box-in-margin-small">
                        <asp:UpdatePanel ID="upnCondition" runat="server">
                            <ContentTemplate>
                                <div class="condition-box">
                                    <div class="condition-in-box-main">
                                        <table class="table-border-layout">
                                            <tr class="tr-fix-zero-height">
                                                <td style="width: 250px"></td>
                                                <td style="width: 210px"></td>
                                                <td style="width: 120px"></td>
                                                <td style="width: 120px"></td>
                                                <td style="width: 120px"></td>
                                            </tr>
                                            <tr class="font-default tr-condition-header ui-state-default">
                                                <td>要求日時</td>
                                                <td>エリア</td>
                                                <td>材管ロケ大番地</td>
                                                <td>材管ロケ中番地</td>
                                                <td>材管ロケ小番地</td>
                                            </tr>
                                            <tr class="tr-condition-body">
                                                <td>
                                                    <table class="table-condition-sub">
                                                        <tr class="font-default">
                                                            <td style="width: 50px">From</td>
                                                            <td>
                                                                <KTCC:KTCalendar ID="cldSendDateFrom" runat="server" UseCalendar="true" Type="yyyyMMdd" CssClass="font-default txt-default cld-ymd"></KTCC:KTCalendar>
                                                            </td>
                                                            <td>
                                                                <KTCC:KTTimeTextBox ID="txtSendTimeFrom" runat="server" CssClass="font-default txt-default txt-width-short"></KTCC:KTTimeTextBox>
                                                            </td>
                                                        </tr>
                                                        <tr class="font-default">
                                                            <td style="width: 50px">To</td>
                                                            <td>
                                                                <KTCC:KTCalendar ID="cldSendDateTo" runat="server" UseCalendar="true" Type="yyyyMMdd" CssClass="font-default txt-default cld-ymd"></KTCC:KTCalendar>
                                                            </td>
                                                            <td>
                                                                <KTCC:KTTimeTextBox ID="txtSendTimeTo" runat="server" CssClass="font-default txt-default txt-width-short"></KTCC:KTTimeTextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td>
                                                    <table class="table-condition-sub">
                                                        <tr class="font-default">
                                                            <td>
                                                                <div>
                                                                    <KTCC:KTDropDownList ID="ddlArea" runat="server" CssClass="font-default ddl-default ddl-width-long" AutoPostBack="false" />
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td>
                                                    <table class="table-condition-sub">
                                                        <tr class="font-default">
                                                            <td class="box-in-center">
                                                                <KTCC:KTTextBox ID="txtZaikanPrimaryLocation" runat="server" InputMode="HalfKana" MaxLength="3" CssClass="font-default txt-default ime-inactive txt-width-short" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td>
                                                    <table class="table-condition-sub">
                                                        <tr class="font-default">
                                                            <td class="box-in-center">
                                                                <KTCC:KTTextBox ID="txtZaikanSecondaryLocation" runat="server" InputMode="HalfKana" MaxLength="1" CssClass="font-default txt-default ime-inactive txt-width-short" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td>
                                                    <table class="table-condition-sub">
                                                        <tr class="font-default">
                                                            <td class="box-in-center">
                                                                <KTCC:KTTextBox ID="txtZaikanTertiaryLocation" runat="server" InputMode="HalfKana" MaxLength="2" CssClass="font-default txt-default ime-inactive txt-width-short" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div class="condition-in-box-main">
                                        <table class="table-border-layout">
                                            <tr class="tr-fix-zero-height">
                                                <td style="width: 260px"></td>
                                                <td style="width: 170px"></td>
                                                <td style="width: 170px"></td>
                                            </tr>
                                            <tr class="font-default tr-condition-header ui-state-default">
                                                <td>ピッキング者</td>
                                                <td>品番</td>
                                                <td>ピッキングNo</td>
                                            </tr>
                                            <tr class="tr-condition-body">
                                                <td>
                                                    <table class="table-condition-sub">
                                                        <tr class="font-default">
                                                            <td style="display: inline-block;">
                                                                <KTCC:KTTextBox ID="txtPickingUserNm" runat="server" InputMode="All" AutoUpper="false" Enabled="false" CssClass="font-default txt-default txt-width-long" />
                                                                <asp:HiddenField ID="hdnPickingUserNm" runat="server" />
                                                                <asp:HiddenField ID="hdnPickingUserId" runat="server" />
                                                                <button type="button" id="btnSelectPickingUser" style="vertical-align: middle;" class="btn-icon ui-icon ui-icon-search" onclick="KanbanShotageView.ShowPicikingUserSelect()"></button>
                                                            </td>
                                                            <td style="width: 60px">
                                                                <button type="button" id="btnClearPickingUser" style="vertical-align: middle;" onclick="KanbanShotageView.ClearPickingUserSelect()">クリア</button>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td>
                                                    <table class="table-condition-sub">
                                                        <tr class="font-default">
                                                            <td>
                                                                <KTCC:KTTextBox ID="txtPartsNumber" runat="server" InputMode="AlphaNum" AutoUpper="true" MaxLength="12" CssClass="font-default txt-default ime-inactive txt-width-long" onblur="KanbanShotageView.DeleteHyphen(this);"/>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td>
                                                    <table class="table-condition-sub">
                                                        <tr class="font-default">
                                                            <td>
                                                                <KTCC:KTTextBox ID="txtPickingNo" runat="server" InputMode="IntNum" MaxLength="8" CssClass="font-default txt-default ime-inactive txt-width-long" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <div style="clear: both;"></div>
                        <div class="condition-button-area">
                            <KTCC:KTButton ID="btnSearch" runat="server" Text="検索" CssClass="btn-middle" OnClick="btnSearch_Click" OnClientClick="KanbanShotageView.TimeOnlyInputCheck()"/>
                        </div>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div class="ui-widget-header div-result-title">
                        <span class="result-title">検索結果</span>
                    </div>
                    <div class="div-grid-results">
                        <div class="div-result-pager">
                            <asp:Panel ID="pnlPager" runat="server" EnableViewState="true"></asp:Panel>
                        </div>
                        <div id="divgrvCount" class="div-result-count">
                            <span>件数：</span>
                            <KTCC:KTNumericTextBox ID="ntbResultCount" runat="server" CssClass="txt-center-num" ReadOnly="true" />
                            <span>件</span>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <div style="clear: both; height: 0px;"></div>
    <div id="divGrvDisplay" runat="server">
        <table class="table-layout-fix">
            <tr>
                <td>
                    <div id="divLTScroll" class="div-fix-scroll div-left-grid">
                        <div id="divGrvHeaderLT" runat="server">
                            <asp:GridView ID="grvHeaderLT" runat="server" CssClass="grid-layout2 ui-widget-content" AllowPaging="false" AllowSorting="true" PageSize="50" AutoGenerateColumns="false" OnSorting="grvKanbanShortageView_Sorting">
                                <HeaderStyle CssClass="grid-header ui-state-default" />
                            </asp:GridView>
                        </div>
                    </div>
                </td>
                <td>
                    <div id="divRTScroll" class="div-scroll-right-top div-right-grid">
                        <div id="divGrvHeaderRT" runat="server">
                            <asp:GridView ID="grvHeaderRT" runat="server" CssClass="grid-layout2 ui-widget-content" AllowPaging="false" AllowSorting="true" PageSize="50" AutoGenerateColumns="false" OnSorting="grvKanbanShortageView_Sorting">
                                <HeaderStyle CssClass="grid-header ui-state-default" />
                            </asp:GridView>
                        </div>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div id="divLBScroll" class="div-scroll-left-bottom div-left-grid">
                        <div id="divGrvLB" runat="server">
                            <asp:GridView ID="grvKanbanShortageViewLB" runat="server" ShowHeader="false" CssClass="grid-layout2 ui-widget-content" AllowPaging="true" AllowSorting="false" PageSize="50" AutoGenerateColumns="false" OnRowDataBound="grvKanbanShortageView_RowDataBound">
                                <PagerSettings Position="TopAndBottom" Mode="NumericFirstLast" PageButtonCount="10" Visible="false" />
                                <HeaderStyle CssClass="grid-header ui-state-default" />
                                <RowStyle CssClass="grid-row ui-widget" />
                                <SelectedRowStyle CssClass="ui-state-highlight" />
                                <Columns>
                                    <asp:TemplateField HeaderText="PICKING_LIST_NO" />
                                    <asp:TemplateField HeaderText="SEND_TIME" />
                                    <asp:TemplateField HeaderText="AREA_NM" />
                                    <asp:TemplateField HeaderText="PICKING_USER" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </td>
                <td>
                    <div id="divRBScroll" class="div-visible-scroll div-right-grid">
                        <div id="divGrvRB" runat="server">
                            <asp:GridView ID="grvKanbanShortageViewRB" runat="server" ShowHeader="false" CssClass="grid-layout2 ui-widget-content" AllowPaging="true" AllowSorting="false" PageSize="50" AutoGenerateColumns="false" OnRowDataBound="grvKanbanShortageView_RowDataBound">
                                <PagerSettings Position="TopAndBottom" Mode="NumericFirstLast" PageButtonCount="10" Visible="false" />
                                <HeaderStyle CssClass="grid-header ui-state-default" />
                                <RowStyle CssClass="grid-row ui-widget" />
                                <SelectedRowStyle CssClass="ui-state-highlight" />
                                <Columns>
                                    <asp:TemplateField HeaderText="DISP_ORDER" />
                                    <asp:TemplateField HeaderText="PARTS_NUMBER" />
                                    <asp:TemplateField HeaderText="MATERIAL_NAME" />
                                    <asp:TemplateField HeaderText="PRIMARY_LOCATION" />
                                    <asp:TemplateField HeaderText="SECONDARY_TERTIARY_LOCATION" />
                                    <asp:TemplateField HeaderText="SNP" />
                                    <asp:TemplateField HeaderText="PICKING_BOX_COUNT" />
                                    <asp:TemplateField HeaderText="MIN_DELIVERY_INST_DT" />
                                    <asp:TemplateField HeaderText="UNPAID_QTY" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>

<asp:Content ID="MasterBodyBottom" ContentPlaceHolderID="MasterBodyBottom" runat="server">
    <div id="divGrvCtrlButton" runat="server" class="div-bottom-button-area">
        <KTCC:KTButton ID="btnExcel" runat="server" Text="Excel出力" CssClass="btn-middle" Visible="false" OnClientClick="SubmitControl.SetLoadingType(SubmitControl.TYPE_NONE);" OnClick="btnExcel_Click" />
        <button id="btnCancel" class="btn-middle" onclick="ControlCommon.WindowClose()">終了</button>
    </div>
</asp:Content>
