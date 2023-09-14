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
    /// (詳細 エンジン 工程) エンジン運転測定(07エンジン)
    /// </summary>
    public partial class EngineTest07 : System.Web.UI.UserControl, Defines.Interface.IDetail {

        //ロガー定義
        private static readonly Logger logger = new Logger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

        #region 定数定義
        
        /// <summary>
        /// 管理ID
        /// </summary>
        const string MANAGE_ID = Defines.Session.DetailFrame.SESSION_PAGE_INFO_DETAIL_KEY;//CurrentUCInfo.pageId

        /// <summary>
        /// DB取得データ格納先 ディクショナリキー
        /// </summary>
        private string SESSION_PAGE_INFO_DB_KEY = "bindTableData";

        /// <summary>
        /// 選択行制御
        /// </summary>
        const string LIST_VIEW_SELECTED = "EngineTest07.SelectListViewRow(this,{0},'{1}');";
        const string SUB_LIST_VIEW_SELECTED = "EngineTest07.SelectSubListViewRow(this,{0},'{1}');";

        /// <summary>
        /// (メインリスト)コントロール定義
        /// </summary>
        public class GRID_MAIN {
            /// <summary>TR</summary>
            public static readonly ControlDefine TR_ROW_DATA = new ControlDefine( "trRowData", "TR", "", ControlDefine.BindType.None, typeof( String ) );
            /// <summary>(SELECTコマンド送信用ボタン)</summary>
            public static readonly ControlDefine SELECT = new ControlDefine( "btnSelect", "BUTTON", "", ControlDefine.BindType.None, typeof( String ) );
            /// <summary>測定日時</summary>   
            public static readonly ControlDefine INSPECTION_DT = new ControlDefine( "txtInspectionDt", "測定日時", "inspectionDt", ControlDefine.BindType.None, typeof( String ) );
            /// <suumary>ベンチNo</summary>
            public static readonly ControlDefine BENCHI_NO = new ControlDefine( "txtBenchiNo", "ベンチNo", "benchiNo", ControlDefine.BindType.Down, typeof( String ) );
            /// <suumary>結果</summary>
            public static readonly ControlDefine RESULT = new ControlDefine( "txtResult", "結果", "result", ControlDefine.BindType.Down, typeof( String ) );
            /// <suumary>作業者名</summary>
            public static readonly ControlDefine USER_NM = new ControlDefine( "txtUserNm", "作業者名", "userNm", ControlDefine.BindType.Down, typeof( String ) );
            /// <suumary>パレットNo</summary>
            public static readonly ControlDefine PALET_NO = new ControlDefine( "txtPaletNo", "パレットNo", "paletNo", ControlDefine.BindType.Down, typeof( String ) );
            /// <suumary>EGR有無</summary>
            public static readonly ControlDefine ERG_TYP = new ControlDefine( "txtErgTyp", "EGR有無", "ergTyp", ControlDefine.BindType.Down, typeof( String ) );
            /// <suumary>搬入日時</summary>
            public static readonly ControlDefine ENGINE_CARRY_DT = new ControlDefine( "txtEngineCarryDt", "搬入日時", "engineCarryDt", ControlDefine.BindType.None, typeof( String ) );
            /// <suumary>始動</summary>
            public static readonly ControlDefine ENGINE_START_TIME = new ControlDefine( "txtEngineStartTime", "始動", "engineStartTime", ControlDefine.BindType.Down, typeof( String ) );
            /// <suumary>調整開始</summary>
            public static readonly ControlDefine MEASURE_START_TIME = new ControlDefine( "txtMeasureStartTime", "調整開始", "measureStartTime", ControlDefine.BindType.Down, typeof( String ) );
            /// <suumary>調整終了</summary>
            public static readonly ControlDefine MEASURE_END_TIME = new ControlDefine( "txtMeasureEndTime", "調整終了", "measureEndTime", ControlDefine.BindType.Down, typeof( String ) );
            /// <suumary>排ガス区分</summary>
            public static readonly ControlDefine TIRE_TYP = new ControlDefine( "txtTireTyp", "排ガス区分", "tireTyp", ControlDefine.BindType.Down, typeof( String ) );
            /// <suumary>OEM区分</summary>
            public static readonly ControlDefine OEM_TYP = new ControlDefine( "txtOemTyp", "OEM区分", "oemTyp", ControlDefine.BindType.Down, typeof( String ) );
            /// <suumary>CRS区分</summary>
            public static readonly ControlDefine ENGINE_TYP = new ControlDefine( "txtEngineTyp", "CRS区分", "engineTyp", ControlDefine.BindType.Down, typeof( String ) );
            /// <suumary>定格点Qﾗﾝｸ</summary>
            public static readonly ControlDefine TEIKAKU_RTFLJT_VAL = new ControlDefine( "txtTeikakuRtfLjtVal", "定格点Qﾗﾝｸ", "teikakuRtfLjtVal", ControlDefine.BindType.Down, typeof( String ) );
            /// <suumary>ﾄﾙｸ点Qﾗﾝｸ</summary>
            public static readonly ControlDefine TORQUE_RTFLJT_VAL = new ControlDefine( "txtTorqueRtfLjtVal", "ﾄﾙｸ点Qﾗﾝｸ", "torqueRtfLjtVal", ControlDefine.BindType.Down, typeof( String ) );
            /// <suumary>ｲﾝﾀｰｸｰﾗ有無</summary>
            public static readonly ControlDefine INTERCOOLER_TYP = new ControlDefine( "txtInterCoolerTyp", "ｲﾝﾀｰｸｰﾗ有無", "interCoolerTyp", ControlDefine.BindType.Down, typeof( String ) );
            /// <suumary>ｱｸｾﾙ電圧区分</summary>
            public static readonly ControlDefine ACCEL_VOLTAGE_TYP = new ControlDefine( "txtAccelVoltageTyp", "ｱｸｾﾙ電圧区分", "accelVoltageTyp", ControlDefine.BindType.Down, typeof( String ) );
            /// <suumary>CAN通信速度</summary>
            public static readonly ControlDefine CAN_SPEED_KBN = new ControlDefine( "txtCanSpeedKbn", "CAN通信 通信速度", "canSpeedKbn", ControlDefine.BindType.Down, typeof( String ) );
            /// <suumary>ｱｸｾﾙ調整回数</summary>
            public static readonly ControlDefine ACCEL_MANUAL_COUNT = new ControlDefine( "txtAccelManualCount", "ｱｸｾﾙ調整回数", "accelManualCount", ControlDefine.BindType.Down, typeof( String ) );  
        }

        /// <summary>
        /// (サブリスト)コントロール定義
        /// </summary>
        public class GRID_SUB {
            /// <summary>TR</summary>
            public static readonly ControlDefine TR_ROW_DATA = new ControlDefine( "trRowData", "TR", "", ControlDefine.BindType.None, typeof( String ) );
            /// <summary>No</summary>                     
            public static readonly ControlDefine SEQ_NO = new ControlDefine( "txtSeqNo", "No", "seqNo", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>測定項目名</summary>             
            public static readonly ControlDefine MEASURE_NM = new ControlDefine( "txtMeasureNm", "測定項目名", "measureNm", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>測定時間</summary>                
            public static readonly ControlDefine MEASURE_TIME = new ControlDefine( "txtMeasureTime", "測定時間", "measureTime", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>回転数(rpm)</summary>             
            public static readonly ControlDefine POWER_RPM = new ControlDefine( "ntbPowerRpm", "回転数(rpm)", "powerRpm", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>トルク(N･m)</summary>             
            public static readonly ControlDefine POWER_NM = new ControlDefine( "ntbPowerNm", "トルク(N･m)", "powerNm", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>冷却水入口(℃)</summary>          
            public static readonly ControlDefine COOL_TEMP_IN = new ControlDefine( "ntbCoolTempIn", "冷却水入口(℃)", "coolTempIn", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>冷却水出口(℃)</summary>          
            public static readonly ControlDefine COOL_TEMP_OUT = new ControlDefine( "ntbCoolTempOut", "冷却水出口(℃)", "coolTempOut", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>潤滑油温度(℃)</summary>          
            public static readonly ControlDefine L_TEMP = new ControlDefine( "ntbLTemp", "潤滑油温度(℃)", "lTemp", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>潤滑油圧力(kPa)</summary>         
            public static readonly ControlDefine L_PRS = new ControlDefine( "ntbLPrs", "潤滑油圧力(kPa)", "lPrs", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>吸気温度(℃)</summary>            
            public static readonly ControlDefine IN_TEMP = new ControlDefine( "ntbInTemp", "吸気温度(℃)", "inTemp", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>吸気湿度(%)</summary>             
            public static readonly ControlDefine IN_HUM = new ControlDefine( "ntbInHum", "吸気湿度(%)", "inHum", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>燃料測定流量(ml/min)</summary>    
            public static readonly ControlDefine F_ML = new ControlDefine( "ntbFMl", "燃料測定流量(ml/min)", "fMl", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>燃料補正流量(ml/min)</summary>    
            public static readonly ControlDefine F_RML = new ControlDefine( "ntbFRml", "燃料補正流量(ml/min)", "fRml", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>燃料温度(℃)</summary>            
            public static readonly ControlDefine F_TEMP = new ControlDefine( "ntbFTemp", "燃料温度(℃)", "fTemp", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>燃料供給温度(℃)</summary>        
            public static readonly ControlDefine F_SUPPLY = new ControlDefine( "ntbFSupply", "燃料供給温度(℃)", "fSupply", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>燃料供給圧力(kPa)</summary>       
            public static readonly ControlDefine F_PRS = new ControlDefine( "ntbFPrs", "燃料供給圧力(kPa)", "fPrs", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>大気圧(kPa)</summary>             
            public static readonly ControlDefine PA_HG = new ControlDefine( "ntbPaHg", "大気圧(kPa)", "paHg", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>回転ﾊﾝﾁﾝｸﾞ(rpm)</summary>         
            public static readonly ControlDefine TRN_RPM = new ControlDefine( "ntbTrnRpm", "回転ﾊﾝﾁﾝｸﾞ(rpm)", "trnRpm", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>燃料噴射量(mm3/st)</summary>      
            public static readonly ControlDefine Q_ST = new ControlDefine( "ntbQSt", "燃料噴射量(mm3/st)", "qSt", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>出力(kW)</summary>                
            public static readonly ControlDefine PS_KW = new ControlDefine( "ntbPsKw", "出力(kW)", "psKw", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>修正出力(kW)</summary>            
            public static readonly ControlDefine SK_KW = new ControlDefine( "ntbSkKw", "修正出力(kW)", "skKw", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>修正トルク(N･m)</summary>         
            public static readonly ControlDefine TK_NM = new ControlDefine( "ntbTkNm", "修正トルク(N･m)", "tkNm", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>修正係数</summary>                
            public static readonly ControlDefine KEISUU = new ControlDefine( "ntbKeisuu", "修正係数", "keisuu", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>ブースト圧力(kPa)</summary>       
            public static readonly ControlDefine PIN_PRS = new ControlDefine( "ntbPinPrs", "ブースト圧力(kPa)", "pinPrs", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>EGR測定温度(℃)</summary>         
            public static readonly ControlDefine EGR_TEMP = new ControlDefine( "ntbEgrTemp", "EGR測定温度(℃)", "egrTemp", ControlDefine.BindType.Down, typeof( Decimal ) );
            /// <summary>ECU吸気温度(℃)</summary>         
            public static readonly ControlDefine ECU_IN_TEMP = new ControlDefine( "ntbEcuInTemp", "ECU吸気温度(℃)", "ecuInTemp", ControlDefine.BindType.Down, typeof( Decimal ) );
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
        protected void lstMainListRB_ItemDataBound( object sender, ListViewItemEventArgs e ) {
            ItemDataBoundMainRBList( sender, e );
        }
        /// <summary>
        /// メインリスト選択行変更中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lstMainListRB_SelectedIndexChanging( object sender, ListViewSelectEventArgs e ) {
            //処理なし
        }

        protected void lstMainListRB_SelectedIndexChanged( object sender, EventArgs e ) {
            CurrentForm.RaiseEvent( SelectedIndexChangedMainRBList );
        }

        /// <summary>
        /// メインリスト行バインド
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lstMainListLB_ItemDataBound( object sender, ListViewItemEventArgs e ) {
            ItemDataBoundMainLBList( sender, e );
        }
        /// <summary>
        /// メインリスト選択行変更中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lstMainListLB_SelectedIndexChanging( object sender, ListViewSelectEventArgs e ) {
            //処理なし
        }

        protected void lstMainListLB_SelectedIndexChanged( object sender, EventArgs e ) {
            CurrentForm.RaiseEvent( SelectedIndexChangedMainLBList );

        }
        /// <summary>
        /// サブリスト行バインド
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        protected void lstSubListRB_ItemDataBound( object sender, ListViewItemEventArgs e ) {
            ItemDataBoundSubListRB( sender, e );
        }
        /// <summary>
        /// サブリスト行バインド
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        protected void lstSubListLB_ItemDataBound( object sender, ListViewItemEventArgs e ) {
            ItemDataBoundSubListLB( sender, e );
        }

        #endregion

        #region イベントメソッド

        #region ページイベント

        /// <summary>
        /// ページロード処理
        /// </summary>
        private void DoPageLoad() {
        }

        /// <summary>
        /// 初期化処理
        /// </summary>
        public void Initialize() {
            //検索結果取得
            Business.DetailViewBusiness.ResultSetEngineTest res = new Business.DetailViewBusiness.ResultSetEngineTest();
            try {
                res = Business.DetailViewBusiness.SelectEngineTestDetail( DetailKeyParam.AssemblyPatternCd, DetailKeyParam.ModelCd, DetailKeyParam.ProductModelCd, DetailKeyParam.Serial );
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

            //取得データをセッションに格納
            Dictionary<string, object> dicPageControlInfo = new Dictionary<string, object>();
            dicPageControlInfo.Add( SESSION_PAGE_INFO_DB_KEY, res );
            CurrentForm.SessionManager.GetPageControlInfoHandler( CurrentForm.Token ).SetPageControlInfo( MANAGE_ID, dicPageControlInfo );

            if ( 0 == res.MainTable.Rows.Count ) {
                //検索結果0件
                CurrentForm.WriteApplicationMessage( MsgManager.MESSAGE_WRN_62010, CurrentUCInfo.title );
                return;
            }

            InitializeValues( res );
        }

        #endregion

        /// <summary>
        /// 初期値設定
        /// </summary>
        private void InitializeValues( Business.DetailViewBusiness.ResultSetEngineTest res ) {

            //メインリストバインド
            lstMainListLB.DataSource = res.MainTable;
            lstMainListLB.DataBind();
            lstMainListLB.SelectedIndex = 0;

            lstMainListRB.DataSource = res.MainTable;
            lstMainListRB.DataBind();
            lstMainListRB.SelectedIndex = 0;

            //サブリストバインド
            SelectedIndexChangedMainList( lstMainListLB.SelectedIndex );

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
                CurrentForm.SetControlValues( e.Item, MainControls, rowBind, ref dicSetValues );

                //検査日時加工/セット
                KTTextBox txtInspectionDt = ( (KTTextBox)e.Item.FindControl( GRID_MAIN.INSPECTION_DT.controlId ) );
                string inspectionDt = DateUtils.ToString( rowBind[GRID_MAIN.INSPECTION_DT.bindField], DateUtils.FormatType.Second );
                txtInspectionDt.Value = inspectionDt;

                //搬入日時加工/セット
                KTTextBox txtCarryDt = ( (KTTextBox)e.Item.FindControl( GRID_MAIN.ENGINE_CARRY_DT.controlId ) );
                string carryDt = DateUtils.ToString( rowBind[GRID_MAIN.ENGINE_CARRY_DT.bindField], DateUtils.FormatType.Second );
                txtCarryDt.Value = carryDt;
                
                //クリックイベントセット
                HtmlTableRow trRow = (HtmlTableRow)e.Item.FindControl( GRID_MAIN.TR_ROW_DATA.controlId );
                Button selectBtn = (Button)e.Item.FindControl( GRID_MAIN.SELECT.controlId );
                trRow.Attributes[ControlUtils.ON_CLICK] = string.Format( ControlUtils.LIST_VIEW_SELECTED, e.Item.DataItemIndex, selectBtn.UniqueID );

                //ツールチップ設定
                ControlUtils.SetToolTip( e.Item );

            }
        }

        /// <summary>
        /// サブリストバインド処理
        /// </summary>
        /// <param name="parameters">イベントパラメータ</param>
        private void ItemDataBoundSubListLB( params object[] parameters ) {
            object sender = parameters[0];
            ListViewItemEventArgs e = (ListViewItemEventArgs)parameters[1];

            if ( e.Item.ItemType == ListViewItemType.DataItem ) {
                DataRow rowBind = ( (DataRowView)e.Item.DataItem ).Row;
                Dictionary<string, object> dicSetValues = new Dictionary<string, object>();
                CurrentForm.SetControlValues( e.Item, SubControls, rowBind, ref dicSetValues );
                //クリックイベントセット
                HtmlTableRow trRow = (HtmlTableRow)e.Item.FindControl( GRID_SUB.TR_ROW_DATA.controlId );
                trRow.Attributes[ControlUtils.ON_CLICK] = string.Format( SUB_LIST_VIEW_SELECTED, e.Item.DataItemIndex, "" );

                //ツールチップ設定
                ControlUtils.SetToolTip( e.Item );
            }
        }
        /// <summary>
        /// サブリストバインド処理
        /// </summary>
        /// <param name="parameters">イベントパラメータ</param>
        private void ItemDataBoundSubListRB( params object[] parameters ) {
            object sender = parameters[0];
            ListViewItemEventArgs e = (ListViewItemEventArgs)parameters[1];

            if ( e.Item.ItemType == ListViewItemType.DataItem ) {
                DataRow rowBind = ( (DataRowView)e.Item.DataItem ).Row;
                Dictionary<string, object> dicSetValues = new Dictionary<string, object>();
                CurrentForm.SetControlValues( e.Item, SubControls, rowBind, ref dicSetValues );
                //クリックイベントセット
                HtmlTableRow trRow = (HtmlTableRow)e.Item.FindControl( GRID_SUB.TR_ROW_DATA.controlId );
                trRow.Attributes[ControlUtils.ON_CLICK] = string.Format( SUB_LIST_VIEW_SELECTED, e.Item.DataItemIndex, "" );

                //ツールチップ設定
                ControlUtils.SetToolTip( e.Item );
            }
        }

        /// <summary>
        /// メインリスト選択行変更処理
        /// </summary>
        /// <param name="parameters">イベントパラメータ</param>
        private void SelectedIndexChangedMainList( int paramIndex ) {

            int mainIndex = paramIndex;

            Business.DetailViewBusiness.ResultSetEngineTest res = new Business.DetailViewBusiness.ResultSetEngineTest();
            Dictionary<string, object> dicPageControlInfo = CurrentForm.SessionManager.GetPageControlInfoHandler( CurrentForm.Token ).GetPageControlInfo( MANAGE_ID );
            if ( true == dicPageControlInfo.ContainsKey( SESSION_PAGE_INFO_DB_KEY ) ) {
                res = (Business.DetailViewBusiness.ResultSetEngineTest)dicPageControlInfo[SESSION_PAGE_INFO_DB_KEY];
            }

            //サブリスト初期化
            lstSubListLB.DataSource = null;
            lstSubListLB.DataBind();

            lstSubListRB.DataSource = null;
            lstSubListRB.DataBind();

            solidSubLTHeader.Visible = true;
            solidSubRTHeader.Visible = true;

            //サブリストバインド
            if ( 0 < res.SubTables.Length
                && 0 < res.SubTables[mainIndex].Rows.Count ) {
                lstSubListLB.DataSource = res.SubTables[mainIndex];
                lstSubListLB.DataBind();
                lstSubListRB.DataSource = res.SubTables[mainIndex];
                lstSubListRB.DataBind();
            } else {
                solidSubLTHeader.Visible = false;
                solidSubRTHeader.Visible = false;
            }
        }


        private void ItemDataBoundMainRBList( params object[] parameters ) {
            object sender = parameters[0];
            ListViewItemEventArgs e = (ListViewItemEventArgs)parameters[1];

            if ( e.Item.ItemType == ListViewItemType.DataItem ) {
                DataRow rowBind = ( (DataRowView)e.Item.DataItem ).Row;
                Dictionary<string, object> dicSetValues = new Dictionary<string, object>();
                CurrentForm.SetControlValues( e.Item, MainControls, rowBind, ref dicSetValues );

                //搬入日時加工/セット
                KTTextBox txtCarryDt = ( (KTTextBox)e.Item.FindControl( GRID_MAIN.ENGINE_CARRY_DT.controlId ) );
                string carryDt = DateUtils.ToString( rowBind[GRID_MAIN.ENGINE_CARRY_DT.bindField], DateUtils.FormatType.Second );
                txtCarryDt.Value = carryDt;

                //クリックイベントセット
                HtmlTableRow trRow = (HtmlTableRow)e.Item.FindControl( GRID_MAIN.TR_ROW_DATA.controlId );
                Button selectBtn = (Button)e.Item.FindControl( GRID_MAIN.SELECT.controlId );
                trRow.Attributes[ControlUtils.ON_CLICK] = string.Format( LIST_VIEW_SELECTED, e.Item.DataItemIndex, selectBtn.UniqueID );

                //ツールチップ設定
                ControlUtils.SetToolTip( e.Item );

            }
        }

        private void SelectedIndexChangedMainRBList() {

            int mainIndex = lstMainListRB.SelectedIndex;

            //選択行背景色変更解除
            foreach ( ListViewDataItem li in lstMainListLB.Items ) {
                HtmlTableRow trDeselectRow = (HtmlTableRow)li.FindControl( GRID_MAIN.TR_ROW_DATA.controlId );
                trDeselectRow.Attributes["class"] = trDeselectRow.Attributes["class"].Replace( " " + ResourcePath.CSS.ListSelectedRow, "" );
            }

            foreach ( ListViewDataItem li in lstMainListRB.Items ) {
                HtmlTableRow trDeselectRow = (HtmlTableRow)li.FindControl( GRID_MAIN.TR_ROW_DATA.controlId );
                trDeselectRow.Attributes["class"] = trDeselectRow.Attributes["class"].Replace( " " + ResourcePath.CSS.ListSelectedRow, "" );
            }


            //一覧項目選択済に色変更
            HtmlTableRow trSelectRow = (HtmlTableRow)lstMainListLB.Items[mainIndex].FindControl( GRID_MAIN.TR_ROW_DATA.controlId );
            trSelectRow.Attributes["class"] = trSelectRow.Attributes["class"] + " " + ResourcePath.CSS.ListSelectedRow;

            HtmlTableRow trSelectRowRB = (HtmlTableRow)lstMainListRB.Items[mainIndex].FindControl( GRID_MAIN.TR_ROW_DATA.controlId );
            trSelectRowRB.Attributes["class"] = trSelectRow.Attributes["class"] + " " + ResourcePath.CSS.ListSelectedRow;

            SelectedIndexChangedMainList( mainIndex );
        }

        private void ItemDataBoundMainLBList( params object[] parameters ) {
            object sender = parameters[0];
            ListViewItemEventArgs e = (ListViewItemEventArgs)parameters[1];

            if ( e.Item.ItemType == ListViewItemType.DataItem ) {
                DataRow rowBind = ( (DataRowView)e.Item.DataItem ).Row;
                Dictionary<string, object> dicSetValues = new Dictionary<string, object>();
                CurrentForm.SetControlValues( e.Item, MainControls, rowBind, ref dicSetValues );

                //検査日時加工/セット
                KTTextBox txtInspectionDt = ( (KTTextBox)e.Item.FindControl( GRID_MAIN.INSPECTION_DT.controlId ) );
                string inspectionDt = DateUtils.ToString( rowBind[GRID_MAIN.INSPECTION_DT.bindField], DateUtils.FormatType.Second );
                txtInspectionDt.Value = inspectionDt;

                //クリックイベントセット
                HtmlTableRow trRow = (HtmlTableRow)e.Item.FindControl( GRID_MAIN.TR_ROW_DATA.controlId );
                Button selectBtn = (Button)e.Item.FindControl( GRID_MAIN.SELECT.controlId );
                trRow.Attributes[ControlUtils.ON_CLICK] = string.Format( LIST_VIEW_SELECTED, e.Item.DataItemIndex, selectBtn.UniqueID );

                //ツールチップ設定
                ControlUtils.SetToolTip( e.Item );

            }
        }

        private void SelectedIndexChangedMainLBList() {

            int mainIndex = lstMainListLB.SelectedIndex;

            //選択行背景色変更解除
            foreach ( ListViewDataItem li in lstMainListLB.Items ) {
                HtmlTableRow trDeselectRow = (HtmlTableRow)li.FindControl( GRID_MAIN.TR_ROW_DATA.controlId );
                trDeselectRow.Attributes["class"] = trDeselectRow.Attributes["class"].Replace( " " + ResourcePath.CSS.ListSelectedRow, "" );
            }

            foreach ( ListViewDataItem li in lstMainListRB.Items ) {
                HtmlTableRow trDeselectRow = (HtmlTableRow)li.FindControl( GRID_MAIN.TR_ROW_DATA.controlId );
                trDeselectRow.Attributes["class"] = trDeselectRow.Attributes["class"].Replace( " " + ResourcePath.CSS.ListSelectedRow, "" );
            }


            //一覧項目選択済に色変更
            HtmlTableRow trSelectRow = (HtmlTableRow)lstMainListLB.Items[mainIndex].FindControl( GRID_MAIN.TR_ROW_DATA.controlId );
            trSelectRow.Attributes["class"] = trSelectRow.Attributes["class"] + " " + ResourcePath.CSS.ListSelectedRow;

            HtmlTableRow trSelectRowRB = (HtmlTableRow)lstMainListRB.Items[mainIndex].FindControl( GRID_MAIN.TR_ROW_DATA.controlId );
            trSelectRowRB.Attributes["class"] = trSelectRow.Attributes["class"] + " " + ResourcePath.CSS.ListSelectedRow;

            SelectedIndexChangedMainList( mainIndex );

        }
        #endregion
    }
}