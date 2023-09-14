<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/UI/MasterForm/MasterMain.master" CodeBehind="ChangePassword.aspx.cs" Inherits="TRC_W_PWT_ProductView.UI.Pages.ChangePassword" %>
<%@ Import Namespace="TRC_W_PWT_ProductView.Common" %>

<asp:Content ID="MasterScript" runat="server" ContentPlaceHolderID="MasterScript" >
    <script src="<%: PageUtils.ResolveUrl( this.Page, "~/Scripts/Pages/ChangePassword.js") %>" ></script>
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
                <tr class="tr-no-border" style="height:20px;">
                    <td colspan="2" />
                </tr>
                <tr class="detailtable-header">
                    <td class="ui-state-default detailtable-header">旧パスワード</td>
                    <td>
                        <KTCC:KTTextBox ID="txtOldPassword" runat="server" CssClass="txt-default txt-width-long" TextMode="Password" MaxLength="20" />
                    </td>
                </tr>
                <tr class="detailtable-header">
                    <td class="ui-state-default detailtable-header">新パスワード</td>
                    <td>
                        <KTCC:KTTextBox ID="txtNewPassword" runat="server" CssClass="txt-default txt-width-long" TextMode="Password" MaxLength="20" />
                    </td>
                </tr>
                <tr class="detailtable-header">
                    <td class="ui-state-default detailtable-header">新パスワード確認</td>
                    <td>
                        <KTCC:KTTextBox ID="txtNewPasswordConfirm" runat="server" CssClass="txt-default txt-width-long" TextMode="Password" MaxLength="20" />
                        <asp:Label ID="lblCheckPassword" runat="server" CssClass="lbl-check-pw" Text="" Font-Size="Large" EnableViewState="false"/>
                    </td>
                </tr>
                <tr class="tr-no-border" style="height:20px;">
                    <td colspan="2" />
                </tr>
                <tr  class="tr-no-border">
                    <td colspan="2" style="text-align: center;">
                        <asp:Button ID="btnEdit" runat="server" Text="変更" CssClass="btn-long" OnClick="btnEdit_Click" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
