<%@ Page Language="C#" MasterPageFile="~/UI/MasterForm/MasterMain.master" AutoEventWireup="true" CodeBehind="SearchProductOrder.aspx.cs" Inherits="TRC_W_PWT_ProductView.UI.Pages.Order.SearchProductOrder" %>

<%@ Register Src="~/UI/Pages/UserControl/InputModal.ascx" TagName="InputModal" TagPrefix="UC" %>
<%@ Import Namespace="TRC_W_PWT_ProductView.Common" %>

<asp:Content ID="MasterScript" ContentPlaceHolderID="MasterScript" runat="server">
    <script src="<%: PageUtils.ResolveUrl( this.Page, "~/Scripts/Pages/Order/SearchProductOrder.js") %>"></script>
    <style type="text/css">
        .blue {
            color: #000099;
        }

        .gray {
            color: #777777;
        }
    </style>
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
                                                <td style="width: 140px"></td>
                                                <td style="width: 220px"></td>
                                            </tr>
                                            <tr class="font-default tr-condition-header ui-state-default">
                                                <td>製品種別</td>
                                                <td>エンジン種別</td>
                                                <td>IDNO/機番</td>
                                            </tr>
                                            <tr class="tr-condition-body">
                                                <td>
                                                    <table class="table-condition-sub">
                                                        <tr class="font-default">
                                                            <td>
                                                                <KTCC:KTRadioButtonList ID="rblShijiLevel" runat="Server" RepeatColumns="2" CssClass="rbl-default" AutoPostBack="True" OnSelectedIndexChanged="rblShijiLevel_SelectedIndexChanged">
                                                                    <asp:ListItem Value="指定しない">指定しない(ID又は型式を指定)</asp:ListItem>
                                                                    <asp:ListItem>トラクタ</asp:ListItem>
                                                                    <asp:ListItem>03エンジン</asp:ListItem>
                                                                    <asp:ListItem>07エンジン</asp:ListItem>
                                                                </KTCC:KTRadioButtonList>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td>
                                                    <table class="table-condition-sub">
                                                        <tr class="font-default">
                                                            <td>
                                                                <KTCC:KTRadioButtonList ID="rblPatternFlag" runat="Server" CssClass="rbl-default box-in-left">
                                                                    <asp:ListItem>全て</asp:ListItem>
                                                                    <asp:ListItem>搭載</asp:ListItem>
                                                                    <asp:ListItem>OEM</asp:ListItem>
                                                                </KTCC:KTRadioButtonList>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td>
                                                    <table class="table-condition-sub">
                                                        <tr class="font-default">
                                                            <td style="width: 40px">IDNO</td>
                                                            <td>
                                                                <KTCC:KTTextBox ID="txtIdno" runat="server" InputMode="IntNum" AutoUpper="false" CssClass="font-default txt-default ime-inactive txt-width-long" MaxLength="7" />
                                                            </td>
                                                        </tr>
                                                        <tr class="font-default">
                                                            <td style="width: 40px">機番</td>
                                                            <td>
                                                                <KTCC:KTTextBox ID="txtKiban" runat="server" InputMode="AlphaNum" AutoUpper="false" CssClass="font-default txt-default ime-inactive txt-width-long" MaxLength="7" />
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
                                                <td style="width: 270px"></td>
                                            </tr>
                                            <tr class="font-default tr-condition-header ui-state-default">
                                                <td>型式コード/国コード/型式名</td>
                                                <td>投入順序連番/確定順序連番</td>
                                            </tr>
                                            <tr class="tr-condition-body">
                                                <td>
                                                    <table class="table-condition-sub">
                                                        <tr class="font-default">
                                                            <td style="width: 80px">型式コード</td>
                                                            <td>
                                                                <KTCC:KTTextBox ID="txtKatashikiCd" runat="Server" CssClass="font-default txt-default ime-inactive txt-width-long" InputMode="RegExp" MaxLength="11" RegExpression="^[a-zA-Z0-9!-/:-@¥[-`{-~]*$" />
                                                            </td>
                                                        </tr>
                                                        <tr class="font-default">
                                                            <td style="width: 80px">国コード</td>
                                                            <td>
                                                                <KTCC:KTTextBox ID="txtKuniCd" runat="Server" CssClass="font-default txt-default ime-inactive txt-width-long" InputMode="IntNum" MaxLength="6" />
                                                            </td>
                                                        </tr>
                                                        <tr class="font-default">
                                                            <td style="width: 80px">型式名</td>
                                                            <td>
                                                                <KTCC:KTTextBox ID="txtKatashikiNm" runat="Server" CssClass="font-default txt-default ime-inactive txt-width-long" InputMode="HalfKana" MaxLength="20" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td>
                                                    <table class="table-condition-sub">
                                                        <tr class="font-default">
                                                            <td style="width: 90px">投入順序連番</td>
                                                            <td>
                                                                <KTCC:KTTextBox ID="txtTonyuYmNum" runat="Server" CssClass="font-default txt-default ime-inactive txt-width-long" InputMode="IntNum" MaxLength="14" />
                                                            </td>
                                                        </tr>
                                                        <tr class="font-default">
                                                            <td style="width: 90px">確定順序連番</td>
                                                            <td>
                                                                <KTCC:KTTextBox ID="txtKakuteiYmNum" runat="Server" CssClass="font-default txt-default ime-inactive txt-width-long" InputMode="IntNum" MaxLength="14" />
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
                            <KTCC:KTButton ID="btnSearch" runat="server" Text="検索" CssClass="btn-middle" OnClick="btnSearch_Click"/>
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
                            <asp:GridView ID="grvHeaderLT" runat="server" CssClass="grid-layout2 ui-widget-content" AllowPaging="false" AllowSorting="true" PageSize="1000" AutoGenerateColumns="false" OnSorting="gvSearchProductOrder_Sorting">
                                <HeaderStyle CssClass="grid-header ui-state-default" />
                            </asp:GridView>
                        </div>
                    </div>
                </td>
                <td>
                    <div id="divRTScroll" class="div-scroll-right-top div-right-grid">
                        <div id="divGrvHeaderRT" runat="server">
                            <asp:GridView ID="grvHeaderRT" runat="server" CssClass="grid-layout2 ui-widget-content" AllowPaging="false" AllowSorting="true" PageSize="1000" AutoGenerateColumns="false" OnSorting="gvSearchProductOrder_Sorting">
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
                            <asp:GridView ID="gvSearchProductOrderLB" runat="server" ShowHeader="false" CssClass="grid-layout2 ui-widget-content" AllowPaging="true" AllowSorting="false" PageSize="1000" AutoGenerateColumns="false" OnRowDataBound="gvSearchProductOrder_RowDataBound">
                                <PagerSettings Position="TopAndBottom" Mode="NumericFirstLast" PageButtonCount="10" Visible="false" />
                                <HeaderStyle CssClass="grid-header ui-state-default" />
                                <RowStyle CssClass="grid-row ui-widget" />
                                <SelectedRowStyle CssClass="ui-state-highlight" />
                                <Columns>
                                    <asp:TemplateField HeaderText="DISP_ORDER" />
                                    <asp:TemplateField HeaderText="IDNO" />
                                    <asp:TemplateField HeaderText="KATASHIKI_CODE" />
                                    <asp:TemplateField HeaderText="KUNI_CODE" />
                                    <asp:TemplateField HeaderText="KATASHIKI_NAME" />
                                    <asp:TemplateField HeaderText="KIBAN" />
                                    <asp:TemplateField HeaderText="KUMITATE_PATTERN_NAME" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </td>
                <td>
                    <div id="divRBScroll" class="div-visible-scroll div-right-grid">
                        <div id="divGrvRB" runat="server">
                            <asp:GridView ID="gvSearchProductOrderRB" runat="server" ShowHeader="false" CssClass="grid-layout2 ui-widget-content" AllowPaging="true" AllowSorting="false" PageSize="1000" AutoGenerateColumns="false" OnRowDataBound="gvSearchProductOrder_RowDataBound">
                                <PagerSettings Position="TopAndBottom" Mode="NumericFirstLast" PageButtonCount="10" Visible="false" />
                                <HeaderStyle CssClass="grid-header ui-state-default" />
                                <RowStyle CssClass="grid-row ui-widget" />
                                <SelectedRowStyle CssClass="ui-state-highlight" />
                                <Columns>
                                    <asp:TemplateField HeaderText="TONYU_YM_NUM" />
                                    <asp:TemplateField HeaderText="KAKUTEI_YM_NUM" />
                                    <asp:TemplateField HeaderText="STATION_LIST" />
                                    <asp:TemplateField HeaderText="WAREHOUSE_NAME" />
                                    <asp:TemplateField HeaderText="SYUKKA_DATE" />
                                    <asp:TemplateField HeaderText="ENGINE_KATASHIKI_CODE" />
                                    <asp:TemplateField HeaderText="ENGINE_IDNO" />
                                    <asp:TemplateField HeaderText="ENGINE_KIBAN" />
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