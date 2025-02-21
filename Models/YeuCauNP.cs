namespace Web_QLNS_VTH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("YeuCauNP")]
    public partial class YeuCauNP
    {
        [Key]
        [StringLength(50)]
        public string maYeuCauNP { get; set; }

        [StringLength(50)]
        public string maNV { get; set; }

        [Column(TypeName = "date")]
        public DateTime? tuNgay { get; set; }

        [Column(TypeName = "date")]
        public DateTime? denNgay { get; set; }

        [StringLength(50)]
        public string tinhTrang { get; set; }

        public virtual NhanVien NhanVien { get; set; }
    }
}
