using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TRC_W_PWT_ProductView.UI.Pages {
    /// <summary>
    /// アプリケーション死活チェック画面
    /// </summary>
	public partial class ApCheck : Page {

        #region イベント
        
		/// <summary>
        /// 画面ロード処理
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad( EventArgs e ) {
            DoPageLoad();
        }
        
		#endregion

        #region イベントメソッド

        /// <summary>
        /// ページロード処理
        /// </summary>
        protected void DoPageLoad() {
            this.Session.Abandon();
			Response.Write( "OK" );
			return;
        }

        #endregion
    }

}