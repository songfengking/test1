using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Reflection;
using KTFramework.Common;

[assembly: System.Web.UI.TagPrefix( "KTWebControl.CustomControls", "KTCC" )]
namespace KTWebControl.CustomControls {
    /// <summary>
    /// 年月日入力コントロール
    /// </summary>
    [DefaultProperty( "Text" )]
    [ToolboxData( "<{0}:KTCalendar runat=\"Server\" Type=\"yyyyMMdd\"></{0}:KTCalendar>" )]
    public class KTCalendar : TextBox {

        /// <summary>カレンダー種別</summary>
        public enum CalendarType {
            /// <summary>年月日入力</summary>
            yyyyMMdd = 0,
            /// <summary>年月入力</summary>
            yyyyMM,
        }
        
        #region フォーマット
        /// <summary>不正入力値用表示フォーマット 年月日</summary>
        public const string DEFAULT_DAY_TEXT = "____/__/__";
        /// <summary>不正入力値用表示フォーマット 年月</summary>
        public const string DEFAULT_MONTH_TEXT = "____/__";
        /// <summary>書式フォーマット 年月日(yyyy/MM/dd)</summary>
        private const string TEXT_DAY_FORMAT = KTFramework.Common.DateUtils.DATE_FORMAT_DAY;
        /// <summary>書式フォーマット 年月(yyyy/MM)</summary>
        private const string TEXT_MONTH_FORMAT = KTFramework.Common.DateUtils.DATE_FORMAT_MONTH;
        #endregion

        #region JavaScript
        /// <summary>年月日用Script名</summary>
        private const string JS_NAME_DATE = "KTDatePicker";
        /// <summary>年月用Script名</summary>
        private const string JS_NAME_MONTH = "KTMonthPicker";

        /// <summary>JavaScriptファイルパス</summary>
        private const string INCLUDE_JS_URL = "~/Scripts/CustomControls/";

        /// <summary>JavaScriptイベント(フォーカス)</summary>
        private const string JS_FOCUS_EVENT = "onfocus";
        /// <summary>JavaScriptイベント(入力)</summary>
        private const string JS_INPUT_EVENT = "onkeydown";
        /// <summary>JavaScriptイベント(ロストフォーカス)</summary>
        private const string JS_BLUR_EVENT = "onblur";

        /// <summary>JavaScriptイベント 入力時処理</summary>
        private const string JS_FUNC_INPUT_KEY = "chkInputKey(this);";
        /// <summary>JavaScriptイベント フォーカス時処理</summary>
        private const string JS_FUNC_FOCUS = "setFormat(this,true);";
        /// <summary>JavaScriptイベント ロストフォーカス時処理</summary>
        private const string JS_FUNC_BLUR = "setFormat(this,false);";
        #endregion

        #region 拡張プロパティ

        /// <summary>
        /// カレンダー種別プロパティ
        /// </summary>
        [Category( "表示" )]
        [Description( "日付のフォーマットタイプを取得/設定します。" )]
        [DefaultValue( CalendarType.yyyyMMdd )]
        public virtual CalendarType Type {
            get {
                object vs = ViewState["CalendarType"];
                return ( null == vs ? CalendarType.yyyyMMdd : (CalendarType)vs );
            }
            set {
                ViewState["CalendarType"] = value;
            }
        }

        /// <summary>
        /// 書式フォーマットプロパティ
        /// </summary>
        [Category( "表示" )]
        [Description( "日付のフォーマットを取得します。" )]
        public string Format {
            get {
                return GetFormat();
            }
        }

        /// <summary>
        /// 値評価プロパティ
        /// </summary>
        /// <returns></returns>
        [Category( "表示" )]
        [Description( "日付型の値が設定されているか評価します。" )]
        public bool IsDate {
            get {
                if ( null == GetDate( base.Text ) ) {
                    return false;
                } else {
                    return true;
                }
            }
        }

        /// <summary>
        /// 日付型の値を取得/設定します。<br/>
        /// </summary>
        [Category( "表示" )]
        [Description( "日付型の値を取得/設定します。" )]
        [Browsable( false )]
        public virtual DateTime? Value {
            get {
                return GetDate( base.Text );
            }
            set {
                if ( null == value ) {
                    if ( Type == CalendarType.yyyyMMdd ) {
                        base.Text = DEFAULT_DAY_TEXT;
                    } else {
                        base.Text = DEFAULT_MONTH_TEXT;
                    }
                } else {
                    base.Text = string.Format( "{0:" + GetFormat() + "}", value );
                }
            }
        }

        /// <summary>
        /// 日付書式を設定した日付テキストを取得します。<br/>
        /// </summary>
        [Category( "表示" )]
        [Description( "日付書式を設定した日付テキストを取得します。" )]
        public string TextPlain {
            get {
                string text = base.Text;
                if ( false == this.IsDate ) {
                    text = "";
                }
                return text;
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
                if ( true == value ) {
                }
            }
        }

        /// <summary>
        /// テキストプロパティ
        /// </summary>
        [Category( "表示" )]
        [Description( "表示テキストを取得します。" )]
        public override string Text {
            get {
                string text = base.Text;
                if ( StringUtils.IsBlank( text ) ) {
                    if ( Type == CalendarType.yyyyMMdd ) {
                        text = DEFAULT_DAY_TEXT;
                    } else {
                        text = DEFAULT_MONTH_TEXT;
                    }
                }
                return text;
            }
            set {
                if ( null != GetDate( value ) ) {
                    base.Text = value;
                } else {
                    if ( Type == CalendarType.yyyyMMdd ) {
                        base.Text = DEFAULT_DAY_TEXT;
                    } else {
                        base.Text = DEFAULT_MONTH_TEXT;
                    }
                }
            }
        }
        #endregion

        #region パブリックメソッド
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public KTCalendar() {

        }

        /// <summary>
        /// 引数に指定したフォーマットで値を取得します
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public string ToString( DateUtils.FormatType type ) {
            return ToString( DateUtils.GetFormat( type ) );
        }

        /// <summary>
        /// 引数に指定したフォーマットで値を取得します
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        private string ToString( string format ) {
            if ( ObjectUtils.IsNull(this.Value) ) {
                return "";
            }

            return Value.Value.ToString( format );
        }
        #endregion

        #region オーバーライドイベント
        /// <summary>
        /// ロードイベント
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad( EventArgs e ) {

            if ( false == this.ReadOnly ) {

                //コントロールが使用するJavaScriptの登録
                this.Page.ClientScript.RegisterClientScriptInclude( GetScriptKey(), GetScriptFile() );

                string sJSFunc = "";
                //JavaScriptイベントの追加
                sJSFunc = string.Format( "{0}.{1}", GetScriptName(), JS_FUNC_INPUT_KEY );
                if ( ( null == this.Attributes[JS_INPUT_EVENT] ) ||
                    ( null != this.Attributes[JS_INPUT_EVENT] && -1 == this.Attributes[JS_INPUT_EVENT].IndexOf( sJSFunc ) ) ) {
                    this.Attributes[JS_INPUT_EVENT] += sJSFunc;
                }
                sJSFunc = string.Format( "{0}.{1}", GetScriptName(), JS_FUNC_FOCUS );
                if ( ( null == this.Attributes[JS_FOCUS_EVENT] ) ||
                    ( null != this.Attributes[JS_FOCUS_EVENT] && -1 == this.Attributes[JS_FOCUS_EVENT].IndexOf( sJSFunc ) ) ) {
                    this.Attributes[JS_FOCUS_EVENT] += sJSFunc;
                }
                sJSFunc = string.Format( "{0}.{1}", GetScriptName(), JS_FUNC_BLUR );
                if ( ( null == this.Attributes[JS_BLUR_EVENT] ) ||
                    ( null != this.Attributes[JS_BLUR_EVENT] && -1 == this.Attributes[JS_BLUR_EVENT].IndexOf( sJSFunc ) ) ) {
                    this.Attributes[JS_BLUR_EVENT] += sJSFunc;
                }
            }

            //コントロール入力桁設定
            this.MaxLength = Format.Length;

            //値未設定時には不正入力用表示フォーマットを設定
            if ( null == this.Value ) {
                this.Value = null;
            }

            //書込み不可
            //this.Attributes["readOnly"] = "readOnly";

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
            if ( false == this.ReadOnly ) {
                base.Render( writer );
            } else {

                String toolTip = "";
                if ( true == StringUtils.IsNotBlank( ToolTip ) ) {
                    toolTip = String.Format( "title=\"{0}\"", ToolTip );
                }

                string labelTag = "<span class=\"readonly {0}\" style=\"{1}\" {2}>{3}</span>";

                string width = 0 < this.Width.Value ? string.Format( "width:{0}px;", (int)this.Width.Value ) : "";
                string labelStyle = string.Format( "display:inline-block;{0}", width );

                string text = base.Text;
                if ( false == this.IsDate ) {
                    text = "";
                }
                writer.Write( labelTag, this.CssClass, labelStyle, toolTip, text );
            }
        }
        #endregion

        #region 内部メソッド

        /// <summary>
        /// 設定日時取得
        /// </summary>
        /// <returns></returns>
        private DateTime? GetDate(string dateStr) {
            return DateUtils.ToDateNullable( dateStr, this.Format );
        }

        /// <summary>
        /// 書式フォーマット取得
        /// </summary>
        /// <returns></returns>
        private string GetFormat() {

            string result = "";
            switch ( Type ) {

            case CalendarType.yyyyMMdd:
                result = TEXT_DAY_FORMAT;
                break;
            case CalendarType.yyyyMM:
                result = TEXT_MONTH_FORMAT;
                break;
            }

            return result;
        }

        /// <summary>
        /// デフォルトフォーマット取得
        /// </summary>
        /// <returns></returns>
        private string GetDefaultValue() {

            string result = "";
            switch ( Type ) {

            case CalendarType.yyyyMMdd:
                result = DEFAULT_DAY_TEXT;
                break;
            case CalendarType.yyyyMM:
                result = DEFAULT_MONTH_TEXT;
                break;
            }

            return result;
        }

        /// <summary>
        /// JSファイル定義取得
        /// </summary>
        /// <returns></returns>
        private string GetScriptName() {

            string result = "";
            switch ( Type ) {

            case CalendarType.yyyyMMdd:
                result = JS_NAME_DATE;
                break;
            case CalendarType.yyyyMM:
                result = JS_NAME_MONTH;
                break;
            }

            return result;
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

        #endregion

    }
}