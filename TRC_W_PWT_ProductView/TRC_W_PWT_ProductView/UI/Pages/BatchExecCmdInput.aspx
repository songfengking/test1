<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/UI/MasterForm/MasterMain.master" CodeBehind="BatchExecCmdInput.aspx.cs" Inherits="TRC_W_PWT_ProductView.UI.Pages.BatchExecCmdInput" %>
<%@ Import Namespace="TRC_W_PWT_ProductView.Common" %>

<asp:Content ID="MasterScript" runat="server" ContentPlaceHolderID="MasterScript">
    <script src="<%: PageUtils.ResolveUrl( this.Page, "~/Scripts/Pages/BatchExecCmdInput.js") %>"></script>
</asp:Content>


<asp:Content ID="MasterBody" ContentPlaceHolderID="MasterBody" runat="server">
    <div id="divPartsTrace">
        <table class="table-layout">
            <tr>
                <td>
                    <div>
                        <asp:UpdatePanel ID="pnlDataDL" runat="server">
                            <ContentTemplate>
                                <div id="divDLArea">
                                    <div id="divDataDL" runat="server" style="margin:3px" class="">
                                        <table id="tblDataDL" class="table-border-layout" style="width: 800px;">
                                            <tr class="tr-fix-zero-height">
                                                <td style="width: 0px"></td>
                                                <td style="width: 180px"></td>
                                                <td style="width: 460px"></td>
                                            </tr>
                                            <tr class="font-default tr-condition-header ui-state-default">
                                                <td style="width: 0px"></td>
                                                <td>部品</td>
                                                <td>日付</td>
                                            </tr>
                                            <tr>
                                                <td style="width: 0px">
                                                    <KTCC:KTRadioButtonList ID="rblProductKind" runat="server" RepeatDirection="Vertical" CssClass="rbl-default" AutoPostBack="true" OnSelectedIndexChanged="rblProductKind_SelectedIndexChanged"></KTCC:KTRadioButtonList>
                                                </td>
                                                <td>
                                                    <div>
                                                        <table class="table-condition-sub">
                                                            <tr class="font-default">
                                                                <td style="width: 60px">品番</td>
                                                                <td>
                                                                    <KTCC:KTTextBox ID="KTtxtParts" runat="server" InputMode="RegExp" RegExpression="[-A-Za-z0-9]+" AutoUpper="true" MaxLength="10" CssClass="font-default txt-default ime-inactive txt-width-short" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </td>
                                                <td>
                                                    <table class="table-condition-sub">
                                                        <tr class="font-default">
                                                            <td style="width:160px">
                                                                <asp:CheckBox ID="chkProduct" runat="server" Text="生産中"/> 
                                                            </td>
                                                            <td style="width: 80px">完成予定日</td>
                                                            <td>
                                                                <KTCC:KTCalendar ID="cldProductFrom" runat="server" UseCalendar="false" Type="yyyyMMdd" CssClass="font-default txt-default cld-ymd" />～<KTCC:KTCalendar ID="cldProductTo" runat="server" UseCalendar="false" Type="yyyyMMdd" CssClass="font-default txt-default cld-ymd" />
                                                            </td>
                                                        </tr>
                                                        <tr class="font-default">
                                                            <td style="width:160px">
                                                                <asp:CheckBox ID="chkStock" runat="server" Text="在庫"/> 
                                                            </td>
                                                            <td style="width: 80px">完成日</td>
                                                            <td>
                                                                <KTCC:KTCalendar ID="cldStockFrom" runat="server" UseCalendar="false" Type="yyyyMMdd" CssClass="font-default txt-default cld-ymd" />～<KTCC:KTCalendar ID="cldStockTo" runat="server" UseCalendar="false" Type="yyyyMMdd" CssClass="font-default txt-default cld-ymd" />
                                                            </td>
                                                        </tr>
                                                        <tr class="font-default">
                                                            <td style="width:160px">
                                                                <asp:CheckBox ID="chkShipment" runat="server" Text="出荷済"/> 
                                                            </td>
                                                            <td style="width: 80px">出荷日</td>
                                                            <td>
                                                                <KTCC:KTCalendar ID="cldShipmentFrom" runat="server" UseCalendar="false" Type="yyyyMMdd" CssClass="font-default txt-default cld-ymd" />～<KTCC:KTCalendar ID="cldShipmentTo" runat="server" UseCalendar="false" Type="yyyyMMdd" CssClass="font-default txt-default cld-ymd" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                        <div style="clear: both;"/>
                                        <div class="condition-button-area">
                                            <KTCC:KTButton ID="btnDLPartsTrace" runat="server" Text="実行" CssClass="btn-middle" OnClick="btnDLPartsTrace_Click"/>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>                   
                </td>
            </tr>
            <tr>
                <td>
                    <div class="ui-widget-header div-result-title"></div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>


<asp:Content ID="MasterBodyBottom" ContentPlaceHolderID="MasterBodyBottom" runat="server" >
    <div id="divGrvCtrlButton" runat="server" class="div-bottom-button-area">
    </div>
</asp:Content>
