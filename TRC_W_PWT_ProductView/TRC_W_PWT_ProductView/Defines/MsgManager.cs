using KTFramework.Common;
using KTFramework.Common.App;
using KTFramework.Common.App.WebApp;

namespace TRC_W_PWT_ProductView.Defines {

    /// <summary>
    /// メッセージ管理クラス
    /// </summary>
    public class MsgManager : MsgManagerBase {
        ////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>プロジェクトコード</summary>
        /// <remarks>
        /// システムメッセージに指定するプロジェクトコード(2-5桁)を指定して下さい。
        /// この値はプロジェクトごとに指定する必要があります。
        /// </remarks>
        private const string PROJECT_CD = "TRC";

        /// <summary>アプリケーション種別</summary>
        protected const AppKind APP_KIND = AppKind.WEBVIEW;

        /// <summary>
        /// アプリケーション用メッセージ定義
        /// </summary>
        /// <param name="messageType">メッセージ種別(MessageType…Info:情報 Warn:警告 Error:異常 Conf:確認)</param>
        /// <param name="messageCode">5桁のユニークなID</param>
        /// <param name="message">メッセージ文言(書式項目の指定も可…メッセージ取得時に注意が必要)</param>
        /// <returns>メッセージ定義</returns>
        protected static MsgDef MessageDefine( MessageType messageType, string messageCode, string message ) {
            return Define( PROJECT_CD, messageType, APP_KIND, messageCode, message );
        }

        //■個別：情報メッセージ(10000番～44999番)
        /// <summary>登録しました。</summary>
        public readonly static MsgDef MESSAGE_INF_10010 = MessageDefine( MessageType.Info, "10010", "登録しました。" );
        /// <summary>更新しました。</summary>
        public readonly static MsgDef MESSAGE_INF_10020 = MessageDefine( MessageType.Info, "10020", "更新しました。" );
        /// <summary>削除しました。</summary>
        public readonly static MsgDef MESSAGE_INF_10030 = MessageDefine( MessageType.Info, "10030", "削除しました。" );
        /// <summary>ユーザ実行JOB起動要求結果(正常)：[{0}]</summary>
        public readonly static MsgDef MESSAGE_INF_10040 = MessageDefine( MessageType.Info, "10040", "ユーザ実行JOB起動要求結果：{0}" );

        //■個別：情報メッセージ(50000番～54999番)
        /// <summary>パスワードを更新しました。</summary>
        public readonly static MsgDef MESSAGE_INF_50010 = MessageDefine( MessageType.Info, "50010", "パスワードを更新しました。" );
        /// <summary>検索台数 : {0} 台  直通台数 : {1} 台   直通率 : {2} %</summary>
        public readonly static MsgDef MESSAGE_INF_50020 = MessageDefine( MessageType.Info, "50020", "検索台数 : {0} 台  直通台数 : {1} 台   直通率 : {2} %" );
        /// <summary>検索台数 : {0}台</summary>
        public readonly static MsgDef MESSAGE_INF_50030 = MessageDefine( MessageType.Info, "50030", "検索台数 : {0}台" );
        /// <summary>{0} 現在</summary>
        public readonly static MsgDef MESSAGE_INF_50040 = MessageDefine( MessageType.Info, "50040", "{0} 現在" );



        //■個別：警告メッセージ(60000番～64999番)
        /// <summary>検索対象が見つかりませんでした。条件を見直してください。</summary>
        public readonly static MsgDef MESSAGE_WRN_61010 = MessageDefine( MessageType.Warn, "61010", "検索対象が見つかりませんでした。条件を見直してください。" );
        /// <summary>検索対象が多すぎる為、すべての結果は表示されません。条件を絞り込んでください。</summary>
        public readonly static MsgDef MESSAGE_WRN_61020 = MessageDefine( MessageType.Warn, "61020", "検索対象が多すぎる為、すべての結果は表示されません。条件を絞り込んでください。" );
        /// <summary>対象型式の抽出件数が多すぎます。対象型式を絞り込んでください。</summary>
        public readonly static MsgDef MESSAGE_WRN_61030 = MessageDefine( MessageType.Warn, "61030", "対象型式の抽出件数が多すぎます。対象型式を絞り込んでください。" );
        /// <summary>出力対象が見つかりませんでした。条件を見直してください。</summary>
        public readonly static MsgDef MESSAGE_WRN_61040 = MessageDefine( MessageType.Warn, "61040", "出力対象が見つかりませんでした。条件を見直してください。" );
        /// <summary>選択した工程はエクセル出力対象外です。</summary>
        public readonly static MsgDef MESSAGE_WRN_61050 = MessageDefine( MessageType.Warn, "61050", "選択した工程はエクセル出力対象外です。" );
        /// <summary>検索期間の{0}を指定してください。</summary>
        public readonly static MsgDef MESSAGE_WRN_61051 = MessageDefine( MessageType.Warn, "61051", "検索期間の{0}を指定してください。" );
        /// <summary>検索期間は{0}ヵ月以内で指定してください。</summary>
        public readonly static MsgDef MESSAGE_WRN_61052 = MessageDefine( MessageType.Warn, "61052", "検索期間は{0}ヵ月以内で指定してください。" );
        /// <summary>ラインを選択してください。</summary>
        public readonly static MsgDef MESSAGE_WRN_61060 = MessageDefine( MessageType.Warn, "61060", "ラインを選択してください。" );
        /// <summary>検索対象が見つかりませんでした。条件を見直してください。</summary>
        public readonly static MsgDef MESSAGE_WRN_61070 = MessageDefine( MessageType.Warn, "61070", "検索対象が見つかりませんでした。条件を見直してください。" );
        /// <summary>対象行を選択してください。</summary>
        public readonly static MsgDef MESSAGE_WRN_61080 = MessageDefine( MessageType.Warn, "61080", "対象行を選択してください。" );
        /// <summary>対象ラインがありません。製品種別を変更してください。</summary>
        public readonly static MsgDef MESSAGE_WRN_61090 = MessageDefine( MessageType.Warn, "61090", "対象ラインがありません。製品種別を変更してください。" );
        /// <summary>検索に時間がかかっています。検索範囲を絞り込んでください。</summary>
        public readonly static MsgDef MESSAGE_WRN_61910 = MessageDefine( MessageType.Warn, "61910", "検索に時間がかかっています。検索範囲を絞り込んでください。" );

        /// <summary>[{0}]の詳細情報が見つかりませんでした。</summary>
        public readonly static MsgDef MESSAGE_WRN_62010 = MessageDefine( MessageType.Warn, "62010", "[{0}]の詳細情報が見つかりませんでした。" );
        /// <summary>[{0}]が選択されていません。</summary>
        public readonly static MsgDef MESSAGE_WRN_62020 = MessageDefine( MessageType.Warn, "62020", "[{0}]が選択されていません。" );
        /// <summary>[{0}]が入力されていません。</summary>
        public readonly static MsgDef MESSAGE_WRN_62030 = MessageDefine( MessageType.Warn, "62030", "[{0}]が入力されていません。" );
        /// <summary>[{0}]選択時は、{1}桁の機番番号を入力してください。</summary>
        public readonly static MsgDef MESSAGE_WRN_62040 = MessageDefine( MessageType.Warn, "62040", "[{0}]選択時は、{1}桁の機番を入力してください。" );
        /// <summary>[{0}]の入力内容が不適切です。</summary>
        public readonly static MsgDef MESSAGE_WRN_62050 = MessageDefine( MessageType.Warn, "62050", "[{0}]の入力内容が不適切です。" );
        /// <summary>既に登録済みの[{0}]ため、選択できません。</summary>
        public readonly static MsgDef MESSAGE_WRN_62060 = MessageDefine( MessageType.Warn, "62060", "既に登録済みの[{0}]ため、選択できません。" );
        /// <summary>[{0}]には、[{1}]を入力してください。</summary>
        public readonly static MsgDef MESSAGE_WRN_62070 = MessageDefine( MessageType.Warn, "62070", "[{0}]には、[{1}]を入力してください。" );
        /// <summary>{0}を入力してください。</summary>
        public readonly static MsgDef MESSAGE_WRN_62080 = MessageDefine( MessageType.Warn, "62080", "{0}を入力してください。" );
        /// <summary>{0}は{1}で入力してください。</summary>
        public readonly static MsgDef MESSAGE_WRN_62090 = MessageDefine( MessageType.Warn, "62090", "{0}は{1}で入力してください。" );

        /// <summary>実行中のエクセル出力要求があります。時間を置いてから再実行してください。</summary>
        public readonly static MsgDef MESSAGE_WRN_63010 = MessageDefine( MessageType.Warn, "63010", "実行中のエクセル出力要求があります。時間を置いてから再実行してください。" );

        /// <summary>表示上限件数({0}件)を超過しています。絞り込み条件を入力してください。</summary>
        public readonly static MsgDef MESSAGE_WRN_64010 = MessageDefine( MessageType.Warn, "64010", "表示上限件数({0}件)を超過しています。絞り込み条件を入力してください。" );

        /// <summary>設備情報が取得できません、先に設備を作成してください。</summary>
        public readonly static MsgDef MESSAGE_WRN_64020 = MessageDefine( MessageType.Warn, "64020", "設備情報が取得できません、先に設備を作成してください。" );

        /// <summary>最初行が選択されています、上への行移動はできません。</summary>
        public readonly static MsgDef MESSAGE_WRN_64030 = MessageDefine( MessageType.Warn, "64030", "最初行が選択されています、上への行移動はできません。" );

        /// <summary>末尾行が選択されています、下への行移動はできません。</summary>
        public readonly static MsgDef MESSAGE_WRN_64040 = MessageDefine( MessageType.Warn, "64040", "末尾行が選択されています、下への行移動はできません。" );


        //■個別：異常メッセージ(70000番～74999番)
        /// <summary>セッションが切断されました。</summary>
        public readonly static MsgDef MESSAGE_ERR_70110 = MessageDefine( MessageType.Error, "70110", "セッションが切断されました。" );
        /// <summary>不正な画面遷移が行われました。</summary>
        public readonly static MsgDef MESSAGE_ERR_70120 = MessageDefine( MessageType.Error, "70120", "不正な画面遷移が行われました。" );

        /// <summary>別ウィンドウからのログインが行われています。ブラウザから画面を終了してください。</summary>
        public readonly static MsgDef MESSAGE_ERR_70130 = MessageDefine( MessageType.Error, "70130", "別ウィンドウからのログインが行われています。ブラウザから画面を終了してください。" );
        /// <summary>画面遷移先指定に異常があります。</summary>
        public readonly static MsgDef MESSAGE_ERR_70140 = MessageDefine( MessageType.Error, "70140", "画面遷移先指定に異常があります。" );

        /// <summary>入力内容が不適切です。</summary>
        public readonly static MsgDef MESSAGE_ERR_70210 = MessageDefine( MessageType.Error, "70210", "入力内容が不適切です。" );
        /// <summary>指定ファイル[{0}]が見つかりません。</summary>
        public readonly static MsgDef MESSAGE_ERR_71010 = MessageDefine( MessageType.Error, "71010", "指定ファイル[{0}]が見つかりません。" );
        /// <summary>既に登録済みのため、登録できません。</summary>
        public readonly static MsgDef MESSAGE_ERR_72010 = MessageDefine( MessageType.Error, "72010", "既に登録済みのため、登録できません。" );
        /// <summary>ログインユーザ情報が見つかりません。\n再度ログインしてください。</summary>
        public readonly static MsgDef MESSAGE_ERR_72020 = MessageDefine( MessageType.Error, "72020", "ログインユーザ情報が見つかりません。\n再度ログインしてください。" );
        /// <summary>更新権限がないため、更新できません。</summary>
        public readonly static MsgDef MESSAGE_ERR_72030 = MessageDefine( MessageType.Error, "72030", "更新権限がないため、更新できません。" );
        /// <summary>対象データが見つかりません。</summary>
        public readonly static MsgDef MESSAGE_ERR_72040 = MessageDefine( MessageType.Error, "72040", "対象データが見つかりません。" );
        /// <summary>完成済のため、削除できません。</summary>
        public readonly static MsgDef MESSAGE_ERR_72050 = MessageDefine( MessageType.Error, "72050", "完成済のため、削除できません。" );
        /// <summary>{0}処理に失敗しました。</summary>
        public readonly static MsgDef MESSAGE_ERR_72060 = MessageDefine( MessageType.Error, "72060", "{0}処理に失敗しました。" );

        /// <summary>ユーザ実行JOB起動要求結果(エラー)：[{0}]</summary>
        public readonly static MsgDef MESSAGE_ERR_72070 = MessageDefine( MessageType.Error, "72070", "ユーザ実行JOB起動要求結果：{0}" );

        /// <summary>加工日は数字8桁(yyyymmdd)で入力してください。</summary>
        public readonly static MsgDef MESSAGE_ERR_72080 = MessageDefine( MessageType.Error, "72080", "加工日は数字8桁(yyyymmdd)で入力してください。" );
        /// <summary>連番は数字3桁以内で入力してください。</summary>
        public readonly static MsgDef MESSAGE_ERR_72090 = MessageDefine( MessageType.Error, "72090", "連番は数字3桁以内で入力してください。" );
        /// <summary>加工ラインは英数字4桁以内で入力してください。</summary>
        public readonly static MsgDef MESSAGE_ERR_72095 = MessageDefine( MessageType.Error, "72095", "加工ラインは英数字4桁以内で入力してください。" );
        /// <summary>備考は全角100字以内で入力してください。</summary>
        public readonly static MsgDef MESSAGE_ERR_72100 = MessageDefine( MessageType.Error, "72100", "備考は全角100字以内で入力してください。" );

        /// <summary>製品種別を特定できませんでした。指示レベルを指定してください。\n指示レベルを指定しない場合は型式コード(10桁)又はIDNOを指定してください。</summary>
        public readonly static MsgDef MESSAGE_ERR_72200 = MessageDefine( MessageType.Error, "72200", "製品種別を特定できませんでした。指示レベルを指定してください。\n指示レベルを指定しない場合は型式コード(10桁)又はIDNOを指定してください。" );

        //■個別：例外メッセージ(80000番～84999番)
        /// <summary>システムエラーが発生しました。</summary>
        public readonly static MsgDef MESSAGE_ERR_80010 = MessageDefine( MessageType.Error, "80010", "システムエラーが発生しました。" );
        /// <summary>{0}システムエラーが発生しました。</summary>
        public readonly static MsgDef MESSAGE_ERR_80011 = MessageDefine( MessageType.Error, "80011", "{0}システムエラーが発生しました。" );
        /// <summary>イベント[{0}]が定義されていません。</summary>
        public readonly static MsgDef MESSAGE_ERR_80020 = MessageDefine( MessageType.Error, "80020", "イベント[{0}]が定義されていません。" );
        /// <summary>ファイル[{0}]出力処理中に異常が発生しました。</summary>
        public readonly static MsgDef MESSAGE_ERR_80030 = MessageDefine( MessageType.Error, "80030", "ファイル[{0}]出力処理中に異常が発生しました。" );

        /// <summary>型式コードまたは型式名を入力してください。</summary>
        public readonly static MsgDef MESSAGE_ERR_81010 = MessageDefine( MessageType.Error, "81010", "型式コードまたは型式名を入力してください。" );
        /// <summary>部品品番または部品機番を入力してください。</summary>
        public readonly static MsgDef MESSAGE_ERR_81020 = MessageDefine( MessageType.Error, "81020", "型式または部品品番、部品機番を入力してください。" );
        /// <summary>製品検索に失敗しました。</summary>
        public readonly static MsgDef MESSAGE_ERR_82010 = MessageDefine( MessageType.Error, "82010", "製品検索に失敗しました。" );

        /// <summary>ユーザID又はパスワードが入力されていません。</summary>
        public readonly static MsgDef MESSAGE_ERR_83010 = MessageDefine( MessageType.Error, "83010", "ユーザID又はパスワードが入力されていません。" );
        /// <summary>ユーザID、旧パスワード、新パスワード、新パスワード確認を全て入力してください。</summary>
        public readonly static MsgDef MESSAGE_ERR_83020 = MessageDefine( MessageType.Error, "83020", "ユーザID、旧パスワード、新パスワード、新パスワード確認を全て入力してください。" );
        /// <summary>新パスワードと新パスワード確認が不一致です。</summary>
        public readonly static MsgDef MESSAGE_ERR_83030 = MessageDefine( MessageType.Error, "83030", "新パスワードと新パスワード確認が不一致です。" );
        /// <summary>旧パスワードと新パスワードが同一です。</summary>
        public readonly static MsgDef MESSAGE_ERR_83040 = MessageDefine( MessageType.Error, "83040", "旧パスワードと新パスワードが同一です。" );
        /// <summary>入力されたユーザのパスワードは変更できません。</summary>
        public readonly static MsgDef MESSAGE_ERR_83050 = MessageDefine( MessageType.Error, "83050", "入力されたユーザのパスワードは変更できません。" );
        /// <summary>DB接続ができませんでした。処理をキャンセルします。</summary>
        public readonly static MsgDef MESSAGE_ERR_84010 = MessageDefine( MessageType.Error, "84010", "DB接続ができませんでした。処理をキャンセルします。" );
        //WEBService関連

        /// <summary>WEBサービスで実行エラーが発生しました。</summary>
        public readonly static MsgDef MESSAGE_ERR_89000 = MessageDefine( MessageType.Error, "89000", "WEBサービスで実行エラーが発生しました。" );
        /// <summary>認証に失敗しました。エラーコード:{0} エラーメッセージ:{1}</summary>
        public readonly static MsgDef MESSAGE_ERR_89001 = MessageDefine( MessageType.Error, "89001", "認証に失敗しました。エラーコード:{0} エラーメッセージ:{1}" );
        /// <summary>システムへの利用権限がありません。</summary>
        public readonly static MsgDef MESSAGE_ERR_89002 = MessageDefine( MessageType.Error, "89002", "システムへの利用権限がありません。" );
        /// <summary>WEBサービスURLが取得できません。</summary>
        public readonly static MsgDef MESSAGE_ERR_89003 = MessageDefine( MessageType.Error, "89003", "WEBサービスURLが取得できません。" );

        //■個別：確認メッセージ(90000番～94999番)
        /// <summary>ログアウトします。よろしいですか？</summary>
        public readonly static MsgDef MESSAGE_CNF_90010 = MessageDefine( MessageType.Conf, "90010", "ログアウトします。よろしいですか？" );
    }
}