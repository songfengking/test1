<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CheckSheet.ascx.cs" Inherits="TRC_W_PWT_ProductView.UI.Pages.ProcessView.CheckSheet" %>
<%@ Import Namespace="TRC_W_PWT_ProductView.Common" %>

<%-- デザイン表示時使用 マスターページ使用時不要 --%>
<%--<link href="../../../CSS/cobalt/jquery-wijmo.css" rel="stylesheet" />
<link href="../../../CSS/Base.css" rel="stylesheet" />
<link href="../../../CSS/TRC.css" rel="stylesheet" />--%>

<%-- 画面CSS/スクリプト定義領域 --%>
<asp:PlaceHolder ID="SubScripts" runat="server">
    <script src="<%: ResolveUrl( "~/Scripts/LibScript/scrollViewer.js") %>" ></script>
    <script src="<%: PageUtils.ResolveUrl( this.Page, "~/Scripts/Pages/ProcessView/CheckSheet.js") %>" ></script>
    <script src="<%: PageUtils.ResolveUrl( this.Page, "~/Scripts/ControlCommon.js") %>" ></script>
</asp:PlaceHolder>

<div class="div-detail-info-margin">
    <div id="divDetailBodyScroll" class="div-fix-scroll">
        <asp:Button id="btnPrint" runat="server" Text="印刷" CssClass="btn-middle"/>
        <div id="divDetailListArea" style="width: 231px" class="div-y-scroll-flt">
            <asp:ListView ID="lstCheckSheetList" runat="server" OnItemDataBound="lstCheckSheetList_ItemDataBound">
                <LayoutTemplate>
                    <div class="" id="itemPlaceholder" runat="server" />
                </LayoutTemplate>
                <ItemTemplate>
                    <div id="divRowData" runat="server" class="div-list-view-item" style="width: 208px; height: auto; margin-bottom: 3px;">
                        <table class="table-border-layout" style="margin-left: 0px; margin-right:1px">
                            <colgroup>
                                <col style="width: 202px" />
                            </colgroup>
                            <tr>
                                <td>
                                    <div>
                                        <asp:Image ID="imgCheckSheet" runat="server" CssClass="thumbnail-area-a4" />
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                </ItemTemplate>
            </asp:ListView>
        </div>
        <div id="divDetailViewArea" class="div-fix-scroll-flt">
            <div id="divDetailViewBox" class="div-auto-scroll">
                <asp:Image ID="imgMainArea" runat="server" AlternateText="" class="box-in-center" />
            </div>
        </div>
    </div>
</div>
