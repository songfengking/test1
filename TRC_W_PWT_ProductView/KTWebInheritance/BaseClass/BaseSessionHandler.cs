using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KTFramework.Common;

namespace KTWebInheritance.BaseClass {

    /// <summary>
    /// セッションハンドラベースクラス
    /// </summary>
    [Serializable]
    public abstract class BaseSessionHandler {
        /// <summary> セッション情報 </summary>
        private Dictionary<string, object> _sessionInfo = null;
        /// <summary> セッション情報 </summary>
        protected Dictionary<string, object> SessionInfo { set { _sessionInfo = value; } get { return new Dictionary<string, object>( _sessionInfo ); } }

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BaseSessionHandler() {
            _sessionInfo = new Dictionary<string, object>();
        }
        #endregion

        #region セッション情報の操作

        /// <summary>
        /// セッション情報の設定
        /// </summary>
        /// <param name="key">設定するフィールドキー</param>
        /// <param name="fieldValue">設定するフィールド値</param>
        protected void Set( string key, object value ) {
            if ( true == _sessionInfo.ContainsKey( key ) ) {
                _sessionInfo[key] = value;
            } else {
                _sessionInfo.Add( key, value );
            }
        }
        /// <summary>
        /// セッション情報の取得
        /// </summary>
        /// <param name="key">取得するフィールドキー</param>
        /// <returns>フィールド値</returns>
        protected object Get( string key ) {
            object obj = null;
            if ( true == _sessionInfo.ContainsKey( key ) ) {
                obj = _sessionInfo[key];
            }
            return obj;
        }
        /// <summary>
        /// セッション情報の取得
        /// </summary>
        /// <param name="key">取得するフィールドキー</param>
        /// <returns>フィールド値</returns>
        protected string GetByString( string key ) {
            return StringUtils.Nvl( Get( key ) );
        }
        /// <summary>
        /// セッション情報の取得
        /// </summary>
        /// <param name="key">取得するフィールドキー</param>
        /// <returns>フィールド値</returns>
        protected int GetByInt( string key ) {
            return NumericUtils.ToInt( Get( key ) );
        }
        /// <summary>
        /// フィールドキーを指定してセッション情報の削除
        /// </summary>
        /// <param name="key">削除するフィールドキー</param>
        protected void Remove( string key ) {
            _sessionInfo.Remove( key );
        }
        /// <summary>
        /// セッション情報のクリア
        /// </summary>
        protected void Clear() {
            _sessionInfo.Clear();
        }
        /// <summary>
        /// セッション情報のクローンを作成
        /// </summary>
        /// <returns>セッション情報</returns>
        protected Dictionary<string, object> Clone() {
            return new Dictionary<string, object>( _sessionInfo );
        }
        #endregion
    }
}