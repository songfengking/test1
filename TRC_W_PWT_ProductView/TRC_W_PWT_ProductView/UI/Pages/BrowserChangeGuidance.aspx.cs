using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KTFramework.Common;
using KTWebInheritance.Common;
using TRC_W_PWT_ProductView.UI.Base;
using TRC_W_PWT_ProductView.Common;
using TRC_W_PWT_ProductView.Defines;
using TRC_W_PWT_ProductView.Defines.TypeDefine;

namespace TRC_W_PWT_ProductView.UI.Pages {
    /// <summary>
    /// ブラウザ変更促し画面
    /// </summary>
    public partial class BrowserChangeGuidance : BaseForm {

        #region イベント

        /// <summary>
        /// ページロード
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load( object sender, EventArgs e ) {
            RaiseEvent( DoPageLoad );
        }
        #endregion

        #region イベントメソッド
        /// <summary>
        /// ページロード処理
        /// </summary>
        protected override void DoPageLoad() {
            //ベース ページロード処理
            base.DoPageLoad();

            //パラメータを取得
            string url = Page.Request.QueryString.Get( RequestParameter.BrowserChangeGuidance.URL);

            hyperLinkURL.NavigateUrl = url;
            hyperLinkURL.Text = url;

        }
        #endregion

    }
}