<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/UI/MasterForm/MasterSub.master" CodeBehind="MainCorrectiveInput.aspx.cs" Inherits="TRC_W_PWT_ProductView.UI.Pages.AiImageCheck.MainCorrectiveInput" %>
<%@ Import Namespace="TRC_W_PWT_ProductView.Common" %>

<asp:Content ID="MasterScript" runat="server" ContentPlaceHolderID="MasterScript">
	<link href="../../../CSS/cobalt/jquery-wijmo.css" rel="stylesheet" />
	<link href="../../../CSS/Base.css" rel="stylesheet" />
	<link href="../../../CSS/TRC.css" rel="stylesheet" />
	<script src="<%: PageUtils.ResolveUrl( this.Page, "~/Scripts/Pages/AiImageCheck/MainCorrectiveInput.js") %>"></script>
	<script src="<%: PageUtils.ResolveUrl( this.Page, "~/Scripts/Pages/AiImageCheck/AiImageCheck.js") %>"></script>
    <script src="<%: PageUtils.ResolveUrl( this.Page, "~/Scripts/Pages/InputModal.js") %>" ></script>

</asp:Content>

<asp:Content ID="MasterBody" ContentPlaceHolderID="MasterBody" runat="server">
	<div style="width: 650px; height: 250px; margin-left: 30px;">
		<div style="margin-top: 30px; text-align: center;"></div>
		<table id="tblMain" class="table-layout">
			<colgroup>
				<col style="width: 80px" />
				<col style="width: 400px" />
			</colgroup>
			<tr>
				<td class="box-in-right">検査結果: </td>
				<td>
                    <asp:Label ID="lblAnlResult" runat="server" Text=""></asp:Label>
				</td>
			</tr>
			<tr>
				<td class="box-in-right">検査項目名: </td>
				<td>
                    <asp:Label ID="lblImgAnlItemNm" runat="server" Text=""></asp:Label>
				</td>
			</tr>
			<tr>
				<td class="box-in-right">是正結果: </td>
				<td>
					<asp:Label ID="lblCorrectiveResult" runat="server" Text=""></asp:Label>
				</td>
			</tr>
			<tr>
				<td class="box-in-right">不良タイプ: </td>
				<td>
					<KTCC:KTDropDownList ID="ddlNgType" runat="server" CssClass="font-default ddl-default" AutoPostBack="True" CausesValidation="True" />
				</td>
			</tr>
			<tr>
				<td class="box-in-right">是正タイプ: </td>
				<td>
					<KTCC:KTDropDownList ID="ddlCorrectiveType" runat="server" CssClass="font-default ddl-default" AutoPostBack="True" CausesValidation="True" OnSelectedIndexChanged="ddlCorrectiveTypeList_SelectedIndexChanged" />
				</td>
			</tr>
			<tr>
				<td class="box-in-right" style="vertical-align: top">是正コメント: </td>
				<td>
					<asp:TextBox ID="txtCorrectiveComment" runat="server" class="font-default txt-default"  MaxLength="100" Style="width: 400px; height: 40px; vertical-align: bottom" TextMode="MultiLine" OnTextChanged="txtCorrectiveComment_TextChange"  />
				</td>
			</tr>
			<tr>
				<td class="box-in-right">前回更新者: </td>
				<td>
                    <asp:Label ID="lblUpdateBy" runat="server" Text=""></asp:Label>
				</td>
			</tr>
			<tr>
				<td class="box-in-right">今回更新者: </td>
				<td>
                    <asp:Label ID="lblThisTimeUpdateBy" runat="server" Text=""></asp:Label>
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
					<KTCC:KTButton ID="btnUpdate" runat="server" Text="更新" CssClass="btn-middle" OnClick="btnUpdate_Click" />
					<KTCC:KTButton ID="ktbtnClose" runat="server" Text="戻る" CssClass="btn-middle" />
				</td>
				<td></td>
			</tr>
		</table>
	</div>
</asp:Content>
