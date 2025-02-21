namespace Web_QLNS_VTH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TangCa")]
    public partial class TangCa
    {
        [Key]
        [StringLength(50)]
        public string maTangCa { get; set; }

        [StringLength(50)]
        public string maNV { get; set; }

        public int? soGio { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ngayTangCa { get; set; }

        [StringLength(50)]
        public string loaiTangCa { get; set; }

        public virtual NhanVien NhanVien { get; set; }
    }
}
