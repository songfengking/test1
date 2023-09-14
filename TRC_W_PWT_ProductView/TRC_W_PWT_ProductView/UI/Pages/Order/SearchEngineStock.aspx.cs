////////////////////////////////////////////////////////////////////////////////////////////////
//      クボタ筑波工場  TRC
//      概要：トレーサビリティシステム エンジン立体倉庫在庫検索
//---------------------------------------------------------------------------
//           Ver 1.44.0.0  :  2021/06/08  豊島  新規作成(java版からリプレース)
//           Ver 1.44.0.1  :  2021/09/28  豊島  表示件数の変更
//           Ver 1.44.0.3  :  2021/10/06  星野  Excel出力を複数シート対応、メソッドの記述位置調整
//           Ver 1.44.1.1  :  2021/10/25  豊島　RowDataBoundのログ出力抑制
////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KTFramework.C1Common.Excel;
using KTFramework.Common;
using KTFramework.Dao;
using TRC_W_PWT_ProductView.Business;
using TRC_W_PWT_ProductView.Common;
using TRC_W_PWT_ProductView.Dao.Com;
using TRC_W_PWT_ProductView.Defines;
using TRC_W_PWT_ProductView.Defines.TypeDefine;
using TRC_W_PWT_ProductView.Session;
using TRC_W_PWT_ProductView.UI.Base;

namespace TRC_W_PWT_ProductView.UI.Pages.Order {
    /// <summary>
    /// エンジン立体倉庫在庫検索画面
    /// </summary>
    public partial class SearchEngineStock : BaseForm {
        #region 子画面から取得する情報
        /// <summary>立体倉庫</summary>
        public static string RITTAI_NUM = "";
        /// <summary>禁止棚フラグ</summary>
        public static string STOP_FLAG = "";
        /// <summary>ロケーションフラグ</summary>
        public static string LOCATION_FLAG = "";
        /// <summary>エンジン種別</summary>
        public static string ENGINE_SYUBETSU = "";
        /// <summary>搭載OEM</summary>
        public static string TOUSAI_OEM = "";
        /// <summary>内外作</summary>
        public static string NAIGAISAKU = "";
        /// <summary>運転フラグ</summary>
        public static string UNTEN_FLAG = "";
        /// <summary>IDNO</summary>
        public static string IDNO = "";
        /// <summary>機番</summary>
        public static string KIBAN = "";
        /// <summary>特記事項</summary>
        public static string TOKKI = "";
        #endregion

        #region 定数
        #region 固定値、文字列
        /// <summary>メッセージ(Key)</summary>
        const string MSG_KEY = "MSG";
        /// <summary>基準日</summary>
        private const string BASE_DATA_STR = "2001/01/01";
        /// <summary>台数</summary>
        private const string QNT = "台数";
        /// <summary>コロン</summary>
        private const string COLON = "：";
        /// <summary>台(全角スペースは文字間隔)</summary>
        private const string DAI = "台　";
        /// <summary>合計</summary>
        private const string TOTAL = "合計";
        /// <summary>初期化時台数</summary>
        private const string INIT_QNT = "0";
        #endregion

        #region 検索対象
        public enum SearchTarget {
            // 型式別
            KATASHIKIBETSU = 1,
            // 全在庫
            ZENZAIKO = 2,
        }
        #endregion

        #region 立体倉庫
        /// <summary>立体倉庫</summary>
        private const string RITTAI_STOCK = "立体倉庫";
        /// <summary>全て(表示用)</summary>
        private const string RITTAI_ALL_DISP = "全て";
        /// <summary>全て</summary>
        private const string RITTAI_ALL = "";
        /// <summary>筑波(表示用)</summary>
        private const string RITTAI_TSUKUBA_DISP = "筑波";
        /// <summary>筑波</summary>
        private const string RITTAI_TSUKUBA = "1";
        /// <summary>OEM(表示用)</summary>
        private const string RITTAI_OEM_DISP = "OEM";
        /// <summary>OEM</summary>
        private const string RITTAI_OEM = "2";
        /// <summary>堺(表示用)</summary>
        private const string RITTAI_SAKAI_DISP = "堺";
        /// <summary>堺</summary>
        private const string RITTAI_SAKAI = "3";
        /// <summary>塗装後(表示用)</summary>
        private const string RITTAI_TOSOUGO_DISP = "塗装後";
        /// <summary>塗装後summary>
        private const string RITTAI_TOSOUGO = "4";
        #endregion

        #region 棚
        /// <summary>全て(表示用)</summary>
        private const string RITTAI_STOP_ALL_DISP = "全て";
        /// <summary>全て</summary>
        private const string RITTAI_STOP_ALL = "";
        /// <summary>通常(表示用)</summary>
        private const string RITTAI_STOP_NORMAL_DISP = "通常";
        /// <summary>通常</summary>
        private const string RITTAI_STOP_NORMAL = "0";
        /// <summary>禁止棚(表示用)</summary>
        private const string RITTAI_STOP_STOP_DISP = "禁止棚";
        /// <summary>禁止棚</summary>
        private const string RITTAI_STOP_STOP = "1";
        #endregion

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

        #region 種別
        /// <summary>全て(表示用)</summary>
        private const string ENGINE_SYUBETSU_ALL_DISP = "全て";
        /// <summary>全て</summary>
        private const string ENGINE_SYUBETSU_ALL = "";
        /// <summary>03(表示用)</summary>
        private const string ENGINE_SYUBETSU_03_DISP = "03";
        /// <summary>03</summary>
        private const string ENGINE_SYUBETSU_03 = "1";
        /// <summary>07(表示用)</summary>
        private const string ENGINE_SYUBETSU_07_DISP = "07";
        /// <summary>07</summary>
        private const string ENGINE_SYUBETSU_07 = "2";
        #endregion

        #region エンジン生産種別
        /// <summary>エンジン生産種別</summary>
        private const string ENGINE_PROD_SYUBETSU = "エンジン生産種別";
        /// <summary>03搭載(表示用)</summary>
        private const string ENGINE_PROD_03_TOUSAI_DISP = "03搭載";
        /// <summary>03搭載</summary>
        private const string ENGINE_PROD_03_TOUSAI = "1";
        /// <summary>07搭載(表示用)</summary>
        private const string ENGINE_PROD_07_TOUSAI_DISP = "07搭載";
        /// <summary>07搭載</summary>
        private const string ENGINE_PROD_07_TOUSAI = "2";
        /// <summary>03OEM(表示用)</summary>
        private const string ENGINE_PROD_03_OEM_DISP = "03OEM";
        /// <summary>03OEM</summary>
        private const string ENGINE_PROD_03_OEM = "3";
        /// <summary>07OEM(表示用)</summary>
        private const string ENGINE_PROD_07_OEM_DISP = "07OEM";
        /// <summary>07OEM</summary>
        private const string ENGINE_PROD_07_OEM = "4";
        /// <summary>堺(表示用)</summary>
        private const string ENGINE_PROD_SAKAI_DISP = "堺";
        /// <summary>堺</summary>
        private const string ENGINE_PROD_SAKAI = "5";
        #endregion

        #region 搭載/OEM
        /// <summary>全て(表示用)</summary>
        private const string ENGINE_TOUSAIOEM_ALL_DISP = "全て";
        /// <summary>全て</summary>
        private const string ENGINE_TOUSAIOEM_ALL = "";
        /// <summary>搭載(表示用)</summary>
        private const string ENGINE_TOUSAIOEM_TOUSAI_DISP = "搭載";
        /// <summary>搭載</summary>
        private const string ENGINE_TOUSAIOEM_TOUSAI = "1";
        /// <summary>OEM(表示用)</summary>
        private const string ENGINE_TOUSAIOEM_OEM_DISP = "OEM";
        /// <summary>OEM</summary>
        private const string ENGINE_TOUSAIOEM_OEM = "2";
        #endregion

        #region 内外作
        /// <summary>全て(表示用)</summary>
        private const string ENGINE_NAIGAISAKU_ALL_DISP = "全て";
        /// <summary>全て</summary>
        private const string ENGINE_NAIGAISAKU_ALL = "";
        /// <summary>内作(表示用)</summary>
        private const string ENGINE_NAIGAISAKU_NAISAKU_DISP = "内作";
        /// <summary>内作</summary>
        private const string ENGINE_NAIGAISAKU_NAISAKU = "0";
        /// <summary>堺(表示用)</summary>
        private const string ENGINE_NAIGAISAKU_SAKAI_DISP = "堺";
        /// <summary>堺</summary>
        private const string ENGINE_NAIGAISAKU_SAKAI = "1";
        #endregion

        #region 運転
        /// <summary>全て(表示用)</summary>
        private const string ENGINE_UNTEN_ALL_DISP = "全て";
        /// <summary>全て</summary>
        private const string ENGINE_UNTEN_ALL = "";
        /// <summary>運転前(表示用)</summary>
        private const string ENGINE_UNTEN_BEFORE_DISP = "運転前";
        /// <summary>運転前</summary>
        private const string ENGINE_UNTEN_BEFORE = "0";
        /// <summary>完了(表示用)</summary>
        private const string ENGINE_UNTEN_AFTER_DISP = "完了";
        /// <summary>完了</summary>
        private const string ENGINE_UNTEN_AFTER = "1";
        #endregion

        #region 検索条件
        /// <summary>
        /// 検索条件
        /// </summary>
        public class CONDITION {
            /// <summary>
            /// 立体倉庫
            /// </summary>
            public static readonly ControlDefine RITTAI_NUM = new ControlDefine( "ddlRittai", "立体倉庫", "ddlRittai", ControlDefine.BindType.Both, typeof( string ) );
            /// <summary>
            /// 棚
            /// </summary>
            public static readonly ControlDefine STOP_FLAG = new ControlDefine( "ddlStopFlag", "棚", "ddlStopFlag", ControlDefine.BindType.Both, typeof( string ) );
            /// <summary>
            /// 状態
            /// </summary>
            public static readonly ControlDefine LOCATION_FLAG = new ControlDefine( "ddlLocationFlag", "状態", "ddlLocationFlag", ControlDefine.BindType.Both, typeof( string ) );
            /// <summary>
            /// 種別
            /// </summary>
            public static readonly ControlDefine ENGINE_SYUBETSU = new ControlDefine( "ddlEngineSyubetsu", "種別", "ddlEngineSyubetsu", ControlDefine.BindType.Both, typeof( string ) );
            /// <summary>
            /// 搭載/OEM
            /// </summary>
            public static readonly ControlDefine TOUSAI_OEM = new ControlDefine( "ddlTousaiOem", "搭載/OEM", "ddlTousaiOem", ControlDefine.BindType.Both, typeof( string ) );
            /// <summary>
            /// 内外作
            /// </summary>
            public static readonly ControlDefine NAIGAISAKU = new ControlDefine( "ddlNaigaisaku", "内外作", "ddlNaigaisaku", ControlDefine.BindType.Both, typeof( string ) );
            /// <summary>
            /// 運転
            /// </summary>
            public static readonly ControlDefine UNTEN_FLAG = new ControlDefine( "ddlUntenFlag", "運転", "ddlUntenFlag", ControlDefine.BindType.Both, typeof( string ) );
            /// <summary>
            /// IDNO
            /// </summary>
            public static readonly ControlDefine IDNO = new ControlDefine( "txtIdno", "IDNO", "txtIdno", ControlDefine.BindType.Both, typeof( string ) );
            /// <summary>
            /// 機番
            /// </summary>
            public static readonly ControlDefine KIBAN = new ControlDefine( "txtKiban", "機番", "txtKiban", ControlDefine.BindType.Both, typeof( string ) );
            /// <summary>
            /// 型式コード
            /// </summary>
            public static readonly ControlDefine KATASHIKI_CODE = new ControlDefine( "txtKatashikiCd", "型式コード", "txtKatashikiCd", ControlDefine.BindType.Both, typeof( string ) );
            /// <summary>
            /// 型式名
            /// </summary>
            public static readonly ControlDefine KATASHIKI_NAME = new ControlDefine( "txtKatashikiNm", "型式名", "txtKatashikiNm", ControlDefine.BindType.Both, typeof( string ) );
            /// <summary>
            /// 特記事項
            /// </summary>
            public static readonly ControlDefine TOKKI = new ControlDefine( "txtTokki", "特記事項", "txtTokki", ControlDefine.BindType.Both, typeof( string ) );
        }
        #endregion

        #region グリッドビュー定義
        #region 型式別在庫一覧用グリッドビュー定義
        /// <summary>
        /// グリッドビュー定義(検索対象1の場合：固定列)
        /// </summary>
        public class GRID_SEARCHENGINESTOCK_FOR_SEARCH_TARGET_1_L {
            /// <summary>
            /// No
            /// </summary>
            public static readonly GridViewDefine DISP_ORDER = new GridViewDefine( "No", "DISP_ORDER", typeof( string ), "", false, HorizontalAlign.Right, 40, true, true );
        }
        /// <summary>
        /// グリッドビュー定義(検索対象1の場合：可変列)
        /// </summary>
        public class GRID_SEARCHENGINESTOCK_FOR_SEARCH_TARGET_1_R {
            /// <summary>
            /// 型式コード
            /// </summary>
            public static readonly GridViewDefine KATASHIKI_CODE = new GridViewDefine( "型式コード", "型式コード", typeof( string ), "", true, HorizontalAlign.Center, 110, true, true );
            /// <summary>
            /// 国コード
            /// </summary>
            public static readonly GridViewDefine KUNI_CODE = new GridViewDefine( "国コード", "国コード", typeof( string ), "", true, HorizontalAlign.Center, 100, true, true );
            /// <summary>
            /// 型式名
            /// </summary>
            public static readonly GridViewDefine KATASHIKI_NAME = new GridViewDefine( "型式名", "型式名", typeof( string ), "", true, HorizontalAlign.Left, 160, true, true );
            /// <summary>
            /// GP
            /// </summary>
            public static readonly GridViewDefine KUMITATE_PATTERN = new GridViewDefine( "GP", "パターンコード", typeof( string ), "", true, HorizontalAlign.Center, 50, true, true );
            /// <summary>
            /// 種別
            /// </summary>
            public static readonly GridViewDefine ENGINE_SYUBETSU = new GridViewDefine( "種別", "エンジン種別", typeof( string ), "", true, HorizontalAlign.Center, 60, true, true );
            /// <summary>
            /// 搭載/OEM
            /// </summary>
            public static readonly GridViewDefine TOUSAI_OEM = new GridViewDefine( "搭載/OEM", "搭載OEM", typeof( string ), "", true, HorizontalAlign.Center, 100, true, true );
            /// <summary>
            /// 内外作
            /// </summary>
            public static readonly GridViewDefine NAIGAISKAU = new GridViewDefine( "内外作", "内外作", typeof( string ), "", true, HorizontalAlign.Center, 80, true, true );
            /// <summary>
            /// 台数
            /// </summary>
            public static readonly GridViewDefine DAISU = new GridViewDefine( "台数", "台数", typeof( string ), "", true, HorizontalAlign.Center, 80, true, true );
        }
        #endregion

        #region ロケーション一覧用グリッドビュー定義
        /// <summary>
        /// グリッドビュー定義(検索対象2の場合：固定列)
        /// </summary>
        public class GRID_SEARCHENGINESTOCK_FOR_SEARCH_TARGET_2_L {
            /// <summary>
            /// No
            /// </summary>
            public static readonly GridViewDefine DISP_ORDER = new GridViewDefine( "No", "DISP_ORDER", typeof( string ), "", false, HorizontalAlign.Right, 40, true, true );
            /// <summary>
            /// 立体倉庫
            /// </summary>
            public static readonly GridViewDefine RITTAI_NAME = new GridViewDefine( "立体倉庫", "立体倉庫", typeof( string ), "", true, HorizontalAlign.Center, 100, true, true );
            /// <summary>
            /// 棚番
            /// </summary>
            public static readonly GridViewDefine LOCATION_NAME = new GridViewDefine( "棚番", "棚番", typeof( string ), "", true, HorizontalAlign.Center, 100, true, true );
            /// <summary>
            /// 号機
            /// </summary>
            public static readonly GridViewDefine STOCK_KBN = new GridViewDefine( "号機", "号機", typeof( string ), "", true, HorizontalAlign.Center, 100, false, true );
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
            public static readonly GridViewDefine STOP_FLAG = new GridViewDefine( "禁止棚", "禁止棚フラグ", typeof( string ), "", true, HorizontalAlign.Center, 100, true, true );
            /// <summary>
            /// 状態
            /// </summary>
            public static readonly GridViewDefine LOCATION_FLAG = new GridViewDefine( "状態", "ロケーションフラグ", typeof( string ), "", true, HorizontalAlign.Center, 100, true, true );
        }

        /// <summary>
        /// グリッドビュー定義(検索対象2の場合：可変列)
        /// </summary>
        public class GRID_SEARCHENGINESTOCK_FOR_SEARCH_TARGET_2_R {
            /// <summary>
            /// 型式コード
            /// </summary>
            public static readonly GridViewDefine KATASHIKI_CODE = new GridViewDefine( "型式コード", "型式コード", typeof( string ), "", true, HorizontalAlign.Center, 110, true, true );
            /// <summary>
            /// 国コード
            /// </summary>
            public static readonly GridViewDefine KUNI_CODE = new GridViewDefine( "国コード", "国コード", typeof( string ), "", true, HorizontalAlign.Center, 100, true, true );
            /// <summary>
            /// 型式名
            /// </summary>
            public static readonly GridViewDefine KATASHIKI_NAME = new GridViewDefine( "型式名", "型式名", typeof( string ), "", true, HorizontalAlign.Left, 160, true, true );
            /// <summary>
            /// GP
            /// </summary>
            public static readonly GridViewDefine KUMITATE_PATTERN = new GridViewDefine( "GP", "パターンコード", typeof( string ), "", true, HorizontalAlign.Center, 50, true, true );
            /// <summary>
            /// 種別
            /// </summary>
            public static readonly GridViewDefine ENGINE_SYUBETSU = new GridViewDefine( "種別", "エンジン種別", typeof( string ), "", true, HorizontalAlign.Center, 60, true, true );
            /// <summary>
            /// 搭載/OEM
            /// </summary>
            public static readonly GridViewDefine TOUSAI_OEM = new GridViewDefine( "搭載/OEM", "搭載OEM", typeof( string ), "", true, HorizontalAlign.Center, 100, true, true );
            /// <summary>
            /// 内外作
            /// </summary>
            public static readonly GridViewDefine NAIGAISKAU = new GridViewDefine( "内外作", "内外作", typeof( string ), "", true, HorizontalAlign.Center, 80, true, true );
            /// <summary>
            /// IDNO
            /// </summary>
            public static readonly GridViewDefine IDNO = new GridViewDefine( "IDNO", "IDNO", typeof( string ), "", true, HorizontalAlign.Center, 100, true, true );
            /// <summary>
            /// 機番
            /// </summary>
            public static readonly GridViewDefine KIBAN = new GridViewDefine( "機番", "機番", typeof( string ), "", true, HorizontalAlign.Center, 100, true, true );
            /// <summary>
            /// 特記
            /// </summary>
            public static readonly GridViewDefine TOKKI = new GridViewDefine( "特記", "特記事項", typeof( string ), "", true, HorizontalAlign.Center, 100, true, true );
            /// <summary>
            /// 運転
            /// </summary>
            public static readonly GridViewDefine UNTENFLAG = new GridViewDefine( "運転", "運転フラグ", typeof( string ), "", true, HorizontalAlign.Center, 100, true, true );
            /// <summary>
            /// 入庫日時
            /// </summary>
            public static readonly GridViewDefine INSTOCK_DATE = new GridViewDefine( "入庫日時", "入庫日時", typeof( string ), "{0:" + "MM/dd HH:mm" + "}", true, HorizontalAlign.Center, 100, true, true );
        }
        #endregion

        /// <summary>
        /// グリッドビューグループ
        /// </summary>
        const string GRID_ORDER_SEARCH_ENGINE_STOCK_GROUP_CD = "SearchEngineStock";
        #endregion
        #endregion

        #region プロパティ
        /// <summary>
        /// ロガー
        /// </summary>
        private static readonly Logger Logger = new Logger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

        /// <summary>
        /// 表示中ページ(ベースフォーム)情報
        /// </summary>
        private BaseForm CurrentForm
        {
            get
            {
                return ( (BaseForm)Page );
            }
        }

        /// <summary>
        /// 検索条件定義情報
        /// </summary>
        ControlDefine[] _conditionControls = null;
        /// <summary>
        /// 検索条件定義情報アクセサ
        /// </summary>
        ControlDefine[] ConditionControls
        {
            get
            {
                if ( true == ObjectUtils.IsNull( _conditionControls ) ) {
                    _conditionControls = ControlUtils.GetControlDefineArray( typeof( CONDITION ) );
                }
                return _conditionControls;
            }
        }

        /// <summary>
        /// 一覧定義情報(固定列)
        /// </summary>
        GridViewDefine[] _gridviewDefault_l = null;
        /// <summary>
        /// 一覧定義情報アクセサ(固定列)
        /// </summary>
        GridViewDefine[] GridviewDefault_L
        {
            get
            {
                if ( hdnSearchTarget.Value == SearchTarget.KATASHIKIBETSU.ToString() ) {
                    // 検索対象が1の場合、定義1(固定列)から作成
                    _gridviewDefault_l = ControlUtils.GetGridViewDefineArray( typeof( GRID_SEARCHENGINESTOCK_FOR_SEARCH_TARGET_1_L ) );
                } else {
                    // 検索対象が2の場合、定義2(固定列)から作成
                    _gridviewDefault_l = ControlUtils.GetGridViewDefineArray( typeof( GRID_SEARCHENGINESTOCK_FOR_SEARCH_TARGET_2_L ) );
                }
                return _gridviewDefault_l;
            }
        }
        /// <summary>
        /// 一覧定義情報(可変列)
        /// </summary>
        GridViewDefine[] _gridviewDefault_r = null;
        /// <summary>
        /// 一覧定義情報アクセサ(可変列)
        /// </summary>
        GridViewDefine[] GridviewDefault_R
        {
            get
            {
                if ( hdnSearchTarget.Value == SearchTarget.KATASHIKIBETSU.ToString() ) {
                    // 検索対象が1の場合、定義1(可変列)から作成
                    _gridviewDefault_r = ControlUtils.GetGridViewDefineArray( typeof( GRID_SEARCHENGINESTOCK_FOR_SEARCH_TARGET_1_R ) );
                } else {
                    // 検索対象が2の場合、定義2(可変列)から作成
                    _gridviewDefault_r = ControlUtils.GetGridViewDefineArray( typeof( GRID_SEARCHENGINESTOCK_FOR_SEARCH_TARGET_2_R ) );
                }
                return _gridviewDefault_r;
            }
        }
        #endregion

        #region イベント
        /// <summary>
        /// 画面ロード時イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load( object sender, EventArgs e ) {
            base.RaiseEvent( DoPageLoad, false );
        }

        /// <summary>
        /// 型式別検索ボタン押下時イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnKatashikibetsu_Click( object sender, EventArgs e ) {
            // 隠し項目に検索対象(1)を設定
            hdnSearchTarget.Value = SearchTarget.KATASHIKIBETSU.ToString();
            // gvSearchEngineStockLBの作り直し
            gvSearchEngineStockLB.Columns.Clear();
            for ( int idx = 0; idx < GridviewDefault_L.Length; idx++ ) {
                TemplateField tf = new TemplateField();
                tf.HeaderText = StringUtils.ToString( GridviewDefault_L[idx].bindField );
                gvSearchEngineStockLB.Columns.Add( tf );
            }
            // gvSearchEngineStockRBの作り直し
            gvSearchEngineStockRB.Columns.Clear();
            for ( int idx = 0; idx < GridviewDefault_R.Length; idx++ ) {
                TemplateField tf = new TemplateField();
                tf.HeaderText = StringUtils.ToString( GridviewDefault_R[idx].bindField );
                gvSearchEngineStockRB.Columns.Add( tf );
            }
            // 検索呼び出し
            base.RaiseEvent( DoSearch );
        }

        /// <summary>
        /// 明細ボタン押下時イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnMeisai_Click( object sender, EventArgs e ) {
            // 隠し項目に検索対象(2)を設定
            hdnSearchTarget.Value = SearchTarget.ZENZAIKO.ToString();
            // gvSearchEngineStockLBの作り直し
            gvSearchEngineStockLB.Columns.Clear();
            for ( int idx = 0; idx < GridviewDefault_L.Length; idx++ ) {
                TemplateField tf = new TemplateField();
                tf.HeaderText = StringUtils.ToString( GridviewDefault_L[idx].bindField );
                gvSearchEngineStockLB.Columns.Add( tf );
            }
            // gvSearchEngineStockRBの作り直し
            gvSearchEngineStockRB.Columns.Clear();
            for ( int idx = 0; idx < GridviewDefault_R.Length; idx++ ) {
                TemplateField tf = new TemplateField();
                tf.HeaderText = StringUtils.ToString( GridviewDefault_R[idx].bindField );
                gvSearchEngineStockRB.Columns.Add( tf );
            }
            // 検索呼び出し
            base.RaiseEvent( DoSearch );
        }

        /// <summary>
        /// Excel出力ボタン押下時イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExcel_Click( object sender, EventArgs e ) {
            base.RaiseEvent( OutputExcel );
        }

        #region グリッドビュー操作イベント
        /// <summary>
        /// グリッドビュー並び替えイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvSearchEngineStock_Sorting( object sender, GridViewSortEventArgs e ) {
            base.RaiseEvent( Sorting, sender, e );
            // メッセージの復元
            RestoreMsg();
        }

        /// <summary>
        /// グリッドビューページチェンジイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvSearchEngineStock_PageIndexChanging( object sender, CommandEventArgs e ) {
            base.RaiseEvent( PageIndexChanging, sender, e );
            // メッセージの復元
            RestoreMsg();
        }

        /// <summary>
        /// 一覧データバインドイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvSearchEngineStock_RowDataBound( object sender, GridViewRowEventArgs e ) {
            if ( hdnSearchTarget.Value == SearchTarget.KATASHIKIBETSU.ToString() ) {
                // 検索対象が型式別の場合、型式別用一覧行データバインドを呼び出す
                try {
                    ClearApplicationMessage();
                    RowDataBoundForKatashikibetsu( sender, e );
                } catch ( Exception ex ) {
                    //イベント処理中にエラー発生
                    Logger.Exception( ex );
                    throw ex;
                }
            } else if ( hdnSearchTarget.Value == SearchTarget.ZENZAIKO.ToString() ) {
                // 検索対象が全在庫の場合、全在庫・ダイアログ用一覧行データバインドを呼び出す
                try {
                    ClearApplicationMessage();
                    RowDataBoundForZenzaiko( sender, e );
                } catch ( Exception ex ) {
                    //イベント処理中にエラー発生
                    Logger.Exception( ex );
                    throw ex;
                }
            }
        }
        #endregion
        #endregion

        #region ページ処理
        /// <summary>
        /// ページロード処理
        /// </summary>
        protected override void DoPageLoad() {
            // グリッドビューの再表示
            ControlUtils.SetGridViewTemplateField( gvSearchEngineStockLB, GridviewDefault_L );
            ControlUtils.SetGridViewTemplateField( gvSearchEngineStockRB, GridviewDefault_R );
            ControlUtils.ReMakeBoundTemplateFieldCtrl( gvSearchEngineStockLB, GridviewDefault_L );
            ControlUtils.ReMakeBoundTemplateFieldCtrl( gvSearchEngineStockRB, GridviewDefault_R );
            // ベースページロード処理
            base.DoPageLoad();
            int resultCnt = 0;
            if ( ObjectUtils.IsNotNull( ConditionInfo.ResultData ) ) {
                resultCnt = ConditionInfo.ResultData.Rows.Count;
            }
            ControlUtils.SetGridViewPager( ref pnlPager, gvSearchEngineStockRB, gvSearchEngineStock_PageIndexChanging, resultCnt, gvSearchEngineStockRB.PageIndex );
        }

        /// <summary>
        /// 初期化処理
        /// </summary>
        protected override void Initialize() {
            // アクセスカウンター登録
            Dao.Com.AccessCounterDao.Entry( base.CurrentPageInfo.pageId );
            // ベース処理初期化処理
            base.Initialize();
            // セッションをクリアする
            ConditionInfo = new ConditionInfoSessionHandler.ST_CONDITION();
            // 初期処理
            InitializeValues();
        }

        /// <summary>
        /// メッセージの復元
        /// </summary>
        private void RestoreMsg() {
            var msg = CurrentForm.SessionManager.GetPageControlInfoHandler( CurrentForm.Token ).GetPageControlInfo( MSG_KEY );
            if ( 0 < msg.Count ) {
                // メッセージが設定されていた場合、メッセージ表示
                base.WriteApplicationMessage( (Msg)msg[MSG_KEY] );
            }
        }
        #endregion

        #region 機能別処理
        #region 初期処理
        /// <summary>
        /// 初期処理
        /// </summary>
        private void InitializeValues() {
            // SEARCH_TARGET = 型式別(1)
            hdnSearchTarget.Value = SearchTarget.KATASHIKIBETSU.ToString();

            // 検索条件：立体倉庫リストセット
            ddlRittai.Items.Add( RITTAI_ALL_DISP );
            ddlRittai.Items.Add( RITTAI_TSUKUBA_DISP );
            ddlRittai.Items.Add( RITTAI_OEM_DISP );
            ddlRittai.Items.Add( RITTAI_SAKAI_DISP );
            ddlRittai.Items.Add( RITTAI_TOSOUGO_DISP );

            // 検索条件：棚リストセット
            ddlStopFlag.Items.Add( RITTAI_STOP_ALL_DISP );
            ddlStopFlag.Items.Add( RITTAI_STOP_NORMAL_DISP );
            ddlStopFlag.Items.Add( RITTAI_STOP_STOP_DISP );

            // 検索条件：状態のリストに以下の値をセット
            ddlLocationFlag.Items.Add( LOCATION_FLAG_ALL_DISP );
            ddlLocationFlag.Items.Add( LOCATION_FLAG_NOENGINE_DISP );
            ddlLocationFlag.Items.Add( LOCATION_FLAG_ANYENGINE_DISP );
            ddlLocationFlag.Items.Add( LOCATION_FLAG_STOCKED_DISP );
            ddlLocationFlag.Items.Add( LOCATION_FLAG_ACCEPTED_DISP );
            ddlLocationFlag.Items.Add( LOCATION_FLAG_DELIVERED_DISP );
            ddlLocationFlag.Items.Add( LOCATION_FLAG_PALETTE_DISP );

            // 検索条件：種別のリストに以下の値をセット
            ddlEngineSyubetsu.Items.Add( ENGINE_SYUBETSU_ALL_DISP );
            ddlEngineSyubetsu.Items.Add( ENGINE_SYUBETSU_03_DISP );
            ddlEngineSyubetsu.Items.Add( ENGINE_SYUBETSU_07_DISP );

            // 検索条件：搭載/OEMのリストに以下の値をセット
            ddlTousaiOem.Items.Add( ENGINE_TOUSAIOEM_ALL_DISP );
            ddlTousaiOem.Items.Add( ENGINE_TOUSAIOEM_TOUSAI_DISP );
            ddlTousaiOem.Items.Add( ENGINE_TOUSAIOEM_OEM_DISP );

            // 検索条件：内外作のリストに以下の値をセット
            ddlNaigaisaku.Items.Add( ENGINE_NAIGAISAKU_ALL_DISP );
            ddlNaigaisaku.Items.Add( ENGINE_NAIGAISAKU_NAISAKU_DISP );
            ddlNaigaisaku.Items.Add( ENGINE_NAIGAISAKU_SAKAI_DISP );

            // 検索条件：運転のリストに以下の値をセット
            ddlUntenFlag.Items.Add( ENGINE_UNTEN_ALL_DISP );
            ddlUntenFlag.Items.Add( ENGINE_UNTEN_BEFORE_DISP );
            ddlUntenFlag.Items.Add( ENGINE_UNTEN_AFTER_DISP );

            // 状態 = 在席(1)
            ddlLocationFlag.SelectedValue = LOCATION_FLAG_STOCKED_DISP;
            // 運転 = 完了(1)
            ddlUntenFlag.SelectedValue = ENGINE_UNTEN_AFTER_DISP;

            // 検索処理
            DoSearch();
        }
        #endregion

        #region 検索関連処理
        /// <summary>
        /// 検索処理
        /// </summary>
        private void DoSearch() {
            // 子画面用に検索条件を保持
            RetentionSearchCond();
            // 検索条件を作成する
            Dictionary<string, object> dicCondition = new Dictionary<string, object>();
            base.GetControlValues( ConditionControls, ref dicCondition );
            // 検索時画面情報を取得する
            Dictionary<string, string> dicIdWithText = new Dictionary<string, string>();
            base.GetControlTexts( ConditionControls, out dicIdWithText );
            // 一覧定義情報を作成する
            var gridviewDefineTmp = GridviewDefault_L.Concat( GridviewDefault_R ).ToArray();
            // 検索結果取得
            // エラー時にも最後まで処理を通すため、インスタンスを生成しておく
            var result = new OrderBusiness.ResultSet();
            DataTable tblResult = null;
            // 検索上限数を取得する
            int maxGridViewCount = WebAppInstance.GetInstance().Config.WebCommonInfo.maxGridViewCount;
            try {
                // 処理結果 = 共通処理.検索処理
                result = OrderBusiness.SearchOfSearchEngineStock( ref dicCondition, maxGridViewCount, hdnSearchTarget.Value );
            } catch ( DataAccessException ex ) {
                if ( ex.OracleErrorNumber == DataAccessBase.ORAERROR_TIMEOUT ) {
                    // クエリ発行タイムアウトが発生した場合、エラーメッセージを設定する
                    base.WriteApplicationMessage( MsgManager.MESSAGE_WRN_61910 );
                } else {
                    // タイムアウト以外のExceptionが発生した場合、エラーメッセージを設定する
                    Logger.Exception( ex );
                    base.WriteApplicationMessage( MsgManager.MESSAGE_ERR_80011, "検索処理で" );
                }
                // 在庫数の非表示化
                StockQntNonDisp();
            } catch ( Exception ex ) {
                // Exceptionが発生した場合、エラーメッセージを設定する
                Logger.Exception( ex );
                base.WriteApplicationMessage( MsgManager.MESSAGE_ERR_84010 );
                // 在庫数の非表示化
                StockQntNonDisp();
            } finally {
            }
            ConditionInfoSessionHandler.ST_CONDITION cond = new ConditionInfoSessionHandler.ST_CONDITION();
            tblResult = result.ListTable;
            if ( null != tblResult ) {

                // 在庫数のテキスト設定
                StockQntSetting( result );

                // 列定義から列名のキャプションを設定する
                gridviewDefineTmp.ToList().ForEach( cd => {
                    tblResult.Columns[cd.bindField].Caption = cd.headerText;
                } );

                // 検索結果が存在する場合、件数表示、ページャーの設定を行う
                ntbResultCount.Value = tblResult.Rows.Count;
                ControlUtils.SetGridViewPager( ref pnlPager, gvSearchEngineStockRB, gvSearchEngineStock_PageIndexChanging, tblResult.Rows.Count, 0 );
                // 検索条件/結果インスタンスを保持する
                cond.conditionValue = dicCondition;
                cond.IdWithText = dicIdWithText;
                cond.ResultData = tblResult.DefaultView.ToTable();
            } else {
                // タイムアウト等Exception時には、GridViewクリア
                ClearGridView();
            }
            // 検索条件をセッションに格納する
            ConditionInfo = cond;
            // グリッドビューの表示処理を行う
            if ( true == ObjectUtils.IsNotNull( tblResult ) ) {
                if ( 0 < tblResult.Rows.Count ) {
                    // TemplateFieldの追加
                    grvHeaderLT.Columns.Clear();
                    grvHeaderRT.Columns.Clear();
                    gvSearchEngineStockLB.Columns.Clear();
                    for ( int idx = 0; idx < GridviewDefault_L.Length; idx++ ) {
                        TemplateField tf = new TemplateField();
                        tf.HeaderText = StringUtils.ToString( GridviewDefault_L[idx].bindField );
                        gvSearchEngineStockLB.Columns.Add( tf );
                    }
                    gvSearchEngineStockRB.Columns.Clear();
                    for ( int idx = 0; idx < GridviewDefault_R.Length; idx++ ) {
                        TemplateField tf = new TemplateField();
                        tf.HeaderText = StringUtils.ToString( GridviewDefault_R[idx].bindField );
                        gvSearchEngineStockRB.Columns.Add( tf );
                    }
                    // 新規バインド
                    ControlUtils.ShowGridViewHeader( grvHeaderLT, GridviewDefault_L, ConditionInfo, true );
                    ControlUtils.ShowGridViewHeader( grvHeaderRT, GridviewDefault_R, ConditionInfo, true );
                    ControlUtils.BindGridView_WithTempField( gvSearchEngineStockLB, GridviewDefault_L, tblResult );
                    ControlUtils.BindGridView_WithTempField( gvSearchEngineStockRB, GridviewDefault_R, tblResult );
                    // GridView表示
                    divGrvDisplay.Visible = true;
                    // グリッドビュー外のDivサイズ変更
                    SetDivGridViewWidth();
                } else {
                    ClearGridView();
                }
            }
            //メッセージ表示
            if ( null != result.Message ) {
                // メッセージが存在する場合、メッセージ表示
                base.WriteApplicationMessage( result.Message );

                // メッセージの保存
                Dictionary<string, object> dicMsgInfo = new Dictionary<string, object>();
                dicMsgInfo.Add( MSG_KEY, result.Message );
                CurrentForm.SessionManager.GetPageControlInfoHandler( CurrentForm.Token ).SetPageControlInfo( MSG_KEY, dicMsgInfo );
            }
            //Excel出力ボタン活性
            if ( null != tblResult && 0 < tblResult.Rows.Count ) {
                // 出力対象データありの場合、Excel出力ボタンを表示する
                this.btnExcel.Visible = true;
            } else {
                //出力対象データなしの場合、Excel出力ボタンを非表示にする
                this.btnExcel.Visible = false;
            }
        }

        /// <summary>
        /// 在庫数のテキスト設定
        /// </summary>
        /// <param name="result">検索結果</param>
        private void StockQntSetting( OrderBusiness.ResultSet result ) {

            // 立体別在庫数
            DataTable rittaiZaiko = result.ListTableRittaibetsuZaiko;
            if ( 0 < rittaiZaiko.Rows.Count ) {
                // 立体別在庫数の結果があれば、表示に切り替えて各テキストを設定

                // 各ラベルのテキスト初期化
                this.lblTsukuba.Text = RITTAI_TSUKUBA_DISP + COLON + INIT_QNT + DAI;
                this.lblOem.Text = RITTAI_OEM_DISP + COLON + INIT_QNT + DAI;
                this.lblRittaiSakai.Text = RITTAI_SAKAI_DISP + COLON + INIT_QNT + DAI;
                this.lblTosougo.Text = RITTAI_TOSOUGO_DISP + COLON + INIT_QNT + DAI;
                // 各ラベルの表示
                this.lblRittaibetsuStockQnt.Visible = true;
                this.lblTsukuba.Visible = true;
                this.lblOem.Visible = true;
                this.lblRittaiSakai.Visible = true;
                this.lblTosougo.Visible = true;

                for ( int idx = 0; idx < rittaiZaiko.Rows.Count; idx++ ) {
                    // 各ラベルのテキスト設定(立体倉庫名：在庫数、立体倉庫による色変更)
                    string rittaiStcNum = rittaiZaiko.Rows[idx][RITTAI_STOCK].ToString();
                    if ( rittaiStcNum == RITTAI_TSUKUBA ) {
                        // 立体倉庫No=1なら筑波
                        this.lblTsukuba.Text = RITTAI_TSUKUBA_DISP + COLON + rittaiZaiko.Rows[idx][QNT].ToString() + DAI;
                    } else if ( rittaiStcNum == RITTAI_OEM ) {
                        // 立体倉庫No=2ならOEM
                        this.lblOem.Text = RITTAI_OEM_DISP + COLON + rittaiZaiko.Rows[idx][QNT].ToString() + DAI;
                    } else if ( rittaiStcNum == RITTAI_SAKAI ) {
                        // 立体倉庫No=3なら堺
                        this.lblRittaiSakai.Text = RITTAI_SAKAI_DISP + COLON + rittaiZaiko.Rows[idx][QNT].ToString() + DAI;
                    } else if ( rittaiStcNum == RITTAI_TOSOUGO ) {
                        // 立体倉庫No=4なら塗装後
                        this.lblTosougo.Text = RITTAI_TOSOUGO_DISP + COLON + rittaiZaiko.Rows[idx][QNT].ToString() + DAI;
                    }
                }
            } else {
                // 立体別在庫数の結果がなければ、非表示に切り替える
                this.lblRittaibetsuStockQnt.Visible = false;
                this.lblTsukuba.Visible = false;
                this.lblOem.Visible = false;
                this.lblRittaiSakai.Visible = false;
                this.lblTosougo.Visible = false;
            }

            // 合計台数
            int totalQnt = 0;

            //種類別在庫数
            DataTable syuruibetuZaiko = result.ListTableSyubetsuZaiko;
            if ( 0 < syuruibetuZaiko.Rows.Count ) {
                // 種類別在庫数の結果があれば、表示に切り替えて各テキストを設定

                // 各ラベルのテキスト初期化
                this.lbl03Tousai.Text = ENGINE_PROD_03_TOUSAI_DISP + COLON + INIT_QNT + DAI;
                this.lbl07Tousai.Text = ENGINE_PROD_07_TOUSAI_DISP + COLON + INIT_QNT + DAI;
                this.lbl03Oem.Text = ENGINE_PROD_03_OEM_DISP + COLON + INIT_QNT + DAI;
                this.lbl07Oem.Text = ENGINE_PROD_07_OEM_DISP + COLON + INIT_QNT + DAI;
                this.lblSyuruiSakai.Text = ENGINE_PROD_SAKAI_DISP + COLON + INIT_QNT + DAI;
                this.lblTotalQnt.Text = TOTAL + COLON + INIT_QNT + DAI;
                // 各ラベルの表示
                this.lblSyuruibetsuStockQnt.Visible = true;
                this.lbl03Tousai.Visible = true;
                this.lbl07Tousai.Visible = true;
                this.lbl03Oem.Visible = true;
                this.lbl07Oem.Visible = true;
                this.lblSyuruiSakai.Visible = true;
                this.lblTotalQnt.Visible = true;
                this.lblSup.Visible = true;

                for ( int idx = 0; idx < syuruibetuZaiko.Rows.Count; idx++ ) {
                    // 各ラベルのテキスト設定(エンジン生産種別名：在庫数、エンジン生産種別による色変更)
                    string engProdSyubetsuNum = syuruibetuZaiko.Rows[idx][ENGINE_PROD_SYUBETSU].ToString();
                    // 数値チェック用変数の定義
                    int parseCheck = 0;
                    if ( engProdSyubetsuNum == ENGINE_PROD_03_TOUSAI ) {
                        // エンジン生産種別No=1なら03搭載
                        this.lbl03Tousai.Text = ENGINE_PROD_03_TOUSAI_DISP + COLON + syuruibetuZaiko.Rows[idx][QNT].ToString() + DAI;
                        if ( true == int.TryParse( syuruibetuZaiko.Rows[idx][QNT].ToString(), out parseCheck ) ) {
                            // 数値なら合計台数に加算
                            totalQnt += parseCheck;
                        }
                    } else if ( engProdSyubetsuNum == ENGINE_PROD_07_TOUSAI ) {
                        // エンジン生産種別No=2なら07搭載
                        this.lbl07Tousai.Text = ENGINE_PROD_07_TOUSAI_DISP + COLON + syuruibetuZaiko.Rows[idx][QNT].ToString() + DAI;
                        if ( true == int.TryParse( syuruibetuZaiko.Rows[idx][QNT].ToString(), out parseCheck ) ) {
                            // 数値なら合計台数に加算
                            totalQnt += parseCheck;
                        }
                    } else if ( engProdSyubetsuNum == ENGINE_PROD_03_OEM ) {
                        // エンジン生産種別No=3なら03OEM
                        this.lbl03Oem.Text = ENGINE_PROD_03_OEM_DISP + COLON + syuruibetuZaiko.Rows[idx][QNT].ToString() + DAI;
                        if ( true == int.TryParse( syuruibetuZaiko.Rows[idx][QNT].ToString(), out parseCheck ) ) {
                            // 数値なら合計台数に加算
                            totalQnt += parseCheck;
                        }
                    } else if ( engProdSyubetsuNum == ENGINE_PROD_07_OEM ) {
                        // エンジン生産種別No=4なら07OEM
                        this.lbl07Oem.Text = ENGINE_PROD_07_OEM_DISP + COLON + syuruibetuZaiko.Rows[idx][QNT].ToString() + DAI;
                        if ( true == int.TryParse( syuruibetuZaiko.Rows[idx][QNT].ToString(), out parseCheck ) ) {
                            // 数値なら合計台数に加算
                            totalQnt += parseCheck;
                        }
                    } else if ( engProdSyubetsuNum == ENGINE_PROD_SAKAI ) {
                        // エンジン生産種別No=5なら堺
                        this.lblSyuruiSakai.Text = ENGINE_PROD_SAKAI_DISP + COLON + syuruibetuZaiko.Rows[idx][QNT].ToString() + DAI;
                        if ( true == int.TryParse( syuruibetuZaiko.Rows[idx][QNT].ToString(), out parseCheck ) ) {
                            // 数値なら合計台数に加算
                            totalQnt += parseCheck;
                        }
                    }
                }
            } else {
                // 種類別在庫数の結果がなければ、非表示に切り替える
                this.lblSyuruibetsuStockQnt.Visible = false;
                this.lbl03Tousai.Visible = false;
                this.lbl07Tousai.Visible = false;
                this.lbl03Oem.Visible = false;
                this.lbl07Oem.Visible = false;
                this.lblSyuruiSakai.Visible = false;
                this.lblTotalQnt.Visible = false;
                this.lblSup.Visible = false;
            }

            // 合計台数
            this.lblTotalQnt.Text = TOTAL + COLON + totalQnt + DAI;
        }

        /// <summary>
        /// 在庫数の非表示化
        /// </summary>
        /// <param name="result">検索結果</param>
        private void StockQntNonDisp() {

            // 立体別在庫数の非表示
            this.lblRittaibetsuStockQnt.Visible = false;
            this.lblTsukuba.Visible = false;
            this.lblOem.Visible = false;
            this.lblRittaiSakai.Visible = false;
            this.lblTosougo.Visible = false;

            // 種類別在庫数の非表示
            this.lblSyuruibetsuStockQnt.Visible = false;
            this.lbl03Tousai.Visible = false;
            this.lbl07Tousai.Visible = false;
            this.lbl03Oem.Visible = false;
            this.lbl07Oem.Visible = false;
            this.lblSyuruiSakai.Visible = false;
            this.lblTotalQnt.Visible = false;
            this.lblSup.Visible = false;

        }

        /// <summary>
        /// 子画面用に検索条件を保持
        /// </summary>
        private void RetentionSearchCond() {
            RITTAI_NUM = ddlRittai.SelectedValue.ToString();
            STOP_FLAG = ddlStopFlag.SelectedValue.ToString();
            LOCATION_FLAG = ddlLocationFlag.SelectedValue.ToString();
            ENGINE_SYUBETSU = ddlEngineSyubetsu.SelectedValue.ToString();
            TOUSAI_OEM = ddlTousaiOem.SelectedValue.ToString();
            NAIGAISAKU = ddlNaigaisaku.SelectedValue.ToString();
            UNTEN_FLAG = ddlUntenFlag.SelectedValue.ToString();
            IDNO = txtIdno.Value;
            KIBAN = txtKiban.Value;
            TOKKI = txtTokki.Value;
        }
        #endregion

        #region 行データバインド関連処理
        /// <summary>
        /// 型式別用一覧行データバインド
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RowDataBoundForKatashikibetsu( params object[] parameters ) {
            var sender = (GridView)parameters[0];
            var e = (GridViewRowEventArgs)parameters[1];
            if ( e.Row.RowType == DataControlRowType.DataRow ) {
                // データ行のみバインド処理を行う
                int index = 0;
                if ( GetColumnIndex( sender, GRID_SEARCHENGINESTOCK_FOR_SEARCH_TARGET_1_L.DISP_ORDER, out index ) == true ) {
                    //NOの場合、連番を振る
                    var data = e.Row.RowIndex + 1 + sender.PageIndex * sender.PageSize;
                    ( (Label)e.Row.Cells[index].Controls[0] ).Text = data.ToString();
                }
                if ( GetColumnIndex( sender, GRID_SEARCHENGINESTOCK_FOR_SEARCH_TARGET_1_R.KATASHIKI_CODE, out index ) == true ) {
                    // 型式コードの場合、XXXXX-XXXXX形式に変換する（変換できない場合は空文字）
                    var data = ( (DataRowView)e.Row.DataItem ).Row[GRID_SEARCHENGINESTOCK_FOR_SEARCH_TARGET_1_R.KATASHIKI_CODE.bindField].ToString();
                    ( (Label)e.Row.Cells[index].Controls[0] ).Text = OrderBusiness.getDisplayKatasikiCode( data );
                }
                if ( GetColumnIndex( sender, GRID_SEARCHENGINESTOCK_FOR_SEARCH_TARGET_1_R.ENGINE_SYUBETSU, out index ) == true ) {
                    // 種別の場合、該当する文字列に変換する (該当なしの場合は"")
                    var data = ( (DataRowView)e.Row.DataItem ).Row[GRID_SEARCHENGINESTOCK_FOR_SEARCH_TARGET_1_R.ENGINE_SYUBETSU.bindField].ToString();
                    ( (Label)e.Row.Cells[index].Controls[0] ).Text = OrderBusiness.convertToDisplayEngineSyubetsu( data );
                }
                if ( GetColumnIndex( sender, GRID_SEARCHENGINESTOCK_FOR_SEARCH_TARGET_1_R.TOUSAI_OEM, out index ) == true ) {
                    // 搭載/OEMの場合、該当する文字列に変換する (該当なしの場合は"")
                    var data = ( (DataRowView)e.Row.DataItem ).Row[GRID_SEARCHENGINESTOCK_FOR_SEARCH_TARGET_1_R.TOUSAI_OEM.bindField].ToString();
                    ( (Label)e.Row.Cells[index].Controls[0] ).Text = OrderBusiness.convertToDisplayTousaiOem( data );
                }
                if ( GetColumnIndex( sender, GRID_SEARCHENGINESTOCK_FOR_SEARCH_TARGET_1_R.NAIGAISKAU, out index ) == true ) {
                    // 内外作の場合、該当する文字列に変換する (該当なしの場合は"")
                    var data = ( (DataRowView)e.Row.DataItem ).Row[GRID_SEARCHENGINESTOCK_FOR_SEARCH_TARGET_1_R.NAIGAISKAU.bindField].ToString();
                    ( (Label)e.Row.Cells[index].Controls[0] ).Text = OrderBusiness.convertToDisplayNaigaisaku( data );
                }

                // 選択行の背景色変更を追加
                // ダブルクリックイベントにダイアログ表示を行うスクリプトを追加
                ControlUtils.GridViewRowBound( (GridView)sender, e, GRID_ORDER_SEARCH_ENGINE_STOCK_GROUP_CD, "SearchEngineStock.dialogOpen();" );
            }
        }

        /// <summary>
        /// 全在庫用一覧行データバインド
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RowDataBoundForZenzaiko( params object[] parameters ) {
            var sender = (GridView)parameters[0];
            var e = (GridViewRowEventArgs)parameters[1];
            if ( e.Row.RowType == DataControlRowType.DataRow ) {
                // データ行のみバインド処理を行う
                int index = 0;
                if ( GetColumnIndex( sender, GRID_SEARCHENGINESTOCK_FOR_SEARCH_TARGET_2_L.DISP_ORDER, out index ) == true ) {
                    //NOの場合、連番を振る
                    var data = e.Row.RowIndex + 1 + sender.PageIndex * sender.PageSize;
                    ( (Label)e.Row.Cells[index].Controls[0] ).Text = data.ToString();
                }
                if ( GetColumnIndex( sender, GRID_SEARCHENGINESTOCK_FOR_SEARCH_TARGET_2_L.RITTAI_NAME, out index ) == true ) {
                    // 立体倉庫の場合、該当する文字列に変換する (該当なしの場合は"")
                    var data = ( (DataRowView)e.Row.DataItem ).Row[GRID_SEARCHENGINESTOCK_FOR_SEARCH_TARGET_2_L.RITTAI_NAME.bindField].ToString();
                    ( (Label)e.Row.Cells[index].Controls[0] ).Text = OrderBusiness.convertToDisplayRittai( data );
                }
                if ( GetColumnIndex( sender, GRID_SEARCHENGINESTOCK_FOR_SEARCH_TARGET_2_L.LOCATION_NAME, out index ) == true ) {
                    // 棚番の場合、号機-連-段-列を連結する
                    string locNm = "";
                    // 号機
                    locNm += ( (DataRowView)e.Row.DataItem ).Row[GRID_SEARCHENGINESTOCK_FOR_SEARCH_TARGET_2_L.STOCK_KBN.bindField].ToString() + "-";
                    // 連
                    locNm += ( (DataRowView)e.Row.DataItem ).Row[GRID_SEARCHENGINESTOCK_FOR_SEARCH_TARGET_2_L.STOCK_REN.bindField].ToString() + "-";
                    // 段
                    locNm += ( (DataRowView)e.Row.DataItem ).Row[GRID_SEARCHENGINESTOCK_FOR_SEARCH_TARGET_2_L.STOCK_DAN.bindField].ToString() + "-";
                    // 列
                    locNm += ( (DataRowView)e.Row.DataItem ).Row[GRID_SEARCHENGINESTOCK_FOR_SEARCH_TARGET_2_L.STOCK_RETSU.bindField].ToString();
                    ( (Label)e.Row.Cells[index].Controls[0] ).Text = locNm;
                }
                if ( GetColumnIndex( sender, GRID_SEARCHENGINESTOCK_FOR_SEARCH_TARGET_2_L.STOP_FLAG, out index ) == true ) {
                    // 禁止棚の場合、該当する文字列に変換する (該当なしの場合は"")
                    var data = ( (DataRowView)e.Row.DataItem ).Row[GRID_SEARCHENGINESTOCK_FOR_SEARCH_TARGET_2_L.STOP_FLAG.bindField].ToString();
                    ( (Label)e.Row.Cells[index].Controls[0] ).Text = OrderBusiness.convertToDisplayRittaiStop( data );
                }
                if ( GetColumnIndex( sender, GRID_SEARCHENGINESTOCK_FOR_SEARCH_TARGET_2_L.LOCATION_FLAG, out index ) == true ) {
                    // 状態の場合、該当する文字列に変換する (該当なしの場合は"")
                    var data = ( (DataRowView)e.Row.DataItem ).Row[GRID_SEARCHENGINESTOCK_FOR_SEARCH_TARGET_2_L.LOCATION_FLAG.bindField].ToString();
                    ( (Label)e.Row.Cells[index].Controls[0] ).Text = OrderBusiness.convertToDisplayLocation( data );
                }
                int katashikiCdIndex = 0;
                if ( GetColumnIndex( sender, GRID_SEARCHENGINESTOCK_FOR_SEARCH_TARGET_2_R.KATASHIKI_CODE, out katashikiCdIndex ) == true ) {
                    // 型式コードの場合、固定列の状態が「空き(0)」でなければ、型式コードのフォーマット変換を行う
                    if ( GetColumnIndex( gvSearchEngineStockLB, GRID_SEARCHENGINESTOCK_FOR_SEARCH_TARGET_2_L.LOCATION_FLAG, out index ) == true ) {
                        if ( false == ( LOCATION_FLAG_NOENGINE_DISP == gvSearchEngineStockLB.Rows[e.Row.RowIndex].Cells[index].Text.ToString() ) ) {
                            var katashikiCd = ( (DataRowView)e.Row.DataItem ).Row[GRID_SEARCHENGINESTOCK_FOR_SEARCH_TARGET_2_R.KATASHIKI_CODE.bindField].ToString();
                            ( (Label)e.Row.Cells[katashikiCdIndex].Controls[0] ).Text = OrderBusiness.getDisplayKatasikiCode( katashikiCd );
                        }
                    }
                }
                int syubetsuIndex = 0;
                if ( GetColumnIndex( sender, GRID_SEARCHENGINESTOCK_FOR_SEARCH_TARGET_2_R.ENGINE_SYUBETSU, out syubetsuIndex ) == true ) {
                    // 種別の場合、固定列の状態が「空き(0)」でなければ、該当する文字列に変換する (該当なしの場合は"")
                    if ( GetColumnIndex( gvSearchEngineStockLB, GRID_SEARCHENGINESTOCK_FOR_SEARCH_TARGET_2_L.LOCATION_FLAG, out index ) == true ) {
                        if ( false == ( LOCATION_FLAG_NOENGINE_DISP == gvSearchEngineStockLB.Rows[e.Row.RowIndex].Cells[index].Text.ToString() ) ) {
                            var syubetsu = ( (DataRowView)e.Row.DataItem ).Row[GRID_SEARCHENGINESTOCK_FOR_SEARCH_TARGET_2_R.ENGINE_SYUBETSU.bindField].ToString();
                            ( (Label)e.Row.Cells[syubetsuIndex].Controls[0] ).Text = OrderBusiness.convertToDisplayEngineSyubetsu( syubetsu );
                        }
                    }
                }
                int tousaiIndex = 0;
                if ( GetColumnIndex( sender, GRID_SEARCHENGINESTOCK_FOR_SEARCH_TARGET_2_R.TOUSAI_OEM, out tousaiIndex ) == true ) {
                    // 搭載/OEMの場合、固定列の状態が「空き(0)」でなければ、該当する文字列に変換する (該当なしの場合は"")
                    if ( GetColumnIndex( gvSearchEngineStockLB, GRID_SEARCHENGINESTOCK_FOR_SEARCH_TARGET_2_L.LOCATION_FLAG, out index ) == true ) {
                        if ( false == ( LOCATION_FLAG_NOENGINE_DISP == gvSearchEngineStockLB.Rows[e.Row.RowIndex].Cells[index].Text.ToString() ) ) {
                            var tousaiOem = ( (DataRowView)e.Row.DataItem ).Row[GRID_SEARCHENGINESTOCK_FOR_SEARCH_TARGET_2_R.TOUSAI_OEM.bindField].ToString();
                            ( (Label)e.Row.Cells[tousaiIndex].Controls[0] ).Text = OrderBusiness.convertToDisplayTousaiOem( tousaiOem );
                        }
                    }
                }
                int naigaiIndex = 0;
                if ( GetColumnIndex( sender, GRID_SEARCHENGINESTOCK_FOR_SEARCH_TARGET_2_R.NAIGAISKAU, out naigaiIndex ) == true ) {
                    // 内外作の場合、固定列の状態が「空き(0)」でなければ、該当する文字列に変換する (該当なしの場合は"")
                    if ( GetColumnIndex( gvSearchEngineStockLB, GRID_SEARCHENGINESTOCK_FOR_SEARCH_TARGET_2_L.LOCATION_FLAG, out index ) == true ) {
                        if ( false == ( LOCATION_FLAG_NOENGINE_DISP == gvSearchEngineStockLB.Rows[e.Row.RowIndex].Cells[index].Text.ToString() ) ) {
                            var naigaisaku = ( (DataRowView)e.Row.DataItem ).Row[GRID_SEARCHENGINESTOCK_FOR_SEARCH_TARGET_2_R.NAIGAISKAU.bindField].ToString();
                            ( (Label)e.Row.Cells[naigaiIndex].Controls[0] ).Text = OrderBusiness.convertToDisplayNaigaisaku( naigaisaku );
                        }
                    }
                }
                int untenIndex = 0;
                if ( GetColumnIndex( sender, GRID_SEARCHENGINESTOCK_FOR_SEARCH_TARGET_2_R.UNTENFLAG, out untenIndex ) == true ) {
                    // 運転の場合、固定列の状態が「空き(0)」でなければ、該当する文字列に変換する (該当なしの場合は"")
                    if ( GetColumnIndex( gvSearchEngineStockLB, GRID_SEARCHENGINESTOCK_FOR_SEARCH_TARGET_2_L.LOCATION_FLAG, out index ) == true ) {
                        if ( false == ( LOCATION_FLAG_NOENGINE_DISP == gvSearchEngineStockLB.Rows[e.Row.RowIndex].Cells[index].Text.ToString() ) ) {
                            var unten = ( (DataRowView)e.Row.DataItem ).Row[GRID_SEARCHENGINESTOCK_FOR_SEARCH_TARGET_2_R.UNTENFLAG.bindField].ToString();
                            ( (Label)e.Row.Cells[untenIndex].Controls[0] ).Text = OrderBusiness.convertToDisplayUnten( unten );
                        }
                    }
                }
                int insIndex = 0;
                if ( GetColumnIndex( sender, GRID_SEARCHENGINESTOCK_FOR_SEARCH_TARGET_2_R.INSTOCK_DATE, out insIndex ) == true ) {
                    // 入庫日時の場合、固定列の状態が「空き(0)」でなければ、日付チェックをする
                    // 2001 /01/01より古いならnullを設定する
                    if ( GetColumnIndex( gvSearchEngineStockLB, GRID_SEARCHENGINESTOCK_FOR_SEARCH_TARGET_2_L.LOCATION_FLAG, out index ) == true ) {
                        if ( false == ( LOCATION_FLAG_NOENGINE_DISP == gvSearchEngineStockLB.Rows[e.Row.RowIndex].Cells[index].Text.ToString() ) ) {
                            var instockDt = ( (DataRowView)e.Row.DataItem ).Row[GRID_SEARCHENGINESTOCK_FOR_SEARCH_TARGET_2_R.INSTOCK_DATE.bindField].ToString();
                            DateTime dt;
                            if ( DateTime.TryParse( instockDt, out dt ) ) {
                                if ( true == OrderBusiness.checkOlderBaseDate( instockDt, BASE_DATA_STR ) ) {
                                    ( (Label)e.Row.Cells[insIndex].Controls[0] ).Text = null;
                                }
                            } else {
                                ( (Label)e.Row.Cells[insIndex].Controls[0] ).Text = null;
                            }
                        }
                    }
                }
                // 選択行の背景色変更を追加
                // 製品別通過実績検索画面画面への遷移機能を設定
                string keyKatashikiCode = StringUtils.ToString( ( (DataRowView)e.Row.DataItem )[GRID_SEARCHENGINESTOCK_FOR_SEARCH_TARGET_2_R.KATASHIKI_CODE.bindField] );
                ControlUtils.GridViewRowBound( (GridView)sender, e, GRID_ORDER_SEARCH_ENGINE_STOCK_GROUP_CD, ControlUtils.GridRowDoubleClickEvent.WindowOpen, PageInfo.ResolveClientUrl( this, PageInfo.SearchProductOrder ), base.Token, keyKatashikiCode );
            }
        }
        #endregion

        #region Excel出力関連処理
        /// <summary>
        /// Excel出力処理
        /// </summary>
        private void OutputExcel() {
            try {
                // セッションから検索データの取得
                ConditionInfoSessionHandler.ST_CONDITION cond = base.ConditionInfo;
                if ( null == cond.ResultData || 0 == cond.ResultData.Rows.Count ) {
                    // 出力対象データなし
                    return;
                }
                // 検索条件出力データ作成
                List<ExcelConditionItem> excelCond = new List<ExcelConditionItem>();
                string condition = "";
                string value = "";

                // 立体倉庫
                condition = CONDITION.RITTAI_NUM.displayNm;
                value = cond.IdWithText[CONDITION.RITTAI_NUM.controlId];
                excelCond.Add( new ExcelConditionItem( condition, value ) );
                // 棚
                condition = CONDITION.STOP_FLAG.displayNm;
                value = cond.IdWithText[CONDITION.STOP_FLAG.controlId];
                excelCond.Add( new ExcelConditionItem( condition, value ) );
                // 状態
                condition = CONDITION.LOCATION_FLAG.displayNm;
                value = cond.IdWithText[CONDITION.LOCATION_FLAG.controlId];
                excelCond.Add( new ExcelConditionItem( condition, value ) );
                // 種別
                condition = CONDITION.ENGINE_SYUBETSU.displayNm;
                value = cond.IdWithText[CONDITION.ENGINE_SYUBETSU.controlId];
                excelCond.Add( new ExcelConditionItem( condition, value ) );
                // 搭載/OEM
                condition = CONDITION.TOUSAI_OEM.displayNm;
                value = cond.IdWithText[CONDITION.TOUSAI_OEM.controlId];
                excelCond.Add( new ExcelConditionItem( condition, value ) );
                // 内外作
                condition = CONDITION.NAIGAISAKU.displayNm;
                value = cond.IdWithText[CONDITION.NAIGAISAKU.controlId];
                excelCond.Add( new ExcelConditionItem( condition, value ) );
                // 運転
                condition = CONDITION.UNTEN_FLAG.displayNm;
                value = cond.IdWithText[CONDITION.UNTEN_FLAG.controlId];
                excelCond.Add( new ExcelConditionItem( condition, value ) );
                // IDNO
                condition = CONDITION.IDNO.displayNm;
                value = cond.IdWithText[CONDITION.IDNO.controlId];
                excelCond.Add( new ExcelConditionItem( condition, value ) );
                // 機番
                condition = CONDITION.KIBAN.displayNm;
                value = cond.IdWithText[CONDITION.KIBAN.controlId];
                excelCond.Add( new ExcelConditionItem( condition, value ) );
                // 型式コード
                condition = CONDITION.KATASHIKI_CODE.displayNm;
                value = cond.IdWithText[CONDITION.KATASHIKI_CODE.controlId];
                excelCond.Add( new ExcelConditionItem( condition, value ) );
                // 型式名
                condition = CONDITION.KATASHIKI_NAME.displayNm;
                value = cond.IdWithText[CONDITION.KATASHIKI_NAME.controlId];
                excelCond.Add( new ExcelConditionItem( condition, value ) );
                // 特記事項
                condition = CONDITION.TOKKI.displayNm;
                value = cond.IdWithText[CONDITION.TOKKI.controlId];
                excelCond.Add( new ExcelConditionItem( condition, value ) );
                // Excelダウンロード実行
                var excelSheetDataList = new List<ExcelSheetData>() {
                    new ExcelSheetData() { Condition = excelCond, SheetName = "型式別在庫一覧", TargetData = GetExcelTableForKatashikibetsu() },
                    new ExcelSheetData() { Condition = excelCond, SheetName = "ロケーション一覧", TargetData = GetExcelTableForZenzaiko() }
                };
                Excel.Download( Response, "エンジン立体倉庫在庫", excelSheetDataList, true );
            } catch ( System.Threading.ThreadAbortException ) {
                // response.Endで必ず発生する為、正常として扱う
            } catch ( Exception ex ) {
                // 例外発生時、ログ出力とメッセージ表示
                Logger.Exception( ex );
                base.WriteApplicationMessage( MsgManager.MESSAGE_ERR_80030, "エンジン立体倉庫在庫_検索結果" );
            }
        }

        /// <summary>
        /// 型式別在庫一覧シート用データテーブル取得
        /// </summary>
        /// <returns>型式別在庫一覧シート用データテーブル</returns>
        private DataTable GetExcelTableForKatashikibetsu() {
            // 出力対象列(Item1 = 表示用ヘッダ、Item2 = 列名)
            var outputHeaderList = new List<Tuple<string, string>>() {
                new Tuple<string, string>("型式ｺｰﾄﾞ","型式コード"),
                new Tuple<string, string>("国ｺｰﾄﾞ","国コード"),
                new Tuple<string, string>("型式名","型式名"),
                new Tuple<string, string>("GP","パターンコード"),
                new Tuple<string, string>("種別","エンジン種別"),
                new Tuple<string, string>("搭載/OEM","搭載OEM"),
                new Tuple<string, string>("内外作","内外作"),
                new Tuple<string, string>("台数","台数"),
            };
            // セッションから検索条件を取得する
            var dicCondition = base.ConditionInfo.conditionValue;
            // 検索上限数を取得する
            int maxGridViewCount = WebAppInstance.GetInstance().Config.WebCommonInfo.maxGridViewCount;
            // 型式別在庫一覧を検索
            var result = OrderBusiness.SearchOfSearchEngineStock( ref dicCondition, maxGridViewCount, SearchTarget.KATASHIKIBETSU.ToString() );
            // 出力結果テーブル
            var tblResult = new DataTable();
            // 出力結果テーブルの列を作成する
            // 表示列の定義を取得
            foreach ( var outputHeader in outputHeaderList ) {
                // 出力対象列の列名に一致する表示列の型を取得し、出力結果テーブルの列として作成する
                var newCol = new DataColumn( outputHeader.Item2 );
                // 表示用ヘッダを設定する
                newCol.Caption = outputHeader.Item1;
                tblResult.Columns.Add( newCol );
            }
            // 一覧元DataTableの情報をExcel出力用テーブルにコピー
            foreach ( DataRow rowSrc in result.ListTable.Rows ) {
                DataRow rowTo = tblResult.NewRow();
                foreach ( var outputHeader in outputHeaderList ) {
                    rowTo[outputHeader.Item2] = rowSrc[outputHeader.Item2];
                }
                if ( outputHeaderList.Any( e => e.Item2 == GRID_SEARCHENGINESTOCK_FOR_SEARCH_TARGET_1_R.KATASHIKI_CODE.bindField ) ) {
                    // 型式コード列の変換を実行
                    rowTo[GRID_SEARCHENGINESTOCK_FOR_SEARCH_TARGET_1_R.KATASHIKI_CODE.bindField] = OrderBusiness.getDisplayKatasikiCode( rowSrc[GRID_SEARCHENGINESTOCK_FOR_SEARCH_TARGET_1_R.KATASHIKI_CODE.bindField].ToString() );
                }
                if ( outputHeaderList.Any( e => e.Item2 == GRID_SEARCHENGINESTOCK_FOR_SEARCH_TARGET_1_R.ENGINE_SYUBETSU.bindField ) ) {
                    // 種別の変換を実行
                    rowTo[GRID_SEARCHENGINESTOCK_FOR_SEARCH_TARGET_1_R.ENGINE_SYUBETSU.bindField] = OrderBusiness.convertToDisplayEngineSyubetsu( rowSrc[GRID_SEARCHENGINESTOCK_FOR_SEARCH_TARGET_1_R.ENGINE_SYUBETSU.bindField].ToString() );
                }
                if ( outputHeaderList.Any( e => e.Item2 == GRID_SEARCHENGINESTOCK_FOR_SEARCH_TARGET_1_R.TOUSAI_OEM.bindField ) ) {
                    // 搭載/OEMの変換を実行
                    rowTo[GRID_SEARCHENGINESTOCK_FOR_SEARCH_TARGET_1_R.TOUSAI_OEM.bindField] = OrderBusiness.convertToDisplayTousaiOem( rowSrc[GRID_SEARCHENGINESTOCK_FOR_SEARCH_TARGET_1_R.TOUSAI_OEM.bindField].ToString() );
                }
                if ( outputHeaderList.Any( e => e.Item2 == GRID_SEARCHENGINESTOCK_FOR_SEARCH_TARGET_1_R.NAIGAISKAU.bindField ) ) {
                    // 内外作の変換を実行
                    rowTo[GRID_SEARCHENGINESTOCK_FOR_SEARCH_TARGET_1_R.NAIGAISKAU.bindField] = OrderBusiness.convertToDisplayNaigaisaku( rowSrc[GRID_SEARCHENGINESTOCK_FOR_SEARCH_TARGET_1_R.NAIGAISKAU.bindField].ToString() );
                }
                tblResult.Rows.Add( rowTo );
            }
            tblResult.AcceptChanges();
            return tblResult;
        }

        /// <summary>
        /// ロケーション一覧シート用データテーブル取得
        /// </summary>
        /// <returns>ロケーション一覧シート用データテーブル</returns>
        private DataTable GetExcelTableForZenzaiko() {
            // 出力対象列(Item1 = 表示用ヘッダ、Item2 = 列名)
            // 立体倉庫,棚番,禁止棚,状態,型式ｺｰﾄﾞ,国ｺｰﾄﾞ,型式名,GP,種別,搭載/OEM,内外作,IDNO,機番,特記,運転,入庫日時
            var outputHeaderList = new List<Tuple<string, string>>() {
                new Tuple<string, string>("立体倉庫","立体倉庫"),
                new Tuple<string, string>("棚番","棚番"),
                new Tuple<string, string>("禁止棚","禁止棚フラグ"),
                new Tuple<string, string>("状態","ロケーションフラグ"),
                new Tuple<string, string>("型式ｺｰﾄﾞ","型式コード"),
                new Tuple<string, string>("国ｺｰﾄﾞ","国コード"),
                new Tuple<string, string>("型式名","型式名"),
                new Tuple<string, string>("GP","パターンコード"),
                new Tuple<string, string>("種別","エンジン種別"),
                new Tuple<string, string>("搭載/OEM","搭載OEM"),
                new Tuple<string, string>("内外作","内外作"),
                new Tuple<string, string>("IDNO","IDNO"),
                new Tuple<string, string>("機番","機番"),
                new Tuple<string, string>("特記","特記事項"),
                new Tuple<string, string>("運転","運転フラグ"),
                new Tuple<string, string>("入庫日時","入庫日時"),
            };
            // セッションから検索条件を取得する
            var dicCondition = base.ConditionInfo.conditionValue;
            // 検索上限数を取得する
            int maxGridViewCount = WebAppInstance.GetInstance().Config.WebCommonInfo.maxGridViewCount;
            var result = OrderBusiness.SearchOfSearchEngineStock( ref dicCondition, maxGridViewCount, SearchTarget.ZENZAIKO.ToString() );
            // 出力結果テーブル
            var tblResult = new DataTable();
            // 出力結果テーブルの列を作成する
            // 表示列の定義を取得
            var tblColumns = result.ListTable.Columns.Cast<DataColumn>().Select( c => new { ColumnName = c.ColumnName, DataType = c.DataType } );
            foreach ( var outputHeader in outputHeaderList ) {
                // 出力対象列の列名に一致する表示列の型を取得し、出力結果テーブルの列として作成する
                var newCol = new DataColumn( outputHeader.Item2, tblColumns.Where( c => c.ColumnName == outputHeader.Item2 ).First().DataType );
                // 表示用ヘッダを設定する
                newCol.Caption = outputHeader.Item1;
                tblResult.Columns.Add( newCol );
            }
            // 一覧元DataTableの情報をExcel出力用テーブルにコピー
            foreach ( DataRow rowSrc in result.ListTable.Rows ) {
                DataRow rowTo = tblResult.NewRow();
                foreach ( var outputHeader in outputHeaderList ) {
                    rowTo[outputHeader.Item2] = rowSrc[outputHeader.Item2];
                    if ( outputHeader.Item2 == GRID_SEARCHENGINESTOCK_FOR_SEARCH_TARGET_2_L.RITTAI_NAME.bindField ) {
                        // 立体倉庫の変換を実行
                        rowTo[outputHeader.Item2] = OrderBusiness.convertToDisplayRittai( rowSrc[outputHeader.Item2].ToString() );
                    } else if ( outputHeader.Item2 == GRID_SEARCHENGINESTOCK_FOR_SEARCH_TARGET_2_L.LOCATION_NAME.bindField ) {
                        // 棚番の変換を実行
                        rowTo[outputHeader.Item2] = string.Format( "{0}-{1}-{2}-{3}",
                            rowSrc[GRID_SEARCHENGINESTOCK_FOR_SEARCH_TARGET_2_L.STOCK_KBN.bindField],
                            rowSrc[GRID_SEARCHENGINESTOCK_FOR_SEARCH_TARGET_2_L.STOCK_REN.bindField],
                            rowSrc[GRID_SEARCHENGINESTOCK_FOR_SEARCH_TARGET_2_L.STOCK_DAN.bindField],
                            rowSrc[GRID_SEARCHENGINESTOCK_FOR_SEARCH_TARGET_2_L.STOCK_RETSU.bindField]
                            );
                    } else if ( outputHeader.Item2 == GRID_SEARCHENGINESTOCK_FOR_SEARCH_TARGET_2_L.STOP_FLAG.bindField ) {
                        // 禁止棚の変換を実行
                        rowTo[outputHeader.Item2] = OrderBusiness.convertToDisplayRittaiStop( rowSrc[outputHeader.Item2].ToString() );
                    } else if ( outputHeader.Item2 == GRID_SEARCHENGINESTOCK_FOR_SEARCH_TARGET_2_L.LOCATION_FLAG.bindField ) {
                        // 状態の変換を実行
                        rowTo[outputHeader.Item2] = OrderBusiness.convertToDisplayLocation( rowSrc[outputHeader.Item2].ToString() );
                    } else if ( rowTo[GRID_SEARCHENGINESTOCK_FOR_SEARCH_TARGET_2_L.LOCATION_FLAG.bindField].ToString() != LOCATION_FLAG_NOENGINE_DISP ) {
                        // 状態が「空き」でない場合
                        if ( outputHeader.Item2 == GRID_SEARCHENGINESTOCK_FOR_SEARCH_TARGET_2_R.KATASHIKI_CODE.bindField ) {
                            // 型式コードの変換を実行
                            rowTo[outputHeader.Item2] = OrderBusiness.getDisplayKatasikiCode( rowSrc[outputHeader.Item2].ToString() );
                        } else if ( outputHeader.Item2 == GRID_SEARCHENGINESTOCK_FOR_SEARCH_TARGET_2_R.ENGINE_SYUBETSU.bindField ) {
                            // 種別の変換を実行
                            rowTo[outputHeader.Item2] = OrderBusiness.convertToDisplayEngineSyubetsu( rowSrc[outputHeader.Item2].ToString() );
                        } else if ( outputHeader.Item2 == GRID_SEARCHENGINESTOCK_FOR_SEARCH_TARGET_2_R.TOUSAI_OEM.bindField ) {
                            // 搭載/OEMの変換を実行
                            rowTo[outputHeader.Item2] = OrderBusiness.convertToDisplayTousaiOem( rowSrc[outputHeader.Item2].ToString() );
                        } else if ( outputHeader.Item2 == GRID_SEARCHENGINESTOCK_FOR_SEARCH_TARGET_2_R.NAIGAISKAU.bindField ) {
                            // 内外作の変換を実行
                            rowTo[outputHeader.Item2] = OrderBusiness.convertToDisplayNaigaisaku( rowSrc[outputHeader.Item2].ToString() );
                        } else if ( outputHeader.Item2 == GRID_SEARCHENGINESTOCK_FOR_SEARCH_TARGET_2_R.INSTOCK_DATE.bindField ) {
                            // 入庫日時の変換を実行
                            rowTo[outputHeader.Item2] = ( ( true == OrderBusiness.checkOlderBaseDate( DateUtils.ToDate( rowSrc[outputHeader.Item2] ).ToString(), BASE_DATA_STR ) ? null : rowSrc[outputHeader.Item2].ToString() ) );
                        }
                    }
                }
                tblResult.Rows.Add( rowTo );
            }
            tblResult.AcceptChanges();
            return tblResult;
        }
        #endregion
        #endregion

        #region グリッドビュー操作
        /// <summary>
        /// グリッドビュークリア
        /// </summary>
        private void ClearGridView() {
            // 列名非表示 グリッドビュークリア
            ControlUtils.InitializeGridView( grvHeaderLT, false );
            ControlUtils.InitializeGridView( grvHeaderRT, false );
            ControlUtils.InitializeGridView( gvSearchEngineStockLB, false );
            ControlUtils.InitializeGridView( gvSearchEngineStockRB, false );
            // 件数表示
            ntbResultCount.Value = 0;
            // ページャークリア
            ControlUtils.ClearPager( ref pnlPager );
            // GridView非表示
            divGrvDisplay.Visible = false;
        }

        /// <summary>
        /// グリッドビューページ切替
        /// </summary>
        /// <param name="parameters"></param>
        private void PageIndexChanging( params object[] parameters ) {
            object sender = parameters[0];
            CommandEventArgs e = (CommandEventArgs)parameters[1];
            int newPageIndex = Convert.ToInt32( e.CommandArgument );
            int pageSize = gvSearchEngineStockRB.PageSize;
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
            // 背面ユーザ切替対応
            ConditionInfoSessionHandler.ST_CONDITION cond = ConditionInfo;
            ControlUtils.ShowGridViewHeader( grvHeaderLT, GridviewDefault_L, cond, true );
            ControlUtils.ShowGridViewHeader( grvHeaderRT, GridviewDefault_R, cond, true );
            ControlUtils.BindGridView_WithTempField( gvSearchEngineStockLB, GridviewDefault_L, ConditionInfo.ResultData );
            ControlUtils.BindGridView_WithTempField( gvSearchEngineStockRB, GridviewDefault_R, ConditionInfo.ResultData );
            ControlUtils.GridViewPageIndexChanging( gvSearchEngineStockLB, ConditionInfo.ResultData, new GridViewPageEventArgs( newPageIndex ) );
            ControlUtils.GridViewPageIndexChanging( gvSearchEngineStockRB, ConditionInfo.ResultData, new GridViewPageEventArgs( newPageIndex ) );
            ControlUtils.SetGridViewPager( ref pnlPager, gvSearchEngineStockRB, gvSearchEngineStock_PageIndexChanging, ConditionInfo.ResultData.Rows.Count, gvSearchEngineStockRB.PageIndex );
            // グリッドビュー外のDivサイズ変更
            SetDivGridViewWidth();
        }

        /// <summary>
        /// グリッドビュー並び替え
        /// </summary>
        /// <param name="parameters"></param>
        private void Sorting( params object[] parameters ) {
            object sender = parameters[0];
            GridViewSortEventArgs e = (GridViewSortEventArgs)parameters[1];
            ConditionInfoSessionHandler.ST_CONDITION cond = ConditionInfo;
            ControlUtils.GridViewSorting( gvSearchEngineStockLB, ref cond, e, true );
            ControlUtils.GridViewSorting( gvSearchEngineStockRB, ref cond, e );
            // 背面ユーザ切替対応
            ControlUtils.ShowGridViewHeader( grvHeaderLT, GridviewDefault_L, cond, true );
            ControlUtils.ShowGridViewHeader( grvHeaderRT, GridviewDefault_R, cond, true );
            ControlUtils.BindGridView_WithTempField( gvSearchEngineStockLB, GridviewDefault_L, cond.ResultData );
            ControlUtils.BindGridView_WithTempField( gvSearchEngineStockRB, GridviewDefault_R, cond.ResultData );
            ControlUtils.SetGridViewPager( ref pnlPager, gvSearchEngineStockRB, gvSearchEngineStock_PageIndexChanging, cond.ResultData.Rows.Count, gvSearchEngineStockRB.PageIndex );
            ConditionInfo = cond;
            // グリッドビュー外のDivサイズ変更
            SetDivGridViewWidth();
        }

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

        /// <summary>
        /// グリッドビュー外のDivサイズ変更
        /// </summary>
        private void SetDivGridViewWidth() {
            SetDivGridViewWidth( grvHeaderLT, divGrvHeaderLT );
            SetDivGridViewWidth( grvHeaderRT, divGrvHeaderRT );
            SetDivGridViewWidth( gvSearchEngineStockLB, divGrvLB );
            SetDivGridViewWidth( gvSearchEngineStockRB, divGrvRB );
        }
        /// <summary>
        /// グリッドビュー外のDivサイズ変更
        /// </summary>
        private void SetDivGridViewWidth( GridView grv, System.Web.UI.HtmlControls.HtmlGenericControl div ) {
            // セル幅補正 Padding4px + Border(片側) 1px
            const int CELL_PADDING = 4 * 2 + 1;
            // テーブル幅補正 Border(片側) 1px
            const int OUT_BORDER = 1;
            var visibleColumns = grv.Columns.Cast<DataControlField>().Where( x => x.Visible ).ToList();
            int sumWidth = NumericUtils.ToInt( visibleColumns.Sum( x => x.HeaderStyle.Width.Value ) )
                                + CELL_PADDING * visibleColumns.Count()
                                + ( visibleColumns.Any() ? OUT_BORDER : 0 );
            div.Style["width"] = $"{ sumWidth }px";
        }
        #endregion
    }
}