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
    /// <remarks>
    /// 整数用 基本処理はKTDecimalTextBoxを継承
    /// </remarks>
    [DefaultProperty( "Text" )]
    [ToolboxData( "<{0}:KTNumericTextBox runat=\"Server\" />" )]
    public class KTNumericTextBox : KTDecimalTextBox {
        
        /// <summary>
        /// 値を取得/設定します。
        /// </summary>
        [Category( "表示" )]
        [Description( "値を取得/設定します。" )]
        [Browsable( false )]
        public new int? Value {
            get {
                int? value = null;
                string txt = base.Text.Replace(",", "");
                if ( true == NumericUtils.IsInt( txt ) ) {
                    value = NumericUtils.ToInt( txt );
                }
                return value;
            }
            set {
                base.Value =  value;
            }
        }
    }
}
