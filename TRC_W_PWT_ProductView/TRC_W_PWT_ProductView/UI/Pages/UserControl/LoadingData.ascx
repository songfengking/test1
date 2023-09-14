<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LoadingData.ascx.cs" Inherits="TRC_W_PWT_ProductView.UI.Pages.UserControl.LoadingData" %>

<%-- デザイン表示時使用 マスターページ使用時不要 --%>
<%--<link href="../../../CSS/cobalt/jquery-wijmo.css" rel="stylesheet" />
<link href="../../../CSS/Base.css" rel="stylesheet" />
<link href="../../../CSS/TRC.css" rel="stylesheet" />--%>

<%-- 画面CSS/スクリプト定義領域 --%>

<div id="divLoadingBackGround" runat="server" class="loading-back-ground size-zero"></div>
<asp:Image ID="imgLoadingImage" runat="server" CssClass="size-zero" />
<asp:Label id="lblLoadingChar" runat="server"></asp:Label>
