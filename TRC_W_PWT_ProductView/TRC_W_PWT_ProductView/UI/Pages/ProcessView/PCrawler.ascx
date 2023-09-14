<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PCrawler.ascx.cs" Inherits="TRC_W_PWT_ProductView.UI.Pages.ProcessView.PCrawler" %>
<%@ Import Namespace="TRC_W_PWT_ProductView.Common" %>

<%-- デザイン表示時使用 マスターページ使用時不要 --%>
<%--<link href="../../../CSS/cobalt/jquery-wijmo.css" rel="stylesheet" />
<link href="../../../CSS/Base.css" rel="stylesheet" />
<link href="../../../CSS/TRC.css" rel="stylesheet" />--%>

<%-- 画面CSS/スクリプト定義領域 --%>
<asp:PlaceHolder ID="SubScripts" runat="server">
    <script src="<%: PageUtils.ResolveUrl( this.Page, "~/Scripts/Pages/ProcessView/PCrawler.js") %>" ></script>
</asp:PlaceHolder>

<div class="div-detail-info-margin">
    <div id="divDetailBodyScroll" class="div-fix-scroll">
        <div class="div-detail-table-title">■来歴情報</div>
        <div style="margin-top:10px;"/>
        <div id="divMainListArea" style="height: 150px">
            <table class="table-layout-fix" style="margin-left:10px">
                <tr>
                    <td>
                        <div id="divLTScroll" class="div-fix-scroll div-left-grid">
                            <div id="divHeaderLT" runat="server">
                                <table id="solidLTHeader" runat="server" class="grid-layout" style="width: 220px;">
                                    <tr id="headerLTMainContent"      runat="server" class="listview-header_2r ui-state-default">
                                        <th id="Th20"  runat="server" style="width:150px;">検査日時</th>
                                        <th id="Th21"  runat="server" style="width: 70px;">結果</th>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </td>
                    <td>
                        <div id="divRTScroll" class="div-scroll-right-top div-right-grid">
                            <div id="divHeaderRT" runat="server">
                                <table id="solidRTHeader" runat="server" class="grid-layout" style="width: 820px;">
                                    <tr id="headerRTMainContent"    runat="server" class="listview-header_2r ui-state-default">
                                        <th id="Th15"  runat="server" style="width:100px;">検査員</th>
                                        <th id="Th16"  runat="server" style="width:160px;">検査グループ</th>
                                        <th id="Th17"  runat="server" style="width: 70px;">モンロー</th>
                                        <th id="Th18"  runat="server" style="width: 70px;">PTO</th>
                                        <th id="Th19"  runat="server" style="width: 70px;">油圧</th>
                                        <th id="Th22"  runat="server" style="width: 70px;">ライト</th>
                                        <th id="Th23"  runat="server" style="width: 70px;">キー<br />ストップ</th>
                                        <th id="Th24"  runat="server" style="width: 70px;">ハンドル<br />締付</th>
                                        <th id="Th11"  runat="server" style="width: 70px;">走行</th>
                                        <th id="Th12"  runat="server" style="width: 70px;">異音</th>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div id="divLBScroll" class="div-scroll-left-bottom div-left-grid">
                            <div id="divGrvLB" runat="server">
                                <asp:ListView ID="lstMainListLB" runat="server" OnItemDataBound="lstMainListLB_ItemDataBound" OnSelectedIndexChanging="lstMainListLB_SelectedIndexChanging" OnSelectedIndexChanged="lstMainListLB_SelectedIndexChanged">
                                    <LayoutTemplate>
                                        <table id="itemPlaceholderContainerLB" runat="server" class="grid-layout" style="width:220px">
                                            <tr id="headerLBContent"   runat="server" class="listview-header_3r ui-state-default">
                                                <th id="Th1" 　runat="server" style="width:150px;"/>
                                                <th id="Th2"   runat="server" style="width: 70px;"/>
                                            </tr>
                                            <tr id="itemPlaceholder" runat="server">
                                            </tr>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr id="trRowData" runat="server" class="listview-row ui-widget">
                                            <td>
                                                <asp:Button ID="btnSelect" runat="server" CommandName="Select" Text="" CssClass="btnSelect invisible" />
                                                <KTCC:KTTextBox ID="txtInspectionDt" runat="server" CssClass="font-default txt-default txt-width-full al-ct" ReadOnly="true"></KTCC:KTTextBox>
                                            </td>
                                            <td>
                                                <KTCC:KTTextBox ID="txtResult" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:ListView>
                            </div>
                        </div>
                    </td>
                    <td>
                        <div id="divRBScroll" class="div-visible-scroll div-right-grid">
                            <div id="divGrvRB" runat="server">
                                <asp:ListView ID="lstMainListRB" runat="server" OnItemDataBound="lstMainListRB_ItemDataBound" OnSelectedIndexChanging="lstMainListRB_SelectedIndexChanging" OnSelectedIndexChanged="lstMainListRB_SelectedIndexChanged">
                                    <LayoutTemplate>
                                        <table id="itemPlaceholderContainerRB" runat="server" class="grid-layout" style="width:820px">
                                            <tr id="headerRBContent"   runat="server" class="listview-header_3r ui-state-default">
                                                <th id="Th15"  runat="server" style="width:100px;"/>
                                                <th id="Th16"  runat="server" style="width:160px;"/>
                                                <th id="Th17"  runat="server" style="width: 70px;"/>
                                                <th id="Th18"  runat="server" style="width: 70px;"/>
                                                <th id="Th19"  runat="server" style="width: 70px;"/>
                                                <th id="Th22"  runat="server" style="width: 70px;"/>
                                                <th id="Th23"  runat="server" style="width: 70px;"/>
                                                <th id="Th24"  runat="server" style="width: 70px;"/>
                                                <th id="Th11"  runat="server" style="width: 70px;"/>
                                                <th id="Th12"  runat="server" style="width: 70px;"/>
                                            </tr>
                                            <tr id="itemPlaceholder" runat="server">
                                            </tr>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr id="trRowData" runat="server" class="listview-row ui-widget">
                                            <td>
                                                <asp:Button ID="btnSelect" runat="server" CommandName="Select" Text="" CssClass="btnSelect invisible" />
                                                <KTCC:KTTextBox ID="txtEmployeeCd" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" ></KTCC:KTTextBox>
                                            </td>
                                            <td>
                                                <KTCC:KTTextBox ID="txtInspectionGroup" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lt" />
                                            </td>
                                            <td>
                                                <KTCC:KTTextBox ID="txtMonroe" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                            </td>
                                            <td>
                                                <KTCC:KTTextBox ID="txtPTO" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                            </td>
                                            <td>
                                                <KTCC:KTTextBox ID="txtHydraulic" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                            </td>
                                            <td>
                                                <KTCC:KTTextBox ID="txtHeadLight" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                            </td>
                                            <td>
                                                <KTCC:KTTextBox ID="txtKeyStop" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                            </td>
                                            <td>
                                                <KTCC:KTTextBox ID="txtSteeringTighten" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                            </td>
                                            <td>
                                                <KTCC:KTTextBox ID="txtSpeed" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                            </td>
                                            <td>
                                                <KTCC:KTTextBox ID="txtAbnormalNoise" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:ListView>
                            </div>
                        </div>
                    </td>
                </tr>
            </table>


            <%-- 

            <div id="divMainListArea" class="div-y-scroll-flt2" >
                <asp:ListView ID="lstMainList" runat="server" OnItemDataBound="lstMainList_ItemDataBound" OnSelectedIndexChanging="lstMainList_SelectedIndexChanging" OnSelectedIndexChanged="lstMainList_SelectedIndexChanged">
                    <LayoutTemplate>
                        <table id="itemPlaceholderContainer" runat="server" class="grid-layout" style="width: 1040px; height:auto">
                            <tr id="headerMainContent" runat="server" class="listview-header_3r ui-state-default">
                                <th id="Th1"  runat="server" style="width:150px;"/>
                                <th id="Th2"  runat="server" style="width: 70px;"/>
                                <th id="Th3"  runat="server" style="width:100px;"/>
                                <th id="Th4"  runat="server" style="width:160px;"/>
                                <th id="Th5"  runat="server" style="width: 70px;"/>
                                <th id="Th6"  runat="server" style="width: 70px;"/>
                                <th id="Th7"  runat="server" style="width: 70px;"/>
                                <th id="Th8"  runat="server" style="width: 70px;"/>
                                <th id="Th9"  runat="server" style="width: 70px;"/>
                                <th id="Th10" runat="server" style="width: 70px;"/>
                                <th id="Th11" runat="server" style="width: 70px;"/>
                                <th id="Th12" runat="server" style="width: 70px;"/>
                            </tr>
                            <tr id="itemPlaceholder" runat="server">
                            </tr>
                        </table>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr id="trRowData" runat="server" class="listview-row ui-widget">
                            <td>
                                <asp:Button ID="btnSelect" runat="server" CommandName="Select" Text="" CssClass="btnSelect invisible" />
                                <KTCC:KTTextBox ID="txtInspectionDt" runat="server" CssClass="font-default txt-default txt-width-full al-ct" ReadOnly="true"></KTCC:KTTextBox>
                            </td>
                            <td>
                                <KTCC:KTTextBox ID="txtResult" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                            </td>
                            <td>
                                <KTCC:KTTextBox ID="txtEmployeeCd" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                            </td>
                            <td>
                                <KTCC:KTTextBox ID="txtInspectionGroup" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lt" />
                            </td>
                            <td>
                                <KTCC:KTTextBox ID="txtMonroe" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                            </td>
                            <td>
                                <KTCC:KTTextBox ID="txtPTO" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                            </td>
                            <td>
                                <KTCC:KTTextBox ID="txtHydraulic" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                            </td>
                            <td>
                                <KTCC:KTTextBox ID="txtHeadLight" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                            </td>
                            <td>
                                <KTCC:KTTextBox ID="txtKeyStop" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                            </td>
                            <td>
                                <KTCC:KTTextBox ID="txtSteeringTighten" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                            </td>
                            <td>
                                <KTCC:KTTextBox ID="txtSpeed" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                            </td>
                            <td>
                                <KTCC:KTTextBox ID="txtAbnormalNoise" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:ListView>
                --%>
            </div>
        </div>
        <div class="div-detail-table-title">■走行検査測定結果</div>
        <div style="clear: both; height: 10px;width:auto"></div>
        <div id="divGrvSubDisplay" runat="server" style="margin-left:10px">
            <table class="table-layout-fix">
                <tr>
                    <td>
                        <div id="divLTSubScroll" class="div-fix-scroll div-left-grid">
                            <div id="divGrvSubHeaderLT" runat="server">
                                <asp:GridView ID="grvSubHeaderLT" runat="server" CssClass="grid-layout2 ui-widget-content" AllowPaging="false" AllowSorting="false" PageSize="100" AutoGenerateColumns="false">
                                    <HeaderStyle CssClass="grid-header-newline ui-state-default" />
                                </asp:GridView>
                            </div>
                        </div>
                    </td>
                    <td>
                        <div id="divRTSubScroll" class="div-scroll-right-top div-right-grid">
                            <div id="divGrvSubHeaderRT" runat="server">
                                <asp:GridView ID="grvSubHeaderRT" runat="server" CssClass="grid-layout2 ui-widget-content" AllowPaging="false" AllowSorting="false" PageSize="100" AutoGenerateColumns="false">
                                    <HeaderStyle CssClass="grid-header-newline ui-state-default" />
                                </asp:GridView>
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div id="divLBSubScroll" class="div-scroll-left-bottom div-left-grid">
                            <div id="divGrvSubLB" runat="server">
                                <asp:GridView ID="grvSubViewLB" runat="server" ShowHeader="false" CssClass="grid-layout2 ui-widget-content" AllowPaging="false" AllowSorting="false" PageSize="1000" AutoGenerateColumns="false" OnRowDataBound="grvSubViewLB_RowDataBound">
                                    <HeaderStyle CssClass="grid-header ui-state-default" />
                                    <RowStyle CssClass="grid-row ui-widget" />
                                    <SelectedRowStyle CssClass="ui-state-highlight" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="inspectionSeq" />
                                        <asp:TemplateField HeaderText="result" />
                                        <asp:TemplateField HeaderText="standardWheelDrive" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </td>
                    <td>
                        <div id="divRBSubScroll" class="div-visible-scroll div-right-grid">
                            <div id="divGrvSubRB" runat="server">
                                <asp:GridView ID="grvSubViewRB" runat="server" ShowHeader="false" CssClass="grid-layout2 ui-widget-content" AllowPaging="false" AllowSorting="false" PageSize="1000" AutoGenerateColumns="false" OnRowDataBound="grvSubViewRB_RowDataBound">
                                    <HeaderStyle CssClass="grid-header ui-state-default" />
                                    <RowStyle CssClass="grid-row ui-widget" />
                                    <SelectedRowStyle CssClass="ui-state-highlight" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="standardSubTransmission" />
                                        <asp:TemplateField HeaderText="standardMainTransmission" />
                                        <asp:TemplateField HeaderText="standardForwardReverseKind" />
                                        <asp:TemplateField HeaderText="measureValueLFrontWheel" />
                                        <asp:TemplateField HeaderText="measureValueRFrontWheel" />
                                        <asp:TemplateField HeaderText="measureValueLRearWheel" />
                                        <asp:TemplateField HeaderText="measureValueRRearWheel" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
        </div>

        <%-- 
        <div id="divSubListArea" class="div-auto-scroll">
            <asp:ListView ID="lstSubList" runat="server" OnItemDataBound="lstSubList_ItemDataBound">
                <LayoutTemplate>
                    <table id="itemPlaceholderContainer" runat="server" class="grid-layout" style="width: 729px; height:auto">
                        <tr id="headerMainContent" runat="server" class="listview-header_2r ui-state-default">
                            <th id="Th1"  runat="server" style="width: 90px;">検査順序</th>
                            <th id="Th2"  runat="server" style="width: 70px;">結果</th>
                            <th id="Th3"  runat="server" style="width: 70px;">2WD<br />4WD</th>
                            <th id="Th4"  runat="server" style="width: 70px;">副変速</th>
                            <th id="Th5"  runat="server" style="width: 70px;">主変速</th>
                            <th id="Th6"  runat="server" style="width: 70px;">前進<br />後進</th>
                            <th id="Th7"  runat="server" style="width: 70px;">前輪左<br />(km/h)</th>
                            <th id="Th8"  runat="server" style="width: 70px;">前輪右<br />(km/h)</th>
                            <th id="Th9"  runat="server" style="width: 70px;">後輪左<br />(km/h)</th>
                            <th id="Th10" runat="server" style="width: 70px;">後輪右<br />(km/h)</th>
                        </tr>
                        <tr class="" id="itemPlaceholder" runat="server">
                        </tr>
                    </table>
                </LayoutTemplate>
                <ItemTemplate>
                    <tr id="trRowData" runat="server" class="listview-row ui-widget">
                        <td>
                            <KTCC:KTNumericTextBox ID="ntbInspectionSeq" runat="server" CssClass="font-default txt-default txt-numeric txt-width-full" ReadOnly="true" />
                        </td>
                        <td>
                            <KTCC:KTTextBox ID="txtResult" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                        </td>
                        <td>
                            <KTCC:KTTextBox ID="txtStandardWheelDrive" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                        </td>
                        <td>
                            <KTCC:KTTextBox ID="txtStandardSubTransmission" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                        </td>
                        <td>
                            <KTCC:KTTextBox ID="txtStandardMainTransmission" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                        </td>
                        <td>
                            <KTCC:KTTextBox ID="txtStandardForwardReverseKind" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                        </td>
                        <td>
                            <KTCC:KTDecimalTextBox ID="ntbMeasureValueLFrontWheel" DecLen="1" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-numeric txt-width-full" />
                        </td>
                        <td>
                            <KTCC:KTDecimalTextBox ID="ntbMeasureValueRFrontWheel" DecLen="1" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-numeric txt-width-full" />
                        </td>
                        <td>
                            <KTCC:KTDecimalTextBox ID="ntbMeasureValueLRearWheel" DecLen="1" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-numeric txt-width-full" />
                        </td>
                        <td>
                            <KTCC:KTDecimalTextBox ID="ntbMeasureValueRRearWheel" DecLen="1" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-numeric txt-width-full" />
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:ListView>
        </div>
        --%>
    </div>
</div>