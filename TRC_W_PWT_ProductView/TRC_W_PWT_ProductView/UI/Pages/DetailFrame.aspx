<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/UI/MasterForm/MasterMain.master" CodeBehind="DetailFrame.aspx.cs" Inherits="TRC_W_PWT_ProductView.UI.Pages.DetailFrame" %>
<%@ Import Namespace="TRC_W_PWT_ProductView.Common" %>

<asp:Content ID="MasterScript" runat="server" ContentPlaceHolderID="MasterScript" >
    <script src="<%: PageUtils.ResolveUrl( this.Page, "~/Scripts/Pages/DetailFrame.js") %>" ></script>
    <script src="<%: PageUtils.ResolveUrl( this.Page, "~/Scripts/CustomControls/KTExpander.js") %>"></script>
</asp:Content>

<asp:Content ID="MasterBody" ContentPlaceHolderID="MasterBody" runat="server">
    <div>
        <table class="table-layout">
            <tr>
                <td>
                    <div class="div-detail-info-margin">
                        <asp:HiddenField runat="server" ID="hdnProductKind" />
                        <asp:HiddenField runat="server" ID="hdnAssemblyPatternCd" />
                        <asp:HiddenField runat="server" ID="hdnSelectedGroupCd" />
                        <asp:HiddenField runat="server" ID="hdnSelectedClassCd" />
                        <asp:HiddenField runat="server" ID="hdnSelectedLineCd" />
                        <asp:HiddenField runat="server" ID="hdnSelectedProcessCd" />
                        <table class="table-border-layout" style="width: 920px">
                            <colgroup>
                                <col style="width: 100px" />
                                <col style="width: 150px" />
                                <col style="width:  90px" />
                                <col style="width:  90px" />
                                <col style="width: 130px" />
                                <col style="width:  90px" />
                                <col style="width:  90px" />
                                <col style="width:  90px" />
                                <col style="width:  90px" />
		                    </colgroup>
                            <tr class="font-default tr-category-header">
                                <td colspan="9">
                                    <asp:Label ID="lblProductInfo" runat="server" Text="製品情報"></asp:Label>
                                </td>
                            </tr>
                            <tr class="font-default tr-content-header">
                                <td colspan="2">生産型式</td>
                                <td>生産国</td>
                                <td>機番</td>
                                <td>PINコード</td>
                                <td>IDNO</td>
                                <td>完成予定</td>
                                <td>完成日</td>
                                <td>出荷日</td>
                            </tr>
                            <tr class="tr-ctrl-height">
                                <td>
                                    <KTCC:KTTextBox ID="txtProductModelCd" runat="server" MaxLength="11" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                </td>
                                <td>
                                    <KTCC:KTTextBox ID="txtProductModelNm" runat="server" MaxLength="20" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                </td>
                                <td>
                                    <KTCC:KTTextBox ID="txtProductCountryCd" runat="server" MaxLength="3" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                </td>
                                <td>
                                    <KTCC:KTTextBox ID="txtProductSerial" runat="server" MaxLength="7" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                </td>
                                <td>
                                    <KTCC:KTTextBox ID="txtPinCd" runat="server" MaxLength="10" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                </td>
                                <td>
                                    <KTCC:KTTextBox ID="txtIdno" runat="server" MaxLength="7" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                </td>
                                <td>
                                    <KTCC:KTCalendar ID="cldPlanDt" runat="server" UseCalendar="false" Type="yyyyMMdd" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                </td>
                                <td>
                                    <KTCC:KTCalendar ID="cldProductDt" runat="server" UseCalendar="false" Type="yyyyMMdd" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                </td>
                                <td>
                                    <KTCC:KTCalendar ID="cldShippedDt" runat="server" UseCalendar="false" Type="yyyyMMdd" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="divTractorInfo" runat="server" class="div-detail-info-margin">
                        <table class="table-border-layout" style="width: 340px">
                            <colgroup>
                                <col style="width: 100px" />
                                <col style="width: 150px" />
                                <col style="width:  90px" />
                                <col style="width:  90px" />
		                    </colgroup>
                            <tr class="font-default tr-category-header">
                                <td colspan="4"><KTCC:KTButton runat="server" ID="btnTractorTransfer" Text="本機情報" CssClass="btn-model-transfer" /></td>
                            </tr>
                            <tr class="font-default tr-content-header">
                                <td colspan="2">生産型式</td>
                                <td>生産国</td>
                                <td>機番</td>
                            </tr>
                            <tr class="tr-ctrl-height">
                                <td>
                                    <KTCC:KTTextBox ID="txtTractorModelCd" runat="server" MaxLength="11" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                </td>
                                <td>
                                    <KTCC:KTTextBox ID="txtTractorModelNm" runat="server" MaxLength="20" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                </td>
                                <td>
                                    <KTCC:KTTextBox ID="txtTractorCountryCd" runat="server" MaxLength="3" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                </td>
                                <td>
                                    <KTCC:KTTextBox ID="txtTractorSerial" runat="server" MaxLength="7" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="divEngineInfo" runat="server" class="div-detail-info-margin">
                        <table class="table-border-layout" style="width: 340px">
                            <colgroup>
                                <col style="width: 100px" />
                                <col style="width: 150px" />
                                <col style="width:  90px" />
                                <col style="width:  90px" />
		                    </colgroup>
                            <tr class="font-default tr-category-header">
                                <td colspan="4"><KTCC:KTButton runat="server" ID="btnEngineTransfer" Text="搭載エンジン情報" CssClass="btn-model-transfer" /></td>
                            </tr>
                            <tr class="font-default tr-content-header">
                                <td colspan="2">生産型式</td>
                                <td>生産国</td>
                                <td>機番</td>
                            </tr>
                            <tr class="tr-ctrl-height">
                                <td>
                                    <KTCC:KTTextBox ID="txtEngineModelCd" runat="server" MaxLength="11" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                </td>
                                <td>
                                    <KTCC:KTTextBox ID="txtEngineModelNm" runat="server" MaxLength="20" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                </td>
                                <td>
                                    <KTCC:KTTextBox ID="txtEngineCountryCd" runat="server" MaxLength="3" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                </td>
                                <td>
                                    <KTCC:KTTextBox ID="txtEngineSerial" runat="server" MaxLength="7" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div id="divBodyScroll" class="div-fix-scroll">
                        <div style="width: 223px" class="div-fix-scroll-flt">
                            <div id="divListArea" class="div-y-scroll-flt">
                                <div style="width: 223px" class="div-content-title">
                                    <span class="content-title">一覧</span>
                                </div>
                                <div id="divContentList" class="div-auto-scroll div-content-body">
                                    <asp:Panel ID="pnlContentList" runat="server"></asp:Panel>
                                </div>
                            </div>
                        </div>
                        <div id="divViewArea" class="div-fix-scroll-flt">
                            <div class="div-content-title-r">
                                <asp:Label ID="lblDetailTitle" runat="server" Text="" class="content-title"></asp:Label>
                            </div>
                            <div id="divViewBox" class="div-fix-scroll div-content-body-r">
                                <asp:Panel ID="pnlDetailControl" runat="server" EnableViewState="true"></asp:Panel>
                            </div>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
