using KTFramework.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TRC_W_PWT_ProductView.Common;
using TRC_W_PWT_ProductView.Defines.ListDefine;
using TRC_W_PWT_ProductView.Defines.ProcessViewDefine;

namespace TRC_W_PWT_ProductView.UI.Pages.UserControl {
    /// <summary>
    /// 工程検索 個別検索条件
    /// </summary>
    public partial class MainProcessPartialView : System.Web.UI.UserControl {

        //ロガー定義
        private static readonly Logger logger = new Logger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

        /// <summary>
        /// 噴射時期計測03 検索条件セット
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void divEngineInjection03Condition_Init( object sender, EventArgs e ) {
            this.engineInjection03_cldStart.Value = DateTime.Now;
            this.engineInjection03_cldEnd.Value = DateTime.Now;
            ControlUtils.SetListControlItems( this.engineInjection03_ddlResultKind, ResultKind.EngineInjection03.GetList() );
            ControlUtils.SetListControlItems( this.engineInjection03_ddlMeasurementTerminalKind, IndividualDropDownItem.EngineInjection03.MeasurementTerminal.GetList() );
        }

        /// <summary>
        /// 噴射時期計測07 検索条件セット
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void divEngineInjection07Condition_Init( object sender, EventArgs e ) {
            this.engineInjection07_cldStart.Value = DateTime.Now;
            this.engineInjection07_cldEnd.Value = DateTime.Now;
            ControlUtils.SetListControlItems( this.engineInjection07_ddlResultKind, ResultKind.Inspection.GetList() );
        }

        /// <summary>
        /// フリクションロス 検索条件セット
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void divEngineFrictionCondition_Init( object sender, EventArgs e ) {
            this.engineFriction_cldStart.Value = DateTime.Now;
            this.engineFriction_cldEnd.Value = DateTime.Now;
            ControlUtils.SetListControlItems( this.engineFriction_ddlResultKind, ResultKind.Inspection.GetList() );
        }

        /// <summary>
        /// エンジン運転測定03 検索条件セット
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void divEngineTest03Condition_Init( object sender, EventArgs e ) {
            this.engineTest03_cldStart.Value = DateTime.Now;
            this.engineTest03_cldEnd.Value = DateTime.Now;
            ControlUtils.SetListControlItems( this.engineTest03_ddlResultKind, ResultKind.EngineTest.GetList() );
        }

        /// <summary>
        /// エンジン運転測定07 検索条件セット
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void divEngineTest07Condition_Init( object sender, EventArgs e ) {
            this.engineTest07_cldStart.Value = DateTime.Now;
            this.engineTest07_cldEnd.Value = DateTime.Now;
            ControlUtils.SetListControlItems( this.engineTest07_ddlResultKind, ResultKind.EngineTest.GetList() );
        }
    }
}