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
    public class MainViewBusiness {
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
            string partsCd = DataUtils.GetDictionaryStringVal( condition, MainView.CONDITION.PARTS_CD.bindField );           //部品区分
            string processCd = DataUtils.GetDictionaryStringVal( condition, MainView.CONDITION.PROCESS_CD.bindField );       //工程区分
            string modelCd = DataUtils.GetModelCd( DataUtils.GetDictionaryStringVal( condition, MainView.CONDITION.MODEL_CD.bindField ) ); //型式コード
            string modelNm = DataUtils.GetDictionaryStringVal( condition, MainView.CONDITION.MODEL_NM.bindField );           //型式名

            //検索条件から、検索母体(製品 or 部品)を判定
            bool searchProductsFirst = IsSearchProductsFirst( condition );
            int mainTableMaxCount = maxRecordCount;
            int subTableMaxCount = maxRecordCount;
            if ( true == searchProductsFirst && ( false == StringUtils.IsBlank( partsCd ) || false == StringUtils.IsBlank( processCd ) ) ) {
                //製品検索を先に検索する場合で、部品または工程を結合する時は、製品情報の取得件数は制限しない
                mainTableMaxCount = Int32.MaxValue;
            }

            //検索条件から、製品/部品の検索優先順に従い検索を行い、テーブルを結合する
            DataTable tblMain = null;
            DataTable tblSub = null;
            if ( true == searchProductsFirst ) {
                //製品検索後、抽出した型式/機番から部品を検索する
                _logger.Info( "製品検索(主)実行" );
                //製品検索
                tblMain = SelectProductsList( condition, mainTableMaxCount );
                if ( null != tblMain ) {
                    _logger.Info( "製品検索(主)結果 …{0}件", tblMain.Rows.Count );
                }

                //製品付属情報検索
                if ( false == StringUtils.IsBlank( partsCd ) ) {
                    //部品
                    _logger.Info( "部品検索(副)実行" );
                    tblSub = SelectPartsList( condition, tblMain, subTableMaxCount );
                    if ( null != tblSub ) {
                        _logger.Info( "部品検索(副)結果 …{0}件", tblSub.Rows.Count );
                    }
                } else if ( false == StringUtils.IsBlank( processCd ) ) {
                    //工程
                    _logger.Info( "工程検索(副)実行" );
                    tblSub = SelectProcessList( condition, tblMain, subTableMaxCount );
                    if ( null != tblSub ) {
                        _logger.Info( "工程検索(副)結果 …{0}件", tblSub.Rows.Count );
                    }
                }
            } else {
                //部品検索後、抽出した型式/機番から製品を検索する

                //型式コードが未指定の場合には、型式名から型式コードを逆引きする
                List<string> modelCdList = GetModelCdList( modelNm );
                if ( null != modelCdList && MODEL_CD_LIST_MAX < modelCdList.Count ) {
                    //型式コード抽出件数上限オーバー
                    result.Message = new Msg( MsgManager.MESSAGE_WRN_61030 );
                    return result;
                }

                //製品付属情報検索
                if ( false == StringUtils.IsBlank( partsCd ) ) {
                    //部品
                    _logger.Info( "部品検索(主)実行" );
                    tblMain = SelectPartsList( condition, modelCdList, mainTableMaxCount );
                    if ( null != tblMain ) {
                        _logger.Info( "部品検索(主)結果 …{0}件", tblMain.Rows.Count );
                    }
                } else if ( false == StringUtils.IsBlank( processCd ) ) {
                    //工程
                    _logger.Info( "工程検索(主)実行" );
                    tblMain = SelectProcessList( condition, modelCdList, mainTableMaxCount );
                    if ( null != tblMain ) {
                        _logger.Info( "工程検索(主)結果 …{0}件", tblMain.Rows.Count );
                    }
                }

                //製品検索
                _logger.Info( "製品検索(副)実行" );
                tblSub = SelectProductsList( condition, tblMain, subTableMaxCount );
                if ( null != tblSub ) {
                    _logger.Info( "製品検索(副)結果 …{0}件", tblSub.Rows.Count );
                }
            }
            //製品/部品の検索結果を型式/機番で結合する
            string productKindCd = DataUtils.GetDictionaryStringVal( condition, MainView.CONDITION.PRODUCT_KIND_CD.bindField );

            //CLASSコードの固定99以下(Alphabet)を含めない
            //            if ( productKindCd.Equals( ProductKind.Tractor ) && partsCd.CompareTo( StringUtils.ToString( PartsKind.PARTS_CD_ENGINE_SCR ) )<0 ) {
            if ( false == StringUtils.IsEmpty( partsCd ) && productKindCd.Equals( ProductKind.Tractor ) && partsCd.CompareTo( "99" ) < 0 ) {
                if ( true == searchProductsFirst ) {
                    result.ListTable = GetJoinTractorTable( columns, tblMain, tblSub );
                } else {
                    result.ListTable = GetJoinTractorTable2( columns, tblMain, tblSub );
                }
            } else {
                result.ListTable = GetJoinTable( columns, tblMain, tblSub );
            }

            //ECU種別の取得と関連項目の変更
            //製品情報と部品情報が取得できるこのタイミングで実施
            switch ( partsCd ) {
            case PartsKind.PARTS_CD_ENGINE_ECU:           //ECU
            case PartsKind.PARTS_CD_ENGINE_EPR:           //EPR
            case PartsKind.PARTS_CD_ENGINE_MIXER:         //MIXER

                foreach ( DataRow row in result.ListTable.Rows ) {
                    DataTable dtSetubi = EnginePartsDao.SelectSetubiJyoho( StringUtils.ToString( row["productModelCd"] ), StringUtils.ToString( row["productCountryCd"] ) );
                    if ( dtSetubi != null && 0 < dtSetubi.Rows.Count ) {
                        DataRow drSetubi = dtSetubi.Rows[0];
                        if ( "0" == StringUtils.ToString( drSetubi["MS_JYOHO_1"] ) ) {
                            row["partsMakerNum"] = null;
                        }

                    }
                }
                break;
            default:
                break;
            }

            //            result.ListTable = GetJoinTable( columns, tblMain, tblSub );
            if ( null != result.ListTable ) {
                _logger.Info( "製品一覧結合結果 …{0}件", result.ListTable.Rows.Count );
            }

            //メッセージ設定
            result.Message = null;
            if ( null == result.ListTable || 0 == result.ListTable.Rows.Count ) {
                //検索結果0件
                result.Message = new Msg( MsgManager.MESSAGE_WRN_61010 );
            } else if ( ( null != tblMain && mainTableMaxCount <= tblMain.Rows.Count ) || ( null != tblSub && subTableMaxCount <= tblSub.Rows.Count ) ) {
                //検索件数が上限を上回っている場合には警告メッセージをセット
                result.Message = new Msg( MsgManager.MESSAGE_WRN_61020 );
            }

            return result;
        }

        /// <summary>
        /// 検索順序判定処理(検索条件のパターン別に、製品検索を優先して検索するか判定する)
        /// </summary>
        /// <param name="condition">検索条件</param>
        /// <returns>true:製品検索を優先 false:部品/工程検索を優先</returns>
        private static bool IsSearchProductsFirst( Dictionary<string, object> condition ) {
            //検索条件から、検索母体(製品 or 部品)を判定
            bool searchProductsFirst = true;

            string productKindCd = DataUtils.GetDictionaryStringVal( condition, MainView.CONDITION.PRODUCT_KIND_CD.bindField );   //製品区分
            string partsCd = DataUtils.GetDictionaryStringVal( condition, MainView.CONDITION.PARTS_CD.bindField );           //部品区分
            string processCd = DataUtils.GetDictionaryStringVal( condition, MainView.CONDITION.PROCESS_CD.bindField );       //工程区分
            string modelCd = DataUtils.GetDictionaryStringVal( condition, MainView.CONDITION.MODEL_CD.bindField );           //型式コード
            string modelNm = DataUtils.GetDictionaryStringVal( condition, MainView.CONDITION.MODEL_NM.bindField );           //型式名
            string serial = DataUtils.GetDictionaryStringVal( condition, MainView.CONDITION.SERIAL.bindField );              //機番
            string idno = DataUtils.GetDictionaryStringVal( condition, MainView.CONDITION.IDNO.bindField );                  //IDNO
            string partsNum = DataUtils.GetDictionaryStringVal( condition, MainView.CONDITION.PARTS_NUM.bindField );         //部品品番
            string partsSerial = DataUtils.GetDictionaryStringVal( condition, MainView.CONDITION.PARTS_SERIAL.bindField );   //部品機番
            string dateKindCd = DataUtils.GetDictionaryStringVal( condition, MainView.CONDITION.DATE_KIND_CD.bindField );    //日付区分

            if ( false == StringUtils.IsBlank( serial ) ) {
                //機番が入力--->製品検索を優先する。
                searchProductsFirst = true;
            } else if ( false == StringUtils.IsBlank( idno ) ) {
                //IDNOが入力--->製品検索を優先する。
                searchProductsFirst = true;
            } else if ( productKindCd == ProductKind.Tractor && ( partsCd == PartsKind.PARTS_CD_TRACTOR_CRATE || partsCd == PartsKind.PARTS_CD_TRACTOR_ROPS ) ) {
                //製品:トラクタ 部品:クレート/ロプス選択--->製品検索を優先する。
                searchProductsFirst = true;
                //} else if ( productKindCd == ProductKind.Tractor && partsCd == PartsKind.PARTS_CD_TRACTOR_WECU && false == StringUtils.IsBlank( partsSerial ) ) {
                //    //製品:トラクタ 部品:WiFiECU 部品機番が入力--->部品検索を優先する。
                //    //※部品品番は分散が低い為効果なし
                //    searchProductsFirst = false;
                //} else if ( productKindCd == ProductKind.Engine && partsCd == PartsKind.PARTS_CD_ENGINE_SUPPLYPUMP && false == StringUtils.IsBlank( partsSerial ) ) {
                //    //製品:エンジン 部品:サプライポンプ 部品機番が入力--->部品検索を優先する。
                //    //※部品品番は分散が低い為効果なし
                //    searchProductsFirst = false;
                //} else if ( productKindCd == ProductKind.Engine && partsCd == PartsKind.PARTS_CD_ENGINE_ECU && false == StringUtils.IsBlank( partsSerial ) ) {
                //    //製品:エンジン 部品:ECU 部品機番が入力--->部品検索を優先する。
                //    searchProductsFirst = false;
                //} else if ( productKindCd == ProductKind.Engine && partsCd == PartsKind.PARTS_CD_ENGINE_INJECTOR &&  false == StringUtils.IsBlank( partsSerial ) ) {
                //    //製品:エンジン 部品:インジェクタ 部品機番が入力--->部品検索を優先する。
                //    searchProductsFirst = false;
                //} else if ( productKindCd == ProductKind.Engine && partsCd == PartsKind.PARTS_CD_ENGINE_DPF && false == StringUtils.IsBlank( partsSerial ) ) {
                //    //製品:エンジン 部品:DPF 部品機番が入力--->部品検索を優先する。
                //    //※部品品番は分散が低い為効果なし
                //    searchProductsFirst = false;
                //※CCの部品品番は分散が低い為効果なし
                //} else if ( productKindCd == ProductKind.Engine && partsCd == PartsKind.PARTS_CD_ENGINE_CC && false == StringUtils.IsBlank( partsNum ) ) {
                //    //製品:エンジン 部品:CC 部品品番が入力--->部品検索を優先する。
                //    searchProductsFirst = false;
                //} else if ( productKindCd == ProductKind.Engine && partsCd == PartsKind.PARTS_CD_ENGINE_EPR && false == StringUtils.IsBlank( partsSerial ) ) {
                //    //製品:エンジン 部品:EPR 部品品番が入力--->部品検索を優先する。
                //    //※部品品番は分散が低い為効果なし
                //    searchProductsFirst = false;
                //} else if ( productKindCd == ProductKind.Engine && partsCd == PartsKind.PARTS_CD_ENGINE_MIXER && false == StringUtils.IsBlank( partsSerial ) ) {
                //    //製品:エンジン 部品:MIXER 部品品番が入力--->部品検索を優先する。
                //    //※部品品番は分散が低い為効果なし
                //    searchProductsFirst = false;
                //※CYHの部品品番は分散が低い為効果なし
                //} else if ( productKindCd == ProductKind.Engine && partsCd == PartsKind.PARTS_CD_ENGINE_CYH && false == StringUtils.IsBlank( partsNum ) ) {
                //    //製品:エンジン 部品:CYH 部品品番が入力--->部品検索を優先する。
                //    searchProductsFirst = false;
                //※CSの部品品番は分散が低い為効果なし
                //} else if ( productKindCd == ProductKind.Engine && partsCd == PartsKind.PARTS_CD_ENGINE_CS && false == StringUtils.IsBlank( partsNum ) ) {
                //    //製品:エンジン 部品:CS 部品品番が入力--->部品検索を優先する。
                //    searchProductsFirst = false;
            } else {
                //日付区分による優先度判定
                if ( DateKind.ProductClass.ToBeComplete.value == dateKindCd ) {
                    //完成予定日(製品検索を優先)
                    searchProductsFirst = true;
                } else if ( DateKind.ProductClass.Completed.value == dateKindCd ) {
                    //完成日(製品検索を優先)
                    searchProductsFirst = true;
                } else if ( DateKind.ProductClass.StockOnly.value == dateKindCd ) {
                    //完成予定日＋完成日[出荷以外](製品検索を優先)
                    searchProductsFirst = true;
                } else if ( DateKind.ProductClass.Shipped.value == dateKindCd ) {
                    //出荷日(製品検索を優先)
                    searchProductsFirst = true;
                } else if ( DateKind.PartsClass.Installing.value == dateKindCd ) {
                    //部品:組付日(部品検索を優先)
                    searchProductsFirst = false;
                } else if ( DateKind.PartsClass.Processing.value == dateKindCd ) {
                    //部品:加工日(部品検索を優先)
                    searchProductsFirst = false;
                } else if ( DateKind.ProcessClass.InspectionMeasuring.value == dateKindCd ) {
                    //工程:測定日(工程検索を優先)
                    searchProductsFirst = false;
                }
            }
            return searchProductsFirst;
        }
        #endregion

        #region 製品一覧検索
        /// <summary>
        /// 製品一覧検索処理(主)
        /// </summary>
        /// <param name="condition">画面検索条件</param>
        /// <param name="maxRecordCount">最大取得件数</param>
        /// <returns>製品DataTable</returns>
        private static DataTable SelectProductsList( Dictionary<string, object> condition, int maxRecordCount ) {
            ProductsDao.SearchParameter param = new ProductsDao.SearchParameter();

            //画面パラメータの取得
            string productKindCd = DataUtils.GetDictionaryStringVal( condition, MainView.CONDITION.PRODUCT_KIND_CD.bindField );   //製品区分
            string partsCd = DataUtils.GetDictionaryStringVal( condition, MainView.CONDITION.PARTS_CD.bindField );                //部品区分
            string modelType = DataUtils.GetDictionaryStringVal( condition, MainView.CONDITION.MODEL_TYPE.bindField );            //型式種別
            string modelCd = DataUtils.GetModelCd( DataUtils.GetDictionaryStringVal( condition, MainView.CONDITION.MODEL_CD.bindField ) ); //型式コード
            string modelNm = DataUtils.GetDictionaryStringVal( condition, MainView.CONDITION.MODEL_NM.bindField );                //型式名
            string serial = DataUtils.GetDictionaryStringVal( condition, MainView.CONDITION.SERIAL.bindField );                   //機番
            string idno = DataUtils.GetDictionaryStringVal( condition, MainView.CONDITION.IDNO.bindField );                       //IDNO
            string dateKindCd = DataUtils.GetDictionaryStringVal( condition, MainView.CONDITION.DATE_KIND_CD.bindField );         //日付区分
            DateTime dtFrom = DataUtils.GetDictionaryDateVal( condition, MainView.CONDITION.DATE_FROM.bindField );                //日付(FROM)
            DateTime dtTo = DataUtils.GetDictionaryDateVal( condition, MainView.CONDITION.DATE_TO.bindField );                    //日付(TO)
            string partsNum = DataUtils.GetDictionaryStringVal( condition, MainView.CONDITION.PARTS_NUM.bindField );              //部品品番(ロプス/クレート用)
            string partsSerial = DataUtils.GetDictionaryStringVal( condition, MainView.CONDITION.PARTS_SERIAL.bindField );        //部品機番(ロプス/クレート用)
            string pinCdCheck = DataUtils.GetDictionaryStringVal( condition, MainView.CONDITION.PIN_CD_CHECK.bindField );        //PINコード(チェックボックス)
            string pinCd = DataUtils.GetDictionaryStringVal( condition, MainView.CONDITION.PIN_CD.bindField );              //PINコード

            _logger.Info( "検索条件--->製品区分={0} 部品区分={1} 型式種別={2} 型式コード={3} 型式名={4} " +
                          "機番={5} IDNO={6} 日付区分={7} 日付={8}～{9} 部品品番={10} 部品機番={11}",
                productKindCd, partsCd, modelType, modelCd, modelNm, serial, idno, dateKindCd, dtFrom, dtTo, partsNum, partsSerial );

            //************************************************************************************
            //検索対象テーブル設定(製品区分と日付区分から対象テーブルを設定)
            //************************************************************************************
            SetTargetProductsTables( ref param, productKindCd, modelType, dateKindCd );

            //************************************************************************************
            //部品テーブルの結合有無(トラクタ部品:クレート/ロプスのみ)
            //************************************************************************************
            if ( productKindCd == ProductKind.Tractor ) {
                param.appendPartsCd = partsCd;
                param.paramPartsNum = partsNum;
                param.paramPartsSerial = partsSerial;
            }

            //************************************************************************************
            //製品情報設定
            //************************************************************************************
            param.paramModelType = modelType;
            if ( ModelType.Product == modelType ) {
                //生産型式を割当
                param.paramProductModelCd = modelCd;            //生産型式コード
                param.paramProductModelNm = modelNm;            //生産型式名
            } else {
                //販売型式を割当
                param.paramSalesModelCd = modelCd;              //販売型式コード
                param.paramSalesModelNm = modelNm;              //販売型式名
            }
            param.paramSerial = DataUtils.GetSerialTractor( serial ); //機番(6桁機番のみでは型式違いが抽出される為、条件追加)
            param.paramSerial6 = DataUtils.GetSerial6( serial );//6桁機番(副次問合せ時の絞り込み)
            param.paramGeneralPatternCd = productKindCd;        //総称パターンコード(製品区分を流用)
            if ( ProductKind.Tractor == productKindCd ) {
                param.paramAssemblyPatternCdList = new List<string>();
                param.paramAssemblyPatternCdList.Add( AssemblyPatternCode.Tractor );
            } else if ( ProductKind.Engine == productKindCd ) {
                param.paramAssemblyPatternCdList = new List<string>();
                param.paramAssemblyPatternCdList.Add( AssemblyPatternCode.OemEngine03 );
                param.paramAssemblyPatternCdList.Add( AssemblyPatternCode.OemEngine07 );
                param.paramAssemblyPatternCdList.Add( AssemblyPatternCode.InstalledEngine03 );
                param.paramAssemblyPatternCdList.Add( AssemblyPatternCode.InstalledEngine07 );
            } else if ( ProductKind.Rotary == productKindCd ) {
                param.paramAssemblyPatternCdList = new List<string>();
                param.paramAssemblyPatternCdList.Add( AssemblyPatternCode.ExternalProductions );
                param.paramAssemblyPatternCdList.Add( AssemblyPatternCode.Rotary );
                param.paramAssemblyPatternCdList.Add( AssemblyPatternCode.Unit );
                param.paramAssemblyPatternCdList.Add( AssemblyPatternCode.Wheel );
                param.paramAssemblyPatternCdList.Add( AssemblyPatternCode.BundledItems );
                param.paramAssemblyPatternCdList.Add( AssemblyPatternCode.Imple );
                param.paramAssemblyPatternCdList.Add( AssemblyPatternCode.InhouseProductedRotary3 );
            }
            param.paramIdno = idno;                             //IDNO

            //************************************************************************************
            //日付範囲設定
            //************************************************************************************
            if ( DateKind.ProductClass.ToBeComplete.value == dateKindCd ) {
                //完成予定日
                param.paramPlanDtFrom = GetSearchFromDate( dtFrom );              //FROM
                param.paramPlanDtTo = GetSearchToDate( dtTo );                    //TO
            } else if ( DateKind.ProductClass.Completed.value == dateKindCd ) {
                //完成日
                param.paramProductDtFrom = GetSearchFromDate( dtFrom );         //FROM
                param.paramProductDtTo = GetSearchToDate( dtTo );               //TO
            } else if ( DateKind.ProductClass.StockOnly.value == dateKindCd ) {
                //完成予定＋完成日[出荷以外]
                param.paramPlanDtFrom = GetSearchFromDate( dtFrom );            //FROM
                param.paramPlanDtTo = GetSearchToDate( dtTo );                  //TO
                param.paramProductDtFrom = GetSearchFromDate( dtFrom );         //FROM
                param.paramProductDtTo = GetSearchToDate( dtTo );               //TO
            } else if ( DateKind.ProductClass.Shipped.value == dateKindCd ) {
                //出荷日
                param.paramShippedDtFrom = GetSearchFromDate( dtFrom );         //FROM
                param.paramShippedDtTo = GetSearchToDate( dtTo );               //TO
            } else if ( DateKind.PartsClass.Installing.value == dateKindCd ) {
                //組付日(クレート/ロプスのみ)
                param.paramInstallDtFrom = GetSearchFromDate( dtFrom );         //FROM
                param.paramInstallDtTo = GetSearchToDate( dtTo );               //TO
            }

            //************************************************************************************
            //部品情報設定(クレート/ロプスのみ)
            //************************************************************************************
            param.paramPartsNum = partsNum;                     //部品品番
            param.paramPartsSerial = partsSerial;               //部品機番
            param.paramPinCdCheck = pinCdCheck;                 //PINコード(チェックボックス)
            param.paramPinCd = pinCd;                           //PINコード

            //検索実行
            return ProductsDao.SelectList( param, maxRecordCount );
        }

        /// <summary>
        /// 製品一覧検索処理(副)
        /// 部品一覧の型式/機番から対象データを抽出する
        /// </summary>
        /// <param name="condition">画面検索条件</param>
        /// <param name="tblParts">部品検索結果</param>
        /// <param name="maxRecordCount">最大取得件数</param>
        /// <returns>製品DataTable</returns>
        private static DataTable SelectProductsList( Dictionary<string, object> condition, DataTable tblParts, int maxRecordCount ) {
            DataTable tblProducts = null;
            ProductsDao.SearchParameter param = new ProductsDao.SearchParameter();

            //画面パラメータの取得
            string productKindCd = DataUtils.GetDictionaryStringVal( condition, MainView.CONDITION.PRODUCT_KIND_CD.bindField );   //製品区分
            string modelType = DataUtils.GetDictionaryStringVal( condition, MainView.CONDITION.MODEL_TYPE.bindField );            //型式種別
            string partsCd = DataUtils.GetDictionaryStringVal( condition, MainView.CONDITION.PARTS_CD.bindField );                //部品区分
            string dateKindCd = DataUtils.GetDictionaryStringVal( condition, MainView.CONDITION.DATE_KIND_CD.bindField );         //日付区分
            DateTime dtFrom = DataUtils.GetDictionaryDateVal( condition, MainView.CONDITION.DATE_FROM.bindField );                //日付(FROM)
            DateTime dtTo = DataUtils.GetDictionaryDateVal( condition, MainView.CONDITION.DATE_TO.bindField );                    //日付(TO)
            string pinCdCheck = DataUtils.GetDictionaryStringVal( condition, MainView.CONDITION.PIN_CD_CHECK.bindField );         //PINコード(チェックボックス)
            string pinCd = DataUtils.GetDictionaryStringVal( condition, MainView.CONDITION.PIN_CD.bindField );                    //PINコード

            _logger.Info( "検索条件--->製品区分={0} 部品区分={1} 日付区分={2} 日付={3}～{4} ※型式/機番指定",
                productKindCd, partsCd, dateKindCd, dtFrom, dtTo );

            //部品一覧から型式/機番リストを取得
            List<SerialParam>[] serialsList = GetSerialsList( tblParts );

            param.paramModelType = ModelType.Product;

            //************************************************************************************
            //検索対象テーブル設定(製品区分と日付区分から対象テーブルを設定)
            //************************************************************************************
            SetTargetProductsTables( ref param, productKindCd, modelType, dateKindCd );

            //************************************************************************************
            //部品テーブルの結合有無(トラクタ部品:クレート/ロプスのみ)
            //************************************************************************************
            if ( productKindCd == ProductKind.Tractor ) {
                param.appendPartsCd = partsCd;
            }

            //************************************************************************************
            //日付範囲設定(副検索時でも日付範囲は指定する)
            //************************************************************************************
            if ( DateKind.ProductClass.ToBeComplete.value == dateKindCd ) {
                //完成予定日
                param.paramPlanDtFrom = GetSearchFromDate( dtFrom );                  //FROM
                param.paramPlanDtTo = GetSearchToDate( dtTo );                      //TO
            } else if ( DateKind.ProductClass.Completed.value == dateKindCd ) {
                //完成日
                param.paramProductDtFrom = GetSearchFromDate( dtFrom );             //FROM
                param.paramProductDtTo = GetSearchToDate( dtTo );                   //TO
            } else if ( DateKind.ProductClass.Shipped.value == dateKindCd ) {
                //出荷日
                param.paramShippedDtFrom = GetSearchFromDate( dtFrom );             //FROM
                param.paramShippedDtTo = GetSearchToDate( dtTo );                   //TO
            } else if ( DateKind.PartsClass.Installing.value == dateKindCd ) {
                //組付日(クレート/ロプスのみ)
                param.paramInstallDtFrom = GetSearchFromDate( dtFrom );             //FROM
                param.paramInstallDtTo = GetSearchToDate( dtTo );                   //TO
            }

            param.paramPinCdCheck = pinCdCheck;                                     //PINコード(チェックボックス)
            param.paramPinCd = pinCd;                                          //PINコード

            //************************************************************************************
            //製品情報取得
            //************************************************************************************
            param.searchFirstProduct = 0;                                           //改造前型式検索 [0=非対象/1=対象]
            for ( int transactionIndex = 0; transactionIndex < serialsList.Length; transactionIndex++ ) {
                //パラメータ(トランザクション別)設定
                if ( productKindCd == ProductKind.Tractor &&
                    ( partsCd.Equals( PartsKind.PARTS_CD_ENGINE_DPF ) || partsCd.Equals( PartsKind.PARTS_CD_ENGINE_SCR ) || partsCd.Equals( PartsKind.PARTS_CD_ENGINE_ACU ) ) ) {
                    param.paramSerialListForTractor = serialsList[transactionIndex];        //型式/機番リスト(トラクタのエンジン部品検索用)
                } else {
                    param.paramSerialList = serialsList[transactionIndex];                  //型式/機番リスト
                }
                //製品情報検索実行
                DataTable tblTemp = ProductsDao.SelectList( param );
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
        /// 検索対象製品テーブルの判定/設定処理
        /// </summary>
        /// <param name="param">Dao検索パラメータ</param>
        /// <param name="productKindCd">製品種別</param>
        /// <param name="modelType">型式種別</param>
        /// <param name="dateKindCd">日付区分</param>
        private static void SetTargetProductsTables( ref ProductsDao.SearchParameter param, string productKindCd, string modelType, string dateKindCd ) {
            //製品区分と日付区分から検索テーブルを判定する
            int searchProductingTractor = 0;        //生産中トラクタ参照
            int searchProductingEngine = 0;         //生産中エンジン参照

            int unionStockProducts = 0;             //在庫製品結合
            int searchStockTractor = 0;             //在庫トラクタ参照
            int searchStockEngine = 0;              //在庫エンジン参照
            int searchStockRotary = 0;              //生産中ロータリー参照

            int unionShippedProducts = 0;           //出荷済製品結合
            int searchShippedTractor = 0;           //出荷済トラクタ参照
            int searchShippedEngine = 0;            //出荷済エンジン参照
            int searchShippedRotary = 0;            //出荷済ロータリー参照

            //トラクタ
            if ( ProductKind.Tractor == productKindCd ) {
                if ( DateKind.ProductClass.ToBeComplete.value == dateKindCd ) {
                    //完成予定日
                    searchProductingTractor = 1;        //作業指示保存
                } else if ( DateKind.ProductClass.Completed.value == dateKindCd ) {
                    //完成日
                    searchStockTractor = 1;             //BIDM
                    unionShippedProducts = 1;           //UNION式を付加
                    searchShippedTractor = 1;           //機番情報F
                } else if ( DateKind.ProductClass.Shipped.value == dateKindCd ) {
                    //出荷日
                    searchShippedTractor = 1;           //機番情報F
                } else if ( DateKind.ProductClass.StockOnly.value == dateKindCd ) {
                    //在庫のみ
                    //完成予定日
                    searchProductingTractor = 1;        //作業指示保存
                    //完成日
                    searchStockTractor = 1;             //BIDM
                    unionStockProducts = 1;           //UNION式を付加
                } else {
                    //部品/工程の日付区分が選ばれている
                    searchProductingTractor = 1;        //作業指示保存
                    unionStockProducts = 1;             //UNION式を付加
                    searchStockTractor = 1;             //BIDM
                    unionShippedProducts = 1;           //UNION式を付加
                    searchShippedTractor = 1;           //機番情報F
                }
            }
            //エンジン
            if ( ProductKind.Engine == productKindCd ) {
                if ( DateKind.ProductClass.ToBeComplete.value == dateKindCd ) {
                    //完成予定日
                    searchProductingEngine = 1;         //作業指示保存
                } else if ( DateKind.ProductClass.Completed.value == dateKindCd ) {
                    //完成日
                    searchStockEngine = 1;              //BIDM
                    unionShippedProducts = 1;           //UNION式を付加
                    searchShippedEngine = 1;            //機番情報F
                } else if ( DateKind.ProductClass.Shipped.value == dateKindCd ) {
                    //出荷日
                    searchShippedEngine = 1;            //機番情報F
                } else if ( DateKind.ProductClass.StockOnly.value == dateKindCd ) {
                    //在庫のみ
                    //完成予定日
                    searchProductingEngine = 1;         //作業指示保存
                    //完成日
                    searchStockEngine = 1;              //BIDM
                    unionStockProducts = 1;           //UNION式を付加
                } else {
                    //部品/工程の日付区分が選ばれている
                    searchProductingEngine = 1;         //作業指示保存
                    unionStockProducts = 1;             //UNION式を付加
                    searchStockEngine = 1;              //BIDM
                    unionShippedProducts = 1;           //UNION式を付加
                    searchShippedEngine = 1;            //機番情報F
                }
            }
            //ロータリー
            if ( ProductKind.Rotary == productKindCd ) {
                if ( DateKind.ProductClass.Completed.value == dateKindCd ) {
                    //完成日
                    searchStockRotary = 1;              //BIDM
                    unionShippedProducts = 1;           //UNION式を付加
                    searchShippedRotary = 1;            //機番情報F
                } else if ( DateKind.ProductClass.Shipped.value == dateKindCd ) {
                    //出荷日
                    searchShippedRotary = 1;            //機番情報F
                }
            }
            param.searchProductingTractor = searchProductingTractor;            //生産中トラクタ参照
            param.searchProductingEngine = searchProductingEngine;              //生産中エンジン参照
            param.unionStockProducts = unionStockProducts;                      //在庫製品UNION結合
            param.searchStockTractor = searchStockTractor;                      //在庫トラクタ参照
            param.searchStockEngine = searchStockEngine;                        //在庫エンジン参照
            param.searchStockRotary = searchStockRotary;                        //在庫ロータリー参照
            param.unionShippedProducts = unionShippedProducts;                  //在庫製品UNION結合
            param.searchShippedTractor = searchShippedTractor;                  //在庫トラクタ参照
            param.searchShippedEngine = searchShippedEngine;                    //在庫エンジン参照
            param.searchShippedRotary = searchShippedRotary;                    //在庫ロータリー参照

            //作業指示保存検索対象判定
            if ( DateKind.ProductClass.ToBeComplete.value == dateKindCd ) {
                //完成予定日
                param.paramProductStatus = 0;        //仕掛
            } else if ( DateKind.ProductClass.Completed.value == dateKindCd ) {
                //完成日
                param.paramProductStatus = 1;        //未出荷(仕掛を含む)
            } else {
                //その他
                param.paramProductStatus = 1;        //未出荷(仕掛を含む)
            }
        }
        #endregion

        #region 部品一覧検索
        /// <summary>
        /// 部品一覧検索処理(主)
        /// </summary>
        /// <param name="condition">画面検索条件</param>
        /// <param name="modelCdList">型式名からの逆引き型式コードリスト</param>
        /// <param name="maxRecordCount">最大取得件数</param>
        /// <returns>部品DataTable</returns>
        private static DataTable SelectPartsList( Dictionary<string, object> condition, List<string> modelCdList, int maxRecordCount ) {
            DataTable tblParts = null;

            //画面パラメータの取得
            string productKindCd = DataUtils.GetDictionaryStringVal( condition, MainView.CONDITION.PRODUCT_KIND_CD.bindField );   //製品区分
            string partsCd = DataUtils.GetDictionaryStringVal( condition, MainView.CONDITION.PARTS_CD.bindField );                //部品区分
            string modelType = DataUtils.GetDictionaryStringVal( condition, MainView.CONDITION.MODEL_TYPE.bindField );            //型式種別
            string modelCd = DataUtils.GetModelCd( DataUtils.GetDictionaryStringVal( condition, MainView.CONDITION.MODEL_CD.bindField ) ); //型式コード
            string modelNm = DataUtils.GetDictionaryStringVal( condition, MainView.CONDITION.MODEL_NM.bindField );                //型式名
            string dateKindCd = DataUtils.GetDictionaryStringVal( condition, MainView.CONDITION.DATE_KIND_CD.bindField );         //日付区分
            DateTime dtFrom = DataUtils.GetDictionaryDateVal( condition, MainView.CONDITION.DATE_FROM.bindField );                //日付(FROM)
            DateTime dtTo = DataUtils.GetDictionaryDateVal( condition, MainView.CONDITION.DATE_TO.bindField );                    //日付(TO)
            string partsNum = DataUtils.GetDictionaryStringVal( condition, MainView.CONDITION.PARTS_NUM.bindField );              //部品品番
            string partsSerial = DataUtils.GetDictionaryStringVal( condition, MainView.CONDITION.PARTS_SERIAL.bindField );        //部品機番

            _logger.Info( "検索条件--->製品区分={0} 部品区分={1} 型式種別={2} 型式コード={3} 型式名={4} " +
                          "日付区分={5} 日付={6}～{7} 部品品番={8} 部品機番={9}",
                productKindCd, partsCd, modelType, modelCd, modelNm, dateKindCd, dtFrom, dtTo, partsNum, partsSerial );

            //検索実行(製品区分/部品区分から対象テーブルの一覧を取得)
            if ( productKindCd == ProductKind.Tractor ) {
                //トラクタ
                //検索パラメータ設定
                TractorPartsDao.SearchParameter param = new TractorPartsDao.SearchParameter();
                param.paramPartsCd = partsCd;                                                       //部品区分
                param.paramProductModelCd = modelCd;                                                //型式コード
                param.paramPartsNum = partsNum;                                                     //部品品番(クボタ品番)
                param.paramPartsSerial = partsSerial;                                               //部品機番
                if ( DateKind.PartsClass.Installing.value == dateKindCd ) {
                    //組付日指定
                    param.paramInstallDtFrom = GetSearchFromDate( dtFrom );                         //組付日(FROM)
                    param.paramInstallDtTo = GetSearchToDate( dtTo );                               //組付日(TO)
                }
                param.paramProductModelCdList = modelCdList;                                        //生産型式コードリスト

                // * トラクタ選択時に部品表示対応 *//
                EnginePartsDao.SearchParameter paramTrac = new EnginePartsDao.SearchParameter();
                paramTrac.paramPartsCd = partsCd;                                                       //部品区分
                paramTrac.paramProductModelCd = modelCd;                                                //型式コード
                paramTrac.paramPartsNum = partsNum;                                                     //部品品番(クボタ品番)
                paramTrac.paramPartsSerial = partsSerial;                                               //部品機番
                if ( DateKind.PartsClass.Installing.value == dateKindCd ) {
                    //組付日指定
                    paramTrac.paramInstallDtFrom = GetSearchFromDate( dtFrom );                         //組付日(FROM)
                    paramTrac.paramInstallDtTo = GetSearchToDate( dtTo );                               //組付日(TO)
                }
                paramTrac.paramProductModelCdList = modelCdList;                                        //生産型式コードリスト


                switch ( partsCd ) {
                case PartsKind.PARTS_CD_TRACTOR_CRATE:      //クレート
                case PartsKind.PARTS_CD_TRACTOR_ROPS:       //ロプス
                    //製品検索時に抽出する為、不要
                    break;
                case PartsKind.PARTS_CD_TRACTOR_WECU:       //WiFi ECU
                    //PAIRRSLT_F検索実行
                    tblParts = TractorPartsDao.SelectWiFiEcuList( param, maxRecordCount );
                    break;
                case PartsKind.PARTS_CD_TRACTOR_NAMEPLATE:  //銘板ラベル
                    tblParts = TractorPartsDao.SelectTractorNameplateList( param, maxRecordCount );
                    break;
                case PartsKind.PARTS_CD_TRACTOR_MISSION: //ミッション
                    tblParts = TractorPartsDao.SelectTractorMissionList( param, maxRecordCount );
                    break;
                case PartsKind.PARTS_CD_TRACTOR_HOUSING: //ハウジング
                    tblParts = TractorPartsDao.SelectTractorHousingList( param, maxRecordCount );
                    break;
                // * トラクタ選択時に部品表示対応 *//
                case PartsKind.PARTS_CD_ENGINE_DPF:           //DPF
                case PartsKind.PARTS_CD_ENGINE_SCR:           //SCR
                case PartsKind.PARTS_CD_ENGINE_ACU:           //ACU
                    //ENGBKJ_F検索実行
                    tblParts = EnginePartsDao.SelectPartsList( paramTrac, maxRecordCount );
                    break;
                default:
                    if ( true == partsCd.StartsWith( PartsKind.PARTS_CD_TRACTOR_PREFIX_COREPARTS ) ) {
                        // 基幹部品
                        tblParts = TractorPartsDao.SelectTractorCorePartsList( param, maxRecordCount );
                    }
                    break;
                }

            } else if ( productKindCd == ProductKind.Engine ) {
                //エンジン

                //検索パラメータ設定
                EnginePartsDao.SearchParameter param = new EnginePartsDao.SearchParameter();
                param.paramPartsCd = partsCd;                                                       //部品区分
                param.paramProductModelCd = modelCd;                                                //型式コード
                param.paramPartsNum = partsNum;                                                     //部品品番(クボタ品番)
                param.paramPartsSerial = partsSerial;                                               //部品機番
                if ( DateKind.PartsClass.Installing.value == dateKindCd ) {
                    //組付日指定
                    param.paramInstallDtFrom = GetSearchFromDate( dtFrom );                         //組付日(FROM)
                    param.paramInstallDtTo = GetSearchToDate( dtTo );                               //組付日(TO)
                } else if ( DateKind.PartsClass.Processing.value == dateKindCd ) {
                    //加工日指定(TT_SQ_3C_DETAIL結合が必要)
                    param.paramProcessDtFrom = GetSearchFromDate( dtFrom );                         //加工日(FROM)
                    param.paramProcessDtTo = GetSearchToDate( dtTo );                               //加工日(TO)
                }
                //TODO:加工日=999999の指定条件が必要(param.paramPassedRegist)
                param.paramProductModelCdList = modelCdList;                                        //生産型式コードリスト

                switch ( partsCd ) {
                case PartsKind.PARTS_CD_ENGINE_SUPPLYPUMP:    //サプライポンプ
                case PartsKind.PARTS_CD_ENGINE_IPU:           //IPU
                case PartsKind.PARTS_CD_ENGINE_EHC:           //EHC
                case PartsKind.PARTS_CD_ENGINE_ECU:           //ECU
                case PartsKind.PARTS_CD_ENGINE_INJECTOR:      //インジェクタ
                case PartsKind.PARTS_CD_ENGINE_DPF:           //DPF
                case PartsKind.PARTS_CD_ENGINE_EPR:           //EPR
                case PartsKind.PARTS_CD_ENGINE_MIXER:         //MIXER
                case PartsKind.PARTS_CD_ENGINE_SCR:           //SCR
                case PartsKind.PARTS_CD_ENGINE_DOC:           //DOC
                case PartsKind.PARTS_CD_ENGINE_ACU:           //ACU
                case PartsKind.PARTS_CD_ENGINE_RACK_POSITION_SENSOR:  //ラック位置センサ
                    //ENGBKJ_F検索実行
                    tblParts = EnginePartsDao.SelectPartsList( param, maxRecordCount );
                    break;
                case PartsKind.PARTS_CD_ENGINE_SERIALPRINT:   //SERIALPRINT
                    TractorPartsDao.SearchParameter param1 = new TractorPartsDao.SearchParameter();
                    param1.paramProductModelCd = modelCd;                                                //型式コード
                    param1.paramPartsNum = partsNum;                                                     //部品品番(クボタ品番)
                    param1.paramPartsSerial = partsSerial;                                               //部品機番
                    tblParts = TractorPartsDao.SelectEngineSerialPrintList( param1, maxRecordCount );
                    break;
                case PartsKind.PARTS_CD_ENGINE_CC:            //クランクケース
                case PartsKind.PARTS_CD_ENGINE_CYH:           //シリンダヘッド
                case PartsKind.PARTS_CD_ENGINE_CS:            //クランクシャフト
                    //ENGBKJ_F検索実行
                    tblParts = EnginePartsDao.Select3cList( param, maxRecordCount );
                    break;
                default:
                    if ( true == partsCd.StartsWith( PartsKind.PARTS_CD_TRACTOR_PREFIX_COREPARTS ) ) {
                        // 基幹部品
                        tblParts = EnginePartsDao.SelectEngineCorePartsList( param, maxRecordCount );
                    }
                    break;
                }
            }

            return tblParts;
        }

        /// <summary>
        /// 部品一覧検索処理(副)
        /// 製品情報の型式/機番から対象データを抽出する
        /// </summary>
        /// <param name="condition">画面検索条件</param>
        /// <param name="tblProducts">製品検索結果</param>
        /// <param name="maxRecordCount">最大取得件数</param>
        /// <returns>部品DataTable</returns>
        private static DataTable SelectPartsList( Dictionary<string, object> condition, DataTable tblProducts, int maxRecordCount ) {
            DataTable tblParts = null;

            //画面パラメータの取得
            string productKindCd = DataUtils.GetDictionaryStringVal( condition, MainView.CONDITION.PRODUCT_KIND_CD.bindField );   //製品区分
            string partsCd = DataUtils.GetDictionaryStringVal( condition, MainView.CONDITION.PARTS_CD.bindField );                //部品区分
            string partsNum = DataUtils.GetDictionaryStringVal( condition, MainView.CONDITION.PARTS_NUM.bindField );              //部品品番
            string partsSerial = DataUtils.GetDictionaryStringVal( condition, MainView.CONDITION.PARTS_SERIAL.bindField );        //部品機番

            _logger.Info( "検索条件--->製品区分={0} 部品区分={1} 部品品番={2} 部品機番={3} ※型式/機番指定", productKindCd, partsCd, partsNum, partsSerial );

            //製品一覧から型式/機番リストを取得

            List<SerialParam>[] serialsList = null;
            if ( productKindCd == ProductKind.Engine || partsCd.Equals( PartsKind.PARTS_CD_TRACTOR_WECU ) || partsCd.Equals( PartsKind.PARTS_CD_TRACTOR_NAMEPLATE ) ||
                partsCd.Equals( PartsKind.PARTS_CD_TRACTOR_MISSION ) || partsCd.Equals( PartsKind.PARTS_CD_TRACTOR_HOUSING ) || partsCd.StartsWith( PartsKind.PARTS_CD_TRACTOR_PREFIX_COREPARTS ) ) {
                serialsList = GetSerialsList( tblProducts );
            } else {
                //トラクタで[エンジン]サプライポンプ[01]～[エンジン]ACU[13]が選択されている場合
                serialsList = GetSerialsTractorList( tblProducts );
            }

            //            List<SerialParam>[] serialsList = GetSerialsList( tblProducts );

            //************************************************************************************
            //部品情報取得
            //************************************************************************************
            for ( int transactionIndex = 0; transactionIndex < serialsList.Length; transactionIndex++ ) {
                DataTable tblTemp = null;                                                   //一時格納用DataTable

                //検索実行(製品区分/部品区分から対象テーブルの一覧を取得)
                if ( productKindCd == ProductKind.Tractor ) {
                    //トラクタ

                    //検索パラメータ設定
                    TractorPartsDao.SearchParameter param = new TractorPartsDao.SearchParameter();
                    param.paramPartsCd = partsCd;                                           //部品区分
                    param.paramPartsNum = partsNum;                                         //部品品番(クボタ品番)
                    param.paramPartsSerial = partsSerial;                                   //部品機番
                    param.paramSerialList = serialsList[transactionIndex];                  //型式/機番リスト

                    EnginePartsDao.SearchParameter paramTrac = new EnginePartsDao.SearchParameter();
                    paramTrac.paramPartsCd = partsCd;                                           //部品区分
                    paramTrac.paramPartsNum = partsNum;                                         //部品品番(クボタ品番)
                    paramTrac.paramPartsSerial = partsSerial;                                   //部品機番
                    paramTrac.paramSerialList = serialsList[transactionIndex];                  //型式/機番リスト

                    switch ( partsCd ) {
                    case PartsKind.PARTS_CD_TRACTOR_CRATE:      //クレート
                    case PartsKind.PARTS_CD_TRACTOR_ROPS:       //ロプス
                        //製品検索時に抽出する為、不要
                        break;
                    case PartsKind.PARTS_CD_TRACTOR_WECU:       //WiFi ECU
                        //PAIRRSLT_F検索実行
                        tblTemp = TractorPartsDao.SelectWiFiEcuList( param );
                        break;
                    case PartsKind.PARTS_CD_TRACTOR_NAMEPLATE:  //銘板ラベル
                        //検索実行
                        tblTemp = TractorPartsDao.SelectTractorNameplateList( param );
                        break;
                    case PartsKind.PARTS_CD_TRACTOR_MISSION: //ミッション
                        tblTemp = TractorPartsDao.SelectTractorMissionList( param );
                        break;
                    case PartsKind.PARTS_CD_TRACTOR_HOUSING: //ハウジング
                        tblTemp = TractorPartsDao.SelectTractorHousingList( param );
                        break;
                    // * トラクタ選択時に部品表示対応 *//
                    case PartsKind.PARTS_CD_ENGINE_DPF:         //DPF
                    case PartsKind.PARTS_CD_ENGINE_SCR:         //SCR
                    case PartsKind.PARTS_CD_ENGINE_ACU:         //ACU
                        //ENGBKJ_F検索実行
                        tblTemp = EnginePartsDao.SelectPartsList( paramTrac );
                        break;
                    default:
                        if ( true == partsCd.StartsWith( PartsKind.PARTS_CD_TRACTOR_PREFIX_COREPARTS ) ) {
                            // 基幹部品
                            tblTemp = TractorPartsDao.SelectTractorCorePartsList( param );
                        }
                        break;
                    }

                } else if ( productKindCd == ProductKind.Engine ) {
                    //エンジン

                    //検索パラメータ設定
                    EnginePartsDao.SearchParameter param = new EnginePartsDao.SearchParameter();
                    param.paramPartsCd = partsCd;                                           //部品区分
                    param.paramPartsNum = partsNum;                                         //部品品番(クボタ品番)
                    param.paramPartsSerial = partsSerial;                                   //部品機番
                    param.paramSerialList = serialsList[transactionIndex];                  //型式/機番リスト

                    switch ( partsCd ) {
                    case PartsKind.PARTS_CD_ENGINE_SUPPLYPUMP:    //サプライポンプ
                    case PartsKind.PARTS_CD_ENGINE_IPU:           //IPU
                    case PartsKind.PARTS_CD_ENGINE_EHC:           //EHC
                    case PartsKind.PARTS_CD_ENGINE_ECU:           //ECU
                    case PartsKind.PARTS_CD_ENGINE_INJECTOR:      //インジェクタ
                    case PartsKind.PARTS_CD_ENGINE_DPF:           //DPF
                    case PartsKind.PARTS_CD_ENGINE_EPR:           //EPR
                    case PartsKind.PARTS_CD_ENGINE_MIXER:         //MIXER
                    case PartsKind.PARTS_CD_ENGINE_SCR:           //SCR
                    case PartsKind.PARTS_CD_ENGINE_DOC:           //DOC
                    case PartsKind.PARTS_CD_ENGINE_ACU:           //ACU
                    case PartsKind.PARTS_CD_ENGINE_RACK_POSITION_SENSOR: //ラック位置センサ
                        //ENGBKJ_F検索実行
                        tblTemp = EnginePartsDao.SelectPartsList( param );
                        break;
                    case PartsKind.PARTS_CD_ENGINE_CC:            //クランクケース
                    case PartsKind.PARTS_CD_ENGINE_CYH:           //シリンダヘッド
                    case PartsKind.PARTS_CD_ENGINE_CS:            //クランクシャフト
                        //ENGBKJ_F検索実行
                        tblTemp = EnginePartsDao.Select3cList( param );
                        break;
                    case PartsKind.PARTS_CD_ENGINE_SERIALPRINT:   //SERIALPRINT
                        //ENGBKJ_F検索実行
                        tblTemp = EnginePartsDao.SelectSerialPrintList( param );
                        break;
                    default:
                        if ( true == partsCd.StartsWith( PartsKind.PARTS_CD_TRACTOR_PREFIX_COREPARTS ) ) {
                            // 基幹部品
                            tblTemp = EnginePartsDao.SelectEngineCorePartsList( param );
                        }
                        break;
                    }
                }

                if ( null != tblTemp ) {
                    if ( null == tblParts ) {
                        //1回目のトランザクション
                        tblParts = tblTemp;
                    } else {
                        //取得した行をインポート
                        foreach ( DataRow row in tblTemp.Rows ) {
                            tblParts.ImportRow( row );
                        }
                    }
                }

                if ( null != tblParts && maxRecordCount <= tblParts.Rows.Count ) {
                    //最大取得件数超過(取得処理中断)
                    break;
                }
            }
            if ( null != tblParts ) {
                tblParts.AcceptChanges();
            }

            return tblParts;
        }
        #endregion

        #region 工程一覧検索
        /// <summary>
        /// 工程検索処理(主)
        /// </summary>
        /// <param name="condition">画面検索条件</param>
        /// <param name="modelCdList">型式名からの逆引き型式コードリスト</param>
        /// <param name="maxRecordCount">最大取得件数</param>
        /// <returns>工程DataTable</returns>
        private static DataTable SelectProcessList( Dictionary<string, object> condition, List<string> modelCdList, int maxRecordCount ) {
            DataTable tblProcess = null;

            //画面パラメータの取得
            string lineCd = DataUtils.GetDictionaryStringVal( condition, MainView.CONDITION.LINE_CD.bindField );                  //ラインコード
            string workCd = DataUtils.GetDictionaryStringVal( condition, MainView.CONDITION.WORK_CD.bindField );                  //作業コード
            string productKindCd = DataUtils.GetDictionaryStringVal( condition, MainView.CONDITION.PRODUCT_KIND_CD.bindField );   //製品区分
            string processCd = DataUtils.GetDictionaryStringVal( condition, MainView.CONDITION.PROCESS_CD.bindField );            //工程区分
            string modelType = DataUtils.GetDictionaryStringVal( condition, MainView.CONDITION.MODEL_TYPE.bindField );            //型式種別
            string modelCd = DataUtils.GetModelCd( DataUtils.GetDictionaryStringVal( condition, MainView.CONDITION.MODEL_CD.bindField ) ); //型式コード
            string modelNm = DataUtils.GetDictionaryStringVal( condition, MainView.CONDITION.MODEL_NM.bindField );                //型式名
            string dateKindCd = DataUtils.GetDictionaryStringVal( condition, MainView.CONDITION.DATE_KIND_CD.bindField );         //日付区分
            DateTime dtFrom = DataUtils.GetDictionaryDateVal( condition, MainView.CONDITION.DATE_FROM.bindField );                //日付(FROM)
            DateTime dtTo = DataUtils.GetDictionaryDateVal( condition, MainView.CONDITION.DATE_TO.bindField );                    //日付(TO)

            _logger.Info( "検索条件--->製品区分={0} 工程区分={1} 型式種別={2} 型式コード={3} 型式名={4} 日付区分={5} 日付={6}～{7} ラインコード={8} 作業コード={9}",
                productKindCd, processCd, modelType, modelCd, modelNm, dateKindCd, dtFrom, dtTo, lineCd, workCd );
            // 汎用テーブル使用フラグ
            var genericTableUseFlag = false;
            if ( false == StringUtils.IsEmpty( processCd ) ) {
                // 工程コードがNULLでない場合
                if ( 0 != GenericTableDao.SelectSearchTargetFlag( lineCd, processCd ) ) {
                    // 検索対象フラグONの検索結果が存在する場合、汎用テーブル使用フラグをON
                    genericTableUseFlag = true;
                } else {
                    // OFFの場合、工程コードを2桁工程コードに変換する
                    processCd = DataUtils.GetInspectionCd( lineCd: lineCd, processCd: processCd, productKind: productKindCd ) ?? processCd;
                }
            }
            //検索実行(製品区分/部品区分から対象テーブルの一覧を取得)
            if ( true == genericTableUseFlag ) {
                // 汎用テーブル使用フラグがONの場合
                // 汎用テーブル情報検索
                tblProcess = GenericTableDao.SelectGenericTable( new GenericTableDao.SearchParameter() {
                    // ラインコード
                    LineCd = lineCd,
                    // 工程コード
                    ProcessCd = processCd,
                    // 作業コード
                    WorkCd = workCd,
                    // 日付From
                    DtFrom = dtFrom,
                    // 日付To
                    DtTo = dtTo,
                    // 型式機番リスト
                    ModelCdList = modelCdList
                } );
            } else {
                if ( productKindCd == ProductKind.Tractor ) {
                    //トラクタ
                    //検索パラメータ設定
                    TractorProcessDao.SearchParameter param = new TractorProcessDao.SearchParameter();
                    param.paramProductModelCd = modelCd;                                                //型式コード
                    if ( DateKind.ProcessClass.InspectionMeasuring.value == dateKindCd ) {
                        //組付日指定
                        param.paramInspectionDtFrom = GetSearchFromDate( dtFrom );                      //測定日(FROM)
                        param.paramInspectionDtTo = GetSearchToDate( dtTo );                            //測定日(TO)
                    }
                    param.paramProductModelCdList = modelCdList;                                        //生産型式コードリスト

                    switch ( processCd ) {
                    case ProcessKind.PROCESS_CD_TRACTOR_PCRAWER:    //パワクロ走行検査
                        tblProcess = TractorProcessDao.SelectPCrawlerList( param, maxRecordCount );
                        break;
                    case ProcessKind.PROCESS_CD_TRACTOR_CHKSHEET:   //チェックシート
                        tblProcess = TractorProcessDao.SelectCheckSheetList( param, maxRecordCount );
                        break;
                    case ProcessKind.PROCESS_CD_TRACTOR_CAMIMAGE:   //品質画像証跡
                        tblProcess = TractorProcessDao.SelectCamImageList( param, maxRecordCount );
                        break;
                    case ProcessKind.PROCESS_CD_TRACTOR_ELCHECK:    //電子チェックシート
                        tblProcess = TractorProcessDao.SelectELCheckList( param, maxRecordCount );
                        if ( tblProcess.Rows.Count > 0 ) {
                            tblProcess = JoinEmpInfo( tblProcess );
                        }
                        break;
                    case ProcessKind.PROCESS_CD_TRACTOR_SHEEL:        //刻印
                        tblProcess = TractorProcessDao.SelectPrintSheelList( param, maxRecordCount );
                        break;
                    case ProcessKind.PROCESS_CD_TRACTOR_NUTRUNNER:  //ナットランナー
                        tblProcess = TractorProcessDao.SelectNutRunnerList( param, maxRecordCount );
                        break;
                    case ProcessKind.PROCESS_CD_TRACTOR_ALL:  //トラクタ走行検査
                        tblProcess = TractorProcessDao.SelectTractorAllList( param, maxRecordCount );
                        break;
                    case ProcessKind.PROCESS_CD_TRACTOR_OPTAXIS:  //光軸検査
                        tblProcess = TractorProcessDao.SelectOptaxisList( param, maxRecordCount );
                        break;
                    case ProcessKind.PROCESS_CD_TRACTOR_CHKPOINT:  //関所
                        DataTable modelList = TractorProcessDao.SelectWorkResultModelList( param, lineCd );
                        if ( modelList.Rows.Count == 0 ) {
                            tblProcess = modelList;
                        } else {
                            //製造支援一覧から型式/機番リストを取得
                            List<SerialParam>[] modelSerialsList = GetSerialsList( modelList );
                            DataTable tblCheckPoint = new DataTable();
                            for ( int i = 0; i < modelSerialsList.Length; i++ ) {
                                param.paramSerialList = modelSerialsList[i];
                                tblCheckPoint = TractorProcessDao.SelectWorkResultList( param, lineCd );
                                if ( null != tblCheckPoint ) {
                                    tblCheckPoint.AcceptChanges();
                                    if ( null == tblProcess ) {
                                        //1回目のトランザクション
                                        tblProcess = tblCheckPoint;
                                    } else {
                                        //取得した行をインポート
                                        foreach ( DataRow row in tblCheckPoint.Rows ) {
                                            tblProcess.ImportRow( row );
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    case ProcessKind.PROCESS_CD_TRACTOR_IMGCHECK:   //イメージチェックシート
                        tblProcess = TractorProcessDao.SelectImgCheckList( param, maxRecordCount );
                        if ( 0 < tblProcess.Rows.Count ) {
                            tblProcess = JoinEmpInfo( tblProcess );
                        }
                        break;
                    case ProcessKind.PROCESS_CD_TRACTOR_TESTBENCH:  // 検査ベンチ

                        break;
                    case ProcessKind.PROCESS_CD_TRACTOR_AIIMAGE:  // AI画像解析
                        tblProcess = TractorProcessDao.SelectAiImageList( param, maxRecordCount );
                        break;
                    default:
                        break;
                    }

                } else if ( productKindCd == ProductKind.Engine ) {
                    //エンジン

                    //検索パラメータ設定
                    EngineProcessDao.SearchParameter param = new EngineProcessDao.SearchParameter();
                    param.paramProductModelCd = modelCd;                                                //型式コード
                    if ( DateKind.ProcessClass.InspectionMeasuring.value == dateKindCd ) {
                        //組付日指定
                        param.paramInspectionDtFrom = GetSearchFromDate( dtFrom );                      //測定日(FROM)
                        param.paramInspectionDtTo = GetSearchToDate( dtTo );                            //測定日(TO)
                    }
                    param.paramProductModelCdList = modelCdList;                                        //生産型式コードリスト

                    switch ( processCd ) {
                    case ProcessKind.PROCESS_CD_ENGINE_TORQUE:      //トルク締付
                        tblProcess = EngineProcessDao.SelectTorqueList( param, maxRecordCount );
                        break;
                    case ProcessKind.PROCESS_CD_ENGINE_HARNESS:     //ハーネス検査
                        tblProcess = EngineProcessDao.SelectHarnessList( param, maxRecordCount );
                        break;
                    case ProcessKind.PROCESS_CD_ENGINE_TEST:        //エンジン運転測定
                        tblProcess = EngineProcessDao.SelectEngineTestList( param, maxRecordCount );
                        break;
                    case ProcessKind.PROCESS_CD_ENGINE_FRICTION:    //フリクションロス
                        tblProcess = EngineProcessDao.SelectFrictionLossList( param, maxRecordCount );
                        break;
                    //case ProcessKind.PROCESS_CD_ENGINE_CYH_ASSEMBLY://シリンダヘッド組付
                    //    tblProcess = EngineProcessDao.SelectCyhAssemblyList( param, maxRecordCount );
                    //    break;
                    case ProcessKind.PROCESS_CD_ENGINE_CYH_INSPECT: //シリンダヘッド精密測定
                        param.paramPartsCd = PartsKind.PARTS_CD_ENGINE_CYH;                             //CYH部品区分コード
                        tblProcess = EngineProcessDao.Select3cInspectionList( param, maxRecordCount );
                        break;
                    case ProcessKind.PROCESS_CD_ENGINE_CS_INSPECT:  //クランクシャフト精密測定
                        param.paramPartsCd = PartsKind.PARTS_CD_ENGINE_CS;                              //CS部品区分コード
                        tblProcess = EngineProcessDao.Select3cInspectionList( param, maxRecordCount );
                        break;
                    case ProcessKind.PROCESS_CD_ENGINE_CC_INSPECT:  //クランクケース精密測定
                        param.paramPartsCd = PartsKind.PARTS_CD_ENGINE_CC;                              //CC部品区分コード
                        tblProcess = EngineProcessDao.Select3cInspectionList( param, maxRecordCount );
                        break;
                    case ProcessKind.PROCESS_CD_ENGINE_INJECTION:   //噴射時期計測
                        tblProcess = EngineProcessDao.SelectInjectionList( param, maxRecordCount );
                        break;
                    case ProcessKind.PROCESS_CD_ENGINE_CAMIMAGE:    //品質画像証跡
                        tblProcess = EngineProcessDao.SelectCamImageList( param, maxRecordCount );
                        break;
                    case ProcessKind.PROCESS_CD_ENGINE_ELCHECK:    //電子チェックシート
                        tblProcess = EngineProcessDao.SelectELCheckList( param, maxRecordCount );
                        if ( tblProcess.Rows.Count > 0 ) {
                            tblProcess = JoinEmpInfo( tblProcess );
                        }
                        break;
                    case ProcessKind.PROCESS_CD_ENGINE_AIIMAGE:    //AI画像解析
                        tblProcess = EngineProcessDao.SelectAiImageList( param, maxRecordCount );
                        break;
                    default:
                        break;
                    }
                }
            }
            return tblProcess;
        }

        /// <summary>
        /// 工程検索処理(副)
        /// 製品情報の型式/機番から対象データを抽出する
        /// </summary>
        /// <param name="condition">画面検索条件</param>
        /// <param name="tblProducts">製品検索結果</param>
        /// <param name="maxRecordCount">最大取得件数</param>
        /// <returns>工程DataTable</returns>
        private static DataTable SelectProcessList( Dictionary<string, object> condition, DataTable tblProducts, int maxRecordCount ) {
            DataTable tblProcess = null;

            //画面パラメータの取得
            string lineCd = DataUtils.GetDictionaryStringVal( condition, MainView.CONDITION.LINE_CD.bindField );                  //ラインコード
            string workCd = DataUtils.GetDictionaryStringVal( condition, MainView.CONDITION.WORK_CD.bindField );                  //作業コード
            string productKindCd = DataUtils.GetDictionaryStringVal( condition, MainView.CONDITION.PRODUCT_KIND_CD.bindField );   //製品区分
            string processCd = DataUtils.GetDictionaryStringVal( condition, MainView.CONDITION.PROCESS_CD.bindField );            //工程区分
            string dateKindCd = DataUtils.GetDictionaryStringVal( condition, MainView.CONDITION.DATE_KIND_CD.bindField );         //日付区分
            DateTime dtFrom = DataUtils.GetDictionaryDateVal( condition, MainView.CONDITION.DATE_FROM.bindField );                //日付(FROM)
            DateTime dtTo = DataUtils.GetDictionaryDateVal( condition, MainView.CONDITION.DATE_TO.bindField );                    //日付(TO)

            _logger.Info( "検索条件--->製品区分={0} 工程区分={1} ラインコード={2} 作業コード={3}", productKindCd, processCd, lineCd, workCd );

            //製品一覧から型式/機番リストを取得
            List<SerialParam>[] serialsList = GetSerialsList( tblProducts );
            // 汎用テーブル使用フラグ
            var genericTableUseFlag = false;
            if ( false == StringUtils.IsEmpty( processCd ) ) {
                // 工程コードがNULLでない場合
                if ( 0 != GenericTableDao.SelectSearchTargetFlag( lineCd, processCd ) ) {
                    // 検索対象フラグONの検索結果が存在する場合、汎用テーブル使用フラグをON
                    genericTableUseFlag = true;
                } else {
                    // OFFの場合、工程コードを2桁工程コードに変換する
                    processCd = DataUtils.GetInspectionCd( lineCd: lineCd, processCd: processCd, productKind: productKindCd ) ?? processCd;
                }
            }
            //************************************************************************************
            //部品情報取得
            //************************************************************************************
            for ( int transactionIndex = 0; transactionIndex < serialsList.Length; transactionIndex++ ) {
                DataTable tblTemp = null;                                                   //一時格納用DataTable
                if ( true == genericTableUseFlag ) {
                    // 汎用テーブル使用フラグがONの場合
                    // 汎用テーブル情報検索

                    GenericTableDao.SearchParameter searchParameter = new GenericTableDao.SearchParameter();
                    // ラインコード
                    searchParameter.LineCd = lineCd;
                    // 工程コード
                    searchParameter.ProcessCd = processCd;
                    // 作業コード
                    searchParameter.WorkCd = workCd;
                    // 型式機番リスト
                    searchParameter.SerialsList = serialsList[transactionIndex];
                    if ( DateKind.ProcessClass.InspectionMeasuring.value == dateKindCd ) {
                        //日付区分が「検査・計測」の場合、日付も検索条件に加える。
                        // 日付From
                        searchParameter.DtFrom = dtFrom;
                        // 日付To
                        searchParameter.DtTo = dtTo;
                    }

                    tblTemp = GenericTableDao.SelectGenericTable( searchParameter );

                } else {

                    //検索実行(製品区分/部品区分から対象テーブルの一覧を取得)
                    if ( productKindCd == ProductKind.Tractor ) {
                        //トラクタ

                        //検索パラメータ設定
                        TractorProcessDao.SearchParameter param = new TractorProcessDao.SearchParameter();
                        param.paramSerialList = serialsList[transactionIndex];                  //型式/機番リスト
                        if ( DateKind.ProcessClass.InspectionMeasuring.value == dateKindCd ) {
                            //組付日指定
                            param.paramInspectionDtFrom = GetSearchFromDate( dtFrom );                      //測定日(FROM)
                            param.paramInspectionDtTo = GetSearchToDate( dtTo );                            //測定日(TO)
                        }

                        switch ( processCd ) {
                        case ProcessKind.PROCESS_CD_TRACTOR_PCRAWER:    //パワクロ走行検査
                            tblTemp = TractorProcessDao.SelectPCrawlerList( param );
                            break;
                        case ProcessKind.PROCESS_CD_TRACTOR_CHKSHEET:   //チェックシート
                            tblTemp = TractorProcessDao.SelectCheckSheetList( param );
                            break;
                        case ProcessKind.PROCESS_CD_TRACTOR_CAMIMAGE:   //品質画像証跡
                            tblTemp = TractorProcessDao.SelectCamImageList( param );
                            break;
                        case ProcessKind.PROCESS_CD_TRACTOR_ELCHECK:   //電子チェックシート
                            tblTemp = TractorProcessDao.SelectELCheckList( param );
                            if ( tblTemp.Rows.Count > 0 ) {
                                tblTemp = JoinEmpInfo( tblTemp );
                            }
                            break;
                        case ProcessKind.PROCESS_CD_TRACTOR_SHEEL:        //刻印
                            tblTemp = TractorProcessDao.SelectPrintSheelList( param );
                            break;
                        case ProcessKind.PROCESS_CD_TRACTOR_NUTRUNNER:  //ナットランナー
                            tblTemp = TractorProcessDao.SelectNutRunnerList( param );
                            break;
                        case ProcessKind.PROCESS_CD_TRACTOR_ALL:  //トラクタ走行検査
                            tblTemp = TractorProcessDao.SelectTractorAllList( param );
                            break;
                        case ProcessKind.PROCESS_CD_TRACTOR_OPTAXIS:  //光軸検査
                            tblTemp = TractorProcessDao.SelectOptaxisList( param );
                            break;
                        case ProcessKind.PROCESS_CD_TRACTOR_CHKPOINT:  //関所
                            tblTemp = TractorProcessDao.SelectWorkResultList( param, lineCd );
                            break;
                        case ProcessKind.PROCESS_CD_TRACTOR_IMGCHECK:   //イメージチェックシート
                            tblTemp = TractorProcessDao.SelectImgCheckList( param );
                            if ( tblTemp.Rows.Count > 0 ) {
                                tblTemp = JoinEmpInfo( tblTemp );
                            }
                            break;
                        case ProcessKind.PROCESS_CD_TRACTOR_TESTBENCH:  //検査ベンチ
                            tblTemp = TractorProcessDao.SelectTestBenchList( param );
                            break;
                        case ProcessKind.PROCESS_CD_TRACTOR_AIIMAGE:    //AI画像解析
                            tblTemp = TractorProcessDao.SelectAiImageList( param );
                            break;
                        default:
                            break;
                        }

                    } else if ( productKindCd == ProductKind.Engine ) {
                        //エンジン

                        //検索パラメータ設定
                        EngineProcessDao.SearchParameter param = new EngineProcessDao.SearchParameter();
                        param.paramSerialList = serialsList[transactionIndex];                  //型式/機番リスト

                        switch ( processCd ) {
                        case ProcessKind.PROCESS_CD_ENGINE_TORQUE:      //トルク締付
                            tblTemp = EngineProcessDao.SelectTorqueList( param );
                            break;
                        case ProcessKind.PROCESS_CD_ENGINE_HARNESS:     //ハーネス検査
                            tblTemp = EngineProcessDao.SelectHarnessList( param );
                            break;
                        case ProcessKind.PROCESS_CD_ENGINE_TEST:        //エンジン運転測定
                            tblTemp = EngineProcessDao.SelectEngineTestList( param );
                            break;
                        case ProcessKind.PROCESS_CD_ENGINE_FRICTION:    //フリクションロス
                            tblTemp = EngineProcessDao.SelectFrictionLossList( param );
                            break;
                        //case ProcessKind.PROCESS_CD_ENGINE_CYH_ASSEMBLY://シリンダヘッド組付
                        //    tblTemp = EngineProcessDao.SelectCyhAssemblyList( param );
                        //    break;
                        case ProcessKind.PROCESS_CD_ENGINE_CYH_INSPECT: //シリンダヘッド精密測定
                            param.paramPartsCd = PartsKind.PARTS_CD_ENGINE_CYH;                             //CYH部品区分コード
                            tblTemp = EngineProcessDao.Select3cInspectionList( param );
                            break;
                        case ProcessKind.PROCESS_CD_ENGINE_CS_INSPECT:  //クランクシャフト精密測定
                            param.paramPartsCd = PartsKind.PARTS_CD_ENGINE_CS;                              //CS部品区分コード
                            tblTemp = EngineProcessDao.Select3cInspectionList( param );
                            break;
                        case ProcessKind.PROCESS_CD_ENGINE_CC_INSPECT:  //クランクケース精密測定
                            param.paramPartsCd = PartsKind.PARTS_CD_ENGINE_CC;                              //CC部品区分コード
                            tblTemp = EngineProcessDao.Select3cInspectionList( param );
                            break;
                        case ProcessKind.PROCESS_CD_ENGINE_INJECTION:   //噴射時期計測
                            tblTemp = EngineProcessDao.SelectInjectionList( param );
                            break;
                        case ProcessKind.PROCESS_CD_ENGINE_CAMIMAGE:    //品質画像証跡
                            tblTemp = EngineProcessDao.SelectCamImageList( param );
                            break;
                        case ProcessKind.PROCESS_CD_ENGINE_ELCHECK:    //電子チェックシート
                            tblTemp = EngineProcessDao.SelectELCheckList( param );
                            if ( tblTemp.Rows.Count > 0 ) {
                                tblTemp = JoinEmpInfo( tblTemp );
                            }
                            break;
                        case ProcessKind.PROCESS_CD_ENGINE_AIIMAGE:    //AI画像解析
                            tblTemp = EngineProcessDao.SelectAiImageList( param );
                            break;
                        default:
                            break;
                        }
                    }
                }

                if ( null != tblTemp ) {
                    tblTemp.AcceptChanges();

                    if ( null == tblProcess ) {
                        //1回目のトランザクション
                        tblProcess = tblTemp;
                    } else {
                        //取得した行をインポート
                        foreach ( DataRow row in tblTemp.Rows ) {
                            tblProcess.ImportRow( row );
                        }
                    }
                }

                if ( null != tblProcess && maxRecordCount <= tblProcess.Rows.Count ) {
                    //最大取得件数超過(取得処理中断)
                    break;
                }
            }
            if ( null != tblProcess ) {
                tblProcess.AcceptChanges();
            }

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

                //左外部結合(型式/機番で結合)
                //var rows = from l in tblLeft.AsEnumerable()
                //           join r in tblRight.AsEnumerable()
                //           on
                //            new { modelCd = StringUtils.ToString( l["productModelCd"] ), serial = StringUtils.ToString( l["serial6"] ) }
                //           equals
                //            new { modelCd = StringUtils.ToString( r["productModelCd"] ), serial = StringUtils.ToString( r["serial6"] ) }
                //           into rgrps
                //           from rgrp in rgrps.DefaultIfEmpty( null )
                //           select new {
                //               lrow = l,
                //               rrow = rgrp
                //           };
                //内部結合(型式/機番で結合)
                var rows = from l in tblLeft.AsEnumerable()
                           join r in tblRight.AsEnumerable()
                           on
                            new { modelCd = StringUtils.ToString( l["productModelCd"] ), serial = StringUtils.ToString( l["serial6"] ) }
                           equals
                            new { modelCd = StringUtils.ToString( r["productModelCd"] ), serial = StringUtils.ToString( r["serial6"] ) }
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

        /// <summary>
        /// 製品テーブルと部品テーブルの結合処理(型式/機番による内部結合)　※トラクタ選択時の結合
        /// </summary>
        /// <param name="gridColumns">結合結果出力DataTable列定義</param>
        /// <param name="tblLeft">左辺結合DataTable(列定義にproductModelCd/serial6が必要)</param>
        /// <param name="tblRight">右辺結合DataTable(列定義にproductModelCd/serial6が必要)</param>
        /// <returns>結合済みDataTable</returns>
        private static DataTable GetJoinTractorTable( GridViewDefine[] gridColumns, DataTable tblLeft, DataTable tblRight ) {
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
                //内部結合(型式/機番で結合)
                var rows = from l in tblLeft.AsEnumerable()
                           join r in tblRight.AsEnumerable()
                           on
                            new { modelCd = StringUtils.ToString( l["engineModelCd"] ), serial = StringUtils.ToString( l["engineSerial6"] ) }
                           equals
                            new { modelCd = StringUtils.ToString( r["productModelCd"] ), serial = StringUtils.ToString( r["serial6"] ) }
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

        /// <summary>
        /// 製品テーブルと部品テーブルの結合処理(型式/機番による内部結合)　※トラクタ選択時の結合
        /// </summary>
        /// <param name="gridColumns">結合結果出力DataTable列定義</param>
        /// <param name="tblLeft">左辺結合DataTable(列定義にproductModelCd/serial6が必要)</param>
        /// <param name="tblRight">右辺結合DataTable(列定義にproductModelCd/serial6が必要)</param>
        /// <returns>結合済みDataTable</returns>
        private static DataTable GetJoinTractorTable2( GridViewDefine[] gridColumns, DataTable tblLeft, DataTable tblRight ) {
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
                //内部結合(型式/機番で結合)
                var rows = from l in tblLeft.AsEnumerable()
                           join r in tblRight.AsEnumerable()
                           on
                            new { modelCd = StringUtils.ToString( l["productModelCd"] ), serial = StringUtils.ToString( l["serial6"] ) }
                           equals
                            new { modelCd = StringUtils.ToString( r["engineModelCd"] ), serial = StringUtils.ToString( r["engineSerial6"] ) }
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

        #region 一覧検索補助
        /// <summary>
        /// 型式名(前方一致)から型式コードの一覧を取得
        /// </summary>
        /// <param name="modelNm">型式名(前方一致)</param>
        /// <returns>型式コードの一覧</returns>
        public static List<string> GetModelCdList( string modelNm ) {
            List<string> modelCdList = null;

            //型式コードが未指定の場合には、型式名から型式コードを逆引きする
            if ( false == StringUtils.IsBlank( modelNm ) ) {
                DataTable tbl = ModelDao.GetModelCdListByName( modelNm );
                if ( 0 < tbl.Rows.Count ) {
                    modelCdList = new List<string>();
                    foreach ( DataRow row in tbl.Rows ) {
                        string cd = StringUtils.ToString( row["modelCd"] );
                        if ( false == StringUtils.IsBlank( cd ) ) {
                            modelCdList.Add( cd );
                        }
                    }
                }
            }

            return modelCdList;
        }

        /// <summary>
        /// トランザクションごとに分割した主テーブルの型式/機番リストを作成
        /// </summary>
        /// <param name="tblMain">型式(productModelCd)/機番(serial6)を格納したDataTable</param>
        /// <returns>型式/機番リスト(配列はトランザクション分作成)</returns>
        private static List<SerialParam>[] GetSerialsList( DataTable tblMain ) {

            if ( tblMain == null ) {
                return new List<SerialParam>[0];
            }

            //製品一覧から型式/機番リストを取得

            //重複削除(7桁機番は工程/部品テーブルからは返されない為注意)
            var serials = (
                from row in tblMain.AsEnumerable()
                select new {
                    productModelCd = row["productModelCd"],
                    serial6 = row["serial6"],
                    serial = tblMain.Columns.Contains( "serial" ) ? row["serial"] : row["serial6"]
                }
                ).Distinct();

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
        /// トランザクションごとに分割した主テーブルのエンジン型式/エンジン機番リストを作成(トラクタ選択時の)
        /// </summary>
        /// <param name="tblMain">型式(productModelCd)/機番(serial6)を格納したDataTable</param>
        /// <returns>エンジン型式/エンジン機番リスト(配列はトランザクション分作成)</returns>
        private static List<SerialParam>[] GetSerialsTractorList( DataTable tblMain ) {
            //製品一覧から型式/機番リストを取得

            //重複削除(7桁機番は工程/部品テーブルからは返されない為注意)
            var serials = (
                from row in tblMain.AsEnumerable()
                select new {
                    productModelCd = row["engineModelCd"],
                    serial6 = row["engineSerial6"],
                    serial = tblMain.Columns.Contains( "engineSerial" ) ? row["engineSerial"] : row["engineSerial6"]
                }
                ).Distinct();

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

        /// <summary>
        /// 従業員番号に紐づく従業員名を結合する
        /// </summary>
        /// <param name="dt"></param>
        private static DataTable JoinEmpInfo( DataTable dt ) {
            DataTable tblTemp = null;
            bool isExistStartBy = dt.Columns.Contains( "startBy" );
            bool isExistUpdateBy = dt.Columns.Contains( "updateBy" );

            //勤怠従業員情報を取得
            tblTemp = KintaiDao.SelectByList( null, null );
            Dictionary<String, String> _dicEmp = new Dictionary<String, String>();

            foreach ( DataRow dr in tblTemp.Rows ) {
                _dicEmp.Add( dr["EMP_NO"].ToString().Trim(), dr["EMP_NM"].ToString().Trim() );
            }

            foreach ( DataRow dr in dt.Rows ) {
                string strEmpCd = StringUtils.ToString( dr["judgeEmpCd"] );
                string strStartByCd = string.Empty;
                string strUpdateByCd = string.Empty;

                //合格判定社員名
                if ( StringUtils.IsNotEmpty( strEmpCd ) ) {
                    if ( true == _dicEmp.ContainsKey( strEmpCd ) ) {
                        dr["judgeEmpCd"] = strEmpCd + ":" + _dicEmp[strEmpCd].Trim();
                    }
                }

                //検査開始社員名
                if ( true == isExistStartBy ) {
                    strStartByCd = StringUtils.ToString( dr["startBy"] );
                    if ( StringUtils.IsNotEmpty( strStartByCd ) ) {
                        if ( true == _dicEmp.ContainsKey( strStartByCd ) ) {
                            dr["startBy"] = strStartByCd + ":" + _dicEmp[strStartByCd].Trim();
                        }
                    }
                }

                //更新社員名
                if ( true == isExistUpdateBy ) {
                    strUpdateByCd = StringUtils.ToString( dr["updateBy"] );
                    if ( StringUtils.IsNotEmpty( strUpdateByCd ) ) {
                        if ( true == _dicEmp.ContainsKey( strUpdateByCd ) ) {
                            dr["updateBy"] = strUpdateByCd + ":" + _dicEmp[strUpdateByCd].Trim();
                        }
                    }
                }
            }

            return dt;
        }


        #endregion
    }
}