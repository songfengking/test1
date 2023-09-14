using System;
using System.Collections;
using System.Web;
using System.Web.SessionState;
using KTFramework.Common;
using KTWebInheritance.Common;
using System.Collections.Generic;

namespace KTWebInheritance.BaseClass {

    /// <summary>
    /// セッションマネージャクラス
    /// </summary>
    [Serializable]
    public abstract class BaseSessionManager : IHttpSessionState {

        //ロガー定義
        private static readonly Logger logger = new Logger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

        /// <summary>セッションステータス</summary>
        protected HttpSessionState _httpSessionState = null;
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="baseSession">セッションステータス</param>
        public BaseSessionManager( HttpSessionState httpSessionState ) {
            _httpSessionState = httpSessionState;
        }

        #region トークン単位アクセサ
        /// <summary>
        /// セッション情報の存在チェック
        /// </summary>
        /// <param name="token">チェックするトークン</param>
        protected bool IsAlive( string token ) {
            return _httpSessionState[token] == null ? false : true;

        }
        /// <summary>
        /// セッション情報の設定
        /// </summary>
        /// <param name="token">設定するトークン</param>
        /// <param name="key">設定するキー</param>
        /// <param name="Value">設定するフィールド値</param>
        protected void Set( string token, string key, object value ) {

            lock ( _httpSessionState ) {
                object obj = null;
                if ( false == IsAlive( token ) ) {
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    dic.Add( key, value );
                    _httpSessionState.Add( token, dic );
                    return;
                }

                obj = _httpSessionState[token];
                if ( null != obj ) {
                    Dictionary<string, object> dic = (Dictionary<string, object>)obj;
                    if ( true == dic.ContainsKey( key ) ) {
                        dic[key] = value;
                        _httpSessionState[token] = dic;
                    } else {
                        dic.Add( key, value );
                        _httpSessionState.Add( token, dic );
                    }
                }
            }
        }
        /// <summary>
        /// セッション情報の取得
        /// </summary>
        /// <param name="token">取得するトークン</param>
        /// <param name="key">取得するフィールドキー</param>
        /// <returns>フィールド値</returns>
        protected object Get( string token, string key ) {
            object result = null;
            lock ( _httpSessionState ) {
                if ( true == IsAlive( token ) ) {
                    object obj = _httpSessionState[token];
                    if ( null != obj ) {
                        Dictionary<string, object> dic = (Dictionary<string, object>)obj;
                        if ( true == dic.ContainsKey( key ) ) {
                            result = dic[key];
                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// セッション情報の削除
        /// </summary>
        /// <param name="token">対象のトークン</param>
        /// <param name="key">削除対象のキー</param>
        protected void RemoveByToken( string token, string key ) {
            lock ( _httpSessionState ) {
                if ( true == IsAlive( token ) ) {
                    object obj = _httpSessionState[token];
                    if ( null != obj ) {
                        Dictionary<string, object> dic = (Dictionary<string, object>)obj;
                        if ( true == dic.ContainsKey( key ) ) {
                            dic.Remove( key );
                            _httpSessionState[token] = dic;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// セッション情報の削除
        /// </summary>
        /// <param name="token">削除するトークン</param>
        /// <returns>フィールド値</returns>
        protected internal void ClearByToken( string token ) {
            lock ( _httpSessionState ) {
                if ( true == IsAlive( token ) ) {
                    _httpSessionState.Remove( token );
                }
            }
        }
        #endregion

        #region IHttpSessionState メンバ
        /// <summary>
        /// セッション情報
        /// </summary>
        public void Abandon() {
            _httpSessionState.Abandon();
        }
        /// <summary>
        /// セッション情報設定
        /// </summary>
        /// <param name="name">セッションキー</param>
        /// <param name="value">設定値</param>
        public void Add( string name, object value ) {
            _httpSessionState.Add( name, value );
        }
        /// <summary>
        /// セッション情報削除
        /// </summary>
        /// <param name="name">セッションキー</param>
        public void Remove( string name ) {
            _httpSessionState.Remove( name );
            logger.Debug( "セッション REMOVE Key:{0}", name );
        }
        /// <summary>
        /// セッション情報削除
        /// </summary>
        /// <param name="index">セッションインデックス</param>
        public void RemoveAt( int index ) {
            _httpSessionState.RemoveAt( index );
            logger.Debug( "セッション REMOVE Index:{0}", index );
        }
        /// <summary>
        /// セッション情報クリア
        /// </summary>
        public void Clear() {
            _httpSessionState.Clear();
            logger.Debug( "セッション Clear" );
        }
        /// <summary>
        /// セッション情報削除
        /// </summary>
        public void RemoveAll() {
            _httpSessionState.RemoveAll();
            logger.Debug( "セッション RemoveAll" );
        }
        /// <summary>
        /// セッション情報取得
        /// </summary>
        /// <returns>コレクション</returns>
        public IEnumerator GetEnumerator() {
            return _httpSessionState.GetEnumerator();
        }
        /// <summary>
        /// セッション情報設定
        /// </summary>
        /// <param name="array">設定値</param>
        /// <param name="index">インデックス</param>
        public void CopyTo( Array array, int index ) {
            _httpSessionState.CopyTo( array, index );
        }
        /// <summary></summary>
        public string SessionID { get { return _httpSessionState.SessionID; } }
        /// <summary></summary>
        public int Timeout { get { return _httpSessionState.Timeout; } set { _httpSessionState.Timeout = value; } }
        /// <summary></summary>
        public bool IsNewSession { get { return _httpSessionState.IsNewSession; } }
        /// <summary></summary>
        public SessionStateMode Mode { get { return _httpSessionState.Mode; } }
        /// <summary></summary>
        public bool IsCookieless { get { return _httpSessionState.IsCookieless; } }
        /// <summary></summary>
        public HttpCookieMode CookieMode { get { return _httpSessionState.CookieMode; } }
        /// <summary></summary>
        public int LCID { get { return _httpSessionState.LCID; } set { _httpSessionState.LCID = value; } }
        /// <summary></summary>
        public int CodePage { get { return _httpSessionState.CodePage; } set { _httpSessionState.CodePage = value; } }
        /// <summary></summary>
        public HttpStaticObjectsCollection StaticObjects { get { return _httpSessionState.StaticObjects; } }
        /// <summary></summary>
        public object this[int index] { get { return _httpSessionState[index]; } set { _httpSessionState[index] = value; } }
        /// <summary></summary>
        public object this[string name] { get { return _httpSessionState[name]; } set { _httpSessionState[name] = value; } }
        /// <summary></summary>
        public int Count { get { return _httpSessionState.Count; } }
        /// <summary></summary>
        public System.Collections.Specialized.NameObjectCollectionBase.KeysCollection Keys { get { return _httpSessionState.Keys; } }
        /// <summary></summary>
        public object SyncRoot { get { return _httpSessionState.SyncRoot; } }
        /// <summary></summary>
        public bool IsReadOnly { get { return _httpSessionState.IsReadOnly; } }
        /// <summary></summary>
        public bool IsSynchronized { get { return _httpSessionState.IsSynchronized; } }
        #endregion

    }
}