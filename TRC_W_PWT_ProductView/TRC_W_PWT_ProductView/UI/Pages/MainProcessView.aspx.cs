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
using TRC_W_PWT_ProductView.Dao.Process;
using TRC_W_PWT_ProductView.Dao.Com;
using KTFramework.C1Common.Excel;
using TRC_W_PWT_ProductView.Defines.ProcessViewDefine;
using TRC_W_PWT_ProductView.UI.Pages.UserControl;

namespace TRC_W_PWT_ProductView.UI.Pages {

    /// <summary>
    /// メイン一覧ページ
    /// </summary>
    public partial class MainProcessView : BaseForm {

        //ロガー定義
        private static readonly Logger logger = new Logger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

        #region 定数定義
        /// <summary>
        /// グリッドビューグループ
        /// </summary>
        const string GRID_MAIN_PROCESS_VIEW_GROUP_CD = "MainProcessView";
        #endregion

        #region プロパティ
        /// <summary>
        /// 検索条件
        /// </summary>
        public ConditionInfoSessionHandler.ST_CONDITION SessionInfo => ConditionInfo;

        /// <summary>
        /// 選択行（左グリッド）
        /// </summary>
        public int SelectedDataItemIndex => int.Parse( hdnSelectedDataItemIndex.Value );

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
        /// 検索区分切替動作（製品検索）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ChangeProductSearch_Click( object sender, EventArgs e ) {
            base.RaiseEvent( MoveToProductSearch );
        }

        /// <summary>
        /// 検索区分切替動作（部品検索）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ChangePartsSearch_Click( object sender, EventArgs e ) {
            base.RaiseEvent( MoveToPartsSearch );
        }

        /// <summary>
        /// 製品種別リスト選択動作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rblProductKind_SelectedIndexChanged( object sender, EventArgs e ) {
            base.RaiseEvent( ChangeProductKind );
        }

        /// <summary>
        /// 工程リスト選択動作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlProcessKind_SelectedIndexChanged( object sender, EventArgs e ) {
            base.RaiseEvent( ChangeProcessKind );
        }

        /// <summary>
        /// エンジン種別変更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlEngineKind_SelectedIndexChanged( object sender, EventArgs e ) {
            base.RaiseEvent( ChangeEngineKind );
        }

        /// <summary>
        /// 検索ボタン選択動作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click( object sender, EventArgs e ) {
            base.RaiseEvent( DoSearch );
        }

        /// <summary>
        /// グリッドビュー行バインド
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grvMainProcessView_RowDataBound( object sender, GridViewRowEventArgs e ) {
            RowDataBoundMainProcessView( sender, e );
        }

        protected void grvMainProcessViewLB_RowDataBound( object sender, GridViewRowEventArgs e ) {
            RowDataBoundMainProcessViewLB( sender, e );
        }

        protected void grvMainProcessViewRB_RowDataBound( object sender, GridViewRowEventArgs e ) {
            RowDataBoundMainProcessViewRB( sender, e );
        }

        /// <summary>
        /// グリッドビューページチェンジイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grvMainProcessView_PageIndexChanging( object sender, CommandEventArgs e ) {
            base.RaiseEvent( PageIndexChangingMainProcessView, sender, e );
        }

        /// <summary>
        /// グリッドビュー並び替えイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grvMainProcessView_Sorting( object sender, GridViewSortEventArgs e ) {
            base.RaiseEvent( SortingMainProcessView, sender, e );
        }

        protected void btnExcel_Click( object sender, EventArgs e ) {
            base.RaiseEvent( OutputExcel );
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

            //動的処理
            ControlUtils.SetGridViewTemplateField( grvMainProcessViewLB, ControlUtils.GetFrozenColumns( grvMainProcessViewLB, ControlUtils.GetGridViewDefineArray( typeof( SearchResultDefine.GRID_COMMON ) ), true ) );
            ControlUtils.SetGridViewTemplateField( grvMainProcessViewRB, ControlUtils.GetFrozenColumns( grvMainProcessViewLB, ControlUtils.GetGridViewDefineArray( typeof( SearchResultDefine.GRID_COMMON ) ), false ) );

            ControlUtils.ReMakeBoundTemplateFieldCtrl( grvMainProcessViewLB, ControlUtils.GetFrozenColumns( grvMainProcessViewLB, ControlUtils.GetGridViewDefineArray( typeof( SearchResultDefine.GRID_COMMON ) ), true ) );
            ControlUtils.ReMakeBoundTemplateFieldCtrl( grvMainProcessViewRB, ControlUtils.GetFrozenColumns( grvMainProcessViewLB, ControlUtils.GetGridViewDefineArray( typeof( SearchResultDefine.GRID_COMMON ) ), false ) );

            ControlUtils.SetGridViewPager( ref pnlPager, grvMainProcessViewRB, grvMainProcessView_PageIndexChanging,
                ObjectUtils.IsNotNull( ConditionInfo.ResultData ) ? ConditionInfo.ResultData.Rows.Count : 0, grvMainProcessViewRB.PageIndex );

        }

        #endregion

        #endregion

        #region リスト選択処理


        /// <summary>
        /// 検索区分変更(製品検索)
        /// </summary>
        private void MoveToProductSearch() {
            PageUtils.RedirectToTRC( this.Page, CurrentPageInfo.pageId, PageInfo.MainView.url, this.Token, null );
        }

        /// <summary>
        /// 検索区分変更(部品検索)
        /// </summary>
        private void MoveToPartsSearch() {
            PageUtils.RedirectToTRC( this.Page, CurrentPageInfo.pageId, PageInfo.MainPartsView.url, this.Token, null );
        }

        #endregion

        #region 検索処理

        /// <summary>
        /// 検索処理
        /// </summary>
        private void DoSearch() {

            //選択行初期化
            hdnSelectedDataItemIndex.Value = "-1";

            //検索条件
            var dicCondition = new Dictionary<string, object>();
            //検索条件コントロールとID
            var dicIdWithText = new Dictionary<string, string>();
            //検索結果カラム一覧
            var resultDefine = new GridViewDefine[] { };

            //選択された工程毎に検索条件と検索結果の定義を作成
            CreateSearchDefine( dicCondition, ref dicIdWithText, ref resultDefine );

            //検索条件制約
            if ( !CheckSearchCondition( dicCondition ) ) {
                return;
            }

            //検索結果
            var result = new MainProcessViewBusiness.ResultSet();

            try {
                //検索実行
                result = MainProcessViewBusiness.Search( dicCondition );
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
            }

            //セッション保持
            ConditionInfo = new ConditionInfoSessionHandler.ST_CONDITION() {
                conditionValue = dicCondition,
                IdWithText = dicIdWithText,
                ResultDefine = resultDefine,
                ResultData = result.ResultTable,
            };

            if ( result.ResultTable != null && result.ResultTable.Rows.Count > 0 ) {
                //件数表示
                ntbResultCount.Value = result.ResultTable.Rows.Count;

                //ページャー設定
                ControlUtils.SetGridViewPager( ref pnlPager, grvMainProcessViewRB, grvMainProcessView_PageIndexChanging, result.ResultTable.Rows.Count, 0 );

                //検索結果設定
                CreateResultGridView( result.ResultTable, resultDefine );

                //Excel出力ボタン表示
                this.btnExcel.Visible = true;

            } else {
                //Excel出力ボタン表示
                this.btnExcel.Visible = false;

                //グリッドクリア
                ClearGridView();
            }

            //工程別操作ボタン表示
            SetSubOperation();

            //メッセージがあれば表示
            if ( result.Message != null ) {
                this.WriteApplicationMessage( result.Message );
            }



        }

        #endregion

        #region グリッドビューイベント

        /// <summary>
        /// グリッドビュー行バインド
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RowDataBoundMainProcessView( params object[] parameters ) {
            object sender = parameters[0];
            GridViewRowEventArgs e = (GridViewRowEventArgs)parameters[1];

            if ( e.Row.RowType == DataControlRowType.DataRow ) {
                ControlUtils.GridViewRowBound( (GridView)sender, e, ControlUtils.GridRowDoubleClickEvent.WindowOpen, PageInfo.ResolveClientUrl( this, PageInfo.DetailFrame ), base.Token, PageInfo.MainProcessView.pageId );
            }
        }

        /// <summary>
        /// グリッドビューページ切替
        /// </summary>
        /// <param name="parameters"></param>
        private void PageIndexChangingMainProcessView( params object[] parameters ) {
            object sender = parameters[0];

            CommandEventArgs e = (CommandEventArgs)parameters[1];
            int newPageIndex = Convert.ToInt32( e.CommandArgument );

            int allPages = ConditionInfo.ResultData.Rows.Count / grvMainProcessViewRB.PageSize;

            if ( ObjectUtils.IsNotNull( ConditionInfo.ResultData ) && ConditionInfo.ResultData.Rows.Count % grvMainProcessViewRB.PageSize != 0 ) {
                allPages++;
            }
            //ページが無くなっている場合には、先頭ページへ戻す
            if ( newPageIndex >= allPages ) {
                newPageIndex = 0;
            }

            //背面ユーザ切替対応
            RebindGridView();
            ControlUtils.GridViewPageIndexChanging( grvMainProcessViewLB, ConditionInfo.ResultData, new GridViewPageEventArgs( newPageIndex ) );
            ControlUtils.GridViewPageIndexChanging( grvMainProcessViewRB, ConditionInfo.ResultData, new GridViewPageEventArgs( newPageIndex ) );

            ControlUtils.SetGridViewPager( ref pnlPager, grvMainProcessViewRB, grvMainProcessView_PageIndexChanging, ConditionInfo.ResultData.Rows.Count, grvMainProcessViewRB.PageIndex );

            //グリッドビュー外のDivサイズ変更
            SetDivGridViewWidth();
        }

        /// <summary>
        /// グリッドビュー並び替え
        /// </summary>
        /// <param name="parameters"></param>
        private void SortingMainProcessView( params object[] parameters ) {
            object sender = parameters[0];
            GridViewSortEventArgs e = (GridViewSortEventArgs)parameters[1];

            ConditionInfoSessionHandler.ST_CONDITION cond = ConditionInfo;
            ControlUtils.GridViewSorting( grvMainProcessViewLB, ref cond, e, true );
            ControlUtils.GridViewSorting( grvMainProcessViewRB, ref cond, e );

            //背面ユーザ切替対応
            RebindGridView();

            ControlUtils.SetGridViewPager( ref pnlPager, grvMainProcessViewRB, grvMainProcessView_PageIndexChanging, cond.ResultData.Rows.Count, grvMainProcessViewRB.PageIndex );

            ConditionInfo = cond;

            //グリッドビュー外のDivサイズ変更
            SetDivGridViewWidth();
        }
        #endregion

        #region Excel出力

        /// <summary>
        /// Excel出力処理
        /// </summary>
        private void OutputExcel() {
            //出力データ無し
            if ( ConditionInfo.ResultData == null || ConditionInfo.ResultData.Rows.Count == 0 ) {
                return;
            }

            //検索条件定義
            var searchConditionDefine = GetControlDefineArray( ConditionInfo.conditionValue[SearchConditionDefine.CONDITION_COMMON.PRODUCT_KIND.bindField].ToString(),
                                                               ConditionInfo.conditionValue[SearchConditionDefine.CONDITION_COMMON.PROCESS_KIND.bindField].ToString(),
                                                               ConditionInfo.conditionValue[SearchConditionDefine.CONDITION_COMMON.ENGINE_KIND.bindField].ToString() );

            //検索条件Dictionary作成
            var condition = ConditionInfo.IdWithText.Where( x => searchConditionDefine.Any( y => x.Key.Equals( y.controlId ) ) )
                                                    .ToDictionary( x => searchConditionDefine.First( y => x.Key.Equals( y.controlId ) ).displayNm,
                                                                   x => x.Value );
            try {
                //ダウンロード
                Excel.Download( Response, $"工程詳細情報_{ DateTime.Now.ToString( "yyyyMMddHHmmss" ) }.xls", condition,
                    MainProcessViewBusiness.Search( ConditionInfo.conditionValue, true ).ResultTable );
            } catch ( System.Threading.ThreadAbortException ) {
                //response.Endで必ず発生する為、正常として扱う
            } catch ( Exception ex ) {
                logger.Exception( ex );
                base.WriteApplicationMessage( MsgManager.MESSAGE_ERR_80030, "工程情報_検索結果" );
            }
        }

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

            //初期化、初期値設定
            InitializeValues();
        }

        /// <summary>
        /// 初期化、初期値設定
        /// </summary>
        private void InitializeValues() {

            //製品種別セット
            SetProductKind();
            this.rblProductKind.SelectedValue = ProductKind.Engine;

            //工程一覧セット
            SetProcessKind();
            this.ddlProcessKind.SelectedValue = ProcessKind.PROCESS_CD_ENGINE_INJECTION;

            //エンジン種別セット
            SetEngineKind();
            this.ddlEngineKind.SelectedValue = EngineKind.ENGINE_03;

            //共通条件の活性・非活性設定
            SetCommonConditionAvailability();

            //工程別検索条件
            SetSubCondition();

            //セッション情報初期化
            ConditionInfo = new ConditionInfoSessionHandler.ST_CONDITION();

            //グリッドビュー初期化
            ClearGridView();

            //Excel出力ボタン非活性
            this.btnExcel.Visible = false;

            //未実装部分を非活性 or 削除
            DisableUnimplementedParts();
        }

        /// <summary>
        /// 製品種別セット
        /// </summary>
        private void SetProductKind() {
            //製品種別リスト
            ControlUtils.SetListControlItems( this.rblProductKind, Dao.Com.MasterList.ProductKindList );
        }

        /// <summary>
        /// 工程一覧セット
        /// </summary>
        private void SetProcessKind() {
            ControlUtils.SetListControlItems( this.ddlProcessKind,
                Dao.Com.MasterList.GetClassItem( rblProductKind.SelectedValue, Defines.ListDefine.GroupCd.Process, false ) );
        }

        /// <summary>
        /// エンジン種別セット
        /// </summary>
        private void SetEngineKind() {
            if ( this.rblProductKind.SelectedValue.Equals( ProductKind.Engine ) ) {
                switch ( this.ddlProcessKind.SelectedValue ) {
                case ProcessKind.PROCESS_CD_ENGINE_INJECTION:
                case ProcessKind.PROCESS_CD_ENGINE_TEST:
                    ControlUtils.SetListControlItems( this.ddlEngineKind, Defines.ListDefine.EngineKind.GetList() );
                    this.ddlEngineKind.SelectedValue = EngineKind.ENGINE_03;
                    this.ddlEngineKind.Enabled = true;
                    break;
                default:
                    this.ddlEngineKind.Items.Clear();
                    this.ddlEngineKind.Enabled = false;
                    break;
                }
            } else {
                this.ddlEngineKind.Items.Clear();
                this.ddlEngineKind.Enabled = false;
            }
        }

        /// <summary>
        /// 工程別検索条件セット
        /// </summary>
        private void SetSubCondition() {
            //各工程別検索条件を不可視状態に変更
            this.ucSubCondition.Controls.Cast<Control>().ToList().ForEach( c => c.Visible = false );

            //エンジン or トラクタの工程別検索条件をセット
            if ( this.rblProductKind.SelectedValue.Equals( ProductKind.Engine ) ) {
                SetEngineSubCondition();
            } else if ( this.rblProductKind.SelectedValue.Equals( ProductKind.Tractor ) ) {
                SetTractorSubCondition();
            }
        }

        /// <summary>
        /// 工程別操作セット
        /// </summary>
        private void SetSubOperation() {
            //各工程別操作ボタンを不可視状態に変更
            this.ucSubOperation.Controls.Cast<Control>().ToList().ForEach( c => c.Visible = false );

            //エンジン or トラクタの工程別操作をセット
            if ( this.rblProductKind.SelectedValue.Equals( ProductKind.Engine ) ) {
                SetEngineSubOperation();
            } else if ( this.rblProductKind.SelectedValue.Equals( ProductKind.Tractor ) ) {
                SetTractorSubOperation();
            }
        }

        /// <summary>
        /// 共通条件の活性・非活性設定
        /// </summary>
        private void SetCommonConditionAvailability() {
            if ( this.rblProductKind.SelectedValue.Equals( ProductKind.Engine ) ) {
                this.txtPinCd.Value = string.Empty;
                this.txtPinCd.Enabled = false;
            } else if ( this.rblProductKind.SelectedValue.Equals( ProductKind.Tractor ) ) {
                this.txtPinCd.Enabled = true;
            }
        }

        /// <summary>
        /// エンジン工程検索条件セット
        /// </summary>
        private void SetEngineSubCondition() {

            string subConditionDivName = string.Empty;

            switch ( this.ddlProcessKind.SelectedValue ) {
            case ProcessKind.PROCESS_CD_ENGINE_INJECTION:
                if ( this.ddlEngineKind.SelectedValue.Equals( EngineKind.ENGINE_03 ) ) {
                    subConditionDivName = "divEngineInjection03Condition";

                } else if ( this.ddlEngineKind.SelectedValue.Equals( EngineKind.ENGINE_07 ) ) {
                    subConditionDivName = "divEngineInjection07Condition";
                }
                break;
            case ProcessKind.PROCESS_CD_ENGINE_FRICTION:
                subConditionDivName = "divEngineFrictionCondition";
                break;
            case ProcessKind.PROCESS_CD_ENGINE_TEST:
                if ( this.ddlEngineKind.SelectedValue.Equals( EngineKind.ENGINE_03 ) ) {
                    subConditionDivName = "divEngineTest03Condition";
                } else if ( this.ddlEngineKind.SelectedValue.Equals( EngineKind.ENGINE_07 ) ) {
                    subConditionDivName = "divEngineTest07Condition";
                }
                break;
            }

            if ( string.IsNullOrEmpty( subConditionDivName ) ) {
                return;
            }

            var subConditionDiv = this.ucSubCondition.FindControl( subConditionDivName );

#if DEBUG
            if( subConditionDiv == null ) {
                throw new Exception( $"工程別検索条件の定義が見つかりません。ID:{ subConditionDivName }の検索条件div要素をMainProcessConditionPartialView.ascxに追加してください。" );
                return;
            }
#endif
            subConditionDiv.Visible = true;

        }

        /// <summary>
        /// トラクタ工程検索条件セット
        /// </summary>
        private void SetTractorSubCondition() {
            throw new NotImplementedException();
        }

        /// <summary>
        /// エンジン工程操作セット
        /// </summary>
        private void SetEngineSubOperation() {

            string subOperationSpanName = string.Empty;

            switch ( this.ddlProcessKind.SelectedValue ) {
            case ProcessKind.PROCESS_CD_ENGINE_INJECTION:
                if ( this.ddlEngineKind.SelectedValue.Equals( EngineKind.ENGINE_03 ) ) {
                    subOperationSpanName = ConditionInfo.ResultData.Rows.Count != 0 ? "spnEngineInjection03Operation" : string.Empty;
                } else if ( this.ddlEngineKind.SelectedValue.Equals( EngineKind.ENGINE_07 ) ) {
                    subOperationSpanName = ConditionInfo.ResultData.Rows.Count != 0 ? "spnEngineInjection07Operation" : string.Empty;
                }
                break;
            case ProcessKind.PROCESS_CD_ENGINE_FRICTION:
            case ProcessKind.PROCESS_CD_ENGINE_TEST:
                break;
            }

            //工程別操作可視設定
            if ( string.IsNullOrEmpty( subOperationSpanName ) ) {
                return;
            } else {
                var subOperationSpan = this.ucSubOperation.FindControl( subOperationSpanName );
#if DEBUG
                if( subOperationSpan == null ) {
                    throw new InvalidOperationException( $"工程別操作の定義が見つかりません。ID:{ subOperationSpanName }の工程別操作span要素をMainProcessOperationPartialView.ascxに追加してください。" );
                }
#endif
                subOperationSpan.Visible = true;
            }
        }

        /// <summary>
        /// トラクタ工程検索条件セット
        /// </summary>
        private void SetTractorSubOperation() {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 未実装の箇所を非活性 or 削除
        /// TODO:全ての工程が実装されたらこのメソッドを削除
        /// </summary>
        private void DisableUnimplementedParts() {
            //製品種別ロータリー削除、トラクタ非活性
            this.rblProductKind.Items.FindByValue( ProductKind.Tractor ).Enabled = false;
            this.rblProductKind.Items.Remove( this.rblProductKind.Items.FindByValue( ProductKind.Rotary ) );

            //未実装エンジン工程削除（追加したら消すこと）
            this.ddlProcessKind.Items.Remove( ddlProcessKind.Items.FindByValue( ProcessKind.PROCESS_CD_ENGINE_TORQUE ) );
            this.ddlProcessKind.Items.Remove( ddlProcessKind.Items.FindByValue( ProcessKind.PROCESS_CD_ENGINE_CAMIMAGE ) );
            this.ddlProcessKind.Items.Remove( ddlProcessKind.Items.FindByValue( ProcessKind.PROCESS_CD_ENGINE_CC_INSPECT ) );
            this.ddlProcessKind.Items.Remove( ddlProcessKind.Items.FindByValue( ProcessKind.PROCESS_CD_ENGINE_CS_INSPECT ) );
            this.ddlProcessKind.Items.Remove( ddlProcessKind.Items.FindByValue( ProcessKind.PROCESS_CD_ENGINE_CYH_INSPECT ) );
            this.ddlProcessKind.Items.Remove( ddlProcessKind.Items.FindByValue( ProcessKind.PROCESS_CD_ENGINE_HARNESS ) );
            this.ddlProcessKind.Items.Remove( ddlProcessKind.Items.FindByValue( ProcessKind.PROCESS_CD_ENGINE_ELCHECK ) );
            this.ddlProcessKind.Items.Remove( ddlProcessKind.Items.FindByValue( ProcessKind.PROCESS_CD_ENGINE_SHIPMENTPARTS ) );
            this.ddlProcessKind.Items.Remove( ddlProcessKind.Items.FindByValue( ProcessKind.PROCESS_CD_ENGINE_AIIMAGE ) );
        }

        /// <summary>
        /// 選択された工程を基に検索条件と検索結果定義を作成する
        /// </summary>
        /// <param name="dicCondition"></param>
        /// <param name="dicIdWithText"></param>
        /// <param name="gridViewResult"></param>
        private void CreateSearchDefine( Dictionary<string, object> dicCondition, ref Dictionary<string, string> dicIdWithText, ref GridViewDefine[] resultDefine ) {

            //検索パラメータ（共通）取得
            var dicCommonParameters = new Dictionary<string, object>();
            base.GetControlValues( ControlUtils.GetControlDefineArray( typeof( SearchConditionDefine.CONDITION_COMMON ) ), ref dicCommonParameters );
            string targetProductKind = dicCommonParameters[SearchConditionDefine.CONDITION_COMMON.PRODUCT_KIND.bindField].ToString();
            string targetProcessKind = dicCommonParameters[SearchConditionDefine.CONDITION_COMMON.PROCESS_KIND.bindField].ToString();
            string targetEngineKind = dicCommonParameters[SearchConditionDefine.CONDITION_COMMON.ENGINE_KIND.bindField].ToString();

            //検索条件の作成
            base.GetControlValues( GetControlDefineArray( targetProductKind, targetProcessKind, targetEngineKind ), ref dicCondition );
            base.GetControlValues( this.ucSubCondition, GetControlDefineArray( targetProductKind, targetProcessKind, targetEngineKind ), ref dicCondition );

            //工程別検索条件コントロールテキスト定義の作成
            var conditionTexts = new Dictionary<string, string>();
            base.GetControlTexts( GetControlDefineArray( targetProductKind, targetProcessKind, targetEngineKind ), out conditionTexts );
            var subConditionTexts = new Dictionary<string, string>();
            base.GetControlTexts( this.ucSubCondition, GetControlDefineArray( targetProductKind, targetProcessKind, targetEngineKind ), out subConditionTexts );
            subConditionTexts.Where( x => !conditionTexts.ContainsKey( x.Key ) ).ToList().ForEach( x => conditionTexts[x.Key] = x.Value );
            dicIdWithText = conditionTexts;

            //工程別検索結果定義の作成
            resultDefine = GetGridViewDefineArray( targetProductKind, targetProcessKind, targetEngineKind );
        }

        /// <summary>
        /// 検索条件コントロール定義取得
        /// </summary>
        /// <param name="productKind"></param>
        /// <param name="processKind"></param>
        /// <param name="engineKind"></param>
        /// <returns></returns>
        private ControlDefine[] GetControlDefineArray( string productKind, string processKind, string engineKind = null ) {
            //工程別検索条件定義クラス
            Type searchConditionType = typeof( object );

            //選択された製品種別、工程種別、エンジン種別を基に検索条件、検索結果の定義を取得
            if ( productKind.Equals( ProductKind.Engine ) ) {
                //エンジン
                switch ( processKind ) {
                //噴射時期計測
                case ProcessKind.PROCESS_CD_ENGINE_INJECTION:
                    if ( engineKind.Equals( EngineKind.ENGINE_03 ) ) {
                        searchConditionType = typeof( SearchConditionDefine.CONDITION_ENGINE_INJECTION_03 );
                    } else if ( engineKind.Equals( EngineKind.ENGINE_07 ) ) {
                        searchConditionType = typeof( SearchConditionDefine.CONDITION_ENGINE_INJECTION_07 );
                    }
                    break;
                //フリクションロス
                case ProcessKind.PROCESS_CD_ENGINE_FRICTION:
                    searchConditionType = typeof( SearchConditionDefine.CONDITION_ENGINE_FRICTION );
                    break;
                //エンジン運転測定
                case ProcessKind.PROCESS_CD_ENGINE_TEST:
                    if ( engineKind.Equals( EngineKind.ENGINE_03 ) ) {
                        searchConditionType = typeof( SearchConditionDefine.CONDITION_ENGINE_TEST_03 );
                    } else if ( engineKind.Equals( EngineKind.ENGINE_07 ) ) {
                        searchConditionType = typeof( SearchConditionDefine.CONDITION_ENGINE_TEST_07 );
                    }
                    break;
                default:
                    throw new NotImplementedException();
                }
            } else if ( productKind.Equals( ProductKind.Tractor ) ) {
                //トラクタ
                throw new NotImplementedException();
            } else {
                throw new ArgumentException( "不正な製品種別です。", nameof( productKind ) );
            }

            //結果
            var result = new List<ControlDefine>();
            result.AddRange( ControlUtils.GetControlDefineArray( typeof( SearchConditionDefine.CONDITION_COMMON ) ) );
            result.AddRange( ControlUtils.GetControlDefineArray( searchConditionType ) );
            return result.ToArray();
        }

        /// <summary>
        /// 検索結果グリッド定義取得
        /// </summary>
        /// <param name="productKind"></param>
        /// <param name="processKind"></param>
        /// <param name="engineKind"></param>
        /// <returns></returns>
        private GridViewDefine[] GetGridViewDefineArray( string productKind, string processKind, string engineKind = null ) {
            //工程別検索結果定義クラス
            Type searchResultType = typeof( object );

            //選択された製品種別、工程種別、エンジン種別を基に検索条件、検索結果の定義を取得
            if ( productKind.Equals( ProductKind.Engine ) ) {
                //エンジン
                switch ( processKind ) {
                //噴射時期計測
                case ProcessKind.PROCESS_CD_ENGINE_INJECTION:
                    if ( engineKind.Equals( EngineKind.ENGINE_03 ) ) {
                        searchResultType = typeof( SearchResultDefine.GRID_ENGINE_INJECTION_03 );
                    } else if ( engineKind.Equals( EngineKind.ENGINE_07 ) ) {
                        searchResultType = typeof( SearchResultDefine.GRID_ENGINE_INJECTION_07 );
                    }
                    break;
                //フリクションロス
                case ProcessKind.PROCESS_CD_ENGINE_FRICTION:
                    searchResultType = typeof( SearchResultDefine.GRID_ENGINE_FRICTION );
                    break;
                //エンジン運転測定
                case ProcessKind.PROCESS_CD_ENGINE_TEST:
                    if ( engineKind.Equals( EngineKind.ENGINE_03 ) ) {
                        searchResultType = typeof( SearchResultDefine.GRID_ENGINE_TEST_03 );
                    } else if ( engineKind.Equals( EngineKind.ENGINE_07 ) ) {
                        searchResultType = typeof( SearchResultDefine.GRID_ENGINE_TEST_07 );
                    }
                    break;
                default:
                    throw new NotImplementedException();
                }
            } else if ( productKind.Equals( ProductKind.Tractor ) ) {
                //トラクタ
                throw new NotImplementedException();
            } else {
                throw new ArgumentException( "不正な製品種別です。", nameof( productKind ) );
            }

            //結果
            var result = new List<GridViewDefine>();
            result.AddRange( ControlUtils.GetGridViewDefineArray( typeof( SearchResultDefine.GRID_COMMON ) ) );
            result.AddRange( ControlUtils.GetGridViewDefineArray( searchResultType ) );
            return result.ToArray();
        }

        /// <summary>
        /// 検索条件チェック
        /// </summary>
        /// <param name="dicCondition"></param>
        /// <returns></returns>
        private bool CheckSearchCondition( Dictionary<string, object> dicCondition ) {
            return CheckCommonSearchCondition( dicCondition ) && CheckSubSearchCondition( dicCondition );
        }

        /// <summary>
        /// 検索条件チェック（共通）
        /// </summary>
        /// <param name="dicCondition"></param>
        /// <returns></returns>
        private bool CheckCommonSearchCondition( Dictionary<string, object> dicCondition ) {
            //特になし
            return true;
        }

        /// <summary>
        /// 検索条件チェック（工程別）
        /// </summary>
        /// <param name="dicCondition"></param>
        /// <returns></returns>
        private bool CheckSubSearchCondition( Dictionary<string, object> dicCondition ) {
            //特になし
            return true;
        }

        /// <summary>
        /// 検索結果GridViewを作成
        /// </summary>
        /// <param name="resultTable"></param>
        /// <param name="gridViewResults"></param>
        private void CreateResultGridView( DataTable resultTable, GridViewDefine[] resultDefine ) {
            //結果テーブルに型式コード表示文字列追加
            if ( !resultTable.Columns.Contains( SearchResultDefine.GRID_COMMON.MODEL_CD_DISP_NM.bindField ) ) {
                resultTable.Columns.Add( SearchResultDefine.GRID_COMMON.MODEL_CD_DISP_NM.bindField );
                resultTable.AsEnumerable()
                           .ToList()
                           .ForEach( x => x[SearchResultDefine.GRID_COMMON.MODEL_CD_DISP_NM.bindField] = DataUtils.GetModelCdStr( x[SearchResultDefine.GRID_COMMON.MODEL_CD.bindField].ToString() ) );
            }

            //結果テーブルに国コード表示文字列追加
            if ( !resultTable.Columns.Contains( SearchResultDefine.GRID_COMMON.COUNTRY_CD_DISP_NM.bindField ) ) {
                resultTable.Columns.Add( SearchResultDefine.GRID_COMMON.COUNTRY_CD_DISP_NM.bindField );
                resultTable.AsEnumerable()
                           .ToList()
                           .ForEach( x => x[SearchResultDefine.GRID_COMMON.COUNTRY_CD_DISP_NM.bindField] = x[SearchResultDefine.GRID_COMMON.COUNTRY_CD.bindField].ToString().TrimEnd() );
            }

            //結果テーブルにPINコード追加
            if ( !resultTable.Columns.Contains( SearchResultDefine.GRID_COMMON.PIN_CD.bindField ) ) {
                resultTable.Columns.Add( SearchResultDefine.GRID_COMMON.PIN_CD.bindField );
            }

            //TemplateFieldの追加
            grvHeaderRT.Columns.Clear();
            grvMainProcessViewRB.Columns.Clear();
            Enumerable.Range( grvMainProcessViewLB.Columns.Count, resultDefine.Length - grvMainProcessViewLB.Columns.Count )
                      .ToList()
                      .ForEach( x => grvMainProcessViewRB.Columns.Add( new TemplateField { HeaderText = resultDefine[x].bindField } ) );

            ControlUtils.ShowGridViewHeader( grvHeaderLT, ControlUtils.GetFrozenColumns( grvMainProcessViewLB, resultDefine, true ), ConditionInfo, true );
            ControlUtils.ShowGridViewHeader( grvHeaderRT, ControlUtils.GetFrozenColumns( grvMainProcessViewLB, resultDefine, false ), ConditionInfo, true );
            ControlUtils.BindGridView_WithTempField( grvMainProcessViewLB, ControlUtils.GetFrozenColumns( grvMainProcessViewLB, resultDefine, true ), resultTable );
            ControlUtils.BindGridView_WithTempField( grvMainProcessViewRB, ControlUtils.GetFrozenColumns( grvMainProcessViewLB, resultDefine, false ), resultTable );

            //GridView表示
            divGrvDisplay.Visible = true;

            //グリッドビュー外のDivサイズ変更
            SetDivGridViewWidth();
        }

        #endregion

        #region グリッドビュー

        /// <summary>
        /// グリッドビュークリア
        /// </summary>
        private void ClearGridView() {

            //列名非表示 グリッドビュークリア
            ControlUtils.InitializeGridView( grvHeaderLT, false );
            ControlUtils.InitializeGridView( grvHeaderRT, false );
            ControlUtils.InitializeGridView( grvMainProcessViewLB, false );
            ControlUtils.InitializeGridView( grvMainProcessViewRB, false );

            //グリッドビュー外のDivサイズ変更
            SetDivGridViewWidth();

            //件数表示
            ntbResultCount.Value = 0;

            //ページャークリア
            ControlUtils.ClearPager( ref pnlPager );

            //GridView非表示
            divGrvDisplay.Visible = false;

        }

        /// <summary>
        /// 製品種別変更
        /// </summary>
        private void ChangeProductKind() {
            //製品、工程、エンジン以外の共通条件の活性・非活性を設定
            SetCommonConditionAvailability();
            //工程種別を設定
            SetProcessKind();
            //エンジン種別を設定
            SetEngineKind();
            //工程別検索条件を設定
            SetSubCondition();
        }

        /// <summary>
        /// 工程種別変更
        /// </summary>
        private void ChangeProcessKind() {
            //共通条件の活性・非活性を設定
            SetCommonConditionAvailability();
            //エンジン種別を設定
            SetEngineKind();
            //工程別検索条件を設定
            SetSubCondition();
        }

        /// <summary>
        /// エンジン種別変更
        /// </summary>
        private void ChangeEngineKind() {
            //共通条件の活性・非活性を設定
            SetCommonConditionAvailability();
            //工程別検索条件を設定
            SetSubCondition();
        }

        /// <summary>
        /// グリッドビュー外のDivサイズ変更
        /// </summary>
        private void SetDivGridViewWidth() {

            SetDivGridViewWidth( grvHeaderLT, divGrvHeaderLT );
            SetDivGridViewWidth( grvHeaderRT, divGrvHeaderRT );

            SetDivGridViewWidth( grvMainProcessViewLB, divGrvLB );
            SetDivGridViewWidth( grvMainProcessViewRB, divGrvRB );
        }

        /// <summary>
        /// グリッドビュー外のDivサイズ変更
        /// </summary>
        private void SetDivGridViewWidth( GridView grv, System.Web.UI.HtmlControls.HtmlGenericControl div ) {
            //セル幅補正 Padding4px + Border(片側) 1px
            const int CELL_PADDING = 4 * 2 + 1;
            //テーブル幅補正 Border(片側) 1px
            const int OUT_BORDER = 1;

            var visibleColumns = grv.Columns.Cast<DataControlField>().Where( x => x.Visible ).ToList();
            int sumWidth = NumericUtils.ToInt( visibleColumns.Sum( x => x.HeaderStyle.Width.Value ) )
                                + CELL_PADDING * visibleColumns.Count()
                                + ( visibleColumns.Any() ? OUT_BORDER : 0 );

            div.Style["width"] = $"{ sumWidth }px";
        }

        #endregion


        #region グリッドビューイベント処理

        /// <summary>
        /// グリッドビュー行バインド(左下)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RowDataBoundMainProcessViewLB( params object[] parameters ) {
            object sender = parameters[0];
            GridViewRowEventArgs e = (GridViewRowEventArgs)parameters[1];

            if ( e.Row.RowType == DataControlRowType.DataRow ) {
                //マウスカーソル設定
                e.Row.Attributes["onmouseover"] = "this.style.cursor='pointer'";

                var _ = new Dictionary<string, object>();
                base.SetControlValues( e.Row, ControlUtils.GetControlDefineArray( typeof( SearchResultDefine.GRID_COMMON ) ), ( (DataRowView)e.Row.DataItem ).Row, ref _ );

                ControlUtils.GridViewRowBound( (GridView)sender, e, GRID_MAIN_PROCESS_VIEW_GROUP_CD, $"document.getElementById(\"{ hdnSelectedDataItemIndex.ClientID }\").value=\"{ e.Row.DataItemIndex }\";",
                    ControlUtils.GridRowDoubleClickEvent.WindowOpen, PageInfo.ResolveClientUrl( this, PageInfo.DetailFrame ), base.Token, PageInfo.MainProcessView.pageId );
            }
        }

        /// <summary>
        /// グリッドビュー行バインド(右下)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RowDataBoundMainProcessViewRB( params object[] parameters ) {
            object sender = parameters[0];
            GridViewRowEventArgs e = (GridViewRowEventArgs)parameters[1];

            if ( e.Row.RowType == DataControlRowType.DataRow ) {
                //マウスカーソル設定
                e.Row.Attributes["onmouseover"] = "this.style.cursor='pointer'";

                ControlUtils.GridViewRowBound( (GridView)sender, e, GRID_MAIN_PROCESS_VIEW_GROUP_CD, $"document.getElementById(\"{ hdnSelectedDataItemIndex.ClientID }\").value=\"{ e.Row.RowIndex }\";",
                    ControlUtils.GridRowDoubleClickEvent.WindowOpen, PageInfo.ResolveClientUrl( this, PageInfo.DetailFrame ), base.Token, PageInfo.MainProcessView.pageId );
            }
        }

        /// <summary>
        /// グリッドビュー再バインド
        /// </summary>
        /// <param name="recoverPageIndex">ページインデックスを保持有無</param>
        public void RebindGridView( bool recoverPageIndex = false ) {

            int tempPageIndex = grvMainProcessViewLB.PageIndex;

            if ( recoverPageIndex ) {
                grvMainProcessViewLB.PageIndex = 1;
                grvMainProcessViewRB.PageIndex = 1;
            }

            ControlUtils.ShowGridViewHeader( grvHeaderLT, ControlUtils.GetFrozenColumns( grvMainProcessViewLB, ConditionInfo.ResultDefine, true ), ConditionInfo, true );
            ControlUtils.ShowGridViewHeader( grvHeaderRT, ControlUtils.GetFrozenColumns( grvMainProcessViewLB, ConditionInfo.ResultDefine, false ), ConditionInfo, true );

            ControlUtils.BindGridView_WithTempField( grvMainProcessViewLB, ControlUtils.GetFrozenColumns( grvMainProcessViewLB, ConditionInfo.ResultDefine, true ), ConditionInfo.ResultData );
            ControlUtils.BindGridView_WithTempField( grvMainProcessViewRB, ControlUtils.GetFrozenColumns( grvMainProcessViewLB, ConditionInfo.ResultDefine, false ), ConditionInfo.ResultData );

            if ( recoverPageIndex ) {
                ControlUtils.GridViewPageIndexChanging( grvMainProcessViewLB, ConditionInfo.ResultData, new GridViewPageEventArgs( tempPageIndex ) );
                ControlUtils.GridViewPageIndexChanging( grvMainProcessViewRB, ConditionInfo.ResultData, new GridViewPageEventArgs( tempPageIndex ) );
            }

            //選択行初期化
            hdnSelectedDataItemIndex.Value = "-1";
        }
        #endregion

        #endregion

    }
}