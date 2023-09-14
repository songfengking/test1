using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Reflection;
using KTFramework.Common;
using KTWebInheritance.Common;
using TRC_W_PWT_ProductView.Defines;

namespace TRC_W_PWT_ProductView.Common {

    /// <summary>
    /// ページ用ユーティリティクラス
    /// </summary>
    public class PageUtils {
        /// <summary>
        /// Ver番号を付与したクライアント用URLへ変換する
        /// </summary>
        /// <param name="page">ページインスタンス</param>
        /// <param name="path">URLパス</param>
        /// <returns>変換後URL</returns>
        public static string ResolveUrl( Page page, string path ) {

            string bindChar = "?";
            if ( 0 <= path.IndexOf( '?' ) ) {
                bindChar = "&";
            }

            return page.ResolveUrl( path ) + GetAssemblyVer( bindChar );
        }

        /// <summary>
        /// リリースVer番号取得
        /// </summary>
        /// <param name="bindChar">Parameter結合文字</param>
        /// <returns>リリースVer番号</returns>
        public static string GetAssemblyVer( string bindChar = "?" ) {
            return bindChar + "AssemblyVersion=" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        /// <summary>
        /// ページインスタンスからPageInfoを取得する
        /// </summary>
        /// <param name="page">ページインスタンス</param>
        /// <returns>画面定義情報</returns>
        public static PageInfo.ST_PAGE_INFO GetPageInfo( Page page ) {
            Type pageTp = page.GetType().BaseType;
            PageInfo.ST_PAGE_INFO pageInfo = new PageInfo.ST_PAGE_INFO();

            FieldInfo fieldInfo = typeof( PageInfo ).GetField( pageTp.Name );
            if ( true == ObjectUtils.IsNotNull( fieldInfo ) ) {
                pageInfo = (PageInfo.ST_PAGE_INFO)fieldInfo.GetValue( null );
            } else {
                fieldInfo = typeof( PageInfo.ST_PAGE_INFO ).GetField( pageTp.Name );
                if ( true == ObjectUtils.IsNotNull( fieldInfo ) ) {
                    pageInfo = (PageInfo.ST_PAGE_INFO)fieldInfo.GetValue( null );
                }
            }
            return pageInfo;
        }

        /// <summary>
        /// ページIDからPageInfoを取得する
        /// </summary>
        /// <param name="pageId">ページID</param>
        /// <returns>画面定義情報</returns>
        public static PageInfo.ST_PAGE_INFO GetPageInfo( String pageId ) {

            PageInfo.ST_PAGE_INFO pageInfo = new PageInfo.ST_PAGE_INFO();

            FieldInfo fieldInfo = typeof( PageInfo ).GetField( pageId );
            if ( true == ObjectUtils.IsNotNull( fieldInfo ) ) {
                pageInfo = (PageInfo.ST_PAGE_INFO)fieldInfo.GetValue( null );
            } else {
                fieldInfo = typeof( PageInfo.ST_PAGE_INFO ).GetField( pageId );
                if ( true == ObjectUtils.IsNotNull( fieldInfo ) ) {
                    pageInfo = (PageInfo.ST_PAGE_INFO)fieldInfo.GetValue( null );
                }
            }
            return pageInfo;
        }

        /// <summary>
        /// エラーページへ遷移する
        /// </summary>
        /// <param name="page">ページインスタンス</param>
        /// <param name="msgDef">メッセージ定義</param>
        /// <param name="msgParams">メッセージ定義パラメータ</param>
        internal static void RedirectToErrorPage( Page page, MsgDef msgDef, params object[] msgParams ) {
            Dictionary<string, string> dicErrorParam = new Dictionary<string, string>();
            dicErrorParam.Add( RequestParameter.Error.ERROR_CD, msgDef.Code );
            dicErrorParam.Add( RequestParameter.Error.ERROR_MSG, msgDef.ToString(msgParams) );
            KTWebUtils.RedirectTo( page, PageInfo.Error.url, dicErrorParam );
        }

        /// <summary>
        /// 画面遷移(Redirect) 前画面 トークンを引継ぎする
        /// </summary>
        /// <param name="page">遷移元ページ</param>
        /// <param name="callerPageId">呼び元ページID</param>
        /// <param name="toUrl">遷移先ページURL</param>
        /// <param name="token">トークン</param>
        /// <param name="requestParameters">リクエストパラメータ</param>
        internal static void RedirectToTRC( Page page, string callerPageId, string toUrl, string token, Dictionary<string, string> requestParameters ) {

            if ( true == ObjectUtils.IsNull( requestParameters ) ) {
                requestParameters = new Dictionary<string, string>();
            }

            if ( false == requestParameters.ContainsKey( RequestParameter.Common.CALLER ) ) {
                requestParameters.Add( RequestParameter.Common.CALLER, callerPageId );
            }

            if ( false == requestParameters.ContainsKey( RequestParameter.Common.TOKEN ) ) {
                requestParameters.Add( RequestParameter.Common.TOKEN, token );
            }

            KTWebUtils.RedirectTo( page, toUrl, requestParameters );
        }
    }
}