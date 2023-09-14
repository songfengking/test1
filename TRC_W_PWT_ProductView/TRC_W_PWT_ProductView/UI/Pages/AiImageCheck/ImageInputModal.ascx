<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ImageInputModal.ascx.cs" Inherits="TRC_W_PWT_ProductView.UI.Pages.AiImageCheck.ImageInputModal" %>
<%@ Import Namespace="TRC_W_PWT_ProductView.Common" %>

<script src="<%: PageUtils.ResolveUrl( this.Page, "~/Scripts/Pages/AiImageCheck/AiImageCheck.js") %>" ></script>

    <div id="divLoadingBackGround" runat="server" class="loading-back-ground size-zero"></div>
    <div id="MainModal" runat="server">
        <div id="ifrmArea"></div>
    </div>

