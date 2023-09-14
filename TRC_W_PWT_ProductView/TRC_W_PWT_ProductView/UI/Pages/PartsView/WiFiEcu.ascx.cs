using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using KTFramework.Common;
using KTFramework.Dao;
using KTWebControl.CustomControls;
using KTWebInheritance.Common;
using TRC_W_PWT_ProductView.Common;
using TRC_W_PWT_ProductView.Defines;
using TRC_W_PWT_ProductView.Defines.TypeDefine;
using TRC_W_PWT_ProductView.UI.Base;

namespace TRC_W_PWT_ProductView.UI.Pages.PartsView {
    /// <summary>
    /// (詳細 エンジン 部品) XXXXXXX
    /// </summary>
    public partial class WiFiEcu : System.Web.UI.UserControl, Defines.Interface.IDetail {

        //ロガー定義
        private static readonly Logger logger = new Logger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

        #region 定数定義
        /// <summary>
        /// 来歴情報
        /// </summary>
        public class GRID_MAIN {
            /// <summary>測定日時(yyyy/MM/dd hh:mm:ss)</summary>
            public static readonly ControlDefine INSPECTION_DT = new ControlDefine( "txtInspectionDt", "測定日時", "inspectionDt", ControlDefine.BindType.None, typeof( DateTime ) );
            /// <summary>ステータス</summary>
            public static readonly ControlDefine STATUS = new ControlDefine( "txtStatus", "ステータス", "statusStr", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>検査員名</summary>
            public static readonly ControlDefine EMPLOYEE_NM = new ControlDefine( "txtEmployeeNm", "検査員名", "employeeNm", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>本機ECU機番</summary>
            public static readonly ControlDefine ECU_SERIAL = new ControlDefine( "txtEcuSerial", "本機ECU機番", "ecuSerial", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>WiFi ECU品番</summary>
            public static readonly ControlDefine HARD_HINBAN = new ControlDefine( "txtHardNum", "WiFi ECU品番", "hardHinban", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>INI-W品番</summary>
            public static readonly ControlDefine SOFT_HINBAN = new ControlDefine( "txtSoftNum", "INI-W品番", "softHinban", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>WiFi ECU アッシ品番</summary>
            public static readonly ControlDefine HARD_ASSY_HINBAN = new ControlDefine( "txtHardAssyNum", "WiFi ECU アッシ品番", "hardAssyHinban", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>WiFi ECU ソフト品番(WiFi)</summary>
            public static readonly ControlDefine HARD_SOFT_HINBAN_WIFI = new ControlDefine( "txtHardSoftNumWifi", "WiFi ECU ソフト品番(WiFi)", "hardSoftHinbanWifi", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>WiFi ECU ソフト品番(KIND)</summary>
            public static readonly ControlDefine HARD_SOFT_HINBAN_KIND = new ControlDefine( "txtHardSoftNumKind", "WiFi ECU ソフト品番(KIND)", "hardSoftHinbanKind", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>WiFi IC ソフト品番(WiFi)</summary>
            public static readonly ControlDefine IC_SOFT_HINBAN_WIFI = new ControlDefine( "txtIcSoftNumWifi", "WiFi IC ソフト品番(WiFi)", "icSoftHinbanWifi", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>WiFi IC ソフト品番(KIND)</summary>
            public static readonly ControlDefine IC_SOFT_HINBAN_KIND = new ControlDefine( "txtIcSoftNumKind", "WiFi IC ソフト品番(KIND)", "icSoftHinbanKind", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>WiFi ECU機番</summary>
            public static readonly ControlDefine HARD_SERIAL = new ControlDefine( "txtHardSerial", "WiFi ECU機番", "hardSerial", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>書込アワーメータ値</summary>
            public static readonly ControlDefine HOURMETER_WRITE = new ControlDefine( "ntbHourMeterWrite", "書込アワーメータ値", "hourMeterWrite", ControlDefine.BindType.Down, typeof( int ) );
            /// <summary>アワーメータ判定</summary>
            public static readonly ControlDefine HOURMETER_JUD = new ControlDefine( "txtHourMeterJud", "アワーメータ判定", "hourMeterJud", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>開始アワーメータ値</summary>
            public static readonly ControlDefine HOURMETER_START = new ControlDefine( "ntbHourMeterStart", "開始アワーメータ値", "hourMeterStart", ControlDefine.BindType.Down, typeof( int ) );
            /// <summary>終了アワーメータ値</summary>
            public static readonly ControlDefine HOURMETER_END = new ControlDefine( "ntbHourMeterEnd", "終了アワーメータ値", "hourMeterEnd", ControlDefine.BindType.Down, typeof( int ) );
            /// <summary>アワーメータ誤差範囲</summary>
            public static readonly ControlDefine HOURMETER_CHECK = new ControlDefine( "ntbHourMeterCheck", "アワーメータ誤差範囲", "hourMeterCheck", ControlDefine.BindType.Down, typeof( int ) );
            /// <summary>本機ECUペアリング</summary>
            public static readonly ControlDefine ECU_PAIRING = new ControlDefine( "txtEcuPairing", "本機ECUペアリング", "ecuPairingStr", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>稼働収集</summary>
            public static readonly ControlDefine COLLECTION = new ControlDefine( "txtCollection", "稼働収集", "collectionStr", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>検査モード</summary>
            public static readonly ControlDefine CHK_MODE = new ControlDefine( "txtChkMode", "検査モード", "chkModeStr", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>IGN状態</summary>
            public static readonly ControlDefine IGN_STATE = new ControlDefine( "txtIgnState", "IGN状態", "ignStateStr", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>汎用出力</summary>
            public static readonly ControlDefine OUTPUT = new ControlDefine( "txtOutput", "汎用出力", "outputStr", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>EEPROM 製造工場設定エリア状態</summary>
            public static readonly ControlDefine EEPROM_MANUFACTURE_STATE = new ControlDefine( "txTeepromManufactureState", "EEPROM 製造工場設定エリア状態", "eepromManufactureStateStr", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>EEPROM システム設定エリア状態</summary>
            public static readonly ControlDefine EEPROM_SYSTEM_STATE = new ControlDefine( "txtEepromSystemState", "EEPROM システム設定エリア状態", "eepromSystemStateStr", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>EEPROM 工場設定エリア状態</summary>
            public static readonly ControlDefine EEPROM_KOJO_STATE = new ControlDefine( "txTeepromKojoState", "EEPROM 工場設定エリア状態", "eepromKojoStateStr", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>EEPROM 収集条件設定エリア状態</summary>
            public static readonly ControlDefine EEPROM_COLLECTION_STATE = new ControlDefine( "txtEepromCollectionState", "EEPROM 収集条件設定エリア状態", "eepromCollectionStateStr", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>EEPROM カルテデータエリア状態</summary>
            public static readonly ControlDefine EEPROM_KARTE_STATE = new ControlDefine( "txTeepromKarteState", "EEPROM カルテデータエリア状態", "eepromKarteStateStr", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>EEPROM その他エリア状態</summary>
            public static readonly ControlDefine EEPROM_ETC_STATE = new ControlDefine( "txtEepromEtcState", "EEPROM その他エリア状態", "eepromEtcStateStr", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>EEPROM 動作オプションエリア状態</summary>
            public static readonly ControlDefine EEPROM_OPTION_STATE = new ControlDefine( "txtEepromOptionState", "EEPROM 動作オプションエリア状態", "eepromOptionStateStr", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>EEPROM デフォルトパスフレーズ状態</summary>
            public static readonly ControlDefine EEPROM_DEFAULT_PASS_STATE = new ControlDefine( "txtEepromDefaultPassState", "EEPROM デフォルトパスフレーズ状態", "eepromDefaultPassStateStr", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>EEPROM ユーザパスフレーズ状態</summary>
            public static readonly ControlDefine EEPROM_USER_PASS_STATE = new ControlDefine( "txtEepromUserPassState", "EEPROM ユーザパスフレーズ状態", "eepromUserPassStateStr", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>高速収集条件エリア設定</summary>
            public static readonly ControlDefine HI_COLLECTION_STATE = new ControlDefine( "txtHiCollectionState", "高速収集条件エリア設定", "hiCollectionState", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>サーバ設定0エリア設定</summary>
            public static readonly ControlDefine SVR0_STATE = new ControlDefine( "txtSvr0State", "サーバ設定0エリア設定", "svr0State", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>サーバ設定1エリア設定</summary>
            public static readonly ControlDefine SVR1_STATE = new ControlDefine( "txtSvr1State", "サーバ設定1エリア設定", "svr1State", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>サーバ設定2エリア設定</summary>
            public static readonly ControlDefine SVR2_STATE = new ControlDefine( "txtSvr2State", "サーバ設定2エリア設定", "svr2State", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>サーバ設定3エリア設定</summary>
            public static readonly ControlDefine SVR3_STATE = new ControlDefine( "txtSvr3State", "サーバ設定3エリア設定", "svr3State", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>サーバ設定4エリア設定</summary>
            public static readonly ControlDefine SVR4_STATE = new ControlDefine( "txtSvr4State", "サーバ設定4エリア設定", "svr4State", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>サーバ設定5エリア設定</summary>
            public static readonly ControlDefine SVR5_STATE = new ControlDefine( "txtSvr5State", "サーバ設定5エリア設定", "svr5State", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>サーバ設定6エリア設定</summary>
            public static readonly ControlDefine SVR6_STATE = new ControlDefine( "txtSvr6State", "サーバ設定6エリア設定", "svr6State", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>サーバ設定7エリア設定</summary>
            public static readonly ControlDefine SVR7_STATE = new ControlDefine( "txtSvr7State", "サーバ設定7エリア設定", "svr7State", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>サーバ設定8エリア設定</summary>
            public static readonly ControlDefine SVR8_STATE = new ControlDefine( "txtSvr8State", "サーバ設定8エリア設定", "svr8State", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>サーバ設定9エリア設定</summary>
            public static readonly ControlDefine SVR9_STATE = new ControlDefine( "txtSvr9State", "サーバ設定9エリア設定", "svr9State", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>サーバ設定10エリア設定</summary>
            public static readonly ControlDefine SVR10_STATE = new ControlDefine( "txtSvr10State", "サーバ設定10エリア設定", "svr10State", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>サーバ設定11エリア設定</summary>
            public static readonly ControlDefine SVR11_STATE = new ControlDefine( "txtSvr11State", "サーバ設定11エリア設定", "svr11State", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>3G通信結果</summary>
            public static readonly ControlDefine THREEG_STATE = new ControlDefine( "txtThreeGState", "3G通信結果", "threeGState", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>IMEI</summary>
            public static readonly ControlDefine IMEI = new ControlDefine( "txtImei", "IMEI", "imei", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>ICCID</summary>
            public static readonly ControlDefine ICCID = new ControlDefine( "txtIccid", "ICCID", "iccid", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>INI-XML品番</summary>
            public static readonly ControlDefine INI_XML_HINBAN = new ControlDefine( "txtIniXmlHinban", "INI-XML品番", "iniXmlHinban", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>認証ID</summary>
            public static readonly ControlDefine AUTHENTICATION_ID = new ControlDefine( "txtAuthenticationId", "認証ID", "authenticationId", ControlDefine.BindType.Down, typeof( string ) );
        }

        /// <summary>
        /// 詳細情報
        /// </summary>
        public class GRID_SUB {
            //詳細なし
        }
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
        /// 表示中ユーザコントロール情報
        /// </summary>
        private PageInfo.ST_PAGE_INFO CurrentUCInfo {
            get {
                return PageInfo.GetUCPageInfo( DetailKeyParam.ProductKind, DetailKeyParam.GroupCd, DetailKeyParam.ClassCd );
            }
        }

        /// <summary>
        /// (メイン)コントロール定義
        /// </summary>
        ControlDefine[] _mainControls = null;
        /// <summary>
        /// (メイン)コントロール定義アクセサ
        /// </summary>
        ControlDefine[] MainControls {
            get {
                if ( true == ObjectUtils.IsNull( _mainControls ) ) {
                    _mainControls = ControlUtils.GetControlDefineArray( typeof( GRID_MAIN ) );
                }
                return _mainControls;
            }
        }

        /// <summary>
        /// (サブ)コントロール定義
        /// </summary>
        ControlDefine[] _subControls = null;
        /// <summary>
        /// (サブ)コントロール定義アクセサ
        /// </summary>
        ControlDefine[] SubControls {
            get {
                if ( true == ObjectUtils.IsNull( _subControls ) ) {
                    _subControls = ControlUtils.GetControlDefineArray( typeof( GRID_SUB ) );
                }
                return _subControls;
            }
        }

        /// <summary>
        /// 詳細表示キー情報
        /// </summary>
        private Defines.Interface.ST_DETAIL_PARAM _detailKeyParam;
        /// <summary>
        /// 詳細表示キー情報
        /// </summary>
        public Defines.Interface.ST_DETAIL_PARAM DetailKeyParam {
            get {
                return _detailKeyParam;
            }
            set {
                _detailKeyParam = value;
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
            CurrentForm.RaiseEvent( DoPageLoad );
        }

        /// <summary>
        /// メインリスト行バインド
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lstMainList_ItemDataBound( object sender, ListViewItemEventArgs e ) {
            ItemDataBoundMainList( sender, e );
        }

        #endregion

        #region イベントメソッド

        #region ページイベント
        /// <summary>
        /// 初期化処理
        /// </summary>
        public void Initialize() {
            //検索結果取得
            Business.DetailViewBusiness.ResultSet resultSet = new Business.DetailViewBusiness.ResultSet();
            try {
                resultSet = Business.DetailViewBusiness.SelectTractorWifiEcuDetail( DetailKeyParam.ProductModelCd, DetailKeyParam.Serial );
            } catch ( DataAccessException ex ) {
                logger.Exception( ex );
                CurrentForm.WriteApplicationMessage( MsgManager.MESSAGE_ERR_80010 );
                return;
            } catch ( Exception ex ) {
                logger.Exception( ex );
                CurrentForm.WriteApplicationMessage( MsgManager.MESSAGE_ERR_80010 );
                return;
            } finally {
            }

            if ( 0 == resultSet.MainTable.Rows.Count ) {
                //検索結果0件
                CurrentForm.WriteApplicationMessage( MsgManager.MESSAGE_WRN_62010, CurrentUCInfo.title );
                return;
            }

            InitializeValues( resultSet );
        }

        /// <summary>
        /// ページロード処理
        /// </summary>
        private void DoPageLoad() {
        }

        #endregion

        /// <summary>
        /// 初期値設定
        /// </summary>
        private void InitializeValues( Business.DetailViewBusiness.ResultSet resultSet ) {
            //来歴情報セット
            //サブリストバインド(ListViewのBindingイベント内でデータをセット)
            lstMainList.DataSource = resultSet.MainTable;
            lstMainList.DataBind();

            lstMainList.SelectedIndex = 0;

        }
        #endregion

        #region リストバインド
        /// <summary>
        /// メインリストバインド処理
        /// </summary>
        /// <param name="parameters">イベントパラメータ</param>
        private void ItemDataBoundMainList( params object[] parameters ) {
            object sender = parameters[0];
            ListViewItemEventArgs e = (ListViewItemEventArgs)parameters[1];

            if ( e.Item.ItemType == ListViewItemType.DataItem ) {
                DataRow rowBind = ( (DataRowView)e.Item.DataItem ).Row;
                Dictionary<string, object> dicSetValues = new Dictionary<string, object>();
                //コントロールへの自動データバインド
                CurrentForm.SetControlValues( e.Item, MainControls, rowBind, ref dicSetValues );

                //コントロールへの独自データバインド
                //組付日時加工/セット
                KTTextBox txtInspectionDt = ( (KTTextBox)e.Item.FindControl( GRID_MAIN.INSPECTION_DT.controlId ) );
                txtInspectionDt.Value = DateUtils.ToString( rowBind[GRID_MAIN.INSPECTION_DT.bindField], DateUtils.FormatType.Second );

                //WiFiECU/DCU表示切替
                e.Item.FindControl( "pnlWfecu" ).Visible = rowBind["WdKbn"]?.ToString().Equals( "W" ) ?? false;
                e.Item.FindControl( "pnlDcu" ).Visible = rowBind["WdKbn"]?.ToString().Equals( "D" ) ?? false;

                //ツールチップ設定
                ControlUtils.SetToolTip( e.Item );
            }
        }
        #endregion
    }
}