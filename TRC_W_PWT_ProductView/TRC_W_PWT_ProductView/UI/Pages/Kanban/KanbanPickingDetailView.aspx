<%@ Page Language="C#" MasterPageFile="~/UI/MasterForm/MasterMain.master" AutoEventWireup="true" CodeBehind="KanbanPickingDetailView.aspx.cs" Inherits="TRC_W_PWT_ProductView.UI.Pages.Kanban.KanbanPickingDetailView" %>

<%@ Register Src="~/UI/Pages/UserControl/InputModal.ascx" TagName="InputModal" TagPrefix="UC" %>
<%@ Import Namespace="TRC_W_PWT_ProductView.Common" %>

<asp:Content ID="MasterScript" ContentPlaceHolderID="MasterScript" runat="server">
    <script src="<%: PageUtils.ResolveUrl( this.Page, "~/Scripts/ControlCommon.js") %>"></script>
    <script src="<%: PageUtils.ResolveUrl( this.Page, "~/Scripts/Pages/Kanban/KanbanPickingDetailView.js") %>"></script>
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



    <div id="divGrvDisplay" runat="server">
        <table class="table-layout-fix">
            <tr>
                <td>
                    <div id="divLTScroll" class="div-fix-scroll div-left-grid">
                        <div id="divGrvHeaderLT" runat="server">
                            <asp:GridView ID="grvDetailHeader" runat="server" CssClass="grid-layout2 ui-widget-content" AllowPaging="false" AllowSorting="true" PageSize="100" AutoGenerateColumns="false" OnSorting="grvDetailView_Sorting">
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
                            <asp:GridView ID="grvDetailBody" runat="server" ShowHeader="false" CssClass="grid-layout2 ui-widget-content grid-layout" AllowPaging="true" AllowSorting="false" PageSize="50" AutoGenerateColumns="false" OnRowDataBound="grvDetailView_RowDataBound">
                                <PagerSettings Position="TopAndBottom" Mode="NumericFirstLast" PageButtonCount="10" Visible="false" />
                                <HeaderStyle CssClass="grid-header ui-state-default" />
                                <RowStyle CssClass="grid-row ui-widget" />
                                <SelectedRowStyle CssClass="ui-state-highlight" />
                                <Columns>
                                    <asp:TemplateField HeaderText="no" />
                                    <asp:TemplateField HeaderText="partsNumber" />
                                    <asp:TemplateField HeaderText="materialName" />
                                    <asp:TemplateField HeaderText="endTime" />
                                    <asp:TemplateField HeaderText="deliveryInstructionDate" />
                                    <asp:TemplateField HeaderText="zaikanPrimaryLocation" />
                                    <asp:TemplateField HeaderText="zaikanSecTerLocation" />
                                    <asp:TemplateField HeaderText="snp" />
                                    <asp:TemplateField HeaderText="pickingBoxCount" />
                                    <asp:TemplateField HeaderText="pickingStatus" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField ID="hdnPickingNo" runat="server" />
</asp:Content>
<asp:Content ID="MasterBodyBottom" ContentPlaceHolderID="MasterBodyBottom" runat="server">
    <div id="divGrvCtrlButton" runat="server" class="div-bottom-button-area">
        <KTCC:KTButton ID="btnExcel" runat="server" Text="Excel出力" CssClass="btn-middle" OnClick="btnExcel_Click" OnClientClick="SubmitControl.SetLoadingType(SubmitControl.TYPE_NONE);" Visible="False" />
        <button id="btnCancel" class="btn-middle" onclick="ControlCommon.WindowClose()">終了</button>
    </div>
</asp:Content>
