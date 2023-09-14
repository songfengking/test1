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

namespace TRC_W_PWT_ProductView.UI.Pages.PartsView {
    /// <summary>
    /// トラクタ部品詳細画面:銘板ラベル
    /// </summary>
    public partial class Nameplate : System.Web.UI.UserControl, Defines.Interface.IDetail {

        //ロガー定義
        private static readonly Logger logger = new Logger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

        #region 定数定義
        /// <summary>
        /// 部品組付情報
        /// </summary>
        public class GRID_TRACTOR {
            /// <summary>ラベル種別</summary>
            public static readonly ControlDefine PLATE_TYPE = new ControlDefine( "txtPlateTypeTractor", "ラベル種別", "plateType", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>銘板名</summary>
            public static readonly ControlDefine NAME_PLATE_CD = new ControlDefine( "txtNamePlateCdTractor", "銘板コード", "namePlateCd", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>銘板名</summary>
            public static readonly ControlDefine NAME_PLATE_NM = new ControlDefine( "txtNamePlateNmTractor", "銘板名", "namePlateNm", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>製造番号</summary>
            public static readonly ControlDefine SUB_PRODUCT_CD = new ControlDefine( "txtSubProductCdTractor", "製造番号", "subProductCd", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>発行年月日日時(yyyy/MM/dd hh:mm:ss)</summary>
            public static readonly ControlDefine PRINT_DT = new ControlDefine( "txtPrintDtTractor", "発行日時", "installDt", ControlDefine.BindType.Down, typeof( DateTime ) );

        }

        /// <summary>
        /// 部品組付情報
        /// </summary>
        public class GRID_CAB {
            /// <summary>ラベル種別</summary>
            public static readonly ControlDefine PLATE_TYPE = new ControlDefine( "txtPlateTypeCab", "ラベル種別", "plateType", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>銘板名</summary>
            public static readonly ControlDefine NAME_PLATE_CD = new ControlDefine( "txtNamePlateCdCab", "銘板コード", "namePlateCd", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>銘板名</summary>
            public static readonly ControlDefine NAME_PLATE_NM = new ControlDefine( "txtNamePlateNmCab", "銘板名", "namePlateNm", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>製造番号</summary>
            public static readonly ControlDefine SUB_PRODUCT_CD = new ControlDefine( "txtSubProductCdCab", "製造番号", "subProductCd", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>発行年月日日時(yyyy/MM/dd hh:mm:ss)</summary>
            public static readonly ControlDefine PRINT_DT = new ControlDefine( "txtPrintDtCab", "発行日時", "installDt", ControlDefine.BindType.Down, typeof( DateTime ) );

        }

        /// <summary>
        /// 部品組付情報
        /// </summary>
        public class GRID_ROPS {
            /// <summary>ラベル種別</summary>
            public static readonly ControlDefine PLATE_TYPE = new ControlDefine( "txtPlateTypeRops", "ラベル種別", "plateType", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>銘板名</summary>
            public static readonly ControlDefine NAME_PLATE_CD = new ControlDefine( "txtNamePlateCdRops", "銘板コード", "namePlateCd", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>銘板名</summary>
            public static readonly ControlDefine NAME_PLATE_NM = new ControlDefine( "txtNamePlateNmRops", "銘板名", "namePlateNm", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>製造番号</summary>
            public static readonly ControlDefine SUB_PRODUCT_CD = new ControlDefine( "txtSubProductCdRops", "製造番号", "subProductCd", ControlDefine.BindType.Down, typeof( string ) );
            /// <summary>発行年月日日時(yyyy/MM/dd hh:mm:ss)</summary>
            public static readonly ControlDefine PRINT_DT = new ControlDefine( "txtPrintDtRops", "発行日時", "installDt", ControlDefine.BindType.Down, typeof( DateTime ) );

        }

        /// <summary>
        /// 印刷種別
        /// </summary>
        public static class PlateType {
            //本機
            public static readonly string TRACTOR = "本機";
            //CAB
            public static readonly string CAB = "CAB";
            //ROPS
            public static readonly string ROPS = "ROPS";
        }

        #endregion

        #region プロパティ
        /// <summary>
        /// 表示中ページ(ベースフォーム)情報
        /// </summary>
        private BaseForm CurrentForm => ( BaseForm )Page;

        /// <summary>
        /// 表示中ユーザコントロール情報
        /// </summary>
        private PageInfo.ST_PAGE_INFO CurrentUCInfo => PageInfo.GetUCPageInfo( DetailKeyParam.ProductKind, DetailKeyParam.GroupCd, DetailKeyParam.ClassCd );

        /// <summary>
        /// 詳細表示キー情報
        /// </summary>
        public Defines.Interface.ST_DETAIL_PARAM DetailKeyParam { get; set; }
        #endregion

        #region イベントメソッド

        #region ページイベント

        /// <summary>
        /// 初期化処理
        /// </summary>
        public void Initialize() {

            //検索結果取得
            DataTable tblMain;
            try {
                tblMain = Business.DetailViewBusiness.SelectTractorNameplate( DetailKeyParam.ProductModelCd, DetailKeyParam.Serial ).MainTable;
            } catch ( DataAccessException ex ) {
                logger.Exception( ex );
                CurrentForm.WriteApplicationMessage( MsgManager.MESSAGE_ERR_80010 );
                return;
            } catch ( Exception ex ) {
                logger.Exception( ex );
                CurrentForm.WriteApplicationMessage( MsgManager.MESSAGE_ERR_80010 );
                return;
            } 

            if ( 0 == tblMain.Rows.Count ) {
                //検索結果0件
                CurrentForm.WriteApplicationMessage( MsgManager.MESSAGE_WRN_62010, CurrentUCInfo.title );
                return;
            }

            InitializeValues( tblMain );
        }

        #endregion

        /// <summary>
        /// 初期値設定
        /// </summary>
        private void InitializeValues( DataTable tblMainData ) {

            //本機表示
            DataRow tractorRow = tblMainData.AsEnumerable()
                                     .FirstOrDefault( r => r.Field<string>( GRID_TRACTOR.PLATE_TYPE.bindField ).Equals( PlateType.TRACTOR ) );
            if ( tractorRow != null ) {
                Dictionary<string, object> _ = new Dictionary<string, object>();
                CurrentForm.SetControlValues( this, ControlUtils.GetControlDefineArray( typeof( GRID_TRACTOR ) ), tractorRow, ref _ );
                divTractor.Visible = true;
            }

            //CAB表示
            DataRow cabRow = tblMainData.AsEnumerable()
                                     .FirstOrDefault( r => r.Field<string>( GRID_CAB.PLATE_TYPE.bindField ).Equals( PlateType.CAB ) );
            if ( cabRow != null ) {
                Dictionary<string, object> _ = new Dictionary<string, object>();
                CurrentForm.SetControlValues( this, ControlUtils.GetControlDefineArray( typeof( GRID_CAB ) ), cabRow, ref _ );
                divCab.Visible = true;
            }

            //ROPS表示
            DataRow ropsRow = tblMainData.AsEnumerable()
                                     .FirstOrDefault( r => r.Field<string>( GRID_ROPS.PLATE_TYPE.bindField ).Equals( PlateType.ROPS ) );
            if ( ropsRow != null ) {
                Dictionary<string, object> _ = new Dictionary<string, object>();
                CurrentForm.SetControlValues( this, ControlUtils.GetControlDefineArray( typeof( GRID_ROPS ) ), ropsRow, ref _ );
                divRops.Visible = true;
            }

        }

        #endregion

    }
}