////////////////////////////////////////////////////////////////////////////////////////////////
//      クボタ筑波工場  TRC
//      概要：トレーサビリティシステム 型式IDNOリストダイアログ
//---------------------------------------------------------------------------
//           Ver 1.44.0.0  :  2021/06/18  豊島  新規作成(java版からリプレース)
//           Ver 1.44.1.1  :  2021/10/25  豊島　RowDataBoundのログ出力抑制
////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KTFramework.Common;
using TRC_W_PWT_ProductView.Business;
using TRC_W_PWT_ProductView.Common;
using TRC_W_PWT_ProductView.Dao.Com;
using TRC_W_PWT_ProductView.Defines;
using TRC_W_PWT_ProductView.Defines.TypeDefine;
using TRC_W_PWT_ProductView.Session;
using TRC_W_PWT_ProductView.UI.Base;

namespace TRC_W_PWT_ProductView.UI.Pages.Order {
    /// <summary>
    /// 型式IDNOリストダイアログ
    /// </summary>
    public partial class ProCodeIdnoDialog : BaseForm {

        // セッションをクリアする
        private ConditionInfoSessionHandler.ST_CONDITION ConditionInfo = new ConditionInfoSessionHandler.ST_CONDITION();

        #region 定数
        /// <summary>基準日</summary>
        private const string BASE_DATA_STR = "2001/01/01";
        /// <summary>型式コードURLパラメータ</summary>
        private const string KATASHIKICD_URL_PRM = "katashikiCd";
        /// <summary>型式名URLパラメータ</summary>
        private const string KATASHIKINM_URL_PRM = "katashikiNm";

        #region 状態
        /// <summary>全て(表示用)</summary>
        private const string LOCATION_FLAG_ALL_DISP = "全て";
        /// <summary>全て</summary>
        private const string LOCATION_FLAG_ALL = "";
        /// <summary>空き(表示用)</summary>
        private const string LOCATION_FLAG_NOENGINE_DISP = "空き";
        /// <summary>空き</summary>
        private const string LOCATION_FLAG_NOENGINE = "0";
        /// <summary>空き以外(表示用)</summary>
        private const string LOCATION_FLAG_ANYENGINE_DISP = "空き以外";
        /// <summary>空き以外</summary>
        private const string LOCATION_FLAG_ANYENGINE = "99";
        /// <summary>在席(表示用)</summary>
        private const string LOCATION_FLAG_STOCKED_DISP = "在席";
        /// <summary>在席</summary>
        private const string LOCATION_FLAG_STOCKED = "1";
        /// <summary>入庫(表示用)</summary>
        private const string LOCATION_FLAG_ACCEPTED_DISP = "入庫";
        /// <summary>入庫</summary>
        private const string LOCATION_FLAG_ACCEPTED = "2";
        /// <summary>出庫(表示用)</summary>
        private const string LOCATION_FLAG_DELIVERED_DISP = "出庫";
        /// <summary>出庫</summary>
        private const string LOCATION_FLAG_DELIVERED = "3";
        /// <summary>空ﾊﾟﾚ(表示用)</summary>
        private const string LOCATION_FLAG_PALETTE_DISP = "空ﾊﾟﾚ";
        /// <summary>空ﾊﾟﾚ</summary>
        private const string LOCATION_FLAG_PALETTE = "4";
        #endregion

        /// <summary>
        /// グリッドビューグループ
        /// </summary>
        const string GRID_ORDER_PROCODE_IDNO_DIALOG_GROUP_CD = "ProCodeIdnoDialog";

        /// <summary>
        /// 型式IDNOリスト定義
        /// </summary>
        internal class GRID_PROCODE_IDNO_LIST {
            /// <summary>
            /// No
            /// </summary>
            public static readonly GridViewDefine DISP_ORDER = new GridViewDefine( "No", "DISP_ORDER", typeof( string ), "", false, HorizontalAlign.Right, 40, true, true );
            /// <summary>
            /// 立体倉庫
            /// </summary>
            public static readonly GridViewDefine RITTAI_NAME = new GridViewDefine( "立体倉庫", "立体倉庫", typeof( string ), "", true, HorizontalAlign.Center, 90, true, true );
            /// <summary>
            /// 号機
            /// </summary>
            public static readonly GridViewDefine STOCK_KBN = new GridViewDefine( "棚番", "号機", typeof( string ), "", true, HorizontalAlign.Center, 80, true, true );
            /// <summary>
            /// 連
            /// </summary>
            public static readonly GridViewDefine STOCK_REN = new GridViewDefine( "連", "連", typeof( string ), "", true, HorizontalAlign.Center, 100, false, true );
            /// <summary>
            /// 段
            /// </summary>
            public static readonly GridViewDefine STOCK_DAN = new GridViewDefine( "段", "段", typeof( string ), "", true, HorizontalAlign.Center, 100, false, true );
            /// <summary>
            /// 列
            /// </summary>
            public static readonly GridViewDefine STOCK_RETSU = new GridViewDefine( "列", "列", typeof( string ), "", true, HorizontalAlign.Center, 100, false, true );
            /// <summary>
            /// 禁止棚
            /// </summary>
            public static readonly GridViewDefine STOP_FLAG = new GridViewDefine( "禁止棚", "禁止棚フラグ", typeof( string ), "", true, HorizontalAlign.Center, 90, true, true );
            /// <summary>
            /// 状態
            /// </summary>
            public static readonly GridViewDefine LOCATION_FLAG = new GridViewDefine( "状態", "ロケーションフラグ", typeof( string ), "", true, HorizontalAlign.Center, 90, true, true );
            /// <summary>
            /// GP
            /// </summary>
            public static readonly GridViewDefine KUMITATE_PATTERN = new GridViewDefine( "GP", "パターンコード", typeof( string ), "", true, HorizontalAlign.Center, 50, true, true );
            /// <summary>
            /// 種別
            /// </summary>
            public static readonly GridViewDefine ENGINE_SYUBETSU = new GridViewDefine( "種別", "エンジン種別", typeof( string ), "", true, HorizontalAlign.Center, 50, true, true );
            /// <summary>
            /// 搭載/OEM
            /// </summary>
            public static readonly GridViewDefine TOUSAI_OEM = new GridViewDefine( "搭載/OEM", "搭載OEM", typeof( string ), "", true, HorizontalAlign.Center, 90, true, true );
            /// <summary>
            /// 内外作
            /// </summary>
            public static readonly GridViewDefine NAIGAISKAU = new GridViewDefine( "内外作", "内外作", typeof( string ), "", true, HorizontalAlign.Center, 60, true, true );
            /// <summary>
            /// IDNO
            /// </summary>
            public static readonly GridViewDefine IDNO = new GridViewDefine( "IDNO", "IDNO", typeof( string ), "", true, HorizontalAlign.Center, 90, true, true );
            /// <summary>
            /// 機番
            /// </summary>
            public static readonly GridViewDefine KIBAN = new GridViewDefine( "機番", "機番", typeof( string ), "", true, HorizontalAlign.Center, 90, true, true );
            /// <summary>
            /// 特記
            /// </summary>
            public static readonly GridViewDefine TOKKI = new GridViewDefine( "特記", "特記事項", typeof( string ), "", true, HorizontalAlign.Center, 100, true, true );
            /// <summary>
            /// 運転
            /// </summary>
            public static readonly GridViewDefine UNTENFLAG = new GridViewDefine( "運転", "運転フラグ", typeof( string ), "", true, HorizontalAlign.Center, 90, true, true );
            /// <summary>
            /// 入庫日時
            /// </summary>
            public static readonly GridViewDefine INSTOCK_DATE = new GridViewDefine( "入庫日時", "入庫日時", typeof( string ), "{0:" + "MM/dd HH:mm" + "}", true, HorizontalAlign.Center, 100, true, true );
        }
        #endregion

        #region プロパティ
        //ロガー定義
        private static readonly Logger _logger = new Logger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

        /// <summary>
        /// 一覧定義情報
        /// </summary>
        GridViewDefine[] _gridviewDefault = null;
        /// <summary>
        /// 一覧定義情報アクセサ
        /// </summary>
        GridViewDefine[] gridviewDefault {
            get {
                if ( true == ObjectUtils.IsNull( _gridviewDefault ) ) {
                    _gridviewDefault = ControlUtils.GetGridViewDefineArray( typeof( GRID_PROCODE_IDNO_LIST ) );
                }
                return _gridviewDefault;
            }
        }
        #endregion

        #region イベント
        /// <summary>
        /// ページロードイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load( object sender, EventArgs e ) {
            base.RaiseEvent( DoPageLoad );
        }

        /// <summary>
        /// 一覧行データバインドイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvProCodeIdnoDialogBody_RowDataBound( object sender, GridViewRowEventArgs e ) {
            try {
                ClearApplicationMessage();
                RowDataBound( sender, e );
            } catch ( Exception ex ) {
                //イベント処理中にエラー発生
                _logger.Exception( ex );
                throw ex;
            }
        }

        /// <summary>
        /// 一覧ページ遷移イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvProCodeIdnoDialogBody_PageIndexChanging( object sender, CommandEventArgs e ) {
            base.RaiseEvent( DoChangePageIndex, sender, e );
        }
        #endregion

        #region 画面初期化処理
        /// <summary>
        /// 初期処理
        /// </summary>
        protected override void DoPageLoad() {
            // ベース ページロード処理
            base.DoPageLoad();
            int resultCnt = 0;
            if ( ObjectUtils.IsNotNull( ConditionInfo.ResultData ) ) {
                resultCnt = ConditionInfo.ResultData.Rows.Count;
            }
            ControlUtils.SetGridViewPager( ref pnlPager, gvProCodeIdnoDialogBody, gvProCodeIdnoDialogBody_PageIndexChanging, resultCnt, gvProCodeIdnoDialogBody.PageIndex );
        }
        #endregion

        #region 機能別処理
        /// <summary>
        /// 初期処理
        /// </summary>
        protected override void Initialize() {
            // アクセスカウンター登録
            Dao.Com.AccessCounterDao.Entry( base.CurrentPageInfo.pageId );
            // ベース処理初期化処理
            base.Initialize();
            try {
                // 親画面の検索情報を取得
                string rittaiNum = SearchEngineStock.RITTAI_NUM;
                string stopFlag = SearchEngineStock.STOP_FLAG;
                string locationFlag = SearchEngineStock.LOCATION_FLAG;
                string engineSyubetsu = SearchEngineStock.ENGINE_SYUBETSU;
                string tousaiOem = SearchEngineStock.TOUSAI_OEM;
                string naigaisaku = SearchEngineStock.NAIGAISAKU;
                string untenFlag = SearchEngineStock.UNTEN_FLAG;
                string idno = SearchEngineStock.IDNO;
                string kiban = SearchEngineStock.KIBAN;
                string tokki = SearchEngineStock.TOKKI;

                // URL引数から型式コード・型式名を取得
                string katashikiCode = Request.QueryString[KATASHIKICD_URL_PRM];
                string katashikiName = Request.QueryString[KATASHIKINM_URL_PRM];

                // ダイアログタイトルの差し替え
                lblTitle.Text = katashikiCode + "(" + katashikiName.Trim() + ")";

                // 全在庫検索（型式別IDNO一覧用 ソート条件固定検索）
                var resultDt = OrderBusiness.SelectRittaiZaikoAllForKatashikiIdnoList( rittaiNum, stopFlag, locationFlag, engineSyubetsu, tousaiOem, naigaisaku, untenFlag, idno, kiban, tokki, katashikiCode, katashikiName );
                // 結果をセッションに格納
                ConditionInfoSessionHandler.ST_CONDITION cond = new ConditionInfoSessionHandler.ST_CONDITION();
                cond.ResultData = resultDt;
                ConditionInfo = cond;
                if ( ConditionInfo.ResultData != null ) {
                    // 結果が存在する場合、件数表示
                    ntbResultCount.Value = resultDt.Rows.Count;
                    if ( ConditionInfo.ResultData.Rows.Count > 0 ) {
                        // 結果が1件以上存在する場合、全在庫をDataGridViewに設定
                        ControlUtils.ShowGridViewHeader( gvProCodeIdnoDialogHeader, gridviewDefault, ConditionInfo, true );
                        ControlUtils.BindGridView_WithTempField( gvProCodeIdnoDialogBody, gridviewDefault, resultDt );
                        ControlUtils.SetGridViewPager( ref pnlPager, gvProCodeIdnoDialogBody, gvProCodeIdnoDialogBody_PageIndexChanging, ConditionInfo.ResultData.Rows.Count, gvProCodeIdnoDialogBody.PageIndex );
                    }
                }
            } catch ( Exception ex ) {
                _logger.Error( "型式IDNOリスト画面.初期処理で例外発生：{0}", ex.Message );
                base.WriteApplicationMessage( MsgManager.MESSAGE_ERR_84010 );
            }
        }
        #endregion

        /// <summary>
        /// 一覧行データバインド
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RowDataBound( params object[] parameters ) {
            var sender = (GridView)parameters[0];
            var e = (GridViewRowEventArgs)parameters[1];
            if ( e.Row.RowType == DataControlRowType.DataRow ) {
                // データ行のみバインド処理を行う
                int index = 0;
                if ( GetColumnIndex( sender, GRID_PROCODE_IDNO_LIST.DISP_ORDER, out index ) == true ) {
                    //NOの場合、連番を振る
                    var data = e.Row.RowIndex + 1 + sender.PageIndex * sender.PageSize;
                    ( (Label)e.Row.Cells[index].Controls[0] ).Text = data.ToString();
                }
                if ( GetColumnIndex( sender, GRID_PROCODE_IDNO_LIST.RITTAI_NAME, out index ) == true ) {
                    // 立体倉庫の場合、該当する文字列に変換する (該当なしの場合は"")
                    var data = ( (DataRowView)e.Row.DataItem ).Row[GRID_PROCODE_IDNO_LIST.RITTAI_NAME.bindField].ToString();
                    ( (Label)e.Row.Cells[index].Controls[0] ).Text = OrderBusiness.convertToDisplayRittai( data );
                }
                if ( GetColumnIndex( sender, GRID_PROCODE_IDNO_LIST.STOCK_KBN, out index ) == true ) {
                    // 号機の場合、号機-連-段-列を連結する
                    // 本来は棚番に連結したものを設定するが、一覧定義と検索で取得した項目がずれるため、
                    // 号機列を棚番列として扱う
                    string locNm = "";
                    // 号機
                    locNm += ( (DataRowView)e.Row.DataItem ).Row[GRID_PROCODE_IDNO_LIST.STOCK_KBN.bindField].ToString() + "-";
                    // 連
                    locNm += ( (DataRowView)e.Row.DataItem ).Row[GRID_PROCODE_IDNO_LIST.STOCK_REN.bindField].ToString() + "-";
                    // 段
                    locNm += ( (DataRowView)e.Row.DataItem ).Row[GRID_PROCODE_IDNO_LIST.STOCK_DAN.bindField].ToString() + "-";
                    // 列
                    locNm += ( (DataRowView)e.Row.DataItem ).Row[GRID_PROCODE_IDNO_LIST.STOCK_RETSU.bindField].ToString();
                    ( (Label)e.Row.Cells[index].Controls[0] ).Text = locNm;
                }
                if ( GetColumnIndex( sender, GRID_PROCODE_IDNO_LIST.STOP_FLAG, out index ) == true ) {
                    // 禁止棚の場合、該当する文字列に変換する (該当なしの場合は"")
                    var data = ( (DataRowView)e.Row.DataItem ).Row[GRID_PROCODE_IDNO_LIST.STOP_FLAG.bindField].ToString();
                    ( (Label)e.Row.Cells[index].Controls[0] ).Text = OrderBusiness.convertToDisplayRittaiStop( data );
                }
                if ( GetColumnIndex( sender, GRID_PROCODE_IDNO_LIST.LOCATION_FLAG, out index ) == true ) {
                    // 状態の場合、該当する文字列に変換する (該当なしの場合は"")
                    var data = ( (DataRowView)e.Row.DataItem ).Row[GRID_PROCODE_IDNO_LIST.LOCATION_FLAG.bindField].ToString();
                    ( (Label)e.Row.Cells[index].Controls[0] ).Text = OrderBusiness.convertToDisplayLocation( data );

                    if ( false == ( data == LOCATION_FLAG_NOENGINE ) ) {
                        // 「空き(0)」でない場合、フォーマット変換を行い、入庫日時が2001/01/01よりも古いかチェックする
                        if ( GetColumnIndex( sender, GRID_PROCODE_IDNO_LIST.ENGINE_SYUBETSU, out index ) == true ) {
                            // 種別の場合、該当する文字列に変換する (該当なしの場合は"")
                            var syubetsu = ( (DataRowView)e.Row.DataItem ).Row[GRID_PROCODE_IDNO_LIST.ENGINE_SYUBETSU.bindField].ToString();
                            ( (Label)e.Row.Cells[index].Controls[0] ).Text = OrderBusiness.convertToDisplayEngineSyubetsu( syubetsu );

                        }
                        if ( GetColumnIndex( sender, GRID_PROCODE_IDNO_LIST.TOUSAI_OEM, out index ) == true ) {
                            // 搭載/OEMの場合、該当する文字列に変換する (該当なしの場合は"")
                            var tousaiOem = ( (DataRowView)e.Row.DataItem ).Row[GRID_PROCODE_IDNO_LIST.TOUSAI_OEM.bindField].ToString();
                            ( (Label)e.Row.Cells[index].Controls[0] ).Text = OrderBusiness.convertToDisplayTousaiOem( tousaiOem );
                        }
                        if ( GetColumnIndex( sender, GRID_PROCODE_IDNO_LIST.NAIGAISKAU, out index ) == true ) {
                            // 内外作の場合、該当する文字列に変換する (該当なしの場合は"")
                            var naigaisaku = ( (DataRowView)e.Row.DataItem ).Row[GRID_PROCODE_IDNO_LIST.NAIGAISKAU.bindField].ToString();
                            ( (Label)e.Row.Cells[index].Controls[0] ).Text = OrderBusiness.convertToDisplayNaigaisaku( naigaisaku );
                        }
                        if ( GetColumnIndex( sender, GRID_PROCODE_IDNO_LIST.UNTENFLAG, out index ) == true ) {
                            // 運転の場合、該当する文字列に変換する (該当なしの場合は"")
                            var unten = ( (DataRowView)e.Row.DataItem ).Row[GRID_PROCODE_IDNO_LIST.UNTENFLAG.bindField].ToString();
                            ( (Label)e.Row.Cells[index].Controls[0] ).Text = OrderBusiness.convertToDisplayUnten( unten );
                        }
                        if ( GetColumnIndex( sender, GRID_PROCODE_IDNO_LIST.INSTOCK_DATE, out index ) == true ) {
                            // 入庫日時の場合、日付チェックをする
                            // 2001 /01/01より古いならnullを設定する
                            var instockDt = ( (DataRowView)e.Row.DataItem ).Row[GRID_PROCODE_IDNO_LIST.INSTOCK_DATE.bindField].ToString();
                            DateTime dt;
                            if ( DateTime.TryParse( instockDt, out dt ) ) {
                                if ( true == OrderBusiness.checkOlderBaseDate( instockDt, BASE_DATA_STR ) ) {
                                    ( (Label)e.Row.Cells[index].Controls[0] ).Text = null;
                                }
                            } else {
                                ( (Label)e.Row.Cells[index].Controls[0] ).Text = null;
                            }
                        }
                    }
                }

                // 選択行の背景色変更を追加
                ControlUtils.GridViewRowBound( sender, e, GRID_ORDER_PROCODE_IDNO_DIALOG_GROUP_CD, ControlUtils.GridRowDoubleClickEvent.None );
            }
        }

        #region グリッドビュー操作
        /// <summary>
        /// グリッドビューページ切替
        /// </summary>
        /// <param name="parameters"></param>
        private void DoChangePageIndex( params object[] parameters ) {
            object sender = parameters[0];
            CommandEventArgs e = (CommandEventArgs)parameters[1];
            int newPageIndex = Convert.ToInt32( e.CommandArgument );
            int pageSize = gvProCodeIdnoDialogBody.PageSize;
            int rowCount = 0;
            if ( true == ObjectUtils.IsNotNull( ConditionInfo.ResultData ) ) {
                rowCount = ConditionInfo.ResultData.Rows.Count;
            }
            int allPages = 0;
            allPages = ConditionInfo.ResultData.Rows.Count / pageSize;
            if ( 0 != rowCount % pageSize ) {
                allPages += 1;
            }
            // ページが無くなっている場合には、先頭ページへ戻す
            if ( newPageIndex >= allPages ) {
                newPageIndex = 0;
            }
            ControlUtils.ShowGridViewHeader( gvProCodeIdnoDialogHeader, gridviewDefault, ConditionInfo, true );
            ControlUtils.BindGridView_WithTempField( gvProCodeIdnoDialogBody, gridviewDefault, ConditionInfo.ResultData );
            ControlUtils.GridViewPageIndexChanging( gvProCodeIdnoDialogBody, ConditionInfo.ResultData, new GridViewPageEventArgs( newPageIndex ) );
            ControlUtils.SetGridViewPager( ref pnlPager, gvProCodeIdnoDialogBody, gvProCodeIdnoDialogBody_PageIndexChanging, ConditionInfo.ResultData.Rows.Count, gvProCodeIdnoDialogBody.PageIndex );
        }
        #endregion

        /// <summary>
        /// 列番号取得
        /// </summary>
        /// <param name="target">確認対象のグリッドビュー</param>
        /// <param name="def">確認する列定義</param>gvSearchEngineStock
        /// <param name="index">列番号</param>
        /// <returns>列定義がグリッドビューに含まれている場合はtrue、そうでなければfalse</returns>
        private bool GetColumnIndex( GridView target, GridViewDefine def, out int index ) {
            // 列番号を初期化
            index = 0;
            foreach ( DataControlField c in target.Columns ) {
                // グリッドビューの列を順次取得する
                if ( c.HeaderText == def.headerText ) {
                    // グリッドビューの列のヘッダーテキストと列定義のヘッダーテキストが一致した場合、列が存在するとする
                    return true;
                }
                // 列番号を加算する
                index++;
            }
            // すべての列を確認し、存在しなかった場合列が存在しなかったとする
            return false;
        }
    }
}