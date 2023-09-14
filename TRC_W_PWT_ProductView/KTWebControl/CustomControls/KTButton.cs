using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

[assembly: System.Web.UI.TagPrefix( "KTWebControl.CustomControls", "KTCC" )]
namespace KTWebControl.CustomControls {
	////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>
	/// 拡張ボタンクラス<br/>
	/// ボタン押下時のメッセージ表示機能を実装したボタンコントロールです。<br/>
	/// </summary>
	public class KTButton	: Button
	{
		#region Javascript
		//JavaScriptイベント定義
		private const string JS_EVENT_ONCLICK = "onclick";
		#endregion

		#region 拡張プロパティ
		////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// ボタン押下時にconfirmダイアログに表示するメッセージを取得/設定します。
		/// </summary>
		[Bindable(true)]
		[Category("表示")]
		[Description("ボタン押下時にconfirmダイアログに表示するメッセージを取得/設定します")]
		[Localizable(true)]
		public string Message 
		{
			get 
			{
				object vs =  ViewState["Message"];
				return (null == vs ? "" : (string)vs);
			}
			set 
			{
				ViewState["Message"] = value;
				UpdateMessage();
			}
		}
		////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// confirmダイアログでOKボタン押下時に実行するJavaScriptを取得/設定します。
		/// </summary>
		[Bindable(true)]
		[Category("動作")]
		[Description("confirmダイアログでOKボタン押下時に実行するJavaScriptを取得/設定します")]
		[Localizable(true)]
		public string OKAction {
			get 
			{
				object vs =  ViewState["OKAction"];
				return (null == vs ? "" : (string)vs); 
			}
			set 
			{
				ViewState["OKAction"] = value;
				UpdateMessage();
			}
		}
		////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// confirmダイアログでCancelボタン押下時に実行するJavaScriptを取得/設定します。
		/// </summary>
		[Bindable(true)]
		[Category("動作")]
		[Description("confirmダイアログでCancelボタン押下時に実行するJavaScriptを取得/設定します")]
		[Localizable(true)]
		public string CancelAction {
			get
			{
				object vs =  ViewState["CancelAction"];
				return (null == vs ? "" : (string)vs);
			}
			set
			{
				ViewState["CancelAction"] = value;
				UpdateMessage();
			}
		}
		#endregion

		#region メソッド(private)
		////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// メッセージ表示用JavaScript作成処理
		/// </summary>
		private void UpdateMessage()
		{
			//メッセージ未指定時には、confirmダイアログは表示しない
			if (0 < Message.Length)
			{
				StringBuilder sbScript = new StringBuilder(null);
				string sVarName = this.ClientID + "_" + "confirm_res";
				sbScript.Append("var " + sVarName + " = confirm(decodeURI('");
				sbScript.Append(HttpUtility.UrlEncode(Message));
				sbScript.Append("'));");
				sbScript.Append("if(" + sVarName + "== true){");
				sbScript.Append(OKAction);
				sbScript.Append("return true;");
				sbScript.Append("}else{");
				sbScript.Append(CancelAction);
				sbScript.Append("return false;");
				sbScript.Append("}");

				//onclickイベントに割当
                if ((null == this.Attributes[JS_EVENT_ONCLICK]) ||
                    (null != this.Attributes[JS_EVENT_ONCLICK] && -1 == this.Attributes[JS_EVENT_ONCLICK].IndexOf(sbScript.ToString())))
                {
                    this.Attributes.Add(JS_EVENT_ONCLICK, sbScript.ToString());
                }
		 }
		}
		#endregion

		#region オーバーライドイベント


		////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// ロードイベント
		/// </summary>
        /// <param name="e"></param>
		protected override void OnLoad(EventArgs e)
		{
			//コントロールが使用するJavaScriptの登録
			UpdateMessage();

		}
		#endregion
	}
}
