using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace TRC_W_PWT_ProductView.Defines.ListDefine {

    /// <summary>
    /// 引当先倉庫区分
    /// </summary>
    public class CallEngineWarehouse {

        /// <summary>引当先倉庫区分:筑波立体1号機</summary>
        public const string MACHINE_NO_01 = "01";
        /// <summary>引当先倉庫区分:筑波立体2号機</summary>
        public const string MACHINE_NO_02 = "02";

        /// <summary>引当先倉庫区分:筑波立体1号機</summary>
        public const string NAME_MACHINE_NO_01 = "筑波立体1号機";
        /// <summary>引当先倉庫区分:筑波立体2号機</summary>
        public const string NAME_MACHINE_NO_02 = "筑波立体2号機";


        /// <summary>
        /// 引当先倉リスト
        /// </summary>
        private static readonly ListItem[] CallEngineWarehouseList = new ListItem[]
        {
            new ListItem( NAME_MACHINE_NO_01, MACHINE_NO_01),
            new ListItem( NAME_MACHINE_NO_02, MACHINE_NO_02 ),
        };

        /// <summary>
        /// リストを取得する
        /// </summary>
        /// <param name="addBlank">空白行追加有無</param>
        /// <returns>リストアイテム配列</returns>
        internal static ListItem[] GetList( bool addBlank = true ) {

            ListItem[] resultArr = null;
            if ( true == addBlank ) {
                resultArr = Common.DataUtils.InsertBlankItem( CallEngineWarehouseList );
            } else {
                resultArr = CallEngineWarehouseList;
            }

            return resultArr;

        }

    }
}