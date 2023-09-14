<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/UI/MasterForm/MasterSub.master" CodeBehind="AnlItemInput.aspx.cs" Inherits="TRC_W_PWT_ProductView.UI.Pages.AiImageCheck.AnlItemInput" %>
<%@ Import Namespace="TRC_W_PWT_ProductView.Common" %>
<%@ Register src="~/UI/Pages/UserControl/LoadingData.ascx" tagname="LoadingData" tagprefix="UC" %>
<asp:Content ID="MasterScript" runat="server" ContentPlaceHolderID="MasterScript">
	<link href="../../../CSS/cobalt/jquery-wijmo.css" rel="stylesheet" />
	<link href="../../../CSS/Base.css" rel="stylesheet" />
	<link href="../../../CSS/TRC.css" rel="stylesheet" />
	<script src="<%: PageUtils.ResolveUrl( this.Page, "~/Scripts/Pages/AiImageCheck/AnlItemInput.js") %>"></script>
	<script src="<%: PageUtils.ResolveUrl( this.Page, "~/Scripts/Pages/AiImageCheck/AiImageCheck.js") %>"></script>
    <script src="<%: PageUtils.ResolveUrl( this.Page, "~/Scripts/Pages/InputModal.js") %>" ></script>
	<script src="<%: PageUtils.ResolveUrl( this.Page, "~/Scripts/ControlCommon.js") %>" ></script>
	<script src="<%: PageUtils.ResolveUrl( this.Page, "~/Scripts/Pages/AiImageCheck/SubmitControl.js") %>" ></script>
    <script src="<%: PageUtils.ResolveUrl( this.Page, "~/Scripts/Pages/AiImageCheck/Ready.js") %>" ></script>

</asp:Content>

<asp:Content ID="MasterBody" ContentPlaceHolderID="MasterBody" runat="server">
	<div style="width: 650px; height: 250px; margin-left: 30px;">
		<div style="margin-top: 30px; text-align: center;"></div>
		<table id="tblMain" class="table-layout">
			<colgroup>
				<col style="width: 200px" />
				<col style="width: 400px" />
			</colgroup>
			<tr>
				<td class="box-in-right">画像解析項目コード: </td>
				<td>
                    <asp:Label ID="lblItemCd" runat="server" Text=""></asp:Label>
				</td>
			</tr>
			<tr>
				<td class="box-in-right">画像解析項目名: </td>
				<td>
                   <KTCC:KTTextBox ID="txtItemNm" runat="server" MaxLength="40" CssClass="font-default txt-default ime-inactive txt-width-long" AutoUpper="False" />
				</td>
			</tr>
			<tr>
				<td class="box-in-right">ライン: </td>
				<td>
					<asp:Label ID="lblLineNm" runat="server" Text=""></asp:Label>
				</td>
			</tr>
			<tr>
				<td class="box-in-right">ステーション: </td>
				<td>
					<asp:Label ID="lblStationNm" runat="server" Text=""></asp:Label>
				</td>
			</tr>
			<tr>
				<td class="box-in-right">利用ツール: </td>
				<td>
					<KTCC:KTDropDownList ID="ddlUsingTool" runat="server" CssClass="font-default ddl-default" AutoPostBack="True" CausesValidation="True"  OnSelectedIndexChanged="ddlUsingToolList_SelectedIndexChanged" />
				</td>
			</tr>
			<tr>
				<td class="box-in-right" >ストリーム: </td>
				<td>
					<KTCC:KTTextBox ID="txtStream" runat="server" MaxLength="40" CssClass="font-default txt-default ime-inactive txt-width-long" AutoUpper="False" />
				</td>
			</tr>
			<tr>
				<td class="box-in-right">利用モデル: </td>
				<td>
                    <KTCC:KTTextBox ID="txtUsingModel" runat="server" MaxLength="40" CssClass="font-default txt-default ime-inactive txt-width-long" AutoUpper="False" />
				</td>
			</tr>
			<tr>
				<td class="box-in-right">期待結果: </td>
				<td>
                    <KTCC:KTTextBox ID="txtExpectResult" runat="server" MaxLength="40" CssClass="font-default txt-default ime-inactive txt-width-long" AutoUpper="False" />
				</td>
			</tr>
			<tr class="tr-no-border" style="height: 30px;">
				<td colspan="2" />
			</tr>
		</table>
		<table id="Table1">
			<colgroup>
				<col style="width: 160px" />
				<col style="width: 50%" />
				<col style="width: 25%" />
			</colgroup>
			<tr>
				<td></td>
				<td>
					<KTCC:KTButton ID="btnUpdate" runat="server" Text="更新" CssClass="btn-middle" onclick="btnUpdate_Click" OnClientClick="return true;"  />
					<KTCC:KTButton ID="ktbtnClose" runat="server" Text="戻る" CssClass="btn-middle" />
				</td>
				<td></td>
			</tr>
		</table>
	</div>
	 <UC:LoadingData ID="divLoadingData" runat="server" />
</asp:Content>

