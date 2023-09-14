using KTFramework.Common;
using KTFramework.Dao;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TRC_W_PWT_ProductView.Common;
using TRC_W_PWT_ProductView.Defines;
using TRC_W_PWT_ProductView.Defines.TypeDefine;
using TRC_W_PWT_ProductView.UI.Base;

namespace TRC_W_PWT_ProductView.UI.Pages.PartsView {
    public partial class Mission : System.Web.UI.UserControl, Defines.Interface.IDetail {
        //ロガー定義
        private static readonly Logger logger = new Logger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

        #region 定数定義
        ///<summary>区切り位置：品番</summary>
        private int SEPARATOR_POS_PARTS_NUM = 5;
        ///<summary>区切り文字：ハイフン</summary>
        private string SEPARATOR_HYPHEN = "-";

        #region グリッド定義
        /// <summary>
        /// 部品組付情報
        /// </summary>
        public class GRID_MAIN {
            /// <summary>品番</summary>
            public static readonly ControlDefine PARTS_NUM = new ControlDefine( "txtPartsNum", "品番", "partsNum", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>加工日(YYYY/MM/DD)</summary>
            public static readonly ControlDefine PROCESSING_YMD = new ControlDefine( "txtProcessingYmd", "加工日", "processingYmd", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>出庫要求日時</summary>
            public static readonly ControlDefine DLV_REQ_DT = new ControlDefine( "txtDlvReqDt", "出庫要求日時", "dlvReqDt", ControlDefine.BindType.Down, typeof( DateTime ) );
            /// <summary>加工連番</summary>
            public static readonly ControlDefine PROCESSING_SEQ = new ControlDefine( "txtProcessingSeq", "加工連番", "processingSeq", ControlDefine.BindType.Down, typeof( string ) );
        }

        /// <summary>
        /// 詳細情報
        /// </summary>
        public class GRID_SUB {
            //詳細なし
        }
        #endregion

        #endregion

        #region プロパティ
        /// <summary>
        /// 表示中ページ(ベースフォーム)情報
        /// </summary>
        private BaseForm CurrentForm {
            get {
                return ( ( BaseForm )Page );
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
        /// コントロール定義
        /// </summary>
        ControlDefine[] _criticalPartsControls = null;
        /// <summary>
        /// コントロール定義アクセサ
        /// </summary>
        ControlDefine[] CriticalPartsControls {
            get {
                if ( true == ObjectUtils.IsNull( _criticalPartsControls ) ) {
                    _criticalPartsControls = ControlUtils.GetControlDefineArray( typeof( GRID_MAIN ) );
                }
                return _criticalPartsControls;
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

        #region イベントハンドラ
        protected void Page_Load( object sender, EventArgs e ) {
            Initialize();
        }
        #endregion

        #region 初期化

        /// <summary>
        /// 初期値設定
        /// </summary>
        public void Initialize() {
            //検索結果取得
            DataTable tblMain = null;
            try {
                tblMain = Business.DetailViewBusiness.SelectTractorMissionDetail( DetailKeyParam.ProductModelCd, DetailKeyParam.Serial ).MainTable;
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

            if ( 0 == tblMain.Rows.Count ) {
                //検索結果0件
                CurrentForm.WriteApplicationMessage( MsgManager.MESSAGE_WRN_62010, CurrentUCInfo.title );
                return;
            }

            InitializeValues( tblMain );
        }

        /// <summary>
        /// データをセット
        /// </summary>
        /// <param name="dt"></param>
        private void InitializeValues( DataTable dt ) {
            DataRow row = dt.Rows[0];
            string partNum = StringUtils.ToString( row[GRID_MAIN.PARTS_NUM.bindField] );    //品番

            //コントロールへの自動データバインド
            Dictionary<string, object> dicControlValues = new Dictionary<string, object>();
            CurrentForm.SetControlValues( this, CriticalPartsControls, row, ref dicControlValues );

            //コントロールへの独自データバインド
            //品番
            if ( false == StringUtils.IsBlank( partNum ) && SEPARATOR_POS_PARTS_NUM < partNum.Length ) {
                //表示形式変換：XXXXXXXXX→XXXXX-XXXX
                partNum = partNum.Insert( SEPARATOR_POS_PARTS_NUM, SEPARATOR_HYPHEN );
            }
            txtPartNum.Value = partNum;

            //出庫要求日時
            txtDvlReqDt.Value = DateUtils.ToString( row[GRID_MAIN.DLV_REQ_DT.bindField], DateUtils.FormatType.Second );
        }

        #endregion

    }
}