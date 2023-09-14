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
    /// (詳細 ATU 工程) ATU機番管理
    /// </summary>
    public partial class AtuPartsSerial : System.Web.UI.UserControl, Defines.Interface.IDetailParts {

        //ロガー定義
        private static readonly Logger logger = new Logger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

        #region 定数定義

        /// <summary>
        /// (メインリスト)コントロール定義
        /// </summary>
        public class GRID_MAIN {

            /// <summary>読込日時</summary>                    
            public static readonly ControlDefine READ_DT = new ControlDefine( "txtReadDt", "読込日時", "readDt", ControlDefine.BindType.None, typeof( String ) );
            /// <summary>ステーション名</summary>
            public static readonly ControlDefine STATION_CD = new ControlDefine( "txtStationCd", "ステーション", "stationNm", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>アッシ品番</summary>
            public static readonly ControlDefine ASSY_NUM = new ControlDefine( "txtAssyPartsNum", "アッシ品番", "assyPartsNum", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>アッシ機番</summary>
            public static readonly ControlDefine ASSY_SERIAL = new ControlDefine( "txtAssySerial", "アッシ機番", "assySerial", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>構成品品番1</summary>
            public static readonly ControlDefine COMPONENT_PARTS_NUM1 = new ControlDefine( "txtComponentPartsNum1", "構成品品番1", "componentPartsNum1", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>構成品機番1</summary>
            public static readonly ControlDefine COMPONENT_PARTS_SERIAL1 = new ControlDefine( "txtComponentSerial1", "構成品機番1", "componentSerial1", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>構成品品番2</summary>
            public static readonly ControlDefine COMPONENT_PARTS_NUM2 = new ControlDefine( "txtComponentPartsNum2", "構成品品番2", "componentPartsNum2", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>構成品機番2</summary>
            public static readonly ControlDefine COMPONENT_PARTS_SERIAL2 = new ControlDefine( "txtComponentSerial2", "構成品機番2", "componentSerial2", ControlDefine.BindType.Down, typeof( String ) );


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
                resultSet = Business.DetailPartsViewBusiness.SelectAtuSerialDetail( DetailPartsKeyParam.ModelCd, DetailPartsKeyParam.Serial );
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
                KTTextBox txtInspectionDt = ( (KTTextBox)e.Item.FindControl( GRID_MAIN.READ_DT.controlId ) );
                txtInspectionDt.Value = DateUtils.ToString( rowBind[GRID_MAIN.READ_DT.bindField], DateUtils.FormatType.Second );

                //ツールチップ設定
                ControlUtils.SetToolTip( e.Item );
            }
        }
        #endregion

    }
}