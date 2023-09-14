<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PreViewForm.aspx.cs" Inherits="TRC_W_PWT_ProductView.UI.Pages.PreViewForm" %>
<%@ Import Namespace="TRC_W_PWT_ProductView.Common" %>

<!DOCTYPE html>
<html lang="ja">
<head>
    <meta http-equiv="X-UA-Compatible" content="IE=8"/>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta charset="utf-8" />
    <meta http-equiv="Cache-control" content="no-cache" />
    <meta http-equiv="Pragma" content="no-cache" />
    <meta http-equiv="Expires" content="0" />
    <title></title>
    <script type="text/javascript" src="../../Scripts/Pages/PrintViewForm.js"></script>
    <script src="<%: ResolveUrl( "~/Scripts/LibScript/jquery-1.11.1.min.js") %>" ></script>
</head>
<body onload="PrintViewForm.loadPrintDisp(0);PrintViewForm.zoomSize(0);" onresize="PrintViewForm.loadPrintDisp();" style="overflow:hidden">

    <div id="Body" class="print-div-disp" style="width:100%">
        <table id="btnTbl">
	        <tr>
		        <td id="btnArea">
			        <input type="button" name="btnReduct"   id="btnReduct"  value="縮小" onclick="PrintViewForm.zoomSize('-1');"/>
			        <input type="button" name="btnDefault"  id="btnDefault" value="初期" onclick="PrintViewForm.zoomSize('0');"/>
			        <input type="button" name="btnExpan"    id="btnExpan"   value="拡大" onclick="PrintViewForm.zoomSize('1');"/>
			        <input type="button" name="btnPrint"    id="btnPrint"   value="印刷" onclick="PrintViewForm.PrintPreview();"/>
		        </td>
		        <td id="buttonArea">
                    <input type="button" name="searchBtn"  class="button" value="閉じる" onclick="window.close();">
		        </td>
	        </tr>
        </table>
        <HR>
    </div>
    <div id="View" class="print-div-view">
        <table id="imgTbl">
	        <tr>
		        <td id="viewArea">
                    <div id="divDetailViewBox">
                        <asp:Panel name="pnlDetailControl" ID="pnlDetailControl" runat="server" EnableViewState="true"></asp:Panel>
                    </div>
                </td>
	        </tr>
        </table>
    </div>
</body>
</html>

