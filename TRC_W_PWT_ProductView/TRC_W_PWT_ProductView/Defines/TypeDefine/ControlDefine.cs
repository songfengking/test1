using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TRC_W_PWT_ProductView.Defines.TypeDefine {

    /// <summary>
    /// コントロール定義
    /// </summary>
    public class ControlDefine  {
        /// <summary>
        /// コントロール
        /// </summary>
        public string controlId;
        /// <summary>
        /// 表示名
        /// </summary>
        public string displayNm;
        /// <summary>
        /// バインドフィールド
        /// </summary>
        public String bindField;
        /// <summary>
        /// バインドタイプ
        /// </summary>
        public BindType bindType;
        /// <summary>
        /// 表示入力値Type
        /// </summary>
        public Type valueTp;

        /// <summary>
        /// コンストラクタ(BindTypeなし、TypeをStringで定義します)
        /// </summary>
        /// <param name="controlId">コントロールID</param>
        /// <param name="displayNm">表示名</param>
        public ControlDefine( string controlId, string displayNm ) {
            this.controlId = controlId;
            this.displayNm = displayNm;
            this.bindType = BindType.None;
            this.valueTp = typeof( String );
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="controlId">コントロールID</param>
        /// <param name="displayNm">表示名</param>
        /// <param name="bindType">バインドタイプ</param>
        /// <param name="valueTp">表示入力値Type</param>
        public ControlDefine( string controlId, string displayNm, String bindField, BindType bindType, Type valueTp ) {
            this.controlId = controlId;
            this.displayNm = displayNm;
            this.bindField = bindField;
            this.bindType = bindType;
            this.valueTp = valueTp;
        }

        /// <summary>
        /// バインドタイプ
        /// </summary>
        public enum BindType {
            /// <summary>
            /// バインド処理なし
            /// </summary>
            None = 0,
            /// <summary>
            /// バインド処理(Server→Client)
            /// </summary>
            Down = 1,
            /// <summary>
            /// バインド処理(Client→Server)
            /// </summary>
            Up   = 2,
            /// <summary>
            /// バインド処理(双方向)
            /// </summary>
            Both = 3,
        }
    }
}