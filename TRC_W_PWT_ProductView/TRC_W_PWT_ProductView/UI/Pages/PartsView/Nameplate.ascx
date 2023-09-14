<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Nameplate.ascx.cs" Inherits="TRC_W_PWT_ProductView.UI.Pages.PartsView.Nameplate" %>
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
        <div class="div-detail-table-title" style="margin-bottom:10px;">■ラベル情報</div>
        <div runat="server" id="divTractor" class="div-auto-scroll" style="margin-left:10px;margin-bottom:20px;" visible="false">
            <table class="table-border-layout" style="width: 400px; height: auto;" runat="server">
                <colgroup>
		            <col style="width: 150px" />
			        <col style="width: 250px" />
		        </colgroup>
                <tr class="detailtable-header">
                    <td class="ui-state-default detailtable-header">ラベル種別</td>
                    <td>
                        <KTCC:KTTextBox ID="txtPlateTypeTractor" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-long" />
                    </td>
                </tr>
                <tr class="detailtable-header">
                    <td class="ui-state-default detailtable-header">銘板コード</td>
                    <td>
                        <KTCC:KTTextBox ID="txtNamePlateCdTractor" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-long" />
                    </td>
                </tr>
                <tr class="detailtable-header">
                    <td class="ui-state-default detailtable-header">銘板名</td>
                    <td>
                        <KTCC:KTTextBox ID="txtNamePlateNmTractor" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-long" />
                    </td>
                </tr>
                <tr class="detailtable-header">
                    <td class="ui-state-default detailtable-header">製造番号</td>
                    <td>
                        <KTCC:KTTextBox ID="txtSubProductCdTractor" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-long" />
                    </td>
                </tr>
                <tr class="detailtable-header">
                    <td class="ui-state-default detailtable-header">発行日時</td>
                    <td>
                        <KTCC:KTTextBox ID="txtPrintDtTractor" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-ymdhms" />
                    </td>
                </tr>

            </table>
        </div>
        <div id="divCab" runat="server" class="div-auto-scroll" style="margin-left:10px;margin-bottom:20px;" visible="false">
            <table class="table-border-layout" style="width: 400px; height: auto;" runat="server">
                <colgroup>
		            <col style="width: 150px" />
			        <col style="width: 250px" />
		        </colgroup>
                <tr class="detailtable-header">
                    <td class="ui-state-default detailtable-header">ラベル種別</td>
                    <td>
                        <KTCC:KTTextBox ID="txtPlateTypeCab" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-long" />
                    </td>
                </tr>
                <tr class="detailtable-header">
                    <td class="ui-state-default detailtable-header">銘板コード</td>
                    <td>
                        <KTCC:KTTextBox ID="txtNamePlateCdCab" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-long" />
                    </td>
                </tr>
                <tr class="detailtable-header">
                    <td class="ui-state-default detailtable-header">銘板名</td>
                    <td>
                        <KTCC:KTTextBox ID="txtNamePlateNmCab" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-long" />
                    </td>
                </tr>
                <tr class="detailtable-header">
                    <td class="ui-state-default detailtable-header">製造番号</td>
                    <td>
                        <KTCC:KTTextBox ID="txtSubProductCdCab" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-long" />
                    </td>
                </tr>
                <tr class="detailtable-header">
                    <td class="ui-state-default detailtable-header">発行日時</td>
                    <td>
                        <KTCC:KTTextBox ID="txtPrintDtCab" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-ymdhms" />
                    </td>
                </tr>

            </table>
        </div>
        <div id="divRops" runat="server" class="div-auto-scroll" style="margin-left:10px" visible="false">
            <table class="table-border-layout" style="width: 400px; height: auto;" runat="server">
                <colgroup>
		            <col style="width: 150px" />
			        <col style="width: 250px" />
		        </colgroup>
                <tr class="detailtable-header">
                    <td class="ui-state-default detailtable-header">ラベル種別</td>
                    <td>
                        <KTCC:KTTextBox ID="txtPlateTypeRops" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-long" />
                    </td>
                </tr>
                <tr class="detailtable-header">
                    <td class="ui-state-default detailtable-header">銘板コード</td>
                    <td>
                        <KTCC:KTTextBox ID="txtNamePlateCdRops" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-long" />
                    </td>
                </tr>
                <tr class="detailtable-header">
                    <td class="ui-state-default detailtable-header">銘板名</td>
                    <td>
                        <KTCC:KTTextBox ID="txtNamePlateNmRops" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-long" />
                    </td>
                </tr>
                <tr class="detailtable-header">
                    <td class="ui-state-default detailtable-header">製造番号</td>
                    <td>
                        <KTCC:KTTextBox ID="txtSubProductCdRops" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-long" />
                    </td>
                </tr>
                <tr class="detailtable-header">
                    <td class="ui-state-default detailtable-header">発行日時</td>
                    <td>
                        <KTCC:KTTextBox ID="txtPrintDtRops" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-ymdhms" />
                    </td>
                </tr>

            </table>
        </div>
    </div>
</div>
