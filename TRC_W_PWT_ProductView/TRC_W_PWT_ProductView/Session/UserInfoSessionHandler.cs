using KTWebInheritance.BaseClass;
using KTFramework.Common;
using System;
using System.Data;
using System.Collections.Generic;
using KTFramework.Web.Client.SvcCore;

namespace TRC_W_PWT_ProductView.Common {

    /// <summary>
    /// ユーザ情報セッションハンドラ
    /// </summary>
    [Serializable]
    public class UserInfoSessionHandler : BaseSessionHandler {

        /// <summary>
        /// ユーザ情報格納構造体
        /// </summary>
        [Serializable]
        public struct ST_USER {
            /// <summary>
            /// ユーザ情報
            /// </summary>
            public C9910WSUserInfoDto UserInfo;
        }

        /// <summary>ユーザ情報</summary>
        private C9910WSUserInfoDto _userInfo = null;
        /// <summary>ユーザ情報</summary>
        public C9910WSUserInfoDto UserInfo { get { return _userInfo; } set { _userInfo = value; } }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public UserInfoSessionHandler() : base() {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="handler">ログイン情報クラス</param>
        public UserInfoSessionHandler( UserInfoSessionHandler handler ) {
            SessionInfo = handler.Clone();
        }

        /// <summary>
        /// ユーザ情報
        /// </summary>
        /// <returns>ユーザ情報</returns>
        public ST_USER GetUserInfo() {

            ST_USER result = new ST_USER();

            object obj = base.Get( Defines.Session.SESSION_STATE_KEY_USER_INFO );
            if ( ObjectUtils.IsNotNull( obj ) ) {
                result = (ST_USER)obj;
            }

            return result;
        }

        /// <summary>
        /// ユーザ情報格納
        /// </summary>
        /// <param name="userInfo">ユーザ情報</param>
        public void SetUserInfo( ST_USER userInfo ) {
            base.Set( Defines.Session.SESSION_STATE_KEY_USER_INFO, userInfo );
        }

        /// <summary>
        /// ユーザ情報削除
        /// </summary>
        public void ClearUserInfo() {
            base.Remove( Defines.Session.SESSION_STATE_KEY_USER_INFO );
        }
    }
}