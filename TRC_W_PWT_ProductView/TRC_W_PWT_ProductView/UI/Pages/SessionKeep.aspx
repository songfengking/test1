<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SessionKeep.aspx.cs" Inherits="TRC_W_PWT_ProductView.UI.Pages.SessionKeep" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <asp:PlaceHolder ID="BaseScripts" runat="server">
        <script src="<%: ResolveUrl( "~/Scripts/LibScript/jquery-1.11.1.min.js") %>"></script>
        <script type="text/javascript">
            $(document).ready(function () {
                var mngr = Sys.WebForms.PageRequestManager.getInstance();
                mngr.add_endRequest(
                    function (sender, args) {
                        if (null != args.get_error()) {
                            if (null != args._response && null != args._response._timedOut && true == args._response._timedOut) {
                                //alert("サーバとの通信がタイムアウトしました。");
                            } else {
                                //alert("サーバとの通信が正常に完了しませんでした。");
                            }
                        }
                        args.set_errorHandled(true);
                    }
                );
            });
        </script>
    </asp:PlaceHolder>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div>    
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:Timer ID="timSessionKeepTimer" runat="server" OnTick="timSessionKeepTimer_Tick"></asp:Timer>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
