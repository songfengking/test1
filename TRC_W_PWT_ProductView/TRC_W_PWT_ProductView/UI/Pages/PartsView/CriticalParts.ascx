<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CriticalParts.ascx.cs" Inherits="TRC_W_PWT_ProductView.UI.Pages.PartsView.CriticalParts" %>
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
        <div>
            <div class="div-detail-table-title">■最終組付情報</div>
            <div style="clear: both; width:auto;margin-top:10px"></div>
            <table id="tblMain" class="table-border-layout grid-layout" style="width: 900px; height: auto;margin-left:10px;" runat="server">
                <colgroup>
                    <col style="width: 200px" />
                    <col style="width: 250px" />
                    <col style="width: 200px" />
                    <col style="width: 250px" />
                </colgroup>
                <tr class="detailtable-header">
                    <td class="ui-state-default detailtable-header">組付日時</td>
                    <td>
                        <KTCC:KTTextBox ID="txtInstallDt" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full" />
                    </td>
                    <td class="ui-state-default detailtable-header">ステーション</td>
                    <td>
                        <KTCC:KTTextBox ID="txtStationNm" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full" />
                    </td>
                </tr>
                <tr class="detailtable-header">
                    <td class="ui-state-default detailtable-header">クボタ品番</td>
                    <td>
                        <KTCC:KTTextBox ID="txtPartsKubotaNum" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full" />
                    </td>
                    <td colspan="2"></td>
                </tr>
                <tr class="detailtable-header">
                    <td class="ui-state-default detailtable-header">来歴NO</td>
                    <td>
                        <KTCC:KTNumericTextBox ID="ntbHistoryIndex" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full" />
                    </td>
                    <td colspan="2"></td>
                </tr>
            </table>
        </div>

        <div>
            <div class="div-detail-table-title detailtable-header">■部品情報</div>
            <div style="clear: both; width:auto;margin-top:10px"></div>
            <table id="tblSub" class="table-border-layout grid-layout" style="width: 900px;margin-left:10px;" runat="server">
                <colgroup>
                    <col style="width: 200px" />
                    <col style="width: 250px" />
                    <col style="width: 200px" />
                    <col style="width: 250px" />
                </colgroup>
                <tr class="detailtable-header">
                    <td class="ui-state-default detailtable-header">加工ライン</td>
                    <td>
                        <KTCC:KTTextBox ID="txtProcessLine" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full" />
                    </td>
                    <td class="ui-state-default detailtable-header">部品名</td>
                    <td>
                        <KTCC:KTTextBox ID="txtPartsNm" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full" />
                    </td>
                </tr>
                <tr class="detailtable-header">
                    <td class="ui-state-default detailtable-header">加工日</td>
                    <td>
                        <KTCC:KTTextBox ID="txtProcessYmd" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full" />
                    </td>
                    <td class="ui-state-default detailtable-header">連番</td>
                    <td>
                        <KTCC:KTTextBox ID="txtProcessNum" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full" />
                    </td>
                </tr>
            </table>
        </div>

                <div>
            <div class="div-detail-table-title detailtable-header">■修正情報</div>
            <div style="clear: both; width:auto;margin-top:10px"></div>
            <table id="Table1" class="table-border-layout grid-layout" style="width: 900px;margin-left:10px;" runat="server">
                <colgroup>
                    <col style="width: 200px" />
                    <col style="width: 250px" />
                    <col style="width: 200px" />
                    <col style="width: 250px" />
                </colgroup>
                <tr class="detailtable-header">
                    <td class="ui-state-default detailtable-header">修正理由</td>
                    <td>
                        <KTCC:KTTextBox ID="txtRemarks" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full" TextMode="MultiLine"/>
                    </td>
                    <td class="ui-state-default detailtable-header">修正者</td>
                    <td>
                        <KTCC:KTTextBox ID="txtUpdateBy" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
</div>
  