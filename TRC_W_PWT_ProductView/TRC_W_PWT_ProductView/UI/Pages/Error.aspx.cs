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

namespace TRC_W_PWT_ProductView.UI.Pages {
    /// <summary>
    /// エラー画面
    /// </summary>
    public partial class Error : BaseForm {

        #region プロパティ

        #endregion

        #region イベント

        /// <summary>
        /// ページロード
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load( object sender, EventArgs e ) {
            RaiseEvent( DoPageLoad );
        }

        /// <summary>
        /// ログイン(検索一覧)ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnLogin_Click( object sender, EventArgs e ) {
            RaiseEvent( DoLogin );
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
            string code = Page.Request.QueryString.Get( RequestParameter.Error.ERROR_CD );
            string message = Page.Request.QueryString.Get( RequestParameter.Error.ERROR_MSG );

            if ( true == StringUtils.IsEmpty( code ) || true == StringUtils.IsEmpty( message ) ) {
                code = MsgManager.MESSAGE_ERR_80010.Code;
                message = MsgManager.MESSAGE_ERR_80010.ToString();
            }

            lblErrorCode.Text = code;
            lblErrorInformation.Text = message;

            Msg msg = new Msg( code, message );
            base.WriteApplicationMessage( msg );
        }

        /// <summary>
        /// ログイン処理
        /// </summary>
        private void DoLogin() {
            KTWebUtils.RedirectTo( this.Page, PageInfo.LoginByTrc.url, null );
        }

        #endregion

    }
}