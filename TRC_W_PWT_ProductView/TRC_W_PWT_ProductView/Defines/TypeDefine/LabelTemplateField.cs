using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using KTFramework.Common;

namespace TRC_W_PWT_ProductView.Defines.TypeDefine {
    public class LabelTemplateField:ITemplate {

        /// <summary>
        /// ボーダー幅 補正
        /// </summary>
        const int BORDER_WIDTH = 2;

        /// <summary>
        /// CSS定義 中央揃え al-ct
        /// </summary>
        const string CSS_ALIGN_CENTER = "al-ct";
        /// <summary>
        /// CSS定義 中央揃え al-lf
        /// </summary>
        const string CSS_ALIGN_LEFT = "al-lf";
        /// <summary>
        /// CSS定義 中央揃え al-rg
        /// </summary>
        const string CSS_ALIGN_RIGHT = "al-rg";
        /// <summary>
        /// CSS定義 txt-width-full readonly
        /// </summary>
        const string CSS_FULLWIDTH_READONLY = "txt-width-full readonly";

        /// <summary>
        /// 属性 動的バインド用フィールド
        /// </summary>
        const string ATTR_BIND_FIELD = "attr_bindField";
        /// <summary>
        /// 属性 動的バインド列フォーマット
        /// </summary>
        const string ATTR_FORMAT = "attr_formatString";
        /// <summary>
        /// 属性 動的バインド列文字揃え
        /// </summary>
        const string ATTR_CSS = "attr_css";
        /// <summary>
        /// 属性 動的バインド列表示 表示判定
        /// </summary>
        public const string ATTR_VISIBLE = "attr_visible";

        GridViewDefine _gridDef = null;
        public LabelTemplateField( GridViewDefine gridDef ) {
            _gridDef = gridDef;
        }

        /// <summary>
        /// ラベルコントロール属性セット
        /// </summary>
        public void SetLabelAttr( ref Label label ) {
            label.EnableViewState = true;
            label.ID = "lbl_" + _gridDef.bindField;
            label.Visible = true;
            label.Width = _gridDef.width;//            -BORDER_WIDTH;
            label.Attributes[ATTR_BIND_FIELD] = _gridDef.bindField;
            label.Attributes[ATTR_FORMAT] = _gridDef.format;

            string css = CSS_FULLWIDTH_READONLY;
            if ( _gridDef.align == HorizontalAlign.Center ) {
                css += " " + CSS_ALIGN_CENTER;
            } else if ( _gridDef.align == HorizontalAlign.Left ) {
                css += " " + CSS_ALIGN_LEFT;
            } else if ( _gridDef.align == HorizontalAlign.Right ) {
                css += " " + CSS_ALIGN_RIGHT;
            }

            label.Attributes[ATTR_CSS] = css;
        }

        public void InstantiateIn( System.Web.UI.Control container ) {
            Label label = new Label();
            SetLabelAttr( ref label );
            label.DataBinding += new EventHandler( this.OnDataBinding );
            container.Controls.Add( label );
        }

        public void OnDataBinding( object sender, EventArgs e ) {
            Label label = (Label)sender;
            GridViewRow container = (GridViewRow)label.NamingContainer;

            object result = ( (DataRowView)container.DataItem )[label.Attributes[ATTR_BIND_FIELD]];
            string formatString = label.Attributes[ATTR_FORMAT].ToString();
            string css = label.Attributes[ATTR_CSS].ToString();
            if ( true == StringUtils.IsNotEmpty( formatString ) ) {
                result = String.Format( formatString, result);
            }
            label.CssClass = css;
            label.Text = result.ToString();
            label.Attributes[ATTR_VISIBLE] = true.ToString();
            if ( StringUtils.IsEmpty( label.Text ) ) {
                label.Attributes[ATTR_VISIBLE] = false.ToString();
            }
        }
    }
}