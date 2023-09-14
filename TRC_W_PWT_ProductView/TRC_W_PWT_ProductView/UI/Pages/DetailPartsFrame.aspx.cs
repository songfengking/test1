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

namespace TRC_W_PWT_ProductView.UI.Pages {

    /// <summary>
    /// 詳細外枠画面
    /// </summary>
    public partial class DetailPartsFrame : BaseForm {

        //ロガー定義
        private static readonly Logger logger = new Logger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

        #region 定数定義
                
        #region Attribute

        /// <summary>
        /// 詳細一覧項目Attribute属性(GroupCd)
        /// </summary>
        const string DETAIL_LIST_ATTR_GROUP = "GroupCd";
        /// <summary>
        /// 詳細一覧項目Attribute属性(ClassCd)
        /// </summary>
        const string DETAIL_LIST_ATTR_CLASS = "ClassCd";

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
        const string MODEL_INFO_TITLE_PARTS = "部品情報";
        
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

        /// <summary>
        /// 詳細一覧項目 項目選択済用CSS
        /// </summary>
        const string DETAIL_LIST_SELECTED_CONTENT_CSS = "btn-list-content-selected";

        #endregion

        #endregion

        #region 型式情報欄定義
        /// <summary>
        /// 型式情報欄定義(部品情報)
        /// </summary>
        public class MODEL_INFO_ATU {
            /// <summary>機種区分</summary>
            public static readonly ControlDefine MODEL_TYPE = new ControlDefine( "txtModelType", "機種区分", "modelType", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>ATU型式(非表示)</summary>
            public static readonly ControlDefine MODEL_CD = new ControlDefine( "", "ATU型式(非表示)", "modelCd", ControlDefine.BindType.None, typeof( String ) );
            /// <summary>ATU型式</summary>
            public static readonly ControlDefine MODEL_CD_STR = new ControlDefine( "txtModelCd", "ATU型式", "modelCdStr", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>ATU型式名</summary>
            public static readonly ControlDefine MODEL_NM = new ControlDefine( "txtModelNm", "ATU型式名", "modelNm", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>機番</summary>
            public static readonly ControlDefine SERIAL = new ControlDefine( "txtSerial", "機番", "serial", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>IDNO</summary>
            //public static readonly ControlDefine IDNO = new ControlDefine( "txtIdno", "IDNO", "idno", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>フルアッシ品番</summary>
            public static readonly ControlDefine FULL_ASSY_PARTS_NUM = new ControlDefine( "txtfullAssyPartsNum", "フルアッシ品番", "fullAssyPartsNumStr", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>投入順序連番</summary>
            public static readonly ControlDefine THROW_MONTHLY_SEQUENCE_NUM = new ControlDefine( "txtThrowMonthlySequenceNum", "投入順序連番", "throwMonthlySequenceNum", ControlDefine.BindType.Down, typeof( String ) );
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
        ControlDefine[] ModelInfoControls {
            get {
                if ( true == ObjectUtils.IsNull( _modelInfoControls ) ) {
                    List<ControlDefine> modelInfoList = new List<ControlDefine>();
                    modelInfoList.AddRange( ControlUtils.GetControlDefineArray( typeof( MODEL_INFO_ATU ) ) );
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
        /// <returns>型式情報構造体</returns>
        private Defines.Interface.ST_DETAIL_PARTS_PARAM GetPartsModelInfo( string searchTarget, string partsKind, string processCd ) {

            Defines.Interface.ST_DETAIL_PARTS_PARAM stInfo = new Defines.Interface.ST_DETAIL_PARTS_PARAM();
            if ( true == base.PageControlInfo.ContainsKey( Defines.Session.DetailPartsFrame.SESSION_PAGE_INFO_MODEL_INFO_KEY ) ) {
                Dictionary<string, object> dicPageControlInfo = (Dictionary<string, object>)base.PageControlInfo[Defines.Session.DetailPartsFrame.SESSION_PAGE_INFO_MODEL_INFO_KEY];
                stInfo.SearchTarget = searchTarget;
                stInfo.PartsKind = partsKind;
                stInfo.ProcessCd = processCd;
                stInfo.ModelCd = DataUtils.GetDictionaryStringVal( dicPageControlInfo, MODEL_INFO_ATU.MODEL_CD.bindField );
                stInfo.Serial = DataUtils.GetDictionaryStringVal( dicPageControlInfo, MODEL_INFO_ATU.SERIAL.bindField );
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
            base.RaiseEvent(SelectContentLabel, sender, e );
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
                ConditionInfoSessionHandler.ST_CONDITION condMainView = SessionManager.GetConditionInfoHandler( callerToken ).GetCondition( PageInfo.MainPartsView.pageId );
                string selectGroupCd = "";
                string selectClassCd = "";
                string searchTarget = DataUtils.GetDictionaryStringVal( condMainView.conditionValue, MainPartsView.CONDITION.PARTS_SERACH_TARGET.bindField );
                string partsKind = DataUtils.GetDictionaryStringVal( condMainView.conditionValue, MainPartsView.CONDITION.PARTS_KIND.bindField );
                string processCd = DataUtils.GetDictionaryStringVal( condMainView.conditionValue, MainPartsView.CONDITION.PROCESS_CD.bindField );

                //ページの指定がある場合
                string pageID = StringUtils.Nvl( base.GetTransInfoValue( RequestParameter.DetailPartsFrame.PAGE_ID ) );

                if ( true == StringUtils.IsNotBlank( processCd ) ) {
                    selectGroupCd = Defines.ListDefine.PartsSearchTarget.Atu;
                    selectClassCd = processCd;
                } else if ( true == StringUtils.IsNotBlank( pageID ) ) {
                    //ページIDからグループコードとクラスコードを設定
                    PageInfo.GetPageCdInfo( pageID, out selectGroupCd, out selectClassCd );
                }

                hdnSelectedGroupCd.Value = selectGroupCd;
                hdnSelectedClassCd.Value = selectClassCd;

                foreach ( Control ctrl in pnlContentList.Controls ) {
                    if ( ctrl.GetType() == typeof( Button ) ) {
                        string groupCd = ( (Button)ctrl ).Attributes[DETAIL_LIST_ATTR_GROUP];
                        string classCd = ( (Button)ctrl ).Attributes[DETAIL_LIST_ATTR_CLASS];

                        if ( true == StringUtils.IsNotBlank( groupCd ) && true == StringUtils.IsNotBlank( classCd )
                            && groupCd == selectGroupCd && classCd == selectClassCd ) {
                            SelectContentLabel( ctrl );
                            break;
                        }
                    }
                }

            } else {
                //詳細画面作成
                string selectGroupCd = hdnSelectedGroupCd.Value;
                string selectClassCd = hdnSelectedClassCd.Value;

                SetUserControl( selectGroupCd, selectClassCd, false );
            }
        }

        #endregion

        #region 一覧選択イベント
        /// <summary>
        /// 一覧項目ボタン押下
        /// </summary>
        /// <param name="parameters"></param>
        private void SelectContentLabel( params object[] parameters) {
           
            //詳細画面用セッションのクリア
            ClearDetailSession();

            foreach ( Control ctrl in pnlContentList.Controls ) {
                if ( ctrl.GetType() == typeof( Button ) ) {
                    ( (Button)ctrl ).CssClass = ( (Button)ctrl ).CssClass.Replace( DETAIL_LIST_SELECTED_CONTENT_CSS, "" );
                }
            }

            object sender = parameters[0];
            Button SelectedButton = ( (Button)sender );
            string selectGroupCd = SelectedButton.Attributes[DETAIL_LIST_ATTR_GROUP];
            string selectClassCd = SelectedButton.Attributes[DETAIL_LIST_ATTR_CLASS];

            SelectedButton.CssClass += " " + DETAIL_LIST_SELECTED_CONTENT_CSS;

            //詳細画面セット
            hdnSelectedGroupCd.Value = selectGroupCd;
            hdnSelectedClassCd.Value = selectClassCd;
            SetUserControl( selectGroupCd, selectClassCd );
            
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
            string serial = "";

            DataRow rowMainPartsView = new DataTable().NewRow();

            string coop =  StringUtils.Nvl( base.GetTransInfoValue(RequestParameter.DetailPartsFrame.EXTERNAL_COOP ));
            if ( true == StringUtils.IsNotBlank( coop ) ) {

                //MACs連携
                if ( coop == RequestParameter.ExternalCoop.MACs ) {
                    modelCd = StringUtils.Nvl( base.GetTransInfoValue( RequestParameter.DetailPartsFrame.MODEL_CD ) );
                    serial = StringUtils.Nvl( base.GetTransInfoValue( RequestParameter.DetailPartsFrame.SERIAL ) );

                    logger.Info( "詳細画面表示(MACs連携) 型式:{0} 機番:{1}", modelCd, serial );

                    //製品情報取得
                    DataTable tblParts = DetailPartsViewBusiness.SearchPartsDetail( modelCd, serial );
                    if ( null == tblParts || 1 != tblParts.Rows.Count ) {
                        //製品検索失敗
                        logger.Error( "製品情報取得失敗 型式={0} 機番={1}", modelCd, serial );
                        base.WriteApplicationMessage( MsgManager.MESSAGE_ERR_82010 );
                        return;
                    } else {
                        rowMainPartsView = tblParts.Rows[0];
                    }

                }
            } else {
                //一覧表示
                string callerToken = StringUtils.Nvl( base.GetTransInfoValue( RequestParameter.Common.TOKEN ) );
                string indexStr = StringUtils.Nvl( base.GetTransInfoValue( RequestParameter.DetailPartsFrame.INDEX ) );
                int index = NumericUtils.ToInt( indexStr, -1 );
                //一覧からの遷移
                if ( 0 <= index ) {
                    //一覧の検索情報を取得
                    ConditionInfoSessionHandler.ST_CONDITION condMainPartsView = SessionManager.GetConditionInfoHandler( callerToken ).GetCondition( PageInfo.MainPartsView.pageId );
                    if ( true == ObjectUtils.IsNotNull( condMainPartsView.ResultData )
                        && index < condMainPartsView.ResultData.Rows.Count ) {
                            rowMainPartsView = condMainPartsView.ResultData.Rows[index];
                        modelCd = rowMainPartsView[MainPartsView.GRID_ATU.MODEL_CD.bindField].ToString();
                        serial = rowMainPartsView[MainPartsView.GRID_ATU.SERIAL.bindField].ToString();

                        logger.Info( "詳細画面表示(一覧画面から遷移) 型式:{0} 機番:{1}", modelCd, serial );

                    }
                }
            }

            //製品別通過実績の設定
            SetImaDokoData( rowMainPartsView );

            //初期化、初期値設定
            InitializeValues( rowMainPartsView );

        }

        /// <summary>
        /// 初期値設定
        /// </summary>
        private void InitializeValues( DataRow rowModelProduct ) {
            Dictionary<string, object> dicControlValues = new Dictionary<string, object>();
            base.SetControlValues( ModelInfoControls, rowModelProduct, ref dicControlValues );

            //取得データをセッションに格納
            base.SetPageControlInfo( Defines.Session.DetailPartsFrame.SESSION_PAGE_INFO_MODEL_INFO_KEY, dicControlValues );
            
            //部品情報
            lblPartsInfo.Text = MODEL_INFO_TITLE_PARTS;

        }
        
        /// <summary>
        /// 一覧項目の作成
        /// </summary>
        private void SetContentsLabel() {

            pnlContentList.Controls.Clear();

            //工程部の作成
            Label lblProcessTitle = new Label();
            lblProcessTitle.Text = DETAIL_LIST_PROCESS_NM;
            lblProcessTitle.CssClass = DETAIL_LIST_TITLE_CSS;
            pnlContentList.Controls.Add( lblProcessTitle );
            //ATUのリストを取得
            ListItem[] liProcess = AtuProcessKind.GetList( false );
            if ( true == ObjectUtils.IsNotNull( liProcess ) && 0 < liProcess.Length ) {
                foreach ( ListItem li in liProcess ) {
                    Button btnProcessContentWk = new Button();
                    btnProcessContentWk.Text = li.Text;
                    btnProcessContentWk.Attributes[DETAIL_LIST_ATTR_GROUP] = Defines.ListDefine.PartsSearchTarget.Atu;
                    btnProcessContentWk.Attributes[DETAIL_LIST_ATTR_CLASS] = li.Value;
                    if ( CheckDataExists( li.Value ) ) {
                        btnProcessContentWk.CssClass = DETAIL_LIST_CONTENT_CSS;
                        btnProcessContentWk.Click += new System.EventHandler( ContentLabel_Click );
                        btnProcessContentWk.OnClientClick = LIST_CONTENT_CLICK;
                    } else {
                        btnProcessContentWk.CssClass = DETAIL_LIST_NONECONTENT_CSS;
                        btnProcessContentWk.Enabled = false;
                    }
                    pnlContentList.Controls.Add( btnProcessContentWk );
                }
            }
        }
        #endregion

        /// <summary>
        /// 詳細画面セット
        /// </summary>
        /// <param name="groupCd">グループコード</param>
        /// <param name="classCd">クラスコード</param>
        /// <param name="initialize">初期化実行フラグ</param>
        private void SetUserControl( string searchTarget, string processCd, bool initialize = true ) {

            lblDetailTitle.Text = "";
            string partsKind = null;
            pnlDetailControl.Controls.Clear();

            if ( true == StringUtils.IsBlank( searchTarget )
                || true == StringUtils.IsBlank( processCd ) ) {
                return;
            }

            PageInfo.ST_PAGE_INFO pageInfo = PageInfo.GetUCPageInfo( searchTarget, partsKind, processCd );
            if ( true == StringUtils.IsBlank( pageInfo.url ) ) {
                return;
            }

            System.Web.UI.UserControl uc = (System.Web.UI.UserControl)LoadControl( pageInfo.url );
            uc.ID = pageInfo.pageId;
            ( (Defines.Interface.IDetailParts)uc ).DetailPartsKeyParam
                = GetPartsModelInfo( searchTarget, partsKind, processCd );
            pnlDetailControl.Controls.Add( uc );

            if ( true == initialize ) {
                //アクセスカウンター登録
                Dao.Com.AccessCounterDao.Entry( pageInfo.pageId );

                logger.Info( "詳細画面表示切替:{0} {1} {2}", pageInfo.pageId, pageInfo.title, pageInfo.url );
                ( (Defines.Interface.IDetailParts)uc ).Initialize();
            }

            //詳細タイトル設定
            lblDetailTitle.Text = pageInfo.title;
        }

        /// <summary>
        /// 詳細外枠画面で管理する詳細画面(UserControl)用のセッションをクリア
        /// </summary>
        private void ClearDetailSession() {
            //(詳細画面)ページコントロール情報をクリア
            SessionManager.GetPageControlInfoHandler( base.Token ).SetPageControlInfo( Defines.Session.DetailPartsFrame.SESSION_PAGE_INFO_DETAIL_KEY, null );
            //(詳細画面)イメージ情報をクリア
            SessionManager.GetImageInfoHandler( base.Token ).SetImages( Defines.Session.DetailPartsFrame.SESSION_PAGE_INFO_DETAIL_KEY, null );
        }

        #endregion

        #region 一覧ボタン制御用データチェック処理
        /// <summary>
        /// データが存在するかを画面ごとにチェックする
        /// </summary>
        /// <param name="groupCd">グループコード</param>
        /// <param name="execKind">チェック対象区分</param>
        /// <returns></returns>
        private bool CheckDataExists( string execKind ) {

            bool blRet = false;
            DataTable resultSet = new DataTable();
            string modelCd = txtModelCd.Value;
            string serial = txtSerial.Value;

            Defines.Interface.ST_DETAIL_PARAM stInfo = new Defines.Interface.ST_DETAIL_PARAM();
            Dictionary<string, object> dicPageControlInfo = (Dictionary<string, object>)base.PageControlInfo[Defines.Session.DetailFrame.SESSION_PAGE_INFO_MODEL_INFO_KEY];
            stInfo.ProductModelCd = DataUtils.GetDictionaryStringVal( dicPageControlInfo, MODEL_INFO_ATU.MODEL_CD.bindField );

            //Defines.Interface.ST_DETAIL_PARAM DetailKeyParam = GetProductModelInfo( groupCd, execKind );


            try {
                switch ( execKind ) {
                case AtuProcessKind.ATU_PARTS_SERIAL:                  //ATU機番管理
                    resultSet = Business.DetailPartsViewBusiness.SelectAtuSerialDetail( stInfo.ProductModelCd, serial ).MainTable;
                    break;
                case AtuProcessKind.ATU_TORQUE_TIGHTENING_RECORD:      //トルク締付
                    resultSet = Business.DetailPartsViewBusiness.SelectAtuTorqueDetail( stInfo.ProductModelCd, serial ).MainTable;
                    break;
                case AtuProcessKind.ATU_LEAK_MEASURE_RESULT:           //リーク計測
                    resultSet = Business.DetailPartsViewBusiness.SelectAtuLeakDetail( stInfo.ProductModelCd, serial ).MainTable;
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
        #endregion

        #region 製品別通過実績
        /// <summary>
        /// 製品別通過実績取得
        /// </summary>
        /// <param name="assemblyPatternCd">組立パターン</param>
        private void SetImaDokoData( DataRow rowMainPartsView ) {
            
            DataTable tblView = new DataTable();        //画面表示用
           
            tblView.Columns.Add( "生産指示" );
            tblView.Columns.Add( "投入" );
            tblView.Columns.Add( "完成" );
            tblView.Columns.Add( "行先" );

            DataRow row = tblView.NewRow();
            row["生産指示"] = rowMainPartsView[MainPartsView.GRID_ATU.CREATE_DT.bindField].ToString();
            row["投入"] = rowMainPartsView[MainPartsView.GRID_ATU.THROW_DT.bindField].ToString();
            row["完成"] = rowMainPartsView[MainPartsView.GRID_ATU.COMPLETION_DT.bindField].ToString();
            row["行先"] = rowMainPartsView[MainPartsView.GRID_ATU.COMPLETION_DESTINATION.bindField].ToString();
           
            tblView.Rows.Add( row );

            //データセット
            ( (UI.MasterForm.MasterMain)this.Master ).SetImaDokoData( tblView );

        }
        #endregion

    }
}