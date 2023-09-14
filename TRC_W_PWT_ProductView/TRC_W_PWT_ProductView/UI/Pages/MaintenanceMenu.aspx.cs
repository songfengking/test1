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
using KTFramework.Web.Client.CoreService;
using System.Text.RegularExpressions;
using TRC_W_PWT_ProductView.Dao.Com;
using TRC_W_PWT_ProductView.SrvCore;

namespace TRC_W_PWT_ProductView.UI.Pages {

	/// <summary>
	/// メンテナンスメニュー画面
	/// </summary>
	public partial class MaintenanceMenu : BaseForm {

		#region 変数

		///<summary>ロガー定義</summary>
		private static readonly Logger logger = new Logger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

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
		#endregion

		#region スクリプトイベント
		/// <summary>
		/// ID/旧PW/新PW/新PW確認 未入力側にフォーカス移動 全て入力済みの時には、更新ボタン押下
		/// 新PWと新PW確認の入力値チェック
		/// </summary>
		const string CHECK_INPUT = "ChangePassword.CheckInput();";

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
		/// 複数型式検索出力
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void btnMultipleSearchModelCd_Click( object sender, EventArgs e ) {
			base.RaiseEvent( DoMultipleSearchModelCd );
		}

		#endregion

		#region イベントメソッド

		#region ページイベント

		/// <summary>
		/// ページロード処理
		/// </summary>
		protected override void DoPageLoad( ) {

			//ベース ページロード処理
			base.DoPageLoad();

			//javascriptイベント文字列作成
			string javaScriptEvent = String.Format( CHECK_INPUT );

			//権限別ボタン制御
			ButtonControl();

		}

		#endregion

		#region 複数型式検索出力
		/// <summary>
		/// 複数型式検索出力ボタン押下処理
		/// </summary>
		private void DoMultipleSearchModelCd( ) {

			// 処理実行中チェック
			DataTable jobControlInfo = JobControlInfoDao.Select( TrcWebConsts.JOB_PARAM_KEY_JOB_NM_PSS_DATA_OUTPUT );

			if ( 0 != jobControlInfo.Rows.Count ) {
				base.WriteApplicationMessage( MsgManager.MESSAGE_WRN_63010 );
				return;
			}

			// ユーザ実行JOB起動要求
			PSSDataOutput();
		}

		/// <summary>
		/// ユーザ実行JOB起動要求
		/// </summary>
		private void PSSDataOutput( ) {
			// 起動パラメータ設定
			List<CoreService.JobParameter> jobParam = new List<CoreService.JobParameter>();
			// 従業員番号
			jobParam.Add( new CoreService.JobParameter(
				TrcWebConsts.JOB_PARAM_KEY_USER_ID, SessionManager.GetUserInfoHandler().GetUserInfo().UserInfo.userId ) );
			// メールアドレス
			jobParam.Add( new CoreService.JobParameter(
				TrcWebConsts.JOB_PARAM_KEY_MAIL_ADDRESS, SessionManager.GetUserInfoHandler().GetUserInfo().UserInfo.mailAddress ) );
			// ユーザ実行JOB起動要求送信
			C1010RequestDto resultDto = new CoreService().RequestBatchExec2(
				//実行タスクID
				TrcWebConsts.JOB_PSS_DATA_OUTPUT,
				// ログインユーザID
				SessionManager.GetUserInfoHandler().GetUserInfo().UserInfo.userId,
				// ユーザ名
				SessionManager.GetUserInfoHandler().GetUserInfo().UserInfo.userName,
				// メールアドレス
				SessionManager.GetUserInfoHandler().GetUserInfo().UserInfo.mailAddress,
				// 端末IPアドレス
				SessionManager.GetUserInfoHandler().GetUserInfo().UserInfo.ipAddress,
				// JOBネット名
				TrcWebConsts.JOB_PARAM_KEY_JOB_NM_PSS_DATA_OUTPUT,
				// 起動パラメータ         
				jobParam );
			// 要求結果確認
			if ( resultDto.resultCd.IndexOf( "INF" ) >= 0 ) {
				//正常終了
				base.WriteApplicationMessage( MsgManager.MESSAGE_INF_10040, resultDto.resultMessage );
			} else {
				//それ以外
				base.WriteApplicationMessage( MsgManager.MESSAGE_ERR_72070, resultDto.resultMessage );
			}
		}

		#endregion

		#endregion

		#region メソッド

		#region 画面初期化処理
		/// <summary>
		/// 初期化処理
		/// </summary>
		protected override void Initialize( ) {
			//ベース処理初期化処理
			base.Initialize();

			// P0113_トレーサビリティシステムリプレース（Java→C#.NET）
			// C#版画面追加に伴い、以下5画面のリンク先URL設定を削除
			// ・製品別通過実績検索
			// ・ステーション別通過実績検索
			// ・順序情報検索
			// ・エンジン立体倉庫在庫検索
			// ・搭載エンジン引当検索
		}
		#endregion

		#region ボタン制御

		/// <summary>
		/// 権限によるボタン制御
		/// </summary>
		private void ButtonControl( ) {
			//メンテナンスの入力画面の権限を所持しているかでボタン制御を行う

			//対象外リスト
			AppPermission.PERMISSION_INFO permNACheckList = AppPermission.GetTransactionPermission( PageInfo.NACheckList, LoginInfo.UserInfo );
			if ( permNACheckList.IsView == false ) {
				this.ktbtnImpChkNAList.Enabled = false;
			}

			//3Cメンテ画面
			AppPermission.PERMISSION_INFO permProcDtEdit = AppPermission.GetTransactionPermission( PageInfo.ProcessingDtEdit, LoginInfo.UserInfo );
			if ( permProcDtEdit.IsView == false ) {
				this.btnMasterMainte3CList.Enabled = false;
			}

			//DPF機番情報メンテ画面
			AppPermission.PERMISSION_INFO permDpfSerial = AppPermission.GetTransactionPermission( PageInfo.DpfSerial, LoginInfo.UserInfo );
			if ( permDpfSerial.IsView == false ) {
				this.btnMasterMainteDpf.Enabled = false;
			}

			//是正処置入力画面
			AppPermission.PERMISSION_INFO permMainCorrectiveInput = AppPermission.GetTransactionPermission( PageInfo.MainCorrectiveInput, LoginInfo.UserInfo );
			if ( permMainCorrectiveInput.IsView == false ) {
				this.btnMainCorrectiveView.Enabled = false;
			}

			// 検査項目マスタ画面
			AppPermission.PERMISSION_INFO permAnlItemInput = AppPermission.GetTransactionPermission( PageInfo.AnlItemInput, LoginInfo.UserInfo );
			if ( permAnlItemInput.IsView == false ) {
				this.btnAnlItemView.Enabled = false;
			}

			//検査グループマスタ画面
			AppPermission.PERMISSION_INFO permAnlGroupInput = AppPermission.GetTransactionPermission( PageInfo.AnlGroupInput, LoginInfo.UserInfo );
			if ( permAnlGroupInput.IsView == false ) {
				this.btnAnlGroupView.Enabled = false;
			}

			//型式紐づけ画面
			AppPermission.PERMISSION_INFO permAnlGroupModelInput = AppPermission.GetTransactionPermission( PageInfo.AnlGroupModelInput, LoginInfo.UserInfo );
			if ( permAnlGroupModelInput.IsView == false ) {
				this.btnAnlGroupModelView.Enabled = false;
			}

			// 複数型式検索出力ボタンの表示状態をAction1権限に応じて切り替える(検索[Excel出力]の権限を流用)
			btnMultipleSearchModelCd.Enabled = AppPermission.GetTransactionPermission( TRC_W_PWT_ProductView.Defines.PageInfo.MaintenanceMenu, LoginInfo.UserInfo ).IsAction1;

		}
		#endregion

		#endregion

		/// <summary>
		/// チェック対象外リスト
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void ktbtnImpChkNAList_Click( object sender, EventArgs e ) {
			PageUtils.RedirectToTRC( this.Page, CurrentPageInfo.pageId, PageInfo.MasterMainteNAList.url, this.Token, null );
		}

		/// <summary>
		/// 3C加工日修正画面
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void btnMasterMainte3CList_Click( object sender, EventArgs e ) {
			PageUtils.RedirectToTRC( this.Page, CurrentPageInfo.pageId, PageInfo.MasterMainte3CList.url, this.Token, null );
		}
		/// <summary>
		/// DPF機番情報修正画面起動
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void btnMasterMainteDpf_Click( object sender, EventArgs e ) {
			PageUtils.RedirectToTRC( this.Page, CurrentPageInfo.pageId, PageInfo.MasterMainteDpfList.url, this.Token, null );
		}


		/// <summary>
		/// 検索画面起動
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void btnMainView_Click( object sender, EventArgs e ) {
			PageUtils.RedirectToTRC( this.Page, CurrentPageInfo.pageId, PageInfo.MainView.url, this.Token, null );
		}

		/// <summary>
		/// 是正処置入力検索画面起動sfadd
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void btnMainCorrectiveView_Click( object sender, EventArgs e ) {
			PageUtils.RedirectToTRC( this.Page, CurrentPageInfo.pageId, PageInfo.MainCorrectiveView.url, this.Token, null );
		}

		/// <summary>
		/// 画像解析グループ検索画面起動sfadd
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void btnAnlGroupView_Click( object sender, EventArgs e ) {
			PageUtils.RedirectToTRC( this.Page, CurrentPageInfo.pageId, PageInfo.AnlGroupView.url, this.Token, null );
		}

		/// <summary>
		/// 検査項目マスタ検索画面起動maadd
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void btnAnlItemView_Click( object sender, EventArgs e ) {
			PageUtils.RedirectToTRC( this.Page, CurrentPageInfo.pageId, PageInfo.AnlItemView.url, this.Token, null );
		}

		/// <summary>
		/// 型式紐づけ画面起動sfadd
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void btnAnlGroupModelView_Click( object sender, EventArgs e ) {
			PageUtils.RedirectToTRC( this.Page, CurrentPageInfo.pageId, PageInfo.AnlGroupModelView.url, this.Token, null );
		}

		/// <summary>
		/// 製品別通過実績検索画面起動
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void btnSearchProductOrder_Click( object sender, EventArgs e ) {
			PageUtils.RedirectToTRC( this.Page, CurrentPageInfo.pageId, PageInfo.SearchProductOrder.url, this.Token, null );
		}

		/// <summary>
		/// ステーション別通過実績検索画面起動
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void btnSearchStationOrder_Click( object sender, EventArgs e ) {
			PageUtils.RedirectToTRC( this.Page, CurrentPageInfo.pageId, PageInfo.SearchStationOrder.url, this.Token, null );
		}

		/// <summary>
		/// 順序情報検索画面起動
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void btnSearchOrderInfo_Click( object sender, EventArgs e ) {
			PageUtils.RedirectToTRC( this.Page, CurrentPageInfo.pageId, PageInfo.SearchOrderInfo.url, this.Token, null );
		}

		/// <summary>
		/// エンジン立体倉庫在庫検索画面起動
		/// </summary>
		/// <param namje="sender"></param>
		/// <param name="e"></param>
		protected void btnSearchEngineStock_Click( object sender, EventArgs e ) {
			PageUtils.RedirectToTRC( this.Page, CurrentPageInfo.pageId, PageInfo.SearchEngineStock.url, this.Token, null );
		}

		/// <summary>
		/// 搭載エンジン引当検索画面起動
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void btnSearchCallEngineSimulation_Click( object sender, EventArgs e ) {
			PageUtils.RedirectToTRC( this.Page, CurrentPageInfo.pageId, PageInfo.SearchCallEngineSimulation.url, this.Token, null );
		}

		/// <summary>
		/// 部品消費予定照会画面起動
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void btnPartsConsumptionPlan_Click( object sender, EventArgs e ) {
			//PageUtils.RedirectToTRC( this.Page, CurrentPageInfo.pageId, PageInfo.BatchPartsTrace.url, this.Token, null );
		}

		/// <summary>
		/// 部品消費実績照会画面起動
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void btnPartsConsumptionResult_Click( object sender, EventArgs e ) {
			//PageUtils.RedirectToTRC( this.Page, CurrentPageInfo.pageId, PageInfo.BatchPartsTrace.url, this.Token, null );
		}

		/// <summary>
		/// ピッキング状況画面起動
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void btnKanbanPickingStatusView_Click( object sender, EventArgs e ) {
			PageUtils.RedirectToTRC( this.Page, CurrentPageInfo.pageId, PageInfo.KanbanPickingStatusView.url, this.Token, null );
		}

		/// <summary>
		/// 未完了ピッキング画面起動
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void btnKanbanShortageView_Click( object sender, EventArgs e ) {
			PageUtils.RedirectToTRC( this.Page, CurrentPageInfo.pageId, PageInfo.KanbanShortageView.url, this.Token, null );
		}

		/// <summary>
		/// ブラウザ判定
		/// </summary>
		private bool IsIE( ) {
			try {
				string browserName = Request.Browser.Browser;
				logger.Info( "IE判定 clientIp={0},clientBrowser={1}", KTWebUtils.GetClientIp( Request ), browserName );

				if ( browserName == "InternetExplorer"
					|| browserName == "IE"
					|| browserName == "Trident" ) {
					return true;
				}
				return false;
			} catch ( Exception ex ) {
				logger.Exception( ex );
				return false;
			}
		}


	}
}