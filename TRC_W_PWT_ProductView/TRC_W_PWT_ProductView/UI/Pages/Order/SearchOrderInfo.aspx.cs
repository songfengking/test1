////////////////////////////////////////////////////////////////////////////////////////////////
//      クボタ筑波工場  TRC
//      概要：トレーサビリティシステム 順序情報検索
//---------------------------------------------------------------------------
//           Ver 1.44.0.0  :  2021/06/08  大沼 新規作成(java版からリプレース)
//           Ver 1.44.0.1  :  2021/09/28  大沼 表示件数の変更
//           Ver 1.44.1.1  :  2021/10/25  豊島 RowDataBoundのログ出力抑制
//           Ver 1.44.1.2  :  2021/12/16  豊島 EIDNO、E機番をexcelに出力するよう修正
////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
    public partial class SearchOrderInfo : BaseForm {
        #region 定数

        #region 製品種別
        /// <summary>ID/機番検索</summary>
        public const string ID_KIBAN_SEARCH = "ID/機番検索";
        /// <summary>03エンジン</summary>
        public const string ENGINE_03 = "03エンジン";
        /// <summary>07エンジン</summary>
        public const string ENGINE_07 = "07エンジン";
        /// <summary>ミッション投入</summary>
        public const string MISSION_THROW = "ミッション投入";
        /// <summary>本機確定</summary>
        public const string TRACTOR_FIX = "本機確定";
        #endregion

        #region CSS指定
        /// <summary>灰</summary>
        private const string COLOR_GRAY = "<span class=base-katashiki-name>";
        #endregion

        /// <summary>spanタグ(末尾)</summary>
        private const string END_SPAN = "</span>";
        /// <summary>brタグ</summary>
        private const string BREAK = "<br />";
        /// <summary>スペース(HTML)</summary>
        private const string SPACE_HTML = "&nbsp;";

        /// <summary>ハイフン</summary>
        private const string HYPHEN = "-";
        /// <summary>丸印</summary>
        private const string CIRCLE = "○";
        /// <summary>括弧（開始）</summary>
        private const string OPEN_BRACKETS = "(";
        /// <summary>括弧（終了）</summary>
        private const string CLOSE_BRACKETS = ")";
        /// <summary>スペース(Char型)</summary>
        private const Char SPACE_CHAR = ' ';
        /// <summary>改行コード</summary>
        private const string BREAK_CODE = "\r\n";
        /// <summary>スラッシュ</summary>
        private const string SLASH = "/";

        /// <summary>メッセージ(Key)</summary>
        const string MSG_KEY = "MSG";


        #region 検索条件
        /// <summary>
        /// 検索条件
        /// </summary>
        public class CONDITION {
            /// <summary>
            /// 製品種別
            /// </summary>
            public static readonly ControlDefine SHIJI_LEVEL = new ControlDefine( "rblShijiLevel", "製品種別", "rblShijiLevel", ControlDefine.BindType.Both, typeof( string ) );
            /// <summary>
            /// IDNO
            /// </summary>
            public static readonly ControlDefine IDNO = new ControlDefine( "txtIdno", "IDNO", "txtIdno", ControlDefine.BindType.Both, typeof( string ) );
            /// <summary>
            /// 機番
            /// </summary>
            public static readonly ControlDefine KIBAN = new ControlDefine( "txtKiban", "機番", "txtKiban", ControlDefine.BindType.Both, typeof( string ) );
            /// <summary>
            /// 月度
            /// </summary>
            public static readonly ControlDefine SHIJI_YM = new ControlDefine( "ddlShijiYM", "月度", "ddlShijiYM", ControlDefine.BindType.Both, typeof( string ) );
            /// <summary>
            /// 型式コード
            /// </summary>
            public static readonly ControlDefine KATASHIKI_CODE = new ControlDefine( "txtKatashikiCode", "型式コード", "txtKatashikiCode", ControlDefine.BindType.Both, typeof( string ) );
            /// <summary>
            /// 国コード
            /// </summary>
            public static readonly ControlDefine KUNI_CODE = new ControlDefine( "txtKuniCode", "国コード", "txtKuniCode", ControlDefine.BindType.Both, typeof( string ) );
            /// <summary>
            /// 型式名
            /// </summary>
            public static readonly ControlDefine KATASHIKI_NAME = new ControlDefine( "txtKatashikiName", "型式名", "txtKatashikiName", ControlDefine.BindType.Both, typeof( string ) );
            /// <summary>
            /// 特記事項
            /// </summary>
            public static readonly ControlDefine TOKKI = new ControlDefine( "txtTokki", "特記事項", "txtTokki", ControlDefine.BindType.Both, typeof( string ) );
        }
        #endregion

        #region グリッドビュー定義
        #region 種別がID/機番検索の場合
        /// <summary>
        /// グリッドビュー定義(種別がID/機番検索の場合：固定列)
        /// </summary>
        public class GRID_SEARCHORDERINFO_ID_KIBAN_L {
            /// <summary>
            /// NO
            /// </summary>
            public static readonly GridViewDefine DISP_ORDER = new GridViewDefine( "NO", "DISP_ORDER", typeof( string ), "", false, HorizontalAlign.Right, 40, true, true );
            /// <summary>
            /// 月度連番
            /// </summary>
            public static readonly GridViewDefine SHIJI_YM_NUM_UNLABELED = new GridViewDefine( "月度連番", "MS_YYMM_NO", typeof( string ), "", true, HorizontalAlign.Center, 110, true, true );
            /// <summary>
            /// IDNO
            /// </summary>
            public static readonly GridViewDefine IDNO = new GridViewDefine( "IDNO", "MS_IDNO", typeof( string ), "", true, HorizontalAlign.Center, 60, true, true );
            /// <summary>
            /// 型式ｺｰﾄﾞ
            /// </summary>
            public static readonly GridViewDefine KATASHIKI_CODE = new GridViewDefine( "型式ｺｰﾄﾞ", "MS_B_KATA_C", typeof( string ), "", true, HorizontalAlign.Center, 100, true, true );
            /// <summary>
            /// 国ｺｰﾄﾞ
            /// </summary>
            public static readonly GridViewDefine KUNI_CODE = new GridViewDefine( "国ｺｰﾄﾞ", "MS_B_KUNI_C", typeof( string ), "", true, HorizontalAlign.Left, 100, true, true );
            /// <summary>
            /// 型式名
            /// </summary>
            public static readonly GridViewDefine KATASHIKI_NAME = new GridViewDefine( "型式名", "MS_B_KATA_N", typeof( string ), "", true, HorizontalAlign.Left, 170, true, true );
            /// <summary>
            /// ベース型式名
            /// </summary>
            public static readonly GridViewDefine BASE_KATASHIKI_NAME = new GridViewDefine( "ベース型式名", "MS_K_KATA_N", typeof( string ), "", true, HorizontalAlign.Left, 160, false, true );
            /// <summary>
            /// 機番
            /// </summary>
            public static readonly GridViewDefine KIBAN = new GridViewDefine( "機番", "MS_KIBAN", typeof( string ), "", true, HorizontalAlign.Center, 60, true, true );
        }

        /// <summary>
        /// グリッドビュー定義(種別がID/機番検索のとき：可変列)
        /// </summary>
        public class GRID_SEARCHORDERINFO_ID_KIBAN_R {
            /// <summary>
            /// 特記事項
            /// </summary>
            public static readonly GridViewDefine TOKKI = new GridViewDefine( "特記事項", "MS_TOKKIJIKOU", typeof( string ), "", true, HorizontalAlign.Left, 90, true, true );
            /// <summary>
            /// 完成予定日
            /// </summary>
            public static readonly GridViewDefine KANSEI_YOTEI_DATE = new GridViewDefine( "完成予定日", "MS_KAN_YYMMDD", typeof( string ), "{0:" + "MM/dd" + "}", true, HorizontalAlign.Center, 120, true, true );
            /// <summary>
            /// 完成日
            /// </summary>
            public static readonly GridViewDefine KANSEI_DATE = new GridViewDefine( "完成日", "完成日時", typeof( string ), "{0:" + "MM/dd" + "}", true, HorizontalAlign.Center, 100, true, true );
            /// <summary>
            /// 出荷日
            /// </summary>
            public static readonly GridViewDefine SYUKKA_DATE = new GridViewDefine( "出荷日", "出荷日", typeof( string ), "{0:" + "MM/dd" + "}", true, HorizontalAlign.Center, 100, true, true );
            /// <summary>
            /// E型式ｺｰﾄﾞ
            /// </summary>
            public static readonly GridViewDefine ENGINE_KATASHIKI_CODE = new GridViewDefine( "E型式ｺｰﾄﾞ", "MS_E_KATA_C", typeof( string ), "", true, HorizontalAlign.Center, 110, true, true );
            /// <summary>
            /// E型式名
            /// </summary>
            public static readonly GridViewDefine ENGINE_KATASHIKI_NAME = new GridViewDefine( "E型式名", "MS_E_KATA_N", typeof( string ), "", true, HorizontalAlign.Left, 160, true, true );
            /// <summary>
            /// E-IDNO
            /// </summary>
            public static readonly GridViewDefine ENGINE_IDNO = new GridViewDefine( "E-IDNO", "エンジンIDNO", typeof( string ), "", true, HorizontalAlign.Center, 90, true, true );
            /// <summary>
            /// E機番
            /// </summary>
            public static readonly GridViewDefine ENGINE_KIBAN = new GridViewDefine( "E機番", "エンジン機番", typeof( string ), "", true, HorizontalAlign.Center, 90, true, true );
            /// <summary>
            /// ﾊﾟﾀﾝ
            /// </summary>
            public static readonly GridViewDefine KUMITATE_PATTERN = new GridViewDefine( "ﾊﾟﾀﾝ", "MS_K_PATAN", typeof( string ), "", true, HorizontalAlign.Center, 70, true, true );
            /// <summary>
            /// Eﾊﾟﾀﾝ
            /// </summary>
            public static readonly GridViewDefine ENGINE_KUMITATE_PATTERN = new GridViewDefine( "Eﾊﾟﾀﾝ", "エンジン組立パターン", typeof( string ), "", true, HorizontalAlign.Center, 70, true, true );
            /// <summary>
            /// LV
            /// </summary>
            public static readonly GridViewDefine SHIJI_LEVEL = new GridViewDefine( "LV", "MS_SIJI_LVL", typeof( string ), "", true, HorizontalAlign.Center, 70, true, true );
            /// <summary>
            /// 作成日
            /// </summary>
            public static readonly GridViewDefine CREATE_DATE = new GridViewDefine( "作成日", "MS_INS_YMD", typeof( string ), "", true, HorizontalAlign.Center, 170, true, true );
        }
        #endregion

        #region グリッドビュー定義(種別が本機確定の場合)
        /// <summary>
        /// グリッドビュー定義(種別が本機確定の場合：固定列)
        /// </summary>
        public class GRID_SEARCHORDERINFO_TRACTOR_FIX_L {
            /// <summary>
            /// NO
            /// </summary>
            public static readonly GridViewDefine DISP_ORDER = new GridViewDefine( "NO", "DISP_ORDER", typeof( string ), "", false, HorizontalAlign.Right, 40, true, true );
            /// <summary>
            /// 確定月度連番
            /// </summary>
            public static readonly GridViewDefine SHIJI_YM_NUM = new GridViewDefine( "確定月度連番", "MS_YYMM_NO", typeof( string ), "", true, HorizontalAlign.Center, 110, true, true );
            /// <summary>
            /// 指示月度連番
            /// </summary>
            public static readonly GridViewDefine SHIJI_YM_NUM_FOR_KAKUTEI = new GridViewDefine( "指示月度連番", "指示月度連番", typeof( string ), "", true, HorizontalAlign.Center, 110, true, true );
            /// <summary>
            /// 直通
            /// </summary>
            public static readonly GridViewDefine TYOKKO_SIGN = new GridViewDefine( "直通", "直通", typeof( string ), "", true, HorizontalAlign.Center, 60, true, true );
            /// <summary>
            /// IDNO
            /// </summary>
            public static readonly GridViewDefine IDNO = new GridViewDefine( "IDNO", "MS_IDNO", typeof( string ), "", true, HorizontalAlign.Center, 60, true, true );
            /// <summary>
            /// 型式ｺｰﾄﾞ
            /// </summary>
            public static readonly GridViewDefine KATASHIKI_CODE = new GridViewDefine( "型式ｺｰﾄﾞ", "MS_B_KATA_C", typeof( string ), "", true, HorizontalAlign.Center, 100, true, true );
            /// <summary>
            /// 国ｺｰﾄﾞ
            /// </summary>
            public static readonly GridViewDefine KUNI_CODE = new GridViewDefine( "国ｺｰﾄﾞ", "MS_B_KUNI_C", typeof( string ), "", true, HorizontalAlign.Left, 100, true, true );
            /// <summary>
            /// 型式名
            /// </summary>
            public static readonly GridViewDefine KATASHIKI_NAME = new GridViewDefine( "型式名", "MS_B_KATA_N", typeof( string ), "", true, HorizontalAlign.Left, 170, true, true );
            /// <summary>
            /// ベース型式名
            /// </summary>
            public static readonly GridViewDefine BASE_KATASHIKI_NAME = new GridViewDefine( "ベース型式名", "MS_K_KATA_N", typeof( string ), "", true, HorizontalAlign.Left, 160, false, true );
            /// <summary>
            /// 機番
            /// </summary>
            public static readonly GridViewDefine KIBAN = new GridViewDefine( "機番", "MS_KIBAN", typeof( string ), "", true, HorizontalAlign.Center, 60, true, true );
        }

        /// <summary>
        /// グリッドビュー定義(種別が本機確定の場合：可変列)
        /// </summary>
        public class GRID_SEARCHORDERINFO_TRACTOR_FIX_R {
            /// <summary>
            /// 特記事項
            /// </summary>
            public static readonly GridViewDefine TOKKI = new GridViewDefine( "特記事項", "MS_TOKKIJIKOU", typeof( string ), "", true, HorizontalAlign.Left, 90, true, true );
            /// <summary>
            /// 完成予定日
            /// </summary>
            public static readonly GridViewDefine KANSEI_YOTEI_DATE = new GridViewDefine( "完成予定日", "MS_KAN_YYMMDD", typeof( string ), "{0:" + "MM/dd" + "}", true, HorizontalAlign.Center, 120, true, true );
            /// <summary>
            /// 完成日
            /// </summary>
            public static readonly GridViewDefine KANSEI_DATE = new GridViewDefine( "完成日", "完成日時", typeof( string ), "{0:" + "MM/dd" + "}", true, HorizontalAlign.Center, 100, true, true );
            /// <summary>
            /// 出荷日
            /// </summary>
            public static readonly GridViewDefine SYUKKA_DATE = new GridViewDefine( "出荷日", "出荷日", typeof( string ), "{0:" + "MM/dd" + "}", true, HorizontalAlign.Center, 100, true, true );
            /// <summary>
            /// E型式ｺｰﾄﾞ
            /// </summary>
            public static readonly GridViewDefine ENGINE_KATASHIKI_CODE = new GridViewDefine( "E型式ｺｰﾄﾞ", "MS_E_KATA_C", typeof( string ), "", true, HorizontalAlign.Center, 110, true, true );
            /// <summary>
            /// E型式名
            /// </summary>
            public static readonly GridViewDefine ENGINE_KATASHIKI_NAME = new GridViewDefine( "E型式名", "MS_E_KATA_N", typeof( string ), "", true, HorizontalAlign.Left, 160, true, true );
            /// <summary>
            /// E-IDNO
            /// </summary>
            public static readonly GridViewDefine ENGINE_IDNO = new GridViewDefine( "E-IDNO", "エンジンIDNO", typeof( string ), "", true, HorizontalAlign.Center, 90, true, true );
            /// <summary>
            /// E機番
            /// </summary>
            public static readonly GridViewDefine ENGINE_KIBAN = new GridViewDefine( "E機番", "エンジン機番", typeof( string ), "", true, HorizontalAlign.Center, 90, true, true );
            /// <summary>
            /// ﾊﾟﾀﾝ
            /// </summary>
            public static readonly GridViewDefine KUMITATE_PATTERN = new GridViewDefine( "ﾊﾟﾀﾝ", "MS_K_PATAN", typeof( string ), "", true, HorizontalAlign.Center, 70, true, true );
            /// <summary>
            /// Eﾊﾟﾀﾝ
            /// </summary>
            public static readonly GridViewDefine ENGINE_KUMITATE_PATTERN = new GridViewDefine( "Eﾊﾟﾀﾝ", "エンジン組立パターン", typeof( string ), "", true, HorizontalAlign.Center, 70, true, true );
            /// <summary>
            /// 計画台数
            /// </summary>
            public static readonly GridViewDefine KEIKAKU_DAISU = new GridViewDefine( "計画台数", "型式別計画台数", typeof( string ), "", false, HorizontalAlign.Right, 90, true, true );
            /// <summary>
            /// 累計台数
            /// </summary>
            public static readonly GridViewDefine RUIKEI_DAISU = new GridViewDefine( "累計台数", "型式別累計台数", typeof( string ), "", false, HorizontalAlign.Center, 90, false, true );
            /// <summary>
            /// 作成日
            /// </summary>
            public static readonly GridViewDefine CREATE_DATE = new GridViewDefine( "作成日", "MS_INS_YMD", typeof( string ), "", true, HorizontalAlign.Center, 170, true, true );

        }
        #endregion

        #region グリッドビュー定義(種別がID/機番検索、本機確定以外の場合）
        /// <summary>
        /// グリッドビュー定義(種別がID/機番検索、本機確定以外の場合：固定列）
        /// </summary>
        public class GRID_SEARCHORDERINFO_OTHER_L {
            /// <summary>
            /// NO
            /// </summary>
            public static readonly GridViewDefine DISP_ORDER = new GridViewDefine( "NO", "DISP_ORDER", typeof( string ), "", false, HorizontalAlign.Right, 40, true, true );
            /// <summary>
            /// 月度連番
            /// </summary>
            public static readonly GridViewDefine SHIJI_YM_NUM_UNLABELED = new GridViewDefine( "月度連番", "MS_YYMM_NO", typeof( string ), "", true, HorizontalAlign.Center, 110, true, true );
            /// <summary>
            /// IDNO
            /// </summary>
            public static readonly GridViewDefine IDNO = new GridViewDefine( "IDNO", "MS_IDNO", typeof( string ), "", true, HorizontalAlign.Center, 60, true, true );
            /// <summary>
            /// 型式ｺｰﾄﾞ
            /// </summary>
            public static readonly GridViewDefine KATASHIKI_CODE = new GridViewDefine( "型式ｺｰﾄﾞ", "MS_B_KATA_C", typeof( string ), "", true, HorizontalAlign.Center, 100, true, true );
            /// <summary>
            /// 国ｺｰﾄﾞ
            /// </summary>
            public static readonly GridViewDefine KUNI_CODE = new GridViewDefine( "国ｺｰﾄﾞ", "MS_B_KUNI_C", typeof( string ), "", true, HorizontalAlign.Left, 100, true, true );
            /// <summary>
            /// 型式名
            /// </summary>
            public static readonly GridViewDefine KATASHIKI_NAME = new GridViewDefine( "型式名", "MS_B_KATA_N", typeof( string ), "", true, HorizontalAlign.Left, 170, true, true );
            /// <summary>
            /// ベース型式名
            /// </summary>
            public static readonly GridViewDefine BASE_KATASHIKI_NAME = new GridViewDefine( "ベース型式名", "MS_K_KATA_N", typeof( string ), "", true, HorizontalAlign.Left, 160, false, true );
            /// <summary>
            /// 機番
            /// </summary>
            public static readonly GridViewDefine KIBAN = new GridViewDefine( "機番", "MS_KIBAN", typeof( string ), "", true, HorizontalAlign.Center, 60, true, true );
        }

        /// <summary>
        /// グリッドビュー定義(種別がID/機番検索、本機確定以外の場合：可変列）
        /// </summary>
        public class GRID_SEARCHORDERINFO_OTHER_R {
            /// <summary>
            /// 特記事項
            /// </summary>
            public static readonly GridViewDefine TOKKI = new GridViewDefine( "特記事項", "MS_TOKKIJIKOU", typeof( string ), "", true, HorizontalAlign.Left, 90, true, true );
            /// <summary>
            /// 完成予定日
            /// </summary>
            public static readonly GridViewDefine KANSEI_YOTEI_DATE = new GridViewDefine( "完成予定日", "MS_KAN_YYMMDD", typeof( string ), "{0:" + "MM/dd" + "}", true, HorizontalAlign.Center, 120, true, true );
            /// <summary>
            /// 完成日
            /// </summary>
            public static readonly GridViewDefine KANSEI_DATE = new GridViewDefine( "完成日", "完成日時", typeof( string ), "{0:" + "MM/dd" + "}", true, HorizontalAlign.Center, 100, true, true );
            /// <summary>
            /// 出荷日
            /// </summary>
            public static readonly GridViewDefine SYUKKA_DATE = new GridViewDefine( "出荷日", "出荷日", typeof( string ), "{0:" + "MM/dd" + "}", true, HorizontalAlign.Center, 100, true, true );
            /// <summary>
            /// E型式ｺｰﾄﾞ
            /// </summary>
            public static readonly GridViewDefine ENGINE_KATASHIKI_CODE = new GridViewDefine( "E型式ｺｰﾄﾞ", "MS_E_KATA_C", typeof( string ), "", true, HorizontalAlign.Center, 110, true, true );
            /// <summary>
            /// E型式名
            /// </summary>
            public static readonly GridViewDefine ENGINE_KATASHIKI_NAME = new GridViewDefine( "E型式名", "MS_E_KATA_N", typeof( string ), "", true, HorizontalAlign.Left, 160, true, true );
            /// <summary>
            /// E-IDNO
            /// </summary>
            public static readonly GridViewDefine ENGINE_IDNO = new GridViewDefine( "E-IDNO", "エンジンIDNO", typeof( string ), "", true, HorizontalAlign.Center, 90, true, true );
            /// <summary>
            /// E機番
            /// </summary>
            public static readonly GridViewDefine ENGINE_KIBAN = new GridViewDefine( "E機番", "エンジン機番", typeof( string ), "", true, HorizontalAlign.Center, 90, true, true );
            /// <summary>
            /// ﾊﾟﾀﾝ
            /// </summary>
            public static readonly GridViewDefine KUMITATE_PATTERN = new GridViewDefine( "ﾊﾟﾀﾝ", "MS_K_PATAN", typeof( string ), "", true, HorizontalAlign.Center, 70, true, true );
            /// <summary>
            /// Eﾊﾟﾀﾝ
            /// </summary>
            public static readonly GridViewDefine ENGINE_KUMITATE_PATTERN = new GridViewDefine( "Eﾊﾟﾀﾝ", "エンジン組立パターン", typeof( string ), "", true, HorizontalAlign.Center, 70, true, true );
            /// 計画台数
            /// </summary>
            public static readonly GridViewDefine KEIKAKU_DAISU = new GridViewDefine( "計画台数", "型式別計画台数", typeof( string ), "", false, HorizontalAlign.Right, 90, true, true );
            /// <summary>
            /// 累計台数
            /// </summary>
            public static readonly GridViewDefine RUIKEI_DAISU = new GridViewDefine( "累計台数", "型式別累計台数", typeof( string ), "", false, HorizontalAlign.Center, 90, false, true );
            /// <summary>
            /// 作成日
            /// </summary>
            public static readonly GridViewDefine CREATE_DATE = new GridViewDefine( "作成日", "MS_INS_YMD", typeof( string ), "", true, HorizontalAlign.Center, 170, true, true );

        }
        #endregion
        #endregion

        /// <summary>
        /// グリッドビューグループ
        /// </summary>
        const string GRID_ORDER_SEARCH_ORDER_INFO_CD = "SearchOrderInfo";

        /// <summary>
        /// 直行
        /// </summary>
        const int TYOKKO = 1;
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

        #region 検索条件
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
        #endregion

        #region 一覧定義
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
                if ( true == ObjectUtils.IsNotNull( base.ConditionInfo.ResultDefine ) ) {
                    // 検索条件セッション情報があれば検索条件セッション情報を返す
                    _gridviewDefault_l = GetGridviewDefaultFromSession_L();
                } else {
                    _gridviewDefault_l = GetGridviewDefaultFromSijiLevel_L();
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
                if ( true == ObjectUtils.IsNotNull( base.ConditionInfo.ResultDefine ) ) {
                    // 検索条件セッション情報があれば検索条件セッション情報を返す
                    _gridviewDefault_r = GetGridviewDefaultFromSession_R();
                } else {
                    _gridviewDefault_r = GetGridviewDefaultFromSijiLevel_R();

                }
                return _gridviewDefault_r;
            }
        }
        #endregion

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
        /// 検索ボタン押下時イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click( object sender, EventArgs e ) {
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
        protected void grvSearchOrderInfo_Sorting( object sender, GridViewSortEventArgs e ) {
            base.RaiseEvent( Sorting, sender, e );
            RestoreMsg();
        }

        /// <summary>
        /// グリッドビューページチェンジイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grvSearchOrderInfo_PageIndexChanging( object sender, CommandEventArgs e ) {
            base.RaiseEvent( PageIndexChanging, sender, e );
            RestoreMsg();
        }

        /// <summary>
        /// 一覧データバインドイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grvSearchOrderInfo_RowDataBound( object sender, GridViewRowEventArgs e ) {
            try {
                ClearApplicationMessage();
                RowDataBound( sender, e );
            } catch ( Exception ex ) {
                //イベント処理中にエラー発生
                Logger.Exception( ex );
                throw ex;
            }
        }

        /// <summary>
        /// 製品種別リスト選択動作
        /// </summary>
        protected void rblShijiLevel_SelectedIndexChanged( object sender, EventArgs e ) {
            base.RaiseEvent( ChangeShijiLevel );
        }
        #endregion
        #endregion

        #region ページ処理
        /// <summary>
        /// ページロード処理
        /// </summary>
        protected override void DoPageLoad() {
            // ベースページロード処理
            base.DoPageLoad();
            // grvSearchOrderInfoLBの作り直し
            grvSearchOrderInfoLB.Columns.Clear();
            for ( int idx = 0; idx < GridviewDefault_L.Length; idx++ ) {
                TemplateField tf = new TemplateField();
                tf.HeaderText = StringUtils.ToString( GridviewDefault_L[idx].bindField );
                grvSearchOrderInfoLB.Columns.Add( tf );
            }
            // grvSearchOrderInfoRBの作り直し
            grvSearchOrderInfoRB.Columns.Clear();
            for ( int idx = 0; idx < GridviewDefault_R.Length; idx++ ) {
                TemplateField tf = new TemplateField();
                tf.HeaderText = StringUtils.ToString( GridviewDefault_R[idx].bindField );
                grvSearchOrderInfoRB.Columns.Add( tf );
            }
            // グリッドビューの再表示
            ControlUtils.SetGridViewTemplateField( grvSearchOrderInfoLB, GridviewDefault_L );
            ControlUtils.SetGridViewTemplateField( grvSearchOrderInfoRB, GridviewDefault_R );
            ControlUtils.ReMakeBoundTemplateFieldCtrl( grvSearchOrderInfoLB, GridviewDefault_L );
            ControlUtils.ReMakeBoundTemplateFieldCtrl( grvSearchOrderInfoRB, GridviewDefault_R );
            int resultCnt = 0;
            if ( ObjectUtils.IsNotNull( ConditionInfo.ResultData ) ) {
                //検索結果がnullでない場合
                resultCnt = ConditionInfo.ResultData.Rows.Count;
            }
            ControlUtils.SetGridViewPager( ref pnlPager, grvSearchOrderInfoRB, grvSearchOrderInfo_PageIndexChanging, resultCnt, grvSearchOrderInfoRB.PageIndex );
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
        #endregion

        #region 機能別処理
        /// <summary>
        /// 初期処理
        /// </summary>
        private void InitializeValues() {
            try {
                // 作業指示月度リスト情報を検索条件：月度のリストに設定する
                ControlUtils.SetListControlItems( ddlShijiYM, MasterList.SagyoYMList );
                // 製品種別.ミッション投入にチェック
                this.rblShijiLevel.SelectedValue = MISSION_THROW;
                // システム日付を検索条件：月度の初期値に設定する
                String systemDate = DateUtils.ToString( DateTime.Now, DateUtils.DATE_FORMAT_MONTH_NOSEP );
                ddlShijiYM.SelectedValue = systemDate;

                // オプションボタン制御
                ChangeShijiLevel();
            } catch ( Exception ex ) {
                // 例外ログ、メッセージ表示を実行する
                Logger.Exception( ex );
                base.WriteApplicationMessage( MsgManager.MESSAGE_ERR_84010 );
            }
        }

        /// <summary>
        /// 検索処理
        /// </summary>
        private void DoSearch() {
            // 検索条件を作成する
            Dictionary<string, object> dicCondition = new Dictionary<string, object>();
            base.GetControlValues( ConditionControls, ref dicCondition );
            // 検索時画面情報を取得する
            Dictionary<string, string> dicIdWithText = new Dictionary<string, string>();
            base.GetControlTexts( ConditionControls, out dicIdWithText );
            // 一覧定義情報を作成する          
            var gridviewDefineTmp = GetGridviewDefaultFromSijiLevel_L().Concat( GetGridviewDefaultFromSijiLevel_R() ).ToArray();
            // 検索結果取得
            // エラー時にも最後まで処理を通すため、インスタンスを生成しておく
            var result = new OrderBusiness.ResultSet();
            DataTable tblResult = null;
            // 検索上限数を取得する
            int maxGridViewCount = WebAppInstance.GetInstance().Config.WebCommonInfo.searchOrderInfoMaxGridViewCount;
            try {
                //検索を実行する
                result = OrderBusiness.SearchOfSearchOrderInfo( ref dicCondition, gridviewDefineTmp, maxGridViewCount );
            } catch ( DataAccessException ex ) {
                if ( ex.OracleErrorNumber == DataAccessBase.ORAERROR_TIMEOUT ) {
                    // クエリ発行タイムアウトが発生した場合、エラーメッセージを設定する
                    base.WriteApplicationMessage( MsgManager.MESSAGE_WRN_61910 );
                } else {
                    // タイムアウト以外のExceptionが発生した場合、エラーメッセージを設定する
                    Logger.Exception( ex );
                    base.WriteApplicationMessage( MsgManager.MESSAGE_ERR_80011, "検索処理で" );
                }
            } catch ( Exception ex ) {
                // Exceptionが発生した場合、エラーメッセージを設定する
                Logger.Exception( ex );
                base.WriteApplicationMessage( MsgManager.MESSAGE_ERR_84010 );
            } finally {
                // 加工した検索条件を反映する
                txtKiban.Value = dicCondition[CONDITION.KIBAN.bindField].ToString();
            }
            ConditionInfoSessionHandler.ST_CONDITION cond = new ConditionInfoSessionHandler.ST_CONDITION();
            tblResult = result.ListTable;
            if ( true == ObjectUtils.IsNotNull( tblResult ) ) {
                // 検索結果が存在する場合、件数表示、ページャーの設定を行う
                ntbResultCount.Value = tblResult.Rows.Count;
                ControlUtils.SetGridViewPager( ref pnlPager, grvSearchOrderInfoRB, grvSearchOrderInfo_PageIndexChanging, tblResult.Rows.Count, 0 );
                // 検索条件/結果インスタンスを保持する
                cond.conditionValue = dicCondition;
                cond.IdWithText = dicIdWithText;
                cond.ResultData = tblResult.DefaultView.ToTable();
                cond.ResultDefine = gridviewDefineTmp;
            } else {
                // タイムアウト等Exception時には、GridViewクリア
                ClearGridView();
            }
            // 検索条件をセッションに格納する
            ConditionInfo = cond;
            // グリッドビューの表示処理を行う
            if ( true == ObjectUtils.IsNotNull( tblResult ) ) {
                // TemplateFieldの追加
                grvHeaderLT.Columns.Clear();
                grvHeaderRT.Columns.Clear();
                grvSearchOrderInfoLB.Columns.Clear();
                for ( int idx = 0; idx < GridviewDefault_L.Length; idx++ ) {
                    TemplateField tf = new TemplateField();
                    tf.HeaderText = StringUtils.ToString( GridviewDefault_L[idx].bindField );
                    grvSearchOrderInfoLB.Columns.Add( tf );
                }
                grvSearchOrderInfoRB.Columns.Clear();
                for ( int idx = 0; idx < GridviewDefault_R.Length; idx++ ) {
                    TemplateField tf = new TemplateField();
                    tf.HeaderText = StringUtils.ToString( GridviewDefault_R[idx].bindField );
                    grvSearchOrderInfoRB.Columns.Add( tf );
                }
                if ( 0 < tblResult.Rows.Count ) {
                    // 新規バインド
                    ControlUtils.ShowGridViewHeader( grvHeaderLT, GridviewDefault_L, ConditionInfo, true );
                    ControlUtils.ShowGridViewHeader( grvHeaderRT, GridviewDefault_R, ConditionInfo, true );
                    ControlUtils.BindGridView_WithTempField( grvSearchOrderInfoLB, GridviewDefault_L, tblResult );
                    ControlUtils.BindGridView_WithTempField( grvSearchOrderInfoRB, GridviewDefault_R, tblResult );
                    // GridView表示
                    divGrvDisplay.Visible = true;
                    // グリッドビュー外のDivサイズ変更
                    SetDivGridViewWidth();

                    // メッセージ設定
                    if ( null == result.Message ) {
                        // メッセージがない場合
                        if ( TRACTOR_FIX == DataUtils.GetDictionaryStringVal( dicCondition, CONDITION.SHIJI_LEVEL.bindField ) ) {
                            // 製品種別が本機確定の場合、直行率を計算する
                            var tyokkoDaisu = JudgeTyokko( tblResult );
                            var tyokkoRatio = tyokkoDaisu * 100 / tblResult.Rows.Count;
                            // メッセージ設定
                            result.Message = new Msg( MsgManager.MESSAGE_INF_50020, tblResult.Rows.Count, tyokkoDaisu, tyokkoRatio );
                        } else {
                            // メッセージ設定
                            result.Message = new Msg( MsgManager.MESSAGE_INF_50030, tblResult.Rows.Count );
                        }
                    }
                } else {
                    //一覧初期化
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
        /// 直行判定
        /// </summary>
        /// <param name="tblResult">処理結果</param>
        /// <returns>直行台数</returns>
        private int JudgeTyokko( DataTable tblResult ) {
            // 直行台数
            var tyokkoDaisu = 0;
            for ( int idx = 0; idx < tblResult.Rows.Count; idx++ ) {
                // 処理結果をループ
                if ( TYOKKO == NumericUtils.ToInt( tblResult.Rows[idx][GRID_SEARCHORDERINFO_TRACTOR_FIX_L.TYOKKO_SIGN.bindField].ToString() ) ) {
                    // 直行台数が1の場合
                    tyokkoDaisu++;
                }
            }
            return tyokkoDaisu;
        }

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
                if ( true == GetColumnIndex( sender, GRID_SEARCHORDERINFO_TRACTOR_FIX_L.DISP_ORDER, out index ) ) {
                    // NOの場合、連番を振る
                    var data = e.Row.RowIndex + 1 + sender.PageIndex * sender.PageSize;
                    ( (Label)e.Row.Cells[index].Controls[0] ).Text = data.ToString();
                }
                if ( true == GetColumnIndex( sender, GRID_SEARCHORDERINFO_TRACTOR_FIX_L.SHIJI_YM_NUM, out index ) ) {
                    // 確定月度連番の場合、yyyyMM-nnnnn形式に変換する（変換できない場合は空文字）
                    var data = ( (DataRowView)e.Row.DataItem ).Row[GRID_SEARCHORDERINFO_TRACTOR_FIX_L.SHIJI_YM_NUM.bindField].ToString();
                    ( (Label)e.Row.Cells[index].Controls[0] ).Text = OrderBusiness.getDisplayYMNum( data );
                }
                if ( true == GetColumnIndex( sender, GRID_SEARCHORDERINFO_TRACTOR_FIX_L.SHIJI_YM_NUM_FOR_KAKUTEI, out index ) ) {
                    // 指示月度連番の場合、yyyyMM-nnnnn形式に変換する（変換できない場合は空文字）
                    var data = ( (DataRowView)e.Row.DataItem ).Row[GRID_SEARCHORDERINFO_TRACTOR_FIX_L.SHIJI_YM_NUM_FOR_KAKUTEI.bindField].ToString();
                    ( (Label)e.Row.Cells[index].Controls[0] ).Text = OrderBusiness.getDisplayYMNum( data );
                }
                if ( true == GetColumnIndex( sender, GRID_SEARCHORDERINFO_TRACTOR_FIX_L.KATASHIKI_CODE, out index ) ) {
                    // 型式コードの場合、XXXXX-XXXXX形式に変換する（変換できない場合は空文字）
                    var data = ( (DataRowView)e.Row.DataItem ).Row[GRID_SEARCHORDERINFO_TRACTOR_FIX_L.KATASHIKI_CODE.bindField].ToString();
                    ( (Label)e.Row.Cells[index].Controls[0] ).Text = OrderBusiness.getDisplayKatasikiCode( data );
                }
                if ( true == GetColumnIndex( sender, GRID_SEARCHORDERINFO_TRACTOR_FIX_R.ENGINE_KATASHIKI_CODE, out index ) ) {
                    // E型式コードの場合、XXXXX-XXXXX形式に変換する（変換できない場合は空文字）
                    var data = ( (DataRowView)e.Row.DataItem ).Row[GRID_SEARCHORDERINFO_TRACTOR_FIX_R.ENGINE_KATASHIKI_CODE.bindField].ToString();
                    ( (Label)e.Row.Cells[index].Controls[0] ).Text = OrderBusiness.getDisplayKatasikiCode( data );
                }
                if ( true == GetColumnIndex( sender, GRID_SEARCHORDERINFO_TRACTOR_FIX_R.KEIKAKU_DAISU, out index ) ) {
                    // 計画台数の場合、累計台数/計画台数に変換する
                    var keikaku = ( (DataRowView)e.Row.DataItem ).Row[GRID_SEARCHORDERINFO_TRACTOR_FIX_R.KEIKAKU_DAISU.bindField].ToString();
                    var ruikei = ( (DataRowView)e.Row.DataItem ).Row[GRID_SEARCHORDERINFO_TRACTOR_FIX_R.RUIKEI_DAISU.bindField].ToString();
                    ( (Label)e.Row.Cells[index].Controls[0] ).Text = ruikei + SLASH + keikaku;
                }
                if ( true == GetColumnIndex( sender, GRID_SEARCHORDERINFO_TRACTOR_FIX_L.KATASHIKI_NAME, out index ) ) {
                    // 型式名の場合、型式名<br />ベース型式名に変換する
                    var katashikiName = ( (DataRowView)e.Row.DataItem ).Row[GRID_SEARCHORDERINFO_TRACTOR_FIX_L.KATASHIKI_NAME.bindField].ToString();
                    var baseKatashikiName = ( (DataRowView)e.Row.DataItem ).Row[GRID_SEARCHORDERINFO_TRACTOR_FIX_L.BASE_KATASHIKI_NAME.bindField].ToString();
                    ( (Label)e.Row.Cells[index].Controls[0] ).Text = katashikiName + BREAK + SPACE_HTML + SPACE_HTML + COLOR_GRAY + OPEN_BRACKETS + baseKatashikiName + CLOSE_BRACKETS + END_SPAN;
                }
                if ( true == GetColumnIndex( sender, GRID_SEARCHORDERINFO_TRACTOR_FIX_L.TYOKKO_SIGN, out index ) ) {
                    // 直通の場合
                    var data = ( (DataRowView)e.Row.DataItem ).Row[GRID_SEARCHORDERINFO_TRACTOR_FIX_L.TYOKKO_SIGN.bindField].ToString();
                    if ( TYOKKO.ToString() == data ) {
                        // 直通が1の場合、○に変換する
                        ( (Label)e.Row.Cells[index].Controls[0] ).Text = CIRCLE;
                        // 背景色を#E0F0E0に設定
                        e.Row.Cells[index].BackColor = System.Drawing.Color.FromArgb( 224, 240, 224 );
                    } else {
                        // 直通が1以外の場合、-に変換する
                        ( (Label)e.Row.Cells[index].Controls[0] ).Text = HYPHEN;
                    }
                }
                if ( true == GetColumnIndex( sender, GRID_SEARCHORDERINFO_TRACTOR_FIX_R.CREATE_DATE, out index ) ) {
                    // 作成日の場合、yy/MM/dd HH:mm:ss形式に変換する
                    var data = ( (DataRowView)e.Row.DataItem ).Row[GRID_SEARCHORDERINFO_TRACTOR_FIX_R.CREATE_DATE.bindField].ToString();
                    ( (Label)e.Row.Cells[index].Controls[0] ).Text = OrderBusiness.convertDatetimeToYYMDH( data );
                }

                // 選択行の背景色変更を追加
                ControlUtils.GridViewRowBound( sender, e, GRID_ORDER_SEARCH_ORDER_INFO_CD, ControlUtils.GridRowDoubleClickEvent.None );

                if ( true == GetColumnIndex( sender, GRID_SEARCHORDERINFO_TRACTOR_FIX_L.KATASHIKI_NAME, out index ) ) {
                    // 型式名の場合、ツールチップを追加
                    var katashikiName = ( (DataRowView)e.Row.DataItem ).Row[GRID_SEARCHORDERINFO_TRACTOR_FIX_L.KATASHIKI_NAME.bindField].ToString();
                    var baseKatashikiName = ( (DataRowView)e.Row.DataItem ).Row[GRID_SEARCHORDERINFO_TRACTOR_FIX_L.BASE_KATASHIKI_NAME.bindField].ToString();
                    e.Row.Cells[index].ToolTip = katashikiName + BREAK_CODE + SPACE_CHAR + SPACE_CHAR + OPEN_BRACKETS + baseKatashikiName + CLOSE_BRACKETS;
                }
            }
        }

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

                // 製品種別
                condition = CONDITION.SHIJI_LEVEL.displayNm;
                value = cond.IdWithText[CONDITION.SHIJI_LEVEL.controlId];
                excelCond.Add( new ExcelConditionItem( condition, value ) );

                // IDNO
                condition = CONDITION.IDNO.displayNm;
                value = cond.IdWithText[CONDITION.IDNO.controlId];
                excelCond.Add( new ExcelConditionItem( condition, value ) );

                // 機番
                condition = CONDITION.KIBAN.displayNm;
                value = cond.IdWithText[CONDITION.KIBAN.controlId];
                excelCond.Add( new ExcelConditionItem( condition, value ) );

                // 月度
                condition = CONDITION.SHIJI_YM.displayNm;
                value = cond.IdWithText[CONDITION.SHIJI_YM.controlId];
                excelCond.Add( new ExcelConditionItem( condition, value ) );

                // 型式コード
                condition = CONDITION.KATASHIKI_CODE.displayNm;
                value = cond.IdWithText[CONDITION.KATASHIKI_CODE.controlId];
                excelCond.Add( new ExcelConditionItem( condition, value ) );

                // 国コード
                condition = CONDITION.KUNI_CODE.displayNm;
                value = cond.IdWithText[CONDITION.KUNI_CODE.controlId];
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
                Excel.Download( Response, "順序情報", GetExcelTable( cond.ResultData ), excelCond );
            } catch ( System.Threading.ThreadAbortException ) {
                // response.Endで必ず発生する為、正常として扱う
            } catch ( Exception ex ) {
                // 例外発生時、ログ出力とメッセージ表示
                Logger.Exception( ex );
                base.WriteApplicationMessage( MsgManager.MESSAGE_ERR_80030, "順序情報_検索結果" );
            }
        }

        /// <summary>
        /// Excel出力用データテーブル作成
        /// </summary>
        /// <param name="tblSource">検索結果</param>
        /// <returns>DataTable</returns>
        private DataTable GetExcelTable( DataTable tblSource ) {
            // 2021/12/16 toyoshima 開始
            // EIDNO,E機番のItem2を修正
            // 出力対象列(Item1 = 表示用ヘッダ、Item2 = 列名)
            var outputHeaderList = new List<Tuple<string, string>>() {
                new Tuple<string, string>("確定月度連番","MS_YYMM_NO"),
                new Tuple<string, string>("指示月度連番","指示月度連番"),
                new Tuple<string, string>("直通","直通"),
                new Tuple<string, string>("IDNO","MS_IDNO"),
                new Tuple<string, string>("型式ｺｰﾄﾞ","MS_B_KATA_C"),
                new Tuple<string, string>("国ｺｰﾄﾞ","MS_B_KUNI_C"),
                new Tuple<string, string>("型式名","MS_B_KATA_N"),
                new Tuple<string, string>("機番","MS_KIBAN"),
                new Tuple<string, string>("特記事項","MS_TOKKIJIKOU"),
                new Tuple<string, string>("完成予定日","MS_KAN_YYMMDD"),
                new Tuple<string, string>("完成日","完成日時"),
                new Tuple<string, string>("出荷日","出荷日"),
                new Tuple<string, string>("E型式ｺｰﾄﾞ","MS_E_KATA_C"),
                new Tuple<string, string>("E型式名","MS_E_KATA_N"),
                new Tuple<string, string>("E-IDNO","エンジンIDNO"),
                new Tuple<string, string>("E機番","エンジン機番"),
                new Tuple<string, string>("ﾊﾟﾀﾝ","MS_K_PATAN"),
                new Tuple<string, string>("Eﾊﾟﾀﾝ","エンジン組立パターン"),
                new Tuple<string, string>("指示ﾚﾍﾞﾙ","MS_SIJI_LVL"),
                new Tuple<string, string>("累計台数","型式別累計台数"),
                new Tuple<string, string>("計画台数","型式別計画台数"),
                new Tuple<string, string>("作成日","MS_INS_YMD"),
            };
            // 2021/12/16 toyoshima 終了
            // 出力結果テーブル
            DataTable tblResult = new DataTable();
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
            var srcColumnNames = tblSource.Columns.Cast<DataColumn>().Select( c => c.ColumnName );
            foreach ( DataRow rowSrc in tblSource.Rows ) {
                DataRow rowTo = tblResult.NewRow();
                foreach ( var outputHeader in outputHeaderList ) {
                    if ( srcColumnNames.Contains( outputHeader.Item2 ) == false ) {
                        continue;
                    }
                    rowTo[outputHeader.Item2] = rowSrc[outputHeader.Item2];
                    if ( outputHeader.Item2 == GRID_SEARCHORDERINFO_TRACTOR_FIX_L.SHIJI_YM_NUM.bindField ) {
                        // 指示月度連番の変換を実行
                        rowTo[outputHeader.Item2] = OrderBusiness.getDisplayYMNum( rowSrc[outputHeader.Item2].ToString() );
                    } else if ( outputHeader.Item2 == GRID_SEARCHORDERINFO_TRACTOR_FIX_L.SHIJI_YM_NUM_FOR_KAKUTEI.bindField ) {
                        // 確定月度連番の変換を実行
                        rowTo[outputHeader.Item2] = OrderBusiness.getDisplayYMNum( rowSrc[outputHeader.Item2].ToString() );
                    } else if ( outputHeader.Item2 == GRID_SEARCHORDERINFO_TRACTOR_FIX_L.KATASHIKI_CODE.bindField ) {
                        // 型式コードの変換を実行
                        rowTo[outputHeader.Item2] = OrderBusiness.getDisplayKatasikiCode( rowSrc[outputHeader.Item2].ToString() );
                    } else if ( outputHeader.Item2 == GRID_SEARCHORDERINFO_TRACTOR_FIX_R.ENGINE_KATASHIKI_CODE.bindField ) {
                        // E型式コードの変換を実行
                        rowTo[outputHeader.Item2] = OrderBusiness.getDisplayKatasikiCode( rowSrc[outputHeader.Item2].ToString() );
                    }
                }
                tblResult.Rows.Add( rowTo );
            }
            tblResult.AcceptChanges();

            return tblResult;
        }

        /// <summary>
        /// オプションボタン制御
        /// </summary>
        private void ChangeShijiLevel() {
            if ( ID_KIBAN_SEARCH == rblShijiLevel.SelectedValue ) {
                // 製品種別 = ID/機番検索
                ddlShijiYM.Enabled = true;
                txtKatashikiCode.Enabled = false;
                txtKuniCode.Enabled = false;
                txtKatashikiName.Enabled = false;
                txtTokki.Enabled = false;
            } else {
                // 製品種別 = ID/機番検索以外
                ddlShijiYM.Enabled = true;
                txtKatashikiCode.Enabled = true;
                txtKuniCode.Enabled = true;
                txtKatashikiName.Enabled = true;
                txtTokki.Enabled = true;
            }
        }

        #endregion

        #region グリッドビュー操作
        /// <summary>
        /// グリッドビュークリア
        /// </summary>
        private void ClearGridView() {
            // 列名非表示 グリッドビュークリア
            ControlUtils.InitializeGridView( grvHeaderLT, false );
            ControlUtils.InitializeGridView( grvHeaderRT, false );
            ControlUtils.InitializeGridView( grvSearchOrderInfoLB, false );
            ControlUtils.InitializeGridView( grvSearchOrderInfoRB, false );
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
            int pageSize = grvSearchOrderInfoRB.PageSize;
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
            ControlUtils.BindGridView_WithTempField( grvSearchOrderInfoLB, GridviewDefault_L, ConditionInfo.ResultData );
            ControlUtils.BindGridView_WithTempField( grvSearchOrderInfoRB, GridviewDefault_R, ConditionInfo.ResultData );
            ControlUtils.GridViewPageIndexChanging( grvSearchOrderInfoLB, ConditionInfo.ResultData, new GridViewPageEventArgs( newPageIndex ) );
            ControlUtils.GridViewPageIndexChanging( grvSearchOrderInfoRB, ConditionInfo.ResultData, new GridViewPageEventArgs( newPageIndex ) );
            ControlUtils.SetGridViewPager( ref pnlPager, grvSearchOrderInfoRB, grvSearchOrderInfo_PageIndexChanging, ConditionInfo.ResultData.Rows.Count, grvSearchOrderInfoRB.PageIndex );
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
            ControlUtils.GridViewSorting( grvSearchOrderInfoLB, ref cond, e, true );
            ControlUtils.GridViewSorting( grvSearchOrderInfoRB, ref cond, e );
            // 背面ユーザ切替対応
            ControlUtils.ShowGridViewHeader( grvHeaderLT, GridviewDefault_L, cond, true );
            ControlUtils.ShowGridViewHeader( grvHeaderRT, GridviewDefault_R, cond, true );
            ControlUtils.BindGridView_WithTempField( grvSearchOrderInfoLB, GridviewDefault_L, ConditionInfo.ResultData );
            ControlUtils.BindGridView_WithTempField( grvSearchOrderInfoRB, GridviewDefault_R, ConditionInfo.ResultData );
            ControlUtils.SetGridViewPager( ref pnlPager, grvSearchOrderInfoRB, grvSearchOrderInfo_PageIndexChanging, cond.ResultData.Rows.Count, grvSearchOrderInfoRB.PageIndex );
            ConditionInfo = cond;
            // グリッドビュー外のDivサイズ変更
            SetDivGridViewWidth();
        }

        /// <summary>
        /// 列番号取得
        /// </summary>
        /// <param name="target">確認対象のグリッドビュー</param>
        /// <param name="def">確認する列定義</param>
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
            SetDivGridViewWidth( grvSearchOrderInfoLB, divGrvLB );
            SetDivGridViewWidth( grvSearchOrderInfoRB, divGrvRB );
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

        ///// <summary>
        ///// セッション情報からGridviewDefaultを作成（固定列）
        ///// </summary>
        private GridViewDefine[] GetGridviewDefaultFromSession_L() {
            var sessionData = base.ConditionInfo.ResultDefine;
            var sdBindField = sessionData.Select( gvd => gvd.bindField );
            var idKibBindField = ControlUtils.GetGridViewDefineArray( typeof( GRID_SEARCHORDERINFO_ID_KIBAN_L ) ).Concat( ControlUtils.GetGridViewDefineArray( typeof( GRID_SEARCHORDERINFO_ID_KIBAN_R ) ) ).Select( gvd => gvd.bindField );
            var trFixBindField = ControlUtils.GetGridViewDefineArray( typeof( GRID_SEARCHORDERINFO_TRACTOR_FIX_L ) ).Concat( ControlUtils.GetGridViewDefineArray( typeof( GRID_SEARCHORDERINFO_TRACTOR_FIX_R ) ) ).Select( gvd => gvd.bindField );
            var otherBindField = ControlUtils.GetGridViewDefineArray( typeof( GRID_SEARCHORDERINFO_OTHER_L ) ).Concat( ControlUtils.GetGridViewDefineArray( typeof( GRID_SEARCHORDERINFO_OTHER_R ) ) ).Select( gvd => gvd.bindField );
            if ( true == sdBindField.SequenceEqual( idKibBindField ) ) {
                return ControlUtils.GetGridViewDefineArray( typeof( GRID_SEARCHORDERINFO_ID_KIBAN_L ) );
            } else if ( true == sdBindField.SequenceEqual( trFixBindField ) ) {
                return ControlUtils.GetGridViewDefineArray( typeof( GRID_SEARCHORDERINFO_TRACTOR_FIX_L ) );
            } else if ( true == sdBindField.SequenceEqual( otherBindField ) ) {
                return ControlUtils.GetGridViewDefineArray( typeof( GRID_SEARCHORDERINFO_OTHER_L ) );
            } else {
                return new GridViewDefine[0];
            }
        }

        ///// <summary>
        ///// セッション情報からGridviewDefaultを作成（可変列）
        ///// </summary>
        private GridViewDefine[] GetGridviewDefaultFromSession_R() {
            var sessionData = base.ConditionInfo.ResultDefine;
            var sdBindField = sessionData.Select( gvd => gvd.bindField );
            var idKibBindField = ControlUtils.GetGridViewDefineArray( typeof( GRID_SEARCHORDERINFO_ID_KIBAN_L ) ).Concat( ControlUtils.GetGridViewDefineArray( typeof( GRID_SEARCHORDERINFO_ID_KIBAN_R ) ) ).Select( gvd => gvd.bindField );
            var trFixBindField = ControlUtils.GetGridViewDefineArray( typeof( GRID_SEARCHORDERINFO_TRACTOR_FIX_L ) ).Concat( ControlUtils.GetGridViewDefineArray( typeof( GRID_SEARCHORDERINFO_TRACTOR_FIX_R ) ) ).Select( gvd => gvd.bindField );
            var otherBindField = ControlUtils.GetGridViewDefineArray( typeof( GRID_SEARCHORDERINFO_OTHER_L ) ).Concat( ControlUtils.GetGridViewDefineArray( typeof( GRID_SEARCHORDERINFO_OTHER_R ) ) ).Select( gvd => gvd.bindField );
            if ( true == sdBindField.SequenceEqual( idKibBindField ) ) {
                return ControlUtils.GetGridViewDefineArray( typeof( GRID_SEARCHORDERINFO_ID_KIBAN_R ) );
            } else if ( true == sdBindField.SequenceEqual( trFixBindField ) ) {
                return ControlUtils.GetGridViewDefineArray( typeof( GRID_SEARCHORDERINFO_TRACTOR_FIX_R ) );
            } else if ( true == sdBindField.SequenceEqual( otherBindField ) ) {
                return ControlUtils.GetGridViewDefineArray( typeof( GRID_SEARCHORDERINFO_OTHER_R ) );
            } else {
                return new GridViewDefine[0];
            }
        }


        ///// <summary>
        ///// 指示レベルからGridviewDefaultを作成（固定列）
        ///// </summary>
        private GridViewDefine[] GetGridviewDefaultFromSijiLevel_L() {
            Dictionary<string, object> dicCondition = new Dictionary<string, object>();
            base.GetControlValues( ConditionControls, ref dicCondition );
            var shijiLevel = DataUtils.GetDictionaryStringVal( dicCondition, CONDITION.SHIJI_LEVEL.bindField );
            switch ( shijiLevel ) {
            // 製品種別の値で分岐する
            case ID_KIBAN_SEARCH:
                // 製品種別がID/機番検索の場合
                return ControlUtils.GetGridViewDefineArray( typeof( GRID_SEARCHORDERINFO_ID_KIBAN_L ) );
            case TRACTOR_FIX:
                // 製品種別が本機確定の場合
                return ControlUtils.GetGridViewDefineArray( typeof( GRID_SEARCHORDERINFO_TRACTOR_FIX_L ) );
            default:
                // 製品種別がそれ以外の場合
                return ControlUtils.GetGridViewDefineArray( typeof( GRID_SEARCHORDERINFO_OTHER_L ) );
            }
        }


        ///// <summary>
        ///// 指示レベルからGridviewDefaultを作成（可変列）
        ///// </summary>
        private GridViewDefine[] GetGridviewDefaultFromSijiLevel_R() {
            Dictionary<string, object> dicCondition = new Dictionary<string, object>();
            base.GetControlValues( ConditionControls, ref dicCondition );
            var shijiLevel = DataUtils.GetDictionaryStringVal( dicCondition, CONDITION.SHIJI_LEVEL.bindField );
            switch ( shijiLevel ) {
            // 製品種別の値で分岐する
            case ID_KIBAN_SEARCH:
                // 製品種別がID/機番検索の場合
                return ControlUtils.GetGridViewDefineArray( typeof( GRID_SEARCHORDERINFO_ID_KIBAN_R ) );
            case TRACTOR_FIX:
                // 製品種別が本機確定の場合
                return ControlUtils.GetGridViewDefineArray( typeof( GRID_SEARCHORDERINFO_TRACTOR_FIX_R ) );
            default:
                // 製品種別がそれ以外の場合
                return ControlUtils.GetGridViewDefineArray( typeof( GRID_SEARCHORDERINFO_OTHER_R ) );
            }
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
    }
}