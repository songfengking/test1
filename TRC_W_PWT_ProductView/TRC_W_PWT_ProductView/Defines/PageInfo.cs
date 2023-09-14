using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using KTFramework.Common;
using TRC_W_PWT_ProductView.Common;
using TRC_W_PWT_ProductView.Defines.ListDefine;

namespace TRC_W_PWT_ProductView.Defines {

    /// <summary>
    /// ページ定義情報
    /// </summary>
    public static class PageInfo {

        #region メイン画面

        /// <summary>画面定義情報：トップ画面</summary>
        public static readonly ST_PAGE_INFO Top = new ST_PAGE_INFO( "Top", "トップ画面", "~/UI/Pages/Top.aspx", typeof( UI.Pages.Top ) );

        /// <summary>画面定義情報：ログイン画面</summary>
        //public static readonly ST_PAGE_INFO LoginByID = new ST_PAGE_INFO( "LoginByID", "ログイン画面", "~/UI/Pages/LoginByID.aspx", typeof( UI.Pages.LoginByID ) );

        /// <summary>画面定義情報：AD認証画面</summary>
        //public static readonly ST_PAGE_INFO LoginByAD = new ST_PAGE_INFO( "LoginByAD", "AD認証画面", "~/UI/Pages/LoginByAD.aspx", typeof( UI.Pages.LoginByAD ) );

        /// <summary>画面定義情報：認証キー受信画面</summary>
        public static readonly ST_PAGE_INFO LoginByTrc = new ST_PAGE_INFO( "LoginByTrc", "認証キー受信画面", "~/UI/Pages/LoginByTrc.aspx", typeof( UI.Pages.LoginByTrc ) );

        /// <summary>画面定義情報：パスワード変更画面</summary>
        public static readonly ST_PAGE_INFO ChangePassword = new ST_PAGE_INFO( "ChangePassword", "パスワード変更画面", "~/UI/Pages/ChangePassword.aspx", typeof( UI.Pages.ChangePassword ) );

        /// <summary>画面定義情報：検索画面</summary>
        public static readonly ST_PAGE_INFO MainView = new ST_PAGE_INFO( "MainView", "製品検索画面", "~/UI/Pages/MainView.aspx", typeof( UI.Pages.MainView ) );

        /// <summary>画面定義情報：検索画面(部品)</summary>
        public static readonly ST_PAGE_INFO MainPartsView = new ST_PAGE_INFO( "MainPartsView", "部品検索画面", "~/UI/Pages/MainPartsView.aspx", typeof( UI.Pages.MainPartsView ) );

        /// <summary>画面定義情報：検索画面(工程)</summary>
        public static readonly ST_PAGE_INFO MainProcessView = new ST_PAGE_INFO( "MainProcessView", "工程検索画面", "~/UI/Pages/MainProcessView.aspx", typeof( UI.Pages.MainProcessView ) );

        /// <summary>画面定義情報：詳細画面</summary>
        public static readonly ST_PAGE_INFO DetailFrame = new ST_PAGE_INFO( "DetailFrame", "詳細画面", "~/UI/Pages/DetailFrame.aspx", typeof( UI.Pages.DetailFrame ) );

        /// <summary>画面定義情報：詳細画面(部品)</summary>
        public static readonly ST_PAGE_INFO DetailPartsFrame = new ST_PAGE_INFO( "DetailPartsFrame", "詳細画面", "~/UI/Pages/DetailPartsFrame.aspx", typeof( UI.Pages.DetailPartsFrame ) );

        /// <summary>画面定義情報：イメージ画面</summary>
        public static readonly ST_PAGE_INFO ImageView = new ST_PAGE_INFO( "ImageView", "画像表示画面", "~/UI/Pages/ImageView.aspx", typeof( UI.Pages.ImageView ) );

        /// <summary>画面定義情報：エラー画面</summary>
        public static readonly ST_PAGE_INFO Error = new ST_PAGE_INFO( "Error", "エラー画面", "~/UI/Pages/Error.aspx", typeof( UI.Pages.Error ) );

        /// <summary>画面定義情報：メニュー画面</summary>
        public static readonly ST_PAGE_INFO MaintenanceMenu = new ST_PAGE_INFO( "MaintenanceMenu", "メニュー画面", "~/UI/Pages/MaintenanceMenu.aspx", typeof( UI.Pages.MaintenanceMenu ) );

        /// <summary>画面定義情報：セッション維持画面(マスターページ内IFRAME)</summary>
        public static readonly ST_PAGE_INFO SessionKeep = new ST_PAGE_INFO( "SessionKeep", "セッション維持画面", "~/UI/Pages/SessionKeep.aspx", typeof( UI.Pages.SessionKeep ) );

        /// <summary>画面定義情報：アプリケーション死活チェック画面</summary>
        public static readonly ST_PAGE_INFO ApCheck = new ST_PAGE_INFO( "ApCheck", "アプリケーション死活チェック画面", "~/UI/Pages/ApCheck.aspx", typeof( UI.Pages.ApCheck ) );

        /// <summary>画面定義情報：ブラウザ変更促し画面</summary>
        public static readonly ST_PAGE_INFO BrowserChangeGuidance = new ST_PAGE_INFO( "BrowserChangeGuidance", "", "~/UI/Pages/BrowserChangeGuidance.aspx", typeof( UI.Pages.BrowserChangeGuidance ) );

        #endregion

        #region 製品詳細画面

        #region 工程画面
        /// <summary>詳細定義情報：工程汎用</summary>
        public static readonly ST_PAGE_INFO GenericDetail = new ST_PAGE_INFO( "GenericDetail", "", "~/UI/Pages/ProcessView/GenericDetail.ascx", typeof( UI.Pages.ProcessView.GenericDetail ) );
        /// <summary>トラクタ詳細定義情報：パワクロ走行検査</summary>
        public static readonly ST_PAGE_INFO PCrawler = new ST_PAGE_INFO( "PCrawler", "パワクロ走行検査", "~/UI/Pages/ProcessView/PCrawler.ascx", typeof( UI.Pages.ProcessView.PCrawler ) );
        /// <summary>トラクタ詳細定義情報：完成検査チェックシート</summary>
        public static readonly ST_PAGE_INFO CheckSheet = new ST_PAGE_INFO( "CheckSheet", "完成検査チェックシート", "~/UI/Pages/ProcessView/CheckSheet.ascx", typeof( UI.Pages.ProcessView.CheckSheet ) );
        /// <summary>トラクタ詳細定義情報：電子チェックシート</summary>
        public static readonly ST_PAGE_INFO ELCheckSheet = new ST_PAGE_INFO( "ELCheckSheet", "電子チェックシート", "~/UI/Pages/ProcessView/ELCheckSheet.ascx", typeof( UI.Pages.ProcessView.ELCheckSheet ) );
        /// <summary>トラクタ詳細定義情報：関所画面</summary>
        public static readonly ST_PAGE_INFO CheckPoint = new ST_PAGE_INFO( "CheckPoint", "関所", "~/UI/Pages/ProcessView/CheckPoint.ascx", typeof( UI.Pages.ProcessView.CheckPoint ) );
        /// <summary>トラクタ詳細定義情報：イメージチェックシート</summary>
        public static readonly ST_PAGE_INFO ImgCheckSheet = new ST_PAGE_INFO( "EngineELCheckSheet", "イメージチェックシート", "~/UI/Pages/ProcessView/EngineELCheckSheet.ascx", typeof( UI.Pages.ProcessView.EngineELCheckSheet ) );
        /// <summary>トラクタ詳細定義情報：検査ベンチト</summary>
        public static readonly ST_PAGE_INFO TestBench = new ST_PAGE_INFO( "TestBench", "検査ベンチ", "~/UI/Pages/ProcessView/TestBench.ascx", typeof( UI.Pages.ProcessView.TestBench ) );
        /// <summary>エンジン/トラクタ共通詳細定義情報：品質画像証跡画面</summary>
        public static readonly ST_PAGE_INFO CameraImage = new ST_PAGE_INFO( "CameraImage", "品質画像証跡", "~/UI/Pages/ProcessView/CameraImage.ascx", typeof( UI.Pages.ProcessView.CameraImage ) );
        /// <summary>エンジン/トラクタ共通詳細定義情報：刻印画面</summary>
        public static readonly ST_PAGE_INFO Sheel = new ST_PAGE_INFO( "Sheel", "刻印", "~/UI/Pages/ProcessView/Sheel.ascx", typeof( UI.Pages.ProcessView.Sheel ) );
        /// <summary>エンジン/トラクタ共通詳細定義情報：ナットランナー画面</summary>
        public static readonly ST_PAGE_INFO NutRunner = new ST_PAGE_INFO( "NutRunner", "ナットランナー", "~/UI/Pages/ProcessView/NutRunner.ascx", typeof( UI.Pages.ProcessView.NutRunner ) );
        /// <summary>エンジン/トラクタ共通詳細定義情報：トラクタ走行検査画面</summary>
        public static readonly ST_PAGE_INFO TractorAll = new ST_PAGE_INFO( "TractorAll", "トラクタ走行検査", "~/UI/Pages/ProcessView/TractorAll.ascx", typeof( UI.Pages.ProcessView.NutRunner ) );
        /// <summary>エンジン/トラクタ共通詳細定義情報：光軸検査画面</summary>
        public static readonly ST_PAGE_INFO Optaxis = new ST_PAGE_INFO( "Optaxis", "光軸検査", "~/UI/Pages/ProcessView/Optaxis.ascx", typeof( UI.Pages.ProcessView.NutRunner ) );
        /// <summary>エンジン/トラクタ共通詳細定義情報：AI画像解析画面</summary>
        public static readonly ST_PAGE_INFO AiImage = new ST_PAGE_INFO( "AiImage", "AI画像解析", "~/UI/Pages/ProcessView/AiImage.ascx", typeof( UI.Pages.ProcessView.AiImage ) );

        /// <summary>エンジン詳細定義情報：シリンダヘッド精密測定データ</summary>
        public static readonly ST_PAGE_INFO CriticalInspectionCYH = new ST_PAGE_INFO( "CriticalInspectionCYH", "シリンダヘッド精密測定データ", "~/UI/Pages/ProcessView/CriticalInspection.ascx", typeof( UI.Pages.ProcessView.CriticalInspection ) );
        /// <summary>エンジン詳細定義情報：クランクシャフト精密測定データ</summary>
        public static readonly ST_PAGE_INFO CriticalInspectionCS = new ST_PAGE_INFO( "CriticalInspectionCS", "クランクシャフト精密測定データ", "~/UI/Pages/ProcessView/CriticalInspection.ascx", typeof( UI.Pages.ProcessView.CriticalInspection ) );
        /// <summary>エンジン詳細定義情報：クランクケース精密測定データ</summary>
        public static readonly ST_PAGE_INFO CriticalInspectionCC = new ST_PAGE_INFO( "CriticalInspectionCC", "クランクケース精密測定データ", "~/UI/Pages/ProcessView/CriticalInspection.ascx", typeof( UI.Pages.ProcessView.CriticalInspection ) );

        /// <summary>エンジン詳細定義情報：エンジン運転測定(03エンジン)</summary>
        public static readonly ST_PAGE_INFO EngineTest03 = new ST_PAGE_INFO( "EngineTest03", "エンジン運転測定", "~/UI/Pages/ProcessView/EngineTest03.ascx", typeof( UI.Pages.ProcessView.EngineTest03 ) );
        /// <summary>エンジン詳細定義情報：エンジン運転測定(07エンジン)</summary>
        public static readonly ST_PAGE_INFO EngineTest07 = new ST_PAGE_INFO( "EngineTest07", "エンジン運転測定", "~/UI/Pages/ProcessView/EngineTest07.ascx", typeof( UI.Pages.ProcessView.EngineTest07 ) );

        /// <summary>エンジン詳細定義情報：フリクションロス</summary>
        public static readonly ST_PAGE_INFO FrictionLoss = new ST_PAGE_INFO( "FrictionLoss", "フリクションロス", "~/UI/Pages/ProcessView/FrictionLoss.ascx", typeof( UI.Pages.ProcessView.FrictionLoss ) );
        /// <summary>エンジン詳細定義情報：ハーネス検査</summary>
        public static readonly ST_PAGE_INFO Harnes = new ST_PAGE_INFO( "Harnes", "ハーネス検査", "~/UI/Pages/ProcessView/Harnes.ascx", typeof( UI.Pages.ProcessView.Harnes ) );
        /// <summary>エンジン詳細定義情報：噴射時期計測(03エンジン)</summary>
        public static readonly ST_PAGE_INFO FuelInjection03 = new ST_PAGE_INFO( "FuelInjection03", "噴射時期計測", "~/UI/Pages/ProcessView/FuelInjection03.ascx", typeof( UI.Pages.ProcessView.FuelInjection03 ) );
        /// <summary>エンジン詳細定義情報：噴射時期計測(07エンジン)</summary>
        public static readonly ST_PAGE_INFO FuelInjection07 = new ST_PAGE_INFO( "FuelInjection07", "噴射時期計測", "~/UI/Pages/ProcessView/FuelInjection07.ascx", typeof( UI.Pages.ProcessView.FuelInjection07 ) );
        /// <summary>エンジン詳細定義情報：トルク締付</summary>
        public static readonly ST_PAGE_INFO Torque = new ST_PAGE_INFO( "Torque", "トルク締付", "~/UI/Pages/ProcessView/Torque.ascx", typeof( UI.Pages.ProcessView.Torque ) );
        /// <summary>エンジン詳細定義情報：電子チェックシート</summary>
        public static readonly ST_PAGE_INFO EngineELCheckSheet = new ST_PAGE_INFO( "EngineELCheckSheet", "電子チェックシート", "~/UI/Pages/ProcessView/EngineELCheckSheet.ascx", typeof( UI.Pages.ProcessView.EngineELCheckSheet ) );
        /// <summary>エンジン詳細定義情報：出荷部品</summary>
        public static readonly ST_PAGE_INFO ShipmentParts = new ST_PAGE_INFO( "ShipmentParts", "出荷部品", "~/UI/Pages/ProcessView/ShipmentParts.ascx", typeof( UI.Pages.ProcessView.ShipmentParts ) );

        #endregion

        #region 部品画面
        /// <summary>トラクタ詳細定義情報：WiFi-ECU</summary>
        public static readonly ST_PAGE_INFO WiFiEcu = new ST_PAGE_INFO( "WiFiEcu", "WiFi-ECU", "~/UI/Pages/PartsView/WiFiEcu.ascx", typeof( UI.Pages.PartsView.WiFiEcu ) );
        /// <summary>トラクタ詳細定義情報：ロプス</summary>
        public static readonly ST_PAGE_INFO Rops = new ST_PAGE_INFO( "Rops", "ロプス", "~/UI/Pages/PartsView/Rops.ascx", typeof( UI.Pages.PartsView.Rops ) );
        /// <summary>トラクタ詳細定義情報：クレート</summary>
        public static readonly ST_PAGE_INFO Crate = new ST_PAGE_INFO( "Crate", "クレート", "~/UI/Pages/PartsView/Crate.ascx", typeof( UI.Pages.PartsView.Crate ) );
        /// <summary>トラクタ詳細定義情報：銘板ラベル</summary>
        public static readonly ST_PAGE_INFO Nameplate = new ST_PAGE_INFO( "Nameplate", "銘板ラベル", "~/UI/Pages/PartsView/Nameplate.ascx", typeof( UI.Pages.PartsView.Nameplate ) );
        /// <summary>トラクタ詳細定義情報：ミッション</summary>
        public static readonly ST_PAGE_INFO Mission = new ST_PAGE_INFO( "Mission", "ミッション", "~/UI/Pages/PartsView/Mission.ascx", typeof( UI.Pages.PartsView.Mission ) );
        /// <summary>トラクタ詳細定義情報：ハウジング</summary>
        public static readonly ST_PAGE_INFO Housing = new ST_PAGE_INFO( "Housing", "ハウジング", "~/UI/Pages/PartsView/Housing.ascx", typeof( UI.Pages.PartsView.Housing ) );
        /// <summary>トラクタ詳細定義情報：基幹部品</summary>
        public static readonly ST_PAGE_INFO CoreParts = new ST_PAGE_INFO( "CoreParts", "基幹部品", "~/UI/Pages/PartsView/CoreParts.ascx", typeof( UI.Pages.PartsView.CoreParts ) );

        /// <summary>エンジン詳細定義情報：クランクケース</summary>
        public static readonly ST_PAGE_INFO CrankCase = new ST_PAGE_INFO( "CrankCase", "クランクケース", "~/UI/Pages/PartsView/CriticalParts.ascx", typeof( UI.Pages.PartsView.CriticalParts ) );
        /// <summary>エンジン詳細定義情報：シリンダヘッド</summary>
        public static readonly ST_PAGE_INFO CylinderHead = new ST_PAGE_INFO( "CylinderHead", "シリンダヘッド", "~/UI/Pages/PartsView/CriticalParts.ascx", typeof( UI.Pages.PartsView.CriticalParts ) );
        /// <summary>エンジン詳細定義情報：クランクシャフト</summary>
        public static readonly ST_PAGE_INFO CrankShaft = new ST_PAGE_INFO( "CrankShaft", "クランクシャフト", "~/UI/Pages/PartsView/CriticalParts.ascx", typeof( UI.Pages.PartsView.CriticalParts ) );
        /// <summary>エンジン詳細定義情報：DPF</summary>
        public static readonly ST_PAGE_INFO Dpf = new ST_PAGE_INFO( "Dpf", "DPF", "~/UI/Pages/PartsView/Dpf.ascx", typeof( UI.Pages.PartsView.Dpf ) );
        /// <summary>エンジン詳細定義情報：ECU</summary>
        public static readonly ST_PAGE_INFO Ecu = new ST_PAGE_INFO( "Ecu", "ECU", "~/UI/Pages/PartsView/Ecu.ascx", typeof( UI.Pages.PartsView.Ecu ) );
        /// <summary>エンジン詳細定義情報：EPR</summary>
        public static readonly ST_PAGE_INFO Epr = new ST_PAGE_INFO( "Epr", "EPR", "~/UI/Pages/PartsView/Epr.ascx", typeof( UI.Pages.PartsView.Epr ) );
        /// <summary>エンジン詳細定義情報：インジェクタ</summary>
        public static readonly ST_PAGE_INFO Injecter = new ST_PAGE_INFO( "Injecter", "インジェクタ", "~/UI/Pages/PartsView/Injecter.ascx", typeof( UI.Pages.PartsView.Injecter ) );
        /// <summary>エンジン詳細定義情報：MIXER</summary>
        public static readonly ST_PAGE_INFO Mixer = new ST_PAGE_INFO( "Mixer", "MIXER", "~/UI/Pages/PartsView/Mixer.ascx", typeof( UI.Pages.PartsView.Mixer ) );
        /// <summary>エンジン詳細定義情報：サプライポンプ</summary>
        public static readonly ST_PAGE_INFO SupplyPump = new ST_PAGE_INFO( "SupplyPump", "サプライポンプ", "~/UI/Pages/PartsView/SupplyPump.ascx", typeof( UI.Pages.PartsView.SupplyPump ) );
        /// <summary>エンジン詳細定義情報：IPU</summary>
        public static readonly ST_PAGE_INFO Ipu = new ST_PAGE_INFO( "SupplyPump", "IPU", "~/UI/Pages/PartsView/SupplyPump.ascx", typeof( UI.Pages.PartsView.SupplyPump ) );
        /// <summary>エンジン詳細定義情報：EHC</summary>
        public static readonly ST_PAGE_INFO Ehc = new ST_PAGE_INFO( "SupplyPump", "EHC", "~/UI/Pages/PartsView/SupplyPump.ascx", typeof( UI.Pages.PartsView.SupplyPump ) );
        /// <summary>エンジン詳細定義情報：SCR</summary>
        public static readonly ST_PAGE_INFO Scr = new ST_PAGE_INFO( "Scr", "SCR", "~/UI/Pages/PartsView/Scr.ascx", typeof( UI.Pages.PartsView.Scr ) );
        /// <summary>エンジン詳細定義情報：DOC</summary>
        public static readonly ST_PAGE_INFO Doc = new ST_PAGE_INFO( "Doc", "DOC", "~/UI/Pages/PartsView/Doc.ascx", typeof( UI.Pages.PartsView.Doc ) );
        /// <summary>エンジン詳細定義情報：Acu</summary>
        public static readonly ST_PAGE_INFO Acu = new ST_PAGE_INFO( "Acu", "ACU", "~/UI/Pages/PartsView/Acu.ascx", typeof( UI.Pages.PartsView.Acu ) );
        /// <summary>エンジン詳細定義情報：機番ラベル印刷</summary>
        public static readonly ST_PAGE_INFO SerialPrint = new ST_PAGE_INFO( "SerialPrint", "機番ラベル印刷", "~/UI/Pages/PartsView/SerialPrint.ascx", typeof( UI.Pages.PartsView.SerialPrint ) );
        /// <summary>エンジン詳細定義情報：ラック位置センサ</summary>
        public static readonly ST_PAGE_INFO RackPositionSensor = new ST_PAGE_INFO( "RackPositionSensor", "ラック位置センサ", "~/UI/Pages/PartsView/RackPositionSensor.ascx", typeof( UI.Pages.PartsView.RackPositionSensor ) );

        #endregion

        #endregion

        #region 部品詳細画面

        #region 工程画面

        /// <summary>部品詳細定義情報：ATU機番管理</summary>
        public static readonly ST_PAGE_INFO AtuPartsSerial = new ST_PAGE_INFO( "AtuPartsSerial", "ATU機番管理", "~/UI/Pages/PartsSearch/AtuPartsSerial.ascx", typeof( UI.Pages.PartsSearch.AtuPartsSerial ) );
        /// <summary>部品詳細定義情報：トルク締付</summary>
        public static readonly ST_PAGE_INFO TorqueTightenRecord = new ST_PAGE_INFO( "TorqueTightenRecord", "トルク締付", "~/UI/Pages/PartsSearch/TorqueTightenRecord.ascx", typeof( UI.Pages.PartsSearch.TorqueTightenRecord ) );
        /// <summary>部品詳細定義情報：リーク計測</summary>
        public static readonly ST_PAGE_INFO LeakMeasureResult = new ST_PAGE_INFO( "LeakMeasureResult", "リーク計測", "~/UI/Pages/PartsSearch/LeakMeasureResult.ascx", typeof( UI.Pages.PartsSearch.LeakMeasureResult ) );

        #endregion

        #endregion

        #region メンテナンス画面
        /// <summary>画面定義情報：重要チェック対象外リスト</summary>
        public static readonly ST_PAGE_INFO MasterMainteNAList = new ST_PAGE_INFO( "MasterMainteNAList", "重要チェック対象外リスト検索画面", "~/UI/Pages/Maintenance/MasterMainteNAList.aspx", typeof( UI.Pages.MasterMainteNAList ) );
        /// <summary>画面定義情報：重要チェック対象外リスト詳細画面</summary>
        public static readonly ST_PAGE_INFO MaintenanceNAListDetail = new ST_PAGE_INFO( "MaintenanceNAListDetail", "重要チェック対象外リスト詳細画面", "~/UI/Pages/Maintenance/MaintenanceNAListDetail.aspx", typeof( UI.Pages.MaintenanceNAListDetail ), PageType.Maintenance );
        /// <summary>画面定義情報：重要チェック対象外リスト入力画面</summary>
        public static readonly ST_PAGE_INFO NACheckList = new ST_PAGE_INFO( "NACheckList", "重要チェック対象外リスト入力画面", "~/UI/Pages/Maintenance/NACheckList.aspx", typeof( UI.Pages.Maintenance.NACheckList ), PageType.Maintenance );
        /// <summary>画面定義情報：3C加工日データ修正</summary>
        public static readonly ST_PAGE_INFO MasterMainte3CList = new ST_PAGE_INFO( "MasterMainte3CList", "3C加工日データ修正", "~/UI/Pages/Maintenance/MasterMainte3CList.aspx", typeof( UI.Pages.MasterMainte3CList ) );
        /// <summary>画面定義情報：加工日修正</summary>
        public static readonly ST_PAGE_INFO ProcessingDtEdit = new ST_PAGE_INFO( "ProcessingDtEdit", "加工日修正画面", "~/UI/Pages/Maintenance/ProcessingDtEdit.aspx", typeof( UI.Pages.Maintenance.ProcessingDtEdit ), PageType.Maintenance );
        /// <summary>画面定義情報：排ガス規制部品 検索画面</summary>
        public static readonly ST_PAGE_INFO MasterMainteDpfList = new ST_PAGE_INFO( "MasterMainteDpfList", "排ガス規制部品 検索画面", "~/UI/Pages/Maintenance/MasterMainteDpfList.aspx", typeof( UI.Pages.MasterMainteDpfList ) );
        /// <summary>画面定義情報：排ガス規制部品 詳細画面</summary>
        public static readonly ST_PAGE_INFO MaintenanceDpfListDetail = new ST_PAGE_INFO( "MaintenanceDpfListDetail", "排ガス規制部品 詳細画面", "~/UI/Pages/Maintenance/MaintenanceDpfListDetail.aspx", typeof( UI.Pages.MaintenanceDpfListDetail ), PageType.Maintenance );
        /// <summary>画面定義情報：DPF機番情報メンテ</summary>
        public static readonly ST_PAGE_INFO DpfSerial = new ST_PAGE_INFO( "DpfSerial", "規制部品 修正画面", "~/UI/Pages/Maintenance/DpfSerial.aspx", typeof( UI.Pages.Maintenance.DpfSerial ), PageType.Maintenance );

        #endregion

        #region 印刷画面
        /// <summary>画面定義情報：印刷画面 メイン</summary>
        public static readonly ST_PAGE_INFO PreViewForm = new ST_PAGE_INFO( "PreViewForm", "印刷画面", "~/UI/Pages/PreViewForm.aspx", typeof( UI.Pages.PreViewForm ) );
        /// <summary>画面定義情報：印刷画面 チェックシート</summary>
        public static readonly ST_PAGE_INFO PrintCheckSheet = new ST_PAGE_INFO( "PrintCheckSheet", "チェックシート印刷画面画面", "~/UI/Pages/ProcessView/PrintCheckSheet.ascx", typeof( UI.Pages.PrintCheckSheet ) );

        #endregion

        #region バッチ起動
        /// バッチ起動画面
        public static readonly ST_PAGE_INFO BatchExecCmdInput = new ST_PAGE_INFO( "BatchExecCmdInput", "製品トレース", "~/UI/Pages/BatchExecCmdInput.aspx", typeof( UI.Pages.BatchExecCmdInput ) );
        /// <summary>画面定義情報：製品トレース画面</summary>
        public static readonly ST_PAGE_INFO BatchPartsTrace = new ST_PAGE_INFO( "BatchPartsTrace", "製品トレース", "~/UI/Pages/BatchExecCmdInput.aspx", typeof( UI.Pages.BatchExecCmdInput ) );

        #endregion

        #region 電子かんばん
        /// <summary>
        /// 画面定義情報：ピッキング状況
        /// </summary>
        public static readonly ST_PAGE_INFO KanbanPickingStatusView = new ST_PAGE_INFO( "KanbanPickingStatusView", "ピッキング状況", "~/UI/Pages/Kanban/KanbanPickingStatusView.aspx", typeof( UI.Pages.Kanban.KanbanPickingStatusView ) );
        /// <summary>
        /// 画面定義情報：未完了ピッキング
        /// </summary>
        public static readonly ST_PAGE_INFO KanbanShortageView = new ST_PAGE_INFO( "KanbanShortageView", "未完了ピッキング", "~/UI/Pages/Kanban/KanbanShortageView.aspx", typeof( UI.Pages.Kanban.KanbanShortageView ) );
        /// <summary>
        /// 画面定義情報：ピッキング明細
        /// </summary>
        public static readonly ST_PAGE_INFO KanbanPickingDetailView = new ST_PAGE_INFO( "KanbanPickingDetailView", "ピッキング明細", "~/UI/Pages/Kanban/KanbanPickingDetailView.aspx", typeof( UI.Pages.Kanban.KanbanPickingDetailView ) );
        /// <summary>
        /// 画面定義情報：ピッキング者選択
        /// </summary>
        public static readonly ST_PAGE_INFO KanbanPickingUserSelect = new ST_PAGE_INFO( "KanbanPickingUserSelect", "ピッキング者選択", "~/UI/Pages/Kanban/KanbanPickingUserSelect.aspx", typeof( UI.Pages.Kanban.KanbanPickingUserSelect ) );
        #endregion

        // P0113_トレーサビリティシステムリプレース（Java→C#.NET）
        // C#版画面の追加に伴い定義を追加
        #region 順序検索
        /// 画面定義情報：製品別通過実績検索
        /// </summary>
        public static readonly ST_PAGE_INFO SearchProductOrder = new ST_PAGE_INFO( "SearchProductOrder", "製品別通過実績検索", "~/UI/Pages/Order/SearchProductOrder.aspx", typeof( UI.Pages.Order.SearchProductOrder ) );
        /// <summary>
        /// 画面定義情報：ステーション別通過実績検索
        /// </summary>
        public static readonly ST_PAGE_INFO SearchStationOrder = new ST_PAGE_INFO( "SearchStationOrder", "ステーション別通過実績検索", "~/UI/Pages/Order/SearchStationOrder.aspx", typeof( UI.Pages.Order.SearchStationOrder ) );
        /// <summary>
        /// 画面定義情報：順序情報検索
        /// </summary>
        public static readonly ST_PAGE_INFO SearchOrderInfo = new ST_PAGE_INFO( "SearchOrderInfo", "順序情報検索", "~/UI/Pages/Order/SearchOrderInfo.aspx", typeof( UI.Pages.Order.SearchOrderInfo ) );
        /// <summary>
        /// 画面定義情報：エンジン立体倉庫在庫検索
        /// </summary>
        public static readonly ST_PAGE_INFO SearchEngineStock = new ST_PAGE_INFO( "SearchEngineStock", "エンジン立体倉庫在庫検索", "~/UI/Pages/Order/SearchEngineStock.aspx", typeof( UI.Pages.Order.SearchEngineStock ) );
        /// <summary>
        /// 画面定義情報：搭載エンジン引当検索
        /// </summary>
        public static readonly ST_PAGE_INFO SearchCallEngineSimulation = new ST_PAGE_INFO( "SearchCallEngineSimulation", "搭載エンジン引当検索", "~/UI/Pages/Order/SearchCallEngineSimulation.aspx", typeof( UI.Pages.Order.SearchCallEngineSimulation ) );
        /// <summary>
        /// 画面定義情報：型式IDNOリストダイアログ
        /// </summary>
        public static readonly ST_PAGE_INFO ProcodeIdnoListDialog = new ST_PAGE_INFO( "ProCodeIdnoDialog", "型式IDNOリストダイアログ", "~/UI/Pages/Order/ProCodeIdnoDialog.aspx", typeof( UI.Pages.Order.ProCodeIdnoDialog ) );
        #endregion

        #region AI外観検査
        /// <summary>画面定義情報：是正処置入力製品検索画面sfadd</summary>
        public static readonly ST_PAGE_INFO MainCorrectiveView = new ST_PAGE_INFO( "MainCorrectiveView", "是正処置入力製品検索画面", "~/UI/Pages/AiImageCheck/MainCorrectiveView.aspx", typeof( UI.Pages.AiImageCheck.MainCorrectiveView ) );
        /// <summary>画面定義情報：是正処置入力検査結果一覧画面sfadd</summary>
        public static readonly ST_PAGE_INFO MainCorrectiveList = new ST_PAGE_INFO( "MainCorrectiveList", "是正処置入力検査結果一覧画面", "~/UI/Pages/AiImageCheck/MainCorrectiveList.aspx", typeof( UI.Pages.AiImageCheck.MainCorrectiveList ), PageType.Maintenance );
        /// <summary>画面定義情報：是正処置入力画面sfadd</summary>
        public static readonly ST_PAGE_INFO MainCorrectiveInput = new ST_PAGE_INFO( "MainCorrectiveInput", "是正処置入力画面", "~/UI/Pages/AiImageCheck/MainCorrectiveInput.aspx", typeof( UI.Pages.AiImageCheck.MainCorrectiveInput ), PageType.Maintenance );
        /// <summary>画面定義情報：画像解析グループ検索画面　sfadd</summary>
        public static readonly ST_PAGE_INFO AnlGroupView = new ST_PAGE_INFO( "AnlGroupView", "画像解析グループ検索画面", "~/UI/Pages/AiImageCheck/AnlGroupView.aspx", typeof( UI.Pages.AiImageCheck.AnlGroupView ) );
        /// <summary>画面定義情報：画像解析グループ項目構成一覧画面　sfadd</summary>
        public static readonly ST_PAGE_INFO AnlGroupList = new ST_PAGE_INFO( "AnlGroupList", "画像解析グループ項目構成一覧画面", "~/UI/Pages/AiImageCheck/AnlGroupList.aspx", typeof( UI.Pages.AiImageCheck.AnlGroupList ), PageType.Maintenance );
        /// <summary>画面定義情報：画像解析グループ項目構成追加画面　sfadd</summary>
        public static readonly ST_PAGE_INFO AnlGroupListInput = new ST_PAGE_INFO( "AnlGroupListInput", "画像解析グループ項目構成追加画面", "~/UI/Pages/AiImageCheck/AnlGroupListInput.aspx", typeof( UI.Pages.AiImageCheck.AnlGroupListInput ) );
        /// <summary>画面定義情報：画像解析グループ項目入力画面　sfadd</summary>
        public static readonly ST_PAGE_INFO AnlGroupInput = new ST_PAGE_INFO( "AnlGroupInput", "画像解析グループ項目入力画面", "~/UI/Pages/AiImageCheck/AnlGroupInput.aspx", typeof( UI.Pages.AiImageCheck.AnlGroupInput ) );
        /// <summary>画面定義情報：型式紐づけグループ検索画面　sfadd</summary>
        public static readonly ST_PAGE_INFO AnlGroupModelView = new ST_PAGE_INFO( "AnlGroupModelView", "型式紐づけグループ検索画面", "~/UI/Pages/AiImageCheck/AnlGroupModelView.aspx", typeof( UI.Pages.AiImageCheck.AnlGroupModelView ) );
        /// <summary>画面定義情報：型式紐づけ一覧画面　sfadd</summary>
        public static readonly ST_PAGE_INFO AnlGroupModelList = new ST_PAGE_INFO( "AnlGroupModelList", "型式紐づけ一覧画面", "~/UI/Pages/AiImageCheck/AnlGroupModelList.aspx", typeof( UI.Pages.AiImageCheck.AnlGroupModelList ), PageType.Maintenance );
        /// <summary>画面定義情報：型式紐づけ一覧追加画面　sfadd</summary>
        public static readonly ST_PAGE_INFO AnlGroupModelInput = new ST_PAGE_INFO( "AnlGroupModelInput", "型式紐づけ一覧追加画面", "~/UI/Pages/AiImageCheck/AnlGroupModelInput.aspx", typeof( UI.Pages.AiImageCheck.AnlGroupModelInput ), PageType.Maintenance );
        /// <summary>画面定義情報：検査項目マスタ検索画面</summary>
        public static readonly ST_PAGE_INFO AnlItemView = new ST_PAGE_INFO( "AnlItemView", "検査項目マスタ検索画面", "~/UI/Pages/AiImageCheck/AnlItemView.aspx", typeof( UI.Pages.AiImageCheck.AnlItemView ) );
        /// <summary>画面定義情報：検査項目マスタ入力画面</summary>
        public static readonly ST_PAGE_INFO AnlItemInput = new ST_PAGE_INFO( "AnlItemInput", "検査項目マスタ入力画面", "~/UI/Pages/AiImageCheck/AnlItemInput.aspx", typeof( UI.Pages.AiImageCheck.AnlItemInput ), PageType.Maintenance );

        #endregion

        /// <summary>
        /// ページ種別
        /// </summary>
        public enum PageType : int {
            MainView = 0,
            DetailFrame = 1,
            Maintenance = 2,
            Error = 9

        }

        /// <summary>
        /// クライアント用URLの取得
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static string ResolveClientUrl( Control ctrl, ST_PAGE_INFO info ) {
            return ctrl.ResolveClientUrl( info.url );
        }

        /// <summary>
        /// 詳細画面ユーザコントロール定義取得
        /// </summary>
        /// <param name="productKindCd">製品種別</param>
        /// <param name="groupCd">グループコード</param>
        /// <param name="classCd">クラスコード</param>
        /// <param name="assemblyPatternCd">組立パターンコード</param>
        /// <param name="lineCd">ラインコード</param>
        /// <param name="processCd">工程コード</param>
        /// <returns>画面情報定義</returns>
        public static ST_PAGE_INFO GetUCPageInfo( string productKindCd, string groupCd, string classCd, string assemblyPatternCd = "", string lineCd = "", string processCd = "" ) {

            ST_PAGE_INFO resultInfo = new ST_PAGE_INFO();

            //製品種別 トラクタ
            if ( productKindCd == ListDefine.ProductKind.Tractor ) {
                //工程区分
                if ( ListDefine.GroupCd.Process == groupCd ) {
                    switch ( classCd ) {
                    case ListDefine.ProcessKind.PROCESS_CD_TRACTOR_PCRAWER:      //パワクロ走行検査
                        resultInfo = PCrawler;
                        break;
                    case ListDefine.ProcessKind.PROCESS_CD_TRACTOR_CHKSHEET:     //チェックシート
                        resultInfo = CheckSheet;
                        break;
                    case ListDefine.ProcessKind.PROCESS_CD_TRACTOR_CAMIMAGE:     //品質画像証跡
                        resultInfo = CameraImage;
                        break;
                    case ListDefine.ProcessKind.PROCESS_CD_TRACTOR_ELCHECK:      //電子チェックシート
                        resultInfo = ELCheckSheet;
                        break;
                    case ListDefine.ProcessKind.PROCESS_CD_TRACTOR_SHEEL:          //刻印
                        resultInfo = Sheel;
                        break;
                    case ListDefine.ProcessKind.PROCESS_CD_TRACTOR_NUTRUNNER:    //ナットランナー
                        resultInfo = NutRunner;
                        break;
                    case ListDefine.ProcessKind.PROCESS_CD_TRACTOR_ALL:    //トラクタ走行検査
                        resultInfo = TractorAll;
                        break;
                    case ListDefine.ProcessKind.PROCESS_CD_TRACTOR_OPTAXIS:    //光軸検査
                        resultInfo = Optaxis;
                        break;
                    case ListDefine.ProcessKind.PROCESS_CD_TRACTOR_CHKPOINT:    //関所
                        resultInfo = CheckPoint;
                        break;
                    case ListDefine.ProcessKind.PROCESS_CD_TRACTOR_IMGCHECK:    //イメージチェックシート
                        resultInfo = ImgCheckSheet;
                        break;
                    case ListDefine.ProcessKind.PROCESS_CD_TRACTOR_TESTBENCH:    // 検査ベンチ
                        resultInfo = TestBench;
                        break;
                    case ListDefine.ProcessKind.PROCESS_CD_TRACTOR_AIIMAGE:      // AI画像解析
                        resultInfo = AiImage;
                        break;
                    case ListDefine.ProcessKind.PROCESS_CD_COMMON_PROCESS:       // 工程情報が汎用テーブルで管理されている場合
                        resultInfo = GenericDetail;
                        break;
                    default:
                        break;
                    }
                    //部品区分
                } else if ( ListDefine.GroupCd.Parts == groupCd ) {
                    switch ( classCd ) {
                    case ListDefine.PartsKind.PARTS_CD_TRACTOR_WECU:             //WiFi ECU
                        resultInfo = WiFiEcu;
                        break;
                    case ListDefine.PartsKind.PARTS_CD_TRACTOR_CRATE:            //クレート
                        resultInfo = Crate;
                        break;
                    case ListDefine.PartsKind.PARTS_CD_TRACTOR_ROPS:             //ロプス
                        resultInfo = Rops;
                        break;
                    case ListDefine.PartsKind.PARTS_CD_TRACTOR_NAMEPLATE:        //銘板ラベル
                        resultInfo = Nameplate;
                        break;
                    case ListDefine.PartsKind.PARTS_CD_TRACTOR_MISSION:          //ミッション
                        resultInfo = Mission;
                        break;
                    case ListDefine.PartsKind.PARTS_CD_TRACTOR_HOUSING:          //ハウジング
                        resultInfo = Housing;
                        break;
                    //トラクタ選択でもエンジン部品を表示する対応
                    case ListDefine.PartsKind.PARTS_CD_ENGINE_DPF:               //DPF
                        resultInfo = Dpf;
                        break;
                    case ListDefine.PartsKind.PARTS_CD_ENGINE_SCR:               //SCR
                        resultInfo = Scr;
                        break;
                    case ListDefine.PartsKind.PARTS_CD_ENGINE_ACU:               //ACU
                        resultInfo = Acu;
                        break;
                    default:
                        if ( true == classCd.StartsWith( PartsKind.PARTS_CD_TRACTOR_PREFIX_COREPARTS ) ) {
                            resultInfo = CoreParts;                             //基幹部品
                        }
                        break;
                    }
                }
                //製品種別 エンジン
            } else if ( productKindCd == ListDefine.ProductKind.Engine ) {
                //工程区分
                if ( ListDefine.GroupCd.Process == groupCd ) {
                    switch ( classCd ) {
                    case ListDefine.ProcessKind.PROCESS_CD_ENGINE_TORQUE:        //トルク締付
                        resultInfo = Torque;
                        break;
                    case ListDefine.ProcessKind.PROCESS_CD_ENGINE_HARNESS:       //ハーネス検査
                        resultInfo = Harnes;
                        break;
                    case ListDefine.ProcessKind.PROCESS_CD_ENGINE_TEST:          //エンジン運転測定
                        //03、07エンジンによって画面分岐
                        if ( assemblyPatternCd == Defines.AssemblyPatternCode.InstalledEngine03
                            || assemblyPatternCd == Defines.AssemblyPatternCode.OemEngine03 ) {
                            resultInfo = EngineTest03;

                        } else if ( assemblyPatternCd == Defines.AssemblyPatternCode.InstalledEngine07
                            || assemblyPatternCd == Defines.AssemblyPatternCode.OemEngine07 ) {
                            resultInfo = EngineTest07;
                        }
                        break;
                    case ListDefine.ProcessKind.PROCESS_CD_ENGINE_FRICTION:      //フリクションロス
                        resultInfo = FrictionLoss;
                        break;
                    //case ListDefine.ProcessKind.PROCESS_CD_ENGINE_CYH_ASSEMBLY:  //シリンダヘッド組付
                    //    break;
                    case ListDefine.ProcessKind.PROCESS_CD_ENGINE_CYH_INSPECT:   //シリンダヘッド精密測定
                        resultInfo = CriticalInspectionCYH;
                        break;
                    case ListDefine.ProcessKind.PROCESS_CD_ENGINE_CS_INSPECT:    //クランクシャフト精密測定
                        resultInfo = CriticalInspectionCS;
                        break;
                    case ListDefine.ProcessKind.PROCESS_CD_ENGINE_CC_INSPECT:    //クランクケース精密測定
                        resultInfo = CriticalInspectionCC;
                        break;
                    case ListDefine.ProcessKind.PROCESS_CD_ENGINE_INJECTION:     //噴射時期計測

                        //03、07エンジンによって画面分岐
                        if ( assemblyPatternCd == Defines.AssemblyPatternCode.InstalledEngine03
                            || assemblyPatternCd == Defines.AssemblyPatternCode.OemEngine03 ) {
                            resultInfo = FuelInjection03;
                        } else if ( assemblyPatternCd == Defines.AssemblyPatternCode.InstalledEngine07
                            || assemblyPatternCd == Defines.AssemblyPatternCode.OemEngine07 ) {
                            resultInfo = FuelInjection07;
                        }
                        break;
                    case ListDefine.ProcessKind.PROCESS_CD_ENGINE_CAMIMAGE:      //品質画像証跡
                        resultInfo = CameraImage;
                        break;
                    case ProcessKind.PROCESS_CD_ENGINE_ELCHECK:                 //電子チェックシート
                        resultInfo = EngineELCheckSheet;
                        break;
                    case ProcessKind.PROCESS_CD_ENGINE_SHIPMENTPARTS:            //出荷部品
                        resultInfo = ShipmentParts;
                        break;
                    case ListDefine.ProcessKind.PROCESS_CD_ENGINE_AIIMAGE:       // AI画像解析
                        resultInfo = AiImage;
                        break;
                    case ListDefine.ProcessKind.PROCESS_CD_COMMON_PROCESS:       // 工程情報が汎用テーブルで管理されている場合
                        resultInfo = GenericDetail;
                        break;
                    default:
                        break;
                    }
                    //部品区分
                } else if ( ListDefine.GroupCd.Parts == groupCd ) {
                    switch ( classCd ) {
                    case ListDefine.PartsKind.PARTS_CD_ENGINE_SUPPLYPUMP:        //サプライポンプ
                        resultInfo = SupplyPump;
                        break;
                    //case ListDefine.PartsKind.PARTS_CD_ENGINE_RAIL:            //コモンレール
                    //    break;
                    case ListDefine.PartsKind.PARTS_CD_ENGINE_ECU:               //ECU
                        resultInfo = Ecu;
                        break;
                    case ListDefine.PartsKind.PARTS_CD_ENGINE_INJECTOR:          //インジェクタ
                        resultInfo = Injecter;
                        break;
                    case ListDefine.PartsKind.PARTS_CD_ENGINE_DPF:               //DPF
                        resultInfo = Dpf;
                        break;
                    case ListDefine.PartsKind.PARTS_CD_ENGINE_CC:                //クランクケース
                        resultInfo = CrankCase;
                        break;
                    case ListDefine.PartsKind.PARTS_CD_ENGINE_EPR:               //EPR
                        resultInfo = Epr;
                        break;
                    case ListDefine.PartsKind.PARTS_CD_ENGINE_MIXER:             //MIXER
                        resultInfo = Mixer;
                        break;
                    case ListDefine.PartsKind.PARTS_CD_ENGINE_CYH:               //シリンダヘッド
                        resultInfo = CylinderHead;
                        break;
                    case ListDefine.PartsKind.PARTS_CD_ENGINE_CS:                //クランクシャフト
                        resultInfo = CrankShaft;
                        break;
                    case ListDefine.PartsKind.PARTS_CD_ENGINE_SCR:               //SCR
                        resultInfo = Scr;
                        break;
                    case ListDefine.PartsKind.PARTS_CD_ENGINE_DOC:               //DOC
                        resultInfo = Doc;
                        break;
                    case ListDefine.PartsKind.PARTS_CD_ENGINE_ACU:               //ACU
                        resultInfo = Acu;
                        break;
                    case ListDefine.PartsKind.PARTS_CD_ENGINE_SERIALPRINT:       //SERIALPRINT
                        resultInfo = SerialPrint;
                        break;
                    case ListDefine.PartsKind.PARTS_CD_ENGINE_RACK_POSITION_SENSOR: //ラック位置センサ
                        resultInfo = RackPositionSensor;
                        break;
                    case ListDefine.PartsKind.PARTS_CD_ENGINE_IPU:               //IPU
                        resultInfo = Ipu;
                        break;
                    case ListDefine.PartsKind.PARTS_CD_ENGINE_EHC:               //EHC
                        resultInfo = Ehc;
                        break;
                    default:
                        if ( true == classCd.StartsWith( PartsKind.PARTS_CD_TRACTOR_PREFIX_COREPARTS ) ) {
                            resultInfo = CoreParts;                             //基幹部品
                        }
                        break;
                    }
                }
            }

            return resultInfo;
        }



        /// <summary>
        /// 詳細画面ユーザコントロール定義取得(部品検索用)
        /// </summary>
        /// <param name="searchTarget">検索対象</param>
        /// <param name="partsKind">部品種別</param>
        /// <param name="processCd">工程区分</param>
        /// <returns>画面情報定義</returns>
        public static ST_PAGE_INFO GetUCPageInfo( string searchTarget, string partsKind, string processCd ) {

            ST_PAGE_INFO resultInfo = new ST_PAGE_INFO();

            //部品種別 ATU
            if ( searchTarget == ListDefine.PartsSearchTarget.Atu ) {
                //工程区分
                if ( processCd == ListDefine.AtuProcessKind.ATU_PARTS_SERIAL ) {
                    resultInfo = AtuPartsSerial;
                } else if ( processCd == ListDefine.AtuProcessKind.ATU_TORQUE_TIGHTENING_RECORD ) {
                    resultInfo = TorqueTightenRecord;
                } else if ( processCd == ListDefine.AtuProcessKind.ATU_LEAK_MEASURE_RESULT ) {
                    resultInfo = LeakMeasureResult;
                }
            }

            return resultInfo;
        }




        /// <summary>
        /// 定義されているページ情報かチェックする
        /// </summary>
        /// <param name="page">ページインスタンス</param>
        /// <returns>正否</returns>
        public static bool CheckDefinePage( HttpRequest request ) {

            bool result = false;
            string url = "";
            if ( true == ObjectUtils.IsNotNull( request ) ) {
                url = request.AppRelativeCurrentExecutionFilePath;

                object[] pageInfos = ControlUtils.GetStaticDefineArray( typeof( PageInfo ), typeof( ST_PAGE_INFO ) );
                foreach ( ST_PAGE_INFO objInfo in pageInfos ) {
                    ST_PAGE_INFO pageInfo = (ST_PAGE_INFO)objInfo;
                    if ( pageInfo.url == url ) {
                        if ( true == ControlUtils.CheckInheritType( pageInfo.type, typeof( UI.Base.BaseForm ) ) ) {
                            result = true;
                        }
                        break;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// ページ情報構造体
        /// </summary>
        public struct ST_PAGE_INFO {
            /// <summary>
            /// ページ識別ID
            /// </summary>
            public string pageId;
            /// <summary>
            /// タイトル
            /// </summary>
            public string title;
            /// <summary>
            /// URL
            /// </summary>
            public string url;
            /// <summary>
            /// クラスType
            /// </summary>
            public Type type;

            /// <summary>
            /// クラスType
            /// </summary>
            public PageType pagetype;

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="pageId">ページ識別ID</param>
            /// <param name="title">タイトル</param>
            /// <param name="url">URL</param>
            /// <remarks>クラスTypeをBaseFormで設定します</remarks>
            public ST_PAGE_INFO( string pageId, string title, string url ) {
                this.pageId = pageId;
                this.title = title;
                this.url = url;
                this.type = typeof( UI.Base.BaseForm );
                this.pagetype = PageInfo.PageType.MainView;
            }
            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="pageId">ページ識別ID</param>
            /// <param name="title">タイトル</param>
            /// <param name="url">URL</param>
            /// <param name="type">クラスType</param>
            public ST_PAGE_INFO( string pageId, string title, string url, Type type ) {
                this.pageId = pageId;
                this.title = title;
                this.url = url;
                this.type = type;
                this.pagetype = PageInfo.PageType.MainView;
            }
            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="pageId">ページ識別ID</param>
            /// <param name="title">タイトル</param>
            /// <param name="url">URL</param>
            /// <param name="type">クラスType</param>
            /// <param name="pagetype">ページ種別</param>
            public ST_PAGE_INFO( string pageId, string title, string url, Type type, PageType pagetype ) {
                this.pageId = pageId;
                this.title = title;
                this.url = url;
                this.type = type;
                this.pagetype = pagetype;
            }
        }

        public static void GetPageCdInfo( string pageID, out string groupCd, out string classCd ) {
            groupCd = "";
            classCd = "";

            //CASEにするとconstを作成ないといけないため、IF文で対応

            if ( StringUtils.IsEmpty( pageID ) ) {
                //処理なしで終了

            } else if ( pageID.Equals( PageInfo.PCrawler.pageId ) ) {
                //パワクロ
                groupCd = GroupCd.Process;
                classCd = ProcessKind.PROCESS_CD_TRACTOR_PCRAWER;
            } else if ( pageID.Equals( PageInfo.CheckSheet.pageId ) ) {
                //完成検査チェックシート
                groupCd = GroupCd.Process;
                classCd = ProcessKind.PROCESS_CD_TRACTOR_CHKSHEET;
            } else if ( pageID.Equals( PageInfo.ELCheckSheet.pageId ) ) {
                //電子チェックシート
                groupCd = GroupCd.Process;
                classCd = ProcessKind.PROCESS_CD_TRACTOR_ELCHECK;
            } else if ( pageID.Equals( PageInfo.CameraImage.pageId ) ) {
                //品質画像証跡画面
                groupCd = GroupCd.Process;
                classCd = ProcessKind.PROCESS_CD_TRACTOR_CAMIMAGE;
            } else if ( pageID.Equals( PageInfo.Sheel.pageId ) ) {
                //刻印
                groupCd = GroupCd.Process;
                classCd = ProcessKind.PROCESS_CD_TRACTOR_SHEEL;
            } else if ( pageID.Equals( PageInfo.NutRunner.pageId ) ) {
                //ナットランナー
                groupCd = GroupCd.Process;
                classCd = ProcessKind.PROCESS_CD_TRACTOR_NUTRUNNER;
            } else if ( pageID.Equals( PageInfo.TractorAll.pageId ) ) {
                //トラクタ走行検査
                groupCd = GroupCd.Process;
                classCd = ProcessKind.PROCESS_CD_TRACTOR_ALL;
            } else if ( pageID.Equals( PageInfo.Optaxis.pageId ) ) {
                //光軸検査
                groupCd = GroupCd.Process;
                classCd = ProcessKind.PROCESS_CD_TRACTOR_OPTAXIS;
            } else if ( pageID.Equals( PageInfo.CriticalInspectionCYH.pageId ) ) {
                //シリンダヘッド精密測定データ
                groupCd = GroupCd.Process;
                classCd = ProcessKind.PROCESS_CD_ENGINE_CYH_INSPECT;
            } else if ( pageID.Equals( PageInfo.CriticalInspectionCS.pageId ) ) {
                //クランクシャフト精密測定データ
                groupCd = GroupCd.Process;
                classCd = ProcessKind.PROCESS_CD_ENGINE_CS_INSPECT;
            } else if ( pageID.Equals( PageInfo.CriticalInspectionCC.pageId ) ) {
                //クランクケース精密測定データ
                groupCd = GroupCd.Process;
                classCd = ProcessKind.PROCESS_CD_ENGINE_CC_INSPECT;
            } else if ( pageID.Equals( PageInfo.EngineTest03.pageId ) ) {
                //エンジン運転測定(03エンジン)
                groupCd = GroupCd.Process;
                classCd = ProcessKind.PROCESS_CD_ENGINE_TEST;
            } else if ( pageID.Equals( PageInfo.EngineTest07.pageId ) ) {
                //エンジン運転測定(07エンジン)
                groupCd = GroupCd.Process;
                classCd = ProcessKind.PROCESS_CD_ENGINE_TEST;
            } else if ( pageID.Equals( PageInfo.FrictionLoss.pageId ) ) {
                //フリクションロス
                groupCd = GroupCd.Process;
                classCd = ProcessKind.PROCESS_CD_ENGINE_FRICTION;
            } else if ( pageID.Equals( PageInfo.Harnes.pageId ) ) {
                //ハーネス検査
                groupCd = GroupCd.Process;
                classCd = ProcessKind.PROCESS_CD_ENGINE_HARNESS;
            } else if ( pageID.Equals( PageInfo.FuelInjection03.pageId ) ) {
                //噴射時期計測(03エンジン)
                groupCd = GroupCd.Process;
                classCd = ProcessKind.PROCESS_CD_ENGINE_INJECTION;
            } else if ( pageID.Equals( PageInfo.FuelInjection07.pageId ) ) {
                //噴射時期計測(07エンジン)
                groupCd = GroupCd.Process;
                classCd = ProcessKind.PROCESS_CD_ENGINE_INJECTION;
            } else if ( pageID.Equals( PageInfo.Torque.pageId ) ) {
                //トルク締付
                groupCd = GroupCd.Process;
                classCd = ProcessKind.PROCESS_CD_ENGINE_TORQUE;
            } else if ( pageID.Equals( PageInfo.EngineELCheckSheet.pageId ) ) {
                //電子チェックシート
                groupCd = GroupCd.Process;
                classCd = ProcessKind.PROCESS_CD_ENGINE_ELCHECK;
            } else if ( pageID.Equals( PageInfo.CheckPoint.pageId ) ) {
                //関所
                groupCd = GroupCd.Process;
                classCd = ProcessKind.PROCESS_CD_TRACTOR_CHKPOINT;
            } else if ( pageID.Equals( PageInfo.ImgCheckSheet.pageId ) ) {
                //イメージチェックシート
                groupCd = GroupCd.Process;
                classCd = ProcessKind.PROCESS_CD_TRACTOR_IMGCHECK;
            } else if ( pageID.Equals( PageInfo.TestBench.pageId ) ) {
                // 検査ベンチ
                groupCd = GroupCd.Process;
                classCd = ProcessKind.PROCESS_CD_TRACTOR_TESTBENCH;
            } else if ( pageID.Equals( PageInfo.AiImage.pageId ) ) {
                // AI画像解析
                groupCd = GroupCd.Process;
                classCd = ProcessKind.PROCESS_CD_TRACTOR_AIIMAGE;
            } else if ( pageID.Equals( PageInfo.WiFiEcu.pageId ) ) {
                //WiFi-ECU
                groupCd = GroupCd.Parts;
                classCd = PartsKind.PARTS_CD_TRACTOR_WECU;
            } else if ( pageID.Equals( PageInfo.Rops.pageId ) ) {
                //ロプス
                groupCd = GroupCd.Parts;
                classCd = PartsKind.PARTS_CD_TRACTOR_ROPS;
            } else if ( pageID.Equals( PageInfo.Crate.pageId ) ) {
                //クレート
                groupCd = GroupCd.Parts;
                classCd = PartsKind.PARTS_CD_TRACTOR_CRATE;
            } else if ( pageID.Equals( PageInfo.Nameplate.pageId ) ) {
                //銘板ラベル
                groupCd = GroupCd.Parts;
                classCd = PartsKind.PARTS_CD_TRACTOR_NAMEPLATE;
            } else if ( pageID.Equals( PageInfo.Mission.pageId ) ) {
                //ミッション
                groupCd = GroupCd.Parts;
                classCd = PartsKind.PARTS_CD_TRACTOR_MISSION;
            } else if ( pageID.Equals( PageInfo.Housing.pageId ) ) {
                //ハウジング
                groupCd = GroupCd.Parts;
                classCd = PartsKind.PARTS_CD_TRACTOR_HOUSING;
            } else if ( pageID.Equals( PageInfo.CrankCase.pageId ) ) {
                //クランクケース
                groupCd = GroupCd.Parts;
                classCd = PartsKind.PARTS_CD_ENGINE_CC;
            } else if ( pageID.Equals( PageInfo.CylinderHead.pageId ) ) {
                //シリンダヘッド
                groupCd = GroupCd.Parts;
                classCd = PartsKind.PARTS_CD_ENGINE_CYH;
            } else if ( pageID.Equals( PageInfo.CrankShaft.pageId ) ) {
                //クランクシャフト
                groupCd = GroupCd.Parts;
                classCd = PartsKind.PARTS_CD_ENGINE_CS;
            } else if ( pageID.Equals( PageInfo.Dpf.pageId ) ) {
                //DPF
                groupCd = GroupCd.Parts;
                classCd = PartsKind.PARTS_CD_ENGINE_DPF;
            } else if ( pageID.Equals( PageInfo.Ecu.pageId ) ) {
                //ECU
                groupCd = GroupCd.Parts;
                classCd = PartsKind.PARTS_CD_ENGINE_ECU;
            } else if ( pageID.Equals( PageInfo.Epr.pageId ) ) {
                //EPR
                groupCd = GroupCd.Parts;
                classCd = PartsKind.PARTS_CD_ENGINE_EPR;
            } else if ( pageID.Equals( PageInfo.Injecter.pageId ) ) {
                //インジェクタ
                groupCd = GroupCd.Parts;
                classCd = PartsKind.PARTS_CD_ENGINE_INJECTOR;
            } else if ( pageID.Equals( PageInfo.Mixer.pageId ) ) {
                //MIXER
                groupCd = GroupCd.Parts;
                classCd = PartsKind.PARTS_CD_ENGINE_MIXER;
            } else if ( pageID.Equals( PageInfo.SupplyPump.pageId ) ) {
                //サプライポンプ
                groupCd = GroupCd.Parts;
                classCd = PartsKind.PARTS_CD_ENGINE_SUPPLYPUMP;
            } else if ( pageID.Equals( PageInfo.Scr.pageId ) ) {
                //SCR
                groupCd = GroupCd.Parts;
                classCd = PartsKind.PARTS_CD_ENGINE_SCR;
            } else if ( pageID.Equals( PageInfo.Doc.pageId ) ) {
                //DOC
                groupCd = GroupCd.Parts;
                classCd = PartsKind.PARTS_CD_ENGINE_DOC;
            } else if ( pageID.Equals( PageInfo.Acu.pageId ) ) {
                //Acu
                groupCd = GroupCd.Parts;
                classCd = PartsKind.PARTS_CD_ENGINE_ACU;
            } else if ( pageID.Equals( PageInfo.SerialPrint.pageId ) ) {
                //SerialPrint
                groupCd = GroupCd.Parts;
                classCd = PartsKind.PARTS_CD_ENGINE_SERIALPRINT;
            } else if ( pageID.Equals( PageInfo.AtuPartsSerial ) ) {
                //SerialPrint
                groupCd = PartsSearchTarget.Atu;
                classCd = AtuProcessKind.ATU_PARTS_SERIAL;
            } else if ( pageID.Equals( PageInfo.TorqueTightenRecord ) ) {
                //SerialPrint
                groupCd = PartsSearchTarget.Atu;
                classCd = AtuProcessKind.ATU_TORQUE_TIGHTENING_RECORD;
            } else if ( pageID.Equals( PageInfo.LeakMeasureResult ) ) {
                //SerialPrint
                groupCd = PartsSearchTarget.Atu;
                classCd = AtuProcessKind.ATU_LEAK_MEASURE_RESULT;
            } else if ( pageID.Equals( PageInfo.RackPositionSensor ) ) {
                //ラック位置センサ
                groupCd = GroupCd.Parts;
                classCd = PartsKind.PARTS_CD_ENGINE_RACK_POSITION_SENSOR;
            } else if ( pageID.Equals( PageInfo.Ipu.pageId ) ) {
                //IPU
                groupCd = GroupCd.Parts;
                classCd = PartsKind.PARTS_CD_ENGINE_IPU;
            } else if ( pageID.Equals( PageInfo.Ehc.pageId ) ) {
                //EHC
                groupCd = GroupCd.Parts;
                classCd = PartsKind.PARTS_CD_ENGINE_EHC;
            } else if ( pageID.Equals( PageInfo.CoreParts.pageId ) ) {
                //基幹部品
                groupCd = GroupCd.Parts;
            }
        }

    }
}