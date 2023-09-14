using System;
using System.Collections.Generic;
using KTFramework.Common;
using TRC_W_PWT_ProductView.Common;
using TRC_W_PWT_ProductView.UI.Base;
using TRC_W_PWT_ProductView.Session;
using TRC_W_PWT_ProductView.Defines;
using TRC_W_PWT_ProductView.Defines.ListDefine;
using TRC_W_PWT_ProductView.SrvCore;

namespace TRC_W_PWT_ProductView.UI.Pages {
    ////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// ユーザ実行JOB起動要求画面
    /// </summary>
    ////////////////////////////////////////////////////////////////////////////
    public partial class BatchExecCmdInput : BaseForm {

        //ロガー定義
        private static readonly Logger logger = new Logger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

        #region 定数定義

        #region スクリプトイベント
        /// <summary>
        /// CheckBoxクリックイベント
        /// </summary>
        /// <remarks></remarks>
        const string CHANGE_CHECK_BOX = "BatchExecCmdInput.CheckChanged();";

        #endregion
        
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

        #region イベント
        /// <summary>
        /// ページロード
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load( object sender, EventArgs e ) {
            base.RaiseEvent( DoPageLoad, false );
        }

        #endregion

        #region ページイベント

        /// <summary>
        /// ページロード処理
        /// </summary>
        protected override void DoPageLoad() {

            //ベース ページロード処理
            base.DoPageLoad();

            //日付項目制御
            //生産中
            if ( chkProduct.Checked ) {
                cldProductFrom.Enabled = true;
                cldProductTo.Enabled = true;
            }else{
                cldProductFrom.Enabled = false;
                cldProductTo.Enabled = false;
            }
            //在庫
            if ( chkStock.Checked ) {
                cldStockFrom.Enabled = true;
                cldStockTo.Enabled = true;
            }else{
                cldStockFrom.Enabled = false;
                cldStockTo.Enabled = false;
            }
            //出荷
            if ( chkShipment.Checked) {
                cldShipmentFrom.Enabled = true;
                cldShipmentTo.Enabled = true;
            }else{
                cldShipmentFrom.Enabled = false;
                cldShipmentTo.Enabled = false;
            }

        }

        #endregion

        #region 画面初期化処理
        /// <summary>
        /// 初期化処理
        /// </summary>
        protected override void Initialize() {

            //アクセスカウンター登録
            Dao.Com.AccessCounterDao.Entry( base.CurrentPageInfo.pageId );

            //ベース処理初期化処理
            base.Initialize();

            //初期化、初期値設定
            InitializeValues();

            chkProduct.Attributes[ControlUtils.ON_CLICK] = CHANGE_CHECK_BOX;
            chkStock.Attributes[ControlUtils.ON_CLICK] = CHANGE_CHECK_BOX;
            chkShipment.Attributes[ControlUtils.ON_CLICK] = CHANGE_CHECK_BOX;
            //ﾁｪｯｸBOX
            chkProduct.Checked = true;
            chkStock.Checked = true;
            chkShipment.Checked = true;
        }

        /// <summary>
        /// 初期化、初期値設定
        /// </summary>
        private void InitializeValues() {

            //起動画面の確認
            string execProcess = StringUtils.Nvl( base.GetTransInfoValue( "ExecKbn" ) );

            //起動画面に応じて、画面コントロールの制御を行う

            ////■固定リスト項目
            ////製品種別リスト
            //ControlUtils.SetListControlItems( rblProductKind, Dao.Com.MasterList.ProductKindList );

            ////■初期値設定
            ////製品種別 10:エンジン
            //rblProductKind.SelectedValue = ProductKind.Engine;

            //日付(開始、終了 初期値)
            //当月月初
            cldProductFrom.Value = new DateTime( DateTime.Today.Year, DateTime.Today.Month, 1 );
            //当月月末
            cldProductTo.Value = new DateTime( DateTime.Today.Year, DateTime.Today.Month, DateTime.DaysInMonth( DateTime.Today.Year, DateTime.Today.Month ) );

            //当月月初
            cldStockFrom.Value = new DateTime( DateTime.Today.Year, DateTime.Today.Month, 1 );
            //当月月末
            cldStockTo.Value = new DateTime( DateTime.Today.Year, DateTime.Today.Month, DateTime.DaysInMonth( DateTime.Today.Year, DateTime.Today.Month ) );

            //当月月初
            cldShipmentFrom.Value = new DateTime( DateTime.Today.Year, DateTime.Today.Month, 1 );
            //当月月末
            cldShipmentTo.Value = new DateTime( DateTime.Today.Year, DateTime.Today.Month, DateTime.DaysInMonth( DateTime.Today.Year, DateTime.Today.Month ) );

            //権限による制御
            AppPermission.PERMISSION_INFO permAuthInfo = AppPermission.GetTransactionPermission( TRC_W_PWT_ProductView.Defines.PageInfo.BatchPartsTrace, LoginInfo.UserInfo );
            if ( permAuthInfo.IsView == true ) {
                btnDLPartsTrace.Enabled = true;
            } else {
                btnDLPartsTrace.Enabled = false;
            }
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDLPartsTrace_Click( object sender, EventArgs e ) {
            base.RaiseEvent( DL_PartsTrace );
        }

        /// <summary>
        /// 検索条件チェック処理
        /// </summary>
        private bool CheckCondition() {

            bool blRet = false;

            //品番チェック
            if ( StringUtils.IsBlank( KTtxtParts.Text ) ) {
                //未入力
                base.WriteApplicationMessage( MsgManager.MESSAGE_WRN_62030,"品番" );
                return blRet;
            } else if ( KTtxtParts.Text.Length < 9 || 10 < KTtxtParts.Text.Length  ) {
                //桁数過不足
                base.WriteApplicationMessage( MsgManager.MESSAGE_WRN_62070, "品番","9桁または10桁の品番" );
                return blRet;
            }

            //出力対象未選択
            if  (( ( chkProduct.Checked ) || ( chkStock.Checked) || ( chkShipment.Checked ) ) == false) {
                base.WriteApplicationMessage( MsgManager.MESSAGE_WRN_62030, "出力区分" );
                return blRet;         
            }

            return true;

        }
        /// <summary>
        /// ユーザ実行JOB起動要求（製品トレース）
        /// </summary>
        private void DL_PartsTrace() {
           
            //チェック処理
            if ( false == CheckCondition() ) {
                return;
            }

            /********************************************************************************************************/
            //ユーザ実行JOB起動要求()

            //起動パラメータ設定(指示書種別)
            List<CoreService.JobParameter> jobParam = new List<CoreService.JobParameter>();

            //各種パラメータ設定
            jobParam.Add( new CoreService.JobParameter( TrcWebConsts.JOB_PARAM_KEY_PARTS_NUM, StringUtils.ToString( KTtxtParts.Text ) ) );              //品番


            //製造中(仕掛)にチェックあり
            if ( chkProduct.Checked ) {
                jobParam.Add( new CoreService.JobParameter( TrcWebConsts.JOB_PARAM_KEY_PRODUCT, TrcWebConsts.JOB_PARAM_VALUE_TARGET_ON ) );             //検索対象(仕掛)
                //検索対象期間(仕掛From)
                if ( ObjectUtils.IsNotNull( cldProductFrom.Value ) ) {
                    jobParam.Add( new CoreService.JobParameter( TrcWebConsts.JOB_PARAM_KEY_PRODUCT_FROM, DateUtils.ToString( cldProductFrom.Value, DateUtils.DATE_FORMAT_DAY ) ) );
                } else {
                    jobParam.Add( new CoreService.JobParameter( TrcWebConsts.JOB_PARAM_KEY_PRODUCT_FROM, "" ));                
                }

                //検索対象期間(仕掛To)
                if ( ObjectUtils.IsNotNull( cldProductTo.Value ) ) {
                    jobParam.Add( new CoreService.JobParameter( TrcWebConsts.JOB_PARAM_KEY_PRODUCT_TO, DateUtils.ToString( cldProductTo.Value, DateUtils.DATE_FORMAT_DAY ) ) );
                } else {
                    jobParam.Add( new CoreService.JobParameter( TrcWebConsts.JOB_PARAM_KEY_PRODUCT_TO, "" ) );
                }

            } else {
                jobParam.Add( new CoreService.JobParameter( TrcWebConsts.JOB_PARAM_KEY_PRODUCT, TrcWebConsts.JOB_PARAM_VALUE_TARGET_OFF ) );            //検索対象(仕掛)
                jobParam.Add( new CoreService.JobParameter( TrcWebConsts.JOB_PARAM_KEY_PRODUCT_FROM, "" ) );
                jobParam.Add( new CoreService.JobParameter( TrcWebConsts.JOB_PARAM_KEY_PRODUCT_TO, "" ) );
            }

            //在庫にチェックあり
            if ( chkStock.Checked ) {

                //完成品未出荷
                jobParam.Add( new CoreService.JobParameter( TrcWebConsts.JOB_PARAM_KEY_STOCK, TrcWebConsts.JOB_PARAM_VALUE_TARGET_ON ) );               //検索対象(在庫)
                if ( ObjectUtils.IsNotNull( cldStockFrom.Value ) ) {
                    jobParam.Add( new CoreService.JobParameter( TrcWebConsts.JOB_PARAM_KEY_STOCK_FROM, DateUtils.ToString( cldStockFrom.Value, DateUtils.DATE_FORMAT_DAY ) ) );      //検索対象期間(在庫From)
                } else {
                    jobParam.Add( new CoreService.JobParameter( TrcWebConsts.JOB_PARAM_KEY_STOCK_FROM, "" ) );      //検索対象期間(在庫From)                
                }
                if ( ObjectUtils.IsNotNull( cldStockTo.Value ) ) {
                    jobParam.Add( new CoreService.JobParameter( TrcWebConsts.JOB_PARAM_KEY_STOCK_TO, DateUtils.ToString( cldStockTo.Value, DateUtils.DATE_FORMAT_DAY ) ) );          //検索対象期間(在庫To)
                } else {
                    jobParam.Add( new CoreService.JobParameter( TrcWebConsts.JOB_PARAM_KEY_STOCK_TO, "" ) );          //検索対象期間(在庫To)               
                }

            }else{
                jobParam.Add( new CoreService.JobParameter( TrcWebConsts.JOB_PARAM_KEY_STOCK, TrcWebConsts.JOB_PARAM_VALUE_TARGET_OFF ) );              //検索対象(在庫)
                jobParam.Add( new CoreService.JobParameter( TrcWebConsts.JOB_PARAM_KEY_STOCK_FROM, "" ) );      //検索対象期間(在庫From)
                jobParam.Add( new CoreService.JobParameter( TrcWebConsts.JOB_PARAM_KEY_STOCK_TO, "" ) );          //検索対象期間(在庫To)               
            }

            //出荷にチェックあり
            if ( chkShipment.Checked ) {
                jobParam.Add( new CoreService.JobParameter( TrcWebConsts.JOB_PARAM_KEY_SHIPMENT, TrcWebConsts.JOB_PARAM_VALUE_TARGET_ON ) );            //検索対象(出荷)
                if ( ObjectUtils.IsNotNull( cldShipmentFrom.Value ) ) {
                    jobParam.Add( new CoreService.JobParameter( TrcWebConsts.JOB_PARAM_KEY_SHIPMENT_FROM, DateUtils.ToString( cldShipmentFrom.Value, DateUtils.DATE_FORMAT_DAY ) ) );            //検索対象期間(出荷From)
                } else {
                    jobParam.Add( new CoreService.JobParameter( TrcWebConsts.JOB_PARAM_KEY_SHIPMENT_FROM, "" ) );            //検索対象期間(出荷From)                
                }
                if ( ObjectUtils.IsNotNull( cldShipmentTo.Value ) ) {
                    jobParam.Add( new CoreService.JobParameter( TrcWebConsts.JOB_PARAM_KEY_SHIPMENT_TO, DateUtils.ToString( cldShipmentTo.Value, DateUtils.DATE_FORMAT_DAY ) ) );    //検索対象期間(出荷To)
                } else {
                    jobParam.Add( new CoreService.JobParameter( TrcWebConsts.JOB_PARAM_KEY_SHIPMENT_TO, "" ) );    //検索対象期間(出荷To)                
                }

            }else{
                jobParam.Add( new CoreService.JobParameter( TrcWebConsts.JOB_PARAM_KEY_SHIPMENT, TrcWebConsts.JOB_PARAM_VALUE_TARGET_OFF ) );           //検索対象(出荷)
                jobParam.Add( new CoreService.JobParameter( TrcWebConsts.JOB_PARAM_KEY_SHIPMENT_FROM, "" ) );            //検索対象期間(出荷From)                
                jobParam.Add( new CoreService.JobParameter( TrcWebConsts.JOB_PARAM_KEY_SHIPMENT_TO, "" ) );    //検索対象期間(出荷To)                
            }

            CoreService CoreService = new CoreService();

            //ユーザ実行JOB起動要求送信
            C1010RequestDto resultDto = CoreService.RequestBatchExec2(
                TrcWebConsts.JOB_PARTS_TRACE,                           //実行タスクID
                SessionManager.GetUserInfoHandler().GetUserInfo().UserInfo.userId,                              //ログインユーザID
                SessionManager.GetUserInfoHandler().GetUserInfo().UserInfo.userName,                            //ユーザ名
                SessionManager.GetUserInfoHandler().GetUserInfo().UserInfo.mailAddress,                         //メールアドレス
                SessionManager.GetUserInfoHandler().GetUserInfo().UserInfo.ipAddress,                           //端末IPアドレス
                TrcWebConsts.JOB_PARAM_KEY_JOB_NM_PARTS_TRACE,          //JOBネット名
                jobParam );

            //要求結果
            if ( resultDto.resultCd.IndexOf( "INF" ) >= 0 ) {
                //正常終了
                base.WriteApplicationMessage( MsgManager.MESSAGE_INF_10040, resultDto.resultMessage );
            } else {
                //それ以外
                base.WriteApplicationMessage( MsgManager.MESSAGE_ERR_72070, resultDto.resultMessage );
            }
            
        
        }

        protected void rblProductKind_SelectedIndexChanged( object sender, EventArgs e ) {
            base.RaiseEvent( ChangeProductKind );
        }
        /// <summary>
        /// 製品種別変更
        /// </summary>
        private void ChangeProductKind() {

            //ロータリの場合は生産中選択不可
            if ( rblProductKind.SelectedValue.Equals( ProductKind.Rotary ) ) {
                chkProduct.Enabled = false;
                chkProduct.Checked = false;
                cldProductFrom.Enabled = false;
                cldProductTo.Enabled = false;
            } else {
                chkProduct.Enabled = true;
            }
        }
    }
}