<%@ Page Language="C#" MasterPageFile="~/UI/MasterForm/MasterMain.master" AutoEventWireup="true" CodeBehind="MainCorrectiveView.aspx.cs" Inherits="TRC_W_PWT_ProductView.UI.Pages.AiImageCheck.MainCorrectiveView" %>

<%@ Register Src="~/UI/Pages/UserControl/InputModal.ascx" TagName="InputModal" TagPrefix="UC" %>
<%@ Import Namespace="TRC_W_PWT_ProductView.Common" %>

<asp:Content ID="MasterScript" ContentPlaceHolderID="MasterScript" runat="server">
	<script src="<%: PageUtils.ResolveUrl( this.Page, "~/Scripts/Pages/AiImageCheck/MainCorrectiveView.js") %>"></script>
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
												<td style="width: 290px"></td>
												
											</tr>
											<tr class="font-default tr-condition-header ui-state-default">
												<td>ライン</td>
												<td>ステーション</td>
												<td>製品型式</td>
												
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
															<td style="width: 100px">生産型式名</td>
															<td>
																<KTCC:KTTextBox ID="txtModelNm" runat="server" InputMode="HalfKana" MaxLength="20" CssClass="font-default txt-default ime-inactive txt-width-long" AutoUpper="False" />
															</td>
														</tr>
														<tr class="font-default">
															<td style="width: 100px">生産型式コード</td>
															<td>
																<KTCC:KTTextBox ID="txtModelCd" runat="server" InputMode="RegExp" MaxLength="13" CssClass="font-default txt-default ime-inactive txt-width-long" AutoUpper="False" RegExpression="^[a-zA-Z0-9!-/:-@¥[-`{-~]*$" />
															</td>
														</tr>
													</table>
												</td>
											</tr>
										</table>
									</div>
									<div class="condition-in-box">
										<table class="table-border-layout" style="width: 635px">
											<tr class="tr-fix-zero-height">
												<td style="width: 200px"></td>
												<td style="width: 170px"></td>
												<td style="width: 265px"></td>
											</tr>
											<tr class="font-default tr-condition-header ui-state-default">
												
												<td>機番/IDNO/月度連番</td>
												<td>検査結果</td>
												<td>日付</td>
											</tr>
											<tr class="tr-condition-body">
												
												
												<td>
													<table class="table-condition-sub">
														<tr class="font-default">
															<td style="width: 60px">製品機番</td>
															<td>
																<KTCC:KTTextBox ID="txtKiban" runat="server" InputMode="AlphaNum" MaxLength="7" CssClass="font-default txt-default ime-inactive txt-width-short" AutoUpper="False" />
															</td>
														</tr>
														<tr class="font-default">
															<td style="width: 60px">IDNO</td>
															<td>
																<KTCC:KTTextBox ID="txtIdno" runat="server" InputMode="IntNum" MaxLength="7" CssClass="font-default txt-default ime-inactive txt-width-short" />
															</td>
														</tr>
														<tr class="font-default">
															<td style="width: 60px">月度連番</td>
															<td>
																<KTCC:KTTextBox ID="txtMonthlySeqNum" runat="server" InputMode="IntNum" MaxLength="11" CssClass="font-default txt-default ime-inactive txt-width-short" />
															</td>
														</tr>
													</table>
												</td>
												<td>
													<table class="table-condition-sub">
														<tr class="font-default">
															<td style="width: 60px">検査結果</td>
															<td>
																<KTCC:KTDropDownList ID="ddlAnlResult" runat="server" CssClass="font-default ddl-default ddl-width-short" />
															</td>
														</tr>
													</table>
												</td>
												<td>
													<table class="table-condition-sub">
														<tr class="font-default">
															<td style="width: 50px">検査日</td>
															<td>
																<div>
																	<KTCC:KTCalendar ID="cldStart" runat="server" UseCalendar="false" Type="yyyyMMdd" CssClass="font-default txt-default cld-ymd" />～<KTCC:KTCalendar ID="cldEnd" runat="server" UseCalendar="false" Type="yyyyMMdd" CssClass="font-default txt-default cld-ymd" />
																</div>
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
					<div id="divLTScroll" class="div-scroll-right-top div-left-grid">
						<div id="divGrvHeaderLT" runat="server">
							<asp:GridView ID="grvDetailHeaderLT" runat="server" CssClass="grid-layout2 ui-widget-content" AllowPaging="false" AllowSorting="false" PageSize="50" AutoGenerateColumns="false">
								<HeaderStyle CssClass="grid-header ui-state-default" />
							</asp:GridView>
						</div>
					</div>
				</td>
			</tr>
			<tr>
				<td>
					<div id="divLBScroll" class="div-scroll-right-top div-left-grid">
						<div id="divGrvLB" runat="server">
							<asp:GridView ID="grvDetailBodyLB" runat="server" ShowHeader="false" CssClass="grid-layout2 ui-widget-content" AllowPaging="true" AllowSorting="false" PageSize="50" AutoGenerateColumns="false" OnRowDataBound="grvSearch_RowDataBound" >
								<PagerSettings Position="TopAndBottom" Mode="NumericFirstLast" PageButtonCount="50" Visible="false" />
								<HeaderStyle CssClass="grid-header ui-state-default" />
								<RowStyle CssClass="grid-row ui-widget" />
								<SelectedRowStyle CssClass="ui-state-highlight" />
								<Columns>
									<asp:TemplateField HeaderText="LINE_NM" />
									<asp:TemplateField HeaderText="STATION_NM" />
									<asp:TemplateField HeaderText="MODEL_CD" />
									<asp:TemplateField HeaderText="MODEL_NM" />
									<asp:TemplateField HeaderText="KIBAN" />
									<asp:TemplateField HeaderText="IDNO" />
									<asp:TemplateField HeaderText="MONTHLY_SEQ_NUM" />
									<asp:TemplateField HeaderText="RECORD_NO" />
									<asp:TemplateField HeaderText="ANL_RESULT" />
									<asp:TemplateField HeaderText="ANL_DATE" />
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

