<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MainProcessOperationPartialView.ascx.cs" Inherits="TRC_W_PWT_ProductView.UI.Pages.UserControl.MainProcessOperationPartialView" %>
<%--検索条件（噴射時期計測03）--%>
<span runat="server" id="spnEngineInjection03Operation" visible="false" >
    <KTCC:KTButton id="engineInjection03_btnNGReport" runat="server" Text="NG報告書" CssClass="btn-middle" OnClick="engineInjection03_btnNGReport_Click" OnClientClick="SubmitControl.SetLoadingType(SubmitControl.TYPE_NONE);" />
    <KTCC:KTButton id="engineInjection03_btnDetail" runat="server" Text="詳細出力" CssClass="btn-middle" OnClick="engineInjection03_btnDetail_Click" OnClientClick="SubmitControl.SetLoadingType(SubmitControl.TYPE_NONE);" />
</span>
<%--検索条件（噴射時期計測07）--%>
<span runat="server" id="spnEngineInjection07Operation" visible="false" >
    <KTCC:KTButton id="engineInjection07_btnDetail" runat="server" Text="詳細出力" CssClass="btn-middle" OnClick="engineInjection07_btnDetail_Click" OnClientClick="SubmitControl.SetLoadingType(SubmitControl.TYPE_NONE);" />
</span>