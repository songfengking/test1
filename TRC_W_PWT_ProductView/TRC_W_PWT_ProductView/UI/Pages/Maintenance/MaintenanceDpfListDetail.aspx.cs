using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Reflection;
using KTFramework.Common;
using KTFramework.Dao;
using KTWebInheritance.Common;
using TRC_W_PWT_ProductView.Common;
using TRC_W_PWT_ProductView.UI.Base;
using TRC_W_PWT_ProductView.Session;
using TRC_W_PWT_ProductView.Defines;
using TRC_W_PWT_ProductView.Defines.ListDefine;
using TRC_W_PWT_ProductView.Defines.TypeDefine;
using TRC_W_PWT_ProductView.Business;
using TRC_W_PWT_ProductView.Dao.Process;
using TRC_W_PWT_ProductView.UI.Pages.Maintenance;
using TRC_W_PWT_ProductView.UI.Pages.UserControl;
using KTFramework.Web.Client.SvcCore;

namespace TRC_W_PWT_ProductView.UI.Pages {

    /// <summary>
    /// チェック対象外リスト明細画面
    /// </summary>
    public partial class MaintenanceDpfListDetail : BaseForm {

        //ロガー定義
        private static readonly Logger logger = new Logger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

        #region 固定値
        /// <summary>
        /// グリッドビューグループ
        /// </summary>
        const string GRID_MAIN_VIEW_GROUP_CD = "MainView";

        const string CONST_IN_PRODUCT = "投入順序";
        const string CONST_REGISTERED = "登録済";
        const string CONST_PARTS = "部品";
        const string NOT_FOUND_DTL_DATA = "排ガス規制部品詳細";

        //検索区分
        const string CONST_ORDER_NO = "0";      //投入順序
        const string CONST_DPF_LIST = "1";      //登録済

        //ラインコード
        const string CONST_LINE_OEM = "003060"; //OEM
        const string CONST_LINE_HS = "002150";  //HS

        //ステーション
        const string CONST_ST_OEM = "306310";   //OEM
        const string CONST_ST_HS = "215227";    //HS

        //部品区分
        const string CONST_PARTS_DPF = "DPF";  //DPF
        const string CONST_PARTS_DOC = "DOC";  //DOC
        const string CONST_PARTS_SCR = "SCR";  //SCR
        const string CONST_PARTS_ACU = "ACU";  //ACU

        //検索種別
        const string CONST_SEARCH_DPF = "DPF";  //ACU

        #endregion

        Dictionary<string, object> dicControlValues = new Dictionary<string, object>();
        #region メンバ変数
        Dictionary<String, String> _dicEmp = new Dictionary<String, String>();
        public static GridViewDefine[] GridViewResults = null;
        #endregion


        #region 検索条件定義
        /// <summary>
        /// 一覧検索条件定義
        /// </summary>
        public class CONDITION {
            /// <summary>型式コード</summary>
            public static readonly ControlDefine MODEL_CD = new ControlDefine( "txtModelCd", "型式コード", "modelCd", ControlDefine.BindType.Both, typeof( String ) );
            /// <summary>製品機番</summary>
            public static readonly ControlDefine SERIAL_NO = new ControlDefine( "txtSerial", "製品機番", "serial", ControlDefine.BindType.Both, typeof( String ) );
            /// <summary>パターンコード</summary>
            public static readonly ControlDefine PTN_CD = new ControlDefine( "txtPtnCd", "パターンコード", "productKind", ControlDefine.BindType.Both, typeof( String ) );
        }
        #endregion

        #region グリッドビュー定義

        #region ヘッダ
        public class MODEL_INFO_PRODUCT {
            /// <summary>生産型式コード</summary>
            public static readonly ControlDefine MODEL_CD = new ControlDefine( "txtModelCd", "型式コード", "MODEL_CD", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>生産型式名</summary>
            public static readonly ControlDefine MODEL_NM = new ControlDefine( "txtModelNm", "型式名", "MODEL_NM", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>機番</summary>
            public static readonly ControlDefine SERIAL_NO = new ControlDefine( "txtSerial", "製品機番", "SERIAL_NO", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>生産国</summary>
            public static readonly ControlDefine COUNTRY = new ControlDefine( "txtCountry", "生産国", "COUNTRY", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>完成日</summary>
            public static readonly ControlDefine FIN_DT = new ControlDefine( "txtFinDt", "完成日", "FIN_DT", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>パターンコード</summary>
            public static readonly ControlDefine PTN_CD = new ControlDefine( "txtPtnCd", "パターンコード", "PTN_CD", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>ライン</summary>
            public static readonly ControlDefine LINE_CD = new ControlDefine( "txtLineCd", "ライン", "LINE_CD", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>ステーション</summary>
            public static readonly ControlDefine ST = new ControlDefine( "txtST", "ステーション", "ST", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>IDNO</summary>
            public static readonly ControlDefine IDNO = new ControlDefine( "txtIDNO", "IDNO", "IDNO", ControlDefine.BindType.Down, typeof( String ) );
        }
        /// <summary>
        /// 型式情報欄定義(搭載エンジン情報)
        /// </summary>
        public class MODEL_INFO_ENG {
            /// <summary>生産型式コード</summary>
            public static readonly ControlDefine PRODUCT_MODEL_CD_STR = new ControlDefine( "txtEngModelCd", "生産型式コード", "ENG_MODEL_CD", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>生産型式名</summary>
            public static readonly ControlDefine PRODUCT_MODEL_NM = new ControlDefine( "txtEngModelNm", "型式名", "ENG_MODEL_NM", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>国コード</summary>
            public static readonly ControlDefine PRODUCT_COUNTRY_CD = new ControlDefine( "txtEngCountry", "生産国コード", "ENG_COUNTRY", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>機番</summary>
            public static readonly ControlDefine SERIAL = new ControlDefine( "txtEngSerial", "機番", "ENG_SERIAL_NO", ControlDefine.BindType.Down, typeof( String ) );
        }
        /// <summary>
        /// 型式情報欄定義(搭載エンジン情報)
        /// </summary>
        public class MODEL_INFO_TRC {
            /// <summary>生産型式コード</summary>
            public static readonly ControlDefine PRODUCT_MODEL_CD_STR = new ControlDefine( "txtTrcModelCd", "生産型式コード", "TRC_MODEL_CD", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>生産型式名</summary>
            public static readonly ControlDefine PRODUCT_MODEL_NM = new ControlDefine( "txtTrcModelNm", "型式名", "TRC_MODEL_NM", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>国コード</summary>
            public static readonly ControlDefine PRODUCT_COUNTRY_CD = new ControlDefine( "txtTrcCountry", "生産国コード", "TRC_COUNTRY", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>機番</summary>
            public static readonly ControlDefine SERIAL = new ControlDefine( "txtTrcSerial", "機番", "TRC_SERIAL_NO", ControlDefine.BindType.Down, typeof( String ) );
        }
        #endregion
        
        #region 生産中
        /// <summary>
        /// 検索区分：生産中
        /// </summary>
        internal class GRID_IN_PRODUCT {
            /// <summary>型式コード</summary>
            public static readonly GridViewDefine MODEL_CD = new GridViewDefine( "型式コード", "MODEL_CD", typeof( string ), "", true, HorizontalAlign.Center, 140, true );
            /// <summary>型式名</summary>
            public static readonly GridViewDefine MODEL_NM = new GridViewDefine( "型式名", "MODEL_NM", typeof( string ), "", true, HorizontalAlign.Left, 200, true );
            /// <summary>機番</summary>
            public static readonly GridViewDefine SERIAL_NO = new GridViewDefine( "機番", "SERIAL_NO", typeof( string ), "", true, HorizontalAlign.Center, 100, true );
            /// <summary>完成予定日</summary>
            public static readonly GridViewDefine KAN_YO_YM = new GridViewDefine( "完成予定日", "KAN_YO_YM", typeof( string ), "", true, HorizontalAlign.Center, 140, true );
            /// <summary>月度連番</summary>
            public static readonly GridViewDefine DATA_CNT = new GridViewDefine( "ﾁｪｯｸ対象外登録", "DATA_CNT", typeof( string ), "", true, HorizontalAlign.Center, 140, true );

        }

        /// <summary>
        /// 一覧表示情報コントロール(左下)
        /// </summary>
        public class GRID_SEARCH_CONTROLS_L {
            public static readonly ControlDefine UPD_CHECK = new ControlDefine( "chkUpdate", "修正", "", ControlDefine.BindType.None, null );
        }
        /// <summary>チェックボックス</summary>

        /// <summary>
        /// 一覧表示情報コントロール(右下)
        /// </summary>
        public class GRID_SEARCH_CONTROLS_R {
        }
        /// <summary>
        /// 検索結果(更新時共通部)
        /// </summary>
        /// 
        internal class GRID_COMMON {
            /// <suumary>取付日時</summary>
            public static readonly GridViewDefine ATTACHMENT_DT = new GridViewDefine( "取付日時", "ATTACHMENT_DT", typeof( string ), "", true, HorizontalAlign.Center, 140, true, true );
            /// <suumary>部品区分</summary>
            public static readonly GridViewDefine PARTS_KBN = new GridViewDefine( "部品名", "PARTS_KBN", typeof( string ), "", true, HorizontalAlign.Center, 100, true, true );
            /// <suumary>抜取検査</summary>
            public static readonly GridViewDefine SAMPLE_CHECK = new GridViewDefine( "抜取検査", "SAMPLE_CHECK", typeof( string ), "", true, HorizontalAlign.Center, 100, true, true );
            /// <suumary>型式コード</summary>
            public static readonly GridViewDefine DPF_MODEL_CD = new GridViewDefine( "部品型式", "DPF_MODEL_CD", typeof( string ), "", true, HorizontalAlign.Center, 150, true, true );
            /// <suumary>機番</summary>
            public static readonly GridViewDefine DPF_SERIAL_NO = new GridViewDefine( "部品機番", "DPF_SERIAL_NO", typeof( string ), "", true, HorizontalAlign.Left, 160, true, true );
            /// <suumary>ライン</summary>
            public static readonly GridViewDefine LINE_CD = new GridViewDefine( "ライン", "LINE_CD", typeof( string ), "", true, HorizontalAlign.Center, 0, false, true );
            /// <suumary>ステーション</summary>
            public static readonly GridViewDefine ST = new GridViewDefine( "ステーション", "ST", typeof( string ), "", true, HorizontalAlign.Center, 0, false, true );

        }

        /// <summary>
        /// トラクタ
        /// </summary>
        internal class GRID_TRACTOR {
            /// <suumary>IDNO</summary>
            public static readonly GridViewDefine TRC_IDNO = new GridViewDefine( "ﾄﾗｸﾀIDNO", "TRC_IDNO", typeof( string ), "", true, HorizontalAlign.Center, 120, true, true );
            /// <suumary>型式コード</summary>
            public static readonly GridViewDefine TRC_MODEL_CD = new GridViewDefine( "ﾄﾗｸﾀ型式", "TRC_MODEL_CD", typeof( string ), "", true, HorizontalAlign.Center, 130, true, true );
            /// <suumary>型式名</summary>
            public static readonly GridViewDefine TRC_MODEL_NM = new GridViewDefine( "ﾄﾗｸﾀ型式名", "TRC_MODEL_NM", typeof( string ), "", true, HorizontalAlign.Left, 160, true, true );
            /// <suumary>国コード</summary>
            public static readonly GridViewDefine TRC_COUNTRY = new GridViewDefine( "ﾄﾗｸﾀ国", "TRC_COUNTRY", typeof( string ), "", true, HorizontalAlign.Center, 90, true, true );
            /// <suumary>機番</summary>
            public static readonly GridViewDefine TRC_SERIAL_NO = new GridViewDefine( "ﾄﾗｸﾀ機番", "TRC_SERIAL_NO", typeof( string ), "", true, HorizontalAlign.Center, 110, true, true );
        }

        /// <summary>
        /// エンジン
        /// </summary>
        internal class GRID_ENGINE {
            /// <suumary>IDNO</summary>
            public static readonly GridViewDefine ENG_IDNO = new GridViewDefine( "ｴﾝｼﾞﾝIDNO", "ENG_IDNO", typeof( string ), "", true, HorizontalAlign.Center, 120, true, true );
            /// <suumary>型式コード</summary>
            public static readonly GridViewDefine ENG_MODEL_CD = new GridViewDefine( "ｴﾝｼﾞﾝ型式", "ENG_MODEL_CD", typeof( string ), "", true, HorizontalAlign.Center, 130, true, true );
            /// <suumary>型式名</summary>
            public static readonly GridViewDefine ENG_MODEL_NM = new GridViewDefine( "ｴﾝｼﾞﾝ型式名", "ENG_MODEL_NM", typeof( string ), "", true, HorizontalAlign.Left, 160, true, true );
            /// <suumary>機番</summary>
            public static readonly GridViewDefine ENG_SERIAL_NO = new GridViewDefine( "ｴﾝｼﾞﾝ機番", "ENG_SERIAL_NO", typeof( string ), "", true, HorizontalAlign.Center, 110, true, true );
        }
        /// <summary>
        /// 共通部
        /// </summary>
        internal class GRID_COMMON_TAIL {
            /// <suumary>更新日時</summary>
            public static readonly GridViewDefine UPDATE_DT = new GridViewDefine( "更新日時", "UPDATE_DT", typeof( string ), "", true, HorizontalAlign.Center, 150, true, true );
            /// <suumary>更新者</summary>
            public static readonly GridViewDefine UPDATE_BY = new GridViewDefine( "更新者", "UPDATE_BY", typeof( string ), "", true, HorizontalAlign.Left, 140, true, true );
        }
       #endregion

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
        /// ヘッダ情報
        /// </summary>
        ControlDefine[] _HeaderControls = null;
        /// <summary>
        /// ヘッダ情報
        /// </summary>
        ControlDefine[] HeaderControls {
            get {
                if ( true == ObjectUtils.IsNull( _HeaderControls ) ) {
                    _HeaderControls = ControlUtils.GetControlDefineArray( typeof( MODEL_INFO_PRODUCT ) );
                }
                return _HeaderControls;
            }
        }
        /// <summary>
        /// エンジン情報
        /// </summary>
        ControlDefine[] _HeaderControlsEng = null;
        /// <summary>
        /// OEMヘッダ情報
        /// </summary>
        ControlDefine[] HeaderControlsEng {
            get {
                if ( true == ObjectUtils.IsNull( _HeaderControlsEng ) ) {
                    _HeaderControlsEng = ControlUtils.GetControlDefineArray( typeof( MODEL_INFO_ENG ) );
                }
                return _HeaderControlsEng;
            }
        }

        /// <summary>
        /// 本機情報
        /// </summary>
        ControlDefine[] _HeaderControlsTrc = null;
        /// <summary>
        /// 本機ヘッダ情報
        /// </summary>
        ControlDefine[] HeaderControlsTrc {
            get {
                if ( true == ObjectUtils.IsNull( _HeaderControlsTrc ) ) {
                    _HeaderControlsTrc = ControlUtils.GetControlDefineArray( typeof( MODEL_INFO_TRC ) );
                }
                return _HeaderControlsTrc;
            }
        }
        /// <summary>
        /// 検索条件定義情報
        /// </summary>
        ControlDefine[] _conditionControls = null;
        /// <summary>
        /// 検索条件定義情報アクセサ
        /// </summary>
        ControlDefine[] ConditionControls {
            get {
                if ( true == ObjectUtils.IsNull( _conditionControls ) ) {
                    _conditionControls = ControlUtils.GetControlDefineArray( typeof( CONDITION ) );
                }
                return _conditionControls;
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
        /// <summary>
        /// 表示中ユーザコントロール情報
        /// </summary>
        private PageInfo.ST_PAGE_INFO CurrentUCInfo {
            get {
                return PageInfo.GetUCPageInfo( DetailKeyParam.ProductKind, DetailKeyParam.GroupCd, DetailKeyParam.ClassCd, DetailKeyParam.AssemblyPatternCd );
            }
        }
        /// <summary>
        /// ユーザ情報
        /// </summary>
        private UserInfoSessionHandler.ST_USER _loginInfo;
        /// <summary>
        /// ユーザ情報
        /// </summary>
        public UserInfoSessionHandler.ST_USER LoginInfo {

            get {
                if ( true == ObjectUtils.IsNull( _loginInfo.UserInfo ) ) {
                    SessionManagerInstance sesMgr = CurrentForm.SessionManager;
                    _loginInfo = sesMgr.GetUserInfoHandler().GetUserInfo();
                }

                return _loginInfo;
            }
        }
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
                    _gridviewDefault = ControlUtils.GetGridViewDefineArray( typeof( GRID_IN_PRODUCT ) );
                }
                return _gridviewDefault;
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
            base.RaiseEvent( DoPageLoad, false );
        }

        #endregion

        #region イベントメソッド

        #region ページイベント

        /// <summary>
        /// ページロード処理
        /// </summary>
        protected override void DoPageLoad() {

            //件数
            ntbResultCount.Value = 0;

            //ベース ページロード処理
            base.DoPageLoad();
            
            //動的処理
            GridView frozenGrid = grvMainViewLB;
            ControlUtils.SetGridViewTemplateField( grvMainViewLB, ControlUtils.GetFrozenColumns( frozenGrid, gridviewDefault, true ) );
            ControlUtils.SetGridViewTemplateField( grvMainViewRB, ControlUtils.GetFrozenColumns( frozenGrid, gridviewDefault, false ) );

            ControlUtils.ReMakeBoundTemplateFieldCtrl( grvMainViewLB, ControlUtils.GetFrozenColumns( frozenGrid, gridviewDefault, true ) );
            ControlUtils.ReMakeBoundTemplateFieldCtrl( grvMainViewRB, ControlUtils.GetFrozenColumns( frozenGrid, gridviewDefault, false ) );

            int resultCnt = 0;
            if ( ObjectUtils.IsNotNull( ConditionInfo.ResultData ) ) {
                resultCnt = ConditionInfo.ResultData.Rows.Count;
            }

            ControlUtils.SetGridViewPager( ref pnlPager, grvMainViewRB, grvMainView_PageIndexChanging, resultCnt, grvMainViewRB.PageIndex );

        }

        #endregion

        #endregion

        #region メソッド

        #region 画面初期化処理
        /// <summary>
        /// 初期化処理
        /// </summary>
        protected override void Initialize() {

            //アクセスカウンター登録
            Dao.Com.AccessCounterDao.Entry( base.CurrentPageInfo.pageId );

            //ベース処理初期化処理
            base.Initialize();

            string productModelCd = "";
            string productModelNm = "";
            string country = "      ";
            string serial = "";
            string idno = "";
            string ptnCd = "";
            string dataCnt = "";

            //一覧表示列の設定
            GridViewDefine[] gridColumns;
            gridColumns = ControlUtils.GetGridViewDefineArray( typeof( MODEL_INFO_PRODUCT ) );

            string coop = StringUtils.Nvl( base.GetTransInfoValue( RequestParameter.DetailFrame.EXTERNAL_COOP ) );
            if ( true == StringUtils.IsNotBlank( coop ) ) {

                //MACs連携
                //現時点では処理なし

            } else {
                //一覧表示
                string callerToken = StringUtils.Nvl( base.GetTransInfoValue( RequestParameter.Common.TOKEN ) );
                string indexStr = StringUtils.Nvl( base.GetTransInfoValue( RequestParameter.DetailFrame.INDEX ) );
                int index = NumericUtils.ToInt( indexStr, -1 );
                //一覧からの遷移
                if ( 0 <= index ) {
 
                    //一覧の検索情報を取得
                    ConditionInfoSessionHandler.ST_CONDITION condMainView = SessionManager.GetConditionInfoHandler( callerToken ).GetCondition( PageInfo.MasterMainteDpfList.pageId );

                    //列追加
                    if (false == ( condMainView.ResultData.Columns.Contains( "FIN_DT" ) )) { 
                        condMainView.ResultData.Columns.Add( "FIN_DT" );    //完成日
                    }
                    if ( false == ( condMainView.ResultData.Columns.Contains( "MODEL_CD" ) ) ) { 
                        condMainView.ResultData.Columns.Add( "MODEL_CD" );  //型式CD
                        condMainView.ResultData.Columns.Add( "MODEL_NM" );  //型式name
                        condMainView.ResultData.Columns.Add( "COUNTRY" );   //国
                        condMainView.ResultData.Columns.Add( "SERIAL_NO" ); //機番
                        condMainView.ResultData.Columns.Add( "IDNO" );      //IDNO
                    }
                    if ( false == ( condMainView.ResultData.Columns.Contains( "ENG_COUNTRY" ) ) ) {
                        condMainView.ResultData.Columns.Add( "ENG_COUNTRY" );   //国(エンジン)
                    }

                    if ( true == ObjectUtils.IsNotNull( condMainView.ResultData )
                        && index < condMainView.ResultData.Rows.Count ) {

                        DataRow rowMainView = condMainView.ResultData.Rows[index];

                        dataCnt = StringUtils.ToString( rowMainView["DATA_CNT"] );
                        //DPF対応
                        switch ( StringUtils.ToString( rowMainView["LINE_CD"] ) ) {
                        case CONST_LINE_OEM:
                            //エンジン
                            rowMainView["PTN_CD"] = "10";
                            ptnCd = "10";
                            break;
                        case CONST_LINE_HS:
                            //トラクタ
                            rowMainView["PTN_CD"] = "30";
                            ptnCd = "30";
                            break;
                        default:
                            break;
                        }

                        if ( ptnCd.Equals( ProductKind.Engine ) ) {
                            //エンジン
                            productModelCd = StringUtils.ToString( rowMainView["ENG_MODEL_CD"] );
                            serial = StringUtils.ToString( rowMainView["ENG_SERIAL_NO"] );
                            idno = StringUtils.ToString( rowMainView["ENG_IDNO"] );
                            if ( StringUtils.IsNotEmpty( StringUtils.ToString( rowMainView["ENG_COUNTRY"] ) ) ) {
                                country = StringUtils.ToString( rowMainView["ENG_COUNTRY"] );
                            }

                            //型式名取得
                            DataTable tmpModel = Dao.Com.ModelDao.GetModelInfo( DataUtils.GetModelCd( productModelCd ), country );
                            productModelNm = StringUtils.ToString( tmpModel.Rows[0]["modelNm"] );
                            rowMainView["ENG_MODEL_NM"] = productModelNm;

                            divEngineInfo.Visible = false;
                            //搭載エンジンの場合
                            if ( StringUtils.IsBlank( StringUtils.ToString( rowMainView["TRC_MODEL_CD"] ) ) ) {
                                divToractorInfo.Visible = false;
                            } else {
                                //型式名取得
                                DataTable tmpModelTrc = Dao.Com.ModelDao.GetModelInfo( DataUtils.GetModelCd( StringUtils.ToString( rowMainView["TRC_MODEL_CD"] ) ), country );
                                rowMainView["TRC_MODEL_NM"] = StringUtils.ToString( tmpModelTrc.Rows[0]["modelNm"] );
                            }


                         } else {
                            //トラクタ
                            productModelCd = StringUtils.ToString( rowMainView["TRC_MODEL_CD"] );
                            serial = StringUtils.ToString( rowMainView["TRC_SERIAL_NO"] );
                            idno = StringUtils.ToString( rowMainView["TRC_IDNO"] );
                            if ( StringUtils.IsNotEmpty( StringUtils.ToString( rowMainView["TRC_COUNTRY"] ) ) ) {
                                country = StringUtils.ToString( rowMainView["TRC_COUNTRY"] );
                            }

                            //型式名取得
                            DataTable tmpModelTrc = Dao.Com.ModelDao.GetModelInfo( DataUtils.GetModelCd( productModelCd ), country );
                            productModelNm = StringUtils.ToString( tmpModelTrc.Rows[0]["modelNm"] );

                            //エンジンの型式名取得
                            DataTable tmpModelEng = Dao.Com.ModelDao.GetModelInfo( DataUtils.GetModelCd( StringUtils.ToString( rowMainView["ENG_MODEL_CD"] ) ), "      " );
                            rowMainView["ENG_MODEL_NM"] = StringUtils.ToString( tmpModelEng.Rows[0]["modelNm"] );

                            divToractorInfo.Visible = false;

                        }

                        
                        //ヘッダデータ取得
                        string strFinDt = DoSearchHeader( DataUtils.GetModelCd( productModelCd ), serial, ptnCd );
                        rowMainView["FIN_DT"] = strFinDt;
                        rowMainView["MODEL_CD"] = productModelCd;
                        rowMainView["SERIAL_NO"] = serial;
                        rowMainView["MODEL_NM"] = productModelNm;
                        rowMainView["IDNO"] = idno;
                        rowMainView["COUNTRY"] = country;


                        //製品情報ヘッダ
                        base.SetControlValues( HeaderControls, rowMainView, ref dicControlValues );
                        //エンジンヘッダ
                        base.SetControlValues( HeaderControlsEng, rowMainView, ref dicControlValues );

                        //搭載エンジンの場合
                        if ( StringUtils.IsNotEmpty( StringUtils.ToString( rowMainView["TRC_MODEL_CD"] ) ) ) {
                            base.SetControlValues( HeaderControlsTrc, rowMainView, ref dicControlValues );
                        }

                        logger.Info( "詳細画面表示(一覧画面から遷移) 型式:{0} 機番:{1}", productModelCd, serial );
                    }
                }
            }

            //初期化、初期値設定
            InitializeValues( dataCnt );

        }
        /// <summary>
        /// 初期化、初期値設定
        /// </summary>
        private void InitializeValues( string dataCnt ) {

            //■初期値設定
            this.btnInsert.Enabled = true;
            this.btnUpdate.Enabled = true;

            //明細データ取得
            if ( dataCnt.Equals( "有" ) ) {
                DoSearch();
            }else{
                this.btnUpdate.Enabled = false;
            }

            //入力画面呼び出し            
            ListItem[] liArr = new ListItem[7];
            liArr[0] = new ListItem( "exeKbn", "1" );
            liArr[1] = new ListItem( "Token", StringUtils.Nvl( base.GetTransInfoValue( RequestParameter.Common.TOKEN ) ) );
            string subToken = ( (UI.MasterForm.MasterMain)Master ).GetThisToken;
            liArr[2] = new ListItem( "subToken", subToken );
            liArr[3] = new ListItem( "productKind", StringUtils.ToString( txtPtnCd.Text ) );
            liArr[4] = new ListItem( "lineCd", StringUtils.ToString( txtLineCd.Text ) );
            liArr[5] = new ListItem( "st", StringUtils.ToString( txtST.Text ) );
            liArr[6] = new ListItem( "idno", StringUtils.ToString( txtIDNO.Text ) );

            //ボタンクリックイベント
            btnInsert.Attributes[ControlUtils.ON_CLICK] = InputModal.CreateDispUrl( this, PageInfo.DpfSerial, 10, 10, liArr, "1" );
            liArr[0] = new ListItem( "exeKbn", "2" );
            btnUpdate.Attributes[ControlUtils.ON_CLICK] = InputModal.CreateDispUrl( this, PageInfo.DpfSerial, 10, 10, liArr, "2" );
            
        }

        #endregion

        #endregion
        /// <summary>
        /// 検索ボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click( object sender, EventArgs e ) {
            base.RaiseEvent( DoSearch );
        }

        /// <summary>
        /// グリッドビューページチェンジイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grvMainView_PageIndexChanging( object sender, CommandEventArgs e ) {
            base.RaiseEvent( PageIndexChangingMainView, sender, e );
        }
        /// <summary>
        /// グリッドビュー並び替えイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grvMainView_Sorting( object sender, GridViewSortEventArgs e ) {
            base.RaiseEvent( SortingMainView, sender, e );
        }

        #region グリッドビューイベント

        /// <summary>
        /// グリッドビューページ切替
        /// </summary>
        /// <param name="parameters"></param>
        private void PageIndexChangingMainView( params object[] parameters ) {
            object sender = parameters[0];

            CommandEventArgs e = (CommandEventArgs)parameters[1];
            int newPageIndex = Convert.ToInt32( e.CommandArgument );

            int pageSize = grvMainViewRB.PageSize;
            int rowCount = 0;
            if ( true == ObjectUtils.IsNotNull( ConditionInfo.ResultData ) ) {
                rowCount = ConditionInfo.ResultData.Rows.Count;
            }
            int allPages = 0;
            allPages = ConditionInfo.ResultData.Rows.Count / pageSize;
            if ( 0 != rowCount % pageSize ) {
                allPages += 1;
            }
            //ページが無くなっている場合には、先頭ページへ戻す
            if ( newPageIndex >= allPages ) {
                newPageIndex = 0;
            }

            //背面ユーザ切替対応
            ConditionInfoSessionHandler.ST_CONDITION cond = ConditionInfo;
            GridView frozenGrid = grvMainViewLB;

            //Gridカラム再作成
            grvMainViewRB.Columns.Clear();
            for ( int idx = frozenGrid.Columns.Count; idx < GridViewResults.Length; idx++ ) {
                TemplateField tf = new TemplateField();
                tf.HeaderText = StringUtils.ToString( GridViewResults[idx].bindField );
                grvMainViewRB.Columns.Add( tf );
            }

            ControlUtils.ShowGridViewHeader( grvHeaderLT, ControlUtils.GetFrozenColumns( frozenGrid, GridViewResults, true ), cond, true );
            ControlUtils.ShowGridViewHeader( grvHeaderRT, ControlUtils.GetFrozenColumns( frozenGrid, GridViewResults, false ), cond, true );

            ControlUtils.BindGridView_WithTempField( grvMainViewLB, ControlUtils.GetFrozenColumns( frozenGrid, GridViewResults, true ), ConditionInfo.ResultData );
            ControlUtils.BindGridView_WithTempField( grvMainViewRB, ControlUtils.GetFrozenColumns( frozenGrid, GridViewResults, false ), ConditionInfo.ResultData );

            ControlUtils.GridViewPageIndexChanging( grvMainViewLB, ConditionInfo.ResultData, new GridViewPageEventArgs( newPageIndex ) );
            ControlUtils.GridViewPageIndexChanging( grvMainViewRB, ConditionInfo.ResultData, new GridViewPageEventArgs( newPageIndex ) );

            ControlUtils.SetGridViewPager( ref pnlPager, grvMainViewRB, grvMainView_PageIndexChanging, ConditionInfo.ResultData.Rows.Count, grvMainViewRB.PageIndex );

            //グリッドビュー表示列情報修正
            SetGridViewColumns();

            //グリッドビュー外のDivサイズ変更
            SetDivGridViewWidth();

        }

        /// <summary>
        /// グリッドビュー並び替え
        /// </summary>
        /// <param name="parameters"></param>
        private void SortingMainView( params object[] parameters ) {
            object sender = parameters[0];
            GridViewSortEventArgs e = (GridViewSortEventArgs)parameters[1];

            ConditionInfoSessionHandler.ST_CONDITION cond = ConditionInfo;
            ControlUtils.GridViewSorting( grvMainViewLB, ref cond, e, true );
            ControlUtils.GridViewSorting( grvMainViewRB, ref cond, e );

            //背面ユーザ切替対応
            GridView frozenGrid = grvMainViewLB;
            ControlUtils.ShowGridViewHeader( grvHeaderLT, ControlUtils.GetFrozenColumns( frozenGrid, GridViewResults, true ), cond, true );
            ControlUtils.ShowGridViewHeader( grvHeaderRT, ControlUtils.GetFrozenColumns( frozenGrid, GridViewResults, false ), cond, true );

            ControlUtils.BindGridView_WithTempField( grvMainViewLB, ControlUtils.GetFrozenColumns( frozenGrid, GridViewResults, true ), cond.ResultData );
            ControlUtils.BindGridView_WithTempField( grvMainViewRB, ControlUtils.GetFrozenColumns( frozenGrid, GridViewResults, false ), cond.ResultData );

            ControlUtils.SetGridViewPager( ref pnlPager, grvMainViewRB, grvMainView_PageIndexChanging, cond.ResultData.Rows.Count, grvMainViewRB.PageIndex );

            ConditionInfo = cond;

            //グリッドビュー表示列情報修正
            SetGridViewColumns();

            //グリッドビュー外のDivサイズ変更
            SetDivGridViewWidth();
        }
                /// <summary>
        /// グリッドビュークリア
        /// </summary>
        private void ClearGridView() {

            //列名非表示 グリッドビュークリア
            ControlUtils.InitializeGridView( grvHeaderLT,false );
            ControlUtils.InitializeGridView( grvHeaderRT, false );
            ControlUtils.InitializeGridView( grvMainViewLB, false );
            ControlUtils.InitializeGridView( grvMainViewRB, false );

            //グリッドビュー外のDivサイズ変更
            SetDivGridViewWidth();

            //件数表示
            ntbResultCount.Value = 0;

            //ページャークリア
            ControlUtils.ClearPager( ref pnlPager );

            //GridView非表示
            divGrvDisplay.Visible = false;
        }
        #endregion

        #region グリッドビュー関連処理

        /// <summary>
        /// グリッドビューの列コントロール制御
        /// </summary>
        /// <remarks>定義情報で表示となっている列に対して権限情報でグリッドビューの列情報を動的に変更</remarks>
        private void SetGridViewColumns() {
        }

        /// <summary>
        /// グリッドビュー格納DIVサイズ調整
        /// </summary>
        private void SetDivGridViewWidth() {
            SetDivGridViewWidth( grvHeaderLT, divGrvHeaderLT );
            SetDivGridViewWidth( grvHeaderRT, divGrvHeaderRT );

            SetDivGridViewWidth( grvMainViewLB, divGrvLB );
            SetDivGridViewWidth( grvMainViewRB, divGrvRB );
        }

        /// <summary>
        /// グリッドビュー外のDivサイズ変更
        /// </summary>
        private void SetDivGridViewWidth( GridView grv, System.Web.UI.HtmlControls.HtmlGenericControl div ) {

            //セル幅補正 Padding4px + Border(片側) 1px
            const int CELL_PADDING = 4 * 2 + 1;
            //テーブル幅補正 Border(片側) 1px
            const int OUT_BORDER = 1;

            double sumWidth = 0;
            int showColCnt = 0;

            for ( int loop = 0; loop < grv.Columns.Count; loop++ ) {

                if ( false == grv.Columns[loop].Visible ) {
                    continue;
                }

                sumWidth += grv.Columns[loop].HeaderStyle.Width.Value + CELL_PADDING;
                showColCnt += 1;
            }

            if ( 0 < showColCnt ) {
                sumWidth += OUT_BORDER;
            }

            div.Style["width"] = Convert.ToInt32( sumWidth ).ToString() + "px";
        }
        #endregion

        #region 検索処理
        /// <summary>
        /// 検索処理(ヘッダー)
        /// </summary>
        private string DoSearchHeader(string modeCd,string serial,string ptnCd) {


            string retDt = "";
            //検索結果取得
            //エラー時にも最後まで処理を通すため、インスタンスを生成しておく
            MainViewBusiness.ResultSet result = new MainViewBusiness.ResultSet();
            DataTable tblResult = null;
            int maxGridViewCount = WebAppInstance.GetInstance().Config.WebCommonInfo.maxGridViewCount;  //検索上限数
            try {
                //ヘッダデータ取得
                tblResult = EngineProcessDao.SelectHeaderInfo( modeCd, serial, ptnCd, maxGridViewCount );

            } catch ( DataAccessException ex ) {
                if ( ex.OracleErrorNumber == DataAccessBase.ORAERROR_TIMEOUT ) {
                    //クエリ発行タイムアウト
                    base.WriteApplicationMessage( MsgManager.MESSAGE_WRN_61910 );
                } else {
                    //タイムアウト以外のException
                    logger.Exception( ex );
                    base.WriteApplicationMessage( MsgManager.MESSAGE_ERR_80010 );
                }
            } catch ( Exception ex ) {
                logger.Exception( ex );
                base.WriteApplicationMessage( MsgManager.MESSAGE_ERR_80010 );
            } finally {
                //メッセージ設定
                result.Message = null;
                if ( null == tblResult || 0 == tblResult.Rows.Count ) {
                    //検索結果0件
                    result.Message = new Msg( MsgManager.MESSAGE_WRN_61010 );
                    retDt = "";

                } else if ( ( null != tblResult && maxGridViewCount <= tblResult.Rows.Count ) ) {
                    //検索件数が上限を上回っている場合には警告メッセージをセット
                    result.Message = new Msg( MsgManager.MESSAGE_WRN_61020 );
                }else{
                    retDt = StringUtils.ToString( tblResult.Rows[0]["FIN_DT"] );
                }

            }

            return retDt;
        }

        /// <summary>
        /// 検索処理
        /// </summary>
        private void DoSearch() {

            //検索パラメータ作成
            Dictionary<string, object> dicCondition = new Dictionary<string, object>();

            base.GetControlValues( ConditionControls, ref dicCondition );

            //検索時画面情報取得
            Dictionary<string, string> dicIdWithText = new Dictionary<string, string>();
            base.GetControlTexts( ConditionControls, out dicIdWithText );

            //一覧表示列の設定
            GridViewDefine[] gridColumns;
            List<GridViewDefine> columns = new List<GridViewDefine>();
            string productKind = StringUtils.ToString( txtPtnCd.Text );


            columns.AddRange( ControlUtils.GetGridViewDefineArray( typeof( GRID_COMMON ) ) );
            columns.AddRange( ControlUtils.GetGridViewDefineArray( typeof( GRID_COMMON_TAIL ) ) );

            gridColumns = columns.ToArray();
            GridViewResults = gridColumns; 

            //検索結果取得
            //エラー時にも最後まで処理を通すため、インスタンスを生成しておく
            MainViewBusiness.ResultSet result = new MainViewBusiness.ResultSet();
            DataTable tblResult = null;
            int maxGridViewCount = WebAppInstance.GetInstance().Config.WebCommonInfo.maxGridViewCount;  //検索上限数
            try {
                //生産中データ取得
                result.ListTable = EngineProcessDao.SelectDpfSerialList( dicCondition, maxGridViewCount );

            } catch ( DataAccessException ex ) {
                if ( ex.OracleErrorNumber == DataAccessBase.ORAERROR_TIMEOUT ) {
                    //クエリ発行タイムアウト
                    base.WriteApplicationMessage( MsgManager.MESSAGE_WRN_61910 );
                } else {
                    //タイムアウト以外のException
                    logger.Exception( ex );
                    base.WriteApplicationMessage( MsgManager.MESSAGE_ERR_80010 );
                }
            } catch ( Exception ex ) {
                logger.Exception( ex );
                base.WriteApplicationMessage( MsgManager.MESSAGE_ERR_80010 );
            } finally {
                //メッセージ設定
                result.Message = null;
                if ( null == result.ListTable || 0 == result.ListTable.Rows.Count ) {
                    //検索結果0件
                    result.Message = new Msg( MsgManager.MESSAGE_WRN_62010, NOT_FOUND_DTL_DATA );

                } else if ( ( null != result.ListTable && maxGridViewCount <= result.ListTable.Rows.Count ) ) {
                    //検索件数が上限を上回っている場合には警告メッセージをセット
                    result.Message = new Msg( MsgManager.MESSAGE_WRN_61020 );
                }

            }

            ConditionInfoSessionHandler.ST_CONDITION cond = new ConditionInfoSessionHandler.ST_CONDITION();
            tblResult = result.ListTable;
            if ( null != tblResult ) {

                //従業員情報取得
                getEmpInfo();

                //部品名
                Dictionary<string, string> _partsCd = new Dictionary<string, string>();
                foreach ( ListItem tmp in Dao.Com.MasterList.GetClassItem( ProductKind.Engine, GroupCd.Parts, false ) ) {
                    _partsCd.Add( tmp.Value, tmp.Text );
                }

                foreach ( DataRow row in tblResult.Rows ) {
                    //「名称」で上書きする
                    if ( StringUtils.IsNotEmpty( StringUtils.ToString( row["UPDATE_BY"] ) ) ) {
                        if ( _dicEmp.ContainsKey( StringUtils.ToString( row["UPDATE_BY"] ) ) ) {
                            //存在する
                            row["UPDATE_BY"] = _dicEmp[StringUtils.ToString( row["UPDATE_BY"] )];
                        } else {
                            //存在しない
                            row["UPDATE_BY"] = "";
                        }
                    }
                    //DPF型式
                        row["DPF_MODEL_CD"] = DataUtils.GetModelCdStr( StringUtils.ToString( row["DPF_MODEL_CD"] ) );

                    //部品区分名称
                    if ( StringUtils.IsNotEmpty( StringUtils.ToString( row["PARTS_KBN"] ) ) ) {
                        row["PARTS_KBN"] = _partsCd[StringUtils.ToString( row["PARTS_KBN"] )];
                    }
                }
                
                //件数表示
                ntbResultCount.Value = tblResult.Rows.Count;

                //検索条件/結果インスタンス
                cond.conditionValue = dicCondition;
                cond.IdWithText = dicIdWithText;
                cond.ResultData = tblResult.DefaultView.ToTable();

            } else {
                //タイムアウト等Exception時には、GridViewクリア
                ClearGridView();
            }

            //格納実施
			ConditionInfo = cond;
            
            //グリッドビューバインド
            GridView frozenGrid = grvMainViewLB;
            if ( true == ObjectUtils.IsNotNull( tblResult ) ) {
                if ( 0 < tblResult.Rows.Count ) {

                    //TemplateFieldの再作成
                    grvHeaderRT.Columns.Clear();
                    grvMainViewRB.Columns.Clear();
                    for ( int idx = frozenGrid.Columns.Count; idx < GridViewResults.Length; idx++ ) {
                        TemplateField tf = new TemplateField();
                        tf.HeaderText = StringUtils.ToString( GridViewResults[idx].bindField );
                        grvMainViewRB.Columns.Add( tf );
                    }

                    //新規バインド
                    ControlUtils.ShowGridViewHeader( grvHeaderLT, ControlUtils.GetFrozenColumns( frozenGrid, GridViewResults, true ),ConditionInfo, true );
                    ControlUtils.ShowGridViewHeader( grvHeaderRT, ControlUtils.GetFrozenColumns( frozenGrid, GridViewResults, false ),ConditionInfo, true );
                    ControlUtils.BindGridView_WithTempField( grvMainViewLB, ControlUtils.GetFrozenColumns( frozenGrid, GridViewResults, true ), tblResult );
                    ControlUtils.BindGridView_WithTempField( grvMainViewRB, ControlUtils.GetFrozenColumns( frozenGrid, GridViewResults, false ), tblResult );

                    //ページャー作成
                    ControlUtils.SetGridViewPager( ref pnlPager, grvMainViewLB, grvMainView_PageIndexChanging, cond.ResultData.Rows.Count, grvMainViewLB.PageIndex );

                    //グリッドビュー表示列情報修正
                    SetGridViewColumns();

                    //グリッドビュー外のDivサイズ変更
                    SetDivGridViewWidth();
                } else {
                    ClearGridView();
                }
            }

            //項目表示制御
            SetGridViewColumns();

            //権限によるボタン制御
            //AppPermission.PERMISSION_INFO permMainteInfo = AppPermission.GetTransactionPermission( PageInfo.DpfSerial, LoginInfo.UserInfo );
            AppPermission.PERMISSION_INFO permMainteInfo = AppPermission.GetTransactionPermission( PageInfo.DpfSerial, LoginInfo.UserInfo );
            if ( permMainteInfo.IsView == true ) {
                    this.btnInsert.Enabled = true;

                    if ( true == ObjectUtils.IsNotNull( tblResult ) ) {
                        if ( 0 < tblResult.Rows.Count ) {
                            this.btnUpdate.Enabled = true;
                        }
                    }
            } else {
                this.btnInsert.Enabled = false;
                this.btnUpdate.Enabled = false;
            }

            //メッセージ表示
            if ( null != result.Message ) {
                base.WriteApplicationMessage( result.Message );
            }
        }
        /// <summary>
        /// 画面表示用従業員情報の取得
        /// </summary>
        private void getEmpInfo() {
            DataTable tmp = new DataTable();
            try {
                //従業員情報を取得
                tmp = Business.DetailViewBusiness.SelectEmpInfo( null, null );
                _dicEmp = new Dictionary<string, string>();

                foreach ( DataRow dr in tmp.Rows ) {
                    _dicEmp.Add( dr["EMP_NO"].ToString().Trim(), dr["EMP_NM"].ToString().Trim() );
                }

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
        }

        #endregion

        protected void grvMainViewLB_RowDataBound( object sender, GridViewRowEventArgs e ) {
            RowDataBoundMainViewLB( sender, e );
        }

        protected void grvMainViewRB_RowDataBound( object sender, GridViewRowEventArgs e ) {
            RowDataBoundMainViewRB( sender, e );
        }
                /// <summary>
        /// グリッドビュー行バインド(左下)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RowDataBoundMainViewLB( params object[] parameters ) {
            object sender = parameters[0];
            GridViewRowEventArgs e = (GridViewRowEventArgs)parameters[1];

            if ( e.Row.RowType == DataControlRowType.DataRow ) {
                DataRow row = ( (DataRowView)e.Row.DataItem ).Row;

//                Button btnModalDisp = (Button)e.Row.FindControl( GRID_SEARCH_CONTROLS_L.UPD_CHECK.controlId );
                
                Dictionary<string, object> dicControls = new Dictionary<string, object>();
                base.SetControlValues( e.Row, ControlUtils.GetControlDefineArray( typeof( GRID_SEARCH_CONTROLS_L ) ), row, ref dicControls );
            }
            
            ControlUtils.GridViewRowBound( (GridView)sender, e, GRID_MAIN_VIEW_GROUP_CD );
        }

        /// <summary>
        /// グリッドビュー行バインド(右下)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RowDataBoundMainViewRB( params object[] parameters ) {
            object sender = parameters[0];
            GridViewRowEventArgs e = (GridViewRowEventArgs)parameters[1];

            if ( e.Row.RowType == DataControlRowType.DataRow ) {
                DataRow rowData = ( (DataRowView)e.Row.DataItem ).Row;
                Dictionary<string, object> dicControls = new Dictionary<string, object>();
                base.SetControlValues( e.Row, ControlUtils.GetControlDefineArray( typeof( GRID_SEARCH_CONTROLS_R ) ), rowData, ref dicControls );
            }

            ControlUtils.GridViewRowBound( (GridView)sender, e, GRID_MAIN_VIEW_GROUP_CD );
        }
    }
}