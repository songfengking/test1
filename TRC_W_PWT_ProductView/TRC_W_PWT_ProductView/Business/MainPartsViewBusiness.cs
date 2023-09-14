using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using KTFramework.Common;
using KTFramework.Dao;
using TRC_W_PWT_ProductView.Defines.ListDefine;
using TRC_W_PWT_ProductView.Defines.TypeDefine;
using TRC_W_PWT_ProductView.Common;
using TRC_W_PWT_ProductView.Dao.Com;
using TRC_W_PWT_ProductView.Dao.Products;
using TRC_W_PWT_ProductView.Dao.Parts;
using TRC_W_PWT_ProductView.Dao.Process;
using TRC_W_PWT_ProductView.UI.Pages;
using TRC_W_PWT_ProductView.Defines;


namespace TRC_W_PWT_ProductView.Business {
    /// <summary>
    /// 製品一覧検索ビジネスクラス
    /// </summary>
    public class MainPartsViewBusiness {
        //ロガー定義
        private static readonly Logger _logger = new Logger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

        private const double SQL_MIXED_IN_PROC_ENTITY_MAX = 100;     //SQL 複合条件IN句で指定できる最大要素数(1000over指定可能)
        private const int MODEL_CD_LIST_MAX = 500;                   //型式名からの逆引き検索時に有効とする最大型式コード数

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

        #region 検索メイン
        /// <summary>
        /// 一覧検索処理
        /// </summary>
        /// <param name="condition">画面検索条件</param>
        /// <returns>一覧表示DataTable</returns>
        public static ResultSet Search( Dictionary<string, object> condition, GridViewDefine[] columns, int maxRecordCount ) {
            ResultSet result = new ResultSet();
            string partsTarget = DataUtils.GetDictionaryStringVal( condition, MainPartsView.CONDITION.PARTS_SERACH_TARGET.bindField );           //部品検索対象
            string partsKind = DataUtils.GetDictionaryStringVal( condition, MainPartsView.CONDITION.PARTS_KIND.bindField );                      //部品種別
            string processCd = DataUtils.GetDictionaryStringVal( condition, MainPartsView.CONDITION.PROCESS_CD.bindField );                      //工程区分

            ////検索条件から、検索母体(製品 or 部品)を判定
            //bool searchProductsFirst = IsSearchProductsFirst( condition );

            int mainTableMaxCount = maxRecordCount;
            int subTableMaxCount = maxRecordCount;
            //if ( true == searchProductsFirst && ( false == StringUtils.IsBlank( partsCd ) || false == StringUtils.IsBlank( processCd ) ) ) {
            //    //製品検索を先に検索する場合で、部品または工程を結合する時は、製品情報の取得件数は制限しない
            //    mainTableMaxCount = Int32.MaxValue;
            //}

            ////検索条件から、製品/部品の検索優先順に従い検索を行い、テーブルを結合する
            DataTable tblMain = null;
            DataTable tblSub = null;
            DataTable tblShipInfo = null;

            if ( partsTarget == PartsSearchTarget.Atu ) {

                tblMain = SelectAtuList( condition, mainTableMaxCount );
                if ( null != tblMain ) {
                    _logger.Info( "部品検索(ATU)結果 …{0}件", tblMain.Rows.Count );
                }

                //出荷情報取得
                tblShipInfo = SelectShipInfo(tblMain, mainTableMaxCount);
                //出荷情報テーブルを結合
                tblMain = GetLeftJoinTable(ControlUtils.GetGridViewDefineArray(typeof(MainPartsView.GRID_ATU)), tblMain, tblShipInfo);

                if ( processCd != "" ) {

                    tblSub = SelectAtuSerialList( condition, tblMain, subTableMaxCount );
                    if ( null != tblSub ) {
                        _logger.Info( "製品検索(副)結果 …{0}件", tblSub.Rows.Count );
                    }

                    result.ListTable = GetJoinTable( columns, tblMain, tblSub );

                } else {

                    result.ListTable = GetJoinTable( columns, tblMain, tblSub );

                }
            }

            //メッセージ設定
            result.Message = null;
            if ( null == result.ListTable || 0 == result.ListTable.Rows.Count ) {
                //検索結果0件
                result.Message = new Msg( MsgManager.MESSAGE_WRN_61010 );
                //} else if ( ( null != tblMain && mainTableMaxCount <= tblMain.Rows.Count ) || ( null != tblSub && subTableMaxCount <= tblSub.Rows.Count ) ) {
            } else if ( ( null != tblMain && mainTableMaxCount <= tblMain.Rows.Count ) ) {
                //検索件数が上限を上回っている場合には警告メッセージをセット
                result.Message = new Msg( MsgManager.MESSAGE_WRN_61020 );
            }

            return result;
        }


        #region 製品一覧検索
        /// <summary>
        /// 部品一覧検索処理(ATU主)
        /// </summary>
        /// <param name="condition">画面検索条件</param>
        /// <param name="maxRecordCount">最大取得件数</param>
        /// <returns>製品DataTable</returns>
        private static DataTable SelectAtuList( Dictionary<string, object> condition, int maxRecordCount ) {
            AtuDao.SearchParameter param = new AtuDao.SearchParameter();

            //画面パラメータの取得
            string modelType = DataUtils.GetDictionaryStringVal( condition, MainPartsView.CONDITION.PARTS_KIND.bindField );                         //部品種別
            string modelCd = DataUtils.GetModelCd( DataUtils.GetDictionaryStringVal( condition, MainPartsView.CONDITION.MODEL_CD.bindField ) );     //型式コード
            string modelNm = DataUtils.GetDictionaryStringVal( condition, MainPartsView.CONDITION.MODEL_NM.bindField );                             //型式名
            string serial = DataUtils.GetDictionaryStringVal( condition, MainPartsView.CONDITION.SERIAL.bindField );                                //機番
            string idno = DataUtils.GetDictionaryStringVal( condition, MainPartsView.CONDITION.IDNO.bindField );                                    //IDNO
            string partsNum = DataUtils.GetDictionaryStringVal( condition, MainPartsView.CONDITION.PARTS_NUM.bindField );                           //品番
            string dateKindCd = DataUtils.GetDictionaryStringVal( condition, MainPartsView.CONDITION.DATE_KIND_CD.bindField );                      //日付区分
            DateTime dtFrom = DataUtils.GetDictionaryDateVal( condition, MainPartsView.CONDITION.DATE_FROM.bindField );                             //日付(FROM)
            DateTime dtTo = DataUtils.GetDictionaryDateVal( condition, MainPartsView.CONDITION.DATE_TO.bindField );                                 //日付(TO)

            _logger.Info( "検索条件--->部品種別={0} 型式コード={1} 型式名={2} 機番={3} IDNO={4} 品番={5} 日付区分={6} 日付={7}～{8}",
                modelType, modelCd, modelNm, serial, idno, partsNum, dateKindCd, dtFrom, dtTo );

            //************************************************************************************
            //部品情報設定
            //************************************************************************************

            if ( modelType == PartsSearchKind.Dpf ) {
                param.paramModelType = modelType;        //機種区分
            } else if ( modelType == PartsSearchKind.Doc ) {
                param.paramModelType = modelType;        //機種区分
            } else {
                param.paramModelType = modelType;        //機種区分
            }

            param.paramModelCd = modelCd;            //ATU型式コード
            param.paramModelNm = modelNm;            //ATU型式名
            param.paramSerial = serial;              //機番
            param.paramIdno = idno;                  //IDNO
            param.paramPartsNum = partsNum;          //品番

            //************************************************************************************
            //日付範囲設定
            //************************************************************************************
            if ( DateKind.AtuSearchClass.ProductInst.value == dateKindCd ) {
                //生産指示日
                param.paramProductInstDtFrom = GetSearchFromDate( dtFrom );              //FROM
                param.paramProductInstDtTo = GetSearchToDate( dtTo );                    //TO
            } else if ( DateKind.AtuSearchClass.Throw.value == dateKindCd ) {
                //投入日
                param.paramThrowDtFrom = GetSearchFromDate( dtFrom );                    //FROM
                param.paramThrowDtTo = GetSearchToDate( dtTo );                          //TO
            } else if ( DateKind.AtuSearchClass.Completed.value == dateKindCd ) {
                //完成日
                param.paramCompletionDtFrom = GetSearchFromDate( dtFrom );               //FROM
                param.paramCompletionDtTo = GetSearchToDate( dtTo );                     //TO
            }

            //検索実行
            return AtuDao.SelectAtuList( param, maxRecordCount );
        }

        #endregion

        


        /// <summary>
        /// ATU一覧検索処理(副)
        /// ATU一覧の型式/機番から対象データを抽出する
        /// </summary>
        /// <param name="condition">画面検索条件</param>
        /// <param name="tblParts">部品検索結果</param>
        /// <param name="maxRecordCount">最大取得件数</param>
        /// <returns>製品DataTable</returns>
        private static DataTable SelectAtuSerialList( Dictionary<string, object> condition, DataTable tblAtu, int maxRecordCount ) {
            DataTable tblProducts = null;
            AtuDao.SearchParameter param = new AtuDao.SearchParameter();

            //画面パラメータの取得
            string processCd = DataUtils.GetDictionaryStringVal( condition, MainPartsView.CONDITION.PROCESS_CD.bindField );              //部品種別

            //_logger.Info( "検索条件--->検索区分={0} 部品検索区分={1} 工程区分= ※型式/機番指定",
            //    productKindCd, partsCd, dateKindCd, dtFrom, dtTo );

            //ATU順序一覧から型式/機番リストを取得
            List<SerialParam>[] serialsList = GetSerialsList( tblAtu );

            //param.paramModelType = ModelType.Product;

            //************************************************************************************
            //製品情報取得
            //************************************************************************************
            for ( int transactionIndex = 0; transactionIndex < serialsList.Length; transactionIndex++ ) {
                    param.paramSerialList = serialsList[transactionIndex];                  //型式/機番リスト

                    DataTable tblTemp = new DataTable();

                //部品情報検索(副)実行
                    if ( processCd == AtuProcessKind.ATU_PARTS_SERIAL ) {

                        tblTemp = AtuDao.SelectAtuSerialList( param );

                    } else if ( processCd == AtuProcessKind.ATU_TORQUE_TIGHTENING_RECORD ) {

                        tblTemp = AtuDao.SelectAtuTorqueList( param );

                    } else if ( processCd == AtuProcessKind.ATU_LEAK_MEASURE_RESULT ) {

                        tblTemp = AtuDao.SelectAtuLeakList( param );

                    }

                if ( null == tblProducts ) {
                    //1回目のトランザクション
                    tblProducts = tblTemp;
                } else {
                    //取得した行をインポート
                    foreach ( DataRow row in tblTemp.Rows ) {
                        tblProducts.ImportRow( row );
                    }
                }

                if ( null != tblProducts && maxRecordCount <= tblProducts.Rows.Count ) {
                    //最大取得件数超過(取得処理中断)
                    break;
                }
            }
            if ( null != tblProducts ) {
                tblProducts.AcceptChanges();
            }

            return tblProducts;
        }

        /// <summary>
        /// トランザクションごとに分割した主テーブルの型式/機番リストを作成
        /// </summary>
        /// <param name="tblMain">型式(productModelCd)/機番(serial6)を格納したDataTable</param>
        /// <returns>型式/機番リスト(配列はトランザクション分作成)</returns>
        private static List<SerialParam>[] GetSerialsList( DataTable tblMain ) {
            //製品一覧から型式/機番リストを取得

            //重複削除(7桁機番は工程/部品テーブルからは返されない為注意)
            var serials = (
                from row in tblMain.AsEnumerable()
                select new {
                    productModelCd = row["modelCd"],
                    serial = row["serial"]

                });
                //).Distinct();

            int transactionIndex = 0;
            int transactionCount = (int)Math.Ceiling( serials.Count() / SQL_MIXED_IN_PROC_ENTITY_MAX );   //SQLを実行する回数(IN句の制限の為)
            List<SerialParam>[] serialsList = new List<SerialParam>[transactionCount];

            for ( transactionIndex = 0; transactionIndex < serialsList.Length; transactionIndex++ ) {
                serialsList[transactionIndex] = new List<SerialParam>();
            }
            transactionIndex = 0;
            foreach ( var row in serials ) {
                //トランザクション単位に型式/機番リストを作成
                string productModelCd = StringUtils.ToString( row.productModelCd );          //製品検索結果:生産型式コード
                string serial = StringUtils.ToString( row.serial );                          //製品検索結果:機番

                if ( SQL_MIXED_IN_PROC_ENTITY_MAX <= serialsList[transactionIndex].Count ) {
                    //型式/機番リストがIN句制限件数を超えたら次のトランザクションへ回す
                    transactionIndex++;
                }
                serialsList[transactionIndex].Add( new SerialParam( productModelCd, serial ) );
            }
            return serialsList;
        }


        /// <summary>
        /// 出荷情報取得
        /// </summary>
        /// <param name="tblMain"></param>
        /// <param name="maxRecordCount"></param>
        /// <returns></returns>
        private static DataTable SelectShipInfo(DataTable tblMain, int maxRecordCount = Int32.MaxValue) {

            if (tblMain.Rows.Count == 0) {
                return null;
            }

            DataTable tblProcess = null;

            //ATU順序一覧から型式/機番リストを取得
            List<SerialParam>[] serialsList = GetSerialsList(tblMain);
            for (int transactionIndex = 0; transactionIndex < serialsList.Length; transactionIndex++) {

                //出荷情報取得処理
                DataTable tblTemp = WmsDpfDao.SelectShipData(serialsList[transactionIndex]);

                if (null != tblTemp) {
                    tblTemp.AcceptChanges();

                    if (null == tblProcess) {
                        //1回目のトランザクション
                        tblProcess = tblTemp;
                    }
                    else {
                        //取得した行をインポート
                        foreach (DataRow row in tblTemp.Rows) {
                            tblProcess.ImportRow(row);
                        }
                    }
                }

                if (null != tblProcess && maxRecordCount <= tblProcess.Rows.Count) {
                    //最大取得件数超過(取得処理中断)
                    break;
                }
            }

            if (null != tblProcess) {
                tblProcess.AcceptChanges();
            }

            //重複行の削除
            List<string> colNames = new List<string>();
            foreach (DataColumn item in tblProcess.Columns) {
                colNames.Add(item.ColumnName);
            }
            DataView dv = new DataView(tblProcess);
            tblProcess = dv.ToTable(true, colNames.ToArray<string>());

            return tblProcess;
        }

        #endregion

        #region DataTable結合
        /// <summary>
        /// 製品テーブルと部品テーブルの結合処理(型式/機番による内部結合)
        /// </summary>
        /// <param name="gridColumns">結合結果出力DataTable列定義</param>
        /// <param name="tblLeft">左辺結合DataTable(列定義にproductModelCd/serial6が必要)</param>
        /// <param name="tblRight">右辺結合DataTable(列定義にproductModelCd/serial6が必要)</param>
        /// <returns>結合済みDataTable</returns>
        private static DataTable GetJoinTable( GridViewDefine[] gridColumns, DataTable tblLeft, DataTable tblRight ) {
            //結果用DataTableカラム定義作成
            DataTable tblResult = CreateResultTable( gridColumns );

            if ( null == tblLeft ) {
                //左辺がない場合には結果は返さない
                return tblResult;
            } else if ( null == tblRight ) {
                //左辺テーブル側のみの検索結果を返す
                foreach ( DataRow row in tblLeft.Rows ) {
                    DataRow rowResult = tblResult.NewRow();
                    foreach ( GridViewDefine column in gridColumns ) {
                        if ( true == tblLeft.Columns.Contains( column.bindField ) ) {
                            //左辺側フィールドをセット
                            rowResult[column.bindField] = row[column.bindField];
                        }
                    }
                    tblResult.Rows.Add( rowResult );
                }
            } else {
                //テーブル結合した結果を返す

                var rows = from l in tblLeft.AsEnumerable()
                           join r in tblRight.AsEnumerable()
                           on
                new { modelCd = StringUtils.ToString( l["modelCd"] ), serial = StringUtils.ToString( l["serial"] ) }
                           equals
                new { modelCd = StringUtils.ToString( r["modelCd"] ), serial = StringUtils.ToString( r["serial"] ) }
                           select new {
                               lrow = l,
                               rrow = r
                           };
                //結合結果から、出力対象フィールドを格納したDataTableを作成
                foreach ( var row in rows ) {
                    DataRow rowResult = tblResult.NewRow();
                    DataRow lrow = row.lrow;
                    DataRow rrow = row.rrow;

                    //LINQクエリ結果から、結合後データを格納
                    foreach ( GridViewDefine column in gridColumns ) {
                        if ( true == tblLeft.Columns.Contains( column.bindField ) ) {
                            //左辺側フィールドをセット
                            rowResult[column.bindField] = lrow[column.bindField];
                        } else if ( true == tblRight.Columns.Contains( column.bindField ) ) {
                            //右辺側フィールドをセット
                            if ( null != rrow ) {
                                rowResult[column.bindField] = rrow[column.bindField];
                            }
                        }
                    }
                    tblResult.Rows.Add( rowResult );
                }
            }
            tblResult.AcceptChanges();

            return tblResult;
        }


        private static DataTable GetLeftJoinTable(GridViewDefine[] gridColumns, DataTable tblLeft, DataTable tblRight) {
            //結果用DataTableカラム定義作成
            DataTable tblResult = CreateResultTable(gridColumns);

            if (null == tblLeft) {
                //左辺がない場合には結果は返さない
                return tblResult;
            }
            else if (null == tblRight) {
                //左辺テーブル側のみの検索結果を返す
                foreach (DataRow row in tblLeft.Rows) {
                    DataRow rowResult = tblResult.NewRow();
                    foreach (GridViewDefine column in gridColumns) {
                        if (true == tblLeft.Columns.Contains(column.bindField)) {
                            //左辺側フィールドをセット
                            rowResult[column.bindField] = row[column.bindField];
                        }
                    }
                    tblResult.Rows.Add(rowResult);
                }
            }
            else {
                //テーブル結合した結果を返す

                var rows = from l in tblLeft.AsEnumerable()
                           join r in tblRight.AsEnumerable()
                           on
                new { modelCd = StringUtils.ToString(l["modelCd"]), serial = StringUtils.ToString(l["serial"]) }
                           equals
                new { modelCd = StringUtils.ToString(r["modelCd"]), serial = StringUtils.ToString(r["serial"]) }
                into temp from subR in temp.DefaultIfEmpty()
                           select new {
                               lrow = l,
                               rrow = subR
                           };

                //結合結果から、出力対象フィールドを格納したDataTableを作成
                foreach (var row in rows) {
                    DataRow rowResult = tblResult.NewRow();
                    DataRow lrow = row.lrow;
                    DataRow rrow = row.rrow;

                    //LINQクエリ結果から、結合後データを格納
                    foreach (GridViewDefine column in gridColumns) {
                        if (true == tblLeft.Columns.Contains(column.bindField)) {
                            //左辺側フィールドをセット
                            rowResult[column.bindField] = lrow[column.bindField];
                        }
                        else if (true == tblRight.Columns.Contains(column.bindField)) {
                            //右辺側フィールドをセット
                            if (null != rrow) {
                                rowResult[column.bindField] = rrow[column.bindField];
                            }
                        }
                    }
                    tblResult.Rows.Add(rowResult);
                }
            }
            tblResult.AcceptChanges();

            return tblResult;
        }

        #endregion

        #region 一覧用DataTable作成
        /// <summary>
        /// 一覧結果用データテーブルのカラム定義を作成します
        /// </summary>
        /// <param name="gridColumns">一覧表示対象列</param>
        /// <returns>DataTable</returns>
        private static DataTable CreateResultTable( GridViewDefine[] gridColumns ) {
            DataTable tblResult = new DataTable();
            foreach ( GridViewDefine column in gridColumns ) {
                DataColumn colResult = new DataColumn( column.bindField, column.dataType );
                //Excel出力用にキャプションに列表記名を設定
                //表示対象外の列はキャプションを設定しない(Excel出力時に列の有無を判定する為)
                if ( true == column.visible ) {
                    colResult.Caption = column.headerText;
                } else {
                    colResult.Caption = ""; //出力対象外カラムのキャプションはクリア
                }

                tblResult.Columns.Add( colResult );
            }
            return tblResult;
        }
        #endregion

        /// <summary>
        /// 日付検索条件用の値に変換します
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private static DateTime? GetSearchFromDate( DateTime dt ) {
            DateTime dtMin = new DateTime( 1900, 1, 1 );
            if ( dtMin < dt ) {
                return dt;
            }
            return null;
        }

        /// <summary>
        /// 日付検索条件用の値に変換します
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private static DateTime? GetSearchToDate( DateTime dt ) {
            DateTime dtMin = new DateTime( 1900, 1, 1 );
            if ( dtMin < dt ) {
                return dt.AddDays( 1 ).AddMilliseconds( -1 );
            }
            return null;
        }
    }
}