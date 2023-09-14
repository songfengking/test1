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
using TRC_W_PWT_ProductView.Dao.Com;
using TRC_W_PWT_ProductView.Dao.Process;
using TRC_W_PWT_ProductView.Business;
using System.Diagnostics;

namespace TRC_W_PWT_ProductView.UI.Pages.Maintenance {
    public partial class ProcessingDtEdit : BaseForm {
        //ロガー定義
        private static readonly Logger logger = new Logger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

        #region 定数定義

        //画面mode
        const string CONST_EXEC_NONE = "0";          //選択無し
        const string CONST_EXEC_SINGLE = "1";      　//単一選択
        const string CONST_EXEC_MULTIPLE = "2";      //複数選択

        #endregion

        Dictionary<string, object> dicControlValues = new Dictionary<string, object>();
        private Dictionary<string, string> _orgData = new Dictionary<string, string>();

        #region プロパティ

        /// <summary>
        /// 表示中ページ(ベースフォーム)情報
        /// </summary>
        private BaseForm CurrentForm {
            get {
                return ( ( BaseForm )Page );
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
        /// (メイン)コントロール定義
        /// </summary>
        ControlDefine[] _mainControls = null;
        /// <summary>
        /// (メイン)コントロール定義アクセサ
        /// </summary>
        ControlDefine[] MainControls {
            get {
                return _mainControls;
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

        public String _Proc3CDt;
        public String Proc3Cdt {
            get {
                return _Proc3CDt;
            }
            set {
                _Proc3CDt = StringUtils.ToString( txtUpdDt.Text );
            }
        }

        #endregion

        #region スクリプトイベント
        const string UPDATE_3C_PROC_DT = "ProcessingDtEdit.Update3CProcDt();";
        const string CLOSE_MODAL_DISP = "ProcessingDtEdit.CloseModal();";
        const string CHECK_UPDATE = "ProcessingDtEdit.CheckUpdateDt();";
        const string CHECK_UPDATE_NUM = "ProcessingDtEdit.CheckUpdateNum();";
        const string CHECK_UPDATE_LINE = "ProcessingDtEdit.CheckUpdateLine();";
        const string CHECK_REMARK = "ProcessingDtEdit.CheckRemark();";

        #endregion

        protected void Page_Load( object sender, EventArgs e ) {
            base.RaiseEvent( DoPageLoad, false );
        }
        #region ページイベント

        /// <summary>
        /// ページロード処理
        /// </summary>
        protected override void DoPageLoad() {

            //アクセスカウンター登録
            Dao.Com.AccessCounterDao.Entry( base.CurrentPageInfo.pageId );

            //加工日
            txtUpdDt.Attributes["onchange"] = CHECK_UPDATE;
            txtUpdLine.Attributes["onchange"] = CHECK_UPDATE_LINE;
            txtRemark.Attributes["onchange"] = CHECK_REMARK;

            //一覧チェック件数に応じてレイアウト変更
            if( Request.QueryString["subIndex"].Equals( CONST_EXEC_NONE ) ) {
                //一覧未選択 

                //更新ボタン不可               
                btn3CUpdate.Enabled = false;
                base.WriteApplicationMessage( MsgManager.MESSAGE_WRN_62020, "更新対象データ" );
            } else if( Request.QueryString["subIndex"].Equals( CONST_EXEC_SINGLE ) ) {
                //一覧単一選択

                //一括更新文言非表示
                msgMultiple.Visible = false;
                // 連番編集可
                txtUpdNum.Enabled = true;
                txtUpdNum.Attributes["onchange"] = CHECK_UPDATE_NUM;
                //更新ボタン可
                btn3CUpdate.Enabled = true;
                btn3CUpdate.Attributes[ControlUtils.ON_CLICK] = UPDATE_3C_PROC_DT;
            } else {
                //一覧複数選択

                //一括更新文言表示
                msgMultiple.Visible = true;
                // 連番編集不可
                txtUpdNum.Enabled = false;
                //更新ボタン可               
                btn3CUpdate.Enabled = true;
                btn3CUpdate.Attributes[ControlUtils.ON_CLICK] = UPDATE_3C_PROC_DT;
            }

            //閉じるボタン
            btnClose.Attributes[ControlUtils.ON_CLICK] = CLOSE_MODAL_DISP;

            //初期設定
            InitializeSet();

        }

        /// <summary>
        /// 初期化処理
        /// </summary>
        public void InitializeSet() {



            //更新権限チェック
            AppPermission.PERMISSION_INFO permMainteInfo = AppPermission.GetTransactionPermission( PageInfo.ProcessingDtEdit, LoginInfo.UserInfo );
            if ( permMainteInfo.IsView == false ) {
                PageUtils.RedirectToErrorPage( Page, MsgManager.MESSAGE_ERR_72030, null );
                return;
            } else if ( permMainteInfo.IsEdit == false ) {
                this.btn3CUpdate.Enabled = false;
            }


        }

        #endregion

        /// <summary>
        /// 入力エラー(加工日)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnProcDtError_Click( object sender, EventArgs e ) {
            CurrentForm.WriteApplicationMessage( MsgManager.MESSAGE_ERR_72080 );
            return;
        }
        /// <summary>
        /// 入力エラー(連番)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnProcNumError_Click( object sender, EventArgs e ) {
            CurrentForm.WriteApplicationMessage( MsgManager.MESSAGE_ERR_72090 );
            return;
        }
        /// <summary>
        /// 入力エラー(加工ライン)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnProcLineError_Click( object sender, EventArgs e ) {
            CurrentForm.WriteApplicationMessage( MsgManager.MESSAGE_ERR_72095 );
            return;
        }
        /// <summary>
        /// 入力エラー(備考)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnRemarkError_Click( object sender, EventArgs e ) {
            CurrentForm.WriteApplicationMessage( MsgManager.MESSAGE_ERR_72100 );
            return;
        }
        /// <summary>
        /// 加工日未入力
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnInputCheck_Click( object sender, EventArgs e ) {
            CurrentForm.WriteApplicationMessage( MsgManager.MESSAGE_WRN_62030, "加工日" );
            return;
        }

        /// <summary>
        /// メッセージクリア
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnClear_Click( object sender, EventArgs e ) {
            CurrentForm.ClearApplicationMessage();
            return;
        }


    }

}