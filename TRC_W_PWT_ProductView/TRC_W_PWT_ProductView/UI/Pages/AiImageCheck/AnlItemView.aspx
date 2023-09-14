<%@ Page Language="C#" MasterPageFile="~/UI/MasterForm/MasterMain.master" AutoEventWireup="true" CodeBehind="AnlItemView.aspx.cs" Inherits="TRC_W_PWT_ProductView.UI.Pages.AiImageCheck.AnlItemView" %>

<%@ Register Src="~/UI/Pages/UserControl/InputModal.ascx" TagName="InputModal" TagPrefix="UC" %>
<%@ Import Namespace="TRC_W_PWT_ProductView.Common" %>

<asp:Content ID="MasterScript" ContentPlaceHolderID="MasterScript" runat="server">
	<script src="<%: PageUtils.ResolveUrl( this.Page, "~/Scripts/Pages/AiImageCheck/AnlItemView.js") %>"></script>
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
										<table class="table-border-layout" style="width: 890px">
											<tr class="tr-fix-zero-height">
												<td style="width: 250px"></td>
												<td style="width: 320px"></td>
												<td style="width: 320px"></td>
												
											</tr>
											<tr class="font-default tr-condition-header ui-state-default">
												<td>ライン</td>
												<td>ステーション</td>
												<td>画像解析項目</td>
												
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
															<td style="width: 130px">画像解析項目コード</td>
															<td>
																<KTCC:KTTextBox ID="txtItemCd" runat="server" InputMode="IntNum" MaxLength="10" CssClass="font-default txt-default ime-inactive txt-width-long" AutoUpper="False" />
															</td>
														</tr>
														<tr class="font-default">
															<td style="width: 130px">画像解析項目名</td>
															<td>
																<KTCC:KTTextBox ID="txtItemNm" runat="server"  MaxLength="40" CssClass="font-default txt-default ime-inactive txt-width-long" AutoUpper="False"  />
															</td>
														</tr>
													</table>
												</td>
											</tr>
										</table>
									</div>
									<div class="condition-in-box">
										<table class="table-border-layout" style="width: 280px">
											<tr class="tr-fix-zero-height">
												<td style="width: 280px"></td>
											</tr>
											<tr class="font-default tr-condition-header ui-state-default">
												
												<td>利用ツール</td>
											</tr>
											<tr class="tr-condition-body">
												
												<td>
													<table class="table-condition-sub">
														<tr class="font-default">
															<td style="width: 80px">利用ツール</td>
															<td>
																<KTCC:KTDropDownList ID="ddlUsingTool" runat="server" CssClass="font-default ddl-default" />
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
							<asp:GridView ID="grvHeaderLT" runat="server" CssClass="grid-layout2 ui-widget-content" AllowPaging="false" AllowSorting="false" PageSize="50" AutoGenerateColumns="false">
								<HeaderStyle CssClass="grid-header ui-state-default" />
							</asp:GridView>
						</div>
					</div>
				</td>
				<td>
					<div id="divRTScroll" class="div-scroll-right-top div-right-grid">
						<div id="divGrvHeaderRT" runat="server">
							<asp:GridView ID="grvHeaderRT" runat="server" CssClass="grid-layout2 ui-widget-content" AllowPaging="false" AllowSorting="false" PageSize="50" AutoGenerateColumns="false">
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
									<asp:TemplateField HeaderText="LINE_NM" />
									<asp:TemplateField HeaderText="STATION_NM" />
									<asp:TemplateField HeaderText="ITEM_CD" />
									<asp:TemplateField HeaderText="ITEM_NM" />
									<asp:TemplateField HeaderText="REVISION_NO" />
									
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
									<asp:TemplateField HeaderText="USING_TOOL" />
									<asp:TemplateField HeaderText="STREAM" />
									<asp:TemplateField HeaderText="USING_MODEL" />
									<asp:TemplateField HeaderText="EXPECT_RESULT" />
									<asp:TemplateField HeaderText="LINE_CD" />
									<asp:TemplateField HeaderText="STATION_CD" />
									<asp:TemplateField HeaderText="USING_TOOL_CD" />
								</Columns>
							</asp:GridView>
						</div>
					</div>
				</td>
			</tr>
		</table>
	</div>
	<div style="display:none">
        <KTCC:KTButton ID="btnViewRefresh" runat="server" OnClick="btnViewRefresh_Click"/>
    </div>
	<%-- iframeの呼び出し --%>
	<div id="dialog" style="background-color: white">
		<UC:InputModal ID="InputModal1" runat="server" />
	</div>
</asp:Content>

<asp:Content ID="MasterBodyBottom" ContentPlaceHolderID="MasterBodyBottom" runat="server" >
    <div>
        <div id="divGrvCtrlButton" runat="server" class="div-bottom-button-area">
            <KTCC:KTButton ID="btnModalDisp"   runat="server" Text="新規追加" CssClass="btn-middle" />
        </div>
    </div>
</asp:Content>