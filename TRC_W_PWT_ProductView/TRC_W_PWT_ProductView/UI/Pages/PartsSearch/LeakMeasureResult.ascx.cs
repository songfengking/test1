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

namespace TRC_W_PWT_ProductView.UI.Pages.PartsSearch {

    /// <summary>
    /// (詳細 エンジン 工程) 噴射時期計測(07エンジン)
    /// </summary>
    public partial class LeakMeasureResult : System.Web.UI.UserControl, Defines.Interface.IDetailParts {

        //ロガー定義
        private static readonly Logger logger = new Logger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

        #region 定数定義

        /// <summary>
        /// (メインリスト)コントロール定義
        /// </summary>
        public class GRID_MAIN {

            /// <summary>計測日時</summary>txtLeakPressure
            public static readonly ControlDefine MEASURE_DT = new ControlDefine( "txtMeasureDt", "計測日時", "measureDt", ControlDefine.BindType.None, typeof( String ) );
            /// <summary>ステーション</summary>              
            public static readonly ControlDefine STATION_CD = new ControlDefine( "txtStationCd", "ステーション", "stationNm", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>総合判定</summary>            
            public static readonly ControlDefine TOTAL_JUDGE = new ControlDefine( "txtTotalJudge", "総合判定", "totalJudge", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>履歴NO</summary>              
            public static readonly ControlDefine RECORD_NUMD = new ControlDefine( "txtRecordNum", "履歴NO", "recordNum", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>計測器</summary>              
            public static readonly ControlDefine MEASURE_TESTER = new ControlDefine( "txtMeasureTester", "計測器", "measureTester", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>リーク圧力実績</summary>             
            public static readonly ControlDefine LEAK_PRESSURE = new ControlDefine( "txtLeakPressure", "リーク圧力実績", "leakPressure", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>リーク圧力判定</summary>
            public static readonly ControlDefine LEAK_PRESSURE_JUDGE = new ControlDefine( "txtLeakPressureJudge", "リーク圧力判定", "leakPressureJudge", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>リーク流量実績</summary>             
            public static readonly ControlDefine LEAK_FLOWRATE = new ControlDefine( "txtLeakFlowRate", "リーク流量実績", "leakFlowRate", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>リーク流量判定</summary>               
            public static readonly ControlDefine LEAK_FLOW_JUDGE = new ControlDefine( "txtLeakFlowJudge", "リーク流量判定", "leakFlowJudge", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>差圧センサー判定時圧力</summary>             
            public static readonly ControlDefine DP_PRESSURE = new ControlDefine( "txtDpPressure", "差圧センサー判定時圧力", "dpPressure", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>差圧センサー(差圧なし)実績</summary>               
            public static readonly ControlDefine DP_SENSOR_NOTHING = new ControlDefine( "txtDpSensorNothing", "差圧センサー(差圧なし)実績", "dpSensorNothing", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>差圧センサー(差圧なし)判定</summary>             
            public static readonly ControlDefine DP_SENSOR_NOTHING_JUDGE = new ControlDefine( "txtDpSensorNothingJudge", "差圧センサー(差圧なし)判定", "dpSensorNothingJudge", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>差圧センサー(差圧あり)実績</summary>               
            public static readonly ControlDefine DP_SENSOR_EXISTS = new ControlDefine( "txtDpSensorExists", "差圧センサー(差圧あり)実績", "dpSensorExists", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>差圧センサー(差圧あり)判定</summary>          
            public static readonly ControlDefine IDP_SENSOR_EXISTS_JUDGE = new ControlDefine( "txtDpSensorExistsJudge", "差圧センサー(差圧あり)判定", "dpSensorExistsJudge", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>温度センサー室温</summary>            
            public static readonly ControlDefine TEMP_ROOM = new ControlDefine( "txtTempRoom", "温度センサー室温", "tempRoom", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>DOC入口実績</summary>                        
            public static readonly ControlDefine TEMP_SENSOR_DOC_IN = new ControlDefine( "txtTempSensorDocIn", "DOC入口実績", "tempSensorDocIn", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>DOC入口判定</summary>         
            public static readonly ControlDefine TEMP_SENSOR_DOC_IN_JUDGE = new ControlDefine( "txtTempSensorDocInJudge", "DOC入口判定", "tempSensorDocInJudge", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>DPF入口実績</summary>         
            public static readonly ControlDefine TEMP_SENSOR_DPF_IN = new ControlDefine( "txtTempSensorDpfIn", "DPF入口実績", "tempSensorDpfIn", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>DPF入口判定</summary>         
            public static readonly ControlDefine TEMP_SENSOR_DPF_IN_JUDGE = new ControlDefine( "txtTempSensorDpfInJudge", "DPF入口判定", "tempSensorDpfInJudge", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>DPF出口実績</summary>         
            public static readonly ControlDefine TEMP_SENSOR_DPF_OUT = new ControlDefine( "txtTempSensorDpfOut", "DPF出口実績", "tempSensorDpfOut", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>DPF出口判定</summary>      
            public static readonly ControlDefine TEMP_SENSOR_DPF_OUT_JUDGE = new ControlDefine( "txtTempSensorDpfOutJudge", "DPF出口判定", "tempSensorDpfOutJudge", ControlDefine.BindType.Down, typeof( String ) );
            
        }

        /// <summary>
        /// (サブリスト)コントロール定義
        /// </summary>
        public class GRID_SUB {
            //なし
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
                return PageInfo.GetUCPageInfo( DetailPartsKeyParam.SearchTarget, DetailPartsKeyParam.PartsKind, DetailPartsKeyParam.ProcessCd );
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
        private Defines.Interface.ST_DETAIL_PARTS_PARAM _detailPartsKeyParam;
        /// <summary>
        /// 詳細表示キー情報
        /// </summary>
        public Defines.Interface.ST_DETAIL_PARTS_PARAM DetailPartsKeyParam {
            get {
                return _detailPartsKeyParam;
            }
            set {
                _detailPartsKeyParam = value;
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
            Business.DetailPartsViewBusiness.ResultSet resultSet = new Business.DetailPartsViewBusiness.ResultSet();
            try {
                resultSet = Business.DetailPartsViewBusiness.SelectAtuLeakDetail( DetailPartsKeyParam.ModelCd, DetailPartsKeyParam.Serial );
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
        private void InitializeValues( Business.DetailPartsViewBusiness.ResultSet resultSet ) {
            //来歴情報セット
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
                //計測日時加工/セット
                KTTextBox txtInspectionDt = ( (KTTextBox)e.Item.FindControl( GRID_MAIN.MEASURE_DT.controlId ) );
                txtInspectionDt.Value = DateUtils.ToString( rowBind[GRID_MAIN.MEASURE_DT.bindField], DateUtils.FormatType.Second );

                //ツールチップ設定
                ControlUtils.SetToolTip( e.Item );
            }
        }
        #endregion

    }
}