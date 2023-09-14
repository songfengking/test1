using KTFramework.Common;
using System.Reflection;
using System.Collections.Generic;
using KTFramework.Web.Client.Service;
using KTFramework.Common.App.WebApp;
using TRC_W_PWT_ProductView.SrvCore;

namespace TRC_W_PWT_ProductView.Common {

    public class CoreService {
        /// <summary>ロガー定義</summary>
        private static readonly Logger logger = new Logger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

        /// <summary>
        /// ジョブ実行パラメータ構造体
        /// </summary>
        public class JobParameter {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="key">パラメータキー</param>
            /// <param name="value">パラメータ値</param>
            public JobParameter( string key, string value ) {
                this.Key = key;
                this.Value = value;
            }
            /// <summary>パラメータキー</summary>
            public string Key { get; set; }
            /// <summary>パラメータ値</summary>
            public string Value { get; set; }
        }

        /// <summary>
        /// コンストラクタ
        /// CoreサービスはaccessTokenを使用しない
        /// </summary>
        public CoreService( )
             {
        }

        /// <summary>
        /// ユーザ実行JOB起動サービスコール
        /// </summary>
        /// <param name="taskId">起動するユーザ実行JOBのタスクID(DC3INIMAP)</param>
        /// <param name="userId">実行ユーザID</param>
        /// <param name="userNm">実行ユーザ名</param>
        /// <param name="mailAddr">通知先メールアドレス</param>
        /// <param name="ipAddr">呼び出し端末IP</param>        
        /// <param name="paramList">ユーザ実行JOBへ渡すパラメータ</param>
        /// <returns>サービス実行結果</returns>
        public C1010RequestDto RequestBatchExec( string taskId, string userId, string userNm,string mailAddr, string ipAddr, List<JobParameter> paramList ) {
            //WEBサービスによるパスワード変更処理
            WebServices C1010RequestDto = new WebServices();
            C1010RequestDto requestDto = new C1010RequestDto();
            requestDto.callerAppNm = WebAppBase.GetInstance().AppId;    //呼出元アプリケーション
            requestDto.userId = userId;                                 //ユーザID
            requestDto.userNm = userNm;                                 //ユーザ名
            requestDto.mailAddress = mailAddr;                          //メールアドレス
            requestDto.terminalIp = ipAddr;                             //端末IP
            requestDto.taskId = taskId;
            if ( false == ObjectUtils.IsNull( paramList ) && 0 < paramList.Count ) {
                requestDto.parameterList = new C1010BatchParameterDto[paramList.Count];
                for ( int index = 0; index < paramList.Count; index++ ) {
                    requestDto.parameterList[index] = new C1010BatchParameterDto();
                    requestDto.parameterList[index].key = paramList[index].Key;
                    requestDto.parameterList[index].value = paramList[index].Value;
                }
            }
            Core CoreService = new Core();
            return CoreService.C1010sRequestBatchExec( requestDto );
        }

        /// <summary>
        /// ユーザ実行JOB起動サービスコール
        /// </summary>
        /// <param name="taskId">起動するユーザ実行JOBのタスクID(DC3INIMAP)</param>
        /// <param name="userId">実行ユーザID</param>
        /// <param name="userNm">実行ユーザ名</param>
        /// <param name="mailAddr">通知先メールアドレス</param>
        /// <param name="ipAddr">呼び出し端末IP</param>        
        /// <param name="jobNetName">ジョブネット名</param>        
        /// <param name="paramList">ユーザ実行JOBへ渡すパラメータ</param>
        /// <returns>サービス実行結果</returns>
        public C1010RequestDto RequestBatchExec2( string taskId, string userId, string userNm, string mailAddr, string ipAddr, string jobNetName, List<JobParameter> paramList ) {
            C1010RequestDto requestDto = new C1010RequestDto();
            requestDto.callerAppNm = WebAppBase.GetInstance().AppId;    //呼出元アプリケーション
            requestDto.userId = userId;                                 //ユーザID
            requestDto.userNm = userNm;                                 //ユーザ名
            requestDto.mailAddress = mailAddr;                          //メールアドレス
            requestDto.terminalIp = ipAddr;                             //端末IP
            requestDto.taskId = taskId;
            requestDto.jobNetNm = jobNetName;
            if ( false == ObjectUtils.IsNull( paramList ) && 0 < paramList.Count ) {
                requestDto.parameterList = new C1010BatchParameterDto[paramList.Count];
                for ( int index = 0; index < paramList.Count; index++ ) {
                    requestDto.parameterList[index] = new C1010BatchParameterDto();
                    requestDto.parameterList[index].key = paramList[index].Key;
                    requestDto.parameterList[index].value = paramList[index].Value;
                }
            }
            Core CoreService = new Core();
            return CoreService.C1010sRequestBatchExec( requestDto );
        }

        /// <summary>
        /// LOBデータ取得サービスコール
        /// </summary>
        /// <param name="tableNm">テーブル名</param>
        /// <param name="fileNm">ファイル名</param>
        /// <returns>画像データ(Base64デコード)</returns>
        public byte[] GetLobData( string tableNm, string fileNm ) {
            byte[] bs = null;

            C3010SelectDto selectDto = new C3010SelectDto();
            selectDto.tableNm = tableNm;
            selectDto.fileNm = fileNm;

            // 共通サービス呼び出し
            Core CoreService = new Core();
            C3010SelectDto resultDto = CoreService.C3010sGetLobData( selectDto );

            // 画像データがあれば変換
            if ( true == StringUtils.IsNotBlank(resultDto.lobData) ) {
                bs = System.Convert.FromBase64String( resultDto.lobData );
            }
            return bs;
        }
    }
}