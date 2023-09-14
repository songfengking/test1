<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Mission.ascx.cs" Inherits="TRC_W_PWT_ProductView.UI.Pages.PartsView.Mission" %>
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
        <div class="div-detail-table-title">■完成品詳細情報</div>
        <div style="clear: both; height: 10px;width:auto"></div>
        <div id="divMainListArea" class="div-auto-scroll">
            <table id="tblMain" class="table-border-layout grid-layout" style="width: 900px; height: auto;margin-left:10px" runat="server">
                <colgroup>
		            <col style="width: 200px" />
			        <col style="width: 250px" />
			        <col style="width: 200px" />
			        <col style="width: 250px" />
		        </colgroup>
                <tr class="detailtable-header">
                    <td class="ui-state-default detailtable-header">品番</td>
                    <td>
                        <KTCC:KTTextBox ID="txtPartNum" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full" />
                    </td>
                    <td class="ui-state-default detailtable-header">加工日</td>
                    <td>
                        <KTCC:KTTextBox ID="txtProcessingYmd" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full" />
                    </td>
                </tr>
                <tr class="detailtable-header">
                    <td class="ui-state-default detailtable-header">出庫要求日時</td>
                    <td>
                        <KTCC:KTTextBox ID="txtDvlReqDt" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full" />
                    </td>
                    <td class="ui-state-default detailtable-header">加工連番</td>
                    <td>
                        <KTCC:KTTextBox ID="txtProcessingSeq" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
</div>
