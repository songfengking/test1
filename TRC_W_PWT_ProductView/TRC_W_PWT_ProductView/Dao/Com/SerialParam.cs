using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TRC_W_PWT_ProductView.Common;

namespace TRC_W_PWT_ProductView.Dao.Com {
    public class SerialParam {
        /// <summary>生産型式コード</summary>
        public string productModelCd;
        /// <summary>機番</summary>
        public string serial;
        /// <summary>機番(6桁)</summary>
        public string serial6;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="productModelCd">生産型式コード</param>
        /// <param name="serial">機番</param>
        public SerialParam( string productModelCd, string serial ) {
            this.productModelCd = productModelCd;
            this.serial = serial;
            this.serial6 = DataUtils.GetSerial6(serial);
        }
    }
}