<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/UI/MasterForm/MasterMain.master" CodeBehind="MaintenanceNAListDetail.aspx.cs" Inherits="TRC_W_PWT_ProductView.UI.Pages.MaintenanceNAListDetail" %>
<%@ Import Namespace="TRC_W_PWT_ProductView.Common" %>
<%@ Register src="~/UI/Pages/UserControl/InputModal.ascx" tagname="InputModal" tagprefix="UC" %>

<asp:Content ID="MasterScript" runat="server" ContentPlaceHolderID="MasterScript" >
    <script src="<%: PageUtils.ResolveUrl( this.Page, "~/Scripts/Pages/Maintenance/MasterMainteNAListDetail.js") %>"></script>
</asp:Content>

<asp:Content ID="MasterBody" ContentPlaceHolderID="MasterBody" runat="server">
    <div id="header">
        <table class="table-layout">
            <tr>
                <td>
                    <div class="div-detail-info-margin">
                        <table class="table-border-layout" style="width: 440px">
                            <colgroup>
                                <col style="width: 100px" />
                                <col style="width: 150px" />
                                <col style="width: 100px" />
                                <col style="width:  90px" />
                                <col style="width:   0px" />
	                        </colgroup>
                            <tr class="font-default tr-category-header">
                                <td colspan="5">
                                    <asp:Label ID="lblProductInfo" runat="server" Text="製品情報"></asp:Label>
                                </td>
                            </tr>
                            <tr class="font-default tr-content-header">
                                <td colspan="2">生産型式</td>
                                <td>機番</td>
                                <td>完成日</td>
                                <td></td>
                            </tr>
                            <tr class="tr-ctrl-height">
                                <td>
                                    <KTCC:KTTextBox ID="txtModelCd" runat="server" MaxLength="11" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                </td>
                                <td>
                                    <KTCC:KTTextBox ID="txtModelNm" runat="server" MaxLength="20" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                </td>
                                <td>
                                    <KTCC:KTTextBox ID="txtSerial" runat="server" MaxLength="7" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                                </td>
                                <td>
                                    <KTCC:KTTextBox ID="txtFinDt" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct"/>
                                </td>
                                <td>
                                    <KTCC:KTTextBox ID="txtPtnCd" runat="server" ReadOnly="true" visible="false"/>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <div class="div-content-title-r">
        <asp:Label ID="lblDetailTitle" runat="server" Text="一覧" class="content-title"></asp:Label>
    </div>

    <%-- 一覧部 --%>
    <div id="divDetailBodyScroll">
        <div style="margin-top:5px"></div>
        <div id="outMainScroll" class="div-x-scroll-flt2" onscroll="MasterMainteNAListDetail.ResizeDivArea();">
            <div>
                <table id="solidTitleHeader" runat="server" class="grid-layout" style="width: 1000px; height:0px">
                    <tr id="headerMainContent" runat="server" class="listview-header_2r ui-state-default">
                        <th id="partsOpe"   runat="server" style="width:180px">チェック対象外</th>
                        <th id="opeKey"     runat="server" class="tr-fix-zero-height"></th>
                        <th id="dtl"        runat="server" style="width:300px">詳細</th>
                        <th id="dtlkey"     runat="server" class="tr-fix-zero-height"></th>
                        <th id="notas"      runat="server" style="width:700px">理由</th>
                        <th id="insDt"      runat="server" style="width:150px">登録日</th>
                        <th id="insBy"      runat="server" style="width: 80px">登録者</th>
                    </tr>
                </table>
            </div>
            <div id="divMainListArea" class="div-y-scroll-flt2">
                <asp:ListView ID="lstMainList" runat="server" OnItemDataBound="lstMainList_ItemDataBound" OnSelectedIndexChanging="lstMainList_SelectedIndexChanging" OnSelectedIndexChanged="lstMainList_SelectedIndexChanged">
                    <LayoutTemplate>
                        <table id="itemPlaceholderContainer" runat="server" class="grid-layout" style="width:1000px; height:auto">
                            <tr id="headerMainContent" runat="server" class="listview-header_3r ui-state-default">
                                <th id="partsOpe"   runat="server" style="width:180px"/>
                                <th id="opeKey"     runat="server" class="tr-fix-zero-height"/>
                                <th id="dtl"        runat="server" style="width:300px"/>
                                <th id="dtlkey"     runat="server" class="tr-fix-zero-height"/>
                                <th id="notas"      runat="server" style="width:700px"/>
                                <th id="insDt"      runat="server" style="width:150px"/>
                                <th id="insBy"      runat="server" style="width: 80px"/>
                            </tr>
                            <tr id="itemPlaceholder" runat="server">
                            </tr>
                        </table>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr id="trRowData" runat="server" class="listview-row ui-widget">
                            <td>
                                <asp:Button ID="btnSelect" runat="server" CommandName="Select" Text="" CssClass="btnSelect invisible" />
                                <KTCC:KTTextBox ID="txtPartsOpe"   runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                            </td>
                            <td>
                                <KTCC:KTTextBox ID="txtOpeKey" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td>
                            <td>
                                <KTCC:KTTextBox ID="txtDtl"    runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td>
                            <td>
                                <KTCC:KTTextBox ID="txtDtlKey" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td>
                            <td>
                                <KTCC:KTTextBox ID="txtNotes" runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-lf" />
                            </td>
                            <td>
                                <KTCC:KTTextBox ID="txtInsDt"   runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                            </td>
                            <td>
                                <KTCC:KTTextBox ID="txtInsBy"  runat="server" ReadOnly="true" CssClass="font-default txt-default txt-width-full al-ct" />
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:ListView>
            </div>
        </div>
    </div>

</asp:Content>

<asp:Content ID="MasterBodyBottom" ContentPlaceHolderID="MasterBodyBottom" runat="server" >
    <div id="divGrvCtrlButton" runat="server" class="div-bottom-button-area">
        <KTCC:KTButton ID="btnInsert"   runat="server" Text="追加" CssClass="btn-middle" />
        <KTCC:KTButton ID="btnDelete"   runat="server" Text="削除" CssClass="btn-middle" OnClick="btnDelete_Click"/>
        <div style="display:none">
            <KTCC:KTButton ID="btnSearch" runat="server" OnClick="btnSearch_Click"/>
        </div>
    </div>
    <%-- iframeの呼び出し --%>
    <div id="dialog" style="background-color:white">
        <uc:InputModal id="InputModal1" runat="server"/>
    </div>
</asp:Content>

