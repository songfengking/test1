#define FORCE_AD
#undef FORCE_AD

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KTWebInheritance.Common;
using TRC_W_PWT_ProductView.Common;
using TRC_W_PWT_ProductView.UI.Base;
using TRC_W_PWT_ProductView.Defines;

namespace TRC_W_PWT_ProductView.UI.Pages {
    /// <summary>
    /// トップページ
    /// </summary>
    public partial class Top : Page {

        #region 定数定義(DEBUG用)
        /// <summary>
        /// ログオンサービスURL
        /// </summary>
        //static readonly string AP_LOGON_URL = @"http://133.253.80.5:8079/logon/?apid=" + WebAppInstance.KTAUTH_AP_MASTER_ID + "&urlno=2&dbno=2&data=gustsLogin=1";
        #endregion

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
//#if FORCE_AD
//            KTWebUtils.RedirectTo( this, AP_LOGON_URL, null );
//#else
            //KTWebUtils.RedirectTo( this, PageInfo.LoginByAD.url, null );
//#endif

            Dictionary<string, string> dicTransInfo = new Dictionary<string, string>();
            foreach ( string reqKey in Page.Request.QueryString.AllKeys ) {
                dicTransInfo.Add( reqKey, Page.Request.QueryString.Get( reqKey ) );
            }
            KTWebUtils.RedirectTo( this, PageInfo.LoginByTrc.url, dicTransInfo );
        }

        #endregion
    }

}