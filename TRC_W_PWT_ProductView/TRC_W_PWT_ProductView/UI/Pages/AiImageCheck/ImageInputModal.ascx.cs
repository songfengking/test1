using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Diagnostics;
using System.Reflection;
using KTFramework.Common;
using KTFramework.Dao;
using KTWebControl.CustomControls;
using KTWebInheritance.Common;
using TRC_W_PWT_ProductView.Common;
using TRC_W_PWT_ProductView.Defines;
using TRC_W_PWT_ProductView.Defines.TypeDefine;
using TRC_W_PWT_ProductView.UI.Base;


namespace TRC_W_PWT_ProductView.UI.Pages.AiImageCheck {

    public partial class ImageInputModal : System.Web.UI.UserControl {

        //ロガー定義
        private static readonly Logger logger = new Logger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

        protected void Page_Load( object sender, EventArgs e ) {

            DoPageLoad();
        }

        /// <summary>
        /// ページロード処理
        /// </summary>
        protected virtual void DoPageLoad() {
//            MainModal.Attributes["onload"] = "return InputModal.setIframe();";
        }

        #region URL作成
        /// <summary>
        /// 表示画面のURL作成
        /// </summary>
        /// <param name="form">ページ</param>
        /// <param name="manageId">管理ID</param>
        /// <param name="index">インデックス</param>
        /// <param name="width">最大幅</param>
        /// <param name="height">最大高さ</param>
        /// <returns>クライアント用URL</returns>
        public static string ImageCreateDispUrl( Control setCtrl, PageInfo.ST_PAGE_INFO prmPage, int width, int height, ListItem[] keyItems, string extendKey, string dialogFlag ) {

            string strUrl = PageInfo.ResolveClientUrl( setCtrl, prmPage );  //URL
            string strKeys = "";
            string strTmp = "";

            //key設定
            if ( null != keyItems && 0 != keyItems.Length ) {
                for ( int intIdx = 0; intIdx < keyItems.Length; intIdx++ ) {
                    //keyと値をloopで設定していく
                    strTmp = keyItems[intIdx].Text + "=" + keyItems[intIdx].Value;
                    if ( intIdx == 0 ) {
                        strKeys = strTmp;
                    } else {
                        strKeys = strKeys + "&" + strTmp;
                    }
                }
            } 
            
            strUrl = "./" + strUrl + "?" + strKeys;

                string retURL = "";
            if ( dialogFlag == "list" ) {
                retURL = "return AiImageCheck.gridDouClickOpen('" + prmPage.pageId + "','" + strUrl + "','" + extendKey + "')";
            } else if ( dialogFlag == "input" ) {
                retURL = "return AiImageCheck.gridDouClickInputOpen('" + prmPage.pageId + "','" + strUrl + "','" + extendKey + "')";
            } else if ( dialogFlag == "backlist" ) {
                retURL = "return AiImageCheck.btnClickBackOpen('" + prmPage.pageId + "','" + strUrl + "','" + extendKey + "')";
            }
            return retURL;
        }
        #endregion
    }

}