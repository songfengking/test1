<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/UI/MasterForm/MasterMain.master" CodeBehind="MainView.aspx.cs" Inherits="TRC_W_PWT_ProductView.UI.Pages.MainView" %>

<%@ Import Namespace="TRC_W_PWT_ProductView.Common" %>

<asp:Content ID="MasterScript" runat="server" ContentPlaceHolderID="MasterScript">
    <script src="<%: PageUtils.ResolveUrl( this.Page, "~/Scripts/Pages/MainView.js") %>"></script>
</asp:Content>

<asp:Content ID="MasterBody" ContentPlaceHolderID="MasterBody" runat="server">
    <div>
        <table class="table-layout">
            <tr>
                <td style="padding-top: 5px;">
                    <KTCC:KTButton ID="ChangeProductSearch" Text="製品検索" runat="server" Style="margin-left: 3px; padding-top: 3px; padding-bottom: 3px; background-color: lightskyblue; border: 1px solid black; pointer-events: none;" />
                    <KTCC:KTButton ID="ChangePartsSearch" Text="部品検索" runat="server" OnClick="ChangePartsSearch_Click" Style="margin-left: -7px; background-color: #E0ECF8; border: 1px solid black; padding-top: 3px; padding-bottom: 3px;" />
                    <KTCC:KTButton ID="ChangeProcessSearch" Text="工程検索" runat="server" OnClick="ChangeProcessSearch_Click" Style="margin-left: -7px; background-color: #E0ECF8; border: 1px solid black; padding-top: 3px; padding-bottom: 3px;" />
                    <div class="box-in-margin-small">
                        <asp:UpdatePanel ID="upnCondition" runat="server">
                            <ContentTemplate>
                                <div class="condition-box">
                                    <div class="condition-in-box-main">
                                        <table class="table-border-layout" style="width: 600px">
                                            <tr class="tr-fix-zero-height">
                                                <td style="width: 70px"></td>
                                                <td style="width: 200px"></td>
                                                <td style="width: 160px"></td>
                                            </tr>
                                            <tr class="font-default tr-condition-header ui-state-default">
                                                <td>製品種別</td>
                                                <td>製品型式</td>
                                                <td>機番/IDNO/PINコード</td>
                                            </tr>
                                            <tr class="tr-condition-body">
                                                <td>
                                                    <KTCC:KTRadioButtonList ID="rblProductKind" runat="server" AutoPostBack="true" CssClass="rbl-default" OnSelectedIndexChanged="rblProductKind_SelectedIndexChanged" RepeatDirection="Vertical">
                                                    </KTCC:KTRadioButtonList>
                                                </td>
                                                <td>
                                                    <div class="condition-tr-height box-in-center">
                                                        <KTCC:KTRadioButtonList ID="rblModelType" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" CssClass="rbl-default rbl-horizontal" AutoPostBack="true" OnSelectedIndexChanged="rblModelType_SelectedIndexChanged"></KTCC:KTRadioButtonList>
                                                    </div>
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
                                                            <td style="width: 70px">製品機番</td>
                                                            <td>
                                                                <KTCC:KTTextBox ID="txtProductSerial" runat="server" InputMode="AlphaNum" AutoUpper="true" MaxLength="7" CssClass="font-default txt-default ime-disabled txt-width-short" />
                                                            </td>
                                                        </tr>
                                                        <tr class="font-default">
                                                            <td style="width: 70px">IDNO</td>
                                                            <td>
                                                                <KTCC:KTTextBox ID="txtIDNO" runat="server" InputMode="IntNum" MaxLength="7" CssClass="font-default txt-default ime-disabled txt-width-short" />
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
                                    <div class="condition-in-box-main-adjusted">
                                        <table border="0">
                                            <tr>
                                                <td>
                                                    <table class="table-border-layout" style="width: 970px;">
                                                        <tr class="tr-fix-zero-height">
                                                            <td style="width: 320px"></td>
                                                            <td style="width: 270px"></td>
                                                            <td style="width: 265px"></td>
                                                            <td style="width: 115px"></td>
                                                        </tr>
                                                        <tr class="font-default tr-condition-header ui-state-default">
                                                            <td>工程/部品区分</td>
                                                            <td>部品</td>
                                                            <td>日付</td>
                                                            <td>PINコード</td>
                                                        </tr>
                                                        <tr class="tr-condition-body">
                                                            <td>
                                                                <table class="table-condition-sub">
                                                                    <tr class="font-default">
                                                                        <td style="width: 40px">工程</td>
                                                                        <td style="display: inline-block">
                                                                            <asp:HiddenField runat="server" ID="hdnLineCd" Value="" />
                                                                            <asp:HiddenField runat="server" ID="hdnProcessCd" Value="" />
                                                                            <asp:HiddenField runat="server" ID="hdnWorkCd" Value="" />
                                                                            <asp:HiddenField runat="server" ID="hdnSearchTargetFlg" Value="" />
                                                                            <asp:HiddenField runat="server" ID="hdnProcessNm" Value="" />
                                                                            <asp:HiddenField runat="server" ID="hdnWorkNm" Value="" />
                                                                            <KTCC:KTButton ID="btnChangeProcess" runat="server" OnClick="btnChangeProcess_Click" Style="display: none;" />
                                                                            <KTCC:KTTextBox ID="txtProcessNm" runat="server" InputMode="All" AutoUpper="false" Enabled="false" MaxLength="20" CssClass="font-default txt-default txt-width-long" />
                                                                            <button type="button" id="btnProcessFiltering" onclick="MainView.ShowProcessFilteringView()" style="vertical-align: middle;" class="btn-icon ui-icon ui-icon-search"></button>
                                                                            <button type="button" id="btnClearProcessAndWork" onclick="MainView.ClearProcessAndWork()" style="vertical-align: middle;">クリア</button>
                                                                        </td>
                                                                    </tr>
                                                                    <tr class="font-default">
                                                                        <td style="width: 40px">作業</td>
                                                                        <td>
                                                                            <KTCC:KTTextBox ID="txtWorkNm" runat="server" InputMode="All" AutoUpper="false" Enabled="false" MaxLength="20" CssClass="font-default txt-default txt-width-long" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr class="font-default">
                                                                        <td style="width: 40px">部品</td>
                                                                        <td>
                                                                            <KTCC:KTDropDownList ID="ddlParts" runat="server" CssClass="font-default ddl-default ddl-width-long" AutoPostBack="true" OnSelectedIndexChanged="ddlParts_SelectedIndexChanged" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                            <td>
                                                                <div>
                                                                    <div class="condition-tr-height" />
                                                                    <table class="table-condition-sub">
                                                                        <tr class="font-default">
                                                                            <td style="width: 70px">品番</td>
                                                                            <td>
                                                                                <KTCC:KTTextBox ID="txtPartsNo" runat="server" InputMode="RegExp" RegExpression="[-A-Za-z0-9]+" AutoUpper="true" MaxLength="10" CssClass="font-default txt-default ime-disabled txt-width-middle" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr class="font-default">
                                                                            <td style="width: 70px">部品機番</td>
                                                                            <td>
                                                                                <KTCC:KTTextBox ID="txtPartsSerial" runat="server" InputMode="HalfKana" AutoUpper="true" MaxLength="30" CssClass="font-default txt-default ime-inactive txt-width-long" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                            </td>
                                                            <td>
                                                                <div class="condition-tr-height" />
                                                                <table class="table-condition-sub">
                                                                    <tr class="font-default">
                                                                        <td style="width: 40px">対象</td>
                                                                        <td>
                                                                            <KTCC:KTDropDownList ID="ddlDateKind" runat="server" CssClass="font-default ddl-default ddl-width-long" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr class="font-default">
                                                                        <td style="width: 40px">範囲</td>
                                                                        <td>
                                                                            <div>
                                                                                <KTCC:KTCalendar ID="cldStart" runat="server" UseCalendar="false" Type="yyyyMMdd" CssClass="font-default txt-default cld-ymd" />～<KTCC:KTCalendar ID="cldEnd" runat="server" UseCalendar="false" Type="yyyyMMdd" CssClass="font-default txt-default cld-ymd" />
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                            <td>
                                                                <div class="condition-tr-height" />
                                                                <table class="table-condition-sub">
                                                                    <tr class="font-default">
                                                                        <td style="width: 80px">PINコード有</td>
                                                                        <td>
                                                                            <input type="checkbox" runat="server" name="chkPinCd" id="chkPinCd" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                        </table>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <div style="clear: both;"></div>
                        <div class="condition-button-area">
                            <KTCC:KTButton ID="btnMassDataOutput" runat="server" Text="検索[Excel出力]" CssClass="btn-long" OnClick="btnMassDataOutput_Click" Visible="False" />
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
                                    <asp:TemplateField HeaderText="productStatus" />
                                    <asp:TemplateField HeaderText="assemblyPatternNm" />
                                    <asp:TemplateField HeaderText="productModelCdStr" />
                                    <asp:TemplateField HeaderText="countryCd" />
                                    <asp:TemplateField HeaderText="productModelNm" />
                                    <asp:TemplateField HeaderText="serial" />
                                    <asp:TemplateField HeaderText="pinCd" />
                                    <asp:TemplateField HeaderText="idno" />
                                    <%-- 非表示項目 --%>
                                    <asp:TemplateField HeaderText="assemblyPatternCd" />
                                    <asp:TemplateField HeaderText="productModelCd" />
                                    <asp:TemplateField HeaderText="modelCd" />
                                    <asp:TemplateField HeaderText="productCountryCd" />
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
                                <%-- 右グリッドは検索条件によって動的に処理させる --%>
                                <Columns>
                                    <asp:TemplateField HeaderText="planDt" />
                                    <asp:TemplateField HeaderText="productDt" />
                                    <asp:TemplateField HeaderText="stockWarehouseCd" />
                                    <asp:TemplateField HeaderText="preAlterationDetail" />
                                    <asp:TemplateField HeaderText="preAlterationModelCd" />
                                    <asp:TemplateField HeaderText="preAlterationCountryCd" />
                                    <asp:TemplateField HeaderText="shippedDt" />
                                    <asp:TemplateField HeaderText="shippingVoucherNum" />
                                    <asp:TemplateField HeaderText="customerNm" />
                                    <asp:TemplateField HeaderText="deliveryDestinationNm" />
                                    <asp:TemplateField HeaderText="deliveryPrefectureNm" />
                                    <asp:TemplateField HeaderText="branchOfficeNm" />
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
        <KTCC:KTButton ID="btnExcel" runat="server" Text="Excel出力" CssClass="btn-middle" OnClick="btnExcel_Click" OnClientClick="SubmitControl.SetLoadingType(SubmitControl.TYPE_NONE);" />
    </div>
</asp:Content>
