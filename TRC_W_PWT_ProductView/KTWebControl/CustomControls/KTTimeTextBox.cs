using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using KTFramework.Common;

[assembly: System.Web.UI.TagPrefix( "KTWebControl.CustomControls", "KTCC" )]
namespace KTWebControl.CustomControls {

    ////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// 数値入力コントロール
    /// </summary>
    [DefaultProperty( "Text" )]
    [ToolboxData("<{0}:KTTimeTextBox runat=\"Server\" />")]
    public class KTTimeTextBox : TextBox {

        #region フォーマット
        /// <summary>不正入力値用表示フォーマット 時分秒</summary>
        public const string DEFAULT_TIME_TEXT = "__:__:__";
        #endregion

        #region JavaScript
        /// <summary>JavaScript名</summary>
        new private const string JS_NAME = "KTTimeTextBox";
        /// <summary>JavaScriptパス</summary>
        private const string INCLUDE_JS_URL = "~/Scripts/CustomControls/";

        /// <summary>JavaScriptイベント(フォーカス)</summary>
        private const string JS_FOCUS_EVENT = "onfocus";
        //JavaScriptイベント定義(入力時)
        private const string JS_INPUT_EVENT = "onkeydown";
        //JavaScriptイベント定義(ロストフォーカス時)
        private const string JS_BLUR_EVENT = "onblur";

        /// <summary>JavaScriptイベント 入力時処理</summary>
        private const string JS_FUNC_INPUT_KEY = "chkInputKey(this);";
        /// <summary>JavaScriptイベント フォーカス時処理</summary>
        private const string JS_FUNC_FOCUS = "setFormat(this,true);";
        /// <summary>JavaScriptイベント ロストフォーカス時処理</summary>
        private const string JS_FUNC_BLUR = "setFormat(this,false);";

        //CSS定義
        private const string INCLUDE_CSS_URL = "~/css/CustomControls/NumericTextBox.css";
        #endregion

        #region 拡張プロパティ
        /// <summary>
        /// TimeSpan型の値を取得/設定します。<br/>
        /// </summary>
        [Category("表示")]
        [Description("TimeSpan型の値を取得/設定します。")]
        [Browsable(false)]
        public virtual TimeSpan? Value {
            get {
                if (("" == base.Text) || (DEFAULT_TIME_TEXT == base.Text)) {
                    return null;
                } else {
                    int hour = NumericUtils.ToInt(base.Text.Substring(0, 2));
                    int minute = NumericUtils.ToInt(base.Text.Substring(3, 2));
                    int second = NumericUtils.ToInt(base.Text.Substring(6, 2));

                    return new TimeSpan(hour, minute, second);
                }
            }
            set {
                if (null == value) {
                    base.Text = DEFAULT_TIME_TEXT;
                } else {
                    base.Text = ((TimeSpan)value).ToString(@"hh\:mm\:ss");
                }
            }
        }

        #endregion

        #region オーバーライドプロパティ
        /// <summary>
        /// テキストを取得/設定します。
        /// </summary>
        [Category( "表示" )]
        [Description( "表示テキストを取得します。" )]
        public override string Text {
            get {
                string text = base.Text;
                if (StringUtils.IsBlank(text)) {
                    text = DEFAULT_TIME_TEXT;
                }
                return text;
            }
            set {
                if (null != value) {
                    base.Text = value;
                } else {
                    base.Text = DEFAULT_TIME_TEXT;
                }
            }
        }


        #endregion

        #region イベント
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public KTTimeTextBox() {
        }

        /// <summary>
        /// AddAttributesToRenderイベント
        /// </summary>
        /// <param name="writer"></param>
        protected override void AddAttributesToRender( HtmlTextWriter writer ) {
            base.AddAttributesToRender( writer );
        }
        /// <summary>
        /// ロードイベント
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad( EventArgs e ) {

            //if ( base.InputMode == InputModeType.All ) {
            //    base.InputMode = InputModeType.IntNum;          //数値入力モード
            //}

            base.OnLoad( e );

            if (false == this.ReadOnly) {

                //コントロールが使用するJavaScriptの登録
                this.Page.ClientScript.RegisterClientScriptInclude(GetScriptKey(), GetScriptFile());

                string sJSFunc = "";
                //JavaScriptイベントの追加
                //sJSFunc = string.Format("{0}.{1}", GetScriptName(), JS_FUNC_INPUT_KEY);
                //if ((null == this.Attributes[JS_INPUT_EVENT]) ||
                //    (null != this.Attributes[JS_INPUT_EVENT] && -1 == this.Attributes[JS_INPUT_EVENT].IndexOf(sJSFunc))) {
                //    this.Attributes[JS_INPUT_EVENT] += sJSFunc;
                //}
                sJSFunc = string.Format("{0}.{1}", GetScriptName(), JS_FUNC_FOCUS);
                if ((null == this.Attributes[JS_FOCUS_EVENT]) ||
                    (null != this.Attributes[JS_FOCUS_EVENT] && -1 == this.Attributes[JS_FOCUS_EVENT].IndexOf(sJSFunc))) {
                    this.Attributes[JS_FOCUS_EVENT] += sJSFunc;
                }
                sJSFunc = string.Format("{0}.{1}", GetScriptName(), JS_FUNC_BLUR);
                if ((null == this.Attributes[JS_BLUR_EVENT]) ||
                    (null != this.Attributes[JS_BLUR_EVENT] && -1 == this.Attributes[JS_BLUR_EVENT].IndexOf(sJSFunc))) {
                    this.Attributes[JS_BLUR_EVENT] += sJSFunc;
                }

            }

            return;
        }
        #endregion

        /// <summary>
        /// JSファイル定義取得
        /// </summary>
        /// <returns></returns>
        private string GetScriptName() {

            return JS_NAME;
        }

        /// <summary>
        /// JSファイルパス
        /// </summary>
        /// <returns></returns>
        private string GetScriptFile() {
            return ResolveUrl( INCLUDE_JS_URL + GetScriptName() + ".js" + "?AssemblyVersion=" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString() );
        }

        /// <summary>
        /// JS登録用キー
        /// </summary>
        /// <returns></returns>
        private string GetScriptKey() {
            return GetScriptName() + "_js";
        }
    }
}