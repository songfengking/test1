<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/UI/MasterForm/MasterMain.master" CodeBehind="LoginByID.aspx.cs" Inherits="TRC_W_PWT_ProductView.UI.Pages.LoginByID" %>
<%@ Import Namespace="TRC_W_PWT_ProductView.Common" %>

<asp:Content ID="MasterScript" runat="server" ContentPlaceHolderID="MasterScript" >
    <script src="<%: PageUtils.ResolveUrl( this.Page, "~/Scripts/Pages/LoginByID.js") %>" ></script>
</asp:Content>

<asp:Content ID="MasterBody" ContentPlaceHolderID="MasterBody" runat="server">
    <div style="width: 100%; height: 100%;">
        <div class="div-login-table">
            <table id="tblMain" class="table-border-layout" style="width: 400px; height: 166px;" runat="server">
                <colgroup>
                    <col style="width: 200px" />
                    <col style="width: 200px" />
                </colgroup>
                <tr class="detailtable-header">
                    <td class="ui-state-default detailtable-header">ユーザID</td>
                    <td>
                        <KTCC:KTTextBox ID="txtUserID" runat="server" CssClass="font-default txt-default txt-width-long" />
                    </td>
                </tr>
                <tr class="detailtable-header">
                    <td class="ui-state-default detailtable-header">パスワード</td>
                    <td>
                        <KTCC:KTTextBox ID="txtPassword" runat="server" CssClass="txt-default txt-width-long" TextMode="Password" MaxLength="20" />
                    </td>
                </tr>
                <tr class="tr-no-border" style="height:20px;">
                    <td colspan="2" />
                </tr>
                <tr class="tr-no-border">
                    <td style="text-align: center;">
                        <asp:Button ID="btnLogin" runat="server" Text="ログイン" CssClass="btn-long" OnClick="btnLogin_Click" />
                    </td>
                    <td style="text-align: center;">
                        <asp:Button ID="btnGuestLogin" runat="server" Text="ゲストログイン" CssClass="btn-long" OnClick="btnGuestLogin_Click" />
                    </td>
                </tr>
                <tr class="tr-no-border" style="height:20px;">
                    <td colspan="2" />
                </tr>
                <tr  class="tr-no-border">
                    <td colspan="2" style="text-align: center;">
                        <asp:Button ID="btnChangePassword" runat="server" Text="パスワード変更" CssClass="btn-long" OnClick="btnChangePassword_Click" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
