<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/UI/MasterForm/MasterMain.master" CodeBehind="MasterMainteDpfList.aspx.cs" Inherits="TRC_W_PWT_ProductView.UI.Pages.MasterMainteDpfList" %>
<%@ Import Namespace="TRC_W_PWT_ProductView.Common" %>

<asp:Content ID="MasterScript" runat="server" ContentPlaceHolderID="MasterScript" >
    <script src="<%: PageUtils.ResolveUrl( this.Page, "~/Scripts/Pages/Maintenance/MasterMainteDpfList.js") %>"></script>
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
                                    <div class="condition-in-box">
                                        <table class="table-border-layout" style="width: 700px">
                                            <tr class="tr-fix-zero-height">
                                                <td style="width:100px"></td>
                                                <td style="width:100px"></td>
                                                <td style="width:270px"></td>
                                                <td style="width:230px"></td>
                                            </tr>
                                            <tr class="font-default tr-condition-header ui-state-default">
                                                <td>取付場所</td>
                                                <td>検索区分</td>
                                                <td>型式</td>
                                                <td>機番/IDNO</td>
                                            </tr>
                                            <tr class="tr-condition-body">
                                                <td>
                                                    <KTCC:KTRadioButtonList ID="rblProductKind" runat="server" RepeatDirection="Vertical" CssClass="rbl-default" AutoPostBack="true" OnSelectedIndexChanged="rblProductKind_SelectedIndexChanged"></KTCC:KTRadioButtonList>
                                                </td>
                                                <td>
                                                    <KTCC:KTRadioButtonList ID="rblSearchKbn" runat="server" RepeatDirection="Vertical" CssClass="rbl-default" AutoPostBack="true" OnSelectedIndexChanged="rblSearchKbn_SelectedIndexChanged"></KTCC:KTRadioButtonList>
                                                </td>
                                                <td>
                                                    <div>
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
                                                    </div>
                                                </td>
                                                <td>
                                                    <table class="table-condition-sub">
                                                        <tr class="font-default">
                                                            <td style="width: 40px">機番</td>
                                                            <td>
                                                                <KTCC:KTTextBox ID="txtProductSerial" runat="server" InputMode="AlphaNum" AutoUpper="true" MaxLength="7" CssClass="font-default txt-default ime-disabled txt-width-long" />
                                                            </td>
                                                        </tr>
                                                        <tr class="font-default">
                                                            <td style="width: 40px">IDNO</td>
                                                            <td>
                                                                <KTCC:KTTextBox ID="txtIDNO" runat="server" InputMode="IntNum" MaxLength="7" CssClass="font-default txt-default ime-disabled txt-width-short" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div class="condition-in-box">
                                        <table class="table-border-layout" style="width: 600px">
                                            <tr class="tr-fix-zero-height">
                                                <td style="width:150px"></td>
                                                <td style="width:300px"></td>
                                                <td style="width:180px"></td>
                                            </tr>
                                            <tr class="font-default tr-condition-header ui-state-default">
                                                <td>部品名</td>
                                                <td>日付</td>
                                                <td>登録情報</td>
                                            </tr>
                                            <tr class="tr-condition-body">
                                                <td>
                                                    <div>
                                                        <div class="condition-tr-height" />
                                                        <table class="table-condition-sub">
                                                            <tr class="font-default">
                                                                <td style="width: 40px">部品</td>
                                                                <td>
                                                                    <KTCC:KTDropDownList ID="ddlParts" runat="server" CssClass="font-default ddl-default ddl-width-short" AutoPostBack="true" OnSelectedIndexChanged="ddlParts_SelectedIndexChanged" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </td>
                                                <td>
                                                    <table class="table-condition-sub">
                                                      <tr class="font-default">
                                                            <td style="width: 80px">完成予定日</td>
                                                            <td>
                                                                <div>
                                                                    <KTCC:KTCalendar ID="cldCompStart" runat="server" UseCalendar="false" Type="yyyyMMdd" CssClass="font-default txt-default cld-ymd" />～<KTCC:KTCalendar ID="cldCompEnd" runat="server" UseCalendar="false" Type="yyyyMMdd" CssClass="font-default txt-default cld-ymd" />
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr class="font-default">
                                                            <td style="width: 60px">取付日</td>
                                                            <td>
                                                                <div>
                                                                    <KTCC:KTCalendar ID="cldAssemblyStart" runat="server" UseCalendar="false" Type="yyyyMMdd" CssClass="font-default txt-default cld-ymd" />～<KTCC:KTCalendar ID="cldAssemblyEnd" runat="server" UseCalendar="false" Type="yyyyMMdd" CssClass="font-default txt-default cld-ymd" />
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <div style="display:none">
                                                        <asp:TextBox ID="txtLineCd" runat="server"/>
                                                        <asp:TextBox ID="txtST" runat="server"/>
                                                    </div>
                                                </td>
                                                <td>
                                                    <table class="table-condition-sub">
                                                        <tr class="font-default">
                                                            <td style="width: 70px">抜取検査</td>
                                                            <td>
                                                                <KTCC:KTDropDownList ID="ddlCheckSample" runat="server" CssClass="font-default ddl-default ddl-width-short" AutoPostBack="true" OnSelectedIndexChanged="ddlCheckSample_SelectedIndexChanged" />
                                                            </td>
                                                        </tr>
                                                        <tr class="font-default">
                                                            <td style="width: 70px">登録種別</td>
                                                            <td>
                                                                <KTCC:KTDropDownList ID="ddlRegKind" runat="server" CssClass="font-default ddl-default ddl-width-short" AutoPostBack="true" OnSelectedIndexChanged="ddlRegKind_SelectedIndexChanged" />
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
                        <div style="clear: both"></div>
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
                    <div class="div-result-pager">
                        <asp:Panel ID="pnlPager" runat="server" EnableViewState="true"></asp:Panel>
                    </div>
                    <div id="divgrvCount" class="div-result-count">
                        <span>件数：</span>
                        <KTCC:KTNumericTextBox ID="ntbResultCount" runat="server" CssClass="txt-center-num" ReadOnly="true" />
                        <span>件</span>
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
                                    <asp:TemplateField HeaderText="LINE_CD" />
                                    <asp:TemplateField HeaderText="ST" />
                                    <asp:TemplateField HeaderText="ATTACHMENT_DT" />
                                    <asp:TemplateField HeaderText="SAMPLE_CHECK" />
                                    <asp:TemplateField HeaderText="DPF_MODEL_CD" />
                                    <asp:TemplateField HeaderText="DPF_SERIAL" />
                                    <asp:TemplateField HeaderText="JUN_NO" />
                                    <asp:TemplateField HeaderText="KAN_YO_YM" />
                                    <asp:TemplateField HeaderText="DATA_CNT" />
                                    <asp:TemplateField HeaderText="PTN_CD" />
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

