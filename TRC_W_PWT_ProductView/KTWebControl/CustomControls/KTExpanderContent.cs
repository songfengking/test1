using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

[assembly: System.Web.UI.TagPrefix( "KTWebControl.CustomControls", "KTCC" )]
namespace KTWebControl.CustomControls {
    [ToolboxData( "<{0}:KTExpanderContent runat=\"Server\"></{0}:KTExpanderContent>" )]
    [Designer( typeof( KTExpanderDesigners ) )]
    [ParseChildren( false )]
    public class KTExpanderContent : Panel {
        /// <summary>JavaScript名</summary>
        private const string JAVASCRIPT_NAME = "KTExpander";
        /// <summary>JavaScript登録名</summary>
        private const string JAVASCRIPT_REGIST_NAME = JAVASCRIPT_NAME + "_js";
        /// <summary>JavaScriptファイル名</summary>
        private const string JAVASCRIPT_FILE_NAME = JAVASCRIPT_NAME + ".js";
        /// <summary>JavaScriptフルパス</summary>
        private const string JAVASCRIPT_FULL_PATH = "~/scripts/CustomControls/" + JAVASCRIPT_FILE_NAME;

        /// <summary>
        /// 初期化処理
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad( EventArgs e ) {
            //コントロールが使用するJavaScriptの登録
            this.Page.ClientScript.RegisterClientScriptInclude( JAVASCRIPT_REGIST_NAME, ResolveUrl( JAVASCRIPT_FULL_PATH + "?AssemblyVersion=" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString() ) );
            string sJSFunc = "";
            //JavaScriptイベントの追加
            if ( typeof( KTExpander ) == Parent.GetType() ) {
                KTExpander expander = (KTExpander)this.Parent;
                sJSFunc = string.Format( "{0}.headerClick('{1}', '{2}', '{3}', '{4}');", JAVASCRIPT_NAME, this.ClientID, expander.imgOpenClose.ClientID, KTExpander.IMAGE_FILE_OPEN, KTExpander.IMAGE_FILE_CLOSE );
                ((KTExpander)Parent).OnClientClick = sJSFunc;
            }

            base.OnLoad( e );
        }
    }
}
