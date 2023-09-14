<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/UI/MasterForm/MasterMain.master" CodeBehind="MasterMainte3CList.aspx.cs" Inherits="TRC_W_PWT_ProductView.UI.Pages.MasterMainte3CList" %>
<%@ Import Namespace="TRC_W_PWT_ProductView.Common" %>
<%@ Register src="~/UI/Pages/UserControl/InputModal.ascx" tagname="InputModal" tagprefix="UC" %>

<asp:Content ID="MasterScript" runat="server" ContentPlaceHolderID="MasterScript" >
    <script src="<%: PageUtils.ResolveUrl( this.Page, "~/Scripts/Pages/Maintenance/MasterMainte3CList.js") %>"></script>
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
                                        <table class="table-border-layout" style="width: 600px">
                                            <tr class="tr-fix-zero-height">
                                                <td style="width:320px"></td>
                                                <td style="width:500px"></td>
                                            </tr>
                                            <tr class="font-default tr-condition-header ui-state-default">
                                                <td>製品区分</td>
                                                <td>日付</td>
                                            </tr>
                                            <tr class="tr-condition-body">
                                                <td>

                                                    <table class="table-condition-sub">
                                                        <tr class="font-default">
                                                            <td style="width: 100px">製品区分</td>
                                                            <td>
                                                                <div>
                                                                    <KTCC:KTDropDownList ID="ddl3C" runat="server" CssClass="font-default ddl-default ddl-width-short" AutoPostBack="true"/>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr class="font-default">
                                                            <td style="width: 100px">エンジン種別</td>
                                                            <td>
                                                                <div>
                                                                    <KTCC:KTDropDownList ID="ddlEngineKind" runat="server" CssClass="font-default ddl-default ddl-width-middle" AutoPostBack="true"/>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td>
                                                    <table class="table-condition-sub">
                                                        <tr class="font-default">
                                                            <td style="width: 60px">加工日</td>
                                                            <td>
                                                                <div>
                                                                    <KTCC:KTCalendar ID="cldStart" runat="server" UseCalendar="false" Type="yyyyMMdd" CssClass="font-default txt-default cld-ymd" />～<KTCC:KTCalendar ID="cldEnd" runat="server" UseCalendar="false" Type="yyyyMMdd" CssClass="font-default txt-default cld-ymd" />
                                                                </div>
                                                            </td>
                                                            <td>
                                                                <div class="condition-tr-height box-in-center" style="height:20px" />
                                                                    <asp:CheckBox ID="chkTarget" runat="server" Class="font-default ddl-default ddl-width-short" AutoPostBack="true" Text="加工日「999999」のみを検索" OnCheckedChanged="chkTarget_CheckedChanged"/>
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
                                                        <KTCC:KTTextBox ID="txtparamDt" runat="server"/>
                                                        <KTCC:KTTextBox ID="txtparamNum" runat="server"/>
                                                        <KTCC:KTTextBox ID="txtparamLine" runat="server"/>
                                                        <KTCC:KTTextBox ID="txtparamRemark" runat="server"/>
                                                    </div>
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
                            <asp:GridView ID="grvHeaderLT" runat="server" CssClass="grid-layout2 ui-widget-content" AllowPaging="false" AllowSorting="true" PageSize="1000" AutoGenerateColumns="false" OnSorting="grvMainView_Sorting">
                                <HeaderStyle CssClass="grid-header ui-state-default" />
                            </asp:GridView>
                        </div>
                    </div>
                </td>
                <td>
                    <div id="divRTScroll" class="div-scroll-right-top div-right-grid">
                        <div id="divGrvHeaderRT" runat="server">
                            <asp:GridView ID="grvHeaderRT" runat="server" CssClass="grid-layout2 ui-widget-content" AllowPaging="false" AllowSorting="true" PageSize="1000" AutoGenerateColumns="false" OnSorting="grvMainView_Sorting">
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
                            <asp:GridView ID="grvMainViewLB" runat="server" ShowHeader="false" CssClass="grid-layout2 ui-widget-content" AllowPaging="true" AllowSorting="false" PageSize="1000" AutoGenerateColumns="false" OnRowDataBound="grvMainViewLB_RowDataBound">
                                <PagerSettings Position="TopAndBottom" Mode="NumericFirstLast" PageButtonCount="10" Visible="false" />
                                <HeaderStyle CssClass="grid-header ui-state-default" />
                                <RowStyle CssClass="grid-row ui-widget" />
                                <SelectedRowStyle CssClass="ui-state-highlight" />
                                <Columns>
                                    <asp:TemplateField HeaderText="ﾁｪｯｸ">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkUpdate" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="PARTS_CD" />
                                    <asp:TemplateField HeaderText="PARTS_NM" />
                                    <asp:TemplateField HeaderText="PROC_DT" />
                                    <asp:TemplateField HeaderText="NEW_YMD" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </td>
                <td>
                    <div id="divRBScroll" class="div-visible-scroll div-right-grid">
                        <div id="divGrvRB" runat="server">
                            <asp:GridView ID="grvMainViewRB" runat="server" ShowHeader="false" CssClass="grid-layout2 ui-widget-content" AllowPaging="true" AllowSorting="false" PageSize="1000" AutoGenerateColumns="false" OnRowDataBound="grvMainViewRB_RowDataBound">
                                <PagerSettings Position="TopAndBottom" Mode="NumericFirstLast" PageButtonCount="10" Visible="false" />
                                <HeaderStyle CssClass="grid-header ui-state-default" />
                                <RowStyle CssClass="grid-row ui-widget" />
                                <SelectedRowStyle CssClass="ui-state-highlight" />
                                <Columns>
                                    <asp:TemplateField HeaderText="PROC_NUM" />
                                    <asp:TemplateField HeaderText="MODEL_CD" />
                                    <asp:TemplateField HeaderText="MODEL_NM" />
                                    <asp:TemplateField HeaderText="COUNTRY" />
                                    <asp:TemplateField HeaderText="SERIAL_NO" />
                                    <asp:TemplateField HeaderText="PROCESSING_LINE" />
                                    <asp:TemplateField HeaderText="REMARKS" />
                                    <asp:TemplateField HeaderText="UPDATE_BY" />
                                    <%-- 非表示項目 --%>                        
                                    <asp:TemplateField HeaderText="ST" />
                                    <asp:TemplateField HeaderText="PARTS_CD_ORG" />                                    
                                     
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <%-- iframeの呼び出し --%>
    <div id="dialog" class="frame-area">
        <uc:InputModal id="InputModal" runat="server"/>
    </div>
</asp:Content>

<asp:Content ID="MasterBodyBottom" ContentPlaceHolderID="MasterBodyBottom" runat="server" >
    <div>
        <div id="divGrvCtrlButton" runat="server" class="div-bottom-button-area">
            <KTCC:KTButton ID="btnModalDisp"   runat="server" Text="修正" CssClass="btn-middle" />
        </div>
        <div style="display:none">
            <KTCC:KTButton ID="btnUpdate" runat="server" OnClick="btnUpdate_Click"/>
        </div>
    </div>
</asp:Content>

