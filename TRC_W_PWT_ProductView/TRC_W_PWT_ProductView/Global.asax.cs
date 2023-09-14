using System;
using System.Configuration;
using System.Reflection;
using System.Web;
using KTFramework.Common;
using KTFramework.Dao;
using KTWebInheritance.Common;
using TRC_W_PWT_ProductView.Common;
using TRC_W_PWT_ProductView.Defines;

namespace TRC_W_PWT_ProductView {
    /// <summary>
    /// グローバル
    /// </summary>
    public class Global : HttpApplication {
        /// <summary>ロガー定義</summary>
        private static readonly Logger logger = new Logger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

        /// <summary>
        /// アプリケーション起動処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_Start( object sender, EventArgs e ) {
            new WebAppInstance( Assembly.GetExecutingAssembly(), System.Environment.GetCommandLineArgs(), this );
            WebAppInstance.GetInstance().ReadConfig();
        }
        /// <summary>
        /// アプリケーション終了処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_End( object sender, EventArgs e ) {
            WebAppInstance.GetInstance().Dispose();
            logger.Info( "アプリケーションを終了します" );
        }

        /// <summary>
        /// エラー発生処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_Error( object sender, EventArgs e ) {
            if ( true == ObjectUtils.IsNotNull( Server )) {
                logger.Exception( Server.GetLastError() );
                //Server.ClearError();
            }
        }

        /// <summary>
        /// セッション開始処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Session_Start( object sender, EventArgs e ) {
            //セッションキーを設定
            LoggerManager.SetMdcSid( Session.SessionID );

            if ( true == PageInfo.CheckDefinePage( Request ) ) {
                logger.Info( "セッション開始 clientIp={0}", KTWebUtils.GetClientIp( Request ) );
            }
        }
        /// <summary>
        /// セッション終了処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Session_End( object sender, EventArgs e ) {
            try {
                if ( true == PageInfo.CheckDefinePage( Request ) ) {
                    logger.Info( "セッション終了 clientIp={0}", KTWebUtils.GetClientIp( Request ) );
                }
                LoggerManager.RemoveMdcSid();
            } catch ( Exception ex ) {
                logger.Exception( ex );
            }
        }

        /// <summary>
        /// リクエストからセッション要求の発生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_PostAcquireRequestState( object sender, EventArgs e ) {
            if ( true == ObjectUtils.IsNotNull( HttpContext.Current ) && true == ObjectUtils.IsNotNull( HttpContext.Current.Session ) ) {
                LoggerManager.SetMdcSid( Session.SessionID );
                if ( true == PageInfo.CheckDefinePage( Request ) ) {
                    DebugWriteSessionSize();
                }
            }
        }

        /// <summary>
        /// リクエスト開始処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_BeginRequest( object sender, EventArgs e ) {
            if ( true == PageInfo.CheckDefinePage( Request ) ) {
                logger.Info( "リクエスト処理開始 clientIp={0} absoluteUri={1}", KTWebUtils.GetClientIp( Request ), Request.Url.AbsoluteUri );
                logger.Debug( "  HttpMethod={0}", Request.HttpMethod );
                logger.Debug( "  RequestContext={0}", Request.RequestContext.ToString() );
            }
        }
        /// <summary>
        /// 認証処理開始
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_AuthenticateRequest( object sender, EventArgs e ) {
            if ( true == PageInfo.CheckDefinePage( Request ) ) {
                logger.Info( "認証処理開始 clientIp={0} absoluteUri={1}", KTWebUtils.GetClientIp( Request ), Request.Url.AbsoluteUri );
            }
        }

        /// <summary>
        /// リクエスト終了処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_EndRequest( object sender, EventArgs e ) {
            if ( true == PageInfo.CheckDefinePage( Request ) ) {
                //logger.Debug( "リクエスト処理終了" );
                logger.Info( "リクエスト処理終了 clientIp={0} absoluteUri={1}", KTWebUtils.GetClientIp( Request ), Request.Url.AbsoluteUri );
                if ( true == ObjectUtils.IsNotNull( HttpContext.Current ) && true == ObjectUtils.IsNotNull( HttpContext.Current.Session ) ) {
                    DebugWriteSessionSize();
                }
            }
        }

        /// <summary>
        /// セッションサイズ DebugWrite
        /// </summary>
        private void DebugWriteSessionSize() {

            logger.Debug( "------セッションサイズ(DEBUG用) START------" );

            long sesTotalSize = 0;
            foreach ( string sesKey in Session.Keys ) {

                object sesItem = Session[sesKey];
                string typeNm = "(NULL)";
                long sesSize = 0;
                if ( true == ObjectUtils.IsNotNull(sesItem ) ) {
                    System.Runtime.Serialization.Formatters.Binary.BinaryFormatter binfrmt = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    System.IO.MemoryStream memstrm = new System.IO.MemoryStream();
                    typeNm = sesItem.GetType().Name;
                    try {
                        binfrmt.Serialize( memstrm, sesItem );
                        sesSize = memstrm.Length;
                    } catch ( Exception ex ) {
                        //logger.Exception( ex );
                        logger.Debug( "Exception Msg:{0} StackTrace:{1}", ex.Message, ex.StackTrace );
                    } finally {
                        memstrm.Close();
                    }                
                }

                sesTotalSize += sesSize;
                logger.Debug( string.Format( "ID:{0} KEY:{1} 型:{2} サイズ:{3} Byte", Session.SessionID, sesKey, typeNm, sesSize ) );

            }

            logger.Debug( string.Format( "ID:{0} 合計サイズ:{1} Byte", Session.SessionID, sesTotalSize ) );
            logger.Debug( "------セッションサイズ(DEBUG用) END  ------" );

        }
    }
}