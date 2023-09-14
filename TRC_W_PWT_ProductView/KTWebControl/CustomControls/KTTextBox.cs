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
    /// <summary>
    /// 拡張テキストボックスコントロール
    /// </summary>
    [DefaultProperty( "Text" )]
    [ToolboxData( "<{0}:KTTextBox runat=\"Server\" />" )]
    public class KTTextBox : TextBox {

        /// <summary>入力モード列挙体</summary>
        public enum InputModeType : int {
            /// <summary>全角文字を含むすべてを入力可</summary>
            All = 0,
            /// <summary>A-Zとa-z(半角文字)のみ入力可</summary>
            Alpha,
            /// <summary>A-Zとa-zと0-9(半角文字)のみ入力可</summary>
            AlphaNum,
            /// <summary>整数0-9(半角文字)のみ入力可</summary>
            IntNum,
            /// <summary>整数0-9(半角文字)及びマイナス(-)のみ入力可</summary>
            IntNumWithMinus,
            /// <summary>整数0-9(半角文字)及び小数点(.)のみ入力可</summary>
            FloatNum,
            /// <summary>整数0-9(半角文字)及びマイナス(-)と小数点(.)のみ入力可</summary>
            FloatNumWithMinus,
            /// <summary>半角かな</summary>
            HalfKana,
            /// <summary>正規表現指定</summary>
            RegExp,
        }

        #region JavaScript
        /// <summary>JavaScript名</summary>
        protected const string JS_NAME = "KTTextBox";
        /// <summary>JavaScriptパス</summary>
        private const string INCLUDE_JS_URL = "~/Scripts/CustomControls/";

        //JavaScriptイベント定義
        private const string JS_FORMAT_EVENT = "onblur";
        //JavaScriptイベント定義
        private const string JS_CLICK_EVENT = "onclick";
        #endregion

        #region Attribute
        /// <summary>AutoUpper</summary>
        private const string ATTR_AUTO_UPPER = "AutoUpper";
        #endregion

        #region CSS
        //CSS定義
        private const string INCLUDE_CSS_URL = "~/css/CustomControls/FormattedTextBox.css";
        #endregion

        #region 拡張プロパティ

        /// <summary>
        /// 入力方法プロパティ
        /// </summary>
        [Category( "動作" )]
        [Description( "入力方法を取得/設定します。" )]
        [Themeable( false )]
        public virtual InputModeType InputMode {
            get {
                object vs = ViewState["InputMode"];
                return ( null == vs ? InputModeType.All : (InputModeType)vs );
            }
            set {
                ViewState["InputMode"] = value;
            }
        }

        /// <summary>
        /// 正規表現プロパティ
        /// </summary>
        [Category( "動作" )]
        [Description( "正規表現を取得/設定します。(InputModeを「RegExp」に設定時のみ有効)" )]
        [Browsable( true )]
        public virtual string RegExpression {
            get {
                object vs = ViewState["RegExpression"];
                return ( null == vs ? "" : (string)vs );
            }
            set {
                ViewState["RegExpression"] = value;
            }
        }

        /// <summary>
        /// 最大入力文字数プロパティ
        /// </summary>
        [Category( "動作" )]
        [Description( "最大入力文字数を取得/設定します。" )]
        public virtual int MaxStringLength {
            get {
                object vs = ViewState["MaxStringLength"];
                return ( null == vs ? Int32.MaxValue : (int)vs );
            }
            set {
                ViewState["MaxStringLength"] = value;
            }
        }

        /// <summary>
        /// 大文字自動変換有無
        /// </summary>
        [Category( "動作" )]
        [Description( "大文字変換の有無を取得/設定します。" )]
        public virtual bool AutoUpper {
            get {
                object vs = ViewState["AutoUpper"];
                return ( null == vs ? false : (bool)vs );
            }
            set {
                ViewState["AutoUpper"] = value;
            }
        }

        [Category( "動作" )]
        [Description( "値を取得/設定します。" )]
        public virtual string Value {
            get {
                return base.Text;
            }
            set {
                base.Text = value;
            }
        }
        #endregion

        #region オーバーライドプロパティ
        /// <summary>
        /// 読取専用プロパティ
        /// </summary>
        [Category( "表示" )]
        [Description( "読取専用コントロールかどうか取得/設定します。" )]
        public override bool ReadOnly {
            get {
                return base.ReadOnly;
            }
            set {
                base.ReadOnly = value;
            }
        }

        /// <summary>
        /// テキストプロパティ
        /// </summary>
        [Category( "表示" )]
        [Description( "表示テキストを取得します。" )]
        public virtual new string Text {
            get {
                return base.Text;
            }
        }
        #endregion

        #region オーバーライドメソッド
        /// <summary>
        /// ロードイベント
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad( EventArgs e ) {

            switch ( InputMode ) {
            case InputModeType.Alpha: {
                    RegExpression = "[A-Za-z]+";			//アルファベットのみ
                    break;
                }
            case InputModeType.AlphaNum: {
                    RegExpression = "[A-Za-z0-9]+";			//アルファベットと数字のみ
                    break;
                }
            case InputModeType.IntNum: {
                    RegExpression = "[0-9]+";				//整数のみ
                    break;
                }
            case InputModeType.IntNumWithMinus: {
                    RegExpression = "[-0-9]+";				//マイナス整数可
                    break;
                }
            case InputModeType.FloatNum: {
                    RegExpression = "[.0-9]+";				//小数可
                    break;
                }
            case InputModeType.FloatNumWithMinus: {
                    RegExpression = "[-.0-9]+";			    //マイナス小数可
                    break;
                }
            default: {
                    break;
                }
            }

            if ( false == this.ReadOnly ) {

                //コントロールが使用するJavaScriptの登録
                this.Page.ClientScript.RegisterClientScriptInclude( GetScriptKey(), GetScriptFile() );

                //JavaScriptイベント呼び出し定義
                string sJSChangeFunc = GetJSBlurEvent();

                //JavaScriptイベントの追加
                if ( ( null == this.Attributes[JS_FORMAT_EVENT] ) ||
                   ( null != this.Attributes[JS_FORMAT_EVENT] && -1 == this.Attributes[JS_FORMAT_EVENT].IndexOf( sJSChangeFunc ) ) ) {
                    this.Attributes[JS_FORMAT_EVENT] += sJSChangeFunc;
                }

                //自動大文字変換有無の属性セット
                this.Attributes[ATTR_AUTO_UPPER] = this.AutoUpper.ToString().ToUpper();
                string upperScript = string.Format( "{0}.chgUpper(this);", JS_NAME );
                if ( ( null == this.Attributes[JS_FORMAT_EVENT] ) ||
                   ( null != this.Attributes[JS_FORMAT_EVENT] && -1 == this.Attributes[JS_FORMAT_EVENT].IndexOf( upperScript ) ) ) {
                    this.Attributes[JS_FORMAT_EVENT] += upperScript;
                }

                //テキストエリアクリック時フォーカスインスクリプト
                string clickScript = string.Format( "{0}.setCursor(this);", JS_NAME );
                if ( ( null == this.Attributes[JS_CLICK_EVENT] ) ||
                   ( null != this.Attributes[JS_CLICK_EVENT] && -1 == this.Attributes[JS_CLICK_EVENT].IndexOf( clickScript ) ) ) {
                    this.Attributes[JS_CLICK_EVENT] += clickScript;
                }

            }

            base.OnLoad( e );
            return;
        }

        /// <summary>
        /// AddAttributesToRenderイベント
        /// </summary>
        /// <param name="writer"></param>
        protected override void AddAttributesToRender( HtmlTextWriter writer ) {
            base.AddAttributesToRender( writer );
        }

        /// <summary>
        /// Renderイベント
        /// </summary>
        /// <param name="writer"></param>
        protected override void Render( HtmlTextWriter writer ) {
            if ( !this.ReadOnly ) {
                base.Render( writer );
            } else {
                String toolTip = "";
                if ( true == StringUtils.IsNotBlank( ToolTip )) {
                    toolTip = String.Format( "title=\"{0}\"", ToolTip );
                }
                if ( base.TextMode == TextBoxMode.MultiLine ) {

                    //MultiLine用設定
                    string labelTag = "<span class=\"txt-multiline {0}\" style=\"{1}\" {2}>{3}</span>";
                    string width = 0 < this.Width.Value ? string.Format( "width:{0}px;", (int)this.Width.Value ) : "";
                    string labelStyle = string.Format( "display:inline-block;{0}", width );

                    string text = Text;
                    writer.Write( labelTag, this.CssClass, labelStyle, toolTip, text );
                } else {

                    //ラベル
                    string labelTag = "<span class=\"readonly {0}\" style=\"{1}\" {2}>{3}</span>";
                    string width = 0 < this.Width.Value ? string.Format( "width:{0}px;", (int)this.Width.Value ) : "";
                    string labelStyle = string.Format( "display:inline-block;{0}", width );

                    string text = Text;
                    writer.Write( labelTag, this.CssClass, labelStyle, toolTip, text );
                }
            }
        }
        #endregion

        /// <summary>
        /// JavaScriptイベント(onBlur)実装定義取得処理
        /// </summary>
        /// <returns>JavaScript</returns>
        private string GetJSBlurEvent() {
            //JavaScriptイベント呼び出し定義
            string sJSChangeFunc = "";

            switch ( InputMode ) {
            case InputModeType.All:
                sJSChangeFunc = string.Format( "{0}.setFormatAll(this,{1});", JS_NAME, MaxStringLength );
                break;
            case InputModeType.HalfKana:
                sJSChangeFunc = string.Format( "{0}.setFormatHalfKana(this,{1});", JS_NAME, MaxStringLength );
                break;
            case InputModeType.Alpha:
            case InputModeType.AlphaNum:
            case InputModeType.IntNum:
            case InputModeType.IntNumWithMinus:
            case InputModeType.FloatNum:
            case InputModeType.FloatNumWithMinus:
            case InputModeType.RegExp:
                sJSChangeFunc = string.Format( "{0}.setFormatAscii(this,'{1}','{2}');", JS_NAME, MaxStringLength, RegExpression );
                break;
            default:
                break;
            }

            return sJSChangeFunc;
        }

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
