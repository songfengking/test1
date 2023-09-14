using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KTFramework.Common;

namespace TRC_W_PWT_ProductView.Defines.Interface {

    /// <summary>
    /// 詳細用インターフェイス
    /// </summary>
    public interface IDetail {

        /// <summary>
        /// 初期化メソッド
        /// </summary>
        void Initialize();

        /// <summary>
        /// 詳細用必須引き継ぎ情報
        /// </summary>
        ST_DETAIL_PARAM DetailKeyParam { get; set; }

    }

    /// <summary>
    /// 詳細用インターフェイス(部品)
    /// </summary>
    public interface IDetailParts {

        /// <summary>
        /// 初期化メソッド
        /// </summary>
        void Initialize();

        /// <summary>
        /// 詳細用必須引き継ぎ情報(部品)
        /// </summary>
        ST_DETAIL_PARTS_PARAM DetailPartsKeyParam { get; set; }
    }

    /// <summary>
    /// 詳細用必須引き継ぎ情報
    /// </summary>
    public struct ST_DETAIL_PARAM {
        /// <summary>
        /// 製品種別
        /// </summary>
        public string ProductKind;
        /// <summary>
        /// グループコード(工程/部品種別コード)
        /// </summary>
        public string GroupCd;
        /// <summary>
        /// クラスコード(工程区分コード/部品区分コード)
        /// </summary>
        public string ClassCd;

        /// <summary>
        /// 型式コード
        /// </summary>
        public string ModelCd;
        /// <summary>
        /// 生産型式コード
        /// </summary>
        public string ProductModelCd;
        /// <summary>
        /// 国コード
        /// </summary>
        public string CountryCd;
        /// <summary>
        /// 機番
        /// </summary>
        public string Serial;
        /// <summary>
        /// IDNO
        /// </summary>
        public string Idno;
        /// <summary>
        /// 組立パターン
        /// </summary>
        public string AssemblyPatternCd;
        /// <summary>
        /// ラインコード
        /// </summary>
        public string LineCd;
        /// <summary>
        /// 工程コード
        /// </summary>
        public string ProcessCd;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="ProductKind">製品種別</param>
        /// <param name="GroupCd">グループコード</param>
        /// <param name="ClassCd">クラスコード</param>
        /// <param name="ProductModelCd">型式コード</param>
        /// <param name="CountryCd">国コード</param>
        /// <param name="Serial">機番</param>
        /// <param name="Idno">IDNO</param>
        /// <param name="AssemblyPatternCd">組立パターン</param>
        public ST_DETAIL_PARAM( string ProductKind, string GroupCd, string ClassCd, string ModelCd, string ProductModelCd, string CountryCd, string Serial, string Idno, string AssemblyPatternCd,string LineCd,string ProcessCd) {
            this.ProductKind = ProductKind;
            this.GroupCd = GroupCd;
            this.ClassCd = ClassCd;
            this.ModelCd = ModelCd;
            this.ProductModelCd = ProductModelCd;
            this.CountryCd = CountryCd;
            this.Serial = Serial;
            this.Idno = Idno;
            this.AssemblyPatternCd = AssemblyPatternCd;
            this.LineCd = LineCd;
            this.ProcessCd = ProcessCd;
        }
    }


    /// <summary>
    /// 詳細用必須引き継ぎ情報
    /// </summary>
    public struct ST_DETAIL_PARTS_PARAM {
        /// <summary>
        /// 検索対象
        /// </summary>
        public string SearchTarget;
        /// <summary>
        /// 部品種別
        /// </summary>
        public string PartsKind;
        /// <summary>
        /// 工程区分
        /// </summary>
        public string ProcessCd;

        /// <summary>
        /// 型式コード
        /// </summary>
        public string ModelCd;
        /// <summary>
        /// 機番
        /// </summary>
        public string Serial;
        ///// <summary>
        ///// IDNO
        ///// </summary>
        //public string Idno;
        ///// <summary>
        ///// 組立パターン
        ///// </summary>
        //public string AssemblyPatternCd;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="ProductKind">製品種別</param>
        /// <param name="GroupCd">グループコード</param>
        /// <param name="ClassCd">クラスコード</param>
        /// <param name="ModelCd">型式コード</param>
        /// <param name="CountryCd">国コード</param>
        /// <param name="Serial">機番</param>
        /// <param name="Idno">IDNO</param>
        /// <param name="AssemblyPatternCd">組立パターン</param>
        public ST_DETAIL_PARTS_PARAM( string SearchTarget, string PartsKind, string ProcessCd, string ModelCd, string Serial ) {
            this.SearchTarget = SearchTarget;
            this.PartsKind = PartsKind;
            this.ProcessCd = ProcessCd;
            this.ModelCd = ModelCd;
            this.Serial = Serial;
        }
    }
}
