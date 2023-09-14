<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/UI/MasterForm/MasterMain.master" CodeBehind="MaintenanceMenu.aspx.cs" Inherits="TRC_W_PWT_ProductView.UI.Pages.MaintenanceMenu" %>

<%@ Import Namespace="TRC_W_PWT_ProductView.Common" %>

<asp:Content ID="MasterBody" ContentPlaceHolderID="MasterBody" runat="server">
    <table class="menu-columns">
        <tr>
            <td class="menu-column">
                <div>
                    <asp:Label ID="lblMaintenance" runat="server" Text="マスタメンテナンス" CssClass="ui-widget-header menu-header" />
                    <div class="menu-button-area">
                        <KTCC:KTButton ID="ktbtnImpChkNAList" runat="server" Text="重要ﾁｪｯｸ対象外ﾘｽﾄ" CssClass="menu-button menu-button-large" OnClick="ktbtnImpChkNAList_Click" />
                    </div>
                    <div class="menu-button-area">
                        <KTCC:KTButton ID="btnMasterMainte3CList" runat="server" Text="3C加工日修正" CssClass="menu-button menu-button-large" OnClick="btnMasterMainte3CList_Click" />
                    </div>
                    <div class="menu-button-area">
                        <KTCC:KTButton ID="btnMasterMainteDpf" runat="server" Text="排ｶﾞｽ規制部品修正" CssClass="menu-button menu-button-large" OnClick="btnMasterMainteDpf_Click" />
                    </div>
                </div>
            </td>
            <td>
                <div class="menu-column-separator"></div>
            </td>
            <td class="menu-column">
                <asp:Label  ID="lblDataDL"   runat="server" Text="検索機能"   CssClass="ui-widget-header menu-header" />
                <div class="menu-button-area">
                    <KTCC:KTButton ID="btnMainView" runat="server" Text="トレーサビリティ" CssClass="menu-button menu-button-large" OnClick="btnMainView_Click"/>
                </div>
                <div class="menu-button-area">
                    <KTCC:KTButton ID="btnSearchProductOrder" runat="server" Text="製品別通過実績検索" CssClass="menu-button menu-button-large" OnClick="btnSearchProductOrder_Click"/>
                </div>
                <div class="menu-button-area">
                    <KTCC:KTButton ID="btnSearchStationOrder" runat="server" Text="ステーション別通過実績検索" CssClass="menu-button menu-button-large" OnClick="btnSearchStationOrder_Click"/>
                </div>
                <div class="menu-button-area">
                    <KTCC:KTButton ID="btnSearchOrderInfo" runat="server" Text="順序情報検索" CssClass="menu-button menu-button-large" OnClick="btnSearchOrderInfo_Click"/>
                </div>
                <div class="menu-button-area">
                    <KTCC:KTButton ID="btnSearchEngineStock" runat="server" Text="エンジン立体倉庫在庫検索" CssClass="menu-button menu-button-large" OnClick="btnSearchEngineStock_Click"/>
                </div>
                <div class="menu-button-area">
                    <KTCC:KTButton ID="btnSearchCallEngineSimulation" runat="server" Text="搭載エンジン引当検索" CssClass="menu-button menu-button-large" OnClick="btnSearchCallEngineSimulation_Click"/>
                </div>
                <div class="menu-button-area">
                    <KTCC:KTButton ID="btnMultipleSearchModelCd" runat="server" Text="複数型式検索出力" CssClass="menu-button menu-button-large" OnClick="btnMultipleSearchModelCd_Click"/>
                </div>
            </td>
            <td>
                <div class="menu-column-separator"></div>
            </td>
            <td class="menu-column">
                <asp:Label ID="lblEKanban" runat="server" Text="電子かんばん（セオ倉庫）" CssClass="ui-widget-header menu-header" />
                <div class="menu-button-area">
                    <KTCC:KTButton ID="btnKanbanPickingStatusView" runat="server" Text="ピッキング状況" CssClass="menu-button menu-button-large" OnClick="btnKanbanPickingStatusView_Click" />
                </div>
                <div class="menu-button-area">
                    <KTCC:KTButton ID="btnKanbanShortageView" runat="server" Text="未完了ピッキング" CssClass="menu-button menu-button-large" OnClick="btnKanbanShortageView_Click" />
                </div>
            </td>
            <td>
                <div class="menu-column-separator"></div>
            </td>
            <td class="menu-column">
                <asp:Label ID="Label1" runat="server" Text="AI外観検査" CssClass="ui-widget-header menu-header" />
                <!--是正処置入力sfadd-->
                <div class="menu-button-area">
                    <KTCC:KTButton ID="btnMainCorrectiveView" runat="server" Text="是正処置入力" CssClass="menu-button menu-button-large" OnClick="btnMainCorrectiveView_Click" />
                </div>
                <!--検査項目マスタsfadd-->
                <div class="menu-button-area">
                    <KTCC:KTButton ID="btnAnlItemView" runat="server" Text="検査項目マスタ" CssClass="menu-button menu-button-large" OnClick="btnAnlItemView_Click" />
                </div>
                <!--検査グループマスタsfadd-->
                <div class="menu-button-area">
                    <KTCC:KTButton ID="btnAnlGroupView" runat="server" Text="検査グループマスタ" CssClass="menu-button menu-button-large" OnClick="btnAnlGroupView_Click" />
                </div>
                <!--型式紐づけsfadd-->
                <div class="menu-button-area">
                    <KTCC:KTButton ID="btnAnlGroupModelView" runat="server" Text="型式紐づけ" CssClass="menu-button menu-button-large" OnClick="btnAnlGroupModelView_Click" />
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
