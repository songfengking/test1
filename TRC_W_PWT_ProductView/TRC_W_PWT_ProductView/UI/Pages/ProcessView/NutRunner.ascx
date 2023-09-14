<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NutRunner.ascx.cs" Inherits="TRC_W_PWT_ProductView.UI.Pages.ProcessView.NutRunner" %>
<%@ Import Namespace="TRC_W_PWT_ProductView.Common" %>

<%-- デザイン表示時使用 マスターページ使用時不要 --%>
<%--<link href="../../../CSS/cobalt/jquery-wijmo.css" rel="stylesheet" />
<link href="../../../CSS/Base.css" rel="stylesheet" />
<link href="../../../CSS/TRC.css" rel="stylesheet" />--%>

<%-- 画面CSS/スクリプト定義領域 --%>
<asp:PlaceHolder ID="SubScripts" runat="server">
    <script src="<%: PageUtils.ResolveUrl( this.Page, "~/Scripts/Pages/ContentScroll.js") %>" ></script>
</asp:PlaceHolder>

<div class="div-detail-info-margin">
    <div id="divMainListArea" class="div-auto-scroll">
        <div class="div-detail-table-title">■締付情報</div>
        <table>
            <%-- フロント --%>
            <tr valign="top">
                <td>
                    <%-- フロント(左) --%>
                    <div id="divFL" style="height: 200px">
                        <table class="table-layout-fix" style="width:520px">
                            <tr>
                                <td>
                                    <div id="divFLTitle">
                                        <table id="tblFLTitle" >
                                            <tr>
                                                <th class="div-detail-table-title" style="width:190px;text-align:left">フロント(左)</th>
                                                <th class="div-table-subtitle" style="width:100px;text-align:right;">締付結果：</th>
                                                <th style="width: 50px">
                                                    <KTCC:KTTextBox ID="txtFLResult" runat="server" ReadOnly="true"  CssClass="div-table-subtitle" />
                                                </th>
                                            </tr>
                                        </table>
                                    </div>
                                    <div id="divFLScroll" class="div-scroll-right-top div-right-grid" style="margin-left:10px;">
                                        <div id="divFLHeader" runat="server">
                                            <table id="tblFLHeader" runat="server" class="grid-layout" style="width: 480px;">
                                                <tr id="FLtr" runat="server" class="listview-header_2r ui-state-default">
                                                    <th id="Th71" 　runat="server" style="width: 40px;">締付<br />回数</th>
                                                    <th id="Th72"   runat="server" style="width: 80px;">日時</th>
                                                    <th id="Th73" 　runat="server" style="width: 45px;">軸1<br />ﾄﾙｸ/<br />角度</th>
                                                    <th id="Th75"   runat="server" style="width: 45px;">軸2<br />ﾄﾙｸ/<br />角度</th>
                                                    <th id="Th77"   runat="server" style="width: 45px;">軸3<br />ﾄﾙｸ/<br />角度</th>
                                                    <th id="Th79"   runat="server" style="width: 45px;">軸4<br />ﾄﾙｸ/<br />角度</th>
                                                    <th id="Th89"   runat="server" style="width: 45px;">軸5<br />ﾄﾙｸ/<br />角度</th>
                                                    <th id="Th91"   runat="server" style="width: 45px;">軸6<br />ﾄﾙｸ/<br />角度</th>
                                                    <th id="Th93"   runat="server" style="width: 45px;">軸7<br />ﾄﾙｸ/<br />角度</th>
                                                    <th id="Th95"   runat="server" style="width: 45px;">軸8<br />ﾄﾙｸ/<br />角度</th>
                                                </tr>
                                            </table>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div id="divFLDScroll" class="div-scroll-right-top div-right-grid" style="height:136px;margin-left:10px;">
                                        <div id="divFLDHeader" runat="server">
                                            <asp:ListView ID="lstFL" runat="server" OnItemDataBound="lstFL_ItemDataBound">
                                                <LayoutTemplate>
                                                    <table id="itemPlaceholderContainerFL" runat="server" class="grid-layout" style="width: 480px;">
                                                        <tr id="headerMainContent" runat="server" class="listview-header_3r ui-state-default">
                                                            <th id="FL1" 　runat="server" style="width: 40px;"></th>
                                                            <th id="FL2"   runat="server" style="width: 80px;"></th>
                                                            <th id="FL3" 　runat="server" style="width: 45px;"></th>
                                                            <th id="FL4"   runat="server" style="width: 45px;"></th>
                                                            <th id="FL5"   runat="server" style="width: 45px;"></th>
                                                            <th id="FL6"   runat="server" style="width: 45px;"></th>
                                                            <th id="FL7"   runat="server" style="width: 45px;"></th>
                                                            <th id="FL8"   runat="server" style="width: 45px;"></th>
                                                            <th id="FL9"   runat="server" style="width: 45px;"></th>
                                                            <th id="FL10"  runat="server" style="width: 45px;"></th>
                                                            <th id="Th11"  runat="server" class="tr-fix-zero-height"/>
                                                            <th id="Th12"  runat="server" class="tr-fix-zero-height"/>
                                                            <th id="Th13"  runat="server" class="tr-fix-zero-height"/>
                                                            <th id="Th14"  runat="server" class="tr-fix-zero-height"/>
                                                            <th id="Th15"  runat="server" class="tr-fix-zero-height"/>
                                                            <th id="Th16"  runat="server" class="tr-fix-zero-height"/>
                                                            <th id="Th17"  runat="server" class="tr-fix-zero-height"/>
                                                            <th id="Th18"  runat="server" class="tr-fix-zero-height"/>
                                                            <th id="Th43"  runat="server" class="tr-fix-zero-height"/>
                                                            <th id="Th44"  runat="server" class="tr-fix-zero-height"/>
                                                            <th id="Th45"  runat="server" class="tr-fix-zero-height"/>
                                                            <th id="Th46"  runat="server" class="tr-fix-zero-height"/>
                                                            <th id="Th47"  runat="server" class="tr-fix-zero-height"/>
                                                            <th id="Th48"  runat="server" class="tr-fix-zero-height"/>
                                                            <th id="Th49"  runat="server" class="tr-fix-zero-height"/>
                                                            <th id="Th50"  runat="server" class="tr-fix-zero-height"/>
                                                        </tr>
                                                        <tr id="itemPlaceholder" runat="server">
                                                        </tr>
                                                    </table>
                                                </LayoutTemplate>
                                                <ItemTemplate>
                                                    <tr id="trRowData" runat="server" class="listview-row ui-widget" style="height:42px">
                                                        <td><KTCC:KTTextBox ID="txtBenchiNo"        runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" /></td>
                                                        <td><KTCC:KTTextBox ID="txtInspectionDt"    runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" /></td>
                                                        <td><KTCC:KTTextBox ID="txtShaft1"          runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-rg" /></td>
                                                        <td><KTCC:KTTextBox ID="txtShaft2"          runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-rg" /></td>
                                                        <td><KTCC:KTTextBox ID="txtShaft3"          runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-rg" /></td>
                                                        <td><KTCC:KTTextBox ID="txtShaft4"          runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-rg" /></td>
                                                        <td><KTCC:KTTextBox ID="txtShaft5"          runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-rg" /></td>
                                                        <td><KTCC:KTTextBox ID="txtShaft6"          runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-rg" /></td>
                                                        <td><KTCC:KTTextBox ID="txtShaft7"          runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-rg" /></td>
                                                        <td><KTCC:KTTextBox ID="txtShaft8"          runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-rg" /></td>
                                                        <td class="tr-fix-zero-height"><KTCC:KTTextBox ID="txtShaft1_bk" runat="server" ReadOnly="true"/></td>
                                                        <td class="tr-fix-zero-height"><KTCC:KTTextBox ID="txtShaft2_bk" runat="server" ReadOnly="true"/></td>
                                                        <td class="tr-fix-zero-height"><KTCC:KTTextBox ID="txtShaft3_bk" runat="server" ReadOnly="true"/></td>
                                                        <td class="tr-fix-zero-height"><KTCC:KTTextBox ID="txtShaft4_bk" runat="server" ReadOnly="true"/></td>
                                                        <td class="tr-fix-zero-height"><KTCC:KTTextBox ID="txtShaft5_bk" runat="server" ReadOnly="true"/></td>
                                                        <td class="tr-fix-zero-height"><KTCC:KTTextBox ID="txtShaft6_bk" runat="server" ReadOnly="true"/></td>
                                                        <td class="tr-fix-zero-height"><KTCC:KTTextBox ID="txtShaft7_bk" runat="server" ReadOnly="true"/></td>
                                                        <td class="tr-fix-zero-height"><KTCC:KTTextBox ID="txtShaft8_bk" runat="server" ReadOnly="true"/></td>
                                                        <td class="tr-fix-zero-height"><KTCC:KTTextBox ID="txtNr1" runat="server" ReadOnly="true"/></td>
                                                        <td class="tr-fix-zero-height"><KTCC:KTTextBox ID="txtNr2" runat="server" ReadOnly="true"/></td>
                                                        <td class="tr-fix-zero-height"><KTCC:KTTextBox ID="txtNr3" runat="server" ReadOnly="true"/></td>
                                                        <td class="tr-fix-zero-height"><KTCC:KTTextBox ID="txtNr4" runat="server" ReadOnly="true"/></td>
                                                        <td class="tr-fix-zero-height"><KTCC:KTTextBox ID="txtNr5" runat="server" ReadOnly="true"/></td>
                                                        <td class="tr-fix-zero-height"><KTCC:KTTextBox ID="txtNr6" runat="server" ReadOnly="true"/></td>
                                                        <td class="tr-fix-zero-height"><KTCC:KTTextBox ID="txtNr7" runat="server" ReadOnly="true"/></td>
                                                        <td class="tr-fix-zero-height"><KTCC:KTTextBox ID="txtNr8" runat="server" ReadOnly="true"/></td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:ListView>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
                <td>
                    <%-- フロント(右) --%>
                    <div id="divFR" style="height: 200px">
                        <table class="table-layout-fix" style="width:520px;margin-left:10px;">
                            <tr>
                                <td>
                                    <div id="divFRTitle">
                                        <table id="tblFRTitle">
                                            <tr>
                                                <th class="div-detail-table-title" style="width:190px;text-align:left">フロント(右)</th>
                                                <th class="div-table-subtitle" style="width:100px;text-align:right">締付結果：</th>
                                                <th style="width: 50px">
                                                    <KTCC:KTTextBox ID="txtFRResult" runat="server" ReadOnly="true"  CssClass="div-table-subtitle" />
                                                </th>
                                            </tr>
                                        </table>
                                    </div>
                                    <div id="divFRScroll" class="div-scroll-right-top div-right-grid" style="margin-left:10px;">
                                        <div id="divFRHeader" runat="server">
                                            <table id="tblFRHeader" runat="server" class="grid-layout" style="width: 480px;">
                                                <tr id="FRtr" runat="server" class="listview-header_2r ui-state-default">
                                                    <th id="Th1" 　runat="server" style="width: 40px;">締付<br />回数</th>
                                                    <th id="Th2"   runat="server" style="width: 80px;">日時</th>
                                                    <th id="Th3" 　runat="server" style="width: 45px;">軸1<br />ﾄﾙｸ/<br />角度</th>
                                                    <th id="Th4"   runat="server" style="width: 45px;">軸2<br />ﾄﾙｸ/<br />角度</th>
                                                    <th id="Th5"   runat="server" style="width: 45px;">軸3<br />ﾄﾙｸ/<br />角度</th>
                                                    <th id="Th6"   runat="server" style="width: 45px;">軸4<br />ﾄﾙｸ/<br />角度</th>
                                                    <th id="Th7"   runat="server" style="width: 45px;">軸5<br />ﾄﾙｸ/<br />角度</th>
                                                    <th id="Th8"   runat="server" style="width: 45px;">軸6<br />ﾄﾙｸ/<br />角度</th>
                                                    <th id="Th9"   runat="server" style="width: 45px;">軸7<br />ﾄﾙｸ/<br />角度</th>
                                                    <th id="Th10"  runat="server" style="width: 45px;">軸8<br />ﾄﾙｸ/<br />角度</th>
                                                </tr>
                                            </table>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div id="divFRDScroll" class="div-scroll-right-top div-right-grid" style="height:136px;margin-left:10px;">
                                        <div id="divFRDHeader" runat="server">
                                            <asp:ListView ID="lstFR" runat="server" OnItemDataBound="lstFR_ItemDataBound">
                                                <LayoutTemplate>
                                                    <table id="itemPlaceholderContainerFR" runat="server" class="grid-layout" style="width: 480px;">
                                                        <tr id="headerMainContent" runat="server" class="listview-header_3r ui-state-default">
                                                            <th id="FL1" 　runat="server" style="width: 40px;"></th>
                                                            <th id="FL2"   runat="server" style="width: 80px;"></th>
                                                            <th id="FL3" 　runat="server" style="width: 45px;"></th>
                                                            <th id="FL4"   runat="server" style="width: 45px;"></th>
                                                            <th id="FL5"   runat="server" style="width: 45px;"></th>
                                                            <th id="FL6"   runat="server" style="width: 45px;"></th>
                                                            <th id="FL7"   runat="server" style="width: 45px;"></th>
                                                            <th id="FL8"   runat="server" style="width: 45px;"></th>
                                                            <th id="FL9"   runat="server" style="width: 45px;"></th>
                                                            <th id="FL10"  runat="server" style="width: 45px;"></th>
                                                            <th id="Th11"  runat="server" class="tr-fix-zero-height"/>
                                                            <th id="Th12"  runat="server" class="tr-fix-zero-height"/>
                                                            <th id="Th13"  runat="server" class="tr-fix-zero-height"/>
                                                            <th id="Th14"  runat="server" class="tr-fix-zero-height"/>
                                                            <th id="Th15"  runat="server" class="tr-fix-zero-height"/>
                                                            <th id="Th16"  runat="server" class="tr-fix-zero-height"/>
                                                            <th id="Th17"  runat="server" class="tr-fix-zero-height"/>
                                                            <th id="Th18"  runat="server" class="tr-fix-zero-height"/>
                                                            <th id="Th43"  runat="server" class="tr-fix-zero-height"/>
                                                            <th id="Th44"  runat="server" class="tr-fix-zero-height"/>
                                                            <th id="Th45"  runat="server" class="tr-fix-zero-height"/>
                                                            <th id="Th46"  runat="server" class="tr-fix-zero-height"/>
                                                            <th id="Th47"  runat="server" class="tr-fix-zero-height"/>
                                                            <th id="Th48"  runat="server" class="tr-fix-zero-height"/>
                                                            <th id="Th49"  runat="server" class="tr-fix-zero-height"/>
                                                            <th id="Th50"  runat="server" class="tr-fix-zero-height"/>
                                                        </tr>
                                                        <tr id="itemPlaceholder" runat="server">
                                                        </tr>
                                                    </table>
                                                </LayoutTemplate>
                                                <ItemTemplate>
                                                    <tr id="trRowData" runat="server" class="listview-row ui-widget" style="height:42px">
                                                        <td><KTCC:KTTextBox ID="txtBenchiNo"        runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" /></td>
                                                        <td><KTCC:KTTextBox ID="txtInspectionDt"    runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" /></td>
                                                        <td><KTCC:KTTextBox ID="txtShaft1"          runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-rg" /></td>
                                                        <td><KTCC:KTTextBox ID="txtShaft2"          runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-rg" /></td>
                                                        <td><KTCC:KTTextBox ID="txtShaft3"          runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-rg" /></td>
                                                        <td><KTCC:KTTextBox ID="txtShaft4"          runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-rg" /></td>
                                                        <td><KTCC:KTTextBox ID="txtShaft5"          runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-rg" /></td>
                                                        <td><KTCC:KTTextBox ID="txtShaft6"          runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-rg" /></td>
                                                        <td><KTCC:KTTextBox ID="txtShaft7"          runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-rg" /></td>
                                                        <td><KTCC:KTTextBox ID="txtShaft8"          runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-rg" /></td>
                                                        <td class="tr-fix-zero-height"><KTCC:KTTextBox ID="txtShaft1_bk" runat="server" ReadOnly="true"/></td>
                                                        <td class="tr-fix-zero-height"><KTCC:KTTextBox ID="txtShaft2_bk" runat="server" ReadOnly="true"/></td>
                                                        <td class="tr-fix-zero-height"><KTCC:KTTextBox ID="txtShaft3_bk" runat="server" ReadOnly="true"/></td>
                                                        <td class="tr-fix-zero-height"><KTCC:KTTextBox ID="txtShaft4_bk" runat="server" ReadOnly="true"/></td>
                                                        <td class="tr-fix-zero-height"><KTCC:KTTextBox ID="txtShaft5_bk" runat="server" ReadOnly="true"/></td>
                                                        <td class="tr-fix-zero-height"><KTCC:KTTextBox ID="txtShaft6_bk" runat="server" ReadOnly="true"/></td>
                                                        <td class="tr-fix-zero-height"><KTCC:KTTextBox ID="txtShaft7_bk" runat="server" ReadOnly="true"/></td>
                                                        <td class="tr-fix-zero-height"><KTCC:KTTextBox ID="txtShaft8_bk" runat="server" ReadOnly="true"/></td>
                                                        <td class="tr-fix-zero-height"><KTCC:KTTextBox ID="txtNr1" runat="server" ReadOnly="true"/></td>
                                                        <td class="tr-fix-zero-height"><KTCC:KTTextBox ID="txtNr2" runat="server" ReadOnly="true"/></td>
                                                        <td class="tr-fix-zero-height"><KTCC:KTTextBox ID="txtNr3" runat="server" ReadOnly="true"/></td>
                                                        <td class="tr-fix-zero-height"><KTCC:KTTextBox ID="txtNr4" runat="server" ReadOnly="true"/></td>
                                                        <td class="tr-fix-zero-height"><KTCC:KTTextBox ID="txtNr5" runat="server" ReadOnly="true"/></td>
                                                        <td class="tr-fix-zero-height"><KTCC:KTTextBox ID="txtNr6" runat="server" ReadOnly="true"/></td>
                                                        <td class="tr-fix-zero-height"><KTCC:KTTextBox ID="txtNr7" runat="server" ReadOnly="true"/></td>
                                                        <td class="tr-fix-zero-height"><KTCC:KTTextBox ID="txtNr8" runat="server" ReadOnly="true"/></td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:ListView>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
            <tr style="height:10px"/>
            <%-- リア --%>
            <tr valign="top">
                <td>
                    <%-- リア(左) --%>
                    <div id="divRL" style="height: 200px">
                        <table class="table-layout-fix" style="width:520px">
                            <tr>
                                <td>
                                    <div id="divRLTitle">
                                        <table id="tblRLTitle">
                                            <tr>
                                                <th class="div-detail-table-title" style="width:190px;text-align:left">リア(左)</th>
                                                <th class="div-table-subtitle" style="width:100px;text-align:right">締付結果：</th>
                                                <th style="width: 50px">
                                                    <KTCC:KTTextBox ID="txtRLResult" runat="server" ReadOnly="true"  CssClass="div-table-subtitle" />
                                                </th>
                                            </tr>
                                        </table>
                                    </div>

                                    <div id="divRLScroll" class="div-scroll-right-top div-right-grid" style="margin-left:10px;">
                                        <div id="divRLHeader" runat="server">
                                            <table id="tblRLHeader" runat="server" class="grid-layout" style="width: 480px;">
                                                <tr id="RLtr" runat="server" class="listview-header_2r ui-state-default">
                                                    <th id="Th31" 　runat="server" style="width: 40px;">締付<br />回数</th>
                                                    <th id="Th32"   runat="server" style="width: 80px;">日時</th>
                                                    <th id="Th19" 　runat="server" style="width: 45px;">軸1<br />ﾄﾙｸ/<br />角度</th>
                                                    <th id="Th20"   runat="server" style="width: 45px;">軸2<br />ﾄﾙｸ/<br />角度</th>
                                                    <th id="Th21"   runat="server" style="width: 45px;">軸3<br />ﾄﾙｸ/<br />角度</th>
                                                    <th id="Th22"   runat="server" style="width: 45px;">軸4<br />ﾄﾙｸ/<br />角度</th>
                                                    <th id="Th23"   runat="server" style="width: 45px;">軸5<br />ﾄﾙｸ/<br />角度</th>
                                                    <th id="Th24"   runat="server" style="width: 45px;">軸6<br />ﾄﾙｸ/<br />角度</th>
                                                    <th id="Th25"   runat="server" style="width: 45px;">軸7<br />ﾄﾙｸ/<br />角度</th>
                                                    <th id="Th26"   runat="server" style="width: 45px;">軸8<br />ﾄﾙｸ/<br />角度</th>
                                                </tr>
                                            </table>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div id="divRLDScroll" class="div-scroll-right-top div-right-grid" style="height:136px;margin-left:10px;">
                                        <div id="divRLDHeader" runat="server">
                                            <asp:ListView ID="lstRL" runat="server" OnItemDataBound="lstRL_ItemDataBound">
                                                <LayoutTemplate>
                                                    <table id="itemPlaceholderContainerRL" runat="server" class="grid-layout" style="width: 480px;">
                                                        <tr id="headerMainContent" runat="server" class="listview-header_3r ui-state-default">
                                                            <th id="FL1" 　runat="server" style="width: 40px;"></th>
                                                            <th id="FL2"   runat="server" style="width: 80px;"></th>
                                                            <th id="FL3" 　runat="server" style="width: 45px;"></th>
                                                            <th id="FL4"   runat="server" style="width: 45px;"></th>
                                                            <th id="FL5"   runat="server" style="width: 45px;"></th>
                                                            <th id="FL6"   runat="server" style="width: 45px;"></th>
                                                            <th id="FL7"   runat="server" style="width: 45px;"></th>
                                                            <th id="FL8"   runat="server" style="width: 45px;"></th>
                                                            <th id="FL9"   runat="server" style="width: 45px;"></th>
                                                            <th id="FL10"  runat="server" style="width: 45px;"></th>
                                                            <th id="Th11"  runat="server" class="tr-fix-zero-height"/>
                                                            <th id="Th12"  runat="server" class="tr-fix-zero-height"/>
                                                            <th id="Th13"  runat="server" class="tr-fix-zero-height"/>
                                                            <th id="Th14"  runat="server" class="tr-fix-zero-height"/>
                                                            <th id="Th15"  runat="server" class="tr-fix-zero-height"/>
                                                            <th id="Th16"  runat="server" class="tr-fix-zero-height"/>
                                                            <th id="Th17"  runat="server" class="tr-fix-zero-height"/>
                                                            <th id="Th18"  runat="server" class="tr-fix-zero-height"/>
                                                            <th id="Th43"  runat="server" class="tr-fix-zero-height"/>
                                                            <th id="Th44"  runat="server" class="tr-fix-zero-height"/>
                                                            <th id="Th45"  runat="server" class="tr-fix-zero-height"/>
                                                            <th id="Th46"  runat="server" class="tr-fix-zero-height"/>
                                                            <th id="Th47"  runat="server" class="tr-fix-zero-height"/>
                                                            <th id="Th48"  runat="server" class="tr-fix-zero-height"/>
                                                            <th id="Th49"  runat="server" class="tr-fix-zero-height"/>
                                                            <th id="Th50"  runat="server" class="tr-fix-zero-height"/>
                                                        </tr>
                                                        <tr id="itemPlaceholder" runat="server">
                                                        </tr>
                                                    </table>
                                                </LayoutTemplate>
                                                <ItemTemplate>
                                                    <tr id="trRowData" runat="server" class="listview-row ui-widget"  style="height:42px">
                                                        <td><KTCC:KTTextBox ID="txtBenchiNo"        runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" /></td>
                                                        <td><KTCC:KTTextBox ID="txtInspectionDt"    runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" /></td>
                                                        <td><KTCC:KTTextBox ID="txtShaft1"          runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-rg" /></td>
                                                        <td><KTCC:KTTextBox ID="txtShaft2"          runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-rg" /></td>
                                                        <td><KTCC:KTTextBox ID="txtShaft3"          runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-rg" /></td>
                                                        <td><KTCC:KTTextBox ID="txtShaft4"          runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-rg" /></td>
                                                        <td><KTCC:KTTextBox ID="txtShaft5"          runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-rg" /></td>
                                                        <td><KTCC:KTTextBox ID="txtShaft6"          runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-rg" /></td>
                                                        <td><KTCC:KTTextBox ID="txtShaft7"          runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-rg" /></td>
                                                        <td><KTCC:KTTextBox ID="txtShaft8"          runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-rg" /></td>
                                                        <td class="tr-fix-zero-height"><KTCC:KTTextBox ID="txtShaft1_bk" runat="server" ReadOnly="true"/></td>
                                                        <td class="tr-fix-zero-height"><KTCC:KTTextBox ID="txtShaft2_bk" runat="server" ReadOnly="true"/></td>
                                                        <td class="tr-fix-zero-height"><KTCC:KTTextBox ID="txtShaft3_bk" runat="server" ReadOnly="true"/></td>
                                                        <td class="tr-fix-zero-height"><KTCC:KTTextBox ID="txtShaft4_bk" runat="server" ReadOnly="true"/></td>
                                                        <td class="tr-fix-zero-height"><KTCC:KTTextBox ID="txtShaft5_bk" runat="server" ReadOnly="true"/></td>
                                                        <td class="tr-fix-zero-height"><KTCC:KTTextBox ID="txtShaft6_bk" runat="server" ReadOnly="true"/></td>
                                                        <td class="tr-fix-zero-height"><KTCC:KTTextBox ID="txtShaft7_bk" runat="server" ReadOnly="true"/></td>
                                                        <td class="tr-fix-zero-height"><KTCC:KTTextBox ID="txtShaft8_bk" runat="server" ReadOnly="true"/></td>
                                                        <td class="tr-fix-zero-height"><KTCC:KTTextBox ID="txtNr1" runat="server" ReadOnly="true"/></td>
                                                        <td class="tr-fix-zero-height"><KTCC:KTTextBox ID="txtNr2" runat="server" ReadOnly="true"/></td>
                                                        <td class="tr-fix-zero-height"><KTCC:KTTextBox ID="txtNr3" runat="server" ReadOnly="true"/></td>
                                                        <td class="tr-fix-zero-height"><KTCC:KTTextBox ID="txtNr4" runat="server" ReadOnly="true"/></td>
                                                        <td class="tr-fix-zero-height"><KTCC:KTTextBox ID="txtNr5" runat="server" ReadOnly="true"/></td>
                                                        <td class="tr-fix-zero-height"><KTCC:KTTextBox ID="txtNr6" runat="server" ReadOnly="true"/></td>
                                                        <td class="tr-fix-zero-height"><KTCC:KTTextBox ID="txtNr7" runat="server" ReadOnly="true"/></td>
                                                        <td class="tr-fix-zero-height"><KTCC:KTTextBox ID="txtNr8" runat="server" ReadOnly="true"/></td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:ListView>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
                <td>
                    <%-- リア(右) --%>
                    <div id="divRR" style="height: 200px">
                        <table class="table-layout-fix" style="width:520px;margin-left:10px;">
                            <tr>
                                <td>
                                    <div id="divRRTitle">
                                        <table id="tblRRTitle">
                                            <tr>
                                                <th class="div-detail-table-title" style="width:190px;text-align:left">リア(右)</th>
                                                <th class="div-table-subtitle" style="width:100px;text-align:right">締付結果：</th>
                                                <th style="width: 50px">
                                                    <KTCC:KTTextBox ID="txtRRResult" runat="server" ReadOnly="true"  CssClass="div-table-subtitle" />
                                                </th>
                                            </tr>
                                        </table>
                                    </div>
                                    <div id="divRRScroll" class="div-scroll-right-top div-right-grid" style="margin-left:10px;">
                                        <div id="divRRHeader" runat="server">
                                            <table id="tblRRHeader" runat="server" class="grid-layout" style="width: 480px;">
                                                <tr id="RRtr" runat="server" class="listview-header_2r ui-state-default">
                                                    <th id="Th33" 　runat="server" style="width: 40px;">締付<br />回数</th>
                                                    <th id="Th34"   runat="server" style="width: 80px;">日時</th>
                                                    <th id="Th35" 　runat="server" style="width: 45px;">軸1<br />ﾄﾙｸ/<br />角度</th>
                                                    <th id="Th36"   runat="server" style="width: 45px;">軸2<br />ﾄﾙｸ/<br />角度</th>
                                                    <th id="Th37"   runat="server" style="width: 45px;">軸3<br />ﾄﾙｸ/<br />角度</th>
                                                    <th id="Th38"   runat="server" style="width: 45px;">軸4<br />ﾄﾙｸ/<br />角度</th>
                                                    <th id="Th39"   runat="server" style="width: 45px;">軸5<br />ﾄﾙｸ/<br />角度</th>
                                                    <th id="Th40"   runat="server" style="width: 45px;">軸6<br />ﾄﾙｸ/<br />角度</th>
                                                    <th id="Th41"   runat="server" style="width: 45px;">軸7<br />ﾄﾙｸ/<br />角度</th>
                                                    <th id="Th42"   runat="server" style="width: 45px;">軸8<br />ﾄﾙｸ/<br />角度</th>
                                                </tr>
                                            </table>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div id="divRRDScroll" class="div-scroll-right-top div-right-grid" style="height:136px;margin-left:10px;">
                                        <div id="divRRDHeader" runat="server">
                                            <asp:ListView ID="lstRR" runat="server" OnItemDataBound="lstRR_ItemDataBound">
                                                <LayoutTemplate>
                                                    <table id="itemPlaceholderContainerRR" runat="server" class="grid-layout" style="width: 480px;">
                                                        <tr id="headerMainContent" runat="server" class="listview-header_3r ui-state-default">
                                                            <th id="FL1" 　runat="server" style="width: 40px;"></th>
                                                            <th id="FL2"   runat="server" style="width: 80px;"></th>
                                                            <th id="FL3" 　runat="server" style="width: 45px;"></th>
                                                            <th id="FL4"   runat="server" style="width: 45px;"></th>
                                                            <th id="FL5"   runat="server" style="width: 45px;"></th>
                                                            <th id="FL6"   runat="server" style="width: 45px;"></th>
                                                            <th id="FL7"   runat="server" style="width: 45px;"></th>
                                                            <th id="FL8"   runat="server" style="width: 45px;"></th>
                                                            <th id="FL9"   runat="server" style="width: 45px;"></th>
                                                            <th id="FL10"  runat="server" style="width: 45px;"></th>
                                                            <th id="Th11"  runat="server" class="tr-fix-zero-height"/>
                                                            <th id="Th12"  runat="server" class="tr-fix-zero-height"/>
                                                            <th id="Th13"  runat="server" class="tr-fix-zero-height"/>
                                                            <th id="Th14"  runat="server" class="tr-fix-zero-height"/>
                                                            <th id="Th15"  runat="server" class="tr-fix-zero-height"/>
                                                            <th id="Th16"  runat="server" class="tr-fix-zero-height"/>
                                                            <th id="Th17"  runat="server" class="tr-fix-zero-height"/>
                                                            <th id="Th18"  runat="server" class="tr-fix-zero-height"/>
                                                            <th id="Th43"  runat="server" class="tr-fix-zero-height"/>
                                                            <th id="Th44"  runat="server" class="tr-fix-zero-height"/>
                                                            <th id="Th45"  runat="server" class="tr-fix-zero-height"/>
                                                            <th id="Th46"  runat="server" class="tr-fix-zero-height"/>
                                                            <th id="Th47"  runat="server" class="tr-fix-zero-height"/>
                                                            <th id="Th48"  runat="server" class="tr-fix-zero-height"/>
                                                            <th id="Th49"  runat="server" class="tr-fix-zero-height"/>
                                                            <th id="Th50"  runat="server" class="tr-fix-zero-height"/>
                                                        </tr>
                                                        <tr id="itemPlaceholder" runat="server">
                                                        </tr>
                                                    </table>
                                                </LayoutTemplate>
                                                <ItemTemplate>
                                                    <tr id="trRowData" runat="server" class="listview-row ui-widget" style="height:42px">
                                                        <td><KTCC:KTTextBox ID="txtBenchiNo"        runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" /></td>
                                                        <td><KTCC:KTTextBox ID="txtInspectionDt"    runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" /></td>
                                                        <td><KTCC:KTTextBox ID="txtShaft1"          runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-rg" /></td>
                                                        <td><KTCC:KTTextBox ID="txtShaft2"          runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-rg" /></td>
                                                        <td><KTCC:KTTextBox ID="txtShaft3"          runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-rg" /></td>
                                                        <td><KTCC:KTTextBox ID="txtShaft4"          runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-rg" /></td>
                                                        <td><KTCC:KTTextBox ID="txtShaft5"          runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-rg" /></td>
                                                        <td><KTCC:KTTextBox ID="txtShaft6"          runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-rg" /></td>
                                                        <td><KTCC:KTTextBox ID="txtShaft7"          runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-rg" /></td>
                                                        <td><KTCC:KTTextBox ID="txtShaft8"          runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-rg" /></td>
                                                        <td class="tr-fix-zero-height"><KTCC:KTTextBox ID="txtShaft1_bk" runat="server" ReadOnly="true"/></td>
                                                        <td class="tr-fix-zero-height"><KTCC:KTTextBox ID="txtShaft2_bk" runat="server" ReadOnly="true"/></td>
                                                        <td class="tr-fix-zero-height"><KTCC:KTTextBox ID="txtShaft3_bk" runat="server" ReadOnly="true"/></td>
                                                        <td class="tr-fix-zero-height"><KTCC:KTTextBox ID="txtShaft4_bk" runat="server" ReadOnly="true"/></td>
                                                        <td class="tr-fix-zero-height"><KTCC:KTTextBox ID="txtShaft5_bk" runat="server" ReadOnly="true"/></td>
                                                        <td class="tr-fix-zero-height"><KTCC:KTTextBox ID="txtShaft6_bk" runat="server" ReadOnly="true"/></td>
                                                        <td class="tr-fix-zero-height"><KTCC:KTTextBox ID="txtShaft7_bk" runat="server" ReadOnly="true"/></td>
                                                        <td class="tr-fix-zero-height"><KTCC:KTTextBox ID="txtShaft8_bk" runat="server" ReadOnly="true"/></td>
                                                        <td class="tr-fix-zero-height"><KTCC:KTTextBox ID="txtNr1" runat="server" ReadOnly="true"/></td>
                                                        <td class="tr-fix-zero-height"><KTCC:KTTextBox ID="txtNr2" runat="server" ReadOnly="true"/></td>
                                                        <td class="tr-fix-zero-height"><KTCC:KTTextBox ID="txtNr3" runat="server" ReadOnly="true"/></td>
                                                        <td class="tr-fix-zero-height"><KTCC:KTTextBox ID="txtNr4" runat="server" ReadOnly="true"/></td>
                                                        <td class="tr-fix-zero-height"><KTCC:KTTextBox ID="txtNr5" runat="server" ReadOnly="true"/></td>
                                                        <td class="tr-fix-zero-height"><KTCC:KTTextBox ID="txtNr6" runat="server" ReadOnly="true"/></td>
                                                        <td class="tr-fix-zero-height"><KTCC:KTTextBox ID="txtNr7" runat="server" ReadOnly="true"/></td>
                                                        <td class="tr-fix-zero-height"><KTCC:KTTextBox ID="txtNr8" runat="server" ReadOnly="true"/></td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:ListView>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</div>