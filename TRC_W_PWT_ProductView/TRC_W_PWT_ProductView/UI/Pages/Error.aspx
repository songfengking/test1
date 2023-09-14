<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/UI/MasterForm/MasterMain.master" CodeBehind="Error.aspx.cs" Inherits="TRC_W_PWT_ProductView.UI.Pages.Error" %>

<asp:Content ID="MasterScript" runat="server" ContentPlaceHolderID="MasterScript" >
</asp:Content>

<asp:Content ID="MasterBody" ContentPlaceHolderID="MasterBody" runat="server" >
    <div>
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblErrorCode" runat="server" Text="" /></td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblErrorInformation" runat="server" Text="" /></td>
            </tr>
            <tr>
                <td>
                    <KTCC:KTButton ID="btnLogin" runat="server" CssClass="link-style font-default" Onclick="btnLogin_Click" Text="検索一覧"></KTCC:KTButton></td>
            </tr>
        </table>
    </div>
</asp:Content>
