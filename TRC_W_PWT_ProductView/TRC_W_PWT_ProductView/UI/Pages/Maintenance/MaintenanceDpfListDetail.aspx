<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/UI/MasterForm/MasterMain.master" CodeBehind="MaintenanceDpfListDetail.aspx.cs" Inherits="TRC_W_PWT_ProductView.UI.Pages.MaintenanceDpfListDetail" %>
<%@ Import Namespace="TRC_W_PWT_ProductView.Common" %>
<%@ Register src="~/UI/Pages/UserControl/InputModal.ascx" tagname="InputModal" tagprefix="UC" %>

<asp:Content ID="MasterScript" runat="server" ContentPlaceHolderID="MasterScript" >
    <script src="<%: PageUtils.ResolveUrl( this.Page, "~/Scripts/Pages/Maintenance/MasterMainteDpfListDetail.js") %>"></script>
</asp:Content>

<asp:Content ID="MasterBody" ContentPlaceHolderID="MasterBody" runat="server">
    <div id="header">
        <table class="table-layout">
            <tr>
                <td>
                    <div class="div-detail-info-margin">
                        <table class="table-border-layout" style="width: 730px">
                            <colgroup>
                                <col style="width:  70px" />
                                <col style="width: 110px" />
                                <col style="width: 100px" />
                                <col style="width: 150px" />
                                <col style="width:  70px" />
                                <col style="width:  70px" />
                                <col style="width:  70px" />
                                <col style="width:  90px" />
                                <col style="width:   0px" visible="false"/>
	                        </colgroup>
                            <tr class="font-default tr-category-header">
                                <td colspan="8">
                                    <asp:Label ID="lblProductInfo" runat="server" Text="製品情報"></asp:Label>
                                </td>
                            </tr>
                            <tr class="font-default tr-content-header">
                                <td>ライン</td>
                                <td>ステーション</td>
                                <td colspan="2">生産型式</td>
                                <td>生産国</td>
                                <td>機番</td>
                                <td>IDNO</td>
                                <td>完成日</td>
                            </tr>
                            <tr class="tr-ctrl-height">
                                <td>
                                    <KTCC:KTTextBox ID="txtLineCd" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct"/>
                                </td>
                                <td>
                                    <KTCC:KTTextBox ID="txtST" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct"/>
                                </td>
                                <td>
                                    <KTCC:KTTextBox ID="txtModelCd" runat="server" MaxLength="11" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                </td>
                                <td>
                                    <KTCC:KTTextBox ID="txtModelNm" runat="server" MaxLength="20" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                </td>
                                <td>
                                    <KTCC:KTTextBox ID="txtCountry" runat="server" MaxLength="6" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                </td>
                                <td>
                                    <KTCC:KTTextBox ID="txtSerial" runat="server" MaxLength="7" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                </td>
                                <td>
                                    <KTCC:KTTextBox ID="txtIDNO" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct"/>
                                </td>
                                <td>
                                    <KTCC:KTTextBox ID="txtFinDt" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct"/>
                                </td>
                                <td>
                                    <KTCC:KTTextBox ID="txtPtnCd" runat="server" ReadOnly="true" visible="false"/>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="divEngineInfo" runat="server" class="div-detail-info-margin">
                        <table class="table-border-layout" style="width: 340px">
                            <colgroup>
                                <col style="width: 100px" />
                                <col style="width: 150px" />
                                <col style="width:  70px" />
                                <col style="width:  70px" />
		                    </colgroup>
                            <tr class="font-default tr-category-header">
                                <td colspan="4">
                                    <asp:Label runat="server" ID="lblEngineTransfer" Text="搭載エンジン情報"/>
                                </td>
                            </tr>
                            <tr class="font-default tr-content-header">
                                <td colspan="2">生産型式</td>
                                <td>生産国</td>
                                <td>機番</td>
                            </tr>
                            <tr class="tr-ctrl-height">
                                <td>
                                    <KTCC:KTTextBox ID="txtEngModelCd" runat="server" MaxLength="11" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                </td>
                                <td>
                                    <KTCC:KTTextBox ID="txtEngModelNm" runat="server" MaxLength="20" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                </td>
                                <td>
                                    <KTCC:KTTextBox ID="txtEngCountry" runat="server" MaxLength="6" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                </td>
                                <td>
                                    <KTCC:KTTextBox ID="txtEngSerial" runat="server" MaxLength="7" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                </td>
                            </tr>
                        </table>
                    </div>

                    <div id="divToractorInfo" runat="server" class="div-detail-info-margin">
                        <table class="table-border-layout" style="width: 340px">
                            <colgroup>
                                <col style="width: 100px" />
                                <col style="width: 150px" />
                                <col style="width:  70px" />
                                <col style="width:  70px" />
		                    </colgroup>
                            <tr class="font-default tr-category-header">
                                <td colspan="4">
                                    <asp:Label runat="server" ID="Label1" Text="本機情報"/>
                                </td>
                            </tr>
                            <tr class="font-default tr-content-header">
                                <td colspan="2">生産型式</td>
                                <td>生産国</td>
                                <td>機番</td>
                            </tr>
                            <tr class="tr-ctrl-height">
                                <td>
                                    <KTCC:KTTextBox ID="txtTrcModelCd" runat="server" MaxLength="11" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                </td>
                                <td>
                                    <KTCC:KTTextBox ID="txtTrcModelNm" runat="server" MaxLength="20" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                </td>
                                <td>
                                    <KTCC:KTTextBox ID="txtTrcCountry" runat="server" MaxLength="3" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                </td>
                                <td>
                                    <KTCC:KTTextBox ID="txtTrcSerial" runat="server" MaxLength="7" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </table>
    </div>

    <%-- 非表示項目 --%>
    <div style="display:none">
    </div>

    <div style="clear: both; height: 10px;width:auto"></div>
    <div class="div-content-title-r">
        <asp:Label ID="lblDetailTitle" runat="server" Text="一覧" class="content-title"></asp:Label>
    </div>
    <div class="div-result-pager">
        <asp:Panel ID="pnlPager" runat="server" EnableViewState="true"></asp:Panel>
    </div>
    <div id="divgrvCount" class="div-result-count">
        <span>件数：</span>
        <KTCC:KTNumericTextBox ID="ntbResultCount" runat="server" CssClass="txt-center-num" ReadOnly="true" />
        <span>件</span>
    </div>

    <div style="clear: both; height: 0px;"></div>

    <%-- 検索結果 --%>
    <div id="divGrvDisplay" runat="server">
        <table class="table-layout-fix">
            <tr>
                <td>
                    <div id="divLTScroll" class="div-fix-scroll div-left-grid">
                        <div id="divGrvHeaderLT" runat="server">
                            <asp:GridView ID="grvHeaderLT" runat="server" CssClass="grid-layout2 ui-widget-content" AllowPaging="false" AllowSorting="true" PageSize="100" AutoGenerateColumns="false" OnSorting="grvMainView_Sorting">
                                <HeaderStyle CssClass="grid-header ui-state-default" />
                            </asp:GridView>
                        </div>
                    </div>
                </td>
                <td>
                    <div id="divRTScroll" class="div-scroll-right-top div-right-grid">
                        <div id="divGrvHeaderRT" runat="server">
                            <asp:GridView ID="grvHeaderRT" runat="server" CssClass="grid-layout2 ui-widget-content" AllowPaging="false" AllowSorting="true" PageSize="100" AutoGenerateColumns="false" OnSorting="grvMainView_Sorting">
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
                            <asp:GridView ID="grvMainViewLB" runat="server" ShowHeader="false" CssClass="grid-layout2 ui-widget-content" AllowPaging="true" AllowSorting="false" PageSize="100" AutoGenerateColumns="false" OnRowDataBound="grvMainViewLB_RowDataBound">
                                <PagerSettings Position="TopAndBottom" Mode="NumericFirstLast" PageButtonCount="10" Visible="false" />
                                <HeaderStyle CssClass="grid-header ui-state-default" />
                                <RowStyle CssClass="grid-row ui-widget" />
                                <SelectedRowStyle CssClass="ui-state-highlight" />
                                <Columns>
                                    <asp:TemplateField HeaderText="ATTACHMENT_DT" />
                                    <asp:TemplateField HeaderText="PARTS_KBN" />
                                    <asp:TemplateField HeaderText="SAMPLE_CHECK" />
                                    <asp:TemplateField HeaderText="DPF_MODEL_CD" />
                                    <asp:TemplateField HeaderText="DPF_SERIAL" />
                                    <asp:TemplateField HeaderText="LINE_CD" />
                                    <asp:TemplateField HeaderText="ST" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </td>
                <td>
                    <div id="divRBScroll" class="div-visible-scroll div-right-grid">
                        <div id="divGrvRB" runat="server">
                            <asp:GridView ID="grvMainViewRB" runat="server" ShowHeader="false" CssClass="grid-layout2 ui-widget-content" AllowPaging="true" AllowSorting="false" PageSize="100" AutoGenerateColumns="false" OnRowDataBound="grvMainViewRB_RowDataBound">
                                <PagerSettings Position="TopAndBottom" Mode="NumericFirstLast" PageButtonCount="10" Visible="false" />
                                <HeaderStyle CssClass="grid-header ui-state-default" />
                                <RowStyle CssClass="grid-row ui-widget" />
                                <SelectedRowStyle CssClass="ui-state-highlight" />
                                <Columns>
                                    <asp:TemplateField HeaderText="IDNO" />
                                    <asp:TemplateField HeaderText="MODEL_CD" />
                                    <asp:TemplateField HeaderText="MODEL_NM" />
                                    <asp:TemplateField HeaderText="COUNTRY" />
                                    <asp:TemplateField HeaderText="SERIAL_NO" />
                                    <asp:TemplateField HeaderText="REMARKS" />
                                    <asp:TemplateField HeaderText="UPDATE_DT" />
                                    <asp:TemplateField HeaderText="UPDATE_BY" />                                     
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>

</asp:Content>

<asp:Content ID="MasterBodyBottom" ContentPlaceHolderID="MasterBodyBottom" runat="server" >
    <div>
        <div id="divGrvCtrlButton" runat="server" class="div-bottom-button-area">
            <KTCC:KTButton ID="btnInsert"   runat="server" Text="追加" CssClass="btn-middle"/>
            <KTCC:KTButton ID="btnUpdate"   runat="server" Text="更新" CssClass="btn-middle"/>
            <div style="display:none">
                <KTCC:KTButton ID="btnSearch" runat="server" OnClick="btnSearch_Click"/>
            </div>
        </div>
        <%-- iframeの呼び出し --%>
        <div id="dialog" style="background-color:white">
            <uc:InputModal id="InputModal1" runat="server"/>
        </div>

    </div>
</asp:Content>

