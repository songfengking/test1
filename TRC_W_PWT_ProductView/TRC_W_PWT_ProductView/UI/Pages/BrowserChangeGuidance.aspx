<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/UI/MasterForm/MasterMain.master" CodeBehind="BrowserChangeGuidance.aspx.cs" Inherits="TRC_W_PWT_ProductView.UI.Pages.BrowserChangeGuidance" %>

<asp:Content ID="MasterScript" runat="server" ContentPlaceHolderID="MasterScript" >
</asp:Content>

<asp:Content ID="MasterBody" ContentPlaceHolderID="MasterBody" runat="server" >
    <div>
        <h1>◆アクセスしようとしたリンクはInternet Explorer以外のブラウザでは正常に動作しません。<BR>　以下に挙げる２つの方法でリンクを開きなおして下さい。</h1>
        <h1 class="browser-change-url-part" >
            <asp:HyperLink ID="hyperLinkURL" Text=""  NavigateUrl="" runat="server"></asp:HyperLink>
        </h1>
		<h2 class="browser-change-h2" >
            ①リンクのコピー＆ペースト または リンクのドラッグ＆ドロップ
		</h2>
		<ul class="browser-change-ul" >
		    <li>Internet Explorerを開き、上記の対象リンクをコピーし、ブラウザ上部のアドレスバーに貼付をする。</li>
		    <li>または、Internet Explorerを開き、対象リンクをドラッグし、Internet Explorer画面の上でドロップさせる。</li>
		</ul>
		<h2 class="browser-change-h2">
            ②アプリケーション『ポイっとIE』を使用する
		</h2>
		<ul class="browser-change-ul" >
			  <li>『ポイっとIE』アプリを表示させ、対象リンクをドラッグ＆ドロップにて『ポイっとIE』に投げ込む。</li>
			  <BR>
              <BR>
			  <img src="~/Images/browser-change.jpg" alt="簡単！２STEP！！" runat="server" />
		</ul>
    </div>
</asp:Content>
