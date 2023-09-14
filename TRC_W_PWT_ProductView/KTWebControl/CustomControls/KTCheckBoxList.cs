using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using KTFramework.Common;

[assembly: System.Web.UI.TagPrefix( "KTWebControl.CustomControls", "KTCC" )]
namespace KTWebControl.CustomControls {
    [DefaultProperty( "SelectedValue" )]
    [ToolboxData( "<{0}:KTCheckBoxList runat=\"Server\" />" )]
    public class KTCheckBoxList : CheckBoxList {

        #region 拡張プロパティ
        /// <summary>
        /// 読取専用プロパティ
        /// </summary>
        [Category( "表示" )]
        [Description( "読取専用コントロールかどうか取得/設定します。" )]
        public bool ReadOnly {
            get {
                object vs = ViewState["ReadOnly"];
                //this.EnableItems = this.Items;
                return ( null == vs ? false : (bool)vs );
            }
            set {
                ViewState["ReadOnly"] = value;
                if ( true == value ) {
                    this.EnableItems = null;
                } else {
                    this.EnableItems = this.Items;
                }
            }
        }

        /// <summary>
        /// 選択可能項目プロパティ
        /// </summary>
        [Category( "動作" )]
        [Description( "有効なリストアイテムを取得/設定します。" )]
        public ListItemCollection EnableItems {
            get {
                ListItemCollection items = new ListItemCollection();
                foreach ( ListItem item in this.Items ) {
                    if ( true == item.Enabled ) {
                        items.Add( item );
                    }
                }
                return items;
            }
            set {
                foreach ( ListItem item in this.Items ) {
                    item.Enabled = false;
                    if ( null != value ) {
                        foreach ( ListItem enableItem in value ) {
                            if ( item.Value == enableItem.Value ) {
                                item.Enabled = true;
                                break;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 選択可能項目値プロパティ
        /// </summary>
        [Category( "動作" )]
        [Description( "有効なリストアイテムの値を取得/設定します。" )]
        public string[] EnableValues {
            get {
                List<string> values = new List<string>();
                ListItemCollection enableItems = this.EnableItems;
                foreach ( ListItem item in enableItems ) {
                    values.Add( item.Value );
                }
                return values.ToArray();
            }
            set {
                ListItemCollection enableItems = new ListItemCollection();
                if ( null != value ) {
                    foreach ( string enableValue in value ) {
                        foreach ( ListItem item in this.Items ) {
                            if ( enableValue == item.Value ) {
                                enableItems.Add( item );
                            }
                        }
                    }
                }
                this.EnableItems = enableItems;
            }
        }

        /// <summary>
        /// 選択項目プロパティ
        /// </summary>
        [Category( "動作" )]
        [Description( "選択されたリストアイテムを取得/設定します。" )]
        public ListItemCollection SelectedItems {
            get {
                ListItemCollection selectedItems = new ListItemCollection();
                foreach ( ListItem item in this.Items ) {
                    if ( true == item.Selected ) {
                        selectedItems.Add( item );
                    }
                }
                return selectedItems;
            }
            set {
                foreach ( ListItem item in this.Items ) {
                    item.Selected = false;
                    if ( null != value ) {
                        foreach ( ListItem selectedItem in value ) {
                            if ( item.Value == selectedItem.Value ) {
                                item.Selected = true;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 選択値プロパティ
        /// </summary>
        [Category( "動作" )]
        [Description( "選択されたリストアイテムの値を取得/設定します。" )]
        public string[] SelectedValues {
            get {
                List<string> values = new List<string>();
                ListItemCollection selectedItems = this.SelectedItems;
                foreach ( ListItem item in selectedItems ) {
                    values.Add( item.Value );
                }
                return values.ToArray();
            }
            set {
                ListItemCollection selectedItems = new ListItemCollection();
                if ( null != value ) {
                    foreach ( string selectedValue in value ) {
                        foreach ( ListItem item in this.Items ) {
                            if ( selectedValue == item.Value ) {
                                selectedItems.Add( item );
                            }
                        }
                    }
                }
                this.SelectedItems = selectedItems;
            }
        }
        #endregion

        #region オーバーライドプロパティ
        /// <summary>
        /// リストアイテムプロパティ
        /// </summary>
        [Category( "表示" )]
        [Description( "リストアイテムを取得/設定します。" )]
        public override ListItemCollection Items {
            get {
                return base.Items;
            }
        }
        #endregion

        #region オーバーライドメソッド
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public KTCheckBoxList() {

        }

        /// <summary>
        /// レンダリング
        /// </summary>
        /// <param name="writer"></param>
        protected override void Render( HtmlTextWriter writer ) {
            base.Render( writer );
        }
        #endregion
    }
}
