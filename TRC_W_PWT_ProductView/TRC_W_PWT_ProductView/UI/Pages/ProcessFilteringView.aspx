<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/UI/MasterForm/MasterSub.master" CodeBehind="ProcessFilteringView.aspx.cs" Inherits="TRC_W_PWT_ProductView.UI.Pages.ProcessFilteringView" %>

<%@ Import Namespace="TRC_W_PWT_ProductView.Common" %>

<asp:Content ID="MasterScript" runat="server" ContentPlaceHolderID="MasterScript">
    <script src="<%: PageUtils.ResolveUrl( this.Page, "~/Scripts/ControlCommon.js") %>"></script>
    <script src="<%: PageUtils.ResolveUrl( this.Page, "~/Scripts/Pages/ProcessFilteringView.js") %>"></script>
</asp:Content>
<asp:Content ID="MasterBodyTop" ContentPlaceHolderID="MasterBodyTop" runat="server">
    <%-- 画面タイトル --%>
    <div class="ui-widget-header header-title-area">
        <asp:Label ID="lblTitle" runat="server" Text="工程絞込検索"></asp:Label>
    </div>
</asp:Content>
<asp:Content ID="MasterBody" ContentPlaceHolderID="MasterBody" runat="server">
    <div class="condition-area">
        <table class="table-layout" style="width: 340px">
            <tr class="tr-fix-zero-height">
                <td style="width: 120px"></td>
                <td style="width: 220px"></td>
            </tr>
            <tr class="tr-ctrl-height">
                <td>ライン</td>
                <td>
                    <KTCC:KTDropDownList ID="ddlLineCd" runat="server" CssClass="font-default ddl-default ddl-width-long"></KTCC:KTDropDownList>
                </td>
            </tr>
            <tr class="tr-ctrl-height">
                <td>工程名</td>
                <td>
                    <KTCC:KTTextBox ID="txtProcessName" runat="server" CssClass="font-default txt-default txt-width-long"></KTCC:KTTextBox>
                </td>
            </tr>
            <tr class="tr-ctrl-height">
                <td>作業名</td>
                <td>
                    <KTCC:KTTextBox ID="txtWorkName" runat="server" CssClass="font-default txt-default txt-width-long"></KTCC:KTTextBox>
                </td>
            </tr>
        </table>
    </div>
    <div class="condition-button-area">
        <KTCC:KTButton ID="btnSearch" runat="server" Text="検索" CssClass="btn-middle" OnClick="btnSearch_Click" />
    </div>
    <div style="clear: both; height: 0px;"></div>
    <div class="result-area">
        <div style="margin-left: 10px; overflow: auto">
            <asp:GridView ID="grvProcessWork" runat="server" CssClass="grid-layout ui-widget-content" AutoGenerateColumns="False" OnRowDataBound="grvProcessWork_RowDataBound">
                <HeaderStyle CssClass="position:relative; grid-header ui-state-default" />
                <RowStyle CssClass="grid-row ui-widget" />
                <SelectedRowStyle CssClass="ui-state-highlight" />
            </asp:GridView>
            <asp:HiddenField ID="hdnSelectedIndex" runat="server" Value="-1" />
            <asp:HiddenField ID="hdnLineCd" runat="server" Value="" />
            <asp:HiddenField ID="hdnProcessCd" runat="server" Value="" />
            <asp:HiddenField ID="hdnProcessNm" runat="server" Value="" />
            <asp:HiddenField ID="hdnWorkCd" runat="server" Value="" />
            <asp:HiddenField ID="hdnWorkNm" runat="server" Value="" />
            <asp:HiddenField ID="hdnSearchTargetFlg" runat="server" Value="" />
        </div>
    </div>
</asp:Content>
<asp:Content ID="MasterBodyBottom" ContentPlaceHolderID="MasterBodyBottom" runat="server">
    <div class="div-bottom-button-area">
        <KTCC:KTButton ID="btnSelect" runat="server" Text="選択" CssClass="btn-middle" OnClick="btnSelect_Click" OnClientClick="ProcessFilteringView.BeforeSelect()" />
        <KTCC:KTButton ID="btnCancel" runat="server" Text="キャンセル" CssClass="btn-middle" OnClientClick="ControlCommon.WindowClose();return false;" />
    </div>
</asp:Content>
