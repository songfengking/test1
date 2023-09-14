using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
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
using TRC_W_PWT_ProductView.Defines.ProcessViewDefine;
using KTWebControl.CustomControls;
using TRC_W_PWT_ProductView.Dao.Com;

namespace TRC_W_PWT_ProductView.UI.Pages {

    /// <summary>
    /// 詳細外枠画面
    /// </summary>
    public partial class DetailFrame : BaseForm {

        //ロガー定義
        private static readonly Logger logger = new Logger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

        #region 定数定義

        ///// <summary>
        ///// 型式情報欄データ格納先 ディクショナリキー
        ///// </summary>
        //private const string SESSION_PAGE_INFO_MODEL_INFO_KEY = "modelInfoData";

        ///// <summary>
        ///// 詳細情報 ディクショナリキー
        ///// </summary>
        //public const string SESSION_PAGE_INFO_DETAIL_KEY = "detailInfoData";

        #region Attribute
        /// <summary>
        /// 詳細一覧項目Attribute属性(GroupCd)
        /// </summary>
        const string DETAIL_LIST_ATTR_GROUP = "GroupCd";
        /// <summary>
        /// 詳細一覧項目Attribute属性(ClassCd)
        /// </summary>
        const string DETAIL_LIST_ATTR_CLASS = "ClassCd";
        /// <summary>
        /// 詳細一覧項目Attribute属性(LineCd)
        /// </summary>
        const string DETAIL_LIST_ATTR_LINECD = "LineCd";
        /// <summary>
        /// 詳細一覧項目Attribute属性(ProcessCd)
        /// </summary>
        const string DETAIL_LIST_ATTR_PROCESSCD = "ProcessCd";
        /// <summary>
        /// 詳細一覧項目Attribute属性(LineShortName)
        /// </summary>
        const string DETAIL_LIST_ATTR_LINENM = "LineShortNm";
        /// <summary>
        /// 詳細一覧項目Attribute属性(ProcessNm)
        /// </summary>
        const string DETAIL_LIST_ATTR_PROCESSNM = "ProcessNm";
        /// <summary>
        /// 詳細一覧項目Attribute属性(SeachTargetFlg)
        /// </summary>
        const string DETAIL_LIST_ATTR_SEARCHTARGETFLG = "SearchTargetFlg";
        #endregion

        #region スクリプトイベント
        /// <summary>
        /// 詳細画面切替イベント(本機/搭載エンジン)
        /// </summary>
        /// <remarks>パラメータ URL トークン 型式コード 国コード 機番</remarks>
        const string TRANSFER_MODEL_CLICK = "return ControlCommon.WindowOpenChangeDetail('{0}','{1}','{2}','{3}','{4}');";

        /// <summary>
        /// 詳細リスト項目ボタン選択
        /// </summary>
        /// <remarks>パラメータ URL トークン 型式コード 国コード 機番</remarks>
        const string LIST_CONTENT_CLICK = "return DetailFrame.SelectContentList(this);";

        #endregion

        #region 名称

        /// <summary>
        /// 製品情報
        /// </summary>
        const string MODEL_INFO_TITLE_PRODUCT = "製品情報";

        /// <summary>
        /// 詳細一覧項目 部品タイトル
        /// </summary>
        const string DETAIL_LIST_PARTS_NM = "【　部　品　】";

        /// <summary>
        /// 詳細一覧項目 工程タイトル
        /// </summary>
        const string DETAIL_LIST_PROCESS_NM = "【　工　程　】";

        #endregion

        #region CSS

        /// <summary>
        /// 詳細一覧項目 タイトル用CSS
        /// </summary>
        const string DETAIL_LIST_TITLE_CSS = "lbl-list-title";

        /// <summary>
        /// 詳細一覧項目 項目用CSS
        /// </summary>
        const string DETAIL_LIST_CONTENT_CSS = "btn-list-content";
        const string DETAIL_LIST_NONECONTENT_CSS = "btn-list-content-none";


        /////////////////////////荻野追加部分 -START- /////////////////////////

        /// <summary>
        /// 詳細一覧項目 ライン用CSS
        /// </summary>
        const string DETAIL_LIST_LINE_CSS = "btn-list-line";

        /////////////////////////荻野追加部分 -end- /////////////////////////


        /// <summary>
        /// 詳細一覧項目 項目選択済用CSS
        /// </summary>
        const string DETAIL_LIST_SELECTED_CONTENT_CSS = "btn-list-content-selected";

        #endregion

        #endregion

        #region 型式情報欄定義
        /// <summary>
        /// 型式情報欄定義(製品情報)
        /// </summary>
        public class MODEL_INFO_PRODUCT {
            /// <summary>製品種別</summary>
            public static readonly ControlDefine PRODUCT_KIND_CD = new ControlDefine( "hdnProductKind", "製品種別", "generalPatternCd", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>組立パターン</summary>
            public static readonly ControlDefine ASSEMBLY_PATTERN_CD = new ControlDefine( "hdnAssemblyPatternCd", "組立パターン", "assemblyPatternCd", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>生産型式コード(非表示)</summary>
            public static readonly ControlDefine MODEL_CD = new ControlDefine( "", "型式コード(非表示)", "modelCd", ControlDefine.BindType.None, typeof( String ) );
            /// <summary>生産型式コード(非表示)</summary>
            public static readonly ControlDefine PRODUCT_MODEL_CD = new ControlDefine( "", "生産型式コード(非表示)", "productModelCd", ControlDefine.BindType.None, typeof( String ) );
            /// <summary>生産型式コード</summary>
            public static readonly ControlDefine PRODUCT_MODEL_CD_STR = new ControlDefine( "txtProductModelCd", "生産型式コード", "productModelCdStr", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>生産型式名</summary>
            public static readonly ControlDefine PRODUCT_MODEL_NM = new ControlDefine( "txtProductModelNm", "生産型式名", "productModelNm", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>国コード</summary>
            public static readonly ControlDefine PRODUCT_COUNTRY_CD = new ControlDefine( "txtProductCountryCd", "生産国", "CountryCd", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>機番</summary>
            public static readonly ControlDefine SERIAL = new ControlDefine( "txtProductSerial", "機番", "serial", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>PINコード</summary>
            public static readonly ControlDefine PIN_CD = new ControlDefine( "txtPinCd", "PINコード", "pinCd", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>IDNO</summary>
            public static readonly ControlDefine IDNO = new ControlDefine( "txtIdno", "IDNO", "idno", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>完成予定日</summary>
            public static readonly ControlDefine PLAN_DT = new ControlDefine( "cldPlanDt", "完成予定日", "planDt", ControlDefine.BindType.Down, typeof( DateTime ) );
            /// <summary>完成日</summary>
            public static readonly ControlDefine PRODUCT_DT = new ControlDefine( "cldProductDt", "完成日", "productDt", ControlDefine.BindType.Down, typeof( DateTime ) );
            /// <summary>出荷日</summary>
            public static readonly ControlDefine SHIPPED_DT = new ControlDefine( "cldShippedDt", "出荷日", "shippedDt", ControlDefine.BindType.Down, typeof( DateTime ) );
        }
        /// <summary>
        /// 型式情報欄定義(本機情報)
        /// </summary>
        public class MODEL_INFO_TRACTOR {
            /// <summary>生産型式コード(非表示)</summary>
            public static readonly ControlDefine PRODUCT_MODEL_CD = new ControlDefine( "", "生産型式コード(非表示)", "tractorModelCd", ControlDefine.BindType.None, typeof( String ) );
            /// <summary>生産型式コード</summary>
            public static readonly ControlDefine PRODUCT_MODEL_CD_STR = new ControlDefine( "txtTractorModelCd", "生産型式コード", "tractorModelCdStr", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>生産型式名</summary>
            public static readonly ControlDefine PRODUCT_MODEL_NM = new ControlDefine( "txtTractorModelNm", "型式名", "tractorModelNm", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>国コード</summary>
            public static readonly ControlDefine PRODUCT_COUNTRY_CD = new ControlDefine( "txtTractorCountryCd", "生産国コード", "tractorCountryCd", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>機番</summary>
            public static readonly ControlDefine SERIAL = new ControlDefine( "txtTractorSerial", "機番", "tractorSerial", ControlDefine.BindType.Down, typeof( String ) );
        }
        /// <summary>
        /// 型式情報欄定義(搭載エンジン情報)
        /// </summary>
        public class MODEL_INFO_ENGINE {
            /// <summary>生産型式コード(非表示)</summary>
            public static readonly ControlDefine PRODUCT_MODEL_CD = new ControlDefine( "", "生産型式コード(非表示)", "engineModelCd", ControlDefine.BindType.None, typeof( String ) );
            /// <summary>生産型式コード</summary>
            public static readonly ControlDefine PRODUCT_MODEL_CD_STR = new ControlDefine( "txtEngineModelCd", "生産型式コード", "engineModelCdStr", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>生産型式名</summary>
            public static readonly ControlDefine PRODUCT_MODEL_NM = new ControlDefine( "txtEngineModelNm", "型式名", "engineModelNm", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>国コード</summary>
            public static readonly ControlDefine PRODUCT_COUNTRY_CD = new ControlDefine( "txtEngineCountryCd", "生産国コード", "engineCountryCd", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>機番</summary>
            public static readonly ControlDefine SERIAL = new ControlDefine( "txtEngineSerial", "機番", "engineSerial", ControlDefine.BindType.Down, typeof( String ) );
        }
        #endregion

        #region プロパティ
        /// <summary>
        /// 型式情報欄コントロール定義
        /// </summary>
        ControlDefine[] _modelInfoControls = null;
        /// <summary>
        /// 型式情報欄コントロール定義アクセサ
        /// </summary>
        ControlDefine[] ModelInfoControls
        {
            get
            {
                if ( true == ObjectUtils.IsNull( _modelInfoControls ) ) {
                    List<ControlDefine> modelInfoList = new List<ControlDefine>();
                    modelInfoList.AddRange( ControlUtils.GetControlDefineArray( typeof( MODEL_INFO_PRODUCT ) ) );
                    modelInfoList.AddRange( ControlUtils.GetControlDefineArray( typeof( MODEL_INFO_TRACTOR ) ) );
                    modelInfoList.AddRange( ControlUtils.GetControlDefineArray( typeof( MODEL_INFO_ENGINE ) ) );
                    _modelInfoControls = modelInfoList.ToArray();
                }
                return _modelInfoControls;
            }
        }

        /// <summary>
        /// 型式情報構造体
        /// </summary>
        public struct ST_MODEL_INFO {
            public string productKindCd;
            public string productModelCd;
            public string productCountryCd;
            public string productSerial;
            public string idno;
            public string assemblyPatternCd;
        }

        /// <summary>
        /// 型式情報取得
        /// </summary>
        /// <param name="groupCd">工程/部品種別</param>
        /// <param name="classCd">工程区分/部品区分</param>
        /// <param name="lineCd">ラインコード、未入力の場合null</param>
        /// <param name="processCd">工程コード、未入力の場合null</param>
        /// <returns>型式情報構造体</returns>
        private Defines.Interface.ST_DETAIL_PARAM GetProductModelInfo( string groupCd, string classCd, string lineCd = null, string processCd = null ) {

            Defines.Interface.ST_DETAIL_PARAM stInfo = new Defines.Interface.ST_DETAIL_PARAM();
            if ( true == base.PageControlInfo.ContainsKey( Defines.Session.DetailFrame.SESSION_PAGE_INFO_MODEL_INFO_KEY ) ) {
                Dictionary<string, object> dicPageControlInfo = (Dictionary<string, object>)base.PageControlInfo[Defines.Session.DetailFrame.SESSION_PAGE_INFO_MODEL_INFO_KEY];
                stInfo.ProductKind = DataUtils.GetDictionaryStringVal( dicPageControlInfo, MODEL_INFO_PRODUCT.PRODUCT_KIND_CD.bindField );
                stInfo.GroupCd = groupCd;
                stInfo.ClassCd = classCd;
                stInfo.ModelCd = DataUtils.GetDictionaryStringVal( dicPageControlInfo, MODEL_INFO_PRODUCT.MODEL_CD.bindField );
                stInfo.ProductModelCd = DataUtils.GetDictionaryStringVal( dicPageControlInfo, MODEL_INFO_PRODUCT.PRODUCT_MODEL_CD.bindField );
                stInfo.CountryCd = DataUtils.GetDictionaryStringVal( dicPageControlInfo, MODEL_INFO_PRODUCT.PRODUCT_COUNTRY_CD.bindField );
                stInfo.Serial = DataUtils.GetDictionaryStringVal( dicPageControlInfo, MODEL_INFO_PRODUCT.SERIAL.bindField );
                stInfo.Idno = DataUtils.GetDictionaryStringVal( dicPageControlInfo, MODEL_INFO_PRODUCT.IDNO.bindField );
                stInfo.AssemblyPatternCd = DataUtils.GetDictionaryStringVal( dicPageControlInfo, MODEL_INFO_PRODUCT.ASSEMBLY_PATTERN_CD.bindField );
                if ( stInfo.ProductKind.Equals( ProductKind.Tractor ) && groupCd.Equals( GroupCd.Parts ) && classCd.CompareTo( "99" ) < 0 ) {
                    //トラクタ＋部品＋CLASSコードの固定99以下(Alphabet)の場合、型式にはエンジン型式を設定しておく
                    /// ※エンジン部品の子画面の初期データ検索時に「ModelCd」に本機型式コードが設定されており、検索されなくなってしまうのを回避するため
                    stInfo.ProductModelCd = DataUtils.GetDictionaryStringVal( dicPageControlInfo, MODEL_INFO_ENGINE.PRODUCT_MODEL_CD.bindField );
                    stInfo.Serial = DataUtils.GetDictionaryStringVal( dicPageControlInfo, MODEL_INFO_ENGINE.SERIAL.bindField );
                }
                if ( true == StringUtils.IsNotEmpty( lineCd ) ) {
                    // ラインコードが入力された場合
                    stInfo.LineCd = lineCd;
                    stInfo.ProcessCd = processCd;
                }
            }

            return stInfo;
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

        /// <summary>
        /// 一覧項目選択
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ContentLabel_Click( object sender, EventArgs e ) {
            base.RaiseEvent( SelectContentLabel, sender, e );
        }

        #endregion

        #region イベントメソッド

        #region ページイベント

        /// <summary>
        /// ページロード処理
        /// </summary>
        protected override void DoPageLoad() {
            //ベース ページロード処理
            base.DoPageLoad();

            //一覧項目作成
            SetContentsLabel();

            //一覧からの遷移 初回のみ
            if ( false == IsPostBack
                && false == ScriptManager.GetCurrent( Page ).IsInAsyncPostBack ) {
                //詳細画面用セッションのクリア
                ClearDetailSession();

                //一覧検索条件に工程/部品が含まれている時には詳細リスト項目の初期選択とする
                string callerToken = StringUtils.Nvl( base.GetTransInfoValue( RequestParameter.Common.TOKEN ) );
                string callerPageId = StringUtils.Nvl( base.GetTransInfoValue( RequestParameter.Common.CALLER ) );

                // 検索条件情報
                ConditionInfoSessionHandler.ST_CONDITION conditionInfo;
                string selectedGroupCd = string.Empty;
                string selectedClassCd = string.Empty;
                // 選択ラインコード
                string selectedLineCd = string.Empty;
                // 選択工程コード
                string selectedProcessCd = string.Empty;
                string processCd = string.Empty;
                string partsCd = string.Empty;
                // ラインコード
                string lineCd = string.Empty;
                // 検査工程コード
                string checkProcessCd = string.Empty;

                //呼び出し元ページによってパラメータ切り替え
                if ( callerPageId == PageInfo.MainProcessView.pageId ) {
                    //工程検索からの遷移
                    conditionInfo = SessionManager.GetConditionInfoHandler( callerToken ).GetCondition( PageInfo.MainProcessView.pageId );
                    // 検索条件からラインコード、工程コード、検査工程コードを取得
                    lineCd = DataUtils.GetLineCd(
                        productKind: DataUtils.GetDictionaryStringVal( conditionInfo.conditionValue, SearchConditionDefine.CONDITION_COMMON.PRODUCT_KIND.bindField ),
                        inspectionCd: DataUtils.GetDictionaryStringVal( conditionInfo.conditionValue, SearchConditionDefine.CONDITION_COMMON.PROCESS_KIND.bindField ) ) ?? lineCd;
                    processCd = DataUtils.GetProcessCd(
                        productKind: DataUtils.GetDictionaryStringVal( conditionInfo.conditionValue, SearchConditionDefine.CONDITION_COMMON.PRODUCT_KIND.bindField ),
                        inspectionCd: DataUtils.GetDictionaryStringVal( conditionInfo.conditionValue, SearchConditionDefine.CONDITION_COMMON.PROCESS_KIND.bindField ) ) ?? processCd;
                    checkProcessCd = DataUtils.GetDictionaryStringVal( conditionInfo.conditionValue, SearchConditionDefine.CONDITION_COMMON.PROCESS_KIND.bindField );
                } else {
                    //製品検索からの遷移（製品検索はGETパラメータに読み出し元ページIDを設定していないためcallerPageIdは空文字となる）
                    conditionInfo = SessionManager.GetConditionInfoHandler( callerToken ).GetCondition( PageInfo.MainView.pageId );
                    lineCd = DataUtils.GetDictionaryStringVal( conditionInfo.conditionValue, MainView.CONDITION.LINE_CD.bindField );
                    processCd = DataUtils.GetDictionaryStringVal( conditionInfo.conditionValue, MainView.CONDITION.PROCESS_CD.bindField );
                    partsCd = DataUtils.GetDictionaryStringVal( conditionInfo.conditionValue, MainView.CONDITION.PARTS_CD.bindField );
                    if ( true == StringUtils.IsNotEmpty( processCd ) ) {
                        // 工程コードが入力されている場合
                        // 検索対象フラグ
                        var searchTargetFlag = DataUtils.GetDictionaryStringVal( conditionInfo.conditionValue, MainView.CONDITION.SEARCH_TARGET_FLAG.bindField );
                        if ( "0" == searchTargetFlag ) {
                            // 検索対象フラグがOFFの場合、検査工程コードを取得（取得できなかった場合空文字のまま）
                            checkProcessCd = DataUtils.GetInspectionCd(
                                lineCd: DataUtils.GetDictionaryStringVal( conditionInfo.conditionValue, MainView.CONDITION.LINE_CD.bindField ),
                                processCd: DataUtils.GetDictionaryStringVal( conditionInfo.conditionValue, MainView.CONDITION.PROCESS_CD.bindField ),
                                productKind: DataUtils.GetDictionaryStringVal( conditionInfo.conditionValue, MainView.CONDITION.PRODUCT_KIND_CD.bindField ) ) ?? checkProcessCd;
                        } else if ( "1" == searchTargetFlag ) {
                            // 検索対象フラグがONの場合
                            checkProcessCd = ProcessKind.PROCESS_CD_COMMON_PROCESS;
                        }
                    }
                }

                //ページの指定がある場合
                string pageID = StringUtils.Nvl( base.GetTransInfoValue( RequestParameter.DetailFrame.PAGE_ID ) );
                if ( true == StringUtils.IsNotBlank( checkProcessCd ) ) {
                    selectedGroupCd = Defines.ListDefine.GroupCd.Process;
                    selectedClassCd = checkProcessCd;
                    selectedLineCd = lineCd;
                    selectedProcessCd = processCd;
                } else if ( true == StringUtils.IsNotBlank( partsCd ) ) {
                    selectedGroupCd = Defines.ListDefine.GroupCd.Parts;
                    selectedClassCd = partsCd;
                } else if ( true == StringUtils.IsNotBlank( pageID ) ) {
                    //ページIDからグループコードとクラスコードを設定
                    PageInfo.GetPageCdInfo( pageID, out selectedGroupCd, out selectedClassCd );
                }
                hdnSelectedGroupCd.Value = selectedGroupCd;
                hdnSelectedClassCd.Value = selectedClassCd;
                hdnSelectedLineCd.Value = selectedLineCd;
                hdnSelectedProcessCd.Value = selectedProcessCd;
                // 工程アコーディオンコントロールを取得
                var expProcesses = pnlContentList.Controls.OfType<KTExpander>().Where( x => x.lblTitle.Text == DETAIL_LIST_PROCESS_NM ).FirstOrDefault();
                // 部品アコーディオンコントロールを取得
                var expParts = pnlContentList.Controls.OfType<KTExpander>().Where( x => x.lblTitle.Text == DETAIL_LIST_PARTS_NM ).FirstOrDefault();
                if ( selectedGroupCd == GroupCd.Process ) {
                    // グループコードが1の場合
                    // 工程アコーディオンコントロールを全展開
                    expProcesses.OpenAll();
                    // 部品アコーディオンコントロールを展開
                    expParts.Open();
                    // 工程一覧初期選択処理
                    InitialSelectProcessView( selectedLineCd, selectedProcessCd );
                } else if ( selectedGroupCd == GroupCd.Parts ) {
                    // グループコードが2の場合
                    // 工程アコーディオンコントロールを全展開
                    expProcesses.OpenAll();
                    // 部品アコーディオンコントロールを展開
                    expParts.Open();
                    if ( null != expParts ) {
                        // 部品アコーディオンコントロールが存在する場合
                        foreach ( Control ctrl in expParts.IncludingControls ) {
                            // 部品アコーディオンコントロールに含まれるコントロールを操作する
                            if ( ctrl.GetType() == typeof( Button ) ) {
                                string groupCd = ( (Button)ctrl ).Attributes[DETAIL_LIST_ATTR_GROUP];
                                string classCd = ( (Button)ctrl ).Attributes[DETAIL_LIST_ATTR_CLASS];
                                if ( true == StringUtils.IsNotBlank( groupCd ) && true == StringUtils.IsNotBlank( classCd )
                                    && groupCd == selectedGroupCd && classCd == selectedClassCd ) {
                                    SelectContentLabel( ctrl );
                                    break;
                                }
                            }
                        }
                    }
                }
            } else {
                //詳細画面作成
                string selectGroupCd = hdnSelectedGroupCd.Value;
                string selectClassCd = hdnSelectedClassCd.Value;
                SetUserControl( selectGroupCd, selectClassCd, null, null, false );
            }
        }

        #endregion

        #region 一覧選択イベント
        /// <summary>
        /// 一覧項目ボタン押下
        /// </summary>
        /// <param name="parameters"></param>
        private void SelectContentLabel( params object[] parameters ) {
            //詳細画面用セッションのクリア
            ClearDetailSession();

            // 選択済みのボタンを解除
            // 一覧項目の子コントロールを取得
            GetEndControls( pnlContentList )
                // ボタンコントロールを抽出
                .Where( x => x.GetType() == typeof( Button ) ).Cast<Button>().ToList()
                // クラスから選択状態クラスを削除
                .ForEach( btn => btn.CssClass = btn.CssClass.Replace( DETAIL_LIST_SELECTED_CONTENT_CSS, "" ) );

            object sender = parameters[0];
            Button SelectedButton = ( (Button)sender );
            string selectGroupCd = SelectedButton.Attributes[DETAIL_LIST_ATTR_GROUP];
            string selectClassCd = SelectedButton.Attributes[DETAIL_LIST_ATTR_CLASS];
            string selectLineCd = ( GroupCd.Parts == SelectedButton.Attributes[DETAIL_LIST_ATTR_GROUP] ) ? null : SelectedButton.Attributes[DETAIL_LIST_ATTR_LINECD];
            string selectProcessCd = ( GroupCd.Parts == SelectedButton.Attributes[DETAIL_LIST_ATTR_GROUP] ) ? null : SelectedButton.Attributes[DETAIL_LIST_ATTR_PROCESSCD];
            string selectProcessNm = ( GroupCd.Parts == SelectedButton.Attributes[DETAIL_LIST_ATTR_GROUP] ) ? null : SelectedButton.Text;
            SelectedButton.CssClass += " " + DETAIL_LIST_SELECTED_CONTENT_CSS;

            //詳細画面セット
            hdnSelectedGroupCd.Value = selectGroupCd;
            hdnSelectedClassCd.Value = selectClassCd;
            hdnSelectedLineCd.Value = selectLineCd;
            hdnSelectedProcessCd.Value = selectProcessCd;
            SetUserControl( selectGroupCd, selectClassCd, selectLineCd, selectProcessCd, true, selectProcessNm );
        }

        /// <summary>
        /// 検索対象のコントロールに含まれる末端コントロールをすべて取得
        /// </summary>
        /// <param name="parent">検索対象</param>
        /// <returns>末端コントロール一覧</returns>
        private IEnumerable<Control> GetEndControls( Control parent ) {
            foreach ( Control child in parent.Controls ) {
                // 検索対象の子コントロールを列挙する
                if ( child.Controls.Count > 0 ) {
                    // 子コントロールが子コントロールを含むコントロールの場合
                    foreach ( var grandChild in GetEndControls( child ) ) {
                        // 子コントロールの末端コントロールを孫コントロールとして列挙し、末端として取得
                        yield return grandChild;
                    }
                } else {
                    // 検索対象に含まれる、子コントロールを含まないコントロールを末端として取得
                    yield return child;
                }
            }
        }

        #endregion

        #endregion

        #region メソッド

        #region 画面初期化処理
        /// <summary>
        /// 初期化処理
        /// </summary>
        protected override void Initialize() {
            //ベース処理初期化処理
            base.Initialize();

            //型式情報欄取得
            string modelCd = "";
            string productModelCd = "";
            string productCountryCd = "";
            string serial = "";

            //本機/搭載エンジン情報欄を非表示(初期化)
            divTractorInfo.Visible = false;
            divEngineInfo.Visible = false;

            string coop = StringUtils.Nvl( base.GetTransInfoValue( RequestParameter.DetailFrame.EXTERNAL_COOP ) );
            if ( true == StringUtils.IsNotBlank( coop ) ) {

                //MACs連携
                if ( coop == RequestParameter.ExternalCoop.MACs ) {
                    productModelCd = StringUtils.Nvl( base.GetTransInfoValue( RequestParameter.DetailFrame.MODEL_CD ) );
                    string countryCd = StringUtils.Nvl( base.GetTransInfoValue( RequestParameter.DetailFrame.COUNTRY_CD ) );
                    string serial6 = StringUtils.Nvl( base.GetTransInfoValue( RequestParameter.DetailFrame.SERIAL ) );

                    logger.Info( "詳細画面表示(MACs連携) 型式:{0} 国:{1} 機番:{2}", productModelCd, countryCd, serial6 );

                    //国コード
                    productCountryCd = DataUtils.GetCountryCd( countryCd );

                    //7桁機番取得
                    serial = DataUtils.GetSerial( productModelCd, serial6 );

                }
            } else {
                //一覧表示
                string callerToken = StringUtils.Nvl( base.GetTransInfoValue( RequestParameter.Common.TOKEN ) );
                string callerPageId = StringUtils.Nvl( base.GetTransInfoValue( RequestParameter.Common.CALLER ) );
                string indexStr = StringUtils.Nvl( base.GetTransInfoValue( RequestParameter.DetailFrame.INDEX ) );
                int index = NumericUtils.ToInt( indexStr, -1 );
                //一覧からの遷移
                if ( 0 <= index ) {
                    //一覧の検索情報を取得
                    if ( callerPageId == PageInfo.MainProcessView.pageId ) {
                        ConditionInfoSessionHandler.ST_CONDITION conditionInfo = SessionManager.GetConditionInfoHandler( callerToken ).GetCondition( PageInfo.MainProcessView.pageId );
                        if ( true == ObjectUtils.IsNotNull( conditionInfo.ResultData )
                            && index < conditionInfo.ResultData.Rows.Count ) {
                            DataRow rowProcessView = conditionInfo.ResultData.Rows[index];
                            modelCd = rowProcessView[Defines.ProcessViewDefine.SearchResultDefine.GRID_COMMON.MODEL_CD.bindField].ToString();
                            productModelCd = rowProcessView[Defines.ProcessViewDefine.SearchResultDefine.GRID_COMMON.MODEL_CD.bindField].ToString();
                            productCountryCd = rowProcessView[Defines.ProcessViewDefine.SearchResultDefine.GRID_COMMON.COUNTRY_CD.bindField].ToString();
                            serial = rowProcessView[Defines.ProcessViewDefine.SearchResultDefine.GRID_COMMON.SERIAL7.bindField].ToString();

                            logger.Info( "詳細画面表示(一覧画面から遷移) 型式:{0} 国:{1} 機番:{2}", productModelCd, productCountryCd, serial );

                        }
                    } else {
                        ConditionInfoSessionHandler.ST_CONDITION conditionInfo = SessionManager.GetConditionInfoHandler( callerToken ).GetCondition( PageInfo.MainView.pageId );
                        if ( true == ObjectUtils.IsNotNull( conditionInfo.ResultData )
                            && index < conditionInfo.ResultData.Rows.Count ) {
                            DataRow rowMainView = conditionInfo.ResultData.Rows[index];
                            modelCd = rowMainView[MainView.GRID_PRODUCT_COMMON.MODEL_CD.bindField].ToString();
                            productModelCd = rowMainView[MainView.GRID_PRODUCT_COMMON.PRODUCT_MODEL_CD.bindField].ToString();
                            productCountryCd = rowMainView[MainView.GRID_PRODUCT_COMMON.PRODUCT_COUNTRY_CD.bindField].ToString();
                            serial = rowMainView[MainView.GRID_PRODUCT_COMMON.SERIAL.bindField].ToString();

                            logger.Info( "詳細画面表示(一覧画面から遷移) 型式:{0} 国:{1} 機番:{2}", productModelCd, productCountryCd, serial );

                        }
                    }
                } else {
                    //本機/搭載エンジン切替表示
                    productModelCd = StringUtils.Nvl( base.GetTransInfoValue( RequestParameter.DetailFrame.MODEL_CD ) );
                    productCountryCd = StringUtils.Nvl( base.GetTransInfoValue( RequestParameter.DetailFrame.COUNTRY_CD ) );
                    serial = StringUtils.Nvl( base.GetTransInfoValue( RequestParameter.DetailFrame.SERIAL ) );

                    logger.Info( "詳細画面表示(本機/搭載エンジン切替) 型式:{0} 国:{1} 機番:{2}", productModelCd, productCountryCd, serial );

                }
            }

            //製品情報取得
            DataTable tblProduct = DetailViewBusiness.SearchProductDetail( productModelCd, productCountryCd, serial );
            if ( null == tblProduct || 1 != tblProduct.Rows.Count ) {
                //製品検索失敗
                logger.Error( "製品情報取得失敗 型式={0} 国={1} 機番={2}", productModelCd, productCountryCd, serial );
                base.WriteApplicationMessage( MsgManager.MESSAGE_ERR_82010 );
                return;
            }

            //初期化、初期値設定
            InitializeValues( tblProduct.Rows[0] );

        }

        /// <summary>
        /// 初期値設定
        /// </summary>
        private void InitializeValues( DataRow rowModelProduct ) {
            Dictionary<string, object> dicControlValues = new Dictionary<string, object>();
            base.SetControlValues( ModelInfoControls, rowModelProduct, ref dicControlValues );

            //取得データをセッションに格納
            base.SetPageControlInfo( Defines.Session.DetailFrame.SESSION_PAGE_INFO_MODEL_INFO_KEY, dicControlValues );

            string assemblyPatternCd = dicControlValues[MODEL_INFO_PRODUCT.ASSEMBLY_PATTERN_CD.bindField].ToString();

            //製品情報
            lblProductInfo.Text = MODEL_INFO_TITLE_PRODUCT;

            string assemblyPatternNm = Defines.AssemblyPatternCode.GetName( assemblyPatternCd );
            if ( true == StringUtils.IsNotBlank( assemblyPatternNm ) ) {
                lblProductInfo.Text = String.Format( "{0}({1})", MODEL_INFO_TITLE_PRODUCT, assemblyPatternNm );
            }

            //搭載エンジンの時に、本機情報ウィンドウを表示
            if ( Defines.AssemblyPatternCode.InstalledEngine03 == assemblyPatternCd
                || Defines.AssemblyPatternCode.InstalledEngine07 == assemblyPatternCd ) {

                string modelCd = DataUtils.GetDictionaryStringVal( dicControlValues, MODEL_INFO_TRACTOR.PRODUCT_MODEL_CD.bindField );
                string countryCd = DataUtils.GetDictionaryStringVal( dicControlValues, MODEL_INFO_TRACTOR.PRODUCT_COUNTRY_CD.bindField );
                string serial = DataUtils.GetDictionaryStringVal( dicControlValues, MODEL_INFO_TRACTOR.SERIAL.bindField );
                if ( true == StringUtils.IsNotEmpty( modelCd ) && true == StringUtils.IsNotEmpty( serial ) ) {
                    divTractorInfo.Visible = true;
                    btnTractorTransfer.OnClientClick
                        = String.Format( TRANSFER_MODEL_CLICK
                        , PageInfo.ResolveClientUrl( this, PageInfo.DetailFrame )
                        , this.Token
                        , modelCd
                        , countryCd
                        , serial
                    );
                }
            }

            //トラクタの時に、搭載エンジン情報ウィンドウを表示
            if ( Defines.AssemblyPatternCode.Tractor == assemblyPatternCd ) {

                string modelCd = DataUtils.GetDictionaryStringVal( dicControlValues, MODEL_INFO_ENGINE.PRODUCT_MODEL_CD.bindField );
                string countryCd = DataUtils.GetDictionaryStringVal( dicControlValues, MODEL_INFO_ENGINE.PRODUCT_COUNTRY_CD.bindField );
                string serial = DataUtils.GetDictionaryStringVal( dicControlValues, MODEL_INFO_ENGINE.SERIAL.bindField );
                if ( true == StringUtils.IsNotEmpty( modelCd ) && true == StringUtils.IsNotEmpty( serial ) ) {
                    divEngineInfo.Visible = true;
                    btnEngineTransfer.OnClientClick
                        = String.Format( TRANSFER_MODEL_CLICK
                        , PageInfo.ResolveClientUrl( this, PageInfo.DetailFrame )
                        , this.Token
                        , modelCd
                        , countryCd
                        , serial
                    );
                }
            }

            //製品別通過実績の設定
            SetImaDokoData( assemblyPatternCd );

        }

        /// <summary>
        /// 一覧項目の作成
        /// </summary>
        private void SetContentsLabel() {
            pnlContentList.Controls.Clear();
            // 工程部の作成
            // 工程アコーディオンコントロール
            var expProcessTitle = new KTExpander() {
                // 表示タイトル、CSSの設定
                TitleText = DETAIL_LIST_PROCESS_NM,
                CssPanelClass = DETAIL_LIST_TITLE_CSS,
            };
            // 一覧項目パネルに追加
            pnlContentList.Controls.Add( expProcessTitle );
            // ライン工程リストを取得
            var lineProcessList = LineProcessDao.SelectLineProcessList( hdnProductKind.Value, hdnAssemblyPatternCd.Value )
                .AsEnumerable()
                .Select( x => new {
                    LineCd = StringUtils.ToString( x[DETAIL_LIST_ATTR_LINECD] ),
                    LineShortNm = StringUtils.ToString( x[DETAIL_LIST_ATTR_LINENM] ),
                    ProcessCd = StringUtils.ToString( x[DETAIL_LIST_ATTR_PROCESSCD] ),
                    ProcessNm = StringUtils.ToString( x[DETAIL_LIST_ATTR_PROCESSNM] ),
                    SearchTargetFlg = NumericUtils.ToInt( x[DETAIL_LIST_ATTR_SEARCHTARGETFLG] )
                } );
            // ラインコード、ライン短縮名の重複を除いてラインリストを取得
            var lineList = lineProcessList
                .Select( x => new { x.LineCd, x.LineShortNm } )
                .Distinct();
            foreach ( var lineInfo in lineList ) {
                // ラインアコーディオンコントロールを作成
                var expLineContentWk = new KTExpander() {
                    // 表示タイトル、CSS、属性の設定
                    TitleText = $"{lineInfo.LineCd}:{StringUtils.PaddingRight( lineInfo.LineShortNm, 6 )} &nbsp;",

                    /////////////////////////荻野追加部分 -START- /////////////////////////
                    CssPanelClass = DETAIL_LIST_LINE_CSS,
                    /////////////////////////荻野追加部分 -end- /////////////////////////

                };
                expLineContentWk.Attributes[DETAIL_LIST_ATTR_LINECD] = lineInfo.LineCd;
                // 工程リストの作成
                var processList = lineProcessList.Where( x => x.LineCd == lineInfo.LineCd );
                foreach ( var processInfo in processList ) {
                    // 検査工程コード
                    var checkProcessCd = ProcessKind.PROCESS_CD_COMMON_PROCESS;
                    if ( 0 == processInfo.SearchTargetFlg ) {
                        // 検索対象フラグがOFFの場合
                        checkProcessCd = DataUtils.GetInspectionCd(
                            lineCd: lineInfo.LineCd,
                            processCd: processInfo.ProcessCd,
                            productKind: hdnProductKind.Value ) ?? checkProcessCd;
                    }
                    // 工程ボタンを作成
                    Button btnProcessContentWk = new Button();
                    // 表示タイトル、CSS、属性の設定
                    btnProcessContentWk.Text = processInfo.ProcessNm;
                    btnProcessContentWk.Attributes[DETAIL_LIST_ATTR_GROUP] = Defines.ListDefine.GroupCd.Process;
                    btnProcessContentWk.Attributes[DETAIL_LIST_ATTR_CLASS] = checkProcessCd;
                    btnProcessContentWk.Attributes[DETAIL_LIST_ATTR_LINECD] = lineInfo.LineCd;
                    btnProcessContentWk.Attributes[DETAIL_LIST_ATTR_PROCESSCD] = processInfo.ProcessCd;
                    // 一覧ボタン制御用データチェック処理
                    if ( true == CheckDataExists( Defines.ListDefine.GroupCd.Process, checkProcessCd, lineInfo.LineCd, processInfo.ProcessCd ) ) {
                        // データが存在する場合
                        btnProcessContentWk.CssClass = DETAIL_LIST_CONTENT_CSS;
                        btnProcessContentWk.Click += new System.EventHandler( ContentLabel_Click );
                        btnProcessContentWk.OnClientClick = LIST_CONTENT_CLICK;
                    } else {
                        // データが存在しない場合
                        btnProcessContentWk.CssClass = DETAIL_LIST_NONECONTENT_CSS;
                        btnProcessContentWk.Enabled = false;
                    }
                    // ラインアコーディオンコントロールに工程ボタンを追加
                    expLineContentWk.AddControl( btnProcessContentWk );
                }

                // 工程アコーディオンコントロールにラインアコーディオンコントロールを追加
                expProcessTitle.AddControl( expLineContentWk );
            }
            // 部品部の作成
            var expPartsTitle = new KTExpander() {
                // 表示タイトル、CSSの設定
                TitleText = DETAIL_LIST_PARTS_NM,
                CssPanelClass = DETAIL_LIST_TITLE_CSS,
            };
            pnlContentList.Controls.Add( expPartsTitle );
            // トラクタでもエンジン部品情報を見るため、リストデータの取得方法を分岐させる
            ListItem[] liParts = null;
            if ( hdnProductKind.Value.Equals( ProductKind.Engine ) ) {
                // エンジン選択
                liParts = Dao.Com.MasterList.GetClassItem( hdnProductKind.Value, Defines.ListDefine.GroupCd.Parts, false );
            } else {
                // トラクタ
                liParts = Dao.Com.MasterList.GetTractorItem( hdnProductKind.Value, Defines.ListDefine.GroupCd.Parts, false );
            }
            if ( true == ObjectUtils.IsNotNull( liParts ) && 0 < liParts.Length ) {
                foreach ( ListItem li in liParts ) {
                    Button btnPartsContentWk = new Button();
                    btnPartsContentWk.Text = li.Text;
                    btnPartsContentWk.Attributes[DETAIL_LIST_ATTR_GROUP] = Defines.ListDefine.GroupCd.Parts;
                    btnPartsContentWk.Attributes[DETAIL_LIST_ATTR_CLASS] = li.Value;
                    if ( CheckDataExists( Defines.ListDefine.GroupCd.Parts, li.Value ) ) {
                        btnPartsContentWk.CssClass = DETAIL_LIST_CONTENT_CSS;
                        btnPartsContentWk.Click += new System.EventHandler( ContentLabel_Click );
                        btnPartsContentWk.OnClientClick = LIST_CONTENT_CLICK;
                    } else {
                        btnPartsContentWk.CssClass = DETAIL_LIST_NONECONTENT_CSS;
                        btnPartsContentWk.Enabled = false;
                    }
                    expPartsTitle.AddControl( btnPartsContentWk );
                }
            }
        }
        #endregion

        /// <summary>
        /// 詳細画面セット
        /// </summary>
        /// <param name="groupCd">グループコード</param>
        /// <param name="classCd">クラスコード</param>
        /// <param name="lineCd">ラインコード</param>
        /// <param name="processCd">工程コード</param>
        /// <param name="initialize">初期化実行フラグ</param>
        /// <param name="processNm">工程名</param>
        private void SetUserControl( string groupCd, string classCd, string lineCd, string processCd, bool initialize = true, string processNm = "" ) {

            lblDetailTitle.Text = "";
            pnlDetailControl.Controls.Clear();

            if ( true == StringUtils.IsBlank( groupCd )
                || true == StringUtils.IsBlank( classCd ) ) {
                return;
            }

            PageInfo.ST_PAGE_INFO pageInfo = PageInfo.GetUCPageInfo( hdnProductKind.Value, groupCd, classCd, hdnAssemblyPatternCd.Value, lineCd, processCd );
            if ( true == StringUtils.IsBlank( pageInfo.url ) ) {
                return;
            }

            System.Web.UI.UserControl uc = (System.Web.UI.UserControl)LoadControl( pageInfo.url );
            uc.ID = pageInfo.pageId;
            ( (Defines.Interface.IDetail)uc ).DetailKeyParam
                = GetProductModelInfo( groupCd, classCd, lineCd, processCd );
            //                = new Defines.Interface.ST_DETAIL_PARAM( hdnProductKind.Value, groupCd, classCd
            //                    , txtProductModelCd.Value, txtProductCountryCd.Value, txtProductSerial.Value, txtIdno.Value, hdnAssemblyPatternCd.Value );
            pnlDetailControl.Controls.Add( uc );
            if ( true == initialize ) {
                //アクセスカウンター登録
                Dao.Com.AccessCounterDao.Entry( pageInfo.pageId );

                logger.Info( "詳細画面表示切替:{0} {1} {2}", pageInfo.pageId, pageInfo.title, pageInfo.url );
                ( (Defines.Interface.IDetail)uc ).Initialize();
            }

            //詳細タイトル設定
            if ( false == StringUtils.IsBlank( processNm ) && true == StringUtils.IsBlank( pageInfo.title ) ) {
                //工程名をタイトルに設定
                lblDetailTitle.Text = processNm;
            } else if ( true == pageInfo.Equals( PageInfo.CoreParts ) ) {
                //基幹部品
                //区分名を取得しタイトルに設定
                if ( hdnProductKind.Value.Equals( ProductKind.Engine ) ) {
                    //エンジン
                    lblDetailTitle.Text = Dao.Com.MasterList.GetClassItem( hdnProductKind.Value,
                        Defines.ListDefine.GroupCd.Parts, false ).Where( x => x.Value == classCd ).Select( x => x.Text ).First();
                } else {
                    //トラクタ
                    lblDetailTitle.Text = Dao.Com.MasterList.GetTractorItem( hdnProductKind.Value,
                        Defines.ListDefine.GroupCd.Parts, false ).Where( x => x.Value == classCd ).Select( x => x.Text ).First();
                }
            } else {
                lblDetailTitle.Text = pageInfo.title;
            }

        }

        /// <summary>
        /// 詳細外枠画面で管理する詳細画面(UserControl)用のセッションをクリア
        /// </summary>
        private void ClearDetailSession() {
            //(詳細画面)ページコントロール情報をクリア
            SessionManager.GetPageControlInfoHandler( base.Token ).SetPageControlInfo( Defines.Session.DetailFrame.SESSION_PAGE_INFO_DETAIL_KEY, null );
            //(詳細画面)イメージ情報をクリア
            SessionManager.GetImageInfoHandler( base.Token ).SetImages( Defines.Session.DetailFrame.SESSION_PAGE_INFO_DETAIL_KEY, null );
        }

        #endregion

        #region 一覧ボタン制御用データチェック処理
        /// <summary>
        /// データが存在するかを画面ごとにチェックする
        /// </summary>
        /// <param name="groupCd">グループコード</param>
        /// <param name="execKind">チェック対象区分</param>
        /// <param name="lineCd">ラインコード、指定無しの場合null</param>
        /// <param name="processCd">工程コード、指定無しの場合null</param>
        /// <returns></returns>
        private bool CheckDataExists( string groupCd, string execKind, string lineCd = null, string processCd = null ) {
            if ( GroupCd.Parts == groupCd ) {
                // グループコードが部品の場合、ラインコード、工程コードはnull
                lineCd = null;
                processCd = null;
            }
            bool blRet = false;
            DataTable resultSet = new DataTable();
            string productKind = hdnProductKind.Value;
            Defines.Interface.ST_DETAIL_PARAM DetailKeyParam = GetProductModelInfo( groupCd, execKind, lineCd, processCd );


            if ( productKind.Equals( ProductKind.Engine ) ) {

                ////////////////////////
                //  エンジン（工程）
                ////////////////////////
                if ( groupCd.Equals( GroupCd.Process ) ) {
                    try {
                        switch ( execKind ) {
                        case ProcessKind.PROCESS_CD_ENGINE_TORQUE:      //トルク締付
                            resultSet = Business.DetailViewBusiness.SelectEngineTorqueDetail( DetailKeyParam.ProductModelCd, DetailKeyParam.Serial ).MainTable;
                            break;
                        case ProcessKind.PROCESS_CD_ENGINE_INJECTION:   //噴射時期計測
                            resultSet = Business.DetailViewBusiness.SelectEngineFuelInjectionDetail( DetailKeyParam.AssemblyPatternCd, DetailKeyParam.ProductModelCd, DetailKeyParam.Serial ).MainTable;
                            break;
                        case ProcessKind.PROCESS_CD_ENGINE_FRICTION:    //フリクションロス
                            resultSet = Business.DetailViewBusiness.SelectEngineFrictionLossDetail( DetailKeyParam.ProductModelCd, DetailKeyParam.Serial ).MainTable;
                            break;
                        case ProcessKind.PROCESS_CD_ENGINE_TEST:        //エンジン
                            resultSet = Business.DetailViewBusiness.SelectEngineTestDetail( DetailKeyParam.AssemblyPatternCd, DetailKeyParam.ModelCd, DetailKeyParam.ProductModelCd, DetailKeyParam.Serial ).MainTable;
                            break;
                        case ProcessKind.PROCESS_CD_ENGINE_HARNESS:     //ハーネス
                            resultSet = Business.DetailViewBusiness.SelectEngineHarnessDetail( DetailKeyParam.ProductModelCd, DetailKeyParam.Serial ).MainTable;
                            break;
                        case ProcessKind.PROCESS_CD_ENGINE_CAMIMAGE:    //品質画像証跡
                            resultSet = Business.DetailViewBusiness.SelectEngineCamImageDetail( DetailKeyParam.ProductModelCd, DetailKeyParam.Serial ).MainTable;
                            break;
                        case ProcessKind.PROCESS_CD_ENGINE_CC_INSPECT:  //クランクケース
                            resultSet = Business.DetailViewBusiness.SelectEngine3CInspectionDetail( DetailKeyParam.ClassCd, DetailKeyParam.ProductModelCd, DetailKeyParam.Serial ).MainTable;
                            break;
                        case ProcessKind.PROCESS_CD_ENGINE_CS_INSPECT:  //クランクシャフト
                            resultSet = Business.DetailViewBusiness.SelectEngine3CInspectionDetail( DetailKeyParam.ClassCd, DetailKeyParam.ProductModelCd, DetailKeyParam.Serial ).MainTable;
                            break;
                        case ProcessKind.PROCESS_CD_ENGINE_CYH_INSPECT: //シリンダヘッダ
                            resultSet = Business.DetailViewBusiness.SelectEngine3CInspectionDetail( DetailKeyParam.ClassCd, DetailKeyParam.ProductModelCd, DetailKeyParam.Serial ).MainTable;
                            break;
                        case ProcessKind.PROCESS_CD_ENGINE_ELCHECK:        //電子チェックシート
                            resultSet = Business.DetailViewBusiness.SelectEngineELCheckSheetHeader( DetailKeyParam.ProductModelCd, DetailKeyParam.Serial ).MainTable;
                            break;
                        case ProcessKind.PROCESS_CD_ENGINE_SHIPMENTPARTS: // 出荷部品
                            resultSet = Business.DetailViewBusiness.SelectEngineShipmentPartsDetail( DetailKeyParam.ProductModelCd, DetailKeyParam.Serial ).MainTable;
                            break;
                        case ProcessKind.PROCESS_CD_ENGINE_AIIMAGE: // AI画像解析
                            resultSet = Business.DetailViewBusiness.SelectEngineAiImageDetail( DetailKeyParam.ProductModelCd, DetailKeyParam.Serial ).MainTable;
                            break;
                        case ProcessKind.PROCESS_CD_COMMON_PROCESS:     // 工程共用テーブルを参照する場合
                            resultSet = Dao.Process.CommonProcessDao.SelectNewestProcessWorkHistory( DetailKeyParam.ProductModelCd, DetailKeyParam.Serial, DetailKeyParam.LineCd, DetailKeyParam.ProcessCd );
                            break;
                        default:
                            //処理なし
                            break;
                        }
                    } catch ( DataAccessException ex ) {
                        logger.Exception( ex );
                        return blRet;
                    } catch ( Exception ex ) {
                        logger.Exception( ex );
                        return blRet;
                    } finally {
                    }

                    if ( resultSet.Rows.Count > 0 ) {
                        return true;
                    } else {
                        //検索結果0件
                        return blRet;
                    }
                }

                ////////////////////////
                //  エンジン（部品）
                ////////////////////////
                if ( groupCd.Equals( GroupCd.Parts ) ) {
                    try {
                        switch ( execKind ) {
                        case PartsKind.PARTS_CD_ENGINE_CC:          //クランクケース
                            resultSet = Business.DetailViewBusiness.SelectEngine3cPartsDetail( DetailKeyParam.ClassCd, DetailKeyParam.ProductModelCd, DetailKeyParam.Serial ).MainTable;
                            break;
                        case PartsKind.PARTS_CD_ENGINE_CS:          //クランクシャフト
                            resultSet = Business.DetailViewBusiness.SelectEngine3cPartsDetail( DetailKeyParam.ClassCd, DetailKeyParam.ProductModelCd, DetailKeyParam.Serial ).MainTable;
                            break;
                        case PartsKind.PARTS_CD_ENGINE_CYH:         //シリンダヘッダ
                            resultSet = Business.DetailViewBusiness.SelectEngine3cPartsDetail( DetailKeyParam.ClassCd, DetailKeyParam.ProductModelCd, DetailKeyParam.Serial ).MainTable;
                            break;
                        case PartsKind.PARTS_CD_ENGINE_SUPPLYPUMP:  //サプライポンプ
                            resultSet = Business.DetailViewBusiness.SelectEngineSupplyPumpDetail( DetailKeyParam.ProductModelCd, DetailKeyParam.Serial ).MainTable;
                            break;
                        case PartsKind.PARTS_CD_ENGINE_INJECTOR:    //インジェクタ
                            resultSet = Business.DetailViewBusiness.SelectEngineInjecterDetail( DetailKeyParam.ProductModelCd, DetailKeyParam.Serial ).MainTable;
                            break;
                        case PartsKind.PARTS_CD_ENGINE_ECU:         //ECU
                            resultSet = Business.DetailViewBusiness.SelectEngineEcuDetail( DetailKeyParam.ProductModelCd, DetailKeyParam.Serial ).MainTable;
                            break;
                        case PartsKind.PARTS_CD_ENGINE_EPR:         //EPR
                            resultSet = Business.DetailViewBusiness.SelectEngineEprDetail( DetailKeyParam.ProductModelCd, DetailKeyParam.Serial ).MainTable;
                            break;
                        case PartsKind.PARTS_CD_ENGINE_MIXER:       //MIXER
                            resultSet = Business.DetailViewBusiness.SelectEngineMixerDetail( DetailKeyParam.ProductModelCd, DetailKeyParam.Serial ).MainTable;
                            break;
                        case PartsKind.PARTS_CD_ENGINE_DPF:         //DPF
                            resultSet = Business.DetailViewBusiness.SelectEngineDpfDetail( DetailKeyParam.ProductModelCd, DetailKeyParam.Serial ).MainTable;
                            break;
                        case PartsKind.PARTS_CD_ENGINE_SCR:         //SCR
                            resultSet = Business.DetailViewBusiness.SelectEngineSrcDetail( DetailKeyParam.ProductModelCd, DetailKeyParam.Serial ).MainTable;
                            break;
                        case PartsKind.PARTS_CD_ENGINE_DOC:         //DOC
                            resultSet = Business.DetailViewBusiness.SelectEngineDocDetail( DetailKeyParam.ProductModelCd, DetailKeyParam.Serial ).MainTable;
                            break;
                        case PartsKind.PARTS_CD_ENGINE_ACU:         //ACU
                            resultSet = Business.DetailViewBusiness.SelectEngineAcuDetail( DetailKeyParam.ProductModelCd, DetailKeyParam.Serial ).MainTable;
                            break;
                        case PartsKind.PARTS_CD_ENGINE_SERIALPRINT: //機番ラベル印刷
                            resultSet = Business.DetailViewBusiness.SelectSerialPrint( DetailKeyParam.ProductModelCd, DetailKeyParam.Serial ).MainTable;
                            break;
                        case PartsKind.PARTS_CD_ENGINE_RACK_POSITION_SENSOR: //ラック位置センサ
                            resultSet = Business.DetailViewBusiness.SelectEngineRackPositionSensorDetail( DetailKeyParam.ProductModelCd, DetailKeyParam.Serial ).MainTable;
                            break;
                        case PartsKind.PARTS_CD_ENGINE_IPU:         //IPU
                            resultSet = Business.DetailViewBusiness.SelectEngineIpuDetail( DetailKeyParam.ProductModelCd, DetailKeyParam.Serial ).MainTable;
                            break;
                        case PartsKind.PARTS_CD_ENGINE_EHC:         //EHC
                            resultSet = Business.DetailViewBusiness.SelectEngineEhcDetail( DetailKeyParam.ProductModelCd, DetailKeyParam.Serial ).MainTable;
                            break;
                        default:
                            if ( true == execKind.StartsWith( PartsKind.PARTS_CD_TRACTOR_PREFIX_COREPARTS ) ) {
                                // 基幹部品
                                resultSet = Business.DetailViewBusiness.SelectEngineCorePartsDetail( execKind, DetailKeyParam.ProductModelCd, DetailKeyParam.Serial ).MainTable;
                            }
                            break;
                        }
                    } catch ( DataAccessException ex ) {
                        logger.Exception( ex );
                        return blRet;
                    } catch ( Exception ex ) {
                        logger.Exception( ex );
                        return blRet;
                    } finally {
                    }

                    if ( resultSet.Rows.Count > 0 ) {
                        return true;
                    } else {
                        //検索結果0件
                        return blRet;
                    }
                }

            } else if ( productKind.Equals( ProductKind.Tractor ) ) {
                ////////////////////////
                //  トラクタ（工程）
                ////////////////////////

                if ( groupCd.Equals( GroupCd.Process ) ) {
                    try {
                        switch ( execKind ) {
                        case ProcessKind.PROCESS_CD_TRACTOR_CHKSHEET:       //チェックシート
                            resultSet = Business.DetailViewBusiness.SelectTractorCheckSheetDetail( DetailKeyParam.ProductModelCd, DetailKeyParam.Serial ).MainTable;
                            break;
                        case ProcessKind.PROCESS_CD_TRACTOR_ELCHECK:        //電子チェックシート
                            resultSet = Business.DetailViewBusiness.SelectELCheckSheetHeader( DetailKeyParam.ProductModelCd, DetailKeyParam.Serial ).MainTable;
                            break;
                        case ProcessKind.PROCESS_CD_TRACTOR_PCRAWER:        //パワクロ
                            resultSet = Business.DetailViewBusiness.SelectTractorPCrawlerDetail( DetailKeyParam.ProductModelCd, DetailKeyParam.Serial ).MainTable;
                            break;
                        case ProcessKind.PROCESS_CD_TRACTOR_CAMIMAGE:       //品質画像証跡
                            resultSet = Business.DetailViewBusiness.SelectTractorCamImageDetail( DetailKeyParam.ProductModelCd, DetailKeyParam.Serial ).MainTable;
                            break;
                        case ProcessKind.PROCESS_CD_TRACTOR_SHEEL:            //前車軸フレーム刻印
                                                                              //ステーションはとりあえず値固定としているので、刻印の種類が増えた場合はシステム共通のコンストファイル等を作成するなどで所持させる
                            string CONST_STATION_CD = "214302";
                            resultSet = Business.DetailViewBusiness.SelectPrintSheel( DetailKeyParam.ProductModelCd, DetailKeyParam.Serial, CONST_STATION_CD, DetailKeyParam.CountryCd ).MainTable;
                            break;
                        case ProcessKind.PROCESS_CD_TRACTOR_NUTRUNNER:      //ナットランナー
                            resultSet = Business.DetailViewBusiness.SelectTractorNutRunnerDetail( DetailKeyParam.ProductModelCd, DetailKeyParam.Serial ).MainTable;
                            break;
                        case ProcessKind.PROCESS_CD_TRACTOR_ALL:      //トラクタ走行検査
                            resultSet = Business.DetailViewBusiness.SelectTractorAllDetail( DetailKeyParam.ProductModelCd, DetailKeyParam.Serial ).MainTable;
                            break;
                        case ProcessKind.PROCESS_CD_TRACTOR_OPTAXIS:      //光軸検査
                            resultSet = Business.DetailViewBusiness.SelectOptaxisDetail( DetailKeyParam.ProductModelCd, DetailKeyParam.Serial ).MainTable;
                            break;
                        case ProcessKind.PROCESS_CD_TRACTOR_CHKPOINT:          // 関所
                            resultSet = Business.DetailViewBusiness.SelectCheckPointHeader( DetailKeyParam.ProductModelCd, DetailKeyParam.Serial, DetailKeyParam.LineCd ).MainTable;
                            break;
                        case ProcessKind.PROCESS_CD_TRACTOR_IMGCHECK:        //イメージチェックシート
                            resultSet = Business.DetailViewBusiness.SelectTractorImgCheckSheetHeader( DetailKeyParam.ProductModelCd, DetailKeyParam.Serial ).MainTable;
                            break;
                        case ProcessKind.PROCESS_CD_TRACTOR_TESTBENCH:        // 検査ベンチ
                            resultSet = Business.DetailViewBusiness.SelectTractorTestBenchDetail( DetailKeyParam.ProductModelCd, DetailKeyParam.Serial ).MainTable;
                            break;
                        case ProcessKind.PROCESS_CD_TRACTOR_AIIMAGE:    // AI画像解析
                            resultSet = Business.DetailViewBusiness.SelectTractorAiImageDetail( DetailKeyParam.ProductModelCd, DetailKeyParam.Serial ).MainTable;
                            break;
                        case ProcessKind.PROCESS_CD_COMMON_PROCESS:          // 工程共用テーブルを参照する場合
                            resultSet = Dao.Process.CommonProcessDao.SelectNewestProcessWorkHistory( DetailKeyParam.ProductModelCd, DetailKeyParam.Serial, DetailKeyParam.LineCd, DetailKeyParam.ProcessCd );
                            break;
                        default:
                            //処理なし
                            break;
                        }
                    } catch ( DataAccessException ex ) {
                        logger.Exception( ex );
                        return blRet;
                    } catch ( Exception ex ) {
                        logger.Exception( ex );
                        return blRet;
                    } finally {
                    }

                    if ( resultSet.Rows.Count > 0 ) {
                        return true;
                    } else {
                        //検索結果0件
                        return blRet;
                    }
                }

                ////////////////////////
                //  トラクタ（部品）
                ////////////////////////
                if ( groupCd.Equals( GroupCd.Parts ) ) {
                    string engineModelCd = DataUtils.GetModelCd( StringUtils.ToString( txtEngineModelCd.Text ) );     //エンジン型式
                    string engineSerial = DataUtils.GetSerial6( StringUtils.ToString( txtEngineSerial.Text ) );       //エンジン機番

                    try {
                        switch ( execKind ) {
                        case PartsKind.PARTS_CD_TRACTOR_WECU:       //WiFi ECU
                            resultSet = Business.DetailViewBusiness.SelectTractorWifiEcuDetail( DetailKeyParam.ProductModelCd, DetailKeyParam.Serial ).MainTable;
                            break;
                        case PartsKind.PARTS_CD_TRACTOR_CRATE:      //クレート
                            resultSet = Business.DetailViewBusiness.SelectTractorCrateDetail( DetailKeyParam.ProductModelCd, DetailKeyParam.Idno ).MainTable;
                            break;
                        case PartsKind.PARTS_CD_TRACTOR_ROPS:       //ロプス
                            resultSet = Business.DetailViewBusiness.SelectTractorRopsDetail( DetailKeyParam.ProductModelCd, DetailKeyParam.Idno ).MainTable;
                            break;
                        case PartsKind.PARTS_CD_TRACTOR_NAMEPLATE:  //銘板ラベル
                            resultSet = Business.DetailViewBusiness.SelectTractorNameplate( DetailKeyParam.ProductModelCd, DetailKeyParam.Serial ).MainTable;
                            break;
                        case PartsKind.PARTS_CD_TRACTOR_MISSION:    //ミッション
                            resultSet = Business.DetailViewBusiness.SelectTractorMissionDetail( DetailKeyParam.ProductModelCd, DetailKeyParam.Serial ).MainTable;
                            break;
                        case PartsKind.PARTS_CD_TRACTOR_HOUSING:    //ハウジング
                            resultSet = Business.DetailViewBusiness.SelectTractorHousingDetail( DetailKeyParam.ProductModelCd, DetailKeyParam.Serial ).MainTable;
                            break;

                        //トラクタでエンジン部品を参照する対応
                        //エンジン型式、エンジン機番で検索を行う
                        case PartsKind.PARTS_CD_ENGINE_DPF:         //DPF
                            resultSet = Business.DetailViewBusiness.SelectEngineDpfDetail( engineModelCd, engineSerial ).MainTable;
                            break;
                        case PartsKind.PARTS_CD_ENGINE_SCR:         //SCR
                            resultSet = Business.DetailViewBusiness.SelectEngineSrcDetail( engineModelCd, engineSerial ).MainTable;
                            break;
                        case PartsKind.PARTS_CD_ENGINE_ACU:         //ACU
                            resultSet = Business.DetailViewBusiness.SelectEngineAcuDetail( engineModelCd, engineSerial ).MainTable;
                            break;
                        case PartsKind.PARTS_CD_ENGINE_RACK_POSITION_SENSOR: //ラック位置センサ
                            resultSet = Business.DetailViewBusiness.SelectEngineRackPositionSensorDetail( DetailKeyParam.ProductModelCd, DetailKeyParam.Serial ).MainTable;
                            break;
                        default:
                            if ( true == execKind.StartsWith( PartsKind.PARTS_CD_TRACTOR_PREFIX_COREPARTS ) ) {
                                // 基幹部品
                                resultSet = Business.DetailViewBusiness.SelectTractorCorePartsDetail( execKind, DetailKeyParam.ProductModelCd, DetailKeyParam.Serial ).MainTable;
                            }
                            break;
                        }
                    } catch ( DataAccessException ex ) {
                        logger.Exception( ex );
                        return blRet;
                    } catch ( Exception ex ) {
                        logger.Exception( ex );
                        return blRet;
                    } finally {
                    }

                    if ( resultSet.Rows.Count > 0 ) {
                        return true;
                    } else {
                        //検索結果0件
                        return blRet;
                    }
                }
            } else {
                //処理なし
                return blRet;
            }
            return true;
        }
        #endregion

        #region 工程一覧初期選択処理
        /// <summary>
        /// 工程一覧初期選択処理
        /// </summary>
        /// <param name="selectedLineCd">選択ラインコード</param>
        /// <param name="selectedProcessCd">選択工程コード</param>
        private void InitialSelectProcessView( string selectedLineCd, string selectedProcessCd ) {
            // 工程アコーディオンコントロールを取得
            var expProcesses = pnlContentList.Controls.OfType<KTExpander>().Where( x => x.lblTitle.Text == DETAIL_LIST_PROCESS_NM ).FirstOrDefault();
            if ( null != expProcesses ) {
                // 工程アコーディオンコントロールが存在する場合
                foreach ( var ctrlA in expProcesses.IncludingControls.OfType<KTExpander>() ) {
                    // アコーディオンコントロールAを展開
                    ctrlA.Open();
                    var lineCd = ctrlA.Attributes[DETAIL_LIST_ATTR_LINECD];
                    if ( true == StringUtils.IsNotEmpty( lineCd ) && lineCd == selectedLineCd ) {
                        // ラインコードが設定され、選択ラインコードと同じ場合
                        foreach ( var ctrlB in ctrlA.IncludingControls.OfType<Button>() ) {
                            // アコーディオンコントロールA内部のボタンコントロールBを取得
                            var processCd = ctrlB.Attributes[DETAIL_LIST_ATTR_PROCESSCD];
                            if ( true == StringUtils.IsNotEmpty( processCd ) && processCd == selectedProcessCd ) {
                                // 工程コードが設定され、選択工程コードと同じ場合、一覧項目ボタン押下処理を実行
                                SelectContentLabel( ctrlB );
                                break;
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region 製品別通過実績
        /// <summary>
        /// 製品別通過実績取得
        /// </summary>
        /// <param name="assemblyPatternCd">組立パターン</param>
        private void SetImaDokoData( string assemblyPatternCd ) {

            Defines.Interface.ST_DETAIL_PARAM DetailKeyParam = GetProductModelInfo( hdnSelectedGroupCd.Value, hdnSelectedClassCd.Value );
            DataTable tblProcess = new DataTable();     //工程
            DataTable tblResult = new DataTable();      //実績
            DataTable tblView = new DataTable();        //画面表示用

            try {
                //データ取得
                //工程
                tblProcess = Business.DetailViewBusiness.SelectImaDokoProcess( DetailKeyParam.ProductModelCd, DetailKeyParam.Serial, assemblyPatternCd );
                //実績
                tblResult = Business.DetailViewBusiness.SelectImaDokoResult( DetailKeyParam.ProductModelCd, DetailKeyParam.Serial, assemblyPatternCd );

            } catch ( DataAccessException ex ) {
                logger.Exception( ex );
                return;
            } catch ( Exception ex ) {
                logger.Exception( ex );
                return;
            } finally {
            }

            //表示用に加工
            for ( int intProc = 0; intProc < tblProcess.Rows.Count; intProc++ ) {
                tblView.Columns.Add( StringUtils.ToString( tblProcess.Rows[intProc]["表示名称"] ) );
            }

            DataRow row = tblView.NewRow();
            for ( int intProc = 0; intProc < tblProcess.Rows.Count; intProc++ ) {

                string procNo = StringUtils.ToString( tblProcess.Rows[intProc]["工程順序"] );
                row[StringUtils.ToString( tblProcess.Rows[intProc]["表示名称"] )] = "-";

                for ( int intRes = 0; intRes < tblResult.Rows.Count; intRes++ ) {
                    if ( procNo.Equals( StringUtils.ToString( tblResult.Rows[intRes]["工程順序"] ) ) ) {
                        //
                        string resultDT = StringUtils.ToString( tblResult.Rows[intRes]["実績日"] ).PadRight( 9, ' ' ) + Environment.NewLine + " " +
                                          StringUtils.ToString( tblResult.Rows[intRes]["実績時"] );
                        row[StringUtils.ToString( tblProcess.Rows[intProc]["表示名称"] )] = resultDT;
                        break;
                    }
                }
            }

            tblView.Rows.Add( row );

            //データセット
            ( (UI.MasterForm.MasterMain)this.Master ).SetImaDokoData( tblView );

        }
        #endregion

    }
}