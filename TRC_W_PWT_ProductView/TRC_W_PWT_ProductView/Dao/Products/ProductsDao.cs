using System;
using System.Data;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using KTFramework.Common.Dao;
using KTFramework.Dao;
using KTFramework.Common;
using TRC_W_PWT_ProductView.Common;
using TRC_W_PWT_ProductView.Dao.Com;
using TRC_W_PWT_ProductView.Defines;

namespace TRC_W_PWT_ProductView.Dao.Products {
    /// <summary>
    /// 製品検索DAO
    /// </summary>
    public class ProductsDao : DaoBase {
        /// <summary>SQLマップネームスペース</summary>
        private const string SQLMAP_NAMESPACE = "Products";

        /// <summary>
        /// 製品検索パラメータクラス
        /// </summary>
        public class SearchParameter {
            /// <summary>結合部品テーブル [NULL=無し/02=crate/03=rops]</summary>
            public string appendPartsCd;
            /// <summary>仕掛本機 [0=非対象/1=対象]</summary>
            public int searchProductingTractor;
            /// <summary>仕掛エンジン [0=非対象/1=対象]</summary>
            public int searchProductingEngine;

            /// <summary>在庫全製品結合 [0=非対象/1=対象]</summary>
            public int unionStockProducts;
            /// <summary>在庫本機 [0=非対象/1=対象]</summary>
            public int searchStockTractor;
            /// <summary>在庫エンジン [0=非対象/1=対象]</summary>
            public int searchStockEngine;
            /// <summary>在庫ロータリー [0=非対象/1=対象]</summary>
            public int searchStockRotary;

            /// <summary>出荷済全製品結合 [0=非対象/1=対象]</summary>
            public int unionShippedProducts;
            /// <summary>出荷済トラクタ [0=非対象/1=対象]</summary>
            public int searchShippedTractor;
            /// <summary>出荷済エンジン [0=非対象/1=対象]</summary>
            public int searchShippedEngine;
            /// <summary>出荷済ロータリー [0=非対象/1=対象]</summary>
            public int searchShippedRotary;

            //製品テーブル検索条件
            /// <summary>[共通]総称パターンコード</summary>
            public string paramGeneralPatternCd;
            /// <summary>[共通]型式/機番</summary>
            public List<string> paramAssemblyPatternCdList;
            /// <summary>改造前型式検索 [0=非対象/1=対象]</summary>
            public int searchFirstProduct = 0;
            /// <summary>[共通]型式/機番</summary>
            public List<SerialParam> paramSerialList;
            /// <summary>[共通]型式タイプ</summary>
            public string paramModelType;
            /// <summary>[共通]生産型式コード(前方一致)</summary>
            public string paramProductModelCd;
            /// <summary>[共通]生産型式名(前方一致)</summary>
            public string paramProductModelNm;
            /// <summary>[共通]販売型式コード(前方一致)</summary>
            public string paramSalesModelCd;
            /// <summary>[共通]販売型式名(前方一致)</summary>
            public string paramSalesModelNm;
            /// <summary>[共通]機番</summary>
            public string paramSerial;
            /// <summary>[共通]6桁機番</summary>
            public string paramSerial6;
            /// <summary>[共通]IDNO</summary>
            public string paramIdno;
            /// <summary>[共通]製品状態(0=仕掛/1=未出荷/2=出荷済)</summary>
            public int paramProductStatus;
            /// <summary>完成予定日(FROM)…仕掛/在庫品のみ</summary>
            public DateTime? paramPlanDtFrom;
            /// <summary>完成予定日(TO)…仕掛/在庫品のみ</summary>
            public DateTime? paramPlanDtTo;
            /// <summary>[共通]完成日(FROM)</summary>
            public DateTime? paramProductDtFrom;
            /// <summary>[共通]完成日(TO)</summary>
            public DateTime? paramProductDtTo;
            /// <summary>出荷日(FROM)…出荷済のみ</summary>
            public DateTime? paramShippedDtFrom;
            /// <summary>出荷日(TO)…出荷済のみ</summary>
            public DateTime? paramShippedDtTo;
            /// <summary>[共通]型式/機番(トラクタでのエンジン部品検索用)</summary>
            public List<SerialParam> paramSerialListForTractor;

            //部品テーブル検索条件(ロプス/クレート用)
            /// <summary>[部品:ロプス/クレート]部品品番</summary>
            public string paramPartsNum;
            /// <summary>[部品:ロプス/クレート]部品機番</summary>
            public string paramPartsSerial;
            /// <summary>[部品:ロプス/クレート]組付日(FROM)</summary>
            public DateTime? paramInstallDtFrom;
            /// <summary>[部品:ロプス/クレート]組付日(TO)</summary>
            public DateTime? paramInstallDtTo;
            /// <summary>PINコード(チェックボックス)</summary>
            public string paramPinCdCheck;
            /// <summary>PINコード</summary>
            public string paramPinCd;
        }

        /// <summary>
        /// 一覧画面製品情報検索
        /// </summary>
        /// <param name="param">検索パラメータ</param>
        /// <returns>検索結果一覧DataTable</returns>
        public static DataTable SelectList( SearchParameter param, int maxRecordCount = Int32.MaxValue ) {
            //パラメータの設定
            KTBindParameters bindParam = new KTBindParameters();

            //検索対象テーブル条件
            bindParam.Add( "appendPartsCd", param.appendPartsCd );                              //結合部品テーブル[NULL=無し/02=crate/03=rops]
            //仕掛
            bindParam.Add( "searchProductingTractor", param.searchProductingTractor );          //仕掛本機[0=非対象/1=対象]
            bindParam.Add( "searchProductingEngine", param.searchProductingEngine );            //仕掛エンジン[0=非対象/1=対象]
            //在庫
            bindParam.Add( "unionStockProducts", param.unionStockProducts );                    //在庫製品UNION式の付加[1=結合]
            bindParam.Add( "searchStockTractor", param.searchStockTractor );                    //在庫本機[0=非対象/1=対象]
            bindParam.Add( "searchStockEngine", param.searchStockEngine );                      //在庫エンジン[0=非対象/1=対象]
            bindParam.Add( "searchStockRotary", param.searchStockRotary );                      //在庫ロータリー[0=非対象/1=対象]
            //出荷済
            bindParam.Add( "unionShippedProducts", param.unionShippedProducts );                //出荷全製品UNION式の付加[1=結合]
            bindParam.Add( "searchShippedTractor", param.searchShippedTractor );                //出荷済トラクタ[0=非対象/1=対象]
            bindParam.Add( "searchShippedEngine", param.searchShippedEngine );                  //出荷済エンジン[0=非対象/1=対象]
            bindParam.Add( "searchShippedRotary", param.searchShippedRotary );                  //出荷済ロータリー[0=非対象/1=対象]

            //製品検索条件
            bindParam.Add( "paramGeneralPatternCd", param.paramGeneralPatternCd );              //総称パターンコード
            bindParam.Add( "paramAssemblyPatternCdList", param.paramAssemblyPatternCdList );    //組立パターンリスト
            bindParam.Add( "searchFirstProduct", param.searchFirstProduct );                    //改造前型式検索 [0=非対象/1=対象]
            bindParam.Add( "paramSerialList", param.paramSerialList );                          //型式/機番リスト
            bindParam.Add( "paramSerialListForToractor", param.paramSerialListForTractor );     //型式/機番リスト(トラクタでのエンジン部品検索用)
            bindParam.Add( "paramModelType", param.paramModelType );                            //型式種別(0=生産型式 1=販売型式)
            bindParam.Add( "paramProductModelCd", param.paramProductModelCd );                  //生産型式コード(前方一致)
            bindParam.Add( "paramProductModelNm", param.paramProductModelNm );                  //生産型式名(前方一致)
            bindParam.Add( "paramSalesModelCd", param.paramSalesModelCd );                      //販売型式コード(前方一致)
            bindParam.Add( "paramSalesModelNm", param.paramSalesModelNm );                      //販売型式名(前方一致)
            bindParam.Add( "paramSerial6", param.paramSerial6 );                                //6桁機番(完全一致)
            bindParam.Add( "paramSerial", param.paramSerial );                                  //機番(完全一致)
            bindParam.Add( "paramIdno", param.paramIdno );                                      //IDNO(完全一致)
            bindParam.Add( "paramProductStatus", param.paramProductStatus );                    //製品状態(0=仕掛/1=未出荷/2=出荷済)
            if ( null != param.paramPlanDtFrom ) {
                bindParam.Add( "paramPlanDtFrom", param.paramPlanDtFrom.Value );                //完成予定日(FROM)…仕掛/在庫品のみ
            }
            if ( null != param.paramPlanDtTo ) {
                bindParam.Add( "paramPlanDtTo", param.paramPlanDtTo.Value );                    //完成予定日(TO)…仕掛/在庫品のみ
            }
            if ( null != param.paramProductDtFrom ) {
                bindParam.Add( "paramProductDtFrom", param.paramProductDtFrom.Value );          //完成日(FROM)
            }
            if ( null != param.paramProductDtTo ) {
                bindParam.Add( "paramProductDtTo", param.paramProductDtTo.Value );              //完成日(TO)
            }
            if ( null != param.paramShippedDtFrom ) {
                bindParam.Add( "paramShippedDtFrom", param.paramShippedDtFrom.Value );          //出荷日(FROM)…出荷済のみ
            }
            if ( null != param.paramShippedDtTo ) {
                bindParam.Add( "paramShippedDtTo", param.paramShippedDtTo.Value );              //出荷日(TO)…出荷済のみ
            }

            //部品検索条件(ロプス/クレートのみ)
            bindParam.Add( "paramPartsNum", param.paramPartsNum );                              //部品品番
            bindParam.Add( "paramPartsSerial", param.paramPartsSerial );                        //部品機番
            if ( null != param.paramInstallDtFrom ) {
                bindParam.Add( "paramInstallDtFrom", param.paramInstallDtFrom.Value );          //組付日(FROM)
            }
            if ( null != param.paramInstallDtTo ) {
                bindParam.Add( "paramInstallDtTo", param.paramInstallDtTo.Value );              //組付日(TO)
            }
            if ( null != param.paramPinCdCheck ) {
                bindParam.Add( "paramPinCdCheck", param.paramPinCdCheck );                      //PINコード(チェックボックス)
            }
            if ( null != param.paramPinCd ) {
                bindParam.Add( "paramPinCd", param.paramPinCd );                                //PINコード
            }

            //SQL実行
            string statementId = GetFullStatementIdForLibrary( SQLMAP_NAMESPACE, "SelectProductList" );
            Cursor cursor = GiaDao.GetInstance().OpenCursor( statementId, bindParam );
            DataTable resultTable = null;
            try {
                while ( GiaDao.GetInstance().Fetch( ref resultTable, ref cursor ) ) {
                    if ( resultTable.Rows.Count >= maxRecordCount ) {
                        break;
                    }
                }
            } finally {
                GiaDao.GetInstance().CloseCursor( ref cursor );
            }

            //ロジックでの値割当
            if ( null != resultTable ) {
                foreach ( DataRow row in resultTable.Rows ) {
                    //製品種別名(組立パターン名)
                    string assemblyPatternNm = AssemblyPatternCode.GetName( StringUtils.ToString( row["assemblyPatternCd"] ) );
                    row["assemblyPatternNm"] = assemblyPatternNm;

                    //生産型式コード表記(生産型式でなく型式を表示させる[型式／仕向地変更対応])
                    string productModelCd = StringUtils.ToString( row["modelCd"] );
                    row["productModelCdStr"] = DataUtils.GetModelCdStr( productModelCd );

                    //トラクタ型式コード表記
                    string tractorModelCd = StringUtils.ToString( row["tractorModelCd"] );
                    row["tractorModelCdStr"] = DataUtils.GetModelCdStr( tractorModelCd );

                    //エンジン型式コード表記
                    string engineModelCd = StringUtils.ToString( row["engineModelCd"] );
                    row["engineModelCdStr"] = DataUtils.GetModelCdStr( engineModelCd );

                    //販売型式コード表記
                    string salesModelCd = StringUtils.ToString( row["salesModelCd"] );
                    row["salesModelCdStr"] = DataUtils.GetModelCdStr( salesModelCd );

                    //改造前型式コード表記
                    string preAlterationModelCd = StringUtils.ToString(row["preAlterationModelCd"]);
                    row["preAlterationModelCd"] = DataUtils.GetModelCdStr(preAlterationModelCd);

                    //改造情報表記
                    if ( StringUtils.ToString(row["preAlterationDetail"]).Equals("") ) {
                        row["preAlterationModelCd"] = "";
                        row["preAlterationCountryCd"] = "";
                    }
                }

                resultTable.AcceptChanges();
            }


            return resultTable;
        }


    }
}