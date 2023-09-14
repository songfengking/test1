<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/UI/MasterForm/MasterSub.master"  CodeBehind="NACheckList.aspx.cs" Inherits="TRC_W_PWT_ProductView.UI.Pages.Maintenance.NACheckList" %>
<%@ Import Namespace="TRC_W_PWT_ProductView.Common" %>

<asp:Content ID="MasterScript" runat="server" ContentPlaceHolderID="MasterScript" >
    <link href="../../../CSS/cobalt/jquery-wijmo.css" rel="stylesheet" />
    <link href="../../../CSS/Base.css" rel="stylesheet" />
    <link href="../../../CSS/TRC.css" rel="stylesheet" />
    <script src="<%: PageUtils.ResolveUrl( this.Page, "~/Scripts/Pages/Maintenance/NACheckList.js") %>" ></script>
    <script src="<%: PageUtils.ResolveUrl( this.Page, "~/Scripts/Pages/InputModal.js") %>" ></script>
   
</asp:Content>

<asp:Content ID="MasterBody" ContentPlaceHolderID="MasterBody" runat="server">
        <div style="width: 650px; height: 180px;">
            <div style="margin-top:10px; text-align:center;"></div>
            <table id="tblMain" class="table-layout">
                <colgroup>
                    <col style="width: 80px" />
                    <col style="width:400px" />
                </colgroup>
                <tr>
                    <td class="box-in-right" >型式コード: </td>
                    <td>
                        <asp:TextBox ID="txtModelCd" runat="server" InputMode="RegExp" RegExpression="[-A-Za-z0-9]+" AutoUpper="true" MaxLength="10" CssClass="font-default txt-default " ReadOnly="true"/>
                    </td>
                </tr>
                <tr>
                    <td class="box-in-right" >機番: </td>
                    <td>
                        <asp:TextBox ID="txtSerial" runat="server" InputMode="AlphaNum" AutoUpper="true" MaxLength="6" CssClass="font-default txt-default " ReadOnly="true"/>
                    </td>
                </tr>
                <tr>
                    <td class="box-in-right" >チェック対象外: </td>
                    <td>
                        <KTCC:KTDropDownList ID="ddlChkNA" runat="server" CssClass="font-default ddl-default" style="width:330px;margin-left:0px"  AutoPostBack="true" OnSelectedIndexChanged="ddlChkNA_SelectedIndexChanged"/>
                    </td>
                </tr>
                <tr>
                    <td class="box-in-right" >詳細: </td>
                    <td>
                        <KTCC:KTDropDownList ID="ddlDtl" runat="server" CssClass="font-default ddl-default" style="width:330px;margin-left:0px" AutoPostBack="false"  />
                    </td>
                </tr>
                <tr>
                    <td class="box-in-right" >理由: </td>
                    <td>
                        <asp:DropDownList ID="ddlReason" runat="server" CssClass="font-default ddl-default" style="width:725px;margin-left:0px" AutoPostBack="true" OnSelectedIndexChanged="ddlReason_SelectedIndexChanged"/>
                    </td>
                </tr>
                <tr>
                    <td class="box-in-right" style="vertical-align:top">理由記入欄: </td>
                    <td>
                        <asp:TextBox ID="txtNotes" runat="server" class="font-default txt-default" Style="width:720px;height:40px;vertical-align:bottom" TextMode="MultiLine"  MaxLength="100"/>
                    </td>
                </tr>
                <tr class="tr-no-border" style="height:30px;">
                    <td colspan="2" />
                </tr>
            </table>
            <table id="Table1">
                <colgroup>
                    <col style="width:160px" />
                    <col style="width: 50%" />
                    <col style="width: 25%" />
                </colgroup>
                <tr>
                    <td></td>
                    <td>
                        <KTCC:KTButton ID="btnRegist" runat="server" Text="" CssClass="btn-middle" OnClick="btnRegist_Click"/>
                        <KTCC:KTButton ID="ktbtnClose" runat="server" Text="閉じる" CssClass="btn-middle"/>
                    </td>
                    <td></td>
                </tr>
            </table>
        </div>
</asp:Content>
