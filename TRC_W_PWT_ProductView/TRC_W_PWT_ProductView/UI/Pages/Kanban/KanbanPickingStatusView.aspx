<%@ Page Language="C#" MasterPageFile="~/UI/MasterForm/MasterMain.master" AutoEventWireup="true" CodeBehind="KanbanPickingStatusView.aspx.cs" Inherits="TRC_W_PWT_ProductView.UI.Pages.Kanban.KanbanPickingStatusView" %>

<%@ Register Src="~/UI/Pages/UserControl/InputModal.ascx" TagName="InputModal" TagPrefix="UC" %>
<%@ Import Namespace="TRC_W_PWT_ProductView.Common" %>

<asp:Content ID="MasterScript" ContentPlaceHolderID="MasterScript" runat="server">
    <script src="<%: PageUtils.ResolveUrl( this.Page, "~/Scripts/ControlCommon.js") %>"></script>
    <script src="<%: PageUtils.ResolveUrl( this.Page, "~/Scripts/Pages/Kanban/KanbanPickingStatusView.js") %>"></script>
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
                                                <td style="width: 250px"></td>
                                                <td style="width: 170px"></td>
                                            </tr>
                                            <tr class="font-default tr-condition-header ui-state-default">
                                                <td>要求日時</td>
                                                <td>完了日時</td>
                                                <td>状況</td>
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
                                                                <KTCC:KTTimeTextBox ID="ttbSendTimeFrom" runat="Server" MaxLength="8" Width="72px" />
                                                            </td>
                                                        </tr>
                                                        <tr class="font-default">
                                                            <td style="width: 50px">To</td>
                                                            <td>
                                                                <KTCC:KTCalendar ID="cldSendDateTo" runat="server" UseCalendar="true" Type="yyyyMMdd" CssClass="font-default txt-default cld-ymd"></KTCC:KTCalendar>
                                                            </td>
                                                            <td>
                                                                <KTCC:KTTimeTextBox ID="ttbSendTimeTo" runat="Server" MaxLength="8" Width="72px" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td>
                                                    <table class="table-condition-sub">
                                                        <tr class="font-default">
                                                            <td style="width: 50px">From</td>
                                                            <td>
                                                                <KTCC:KTCalendar ID="cldEndDateFrom" runat="server" UseCalendar="true" Type="yyyyMMdd" CssClass="font-default txt-default cld-ymd"></KTCC:KTCalendar>
                                                            </td>
                                                            <td>
                                                                <KTCC:KTTimeTextBox ID="ttbEndTimeFrom" runat="Server" MaxLength="8" Width="72px" />
                                                            </td>
                                                        </tr>
                                                        <tr class="font-default">
                                                            <td style="width: 50px">To</td>
                                                            <td>
                                                                <KTCC:KTCalendar ID="cldEndDateTo" runat="server" UseCalendar="true" Type="yyyyMMdd" CssClass="font-default txt-default cld-ymd"></KTCC:KTCalendar>
                                                            </td>
                                                            <td>
                                                                <KTCC:KTTimeTextBox ID="ttbEndTimeTo" runat="Server" MaxLength="8" Width="72px" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td>
                                                    <table class="table-condition-sub">
                                                        <tr class="font-default">
                                                            <td>
                                                                <div>
                                                                    <KTCC:KTDropDownList ID="ddlStatus" runat="server" CssClass="font-default ddl-default ddl-width-middle" AutoPostBack="false" />
                                                                </div>
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
                                                <td style="width: 210px"></td>
                                                <td style="width: 260px"></td>
                                                <td style="width: 170px"></td>
                                                <td style="width: 170px"></td>
                                            </tr>
                                            <tr class="font-default tr-condition-header ui-state-default">
                                                <td>エリア</td>
                                                <td>ピッキング者</td>
                                                <td>ピッキングNo</td>
                                                <td>品番</td>
                                            </tr>
                                            <tr class="tr-condition-body">
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
                                                            <td style="display: inline-block;">
                                                                <KTCC:KTTextBox ID="txtPickingUserNm" runat="server" InputMode="All" AutoUpper="false" Enabled="false" CssClass="font-default txt-default txt-width-long" />
                                                                <asp:HiddenField ID="hdnPickingUserNm" runat="server" />
                                                                <asp:HiddenField ID="hdnPickingUserId" runat="server" />
                                                                <button type="button" id="btnSelectPickingUser" style="vertical-align: middle;" class="btn-icon ui-icon ui-icon-search" onclick="KanbanPickingStatusView.ShowPicikingUserSelect()"></button>
                                                            </td>
                                                            <td style="width:60px">
                                                                <button type="button" id="btnClearPickingUser" style="vertical-align: middle;" onclick="KanbanPickingStatusView.ClearPickingUserSelect()">クリア</button>
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
                                                <td>
                                                    <table class="table-condition-sub">
                                                        <tr class="font-default">
                                                            <td>
                                                                <KTCC:KTTextBox ID="txtPartsNumber" runat="server" InputMode="AlphaNum" AutoUpper="true" MaxLength="12" CssClass="font-default txt-default ime-inactive txt-width-long" onblur="KanbanPickingStatusView.DeleteHyphen(this);" />
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
                            <KTCC:KTButton ID="btnSearch" runat="server" Text="検索" CssClass="btn-middle" OnClick="btnSearch_Click" OnClientClick="KanbanPickingStatusView.TimeOnlyInputCheck()" />
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
    <%-- 検索結果 --%>
    <div id="divGrvDisplay" runat="server">
        <table class="table-layout-fix">
            <tr>
                <td>
                    <div id="divLTScroll" class="div-fix-scroll div-left-grid">
                        <div id="divGrvHeaderLT" runat="server">
                            <asp:GridView ID="grvPickingHeader" runat="server" CssClass="grid-layout2 ui-widget-content" AllowPaging="false" AllowSorting="true" PageSize="100" AutoGenerateColumns="false" OnSorting="grvPickingView_Sorting">
                                <HeaderStyle CssClass="grid-header ui-state-default" />
                            </asp:GridView>
                        </div>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div id="divLBScroll" class="overflow:auto div-left-grid">
                        <div id="divGrvLB" runat="server">
                            <asp:GridView ID="grvPickingBody" runat="server" ShowHeader="false" CssClass="grid-layout2 ui-widget-content grid-layout" AllowPaging="true" AllowSorting="false" PageSize="50" AutoGenerateColumns="false" OnRowDataBound="grvPickingView_RowDataBound">
                                <PagerSettings Position="TopAndBottom" Mode="NumericFirstLast" PageButtonCount="10" Visible="false" />
                                <HeaderStyle CssClass="grid-header ui-state-default" />
                                <RowStyle CssClass="grid-row ui-widget" />
                                <SelectedRowStyle CssClass="ui-state-highlight" />
                                <Columns>
                                    <asp:TemplateField HeaderText="pickingNo" />
                                    <asp:TemplateField HeaderText="sendDate" />
                                    <asp:TemplateField HeaderText="endDate" />
                                    <asp:TemplateField HeaderText="deliveryInstructionDate" />
                                    <asp:TemplateField HeaderText="area" />
                                    <asp:TemplateField HeaderText="status" />
                                    <asp:TemplateField HeaderText="pickingUser" />
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
        <KTCC:KTButton ID="btnExcel" runat="server" Text="Excel出力" CssClass="btn-middle" OnClick="btnExcel_Click" OnClientClick="SubmitControl.SetLoadingType(SubmitControl.TYPE_NONE);" Visible="False" />
        <button id="btnCancel" class="btn-middle" onclick="ControlCommon.WindowClose()">終了</button>
    </div>
</asp:Content>
