using System;
using System.IO;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Web;
using TRC_W_PWT_ProductView.Report.Dto;
using TRC_W_PWT_ProductView.Common;
using PdfSharp.Drawing;
using TRC_W_PWT_ProductView.Dao.Process;
using System.Reflection;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using PdfSharp;

namespace TRC_W_PWT_ProductView.Report {
    /*
        Copyright (c) 2005-2014 empira Software GmbH, Troisdorf (Germany)

        Permission is hereby granted, free of charge, to any person
        obtaining a copy of this software and associated documentation
        files (the "Software"), to deal in the Software without
        restriction, including without limitation the rights to use,
        copy, modify, merge, publish, distribute, sublicense, and/or sell
        copies of the Software, and to permit persons to whom the
        Software is furnished to do so, subject to the following
        conditions: 

        The above copyright notice and this permission notice shall be
        included in all copies or substantial portions of the Software. 

        THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
        EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
        OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
        NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
        HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
        WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
        FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
        OTHER DEALINGS IN THE SOFTWARE. 
    */
    public class EngineInjection07DetailReport {

        /// <summary>
        /// 出力定義
        /// </summary>
        public struct OutputDefine {
            public Point OutputPosition { get; set; }
            public string Format { get; set; }
            public XFont Font { get; set; }
            public bool Centerized { get; set; }
            public XRect? TextBoxRect { get; set; }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <param name="emSize"></param>
            /// <param name="fontStyle"></param>
            public OutputDefine( double x, double y, double emSize, XFontStyle fontStyle, string format = null ) {
                OutputPosition = new Point( x, y );
                Font = new XFont( "IPAexゴシック", emSize, fontStyle );
                Format = format;
                Centerized = false;
                TextBoxRect = null;
            }
            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="x">X座標</param>
            /// <param name="y">Y座標</param>
            /// <param name="emSize">em単位サイズ</param>
            /// <param name="fontStyle">フォント</param>
            /// <param name="rectWidth">テキストボックス幅</param>
            /// <param name="rectHeight">テキストボックス高さ</param>
            public OutputDefine( double x, double y, double emSize, XFontStyle fontStyle, double rectWidth, double rectHeight, string format = null ) {
                OutputPosition = new Point( x, y );
                Font = new XFont( "IPAexゴシック", emSize, fontStyle );
                Format = format;
                Centerized = true;
                TextBoxRect = new XRect( x, y, rectWidth, rectHeight );
            }
        } 


        /// <summary>
        /// 燃料噴射時期07 詳細データ出力設定クラス
        public class EngineInjection07DetailOutputDefines {

            #region ヘッダー（帳票左上）
            public OutputDefine MeasuringDate { get; set; } = new OutputDefine( 140, 108, 13, XFontStyle.Regular, "yyyy/MM/dd HH:mm:ss" );
            public OutputDefine ModelName { get; set; } = new OutputDefine( 140, 144, 13, XFontStyle.Regular );
            public OutputDefine Serial { get; set; } = new OutputDefine( 140, 180, 13, XFontStyle.Regular );
            public OutputDefine Idno { get; set; } = new OutputDefine( 140, 216, 13, XFontStyle.Regular );
            public OutputDefine PonpuPartNum { get; set; } = new OutputDefine( 140, 252, 13, XFontStyle.Regular );
            public OutputDefine PonpuSerial { get; set; } = new OutputDefine( 140, 288, 13, XFontStyle.Regular );
            public OutputDefine Result { get; set; } = new OutputDefine( 345, 288, 13, XFontStyle.Bold );
            public OutputDefine ResultDisplayString { get; set; } = new OutputDefine( 365, 288, 13, XFontStyle.Underline );
            #endregion

            #region 測定データ1（帳票左1段目）
            /// <summary>ピストン出代</summary>
            public OutputDefine PistonBumpLowerLimit { get; set; } = new OutputDefine( 175, 332, 11, XFontStyle.Regular, 20, 20, "F3" );
            public OutputDefine PistonBumpUpperLimit { get; set; } = new OutputDefine( 240, 332, 11, XFontStyle.Regular, 20, 20, "F3" );
            public OutputDefine PistonBumpCylinder1 { get; set; } = new OutputDefine( 302, 332, 11, XFontStyle.Regular, 20, 20, "F3" );
            public OutputDefine PistonBumpCylinder2 { get; set; } = new OutputDefine( 364, 332, 11, XFontStyle.Regular, 20, 20, "F3" );
            public OutputDefine PistonBumpCylinder3 { get; set; } = new OutputDefine( 426, 332, 11, XFontStyle.Regular, 20, 20, "F3" );
            public OutputDefine PistonBumpCylinder4 { get; set; } = new OutputDefine( 488, 332, 11, XFontStyle.Regular, 20, 20, "F3" );
            public OutputDefine PistonBumpAverage { get; set; } = new OutputDefine( 550, 332, 11, XFontStyle.Regular, 20, 20, "F3" );
            /// <summary>ピストン出代ランク</summary>
            public OutputDefine PistonBumpRankLowerLimit { get; set; } = new OutputDefine( 175, 356, 11, XFontStyle.Regular, 20, 20, "F3" );
            public OutputDefine PistonBumpRankUpperLimit { get; set; } = new OutputDefine( 240, 356, 11, XFontStyle.Regular, 20, 20, "F3" );
            public OutputDefine PistonBumpRank { get; set; } = new OutputDefine( 302, 356, 11, XFontStyle.Regular, 20, 20 );
            /// <summary>燃料噴射時期</summary>
            public OutputDefine InjectionTimingLowerLimit { get; set; } = new OutputDefine( 175, 380, 11, XFontStyle.Regular, 20, 20, "F2" );
            public OutputDefine InjectionTimingUpperLimit { get; set; } = new OutputDefine( 240, 380, 11, XFontStyle.Regular, 20, 20, "F2" );
            public OutputDefine InjectionTimingCylinder1 { get; set; } = new OutputDefine( 302, 380, 11, XFontStyle.Regular, 20, 20, "F3" );
            public OutputDefine InjectionTimingCylinder2 { get; set; } = new OutputDefine( 364, 380, 11, XFontStyle.Regular, 20, 20, "F3" );
            public OutputDefine InjectionTimingCylinder3 { get; set; } = new OutputDefine( 426, 380, 11, XFontStyle.Regular, 20, 20, "F3" );
            public OutputDefine InjectionTimingCylinder4 { get; set; } = new OutputDefine( 488, 380, 11, XFontStyle.Regular, 20, 20, "F3" );
            public OutputDefine InjectionTimingAverage { get; set; } = new OutputDefine( 550, 380, 11, XFontStyle.Regular, 20, 20, "F3" );
            #endregion

            #region 測定データ2（帳票左2段目）
            /// <summary>気温</summary>
            public OutputDefine Temperature { get; set; } = new OutputDefine( 240, 416, 11, XFontStyle.Regular, 20, 20, "F1" );
            #endregion

            #region 測定データ3（帳票左3段目）
            /// <summary>パルスタイミング角度</summary>
            public OutputDefine PulseTimingAngle { get; set; } = new OutputDefine( 240, 452, 11, XFontStyle.Regular, 20, 20, "F1" );
            #endregion
        }

        #region プロパティ

        /// <summary>
        /// 出力データ
        /// </summary>
        public EngineInjection07DetailDto TargetSource { get; set; }

        #endregion

        #region フィールド

        //出力位置定義
        private EngineInjection07DetailOutputDefines _outputDefines;

        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public EngineInjection07DetailReport() {
            _outputDefines = new EngineInjection07DetailOutputDefines();
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="measurementDate"></param>
        /// <param name="modelNm"></param>
        /// <param name="serial"></param>
        public EngineInjection07DetailReport( DateTime measuringDate, string modelName, string serial ) {
            TargetSource = ProcessSearchDao.SelectEngineInjection07DetailForPdfOutput( measuringDate, modelName, serial );
            _outputDefines = new EngineInjection07DetailOutputDefines();
        }

        /// <summary>
        /// PDFダウンロード
        /// </summary>
        /// <param name="httpResponse"></param>
        public void Download( HttpResponse httpResponse ) {

            //一時ファイル名
            string tempFileName = 
                $"{ WebAppInstance.GetInstance().Config.ApplicationInfo.temporaryBasePath }\\{ Path.GetFileNameWithoutExtension( Path.GetRandomFileName() ) }.pdf";

            //返却ファイル名
            string responseFileName = HttpUtility.UrlEncode( $"噴射計測データ_{ DateTime.Now.ToString( "yyyyMMddHHmmss" ) }.pdf" );

            try {
                //PDF出力処理
                OutputDetailToPdf( tempFileName );

                //レスポンス
                httpResponse.ClearContent();
                httpResponse.AddHeader( "Content-Disposition", $"attachment;filename={ responseFileName }" );
                httpResponse.ContentType = "application/pdf";
                httpResponse.BinaryWrite( File.ReadAllBytes( tempFileName ) );
                httpResponse.Flush();
                httpResponse.End();
                httpResponse.Close();
            } catch( Exception e ) {
                throw;
            } finally {
                if( File.Exists( tempFileName ) ) {
                    File.Delete( tempFileName );
                }
            }
        }

        /// <summary>
        /// PDF出力
        /// </summary>
        /// <param name="fileName"></param>
        private void OutputDetailToPdf( string fileName ) {
            if ( TargetSource == null ) {
                throw new InvalidOperationException( "出力ソースを設定してください。" );
            }

            //テンプレートファイルオープン
            PdfDocument document = 
                PdfReader.Open( WebAppInstance.GetInstance().Config.ApplicationInfo.reportTemplateBasePath + WebAppInstance.GetInstance().Config.ApplicationInfo.repEngineInjection07Detail );

            //メタ情報設定
            document.Info.Title = "噴射時期計測データ";
            document.Info.Subject = $"測定日時:{ TargetSource.MeasuringDate.ToString() },エンジン型式名:{ TargetSource.ModelName },エンジン機番:{ TargetSource.Serial },IDNO:{ TargetSource.Idno },噴射ポンプ品番:{ TargetSource.PonpuPartNum },噴射ポンプ機番:{ TargetSource.PonpuSerial }";
            document.Info.Author = WebAppInstance.GetInstance().AppDisplayName;
            document.Info.Keywords = $"{ TargetSource.MeasuringDate.ToString() },{ TargetSource.ModelName },{ TargetSource.Serial },{ TargetSource.Idno },{ TargetSource.PonpuPartNum },{ TargetSource.PonpuSerial }";
            document.Info.Creator = WebAppInstance.GetInstance().AppName;

            //ページレイアウト設定
            PdfPage page = document.Pages[0];
            page.Size = PageSize.A4;
            page.Orientation = PageOrientation.Landscape;

            //PDF書き出し
            PropertyInfo[] defineProps = typeof( EngineInjection07DetailOutputDefines ).GetProperties();
            PropertyInfo[] targetSourceProps = typeof( EngineInjection07DetailDto ).GetProperties();
            XGraphics gfx = XGraphics.FromPdfPage( page );
            foreach ( var dp in defineProps ) {
                PropertyInfo tsp = targetSourceProps.Where( x => x.Name == dp.Name ).Single();
                OutputDefine define = ( OutputDefine )dp.GetValue( _outputDefines , null );
                if ( define.Centerized ) {
                    gfx.DrawString( GetFormattedString( tsp.GetValue( TargetSource, null ), define.Format ), define.Font, XBrushes.Black, ( XRect )define.TextBoxRect, XStringFormat.Center );
                } else {
                    gfx.DrawString( GetFormattedString( tsp.GetValue( TargetSource, null ), define.Format ), define.Font, XBrushes.Black, define.OutputPosition.X, define.OutputPosition.Y );
                }
            }

            //PDF保存
            document.Save( fileName );
        }

        /// <summary>
        /// フォーマット適用文字列取得
        /// </summary>
        /// <param name="target"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        private string GetFormattedString( object target, string format ) {
            //NULLは空文字を返す
            if ( target == null ) {
                return string.Empty;
            }

            //フォーマット
            if ( target is int ) {
                return (( int )target).ToString( format ).Replace("-", "－");
            } else if( target is decimal ) {
                return (( decimal )target).ToString( format ).Replace( "-", "－" );
            } else if( target is DateTime ) {
                return (( DateTime )target).ToString( format );
            } else {
                if ( format != null ) {
                    throw new NotImplementedException( "変換処理が未実装です。" );
                }
            }

            //フォーマット無し or 文字列
            return target.ToString();
        }

    }
}