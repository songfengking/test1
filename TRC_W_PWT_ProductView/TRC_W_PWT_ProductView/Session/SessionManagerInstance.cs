using System;
using System.Web.SessionState;
using KTWebInheritance.BaseClass;
using TRC_W_PWT_ProductView.Common;

namespace TRC_W_PWT_ProductView.Session {
    /// <summary>
    /// 共通セッションマネージャインスタンス
    /// </summary>
    [Serializable]
    public class SessionManagerInstance : BaseSessionManager {

        #region 定数定義

        /// <summary>
        /// ユーザ情報(固定Token)セッションキー
        /// </summary>
        string SESSION_STATE_KEY_USER_INFO = Defines.Session.SESSION_STATE_KEY_USER_INFO;

        /// <summary>
        /// ユーザ情報セッションキー
        /// </summary>
        string SESSION_KEY_USER_INFO = Defines.Session.SESSION_KEY_USER_INFO;

        /// <summary>
        /// 画面間引継情報セッションキー
        /// </summary>
        string SESSION_KEY_TRANSITION_INFO = Defines.Session.SESSION_KEY_TRANSITION_INFO;

        /// <summary>
        /// 検索条件情報セッションキー
        /// </summary>
        string SESSION_KEY_CONDITION_INFO = Defines.Session.SESSION_KEY_CONDITION_INFO;

        /// <summary>
        /// 画面コントロール情報セッションキー
        /// </summary>
        string SESSION_KEY_PAGE_CONTROL_INFO = Defines.Session.SESSION_KEY_PAGE_CONTROL_INFO;

        ///// <summary>
        ///// ViewState情報セッションキー
        ///// </summary>
        //string SESSION_KEY_VIEW_STATE_INFO = Defines.Session.SESSION_KEY_VIEW_STATE_INFO;

        /// <summary>
        /// イメージ情報セッションキー
        /// </summary>
        string SESSION_KEY_IMAGE_INFO = Defines.Session.SESSION_KEY_IMAGE_INFO;

        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="httpSessionState"></param>
        public SessionManagerInstance( HttpSessionState httpSessionState ) : base( httpSessionState ) {
        }

        #region メソッド
        /// <summary>
        /// 指定トークンの存在チェック
        /// </summary>
        /// <param name="token">トークン</param>
        /// <returns>存在有無</returns>
        new public bool IsAlive( string token ) {
            return base.IsAlive( token );
        }

        new internal void ClearByToken( string token ) {
            base.ClearByToken( token );
        }

        #endregion

        #region ユーザ情報セッションハンドラ
        /// <summary>
        /// ユーザ情報セッションハンドラ取得
        /// </summary>
        /// <returns>セッションハンドラ</returns>
        public UserInfoSessionHandler GetUserInfoHandler( ) {
            object obj = Get( SESSION_STATE_KEY_USER_INFO, SESSION_KEY_USER_INFO );
            UserInfoSessionHandler handler = null;
            if ( null != obj ) {
                handler = (UserInfoSessionHandler)obj;
            } else {
                handler = new UserInfoSessionHandler();
                SetUserInfoSessionHandler( handler );
            }
            return handler;
        }
        /// <summary>
        /// ユーザ情報セッションハンドラ設定
        /// </summary>
        /// <param name="handler">セッションハンドラ</param>
        public void SetUserInfoSessionHandler( UserInfoSessionHandler handler ) {
            Set( SESSION_STATE_KEY_USER_INFO, SESSION_KEY_USER_INFO, handler );
        }
        /// <summary>
        /// ユーザ情報セッションハンドラ削除
        /// </summary>
        public void ClearUserInfoSessionHandler() {
            RemoveByToken( SESSION_STATE_KEY_USER_INFO, SESSION_KEY_USER_INFO );
        }
        #endregion

        #region 画面間引継情報セッションハンドラ
        /// <summary>
        /// 画面間引継情報セッションハンドラ取得
        /// </summary>
        /// <returns>セッションハンドラ</returns>
        public TransInfoSessionHandler GetTransitionInfoHandler( string token ) {
            object obj = Get( token , SESSION_KEY_TRANSITION_INFO );
            TransInfoSessionHandler handler = null;
            if ( null != obj ) {
                handler = (TransInfoSessionHandler)obj;
            } else {
                handler = new TransInfoSessionHandler();
                SetTransInfoSessionHandler( token, handler );
            }
            return handler;
        }
        /// <summary>
        /// 画面間引継情報セッションハンドラ設定
        /// </summary>
        /// <param name="handler">セッションハンドラ</param>
        public void SetTransInfoSessionHandler( string token, TransInfoSessionHandler handler ) {
            Set( token, SESSION_KEY_TRANSITION_INFO, handler );
        }
        /// <summary>
        /// 画面間引継情報セッションハンドラ削除
        /// </summary>
        public void ClearTransitionInfoSessionHandler( string token ) {
            RemoveByToken( token, SESSION_KEY_TRANSITION_INFO );
        }
        #endregion

        #region 検索条件情報セッションハンドラ
        /// <summary>
        /// 検索条件情報セッションハンドラ取得
        /// </summary>
        /// <returns>セッションハンドラ</returns>
        public ConditionInfoSessionHandler GetConditionInfoHandler( string token ) {
            object obj = Get( token, SESSION_KEY_CONDITION_INFO );
            ConditionInfoSessionHandler handler = null;
            if ( null != obj ) {
                handler = (ConditionInfoSessionHandler)obj;
            } else {
                handler = new ConditionInfoSessionHandler();
                SetConditionInfoSessionHandler( token, handler );
            }
            return handler;
        }
        /// <summary>
        /// 検索条件情報セッションハンドラ設定
        /// </summary>
        /// <param name="handler">セッションハンドラ</param>
        public void SetConditionInfoSessionHandler( string token, ConditionInfoSessionHandler handler ) {
            Set( token, SESSION_KEY_CONDITION_INFO, handler );
        }
        /// <summary>
        /// 検索条件情報セッションハンドラ削除
        /// </summary>
        public void ClearConditionInfoSessionHandler( string token ) {
            RemoveByToken( token, SESSION_KEY_CONDITION_INFO );
        }
        #endregion

        #region 画面コントロール情報セッションハンドラ
        /// <summary>
        /// 画面コントロール情報セッションハンドラ取得
        /// </summary>
        /// <returns>セッションハンドラ</returns>
        public PageControlInfoSessionHandler GetPageControlInfoHandler( string token ) {
            object obj = Get( token, SESSION_KEY_PAGE_CONTROL_INFO );
            PageControlInfoSessionHandler handler = null;
            if ( null != obj ) {
                handler = (PageControlInfoSessionHandler)obj;
            } else {
                handler = new PageControlInfoSessionHandler();
                SetPageControlInfoSessionHandler( token, handler );
            }
            return handler;
        }
        /// <summary>
        /// 画面コントロール情報セッションハンドラ設定
        /// </summary>
        /// <param name="handler">セッションハンドラ</param>
        public void SetPageControlInfoSessionHandler( string token, PageControlInfoSessionHandler handler ) {
            Set( token, SESSION_KEY_PAGE_CONTROL_INFO, handler );
        }
        /// <summary>
        /// 画面コントロール情報セッションハンドラ削除
        /// </summary>
        public void ClearPageControlInfoSessionHandler( string token ) {
            RemoveByToken( token, SESSION_KEY_PAGE_CONTROL_INFO );
        }
        #endregion

        //#region ViewState情報セッションハンドラ
        ///// <summary>
        ///// ViewState情報セッションハンドラ取得
        ///// </summary>
        ///// <returns>セッションハンドラ</returns>
        //public ViewStateInfoSessionHandler GetViewStateInfoHandler( string token ) {
        //    object obj = Get( token, SESSION_KEY_VIEW_STATE_INFO );
        //    ViewStateInfoSessionHandler handler = null;
        //    if ( null != obj ) {
        //        handler = (ViewStateInfoSessionHandler)obj;
        //    } else {
        //        handler = new ViewStateInfoSessionHandler();
        //        SetViewStateInfoSessionHandler( token, handler );
        //    }
        //    return handler;
        //}
        ///// <summary>
        ///// ViewState情報セッションハンドラ設定
        ///// </summary>
        ///// <param name="handler">セッションハンドラ</param>
        //public void SetViewStateInfoSessionHandler( string token, ViewStateInfoSessionHandler handler ) {
        //    Set( token, SESSION_KEY_VIEW_STATE_INFO, handler );
        //}
        ///// <summary>
        ///// ViewState情報セッションハンドラ削除
        ///// </summary>
        //public void ClearViewStateInfoSessionHandler( string token ) {
        //    RemoveByToken( token, SESSION_KEY_VIEW_STATE_INFO );
        //}
        //#endregion

        #region イメージ情報セッションハンドラ
        /// <summary>
        /// イメージ情報セッションハンドラ取得
        /// </summary>
        /// <returns>セッションハンドラ</returns>
        public ImageInfoSessionHandler GetImageInfoHandler( string token ) {
            object obj = Get( token, SESSION_KEY_IMAGE_INFO );
            ImageInfoSessionHandler handler = null;
            if ( null != obj ) {
                handler = (ImageInfoSessionHandler)obj;
            } else {
                handler = new ImageInfoSessionHandler();
                SetImageInfoSessionHandler( token, handler );
            }
            return handler;
        }
        /// <summary>
        /// イメージ情報セッションハンドラ設定
        /// </summary>
        /// <param name="handler">セッションハンドラ</param>
        public void SetImageInfoSessionHandler( string token, ImageInfoSessionHandler handler ) {
            Set( token, SESSION_KEY_IMAGE_INFO, handler );
        }
        /// <summary>
        /// イメージ情報セッションハンドラ削除
        /// </summary>
        public void ClearImageInfoSessionHandler( string token ) {
            RemoveByToken( token, SESSION_KEY_IMAGE_INFO );
        }
        #endregion

    }
}