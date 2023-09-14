using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using KTFramework.Common;

namespace KTWebInheritance.Common {

    /// <summary>
    /// メッセージ欄更新クラス
    /// </summary>
    public static class WriteMessageUtils {

        //ロガー定義
        private static readonly Logger logger = new Logger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

        #region 前景背景色定義

        /// <summary>
        /// 背景色 初期値 白
        /// </summary>
        private static readonly Color FOOTER_BACK_COLOR_DEFAULT = Color.White;
        /// <summary>
        /// 背景色 情報 青
        /// </summary>
        private static readonly Color FOOTER_BACK_COLOR_INFO = Color.DodgerBlue;
        /// <summary>
        /// 背景色 警告 橙
        /// </summary>
        private static readonly Color FOOTER_BACK_COLOR_WARN = Color.Orange;
        /// <summary>
        /// 背景色 異常 赤
        /// </summary>
        private static readonly Color FOOTER_BACK_COLOR_ERROR = Color.Red;
        /// <summary>
        /// 前景色 初期値 黒
        /// </summary>
        private static readonly Color FOOTER_FORE_COLOR_DEFAULT = Color.Black;
        /// <summary>
        /// 前景色 情報 白
        /// </summary>
        private static readonly Color FOOTER_FORE_COLOR_INFO = Color.White;
        /// <summary>
        /// 前景色 警告 白
        /// </summary>
        private static readonly Color FOOTER_FORE_COLOR_WARN = Color.White;
        /// <summary>
        /// 前景色 異常 白
        /// </summary>
        private static readonly Color FOOTER_FORE_COLOR_ERROR = Color.White;

        #endregion

        #region メッセージエリア更新処理
        /// <summary>
        /// アプリケーションメッセージ表示
        /// </summary>
        /// <param name="txtMsgArea">メッセージ表示テキストボックス</param>
        /// <param name="messageInfo">表示メッセージ情報</param>
        public static void SetApplicationMessage( TextBox txtMsgArea, MsgDef messageInfo ) {
            //ログ出力用メッセージを設定
            StackFrame stackFrame = GetCalledStackFrame();
            SetApplicationMessage( stackFrame, txtMsgArea, messageInfo, null );
        }

        /// <summary>
        /// アプリケーションメッセージ表示
        /// </summary>
        /// <param name="stackFrame">スタックフレーム</param>
        /// <param name="txtMsgArea">メッセージ表示テキストボックス</param>
        /// <param name="messageInfo">表示メッセージ情報</param>
        /// <param name="parameter">フォーマット指定時の表示文字列（MsgDefに「{0}」等が指定されている場合に使用）</param>
        public static void SetApplicationMessage( StackFrame stackFrame, TextBox txtMsgArea, MsgDef messageInfo, params object[] parameter ) {
            Msg message = new Msg( messageInfo, parameter );
            SetApplicationMessage( stackFrame, txtMsgArea, message );
        }

        /// <summary>
        /// アプリケーションメッセージ表示
        /// </summary>
        /// <param name="txtMsgArea">メッセージ表示テキストボックス</param>
        /// <param name="message">表示メッセージ情報</param>
        public static void SetApplicationMessage( TextBox txtMsgArea, Msg message ) {
            //ログ出力用メッセージを設定
            StackFrame stackFrame = GetCalledStackFrame();
            SetApplicationMessage( stackFrame, txtMsgArea, message );
        }

        /// <summary>
        /// アプリケーションメッセージ表示
        /// </summary>
        /// <param name="stackFrame">スタックフレーム</param>
        /// <param name="txtMsgArea">メッセージ表示テキストボックス</param>
        /// <param name="message">表示メッセージ情報</param>
        public static void SetApplicationMessage( StackFrame stackFrame, TextBox txtMsgArea, Msg message ) {

            //初期化
            ClearApplicationMessage( txtMsgArea );

            string logMessage = string.Format( "画面メッセージ表示 Code:{0} message:{1} FileName:{2} MethodName:{3} LineNumber:{4}"
                , message.Code, message.ToString(), stackFrame.GetMethod().DeclaringType.Name, stackFrame.GetMethod().Name, stackFrame.GetFileLineNumber() );
            if ( true == message.IsError ) {
                txtMsgArea.ForeColor = FOOTER_FORE_COLOR_ERROR;
                txtMsgArea.BackColor = FOOTER_BACK_COLOR_ERROR;
                logger.Error( logMessage );
            } else if ( true == message.IsWarn ) {
                txtMsgArea.ForeColor = FOOTER_FORE_COLOR_WARN;
                txtMsgArea.BackColor = FOOTER_BACK_COLOR_WARN;
                logger.Warn( logMessage );
            } else {
                txtMsgArea.ForeColor = FOOTER_FORE_COLOR_INFO;
                txtMsgArea.BackColor = FOOTER_BACK_COLOR_INFO;
                logger.Info( logMessage );
            }
            txtMsgArea.Text = message.ToString();
        }

        /// <summary>
        /// アプリケーションメッセージクリア
        /// </summary>
        /// <param name="txtMsgArea">メッセージ表示テキストボックス</param>
        public static void ClearApplicationMessage( TextBox txtMsgArea ) {
            txtMsgArea.ForeColor = FOOTER_FORE_COLOR_DEFAULT;
            txtMsgArea.BackColor = FOOTER_BACK_COLOR_DEFAULT;
            txtMsgArea.Text = "";
        }

        /// <summary>
        /// 呼び出し元のスタックフレームを取得
        /// </summary>
        /// <param name="excludeMethod">除外メソッド</param>
        /// <returns>スタックフレーム</returns>
        public static StackFrame GetCalledStackFrame( MethodBase excludeMethod = null ) {
            StackTrace stackTrace = new System.Diagnostics.StackTrace( 0, true );
            StackFrame stackFrame = null;
            for ( int loop = 0; loop < stackTrace.FrameCount; loop++ ) {
                stackFrame = stackTrace.GetFrame( loop );
                if ( stackFrame.GetMethod().DeclaringType.Name == MethodBase.GetCurrentMethod().DeclaringType.Name
                    && stackFrame.GetMethod().Name == MethodBase.GetCurrentMethod().Name ) {
                    continue;
                }
                if ( true == ObjectUtils.IsNotNull( excludeMethod ) ) {
                    if ( stackFrame.GetMethod().DeclaringType.Name == excludeMethod.DeclaringType.Name
                        && stackFrame.GetMethod().Name == excludeMethod.Name ) {
                        continue;
                    }
                }
                stackFrame = stackTrace.GetFrame( loop );
                break;
            }

            return stackFrame;
        }
        #endregion
    }
}