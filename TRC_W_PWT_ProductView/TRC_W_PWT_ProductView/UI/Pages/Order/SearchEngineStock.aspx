<%@ Page Language="C#" MasterPageFile="~/UI/MasterForm/MasterMain.master" AutoEventWireup="true" CodeBehind="SearchEngineStock.aspx.cs" Inherits="TRC_W_PWT_ProductView.UI.Pages.Order.SearchEngineStock" %>

<%@ Register Src="~/UI/Pages/UserControl/InputModal.ascx" TagName="InputModal" TagPrefix="UC" %>
<%@ Import Namespace="TRC_W_PWT_ProductView.Common" %>

<asp:Content ID="MasterScript" ContentPlaceHolderID="MasterScript" runat="server">
    <script src="<%: PageUtils.ResolveUrl( this.Page, "~/Scripts/Pages/Order/SearchEngineStock.js") %>"></script>
    <style type="text/css">
        .auto-style1 {
            width: 40px;
            height: 24px;
        }

        .auto-style2 {
            height: 24px;
        }

        .rittai_zaiko1 {
            color: #3333aa;
        }

        .rittai_zaiko2 {
            color: #aa33aa;
        }

        .rittai_zaiko3 {
            color: #33aaaa;
        }

        .rittai_zaiko4 {
            color: #33aa33;
        }

        .syubetsu_zaiko0 {
            color: #333333;
        }

        .syubetsu_zaiko1 {
            color: #3333aa;
        }

        .syubetsu_zaiko2 {
            color: #3333aa;
        }

        .syubetsu_zaiko3 {
            color: #aa33aa;
        }

        .syubetsu_zaiko4 {
            color: #aa33aa;
        }

        .syubetsu_zaiko5 {
            color: #33aaaa;
        }

        .pnl_zaikosu2_margin {
            margin-left: 146px;
        }

        .zaiko_font {
            font-weight: bold;
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
                                                <td style="width: 180px"></td>
                                                <td style="width: 150px"></td>
                                                <td style="width: 180px"></td>
                                                <td style="width: 150px"></td>
                                                <td style="width: 150px"></td>
                                            </tr>
                                            <tr class="font-default tr-condition-header ui-state-default">
                                                <td>立体倉庫</td>
                                                <td>棚/状態</td>
                                                <td>種別/搭載/OEM</td>
                                                <td>内外作</td>
                                                <td>運転</td>
                                            </tr>
                                            <tr class="tr-condition-body">
                                                <td>
                                                    <table class="table-condition-sub">
                                                        <tr class="font-default">
                                                            <td style="width: 70px">立体倉庫</td>
                                                            <td>
                                                                <KTCC:KTDropDownList ID="ddlRittai" runat="server" CssClass="font-default ddl-default ddl-width-short" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td>
                                                    <table class="table-condition-sub">
                                                        <tr class="font-default">
                                                            <td style="width: 40px">棚</td>
                                                            <td>
                                                                <KTCC:KTDropDownList ID="ddlStopFlag" runat="server" CssClass="font-default ddl-default ddl-width-short" />
                                                            </td>
                                                        </tr>
                                                        <tr class="font-default">
                                                            <td style="width: 40px">状態</td>
                                                            <td>
                                                                <KTCC:KTDropDownList ID="ddlLocationFlag" runat="server" CssClass="font-default ddl-default ddl-width-short" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td>
                                                    <table class="table-condition-sub">
                                                        <tr class="font-default">
                                                            <td style="width: 80px">種別</td>
                                                            <td>
                                                                <KTCC:KTDropDownList ID="ddlEngineSyubetsu" runat="server" CssClass="font-default ddl-default ddl-width-short" />
                                                            </td>
                                                        </tr>
                                                        <tr class="font-default">
                                                            <td style="width: 80px">搭載/OEM</td>
                                                            <td>
                                                                <KTCC:KTDropDownList ID="ddlTousaiOem" runat="server" CssClass="font-default ddl-default ddl-width-short" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td>
                                                    <table class="table-condition-sub">
                                                        <tr class="font-default">
                                                            <td style="width: 60px">内外作</td>
                                                            <td>
                                                                <KTCC:KTDropDownList ID="ddlNaigaisaku" runat="server" CssClass="font-default ddl-default ddl-width-short" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td>
                                                    <table class="table-condition-sub">
                                                        <tr class="font-default">
                                                            <td style="width: 40px">運転</td>
                                                            <td>
                                                                <KTCC:KTDropDownList ID="ddlUntenFlag" runat="server" CssClass="font-default ddl-default ddl-width-short" />
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
                                                <td style="width: 230px"></td>
                                                <td style="width: 270px"></td>
                                                <td style="width: 270px"></td>
                                            </tr>
                                            <tr class="font-default tr-condition-header ui-state-default">
                                                <td>IDNO/機番</td>
                                                <td>型式コード/型式名</td>
                                                <td>特記事項</td>
                                            </tr>
                                            <tr class="tr-condition-body">

                                                <td>
                                                    <table class="table-condition-sub">
                                                        <tr class="font-default">
                                                            <td class="auto-style1">IDNO</td>
                                                            <td class="auto-style2">
                                                                <KTCC:KTTextBox ID="txtIdno" runat="Server" CssClass="font-default txt-default ime-inactive txt-width-long" InputMode="IntNum" MaxLength="7" />
                                                            </td>
                                                        </tr>
                                                        <tr class="font-default">
                                                            <td style="width: 40px">機番</td>
                                                            <td>
                                                                <KTCC:KTTextBox ID="txtKiban" runat="Server" CssClass="font-default txt-default ime-inactive txt-width-long" InputMode="AlphaNum" MaxLength="7" AutoUpper="False" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td>
                                                    <table class="table-condition-sub">
                                                        <tr class="font-default">
                                                            <td style="width: 80px">型式コード</td>
                                                            <td>
                                                                <KTCC:KTTextBox ID="txtKatashikiCd" runat="Server" CssClass="font-default txt-default ime-inactive txt-width-long" InputMode="RegExp" MaxLength="11" RegExpression="^[a-zA-Z0-9!-/:-@¥[-`{-~]*$" />
                                                            </td>
                                                        </tr>
                                                        <tr class="font-default">
                                                            <td style="width: 80px">型式名</td>
                                                            <td>
                                                                <KTCC:KTTextBox ID="txtKatashikiNm" runat="Server" CssClass="font-default txt-default ime-inactive txt-width-long" InputMode="HalfKana" MaxLength="20" AutoUpper="False" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td>
                                                    <table class="table-condition-sub">
                                                        <tr class="font-default">
                                                            <td style="width: 80px">特記事項</td>
                                                            <td>
                                                                <KTCC:KTTextBox ID="txtTokki" runat="Server" CssClass="font-default txt-default ime-inactive txt-width-long" InputMode="HalfKana" MaxLength="10" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <asp:HiddenField ID="hdnSearchTarget" runat="server" Value="KATASHIKIBETSU" />
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <div style="clear: both;"></div>
                        <div class="condition-button-area">
                            <KTCC:KTButton ID="btnKatashikibetsuSearch" runat="server" Text="型式別検索" CssClass="btn-middle" OnClick="btnKatashikibetsu_Click" />
                            &nbsp;
                            <KTCC:KTButton ID="btnMeisai" runat="server" Text="明細" CssClass="btn-middle" OnClick="btnMeisai_Click" />
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
                            <asp:GridView ID="grvHeaderLT" runat="server" CssClass="grid-layout2 ui-widget-content" AllowPaging="false" AllowSorting="true" PageSize="1000" AutoGenerateColumns="false" OnSorting="gvSearchEngineStock_Sorting">
                                <HeaderStyle CssClass="grid-header ui-state-default" />
                            </asp:GridView>
                        </div>
                    </div>
                </td>
                <td>
                    <div id="divRTScroll" class="div-scroll-right-top div-right-grid">
                        <div id="divGrvHeaderRT" runat="server">
                            <asp:GridView ID="grvHeaderRT" runat="server" CssClass="grid-layout2 ui-widget-content" AllowPaging="false" AllowSorting="true" PageSize="1000" AutoGenerateColumns="false" OnSorting="gvSearchEngineStock_Sorting">
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
                            <asp:GridView ID="gvSearchEngineStockLB" runat="server" ShowHeader="false" CssClass="grid-layout2 ui-widget-content" AllowPaging="true" AllowSorting="false" PageSize="1000" AutoGenerateColumns="false" OnRowDataBound="gvSearchEngineStock_RowDataBound">
                                <PagerSettings Position="TopAndBottom" Mode="NumericFirstLast" PageButtonCount="10" Visible="false" />
                                <HeaderStyle CssClass="grid-header ui-state-default" />
                                <RowStyle CssClass="grid-row ui-widget" />
                                <SelectedRowStyle CssClass="ui-state-highlight" />
                                <Columns>
                                    <asp:TemplateField HeaderText="DISP_ORDER" />
                                    <asp:TemplateField HeaderText="RITTAI_NAME" />
                                    <asp:TemplateField HeaderText="LOCATION_NAME" />
                                    <asp:TemplateField HeaderText="STOCK_KBN" />
                                    <asp:TemplateField HeaderText="STOCK_REN" />
                                    <asp:TemplateField HeaderText="STOCK_DAN" />
                                    <asp:TemplateField HeaderText="STOCK_RETSU" />
                                    <asp:TemplateField HeaderText="STOP_FLAG" />
                                    <asp:TemplateField HeaderText="LOCATION_FLAG" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </td>
                <td>
                    <div id="divRBScroll" class="div-visible-scroll div-right-grid">
                        <div id="divGrvRB" runat="server">
                            <asp:GridView ID="gvSearchEngineStockRB" runat="server" ShowHeader="false" CssClass="grid-layout2 ui-widget-content" AllowPaging="true" AllowSorting="false" PageSize="1000" AutoGenerateColumns="false" OnRowDataBound="gvSearchEngineStock_RowDataBound">
                                <PagerSettings Position="TopAndBottom" Mode="NumericFirstLast" PageButtonCount="10" Visible="false" />
                                <HeaderStyle CssClass="grid-header ui-state-default" />
                                <RowStyle CssClass="grid-row ui-widget" />
                                <SelectedRowStyle CssClass="ui-state-highlight" />
                                <Columns>
                                    <asp:TemplateField HeaderText="KATASHIKI_CODE" />
                                    <asp:TemplateField HeaderText="KUNI_CODE" />
                                    <asp:TemplateField HeaderText="KATASHIKI_NAME" />
                                    <asp:TemplateField HeaderText="KUMITATE_PATTERN" />
                                    <asp:TemplateField HeaderText="ENGINE_SYUBETSU" />
                                    <asp:TemplateField HeaderText="TOUSAI_OEM" />
                                    <asp:TemplateField HeaderText="NAIGAISKAU" />
                                    <asp:TemplateField HeaderText="DAISU" />
                                    <asp:TemplateField HeaderText="IDNO" />
                                    <asp:TemplateField HeaderText="KIBAN" />
                                    <asp:TemplateField HeaderText="TOKKI" />
                                    <asp:TemplateField HeaderText="UNTENFLAG" />
                                    <asp:TemplateField HeaderText="INSTOCK_DATE" />
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
    <asp:Panel ID="pnlZaikosu" runat="server">
        <asp:Label ID="lblNowStockStat" CssClass="zaiko_font" runat="server">【現在の在庫状況】　</asp:Label>
        <asp:Label ID="lblRittaibetsuStockQnt" CssClass="zaiko_font" runat="server">立体別全在庫数：　</asp:Label>
        <asp:Label ID="lblTsukuba" CssClass="rittai_zaiko1 zaiko_font" runat="server"></asp:Label>
        <asp:Label ID="lblOem" CssClass="rittai_zaiko2 zaiko_font" runat="server"></asp:Label>
        <asp:Label ID="lblRittaiSakai" CssClass="rittai_zaiko3 zaiko_font" runat="server"></asp:Label>
        <asp:Label ID="lblTosougo" CssClass="rittai_zaiko4 zaiko_font" runat="server"></asp:Label>
    </asp:Panel>
    <asp:Panel ID="pnlZaikosu2" runat="server" CssClass="pnl_zaikosu2_margin">
        <asp:Label ID="lblSyuruibetsuStockQnt" CssClass="zaiko_font" runat="server">種類別全在庫数：　</asp:Label>
        <asp:Label ID="lbl03Tousai" CssClass="syubetsu_zaiko1 zaiko_font" runat="server"></asp:Label>
        <asp:Label ID="lbl07Tousai" CssClass="syubetsu_zaiko2 zaiko_font" runat="server"></asp:Label>
        <asp:Label ID="lbl03Oem" CssClass="syubetsu_zaiko3 zaiko_font" runat="server"></asp:Label>
        <asp:Label ID="lbl07Oem" CssClass="syubetsu_zaiko4 zaiko_font" runat="server"></asp:Label>
        <asp:Label ID="lblSyuruiSakai" CssClass="syubetsu_zaiko5 zaiko_font" runat="server"></asp:Label>
        <asp:Label ID="lblTotalQnt" CssClass="zaiko_font" runat="server"></asp:Label>
        <asp:Label ID="lblSup" CssClass="zaiko_font" runat="server">※塗装後立体を除く</asp:Label>
    </asp:Panel>
    <div id="divGrvCtrlButton" runat="server" class="div-bottom-button-area">
        <KTCC:KTButton ID="btnExcel" runat="server" Text="Excel出力" CssClass="btn-middle" Visible="false" OnClientClick="SubmitControl.SetLoadingType(SubmitControl.TYPE_NONE);" OnClick="btnExcel_Click" />
    </div>
</asp:Content>
