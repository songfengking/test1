using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Reflection;
using KTFramework.Common;
using KTFramework.Dao;
using KTWebInheritance.Common;
using TRC_W_PWT_ProductView.Common;
using TRC_W_PWT_ProductView.UI.Base;
using TRC_W_PWT_ProductView.Session;
using TRC_W_PWT_ProductView.Defines;
using TRC_W_PWT_ProductView.Defines.ListDefine;
using TRC_W_PWT_ProductView.Defines.TypeDefine;
using TRC_W_PWT_ProductView.Dao.Com;
using TRC_W_PWT_ProductView.Dao.Process;
using TRC_W_PWT_ProductView.Business;
using System.Diagnostics;

namespace TRC_W_PWT_ProductView.UI.Pages.AiImageCheck {
    public partial class AnlItemInput : BaseForm {

        #region 定数定義

        //ロガー定義
        private static readonly Logger logger = new Logger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );
        const string CLOSE_MODAL_DISP = "AnlItemInput.CloseModal();";

        /// <summary>
        /// 空き(表示用)
        /// </summary>
        private const string FLAG_DISP = "";
        /// <summary>
        /// 利用ツールプルダウンタイプ
        /// </summary>
        public const string PULLDOWNTYPE_USINGTOOL = "3";
        /// <summary>
        /// 入力不可フラグ
        /// </summary>
        public const string INPUT_DISABLE = "1";
        /// <summary>
        /// 入力可能フラグ
        /// </summary>
        public const string INPUT_ENABLE = "2";
        /// <summary>
        /// 入力必須フラグ
        /// </summary>
        public const string INPUT_NOTNULL = "3";
        /// <summary>
        /// ボタン文言　登録
        /// </summary>
        public const string BUTTONTEXT_ADD = "登録";
        /// <summary>
        /// ボタン文言　更新
        /// </summary>
        public const string BUTTONTEXT_UPDATE = "更新";

        //処理区分
        enum ExecKbn : int {
            SELECT = 0,
            INSERT = 1,
            UPDATE = 2,
            DELETE = 3
        }

        #endregion

        #region 変数定義

        private static Dictionary<string, string> _orgData = new Dictionary<string, string>();
        private static Dictionary<string, string> _resultData = new Dictionary<string, string>();
        private int _execKbn = 1;

        #endregion

        #region プロパティ

        /// <summary>
        /// 表示中ページ(ベースフォーム)情報
        /// </summary>
        private BaseForm CurrentForm {
            get {
                return ( (BaseForm)Page );
            }
        }

        /// <summary>
        /// ユーザ情報
        /// </summary>
        private UserInfoSessionHandler.ST_USER _loginInfo;

        /// <summary>
        /// ユーザ情報
        /// </summary>
        public UserInfoSessionHandler.ST_USER LoginInfo {

            get {
                if ( true == ObjectUtils.IsNull( _loginInfo.UserInfo ) ) {
                    SessionManagerInstance sesMgr = CurrentForm.SessionManager;
                    _loginInfo = sesMgr.GetUserInfoHandler().GetUserInfo();
                }

                return _loginInfo;
            }
        }

        #endregion

        #region ページイベント

        protected void Page_Load( object sender, EventArgs e ) {
            base.RaiseEvent( DoPageLoad, false );
        }

        /// <summary>
        /// ページロード処理
        /// </summary>
        protected override void DoPageLoad( ) {

            //初期設定

            //閉じる
            ktbtnClose.Attributes[ControlUtils.ON_CLICK] = CLOSE_MODAL_DISP;
            //実行モード
            this._execKbn = NumericUtils.ToInt( Request.QueryString["exeKbn"] );
            if ( IsPostBack == false ) {
                InitializeSet();
            }
        }

        #endregion

        #region 初期処理

        /// <summary>
        /// 初期化処理
        /// </summary>
        public void InitializeSet( ) {
            //アクセスカウンター登録
            Dao.Com.AccessCounterDao.Entry( base.CurrentPageInfo.pageId );

            // 利用ツールリストの初期値に設定する
            ListItem[] ServiceUsingToolList = ControlUtils.GetListItems( AiImageCheckDao.SelectUsingToolList( PULLDOWNTYPE_USINGTOOL ) );
            _resultData.Clear();
            for ( int decLoop = 0; decLoop < ServiceUsingToolList.Length; decLoop++ ) {
                _resultData.Add( ServiceUsingToolList[decLoop].Value, ServiceUsingToolList[decLoop].Text.Split( '|' )[1] + "|" +  ServiceUsingToolList[decLoop].Text.Split( '|' )[2] + "|" + ServiceUsingToolList[decLoop].Text.Split( '|' )[3]);
                ServiceUsingToolList[decLoop].Text = ServiceUsingToolList[decLoop].Text.Split( '|' )[0];
            }

            if ( 0 < ServiceUsingToolList.Length ) {
                //検索結果がnullでない場合
                ddlUsingTool.Items.Clear();
                ddlUsingTool.Items.Add( FLAG_DISP );
                ddlUsingTool.Items.AddRange( ServiceUsingToolList );
            }
            _orgData.Clear();
            _orgData.Add( "exeKbn", Request.QueryString["exeKbn"] );
            _orgData.Add( "ddlLineCd", Request.QueryString["ddlLineCd"] );
            _orgData.Add( "ddlStation", Request.QueryString["ddlStation"] );
            lblLineNm.Text = ControlUtils.GetListItems( AiImageCheckDao.SelectLineListByStation( _orgData["ddlStation"].ToString() ), AiImageCheckDao.COLNAME_TEXT, AiImageCheckDao.COLNAME_VALUE )[0].Text;
            lblStationNm.Text = _orgData["ddlStation"].ToString() + ":" + MasterList.GetStationNm( _orgData["ddlStation"].ToString() );
            if ( this._execKbn == (int)ExecKbn.INSERT ) {
                //新規追加
                btnUpdate.Text = BUTTONTEXT_ADD;
                //パラメータ取得
            } else if ( this._execKbn == (int)ExecKbn.UPDATE ) {
                //更新
                btnUpdate.Text = BUTTONTEXT_UPDATE;
                //パラメータ取得
                _orgData.Add( "txtItemCode", Request.QueryString["txtItemCode"] );
                _orgData.Add( "txtItemNm", Request.QueryString["txtItemNm"] );
                _orgData.Add( "txtRevisionNo", Request.QueryString["txtRevisionNo"] );
                _orgData.Add( "txtLineNm", lblLineNm.Text );
                _orgData.Add( "txtStationNm", lblStationNm.Text );
                _orgData.Add( "txtUsingToolCd", Request.QueryString["txtUsingToolCd"] );
                _orgData.Add( "txtStream", Request.QueryString["txtStream"] );
                _orgData.Add( "txtUsingModel", Request.QueryString["txtUsingModel"] );
                _orgData.Add( "txtExpectResult", Request.QueryString["txtExpectResult"] );
                //画面の初期化
                lblItemCd.Text = _orgData["txtItemCode"].ToString();
                txtItemNm.Value = _orgData["txtItemNm"].ToString();
                ddlUsingTool.SelectedValue = _orgData["txtUsingToolCd"].ToString();
                txtStream.Value = _orgData["txtStream"].ToString();
                txtUsingModel.Value = _orgData["txtUsingModel"].ToString();
                txtExpectResult.Value = _orgData["txtExpectResult"].ToString();
                //入力項目無効化設定
                string[] selVal = _resultData[_orgData["txtUsingToolCd"].ToString()].ToString().Split('|');
                if ( selVal[0] == INPUT_DISABLE ) {
                    txtStream.Enabled = false;
                }
                if ( selVal[1] == INPUT_DISABLE ) {
                    txtUsingModel.Enabled = false;
                }
                if ( selVal[2] == INPUT_DISABLE ) {
                    txtExpectResult.Enabled = false;
                }
            }
        }

        #endregion

        #region ボタンイベント

        /// <summary>
        /// 更新ボタン押下時イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnUpdate_Click( object sender, EventArgs e ) {
            base.RaiseEvent( DoUpdate );
        }

        /// <summary>
        /// 更新処理
        /// </summary>
        private void DoUpdate( ) {
            //実行前チェック
            if ( false == CheckInputData() ) {
                return;
            }
            string strItemCd = string.Empty;

            //パラメータの設定
            KTBindParameters bindParam = new KTBindParameters();

            //詳細情報検索
            bindParam.Add( "ddlLineCd", _orgData["ddlLineCd"].ToString() );                 //ラインコード
            bindParam.Add( "ddlStation", _orgData["ddlStation"].ToString() );               //ステーション
            bindParam.Add( "txtItemNm", txtItemNm.Value );                                  //画像解析項目名
            bindParam.Add( "ddlUsingTool", ddlUsingTool.SelectedValue );                    //利用ツール
            bindParam.Add( "txtStream", txtStream.Value);                                   //ストリーム
            bindParam.Add( "txtUsingModel", txtUsingModel.Value );                          //利用モデル
            bindParam.Add( "txtExpectResult", txtExpectResult.Value );                      //期待結果
            bindParam.Add( "updateBy", LoginInfo.UserInfo.userId );                         //更新者
            bindParam.Add( "updatePg", PageInfo.AnlItemInput.pageId );                      //更新Pg
            if ( this._execKbn == (int)ExecKbn.INSERT ) {
                bindParam.Add( "txtRevisionNo", "0");                                       //改訂番号
                strItemCd = AiImageCheckDao.SelectNextItemCode();                           
                bindParam.Add( "txtItemCd", strItemCd );                                    //画像解析項目コード

            } else if ( this._execKbn == (int)ExecKbn.UPDATE ) {
                bindParam.Add( "txtRevisionNo", Int32.Parse( _orgData["txtRevisionNo"].ToString() ) +1 );          //改訂番号
                bindParam.Add( "txtItemCd", _orgData["txtItemCode"].ToString() );                                  //画像解析項目コード
            }

            int retCnt = 0;
            //更新
            retCnt = AiImageCheckDao.InsertAnlItemData( bindParam );
            if ( retCnt.Equals( 1 ) ) {
                //正常
                CurrentForm.WriteApplicationMessage( MsgManager.MESSAGE_INF_10010 );
                if ( this._execKbn == (int)ExecKbn.INSERT ) {
                    //登録した項目コードを表示させる
                    lblItemCd.Text = strItemCd;
                }
                    
            } else if ( retCnt.Equals( 0 ) ) {
                //データなし
                CurrentForm.WriteApplicationMessage( MsgManager.MESSAGE_ERR_72040 );
            } else {
                CurrentForm.WriteApplicationMessage( MsgManager.MESSAGE_ERR_80010 );
            }
            btnUpdate.Enabled = false;
        }

        #endregion

        #region 業務処理

        /// <summary>
        /// 利用ツールリスト選択動作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlUsingToolList_SelectedIndexChanged( object sender, EventArgs e ) {
            if ( ddlUsingTool.SelectedValue.Length == 0 ) {
                //利用ツール空白選択
                txtStream.Enabled = true;
                txtUsingModel.Enabled = true;
                txtExpectResult.Enabled = true;
            } else {
                string[] selVal = _resultData[ddlUsingTool.SelectedValue].ToString().Split( '|' );
                //ストリーム
                if ( selVal[0] == INPUT_DISABLE ) {
                    txtStream.Value = "";
                    txtStream.Enabled = false;
                } else {
                    txtStream.Enabled = true;
                }
                //利用モデル
                if ( selVal[1] == INPUT_DISABLE ) {
                    txtUsingModel.Value = "";
                    txtUsingModel.Enabled = false;
                } else {
                    txtUsingModel.Enabled = true;
                }
                //期待結果
                if ( selVal[2] == INPUT_DISABLE ) {
                    txtExpectResult.Value = "";
                    txtExpectResult.Enabled = false;
                } else {
                    txtExpectResult.Enabled = true;
                }
            }
            
        }

        /// <summary>
        /// 処理実行前データチェック
        /// </summary>
        /// <returns></returns>
        private bool CheckInputData( ) {

            string strItemNm = txtItemNm.Value;
            string strUsingTool = ddlUsingTool.SelectedValue;
            string strUsingModel = txtUsingModel.Value;
            string strSteam = txtStream.Value;
            string strExpectResult = txtExpectResult.Value;

            //入力チェック
            if ( true == StringUtils.IsBlank( strItemNm ) || true == StringUtils.IsEmpty( strItemNm ) ) {
                CurrentForm.WriteApplicationMessage( MsgManager.MESSAGE_WRN_62030, "画像解析項目名" );
                SetFocus( txtItemNm );
                return false;
            }

            //利用ツールの選択チェック
            if ( 0 == ddlUsingTool.Text.Length ) {
                CurrentForm.WriteApplicationMessage( MsgManager.MESSAGE_WRN_62020, "利用ツール" );
                SetFocus( ddlUsingTool );
                return false;
            }
            string[] selVal = _resultData[ddlUsingTool.SelectedValue].ToString().Split( '|' );
            //ストリーム
            if ( selVal[0] == INPUT_NOTNULL && true == StringUtils.IsEmpty( strSteam ) ) {
                CurrentForm.WriteApplicationMessage( MsgManager.MESSAGE_WRN_62030, "ストリーム" );
                SetFocus( txtStream );
                return false;
            }
            //利用モデル
            if ( selVal[1] == INPUT_NOTNULL && true == StringUtils.IsEmpty( strUsingModel ) ) {
                CurrentForm.WriteApplicationMessage( MsgManager.MESSAGE_WRN_62030, "利用モデル" );
                SetFocus( txtUsingModel );
                return false;
            }
            //期待結果
            if ( selVal[2] == INPUT_NOTNULL && true == StringUtils.IsEmpty( strExpectResult ) ) {
                CurrentForm.WriteApplicationMessage( MsgManager.MESSAGE_WRN_62030, "期待結果" );
                SetFocus( txtExpectResult );
                return false;
            }

            //ユーザ情報チェック
            if ( true == ObjectUtils.IsNull( LoginInfo.UserInfo )
                || true == StringUtils.IsEmpty( LoginInfo.UserInfo.userName ) ) {
                PageUtils.RedirectToErrorPage( Page, MsgManager.MESSAGE_ERR_72020, null );
                return false;
            }

            //更新権限チェック
            AppPermission.PERMISSION_INFO permMainteInfo = AppPermission.GetTransactionPermission( PageInfo.AnlItemInput, LoginInfo.UserInfo );
            if ( permMainteInfo.IsEdit == false ) {
                PageUtils.RedirectToErrorPage( Page, MsgManager.MESSAGE_ERR_72030, null );
                return false;
            }

            return true;
        }

        #endregion
    }
}