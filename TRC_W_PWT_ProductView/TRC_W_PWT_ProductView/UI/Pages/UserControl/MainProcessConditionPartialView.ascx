<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MainProcessConditionPartialView.ascx.cs" Inherits="TRC_W_PWT_ProductView.UI.Pages.UserControl.MainProcessPartialView" %>
<%--検索条件（噴射時期計測03）--%>
<div runat="server" id="divEngineInjection03Condition" style="width: 400px" oninit="divEngineInjection03Condition_Init" >
    <table class="table-condition-sub">
        <tr class="font-default">
            <td style="width: 80px">測定日</td>
            <td style="width: 180px" colspan="3">
                <KTCC:KTCalendar ID="engineInjection03_cldStart" runat="server" UseCalendar="false" Type="yyyyMMdd" CssClass="font-default txt-default cld-ymd" />～<KTCC:KTCalendar ID="engineInjection03_cldEnd" runat="server" UseCalendar="false" Type="yyyyMMdd" CssClass="font-default txt-default cld-ymd" />
            </td>
            <td style="width: 100px" />
        </tr>
        <tr class="font-default">
            <td style="width: 80px">統合コード</td>
            <td style="width: 100px" >
                <KTCC:KTTextBox ID="engineInjection03_txtIntegratedCd" runat="server" InputMode="FloatNum" AutoUpper="true" MaxLength="11" CssClass="font-default txt-default ime-disabled txt-width-short" />
            </td>
            <td style="width: 15px" />
            <td style="width: 65px">測定号機</td>
            <td style="width: 100px">
                <KTCC:KTDropDownList ID="engineInjection03_ddlMeasurementTerminalKind" runat="server" CssClass="font-default ddl-default ddl-width-short" />
            </td>
        </tr>
        <tr class="font-default">
            <td style="width: 80px">判定</td>
            <td style="width: 100px">
                <KTCC:KTDropDownList ID="engineInjection03_ddlResultKind" runat="server" CssClass="font-default ddl-default ddl-width-short" />
            </td>
            <td style="width: 15px" />
            <td style="width: 65px" />
            <td style="width: 100px" />
        </tr>
    </table>
</div>
<%--検索条件（噴射時期計測07）--%>
<div runat="server" id="divEngineInjection07Condition" style="width: 300px" oninit="divEngineInjection07Condition_Init">
    <table class="table-condition-sub">
        <tr class="font-default">
            <td style="width: 40px">測定日</td>
            <td>
                <KTCC:KTCalendar ID="engineInjection07_cldStart" runat="server" UseCalendar="false" Type="yyyyMMdd" CssClass="font-default txt-default cld-ymd" />～<KTCC:KTCalendar ID="engineInjection07_cldEnd" runat="server" UseCalendar="false" Type="yyyyMMdd" CssClass="font-default txt-default cld-ymd" />
            </td>
        </tr>
        <tr class="font-default">
            <td style="width: 40px">判定</td>
            <td>
                <KTCC:KTDropDownList ID="engineInjection07_ddlResultKind" runat="server" CssClass="font-default ddl-default ddl-width-short" />
            </td>
        </tr>
    </table>
</div>
<%--検索条件（フリクションロス）--%>
<div runat="server" id="divEngineFrictionCondition" style="width: 300px" oninit="divEngineFrictionCondition_Init">
    <table class="table-condition-sub">
        <tr class="font-default">
            <td style="width: 40px">測定日</td>
            <td>
                <KTCC:KTCalendar ID="engineFriction_cldStart" runat="server" UseCalendar="false" Type="yyyyMMdd" CssClass="font-default txt-default cld-ymd" />～<KTCC:KTCalendar ID="engineFriction_cldEnd" runat="server" UseCalendar="false" Type="yyyyMMdd" CssClass="font-default txt-default cld-ymd" />
            </td>
        </tr>
        <tr class="font-default">
            <td style="width: 40px">判定</td>
            <td>
                <KTCC:KTDropDownList ID="engineFriction_ddlResultKind" runat="server" CssClass="font-default ddl-default ddl-width-short" />
            </td>
        </tr>
    </table>
</div>
<%--検索条件（エンジン運転測定03）--%>
<div runat="server" id="divEngineTest03Condition" style="width: 300px" oninit="divEngineTest03Condition_Init">
    <table class="table-condition-sub">
        <tr class="font-default">
            <td style="width: 40px">測定日</td>
            <td>
                <KTCC:KTCalendar ID="engineTest03_cldStart" runat="server" UseCalendar="false" Type="yyyyMMdd" CssClass="font-default txt-default cld-ymd" />～<KTCC:KTCalendar ID="engineTest03_cldEnd" runat="server" UseCalendar="false" Type="yyyyMMdd" CssClass="font-default txt-default cld-ymd" />
            </td>
        </tr>
        <tr class="font-default">
            <td style="width: 40px">判定</td>
            <td>
                <KTCC:KTDropDownList ID="engineTest03_ddlResultKind" runat="server" CssClass="font-default ddl-default ddl-width-short" />
            </td>
        </tr>
    </table>
</div>
<%--検索条件（エンジン運転測定07）--%>
<div runat="server" id="divEngineTest07Condition" style="width: 300px" visible="false" oninit="divEngineTest07Condition_Init">
    <table class="table-condition-sub">
        <tr class="font-default">
            <td style="width: 40px">測定日</td>
            <td>
                <KTCC:KTCalendar ID="engineTest07_cldStart" runat="server" UseCalendar="false" Type="yyyyMMdd" CssClass="font-default txt-default cld-ymd" />～<KTCC:KTCalendar ID="engineTest07_cldEnd" runat="server" UseCalendar="false" Type="yyyyMMdd" CssClass="font-default txt-default cld-ymd" />
            </td>
        </tr>
        <tr class="font-default">
            <td style="width: 40px">判定</td>
            <td>
                <KTCC:KTDropDownList ID="engineTest07_ddlResultKind" runat="server" CssClass="font-default ddl-default ddl-width-short" />
            </td>
        </tr>
    </table>
</div>
