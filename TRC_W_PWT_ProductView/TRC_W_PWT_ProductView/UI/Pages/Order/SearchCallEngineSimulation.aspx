<%@ Page Language="C#" MasterPageFile="~/UI/MasterForm/MasterMain.master" AutoEventWireup="true" CodeBehind="SearchCallEngineSimulation.aspx.cs" Inherits="TRC_W_PWT_ProductView.UI.Pages.Order.SearchCallEngineSimulation" %>

<%@ Register Src="~/UI/Pages/UserControl/InputModal.ascx" TagName="InputModal" TagPrefix="UC" %>
<%@ Import Namespace="TRC_W_PWT_ProductView.Common" %>

<asp:Content ID="MasterScript" ContentPlaceHolderID="MasterScript" runat="server">
    <script src="<%: PageUtils.ResolveUrl( this.Page, "~/Scripts/Pages/Order/SearchCallEngineSimulation.js") %>"></script>
    <style type="text/css">
        .auto-style1 {
            width: 40px;
            height: 24px;
        }

        .auto-style2 {
            height: 24px;
        }

        .hikiate-result0 {
            background-color: #FFC0C0;
        }

        .hikiate-result1 {
            background-color: #C0FFA0;
        }

        .hikiate-result2 {
            background-color: #C0C0FF;
        }

        .hikiate-result3 {
            background-color: #C0C0C0;
        }

        .tousai-engine-07 {
            background-color: #FF9900;
        }

    </style>
</asp:Content>

<asp:Content ID="MasterBody" ContentPlaceHolderID="MasterBody" runat="server">
    <%-- 30秒おきに検索 --%>
    <asp:Timer ID="tmrSearch" runat="server" Interval="30000" OnTick="tmrSearch_Tick"></asp:Timer>
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
                                                <td style="width: 240px"></td>
                                                <td style="width: 210px"></td>
                                                <td style="width: 290px"></td>
                                            </tr>
                                            <tr class="font-default tr-condition-header ui-state-default">
                                                <td>(T)完成予定日</td>
                                                <td>(T)IDNO</td>
                                                <td>(T)型式コード/(T)型式名</td>
                                            </tr>
                                            <tr class="tr-condition-body">
                                                <td>
                                                    <table class="table-condition-sub">
                                                        <tr class="font-default">
                                                            <td style="width: 110px">(T)完成予定日</td>
                                                            <td>
                                                                <KTCC:KTCalendar ID="cldTKanseiYoteiYmd" runat="server" UseCalendar="true" Type="yyyyMMdd" CssClass="font-default txt-default cld-ymd"></KTCC:KTCalendar>
                                                            </td>
                                                            <td style="width: 30px">まで</td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td>
                                                    <table class="table-condition-sub">
                                                        <tr class="font-default">
                                                            <td style="width: 70px">(T)IDNO</td>
                                                            <td>
                                                                <KTCC:KTTextBox ID="txtTIdno" runat="Server" CssClass="font-default txt-default ime-inactive txt-width-middle" InputMode="IntNum" MaxLength="7" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td>
                                                    <table class="table-condition-sub">
                                                        <tr class="font-default">
                                                            <td style="width: 100px">(T)型式コード</td>
                                                            <td>
                                                                <KTCC:KTTextBox ID="txtTKatashikiCd" runat="Server" CssClass="font-default txt-default ime-inactive txt-width-long" InputMode="RegExp" MaxLength="11" RegExpression="^[a-zA-Z0-9!-/:-@¥[-`{-~]*$" />
                                                            </td>
                                                        </tr>
                                                        <tr class="font-default">
                                                            <td style="width: 100px">(T)型式名</td>
                                                            <td>
                                                                <KTCC:KTTextBox ID="txtTKatashikiNm" runat="Server" CssClass="font-default txt-default ime-inactive txt-width-long" InputMode="HalfKana" MaxLength="20" />
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
                                                <td style="width: 270px"></td>
                                                <td style="width: 290px"></td>
                                                <td style="width: 290px"></td>
                                            </tr>
                                            <tr class="font-default tr-condition-header ui-state-default">
                                                <td>(T)特記事項</td>
                                                <td>(E)型式コード/(E)型式名</td>
                                                <td>引当先倉庫 絞込</td>
                                            </tr>
                                            <tr class="tr-condition-body">
                                                <td>
                                                    <table class="table-condition-sub">
                                                        <tr class="font-default">
                                                            <td style="width: 90px">(T)特記事項</td>
                                                            <td>
                                                                <KTCC:KTTextBox ID="txtTTokki" runat="Server" CssClass="font-default txt-default ime-inactive txt-width-long" InputMode="HalfKana" MaxLength="10" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td>
                                                    <table class="table-condition-sub">
                                                        <tr class="font-default">
                                                            <td style="width: 100px">(E)型式コード</td>
                                                            <td>
                                                                <KTCC:KTTextBox ID="txtEKatashikiCd" runat="Server" CssClass="font-default txt-default ime-inactive txt-width-long" InputMode="RegExp" MaxLength="11" RegExpression="^[a-zA-Z0-9!-/:-@¥[-`{-~]*$" />
                                                            </td>
                                                        </tr>
                                                        <tr class="font-default">
                                                            <td style="width: 100px">(E)型式名</td>
                                                            <td>
                                                                <KTCC:KTTextBox ID="txtEKatashikiNm" runat="Server" CssClass="font-default txt-default ime-inactive txt-width-long" InputMode="HalfKana" MaxLength="20" AutoUpper="False" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td>
                                                    <table class="table-condition-sub">
                                                        <tr class="font-default">
                                                            <td style="width: 80px">引当先倉庫</td>
                                                            <td>
                                                                <KTCC:KTDropDownList ID="ddlCallEngineWarehouse" runat="Server" CssClass="font-default ddl-default ddl-width-long" AutoPostBack="true" OnSelectedIndexChanged="ddlCallEngineWarehouse_SelectedIndexChanged" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <asp:HiddenField ID="hdnScrollTop" runat="server"/>
                                    <asp:HiddenField ID="hdnScrollLeft" runat="server"/>
                                    <asp:HiddenField ID="hdnAutoSearch" runat="server"/>
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
                            <asp:GridView ID="grvHeaderLT" runat="server" CssClass="grid-layout2 ui-widget-content" AllowPaging="false" AllowSorting="true" PageSize="1000" AutoGenerateColumns="false" OnSorting="gvSearchCallEngineSimulation_Sorting">
                                <HeaderStyle CssClass="grid-header ui-state-default" />
                            </asp:GridView>
                        </div>
                    </div>
                </td>
                <td>
                    <div id="divRTScroll" class="div-scroll-right-top div-right-grid">
                        <div id="divGrvHeaderRT" runat="server">
                            <asp:GridView ID="grvHeaderRT" runat="server" CssClass="grid-layout2 ui-widget-content" AllowPaging="false" AllowSorting="true" PageSize="1200" AutoGenerateColumns="false" OnSorting="gvSearchCallEngineSimulation_Sorting">
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
                            <asp:GridView ID="gvSearchCallEngineSimulationLB" runat="server" ShowHeader="false" CssClass="grid-layout2 ui-widget-content" AllowPaging="true" AllowSorting="false" PageSize="1000" AutoGenerateColumns="false" OnRowDataBound="gvSearchCallEngineSimulation_RowDataBound">
                                <PagerSettings Position="TopAndBottom" Mode="NumericFirstLast" PageButtonCount="10" Visible="false" />
                                <HeaderStyle CssClass="grid-header ui-state-default" />
                                <RowStyle CssClass="grid-row ui-widget" />
                                <SelectedRowStyle CssClass="ui-state-highlight" />
                                <Columns>
                                    <asp:TemplateField HeaderText="DISP_ORDER" />
                                    <asp:TemplateField HeaderText="T_SHIJI_YM_NUM" />
                                    <asp:TemplateField HeaderText="T_IDNO" />
                                    <asp:TemplateField HeaderText="T_KANSEI_YOTEI_DATE" />
                                    <asp:TemplateField HeaderText="T_KATASHIKI_CODE" />
                                    <asp:TemplateField HeaderText="T_KUNI_CODE" />
                                    <asp:TemplateField HeaderText="T_KATASHIKI_NAME" />
                                    <asp:TemplateField HeaderText="T_KIBAN" />
                                    <asp:TemplateField HeaderText="T_TOKKI" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </td>
                <td>
                    <div id="divRBScroll" class="div-visible-scroll div-right-grid" onscroll="SearchCallEngineSimulation.scrollEvent(this);">
                        <div id="divGrvRB" runat="server">
                            <asp:GridView ID="gvSearchCallEngineSimulationRB" runat="server" ShowHeader="false" CssClass="grid-layout2 ui-widget-content" AllowPaging="true" AllowSorting="false" PageSize="1200" AutoGenerateColumns="false" OnRowDataBound="gvSearchCallEngineSimulation_RowDataBound">
                                <PagerSettings Position="TopAndBottom" Mode="NumericFirstLast" PageButtonCount="10" Visible="false" />
                                <HeaderStyle CssClass="grid-header ui-state-default" />
                                <RowStyle CssClass="grid-row ui-widget" />
                                <SelectedRowStyle CssClass="ui-state-highlight" />
                                <Columns>
                                    <asp:TemplateField HeaderText="E_KATASHIKI_CODE" />
                                    <asp:TemplateField HeaderText="E_KATASHIKI_NAME" />
                                    <asp:TemplateField HeaderText="E_HITSUYO_NUM" />
                                    <asp:TemplateField HeaderText="E_ZAIKO_NUM" />
                                    <asp:TemplateField HeaderText="E_HIKIATE_RESULT" />
                                    <asp:TemplateField HeaderText="MACHINE_NO_01_STOCK" />
                                    <asp:TemplateField HeaderText="MACHINE_NO_02_STOCK" />
                                    <asp:TemplateField HeaderText="E_IDNO" />
                                    <asp:TemplateField HeaderText="MISSHION_JISSEKI_DATE_PREV" />
                                    <asp:TemplateField HeaderText="MISSHION_JISSEKI_DATE_POST" />
                                    <asp:TemplateField HeaderText="AP_CODE" />
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
