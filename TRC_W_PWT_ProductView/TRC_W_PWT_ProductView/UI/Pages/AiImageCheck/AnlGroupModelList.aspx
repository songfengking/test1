<%@ Page Language="C#" MasterPageFile="~/UI/MasterForm/MasterMain.master" AutoEventWireup="true" CodeBehind="AnlGroupModelList.aspx.cs" Inherits="TRC_W_PWT_ProductView.UI.Pages.AiImageCheck.AnlGroupModelList" %>

<%@ Register Src="~/UI/Pages/UserControl/InputModal.ascx" TagName="InputModal" TagPrefix="UC" %>
<%@ Import Namespace="TRC_W_PWT_ProductView.Common" %>

<asp:Content ID="MasterScript" ContentPlaceHolderID="MasterScript" runat="server">
	<script src="<%: PageUtils.ResolveUrl( this.Page, "~/Scripts/Pages/AiImageCheck/AnlGroupModelList.js") %>"></script>
	<script src="<%: PageUtils.ResolveUrl( this.Page, "~/Scripts/Pages/AiImageCheck/AiImageCheck.js") %>"></script>
</asp:Content>

<asp:Content ID="MasterBody" ContentPlaceHolderID="MasterBody" runat="server">
	<%-- 検索条件 --%>
	<div>
		<table class="table-layout">
			<tr>
				<td>
					<div class="box-in-margin-small">
						<div style="clear: both;"></div>
						<div id="divTopScroll" class="div-visible-scroll div-right-grid">
							<div class="div-detail-info-margin" style="overflow: auto">
								<table class="table-border-layout">

									<tr class="font-default tr-content-header">
										<td>
											<asp:Label ID="lLineNm" runat="server" Text="ライン"></asp:Label>
										</td>
										<td>
											<asp:Label ID="lStationNm" runat="server" Text="ステーション"></asp:Label>
										</td>
										<td>
											<asp:Label ID="lSetubi" runat="server" Text="設備"></asp:Label>
										</td>
										<td>
											<asp:Label ID="lGroupCd" runat="server" Text="画像解析グループコード"></asp:Label>
										</td>
										<td>
											<asp:Label ID="lGroupNm" runat="server" Text="画像解析グループ名"></asp:Label>
										</td>
										<td id="thJyoho11" class="print-show">
											<asp:Label ID="lJyoho1" runat="server" Text="Jyoho1" Visible="true"></asp:Label>
										</td>
										<td id="thJyoho12" class="print-show">
											<asp:Label ID="lJyoho2" runat="server" Text="Jyoho2" Visible="true"></asp:Label>
										</td>
										<td id="thJyoho13" class="print-show">
											<asp:Label ID="lJyoho3" runat="server" Text="Jyoho3" Visible="true"></asp:Label>
										</td>
										<td id="thJyoho14" class="print-show">
											<asp:Label ID="lJyoho4" runat="server" Text="Jyoho4" Visible="true"></asp:Label>
										</td>
										<td id="thJyoho15" class="print-show">
											<asp:Label ID="lJyoho5" runat="server" Text="Jyoho5" Visible="true"></asp:Label>
										</td>
										<td id="thJyoho16" class="print-show">
											<asp:Label ID="lJyoho6" runat="server" Text="Jyoho6" Visible="true"></asp:Label>
										</td>
										<td id="thJyoho17" class="print-show">
											<asp:Label ID="lJyoho7" runat="server" Text="Jyoho7" Visible="true"></asp:Label>
										</td>
										<td id="thJyoho18" class="print-show">
											<asp:Label ID="lJyoho8" runat="server" Text="Jyoho8" Visible="true"></asp:Label>
										</td>
										<td id="thJyoho19" class="print-show">
											<asp:Label ID="lJyoho9" runat="server" Text="Jyoho9" Visible="true"></asp:Label>
										</td>
										<td id="thJyoho110" class="print-show">
											<asp:Label ID="lJyoho10" runat="server" Text="Jyoho10" Visible="true"></asp:Label>
										</td>
										<td id="thJyoho111" class="print-show">
											<asp:Label ID="lJyoho11" runat="server" Text="Jyoho11" Visible="true"></asp:Label>
										</td>
										<td id="thJyoho112" class="print-show">
											<asp:Label ID="lJyoho12" runat="server" Text="Jyoho12" Visible="true"></asp:Label>
										</td>
										<td id="thJyoho113" class="print-show">
											<asp:Label ID="lJyoho13" runat="server" Text="Jyoho13" Visible="true"></asp:Label>
										</td>
										<td id="thJyoho114" class="print-show">
											<asp:Label ID="lJyoho14" runat="server" Text="Jyoho14" Visible="true"></asp:Label>
										</td>
										<td id="thJyoho115" class="print-show">
											<asp:Label ID="lJyoho15" runat="server" Text="Jyoho15" Visible="true"></asp:Label>
										</td>
										<td id="thJyoho116" class="print-show">
											<asp:Label ID="lJyoho16" runat="server" Text="Jyoho16" Visible="true"></asp:Label>
										</td>
										<td id="thJyoho117" class="print-show">
											<asp:Label ID="lJyoho17" runat="server" Text="Jyoho17" Visible="true"></asp:Label>
										</td>
										<td id="thJyoho118" class="print-show">
											<asp:Label ID="lJyoho18" runat="server" Text="Jyoho18" Visible="true"></asp:Label>
										</td>
										<td id="thJyoho119" class="print-show">
											<asp:Label ID="lJyoho19" runat="server" Text="Jyoho19" Visible="true"></asp:Label>
										</td>
										<td id="thJyoho120" class="print-show">
											<asp:Label ID="lJyoho20" runat="server" Text="Jyoho20" Visible="true"></asp:Label>
										</td>
										<td id="thJyoho121" class="print-show">
											<asp:Label ID="lJyoho21" runat="server" Text="Jyoho21" Visible="true"></asp:Label>
										</td>
										<td id="thJyoho122" class="print-show">
											<asp:Label ID="lJyoho22" runat="server" Text="Jyoho22" Visible="true"></asp:Label>
										</td>
										<td id="thJyoho123" class="print-show">
											<asp:Label ID="lJyoho23" runat="server" Text="Jyoho23" Visible="true"></asp:Label>
										</td>
										<td id="thJyoho124" class="print-show">
											<asp:Label ID="lJyoho24" runat="server" Text="Jyoho24" Visible="true"></asp:Label>
										</td>
										<td id="thJyoho125" class="print-show">
											<asp:Label ID="lJyoho25" runat="server" Text="Jyoho25" Visible="true"></asp:Label>
										</td>
										<td id="thJyoho126" class="print-show">
											<asp:Label ID="lJyoho26" runat="server" Text="Jyoho26" Visible="true"></asp:Label>
										</td>
										<td id="thJyoho127" class="print-show">
											<asp:Label ID="lJyoho27" runat="server" Text="Jyoho27" Visible="true"></asp:Label>
										</td>
										<td id="thJyoho128" class="print-show">
											<asp:Label ID="lJyoho28" runat="server" Text="Jyoho28" Visible="true"></asp:Label>
										</td>
										<td id="thJyoho129" class="print-show">
											<asp:Label ID="lJyoho29" runat="server" Text="Jyoho29" Visible="true"></asp:Label>
										</td>
									</tr>
									<tr class="tr-ctrl-height">
										<td>
											<KTCC:KTTextBox ID="txtLineNm" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
										</td>
										<td>
											<KTCC:KTTextBox ID="txtStationNm" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
										</td>
										<td>
											<KTCC:KTTextBox ID="txtSetubi" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
										</td>
										<td>
											<KTCC:KTTextBox ID="txtGroupCd" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
										</td>
										<td>
											<KTCC:KTTextBox ID="txtGroupNm" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
										</td>
										<td id="tdJyoho1" class="print-show">
											<KTCC:KTTextBox ID="txtJyoho1" runat="server" ReadOnly="true" Visible="true" CssClass="font-default txt-default txt-width-full al-ct" />
										</td>
										<td id="tdJyoho2" class="print-show">
											<KTCC:KTTextBox ID="txtJyoho2" runat="server" ReadOnly="true" Visible="true" CssClass="font-default txt-default txt-width-full al-ct" />
										</td>
										<td id="tdJyoho3" class="print-show">
											<KTCC:KTTextBox ID="txtJyoho3" runat="server" ReadOnly="true" Visible="true" CssClass="font-default txt-default txt-width-full al-ct" />
										</td>
										<td id="tdJyoho4" class="print-show">
											<KTCC:KTTextBox ID="txtJyoho4" runat="server" ReadOnly="true" Visible="true" CssClass="font-default txt-default txt-width-full al-ct" />
										</td>
										<td id="tdJyoho5" class="print-show">
											<KTCC:KTTextBox ID="txtJyoho5" runat="server" ReadOnly="true" Visible="true" CssClass="font-default txt-default txt-width-full al-ct" />
										</td>
										<td id="tdJyoho6" class="print-show">
											<KTCC:KTTextBox ID="txtJyoho6" runat="server" ReadOnly="true" Visible="true" CssClass="font-default txt-default txt-width-full al-ct" />
										</td>
										<td id="tdJyoho7" class="print-show">
											<KTCC:KTTextBox ID="txtJyoho7" runat="server" ReadOnly="true" Visible="true" CssClass="font-default txt-default txt-width-full al-ct" />
										</td>
										<td id="tdJyoho8" class="print-show">
											<KTCC:KTTextBox ID="txtJyoho8" runat="server" ReadOnly="true" Visible="true" CssClass="font-default txt-default txt-width-full al-ct" />
										</td>
										<td id="tdJyoho9" class="print-show">
											<KTCC:KTTextBox ID="txtJyoho9" runat="server" ReadOnly="true" Visible="true" CssClass="font-default txt-default txt-width-full al-ct" />
										</td>
										<td id="tdJyoho10" class="print-show">
											<KTCC:KTTextBox ID="txtJyoho10" runat="server" ReadOnly="true" Visible="true" CssClass="font-default txt-default txt-width-full al-ct" />
										</td>
										<td id="tdJyoho11" class="print-show">
											<KTCC:KTTextBox ID="txtJyoho11" runat="server" ReadOnly="true" Visible="true" CssClass="font-default txt-default txt-width-full al-ct" />
										</td>
										<td id="tdJyoho12" class="print-show">
											<KTCC:KTTextBox ID="txtJyoho12" runat="server" ReadOnly="true" Visible="true" CssClass="font-default txt-default txt-width-full al-ct" />
										</td>
										<td id="tdJyoho13" class="print-show">
											<KTCC:KTTextBox ID="txtJyoho13" runat="server" ReadOnly="true" Visible="true" CssClass="font-default txt-default txt-width-full al-ct" />
										</td>
										<td id="tdJyoho14" class="print-show">
											<KTCC:KTTextBox ID="txtJyoho14" runat="server" ReadOnly="true" Visible="true" CssClass="font-default txt-default txt-width-full al-ct" />
										</td>
										<td id="tdJyoho15" class="print-show">
											<KTCC:KTTextBox ID="txtJyoho15" runat="server" ReadOnly="true" Visible="true" CssClass="font-default txt-default txt-width-full al-ct" />
										</td>
										<td id="tdJyoho16" class="print-show">
											<KTCC:KTTextBox ID="txtJyoho16" runat="server" ReadOnly="true" Visible="true" CssClass="font-default txt-default txt-width-full al-ct" />
										</td>
										<td id="tdJyoho17" class="print-show">
											<KTCC:KTTextBox ID="txtJyoho17" runat="server" ReadOnly="true" Visible="true" CssClass="font-default txt-default txt-width-full al-ct" />
										</td>
										<td id="tdJyoho18" class="print-show">
											<KTCC:KTTextBox ID="txtJyoho18" runat="server" ReadOnly="true" Visible="true" CssClass="font-default txt-default txt-width-full al-ct" />
										</td>
										<td id="tdJyoho19" class="print-show">
											<KTCC:KTTextBox ID="txtJyoho19" runat="server" ReadOnly="true" Visible="true" CssClass="font-default txt-default txt-width-full al-ct" />
										</td>
										<td id="tdJyoho20" class="print-show">
											<KTCC:KTTextBox ID="txtJyoho20" runat="server" ReadOnly="true" Visible="true" CssClass="font-default txt-default txt-width-full al-ct" />
										</td>
										<td id="tdJyoho21" class="print-show">
											<KTCC:KTTextBox ID="txtJyoho21" runat="server" ReadOnly="true" Visible="true" CssClass="font-default txt-default txt-width-full al-ct" />
										</td>
										<td id="tdJyoho22" class="print-show">
											<KTCC:KTTextBox ID="txtJyoho22" runat="server" ReadOnly="true" Visible="true" CssClass="font-default txt-default txt-width-full al-ct" />
										</td>
										<td id="tdJyoho23" class="print-show">
											<KTCC:KTTextBox ID="txtJyoho23" runat="server" ReadOnly="true" Visible="true" CssClass="font-default txt-default txt-width-full al-ct" />
										</td>
										<td id="tdJyoho24" class="print-show">
											<KTCC:KTTextBox ID="txtJyoho24" runat="server" ReadOnly="true" Visible="true" CssClass="font-default txt-default txt-width-full al-ct" />
										</td>
										<td id="tdJyoho25" class="print-show">
											<KTCC:KTTextBox ID="txtJyoho25" runat="server" ReadOnly="true" Visible="true" CssClass="font-default txt-default txt-width-full al-ct" />
										</td>
										<td id="tdJyoho26" class="print-show">
											<KTCC:KTTextBox ID="txtJyoho26" runat="server" ReadOnly="true" Visible="true" CssClass="font-default txt-default txt-width-full al-ct" />
										</td>
										<td id="tdJyoho27" class="print-show">
											<KTCC:KTTextBox ID="txtJyoho27" runat="server" ReadOnly="true" Visible="true" CssClass="font-default txt-default txt-width-full al-ct" />
										</td>
										<td id="tdJyoho28" class="print-show">
											<KTCC:KTTextBox ID="txtJyoho28" runat="server" ReadOnly="true" Visible="true" CssClass="font-default txt-default txt-width-full al-ct" />
										</td>
										<td id="tdJyoho29" class="print-show">
											<KTCC:KTTextBox ID="txtJyoho29" runat="server" ReadOnly="true" Visible="true" CssClass="font-default txt-default txt-width-full al-ct" />
										</td>
									</tr>
								</table>
							</div>
						</div>
					</div>
				</td>
			</tr>
			<tr>
				<td>
					<div class="ui-widget-header header-title-area">
						<span class="lbltitle">一覧</span>
					</div>
					<div class="div-grid-results">
						<div class="div-result-pager">
							<asp:Panel ID="pnlPager" runat="server" EnableViewState="true"></asp:Panel>
						</div>

						<div id="divgrvCount" class="div-result-count">
							<span>件数：</span>
							<KTCC:KTNumericTextBox ID="ntbResultCount" runat="server" CssClass="txt-center-num" ReadOnly="true" />
							<span>件</span>
						</div>
					</div>
				</td>
			</tr>
		</table>
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
					<div id="divLBScroll" class="div-scroll-left-bottom div-left-grid">
						<div id="divGrvLB" runat="server">
							<asp:GridView ID="grvMainViewLB" runat="server" ShowHeader="false" CssClass="grid-layout2 ui-widget-content" AllowPaging="true" AllowSorting="false" PageSize="50" AutoGenerateColumns="false" OnRowDataBound="grvMainViewLB_RowDataBound">
								<PagerSettings Position="TopAndBottom" Mode="NumericFirstLast" PageButtonCount="50" Visible="false" />
								<HeaderStyle CssClass="grid-header ui-state-default" />
								<RowStyle CssClass="grid-row ui-widget" />
								<SelectedRowStyle CssClass="ui-state-highlight" />
								<Columns>
									<asp:TemplateField HeaderText="ﾁｪｯｸ">
										<ItemTemplate>
											<asp:CheckBox ID="chkUpdate" runat="server" />
										</ItemTemplate>
									</asp:TemplateField>
									<asp:TemplateField HeaderText="MODEL_CD" />
									<asp:TemplateField HeaderText="MODEL_NM" />
									<asp:TemplateField HeaderText="COUNTRY_CD" />
								</Columns>
							</asp:GridView>
						</div>
					</div>
				</td>
				<td>
					<div id="divRBScroll" class="div-visible-scroll div-right-grid">
						<div id="divGrvRB" runat="server">
							<asp:GridView ID="grvMainViewRB" runat="server" ShowHeader="false" CssClass="grid-layout2 ui-widget-content" AllowPaging="true" AllowSorting="false" PageSize="50" AutoGenerateColumns="false" OnRowDataBound="grvMainViewRB_RowDataBound">
								<PagerSettings Position="TopAndBottom" Mode="NumericFirstLast" PageButtonCount="50" Visible="false" />
								<HeaderStyle CssClass="grid-header ui-state-default" />
								<RowStyle CssClass="grid-row ui-widget" />
								<SelectedRowStyle CssClass="ui-state-highlight" />
								<Columns>
									<%-- 非表示項目 --%>
									<asp:TemplateField HeaderText="LINE_CD" />
									<asp:TemplateField HeaderText="STATION_CD" />
								</Columns>
							</asp:GridView>
						</div>
					</div>
				</td>
			</tr>
		</table>
	</div>
	<div style="display: none">
		<KTCC:KTButton ID="btnViewRefresh" runat="server" OnClick="btnViewRefresh_Click" />
		<asp:TextBox ID="hiddenNum" runat="server" />
		<asp:TextBox ID="allCheckState" Text="false" runat="server" />
	</div>
	<%-- iframeの呼び出し --%>
	<div id="dialog" style="background-color: white">
		<UC:InputModal ID="InputModal1" runat="server" />
	</div>
</asp:Content>
<asp:Content ID="MasterBodyBottom" ContentPlaceHolderID="MasterBodyBottom" runat="server">
	<div id="divGrvCtrlButton" runat="server" class="div-bottom-button-area">
		<KTCC:KTButton ID="btnAllCheck" runat="server" Text="すべて選択" CssClass="btn-middle" OnClientClick="AnlGroupModelList.AllCheck(); return false;" />
		<KTCC:KTButton ID="btnInsert" runat="server" Text="追加" CssClass="btn-middle" />
		<KTCC:KTButton ID="btnDelete" runat="server" Text="削除" CssClass="btn-middle" OnClick="btnDelete_Click" />
	</div>
</asp:Content>
