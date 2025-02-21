namespace Web_QLNS_VTH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Admin")]
    public partial class Admin
    {
        [Key]
        [StringLength(50)]
        public string maTK { get; set; }

        [StringLength(50)]
        public string tenDangNhap { get; set; }

        [StringLength(50)]
        public string matKhau { get; set; }
    }
}
