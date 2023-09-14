<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/UI/MasterForm/MasterSub.master" CodeBehind="AnlGroupInput.aspx.cs" Inherits="TRC_W_PWT_ProductView.UI.Pages.AiImageCheck.AnlGroupInput" %>

<%@ Import Namespace="TRC_W_PWT_ProductView.Common" %>
<%@ Register src="~/UI/Pages/UserControl/LoadingData.ascx" tagname="LoadingData" tagprefix="UC" %> 
<asp:Content ID="MasterScript" runat="server" ContentPlaceHolderID="MasterScript">
	<link href="../../../CSS/cobalt/jquery-wijmo.css" rel="stylesheet" />
	<link href="../../../CSS/Base.css" rel="stylesheet" />
	<link href="../../../CSS/TRC.css" rel="stylesheet" />
	<script src="<%: PageUtils.ResolveUrl( this.Page, "~/Scripts/Pages/AiImageCheck/AnlGroupInput.js") %>"></script>
	<script src="<%: PageUtils.ResolveUrl( this.Page, "~/Scripts/Pages/InputModal.js") %>"></script>
<%--	<script src="<%: PageUtils.ResolveUrl( this.Page, "~/Scripts/Pages/AiImageCheck/SubmitControl.js") %>" ></script>
    <script src="<%: PageUtils.ResolveUrl( this.Page, "~/Scripts/Pages/AiImageCheck/Ready.js") %>" ></script>--%>

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
				<td class="box-in-right">画像解析グループコード: </td>
				<td>
					<asp:Label ID="txtGroupCd" runat="server" Text=""></asp:Label>
				</td>
			</tr>
			<tr>
				<td class="box-in-right">画像解析グループ名: </td>
				<td>
					<KTCC:KTTextBox ID="txtGroupNm" runat="server" MaxLength="40" CssClass="font-default txt-default ime-inactive txt-width-long" AutoUpper="False" />
				</td>
			</tr>
			<tr>
				<td class="box-in-right">設備: </td>
				<td>
					<asp:Label ID="txtSetubi" runat="server" Text=""></asp:Label>
				</td>
			</tr>
			<tr>
				<td class="box-in-right">ライン: </td>
				<td>
					<asp:Label ID="txtLineNm" runat="server" Text=""></asp:Label>
				</td>
			</tr>
			<tr>
				<td class="box-in-right">ステーション: </td>
				<td>
					<asp:Label ID="txtStationNm" runat="server" Text=""></asp:Label>
				</td>
			</tr>
			<tr>
				<td id="thJyoho1" class="print-show box-in-right">
					<asp:Label ID="lJyoho1" runat="server" Text="Jyoho1: " Visible="true"></asp:Label>
				</td>
				<td id="tdJyoho1" class="print-show">
					<KTCC:KTTextBox ID="txtJyoho1"  runat="server" CssClass="font-default txt-default txt-width-long"  AutoUpper="False" />
				</td>
			</tr>
			<tr>
				<td id="thJyoho2" class="print-show box-in-right">
					<asp:Label ID="lJyoho2" runat="server" Text="Jyoho2: " Visible="true"></asp:Label>
				</td>
				<td id="tdJyoho2" class="print-show">
					<KTCC:KTTextBox ID="txtJyoho2" runat="server" CssClass="font-default txt-default ime-inactive txt-width-long" Value="" AutoUpper="False" />
				</td>
			</tr>
			<tr>
				<td id="thJyoho3" class="print-show box-in-right">
					<asp:Label ID="lJyoho3" runat="server" Text="Jyoho3: " Visible="true"></asp:Label>
				</td>
				<td id="tdJyoho3" class="print-show">
					<KTCC:KTTextBox ID="txtJyoho3" runat="server" CssClass="font-default txt-default ime-inactive txt-width-long" Value="" AutoUpper="False" />
				</td>
			</tr>
			<tr>
				<td id="thJyoho4" class="print-show box-in-right">
					<asp:Label ID="lJyoho4" runat="server" Text="Jyoho4: " Visible="true"></asp:Label>
				</td>
				<td id="tdJyoho4" class="print-show">
					<KTCC:KTTextBox ID="txtJyoho4" runat="server" CssClass="font-default txt-default ime-inactive txt-width-long" Value="" AutoUpper="False" />
				</td>
			</tr>
			<tr>
				<td id="thJyoho5" class="print-show box-in-right">
					<asp:Label ID="lJyoho5" runat="server" Text="Jyoho5: " Visible="true"></asp:Label>
				</td>
				<td id="tdJyoho5" class="print-show">
					<KTCC:KTTextBox ID="txtJyoho5" runat="server" CssClass="font-default txt-default ime-inactive txt-width-long" Value="" AutoUpper="False" />
				</td>
			</tr>
			<tr>
				<td id="thJyoho6" class="print-show box-in-right">
					<asp:Label ID="lJyoho6" runat="server" Text="Jyoho6: " Visible="true"></asp:Label>
				</td>
				<td id="tdJyoho6" class="print-show">
					<KTCC:KTTextBox ID="txtJyoho6" runat="server" CssClass="font-default txt-default ime-inactive txt-width-long" Value="" AutoUpper="False" />
				</td>
			</tr>
			<tr>
				<td id="thJyoho7" class="print-show box-in-right">
					<asp:Label ID="lJyoho7" runat="server" Text="Jyoho7: " Visible="true"></asp:Label>
				</td>
				<td id="tdJyoho7" class="print-show">
					<KTCC:KTTextBox ID="txtJyoho7" runat="server" CssClass="font-default txt-default ime-inactive txt-width-long" Value="" AutoUpper="False" />
				</td>
			</tr>
			<tr>
				<td id="thJyoho8" class="print-show box-in-right">
					<asp:Label ID="lJyoho8" runat="server" Text="Jyoho8: " Visible="true"></asp:Label>
				</td>
				<td id="tdJyoho8" class="print-show">
					<KTCC:KTTextBox ID="txtJyoho8" runat="server" CssClass="font-default txt-default ime-inactive txt-width-long" Value="" AutoUpper="False" />
				</td>
			</tr>
			<tr>
				<td id="thJyoho9" class="print-show box-in-right">
					<asp:Label ID="lJyoho9" runat="server" Text="Jyoho9: " Visible="true"></asp:Label>
				</td>
				<td id="tdJyoho9" class="print-show">
					<KTCC:KTTextBox ID="txtJyoho9" runat="server" CssClass="font-default txt-default ime-inactive txt-width-long" Value="" AutoUpper="False" />
				</td>
			</tr>
			<tr>
				<td id="thJyoho10" class="print-show box-in-right">
					<asp:Label ID="lJyoho10" runat="server" Text="Jyoho10: " Visible="true"></asp:Label>
				</td>
				<td id="tdJyoho10" class="print-show">
					<KTCC:KTTextBox ID="txtJyoho10" runat="server" CssClass="font-default txt-default ime-inactive txt-width-long" Value="" AutoUpper="False" />
				</td>
			</tr>
			<tr>
				<td id="thJyoho11" class="print-show box-in-right">
					<asp:Label ID="lJyoho11" runat="server" Text="Jyoho11: " Visible="true"></asp:Label>
				</td>
				<td id="tdJyoho11" class="print-show">
					<KTCC:KTTextBox ID="txtJyoho11" runat="server" CssClass="font-default txt-default ime-inactive txt-width-long" Value="" AutoUpper="False" />
				</td>
			</tr>
			<tr>
				<td id="thJyoho12" class="print-show box-in-right">
					<asp:Label ID="lJyoho12" runat="server" Text="Jyoho12: " Visible="true"></asp:Label>
				</td>
				<td id="tdJyoho12" class="print-show">
					<KTCC:KTTextBox ID="txtJyoho12" runat="server" CssClass="font-default txt-default ime-inactive txt-width-long" Value="" AutoUpper="False" />
				</td>
			</tr>
			<tr>
				<td id="thJyoho13" class="print-show box-in-right">
					<asp:Label ID="lJyoho13" runat="server" Text="Jyoho13: " Visible="true"></asp:Label>
				</td>
				<td id="tdJyoho13" class="print-show">
					<KTCC:KTTextBox ID="txtJyoho13" runat="server" CssClass="font-default txt-default ime-inactive txt-width-long" Value="" AutoUpper="False" />
				</td>
			</tr>
			<tr>
				<td id="thJyoho14" class="print-show box-in-right">
					<asp:Label ID="lJyoho14" runat="server" Text="Jyoho14: " Visible="true"></asp:Label>
				</td>
				<td id="tdJyoho14" class="print-show">
					<KTCC:KTTextBox ID="txtJyoho14" runat="server" CssClass="font-default txt-default ime-inactive txt-width-long" Value="" AutoUpper="False" />
				</td>
			</tr>
			<tr>
				<td id="thJyoho15" class="print-show box-in-right">
					<asp:Label ID="lJyoho15" runat="server" Text="Jyoho15: " Visible="true"></asp:Label>
				</td>
				<td id="tdJyoho15" class="print-show">
					<KTCC:KTTextBox ID="txtJyoho15" runat="server" CssClass="font-default txt-default ime-inactive txt-width-long" Value="" AutoUpper="False" />
				</td>
			</tr>
			<tr>
				<td id="thJyoho16" class="print-show box-in-right">
					<asp:Label ID="lJyoho16" runat="server" Text="Jyoho16: " Visible="true"></asp:Label>
				</td>
				<td id="tdJyoho16" class="print-show">
					<KTCC:KTTextBox ID="txtJyoho16" runat="server" CssClass="font-default txt-default ime-inactive txt-width-long" Value="" AutoUpper="False" />
				</td>
			</tr>
			<tr>
				<td id="thJyoho17" class="print-show box-in-right">
					<asp:Label ID="lJyoho17" runat="server" Text="Jyoho17: " Visible="true"></asp:Label>
				</td>
				<td id="tdJyoho17" class="print-show">
					<KTCC:KTTextBox ID="txtJyoho17" runat="server" CssClass="font-default txt-default ime-inactive txt-width-long" Value="" AutoUpper="False" />
				</td>
			</tr>
			<tr>
				<td id="thJyoho18" class="print-show box-in-right">
					<asp:Label ID="lJyoho18" runat="server" Text="Jyoho18: " Visible="true"></asp:Label>
				</td>
				<td id="tdJyoho18" class="print-show">
					<KTCC:KTTextBox ID="txtJyoho18" runat="server" CssClass="font-default txt-default ime-inactive txt-width-long" Value="" AutoUpper="False" />
				</td>
			</tr>
			<tr>
				<td id="thJyoho19" class="print-show box-in-right">
					<asp:Label ID="lJyoho19" runat="server" Text="Jyoho19: " Visible="true"></asp:Label>
				</td>
				<td id="tdJyoho19" class="print-show">
					<KTCC:KTTextBox ID="txtJyoho19" runat="server" CssClass="font-default txt-default ime-inactive txt-width-long" Value="" AutoUpper="False" />
				</td>
			</tr>
			<tr>
				<td id="thJyoho20" class="print-show box-in-right">
					<asp:Label ID="lJyoho20" runat="server" Text="Jyoho20: " Visible="true"></asp:Label>
				</td>
				<td id="tdJyoho20" class="print-show">
					<KTCC:KTTextBox ID="txtJyoho20" runat="server" CssClass="font-default txt-default ime-inactive txt-width-long" Value="" AutoUpper="False" />
				</td>
			</tr>
			<tr>
				<td id="thJyoho21" class="print-show box-in-right">
					<asp:Label ID="lJyoho21" runat="server" Text="Jyoho21: " Visible="true"></asp:Label>
				</td>
				<td id="tdJyoho21" class="print-show">
					<KTCC:KTTextBox ID="txtJyoho21" runat="server" CssClass="font-default txt-default ime-inactive txt-width-long" Value="" AutoUpper="False" />
				</td>
			</tr>
			<tr>
				<td id="thJyoho22" class="print-show box-in-right">
					<asp:Label ID="lJyoho22" runat="server" Text="Jyoho22: " Visible="true"></asp:Label>
				</td>
				<td id="tdJyoho22" class="print-show">
					<KTCC:KTTextBox ID="txtJyoho22" runat="server" CssClass="font-default txt-default ime-inactive txt-width-long" Value="" AutoUpper="False" />
				</td>
			</tr>
			<tr>
				<td id="thJyoho23" class="print-show box-in-right">
					<asp:Label ID="lJyoho23" runat="server" Text="Jyoho23: " Visible="true"></asp:Label>
				</td>
				<td id="tdJyoho23" class="print-show">
					<KTCC:KTTextBox ID="txtJyoho23" runat="server" CssClass="font-default txt-default ime-inactive txt-width-long" Value="" AutoUpper="False" />
				</td>
			</tr>
			<tr>
				<td id="thJyoho24" class="print-show box-in-right">
					<asp:Label ID="lJyoho24" runat="server" Text="Jyoho24: " Visible="true"></asp:Label>
				</td>
				<td id="tdJyoho24" class="print-show">
					<KTCC:KTTextBox ID="txtJyoho24" runat="server" CssClass="font-default txt-default ime-inactive txt-width-long" Value="" AutoUpper="False" />
				</td>
			</tr>
			<tr>
				<td id="thJyoho25" class="print-show box-in-right">
					<asp:Label ID="lJyoho25" runat="server" Text="Jyoho25: " Visible="true"></asp:Label>
				</td>
				<td id="tdJyoho25" class="print-show">
					<KTCC:KTTextBox ID="txtJyoho25" runat="server" CssClass="font-default txt-default ime-inactive txt-width-long" Value="" AutoUpper="False" />
				</td>
			</tr>
			<tr>
				<td id="thJyoho26" class="print-show box-in-right">
					<asp:Label ID="lJyoho26" runat="server" Text="Jyoho26: " Visible="true"></asp:Label>
				</td>
				<td id="tdJyoho26" class="print-show">
					<KTCC:KTTextBox ID="txtJyoho26" runat="server" CssClass="font-default txt-default ime-inactive txt-width-long" Value="" AutoUpper="False" />
				</td>
			</tr>
			<tr>
				<td id="thJyoho27" class="print-show box-in-right">
					<asp:Label ID="lJyoho27" runat="server" Text="Jyoho27: " Visible="true"></asp:Label>
				</td>
				<td id="tdJyoho27" class="print-show">
					<KTCC:KTTextBox ID="txtJyoho27" runat="server" CssClass="font-default txt-default ime-inactive txt-width-long" Value="" AutoUpper="False" />
				</td>
			</tr>
			<tr>
				<td id="thJyoho28" class="print-show box-in-right">
					<asp:Label ID="lJyoho28" runat="server" Text="Jyoho28: " Visible="true"></asp:Label>
				</td>
				<td id="tdJyoho28" class="print-show">
					<KTCC:KTTextBox ID="txtJyoho28" runat="server" CssClass="font-default txt-default ime-inactive txt-width-long" Value="" AutoUpper="False" />
				</td>
			</tr>
			<tr>
				<td id="thJyoho29" class="print-show box-in-right">
					<asp:Label ID="lJyoho29" runat="server" Text="Jyoho29: " Visible="true"></asp:Label>
				</td>
				<td id="tdJyoho29" class="print-show">
					<KTCC:KTTextBox ID="txtJyoho29" runat="server" CssClass="font-default txt-default ime-inactive txt-width-long" Value="" AutoUpper="False" />
				</td>
				
			</tr>
		</table>

	</div>
	<div style="display: none">
		<KTCC:KTButton ID="btnViewRefresh" runat="server" OnClick="btnViewRefresh_Click" />
		<asp:TextBox ID="hiddenNum" runat="server" />
	</div>
	<UC:LoadingData ID="divLoadingData" runat="server" />
</asp:Content>
<asp:Content ID="MasterBodyBottom" ContentPlaceHolderID="MasterBodyBottom" runat="server">
	<div>
		<div id="div1" runat="server" class="div-bottom-button-area">
			<KTCC:KTButton ID="btnUpdate" runat="server" Text="更新" CssClass="btn-middle" OnClick="btnUpdate_Click" style="margin-right:20px" />
			<KTCC:KTButton ID="ktbtnClose" runat="server" Text="閉じる" CssClass="btn-middle" />
		</div>
	</div>
</asp:Content>
