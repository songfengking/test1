<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AtuPartsSerial.ascx.cs" Inherits="TRC_W_PWT_ProductView.UI.Pages.PartsSearch.AtuPartsSerial" %>
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
    <div>
        <div class="div-detail-table-title">■来歴情報</div>
        <div id="divMainListArea" class="div-auto-scroll">
            <asp:ListView ID="lstMainList" runat="server" OnItemDataBound="lstMainList_ItemDataBound">
                <LayoutTemplate>
                    <div class="" id="itemPlaceholder" runat="server">
                    </div>
                </LayoutTemplate>
                <ItemTemplate>
                    <div class="ui-state-default div-detail-table-sub-title">
                        <span>【通過日】</span>
                        <KTCC:KTTextBox ID="txtReadDt" runat="server" ReadOnly="true" CssClass="" />
                    </div>
                    <div class="div-detail-table-subarea">
                        <table class="table-border-layout grid-layout">
                            <colgroup>
                                <col style="width: 150px" />
                                <col style="width: 150px" />
                                <col style="width: 150px" />
                                <col style="width: 150px" />
                            </colgroup>
                            <tr class="listview-header_2r">
                                <td class="ui-state-default detailtable-header">ステーションNO</td>
                                <td>
                                    <KTCC:KTTextBox ID="txtStationCd" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                                </td>
                                <td class="ui-state-default detailtable-header"></td>
                                <td>
                                    <KTCC:KTTextBox ID="txtNull" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                                </td>
                            </tr>
                            <tr class="listview-header_2r">
                                <td class="ui-state-default detailtable-header">アッシ品番</td>
                                <td>
                                    <KTCC:KTTextBox ID="txtAssyPartsNum" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                                </td>
                                <td class="ui-state-default detailtable-header">アッシ機番</td>
                                <td>
                                    <KTCC:KTTextBox ID="txtAssySerial" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                                </td>
                            </tr>
                            <tr class="listview-header_2r">
                                <td class="ui-state-default detailtable-header">構成品品番1</td>
                                <td>
                                    <KTCC:KTTextBox ID="txtComponentPartsNum1" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                                </td>
                                <td class="ui-state-default detailtable-header">構成品機番1</td>
                                <td>
                                    <KTCC:KTTextBox ID="txtComponentSerial1" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                                </td>
                            </tr>
                            <tr class="listview-header_2r">
                                <td class="ui-state-default detailtable-header">構成品品番2</td>
                                <td>
                                    <KTCC:KTTextBox ID="txtComponentPartsNum2" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                                </td>
                                <td class="ui-state-default detailtable-header">構成品機番2</td>
                                <td>
                                    <KTCC:KTTextBox ID="txtComponentSerial2" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                                </td>
                            </tr>
                        </table>
                </ItemTemplate>
            </asp:ListView>
        </div>
    </div>
</div>
