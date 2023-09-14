<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/UI/MasterForm/MasterSub.master"  CodeBehind="ProcessingDtEdit.aspx.cs" Inherits="TRC_W_PWT_ProductView.UI.Pages.Maintenance.ProcessingDtEdit" %>
<%@ Import Namespace="TRC_W_PWT_ProductView.Common" %>

<asp:Content ID="MasterScript" runat="server" ContentPlaceHolderID="MasterScript" >
    <link href="../../../CSS/cobalt/jquery-wijmo.css" rel="stylesheet" />
    <link href="../../../CSS/Base.css" rel="stylesheet" />
    <link href="../../../CSS/TRC.css" rel="stylesheet" />
    <script src="<%: PageUtils.ResolveUrl( this.Page, "~/Scripts/Pages/Maintenance/ProcessingDtEdit.js") %>" ></script>
    <script src="<%: PageUtils.ResolveUrl( this.Page, "~/Scripts/Pages/InputModal.js") %>" ></script>
   
</asp:Content>

<asp:Content ID="MasterBody" ContentPlaceHolderID="MasterBody" runat="server">
        <div style="width: 500px; height: 110px;">
            <div id="msgMultiple" runat="server" style="margin-top:10px; text-align:center;" class="font-default">※チェックした製品を一括で更新を行います。</div>
            <div style="margin-top:20px; text-align:center;"></div>
            <div>
                <table id="tblMain" class="table-layout">
                    <colgroup>
                        <col style="width: 100px" />
                        <col style="width: 400px" />
                    </colgroup>
                    <tr>
                        <td class="box-in-right" >加工日: </td>
                        <td>
                            <asp:TextBox id="txtUpdDt" runat="server" InputMode="IntNum" MaxLength="8" Class="font-default txt-default ime-disabled txt-width-short"/>
                            （yyyymmdd）
                        </td>
                    </tr>
                    <tr>
                        <td class="box-in-right" >連番: </td>
                        <td>
                            <asp:TextBox id="txtUpdNum" runat="server" InputMode="IntNum" MaxLength="3" Class="font-default txt-default ime-disabled txt-width-short"/>
                            （数字3桁）
                        </td>
                    </tr>
                    <tr>
                        <td class="box-in-right" >加工ライン: </td>
                        <td>
                            <asp:TextBox id="txtUpdLine" runat="server" InputMode="RegExp" RegExpression="[-A-Za-z0-9]+" MaxLength="4" Class="font-default txt-default ime-disabled txt-width-short"/>
                            （英数字4桁）
                        </td>
                    </tr>
                    <tr>
                        <td class="box-in-right" style="vertical-align:top">修正理由: </td>
                        <td>
                            <asp:TextBox ID="txtRemark" runat="server" class="font-default txt-default" Style="width:325px;height:80px;vertical-align:bottom" TextMode="MultiLine"  MaxLength="200"/>
                        </td>
                    </tr>
                </table>
            </div>

            <div id="dlgBtn" style="margin-top:30px; text-align:center;">
                <div>
                    <KTCC:KTButton id="btn3CUpdate" runat="server" CssClass="btn-middle" Text="更新"/>
                    <KTCC:KTButton  id="btnClose" runat="server" CssClass="btn-middle" Text="閉じる"/>
                </div>
                <div style="display:none">
                    <KTCC:KTButton ID="btnClear" runat="server" OnClick="btnClear_Click"/>
                    <KTCC:KTButton ID="btnProcDtError" runat="server" OnClick="btnProcDtError_Click"/>
                    <KTCC:KTButton ID="btnProcNumError" runat="server" OnClick="btnProcNumError_Click"/>
                    <KTCC:KTButton ID="btnProcLineError" runat="server" OnClick="btnProcLineError_Click"/>
                    <KTCC:KTButton ID="btnRemarkError" runat="server" OnClick="btnRemarkError_Click"/>
                    <KTCC:KTButton ID="btnInputCheck" runat="server" OnClick="btnInputCheck_Click"/>
                </div>
            </div>
        </div>
</asp:Content>
