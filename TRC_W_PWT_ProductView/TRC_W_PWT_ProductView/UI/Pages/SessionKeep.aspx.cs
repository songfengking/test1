using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KTFramework.Common;
using TRC_W_PWT_ProductView.Common;

namespace TRC_W_PWT_ProductView.UI.Pages {
    /// <summary>
    /// セッションキープ画面
    /// </summary>
    public partial class SessionKeep : System.Web.UI.Page {

        //ロガー定義
        private static readonly Logger logger = new Logger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

        /// <summary>
        /// ロードイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load( object sender, EventArgs e ) {
            //セッション保持用のアクセス時間をWebConfigから取得
            int keepInterval = 0;
            try {
                keepInterval = WebAppInstance.GetInstance().Config.WebCommonInfo.sessionKeep;
            } catch ( Exception ex ) {
                logger.Error("アプリケーションインスタンス未生成");
                logger.Exception( ex );
            }
            if ( 0 < keepInterval ) {
                timSessionKeepTimer.Interval = keepInterval;
            } else {
                timSessionKeepTimer.Enabled = false;
            }

            String token = Page.Request.QueryString.Get( Defines.RequestParameter.Common.TOKEN );
            logger.Debug( "セッションキープイベント TOKEN:{0}", token );

        }

        /// <summary>
        /// セッション保持用イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void timSessionKeepTimer_Tick( object sender, EventArgs e ) {
        }
    }
}