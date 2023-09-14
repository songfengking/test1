using System;
using System.Web.UI;
using KTFramework.Common;

namespace TRC_W_PWT_ProductView.Common {
    /// <summary>
    /// TRCWEB共通定数定義
    /// </summary>
    public class TrcWebConsts {
        #region コールバック用キー
        /// <summary>親画面の再検索用コールバック</summary>
        public const string HIDDEN_KEY_CALL_BACK_REFRESH = "refresh";
        /// <summary>セッションエラー判定用</summary>
        public const string HIDDEN_KEY_SESSION_ERROR = "SessionError";
        /// <summary>ファイルアップロードcomplete通知用</summary>
        public const string HIDDEN_KEY_CALL_BACK_FILE_UPLOAD = "fileUpload";

        #endregion

        #region エラー処理用
        /// <summary>
        /// エラーコード
        /// </summary>
        public const string REQ_PARAM_NAME_ERROR_CODE = "errorCode";
        /// <summary>
        /// エラーメッセージ
        /// </summary>
        public const string REQ_PARAM_NAME_ERROR_MESSAGE = "errorMessage";
        #endregion

        #region リクエストパラメータ

        /// <summary>メインメニュー選択種別</summary>
        public const string REQ_PARAM_NAME_SELECTED_MENU_KIND = "MenuKind";
        /// <summary>メインメニュー選択項目:D1000L用</summary>
        public enum ReqParamMenuKindD1000L {
            /// <summary>未指定</summary>
            Undefined,
            /// <summary>後付作業</summary>
            FactoryInstalling,
            /// <summary>型式変更</summary>
            ModelAlteration,
            /// <summary>仕向地変更</summary>
            ShippingArea,
            /// <summary>各種作業指示</summary>
            SeveralKindsOperationInstruction,
        }
        /// <summary>メインメニュー選択項目：D1100L用</summary>
        public enum ReqParamMenuKindD1100L {
            /// <summary>未指定</summary>
            Undefined,
            /// <summary>後付順序並替</summary>
            FactoryInstallingSequenceSort,
            /// <summary>後付順序指示書発行</summary>
            FactoryInstallingInstructionInvoice,
        }
        /// <summary>メインメニュー選択項目：D1800L用</summary>
        public enum ReqParamMenuKindD1800L {
            /// <summary>未指定</summary>
            Undefined,
            /// <summary>製品検索</summary>
            ProductList,
        }

        /// <summary>メインメニュー選択項目:D2000L用</summary>
        public enum ReqParamMenuKindD2000L {
            /// <summary>未指定</summary>
            Undefined,
            /// <summary>移送指示一覧</summary>
            TransitInstruction,
            /// <summary>出庫指示一覧</summary>
            DeliveryInstruction,
            /// <summary>出庫計上</summary>
            DeliveryInstructionNotifacation,
            /// <summary>仕掛製品変更指示一覧</summary>
            InProcessProductChangeInstruction,
            /// <summary>現在在庫検索</summary>
            StockSearch,
            /// <summary>倉庫別在庫検索</summary>
            StockWarehouseSearch,
            /// <summary>来歴在庫検索</summary>
            StockRecordSearch,
            /// <summary>在庫情報詳細</summary>
            StockDetail,
            /// <summary>有効在庫修正</summary>
            AvailableStockCorrect,
        }
        /// <summary>メインメニュー選択項目:D3000L用</summary>
        public enum ReqParamMenuKindD3000L {
            /// <summary>未指定</summary>
            Undefined,
            /// <summary>出荷連絡</summary>
            ShippingNotification,
            /// <summary>出荷伝票検索</summary>
            ShippingSlipSearch,
            /// <summary>海外拠点向け出荷</summary>
            OverseasBaseShipping,
            //// <summary>機番入替</summary>
            //SerialNumberChange,
            /// <summary>解約返品</summary>
            CancellationReturn,
            /// <summary>解約返品計上</summary>
            CancellationNotification,
            /// <summary>国内出荷運賃修正</summary>
            DomesticShippingFareRetouch,
            /// <summary>輸出船積作業料修正</summary>
            ExportLoadingWorkCharges
        }
        /// <summary>メインメニュー選択項目:D4000L用</summary>
        public enum ReqParamMenuKindD4000L {
            /// <summary>未指定</summary>
            Undefined,
        }
        /// <summary>メインメニュー選択項目:D4900L用</summary>
        public enum ReqParamMenuKindD4900L {
            /// <summary>未指定</summary>
            Undefined,
            /// <summary>請求書発行</summary>
            IssuingInvoice
        }
        /// <summary>メインメニュー選択項目:D5000L用</summary>
        public enum ReqParamMenuKindD5000L {
            /// <summary>未指定</summary>
            Undefined,
            /// <summary>作業履歴検索</summary>
            SearchOperationRecord,
            /// <summary>作業料修正</summary>
            UpdateOperationCharge
        }
        /// <summary>メインメニュー選択項目:D5100L用</summary>
        public enum ReqParamMenuKindD5100L {
            /// <summary>未指定</summary>
            Undefined,
            /// <summary>作業日報</summary>
            SearchOperationReport
        }
        /// <summary>メインメニュー選択項目:D5200L用</summary>
        public enum ReqParamMenuKindD5200L {
            /// <summary>未指定</summary>
            Undefined,
            /// <summary>梱包日程</summary>
            PackingPlan
        }
        /// <summary>メインメニュー選択項目:D5300L用</summary>
        public enum ReqParamMenuKindD5300L {
            /// <summary>未指定</summary>
            Undefined,
            /// <summary>個送出荷明細</summary>
            IndividualDeliveryShippingDetail,
            /// <summary>オーダー情報登録</summary>
            OrderDetailAdd
        }
        /// <summary>メインメニュー選択項目:D9100L用</summary>
        public enum ReqParamMenuKindD9100L {
            /// <summary>未指定</summary>
            Undefined,
            /// <summary>物流作業区分</summary>
            Operation,
        }
        /// <summary>メインメニュー選択項目:D9110L用</summary>
        public enum ReqParamMenuKindD9110L {
            /// <summary>未指定</summary>
            Undefined,
            /// <summary>物流型式</summary>
            Model,
        }
        /// <summary>メインメニュー選択項目:D9200L用</summary>
        public enum ReqParamMenuKindD9200L {
            /// <summary>未指定</summary>
            Undefined,
            /// <summary>荷姿</summary>
            Package,
        }
        /// <summary>メインメニュー選択項目:D9210L用</summary>
        public enum ReqParamMenuKindD9210L {
            /// <summary>未指定</summary>
            Undefined,
            /// <summary>荷姿別型式</summary>
            ModelPackage,
        }
        /// <summary>メインメニュー選択項目:D9300L用</summary>
        public enum ReqParamMenuKindD9300L {
            /// <summary>未指定</summary>
            Undefined,
            /// <summary>物流部品種別</summary>
            PartsKind,
        }
        /// <summary>メインメニュー選択項目:D9310L用</summary>
        public enum ReqParamMenuKindD9310L {
            /// <summary>未指定</summary>
            Undefined,
            /// <summary>物流部品</summary>
            Parts,
        }
        /// <summary>メインメニュー選択項目:D9320L用</summary>
        public enum ReqParamMenuKindD9320L {
            /// <summary>未指定</summary>
            Undefined,
            /// <summary>物流型式部品構成</summary>
            ModelPartsComponent,
        }
        /// <summary>メインメニュー選択項目:D9400L用</summary>
        public enum ReqParamMenuKindD9400L {
            /// <summary>未指定</summary>
            Undefined,
            /// <summary>物流費目</summary>
            ExpenditureItem,
        }
        /// <summary>メインメニュー選択項目:D9410L用</summary>
        public enum ReqParamMenuKindD9410L {
            /// <summary>未指定</summary>
            Undefined,
            /// <summary>作業料金</summary>
            OperationCharge,
        }
        /// <summary>メインメニュー選択項目:D9420L用</summary>
        public enum ReqParamMenuKindD9420L {
            /// <summary>未指定</summary>
            Undefined,
            /// <summary>国内移送運賃</summary>
            TransitFareDom,
        }
        /// <summary>メインメニュー選択項目:D9430L用</summary>
        public enum ReqParamMenuKindD9430L {
            /// <summary>未指定</summary>
            Undefined,
            /// <summary>輸出出荷運賃</summary>
            TransitFareExp,
        }
        /// <summary>メインメニュー選択項目:D9440L用</summary>
        public enum ReqParamMenuKindD9440L {
            /// <summary>未指定</summary>
            Undefined,
            /// <summary>輸出出荷作業料単価</summary>
            ShippingOperationCharge,
        }
        /// <summary>メインメニュー選択項目:D9450L用</summary>
        public enum ReqParamMenuKindD9450L {
            /// <summary>未指定</summary>
            Undefined,
            /// <summary>個送出荷条件</summary>
            DeliveryConditions,
        }
        /// <summary>メインメニュー選択項目:D9500L用</summary>
        public enum ReqParamMenuKindD9500L {
            /// <summary>未指定</summary>
            Undefined,
            /// <summary>外作出荷ラベル発行順序</summary>
            ShippingLabelPrintOrder,
        }
        /// <summary>メインメニュー選択項目:D9600L用</summary>
        public enum ReqParamMenuKindD9600L {
            /// <summary>未指定</summary>
            Undefined,
            /// <summary>DPF同梱型式</summary>
            IncludingDPFModel,
        }
        /// <summary>メインメニュー選択項目:D9900L用</summary>
        public enum ReqParamMenuKindD9900L {
            /// <summary>未指定</summary>
            Undefined,
        }
        /// <summary>
        /// ReqParamMenuKindD1000Lにパースする
        /// </summary>
        /// <param name="menuKind"></param>
        /// <returns></returns>
        public static ReqParamMenuKindD1000L ParseReqParamMenuKindD1000L( string menuKind ) {
            ReqParamMenuKindD1000L parsedMenuKind;
            if ( StringUtils.IsEmpty( menuKind ) ) {
                parsedMenuKind = ReqParamMenuKindD1000L.Undefined;
            } else {
                if ( Enum.TryParse<ReqParamMenuKindD1000L>( menuKind, out parsedMenuKind ) == false ) {
                    parsedMenuKind = ReqParamMenuKindD1000L.Undefined;
                }
            }
            return parsedMenuKind;
        }
        /// <summary>
        /// ReqParamMenuKindD1100Lにパースする
        /// </summary>
        /// <param name="menuKind"></param>
        /// <returns></returns>
        public static ReqParamMenuKindD1100L ParseReqParamMenuKindD1100L( string menuKind ) {
            ReqParamMenuKindD1100L parsedMenuKind;
            if ( StringUtils.IsEmpty( menuKind ) ) {
                parsedMenuKind = ReqParamMenuKindD1100L.Undefined;
            } else {
                if ( Enum.TryParse<ReqParamMenuKindD1100L>( menuKind, out parsedMenuKind ) == false ) {
                    parsedMenuKind = ReqParamMenuKindD1100L.Undefined;
                }
            }
            return parsedMenuKind;
        }
        /// <summary>
        /// ReqParamMenuKindD1800Lにパースする
        /// </summary>
        /// <param name="menuKind"></param>
        /// <returns></returns>
        public static ReqParamMenuKindD1800L ParseReqParamMenuKindD1800L( string menuKind ) {
            ReqParamMenuKindD1800L parsedMenuKind;
            if ( StringUtils.IsEmpty( menuKind ) ) {
                parsedMenuKind = ReqParamMenuKindD1800L.Undefined;
            } else {
                if ( Enum.TryParse<ReqParamMenuKindD1800L>( menuKind, out parsedMenuKind ) == false ) {
                    parsedMenuKind = ReqParamMenuKindD1800L.Undefined;
                }
            }
            return parsedMenuKind;
        }
        /// <summary>
        /// ReqParamMenuKindD2000Lにパースする
        /// </summary>
        /// <param name="menuKind"></param>
        /// <returns></returns>
        public static ReqParamMenuKindD2000L ParseReqParamMenuKindD2000L( string menuKind ) {
            ReqParamMenuKindD2000L parsedMenuKind;
            if ( StringUtils.IsEmpty( menuKind ) ) {
                parsedMenuKind = ReqParamMenuKindD2000L.Undefined;
            } else {
                if ( Enum.TryParse<ReqParamMenuKindD2000L>( menuKind, out parsedMenuKind ) == false ) {
                    parsedMenuKind = ReqParamMenuKindD2000L.Undefined;
                }
            }
            return parsedMenuKind;
        }
        /// <summary>
        /// ReqParamMenuKindD3000Lにパースする
        /// </summary>
        /// <param name="menuKind"></param>
        /// <returns></returns>
        public static ReqParamMenuKindD3000L ParseReqParamMenuKindD3000L( string menuKind ) {
            ReqParamMenuKindD3000L parsedMenuKind;
            if ( StringUtils.IsEmpty( menuKind ) ) {
                parsedMenuKind = ReqParamMenuKindD3000L.Undefined;
            } else {
                if ( Enum.TryParse<ReqParamMenuKindD3000L>( menuKind, out parsedMenuKind ) == false ) {
                    parsedMenuKind = ReqParamMenuKindD3000L.Undefined;
                }
            }
            return parsedMenuKind;
        }
        /// <summary>
        /// ReqParamMenuKindD4000Lにパースする
        /// </summary>
        /// <param name="menuKind"></param>
        /// <returns></returns>
        public static ReqParamMenuKindD4000L ParseReqParamMenuKindD4000L( string menuKind ) {
            ReqParamMenuKindD4000L parsedMenuKind;
            if ( StringUtils.IsEmpty( menuKind ) ) {
                parsedMenuKind = ReqParamMenuKindD4000L.Undefined;
            } else {
                if ( Enum.TryParse<ReqParamMenuKindD4000L>( menuKind, out parsedMenuKind ) == false ) {
                    parsedMenuKind = ReqParamMenuKindD4000L.Undefined;
                }
            }
            return parsedMenuKind;
        }
        /// <summary>
        /// ReqParamMenuKindD4900Lにパースする
        /// </summary>
        /// <param name="menuKind"></param>
        /// <returns></returns>
        public static ReqParamMenuKindD4900L ParseReqParamMenuKindD4900L( string menuKind ) {
            ReqParamMenuKindD4900L parsedMenuKind;
            if ( StringUtils.IsEmpty( menuKind ) ) {
                parsedMenuKind = ReqParamMenuKindD4900L.Undefined;
            } else {
                if ( Enum.TryParse<ReqParamMenuKindD4900L>( menuKind, out parsedMenuKind ) == false ) {
                    parsedMenuKind = ReqParamMenuKindD4900L.Undefined;
                }
            }
            return parsedMenuKind;
        }
        /// <summary>
        /// ReqParamMenuKindD5000Lにパースする
        /// </summary>
        /// <param name="menuKind"></param>
        /// <returns></returns>
        public static ReqParamMenuKindD5000L ParseReqParamMenuKindD5000L( string menuKind ) {
            ReqParamMenuKindD5000L parsedMenuKind;
            if ( StringUtils.IsEmpty( menuKind ) ) {
                parsedMenuKind = ReqParamMenuKindD5000L.Undefined;
            } else {
                if ( Enum.TryParse<ReqParamMenuKindD5000L>( menuKind, out parsedMenuKind ) == false ) {
                    parsedMenuKind = ReqParamMenuKindD5000L.Undefined;
                }
            }
            return parsedMenuKind;
        }
        /// <summary>
        /// ReqParamMenuKindD5100Lにパースする
        /// </summary>
        /// <param name="menuKind"></param>
        /// <returns></returns>
        public static ReqParamMenuKindD5100L ParseReqParamMenuKindD5100L( string menuKind ) {
            ReqParamMenuKindD5100L parsedMenuKind;
            if ( StringUtils.IsEmpty( menuKind ) ) {
                parsedMenuKind = ReqParamMenuKindD5100L.Undefined;
            } else {
                if ( Enum.TryParse<ReqParamMenuKindD5100L>( menuKind, out parsedMenuKind ) == false ) {
                    parsedMenuKind = ReqParamMenuKindD5100L.Undefined;
                }
            }
            return parsedMenuKind;
        }
        /// <summary>
        /// ReqParamMenuKindD5200Lにパースする
        /// </summary>
        /// <param name="menuKind"></param>
        /// <returns></returns>
        public static ReqParamMenuKindD5200L ParseReqParamMenuKindD5200L( string menuKind ) {
            ReqParamMenuKindD5200L parsedMenuKind;
            if ( StringUtils.IsEmpty( menuKind ) ) {
                parsedMenuKind = ReqParamMenuKindD5200L.Undefined;
            } else {
                if ( Enum.TryParse<ReqParamMenuKindD5200L>( menuKind, out parsedMenuKind ) == false ) {
                    parsedMenuKind = ReqParamMenuKindD5200L.Undefined;
                }
            }
            return parsedMenuKind;
        }
        /// <summary>
        /// ReqParamMenuKindD5300Lにパースする
        /// </summary>
        /// <param name="menuKind"></param>
        /// <returns></returns>
        public static ReqParamMenuKindD5300L ParseReqParamMenuKindD5300L( string menuKind ) {
            ReqParamMenuKindD5300L parsedMenuKind;
            if ( StringUtils.IsEmpty( menuKind ) ) {
                parsedMenuKind = ReqParamMenuKindD5300L.Undefined;
            } else {
                if ( Enum.TryParse<ReqParamMenuKindD5300L>( menuKind, out parsedMenuKind ) == false ) {
                    parsedMenuKind = ReqParamMenuKindD5300L.Undefined;
                }
            }
            return parsedMenuKind;
        }
        /// <summary>
        /// ReqParamMenuKindD9100Lにパースする
        /// </summary>
        /// <param name="menuKind"></param>
        /// <returns></returns>
        public static ReqParamMenuKindD9100L ParseReqParamMenuKindD9100L( string menuKind ) {
            ReqParamMenuKindD9100L parsedMenuKind;
            if ( StringUtils.IsEmpty( menuKind ) ) {
                parsedMenuKind = ReqParamMenuKindD9100L.Undefined;
            } else {
                if ( Enum.TryParse<ReqParamMenuKindD9100L>( menuKind, out parsedMenuKind ) == false ) {
                    parsedMenuKind = ReqParamMenuKindD9100L.Undefined;
                }
            }
            return parsedMenuKind;
        }

        /// <summary>
        /// ReqParamMenuKindD9110Lにパースする
        /// </summary>
        /// <param name="menuKind"></param>
        /// <returns></returns>
        public static ReqParamMenuKindD9110L ParseReqParamMenuKindD9110L( string menuKind ) {
            ReqParamMenuKindD9110L parsedMenuKind;
            if ( StringUtils.IsEmpty( menuKind ) ) {
                parsedMenuKind = ReqParamMenuKindD9110L.Undefined;
            } else {
                if ( Enum.TryParse<ReqParamMenuKindD9110L>( menuKind, out parsedMenuKind ) == false ) {
                    parsedMenuKind = ReqParamMenuKindD9110L.Undefined;
                }
            }
            return parsedMenuKind;
        }

        /// <summary>
        /// ReqParamMenuKindD9200Lにパースする
        /// </summary>
        /// <param name="menuKind"></param>
        /// <returns></returns>
        public static ReqParamMenuKindD9200L ParseReqParamMenuKindD9200L( string menuKind ) {
            ReqParamMenuKindD9200L parsedMenuKind;
            if ( StringUtils.IsEmpty( menuKind ) ) {
                parsedMenuKind = ReqParamMenuKindD9200L.Undefined;
            } else {
                if ( Enum.TryParse<ReqParamMenuKindD9200L>( menuKind, out parsedMenuKind ) == false ) {
                    parsedMenuKind = ReqParamMenuKindD9200L.Undefined;
                }
            }
            return parsedMenuKind;
        }

        /// <summary>
        /// ReqParamMenuKindD9210Lにパースする
        /// </summary>
        /// <param name="menuKind"></param>
        /// <returns></returns>
        public static ReqParamMenuKindD9210L ParseReqParamMenuKindD9210L( string menuKind ) {
            ReqParamMenuKindD9210L parsedMenuKind;
            if ( StringUtils.IsEmpty( menuKind ) ) {
                parsedMenuKind = ReqParamMenuKindD9210L.Undefined;
            } else {
                if ( Enum.TryParse<ReqParamMenuKindD9210L>( menuKind, out parsedMenuKind ) == false ) {
                    parsedMenuKind = ReqParamMenuKindD9210L.Undefined;
                }
            }
            return parsedMenuKind;
        }

        /// <summary>
        /// ReqParamMenuKindD9300Lにパースする
        /// </summary>
        /// <param name="menuKind"></param>
        /// <returns></returns>
        public static ReqParamMenuKindD9300L ParseReqParamMenuKindD9300L( string menuKind ) {
            ReqParamMenuKindD9300L parsedMenuKind;
            if ( StringUtils.IsEmpty( menuKind ) ) {
                parsedMenuKind = ReqParamMenuKindD9300L.Undefined;
            } else {
                if ( Enum.TryParse<ReqParamMenuKindD9300L>( menuKind, out parsedMenuKind ) == false ) {
                    parsedMenuKind = ReqParamMenuKindD9300L.Undefined;
                }
            }
            return parsedMenuKind;
        }

        /// <summary>
        /// ReqParamMenuKindD9310Lにパースする
        /// </summary>
        /// <param name="menuKind"></param>
        /// <returns></returns>
        public static ReqParamMenuKindD9310L ParseReqParamMenuKindD9310L( string menuKind ) {
            ReqParamMenuKindD9310L parsedMenuKind;
            if ( StringUtils.IsEmpty( menuKind ) ) {
                parsedMenuKind = ReqParamMenuKindD9310L.Undefined;
            } else {
                if ( Enum.TryParse<ReqParamMenuKindD9310L>( menuKind, out parsedMenuKind ) == false ) {
                    parsedMenuKind = ReqParamMenuKindD9310L.Undefined;
                }
            }
            return parsedMenuKind;
        }

        /// <summary>
        /// ReqParamMenuKindD9320Lにパースする
        /// </summary>
        /// <param name="menuKind"></param>
        /// <returns></returns>
        public static ReqParamMenuKindD9320L ParseReqParamMenuKindD9320L( string menuKind ) {
            ReqParamMenuKindD9320L parsedMenuKind;
            if ( StringUtils.IsEmpty( menuKind ) ) {
                parsedMenuKind = ReqParamMenuKindD9320L.Undefined;
            } else {
                if ( Enum.TryParse<ReqParamMenuKindD9320L>( menuKind, out parsedMenuKind ) == false ) {
                    parsedMenuKind = ReqParamMenuKindD9320L.Undefined;
                }
            }
            return parsedMenuKind;
        }

        /// <summary>
        /// ReqParamMenuKindD9400Lにパースする
        /// </summary>
        /// <param name="menuKind"></param>
        /// <returns></returns>
        public static ReqParamMenuKindD9400L ParseReqParamMenuKindD9400L( string menuKind ) {
            ReqParamMenuKindD9400L parsedMenuKind;
            if ( StringUtils.IsEmpty( menuKind ) ) {
                parsedMenuKind = ReqParamMenuKindD9400L.Undefined;
            } else {
                if ( Enum.TryParse<ReqParamMenuKindD9400L>( menuKind, out parsedMenuKind ) == false ) {
                    parsedMenuKind = ReqParamMenuKindD9400L.Undefined;
                }
            }
            return parsedMenuKind;
        }

        /// <summary>
        /// ReqParamMenuKindD9410Lにパースする
        /// </summary>
        /// <param name="menuKind"></param>
        /// <returns></returns>
        public static ReqParamMenuKindD9410L ParseReqParamMenuKindD9410L( string menuKind ) {
            ReqParamMenuKindD9410L parsedMenuKind;
            if ( StringUtils.IsEmpty( menuKind ) ) {
                parsedMenuKind = ReqParamMenuKindD9410L.Undefined;
            } else {
                if ( Enum.TryParse<ReqParamMenuKindD9410L>( menuKind, out parsedMenuKind ) == false ) {
                    parsedMenuKind = ReqParamMenuKindD9410L.Undefined;
                }
            }
            return parsedMenuKind;
        }

        /// <summary>
        /// ReqParamMenuKindD9420Lにパースする
        /// </summary>
        /// <param name="menuKind"></param>
        /// <returns></returns>
        public static ReqParamMenuKindD9420L ParseReqParamMenuKindD9420L( string menuKind ) {
            ReqParamMenuKindD9420L parsedMenuKind;
            if ( StringUtils.IsEmpty( menuKind ) ) {
                parsedMenuKind = ReqParamMenuKindD9420L.Undefined;
            } else {
                if ( Enum.TryParse<ReqParamMenuKindD9420L>( menuKind, out parsedMenuKind ) == false ) {
                    parsedMenuKind = ReqParamMenuKindD9420L.Undefined;
                }
            }
            return parsedMenuKind;
        }

        /// <summary>
        /// ReqParamMenuKindD9430Lにパースする
        /// </summary>
        /// <param name="menuKind"></param>
        /// <returns></returns>
        public static ReqParamMenuKindD9430L ParseReqParamMenuKindD9430L( string menuKind ) {
            ReqParamMenuKindD9430L parsedMenuKind;
            if ( StringUtils.IsEmpty( menuKind ) ) {
                parsedMenuKind = ReqParamMenuKindD9430L.Undefined;
            } else {
                if ( Enum.TryParse<ReqParamMenuKindD9430L>( menuKind, out parsedMenuKind ) == false ) {
                    parsedMenuKind = ReqParamMenuKindD9430L.Undefined;
                }
            }
            return parsedMenuKind;
        }

        /// <summary>
        /// ReqParamMenuKindD9440Lにパースする
        /// </summary>
        /// <param name="menuKind"></param>
        /// <returns></returns>
        public static ReqParamMenuKindD9440L ParseReqParamMenuKindD9440L( string menuKind ) {
            ReqParamMenuKindD9440L parsedMenuKind;
            if ( StringUtils.IsEmpty( menuKind ) ) {
                parsedMenuKind = ReqParamMenuKindD9440L.Undefined;
            } else {
                if ( Enum.TryParse<ReqParamMenuKindD9440L>( menuKind, out parsedMenuKind ) == false ) {
                    parsedMenuKind = ReqParamMenuKindD9440L.Undefined;
                }
            }
            return parsedMenuKind;
        }

        /// <summary>
        /// ReqParamMenuKindD9450Lにパースする
        /// </summary>
        /// <param name="menuKind"></param>
        /// <returns></returns>
        public static ReqParamMenuKindD9450L ParseReqParamMenuKindD9450L( string menuKind ) {
            ReqParamMenuKindD9450L parsedMenuKind;
            if ( StringUtils.IsEmpty( menuKind ) ) {
                parsedMenuKind = ReqParamMenuKindD9450L.Undefined;
            } else {
                if ( Enum.TryParse<ReqParamMenuKindD9450L>( menuKind, out parsedMenuKind ) == false ) {
                    parsedMenuKind = ReqParamMenuKindD9450L.Undefined;
                }
            }
            return parsedMenuKind;
        }

        /// <summary>
        /// ReqParamMenuKindD9500Lにパースする
        /// </summary>
        /// <param name="menuKind"></param>
        /// <returns></returns>
        public static ReqParamMenuKindD9500L ParseReqParamMenuKindD9500L( string menuKind ) {
            ReqParamMenuKindD9500L parsedMenuKind;
            if ( StringUtils.IsEmpty( menuKind ) ) {
                parsedMenuKind = ReqParamMenuKindD9500L.Undefined;
            } else {
                if ( Enum.TryParse<ReqParamMenuKindD9500L>( menuKind, out parsedMenuKind ) == false ) {
                    parsedMenuKind = ReqParamMenuKindD9500L.Undefined;
                }
            }
            return parsedMenuKind;
        }

        /// <summary>
        /// ReqParamMenuKindD9600Lにパースする
        /// </summary>
        /// <param name="menuKind"></param>
        /// <returns></returns>
        public static ReqParamMenuKindD9600L ParseReqParamMenuKindD9600L( string menuKind ) {
            ReqParamMenuKindD9600L parsedMenuKind;
            if ( StringUtils.IsEmpty( menuKind ) ) {
                parsedMenuKind = ReqParamMenuKindD9600L.Undefined;
            } else {
                if ( Enum.TryParse<ReqParamMenuKindD9600L>( menuKind, out parsedMenuKind ) == false ) {
                    parsedMenuKind = ReqParamMenuKindD9600L.Undefined;
                }
            }
            return parsedMenuKind;
        }

        /// <summary>
        /// ReqParamMenuKindD9900Lにパースする
        /// </summary>
        /// <param name="menuKind"></param>
        /// <returns></returns>
        public static ReqParamMenuKindD9900L ParseReqParamMenuKindD9900L( string menuKind ) {
            ReqParamMenuKindD9900L parsedMenuKind;
            if ( StringUtils.IsEmpty( menuKind ) ) {
                parsedMenuKind = ReqParamMenuKindD9900L.Undefined;
            } else {
                if ( Enum.TryParse<ReqParamMenuKindD9900L>( menuKind, out parsedMenuKind ) == false ) {
                    parsedMenuKind = ReqParamMenuKindD9900L.Undefined;
                }
            }
            return parsedMenuKind;
        }
        #endregion

        #region ユーザ実行JOB定義（タスクID）
        ///<summary>製品トレース 指示作成JOBタスク</summary>
        public const string JOB_PARTS_TRACE = "PartsTrace";
        ///<summary>検索(Excel出力) 指示作成JOBタスク</summary>
        public const string JOB_MASS_DATA_OUTPUT = "MassDataOutput";
        ///<summary>生産・出荷・在庫Excel出力 指示作成JOBタスク</summary>
        public const string JOB_PSS_DATA_OUTPUT = "PSSDataOutput";
        #endregion

        #region 製品トレース：ユーザ実行JOB引数定義
        /// <summary>JOB引数キー：検索対象(完成予定)</summary>
        public const string JOB_PARAM_KEY_PRODUCT = "product";
        /// <summary>JOB引数キー：検索対象(在庫)</summary>
        public const string JOB_PARAM_KEY_STOCK = "stock";
        /// <summary>JOB引数キー：検索対象(完成)</summary>
        public const string JOB_PARAM_KEY_SHIPMENT = "shipment";

        /// <summary>JOB引数キー：製品種別</summary>
        public const string JOB_PARAM_KEY_PRODUCT_KBN = "productKbn";
        /// <summary>JOB引数キー：品番</summary>
        public const string JOB_PARAM_KEY_PARTS_NUM = "partsNum";

        /// <summary>JOB引数キー：検索対象日付(完成予定)</summary>
        public const string JOB_PARAM_KEY_PRODUCT_FROM = "productFrom";
        /// <summary>JOB引数キー：検索対象日付(在庫)</summary>
        public const string JOB_PARAM_KEY_STOCK_FROM = "stockFrom";
        /// <summary>JOB引数キー：検索対象日付(完成)</summary>
        public const string JOB_PARAM_KEY_SHIPMENT_FROM = "shipmentFrom";
        /// <summary>JOB引数キー：検索対象日付(完成予定)</summary>
        public const string JOB_PARAM_KEY_PRODUCT_TO = "productTo";
        /// <summary>JOB引数キー：検索対象日付(在庫)</summary>
        public const string JOB_PARAM_KEY_STOCK_TO = "stockTo";
        /// <summary>JOB引数キー：検索対象日付(完成)</summary>
        public const string JOB_PARAM_KEY_SHIPMENT_TO = "shipmentTo";

        /// <summary>JOB引数バリュー：出力対象外</summary>
        public const string JOB_PARAM_VALUE_TARGET_OFF = "0";
        /// <summary>JOB引数バリュー：出力対象</summary>
        public const string JOB_PARAM_VALUE_TARGET_ON = "1";

        #endregion

        #region 検索(Excel出力):ユーザ実行JOB引数定義
        /// <summary>製品種別</summary>
        public const string JOB_PARAM_KEY_PRODUCT_KIND_CD = "productKindCd";
        /// <summary>型式種別</summary>
        public const string JOB_PARAM_KEY_MODEL_TYPE = "modelType";
        /// <summary>型式コード</summary>
        public const string JOB_PARAM_KEY_MODEL_CD = "modelCd";
        /// <summary>型式名</summary>
        public const string JOB_PARAM_KEY_MODEL_NM = "modelNm";
        /// <summary>PINコード(チェックボックス)</summary>
        public const string JOB_PARAM_KEY_PIN_CD_CHECK = "pinCdCheck";
        /// <summary>PINコード</summary>
        public const string JOB_PARAM_KEY_PIN_CD = "pinCd";
        /// <summary>製品機番</summary>
        public const string JOB_PARAM_KEY_SERIAL = "serial";
        /// <summary>IDNO</summary>
        public const string JOB_PARAM_KEY_IDNO = "idno";
        /// <summary>工程コード</summary>
        public const string JOB_PARAM_KEY_PROCESS_CD = "processCd";
        /// <summary>工程名</summary>
        public const string JOB_PARAM_KEY_PROCESS_NM = "processNm";
        /// <summary>部品区分</summary>
        public const string JOB_PARAM_KEY_PARTS_CD = "partsCd";
        /// <summary>部品機番</summary>
        public const string JOB_PARAM_KEY_PARTS_SERIAL = "partsSerial";
        /// <summary>日付区分</summary>
        public const string JOB_PARAM_KEY_DATE_KIND_CD = "dateKindCd";
        /// <summary>日付From</summary>
        public const string JOB_PARAM_KEY_DATE_FROM = "dateFrom";
        /// <summary>日付To</summary>
        public const string JOB_PARAM_KEY_DATE_TO = "dateTo";
        /// <summary>従業員番号</summary>
        public const string JOB_PARAM_KEY_USER_ID = "userId";
        /// <summary>メールアドレス</summary>
        public const string JOB_PARAM_KEY_MAIL_ADDRESS = "mailAddress";
        /// <summary>ラインコード</summary>
        public const string JOB_PARAM_KEY_LINE_CD = "lineCd";
        /// <summary>作業コード</summary>
        public const string JOB_PARAM_KEY_WORK_CD = "workCd";
        /// <summary>作業名</summary>
        public const string JOB_PARAM_KEY_WORK_NM = "workNm";
        #endregion

        #region ユーザ実行JOB　実行JOB名
        ///<summary>製品トレース 実行JOB名</summary>
        public const string JOB_PARAM_KEY_JOB_NM_PARTS_TRACE = "TRC_B_P_PartsTrace";
        ///<summary>検索(Excel出力) 実行JOB名</summary>
        public const string JOB_PARAM_KEY_JOB_NM_MASS_DATA_OUTPUT = "TRC_B_P_MassDataOutput";
        ///<summary>複数型式検索出力 実行JOB名</summary>
        public const string JOB_PARAM_KEY_JOB_NM_PSS_DATA_OUTPUT = "TRC_B_P_PSSDataOutput";
        #endregion

        #region サービス引き渡しパラメータ定数
        /// <summary>
        /// チェックボックスTRUE
        /// </summary>
        public const string SERVICE_PARAM_CHECKED_TRUE = "1";
        /// <summary>
        /// チェックボックスFALSE
        /// </summary>
        public const string SERVICE_PARAM_CHECKED_FALSE = "0";
        #endregion

        #region ゲストユーザ定義
        /// <summary>ゲストユーザID</summary>
        public const string GUEST_USER_ID = "guest";
        /// <summary>ゲストユーザパスワード</summary>
        public const string GUEST_USER_PW = "guest";
        #endregion


    }
}