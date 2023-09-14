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
    [ToolboxData( "<{0}:KTDecimalTextBox runat=\"Server\" />" )]
    public class KTDecimalTextBox : KTTextBox {

        #region JavaScript
        /// <summary>JavaScript名</summary>
        new private const string JS_NAME = "KTNumericTextBox";
        /// <summary>JavaScriptパス</summary>
        private const string INCLUDE_JS_URL = "~/Scripts/CustomControls/";

        //JavaScriptイベント定義(入力時)
        private const string JS_INPUT_EVENT = "onkeydown";
        //JavaScriptイベント定義(ロストフィーカス時)
        private const string JS_FORMAT_EVENT = "onblur";

        //CSS定義
        private const string INCLUDE_CSS_URL = "~/css/CustomControls/NumericTextBox.css";
        #endregion

        #region 拡張プロパティ
        /// <summary>
        /// 入力桁プロパティ
        /// </summary>
        [Category( "表示" )]
        [Description( "入力桁を取得/設定します。" )]
        [Browsable( false )]
        public override int MaxStringLength {
            get {
                return base.MaxStringLength;
            }
            set {
                base.MaxStringLength = value;
                base.MaxLength = value;
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
                if ( true == ReadOnly ) {
                    string formatPre = "{0:";
                    string formatSuf = "}";
                    string fmt = "#,0";
                    if ( 0 < DecLen ) {
                        fmt += ".";
                        for ( int decLoop = 0; decLoop < DecLen; decLoop++ ) {
                            fmt += "0";
                        }
                    }

                    string format = formatPre + fmt + formatSuf;

                    return String.Format( format, this.Value );
                } else {
                    return StringUtils.ToString( this.Value );
                }
            }
        }

        /// <summary>
        /// 値を取得/設定します。
        /// </summary>
        [Category( "表示" )]
        [Description( "値を取得/設定します。" )]
        [Browsable( false )]
        public new decimal? Value {
            get {
                decimal? value = null;
                string txt = base.Text.Replace( ",", "" );
                if ( true == NumericUtils.IsDecimal( txt ) ) {
                    value = NumericUtils.ToDecimal( txt );
                }
                return value;
            }
            set {
                base.Value = StringUtils.ToString( value );
            }
        }

        /// <summary>
        /// 最大値を取得/設定します。
        /// </summary>
        [Category( "表示" )]
        [Description( "最大値を取得/設定します。" )]
        [Browsable( true )]
        public decimal MaxValue {
            get {
                object vs = ViewState["MaxValue"];
                decimal maxValue = decimal.MaxValue;
                if ( null == vs ) {

                    int numLength = base.MaxLength;

                    if ( InputModeType.IntNumWithMinus == InputMode
                        || InputModeType.FloatNumWithMinus == InputMode ) {
                            if ( 0 != numLength && 1 != numLength ) {
                                numLength--;
                            }
                    }

                    if ( 0 < base.MaxLength ) {
                        if ( (decimal)Math.Pow( 10, numLength ) - 1 < decimal.MaxValue ) {
                            maxValue = (decimal)Math.Pow( 10, numLength ) - 1;
                        }
                    }

                }

                return ( null == vs ? maxValue : (decimal)vs );
            }
            set {
                ViewState["MaxValue"] = value;
            }
        }

        /// <summary>
        /// 最小値を取得/設定します。
        /// </summary>
        [Category( "表示" )]
        [Description( "最小値を取得/設定します。" )]
        [Browsable( true )]
        public decimal MinValue {
            get {
                object vs = ViewState["MinValue"];
                decimal minValue = decimal.MinValue;
                if ( null == vs ) {
                    minValue = MaxValue * -1;
                }
                return ( null == vs ? minValue : (decimal)vs );
            }
            set {
                ViewState["MinValue"] = value;
            }
        }

        /// <summary>
        /// 小数入力時整数部桁を取得/設定します。
        /// </summary>
        [Category( "表示" )]
        [Description( "小数部桁を取得/設定します。" )]
        [Browsable( true )]
        public decimal DecLen {
            get {
                object vs = ViewState["DecLen"];
                decimal decLen = 0;
                return ( null == vs ? decLen : (decimal)vs );
            }
            set {
                ViewState["DecLen"] = value;
            }
        }

        #endregion

        #region イベント
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public KTDecimalTextBox() {
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

            if ( base.InputMode == InputModeType.All ) {
                base.InputMode = InputModeType.IntNum;          //数値入力モード
            }

            base.OnLoad( e );

            if ( false == this.ReadOnly ) {

                //コントロールが使用するJavaScriptの登録
                this.Page.ClientScript.RegisterClientScriptInclude( GetScriptKey(), GetScriptFile() );
                //JavaScriptイベントの追加
                string sJSFunc = "";
                sJSFunc = string.Format( "{0}.chkInputKey(this,{1},{2});", JS_NAME, MaxValue, MinValue );
                if ( ( null == this.Attributes[JS_INPUT_EVENT] ) ||
                   ( null != this.Attributes[JS_INPUT_EVENT] && -1 == this.Attributes[JS_INPUT_EVENT].IndexOf( sJSFunc ) ) ) {
                    this.Attributes[JS_INPUT_EVENT] += sJSFunc;
                }

                if ( InputModeType.IntNum == InputMode
                    || InputModeType.IntNumWithMinus == InputMode ) {
                    sJSFunc = string.Format( "{0}.setFormatInt(this,{1},{2},'{3}');", KTTextBox.JS_NAME, MaxValue, MinValue, base.RegExpression );
                } else if ( InputModeType.FloatNum == InputMode
                    || InputModeType.FloatNumWithMinus == InputMode ) {
                    sJSFunc = string.Format( "{0}.setFormatFloat(this,{1},{2},{3},'{4}');", KTTextBox.JS_NAME, MaxValue, MinValue, DecLen, base.RegExpression );
                } else {
                    sJSFunc = string.Format( "{0}.setFormat(this,{1},{2});", JS_NAME, MaxValue, MinValue );
                }

                if ( ( null == this.Attributes[JS_FORMAT_EVENT] ) ||
                    ( null != this.Attributes[JS_FORMAT_EVENT] && -1 == this.Attributes[JS_FORMAT_EVENT].IndexOf( sJSFunc ) ) ) {
                    this.Attributes[JS_FORMAT_EVENT] += sJSFunc;
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