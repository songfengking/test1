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
    [ToolboxData( "<{0}:KTRadioButtonList runat=\"Server\" />" )]
    public class KTRadioButtonList : RadioButtonList {

        #region 拡張プロパティ
        /// <summary>
        /// 読取専用プロパティ
        /// </summary>
        [Category( "表示" )]
        [Description( "読取専用コントロールかどうか取得/設定します。" )]
        public bool ReadOnly {
            get {
                object vs = ViewState["ReadOnly"];
                return ( null == vs ? false : (bool)vs );
            }
            set {
                ViewState["ReadOnly"] = value;
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
        /// <summary>
        /// 値を取得/選択します。
        /// </summary>
        public override string SelectedValue {
            get {
                return base.SelectedValue;
            }
            set {
                if ( true == ObjectUtils.IsNull( Items.FindByValue( value ) ) ) {
                    base.SelectedIndex = -1;
                    return;
                }
                base.SelectedValue = value;
            }
        }

        /// <summary>
        /// 選択中のDisplayTextを取得します。
        /// </summary>
        public string SelectedText {
            get {
                string text = "";
                if ( ObjectUtils.IsNotNull( this.SelectedItem ) ) {
                    text = this.SelectedItem.Text;
                }
                return text;
            }
        }
                                
        #endregion

        #region オーバーライドメソッド
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public KTRadioButtonList() {
        }

        /// <summary>
        /// レンダリング
        /// </summary>
        /// <param name="writer"></param>
        protected override void Render( HtmlTextWriter writer ) {
            if ( !this.ReadOnly ) {
                base.Render( writer );
            } else {

                String toolTip = "";
                if ( true == StringUtils.IsNotBlank( ToolTip ) ) {
                    toolTip = String.Format( "title=\"{0}\"", ToolTip );
                }

                string labelTag = "<span class=\"readonly {0}\" style=\"{1}\" {2}>{3}</span>";

                string width = 0 < this.Width.Value ? string.Format( "width:{0}px;", (int)this.Width.Value ) : "";
                string labelStyle = string.Format( "display:inline-block;{0}", width );

                writer.Write( labelTag, this.CssClass, labelStyle, toolTip, SelectedText );
            }
        }
        #endregion
    }
}
