using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web.UI.Design;
using System.Web.UI.WebControls;

namespace KTWebControl {
    public class KTExpanderDesigners : ContainerControlDesigner {
        private Style _style = null;
        public override string FrameCaption {
            get {
                return "下記の領域に折りたたみ表示するコンテンツを作成する";
            }
        }
        public override Style FrameStyle {
            get {
                if ( _style == null ) {
                    _style = new Style();
                    _style.Font.Name = "Verdana";
                    _style.Font.Size = new FontUnit( "XSmall" );
                    _style.BackColor = Color.LightBlue;
                    _style.ForeColor = Color.Black;
                }

                return _style;
            }
        }
    }
}
