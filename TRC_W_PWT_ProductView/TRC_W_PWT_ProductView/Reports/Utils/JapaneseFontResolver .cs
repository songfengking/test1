using PdfSharp.Fonts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;

namespace TRC_W_PWT_ProductView.Report.Utils {
    // 日本語フォントのためのフォントリゾルバー
    public class JapaneseFontResolver : IFontResolver {
        // IPAゴシック
        private static readonly string IPA_GOTHIC_MEDIUM_TTF =
            "TRC_W_PWT_ProductView.Reports.Fonts.ipaexg.ttf";

        public byte[] GetFont( string faceName ) {
            switch ( faceName ) {
                case "IPAexゴシック":
                    return LoadFontData( IPA_GOTHIC_MEDIUM_TTF );
            }
            return null;
        }

        public FontResolverInfo ResolveTypeface(
                    string familyName, bool isBold, bool isItalic ) {
            var fontName = familyName.ToLower();

            switch ( fontName ) {
                case "ipaexゴシック":
                    return new FontResolverInfo( "IPAexゴシック" );
            }

            // デフォルトのフォント
            return PlatformFontResolver.ResolveTypeface( "MS Gothic", isBold, isItalic );
        }

        // 埋め込みリソースからフォントファイルを読み込む
        private byte[] LoadFontData( string resourceName ) {
            var assembly = Assembly.GetExecutingAssembly();

            using ( Stream stream = assembly.GetManifestResourceStream( resourceName ) ) {
                if ( stream == null ) {
                    throw new ArgumentException( "リソースが見つかりません ⇒" + resourceName );
                }

                int count = ( int )stream.Length;
                byte[] data = new byte[count];
                stream.Read( data, 0, count );
                return data;
            }
        }
    }
}