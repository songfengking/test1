using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Reflection;
using KTFramework.Common;
using KTFramework.Dao;
using KTWebInheritance.Common;
using TRC_W_PWT_ProductView.Common;
using TRC_W_PWT_ProductView.UI.Base;
using TRC_W_PWT_ProductView.Session;
using TRC_W_PWT_ProductView.Defines;
using TRC_W_PWT_ProductView.Defines.ListDefine;
using TRC_W_PWT_ProductView.Defines.TypeDefine;
using TRC_W_PWT_ProductView.Business;

namespace TRC_W_PWT_ProductView.UI.Pages {
    /// <summary>
    /// 画像表示
    /// </summary>
    public partial class PreViewForm : BaseForm {

        //ロガー定義
        private static readonly Logger logger = new Logger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

        #region 定数
        private const string CONST_TRANS_PATH = "./PreViewForm.aspx?CallerPage=";
        #endregion

        #region 画面初期化処理
        /// <summary>
        /// 画面ロード処理
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad( EventArgs e ) {
            DoPageLoad();
        }
        #endregion

        #region プロパティ

        #endregion

        #region イベントメソッド

        /// <summary>
        /// ページロード処理
        /// </summary>
        protected override void DoPageLoad() {

            //呼び出し元の画面IDを取得し、起動させる画面を設定する
            //好ましくないが、国コードに設定された遷移元画面IDを取得する
            string dispURL = StringUtils.ToString( Request.QueryString["countryCd"] );

            //ベース ページロード処理
//            base.DoPageLoad();
            SetDetailControl( dispURL );
        }

        #endregion

        /// <summary>
        /// 詳細画面セット
        /// </summary>
        /// <param name="callerURL">遷移元画面</param>
        private void SetDetailControl( string callerURL ) {

            System.Web.UI.UserControl uc = new System.Web.UI.UserControl();

            //遷移元画面で起動画面分岐
            if ( PageInfo.CheckSheet.pageId.Equals( callerURL ) ) { 
                //チェックシート
                uc = (System.Web.UI.UserControl)LoadControl( PageInfo.PrintCheckSheet.url );
                uc.ID = PageInfo.PrintCheckSheet.pageId;
            } else if ( PageInfo.ELCheckSheet.pageId.Equals( callerURL ) ) {
                //電子チェックシート
            } else if ( PageInfo.ShipmentParts.pageId.Equals( callerURL ) ) {
                // 出荷部品
                uc = (System.Web.UI.UserControl)LoadControl( PageInfo.PrintCheckSheet.url );
                uc.ID = PageInfo.PrintCheckSheet.pageId;
            }

            //対象画面を設定
            pnlDetailControl.Controls.Add( uc );
            ( (Defines.Interface.IDetail)uc ).Initialize();

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="page">遷移元画面情報</param>
        /// <param name="path">イメージURL</param>
        /// <returns>印刷画面用URL</returns>
        public string SetPrintUrl( PageInfo.ST_PAGE_INFO page, string path ) {
            string retURL = "";
            string tmp = "";

            if ( StringUtils.IsEmpty( path ) ) {
                return retURL;
            }
            
            int idx = path.IndexOf( "?" );

            if ( idx.Equals( 0 ) ) {
                tmp = path;
            } else {
                tmp = path.Substring( idx + 1 );
            }

            retURL = CONST_TRANS_PATH + page.pageId;

            return retURL;
        }

    }
}