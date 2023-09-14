using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KTFramework.Common;
using TRC_W_PWT_ProductView.Defines;
using KTFramework.Web.Client.SvcCore;

namespace TRC_W_PWT_ProductView.Common {
    /// <summary>
    /// ユーザ実行権限取得クラス
    /// </summary>
    public class AppPermission {

        //ロガー定義
        private static readonly Logger logger = new Logger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

        /// <summary>DC3IniMap権限定義</summary>
        private const string TASK_KEY = "TRC";

        #region アプリケーション権限サフィックス定数定義
        /// <summary>アプリケーション権限:表示</summary>
        public static string AP_PERMISSION_VIEW = "View";
        /// <summary>アプリケーション権限:編集</summary>
        public static string AP_PERMISSION_EDIT = "Edit";
        /// <summary>アプリケーション権限:処理1</summary>
        public static string AP_PERMISSION_ACTION1 = "Action1";
        /// <summary>アプリケーション権限:処理2</summary>
        public static string AP_PERMISSION_ACTION2 = "Action2";
        /// <summary>アプリケーション権限:処理3</summary>
        public static string AP_PERMISSION_ACTION3 = "Action3";
        /// <summary>アプリケーション権限:処理4</summary>
        public static string AP_PERMISSION_ACTION4 = "Action4";
        /// <summary>アプリケーション権限:処理5</summary>
        public static string AP_PERMISSION_ACTION5 = "Action5";

        #endregion

        /// <summary>
        /// 画面利用権限構造体
        /// </summary>
        public struct PERMISSION_INFO {
            /// <summary>権限ID</summary>
            public string PermissionId;
            /// <summary>画面表示可否フラグ</summary>
            public Boolean IsView;
            /// <summary>編集トランザクション実行可否フラグ</summary>
            public Boolean IsEdit;
            /// <summary>処理1トランザクション実行可否フラグ(画面独自定義)</summary>
            public Boolean IsAction1;
            /// <summary>処理2トランザクション実行可否フラグ(画面独自定義)</summary>
            public Boolean IsAction2;
            /// <summary>処理3トランザクション実行可否フラグ(画面独自定義)</summary>
            public Boolean IsAction3;
            /// <summary>処理4トランザクション実行可否フラグ(画面独自定義)</summary>
            public Boolean IsAction4;
            /// <summary>処理5トランザクション実行可否フラグ(画面独自定義)</summary>
            public Boolean IsAction5;

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="permissionId">権限ID</param>
            public PERMISSION_INFO( string permissionId ) {
                PermissionId = permissionId;
                IsView = false;
                IsEdit = false;
                IsAction1 = false;
                IsAction2 = false;
                IsAction3 = false;
                IsAction4 = false;
                IsAction5 = false;
            }
        }

        /// <summary>
        /// 画面の各種実行権限を取得します
        /// </summary>
        /// <param name="page">画面ページ情報定義</param>
        /// <param name="userInfo">ログインユーザ情報</param>
        /// <returns>画面利用権限構造体</returns>
        public static PERMISSION_INFO GetTransactionPermission( PageInfo.ST_PAGE_INFO pageInfo, C9910WSUserInfoDto userInfo ) {

            PERMISSION_INFO permissionInfo = new PERMISSION_INFO("");

            //パラメータチェック
            if ( true == ObjectUtils.IsNull( pageInfo.pageId)
                || true == ObjectUtils.IsNull( userInfo )) {
                //パラメータ不足の場合には全項目利用不可
                return permissionInfo;
            }

            //permissionInfo = WebAppInstance.GetInstance().PermissionConfig.GetPermissionSetting( pageInfo.pageId );

            //権限キーを取得
            String detailPermission = pageInfo.pageId.ToUpper();

            //画面の権限キーをチェック
            if ( true == StringUtils.IsNotEmpty( detailPermission ) ) {

                bool isTask = true;//トレーサビリティでは基幹データを更新しない為、DC3IniMap権限不要(使用しない)
                //bool isTask = false;
                //if ( true == ObjectUtils.IsNotNull( userInfo.taskList ) ) {
                //    //指定された画面の遷移権限を検索(TaskIdから判定)
                //    for ( int taskIndex = 0; taskIndex < userInfo.taskList.Length; taskIndex++ ) {
                //        if ( null != userInfo.taskList[taskIndex] && userInfo.taskList[taskIndex].ToUpper() == TASK_KEY ) {
                //            //DC3IniMap権限あり
                //            isTask = true;
                //            break;
                //        }
                //    }
                //}

                //各処理権限を検索(ApPrivilegeListから判定)
                bool isPermissionSet = false;

                if ( true == isTask
                    && true == ObjectUtils.IsNotNull( userInfo.apAuthList ) ) {

                    for ( int apListIndex = 0; apListIndex < userInfo.apAuthList.Length; apListIndex++ ) {
                        if ( null != userInfo.apAuthList[apListIndex] ) {
                            //ユーザ権限情報を取得
                            if ( null != userInfo.apAuthList[apListIndex] ) {
                                isPermissionSet = SetTransactionPermission( ref permissionInfo, detailPermission, userInfo.apAuthList[apListIndex].ToUpper() );
                            }
                        }
                    }
                }
            }

            logger.Info( "アクセス権限:権限ID={0} 表示={1} 編集={2} 処理1={3} 処理2={4} 処理3={5} 処理4={6} 処理5={7}",
                permissionInfo.PermissionId, permissionInfo.IsView, permissionInfo.IsEdit
                , permissionInfo.IsAction1, permissionInfo.IsAction2, permissionInfo.IsAction3, permissionInfo.IsAction4, permissionInfo.IsAction5 );

            return permissionInfo;
        }

        /// <summary>
        /// 画面の表示権限を取得します
        /// </summary>
        /// <param name="pageInfo">遷移先画面のページ情報定義(一覧画面から遷移する先の画面を指定する)</param>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public static bool IsView( PageInfo.ST_PAGE_INFO pageInfo, C9910WSUserInfoDto userInfo ) {
            PERMISSION_INFO permissionInfo = GetTransactionPermission( pageInfo, userInfo );
            return permissionInfo.IsView;
        }

        /// <summary>
        /// 編集権限を取得します
        /// </summary>
        /// <param name="pageInfo">画面ページ情報定義</param>
        /// <param name="userInfo">ログインユーザ情報</param>
        /// <returns>true:利用可 false:利用不可</returns>
        public static bool IsEdit( PageInfo.ST_PAGE_INFO pageInfo, C9910WSUserInfoDto userInfo ) {
            PERMISSION_INFO permissionInfo = GetTransactionPermission( pageInfo, userInfo );
            return permissionInfo.IsEdit;
        }

        /// <summary>
        /// 処理1実行権限を取得します
        /// </summary>
        /// <param name="pageInfo">画面ページ情報定義</param>
        /// <param name="userInfo">ログインユーザ情報</param>
        /// <returns>true:利用可 false:利用不可</returns>
        public static bool IsAction1( PageInfo.ST_PAGE_INFO pageInfo, C9910WSUserInfoDto userInfo ) {
            PERMISSION_INFO permissionInfo = GetTransactionPermission( pageInfo, userInfo );
            return permissionInfo.IsAction1;
        }

        /// <summary>
        /// 処理2実行権限を取得します
        /// </summary>
        /// <param name="pageInfo">画面ページ情報定義</param>
        /// <param name="userInfo">ログインユーザ情報</param>
        /// <returns>true:利用可 false:利用不可</returns>
        public static bool IsAction2( PageInfo.ST_PAGE_INFO pageInfo, C9910WSUserInfoDto userInfo ) {
            PERMISSION_INFO permissionInfo = GetTransactionPermission( pageInfo, userInfo );
            return permissionInfo.IsAction2;
        }

        /// <summary>
        /// 処理3実行権限を取得します
        /// </summary>
        /// <param name="pageInfo">画面ページ情報定義</param>
        /// <param name="userInfo">ログインユーザ情報</param>
        /// <returns>true:利用可 false:利用不可</returns>
        public static bool IsAction3( PageInfo.ST_PAGE_INFO pageInfo, C9910WSUserInfoDto userInfo ) {
            PERMISSION_INFO permissionInfo = GetTransactionPermission( pageInfo, userInfo );
            return permissionInfo.IsAction3;
        }

        /// <summary>
        /// 処理4実行権限を取得します
        /// </summary>
        /// <param name="pageInfo">画面ページ情報定義</param>
        /// <param name="userInfo">ログインユーザ情報</param>
        /// <returns>true:利用可 false:利用不可</returns>
        public static bool IsAction4( PageInfo.ST_PAGE_INFO pageInfo, C9910WSUserInfoDto userInfo ) {
            PERMISSION_INFO permissionInfo = GetTransactionPermission( pageInfo, userInfo );
            return permissionInfo.IsAction4;
        }

        /// <summary>
        /// 処理5実行権限を取得します
        /// </summary>
        /// <param name="pageInfo">画面ページ情報定義</param>
        /// <param name="userInfo">ログインユーザ情報</param>
        /// <returns>true:利用可 false:利用不可</returns>
        public static bool IsAction5( PageInfo.ST_PAGE_INFO pageInfo, C9910WSUserInfoDto userInfo ) {
            PERMISSION_INFO permissionInfo = GetTransactionPermission( pageInfo, userInfo );
            return permissionInfo.IsAction5;
        }

        #region トランザクション権限チェック処理
        /// <summary>
        /// トランザクション権限設定処理
        /// </summary>
        /// <param name="permissionInfo">画面利用権限構造体(参照)</param>
        /// <param name="detailPermission">画面遷移権限キー</param>
        /// <param name="apPrevilege">ユーザ権限情報</param>
        /// <returns>true:権限割当成功 false:権限割当失敗</returns>
        private static bool SetTransactionPermission(ref PERMISSION_INFO permissionInfo, String permissionId, String apPrevilege) {
            bool result = false;
            //対象画面の画面遷移権限と同一文字列が含まれているかチェック
            bool isPermission = false;

            //表示権限を取得
            isPermission = IsView( permissionId, apPrevilege );
            if ( true == isPermission ) {
                permissionInfo.IsView = isPermission;
                result = true;
            }
            //編集権限を取得
            isPermission = IsEdit( permissionId, apPrevilege );
            if ( true == isPermission ) {
                permissionInfo.IsEdit = isPermission;
                result = true;
            }
            //処理1実行権限を取得
            isPermission = IsAction1( permissionId, apPrevilege );
            if ( true == isPermission ) {
                permissionInfo.IsAction1 = isPermission;
                result = true;
            }
            //処理2実行権限を取得
            isPermission = IsAction2( permissionId, apPrevilege );
            if ( true == isPermission ) {
                permissionInfo.IsAction2 = isPermission;
                result = true;
            }
            //処理3実行権限を取得
            isPermission = IsAction3( permissionId, apPrevilege );
            if ( true == isPermission ) {
                permissionInfo.IsAction3 = isPermission;
                result = true;
            }
            //処理4実行権限を取得
            isPermission = IsAction4( permissionId, apPrevilege );
            if ( true == isPermission ) {
                permissionInfo.IsAction4 = isPermission;
                result = true;
            }
            //処理5実行権限を取得
            isPermission = IsAction5( permissionId, apPrevilege );
            if ( true == isPermission ) {
                permissionInfo.IsAction5 = isPermission;
                result = true;
            }

            return result;
        }

        /// <summary>
        /// 画面表示権限
        /// </summary>
        /// <param name="permissionId">権限キー</param>
        /// <param name="apPrevilege">ユーザ権限情報</param>
        /// <returns>true:表示権限あり false:表示権限なし</returns>
        private static bool IsView( String permissionId, String apPrevilege ) {
            //ユーザ権限情報と、画面権限キーに表示権限サフィックスを付加した文字列から実行権限を取得
            return IsTransaction( ( string.Format("{0}_{1}", permissionId , AP_PERMISSION_VIEW) ).ToUpper(), apPrevilege );
        }

        /// <summary>
        /// 編集権限有無チェック
        /// </summary>
        /// <param name="permissionId">権限キー</param>
        /// <param name="apPrevilege">ユーザ権限情報</param>
        /// <returns>true:編集権限あり false:編集権限なし</returns>
        private static bool IsEdit( String permissionId, String apPrevilege ) {
            //ユーザ権限情報と、画面権限キーに編集権限サフィックスを付加した文字列から実行権限を取得
            return IsTransaction( ( string.Format( "{0}_{1}", permissionId, AP_PERMISSION_EDIT ) ).ToUpper(), apPrevilege );
        }

        /// <summary>
        /// 処理1実行権限有無チェック
        /// </summary>
        /// <param name="permissionId">権限キー</param>
        /// <param name="apPrevilege">ユーザ権限情報</param>
        /// <returns>true:モード1実行権限あり false:モード1実行権限なし</returns>
        private static bool IsAction1( String permissionId, String apPrevilege ) {
            //ユーザ権限情報と、画面権限キーに処理1実行権限サフィックスを付加した文字列から実行権限を取得
            return IsTransaction( ( string.Format( "{0}_{1}", permissionId, AP_PERMISSION_ACTION1 ) ).ToUpper(), apPrevilege );
        }

        /// <summary>
        /// 処理2実行権限有無チェック
        /// </summary>
        /// <param name="permissionId">権限キー</param>
        /// <param name="apPrevilege">ユーザ権限情報</param>
        /// <returns>true:処理2実行権限あり false:処理2実行権限なし</returns>
        private static bool IsAction2( String permissionId, String apPrevilege ) {
            //ユーザ権限情報と、画面権限キーに処理2実行権限サフィックスを付加した文字列から実行権限を取得
            return IsTransaction( ( string.Format( "{0}_{1}", permissionId, AP_PERMISSION_ACTION2 ) ).ToUpper(), apPrevilege );
        }

        /// <summary>
        /// 処理3実行権限有無チェック
        /// </summary>
        /// <param name="permissionId">権限キー</param>
        /// <param name="apPrevilege">ユーザ権限情報</param>
        /// <returns>true:処理3実行権限あり false:処理3実行権限なし</returns>
        private static bool IsAction3( String permissionId, String apPrevilege ) {
            //ユーザ権限情報と、画面権限キーに処理3実行権限サフィックスを付加した文字列から実行権限を取得
            return IsTransaction( ( string.Format( "{0}_{1}", permissionId, AP_PERMISSION_ACTION3 ) ).ToUpper(), apPrevilege );
        }

        /// <summary>
        /// 処理4実行権限有無チェック
        /// </summary>
        /// <param name="permissionId">権限キー</param>
        /// <param name="apPrevilege">ユーザ権限情報</param>
        /// <returns>true:処理4実行権限あり false:処理4実行権限なし</returns>
        private static bool IsAction4( String permissionId, String apPrevilege ) {
            //ユーザ権限情報と、画面権限キーに処理4実行権限サフィックスを付加した文字列から実行権限を取得
            return IsTransaction( ( string.Format( "{0}_{1}", permissionId, AP_PERMISSION_ACTION4 ) ).ToUpper(), apPrevilege );
        }

        /// <summary>
        /// 処理5実行権限有無チェック
        /// </summary>
        /// <param name="permissionId">権限キー</param>
        /// <param name="apPrevilege">ユーザ権限情報</param>
        /// <returns>true:処理5実行権限あり false:処理5実行権限なし</returns>
        private static bool IsAction5( String permissionId, String apPrevilege ) {
            //ユーザ権限情報と、画面権限キーに処理5実行権限サフィックスを付加した文字列から実行権限を取得
            return IsTransaction( ( string.Format( "{0}_{1}", permissionId, AP_PERMISSION_ACTION5 ) ).ToUpper(), apPrevilege );
        }

        /// <summary>
        /// ユーザ権限有無チェック
        /// </summary>
        /// <param name="transactionPermission">画面別各トランザクション権限(表示/指示/実績計上/処理1/処理2/処理3/処理4/処理5/マスターメンテナンス編集)</param>
        /// <param name="apPrevilege">ユーザ権限情報</param>
        /// <returns></returns>
        private static bool IsTransaction( String transactionPermission, String apPrevilege ) {
            bool result = false;
            //ユーザ権限情報と、画面別各種権限が一致すれば権限ありと判定
            if ( transactionPermission == apPrevilege ) {
                result = true;  //一致した場合には、権限ありと判定
            }
            return result;
        }
        #endregion
    }
}