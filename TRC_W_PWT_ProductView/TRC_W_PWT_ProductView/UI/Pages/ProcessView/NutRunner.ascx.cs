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
    /// (詳細 エンジン 工程) ナットランナー
    /// </summary>
    public partial class NutRunner : System.Web.UI.UserControl, Defines.Interface.IDetail {

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
        /// NR1軸判定結果NGのstyle
        /// </summary>
        private string CHK_RESULT_NG = " font-ng-condition";

        /// <summary>
        /// (メインリスト)コントロール定義
        /// </summary>
        public class GRID_MAIN {
            /// <summary>TR</summary>
            public static readonly ControlDefine TR_ROW_DATA = new ControlDefine( "trRowData", "TR", "", ControlDefine.BindType.None, typeof( String ) );
            /// <summary>締付回数</summary>
            public static readonly ControlDefine BENCHI_NO = new ControlDefine( "txtBenchiNo", "締付回数", "benchiNo", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>測定日時</summary>   
            public static readonly ControlDefine INSPECTION_DT = new ControlDefine( "txtInspectionDt", "測定日時", "inspectionDt", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>軸１</summary>
            public static readonly ControlDefine SHAFT1 = new ControlDefine( "txtShaft1", "軸1", "shaft1", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>軸２</summary>
            public static readonly ControlDefine SHAFT2 = new ControlDefine( "txtShaft2", "軸2", "shaft2", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>軸３</summary>
            public static readonly ControlDefine SHAFT3 = new ControlDefine( "txtShaft3", "軸3", "shaft3", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>軸４</summary>
            public static readonly ControlDefine SHAFT4 = new ControlDefine( "txtShaft4", "軸4", "shaft4", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>軸５</summary>
            public static readonly ControlDefine SHAFT5 = new ControlDefine( "txtShaft5", "軸5", "shaft5", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>軸６</summary>
            public static readonly ControlDefine SHAFT6 = new ControlDefine( "txtShaft6", "軸6", "shaft6", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>軸７</summary>
            public static readonly ControlDefine SHAFT7 = new ControlDefine( "txtShaft7", "軸7", "shaft7", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>軸８</summary>
            public static readonly ControlDefine SHAFT8 = new ControlDefine( "txtShaft8", "軸8", "shaft8", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>軸1ﾁｪｯｸ</summary>
            public static readonly ControlDefine NR1 = new ControlDefine( "txtNr1", "軸1ﾁｪｯｸ", "nr1", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>軸2ﾁｪｯｸ</summary>
            public static readonly ControlDefine NR2 = new ControlDefine( "txtNr2", "軸2ﾁｪｯｸ", "nr2", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>軸3ﾁｪｯｸ</summary>
            public static readonly ControlDefine NR3 = new ControlDefine( "txtNr3", "軸3ﾁｪｯｸ", "nr3", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>軸4ﾁｪｯｸ</summary>
            public static readonly ControlDefine NR4 = new ControlDefine( "txtNr4", "軸4ﾁｪｯｸ", "nr4", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>軸5ﾁｪｯｸ</summary>
            public static readonly ControlDefine NR5 = new ControlDefine( "txtNr5", "軸5ﾁｪｯｸ", "nr5", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>軸6ﾁｪｯｸ</summary>
            public static readonly ControlDefine NR6 = new ControlDefine( "txtNr6", "軸6ﾁｪｯｸ", "nr6", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>軸7ﾁｪｯｸ</summary>
            public static readonly ControlDefine NR7 = new ControlDefine( "txtNr7", "軸7ﾁｪｯｸ", "nr7", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>軸8ﾁｪｯｸ</summary>
            public static readonly ControlDefine NR8 = new ControlDefine( "txtNr8", "軸8ﾁｪｯｸ", "nr8", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>軸１</summary>
            public static readonly ControlDefine SHAFT1_BK = new ControlDefine( "txtShaft1_bk", "軸1", "shaft1_bk", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>軸２</summary>
            public static readonly ControlDefine SHAFT2_BK = new ControlDefine( "txtShaft2_bk", "軸2", "shaft2_bk", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>軸３</summary>
            public static readonly ControlDefine SHAFT3_BK = new ControlDefine( "txtShaft3_bk", "軸3", "shaft3_bk", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>軸４</summary>
            public static readonly ControlDefine SHAFT4_BK = new ControlDefine( "txtShaft4_bk", "軸4", "shaft4_bk", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>軸５</summary>
            public static readonly ControlDefine SHAFT5_BK = new ControlDefine( "txtShaft5_bk", "軸5", "shaft5_bk", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>軸６</summary>
            public static readonly ControlDefine SHAFT6_BK = new ControlDefine( "txtShaft6_bk", "軸6", "shaft6_bk", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>軸７</summary>
            public static readonly ControlDefine SHAFT7_BK = new ControlDefine( "txtShaft7_bk", "軸7", "shaft7_bk", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>軸８</summary>
            public static readonly ControlDefine SHAFT8_BK = new ControlDefine( "txtShaft8_bk", "軸8", "shaft8_bk", ControlDefine.BindType.Down, typeof( String ) );

        }

        /// <summary>
        /// (サブリスト)コントロール定義
        /// </summary>
        public class GRID_SUB {
        }


        /// <summary>
        /// 締付結果情報
        /// </summary>
        public class FL_RESULT {
            /// <summary>締付結果</summary>
            public static readonly ControlDefine DEF_FL_RESULT = new ControlDefine( "txtFLResult", "締付結果", "result", ControlDefine.BindType.Down, typeof( string ) );
        }
        public class FR_RESULT {
            /// <summary>締付結果</summary>
            public static readonly ControlDefine DEF_FR_RESULT = new ControlDefine( "txtFRResult", "締付結果", "result", ControlDefine.BindType.Down, typeof( string ) );
        }
        public class RL_RESULT {
            /// <summary>締付結果</summary>
            public static readonly ControlDefine DEF_RL_RESULT = new ControlDefine( "txtRLResult", "締付結果", "result", ControlDefine.BindType.Down, typeof( string ) );
        }
        public class RR_RESULT {
            /// <summary>締付結果</summary>
            public static readonly ControlDefine DEF_RR_RESULT = new ControlDefine( "txtRRResult", "締付結果", "result", ControlDefine.BindType.Down, typeof( string ) );
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
        /// (結果)コントロール定義
        /// </summary>
        ControlDefine[] _resultControls = null;
        /// <summary>
        /// (結果)コントロール定義アクセサ
        /// </summary>
        ControlDefine[] ResFLControls {
            get {
                _resultControls = null;
                if ( true == ObjectUtils.IsNull( _resultControls ) ) {
                    _resultControls = ControlUtils.GetControlDefineArray( typeof( FL_RESULT ) );
                }
                return _resultControls;
            }
        }
        ControlDefine[] ResFRControls {
            get {
                _resultControls = null;
                if ( true == ObjectUtils.IsNull( _resultControls ) ) {
                    _resultControls = ControlUtils.GetControlDefineArray( typeof( FR_RESULT ) );
                }
                return _resultControls;
            }
        }
        ControlDefine[] ResRLControls {
            get {
                _resultControls = null;
                if ( true == ObjectUtils.IsNull( _resultControls ) ) {
                    _resultControls = ControlUtils.GetControlDefineArray( typeof( RL_RESULT ) );
                }
                return _resultControls;
            }
        }
        ControlDefine[] ResRRControls {
            get {
                _resultControls = null;
                if ( true == ObjectUtils.IsNull( _resultControls ) ) {
                    _resultControls = ControlUtils.GetControlDefineArray( typeof( RR_RESULT ) );
                }
                return _resultControls;
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
        /// FLバインド
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lstFL_ItemDataBound( object sender, ListViewItemEventArgs e ) {
            ItemDataBoundFL( sender, e );
        }


        protected void lstFR_ItemDataBound( object sender, ListViewItemEventArgs e ) {
            ItemDataBoundFR( sender, e );
        }

        protected void lstRL_ItemDataBound( object sender, ListViewItemEventArgs e ) {
            ItemDataBoundRL( sender, e );
        }

        protected void lstRR_ItemDataBound( object sender, ListViewItemEventArgs e ) {
            ItemDataBoundRR( sender, e );
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
            Business.DetailViewBusiness.ResultSet res = new Business.DetailViewBusiness.ResultSet();
            try {
                res = Business.DetailViewBusiness.SelectTractorNutRunnerDetail( DetailKeyParam.ProductModelCd, DetailKeyParam.Serial );
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
        private void InitializeValues( Business.DetailViewBusiness.ResultSet res ) {

            //改行設定
            foreach ( DataRow dtRow in res.MainTable.Rows ) {
                dtRow["inspectionDt"] = dtRow["inspectionDt"] + Environment.NewLine + dtRow["inspectionTime"];
                for ( int idx = 1; idx < 9; idx++ ) {
                    //結果とトルク、角度を結合
                    dtRow["shaft" + idx] = dtRow["nr" + idx] + Environment.NewLine + dtRow["shaft" + idx] + Environment.NewLine + dtRow["angle" + idx];
                }

            }

            //バインド(List)
            //フロント(左)
            DataView dvFL = new DataView( res.MainTable );
            dvFL.RowFilter = "position = 'FL'";
            dvFL.Sort = "benchiNo DESC";
            DataTable dtBindFL = dvFL.ToTable();
            lstFL.DataSource = dtBindFL;
            lstFL.DataBind();

            //フロント(右)
            DataView dvFR = new DataView( res.MainTable );
            dvFR.RowFilter = "position = 'FR'";
            dvFR.Sort = "benchiNo DESC";
            DataTable dtBindFR = dvFR.ToTable();
            lstFR.DataSource = dtBindFR;
            lstFR.DataBind();

            //リア(左)
            DataView dvRL = new DataView( res.MainTable );
            dvRL.RowFilter = "position = 'RL'";
            dvRL.Sort = "benchiNo DESC";
            DataTable dtBindRL = dvRL.ToTable();
            lstRL.DataSource = dtBindRL;
            lstRL.DataBind();

            //リア(右)
            DataView dvRR = new DataView( res.MainTable );
            dvRR.RowFilter = "position = 'RR'";
            dvRR.Sort = "benchiNo DESC";
            DataTable dtBindRR = dvRR.ToTable();
            lstRR.DataSource = dtBindRR;
            lstRR.DataBind();


            //バインド(結果)
            //フロント(左)
            if ( 0 < dtBindFL.Rows.Count ) {
                DataRow row = dtBindFL.Rows[0];
                Dictionary<string, object> dicControl = new Dictionary<string, object>();
                CurrentForm.SetControlValues( this, ResFLControls, row, ref dicControl );

                if (false == ( row["result"].Equals( "OK" )) ) {
                    string strCss = txtFLResult.CssClass;
                    txtFLResult.CssClass = strCss + CHK_RESULT_NG;
                }
            }
            //フロント(右)
            if ( 0 < dtBindFR.Rows.Count ) {
                DataRow row = dtBindFR.Rows[0];
                Dictionary<string, object> dicControl = new Dictionary<string, object>();
                CurrentForm.SetControlValues( this, ResFRControls, row, ref dicControl );

                if ( false == ( row["result"].Equals( "OK" ) ) ) {
                    string strCss = txtFRResult.CssClass;
                    txtFRResult.CssClass = strCss + CHK_RESULT_NG;
                }
            }
            //リア(左)
            if ( 0 < dtBindRL.Rows.Count ) {
                DataRow row = dtBindRL.Rows[0];
                Dictionary<string, object> dicControl = new Dictionary<string, object>();
                CurrentForm.SetControlValues( this, ResRLControls, row, ref dicControl );

                if ( false == ( row["result"].Equals( "OK" ) ) ) {
                    string strCss = txtRLResult.CssClass;
                    txtRLResult.CssClass = strCss + CHK_RESULT_NG;
                }
            }
            //リア(右)
            if ( 0 < dtBindRR.Rows.Count ) {
                DataRow row = dtBindRR.Rows[0];
                Dictionary<string, object> dicControl = new Dictionary<string, object>();
                CurrentForm.SetControlValues( this, ResRRControls, row, ref dicControl );

                if ( false == ( row["result"].Equals( "OK" ) ) ) {
                    string strCss = txtRRResult.CssClass;
                    txtRRResult.CssClass = strCss + CHK_RESULT_NG;
                }
            }

        }

        #endregion

        #region リストバインド
        /// <summary>
        /// FLバインド処理
        /// </summary>
        /// <param name="parameters">イベントパラメータ</param>
        private void ItemDataBoundFL( params object[] parameters ) {
            object sender = parameters[0];
            ListViewItemEventArgs e = (ListViewItemEventArgs)parameters[1];

            if ( e.Item.ItemType == ListViewItemType.DataItem ) {
                DataRow rowBind = ( (DataRowView)e.Item.DataItem ).Row;
                Dictionary<string, object> dicSetValues = new Dictionary<string, object>();
                CurrentForm.SetControlValues( e.Item, MainControls, rowBind, ref dicSetValues );

                //クリックイベントセット
                HtmlTableRow trRow = (HtmlTableRow)e.Item.FindControl( GRID_MAIN.TR_ROW_DATA.controlId );
                trRow.Attributes[ControlUtils.ON_CLICK] = string.Format( ControlUtils.LIST_VIEW_SELECTED, e.Item.DataItemIndex, "" );

                //フォント設定
                fontControl( e );
                
                //ツールチップ設定
                ControlUtils.SetToolTip( e.Item );
            }
        }
        /// <summary>
        /// FRバインド処理
        /// </summary>
        /// <param name="parameters">イベントパラメータ</param>
        private void ItemDataBoundFR( params object[] parameters ) {
            object sender = parameters[0];
            ListViewItemEventArgs e = (ListViewItemEventArgs)parameters[1];

            if ( e.Item.ItemType == ListViewItemType.DataItem ) {
                DataRow rowBind = ( (DataRowView)e.Item.DataItem ).Row;
                Dictionary<string, object> dicSetValues = new Dictionary<string, object>();
                CurrentForm.SetControlValues( e.Item, MainControls, rowBind, ref dicSetValues );

                //クリックイベントセット
                HtmlTableRow trRow = (HtmlTableRow)e.Item.FindControl( GRID_MAIN.TR_ROW_DATA.controlId );
                trRow.Attributes[ControlUtils.ON_CLICK] = string.Format( ControlUtils.LIST_VIEW_SELECTED, e.Item.DataItemIndex, "" );

                //フォント設定
                fontControl( e );

                //ツールチップ設定
                ControlUtils.SetToolTip( e.Item );

            }
        }
        /// <summary>
        /// RLバインド処理
        /// </summary>
        /// <param name="parameters">イベントパラメータ</param>
        private void ItemDataBoundRL( params object[] parameters ) {
            object sender = parameters[0];
            ListViewItemEventArgs e = (ListViewItemEventArgs)parameters[1];

            if ( e.Item.ItemType == ListViewItemType.DataItem ) {
                DataRow rowBind = ( (DataRowView)e.Item.DataItem ).Row;
                Dictionary<string, object> dicSetValues = new Dictionary<string, object>();
                CurrentForm.SetControlValues( e.Item, MainControls, rowBind, ref dicSetValues );

                //クリックイベントセット
                HtmlTableRow trRow = (HtmlTableRow)e.Item.FindControl( GRID_MAIN.TR_ROW_DATA.controlId );
                trRow.Attributes[ControlUtils.ON_CLICK] = string.Format( ControlUtils.LIST_VIEW_SELECTED, e.Item.DataItemIndex, "" );

                //フォント設定
                fontControl( e );

                //ツールチップ設定
                ControlUtils.SetToolTip( e.Item );

            }
        }
        /// <summary>
        /// RRバインド処理
        /// </summary>
        /// <param name="parameters">イベントパラメータ</param>
        private void ItemDataBoundRR( params object[] parameters ) {
            object sender = parameters[0];
            ListViewItemEventArgs e = (ListViewItemEventArgs)parameters[1];

            if ( e.Item.ItemType == ListViewItemType.DataItem ) {
                DataRow rowBind = ( (DataRowView)e.Item.DataItem ).Row;
                Dictionary<string, object> dicSetValues = new Dictionary<string, object>();
                CurrentForm.SetControlValues( e.Item, MainControls, rowBind, ref dicSetValues );

                //クリックイベントセット
                HtmlTableRow trRow = (HtmlTableRow)e.Item.FindControl( GRID_MAIN.TR_ROW_DATA.controlId );
                trRow.Attributes[ControlUtils.ON_CLICK] = string.Format( ControlUtils.LIST_VIEW_SELECTED, e.Item.DataItemIndex, "" );

                //フォント設定
                fontControl( e );

                //ツールチップ設定
                ControlUtils.SetToolTip( e.Item );

            }
        }

        #endregion

        #region フォント設定
        /// <summary>
        /// 各軸判定結果に応じて、フォントの設定を行う
        /// </summary>
        /// <param name="e"></param>
        private void fontControl( ListViewItemEventArgs e ) {

            //NR1軸トルク
            TextBox txtNr1 = (TextBox)e.Item.FindControl( GRID_MAIN.NR1.controlId );
            TextBox txtShaft1 = (TextBox)e.Item.FindControl( GRID_MAIN.SHAFT1.controlId );
            TextBox txtShaft1_bk = (TextBox)e.Item.FindControl( GRID_MAIN.SHAFT1_BK.controlId );
            string strNR1 = txtNr1.Text;
            if ( false == ( strNR1.Equals( "OK" ) ) && false == ( txtShaft1_bk.Equals( "0" ) ) ) {
                string strCss = txtShaft1.CssClass;
                txtShaft1.CssClass = strCss + CHK_RESULT_NG;
            }

            //NR2軸トルク
            TextBox txtNr2 = (TextBox)e.Item.FindControl( GRID_MAIN.NR2.controlId );
            TextBox txtShaft2 = (TextBox)e.Item.FindControl( GRID_MAIN.SHAFT2.controlId );
            TextBox txtShaft2_bk = (TextBox)e.Item.FindControl( GRID_MAIN.SHAFT2_BK.controlId );
            string strNR2 = txtNr2.Text;
            if ( false == ( strNR2.Equals( "OK" ) )  && false == ( txtShaft2_bk.Text.Equals( "0" ) ) ) {
                string strCss = txtShaft2.CssClass;
                txtShaft2.CssClass = strCss + CHK_RESULT_NG;
            }

            //NR3軸トルク
            TextBox txtNr3 = (TextBox)e.Item.FindControl( GRID_MAIN.NR3.controlId );
            TextBox txtShaft3 = (TextBox)e.Item.FindControl( GRID_MAIN.SHAFT3.controlId );
            TextBox txtShaft3_bk = (TextBox)e.Item.FindControl( GRID_MAIN.SHAFT3_BK.controlId );
            string strNR3 = txtNr3.Text;
            if ( false == ( strNR3.Equals( "OK" ) )  && false == ( txtShaft3_bk.Text.Equals( "0" ) ) ) {
                string strCss = txtShaft3.CssClass;
                txtShaft3.CssClass = strCss + CHK_RESULT_NG;
            }

            //NR4軸トルク
            TextBox txtNr4 = (TextBox)e.Item.FindControl( GRID_MAIN.NR4.controlId );
            TextBox txtShaft4 = (TextBox)e.Item.FindControl( GRID_MAIN.SHAFT4.controlId );
            TextBox txtShaft4_bk = (TextBox)e.Item.FindControl( GRID_MAIN.SHAFT4_BK.controlId );
            string strNR4 = txtNr4.Text;
            if ( false == ( strNR4.Equals( "OK" ) )  && false == ( txtShaft4_bk.Text.Equals( "0" ) ) ) {
                string strCss = txtShaft4.CssClass;
                txtShaft4.CssClass = strCss + CHK_RESULT_NG;
            }

            //NR5軸トルク
            TextBox txtNr5 = (TextBox)e.Item.FindControl( GRID_MAIN.NR5.controlId );
            TextBox txtShaft5 = (TextBox)e.Item.FindControl( GRID_MAIN.SHAFT5.controlId );
            TextBox txtShaft5_bk = (TextBox)e.Item.FindControl( GRID_MAIN.SHAFT5_BK.controlId );
            string strNR5 = txtNr5.Text;
            if ( false == ( strNR5.Equals( "OK" ) )  && false == ( txtShaft5_bk.Text.Equals( "0" ) ) ) {
                string strCss = txtShaft5.CssClass;
                txtShaft5.CssClass = strCss + CHK_RESULT_NG;
            }

            //NR6軸トルク
            TextBox txtNr6 = (TextBox)e.Item.FindControl( GRID_MAIN.NR6.controlId );
            TextBox txtShaft6 = (TextBox)e.Item.FindControl( GRID_MAIN.SHAFT6.controlId );
            TextBox txtShaft6_bk = (TextBox)e.Item.FindControl( GRID_MAIN.SHAFT6_BK.controlId );
            string strNR6 = txtNr6.Text;
            if ( false == ( strNR6.Equals( "OK" ) ) && false == ( txtShaft6_bk.Text.Equals( "0" ) ) ) {
                string strCss = txtShaft6.CssClass;
                txtShaft6.CssClass = strCss + CHK_RESULT_NG;
            }

            //NR7軸トルク
            TextBox txtNr7 = (TextBox)e.Item.FindControl( GRID_MAIN.NR7.controlId );
            TextBox txtShaft7 = (TextBox)e.Item.FindControl( GRID_MAIN.SHAFT7.controlId );
            TextBox txtShaft7_bk = (TextBox)e.Item.FindControl( GRID_MAIN.SHAFT7_BK.controlId );
            string strNR7 = txtNr7.Text;
            if ( false == ( strNR7.Equals( "OK" ) ) && false == ( txtShaft7_bk.Text.Equals( "0" ) ) ) {
                string strCss = txtShaft7.CssClass;
                txtShaft7.CssClass = strCss + CHK_RESULT_NG;
            }

            //NR8軸トルク
            TextBox txtNr8 = (TextBox)e.Item.FindControl( GRID_MAIN.NR8.controlId );
            TextBox txtShaft8 = (TextBox)e.Item.FindControl( GRID_MAIN.SHAFT8.controlId );
            TextBox txtShaft8_bk = (TextBox)e.Item.FindControl( GRID_MAIN.SHAFT8_BK.controlId );
            string strNR8 = txtNr8.Text;
            if ( false == ( strNR8.Equals( "OK" ) ) && false == ( txtShaft8_bk.Text.Equals( "0" ) ) ) {
                string strCss = txtShaft8.CssClass;
                txtShaft8.CssClass = strCss + CHK_RESULT_NG;
            }


            //文言を非表示に設定
            txtNr1.Visible = false;
            txtNr2.Visible = false;
            txtNr3.Visible = false;
            txtNr4.Visible = false;
            txtNr5.Visible = false;
            txtNr6.Visible = false;
            txtNr7.Visible = false;
            txtNr8.Visible = false;

            txtShaft1_bk.Visible = false;
            txtShaft2_bk.Visible = false;
            txtShaft3_bk.Visible = false;
            txtShaft4_bk.Visible = false;
            txtShaft5_bk.Visible = false;
            txtShaft6_bk.Visible = false;
            txtShaft7_bk.Visible = false;
            txtShaft8_bk.Visible = false;


        }

        #endregion
    }
}