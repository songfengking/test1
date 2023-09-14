<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/UI/MasterForm/MasterSub.master" CodeBehind="AnlGroupListInput.aspx.cs" Inherits="TRC_W_PWT_ProductView.UI.Pages.AiImageCheck.AnlGroupListInput" %>

<%@ Import Namespace="TRC_W_PWT_ProductView.Common" %>
<%@ Register src="~/UI/Pages/UserControl/LoadingData.ascx" tagname="LoadingData" tagprefix="UC" %> 
<asp:Content ID="MasterScript" runat="server" ContentPlaceHolderID="MasterScript">
	<script src="<%: PageUtils.ResolveUrl( this.Page, "~/Scripts/ControlCommon.js") %>"></script>
	<script src="<%: PageUtils.ResolveUrl( this.Page, "~/Scripts/Pages/AiImageCheck/AnlGroupListInput.js") %>"></script>
	<script src="<%: PageUtils.ResolveUrl( this.Page, "~/Scripts/Pages/AiImageCheck/AiImageCheck.js") %>"></script>
	<script src="<%: PageUtils.ResolveUrl( this.Page, "~/Scripts/Pages/AiImageCheck/SubmitControl.js") %>" ></script>
    <script src="<%: PageUtils.ResolveUrl( this.Page, "~/Scripts/Pages/AiImageCheck/Ready.js") %>" ></script>
</asp:Content>
<asp:Content ID="MasterBodyTop" ContentPlaceHolderID="MasterBodyTop" runat="server">
	<%-- 画面タイトル --%>
</asp:Content>
<asp:Content ID="MasterBody" ContentPlaceHolderID="MasterBody" runat="server">
	<div class="condition-area">
		<table class="table-layout" style="width: 340px">
			<tr class="tr-fix-zero-height">
				<td style="width: 220px"></td>
				<td style="width: 220px"></td>
				<td style="width: 220px"></td>
				<td style="width: 220px"></td>
				<td style="width: 220px"></td>
			</tr>
			<tr class="tr-ctrl-height">
				<td align="right">画像解析項目コード：</td>
				<td>
					<KTCC:KTTextBox ID="txtItemCd" runat="server" InputMode="IntNum" MaxLength="10" CssClass="font-default txt-default ime-inactive txt-width-long" AutoUpper="False" />
				</td>
				<td align="right">画像解析項目名：</td>
				<td>
					<KTCC:KTTextBox ID="txtItemNm" runat="server" InputMode="HalfKana" MaxLength="40" CssClass="font-default txt-default ime-inactive txt-width-long" AutoUpper="False" />
				</td>
			</tr>
			<tr class="tr-ctrl-height">
				<td align="right">利用ツール：</td>
				<td>
					<KTCC:KTDropDownList ID="ddlUsingTool" runat="server" CssClass="font-default ddl-width-long ddl-default" AutoPostBack="True" CausesValidation="True" Style="width: 100px" />
				</td>
				<td></td>
			</tr>

		</table>
	</div>
	<div class="condition-button-area">
		<KTCC:KTButton ID="btnSearch" runat="server" Text="検索" CssClass="btn-middle" OnClick="btnSearch_Click" />
	</div>
	<div style="clear: both; height: 0px;"></div>
	<div id="divGrvDisplay" runat="server">
		<table class="table-layout-fix">
			<tr>
				<td>
					<div id="divLTScroll" class="div-fix-scroll div-left-grid">
						<div id="divGrvHeaderLT" runat="server">
							<asp:GridView ID="grvHeaderLT" runat="server" CssClass="grid-layout2 ui-widget-content" AllowPaging="false" AllowSorting="false" PageSize="10" AutoGenerateColumns="false">
								<HeaderStyle CssClass="grid-header ui-state-default" />
							</asp:GridView>
						</div>
					</div>
				</td>
				<td>
					<div id="divRTScroll" class="div-scroll-right-top div-right-grid">
						<div id="divGrvHeaderRT" runat="server">
							<asp:GridView ID="grvHeaderRT" runat="server" CssClass="grid-layout2 ui-widget-content" AllowPaging="false" AllowSorting="false" PageSize="10" AutoGenerateColumns="false">
								<HeaderStyle CssClass="grid-header ui-state-default" />
							</asp:GridView>
						</div>
					</div>
				</td>
			</tr>
			<tr>
				<td>
					<div id="divLBScroll" class=" div-left-grid">
						<div id="divGrvLB" runat="server">
							<asp:GridView ID="grvMainViewLB" runat="server" ShowHeader="false" CssClass="grid-layout2 ui-widget-content" AllowPaging="true" AllowSorting="false" PageSize="1000" AutoGenerateColumns="false" OnRowDataBound="grvMainViewLB_RowDataBound">
								<PagerSettings Position="TopAndBottom" Mode="NumericFirstLast" PageButtonCount="1000" Visible="false" />
								<HeaderStyle CssClass="grid-header ui-state-default" />
								<RowStyle CssClass="grid-row ui-widget" />
								<SelectedRowStyle CssClass="ui-state-highlight" />
								<Columns>
									<asp:TemplateField HeaderText="ﾁｪｯｸ">
										<ItemTemplate>
											<asp:CheckBox ID="chkUpdate" runat="server" />
										</ItemTemplate>
									</asp:TemplateField>
									<asp:TemplateField HeaderText="ITEM_CD" />
									<asp:TemplateField HeaderText="ITEM_NM" />
									<asp:TemplateField HeaderText="INDEX_NO">
										<ItemTemplate>
											<KTCC:KTTextBox ID="inputIndexNo" runat="server" InputMode="IntNum" CssClass="font-default txt-default ime-active txt-width-full-input" />
										</ItemTemplate>
									</asp:TemplateField>
									<asp:TemplateField HeaderText="SFLAG">
										<ItemTemplate>
											<KTCC:KTDropDownList ID="inputSFlag" runat="server" CssClass="font-default ddl-default ddl-width-full" />
										</ItemTemplate>
									</asp:TemplateField>
									<asp:TemplateField HeaderText="CFLAG">
										<ItemTemplate>
											<KTCC:KTDropDownList ID="inputCFlag" runat="server" CssClass="font-default ddl-default ddl-width-full" />
										</ItemTemplate>
									</asp:TemplateField>
									<asp:TemplateField HeaderText="USING_TOOL" />
									<asp:TemplateField HeaderText="INDEX_NO_ENABLE" />
									<asp:TemplateField HeaderText="SFLAG_ENABLE" />
									<asp:TemplateField HeaderText="CFLAG_ENABLE" />
								</Columns>
							</asp:GridView>
						</div>
					</div>
				</td>
			</tr>
		</table>
	</div>
	<UC:LoadingData ID="divLoadingData" runat="server" />
	<div style="display: none">
		<asp:TextBox ID="allCheckState" Text="false" runat="server" />
	</div>
</asp:Content>
<asp:Content ID="MasterBodyBottom" ContentPlaceHolderID="MasterBodyBottom" runat="server">
	<div class="div-bottom-button-area">
		<KTCC:KTButton ID="btnAllCheck" runat="server" Text="すべて選択" CssClass="btn-middle" OnClientClick="AnlGroupListInput.AllCheck(); return false;" />
		<KTCC:KTButton ID="btnInsert" runat="server" Text="追加" CssClass="btn-middle" OnClick="btnInsert_Click" />
		<KTCC:KTButton ID="btnCancel" runat="server" Text="閉じる" CssClass="btn-middle" />
	</div>
</asp:Content>
