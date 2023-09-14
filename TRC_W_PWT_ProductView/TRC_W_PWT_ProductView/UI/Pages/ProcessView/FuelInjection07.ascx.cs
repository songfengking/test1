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

namespace TRC_W_PWT_ProductView.UI.Pages.ProcessView {

    /// <summary>
    /// (詳細 エンジン 工程) 噴射時期計測(07エンジン)
    /// </summary>
    public partial class FuelInjection07 : System.Web.UI.UserControl, Defines.Interface.IDetail {

        //ロガー定義
        private static readonly Logger logger = new Logger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

        #region 定数定義

        /// <summary>
        /// (メインリスト)コントロール定義
        /// </summary>
        public class GRID_MAIN {

            /// <summary>計測日時</summary>                    
            public static readonly ControlDefine INSPECTION_DT = new ControlDefine( "txtInspectionDt", "計測日時", "inspectionDt", ControlDefine.BindType.None, typeof( String ) );
            /// <summary>ステーション名</summary>              
            public static readonly ControlDefine STATION_NM = new ControlDefine( "txtStationNm", "ステーション", "stationNm", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>計測端末ホスト名</summary>            
            public static readonly ControlDefine MEASURE_TERMINAL = new ControlDefine( "txtMeasureTerminal", "計測端末ホスト名", "measureTerminal", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>検査担当コード</summary>              
            public static readonly ControlDefine TESTER_CD = new ControlDefine( "txtTesterCd", "検査担当コード", "testerCd", ControlDefine.BindType.Down, typeof( String ) );
            ///// <summary>検査担当名</summary>                  
            //public static readonly ControlDefine TESTER_NM = new ControlDefine( "txtTesterNm", "検査担当名", "testerNm", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>噴射時期実測値1</summary>             
            public static readonly ControlDefine INJECTION_TIMING_VAL_1 = new ControlDefine( "ntbInjectionTimingVal1", "噴射時期実測値1", "injectionTimingVal1", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>噴射時期判定1</summary>               
            public static readonly ControlDefine INJECTION_TIMING_RESULT_1 = new ControlDefine( "txtInjectionTimingResult1", "噴射時期判定1", "injectionTimingResult1", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>噴射時期実測値2</summary>             
            public static readonly ControlDefine INJECTION_TIMING_VAL_2 = new ControlDefine( "ntbInjectionTimingVal2", "噴射時期実測値2", "injectionTimingVal2", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>噴射時期判定2</summary>               
            public static readonly ControlDefine INJECTION_TIMING_RESULT_2 = new ControlDefine( "txtInjectionTimingResult2", "噴射時期判定2", "injectionTimingResult2", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>噴射時期実測値3</summary>             
            public static readonly ControlDefine INJECTION_TIMING_VAL_3 = new ControlDefine( "ntbInjectionTimingVal3", "噴射時期実測値3", "injectionTimingVal3", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>噴射時期判定3</summary>               
            public static readonly ControlDefine INJECTION_TIMING_RESULT_3 = new ControlDefine( "txtInjectionTimingResult3", "噴射時期判定3", "injectionTimingResult3", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>噴射時期実測値4</summary>             
            public static readonly ControlDefine INJECTION_TIMING_VAL_4 = new ControlDefine( "ntbInjectionTimingVal4", "噴射時期実測値4", "injectionTimingVal4", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>噴射時期判定4</summary>               
            public static readonly ControlDefine INJECTION_TIMING_RESULT_4 = new ControlDefine( "txtInjectionTimingResult4", "噴射時期判定4", "injectionTimingResult4", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>噴射時期実測値平均</summary>          
            public static readonly ControlDefine INJECTION_TIMING_AVE_VAL = new ControlDefine( "ntbInjectionTimingAveVal", "噴射時期実測値平均", "injectionTimingAveVal", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>噴射時期判定平均</summary>            
            public static readonly ControlDefine INJECTION_TIMING_AVE_RESULT = new ControlDefine( "txtInjectionTimingAveResult", "噴射時期判定平均", "injectionTimingAveResult", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>気温</summary>                        
            public static readonly ControlDefine TEMPERATURE = new ControlDefine( "ntbTemperature", "気温", "temperature", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>ピストン出代実測値1</summary>         
            public static readonly ControlDefine PISTON_BUMP_VAL_1 = new ControlDefine( "ntbPistonBumpVal1", "ピストン出代実測値1", "pistonBumpVal1", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>ピストン出代実測値2</summary>         
            public static readonly ControlDefine PISTON_BUMP_VAL_2 = new ControlDefine( "ntbPistonBumpVal2", "ピストン出代実測値2", "pistonBumpVal2", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>ピストン出代実測値3</summary>         
            public static readonly ControlDefine PISTON_BUMP_VAL_3 = new ControlDefine( "ntbPistonBumpVal3", "ピストン出代実測値3", "pistonBumpVal3", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>ピストン出代実測値4</summary>         
            public static readonly ControlDefine PISTON_BUMP_VAL_4 = new ControlDefine( "ntbPistonBumpVal4", "ピストン出代実測値4", "pistonBumpVal4", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>ピストン出代実測値平均</summary>      
            public static readonly ControlDefine PISTON_BUMP_AVE_VAL = new ControlDefine( "ntbPistonBumpAveVal", "ピストン出代実測値平均", "pistonBumpAveVal", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>ピストン出代ランク</summary>          
            public static readonly ControlDefine PISTON_BUMP_RANK = new ControlDefine( "txtPistonBumpRank", "ピストン出代ランク", "pistonBumpRank", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>噴射時期上限</summary>                
            public static readonly ControlDefine INJECTION_TIMING_UPPER_LIMIT = new ControlDefine( "ntbInjectionTimingUpperLimit", "噴射時期上限", "injectionTimingUpperLimit", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>噴射時期下限</summary>                
            public static readonly ControlDefine INJECTION_TIMING_LOWER_LIMIT = new ControlDefine( "ntbInjectionTimingLowerLimit", "噴射時期下限", "injectionTimingLowerLimit", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>ピストン出代上限</summary>            
            public static readonly ControlDefine PISTON_BUMP_UPPER_LIMIT = new ControlDefine( "ntbPistonBumpUpperLimit", "ピストン出代上限", "pistonBumpUpperLimit", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>ピストン出代ランク上</summary>        
            public static readonly ControlDefine PISTON_BUMP_RANK_UPPER = new ControlDefine( "ntbPistonBumpRankUpper", "ピストン出代ランク上", "pistonBumpRankUpper", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>ピストン出代ランク下</summary>        
            public static readonly ControlDefine PISTON_BUMP_RANK_LOWER = new ControlDefine( "ntbPistonBumpRankLower", "ピストン出代ランク下", "pistonBumpRankLower", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>ピストン出代下限</summary>            
            public static readonly ControlDefine PISTON_BUMP_LOWER_LIMIT = new ControlDefine( "ntbPistonBumpLowerLimit", "ピストン出代下限", "pistonBumpLowerLimit", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>パルスタイミング角度</summary>        
            public static readonly ControlDefine PULSE_TIMING_ANGLE = new ControlDefine( "ntbPulseTimingAngle", "パルスタイミング角度", "pulseTimingAngle", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>噴射ポンプ品番</summary>              
            public static readonly ControlDefine PUMP_NUM = new ControlDefine( "txtPumpNum", "噴射ポンプ品番", "pumpNum", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>噴射時期気筒間誤差角度</summary>      
            public static readonly ControlDefine INJECTION_TIMING_VARIANCE_ANGLE = new ControlDefine( "ntbInjectionTimingVarianceAngle", "噴射時期気筒間誤差角度", "injectionTimingVarianceAngle", ControlDefine.BindType.Down, typeof( Decimal ) );
            ///// <summary>伝送日付</summary>                    
            //public static readonly ControlDefine TRANSFER_YMD = new ControlDefine( "txtTransferYmd", "伝送日付", "transferYmd", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>伝送判定</summary>                    
            public static readonly ControlDefine TRANSFER_RESULT = new ControlDefine( "txtTransferResult", "伝送判定", "transferResult", ControlDefine.BindType.Down, typeof( String ) );
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
                return PageInfo.GetUCPageInfo( DetailKeyParam.ProductKind, DetailKeyParam.GroupCd, DetailKeyParam.ClassCd, DetailKeyParam.AssemblyPatternCd );
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
                resultSet = Business.DetailViewBusiness.SelectEngineFuelInjectionDetail( DetailKeyParam.AssemblyPatternCd, DetailKeyParam.ProductModelCd, DetailKeyParam.Serial );
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
                KTTextBox txtInspectionDt = ( (KTTextBox)e.Item.FindControl( GRID_MAIN.INSPECTION_DT.controlId ) );
                txtInspectionDt.Value = DateUtils.ToString( rowBind[GRID_MAIN.INSPECTION_DT.bindField], DateUtils.FormatType.Second );

                //ツールチップ設定
                ControlUtils.SetToolTip( e.Item );
            }
        }
        #endregion

    }
}