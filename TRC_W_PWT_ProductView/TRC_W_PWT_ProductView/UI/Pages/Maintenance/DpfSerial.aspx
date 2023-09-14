<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/UI/MasterForm/MasterSub.master"  CodeBehind="DpfSerial.aspx.cs" Inherits="TRC_W_PWT_ProductView.UI.Pages.Maintenance.DpfSerial" %>
<%@ Import Namespace="TRC_W_PWT_ProductView.Common" %>

<asp:Content ID="MasterScript" runat="server" ContentPlaceHolderID="MasterScript" >
    <link href="../../../CSS/cobalt/jquery-wijmo.css" rel="stylesheet" />
    <link href="../../../CSS/Base.css" rel="stylesheet" />
    <link href="../../../CSS/TRC.css" rel="stylesheet" />
    <script src="<%: PageUtils.ResolveUrl( this.Page, "~/Scripts/Pages/Maintenance/DpfSerial.js") %>" ></script>
    <script src="<%: PageUtils.ResolveUrl( this.Page, "~/Scripts/Pages/InputModal.js") %>" ></script>
   
</asp:Content>

<asp:Content ID="MasterBody" ContentPlaceHolderID="MasterBody" runat="server">
        <div style="width: 500px; height: 110px;">
            <div style="margin-top:20px; text-align:center;"></div>
            <div>
                <table id="tblMain" class="table-layout">
                    <colgroup>
                        <col style="width: 150px" />
                        <col style="width: 400px" />
                    </colgroup>
                    <tr>
                        <td class="box-in-right" style="vertical-align:top">部品名：</td>
                        <td>
                            <KTCC:KTDropDownList ID="ddlParts" runat="server" Class="font-default ddl-default ddl-width-middle" AutoPostBack="true" OnSelectedIndexChanged="ddlParts_SelectedIndexChanged"></KTCC:KTDropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="box-in-right" >部品型式コード：</td>
                        <td>
                            <asp:TextBox id="txtDpfModelCd" runat="server" InputMode="RegExp" RegExpression="[-A-Za-z0-9]+" MaxLength="10" Class="font-default txt-default ime-disabled txt-width-middle"/>
                        </td>
                    </tr>
                    <tr>
                        <td class="box-in-right" style="vertical-align:top">部品機番：</td>
                        <td>
                            <asp:TextBox ID="txtDpfSerial"  runat="server" InputMode="AlphaNum" MaxLength="20" Class="font-default txt-default ime-disabled txt-width-long"/>
                        </td>
                    </tr>
                    <tr>
                        <td class="box-in-right" style="vertical-align:top">取付日：</td>
                        <td>
                            <div>
                                <KTCC:KTCalendar ID="cldAssemblyDt" runat="server" UseCalendar="false" Type="yyyyMMdd" CssClass="font-default txt-default cld-ymd" />
                            </div>
                        </td>
                    </tr>
                </table>
            </div>

            <div id="dlgBtn" style="margin-top:30px; text-align:center;">
                <div>
                    <KTCC:KTButton id="btnUpdate" runat="server" CssClass="btn-middle" Text="更新" OnClick="btnUpdate_Click"/>
                    <KTCC:KTButton  id="btnClose" runat="server" CssClass="btn-middle" Text="閉じる"/>
                </div>
                <div style="display:none">
                    <asp:TextBox ID="txtExeKbn" runat="server" />
                    <asp:TextBox ID="txtModelCd" runat="server" />
                    <asp:TextBox ID="txtSerial" runat="server" />
                    <asp:TextBox ID="txtLineCd" runat="server" />
                    <asp:TextBox ID="txtST" runat="server" />
                    <asp:TextBox ID="txtProductKind" runat="server" />
                    <asp:TextBox ID="txtIdno" runat="server" />
                </div>
            </div>
        </div>
</asp:Content>
