<%@ Page Language="C#" MasterPageFile="~/UI/MasterForm/MasterMain.master" AutoEventWireup="true" CodeBehind="SearchOrderInfo.aspx.cs" Inherits="TRC_W_PWT_ProductView.UI.Pages.Order.SearchOrderInfo" %>

<%@ Register Src="~/UI/Pages/UserControl/InputModal.ascx" TagName="InputModal" TagPrefix="UC" %>
<%@ Import Namespace="TRC_W_PWT_ProductView.Common" %>

<asp:Content ID="MasterScript" ContentPlaceHolderID="MasterScript" runat="server">
    <script src="<%: PageUtils.ResolveUrl( this.Page, "~/Scripts/Pages/Order/SearchOrderInfo.js") %>"></script>
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
                                                <td style="width: 350px"></td>
                                                <td style="width: 170px"></td>
                                                <td style="width: 250px"></td>
                                            </tr>
                                            <tr class="font-default tr-condition-header ui-state-default">
                                                <td>製品種別</td>
                                                <td>IDNO/機番</td>
                                                <td>月度</td>
                                            </tr>
                                            <tr class="tr-condition-body">
                                                <td>
                                                    <table class="table-condition-sub">
                                                        <tr class="font-default">
                                                            <td>
                                                                <KTCC:KTRadioButtonList ID="rblShijiLevel" runat="Server" RepeatColumns="3" CssClass="rbl-default box-in-left" AutoPostBack="True" OnSelectedIndexChanged="rblShijiLevel_SelectedIndexChanged">
                                                                    <asp:ListItem>ID/機番検索</asp:ListItem>
                                                                    <asp:ListItem>03エンジン</asp:ListItem>
                                                                    <asp:ListItem>07エンジン</asp:ListItem>
                                                                    <asp:ListItem>ミッション投入</asp:ListItem>
                                                                    <asp:ListItem>本機確定</asp:ListItem>
                                                                </KTCC:KTRadioButtonList>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td>
                                                    <table class="table-condition-sub">
                                                        <tr class="font-default">
                                                            <td style="width: 60px">IDNO</td>
                                                            <td>
                                                                <KTCC:KTTextBox ID="txtIdno" runat="server" InputMode="IntNum" MaxLength="7" CssClass="font-default txt-default ime-inactive txt-width-short" />
                                                            </td>
                                                        </tr>
                                                        <tr class="font-default">
                                                            <td style="width: 60px">機番</td>
                                                            <td>
                                                                <KTCC:KTTextBox ID="txtKiban" runat="server" InputMode="AlphaNum" MaxLength="7" CssClass="font-default txt-default ime-inactive txt-width-short" AutoUpper="False" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td>
                                                    <table class="table-condition-sub">
                                                        <tr class="font-default">
                                                            <td style="width: 60px">月度</td>
                                                            <td>
                                                                <div>
                                                                    <KTCC:KTDropDownList ID="ddlShijiYM" runat="server" CssClass="font-default txt-default ime-inactive ddl-width-middle ddl-default" />
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
                                                <td style="width: 290px"></td>
                                                <td style="width: 170px"></td>
                                            </tr>
                                            <tr class="font-default tr-condition-header ui-state-default">
                                                <td>型式コード/国コード/型式名</td>
                                                <td>特記事項</td>
                                            </tr>
                                            <tr class="tr-condition-body">
                                                <td>
                                                    <table class="table-condition-sub">
                                                        <tr class="font-default">
                                                            <td style="width: 90px">型式コード</td>
                                                            <td>
                                                                <KTCC:KTTextBox ID="txtKatashikiCode" runat="server" InputMode="RegExp" MaxLength="11" CssClass="font-default txt-default ime-inactive txt-width-long" AutoUpper="False" RegExpression="^[a-zA-Z0-9!-/:-@¥[-`{-~]*$" />
                                                            </td>
                                                        </tr>
                                                        <tr class="font-default">
                                                            <td style="width: 90px">国コード</td>
                                                            <td>
                                                                <KTCC:KTTextBox ID="txtKuniCode" runat="server" InputMode="IntNum" MaxLength="6" CssClass="font-default txt-default ime-inactive txt-width-long" />
                                                            </td>
                                                        </tr>
                                                        <tr class="font-default">
                                                            <td style="width: 90px">型式名</td>
                                                            <td>
                                                                <KTCC:KTTextBox ID="txtKatashikiName" runat="server" InputMode="HalfKana" MaxLength="20" CssClass="font-default txt-default ime-inactive txt-width-long" AutoUpper="False" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td>
                                                    <table class="table-condition-sub">
                                                        <tr class="font-default">
                                                            <td style="width: 70px">特記事項</td>
                                                            <td>
                                                                <KTCC:KTTextBox ID="txtTokki" runat="server" InputMode="HalfKana" MaxLength="10" CssClass="font-default txt-default ime-inactive txt-width-short" />
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
                            <KTCC:KTButton ID="btnSearch" runat="server" Text="検索" CssClass="btn-middle" OnClick="btnSearch_Click" />
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
                            <asp:GridView ID="grvHeaderLT" runat="server" CssClass="grid-layout2 ui-widget-content" AllowPaging="false" AllowSorting="true" PageSize="1000" AutoGenerateColumns="false" OnSorting="grvSearchOrderInfo_Sorting">
                                <HeaderStyle CssClass="grid-header ui-state-default" />
                            </asp:GridView>
                        </div>
                    </div>
                </td>
                <td>
                    <div id="divRTScroll" class="div-scroll-right-top div-right-grid">
                        <div id="divGrvHeaderRT" runat="server">
                            <asp:GridView ID="grvHeaderRT" runat="server" CssClass="grid-layout2 ui-widget-content" AllowPaging="false" AllowSorting="true" PageSize="1000" AutoGenerateColumns="false" OnSorting="grvSearchOrderInfo_Sorting">
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
                            <asp:GridView ID="grvSearchOrderInfoLB" runat="server" ShowHeader="false" CssClass="grid-layout2 ui-widget-content" AllowPaging="true" AllowSorting="false" PageSize="1000" AutoGenerateColumns="false" OnRowDataBound="grvSearchOrderInfo_RowDataBound">
                                <PagerSettings Position="TopAndBottom" Mode="NumericFirstLast" PageButtonCount="10" Visible="false" />
                                <HeaderStyle CssClass="grid-header ui-state-default" />
                                <RowStyle CssClass="grid-row ui-widget grid-row-double-height" />
                                <SelectedRowStyle CssClass="ui-state-highlight" />
                                <Columns>
                                    <asp:TemplateField HeaderText="DISP_ORDER" />
                                    <asp:TemplateField HeaderText="SHIJI_YM_NUM" />
                                    <asp:TemplateField HeaderText="SHIJI_YM_NUM_FOR_KAKUTEI" />
                                    <asp:TemplateField HeaderText="TYOKKO_SIGN" />
                                    <asp:TemplateField HeaderText="SHIJI_YM_NUM_UNLABELED" />
                                    <asp:TemplateField HeaderText="IDNO" />
                                    <asp:TemplateField HeaderText="KATASHIKI_CODE" />
                                    <asp:TemplateField HeaderText="KUNI_CODE" />
                                    <asp:TemplateField HeaderText="KATASHIKI_NAME" />
                                    <asp:TemplateField HeaderText="BASE_KATASHIKI_NAME" />
                                    <asp:TemplateField HeaderText="KIBAN" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </td>
                <td>
                    <div id="divRBScroll" class="div-visible-scroll div-right-grid">
                        <div id="divGrvRB" runat="server">
                            <asp:GridView ID="grvSearchOrderInfoRB" runat="server" ShowHeader="false" CssClass="grid-layout2 ui-widget-content" AllowPaging="true" AllowSorting="false" PageSize="1000" AutoGenerateColumns="false" OnRowDataBound="grvSearchOrderInfo_RowDataBound">
                                <PagerSettings Position="TopAndBottom" Mode="NumericFirstLast" PageButtonCount="10" Visible="false" />
                                <HeaderStyle CssClass="grid-header ui-state-default" />
                                <RowStyle CssClass="grid-row ui-widget grid-row-double-height" />
                                <SelectedRowStyle CssClass="ui-state-highlight" />
                                <Columns>
                                    <asp:TemplateField HeaderText="TOKKI" />
                                    <asp:TemplateField HeaderText="KANSEI_YOTEI_DATE" />
                                    <asp:TemplateField HeaderText="KANSEI_DATE" />
                                    <asp:TemplateField HeaderText="SYUKKA_DATE" />
                                    <asp:TemplateField HeaderText="ENGINE_KATASHIKI_CODE" />
                                    <asp:TemplateField HeaderText="ENGINE_KATASHIKI_NAME" />
                                    <asp:TemplateField HeaderText="ENGINE_IDNO" />
                                    <asp:TemplateField HeaderText="ENGINE_KIBAN" />
                                    <asp:TemplateField HeaderText="KUMITATE_PATTERN" />
                                    <asp:TemplateField HeaderText="ENGINE_KUMITATE_PATTERN" />
                                    <asp:TemplateField HeaderText="SHIJI_LEVEL" />
                                    <asp:TemplateField HeaderText="KEIKAKU_DAISU" />
                                    <asp:TemplateField HeaderText="RUIKEI_DAISU" />
                                    <asp:TemplateField HeaderText="CREATE_DATE" />
                                    <asp:TemplateField HeaderText="FIX_NUM" />
                                    <asp:TemplateField HeaderText="FIX_NUM_SEQ" />
                                    <asp:TemplateField HeaderText="CARGO_READY_DT" />
                                    <asp:TemplateField HeaderText="ORDER_QUANTITY" />
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
    </div>
</asp:Content>
