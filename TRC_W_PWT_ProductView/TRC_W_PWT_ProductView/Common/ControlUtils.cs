using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;
using KTFramework.Common;
using KTWebControl.CustomControls;
using TRC_W_PWT_ProductView.Defines;
using TRC_W_PWT_ProductView.Defines.TypeDefine;
using TRC_W_PWT_ProductView.Session;

namespace TRC_W_PWT_ProductView.Common {

    /// <summary>
    /// コントロール制御クラス
    /// </summary>
    public static class ControlUtils {

        #region 定数定義

        #region イベント(Attributes)
        /// <summary>
        /// オブジェクトクリック
        /// </summary>
        public const string ON_CLICK = "onclick";
        /// <summary>
        /// オブジェクトダブルクリック
        /// </summary>
        public const string ON_DBLCLICK = "ondblclick";
        /// <summary>
        /// オブジェクトキーアップ
        /// </summary>
        public const string ON_KEY_UP = "onkeyup";

        /// <summary>
        /// (グリッドビュー)グループコード
        /// </summary>
        public const string GROUP_CD = "group_cd";
        /// <summary>
        /// (グリッドビュー)グリッドインデックス
        /// </summary>
        public const string GRID_INDEX = "grid_index";
        /// <summary>
        /// (グリッドビュー)データインデックス
        /// </summary>
        public const string DATA_INDEX = "data_index";

        #endregion

        #region グリッドビュー用

        /// <summary>
        /// △(ASC)
        /// </summary>
        const string ALLOW_UP = " △";

        /// <summary>
        /// ▼(DESC)
        /// </summary>
        const string ALLOW_DOWN = " ▼";

        /// <summary>
        /// グリッド行ダブルクリックイベント
        /// </summary>
        public enum GridRowDoubleClickEvent {
            None = 0,
            WindowOpen,
        }

        #region スクリプトイベント
        /// <summary>
        /// グリッドビュー行選択イベント
        /// </summary>
        const string GRID_VIEW_SELECTED = "ControlCommon.SelectGridRow(this,{0});";
        /// <summary>
        /// グリッドビュー行選択イベント(ヘッダ固定用)
        /// </summary>
        const string GRID_VIEW_SELECTED_SYNCHRO = "ControlCommon.SelectGridRowSynchro(this,{0});";
        /// <summary>
        /// グリッドビュー行ダブルクリックイベント
        /// </summary>
        const string GRID_VIEW_DOUBLE_CLICK = "ControlCommon.DoubleClickGridRow({0},{1},'{2}','{3}');";
        /// <summary>
        /// グリッドビュー行ダブルクリックイベント
        /// </summary>
        const string GRID_VIEW_DOUBLE_CLICK_WITH_CALLER = "ControlCommon.DoubleClickGridRowWithCaller({0},{1},'{2}','{3}','{4}');";

        /// <summary>
        /// リストビュー行選択イベント
        /// </summary>
        public const string LIST_VIEW_SELECTED = "ControlCommon.SelectListViewRow(this,{0},'{1}');";

        #endregion

        #endregion

        #endregion

        #region リストコントロール用

        /// <summary>
        /// リストコントロール初期化
        /// </summary>
        /// <param name="listControl">リストコントロール</param>
        internal static void ClearListControlItems( ListControl listControl ) {
            listControl.Items.Clear();
            listControl.SelectedValue = null;
        }


        /// <summary>
        /// リストコントロールへアイテムを設定
        /// </summary>
        /// <param name="listControl">リストコントロール</param>
        /// <param name="listItemArr">リストアイテム配列</param>
        internal static void SetListControlItems( ListControl listControl, ListItem[] listItemArr ) {
            ClearListControlItems( listControl );
            if ( true == ObjectUtils.IsNull( listItemArr ) || 0 == listItemArr.Length ) {
                return;
            }

            listControl.Items.AddRange( listItemArr );
        }

        /// <summary>
        /// データテーブルからリストアイテムに変換
        /// </summary>
        /// <param name="tblData">リストアイテム用元データ</param>
        /// <returns>リストアイテム配列</returns>
        /// <remarks>
        /// 以下の値を固定で使用します
        /// txtKey = text
        /// valueKey = value
        /// </remarks>
        internal static ListItem[] GetListItems( DataTable tblData ) {
            const string TXT_FIELD = "text";
            const string VAL_FIELD = "value";
            return GetListItems( tblData, TXT_FIELD, VAL_FIELD );
        }

        /// <summary>
        /// データテーブルからリストアイテムに変換
        /// </summary>
        /// <param name="tblData">リストアイテム用元データ</param>
        /// <param name="txtKey">テキスト用フィールド名</param>
        /// <param name="valueKey">値用フィールド名</param>
        /// <param name="addBlank">空白行追加有無</param>
        /// <returns>リストアイテム配列</returns>
        internal static ListItem[] GetListItems( DataTable tblData, string txtKey, string valueKey, bool addBlank = false ) {

            ListItem[] liArr = new ListItem[0];
            if ( ObjectUtils.IsNull( tblData ) || 0 == tblData.Rows.Count ) {
                return liArr;
            }

            DataRow[] rowArr = new DataRow[tblData.Rows.Count];
            tblData.Rows.CopyTo( rowArr, 0 );
            return GetListItems( rowArr, txtKey, valueKey, addBlank );
        }

        /// <summary>
        /// データ行配列からリストアイテムに変換
        /// </summary>
        /// <param name="rowArr">リストアイテム用元データ</param>
        /// <param name="txtKey">テキスト用フィールド名</param>
        /// <param name="valueKey">値用フィールド名</param>
        /// <param name="addBlank">空白行追加有無</param>
        /// <returns>リストアイテム配列</returns>
        internal static ListItem[] GetListItems( DataRow[] rowArr, string txtKey, string valueKey, bool addBlank = false ) {

            ListItem[] liArr = new ListItem[0];
            if ( ObjectUtils.IsNull( rowArr ) || 0 == rowArr.Length ) {
                return liArr;
            }

            Array.Resize( ref liArr, rowArr.Length );

            for ( int loop = 0; loop < rowArr.Length; loop++ ) {
                DataRow row = rowArr[loop];
                liArr[loop] = new ListItem( row[txtKey].ToString(), row[valueKey].ToString() );
            }

            //空白行追加
            if ( true == addBlank ) {
                liArr = Common.DataUtils.InsertBlankItem( liArr );
            }

            return liArr;
        }

        #endregion

        #region クラス定義情報取得
        /// <summary>
        /// クラス定義情報取得
        /// </summary>
        /// <param name="tpStaticClass">静的定義クラス</param>
        /// <returns>コントロール定義情報</returns>
        internal static Object[] GetStaticDefineArray( Type tpStaticClass, Type findTp ) {

            object[] result = new object[0];
            FieldInfo[] fieldInfos = tpStaticClass.GetFields();

            foreach ( FieldInfo fi in fieldInfos ) {

                if ( false == CheckInheritType( fi.GetValue( null ).GetType(), findTp ) ) {
                    continue;
                }

                Array.Resize( ref result, result.Length + 1 );
                result[result.Length - 1] = fi.GetValue( null );
            }
            return result;
        }

        #endregion

        #region ControlDefine用

        /// <summary>
        /// コントロール定義情報取得
        /// </summary>
        /// <param name="tpControlDefineClass">コントロール定義クラス</param>
        /// <returns>コントロール定義情報</returns>
        internal static ControlDefine[] GetControlDefineArray( Type tpControlDefineClass ) {

            ControlDefine[] result = new ControlDefine[0];

            FieldInfo[] fieldInfos = tpControlDefineClass.GetFields();

            foreach ( FieldInfo fi in fieldInfos ) {

                if ( false == CheckInheritType( fi.GetValue( null ).GetType(), typeof( ControlDefine ) ) ) {
                    continue;
                }

                Array.Resize( ref result, result.Length + 1 );
                result[result.Length - 1] = (ControlDefine)fi.GetValue( null );

            }

            return result;

        }

        #endregion

        #region GridViewDefine用

        /// <summary>
        /// グリッドビュー定義情報取得
        /// </summary>
        /// <param name="tpGridViewDefineClass">グリッドビュー定義クラス</param>
        /// <returns>グリッドビュー定義情報</returns>
        internal static GridViewDefine[] GetGridViewDefineArray( Type tpGridViewDefineClass ) {

            GridViewDefine[] result = new GridViewDefine[0];

            FieldInfo[] fieldInfos = tpGridViewDefineClass.GetFields();

            foreach ( FieldInfo fi in fieldInfos ) {

                if ( false == CheckInheritType( fi.GetValue( null ).GetType(), typeof( GridViewDefine ) ) ) {
                    continue;
                }

                Array.Resize( ref result, result.Length + 1 );
                result[result.Length - 1] = (GridViewDefine)fi.GetValue( null );

            }

            return result;

        }

        /// <summary>
        /// グリッドビューデータ初期化
        /// </summary>
        /// <param name="grv">グリッドビューコントロール</param>
        internal static void InitializeGridView( GridView grv ) {
            grv.PageIndex = 0;
            grv.DataSource = null;
            grv.Columns.Clear();
            grv.DataBind();
        }
        /// <summary>
        /// グリッドビューデータ初期化
        /// </summary>
        /// <param name="grv">グリッドビューコントロール</param>
        internal static void InitializeGridView( GridView grv, bool isClearColumns = false ) {
            grv.PageIndex = 0;
            grv.DataSource = null;
            if ( true == isClearColumns ) {
                grv.Columns.Clear();
            }
            grv.DataBind();
        }
        /// <summary>
        /// グリッドビューデータヘッダ表示のみ
        /// </summary>
        /// <param name="grv">グリッドビューコントロール</param>
        internal static void ShowGridViewHeader( GridView grv, GridViewDefine[] gridViewDefineArr ) {

            InitializeGridView( grv );

            grv.AutoGenerateColumns = false;
            grv.ShowHeaderWhenEmpty = true;

            foreach ( GridViewDefine grvDef in gridViewDefineArr ) {

                if ( true == StringUtils.IsEmpty( grvDef.bindField ) ) {
                    continue;
                }

                BoundField bf = new BoundField();
                bf.HeaderText = grvDef.headerText;
                bf.HeaderStyle.Width = grvDef.width;
                bf.Visible = grvDef.visible;

                grv.Columns.Add( bf );
            }

            grv.DataSource = new DataTable();
            grv.DataBind();
        }
        /// <summary>
        /// グリッドビューデータヘッダ表示のみ
        /// </summary>
        /// <param name="grv">グリッドビューコントロール</param>
        internal static void ShowGridViewHeader( GridView grv, GridViewDefine[] gridViewDefineArr, ConditionInfoSessionHandler.ST_CONDITION cond, bool isSort ) {

            InitializeGridView( grv, true );

            grv.AutoGenerateColumns = false;
            grv.ShowHeaderWhenEmpty = true;

            double sumWidth = 0;
            foreach ( GridViewDefine grvDef in gridViewDefineArr ) {

                BoundField bf = new BoundField();
                bf.HtmlEncode = false;
                bf.HeaderText = grvDef.headerText;
                bf.HeaderStyle.Width = grvDef.width;
                bf.HeaderStyle.Height = 20;

                bf.Visible = grvDef.visible;

                if ( true == isSort && true == grvDef.sorting ) {
                    bf.SortExpression = grvDef.sortingCol;
                }

                grv.Columns.Add( bf );

                if ( true == bf.Visible ) {
                    sumWidth += bf.HeaderStyle.Width.Value;
                }
            }

            RenameSortedColumnNm( grv, cond );

            grv.HeaderStyle.Width = new Unit( sumWidth );
            grv.DataSource = new DataTable();
            grv.DataBind();
        }
        /// <summary>
        /// グリッドビューデータバインド
        /// </summary>
        /// <param name="grv">グリッドビューコントロール</param>
        /// <param name="gridViewColumnDef">グリッドビュー定義配列</param>
        /// <param name="tblData">バインド元データ</param>
        internal static void BindGridView( GridView grv, GridViewDefine[] gridViewColumnDef, DataTable tblData ) {

            //元データがNULLの時には、ヘッダのみ表示
            if ( true == ObjectUtils.IsNull( tblData ) ) {
                ShowGridViewHeader( grv, gridViewColumnDef );
                return;
            }

            InitializeGridView( grv );
            grv.AutoGenerateColumns = false;

            foreach ( GridViewDefine grvDef in gridViewColumnDef ) {

                if ( true == StringUtils.IsEmpty( grvDef.bindField ) ) {
                    continue;
                }

                BoundField bf = new BoundField();
                bf.HeaderText = grvDef.headerText;

                //バインドデータ及び列定義のフィールドがあればバインド
                if ( true == ObjectUtils.IsNotNull( tblData )
                    && 0 <= tblData.Columns.IndexOf( grvDef.bindField ) ) {

                    bf.DataField = grvDef.bindField;
                    bf.DataFormatString = grvDef.format;
                    bf.ItemStyle.HorizontalAlign = grvDef.align;
                    bf.ItemStyle.Wrap = false;
                    if ( true == grvDef.sorting ) {
                        bf.SortExpression = grvDef.bindField;
                    }
                }

                bf.HeaderStyle.Width = grvDef.width;
                bf.HeaderStyle.Wrap = false;
                bf.Visible = grvDef.visible;

                grv.Columns.Add( bf );

            }
            grv.DataSource = tblData.DefaultView;
            grv.DataBind();

        }
        /// <summary>
        /// グリッドビューデータバインド
        /// </summary>
        /// <param name="grv">グリッドビューコントロール</param>
        /// <param name="gridViewColumnDef">グリッドビュー定義配列</param>
        /// <param name="tblData">バインド元データ</param>
        internal static void BindGridView_WithTempField( GridView grv, GridViewDefine[] gridViewColumnDef, DataTable tblData ) {

            InitializeGridView( grv, false );
            grv.AutoGenerateColumns = false;

            int colIdx = 0;
            double sumWidth = 0;

            foreach ( GridViewDefine grvDef in gridViewColumnDef ) {

                ////ラベルテンプレート用にテンプレートフィールドを追加
                //if ( true == grvDef.bindLabel ) {
                //    grv.Columns.Insert( colIdx, new TemplateField() );
                //}

                int addBorderWidth = 1;
                if ( colIdx == 0 ) {
                    addBorderWidth += 1;
                }

                if ( grv.Columns[colIdx] is BoundField ) {

                    BoundField bf = (BoundField)grv.Columns[colIdx];
                    bf.HeaderText = grvDef.headerText;

                    bf.HeaderStyle.Width = grvDef.width;
                    bf.HeaderStyle.Wrap = false;

                    //バインドデータ及び列定義のフィールドがあればバインド
                    if ( true == ObjectUtils.IsNotNull( tblData )
                        && 0 <= tblData.Columns.IndexOf( grvDef.bindField ) ) {

                        bf.DataField = grvDef.bindField;
                        bf.DataFormatString = grvDef.format;
                        bf.ItemStyle.HorizontalAlign = grvDef.align;
                        bf.ItemStyle.Wrap = false;
                        bf.ItemStyle.Width = bf.HeaderStyle.Width;

                        if ( true == grvDef.sorting ) {
                            bf.SortExpression = grvDef.sortingCol;
                        }
                    }

                    bf.Visible = grvDef.visible;

                } else if ( grv.Columns[colIdx] is TemplateField ) {

                    TemplateField tf = (TemplateField)grv.Columns[colIdx];

                    tf.HeaderText = grvDef.headerText;
                    tf.HeaderStyle.Width = grvDef.width;
                    tf.HeaderStyle.Wrap = false;
                    tf.Visible = grvDef.visible;
                    tf.ItemStyle.HorizontalAlign = grvDef.align;
                    tf.ItemStyle.Width = tf.HeaderStyle.Width;

                    if ( true == grvDef.sorting ) {
                        tf.SortExpression = grvDef.sortingCol;
                    }

                    MakeBoundTemplateFieldCtrl( ref tf, grvDef );
                } else if ( grv.Columns[colIdx] is ButtonField ) {

                    ButtonField bf = (ButtonField)grv.Columns[colIdx];
                    bf.HeaderText = grvDef.headerText;

                    bf.HeaderStyle.Width = grvDef.width;
                    bf.HeaderStyle.Wrap = false;

                    //バインドデータ及び列定義のフィールドがあればバインド
                    if ( true == ObjectUtils.IsNotNull( tblData )
                        && 0 <= tblData.Columns.IndexOf( grvDef.bindField ) ) {

                        bf.DataTextField = grvDef.bindField;
                        bf.DataTextFormatString = grvDef.format;
                        bf.ItemStyle.HorizontalAlign = grvDef.align;
                        bf.ItemStyle.Wrap = false;
                        bf.ItemStyle.Width = bf.HeaderStyle.Width;

                        if ( true == grvDef.sorting ) {
                            bf.SortExpression = grvDef.sortingCol;
                        }
                    }

                    bf.Visible = grvDef.visible;

                }

                if ( true == grv.Columns[colIdx].Visible ) {
                    sumWidth += grv.Columns[colIdx].HeaderStyle.Width.Value;
                }

                colIdx++;

            }

            //grv.HeaderStyle.Width = new Unit( sumWidth );

            if ( true == ObjectUtils.IsNotNull( tblData ) ) {
                grv.DataSource = tblData.DefaultView;
                grv.DataBind();
            }

        }

        /// <summary>
        /// グリッドビューデータバインド
        /// </summary>
        /// <param name="grv">グリッドビューコントロール</param>
        /// <param name="gridViewColumnDef">グリッドビュー定義配列</param>
        /// <param name="tblData">バインド元データ</param>
        internal static void SetGridViewTemplateField( GridView grv, GridViewDefine[] gridViewColumnDef ) {

            int colIdx = 0;

            foreach ( GridViewDefine grvDef in gridViewColumnDef ) {

                int addBorderWidth = 1;
                if ( colIdx == 0 ) {
                    addBorderWidth += 1;
                }

                if ( grv.Columns[colIdx] is TemplateField ) {

                    TemplateField tf = (TemplateField)grv.Columns[colIdx];
                    tf.HeaderText = grvDef.headerText;

                    tf.HeaderStyle.Width = grvDef.width;
                    tf.HeaderStyle.Wrap = false;
                    tf.Visible = grvDef.visible;
                    tf.ItemStyle.HorizontalAlign = grvDef.align;
                    tf.ItemStyle.Width = tf.HeaderStyle.Width;

                    if ( true == grvDef.sorting ) {
                        tf.SortExpression = grvDef.sortingCol;
                    }

                    MakeBoundTemplateFieldCtrl( ref tf, grvDef );
                }

                colIdx++;

            }
        }

        /// <summary>
        /// テンプレートフィールド用のバインドフィールド作成
        /// </summary>
        /// <param name="tmpField"></param>
        /// <param name="gridDef"></param>
        //internal static void MakeBoundTemplateFieldCtrl( ref TemplateField tmpField, GridViewDefine gridDef ) {
        internal static void MakeBoundTemplateFieldCtrl( ref TemplateField tmpField, GridViewDefine gridDef ) {

            if ( false == gridDef.bindLabel ) {
                return;
            }

            LabelTemplateField itemTmp = new LabelTemplateField( gridDef );
            tmpField.ItemTemplate = itemTmp;

        }

        /// <summary>
        /// テンプレートフィールド用の復旧
        /// </summary>
        /// <param name="tmpField"></param>
        /// <param name="gridDef"></param>
        internal static void ReMakeBoundTemplateFieldCtrl( GridView grv, GridViewDefine[] gridViewColumnDef ) {

            foreach ( GridViewRow grvRow in grv.Rows ) {
                int colIdx = 0;
                foreach ( GridViewDefine grvDef in gridViewColumnDef ) {

                    if ( grv.Columns[colIdx] is TemplateField ) {
                        if ( true == grvDef.bindLabel ) {
                            LabelTemplateField ltf = new LabelTemplateField( grvDef );
                            ltf.InstantiateIn( grvRow.Cells[colIdx] );
                        }
                    }

                    colIdx++;
                }
            }
        }

        /// <summary>
        /// グリッドビュー定義内のインデックスを取得する
        /// </summary>
        /// <param name="gridviewDefine"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        internal static int GetGridViewDefineIndex( GridViewDefine[] gridviewDefine, GridViewDefine def ) {

            int loopCnt = -1;
            foreach ( GridViewDefine defWK in gridviewDefine ) {
                loopCnt += 1;

                if ( def == defWK ) {
                    return loopCnt;
                }
            }

            return -1;

        }

        /// <summary>
        /// 固定列 まで/以降 の列定義情報を取得
        /// </summary>
        /// <param name="grvLB">グリッドビュー(左下) 列定義情報所持グリッド</param>
        /// <param name="preFrozen">true = 固定列まで / false 固定列以降</param>
        internal static GridViewDefine[] GetFrozenColumns( GridView grvLB, GridViewDefine[] gridviewDefine, bool preFrozen ) {

            //左下GridViewの列数
            int frozenGridColCnt = grvLB.Columns.Count;

            List<GridViewDefine> columnsDef = new List<GridViewDefine>();
            int cnt = 0;
            var colDefines = (
            from x in gridviewDefine
                //右側のグリッドに可変的で項目を設定する場合、非表示項目があると列定義がおかしくなるため、
                //visible条件削除
                //            where x.visible == true
            select x
            ).ToArray();
            foreach ( GridViewDefine def in colDefines ) {

                if ( true == preFrozen && cnt < frozenGridColCnt ) {
                    columnsDef.Add( def );
                } else if ( true == preFrozen && cnt == frozenGridColCnt ) {
                    break;
                } else if ( false == preFrozen && frozenGridColCnt <= cnt ) {
                    columnsDef.Add( def );
                }
                cnt++;
            }

            return columnsDef.ToArray();
        }

        /// <summary>
        /// グリッドビューデータバインド
        /// </summary>
        /// <param name="grv">グリッドビューコントロール</param>
        /// <param name="e">GridViewRowEventArgs</param>
        internal static void GridViewRowBound( GridView grv, GridViewRowEventArgs e ) {
            GridViewRowBound( grv, e, GridRowDoubleClickEvent.None, "" );
        }

        /// <summary>
        /// グリッドビューデータバインド
        /// </summary>
        /// <param name="grv">グリッドビューコントロール</param>
        /// <param name="e">GridViewRowEventArgs</param>
        internal static void GridViewRowBound( GridView grv, GridViewRowEventArgs e, string groupCd ) {
            GridViewRowBound( grv, e, groupCd, GridRowDoubleClickEvent.None );
        }

        /// <summary>
        /// グリッドビューデータバインド
        /// </summary>
        /// <param name="grv">グリッドビューコントロール</param>
        /// <param name="e">GridViewRowEventArgs</param>
        /// <param name="dblClickEvent">ダブルクリック時イベント種別</param>
        /// <param name="parameters">ダブルクリック時 Script処理用パラメータ</param>
        internal static void GridViewRowBound( GridView grv, GridViewRowEventArgs e, GridRowDoubleClickEvent dblClickEvent, params string[] parameters ) {

            if ( e.Row.RowType == DataControlRowType.DataRow ) {

                for ( int index = 0; index < grv.Columns.Count; index++ ) {
                    if ( grv.Columns[index] is BoundField ) {
                        string cellVal = e.Row.Cells[index].Text;
                        e.Row.Cells[index].Text = cellVal.Replace( " ", System.Web.HttpUtility.HtmlDecode( "&nbsp;" ) );
                    }
                }

                SetToolTip( grv, e.Row );

                e.Row.Attributes[ON_CLICK] = string.Format( GRID_VIEW_SELECTED, e.Row.DataItemIndex );

                if ( dblClickEvent != GridRowDoubleClickEvent.None ) {
                    if ( parameters.Length == 2 ) {
                        //パラメータ[0] Url [1] トークン
                        e.Row.Attributes[ON_DBLCLICK] = string.Format( GRID_VIEW_DOUBLE_CLICK, e.Row.DataItemIndex, (int)dblClickEvent, parameters[0], parameters[1] );
                    } else if ( parameters.Length == 3 ) {
                        //パラメータ[0] Url [1] トークン [2] 読み出し元ページID
                        e.Row.Attributes[ON_DBLCLICK] = string.Format( GRID_VIEW_DOUBLE_CLICK_WITH_CALLER, e.Row.DataItemIndex, (int)dblClickEvent, parameters[0], parameters[1], parameters[2] );
                    }
                }
            }
        }

        /// <summary>
        /// グリッドビューデータバインド
        /// </summary>
        /// <param name="grv">グリッドビューコントロール</param>
        /// <param name="e">GridViewRowEventArgs</param>
        /// <param name="groupCd">グリッドビューグループ(分割グリッドのグループ化)</param>
        /// <param name="dblClickEvent">ダブルクリック時イベント種別</param>
        /// <param name="parameters">ダブルクリック時 Script処理用パラメータ</param>
        internal static void GridViewRowBound( GridView grv, GridViewRowEventArgs e, string groupCd, GridRowDoubleClickEvent dblClickEvent, params string[] parameters ) {

            if ( e.Row.RowType == DataControlRowType.DataRow ) {

                for ( int index = 0; index < grv.Columns.Count; index++ ) {
                    if ( grv.Columns[index] is BoundField ) {
                        string cellVal = e.Row.Cells[index].Text;
                        e.Row.Cells[index].Text = cellVal.Replace( " ", System.Web.HttpUtility.HtmlDecode( "&nbsp;" ) );
                    }
                }

                SetToolTip2( grv, e.Row );

                e.Row.Attributes[GROUP_CD] = groupCd;
                e.Row.Attributes[GRID_INDEX] = e.Row.RowIndex.ToString();
                e.Row.Attributes[DATA_INDEX] = e.Row.DataItemIndex.ToString();
                e.Row.Attributes[ON_CLICK] = string.Format( GRID_VIEW_SELECTED_SYNCHRO, e.Row.DataItemIndex );

                if ( dblClickEvent != GridRowDoubleClickEvent.None ) {
                    if ( parameters.Length == 2 ) {
                        //パラメータ[0] Url [1] トークン
                        e.Row.Attributes[ON_DBLCLICK] = string.Format( GRID_VIEW_DOUBLE_CLICK, e.Row.DataItemIndex, (int)dblClickEvent, parameters[0], parameters[1] );
                    } else if ( parameters.Length == 3 ) {
                        //パラメータ[0] Url [1] トークン [2] 読み出し元ページID
                        e.Row.Attributes[ON_DBLCLICK] = string.Format( GRID_VIEW_DOUBLE_CLICK_WITH_CALLER, e.Row.DataItemIndex, (int)dblClickEvent, parameters[0], parameters[1], parameters[2] );
                    }
                }
            }
        }

        /// <summary>
        /// グリッドビューデータバインド（クリックイベント追加版）
        /// </summary>
        /// <param name="grv">グリッドビューコントロール</param>
        /// <param name="e">GridViewRowEventArgs</param>
        /// <param name="groupCd">グリッドビューグループ(分割グリッドのグループ化)</param>
        /// <param name="dblClickEvent">ダブルクリック時イベント種別</param>
        /// <param name="parameters">ダブルクリック時 Script処理用パラメータ</param>
        internal static void GridViewRowBound( GridView grv, GridViewRowEventArgs e, string groupCd, string additionalClickEvent, GridRowDoubleClickEvent dblClickEvent, params string[] parameters ) {

            if ( e.Row.RowType == DataControlRowType.DataRow ) {

                for ( int index = 0; index < grv.Columns.Count; index++ ) {
                    if ( grv.Columns[index] is BoundField ) {
                        string cellVal = e.Row.Cells[index].Text;
                        e.Row.Cells[index].Text = cellVal.Replace( " ", System.Web.HttpUtility.HtmlDecode( "&nbsp;" ) );
                    }
                }

                SetToolTip2( grv, e.Row );

                e.Row.Attributes[GROUP_CD] = groupCd;
                e.Row.Attributes[GRID_INDEX] = e.Row.RowIndex.ToString();
                e.Row.Attributes[DATA_INDEX] = e.Row.DataItemIndex.ToString();
                e.Row.Attributes[ON_CLICK] = string.Format( GRID_VIEW_SELECTED_SYNCHRO, e.Row.DataItemIndex ) + additionalClickEvent;

                if ( dblClickEvent != GridRowDoubleClickEvent.None ) {
                    if ( parameters.Length == 2 ) {
                        //パラメータ[0] Url [1] トークン
                        e.Row.Attributes[ON_DBLCLICK] = string.Format( GRID_VIEW_DOUBLE_CLICK, e.Row.DataItemIndex, (int)dblClickEvent, parameters[0], parameters[1] );
                    } else if ( parameters.Length == 3 ) {
                        //パラメータ[0] Url [1] トークン [2] 読み出し元ページID
                        e.Row.Attributes[ON_DBLCLICK] = string.Format( GRID_VIEW_DOUBLE_CLICK_WITH_CALLER, e.Row.DataItemIndex, (int)dblClickEvent, parameters[0], parameters[1], parameters[2] );
                    }
                }
            }
        }

        // P0113_トレーサビリティシステムリプレース（Java→C#.NET）
        // エンジン立体倉庫在庫検索にて一覧行ダブルクリックでダイアログを表示する機能を実装するために追加
        /// <summary>
        /// グリッドビューデータバインド（ダブルクリックイベント追加版）
        /// </summary>
        /// <param name="grv">グリッドビューコントロール</param>
        /// <param name="e">GridViewRowEventArgs</param>
        /// <param name="groupCd">グリッドビューグループ(分割グリッドのグループ化)</param>
        /// <param name="additionalDblClickEvent">ダブルクリックイベントに追加するスクリプト</param>
        internal static void GridViewRowBound( GridView grv, GridViewRowEventArgs e, string groupCd, string additionalDblClickEvent ) {

            if ( e.Row.RowType == DataControlRowType.DataRow ) {

                for ( int index = 0; index < grv.Columns.Count; index++ ) {
                    if ( grv.Columns[index] is BoundField ) {
                        string cellVal = e.Row.Cells[index].Text;
                        e.Row.Cells[index].Text = cellVal.Replace( " ", System.Web.HttpUtility.HtmlDecode( "&nbsp;" ) );
                    }
                }

                SetToolTip2( grv, e.Row );

                e.Row.Attributes[GROUP_CD] = groupCd;
                e.Row.Attributes[GRID_INDEX] = e.Row.RowIndex.ToString();
                e.Row.Attributes[DATA_INDEX] = e.Row.DataItemIndex.ToString();
                e.Row.Attributes[ON_CLICK] = string.Format( GRID_VIEW_SELECTED_SYNCHRO, e.Row.DataItemIndex );
                e.Row.Attributes[ON_DBLCLICK] = string.Format( GRID_VIEW_SELECTED_SYNCHRO, e.Row.DataItemIndex ) + additionalDblClickEvent;
            }
        }

        /// <summary>
        /// グリッドビューページ切替
        /// </summary>
        /// <param name="grv">グリッドビューコントロール</param>
        /// <param name="e">GridViewRowEventArgs</param>
        internal static void GridViewPageIndexChanging( GridView grv, DataTable tblData, GridViewPageEventArgs e ) {
            grv.PageIndex = e.NewPageIndex;
            grv.DataSource = tblData.DefaultView;
            grv.DataBind();
        }

        /// <summary>
        /// グリッドビューソート
        /// </summary>
        /// <param name="grv">グリッドビューコントロール</param>
        /// <param name="cond">グリッドビューバインド元データ格納構造体</param>
        /// <param name="e">GridViewSortEventArgs</param>
        internal static void GridViewSorting( GridView grv, ref ConditionInfoSessionHandler.ST_CONDITION cond, GridViewSortEventArgs e, bool noRef = false ) {

            DataTable tblData = cond.ResultData;

            if ( e.SortExpression == cond.SortExpression
                && cond.SortDirection == SortDirection.Ascending ) {
                e.SortDirection = SortDirection.Descending;
            }

            string direction = "ASC";
            if ( e.SortDirection == SortDirection.Descending ) {
                direction = "DESC";
            }

            //2014/12/23 SDA MOD ソート速度向上を優先する
            //if ( tblData.Columns[e.SortExpression].DataType == typeof( string ) ) {
            //    tblData = SortDataTableForStringField( tblData, e.SortExpression, e.SortDirection == SortDirection.Ascending ? true : false );
            //} else {
            tblData.DefaultView.Sort = string.Format( "{0} {1}", e.SortExpression, direction );
            //}

            if ( noRef == false ) {
                cond.ResultData = tblData.DefaultView.ToTable();
                cond.SortExpression = e.SortExpression;
                cond.SortDirection = e.SortDirection;
            }

            RenameSortedColumnNm( grv, cond );

            grv.PageIndex = 0;
            grv.DataSource = cond.ResultData.DefaultView;
            grv.DataBind();

        }

        /// <summary>
        /// ソート列名 表示変更
        /// </summary>
        /// <param name="grv">グリッドビューコントロール</param>
        /// <param name="cond">グリッドビューバインド元データ格納構造体</param>
        private static void RenameSortedColumnNm( GridView grv, ConditionInfoSessionHandler.ST_CONDITION cond ) {

            foreach ( DataControlField column in grv.Columns ) {

                column.HeaderText = column.HeaderText.Replace( ALLOW_UP, "" );
                column.HeaderText = column.HeaderText.Replace( ALLOW_DOWN, "" );

                if ( column.SortExpression == cond.SortExpression ) {

                    if ( cond.SortDirection == SortDirection.Ascending ) {
                        column.HeaderText += ALLOW_UP;
                    } else {
                        column.HeaderText += ALLOW_DOWN;
                    }
                }
            }
        }

        /// <summary>
        /// 指定フィールドでの文字列型ソートを実行し実行結果を返す
        /// </summary>
        /// <param name="targetTbl">ソート元データテーブル</param>
        /// <param name="field">ソート対象フィールド</param>
        /// <param name="asc">true = 昇順 / false = 降順</param>
        /// <returns>実行後データテーブル</returns>
        private static DataTable SortDataTableForStringField( DataTable targetTbl, string field, bool asc ) {

            DataTable tblResult = targetTbl.Clone();

            for ( int tgtloop = 0; tgtloop < targetTbl.Rows.Count; tgtloop++ ) {

                bool inserted = false;
                for ( int resloop = 0; resloop < tblResult.Rows.Count; resloop++ ) {

                    string target = targetTbl.Rows[tgtloop][field].ToString();
                    string compare = tblResult.Rows[resloop][field].ToString();
                    StringCompare comp = new StringCompare();
                    int compRes = comp.Compare( compare, target );
                    if ( ( true == asc && 1 == compRes )
                        || ( false == asc && -1 == compRes ) ) {
                        DataRow insRow = tblResult.NewRow();
                        insRow.ItemArray = targetTbl.Rows[tgtloop].ItemArray;
                        tblResult.Rows.InsertAt( insRow, resloop );
                        inserted = true;
                        break;
                    }
                }

                if ( false == inserted ) {
                    DataRow addRow = tblResult.NewRow();
                    addRow.ItemArray = targetTbl.Rows[tgtloop].ItemArray;
                    tblResult.Rows.Add( addRow );
                }
            }

            return tblResult;

        }

        /// <summary>
        /// ツールチップ設定
        /// </summary>
        /// <param name="grv">グリッドビュー</param>
        /// <param name="grvRow">グリッド行</param>
        private static void SetToolTip( GridView grv, GridViewRow grvRow ) {

            for ( int index = 0; index < grv.Columns.Count; index++ ) {
                string toolTipVal = "";
                if ( grv.Columns[index] is BoundField ) {
                    string cellVal = grvRow.Cells[index].Text;
                    grvRow.Cells[index].Text = cellVal.Replace( " ", System.Web.HttpUtility.HtmlDecode( "&nbsp;" ) );
                    toolTipVal = cellVal;
                } else if ( grv.Columns[index] is TemplateField ) {

                    ControlCollection collect = grvRow.Cells[index].Controls;
                    if ( 0 < collect.Count ) {
                        foreach ( Control ctrlChild in collect ) {
                            if ( true == CheckInheritType( ctrlChild.GetType(), typeof( TextBox ) ) ) {
                                TextBox txtBox = ( (TextBox)( ctrlChild ) );
                                if ( true == txtBox.ReadOnly && true == txtBox.Visible ) {
                                    toolTipVal = ControlUtils.GetControlText( ctrlChild, true );
                                }
                                break;
                            }
                        }
                    }
                }

                toolTipVal = toolTipVal.Replace( System.Web.HttpUtility.HtmlDecode( "&nbsp;" ), " " );
                toolTipVal = toolTipVal.Replace( "&nbsp;", " " );
                toolTipVal = toolTipVal.Trim();
                if ( true == StringUtils.IsNotBlank( toolTipVal ) ) {
                    grvRow.Cells[index].ToolTip = System.Web.HttpUtility.HtmlDecode( toolTipVal );
                }
            }
        }
        /// <summary>
        /// ツールチップ設定(ヘッダ固定用)
        /// </summary>
        /// <param name="grv">グリッドビュー</param>
        /// <param name="grvRow">グリッド行</param>
        private static void SetToolTip2( GridView grv, GridViewRow grvRow ) {

            for ( int index = 0; index < grv.Columns.Count; index++ ) {
                string toolTipVal = "";
                if ( grv.Columns[index] is BoundField ) {
                    string cellVal = grvRow.Cells[index].Text;
                    grvRow.Cells[index].Text = cellVal.Replace( " ", System.Web.HttpUtility.HtmlDecode( "&nbsp;" ) );
                    toolTipVal = cellVal;
                } else if ( grv.Columns[index] is TemplateField ) {

                    ControlCollection collect = grvRow.Cells[index].Controls;
                    if ( 0 < collect.Count ) {
                        foreach ( Control ctrlChild in collect ) {
                            if ( true == CheckInheritType( ctrlChild.GetType(), typeof( TextBox ) ) ) {
                                TextBox txtBox = ( (TextBox)( ctrlChild ) );
                                if ( true == txtBox.ReadOnly && true == txtBox.Visible ) {
                                    toolTipVal = ControlUtils.GetControlText( ctrlChild, true );
                                }
                                break;
                            } else if ( ctrlChild.GetType() == typeof( Label ) ) {
                                Label lbl = ( (Label)( ctrlChild ) );
                                string autoLabelAttrVisible = lbl.Attributes[LabelTemplateField.ATTR_VISIBLE];
                                if ( true == lbl.Visible
                                    || true.ToString() == autoLabelAttrVisible ) {
                                    toolTipVal = ControlUtils.GetControlText( ctrlChild, true );
                                }
                                break;
                            }
                        }
                    }
                }

                toolTipVal = toolTipVal.Replace( System.Web.HttpUtility.HtmlDecode( "&nbsp;" ), " " );
                toolTipVal = toolTipVal.Replace( "&nbsp;", " " );
                toolTipVal = toolTipVal.Trim();
                if ( true == StringUtils.IsNotBlank( toolTipVal ) ) {
                    grvRow.Cells[index].ToolTip = System.Web.HttpUtility.HtmlDecode( toolTipVal );
                }
            }
        }


        /// <summary>
        /// ツールチップ設定
        /// </summary>
        /// <param name="lsvItem">リストビューアイテム</param>
        internal static void SetToolTip( ListViewItem lsvItem ) {
            FindAndSetToolTip( lsvItem );
        }

        /// <summary>
        /// ツールチップ設定
        /// </summary>
        /// <param name="ctrl">リストビュー</param>
        internal static void FindAndSetToolTip( Control ctrl ) {
            ControlCollection collect = ctrl.Controls;
            if ( 0 < collect.Count ) {
                foreach ( Control ctrlChild in collect ) {
                    if ( ctrlChild.GetType() == typeof( System.Web.UI.HtmlControls.HtmlTableRow ) ) {
                        SetToolTipForTableRow( (System.Web.UI.HtmlControls.HtmlTableRow)ctrlChild );
                    } else {
                        //ツールチップセット対象のコントロールがあれば、セットする
                        //戻り値 ツールチップセット対象のコントロール有無
                        bool toolTargetFlg = SetToolTipForDirect( ctrlChild );
                        if ( false == toolTargetFlg ) {
                            FindAndSetToolTip( ctrlChild );
                        }
                    }
                }
            }
        }

        /// <summary>
        /// データ行に対するツールチップ設定
        /// </summary>
        /// <param name="tableRow">データ行インスタンス(TR)</param>
        internal static void SetToolTipForTableRow( System.Web.UI.HtmlControls.HtmlTableRow tableRow ) {
            for ( int loopCol = 0; loopCol < tableRow.Cells.Count; loopCol++ ) {
                ControlCollection cellCollection = tableRow.Cells[loopCol].Controls;
                System.Web.UI.HtmlControls.HtmlTableCell tableCell = tableRow.Cells[loopCol];
                foreach ( Control cellCtrl in cellCollection ) {
                    string toolTipVal = "";
                    if ( true == CheckInheritType( cellCtrl.GetType(), typeof( TextBox ) ) ) {
                        TextBox txtBox = ( (TextBox)( cellCtrl ) );
                        if ( true == txtBox.ReadOnly && true == txtBox.Visible ) {
                            toolTipVal = ControlUtils.GetControlText( cellCtrl, true ).Trim();
                        }
                    }

                    if ( true == StringUtils.IsNotBlank( toolTipVal ) ) {
                        string settingTip = tableCell.Attributes["title"];
                        if ( true == StringUtils.IsNotBlank( settingTip )
                            && true == StringUtils.IsNotBlank( toolTipVal ) ) {
                            settingTip += " ";
                        }
                        settingTip += toolTipVal;
                        tableCell.Attributes["title"] = settingTip;
                    }
                }
            }
        }

        /// <summary>
        /// コントロールに対するツールチップ設定
        /// </summary>
        /// <param name="ctrl">コントロール</param>
        /// <returns>ツールチップ設定対象正否 true = 対象コントロール</returns>
        /// <remarks>
        /// テキストボックスを継承 AND 読み取り専用 AND 表示対象
        /// 上記の場合に、テキストボックスの入力値をツールチップに設定
        /// </remarks>
        internal static bool SetToolTipForDirect( Control ctrl ) {
            bool result = false;
            string toolTipVal = ControlUtils.GetControlText( ctrl, true ).Trim();

            if ( true == CheckInheritType( ctrl.GetType(), typeof( TextBox ) ) ) {
                TextBox txtBox = ( (TextBox)( ctrl ) );
                if ( true == txtBox.ReadOnly && true == txtBox.Visible ) {
                    txtBox.ToolTip = toolTipVal;
                }
                result = true;
            }

            return result;
        }
        /// <summary>
        /// グリッドビュー ページャ作成
        /// </summary>
        /// <param name="pnlPager">ページャ用パネル</param>
        /// <param name="grv">グリッドビュー</param>
        /// <param name="eventHandler">イベントハンドラー</param>
        /// <param name="rowCount">データ数</param>
        /// <param name="currentPage">現在ページ 1ページ目=0</param>
        internal static void SetGridViewPager( ref Panel pnlPager, GridView grv, CommandEventHandler eventHandler, int rowCount, int currentPage ) {

            const string FIRST_PAGE = "<<";
            const string PREV_PAGE = "<";
            const string NEXT_PAGE = ">";
            const string LAST_PAGE = ">>";

            const string COMMAND_NAME = "_PageIndexChanging";

            const string CSS_LINK = "btn-link-pager";
            const string CSS_CURRENT = "btn-current-pager";

            int pageSize = grv.PageSize;
            int pageButtonCount = grv.PagerSettings.PageButtonCount;

            ClearPager( ref pnlPager );

            if ( 0 == rowCount ) {
                return;
            }

            int allPages = 0;
            allPages = rowCount / pageSize;
            if ( 0 != rowCount % pageSize ) {
                allPages += 1;
            }

            if ( allPages <= 1 ) {
                return;
            }

            pnlPager.Visible = true;

            int viewPageStart = currentPage / pageButtonCount;
            int viewPageIndex = currentPage % pageButtonCount;

            LinkButton linkFirstPage = new LinkButton();
            linkFirstPage.ID = "linkFirstPage";
            linkFirstPage.Text = FIRST_PAGE;
            linkFirstPage.CommandName = grv.ID + COMMAND_NAME;
            linkFirstPage.CommandArgument = "0";
            linkFirstPage.Command += new CommandEventHandler( eventHandler );
            linkFirstPage.CssClass = CSS_LINK;
            if ( 0 == currentPage ) {
                linkFirstPage.Enabled = false;
            }
            pnlPager.Controls.Add( linkFirstPage );

            LinkButton linkPrevPage = new LinkButton();
            linkPrevPage.ID = "linkPrevPage";
            linkPrevPage.Text = PREV_PAGE;
            linkPrevPage.CommandName = grv.ID + COMMAND_NAME;
            linkPrevPage.CommandArgument = ( currentPage - 1 ).ToString();
            linkPrevPage.Command += new CommandEventHandler( eventHandler );
            linkPrevPage.CssClass = CSS_LINK;
            if ( 0 == currentPage ) {
                linkPrevPage.Enabled = false;
            }
            pnlPager.Controls.Add( linkPrevPage );

            for ( int loopPage = 0; loopPage < pageButtonCount; loopPage++ ) {

                int pageNum = viewPageStart * pageButtonCount + loopPage;
                if ( pageNum >= allPages ) {
                    break;
                }

                if ( pageNum == currentPage ) {
                    Label lblCurrent = new Label();
                    lblCurrent.ID = "lblCurrent";
                    lblCurrent.ForeColor = System.Drawing.Color.Red;
                    lblCurrent.Text = ( pageNum + 1 ).ToString();
                    lblCurrent.CssClass = CSS_CURRENT;
                    pnlPager.Controls.Add( lblCurrent );
                } else {
                    LinkButton linkWK = new LinkButton();
                    linkWK.ID = "link_" + pageNum;
                    linkWK.Text = ( pageNum + 1 ).ToString();
                    linkWK.CommandName = grv.ID + COMMAND_NAME;
                    linkWK.CommandArgument = pageNum.ToString();
                    linkWK.Command += new CommandEventHandler( eventHandler );
                    linkWK.CssClass = CSS_LINK;
                    pnlPager.Controls.Add( linkWK );
                }
            }

            LinkButton linkNextPage = new LinkButton();
            linkNextPage.ID = "linkNextPage";
            linkNextPage.Text = NEXT_PAGE;
            linkNextPage.CommandName = grv.ID + COMMAND_NAME;
            linkNextPage.CommandArgument = ( currentPage + 1 ).ToString();
            linkNextPage.Command += new CommandEventHandler( eventHandler );
            linkNextPage.CssClass = CSS_LINK;
            if ( allPages == currentPage + 1 ) {
                linkNextPage.Enabled = false;
            }
            pnlPager.Controls.Add( linkNextPage );

            LinkButton linkLastPage = new LinkButton();
            linkLastPage.ID = "linkLastPage";
            linkLastPage.Text = LAST_PAGE;
            linkLastPage.CommandName = grv.ID + COMMAND_NAME;
            linkLastPage.CommandArgument = ( allPages - 1 ).ToString();
            linkLastPage.Command += new CommandEventHandler( eventHandler );
            linkLastPage.CssClass = CSS_LINK;
            if ( allPages == currentPage + 1 ) {
                linkLastPage.Enabled = false;
            }
            pnlPager.Controls.Add( linkLastPage );
        }

        /// <summary>
        /// ページャー表示クリア
        /// </summary>
        /// <param name="pnlPager">ページャーパネル</param>
        internal static void ClearPager( ref Panel pnlPager ) {
            pnlPager.Controls.Clear();
            pnlPager.Visible = false;
        }
        #endregion

        #region コントロール設定値制御

        /// <summary>
        /// コントロール入力/選択値設定
        /// </summary>
        /// <param name="ctrl">コントロール</param>
        /// <param name="val">入力値</param>
        internal static void SetControlValue( Control ctrl, object val ) {
            if ( ObjectUtils.IsNotNull( ctrl ) ) {
                Type tp = ctrl.GetType();
                if ( tp == typeof( KTDropDownList ) ) {
                    ( (KTDropDownList)ctrl ).SelectedValue = Convert.ToString( val );
                } else if ( tp == typeof( KTRadioButtonList ) ) {
                    ( (KTRadioButtonList)ctrl ).SelectedValue = Convert.ToString( val );
                    //} else if ( tp == typeof( KTCheckBoxList ) ) {
                    //    ( (KTCheckBoxList)ctrl ).SelectedValue = Convert.ToString(bindval);
                } else if ( tp == typeof( KTDecimalTextBox ) ) {
                    if ( true == ObjectUtils.IsNull( val ) ) {
                        ( (KTDecimalTextBox)ctrl ).Value = null;
                    } else {
                        ( (KTDecimalTextBox)ctrl ).Value = Convert.ToDecimal( val );
                    }
                } else if ( tp == typeof( KTNumericTextBox ) ) {
                    if ( true == ObjectUtils.IsNull( val ) ) {
                        ( (KTNumericTextBox)ctrl ).Value = null;
                    } else {
                        ( (KTNumericTextBox)ctrl ).Value = Convert.ToInt32( val );
                    }
                } else if ( tp == typeof( KTCalendar ) ) {
                    if ( true == ObjectUtils.IsNull( val ) ) {
                        ( (KTCalendar)ctrl ).Value = null;
                    } else {
                        ( (KTCalendar)ctrl ).Value = Convert.ToDateTime( val );
                    }
                } else if ( tp == typeof( KTTextBox ) ) {
                    ( (KTTextBox)ctrl ).Value = Convert.ToString( val );
                    //} else if ( tp == typeof( ListControl ) ) {
                } else if ( Common.ControlUtils.CheckInheritType( tp, typeof( ListControl ) ) ) {
                    string[] valArr = (string[])val;
                    ListControl lc = (ListControl)ctrl;
                    for ( int loop = 0; loop < lc.Items.Count; loop++ ) {
                        ListItem li = lc.Items[loop];
                        li.Selected = false;
                        if ( 0 <= Array.IndexOf( valArr, li.Value ) ) {
                            li.Selected = true;
                        }
                    }
                } else if ( tp == typeof( TextBox ) ) {
                    ( (TextBox)ctrl ).Text = Convert.ToString( val );
                } else if ( tp == typeof( HiddenField ) ) {
                    ( (HiddenField)ctrl ).Value = Convert.ToString( val );
                } else if ( tp == typeof( Label ) ) {
                    ( (Label)ctrl ).Text = Convert.ToString( val );
                }
            }
        }

        /// <summary>
        /// コントロール入力/選択値取得
        /// </summary>
        /// <param name="ctrl">コントロール</param>
        /// <returns>入力/選択値</returns>
        internal static object GetControlValue( Control ctrl ) {

            object resultVal = null;
            if ( ObjectUtils.IsNotNull( ctrl ) ) {
                Type tp = ctrl.GetType();
                if ( tp == typeof( KTDropDownList ) ) {
                    resultVal = ( (KTDropDownList)ctrl ).SelectedValue;
                } else if ( tp == typeof( KTRadioButtonList ) ) {
                    resultVal = ( (KTRadioButtonList)ctrl ).SelectedValue;
                    //} else if ( tp == typeof( KTCheckBoxList ) ) {
                    //    resultVal = ( (KTCheckBoxList)ctrl ).SelectedValue;
                } else if ( tp == typeof( KTDecimalTextBox ) ) {
                    resultVal = ( (KTDecimalTextBox)ctrl ).Value;
                } else if ( tp == typeof( KTNumericTextBox ) ) {
                    resultVal = ( (KTNumericTextBox)ctrl ).Value;
                } else if ( tp == typeof( KTCalendar ) ) {
                    resultVal = ( (KTCalendar)ctrl ).Value;
                } else if ( tp == typeof( KTTextBox ) ) {
                    resultVal = ( (KTTextBox)ctrl ).Value;
                    //} else if ( tp == typeof( ListControl ) ) {
                } else if ( Common.ControlUtils.CheckInheritType( tp, typeof( ListControl ) ) ) {
                    List<string> valArr = new List<string>();
                    foreach ( ListItem li in ( (ListControl)ctrl ).Items ) {
                        if ( false == li.Selected ) {
                            continue;
                        }
                        valArr.Add( li.Value );
                    }
                    resultVal = valArr.ToArray();
                } else if ( tp == typeof( TextBox ) ) {
                    resultVal = ( (TextBox)ctrl ).Text;
                } else if ( tp == typeof( HiddenField ) ) {
                    resultVal = ( (HiddenField)ctrl ).Value;
                } else if ( tp == typeof( System.Web.UI.HtmlControls.HtmlInputCheckBox ) ) {
                    resultVal = ( (System.Web.UI.HtmlControls.HtmlInputCheckBox)ctrl ).Checked;
                } else if ( tp == typeof( KTTimeTextBox ) ) {
                    resultVal = ( (KTTimeTextBox)ctrl ).Value;
                }
            }

            return resultVal;

        }

        /// <summary>
        /// コントロール表示文字列取得
        /// </summary>
        /// <param name="ctrl">コントロール</param>
        /// <returns>表示文字列</returns>
        internal static String GetControlText( Control ctrl, bool viewOnly = false ) {

            String resultTxt = "";
            if ( ObjectUtils.IsNotNull( ctrl ) ) {
                Type tp = ctrl.GetType();
                if ( tp == typeof( KTDropDownList ) ) {
                    if ( true == viewOnly
                        && ( false == ( (KTDropDownList)ctrl ).ReadOnly
                        || false == ( (KTDropDownList)ctrl ).Visible ) ) {
                    } else {
                        resultTxt = ( (KTDropDownList)ctrl ).Text;
                    }
                } else if ( tp == typeof( KTRadioButtonList ) ) {
                    if ( true == viewOnly
                        && ( false == ( (KTRadioButtonList)ctrl ).ReadOnly
                        || false == ( (KTRadioButtonList)ctrl ).Visible ) ) {
                    } else {
                        resultTxt = ( (KTRadioButtonList)ctrl ).SelectedText;
                    }
                    //} else if ( tp == typeof( KTCheckBoxList ) ) {
                    //    resultTxt = ( (KTCheckBoxList)ctrl ).Text;
                } else if ( tp == typeof( KTDecimalTextBox ) ) {
                    if ( true == viewOnly
                        && ( false == ( (KTDecimalTextBox)ctrl ).ReadOnly
                        || false == ( (KTDecimalTextBox)ctrl ).Visible ) ) {
                    } else {
                        resultTxt = ( (KTDecimalTextBox)ctrl ).Text;
                    }
                } else if ( tp == typeof( KTNumericTextBox ) ) {
                    if ( true == viewOnly
                        && ( false == ( (KTNumericTextBox)ctrl ).ReadOnly
                        || false == ( (KTNumericTextBox)ctrl ).Visible ) ) {
                    } else {
                        resultTxt = ( (KTNumericTextBox)ctrl ).Text;
                    }
                } else if ( tp == typeof( KTCalendar ) ) {
                    if ( true == viewOnly
                        && ( false == ( (KTCalendar)ctrl ).ReadOnly
                        || false == ( (KTCalendar)ctrl ).Visible ) ) {
                    } else {
                        resultTxt = ( (KTCalendar)ctrl ).TextPlain;
                    }
                } else if ( tp == typeof( KTTextBox ) ) {
                    if ( true == viewOnly
                        && ( false == ( (KTTextBox)ctrl ).ReadOnly
                        || false == ( (KTTextBox)ctrl ).Visible ) ) {
                    } else {
                        resultTxt = ( (KTTextBox)ctrl ).Text;
                    }
                    //} else if ( tp == typeof( ListControl ) ) {
                } else if ( Common.ControlUtils.CheckInheritType( tp, typeof( ListControl ) ) ) {
                    ListControl lc = (ListControl)ctrl;
                    if ( false == ObjectUtils.IsNotNull( lc.SelectedItem ) ) {

                        if ( true == viewOnly
                        && ( true == ( (ListControl)ctrl ).Enabled
                        || false == ( (ListControl)ctrl ).Visible ) ) {
                        } else {
                            foreach ( ListItem li in ( (ListControl)ctrl ).Items ) {
                                if ( false == li.Selected ) {
                                    continue;
                                }
                                if ( true == StringUtils.IsNotBlank( resultTxt ) ) {
                                    resultTxt = " ";
                                }
                                resultTxt = li.Text;
                            }
                        }
                    }
                } else if ( tp == typeof( TextBox ) ) {
                    if ( true == viewOnly
                        && ( false == ( (TextBox)ctrl ).ReadOnly
                        || false == ( (TextBox)ctrl ).Visible ) ) {
                    } else {
                        resultTxt = ( (TextBox)ctrl ).Text;
                    }
                } else if ( tp == typeof( HiddenField ) ) {
                    if ( false == viewOnly ) {
                        resultTxt = ( (HiddenField)ctrl ).Value;
                    }
                } else if ( tp == typeof( CheckBox ) ) {
                    if ( true == viewOnly
                        && false == ( (CheckBox)ctrl ).Visible ) {
                    } else {
                        //resultTxt = ( (CheckBox)ctrl ).Checked.ToString();
                    }
                } else if ( tp == typeof( Label ) ) {
                    string autoLabelAttrVisible = ( (Label)ctrl ).Attributes[LabelTemplateField.ATTR_VISIBLE];
                    if ( true == viewOnly
                        && ( false == ( (Label)ctrl ).Visible && true.ToString() != autoLabelAttrVisible ) ) {
                    } else {
                        resultTxt = ( (Label)ctrl ).Text;
                    }
                } else if ( tp == typeof( KTTimeTextBox ) ) {
                    if ( false == viewOnly ) {
                        resultTxt = ( ( (KTTimeTextBox)ctrl ).Text != KTTimeTextBox.DEFAULT_TIME_TEXT ) ? ( (KTTimeTextBox)ctrl ).Text : "";
                    }
                }
            }

            return resultTxt;
        }
        #endregion

        #region 汎用メソッド

        /// <summary>
        /// 指定の型が比較型を継承しているかをチェックする
        /// </summary>
        /// <param name="target">比較元</param>
        /// <param name="compare">比較先</param>
        /// <returns>正否</returns>
        static internal bool CheckInheritType( Type target, Type compare ) {

            if ( true == ObjectUtils.IsNull( target )
                || true == ObjectUtils.IsNull( compare ) ) {
                return false;
            }

            if ( target == compare ) {
                return true;
            } else {
                if ( true == ObjectUtils.IsNotNull( target.BaseType ) ) {
                    return CheckInheritType( target.BaseType, compare );
                }
            }

            return false;
        }

        #endregion
    }

    /// <summary>
    /// 文字列型Compareクラス
    /// </summary>
    public class StringCompare : Comparer<string> {

        private readonly System.Globalization.CompareInfo compareInfo;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public StringCompare() {
            compareInfo = System.Globalization.CompareInfo.GetCompareInfo( System.Globalization.CultureInfo.CurrentCulture.Name );
        }

        int result = 0;
        /// <summary>
        /// 比較処理
        /// </summary>
        /// <param name="compare">比較文字列</param>
        /// <param name="target">対象文字列</param>
        /// <returns>-1 = 対象文字列が小さい 0 = 同じ 1 = 対象文字列が大きい</returns>
        public override int Compare( string compare, string target ) {
            result = compareInfo.Compare( compare, target, System.Globalization.CompareOptions.StringSort );
            return result;
        }
    }
}