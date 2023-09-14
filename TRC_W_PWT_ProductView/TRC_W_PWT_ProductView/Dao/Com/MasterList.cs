using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Data;
using KTFramework.Common;
using TRC_W_PWT_ProductView.Common;
using TRC_W_PWT_ProductView.Defines.ListDefine;

namespace TRC_W_PWT_ProductView.Dao.Com {

    /// <summary>
    /// 静的DBリスト項目
    /// </summary>
    public static class MasterList {

        /// <summary>
        /// 全リスト取得
        /// </summary>
        /// <remarks>
        /// アプリケーション開始時に全リスト取得する為に使用します
        /// </remarks>
        public static void GetAllMasterList() {
            //製品区分リスト
            ListItem[] liArr = ProductKindList;
            //区分グループリスト
            DataTable tblClassGrp = ClassGroupTable;
            //ステーションリスト
            DataTable tblStation = MsStationTable;
        }

        #region 製品区分リスト

        /// <summary>
        /// 製品区分リストアクセサー
        /// </summary>
        public static ListItem[] ProductKindList => ProductKindDao.SelectByList();
        #endregion

        #region 区分グループリスト

        /// <summary>
        /// 区分グループリストインスタンス
        /// </summary>
        private static DataTable _classGroupTable;
        /// <summary>
        /// 区分グループリストアクセサー
        /// </summary>
        public static DataTable ClassGroupTable
        {
            get
            {
                if ( ObjectUtils.IsNull( _classGroupTable ) ) {
                    _classGroupTable = ClassGroupDao.SelectAll();
                }

                return _classGroupTable;
            }
        }

        /// <summary>
        /// 工程区分/部品区分のリスト項目を取得します
        /// </summary>
        /// <param name="productKind">製品種別</param>
        /// <param name="groupCd">グループコード</param>
        /// <param name="addBlank">空白行追加有無</param>
        /// <returns>工程区分/部品区分のリストアイテム配列</returns>
        public static ListItem[] GetClassItem( string productKind, string groupCd, bool addBlank = true ) {

            DataRow[] rowClassArr = (
                from row in ClassGroupTable.AsEnumerable()
                let productKindField = row.Field<string>( "PRODUCT_KIND_CD" )
                let groupCdField = row.Field<string>( "GROUP_CD" )
                let displayOrderField = row.Field<int>( "DISPLAY_ORDER" )
                where productKindField == productKind
                && groupCdField == groupCd
                orderby displayOrderField
                select row
            ).ToArray();

            return Common.ControlUtils.GetListItems( rowClassArr, "CLASS_NM", "CLASS_CD", addBlank );

        }


        /// <summary>
        /// 工程区分/部品区分のリスト項目を取得します(トラクタ選択時の部品表示対応)
        /// </summary>
        /// <param name="productKind">製品種別</param>
        /// <param name="groupCd">グループコード</param>
        /// <param name="addBlank">空白行追加有無</param>
        /// <returns>工程区分/部品区分のリストアイテム配列</returns>
        public static ListItem[] GetTractorItem( string productKind, string groupCd, bool addBlank = true ) {

            DataRow[] rowClassArr = (
                from row in ClassGroupTable.AsEnumerable()
                let productKindField = row.Field<string>( "PRODUCT_KIND_CD" )
                let groupCdField = row.Field<string>( "GROUP_CD" )
                let classCd = row.Field<string>( "CLASS_CD" )
                let displayOrderField = row.Field<int>( "DISPLAY_ORDER" )
                where ( groupCdField == groupCd
                && ( classCd == "T1" || classCd == "T2" || classCd == "T3" || classCd == "T4" || classCd == "T5" || classCd == "T6" || classCd == "05" || classCd == "12" || classCd == "13" ) ) ||
                     ( productKindField == productKind && classCd.StartsWith( PartsKind.PARTS_CD_TRACTOR_PREFIX_COREPARTS ) )
                orderby productKindField descending,
                        displayOrderField
                select row
            ).ToArray();

            return Common.ControlUtils.GetListItems( rowClassArr, "CLASS_NM", "CLASS_CD", addBlank );

        }

        #endregion

        #region ステーションリスト

        /// <summary>
        /// ステーションリストインスタンス
        /// </summary>
        private static DataTable _msStationTable;
        /// <summary>
        /// ステーションリストアクセサー
        /// </summary>
        public static DataTable MsStationTable
        {
            get
            {
                if ( ObjectUtils.IsNull( _msStationTable ) ) {
                    _msStationTable = MsStationDao.Select();
                }

                return _msStationTable;
            }
        }

        /// <summary>
        /// ステーションコードからステーション名を取得します
        /// </summary>
        /// <param name="stationCd">ステーションコード</param>
        /// <returns>ステーション名</returns>
        public static string GetStationNm( string stationCd ) {

            string result = "";

            DataRow rowStation = (
                from row in MsStationTable.AsEnumerable()
                let stationCdField = row.Field<string>( "stationCd" )
                where stationCdField == stationCd
                select row
            ).FirstOrDefault();

            if ( true == ObjectUtils.IsNotNull( rowStation ) ) {
                result = rowStation["stationNm"].ToString();
            }

            return result;

        }

        /// <summary>
        /// ステーションコードからステーション名を割り当てます
        /// </summary>
        /// <param name="tblParts">stationCd,stationNm列を保有するDataTable</param>
        /// <returns>stationNm列指定済みDataTable</returns>
        public static DataTable SetStationNm( DataTable tblData ) {

            if ( null == tblData ) {
                return tblData;
            }

            if ( 0 > tblData.Columns.IndexOf( "stationCd" )
                || 0 > tblData.Columns.IndexOf( "stationNm" ) ) {
                return tblData;
            }

            //ステーション名を割当(stationNmフィールドへセット)
            string lastStationCd = "";  //大体のステーションは同一
            string lastStationNm = "";
            foreach ( DataRow row in tblData.Rows ) {
                string stationCd = StringUtils.ToString( row["stationCd"] );
                string stationNm = "";
                if ( lastStationCd == stationCd ) {
                    //前回保持したステーション名を割り当てる
                    stationNm = lastStationNm;
                } else {
                    stationNm = MasterList.GetStationNm( stationCd );
                    lastStationNm = stationNm;
                }
                if ( true == StringUtils.IsBlank( stationNm ) ) {
                    stationNm = stationCd;  //ステーション名取得失敗…コードを表示
                }
                row["stationNm"] = stationNm;
            }
            tblData.AcceptChanges();

            return tblData;
        }

        #endregion

        #region エリアリスト
        /// <summary>
        /// エリアリストアクセサ
        /// </summary>
        public static ListItem[] AreaNameList => ControlUtils.GetListItems( TmGeServerPrinterDao.SelectAreaList(), TmGeServerPrinterDao.COLNAME_LOCATION, TmGeServerPrinterDao.COLNAME_LOCATION, true );
        #endregion


        #region ステーションリスト(ステーション通過実績検索用）
        // 2021/06/03 大沼　追加開始
        /// <summary>
        /// ステーションリストアクセサ
        /// </summary>
        public static ListItem[] StationList => ControlUtils.GetListItems( ItemListDao.SelectJissekiStationList(), ItemListDao.COLNAME_TEXT, ItemListDao.COLNAME_VALUE );
        // 2021/06/03 大沼　追加終了
        #endregion

        #region 作業指示月度リスト(順序情報検索用）
        // 2021/06/08 大沼　追加開始
        /// <summary>
        /// ステーションリストアクセサ
        /// </summary>
        public static ListItem[] SagyoYMList => ControlUtils.GetListItems( ItemListDao.SelectSagyoGatsudoList(), ItemListDao.COLNAME_TEXT, ItemListDao.COLNAME_VALUE );
        // 2021/06/08 大沼　追加終了
        #endregion

        #region ステーションリスト(ステーション是正処置入力製品検索画面）
        /// <summary>
        /// ステーションリストアクセサ sfadd
        /// </summary>
        public static ListItem[] ServiceStationList => ControlUtils.GetListItems( AiImageCheckDao.SelectStationList(), AiImageCheckDao.COLNAME_TEXT, AiImageCheckDao.COLNAME_VALUE );
        #endregion

        #region ラインリスト(ライン是正処置入力製品検索画面）
        /// <summary>
        /// ラインリストアクセサ sfadd
        /// </summary>
        public static ListItem[] ServiceLineList => ControlUtils.GetListItems( AiImageCheckDao.SelectLineList(), AiImageCheckDao.COLNAME_TEXT, AiImageCheckDao.COLNAME_VALUE );
        #endregion

    }
}