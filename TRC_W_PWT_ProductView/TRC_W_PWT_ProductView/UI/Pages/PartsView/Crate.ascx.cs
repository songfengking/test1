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
    /// トラクタ部品詳細画面:クレート
    /// </summary>
    public partial class Crate : System.Web.UI.UserControl, Defines.Interface.IDetail {

        //ロガー定義
        private static readonly Logger logger = new Logger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

        #region 定数定義
        /// <summary>
        /// 部品組付情報
        /// </summary>
        public class GRID_MAIN {
            /// <summary>組付日時(yyyy/MM/dd hh:mm:ss)</summary>
            public static readonly ControlDefine INSTALL_DT = new ControlDefine( "txtInstallDt", "組付日時", "installDt", ControlDefine.BindType.None, typeof( DateTime ) );
            /// <summary>クレート機番</summary>
            public static readonly ControlDefine CRATE_SERIAL = new ControlDefine( "txtCrateSerial", "クレート機番", "crateSerial", ControlDefine.BindType.Down, typeof( string ) );
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

        #region イベント

        /// <summary>
        /// ページロード
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load( object sender, EventArgs e ) {
            CurrentForm.RaiseEvent( DoPageLoad );
        }

        #endregion

        #region イベントメソッド

        #region ページイベント

        /// <summary>
        /// 初期化処理
        /// </summary>
        public void Initialize() {

            //検索結果取得
            DataTable tblMain = null;
            try {
                tblMain = Business.DetailViewBusiness.SelectTractorCrateDetail( DetailKeyParam.ProductModelCd, DetailKeyParam.Idno ).MainTable;
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
        /// ページロード処理
        /// </summary>
        private void DoPageLoad() {
        }

        #endregion

        /// <summary>
        /// 初期値設定
        /// </summary>
        private void InitializeValues( DataTable tblMainData ) {
            if ( 1 == tblMainData.Rows.Count ) {
                //1行のみ表示
                DataRow row = tblMainData.Rows[0];

                //コントロールへの自動データバインド
                Dictionary<string, object> dicControlValues = new Dictionary<string, object>();
                CurrentForm.SetControlValues( this, CriticalPartsControls, row, ref dicControlValues );

                //コントロールへの独自データバインド
                //組付日時加工/セット
                KTTextBox txtInstallDt = ( (KTTextBox)this.tblMain.FindControl( GRID_MAIN.INSTALL_DT.controlId ) );
                txtInstallDt.Value = DateUtils.ToString( row[GRID_MAIN.INSTALL_DT.bindField], DateUtils.FormatType.Second );
            }
        }

        #endregion
    }
}