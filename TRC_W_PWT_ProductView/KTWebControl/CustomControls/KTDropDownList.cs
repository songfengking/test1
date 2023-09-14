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
    /// 拡張ドロップダウンリストコントロール
    /// </summary>
    [DefaultProperty( "SelectedValue" )]
    [ToolboxData( "<{0}:KTDropDownList runat=\"Server\" />" )]
    public class KTDropDownList : DropDownList {

        #region JavaScript
        /// <summary>JavaScript名</summary>
        private const string JS_NAME = "KTDropDownList";
        /// <summary>JavaScriptパス</summary>
        private const string INCLUDE_JS_URL = "~/Scripts/CustomControls/";

        #endregion

        #region CSS
        #endregion

        #region 拡張プロパティ
        /// <summary>
        /// 読取専用プロパティ
        /// </summary>
        [Category( "表示" )]
        [Description( "読取専用コントロールかどうか取得/設定します。" )]
        public bool ReadOnly {
            get {
                object vs = ViewState["ReadOnly"];
                return ( null == vs ? false : (bool)vs );
            }
            set {
                ViewState["ReadOnly"] = value;
            }
        }
        /// <summary>
        /// WhiteSpace Pre
        /// </summary>
        [Category( "表示" )]
        [Description( "表示をWhiteSpace Preかどうか取得/設定します。" )]
        public bool WhiteSpacePre {
            get {
                object vs = ViewState["WhiteSpacePre"];
                return ( null == vs ? false : (bool)vs );
            }
            set {
                ViewState["WhiteSpacePre"] = value;
            }
        }
        #endregion

        #region オーバーライドプロパティ
        /// <summary>
        /// テキストプロパティ
        /// </summary>
        [Category( "表示" )]
        [Description( "表示テキストを取得します。" )]
        public virtual new string Text {
            get {
                string text = "";
                if ( 0 <= this.SelectedIndex ) {
                    text = this.SelectedItem.Text;
                }
                return text;
            }
            /*
            set {
                base.Text = value;
            }
            */
        }
        /// <summary>
        /// 値を取得/選択します。
        /// </summary>
        public override string SelectedValue {
            get {
                return base.SelectedValue;
            }
            set {
                if ( true == ObjectUtils.IsNull( Items.FindByValue( value ) ) ) {
                    base.SelectedIndex = -1;
                    return;
                }
                base.SelectedValue = value;
            }
        }
        /// <summary>
        /// 活性状態を取得/変更します
        /// </summary>
        public override bool Enabled {
            get {
                bool enabled = base.Enabled;
                if ( 0 == Items.Count ) {
                    enabled = false;
                }
                return enabled;
            }
            set {
                base.Enabled = value;
            }
        }
        #endregion

        #region オーバーライドメソッド
        /// <summary>
        /// ロードイベント
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad( EventArgs e ) {

            //コントロールが使用するJavaScriptの登録
            //this.Page.ClientScript.RegisterClientScriptInclude( GetScriptKey(), GetScriptFile() );     

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

            if ( true == WhiteSpacePre ) {
                if ( 0 < Items.Count ) {
                    foreach ( ListItem item in Items ) {
                        item.Text = item.Text.Replace( " ", System.Web.HttpUtility.HtmlDecode( "&nbsp;" ) );
                    }
                }
            }

            //ReadOnly 又は アイテム数が0件の時にはコントロールをSPAN表示 → ReadOnly指定のみに変更
            //if ( false == this.ReadOnly && 0 < this.Items.Count) {
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

                string text = this.Text;
                if ( 0 <= this.SelectedIndex ) {
                    text = this.SelectedItem.Text;
                }
                writer.Write( labelTag, this.CssClass, labelStyle, toolTip, text );
            }
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
