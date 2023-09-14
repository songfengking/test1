<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/UI/MasterForm/MasterMain.master" CodeBehind="DetailPartsFrame.aspx.cs" Inherits="TRC_W_PWT_ProductView.UI.Pages.DetailPartsFrame" %>
<%@ Import Namespace="TRC_W_PWT_ProductView.Common" %>

<asp:Content ID="MasterScript" runat="server" ContentPlaceHolderID="MasterScript" >
    <script src="<%: PageUtils.ResolveUrl( this.Page, "~/Scripts/Pages/DetailFrame.js") %>" ></script>
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
                        <table class="table-border-layout" style="width: 710px">
                            <colgroup>
                                <col style="width:  80px" />
                                <col style="width: 100px" />
                                <col style="width: 150px" />
                                <col style="width: 100px" />
                                <%--<col style="width:  80px" />--%>
                                <col style="width: 150px" />
                                <col style="width: 150px" />
		                    </colgroup>
                            <tr class="font-default tr-category-header">
                                <td colspan="7">
                                    <asp:Label ID="lblPartsInfo" runat="server" Text="部品情報"></asp:Label>
                                </td>
                            </tr>
                            <tr class="font-default tr-content-header">
                                <td>機種区分</td>
                                <td>型式コード</td>
                                <td>型式名</td>
                                <td>機番</td>
                                <%--<td>IDNO</td>--%>
                                <td>フルアッシ品番</td>
                                <td>投入順序連番</td>
                            </tr>
                            <tr class="tr-ctrl-height">
                                <td>
                                    <KTCC:KTTextBox ID="txtModelType" runat="server" MaxLength="10" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                </td>
                                <td>
                                    <KTCC:KTTextBox ID="txtModelCd" runat="server" MaxLength="11" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                </td>
                                <td>
                                    <KTCC:KTTextBox ID="txtModelNm" runat="server" MaxLength="20" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                </td>
                                <td>
                                    <KTCC:KTTextBox ID="txtSerial" runat="server" MaxLength="10" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                </td>
                                <%--<td>
                                    <KTCC:KTTextBox ID="txtIdno" runat="server" MaxLength="7" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                </td>--%>
                                <td>
                                    <KTCC:KTTextBox ID="txtfullAssyPartsNum" runat="server" MaxLength="10" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                </td>
                                <td>
                                    <KTCC:KTTextBox ID="txtThrowMonthlySequenceNum" runat="server" MaxLength="14" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
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
