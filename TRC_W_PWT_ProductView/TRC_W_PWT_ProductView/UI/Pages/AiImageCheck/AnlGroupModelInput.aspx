<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/UI/MasterForm/MasterSub.master" CodeBehind="AnlGroupModelInput.aspx.cs" Inherits="TRC_W_PWT_ProductView.UI.Pages.AiImageCheck.AnlGroupModelInput" %>
<%@ Import Namespace="TRC_W_PWT_ProductView.Common" %>
<%@ Register src="~/UI/Pages/UserControl/LoadingData.ascx" tagname="LoadingData" tagprefix="UC" %>
<asp:Content ID="MasterScript" runat="server" ContentPlaceHolderID="MasterScript">
    <script src="<%: PageUtils.ResolveUrl( this.Page, "~/Scripts/ControlCommon.js") %>"></script>
    <script src="<%: PageUtils.ResolveUrl( this.Page, "~/Scripts/Pages/AiImageCheck/AnlGroupModelInput.js") %>"></script>
    <script src="<%: PageUtils.ResolveUrl( this.Page, "~/Scripts/Pages/AiImageCheck/AiImageCheck.js") %>"></script>
    <script src="<%: PageUtils.ResolveUrl( this.Page, "~/Scripts/Pages/AiImageCheck/SubmitControl.js") %>" ></script>
    <script src="<%: PageUtils.ResolveUrl( this.Page, "~/Scripts/Pages/AiImageCheck/Ready.js") %>" ></script>
</asp:Content>
<asp:Content ID="MasterBodyTop" ContentPlaceHolderID="MasterBodyTop" runat="server">
    <%-- 画面タイトル --%>
    
</asp:Content>
<asp:Content ID="MasterBody" ContentPlaceHolderID="MasterBody" runat="server">
    <div class="condition-area">
        <table class="table-layout" style="width: 340px">
            <tr class="tr-fix-zero-height">
                <td style="width: 220px"></td>
                <td style="width: 220px"></td>
                <td style="width: 220px"></td>
                <td style="width: 220px"></td>
                <td style="width: 220px"></td>
            </tr>
            <tr class="tr-ctrl-height">
                <td align="right">型式コード:</td>
                <td >
                    <KTCC:KTTextBox ID="txtModelCd" runat="server" InputMode="RegExp" MaxLength="11" CssClass="font-default txt-default ime-inactive txt-width-long" AutoUpper="False" RegExpression="^[a-zA-Z0-9!-/:-@¥[-`{-~]*$" />
                </td>
                <td  align="right">型式名:</td>
                <td>
                    <KTCC:KTTextBox ID="txtModelNm" runat="server" InputMode="HalfKana" MaxLength="20" CssClass="font-default txt-default ime-inactive txt-width-long" AutoUpper="False" />
                </td>
                <td>
                    <asp:CheckBox ID="chkTarget" runat="server" Class="font-default ddl-default ddl-width-short" AutoPostBack="false" Text="未割当" />
                </td>
            </tr>
            <tr class="tr-ctrl-height">
                <td  align="right">画像解析グループコード:</td>
                <td>
                    <KTCC:KTTextBox ID="txtImgGroupCd" runat="server" InputMode="IntNum" MaxLength="10" CssClass="font-default txt-default ime-inactive txt-width-long" AutoUpper="False"/>
                </td>
                 <td  align="right">画像解析グループ名</td>
                <td>
                    <KTCC:KTTextBox ID="txtImgGroupNm" runat="server" MaxLength="40" CssClass="font-default txt-default ime-inactive txt-width-long" AutoUpper="False" />
                </td>
                 <td> </td>
            </tr>
            
        </table>
    </div>
    <div class="condition-button-area">
        <KTCC:KTButton ID="btnSearch" runat="server" Text="検索" CssClass="btn-middle" OnClick="btnSearch_Click" />
    </div>
    <div style="clear: both; height: 0px;"></div>
    <div class="result-area">
        <div style="margin-left: 10px; overflow: auto">
            <asp:GridView ID="grvProcessWork" runat="server" CssClass="grid-layout ui-widget-content" AutoGenerateColumns="False" OnRowDataBound="grvProcessWork_RowDataBound">
                <HeaderStyle CssClass="position:relative; grid-header ui-state-default" />
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
                    <asp:TemplateField HeaderText="GROUP_CD" />
				    <asp:TemplateField HeaderText="GROUP_NM" />
									
			    </Columns>
            </asp:GridView>
            <asp:HiddenField ID="hdnSelectedIndex" runat="server" Value="-1" />
            <asp:HiddenField ID="hdnLineCd" runat="server" Value="" />
            <asp:HiddenField ID="hdnProcessCd" runat="server" Value=""/>
            <asp:HiddenField ID="hdnProcessNm" runat="server" Value="" />
            <asp:HiddenField ID="hdnWorkCd" runat="server" Value="" />
            <asp:HiddenField ID="hdnWorkNm" runat="server" Value="" />
            <asp:HiddenField ID="hdnSearchTargetFlg" runat="server" Value="" />
        </div>
    </div>
    <UC:LoadingData ID="divLoadingData" runat="server" />
    <div style="display: none">
		<asp:TextBox ID="allCheckState" Text="false" runat="server" />
	</div>
</asp:Content>
<asp:Content ID="MasterBodyBottom" ContentPlaceHolderID="MasterBodyBottom" runat="server">
    <div class="div-bottom-button-area">
        <KTCC:KTButton ID="btnAllCheck" runat="server" Text="すべて選択" CssClass="btn-middle" OnClientClick="AnlGroupModelInput.AllCheck(); return false;" />
        <KTCC:KTButton ID="btnSelect" runat="server" Text="追加" CssClass="btn-middle" OnClick="btnSelect_Click" ondblclick ="btnSelect_DBClick" />
        <KTCC:KTButton ID="btnCancel" runat="server" Text="閉じる" CssClass="btn-middle" />
    </div>

</asp:Content>
 