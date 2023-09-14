using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using KTFramework.Common;
using TRC_W_PWT_ProductView.Common;
using TRC_W_PWT_ProductView.Dao.Com;
using TRC_W_PWT_ProductView.Defines;
using TRC_W_PWT_ProductView.Defines.TypeDefine;
using TRC_W_PWT_ProductView.UI.Pages.Kanban;

namespace TRC_W_PWT_ProductView.Business {
    /// <summary>
    /// 電子かんばんビジネスクラス
    /// </summary>
    public class KanbanBusiness {
        // ロガー定義
        private static readonly Logger Logger = new Logger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

        #region 一覧情報格納構造体
        /// <summary>
        /// 一覧情報格納構造体
        /// </summary>
        [Serializable]
        public struct ResultSet {
            /// <summary>メイン情報</summary>
            public DataTable ListTable { get; set; }
            /// <summary>サブ情報</summary>
            public Msg Message { get; set; }
        }
        #endregion

        #region 定数定義
        /// <summary>
        /// 状況名称・コード
        /// </summary>
        private const string StatusComp = "完了";
        private const string StatusNotComp = "完了(未完あり)";
        private const string StatusTouched = "着手中";
        private const string StatusUnTouched = "未着手";
        private const string StatusIdComp = "9";
        private const string StatusIdNotComp = "8";
        private const string StatusIdTouched = "2";
        private const string StatusIdUnTouched0 = "0";
        private const string StatusIdUnTouched1 = "1";
        #endregion

        /// <summary>
        /// ピッキング状況画面：検索処理
        /// </summary>
        /// <param name="condition">検索条件</param>
        /// <param name="columnsDef">列定義</param>
        /// <param name="maxRecordCount">最大検索件数</param>
        /// <returns>処理結果</returns>
        public static ResultSet SearchPickingStatus( Dictionary<string, object> condition, GridViewDefine[] columnsDef, int maxRecordCount ) {
            var result = new ResultSet();

            // 要求日時From
            var sendDateTimeFrom = GetSendTime( condition, KanbanPickingStatusView.CONDITION.SEND_DATE_FROM.bindField, KanbanPickingStatusView.CONDITION.SEND_TIME_FROM.bindField, false );
            // 要求日時To
            var sendDateTimeTo = GetSendTime( condition, KanbanPickingStatusView.CONDITION.SEND_DATE_TO.bindField, KanbanPickingStatusView.CONDITION.SEND_TIME_TO.bindField, true );
            // 完了日時From
            var endDateTimeFrom = GetEndTime( condition, KanbanPickingStatusView.CONDITION.END_DATE_FROM.bindField, KanbanPickingStatusView.CONDITION.END_TIME_FROM.bindField, false );
            // 完了日時To
            var endDateTimeTo = GetEndTime( condition, KanbanPickingStatusView.CONDITION.END_DATE_TO.bindField, KanbanPickingStatusView.CONDITION.END_TIME_TO.bindField, true );
            // 状況
            var statusVal = DataUtils.GetDictionaryStringVal( condition, KanbanPickingStatusView.CONDITION.STATUS.bindField );
            List<string> status = new List<string>();
            if ( StatusComp == statusVal ) {
                status.Add( StatusIdComp );
            } else if ( StatusNotComp == statusVal ) {
                status.Add( StatusIdNotComp );
            } else if ( StatusTouched == statusVal ) {
                status.Add( StatusIdTouched );
            } else if ( StatusUnTouched == statusVal ) {
                status.Add( StatusIdUnTouched0 );
                status.Add( StatusIdUnTouched1 );
            } else {
                status = null;
            }
            // エリア
            var area = DataUtils.GetDictionaryStringVal( condition, KanbanPickingStatusView.CONDITION.AREA_NM.bindField );
            // ピッキング者(ID)
            var userId = DataUtils.GetDictionaryStringVal( condition, KanbanPickingStatusView.CONDITION.PICKING_USER_ID.bindField );
            // ピッキングNo
            var pickingListNo = DataUtils.GetDictionaryStringVal( condition, KanbanPickingStatusView.CONDITION.PICKING_LIST_NO.bindField );
            // 品番
            var partsNumber = DataUtils.GetDictionaryStringVal( condition, KanbanPickingStatusView.CONDITION.PARTS_NUMBER.bindField );

            // ピッキング状況検索を実行する
            result.ListTable = PickingSummaryDao.SelectPickingInfo( sendDateTimeFrom, sendDateTimeTo, endDateTimeFrom, endDateTimeTo, status, area, userId, pickingListNo, partsNumber );
            // 列定義から列名のキャプションを設定する
            columnsDef.ToList().ForEach( cd => {
                result.ListTable.Columns[cd.bindField].Caption = cd.headerText;

            } );
            // エラーメッセージを初期化する
            result.Message = null;
            if ( result.ListTable.Rows.Count > maxRecordCount ) {
                // 検索結果が最大検索件数を超えた場合、エラーメッセージを設定
                result.Message = new Msg( MsgManager.MESSAGE_WRN_64010, maxRecordCount );
            } else if ( result.ListTable.Rows.Count == 0 ) {
                // 検索結果が0件の場合、エラーメッセージを設定
                result.Message = new Msg( MsgManager.MESSAGE_WRN_61010 );
            }
            return result;
        }

        /// <summary>
        /// ピッキング明細画面：検索処理
        /// </summary>
        /// <param name="condition">検索条件</param>
        /// <param name="columnsDef">列定義</param>
        /// <returns>処理結果</returns>
        public static ResultSet SearchPickingDetail( Dictionary<string, object> condition, GridViewDefine[] columnsDef ) {
            var result = new ResultSet();

            // ピッキングNo
            var pickingListNo = DataUtils.GetDictionaryStringVal( condition, KanbanPickingDetailView.CONDITION.PICKING_LIST_NO.bindField );

            // ピッキング明細検索を実行する
            result.ListTable = PickingDetailDao.SelectDetailInfo( pickingListNo );
            // 列定義から列名のキャプションを設定する
            columnsDef.ToList().ForEach( cd => {
                result.ListTable.Columns[cd.bindField].Caption = cd.headerText;

            } );
            // エラーメッセージを初期化する
            result.Message = null;
            if ( result.ListTable.Rows.Count == 0 ) {
                // 検索結果が0件の場合、エラーメッセージを設定
                result.Message = new Msg( MsgManager.MESSAGE_WRN_61010 );
            }
            return result;
        }

        /// <summary>
        /// 未完了ピッキング画面：検索処理
        /// </summary>
        /// <param name="condition">検索条件</param>
        /// <param name="columnsDef">列定義</param>
        /// <param name="maxRecordCount">最大検索件数</param>
        /// <returns>処理結果</returns>
        public static ResultSet SearchIncompletePicking( Dictionary<string, object> condition, GridViewDefine[] columnsDef, int maxRecordCount ) {
            var result = new ResultSet();
            // 要求日時From
            var sendDateTimeFrom = GetSendTime( condition, KanbanShortageView.CONDITION.SEND_DATE_FROM.bindField, KanbanShortageView.CONDITION.SEND_TIME_FROM.bindField, false );
            // 要求日時To
            var sendDateTimeTo = GetSendTime( condition, KanbanShortageView.CONDITION.SEND_DATE_TO.bindField, KanbanShortageView.CONDITION.SEND_TIME_TO.bindField, true );
            // エリア
            var area = DataUtils.GetDictionaryStringVal( condition, KanbanShortageView.CONDITION.AREA_NM.bindField );
            // 材管ロケ大番地
            var zaikanPrimaryLocation = DataUtils.GetDictionaryStringVal( condition, KanbanShortageView.CONDITION.ZAIKAN_PRIMARY_LOCATION.bindField );
            // 材管ロケ中番地
            var zaikanSecondaryLocation = DataUtils.GetDictionaryStringVal( condition, KanbanShortageView.CONDITION.ZAIKAN_SECONDARY_LOCATION.bindField );
            // 材管ロケ小番地
            var zaikanTertiaryLocation = DataUtils.GetDictionaryStringVal( condition, KanbanShortageView.CONDITION.ZAIKAN_TERTIARY_LOCATION.bindField );
            // ピッキング者(ID)
            var userId = DataUtils.GetDictionaryStringVal( condition, KanbanShortageView.CONDITION.PICKING_USER_ID.bindField );
            // 品番
            var partsNumber = DataUtils.GetDictionaryStringVal( condition, KanbanShortageView.CONDITION.PARTS_NUMBER.bindField );
            // ピッキングNo
            var pickingListNo = DataUtils.GetDictionaryStringVal( condition, KanbanShortageView.CONDITION.PICKING_LIST_NO.bindField );
            // 未完了ピッキング検索を実行する
            result.ListTable = PickingSummaryDao.SelectIncompletePicking( sendDateTimeFrom, sendDateTimeTo, area, zaikanPrimaryLocation, zaikanSecondaryLocation, zaikanTertiaryLocation, userId, partsNumber, pickingListNo );
            // 列定義から列名のキャプションを設定する
            columnsDef.ToList().ForEach( cd => {
                result.ListTable.Columns[cd.bindField].Caption = cd.headerText;

            } );
            // エラーメッセージを初期化する
            result.Message = null;
            if ( result.ListTable.Rows.Count > maxRecordCount ) {
                // 検索結果が最大検索件数を超えた場合、エラーメッセージを設定し、結果テーブルをクリア
                result.Message = new Msg( MsgManager.MESSAGE_WRN_64010, maxRecordCount );
                result.ListTable.Clear();
            } else if ( result.ListTable.Rows.Count == 0 ) {
                // 検索結果が0件の場合、エラーメッセージを設定
                result.Message = new Msg( MsgManager.MESSAGE_WRN_61010 );
            }
            return result;
        }

        /// <summary>
        /// 要求日付、要求時刻から要求日時を取得する
        /// </summary>
        /// <param name="condition">検索条件</param>
        /// <param name="dateBindField">検索条件：要求日付のバインドフィールド</param>
        /// <param name="timeBindField">検索条件：要求時刻のバインドフィールド</param>
        /// <param name="isDayTo">要求日付Toかどうか（Toの場合かつ時刻未入力の場合23:59:59にする）</param>
        /// <returns>要求日時、要求日付が未入力の場合null、要求時刻が未入力の場合要求日付の00:00:00</returns>
        public static DateTime? GetSendTime( Dictionary<string, object> condition, string dateBindField, string timeBindField, bool isDayTo ) {
            // 要求日時
            DateTime? sendDateTime = null;
            // 要求日付
            DateTime sendDate = new DateTime();
            if ( DateTime.TryParse( DataUtils.GetDictionaryStringVal( condition, dateBindField ), out sendDate ) ) {
                // 要求日付が入力されている場合、要求時刻を取得する
                var sendTime = DataUtils.GetDictionaryObjectVal( condition, timeBindField );
                if ( sendTime != null ) {
                    // 要求時刻が入力されている場合、要求日時を要求日付 + 要求時刻にする
                    sendDateTime = sendDate + (TimeSpan)sendTime;
                } else {
                    // 要求時刻が入力されていない場合、要求日時を要求日付 + 固定の時刻にする
                    sendDateTime = sendDate + (TimeSpan)( ( isDayTo ) ? new TimeSpan( 23, 59, 59 ) : new TimeSpan( 0, 0, 0 ) );
                }
            }
            return sendDateTime;
        }

        /// <summary>
        /// 完了日付、完了時刻から完了日時を取得する
        /// </summary>
        /// <param name="condition">検索条件</param>
        /// <param name="dateBindField">検索条件：完了日付のバインドフィールド</param>
        /// <param name="timeBindField">検索条件：完了時刻のバインドフィールド</param>
        /// <param name="isDayTo">完了日付Toかどうか（Toの場合かつ時刻未入力の場合23:59:59にする）</param>
        /// <returns>完了日時、完了日付が未入力の場合null、完了時刻が未入力の場合完了日付の00:00:00</returns>
        public static DateTime? GetEndTime( Dictionary<string, object> condition, string dateBindField, string timeBindField, bool isDayTo ) {
            // 完了日時
            DateTime? endDateTime = null;
            // 完了日付
            DateTime endDate = new DateTime();
            if ( DateTime.TryParse( DataUtils.GetDictionaryStringVal( condition, dateBindField ), out endDate ) ) {
                // 完了日付が入力されている場合、完了時刻を取得する
                var endTime = DataUtils.GetDictionaryObjectVal( condition, timeBindField );
                if ( endTime != null ) {
                    // 完了時刻が入力されている場合、完了日時を完了日付 + 完了時刻にする
                    endDateTime = endDate + (TimeSpan)endTime;
                } else {
                    // 完了時刻が入力されていない場合、完了日時を完了日付 + 固定の時刻にする
                    endDateTime = endDate + (TimeSpan)( ( isDayTo ) ? new TimeSpan( 23, 59, 59 ) : new TimeSpan( 0, 0, 0 ) );
                }
            }
            return endDateTime;
        }

        /// <summary>
        /// 品番のフォーマット
        /// </summary>
        /// <param name="rawPartsNumber">未加工の品番</param>
        /// <returns>品番が10文字の場合5文字-4文字-1文字の形式、そうでなければ元の値</returns>
        public static string GetFormattedPartsNumber( string rawPartsNumber ) {
            // 未加工の品番から前後の空白を削除する
            var trimPartsNumber = rawPartsNumber.Trim();
            if ( trimPartsNumber.Length != 10 ) {
                // 空白を削除した品番が10文字以外の場合、元の値を返す
                return rawPartsNumber;
            } else {
                // 10文字の場合、5文字-4文字-1文字の形式にして返す
                return $"{trimPartsNumber.Substring( 0, 5 )}-{trimPartsNumber.Substring( 5, 4 )}-{trimPartsNumber.Substring( 9, 1 )}";
            }
        }
    }
}