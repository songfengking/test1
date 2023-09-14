<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/UI/MasterForm/MasterMain.master" CodeBehind="MainProcessView.aspx.cs" Inherits="TRC_W_PWT_ProductView.UI.Pages.MainProcessView" %>

<%@ Import Namespace="TRC_W_PWT_ProductView.Common" %>
<%@ Register src="~/UI/Pages/UserControl/MainProcessConditionPartialView.ascx" tagname="ConditionPartialView" tagprefix="UC" %>
<%@ Register src="~/UI/Pages/UserControl/MainProcessOperationPartialView.ascx" tagname="OperationPartialView" tagprefix="UC" %>

<asp:Content ID="MasterScript" runat="server" ContentPlaceHolderID="MasterScript">
    <script src="<%: PageUtils.ResolveUrl( this.Page, "~/Scripts/Pages/MainProcessView.js") %>"></script>
</asp:Content>

<asp:Content ID="MasterBody" ContentPlaceHolderID="MasterBody" runat="server">
    <div>
        <table class="table-layout">
            <tr>
                <td style="padding-top: 5px;">
                    <KTCC:KTButton ID="ChangeProductSearch" Text="製品検索" runat="server" OnClick="ChangeProductSearch_Click" Style="margin-left: 3px; padding-top: 3px; padding-bottom: 3px; background-color: #E0ECF8; border: 1px solid black;" />
                    <KTCC:KTButton ID="ChangePartsSearch" Text="部品検索" runat="server" OnClick="ChangePartsSearch_Click" Style="margin-left: -7px; background-color: #E0ECF8; border: 1px solid black; padding-top: 3px; padding-bottom: 3px;" />
                    <KTCC:KTButton ID="ChangeProcessSearch" Text="工程検索" runat="server" Style="margin-left: -7px; background-color: lightskyblue; border: 1px solid black; padding-top: 3px; padding-bottom: 3px; pointer-events: none;" />
                    <div class="box-in-margin-small">
                        <asp:UpdatePanel ID="upnCondition" runat="server">
                            <ContentTemplate>
                                <div class="condition-box">
                                    <div class="condition-in-box-main">
                                        <table class="table-border-layout" style="width: 880px">
                                            <tr class="tr-fix-zero-height">
                                                <td style="width: 100px"></td>
                                                <td style="width: 260px"></td>
                                                <td style="width: 260px"></td>
                                                <td style="width: 220px"></td>
                                            </tr>
                                            <tr class="font-default tr-condition-header ui-state-default">
                                                <td>製品種別</td>
                                                <td>工程/エンジン種別</td>
                                                <td>生産型式</td>
                                                <td>機番/IDNO/PIN</td>
                                            </tr>
                                            <tr class="tr-condition-body">
                                                <td>
                                                    <KTCC:KTRadioButtonList ID="rblProductKind" runat="server" AutoPostBack="true" CssClass="rbl-default" OnSelectedIndexChanged="rblProductKind_SelectedIndexChanged" RepeatDirection="Vertical">
                                                    </KTCC:KTRadioButtonList>
                                                </td>
                                                <td>
                                                    <table class="table-condition-sub">
                                                        <tr class="font-default">
                                                            <td style="width: 100px">工程</td>
                                                            <td>
                                                                <KTCC:KTDropDownList ID="ddlProcessKind" runat="server" CssClass="font-default ddl-default ddl-width-middle" AutoPostBack="true" OnSelectedIndexChanged="ddlProcessKind_SelectedIndexChanged" />
                                                            </td>
                                                        </tr>
                                                        <tr class="font-default">
                                                            <td style="width: 100px">エンジン種別</td>
                                                            <td>
                                                                <KTCC:KTDropDownList ID="ddlEngineKind" runat="server" CssClass="font-default ddl-default ddl-width-short" AutoPostBack="true" OnSelectedIndexChanged="ddlEngineKind_SelectedIndexChanged" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td>
                                                    <table class="table-condition-sub">
                                                        <tr class="font-default">
                                                            <td style="width: 80px">型式名</td>
                                                            <td>
                                                                <KTCC:KTTextBox ID="txtModelNm" runat="server" InputMode="HalfKana" AutoUpper="true" MaxLength="20" CssClass="font-default txt-default ime-inactive txt-width-long" />
                                                            </td>
                                                        </tr>
                                                        <tr class="font-default">
                                                            <td style="width: 80px">型式コード</td>
                                                            <td>
                                                                <KTCC:KTTextBox ID="txtModelCd" runat="server" InputMode="RegExp" RegExpression="[-A-Za-z0-9]+" AutoUpper="true" MaxLength="11" CssClass="font-default txt-default ime-disabled txt-width-long" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td>
                                                    <table class="table-condition-sub">
                                                        <tr class="font-default">
                                                            <td style="width: 70px">機番</td>
                                                            <td>
                                                                <KTCC:KTTextBox ID="txtSerial" runat="server" InputMode="AlphaNum" AutoUpper="true" MaxLength="7" CssClass="font-default txt-default ime-disabled txt-width-short" />
                                                            </td>
                                                        </tr>
                                                        <tr class="font-default">
                                                            <td style="width: 70px">IDNO</td>
                                                            <td>
                                                                <KTCC:KTTextBox ID="txtIdno" runat="server" InputMode="IntNum" MaxLength="7" CssClass="font-default txt-default ime-disabled txt-width-short" />
                                                            </td>
                                                        </tr>
                                                        <tr class="font-default">
                                                            <td style="width: 100px">PINコード</td>
                                                            <td>
                                                                <KTCC:KTTextBox ID="txtPinCd" runat="server" MaxLength="17" InputMode="RegExp" RegExpression="[-A-Za-z0-9]+" AutoUpper="true" CssClass="font-default txt-default ime-disabled txt-width-pincd" />
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
                                                <td style="min-width: 170px"></td>
                                            </tr>
                                            <tr class="font-default tr-condition-header ui-state-default">
                                                <td>工程別検索条件</td>
                                            </tr>
                                            <tr class="tr-condition-body">
                                                <td>
                                                    <UC:ConditionPartialView runat="server" ID="ucSubCondition"></UC:ConditionPartialView>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <div style="clear: both;"></div>
                        <div class="condition-button-area">
                            <KTCC:KTButton ID="btnSearch" runat="server" Text="検索" OnClick="btnSearch_Click" CssClass="btn-middle" />
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
                            <asp:GridView ID="grvHeaderLT" runat="server" CssClass="grid-layout2 ui-widget-content" AllowPaging="false" AllowSorting="true" PageSize="100" AutoGenerateColumns="false" OnSorting="grvMainProcessView_Sorting">
                                <HeaderStyle CssClass="grid-header ui-state-default" />
                            </asp:GridView>
                        </div>
                    </div>
                </td>
                <td>
                    <div id="divRTScroll" class="div-scroll-right-top div-right-grid">
                        <div id="divGrvHeaderRT" runat="server">
                            <asp:GridView ID="grvHeaderRT" runat="server" CssClass="grid-layout2 ui-widget-content" AllowPaging="false" AllowSorting="true" PageSize="100" AutoGenerateColumns="false" OnSorting="grvMainProcessView_Sorting">
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
                            <asp:GridView ID="grvMainProcessViewLB" runat="server" ShowHeader="false" CssClass="grid-layout2 ui-widget-content" AllowPaging="true" AllowSorting="false" PageSize="100" AutoGenerateColumns="false" OnRowDataBound="grvMainProcessViewLB_RowDataBound">
                                <PagerSettings Position="TopAndBottom" Mode="NumericFirstLast" PageButtonCount="10" Visible="false" />
                                <HeaderStyle CssClass="grid-header ui-state-default" />
                                <RowStyle CssClass="grid-row ui-widget" />
                                <SelectedRowStyle CssClass="ui-state-highlight" />
                                <Columns>
                                    <asp:TemplateField HeaderText="modelCd" />
                                    <asp:TemplateField HeaderText="modelCdDispNm" />
                                    <asp:TemplateField HeaderText="countryCd" />
                                    <asp:TemplateField HeaderText="countryCdDispNm" />
                                    <asp:TemplateField HeaderText="modelNm" />
                                    <asp:TemplateField HeaderText="serial6" />
                                    <asp:TemplateField HeaderText="serial7" />
                                    <asp:TemplateField HeaderText="pinCd" />
                                    <asp:TemplateField HeaderText="idno" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </td>
                <td>
                    <div id="divRBScroll" class="div-visible-scroll div-right-grid" >
                        <div id="divGrvRB" runat="server">
                            <asp:GridView ID="grvMainProcessViewRB" runat="server" ShowHeader="false" CssClass="grid-layout2 ui-widget-content" AllowPaging="true" AllowSorting="false" PageSize="100" AutoGenerateColumns="false" OnRowDataBound="grvMainProcessViewRB_RowDataBound" EnableViewState ="true">
                                <PagerSettings Position="TopAndBottom" Mode="NumericFirstLast" PageButtonCount="10" Visible="false" />
                                <HeaderStyle CssClass="grid-header ui-state-default" />
                                <RowStyle CssClass="grid-row ui-widget" />
                                <SelectedRowStyle CssClass="ui-state-highlight" />
                                <Columns>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField ID="hdnSelectedDataItemIndex" runat="server" value="-1" EnableViewState="false" />
</asp:Content>
<asp:Content ID="MasterBodyBottom" ContentPlaceHolderID="MasterBodyBottom" runat="server">
    <div id="divGrvCtrlButton" runat="server" class="div-bottom-button-area">
        <UC:OperationPartialView runat="server" ID="ucSubOperation"></UC:OperationPartialView>
        <KTCC:KTButton ID="btnExcel" runat="server" Text="Excel出力" CssClass="btn-middle" OnClick="btnExcel_Click" OnClientClick="SubmitControl.SetLoadingType(SubmitControl.TYPE_NONE);" />
    </div>
</asp:Content>
