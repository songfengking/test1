using KTFramework.Common;
using KTFramework.Excel;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TRC_W_PWT_ProductView.Common;
using TRC_W_PWT_ProductView.Report;
using TRC_W_PWT_ProductView.Dao.Process;
using TRC_W_PWT_ProductView.Defines;
using TRC_W_PWT_ProductView.Report.Utils;
using TRC_W_PWT_ProductView.Defines.ProcessViewDefine;

namespace TRC_W_PWT_ProductView.UI.Pages.UserControl {
    public partial class MainProcessOperationPartialView:System.Web.UI.UserControl {
        //ロガー定義
        private static readonly Logger _logger = new Logger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

        //現在ページ
        private MainProcessView CurrentPage =>
            HttpContext.Current.Handler is MainProcessView ? ( MainProcessView )HttpContext.Current.Handler : null;

        /// <summary>
        /// 噴射時期計測03 NG報告書出力
        /// </summary>
        protected void engineInjection03_btnNGReport_Click( object sender, EventArgs e ) {

            //出代NGエラーメッセージ
            var errorMessages = new List<string> {
                "ｸﾗﾝｸｱﾝｸﾞﾙ異常",
                "センサ異常またはゴミ噛み",
                "出代規格外",
                "出代ばらつきエラー",
                "トップクリアランス規格外",
                "ｶﾞｽｹｯﾄ仮選定",
                "ｶﾞｽｹｯﾄ選定",
                "ｶﾞｽｹｯﾄ上下限ｴﾗｰ",
                "ガスケット選定エラー"
            }.AsReadOnly();

            try {
                //出代NG検索実行
                var result = ProcessSearchDao.SelectEngineInjection03NGList( CurrentPage.SessionInfo.conditionValue, errorMessages );

                //報告日
                DateTime reportingDt = new DateTime();
                DateTime.TryParse( StringUtils.ToString( CurrentPage.SessionInfo.conditionValue["dateTo"] ), out reportingDt );

                //Excel出力処理
                if ( result != null && result.Rows.Count > 0 ) {
                    var excel = new NGReport( result, reportingDt );

                    //エクセルファイル作成
                    excel.Create();

                    //ダウンロード
                    if ( excel.IsOutputable ) {
                        excel.Dowonload( CurrentPage.Response );
                        return;
                    }
                }
                //警告メッセージ
                CurrentPage.WriteApplicationMessage( MsgManager.MESSAGE_WRN_61040 );
            } catch ( System.Threading.ThreadAbortException ) {
                //response.Endで必ず発生する為、正常として扱う
            } catch ( Exception ex ) {
                CurrentPage.WriteApplicationMessage( MsgManager.MESSAGE_ERR_80030, "NG報告書" );
            } finally {
                //グリッド再バインド（PostBackが発生し、検索結果の一部が消えてしまうため）
                CurrentPage.RebindGridView( true );
            }
        }

        /// <summary>
        /// 噴射時期計測03 詳細出力
        /// </summary>
        protected void engineInjection03_btnDetail_Click( object sender, EventArgs e ) {
            try {
                //未選択時
                if ( CurrentPage.SelectedDataItemIndex == -1 ) {
                    CurrentPage.WriteApplicationMessage( MsgManager.MESSAGE_WRN_62020, "出力対象のデータ" );
                    return;
                }

                //選択行取得
                var selectedDataRow = CurrentPage.SessionInfo.ResultData.Rows[CurrentPage.SelectedDataItemIndex];
            
                EngineInjection03DetailReport report = new EngineInjection03DetailReport(
                    DateUtils.ToDate( selectedDataRow[SearchResultDefine.GRID_ENGINE_INJECTION_03.INSPECTION_DT.bindField] ),
                    selectedDataRow[SearchResultDefine.GRID_COMMON.MODEL_NM.bindField].ToString(),
                    selectedDataRow[SearchResultDefine.GRID_COMMON.SERIAL6.bindField].ToString()
                );

                //PDFダウンロード
                report.Download( CurrentPage.Response );
            } catch ( System.Threading.ThreadAbortException ) {
                //response.Endで必ず発生する為、正常として扱う
            } catch ( Exception ex ) {
                _logger.Exception( ex );
                CurrentPage.WriteApplicationMessage( MsgManager.MESSAGE_ERR_80030, "噴射計測データ" );
            } finally {
                //グリッド再バインド（PostBackが発生し、検索結果の一部が消えてしまうため）
                CurrentPage.RebindGridView( true );
            }
        }

        /// <summary>
        /// 噴射時期計測07 詳細出力
        /// </summary>
        protected void engineInjection07_btnDetail_Click( object sender, EventArgs e ) {
            try {
                //未選択時
                if ( CurrentPage.SelectedDataItemIndex == -1 ) {
                    CurrentPage.WriteApplicationMessage( MsgManager.MESSAGE_WRN_62020, "出力対象のデータ" );
                    return;
                }

                //選択行取得
                var selectedDataRow = CurrentPage.SessionInfo.ResultData.Rows[CurrentPage.SelectedDataItemIndex];

                EngineInjection07DetailReport report = new EngineInjection07DetailReport(
                    DateUtils.ToDate( selectedDataRow[SearchResultDefine.GRID_ENGINE_INJECTION_07.INSPECTION_DT.bindField] ),
                    selectedDataRow[SearchResultDefine.GRID_COMMON.MODEL_NM.bindField].ToString(),
                    selectedDataRow[SearchResultDefine.GRID_COMMON.SERIAL6.bindField].ToString()
                );

                //PDFダウンロード
                report.Download( CurrentPage.Response );
            } catch ( System.Threading.ThreadAbortException ) {
                //response.Endで必ず発生する為、正常として扱う
            } catch ( Exception ex ) {
                _logger.Exception( ex );
                CurrentPage.WriteApplicationMessage( MsgManager.MESSAGE_ERR_80030, "噴射計測データ" );
            } finally {
                //グリッド再バインド（PostBackが発生し、検索結果の一部が消えてしまうため）
                CurrentPage.RebindGridView( true );
            }
        }
    }
}

