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
    public class EngineInjection03DetailReport {

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
        /// 燃料噴射時期03 詳細データ出力設定クラス
        public class EngineInjection03DetailOutputDefines {

            #region ヘッダー（帳票左上）
            public OutputDefine MeasuringDate { get; set; } = new OutputDefine( 140, 108, 13, XFontStyle.Regular, "yyyy/MM/dd HH:mm:ss" );
            public OutputDefine ModelName { get; set; } = new OutputDefine( 140, 144, 13, XFontStyle.Regular );
            public OutputDefine EnginePrint { get; set; } = new OutputDefine( 140, 180, 13, XFontStyle.Regular );
            public OutputDefine Serial { get; set; } = new OutputDefine( 140, 216, 13, XFontStyle.Regular );
            public OutputDefine Idno { get; set; } = new OutputDefine( 140, 252, 13, XFontStyle.Regular );
            public OutputDefine Result { get; set; } = new OutputDefine( 285, 252, 13, XFontStyle.Bold );
            public OutputDefine ResultDisplayString { get; set; } = new OutputDefine( 305, 252, 13, XFontStyle.Underline );
            #endregion

            #region 仕様（帳票右上）
            public OutputDefine NumberOfCylinder { get; set; } = new OutputDefine( 511, 118, 11, XFontStyle.Regular, 20, 20 );
            public OutputDefine Stroke { get; set; } = new OutputDefine( 600, 118, 11, XFontStyle.Regular, 20, 20, "F1" );
            public OutputDefine Bore { get; set; } = new OutputDefine(697, 118, 11, XFontStyle.Regular, 20, 20 );
            public OutputDefine CombustionSystem { get; set; } = new OutputDefine( 774, 118, 11, XFontStyle.Regular, 20, 20 );
            public OutputDefine AdjustAdvance { get; set; } = new OutputDefine( 500, 179, 11, XFontStyle.Bold );
            public OutputDefine AdjustAdvanceDisplayString { get; set; } = new OutputDefine( 520, 179, 11, XFontStyle.Regular );
            public OutputDefine LackPosition { get; set; } = new OutputDefine( 577, 165, 11, XFontStyle.Regular, 20, 20, "F1" );
            public OutputDefine LackSizeDifference { get; set; } = new OutputDefine( 630, 165, 11, XFontStyle.Regular, 20, 20, "F1" );
            public OutputDefine ScrewType { get; set; } = new OutputDefine( 697, 165, 11, XFontStyle.Regular, 20, 20 );
            public OutputDefine InjectionTimingOffset { get; set; } = new OutputDefine( 774, 165, 11, XFontStyle.Regular, 20, 20, "F1" );
            public OutputDefine MeasuringTerminal { get; set; } = new OutputDefine( 510, 196, 11, XFontStyle.Regular, 20, 20 );
            public OutputDefine MethodCylinder { get; set; } = new OutputDefine( 690, 209, 11, XFontStyle.Bold );
            public OutputDefine MethodCylinderDisplayString { get; set; } = new OutputDefine( 710, 209, 11, XFontStyle.Regular );
            public OutputDefine Recess { get; set; } = new OutputDefine( 500, 245, 11, XFontStyle.Bold );
            public OutputDefine RecessDisplayString { get; set; } = new OutputDefine( 520, 245, 11, XFontStyle.Regular );
            public OutputDefine MethodDirection { get; set; } = new OutputDefine( 690, 245, 11, XFontStyle.Bold );
            public OutputDefine MethodDirectionDisplayString { get; set; } = new OutputDefine( 710, 245, 11, XFontStyle.Regular );
            #endregion

            #region 測定データ1（帳票左下）
            public OutputDefine PistonBumpLowerLimit { get; set; } = new OutputDefine( 171, 296, 11, XFontStyle.Regular, 20, 20, "F2" );
            public OutputDefine PistonBumpUpperLimit { get; set; } = new OutputDefine( 240, 296, 11, XFontStyle.Regular, 20, 20, "F2" );
            public OutputDefine PistonBumpCylinder1 { get; set; } = new OutputDefine( 305, 296, 11, XFontStyle.Regular, 20, 20, "F3" );
            public OutputDefine PistonBumpCylinder2 { get; set; } = new OutputDefine( 370, 296, 11, XFontStyle.Regular, 20, 20, "F3" );
            public OutputDefine PistonBumpCylinder3 { get; set; } = new OutputDefine( 436, 296, 11, XFontStyle.Regular, 20, 20, "F3" );
            public OutputDefine PistonBumpCylinder4 { get; set; } = new OutputDefine( 505, 296, 11, XFontStyle.Regular, 20, 20, "F3" );
            public OutputDefine TopClearanceLowerLimit { get; set; } = new OutputDefine( 171, 320, 11, XFontStyle.Regular, 20, 20, "F2" );
            public OutputDefine TopClearanceUpperLimit { get; set; } = new OutputDefine( 240, 320, 11, XFontStyle.Regular, 20, 20, "F2" );
            public OutputDefine TopClearanceCylinder1 { get; set; } = new OutputDefine( 305, 320, 11, XFontStyle.Regular, 20, 20, "F3" );
            public OutputDefine TopClearanceCylinder2 { get; set; } = new OutputDefine( 370, 320, 11, XFontStyle.Regular, 20, 20, "F3" );
            public OutputDefine TopClearanceCylinder3 { get; set; } = new OutputDefine( 436, 320, 11, XFontStyle.Regular, 20, 20, "F3" );
            public OutputDefine TopClearanceCylinder4 { get; set; } = new OutputDefine( 505, 320, 11, XFontStyle.Regular, 20, 20, "F3" );
            public OutputDefine InjectionTimingUpperLimit { get; set; } = new OutputDefine( 171, 345, 11, XFontStyle.Regular, 20, 20, "F2" );
            public OutputDefine InjectionTimingLowerLimit { get; set; } = new OutputDefine( 240, 345, 11, XFontStyle.Regular, 20, 20, "F2" );
            public OutputDefine InjectionTimingCylinder1 { get; set; } = new OutputDefine( 305, 345, 11, XFontStyle.Regular, 20, 20, "F2" );
            public OutputDefine InjectionTimingCylinder2 { get; set; } = new OutputDefine( 370, 345, 11, XFontStyle.Regular, 20, 20, "F2" );
            public OutputDefine InjectionTimingCylinder3 { get; set; } = new OutputDefine( 436, 345, 11, XFontStyle.Regular, 20, 20, "F2" );
            public OutputDefine InjectionTimingCylinder4 { get; set; } = new OutputDefine( 505, 345, 11, XFontStyle.Regular, 20, 20, "F2" );
            public OutputDefine BumpSizeVariableBase { get; set; } = new OutputDefine( 202, 369, 11, XFontStyle.Regular, 20, 20, "F2" );
            public OutputDefine BumpSizeError { get; set; } = new OutputDefine( 337, 369, 11, XFontStyle.Regular, 20, 20, "F3" );
            public OutputDefine StandardGascketNumber { get; set; } = new OutputDefine( 205, 393, 11, XFontStyle.Regular, 20, 20 );
            public OutputDefine GascketSize { get; set; } = new OutputDefine( 339, 393, 11, XFontStyle.Regular, 20, 20, "F2" );
            public OutputDefine SelectedGascketNumber { get; set; } = new OutputDefine( 470, 393, 11, XFontStyle.Regular, 20, 20 );
            public OutputDefine BasePumpShimSize { get; set; } = new OutputDefine( 173, 417, 11, XFontStyle.Regular, 20, 20, "F3" );
            public OutputDefine TotalShimSize { get; set; } = new OutputDefine( 173, 442, 11, XFontStyle.Regular, 20, 20, "F3" );
            public OutputDefine NumberOfShim0175 { get; set; } = new OutputDefine( 237, 442, 11, XFontStyle.Regular, 20, 20 );
            public OutputDefine NumberOfShim0200 { get; set; } = new OutputDefine( 305, 442, 11, XFontStyle.Regular, 20, 20 );
            public OutputDefine NumberOfShim0250 { get; set; } = new OutputDefine( 370, 442, 11, XFontStyle.Regular, 20, 20 );
            public OutputDefine NumberOfShim0300 { get; set; } = new OutputDefine( 436, 442, 11, XFontStyle.Regular, 20, 20 );
            public OutputDefine NumberOfShim0350 { get; set; } = new OutputDefine( 505, 442, 11, XFontStyle.Regular, 20, 20 );
            public OutputDefine TotalNumberOfShim { get; set; } = new OutputDefine( 505, 465, 11, XFontStyle.Regular, 20, 20 );
            public OutputDefine JigOffset1 { get; set; } = new OutputDefine( 179, 477, 11, XFontStyle.Regular, 20, 20, "F3" );
            public OutputDefine JigOffset2 { get; set; } = new OutputDefine( 260, 477, 11, XFontStyle.Regular, 20, 20, "F3" );
            public OutputDefine JigOffset3 { get; set; } = new OutputDefine( 337, 477, 11, XFontStyle.Regular, 20, 20, "F3" );
            public OutputDefine MeasurementOffset { get; set; } = new OutputDefine( 177, 513, 11, XFontStyle.Regular, 20, 20, "F2" );
            public OutputDefine FitOffsetCylinderAverage { get; set; } = new OutputDefine( 237, 550, 11, XFontStyle.Regular, 20, 20, "F2" );
            public OutputDefine FitOffsetCylinder1 { get; set; } = new OutputDefine( 305, 550, 11, XFontStyle.Regular, 20, 20, "F2" );
            public OutputDefine FitOffsetCylinder2 { get; set; } = new OutputDefine( 370, 550, 11, XFontStyle.Regular, 20, 20, "F2" );
            public OutputDefine FitOffsetCylinder3 { get; set; } = new OutputDefine( 436, 550, 11, XFontStyle.Regular, 20, 20, "F2" );
            public OutputDefine FitOffsetCylinder4 { get; set; } = new OutputDefine( 505, 550, 11, XFontStyle.Regular, 20, 20, "F2" );
            #endregion

            #region 測定データ2（帳票右下の上）
            public OutputDefine MeasuringRow1Cylinder1 { get; set; } = new OutputDefine( 630, 320, 11, XFontStyle.Regular, 20, 20, "F3" );
            public OutputDefine MeasuringRow1Cylinder2 { get; set; } = new OutputDefine( 630, 345, 11, XFontStyle.Regular, 20, 20, "F3" );
            public OutputDefine MeasuringRow1Cylinder3 { get; set; } = new OutputDefine( 630, 369, 11, XFontStyle.Regular, 20, 20, "F3" );
            public OutputDefine MeasuringRow1Cylinder4 { get; set; } = new OutputDefine( 630, 393, 11, XFontStyle.Regular, 20, 20, "F3" );
            public OutputDefine MeasuringRow2Cylinder1 { get; set; } = new OutputDefine( 683, 320, 11, XFontStyle.Regular, 20, 20, "F3" );
            public OutputDefine MeasuringRow2Cylinder2 { get; set; } = new OutputDefine( 683, 345, 11, XFontStyle.Regular, 20, 20, "F3" );
            public OutputDefine MeasuringRow2Cylinder3 { get; set; } = new OutputDefine( 683, 369, 11, XFontStyle.Regular, 20, 20, "F3" );
            public OutputDefine MeasuringRow2Cylinder4 { get; set; } = new OutputDefine( 683, 393, 11, XFontStyle.Regular, 20, 20, "F3" );
            public OutputDefine MeasuringRow3Cylinder1 { get; set; } = new OutputDefine( 737, 320, 11, XFontStyle.Regular, 20, 20, "F3" );
            public OutputDefine MeasuringRow3Cylinder2 { get; set; } = new OutputDefine( 737, 345, 11, XFontStyle.Regular, 20, 20, "F3" );
            public OutputDefine MeasuringRow3Cylinder3 { get; set; } = new OutputDefine( 737, 369, 11, XFontStyle.Regular, 20, 20, "F3" );
            public OutputDefine MeasuringRow3Cylinder4 { get; set; } = new OutputDefine( 737, 393, 11, XFontStyle.Regular, 20, 20, "F3" );
            public OutputDefine MeasuringRow4Cylinder1 { get; set; } = new OutputDefine( 791, 320, 11, XFontStyle.Regular, 20, 20, "F3" );
            public OutputDefine MeasuringRow4Cylinder2 { get; set; } = new OutputDefine( 791, 345, 11, XFontStyle.Regular, 20, 20, "F3" );
            public OutputDefine MeasuringRow4Cylinder3 { get; set; } = new OutputDefine( 791, 369, 11, XFontStyle.Regular, 20, 20, "F3" );
            public OutputDefine MeasuringRow4Cylinder4 { get; set; } = new OutputDefine( 791, 393, 11, XFontStyle.Regular, 20, 20, "F3" );
            #endregion

            #region 測定データ3（帳票右下の下）
            public OutputDefine MeasuringOffset1Cylinder1 { get; set; } = new OutputDefine( 630, 477, 11, XFontStyle.Regular, 20, 20, "F3" );
            public OutputDefine MeasuringOffset1Cylinder2 { get; set; } = new OutputDefine( 630, 501, 11, XFontStyle.Regular, 20, 20, "F3" );
            public OutputDefine MeasuringOffset1Cylinder3 { get; set; } = new OutputDefine( 630, 525, 11, XFontStyle.Regular, 20, 20, "F3" );
            public OutputDefine MeasuringOffset1Cylinder4 { get; set; } = new OutputDefine( 630, 550, 11, XFontStyle.Regular, 20, 20, "F3" );
            public OutputDefine MeasuringOffset2Cylinder1 { get; set; } = new OutputDefine( 683, 477, 11, XFontStyle.Regular, 20, 20, "F3" );
            public OutputDefine MeasuringOffset2Cylinder2 { get; set; } = new OutputDefine( 683, 501, 11, XFontStyle.Regular, 20, 20, "F3" );
            public OutputDefine MeasuringOffset2Cylinder3 { get; set; } = new OutputDefine( 683, 525, 11, XFontStyle.Regular, 20, 20, "F3" );
            public OutputDefine MeasuringOffset2Cylinder4 { get; set; } = new OutputDefine( 683, 550, 11, XFontStyle.Regular, 20, 20, "F3" );
            public OutputDefine MeasuringOffset3Cylinder1 { get; set; } = new OutputDefine( 737, 477, 11, XFontStyle.Regular, 20, 20, "F3" );
            public OutputDefine MeasuringOffset3Cylinder2 { get; set; } = new OutputDefine( 737, 501, 11, XFontStyle.Regular, 20, 20, "F3" );
            public OutputDefine MeasuringOffset3Cylinder3 { get; set; } = new OutputDefine( 737, 525, 11, XFontStyle.Regular, 20, 20, "F3" );
            public OutputDefine MeasuringOffset3Cylinder4 { get; set; } = new OutputDefine( 737, 550, 11, XFontStyle.Regular, 20, 20, "F3" );
            public OutputDefine MeasuringOffset4Cylinder1 { get; set; } = new OutputDefine( 791, 477, 11, XFontStyle.Regular, 20, 20, "F3" );
            public OutputDefine MeasuringOffset4Cylinder2 { get; set; } = new OutputDefine( 791, 501, 11, XFontStyle.Regular, 20, 20, "F3" );
            public OutputDefine MeasuringOffset4Cylinder3 { get; set; } = new OutputDefine( 791, 525, 11, XFontStyle.Regular, 20, 20, "F3" );
            public OutputDefine MeasuringOffset4Cylinder4 { get; set; } = new OutputDefine( 791, 550, 11, XFontStyle.Regular, 20, 20, "F3" );
            public OutputDefine Remark { get; set; } = new OutputDefine( 560, 268, 9, XFontStyle.Bold );
            #endregion
        }

        #region プロパティ

        /// <summary>
        /// 出力データ
        /// </summary>
        public EngineInjection03DetailDto TargetSource { get; set; }

        #endregion

        #region フィールド

        //出力位置定義
        private EngineInjection03DetailOutputDefines _outputDefines;

        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public EngineInjection03DetailReport() {
            _outputDefines = new EngineInjection03DetailOutputDefines();
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="measurementDate"></param>
        /// <param name="modelNm"></param>
        /// <param name="serial"></param>
        public EngineInjection03DetailReport( DateTime measuringDate, string modelName, string serial ) {
            TargetSource = ProcessSearchDao.SelectEngineInjection03DetailForPdfOutput( measuringDate, modelName, serial );
            _outputDefines = new EngineInjection03DetailOutputDefines();
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
                PdfReader.Open( WebAppInstance.GetInstance().Config.ApplicationInfo.reportTemplateBasePath + WebAppInstance.GetInstance().Config.ApplicationInfo.repEngineInjection03Detail );

            //メタ情報設定
            document.Info.Title = "噴射時期計測データ";
            document.Info.Subject = $"測定日時:{ TargetSource.MeasuringDate.ToString() },生産型式名:{ TargetSource.ModelName },エンジン刻印名:{ TargetSource.EnginePrint },機番:{ TargetSource.Serial },IDNO:{ TargetSource.Idno }";
            document.Info.Author = WebAppInstance.GetInstance().AppDisplayName;
            document.Info.Keywords = $"{ TargetSource.MeasuringDate.ToString() },{ TargetSource.ModelName },{ TargetSource.EnginePrint },{ TargetSource.Serial },{ TargetSource.Idno }";
            document.Info.Creator = WebAppInstance.GetInstance().AppName;

            //ページレイアウト設定
            PdfPage page = document.Pages[0];
            page.Size = PageSize.A4;
            page.Orientation = PageOrientation.Landscape;

            //PDF書き出し
            PropertyInfo[] defineProps = typeof( EngineInjection03DetailOutputDefines ).GetProperties();
            PropertyInfo[] targetSourceProps = typeof( EngineInjection03DetailDto ).GetProperties();
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