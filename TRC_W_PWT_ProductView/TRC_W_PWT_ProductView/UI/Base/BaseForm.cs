using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;
using System.Data;
using System.Diagnostics;
using KTFramework.Common;
using KTWebControl.CustomControls;
using KTWebInheritance.Common;
using TRC_W_PWT_ProductView.Defines;
using TRC_W_PWT_ProductView.Defines.TypeDefine;
using TRC_W_PWT_ProductView.Session;
using TRC_W_PWT_ProductView.Common;

namespace TRC_W_PWT_ProductView.UI.Base {

    /// <summary>
    /// ベースページ
    /// </summary>
    public class BaseForm:Page {

        //ロガー定義
        private static readonly Logger logger = new Logger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

        #region 共通プロパティ

        /// <summary>
        /// 表示中ページ情報
        /// </summary>
        protected internal PageInfo.ST_PAGE_INFO CurrentPageInfo {
            get {
                return Common.PageUtils.GetPageInfo( Page );
            }
        }

        /// <summary>
        /// トークン
        /// </summary>
        protected internal string Token {
            get {
                Type masterTp = this.Master.GetType().BaseType;
                if ( typeof( UI.MasterForm.MasterMain ) == masterTp ) {
                    return ( (UI.MasterForm.MasterMain)Master ).GetThisToken;
                } else if ( typeof( UI.MasterForm.MasterSub ) == masterTp ) {
                    return ( (UI.MasterForm.MasterSub)Master ).GetThisToken;
                }
                return "";
            }            
        }

        /// <summary>
        /// フォーカス(クライアントID)
        /// </summary>
        protected internal string ClientFocus {
            get {
                Type masterTp = this.Master.GetType().BaseType;
                if ( typeof( UI.MasterForm.MasterMain ) == masterTp ) {
                    return ( (UI.MasterForm.MasterMain)Master ).ClientFocus;
                } else if ( typeof( UI.MasterForm.MasterSub ) == masterTp ) {
                    return ( (UI.MasterForm.MasterSub)Master ).ClientFocus;
                }
                return "";
            }  
            set {
                Type masterTp = this.Master.GetType().BaseType;
                if ( typeof( UI.MasterForm.MasterMain ) == masterTp ) {
                    ( (UI.MasterForm.MasterMain)Master ).ClientFocus = value;
                } else if ( typeof( UI.MasterForm.MasterSub ) == masterTp ) {
                    ( (UI.MasterForm.MasterSub)Master ).ClientFocus = value;
                }
            }
        }

        /// <summary>
        /// マスターページ ボディ部プレースホルダー
        /// </summary>
        Control _masterBodyPlaceHolder = null;
        /// <summary>
        /// マスターページ ボディ部プレースホルダーアクセサー
        /// </summary>
        private Control MasterBodyPlaceHolder {
            get {
                if ( true == ObjectUtils.IsNull( _masterBodyPlaceHolder ) ) {
                    _masterBodyPlaceHolder = (ContentPlaceHolder)Master.FindControl( PageControlID.Master.PLACE_HOLDER_MASTER_BODY );
                }

                return _masterBodyPlaceHolder;
            }
        }

        /// <summary>
        /// アプリケーションメッセージ出力
        /// </summary>
        /// <param name="msgDef">メッセージ定義</param>
        /// <param name="parameters">メッセージパラメータ</param>
        protected internal void WriteApplicationMessage( MsgDef msgDef, params object[] parameters ) {
            StackFrame stackFrame = WriteMessageUtils.GetCalledStackFrame( MethodBase.GetCurrentMethod() );
            Type masterTp = this.Master.GetType().BaseType;
            if ( typeof( UI.MasterForm.MasterMain ) == masterTp ) {
                ( (UI.MasterForm.MasterMain)this.Master ).WriteApplicationMessage( stackFrame, msgDef, parameters );
            } else if ( typeof( UI.MasterForm.MasterSub ) == masterTp ) {
                ( (UI.MasterForm.MasterSub)this.Master ).WriteApplicationMessage( stackFrame, msgDef, parameters );
            }
        }

        /// <summary>
        /// アプリケーションメッセージ出力
        /// </summary>
        /// <param name="msg">メッセージ定義</param>
        protected internal void WriteApplicationMessage( Msg msg ) {
            StackFrame stackFrame = WriteMessageUtils.GetCalledStackFrame( MethodBase.GetCurrentMethod() );
            Type masterTp = this.Master.GetType().BaseType;
            if ( typeof( UI.MasterForm.MasterMain ) == masterTp ) {
                ( (UI.MasterForm.MasterMain)this.Master ).WriteApplicationMessage( stackFrame, msg );
            } else if ( typeof( UI.MasterForm.MasterSub ) == masterTp ) {
                ( (UI.MasterForm.MasterSub)this.Master ).WriteApplicationMessage( stackFrame, msg );
            }
        }

        /// <summary>
        /// アプリケーションメッセージクリア
        /// </summary>
        protected internal void ClearApplicationMessage() {
            Type masterTp = this.Master.GetType().BaseType;
            if ( typeof( UI.MasterForm.MasterMain ) == masterTp ) {
                ( (UI.MasterForm.MasterMain)this.Master ).ClearApplicationMessage();
            } else if ( typeof( UI.MasterForm.MasterSub ) == masterTp ) {
                ( (UI.MasterForm.MasterSub)this.Master ).ClearApplicationMessage();
            }
        }

        #endregion
        
        #region セッションアクセサー

        /// <summary>セッションマネージャ</summary>
        private SessionManagerInstance _sessionManager = null;
        /// <summary>セッションマネージャ</summary>
        protected internal SessionManagerInstance SessionManager {
            get {

                if ( true == ObjectUtils.IsNull( _sessionManager ) ) {
                    _sessionManager = new SessionManagerInstance( Page.Session );
                }

                return _sessionManager;
            }
        }


        /// <summary>
        /// ログインユーザセッション情報
        /// </summary>
        public UserInfoSessionHandler.ST_USER LoginInfo {
            get { return SessionManager.GetUserInfoHandler().GetUserInfo(); }
            set { SessionManager.GetUserInfoHandler().SetUserInfo( value ); }
        }

        /// <summary>
        /// 検索条件セッション情報
        /// </summary>
        protected ConditionInfoSessionHandler.ST_CONDITION ConditionInfo {
            get { return SessionManager.GetConditionInfoHandler( Token ).GetCondition( CurrentPageInfo.pageId ); }
            set { SessionManager.GetConditionInfoHandler( Token ).SetCondition( CurrentPageInfo.pageId, value ); }
        }

        /// <summary>
        /// 画面間引き継ぎセッション情報
        /// </summary>
        protected Dictionary<string, object> TransInfo {
            get { return SessionManager.GetTransitionInfoHandler( Token ).GetTransInfo( CurrentPageInfo.pageId ); }
            set { SessionManager.GetTransitionInfoHandler( Token ).SetTransInfo( CurrentPageInfo.pageId, value ); }
        }

        /// <summary>
        /// 画面コントロールセッション情報
        /// </summary>
        protected Dictionary<string, object> PageControlInfo {
            get { return SessionManager.GetPageControlInfoHandler( Token ).GetPageControlInfo( CurrentPageInfo.pageId ); }
            set { SessionManager.GetPageControlInfoHandler( Token ).SetPageControlInfo( CurrentPageInfo.pageId, value ); }
        }

        ///// <summary>
        ///// ViewStateセッション情報
        ///// </summary>
        //protected object ViewStateInfo {
        //    get { return SessionManager.GetViewStateInfoHandler( Token ).GetViewStateInfo( CurrentPageInfo.pageId ); }
        //    set { SessionManager.GetViewStateInfoHandler( Token ).SetViewStateInfo( CurrentPageInfo.pageId, value ); }
        //}

        /// <summary>
        /// 画面間引き継ぎ情報から指定キーに対する値を取得する
        /// </summary>
        /// <param name="key">キー</param>
        /// <returns>キーとペアの値</returns>
        protected object GetTransInfoValue( string key ) {

            object result = null;
            if ( true == TransInfo.ContainsKey( key ) ) {
                result = TransInfo[key];
            }

            return result;
        }

        /// <summary>
        /// 画面コントロール情報に指定キーで項目を追加します(既にキーが存在している時には更新します)
        /// </summary>
        /// <param name="key">キー</param>
        /// <param name="value">キーとペアの値</param>
        /// <remarks>
        /// 画面内コントロールとサーバIDがユニークの場合には、Dictionary
        /// 画面内コントロールがリスト項目に対する場合には、DictionaryのList
        /// </remarks>
        protected void SetPageControlInfo( string key, object value ) {
            Dictionary<string, object> dicPageControlValues = PageControlInfo;
            if ( false == dicPageControlValues.ContainsKey( key ) ) {
                dicPageControlValues.Add( key, value );
            } else {
                dicPageControlValues[key] = value;
            }
            PageControlInfo = dicPageControlValues;
        }

        #endregion

        #region イベント
        
        /// <summary>
        /// リクエスト取得処理
        /// </summary>
        /// <param name="context">HttpContext</param>
        public override void ProcessRequest( HttpContext context ) {
            
            //設定文字列の取得
            byte[] readData = new byte[context.Request.InputStream.Length];
            context.Request.InputStream.Read( readData, 0, readData.Length );
            string readStr = System.Text.Encoding.UTF8.GetString( readData );

            try {
                base.ProcessRequest( context );
            } catch ( Exception ex ) {
                //イベント処理中にエラー発生
                logger.Exception( ex );
                throw ex;
            }
        }

        /// <summary>
        /// ページエラー
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Error( Object sender, EventArgs e ) {

            //ログ出力
            Exception exception = Server.GetLastError();
            logger.Exception( exception );

            Common.PageUtils.RedirectToErrorPage( Page, MsgManager.MESSAGE_ERR_80010, null );

            //例外をクリア
            Server.ClearError();
        }

        #endregion

        #region イベントメソッド
        /// <summary>
        /// ページロード処理
        /// </summary>
        protected virtual void DoPageLoad() {

            Page.Title = CurrentPageInfo.title;

            //初回表示
            if ( false == IsPostBack
                && false == ScriptManager.GetCurrent( Page ).IsInAsyncPostBack ) {
                Initialize();
            }
        }

        #endregion
        
        #region メソッド

        /// <summary>
        /// 初期化処理
        /// </summary>
        protected virtual void Initialize() {
            logger.Info( "新規画面表示:{0} {1} {2} PARAM:{3}", CurrentPageInfo.pageId, CurrentPageInfo.title, CurrentPageInfo.url, Page.Request.QueryString.ToString() );
        }

        /// <summary>
        /// 画面終了時処理
        /// </summary>
        protected internal void Terminate() {

            logger.Info( "画面終了:{0} {1} {2} {3}", Token, CurrentPageInfo.pageId, CurrentPageInfo.title, CurrentPageInfo.url );

            //ページで使用中のセッションを削除
            //検索条件
            //SessionManager.GetConditionInfoHandler( Token ).ClearCondition( CurrentPageInfo.pageId );
            //画面間引き継ぎ情報
            //SessionManager.GetTransitionInfoHandler( Token ).ClearTransInfo( CurrentPageInfo.pageId );
            //画面コントロール情報
            //SessionManager.GetPageControlInfoHandler( Token ).ClearPageControlInfo( CurrentPageInfo.pageId );

            //画面毎にトークン別にした為、トークン内全セッション情報を削除する
            //SessionManager.ClearConditionInfoSessionHandler( Token );
            //SessionManager.ClearTransitionInfoSessionHandler( Token );
            //SessionManager.ClearPageControlInfoSessionHandler( Token );
            //SessionManager.ClearImageInfoSessionHandler( Token );
            SessionManager.ClearByToken( Token );

        }
        
        #endregion

        #region ライズイベント

        /// <summary>
        /// イベント汎用デリゲート
        /// </summary>
        protected internal delegate void RaiseEventHandler();

        /// <summary>
        /// イベント汎用デリゲート(パラメータあり)
        /// </summary>
        /// <param name="param">パラメータ</param>
        protected internal delegate void RaiseParamEventHandler( params object[] raiseParam );

        /// <summary>
        /// ライズイベント
        /// </summary>
        /// <param name="function">呼び出し先イベントハンドラ</param>
        /// <param name="msgAreaClear">メッセージ欄クリア</param>
        /// <returns>成否</returns>
        protected internal bool RaiseEvent( RaiseEventHandler function, bool msgAreaClear = true ) {

            bool result = false;

            try {

                if ( true == msgAreaClear ) {
                    ClearApplicationMessage();
                }

                if ( null == function ) {
                    throw new Exception( MsgManager.MESSAGE_ERR_80020.ToString( true == ObjectUtils.IsNotNull( function ) ? function.Method.Name : "(NULL)" ) );
                }
                //イベント開始ログ出力
                logger.Info( "【イベント開始】 {0}:{1}", function.Method.DeclaringType.Name, function.Method.Name );
                                
                function();

                result = true;
            } catch ( System.Threading.ThreadAbortException ) {
                //ログ出力しない
            } catch ( Exception ex ) {
                //イベント処理中にエラー発生
                logger.Exception( ex );
                throw ex;
            } finally {
                //イベント終了ログ出力
                logger.Info( "【イベント終了】 {0}:{1}", function.Method.DeclaringType.Name, function.Method.Name );
            }

            return result;
        }

        /// <summary>
        /// ライズイベント
        /// </summary>
        /// <param name="function">呼び出し先イベントハンドラ</param>
        /// <param name="parameters">パラメータ</param>
        /// <returns>成否</returns>
        protected internal bool RaiseEvent( RaiseParamEventHandler function, params object[] raiseParam ) {
            bool result = false;

            try {

                ClearApplicationMessage();

                if ( null == function ) {
                    throw new Exception( MsgManager.MESSAGE_ERR_80020.ToString( true == ObjectUtils.IsNotNull( function ) ? function.Method.Name : "(NULL)" ) );
                }
                //イベント開始ログ出力
                logger.Info( "【イベント開始】 {0}:{1}", function.Method.DeclaringType.Name, function.Method.Name );

                function( raiseParam );
                
                result = true;
            } catch ( System.Threading.ThreadAbortException ) {
                //ログ出力しない
            } catch ( Exception ex ) {
                //イベント処理中にエラー発生
                logger.Exception( ex );
                throw ex;
            } finally {
                //イベント終了ログ出力
                logger.Info( "【イベント終了】 {0}:{1}", function.Method.DeclaringType.Name, function.Method.Name );
            }

            return result;
        }

        #endregion

        #region コントロール制御
        
        /// <summary>
        /// データ反映(サーバ→クライアント)
        /// </summary>
        /// <param name="page">ページ</param>
        /// <param name="controlDefineArr">コントロール定義</param>
        /// <param name="rowBindSource">バインド元データ</param>
        /// <param name="dicBindResult">バインド結果データ</param>
        /// <remarks>MasterBodyプレースホルダー直下のコントロールに対して実行します</remarks>
        protected internal void SetControlValues( ControlDefine[] controlDefineArr, DataRow rowBindSource, ref Dictionary<string, object> dicBindResult ) {
            SetControlValues( MasterBodyPlaceHolder, controlDefineArr, rowBindSource, ref dicBindResult );
        }

        /// <summary>
        /// データ反映(サーバ→クライアント)
        /// </summary>
        /// <param name="parentControl">実行対象親コントロール</param>
        /// <param name="controlDefineArr">コントロール定義</param>
        /// <param name="rowBindSource">バインド元データ</param>
        /// <param name="dicBindResult">バインド結果データ</param>
        protected internal void SetControlValues( Control parentControl, ControlDefine[] controlDefineArr, DataRow rowBindSource, ref Dictionary<string, object> dicBindResult ) {

            if ( true == ObjectUtils.IsNull( dicBindResult ) ) {
                dicBindResult = new Dictionary<string, object>();
            }

            if ( true == ObjectUtils.IsNull( rowBindSource )
                || 0 == rowBindSource.Table.Columns.Count ){
                return;
            }

            foreach ( ControlDefine ctrlDef in controlDefineArr ) {

                if ( false == rowBindSource.Table.Columns.Contains( ctrlDef.bindField ) ) {
                    continue;
                }

                object bindval = null;

                if ( false == rowBindSource.Table.Columns.Contains( ctrlDef.bindField ) ) {
                    logger.Debug( "データバインド失敗(フィールド未定義) コントロール:{0} フィールド:{1}", ctrlDef.controlId );
                    continue;
                }

                if ( ObjectUtils.IsNotNull( rowBindSource[ctrlDef.bindField] ) ) {
                    bindval = rowBindSource[ctrlDef.bindField];
                }

                if ( ctrlDef.bindType == ControlDefine.BindType.Both
                    || ctrlDef.bindType == ControlDefine.BindType.Down ) {

                    logger.Debug( "データバインド コントロール:{0} フィールド:{1} 値:{2}", ctrlDef.controlId, ctrlDef.bindField, bindval );

                    Control ctrl = parentControl.FindControl( ctrlDef.controlId );
                    if ( ObjectUtils.IsNotNull( ctrl ) ) {
                        Common.ControlUtils.SetControlValue( ctrl, bindval );
                    } else {
                        logger.Debug( "データバインド失敗(コントロール未定義) コントロール:{0} フィールド:{1}", ctrlDef.controlId, ctrlDef.bindField );
                    }
                }

                if ( true == dicBindResult.ContainsKey( ctrlDef.bindField ) ) {
                    dicBindResult[ctrlDef.bindField] = bindval;
                } else {
                    dicBindResult.Add( ctrlDef.bindField, bindval );
                }
            }
        }

        /// <summary>
        /// データ反映(クライアント→サーバ)
        /// </summary>
        /// <param name="controlDefineArr">コントロール定義</param>
        /// <param name="dicBindResult">バインド先データ</param>
        /// <remarks>MasterBodyプレースホルダー直下のコントロールに対して実行します</remarks>
        protected internal void GetControlValues( ControlDefine[] controlDefineArr, ref Dictionary<string, object> dicBindResult ) {
            GetControlValues( MasterBodyPlaceHolder, controlDefineArr, ref dicBindResult );
        }

        /// <summary>
        /// データ反映(クライアント→サーバ)
        /// </summary>
        /// <param name="parentControl">実行対象親コントロール</param>
        /// <param name="controlDefineArr">コントロール定義</param>
        /// <param name="dicBindResult">バインド先データ</param>
        protected internal void GetControlValues( Control parentControl, ControlDefine[] controlDefineArr, ref Dictionary<string, object> dicBindResult ) {

            if ( true == ObjectUtils.IsNull( dicBindResult ) ) {
                dicBindResult = new Dictionary<string, object>();
            }
           
            foreach ( ControlDefine ctrlDef in controlDefineArr ) {

                if ( ctrlDef.bindType == ControlDefine.BindType.None
                    || ctrlDef.bindType == ControlDefine.BindType.Down ) {
                    continue;
                }

                Control ctrl = parentControl.FindControl( ctrlDef.controlId );
                if ( ObjectUtils.IsNotNull( ctrl ) ) {
                    Type castType = ctrlDef.valueTp;
                    object resultVal = null;
                    resultVal = Common.ControlUtils.GetControlValue( ctrl );

                    if ( true == ObjectUtils.IsNull( resultVal ) ) {
                        if ( true == dicBindResult.ContainsKey( ctrlDef.bindField ) ) {
                            dicBindResult[ctrlDef.bindField] = null;
                        } else {
                            dicBindResult.Add( ctrlDef.bindField, null );
                        }
                    } else {
                        if ( true == dicBindResult.ContainsKey( ctrlDef.bindField ) ) {
                            dicBindResult[ctrlDef.bindField] = Convert.ChangeType( resultVal, castType );
                        } else {
                            dicBindResult.Add( ctrlDef.bindField, Convert.ChangeType( resultVal, castType ) );
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 表示テキスト情報取得(コントロールID, 表示テキスト)
        /// </summary>
        /// <param name="ControlDefineArr">コントロール定義</param>
        /// <param name="dicIdWithText">表示テキスト情報</param>
        /// <remarks>MasterBodyプレースホルダー直下のコントロールに対して実行します</remarks>
        protected internal void GetControlTexts( ControlDefine[] controlDefineArr, out Dictionary<String, String> dicIdWithText ) {
            GetControlTexts( MasterBodyPlaceHolder, controlDefineArr, out dicIdWithText );
        }

        /// <summary>
        /// 表示テキスト情報取得(コントロールID, 表示テキスト)
        /// </summary>
        /// <param name="parentControl">実行対象親コントロール</param>
        /// <param name="ControlDefineArr">コントロール定義</param>
        /// <param name="dicIdWithText">表示テキスト情報</param>
        protected internal void GetControlTexts(  Control parentControl, ControlDefine[] controlDefineArr, out Dictionary<String, String> dicIdWithText ) {

            dicIdWithText = new Dictionary<string, string>();

            foreach ( ControlDefine ctrlDef in controlDefineArr ) {
                Control ctrl = parentControl.FindControl( ctrlDef.controlId );
                if ( ObjectUtils.IsNotNull( ctrl ) ) {
                    String resultTxt = "";
                    resultTxt = Common.ControlUtils.GetControlText( ctrl );
                    dicIdWithText.Add( ctrlDef.controlId, resultTxt );
                }
            }
        }

        #endregion

    }
}