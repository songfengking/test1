<%@ Page Language="C#" MasterPageFile="~/UI/MasterForm/MasterSub.master" AutoEventWireup="true" CodeBehind="KanbanPickingUserSelect.aspx.cs" Inherits="TRC_W_PWT_ProductView.UI.Pages.Kanban.KanbanPickingUserSelect" %>

<%@ Import Namespace="TRC_W_PWT_ProductView.Common" %>
<asp:Content ID="MasterScript" runat="server" ContentPlaceHolderID="MasterScript">
    <script src="<%: PageUtils.ResolveUrl( this.Page, "~/Scripts/ControlCommon.js") %>"></script>
    <script src="<%: PageUtils.ResolveUrl( this.Page, "~/Scripts/Pages/Kanban/KanbanPickingUserSelect.js") %>"></script>
</asp:Content>
<asp:Content ID="MasterBodyTop" ContentPlaceHolderID="MasterBodyTop" runat="server">
    <%-- 画面タイトル --%>
    <div class="ui-widget-header header-title-area">
        <asp:Label ID="lblTitle" runat="server" Text="ピッキング者選択"></asp:Label>
    </div>
</asp:Content>
<asp:Content ID="MasterBody" ContentPlaceHolderID="MasterBody" runat="server">
    <div>
        <table class="table-layout">
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
                            <asp:GridView ID="grvPickingUserHeader" runat="server" CssClass="grid-layout2 ui-widget-content" AllowPaging="false" AllowSorting="true" PageSize="50" AutoGenerateColumns="false" OnSorting="grvPickingUserHeader_Sorting">
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
                            <asp:GridView ID="grvPickingUserBody" runat="server" ShowHeader="false" CssClass="grid-layout grid-layout2 ui-widget-content" AllowPaging="true" AllowSorting="false" PageSize="50" AutoGenerateColumns="false" OnRowDataBound="grvPickingUserBody_RowDataBound">
                                <PagerSettings Position="TopAndBottom" Mode="NumericFirstLast" PageButtonCount="10" Visible="false" />
                                <HeaderStyle CssClass="grid-header ui-state-default" />
                                <RowStyle CssClass="grid-row ui-widget" />
                                <SelectedRowStyle CssClass="ui-state-highlight" />
                                <Columns>
                                    <asp:TemplateField HeaderText="USER_ID" />
                                    <asp:TemplateField HeaderText="USER_NM" />
                                </Columns>
                            </asp:GridView>
                            <asp:HiddenField ID="hdnSelectedIndex" runat="server" Value="-1" />
                            <asp:HiddenField ID="hdnPickingUserNm" runat="server" />
                            <asp:HiddenField ID="hdnPickingUserId" runat="server" />
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
<asp:Content ID="MasterBodyBottom" ContentPlaceHolderID="MasterBodyBottom" runat="server">
    <div class="div-bottom-button-area">
        <KTCC:KTButton ID="btnSelect" runat="server" Text="選択" CssClass="btn-middle" OnClick="btnSelect_Click" OnClientClick="KanbanPickingUserSelect.SelectUser()" />
        <button id="btnCancel" class="btn-middle" onclick="ControlCommon.WindowClose()">キャンセル</button>
    </div>
</asp:Content>
