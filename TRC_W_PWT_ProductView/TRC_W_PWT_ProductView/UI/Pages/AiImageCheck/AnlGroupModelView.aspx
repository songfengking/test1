<%@ Page Language="C#" MasterPageFile="~/UI/MasterForm/MasterMain.master" AutoEventWireup="true" CodeBehind="AnlGroupModelView.aspx.cs" Inherits="TRC_W_PWT_ProductView.UI.Pages.AiImageCheck.AnlGroupModelView" %>

<%@ Register Src="~/UI/Pages/UserControl/InputModal.ascx" TagName="InputModal" TagPrefix="UC" %>
<%@ Import Namespace="TRC_W_PWT_ProductView.Common" %>

<asp:Content ID="MasterScript" ContentPlaceHolderID="MasterScript" runat="server">
	<script src="<%: PageUtils.ResolveUrl( this.Page, "~/Scripts/Pages/AiImageCheck/AnlGroupModelView.js") %>"></script>
	<script src="<%: PageUtils.ResolveUrl( this.Page, "~/Scripts/Pages/AiImageCheck/AiImageCheck.js") %>"></script>
</asp:Content>

<asp:Content ID="MasterBody" ContentPlaceHolderID="MasterBody" runat="server">
	<%-- 検索条件 --%>
	<div>
		<table class="table-layout">
			<tr>
				<td>
					<div class="box-in-margin-small">
						<asp:UpdatePanel ID="upnCondition" runat="server">
							<ContentTemplate>
								<div class="condition-box">
									<div class="condition-in-box">
										<table class="table-border-layout" style="width: 860px">
											<tr class="tr-fix-zero-height">
												<td style="width: 250px"></td>
												<td style="width: 320px"></td>
												<td style="width: 200px"></td>
											</tr>
											<tr class="font-default tr-condition-header ui-state-default">
												<td>ライン</td>
												<td>ステーション</td>
												<td>設備</td>
											</tr>
											<tr class="tr-condition-body">
												<td>
													<table class="table-condition-sub">
														<tr class="font-default">
															<td style="width: 50px">ライン</td>
															<td>
																<div>
																	<KTCC:KTDropDownList ID="ddlLineCd" runat="server" CssClass="font-default ddl-default " AutoPostBack="True" CausesValidation="True" OnSelectedIndexChanged="ddlStationList_SelectedIndexChanged" />
																</div>
															</td>
														</tr>
													</table>
												</td>
												<td>
													<table class="table-condition-sub">
														<tr class="font-default">
															<td style="width: 90px">ステーション</td>
															<td>
																<div>
																	<KTCC:KTDropDownList ID="ddlStation" runat="server" CssClass="font-default ddl-default" AutoPostBack="True" CausesValidation="True" OnSelectedIndexChanged="ddlLineList_SelectedIndexChanged" />
																</div>
															</td>
														</tr>
													</table>
												</td>
												<td>
													<table class="table-condition-sub">
														<tr class="font-default">
															<td style="width: 40px">設備</td>
															<td>
																<div>
																	<KTCC:KTDropDownList ID="ddlSetubi" runat="server" CssClass="font-default ddl-default" AutoPostBack="True" CausesValidation="True" Style="width: 100px" />
																</div>
															</td>
														</tr>
													</table>
												</td>
											</tr>
										</table>
									</div>
									<div class="condition-in-box">
										<table class="table-border-layout" style="width: 430px">
											<tr class="tr-fix-zero-height">
												<td style="width: 400px"></td>
											</tr>
											<tr class="font-default tr-condition-header ui-state-default">
												<td>画像解析グループ</td>
											</tr>
											<tr class="tr-condition-body">
												<td>
													<table class="table-condition-sub">
														<tr class="font-default">
															<td style="width: 160px">画像解析グループコード</td>
															<td>
																<KTCC:KTTextBox ID="txtImgGroupCd" runat="server" InputMode="IntNum" MaxLength="10" CssClass="font-default txt-default ime-inactive txt-width-long" AutoUpper="False" RegExpression="^[a-zA-Z0-9!-/:-@¥[-`{-~]*$" />
															</td>
														</tr>
														<tr class="font-default">
															<td style="width: 160px">画像解析グループ名</td>
															<td>
																<KTCC:KTTextBox ID="txtImgGroupNm" runat="server" MaxLength="40" CssClass="font-default txt-default ime-inactive txt-width-long" AutoUpper="False" />
															</td>
														</tr>
													</table>
												</td>
											</tr>
										</table>
									</div>
								</div>
							</ContentTemplate>
						</asp:UpdatePanel>
						<div style="clear: both;"></div>
						<div class="condition-button-area">
							<KTCC:KTButton ID="btnSearch" runat="server" Text="検索" CssClass="btn-middle" OnClick="btnSearch_Click" />
						</div>
					</div>
				</td>
			</tr>
			<tr>
				<td>
					<div class="ui-widget-header div-result-title">
						<span class="result-title">検索結果</span>
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
							<asp:GridView ID="grvDetailHeaderLT" runat="server" CssClass="grid-layout2 ui-widget-content" AllowPaging="false" AllowSorting="false" PageSize="50" AutoGenerateColumns="false">
								<HeaderStyle CssClass="grid-header ui-state-default" />
							</asp:GridView>
						</div>
					</div>
				</td>
				<td>
					<div id="divRTScroll" class="div-scroll-right-top div-right-grid">
						<div id="divGrvHeaderRT" runat="server">
							<asp:GridView ID="grvDetailHeaderRT" runat="server" CssClass="grid-layout2 ui-widget-content" AllowPaging="false" AllowSorting="false" PageSize="50" AutoGenerateColumns="false">
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
							<asp:GridView ID="grvDetailBodyLB" runat="server" ShowHeader="false" CssClass="grid-layout2 ui-widget-content" AllowPaging="true" AllowSorting="false" PageSize="50" AutoGenerateColumns="false" OnRowDataBound="grvMainViewLB_RowDataBound">
								<PagerSettings Position="TopAndBottom" Mode="NumericFirstLast" PageButtonCount="50" Visible="false" />
								<HeaderStyle CssClass="grid-header ui-state-default" />
								<RowStyle CssClass="grid-row ui-widget" />
								<SelectedRowStyle CssClass="ui-state-highlight" />
								<Columns>
									<asp:TemplateField HeaderText="LINE_NM" />
									<asp:TemplateField HeaderText="STATION_NM" />
									<asp:TemplateField HeaderText="SETUBI" />
									<asp:TemplateField HeaderText="GROUP_CD" />
									<asp:TemplateField HeaderText="GROUP_NM" />
									<%-- 非表示項目 --%>
									<asp:TemplateField HeaderText="LINE_CD" />
									<asp:TemplateField HeaderText="STATION_CD" />
									<asp:TemplateField HeaderText="SETUBI_NM" />
								</Columns>
							</asp:GridView>
						</div>
					</div>
				</td>
				<td>
					<div id="divRBScroll" class="div-visible-scroll div-right-grid">
						<div id="divGrvRB" runat="server">
							<asp:GridView ID="grvDetailBodyRB" runat="server" ShowHeader="false" CssClass="grid-layout2 ui-widget-content" AllowPaging="true" AllowSorting="false" PageSize="50" AutoGenerateColumns="false" OnRowDataBound="grvMainViewRB_RowDataBound">
								<PagerSettings Position="TopAndBottom" Mode="NumericFirstLast" PageButtonCount="50" Visible="false" />
								<HeaderStyle CssClass="grid-header ui-state-default" />
								<RowStyle CssClass="grid-row ui-widget" />
								<SelectedRowStyle CssClass="ui-state-highlight" />
								<Columns>
									<asp:TemplateField HeaderText="JYOHO1" />
									<asp:TemplateField HeaderText="JYOHO2" />
									<asp:TemplateField HeaderText="JYOHO3" />
									<asp:TemplateField HeaderText="JYOHO4" />
									<asp:TemplateField HeaderText="JYOHO5" />
									<asp:TemplateField HeaderText="JYOHO6" />
									<asp:TemplateField HeaderText="JYOHO7" />
									<asp:TemplateField HeaderText="JYOHO8" />
									<asp:TemplateField HeaderText="JYOHO9" />
									<asp:TemplateField HeaderText="JYOHO10" />
									<asp:TemplateField HeaderText="JYOHO11" />
									<asp:TemplateField HeaderText="JYOHO12" />
									<asp:TemplateField HeaderText="JYOHO13" />
									<asp:TemplateField HeaderText="JYOHO14" />
									<asp:TemplateField HeaderText="JYOHO15" />
									<asp:TemplateField HeaderText="JYOHO16" />
									<asp:TemplateField HeaderText="JYOHO17" />
									<asp:TemplateField HeaderText="JYOHO18" />
									<asp:TemplateField HeaderText="JYOHO19" />
									<asp:TemplateField HeaderText="JYOHO20" />
									<asp:TemplateField HeaderText="JYOHO21" />
									<asp:TemplateField HeaderText="JYOHO22" />
									<asp:TemplateField HeaderText="JYOHO23" />
									<asp:TemplateField HeaderText="JYOHO24" />
									<asp:TemplateField HeaderText="JYOHO25" />
									<asp:TemplateField HeaderText="JYOHO26" />
									<asp:TemplateField HeaderText="JYOHO27" />
									<asp:TemplateField HeaderText="JYOHO28" />
									<asp:TemplateField HeaderText="JYOHO29" />
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
		<KTCC:KTButton ID="btnModalDispTmp" runat="server" OnClick="btnModalDisp_Click" />
		<asp:TextBox ID="txtComment" runat="server" />
	</div>
	<%-- iframeの呼び出し --%>
	<div id="dialog" style="background-color: white">
		<UC:InputModal ID="InputModal1" runat="server" />
	</div>
</asp:Content>
<asp:Content ID="MasterBodyBottom" ContentPlaceHolderID="MasterBodyBottom" runat="server">
	<div>
		<div id="div1" runat="server" class="div-bottom-button-area">
		</div>
	</div>
</asp:Content>
