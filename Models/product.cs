//------------------------------------------------------------------------------
// <auto-generated>
//     このコードはテンプレートから生成されました。
//
//     このファイルを手動で変更すると、アプリケーションで予期しない動作が発生する可能性があります。
//     このファイルに対する手動の変更は、コードが再生成されると上書きされます。
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApplicationTest3.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public product()
        {
            this.buy = new HashSet<buy>();
            this.sale = new HashSet<sale>();
        }
    
        public int id { get; set; }
        [Required]
        [Display(Name = "商品コード")]
        public string pcode { get; set; }
        [Required]
        [Display(Name = "商品名")]
        public string name { get; set; }
        [DisplayFormat(DataFormatString = "{0:c}", ApplyFormatInEditMode = false)]
        [Display(Name = "標準販売価格")]
        public Nullable<decimal> value { get; set; }
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Range OverFlow")]
        [Display(Name = "在庫数")]
        public int stok { get; set; }
        public int maker_id { get; set; }
        public Nullable<int> category_id { get; set; }
    　　
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<buy> buy { get; set; }
        public virtual category category { get; set; }
        public virtual maker maker { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<sale> sale { get; set; }
    }
}
