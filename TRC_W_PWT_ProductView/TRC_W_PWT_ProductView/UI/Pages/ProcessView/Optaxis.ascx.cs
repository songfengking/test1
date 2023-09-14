using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using KTFramework.Common;
using KTFramework.Dao;
using KTWebControl.CustomControls;
using KTWebInheritance.Common;
using TRC_W_PWT_ProductView.Common;
using TRC_W_PWT_ProductView.Defines;
using TRC_W_PWT_ProductView.Defines.TypeDefine;
using TRC_W_PWT_ProductView.UI.Base;
using TRC_W_PWT_ProductView.Session;

namespace TRC_W_PWT_ProductView.UI.Pages.ProcessView
{
    public partial class Optaxis : System.Web.UI.UserControl, Defines.Interface.IDetail
    {

        //ロガー定義
        private static readonly Logger logger = new Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region 定数定義
        /// <summary>
        /// 管理ID
        /// </summary>
        const string MANAGE_ID = Defines.Session.DetailFrame.SESSION_PAGE_INFO_DETAIL_KEY;//CurrentUCInfo.pageId

        /// <summary>
        /// DB取得データ格納先 ディクショナリキー
        /// </summary>
        private string SESSION_PAGE_INFO_DB_KEY = "bindTableData";

        /// <summary>
        /// ListView選択行制御
        /// </summary>
        const string MAIN_VIEW_SELECTED = "Optaxis.SelectMainViewRow(this,{0},'{1}');";

        /// <summary>
        /// GridVew選択行制御
        /// </summary>
        const string GRID_SUB_VIEW_GROUP_CD = "SubView";

        /// <summary>
        /// (メインリスト)コントロール定義
        /// </summary>
        public class LIST_MAIN
        {
            /// <summary>TR</summary>
            public static readonly ControlDefine TR_ROW_DATA = new ControlDefine("trRowData", "TR", "", ControlDefine.BindType.None, typeof(String));
            /// <summary>(SELECTコマンド送信用ボタン)</summary>
            public static readonly ControlDefine SELECT = new ControlDefine("btnSelect", "BUTTON", "", ControlDefine.BindType.None, typeof(String));
            /// <summary>型式コード</summary>
            public static readonly ControlDefine MODEL_CD = new ControlDefine("txtModelCd", "型式コード", "modelCd", ControlDefine.BindType.Down, typeof(String));
            /// <summary>型式名</summary>
            //public static readonly ControlDefine MODEL_NM = new ControlDefine( "txtModelNm", "型式名", "modelNm", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>機番</summary>
            public static readonly ControlDefine SERIAL_NUMBER = new ControlDefine("txtSerialNumber", "機番", "serialNumber", ControlDefine.BindType.Down, typeof(String));
            /// <summary>国コード</summary>
            public static readonly ControlDefine COUNTRY_CD = new ControlDefine("txtCountryCd", "国コード", "countryCd", ControlDefine.BindType.Down, typeof(String));
            /// <summary>国名</summary>
            //public static readonly ControlDefine COUNTRY_NM = new ControlDefine( "txtCountryNm", "国名", "countryNm", ControlDefine.BindType.Down, typeof( String ) );
            /// <summary>検査連番</summary>
            public static readonly ControlDefine INSPECTION_SEQ = new ControlDefine("txtInspectionSeq", "検査連番", "inspectionSeq", ControlDefine.BindType.Down, typeof(String));
            /// <summary>IDNO</summary>
            public static readonly ControlDefine IDNO = new ControlDefine("txtIdNo", "IDNO", "idNo", ControlDefine.BindType.Down, typeof(String));
            /// <summary>月度</summary>
            public static readonly ControlDefine MONTH = new ControlDefine("txtMonth", "月度", "month", ControlDefine.BindType.Down, typeof(String));
            /// <summary>月連</summary>
            public static readonly ControlDefine SEQUENCE_NUM = new ControlDefine("txtSequenceNum", "月連", "sequenceNum", ControlDefine.BindType.Down, typeof(String));
            /// <summary>PIN</summary>
            public static readonly ControlDefine PIN_CD = new ControlDefine("txtPinCd", "PIN", "pinCd", ControlDefine.BindType.Down, typeof(String));
            /// <summary>結果</summary>
            public static readonly ControlDefine RESULT = new ControlDefine("txtResult", "結果", "result", ControlDefine.BindType.Down, typeof(String));
            /// <summary>検査グループ</summary>
            public static readonly ControlDefine INSPECTION_GROUP = new ControlDefine("txtInspectionGroup", "検査グループ", "inspectionGroup", ControlDefine.BindType.Down, typeof(String));
            /// <summary>光軸検査フラグ</summary>
            public static readonly ControlDefine OPTAXIS_INS_FLAG = new ControlDefine("txtOptaxisInsFlag", "光軸検査フラグ", "optaxisInsFlag", ControlDefine.BindType.Down, typeof(String));
            /// <summary>光軸リフター上昇値</summary>
            public static readonly ControlDefine OPTAXIS_LIFTUP = new ControlDefine("txtOptaxisLiftup", "光軸検査リフター上昇値", "optaxisLiftup", ControlDefine.BindType.Down, typeof(String));
            /// <summary>光軸リフター停止値</summary>
            public static readonly ControlDefine OPTAXIS_SCREEN = new ControlDefine("txtOptaxisScreen", "光軸検査リフター停止値", "optaxisScreen", ControlDefine.BindType.Down, typeof(String));
            /// <summary>結果光軸検査フラグL</summary>
            public static readonly ControlDefine RT_OPTAXIS_INS_FLAG_L = new ControlDefine("txtRtOptaxisInsFlagL", "結果光軸検査フラグL", "rtOptaxisInsFlagL", ControlDefine.BindType.Down, typeof(String));
            /// <summary>結果光軸検査フラグR</summary>
            public static readonly ControlDefine RT_OPTAXIS_INS_FLAG_R = new ControlDefine("txtRtOptaxisInsFlagR", "結果光軸検査フラグR", "rtOptaxisInsFlagR", ControlDefine.BindType.Down, typeof(String));
            /// <summary>作成日</summary>
            public static readonly ControlDefine INSPECTION_DT = new ControlDefine("txtInspectionDt", "作成日時", "inspectionDt", ControlDefine.BindType.Down, typeof(String));
            /// <summary>画像ファイルL</summary>
            public static readonly ControlDefine IMAGE_L = new ControlDefine("imgL", "画像ファイルL", "imageL", ControlDefine.BindType.None, typeof(Byte[]));
            /// <summary>画像ファイルR</summary>
            public static readonly ControlDefine IMAGE_R = new ControlDefine("imgR", "画像ファイルR", "imageR", ControlDefine.BindType.None, typeof(Byte[]));
            ///<summary>光軸検査ステーションコード</summary>
            public static readonly ControlDefine OPT_AXIS_STATION_CD = new ControlDefine("txtOptAxisStationCd", "光軸検査ステーションコード", "optAxisStationCd", ControlDefine.BindType.Down, typeof(String));

        }
        #endregion

        #region プロパティ

        /// <summary>
        /// 表示中ページ(ベースフォーム)情報
        /// </summary>
        private BaseForm CurrentForm
        {
            get
            {
                return ((BaseForm)Page);
            }
        }

        /// <summary>
        /// 表示中ユーザコントロール情報
        /// </summary>
        private PageInfo.ST_PAGE_INFO CurrentUCInfo
        {
            get
            {
                return PageInfo.GetUCPageInfo(DetailKeyParam.ProductKind, DetailKeyParam.GroupCd, DetailKeyParam.ClassCd);
            }
        }

        /// <summary>
        /// (メイン)コントロール定義
        /// </summary>
        ControlDefine[] _mainControls = null;
        /// <summary>
        /// (メイン)コントロール定義アクセサ
        /// </summary>
        ControlDefine[] MainControls
        {
            get
            {
                if (true == ObjectUtils.IsNull(_mainControls))
                {
                    _mainControls = ControlUtils.GetControlDefineArray(typeof(LIST_MAIN));
                }
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
        public Defines.Interface.ST_DETAIL_PARAM DetailKeyParam
        {
            get
            {
                return _detailKeyParam;
            }
            set
            {
                _detailKeyParam = value;
            }
        }

        #endregion

        #region イベント
        /// <summary>
        /// ページロード
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            CurrentForm.RaiseEvent(DoPageLoad);
        }

        /// <summary>
        /// メインリスト行バインド
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lstMainListRB_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            ItemDataBoundMainRBList(sender, e);
        }
        protected void lstMainListLB_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            ItemDataBoundMainLBList(sender, e);
        }

        /// <summary>
        /// メインリスト選択行変更中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lstMainListLB_SelectedIndexChanging(object sender, ListViewSelectEventArgs e)
        {
            //処理なし
        }
        protected void lstMainListRB_SelectedIndexChanging(object sender, ListViewSelectEventArgs e)
        {
            //処理なし
        }


        /// <summary>
        /// メインリスト選択行変更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lstMainListLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            CurrentForm.RaiseEvent(SelectedIndexChangedMainLBList);
        }
        protected void lstMainListRB_SelectedIndexChanged(object sender, EventArgs e)
        {
            CurrentForm.RaiseEvent(SelectedIndexChangedMainRBList);
        }

        #endregion

        #region リストバインド

        /// <summary>
        /// 左リストビューデータバインド
        /// </summary>
        /// <param name="parameters"></param>
        private void ItemDataBoundMainLBList(params object[] parameters)
        {
            object sender = parameters[0];
            ListViewItemEventArgs e = (ListViewItemEventArgs)parameters[1];

            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                DataRow rowBind = ((DataRowView)e.Item.DataItem).Row;
                Dictionary<string, object> dicSetValues = new Dictionary<string, object>();
                CurrentForm.SetControlValues(e.Item, MainControls, rowBind, ref dicSetValues);

                //行クリックイベントセット
                HtmlTableRow trRow = (HtmlTableRow)e.Item.FindControl(LIST_MAIN.TR_ROW_DATA.controlId);
                Button selectBtn = (Button)e.Item.FindControl(LIST_MAIN.SELECT.controlId);
                trRow.Attributes[ControlUtils.ON_CLICK] = string.Format(MAIN_VIEW_SELECTED, e.Item.DataItemIndex, selectBtn.UniqueID);

                //ツールチップ設定
                ControlUtils.SetToolTip(e.Item);
            }
        }

        /// <summary>
        /// 右リストビューデータバインド
        /// </summary>
        /// <param name="parameters"></param>
        private void ItemDataBoundMainRBList(params object[] parameters)
        {
            object sender = parameters[0];
            ListViewItemEventArgs e = (ListViewItemEventArgs)parameters[1];

            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                DataRow rowBind = ((DataRowView)e.Item.DataItem).Row;
                Dictionary<string, object> dicSetValues = new Dictionary<string, object>();
                CurrentForm.SetControlValues(e.Item, MainControls, rowBind, ref dicSetValues);

                //行クリックイベントセット
                HtmlTableRow trRow = (HtmlTableRow)e.Item.FindControl(LIST_MAIN.TR_ROW_DATA.controlId);
                Button selectBtn = (Button)e.Item.FindControl(LIST_MAIN.SELECT.controlId);
                trRow.Attributes[ControlUtils.ON_CLICK] = string.Format(MAIN_VIEW_SELECTED, e.Item.DataItemIndex, selectBtn.UniqueID);

                //ツールチップ設定
                ControlUtils.SetToolTip(e.Item);
            }
        }

        /// <summary>
        /// メインリストバインド処理（左）
        /// </summary>
        /// <param name="parameters">イベントパラメータ</param>
        private void SelectedIndexChangedMainLBList()
        {

            int mainIndex = lstMainListLB.SelectedIndex;

            //選択行背景色変更解除
            foreach (ListViewDataItem li in lstMainListLB.Items)
            {
                HtmlTableRow trDeselectRow = (HtmlTableRow)li.FindControl(LIST_MAIN.TR_ROW_DATA.controlId);
                trDeselectRow.Attributes["class"] = trDeselectRow.Attributes["class"].Replace(" " + ResourcePath.CSS.ListSelectedRow, "");
            }

            foreach (ListViewDataItem li in lstMainListRB.Items)
            {
                HtmlTableRow trDeselectRow = (HtmlTableRow)li.FindControl(LIST_MAIN.TR_ROW_DATA.controlId);
                trDeselectRow.Attributes["class"] = trDeselectRow.Attributes["class"].Replace(" " + ResourcePath.CSS.ListSelectedRow, "");
            }

            //一覧項目選択済に色変更
            HtmlTableRow trSelectRow = (HtmlTableRow)lstMainListLB.Items[mainIndex].FindControl(LIST_MAIN.TR_ROW_DATA.controlId);
            trSelectRow.Attributes["class"] = trSelectRow.Attributes["class"] + " " + ResourcePath.CSS.ListSelectedRow;

            HtmlTableRow trSelectRowRB = (HtmlTableRow)lstMainListRB.Items[mainIndex].FindControl(LIST_MAIN.TR_ROW_DATA.controlId);
            trSelectRowRB.Attributes["class"] = trSelectRow.Attributes["class"] + " " + ResourcePath.CSS.ListSelectedRow;

            SelectedIndexChangedMainList(mainIndex);
        }

        /// <summary>
        /// メインリストバインド処理（右）
        /// </summary>
        /// <param name="parameters">イベントパラメータ</param>
        private void SelectedIndexChangedMainRBList()
        {

            int mainIndex = lstMainListRB.SelectedIndex;

            //選択行背景色変更解除
            foreach (ListViewDataItem li in lstMainListLB.Items)
            {
                HtmlTableRow trDeselectRow = (HtmlTableRow)li.FindControl(LIST_MAIN.TR_ROW_DATA.controlId);
                trDeselectRow.Attributes["class"] = trDeselectRow.Attributes["class"].Replace(" " + ResourcePath.CSS.ListSelectedRow, "");
            }

            foreach (ListViewDataItem li in lstMainListRB.Items)
            {
                HtmlTableRow trDeselectRow = (HtmlTableRow)li.FindControl(LIST_MAIN.TR_ROW_DATA.controlId);
                trDeselectRow.Attributes["class"] = trDeselectRow.Attributes["class"].Replace(" " + ResourcePath.CSS.ListSelectedRow, "");
            }

            //一覧項目選択済に色変更
            HtmlTableRow trSelectRow = (HtmlTableRow)lstMainListLB.Items[mainIndex].FindControl(LIST_MAIN.TR_ROW_DATA.controlId);
            trSelectRow.Attributes["class"] = trSelectRow.Attributes["class"] + " " + ResourcePath.CSS.ListSelectedRow;

            HtmlTableRow trSelectRowRB = (HtmlTableRow)lstMainListRB.Items[mainIndex].FindControl(LIST_MAIN.TR_ROW_DATA.controlId);
            trSelectRowRB.Attributes["class"] = trSelectRow.Attributes["class"] + " " + ResourcePath.CSS.ListSelectedRow;

            SelectedIndexChangedMainList(mainIndex);
        }

        /// <summary>
        /// リスト行選択時画像表示処理
        /// </summary>
        /// <param name="parameters">イベントパラメータ</param>
        private void SelectedIndexChangedMainList(int paramIndex)
        {

            // セッションデータ取得
            Business.DetailViewBusiness.ResultSetMulti res = new Business.DetailViewBusiness.ResultSetMulti();
            Dictionary<string, object> dicPageControlInfo = CurrentForm.SessionManager.GetPageControlInfoHandler(CurrentForm.Token).GetPageControlInfo(MANAGE_ID);
            if (true == dicPageControlInfo.ContainsKey(SESSION_PAGE_INFO_DB_KEY))
            {
                res = (Business.DetailViewBusiness.ResultSetMulti)dicPageControlInfo[SESSION_PAGE_INFO_DB_KEY];
            }

            if (false == res.MainTable.Rows[paramIndex]["imageLSize"].ToString().Equals("0"))
            {
                // 画像データL
                string urlMainAreaTop = ImageView.GetImageViewUrl(this, CurrentForm.Token, MANAGE_ID, paramIndex, 0, 0, TRC_W_PWT_ProductView.Session.ImageInfoSessionHandler.ContentType.Bitmap, false);
                imgMainArea.ImageUrl = urlMainAreaTop;
            }
            else
            {
                imgMainArea.ImageUrl = "";
            }

            if (false == res.MainTable.Rows[paramIndex]["imageRSize"].ToString().Equals("0"))
            {
                string urlMainAreaTop2 = ImageView.GetImageViewUrl(this, CurrentForm.Token, MANAGE_ID + "_2", paramIndex, 0, 0, TRC_W_PWT_ProductView.Session.ImageInfoSessionHandler.ContentType.Bitmap, false);
                imgMainArea2.ImageUrl = urlMainAreaTop2;
            }
            else
            {
                imgMainArea2.ImageUrl = "";
            }
        }
        #endregion

        #region イベントメソッド

        #region ページイベント
        /// <summary>
        /// ページロード処理
        /// </summary>
        private void DoPageLoad()
        {
        }

        /// <summary>
        /// 初期化処理
        /// </summary>
        public void Initialize()
        {
            //検索結果取得
            Business.DetailViewBusiness.ResultSetMulti res = new Business.DetailViewBusiness.ResultSetMulti();

            try
            {
                //検査情報ヘッダ取得
                res = Business.DetailViewBusiness.SelectOptaxisDetail(DetailKeyParam.ProductModelCd, DetailKeyParam.Serial);

            }
            catch (KTFramework.Dao.DataAccessException ex)
            {
                logger.Exception(ex);
                CurrentForm.WriteApplicationMessage(MsgManager.MESSAGE_ERR_80010);
                return;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                CurrentForm.WriteApplicationMessage(MsgManager.MESSAGE_ERR_80010);
                return;
            }
            finally
            {
            }

            //取得データをセッションに格納
            Dictionary<string, object> dicPageControlInfo = new Dictionary<string, object>();
            dicPageControlInfo.Add(SESSION_PAGE_INFO_DB_KEY, res);
            CurrentForm.SessionManager.GetPageControlInfoHandler(CurrentForm.Token).SetPageControlInfo(MANAGE_ID, dicPageControlInfo);

            if (0 == res.MainTable.Rows.Count)
            {
                //検索結果0件
                CurrentForm.WriteApplicationMessage(MsgManager.MESSAGE_WRN_62010, CurrentUCInfo.title);
                return;
            }

            InitializeValues(res);
        }

        /// <summary>
        /// 初期値設定
        /// </summary>
        private void InitializeValues(Business.DetailViewBusiness.ResultSetMulti res)
        {

            //メインリストバインド
            lstMainListLB.DataSource = res.MainTable;
            lstMainListLB.DataBind();
            lstMainListLB.SelectedIndex = 0;

            lstMainListRB.DataSource = res.MainTable;
            lstMainListRB.DataBind();
            lstMainListRB.SelectedIndex = 0;

            // 画像バインド
            Dictionary<string, byte[]> dicImagesL = new Dictionary<string, byte[]>();
            Dictionary<string, byte[]> dicImagesR = new Dictionary<string, byte[]>();

            for (int loopImg = 0; loopImg < res.MainTable.Rows.Count; loopImg++)
            {
                byte[] byteImageL = new byte[0];
                byte[] byteImageR = new byte[0];

                if (ObjectUtils.IsNotNull(res.MainTable.Rows[loopImg][LIST_MAIN.IMAGE_L.bindField]))
                {
                    byteImageL = (byte[])res.MainTable.Rows[loopImg][LIST_MAIN.IMAGE_L.bindField];
                }

                if (ObjectUtils.IsNotNull(res.MainTable.Rows[loopImg][LIST_MAIN.IMAGE_R.bindField]))
                {
                    byteImageR = (byte[])res.MainTable.Rows[loopImg][LIST_MAIN.IMAGE_R.bindField];
                }

                dicImagesL.Add(loopImg.ToString(), byteImageL);
                dicImagesR.Add(loopImg.ToString(), byteImageR);
            }

            CurrentForm.SessionManager.GetImageInfoHandler(CurrentForm.Token).SetImages(MANAGE_ID, dicImagesL);
            CurrentForm.SessionManager.GetImageInfoHandler(CurrentForm.Token).SetImages(MANAGE_ID + "_2", dicImagesR);

            SelectedIndexChangedMainList(0);

            //選択行背景色変更解除
            foreach (ListViewDataItem li in lstMainListLB.Items)
            {
                HtmlTableRow trDeselectRow = (HtmlTableRow)li.FindControl(LIST_MAIN.TR_ROW_DATA.controlId);
                trDeselectRow.Attributes["class"] = trDeselectRow.Attributes["class"].Replace(" " + ResourcePath.CSS.ListSelectedRow, "");
            }

            foreach (ListViewDataItem li in lstMainListRB.Items)
            {
                HtmlTableRow trDeselectRow = (HtmlTableRow)li.FindControl(LIST_MAIN.TR_ROW_DATA.controlId);
                trDeselectRow.Attributes["class"] = trDeselectRow.Attributes["class"].Replace(" " + ResourcePath.CSS.ListSelectedRow, "");
            }

            //一覧項目選択済に色変更
            HtmlTableRow trSelectRow = (HtmlTableRow)lstMainListLB.Items[lstMainListLB.SelectedIndex].FindControl(LIST_MAIN.TR_ROW_DATA.controlId);
            trSelectRow.Attributes["class"] = trSelectRow.Attributes["class"] + " " + ResourcePath.CSS.ListSelectedRow;

            HtmlTableRow trSelectRowRB = (HtmlTableRow)lstMainListRB.Items[lstMainListLB.SelectedIndex].FindControl(LIST_MAIN.TR_ROW_DATA.controlId);
            trSelectRowRB.Attributes["class"] = trSelectRow.Attributes["class"] + " " + ResourcePath.CSS.ListSelectedRow;
        }


        #endregion

        #endregion
    }
}