using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KTFramework.Dao;
using KTFramework.Common;
using TRC_W_PWT_ProductView.Common;
using TRC_W_PWT_ProductView.Dao.Process;
using TRC_W_PWT_ProductView.Defines;
using TRC_W_PWT_ProductView.Defines.TypeDefine;
using TRC_W_PWT_ProductView.UI.Base;
using TRC_W_PWT_ProductView.Business;
using TRC_W_PWT_ProductView.Session;
using TRC_W_PWT_ProductView.Dao.Com;
using System.Data;
using KTWebControl.CustomControls;

namespace TRC_W_PWT_ProductView.UI.Pages.AiImageCheck {
    /// <summary>
    /// 画像解析構成追加画面
    /// </summary>
    public partial class AnlGroupListInput : BaseForm {

        #region 定数
        const string CLOSE_MODAL_DISP = "AnlGroupListInput.CloseModal();";
        /// <summary>
        /// 空き(表示用)
        /// </summary>
        const string FLAG_DISP = "";

        private const int CONST_MAX_RECORD = 500;
        /// <summary>
        /// 一覧検索条件定義
        /// </summary>
        public class CONDITION {
            /// <summary>画像解析項目コード</summary>
            public static readonly ControlDefine ITEM_CD = new ControlDefine( "txtItemCd", "画像解析項目コード", "txtItemCd", ControlDefine.BindType.Both, typeof( string ) );
            /// <summary>画像解析項目名</summary>
            public static readonly ControlDefine ITEM_NM = new ControlDefine( "txtItemNm", "画像解析項目名", "txtItemNm", ControlDefine.BindType.Both, typeof( string ) );
            /// <summary>
            /// 利用ツール
            /// </summary>
            public static readonly ControlDefine USING_TOOL = new ControlDefine( "ddlUsingTool", "画像解析グループコード", "ddlUsingTool", ControlDefine.BindType.Both, typeof( string ) );
        }

        /// <summary>
        /// 一覧定義
        /// </summary>
        internal class GRID_MODEL {
            /// <summary>
			/// チェックボックス
			/// </summary>
			public static readonly GridViewDefine UPD_CHECK = new GridViewDefine( "ﾁｪｯｸ", "", typeof( string ), "", false, HorizontalAlign.Center, 60, true, false );
            /// <summary>
			/// 型式
			/// </summary>
			public static readonly GridViewDefine ITEM_CD = new GridViewDefine( "画像解析項目コード", "ItemCd", typeof( string ), "", true, HorizontalAlign.Center, 160, true, true );
            /// <summary>
            /// 型式名
            /// </summary>
            public static readonly GridViewDefine ITEM_NM = new GridViewDefine( "画像解析項目名", "ItemNm", typeof( string ), "", true, HorizontalAlign.Left, 160, true, true );
            /// <summary>
            /// インデックスNo
            /// </summary>
            public static GridViewDefine INDEX_NO = new GridViewDefine( "インデックスNo", "", typeof( string ), "", true, HorizontalAlign.Center, 180, true, false );
            /// <summary>
            /// 付帯情報有無
            /// </summary>
            public static GridViewDefine SFLAG = new GridViewDefine( "付帯情報有無", "", typeof( string ), "", true, HorizontalAlign.Center, 120, true, false );
            /// <summary>
            /// 設備連携要否
            /// </summary>
            public static GridViewDefine CFLAG = new GridViewDefine( "設備連携要否", "", typeof( string ), "", true, HorizontalAlign.Center, 120, true, false );
            /// <summary>
            /// 利用ツール
            /// </summary>
            public static GridViewDefine USING_TOOL = new GridViewDefine( "利用ツール", "UsingTool", typeof( string ), "", true, HorizontalAlign.Left, 100, true, true );
            /// <summary>
            /// インデックス制御
            /// </summary>
            public static GridViewDefine INDEX_NO_ENABLE = new GridViewDefine( "インデックス制御", "IndexNoEnable", typeof( string ), "", true, HorizontalAlign.Left, 150, false, true );
            /// <summary>
            /// 付帯情報有無制御
            /// </summary>
            public static GridViewDefine SFLAG_ENABLE = new GridViewDefine( "付帯情報有無制御", "SFlagEnable", typeof( string ), "", true, HorizontalAlign.Left, 150, false, true );
            /// <summary>
            /// 設備連携要否制御
            /// </summary>
            public static GridViewDefine CFLAG_ENABLE = new GridViewDefine( "設備連携要否制御", "CFlagEnable", typeof( string ), "", true, HorizontalAlign.Left, 150, false, true );
        }

        /// <summary>
        /// 一覧表示情報コントロール(左下)
        /// </summary>
        public class GRID_SEARCH_CONTROLS_L {
            public static readonly ControlDefine UPD_CHECK = new ControlDefine( "chkUpdate", "ﾁｪｯｸ", "", ControlDefine.BindType.None, null );
            public static readonly ControlDefine INDEX_NO = new ControlDefine( "inputIndexNo", "インデックスNo", "IndexNo", ControlDefine.BindType.Both, null );
            public static readonly ControlDefine SFLAG = new ControlDefine( "inputSFlag", "付帯情報有無", "SFlag", ControlDefine.BindType.Both, null );
            public static readonly ControlDefine CFLAG = new ControlDefine( "inputCFlag", "設備連携要否", "CFlag", ControlDefine.BindType.Both, null );
        }

        #region スクリプト
        /// <summary>
        /// ポストバック後実行スクリプト
        /// </summary>
        private static readonly KeyValuePair<string, string> SCRIPT_AFTER_SELECT = new KeyValuePair<string, string>( "ProcessFilteringView_AfterSelect", "$(function(){ProcessFilteringView.AfterSelect()});" );
        #endregion

        #endregion

        #region 変数定義

        private static Dictionary<string, string> _orgData = new Dictionary<string, string>();
        private static Dictionary<string, string> _resultData = new Dictionary<string, string>();
        private int _execKbn = 1;

        #endregion

        #region プロパティ
        /// <summary>
        /// ロガー
        /// </summary>
        private static readonly Logger Logger = new Logger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

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
        /// 一覧定義情報
        /// </summary>
        GridViewDefine[] _gridviewDefault = null;

        /// <summary>
        /// 一覧定義情報アクセサ
        /// </summary>
        GridViewDefine[] gridviewDefault
        {
            get
            {
                if ( true == ObjectUtils.IsNull( _gridviewDefault ) ) {
                    _gridviewDefault = ControlUtils.GetGridViewDefineArray( typeof( GRID_MODEL ) );
                }
                return _gridviewDefault;
            }
        }

        /// <summary>
		/// 表示中ページ(ベースフォーム)情報
		/// </summary>
		private BaseForm CurrentForm {
            get {
                return ( (BaseForm)Page );
            }
        }
        #endregion

        #region イベント
        /// <summary>
        /// 初期表示イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load( object sender, EventArgs e ) {
            base.RaiseEvent( DoPageLoad, false );

        }

        /// <summary>
        /// 一覧表示情報コントロール(右下)
        /// </summary>
        public class GRID_SEARCH_CONTROLS_R {
        }
        /// <summary>
        /// ページロード処理
        /// </summary>
        protected override void DoPageLoad( ) {

            // ベースページロード処理
            base.DoPageLoad();

            // グリッドビューの再表示
            GridView frozenGrid = grvMainViewLB;
            ControlUtils.SetGridViewTemplateField( grvMainViewLB, ControlUtils.GetFrozenColumns( frozenGrid, gridviewDefault, true ) );
            ControlUtils.ReMakeBoundTemplateFieldCtrl( grvMainViewLB, ControlUtils.GetFrozenColumns( frozenGrid, gridviewDefault, true ) );
            int resultCnt = 0;

            if ( ObjectUtils.IsNotNull( ConditionInfo.ResultData ) ) {
                //検索結果がnullでない場合
                resultCnt = ConditionInfo.ResultData.Rows.Count;
            }
           
        }

        /// <summary>
        /// 検索ボタン押下イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click( object sender, EventArgs e ) {
            RaiseEvent( DoSearch );
        }

        /// <summary>
        /// 工程作業一覧グリッドビューデータバインドイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grvMainViewLB_RowDataBound( object sender, GridViewRowEventArgs e ) {

            if ( e.Row.RowType == DataControlRowType.DataRow ) {

                DataRow row = ( (DataRowView)e.Row.DataItem ).Row;

                //ここで画面項目制御
                CheckBox chkDel = (CheckBox)e.Row.FindControl( GRID_SEARCH_CONTROLS_L.UPD_CHECK.controlId );
                chkDel.Checked = Convert.ToBoolean( row["check"].ToString() );

                Dictionary<string, object> dicControls = new Dictionary<string, object>();
                base.SetControlValues( e.Row, ControlUtils.GetControlDefineArray( typeof( GRID_SEARCH_CONTROLS_L ) ), row, ref dicControls );

            }

            RowDataBoundMainViewLB( sender, e );

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
                //ここで画面項目制御
                CheckBox chkDel = (CheckBox)e.Row.FindControl( GRID_SEARCH_CONTROLS_L.UPD_CHECK.controlId );
                Dictionary<string, object> dicControls = new Dictionary<string, object>();
                base.SetControlValues( e.Row, ControlUtils.GetControlDefineArray( typeof( GRID_SEARCH_CONTROLS_L ) ), row, ref dicControls );
            }
            ControlUtils.GridViewRowBound( (GridView)sender, e, "" );
        }

        /// <summary>
        /// 選択ボタン押下イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnInsert_Click( object sender, EventArgs e ) {
            RaiseEvent( DoInsert );
        }
        #endregion

        #region メソッド
        /// <summary>
        /// 初期処理
        /// </summary>
        protected override void Initialize() {
            //初期設定
            base.Initialize();

            //閉じる
            btnCancel.Attributes[ControlUtils.ON_CLICK] = CLOSE_MODAL_DISP;

            //実行モード
            this._execKbn = NumericUtils.ToInt( Request.QueryString["exeKbn"] );

            //アクセスカウンター登録
            Dao.Com.AccessCounterDao.Entry( base.CurrentPageInfo.pageId );
            _resultData.Clear();
            _orgData.Clear();
            _orgData.Add( "exeKbn", Request.QueryString["exeKbn"] );
            _orgData.Add( "txtLineCd", Request.QueryString["txtLineCd"] );
            _orgData.Add( "txtStationCd", Request.QueryString["txtStationCd"] );
            _orgData.Add( "txtGroupCd", Request.QueryString["txtGroupCd"] );
            divGrvDisplay.Visible = false;

            //検索結果がnullでない場合
            ddlUsingTool.Items.Clear();
            ddlUsingTool.Items.Add( FLAG_DISP );
            ListItem[] UsingToolList = null;
            try {
                // 一覧情報を検索条件：UsingToolのリストに設定する
                UsingToolList = ControlUtils.GetListItems( AiImageCheckDao.SelectAnlUsingToolList(), AiImageCheckDao.COLNAME_TEXT, AiImageCheckDao.COLNAME_VALUE );
            } catch ( DataAccessException ex ) {
                if ( ex.OracleErrorNumber == DataAccessBase.ORAERROR_TIMEOUT ) {
                    //クエリ発行タイムアウト
                    Logger.Exception( ex );
                } else {
                    //タイムアウト以外のException
                    Logger.Exception( ex );
                }
            } catch ( Exception ex ) {
                Logger.Exception( ex );
            } finally {
            }

            if ( null != UsingToolList ) {
                if (  0 < UsingToolList.Length ) {
                    ddlUsingTool.Items.AddRange( UsingToolList );
                }
            }

            //ユーザ権限取得
            UserInfoSessionHandler.ST_USER loginInfo = base.SessionManager.GetUserInfoHandler().GetUserInfo();
            if ( true == ObjectUtils.IsNotNull( loginInfo.UserInfo ) ) {
                AppPermission.PERMISSION_INFO permInfo = AppPermission.GetTransactionPermission( PageInfo.AnlGroupInput, loginInfo.UserInfo );
                //更新権限があれば追加ボタンを活性化
                if ( permInfo.IsEdit ) {
                    btnInsert.Enabled = true;
                    btnAllCheck.Enabled = false;
                } else {
                    btnInsert.Enabled = false;
                    btnAllCheck.Enabled = false;
                }
            }

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
            gridColumns = ControlUtils.GetGridViewDefineArray( typeof( GRID_MODEL ) );

            //検索結果取得
            //エラー時にも最後まで処理を通すため、インスタンスを生成しておく
            var result = new AiImageCheckBusiness.ImgCheckResultSet();
            DataTable tblResult = null;

            //作業指示保存が重いため、4000件では耐えれない処理速度のため1000件としておく
            int maxGridViewCount = CONST_MAX_RECORD;

            try {
                //throw new Exception();
                result = AiImageCheckBusiness.SelectGroupAsyJyoho( dicCondition, _orgData["txtLineCd"].ToString(), _orgData["txtStationCd"].ToString(), _orgData["txtGroupCd"].ToString(),  maxGridViewCount );
                result.ListTable.AcceptChanges();

            } catch ( DataAccessException ex ) {
                if ( ex.OracleErrorNumber == DataAccessBase.ORAERROR_TIMEOUT ) {
                    //クエリ発行タイムアウト
                    base.WriteApplicationMessage( MsgManager.MESSAGE_WRN_61910 );
                } else {
                    //タイムアウト以外のException
                    Logger.Exception( ex );
                    base.WriteApplicationMessage( MsgManager.MESSAGE_ERR_80010 );
                }
            } catch ( Exception ex ) {
                Logger.Exception( ex );
                base.WriteApplicationMessage( MsgManager.MESSAGE_ERR_84010 );
            } finally {
            }
            ConditionInfoSessionHandler.ST_CONDITION cond = new ConditionInfoSessionHandler.ST_CONDITION();
            tblResult = result.ListTable;
            if ( null != tblResult ) {
                //検索条件/結果インスタンス
                cond.conditionValue = dicCondition;
                cond.IdWithText = dicIdWithText;
                cond.ResultData = tblResult.DefaultView.ToTable();

                //グリッドビューバインド
                GridView frozenGrid = grvMainViewLB;
                tblResult.DefaultView.Sort = "ItemCd";
                tblResult = tblResult.DefaultView.ToTable();
                tblResult.AcceptChanges();
                if ( true == ObjectUtils.IsNotNull( tblResult ) ) {
                    if ( 0 < tblResult.Rows.Count ) {

                        //新規バインド
                        ControlUtils.ShowGridViewHeader( grvHeaderLT, ControlUtils.GetFrozenColumns( frozenGrid, gridviewDefault, true ), ConditionInfo, true );
                        ControlUtils.BindGridView_WithTempField( grvMainViewLB, ControlUtils.GetFrozenColumns( frozenGrid, gridviewDefault, true ), tblResult );

                        for ( int idx = 0; idx < grvMainViewLB.Rows.Count; idx++ ) {
                            foreach ( Control ctrl in grvMainViewLB.Rows[idx].Cells[4].Controls ) {
                                if ( "inputSFlag" == ctrl.ID ) {
                                    ( (KTDropDownList)ctrl ).Items.Add( "" );
                                    ( (KTDropDownList)ctrl ).Items.Add( "なし" );
                                    ( (KTDropDownList)ctrl ).Items.Add( "あり" );
                                    int defVal = Convert.ToInt32( tblResult.Rows[idx].ItemArray[3].ToString().Trim() );
                                    if ( defVal == 1 ) {
                                        ( (KTDropDownList)ctrl ).SelectedValue = "あり";
                                    } else {
                                        ( (KTDropDownList)ctrl ).SelectedValue = "なし";
                                    }
                                    if ( true == tblResult.Rows[idx].ItemArray[7].ToString().Trim().Equals( "1" ) ) {
                                        ( (KTDropDownList)ctrl ).Enabled = false;
                                    }
                                }
                            }

                            foreach ( Control ctrl in grvMainViewLB.Rows[idx].Cells[5].Controls ) {
                                if ( "inputCFlag" == ctrl.ID ) {
                                    ( (KTDropDownList)ctrl ).Items.Add( "" );
                                    ( (KTDropDownList)ctrl ).Items.Add( "不要" );
                                    ( (KTDropDownList)ctrl ).Items.Add( "必要" );
                                    int defVal = Convert.ToInt32( tblResult.Rows[idx].ItemArray[4].ToString().Trim() );
                                    if ( defVal == 1 ) {
                                        ( (KTDropDownList)ctrl ).SelectedValue = "必要";
                                    } else {
                                        ( (KTDropDownList)ctrl ).SelectedValue = "不要";
                                    }
                                    if ( true == tblResult.Rows[idx].ItemArray[6].ToString().Trim().Equals( "1" ) ) {
                                        ( (KTDropDownList)ctrl ).Enabled = false;
                                    }
                                }
                            }

                            foreach ( Control ctrl in grvMainViewLB.Rows[idx].Cells[3].Controls ) {
                                if ( "inputIndexNo" == ctrl.ID ) {
                                    ( (KTTextBox)ctrl ).MaxLength = 3;
                                    ( (KTTextBox)ctrl ).InputMode = (KTTextBox.InputModeType)Enum.Parse( typeof( KTTextBox.InputModeType ), "IntNum" );
                                    if ( true == tblResult.Rows[idx].ItemArray[5].ToString().Equals( "1" ) ) {
                                        ( (KTTextBox)ctrl ).Enabled = false;
                                    }
                                }
                            }
                        }

                        //GridView表示
                        divGrvDisplay.Visible = true;

                        //グリッドビュー外のDivサイズ変更
                        SetDivGridViewWidth();
                    } else {
                        ClearGridView();
                    }
                }

            } else {
                //タイムアウト等Exception時には、GridViewクリア
                ClearGridView();
            }

			//権限によるボタン制御
			AppPermission.PERMISSION_INFO permMainteInfo = AppPermission.GetTransactionPermission( PageInfo.AnlGroupInput, LoginInfo.UserInfo );
			if ( permMainteInfo.IsEdit == true ) {
                //更新権限所持＋データが存在する
                if ( null != tblResult) {
                    if ( 0 < tblResult.Rows.Count ) {
                        btnInsert.Enabled = true;
                        btnAllCheck.Enabled = true;
                    } else {
                        btnInsert.Enabled = false;
                        btnAllCheck.Enabled = false;
                    }
                }
			} else {
                btnInsert.Enabled = false;
                btnAllCheck.Enabled = false;
            }

			//メッセージ表示
			if ( null != result.Message ) {
                base.WriteApplicationMessage( result.Message );
            }

        }


        /// <summary>
        /// 一覧データバインドイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grvMainViewRB_RowDataBound( object sender, GridViewRowEventArgs e ) {

            RowDataBoundMainViewRB( sender, e );
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
            ControlUtils.GridViewRowBound( (GridView)sender, e );
        }

        /// <summary>
        /// 追加処理
        /// </summary>
        private void DoInsert() {

            int exeCnt = 0;
            //追加前権限チェック
            if ( CheckInputData() == false ) {
                return;
            }

            //追加前データ整形
            DataTable dtUpdate = new DataTable();
            dtUpdate.Columns.Add( "ITEM_CD" );
            dtUpdate.Columns.Add( "GROUP_CD" );
            dtUpdate.Columns.Add( "INDEX_NO" );
            dtUpdate.Columns.Add( "SFLAG" );
            dtUpdate.Columns.Add( "CFLAG" );
            dtUpdate.Columns.Add( "UPDATE_BY" );
            dtUpdate.Columns.Add( "UPDATE_PG" );

            foreach ( GridViewRow row in grvMainViewLB.Rows ) {
                CheckBox chkDel = (CheckBox)row.FindControl( GRID_SEARCH_CONTROLS_L.UPD_CHECK.controlId );
                if ( true == chkDel.Checked ) {
                    DataRow row2 = dtUpdate.NewRow();
                    row2["ITEM_CD"] = ( (Label)row.Cells[1].Controls[0] ).Text;
                    row2["GROUP_CD"] = _orgData["txtGroupCd"].ToString();
                    row2["INDEX_NO"] = ( (KTTextBox)row.Cells[3].Controls[1] ).Value.Trim();
                    string sFlag = ( (KTDropDownList)row.Cells[4].Controls[1] ).SelectedValue;
                    if ( true == sFlag.Equals( "あり" ) ) {
                        sFlag = "1";
                    } else {
                        sFlag = "0";
                    }
                    row2["SFLAG"] = sFlag;
                    sFlag = ( (KTDropDownList)row.Cells[5].Controls[1] ).SelectedValue;
                    if ( true == sFlag.Equals( "必要" ) ) {
                        sFlag = "1";
                    } else {
                        sFlag = "0";
                    }
                    row2["CFLAG"] = sFlag;
                    row2["UPDATE_BY"] = LoginInfo.UserInfo.userId;                          //更新者
                    row2["UPDATE_PG"] = PageInfo.AnlGroupInput.pageId;                      //更新Pg
                    dtUpdate.Rows.Add( row2 );
                    exeCnt++;
                }
            }
            if ( exeCnt.Equals( 0 ) ) {
                //処理対象データなし
                CurrentForm.WriteApplicationMessage( MsgManager.MESSAGE_WRN_61080 );
                return;
            }

            int retCnt = 0;
            //追加
            retCnt = AiImageCheckDao.InsertGroupAsyJyohoData( dtUpdate, LoginInfo.UserInfo.userId, PageInfo.AnlGroupModelInput.pageId, exeCnt );

            if ( retCnt.Equals( exeCnt ) ) {
                //正常
                CurrentForm.WriteApplicationMessage( MsgManager.MESSAGE_INF_10010 );
            } else if ( retCnt != exeCnt ) {
                //処理件数不一致
                CurrentForm.WriteApplicationMessage( MsgManager.MESSAGE_ERR_72060, "" );
                return;
            } else {
                CurrentForm.WriteApplicationMessage( MsgManager.MESSAGE_ERR_80010 );
                return;
            }
            ////正常終了時、再検索
            DoSearch();
        }

        /// <summary>
		/// 処理実行前データチェック
		/// </summary>
		/// <returns></returns>
		private bool CheckInputData( ) {

            //ユーザ情報チェック
            if ( true == ObjectUtils.IsNull( LoginInfo.UserInfo )
                || true == StringUtils.IsEmpty( LoginInfo.UserInfo.userName ) ) {
                PageUtils.RedirectToErrorPage( Page, MsgManager.MESSAGE_ERR_72020, null );
                return false;
            }

            //更新権限チェック
            AppPermission.PERMISSION_INFO permMainteInfo = AppPermission.GetTransactionPermission( PageInfo.AnlGroupInput, LoginInfo.UserInfo );
            if ( permMainteInfo.IsEdit == false ) {
                PageUtils.RedirectToErrorPage( Page, MsgManager.MESSAGE_ERR_72030, null );
                return false;
            }

            //データチェック
            int idx = 0;
            foreach ( GridViewRow row in grvMainViewLB.Rows ) {
                CheckBox chkDel = (CheckBox)row.FindControl( GRID_SEARCH_CONTROLS_L.UPD_CHECK.controlId );
                if ( true == chkDel.Checked ) {
                    string txtIndexNo = ( (KTTextBox)row.Cells[3].Controls[1] ).Value.Trim();
                    string txtSFlag = ( (KTDropDownList)row.Cells[4].Controls[1] ).SelectedValue.Trim();
                    string txtCFlag = ( (KTDropDownList)row.Cells[5].Controls[1] ).SelectedValue.Trim();
                    string txtIndexNoEnable = ( (Label)row.Cells[7].Controls[0] ).Text;
                    string txtSFlagEnable = ( (Label)row.Cells[8].Controls[0] ).Text;
                    string txtCFlagEnable = ( (Label)row.Cells[9].Controls[0] ).Text;

                    if ( true == txtIndexNoEnable.Equals( "3" ) && txtIndexNo.Length == 0 ) {
                        CurrentForm.WriteApplicationMessage( MsgManager.MESSAGE_WRN_62030, "インデックスNo" );
                        string colId = "grvMainViewLB_inputIndexNo_" + idx.ToString();
                        ScriptManager.RegisterClientScriptBlock( this, this.GetType(), "return AnlGroupListInput_setFocus", "$(function(){AnlGroupListInput.setFocus('" + colId + "')});", true );
                        return false;
                    }

                    if ( true == txtSFlagEnable.Equals( "3" ) && txtSFlag.Length == 0 ) {
                        CurrentForm.WriteApplicationMessage( MsgManager.MESSAGE_WRN_62030, "付帯情報有無" );
                        string colId = "grvMainViewLB_inputSFlag_" + idx.ToString();
                        ScriptManager.RegisterClientScriptBlock( this, this.GetType(), "return AnlGroupListInput_setFocus", "$(function(){AnlGroupListInput.setFocus('" + colId + "')});", true );
                        return false;
                    }

                    if ( true == txtCFlagEnable.Equals( "3" ) && txtCFlag.Length == 0 ) {
                        CurrentForm.WriteApplicationMessage( MsgManager.MESSAGE_WRN_62030, "設備連携要否" );
                        string colId = "grvMainViewLB_inputFFlag_" + idx.ToString();
                        ScriptManager.RegisterClientScriptBlock( this, this.GetType(), "return AnlGroupListInput_setFocus", "$(function(){AnlGroupListInput.setFocus('" + colId + "')});", true );
                        return false;
                    }
                }
                idx++;
            }

            return true;
        }

        /// <summary>
		/// グリッドビュークリア
		/// </summary>
		private void ClearGridView( ) {

            //列名非表示 グリッドビュークリア
            ControlUtils.InitializeGridView( grvHeaderLT, false );
            ControlUtils.InitializeGridView( grvMainViewLB, false );

            //グリッドビュー外のDivサイズ変更
            SetDivGridViewWidth();

            //GridView非表示
            divGrvDisplay.Visible = false;

            //ボタンの活性状態切替
            SetButtonPermission();
        }

        /// <summary>
        /// グリッドビュー外のDivサイズ変更
        /// </summary>
        private void SetDivGridViewWidth( ) {
            SetDivGridViewWidth( grvHeaderLT, divGrvHeaderLT );
            SetDivGridViewWidth( grvMainViewLB, divGrvLB );
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

        #region ボトムボタン表示処理
        /// <summary>
        /// セッションの検索結果、ユーザー権限からボトムボタンを活性
        /// </summary>
        private void SetButtonPermission( ) {

            //初期化
            //セッションから検索結果を取得
            string token = StringUtils.Nvl( base.GetTransInfoValue( RequestParameter.Common.TOKEN ) );
            ConditionInfoSessionHandler.ST_CONDITION cond = base.SessionManager.GetConditionInfoHandler( token ).GetCondition( PageInfo.AnlGroupModelInput.pageId );
            DataTable tblResult = cond.ResultData;

            //ユーザ権限取得
            UserInfoSessionHandler.ST_USER loginInfo = base.SessionManager.GetUserInfoHandler().GetUserInfo();

            //検索結果の有無確認
            if ( null != tblResult && 0 < tblResult.Rows.Count ) {

                if ( true == ObjectUtils.IsNotNull( loginInfo.UserInfo ) ) {
                    AppPermission.PERMISSION_INFO permInfo = AppPermission.GetTransactionPermission( PageInfo.AnlGroupInput, loginInfo.UserInfo );
                    //更新権限があれば更新、削除ボタン活性
                    if ( permInfo.IsEdit ) {
                    }
                }
            }
        }
        #endregion
    }
}