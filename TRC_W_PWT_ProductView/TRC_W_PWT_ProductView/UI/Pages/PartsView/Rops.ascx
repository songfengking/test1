<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Rops.ascx.cs" Inherits="TRC_W_PWT_ProductView.UI.Pages.PartsView.Rops" %>
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
        <div class="div-detail-table-title">■最終組付情報</div>
        <div style="clear: both; height: 10px;width:auto"></div>
        <div id="divMainListArea" class="div-auto-scroll" style="margin-left:10px">
            <table id="tblMain" class="table-border-layout" style="width: 900px; height: auto;" runat="server">
                <colgroup>
		            <col style="width: 200px" />
			        <col style="width: 250px" />
			        <col style="width: 200px" />
			        <col style="width: 250px" />
		        </colgroup>
                <tr class="detailtable-header">
                    <td class="ui-state-default detailtable-header">組付日時</td>
                    <td>
                        <KTCC:KTTextBox ID="txtInstallDt" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-ymdhms" />
                    </td>
                    <td colspan="2"></td>
                </tr>
                <tr class="detailtable-header">
                    <td class="ui-state-default detailtable-header">ロプス品番</td>
                    <td>
                        <KTCC:KTTextBox ID="txtRopsModelNum" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-long" />
                    </td>
                    <td class="ui-state-default detailtable-header">ロプス機番</td>
                    <td>
                        <KTCC:KTTextBox ID="txtRopsSerial" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-long" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
</div>
