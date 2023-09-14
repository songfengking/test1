using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using KTFramework.Common;
using KTFramework.Dao;
using TRC_W_PWT_ProductView.Defines;
using TRC_W_PWT_ProductView.Dao.Process;
using TRC_W_PWT_ProductView.Defines.TypeDefine;
using TRC_W_PWT_ProductView.Defines.ListDefine;

namespace TRC_W_PWT_ProductView.Common {
    /// <summary>
    /// データ用ユーティリティクラス
    /// </summary>
    public static class DataUtils {

        /// <summary>ロガー定義</summary>
        private static readonly Logger logger = new Logger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

        /// <summary>
        /// コントロール定義とキー値データからバインドパラメータに変換する
        /// </summary>
        /// <param name="ControlDefineArr">コントロール定義</param>
        /// <param name="rowParam">データ行</param>
        /// <returns>バインドパラメータ</returns>
        internal static KTBindParameters GetBindParameters( ControlDefine[] ControlDefineArr, Dictionary<string, object> dicCondition ) {

            KTBindParameters parameters = new KTBindParameters();

            foreach ( ControlDefine ctrlDef in ControlDefineArr ) {
                object val = null;
                if ( true == dicCondition.ContainsKey( ctrlDef.bindField ) ) {
                    val = dicCondition[ctrlDef.bindField];
                }
                parameters.Add( ctrlDef.bindField, val );
            }

            return parameters;
        }

        /// <summary>
        /// リストアイテムにブランク列をセット
        /// </summary>
        /// <param name="listItemArr"></param>
        /// <param name="liBlank">ブランクリストアイテム</param>
        /// <returns>リストアイテム配列</returns>
        /// <remarks>
        /// liBlankがNULLの時には、Text,Value共に""[ブランク]のアイテムを追加します。
        /// </remarks>
        internal static ListItem[] InsertBlankItem( ListItem[] listItemArr, ListItem liBlank = null ) {

            const int COPY_IDX = 1;

            ListItem[] resultArr = new ListItem[listItemArr.Length + 1];

            if ( true == ObjectUtils.IsNull( liBlank ) ) {
                liBlank = new ListItem( "", "" );
            }

            resultArr[0] = liBlank;
            Array.Copy( listItemArr, 0, resultArr, COPY_IDX, listItemArr.Length );
            return resultArr;
        }

        /// <summary>
        /// ディクショナリーの指定したキーに対応する値を取得する
        /// </summary>
        /// <param name="dicCondition">ディクショナリーインスタンス</param>
        /// <param name="key">キー名称</param>
        /// <returns>値</returns>
        internal static object GetDictionaryObjectVal( Dictionary<string, object> dicCondition, string key ) {
            if ( null == dicCondition ) {
                return null;
            }

            if ( dicCondition.ContainsKey( key ) ) {
                return dicCondition[key];
            }

            return null;
        }

        /// <summary>
        /// ディクショナリーの指定したキーに対応する値を文字列で取得する
        /// </summary>
        /// <param name="dicCondition">ディクショナリーインスタンス</param>
        /// <param name="key">キー名称</param>
        /// <returns>値</returns>
        internal static string GetDictionaryStringVal( Dictionary<string, object> dicCondition, string key ) {
            return StringUtils.ToString( GetDictionaryObjectVal( dicCondition, key ) );
        }

        /// <summary>
        /// ディクショナリーの指定したキーに対応する値を文字列で取得する
        /// </summary>
        /// <param name="dicCondition">ディクショナリーインスタンス</param>
        /// <param name="key">キー名称</param>
        /// <returns>値</returns>
        internal static DateTime GetDictionaryDateVal( Dictionary<string, object> dicCondition, string key ) {
            return DateUtils.ToDate( GetDictionaryObjectVal( dicCondition, key ) );
        }

        /// <summary>
        /// データテーブルからリスト型へのコンバート
        /// </summary>
        /// <typeparam name="T">型パラメータ</typeparam>
        /// <param name="table">データテーブル</param>
        /// <returns>変換後リストデータ</returns>
        internal static List<T> ConvertToList<T>( DataTable table ) {
            List<T> list = new List<T>();
            foreach ( DataRow row in table.Rows ) {
                try {
                    var item = Activator.CreateInstance( typeof( T ) );
                    foreach ( System.Reflection.PropertyInfo pr in typeof( T ).GetProperties() ) {
                        if ( 0 <= row.Table.Columns.IndexOf( pr.Name ) ) {
                            pr.SetValue( item, ObjectUtils.IsNull( row[pr.Name] ) ? null : Convert.ChangeType( row[pr.Name], pr.PropertyType ), null );
                        }
                    }

                    list.Add( (T)item );
                } catch ( Exception ex ) {
                    throw;
                }
            }
            return list;
        }


        #region 製品情報加工処理
        /// <summary>
        /// 6桁機番取得処理
        /// </summary>
        /// <param name="serial">7桁機番</param>
        /// <returns>6桁機番</returns>
        internal static string GetSerial6( string serial ) {
            //7桁機番桁数
            const int LEN_SERIAL7 = 7;

            //6桁機番開始インデックス
            const int INDEX_SERIAL6_PART_ST = 1;

            string serial6 = "";

            if ( true == StringUtils.IsBlank( serial ) ) {
                return serial6;
            }
            //7桁機番のみ変換実施
            if ( LEN_SERIAL7 == serial.Length ) {
                //7桁機番の6桁部分のみ抽出
                serial6 = serial.Substring( INDEX_SERIAL6_PART_ST );
            } else {
                //7桁以外はそのまま返す
                serial6 = serial;
            }

            //トラクタ機番用:6桁未満の場合にはゼロを付与
            serial6 = GetSerialTractor( serial6 );

            return serial6;
        }

        /// <summary>
        /// トラクタ6桁機番取得処理
        /// </summary>
        /// <param name="serial">トラクタ機番</param>
        /// <returns>トラクタ機番(0pad)</returns>
        internal static string GetSerialTractor( string serial ) {
            //6桁機番桁数
            const int LEN_SERIAL6 = 6;
            //トラクタ機番RPadding文字
            const char TRACTOR_RPAD_STR = '0';

            //6桁未満の場合にはゼロを付与
            if ( 0 < serial.Length && LEN_SERIAL6 > serial.Length ) {
                serial = serial.PadLeft( LEN_SERIAL6, TRACTOR_RPAD_STR );
            }
            return serial;
        }

        /// <summary>
        /// 7桁機番取得処理
        /// </summary>
        /// <param name="productModelCd">生産型式コード</param>
        /// <param name="serial6">6桁機番</param>
        /// <returns>7桁機番</returns>
        internal static string GetSerial( string productModelCd, string serial6 ) {
            return Dao.Com.Serial7Dao.SelectSerial7( productModelCd, serial6 );
        }

        /// <summary>
        /// 型式コード表記取得処理
        /// </summary>
        /// <param name="modelCd">型式コード</param>
        /// <returns>型式コード表記</returns>
        internal static string GetModelCdStr( string modelCd ) {
            //型式コード桁数
            const int LEN_MODEL_CD = 10;
            //型式コードセパレータ
            const string SEPARATER = "-";

            if ( true == StringUtils.IsBlank( modelCd ) ) {
                return modelCd;
            }
            if ( LEN_MODEL_CD != modelCd.Length ) {
                return modelCd;
            }

            return modelCd.Substring( 0, 5 ) + SEPARATER + modelCd.Substring( 5, 5 );

        }

        /// <summary>
        /// 型式コード取得処理
        /// </summary>
        /// <param name="modelCd">型式コード表記</param>
        /// <returns>型式コード</returns>
        internal static string GetModelCd( string modelCdStr ) {
            //型式コード桁数
            const int LEN_MODEL_CD = 10;
            //型式コードセパレータ
            const string SEPARATER = "-";

            if ( true == StringUtils.IsBlank( modelCdStr ) ) {
                return modelCdStr;
            }
            if ( LEN_MODEL_CD + SEPARATER.Length != modelCdStr.Length ) {
                return modelCdStr;
            }

            return modelCdStr.Substring( 0, 5 ) + modelCdStr.Substring( 6, 5 );
        }

        /// <summary>
        /// 国コード取得処理
        /// </summary>
        /// <param name="countryCd">国コード表記</param>
        /// <returns>国コード</returns>
        internal static string GetCountryCd( object countryCdStr ) {
            //NULLの時には変換を行わない
            if ( true == ObjectUtils.IsNull( countryCdStr ) ) {
                return null;
            }
            return GetCountryCd( countryCdStr.ToString() );
        }

        /// <summary>
        /// 国コード取得処理
        /// </summary>
        /// <param name="countryCd">国コード表記</param>
        /// <returns>国コード</returns>
        internal static string GetCountryCd( string countryCdStr ) {
            //国コード桁数
            const int LEN_COUNTRY_CD = 3;
            const string INLAND_COUNTRY = "   ";

            string countryTrim = countryCdStr.Trim();

            //NULLの時には国内として扱う
            if ( true == StringUtils.IsBlank( countryTrim ) ) {
                return INLAND_COUNTRY;
            }

            if ( LEN_COUNTRY_CD < countryTrim.Length ) {
                return countryTrim.Substring( 0, 3 );
            } else {
                return countryTrim;
            }
        }
        #endregion

        #region 工程区分

        #region 工程区分マスタ情報
        /// <summary>
        /// 工程区分マスタ情報
        /// </summary>
        private static List<TrcProcessClass> _processClassList = null;
        private static List<TrcProcessClass> ProcessClassList
        {
            get
            {
                if ( true == ObjectUtils.IsNull( _processClassList ) || 0 == _processClassList.Count ) {
                    //工程区分データが存在しない場合のみ取得する
                    DataTable processInfo = CommonProcessDao.SelectAllProcessClass();
                    _processClassList = ConvertToList<TrcProcessClass>( processInfo );
                }
                return _processClassList;
            }
        }

        /// <summary>
        /// 工程区分テーブル要素
        /// </summary>
        private class TrcProcessClass {
            /// <summary>ラインコード</summary>
            public string LineCd { get; set; }
            /// <summary>工程コード</summary>
            public string ProcessCd { get; set; }
            /// <summary>総称パターンコード</summary>
            public string GeneralPatternCd { get; set; }
            /// <summary>検査コード</summary>
            public string InspectionCd { get; set; }
            /// <summary>組立パターンコード</summary>
            public string AssemblyPatternCd { get; set; }
        }
        #endregion

        /// <summary>
        /// 検査コード取得処理
        /// </summary>
        /// <param name="lineCd">ラインコード,指定しない場合条件に含めない</param>
        /// <param name="processCd">工程コード,指定しない場合条件に含めない</param>
        /// <param name="productKind">製品種別,指定しない場合条件に含めない</param>
        /// <returns>条件に一致する検査コード</returns>
        /// <remarks>複数が該当する場合、先頭の要素を返す</remarks>
        internal static string GetInspectionCd( string lineCd = null, string processCd = null, string productKind = null ) {


            logger.Info( "検査コード取得" );
            logger.Info( "  ラインコード[{0}]  工程コード[{1}]  製品種別[{2}]", lineCd, processCd, productKind );

            string inspectionCd = null;

            inspectionCd = ProcessClassList.AsEnumerable()
                                           .Where( x => ( ( lineCd      == null ) ? true : ( x.LineCd           == lineCd ) )
                                                     && ( ( processCd   == null ) ? true : ( x.ProcessCd        == processCd ) )
                                                     && ( ( productKind == null ) ? true : ( x.GeneralPatternCd == productKind ) ) )
                                           .Select( x => x.InspectionCd )
                                           .FirstOrDefault();

            logger.Info( "  検査コード[{0}]", inspectionCd );

            return inspectionCd;

        }

        /// <summary>
        /// ラインコード取得処理
        /// </summary>
        /// <param name="processCd">工程コード,指定しない場合条件に含めない</param>
        /// <param name="productKind">製品種別,指定しない場合条件に含めない</param>
        /// <param name="inspectionCd">検査コード,指定しない場合条件に含めない</param>
        /// <returns>条件に一致するラインコード</returns>
        /// <remarks>複数が該当する場合、先頭の要素を返す</remarks>
        internal static string GetLineCd( string processCd = null, string productKind = null, string inspectionCd = null ) {

            string lineCd = null;

            lineCd = ProcessClassList.AsEnumerable()
                                     .Where( x => ( ( processCd    == null ) ? true : ( x.ProcessCd        == processCd ) )
                                               && ( ( productKind  == null ) ? true : ( x.GeneralPatternCd == productKind ) )
                                               && ( ( inspectionCd == null ) ? true : ( x.InspectionCd     == inspectionCd ) ) )
                                     .Select( x => x.LineCd )
                                     .FirstOrDefault();

            return lineCd;

        }

        /// <summary>
        /// 工程コード取得処理
        /// </summary>
        /// <param name="lineCd">ラインコード,指定しない場合条件に含めない</param>
        /// <param name="productKind">製品種別,指定しない場合条件に含めない</param>
        /// <param name="inspectionCd">検査コード,指定しない場合条件に含めない</param>
        /// <returns>条件に一致する工程コード</returns>
        /// <remarks>複数が該当する場合、先頭の要素を返す</remarks>
        internal static string GetProcessCd( string lineCd = null, string productKind = null, string inspectionCd = null ) {

            string processCd = null;

            processCd = ProcessClassList.AsEnumerable()
                                        .Where( x => ( ( lineCd       == null ) ? true : ( x.LineCd           == lineCd ) )
                                                  && ( ( productKind  == null ) ? true : ( x.GeneralPatternCd == productKind ) )
                                                  && ( ( inspectionCd == null ) ? true : ( x.InspectionCd     == inspectionCd ) ) )
                                        .Select( x => x.ProcessCd )
                                        .FirstOrDefault();

            return processCd;

        }
        #endregion
    }
}