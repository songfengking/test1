<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/UI/MasterForm/MasterSub.master" CodeBehind="MainCorrectiveList.aspx.cs" Inherits="TRC_W_PWT_ProductView.UI.Pages.AiImageCheck.MainCorrectiveList" %>

<%@ Import Namespace="TRC_W_PWT_ProductView.Common" %>


<%-- 画面CSS/スクリプト定義領域 --%>
<asp:Content ID="MasterScript" runat="server" ContentPlaceHolderID="MasterScript">
	<script src="<%: ResolveUrl( "~/Scripts/LibScript/scrollViewer.js") %>"></script>
	<script src="<%: PageUtils.ResolveUrl( this.Page, "~/Scripts/Pages/AiImageCheck/MainCorrectiveList.js") %>"></script>
	<script src="<%: PageUtils.ResolveUrl( this.Page, "~/Scripts/Pages/AiImageCheck/AiImageCheck.js") %>"></script>
	<script src="<%: PageUtils.ResolveUrl( this.Page, "~/Scripts/ControlCommon.js") %>"></script>
</asp:Content>

<asp:Content ID="MasterBody" ContentPlaceHolderID="MasterBody" runat="server">
	<div class="div-detail-info-margin">
		<div id="tabResult" class="tabbox" style="margin-left: 50px;">
			<ul id="stationTabs" class="tabs" runat="server" />
			<asp:PlaceHolder ID="pnlTabDefines" runat="server">
				<div id="tabStation" class="tabResult tabDiv" runat="server" style="overflow: hidden; padding: 3px; width: 1050px;">
					<div class="div-detail-table-title">■検査画像</div>
					<div style="margin-top: 10px;"></div>
					<div id="divAnlListArea" class="div-y-scroll-flt"  style="width: 232px; height: 450px;">
						<asp:ListView ID="lstImageList" runat="server" OnItemDataBound="lstImageList_ItemDataBound">
						</asp:ListView>
					</div>
					<div id="divAnlViewArea">
						<div id="divAnlViewBox" class="div-auto-scroll"  style="padding: 3px">
							<asp:Image ID="imgMainArea" runat="server" AlternateText="" />
						</div>
					</div>
				</div>
				<div id="divDetailBodyScroll" class="div-fix-scroll" >
					<div class="div-detail-table-title">■検査結果</div>
					<div style="margin-top: 10px;"></div>
					<div id="divGrvDisplay" runat="server" class="div-y-scroll-flt" style="height: 300px;">
						<table class="table-layout-fix" >
							<tr>
								<td>
									<div id="divLTScroll" class="div-fix-scroll">
										<div id="divGrvHeaderLT" runat="server">
											<asp:GridView ID="grvDetailHeader" runat="server" CssClass="grid-layout2 ui-widget-content" AllowPaging="false" AllowSorting="false" PageSize="10" AutoGenerateColumns="false">
												<HeaderStyle CssClass="grid-header ui-state-default" />
											</asp:GridView>
										</div>
									</div>
								</td>
							</tr>
							<tr>
								<td>
									<div id="divLBScroll" class="div-fix-scroll">
										<div id="divGrvLB" runat="server">
											<asp:GridView ID="grvDetailBody" runat="server" ShowHeader="false" CssClass="grid-layout2 ui-widget-content" AllowPaging="true" AllowSorting="false" PageSize="50" AutoGenerateColumns="false" OnRowDataBound="grvSearch_RowDataBound">
												<PagerSettings Position="TopAndBottom" Mode="NumericFirstLast" PageButtonCount="50" Visible="false" />
												<HeaderStyle CssClass="grid-header ui-state-default" />
												<RowStyle CssClass="grid-row ui-widget" />
												<SelectedRowStyle CssClass="ui-state-highlight" />
												<Columns>
													<asp:TemplateField HeaderText="ANL_RESULT" />
													<asp:TemplateField HeaderText="DISPLAY_ORDER" />
													<asp:TemplateField HeaderText="IMG_ANL_ITEM_NM" />
													<asp:TemplateField HeaderText="CORRECTIVE_RESULT" />
													<asp:TemplateField HeaderText="NG_TYPE_NM" />
													<asp:TemplateField HeaderText="CORRECTIVE_TYPE_NM" />
													<asp:TemplateField HeaderText="CORRECTIVE_COMMENT" />
													<asp:TemplateField HeaderText="UPDATE_BY" />
													<asp:TemplateField HeaderText="IMG_ANL_ITEM_CD" />
													<asp:TemplateField HeaderText="REVISION_NO" />
													<asp:TemplateField HeaderText="NG_TYPE_CD" />
													<asp:TemplateField HeaderText="CORRECTIVE_TYPE_CD" />
												</Columns>
											</asp:GridView>
										</div>
									</div>
								</td>
							</tr>
						</table>
					</div>
					<div style="clear: both;"></div>
					<div class="condition-button-area" style="margin-top: 35px;">
					    <KTCC:KTButton ID="btnClose" runat="server" Text="閉じる" CssClass="btn-middle"/>
                    </div>
				</div>
			</asp:PlaceHolder>
			<div class="div-result-pager">
				<asp:Panel ID="pnlPager" runat="server" EnableViewState="true"></asp:Panel>
			</div>
		</div>
	</div>
</asp:Content>
