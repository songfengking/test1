using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;

[assembly: System.Web.UI.TagPrefix( "KTWebControl.CustomControls", "KTCC" )]
namespace KTWebControl.CustomControls {
    [ToolboxData( "<{0}:KTExpander runat=\"Server\"></{0}:KTExpander>" )]
    [Designer( typeof( ContainerControlDesigner ) )]
    [ParseChildren( false )]
    public class KTExpander : CompositeControl {

        #region 定数定義
        /// <summary>タイトルパネルID</summary>
        private const string PANEL_TITLE = "pnlTitle";
        /// <summary>開閉アイコン表示パネルID</summary>
        private const string IMAGE_OPEN_CLOSE = "imgOpenClose";
        /// <summary>タイトルラベルID</summary>
        private const string LABEL_TITLE = "lblTitle";
        /// <summary>開くアイコン</summary>
        public const string IMAGE_FILE_OPEN = @"DownAllow.png";
        /// <summary>閉じるアイコン</summary>
        public const string IMAGE_FILE_CLOSE = @"UpAllow.png";
        /// <summary>アイコンファイルパス</summary>
        private const string FILE_PATH_IMAGE = @"~/images/";
        /// <summary>子要素用CSSクラス名</summary>
        private const string CSS_KTEXPANDER_CONTENT = " KTExpanderContent ";
        #endregion

        #region メンバ変数定義
        /// <summary>タイトルラベル</summary>
        private Label _lblTitle = null;
        /// <summary>Expander表示タイトル</summary>
        private string _titleText = "";
        /// <summary>CssClass</summary>
        private string _cssPanelClass = "";
        /// <summary>ラベル用CssClass</summary>
        private string _cssLabelClass = "";
        ///<summary>子コントロール格納用コントロール</summary>
        private KTExpanderContent content = new KTExpanderContent();
        #endregion

        #region プロパティ
        /// <summary>
        /// タイトルパネル
        /// </summary>
        public Panel pnlTitle
        {
            get
            {
                return (Panel)FindControl( PANEL_TITLE );
            }
        }
        /// <summary>
        /// 開閉アイコン表示パネル
        /// </summary>
        public System.Web.UI.WebControls.Image imgOpenClose
        {
            get
            {
                return (System.Web.UI.WebControls.Image)FindControl( IMAGE_OPEN_CLOSE );
            }
        }
        /// <summary>
        /// タイトルラベル
        /// </summary>
        public Label lblTitle
        {
            get
            {
                return (Label)FindControl( LABEL_TITLE );
            }
        }
        /// <summary>Expander表示タイトル</summary>
        public string TitleText
        {
            set
            {
                _titleText = value;
            }
            get
            {
                return _titleText;
            }
        }
        /// <summary>CSSクラス名</summary>
        public string CssPanelClass
        {
            set
            {
                _cssPanelClass = value;
            }
            get
            {
                return _cssPanelClass;
            }
        }
        /// <summary>ラベル用CSSクラス名</summary>
        public string CssLabelClass
        {
            set
            {
                _cssLabelClass = value;
            }
            get
            {
                return _cssLabelClass;
            }
        }
        /// <summary>クリックイベント</summary>
        public string OnClientClick
        {
            set
            {
                pnlTitle.Attributes["onClick"] = value;
            }
        }

        /// <summary>
        /// 展開可能なコントロール一覧
        /// </summary>
        public ControlCollection IncludingControls
        {
            get
            {
                return content.Controls;
            }
        }
        #endregion

        /// <summary>
        /// 初期化処理
        /// </summary>
        protected override void CreateChildControls() {
            // 表示パネルの設定
            Panel pnlTitle = new Panel();
            pnlTitle.ID = PANEL_TITLE;
            pnlTitle.CssClass = _cssPanelClass;
            pnlTitle.Width = Unit.Percentage( 100 );
            // 開閉アイコンの設定
            System.Web.UI.WebControls.Image imgAllow = new System.Web.UI.WebControls.Image();
            imgAllow.ImageUrl = FILE_PATH_IMAGE + IMAGE_FILE_CLOSE;
            imgAllow.ID = IMAGE_OPEN_CLOSE;
            imgAllow.ImageAlign = System.Web.UI.WebControls.ImageAlign.Right;
            imgAllow.Height = Unit.Percentage( 100 );
            // 表示ラベルの設定
            _lblTitle = new Label();
            _lblTitle.Text = _titleText;
            _lblTitle.ID = LABEL_TITLE;
            _lblTitle.CssClass = _cssLabelClass;
            // 子要素の設定
            content.CssClass = CSS_KTEXPANDER_CONTENT;
            // コントロールを格納
            pnlTitle.Controls.Add( imgAllow );
            pnlTitle.Controls.Add( _lblTitle );
            Controls.Add( pnlTitle );
            Controls.Add( content );
        }

        /// <summary>
        /// 当コントロールを展開するスクリプトを発行
        /// </summary>
        public void Open() {
            // 展開するコントロールを取得
            if ( content != null ) {
                // 展開するスクリプトを作成
                var script = string.Format(
                    "KTExpander.Open('{0}','{1}','{2}');",
                    content.ClientID,
                    imgOpenClose.ClientID,
                    IMAGE_FILE_CLOSE );
                // ページ表示時に当コントロールを展開
                ScriptManager.RegisterStartupScript( content, content.GetType(), ClientID, script, true );
            }
        }

        /// <summary>
        /// 当コントロールを展開するスクリプトを発行(子要素含む)
        /// </summary>
        public void OpenAll() {
            // 当コントロールを展開
            Open();
            // 孫コントロールにKTExpanderが存在するか確認し、全て展開
            content.Controls.OfType<KTExpander>().ToList().ForEach( x => x.OpenAll() );
        }

        /// <summary>
        /// 当コントロールが展開できる位置にコントロールを格納する
        /// </summary>
        /// <param name="ctrl">コントロール</param>
        public void AddControl( Control ctrl ) {
            if(content == null ) {
                content = new KTExpanderContent();
            }
            // コントロールを設定
            content.Controls.Add( ctrl );
        }
    }
}
