<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PrintCheckSheet.ascx.cs" Inherits="TRC_W_PWT_ProductView.UI.Pages.PrintCheckSheet" %>
<%@ Import Namespace="TRC_W_PWT_ProductView.Common" %>

<!DOCTYPE html>
<html lang="ja">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=8"/>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta charset="utf-8" />
    <meta http-equiv="Cache-control" content="no-cache" />
    <meta http-equiv="Pragma" content="no-cache" />
    <meta http-equiv="Expires" content="0" />
    <title></title>
    <link href="../../../CSS/cobalt/jquery-wijmo.css" rel="stylesheet" />
    <link href="../../../CSS/Base.css" rel="stylesheet" />
    <link href="../../../CSS/TRC.css" rel="stylesheet" />

    <asp:PlaceHolder ID="SubScripts" runat="server" >
        <script src="<%: ResolveUrl( "~/Scripts/LibScript/scrollViewer.js") %>" ></script>
        <script src="<%: ResolveUrl( "~/Scripts/LibScript/jquery-1.11.1.min.js") %>" ></script>
        <script src="<%: PageUtils.ResolveUrl( this.Page, "~/Scripts/Pages/ProcessView/PrintCheckSheet.js") %>" ></script>
        <script src="<%: PageUtils.ResolveUrl( this.Page, "~/Scripts/ControlCommon.js") %>" ></script>
    </asp:PlaceHolder>
    </head>
<body">
    <form runat="server">
        <div id="divDetailBodyScroll" class="div-fix-scroll">
            <div id="divListArea" style="width: 231px" runat="server" class="div-y-scroll-flt div-list-view-item">
           
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
                                            <asp:CheckBox ID="chkPrint" runat="server" Checked="true"/>
                                        </div>
                                        <div>
                                            <asp:Image ID="imgCheckSheet" runat="server" CssClass="thumbnail-area-a4 print-div-list"/>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </ItemTemplate>
                </asp:ListView>
            </div>
            <div id="divViewArea" class="div-fix-scroll">
                <div id="divViewBox" runat="server" class="div-auto-scroll">
                    <asp:Image ID="imgMainArea" runat="server" AlternateText="" name="imgMainArea"/>
                </div>
            </div>
            <div style="clear: both;width:auto"></div>
            <div id="divPrintArea" runat="server" style="width: 794px">
                <asp:ListView ID="lstPrintList" runat="server" OnItemDataBound="lstPrintList_ItemDataBound">
                    <LayoutTemplate>
                        <div class="" id="itemPlaceholder" runat="server" />
                    </LayoutTemplate>
                    <ItemTemplate>
                        <div id="divRowData" runat="server" style="width:100%; height: auto;">                                    
                            <table style="vertical-align:top;">
                                <tr>
                                    <td>
                                        <div>
                                            <asp:Image ID="imgCheckSheet" runat="server" CssClass="print-div-list print-show"/>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </ItemTemplate>
                </asp:ListView>
            </div>
        </div>
    </form>
</body>
</html>
