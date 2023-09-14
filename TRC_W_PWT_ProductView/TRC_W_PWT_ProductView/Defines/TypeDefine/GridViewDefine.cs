using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace TRC_W_PWT_ProductView.Defines.TypeDefine {

    /// <summary>
    /// グリッドビューバインド定義
    /// </summary>
    public class GridViewDefine  {

        /// <summary>
        /// 列名(ヘッダー)
        /// </summary>
        public string headerText;
        /// <summary>
        /// バインドフィールド
        /// </summary>
        public String bindField;
        /// <summary>
        /// フィールドタイプ
        /// </summary>
        public Type dataType;
        /// <summary>
        /// フォーマット
        /// </summary>
        public String format;
        /// <summary>
        /// ソート有無
        /// </summary>
        public bool sorting;
        /// <summary>
        /// 表示位置(左寄せ、センタリング、右寄せ)
        /// </summary>
        public HorizontalAlign align;
        /// <summary>
        /// 列幅
        /// </summary>
        /// <remarks>
        ///  0 = Auto 非表示の時には、visible(表示有無)で指定
        /// </remarks>
        public int width;
        /// <summary>
        /// 表示有無
        /// </summary>
        public bool visible;
        /// <summary>
        /// ソート列
        /// </summary>
        public String sortingCol;
        /// <summary>
        /// ラベルを自動作成し、バインド処理を行う
        /// </summary>
        public bool bindLabel;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="headerText">列名(ヘッダー)</param>
        /// <param name="bindField">バインドフィールド</param>
        /// <param name="format">フォーマット</param>
        /// <param name="sorting">ソート有無</param>
        /// <param name="align">表示位置(左寄せ、センタリング、右寄せ)</param>
        /// <param name="visible">表示有無</param>
        public GridViewDefine( string headerText, String bindField, Type dataType, String format, bool sorting, HorizontalAlign align, int width, bool visible ) {
            this.headerText = headerText;
            this.bindField = bindField;
            this.dataType = dataType;
            this.format = format;
            this.sorting = sorting;
            if ( true == sorting ) {
                this.sortingCol = bindField;
            }
            this.align = align;
            this.width = width;
            this.visible = visible;
            this.bindLabel = false;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="headerText">列名(ヘッダー)</param>
        /// <param name="bindField">バインドフィールド</param>
        /// <param name="format">フォーマット</param>
        /// <param name="sorting">ソート有無</param>
        /// <param name="align">表示位置(左寄せ、センタリング、右寄せ)</param>
        /// <param name="visible">表示有無</param>
        public GridViewDefine( string headerText, String bindField, Type dataType, String format, bool sorting, string sortingCol, HorizontalAlign align, int width, bool visible ) {
            this.headerText = headerText;
            this.bindField = bindField;
            this.dataType = dataType;
            this.format = format;
            this.sorting = sorting;
            this.sortingCol = sortingCol;
            this.align = align;
            this.width = width;
            this.visible = visible;
            this.bindLabel = false;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="headerText">列名(ヘッダー)</param>
        /// <param name="bindField">バインドフィールド</param>
        /// <param name="format">フォーマット</param>
        /// <param name="sorting">ソート有無</param>
        /// <param name="align">表示位置(左寄せ、センタリング、右寄せ)</param>
        /// <param name="visible">表示有無</param>
        public GridViewDefine( string headerText, String bindField, Type dataType, String format, bool sorting, HorizontalAlign align, int width, bool visible, bool bindLabel ) {
            this.headerText = headerText;
            this.bindField = bindField;
            this.dataType = dataType;
            this.format = format;
            this.sorting = sorting;
            if ( true == sorting ) {
                this.sortingCol = bindField;
            }
            this.align = align;
            this.width = width;
            this.visible = visible;
            this.bindLabel = bindLabel;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="headerText">列名(ヘッダー)</param>
        /// <param name="bindField">バインドフィールド</param>
        /// <param name="format">フォーマット</param>
        /// <param name="sorting">ソート有無</param>
        /// <param name="sortingCol">ソート列</param>
        /// <param name="align">表示位置(左寄せ、センタリング、右寄せ)</param>
        /// <param name="visible">表示有無</param>
        /// <param name="bindLabel">バインド用のラベル自動作成(bindFieldの代わりにTemplateFieldにラべルを作成)</param>
        public GridViewDefine( string headerText, String bindField, Type dataType, String format, bool sorting, string sortingCol, HorizontalAlign align, int width, bool visible, bool bindLabel ) {
            this.headerText = headerText;
            this.bindField = bindField;
            this.dataType = dataType;
            this.format = format;
            this.sorting = sorting;
            this.sortingCol = sortingCol;
            this.align = align;
            this.width = width;
            this.visible = visible;
            this.bindLabel = bindLabel;
        }
    }
}