namespace Web_QLNS_VTH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Luong")]
    public partial class Luong
    {
        [Key]
        [StringLength(50)]
        public string maLuong { get; set; }

        [StringLength(50)]
        [DisplayName("Nhân viên")]

        public string maNV { get; set; }

        [StringLength(50)]
        [DisplayName("Tháng năm")]

        public string thangNam { get; set; }
        [DisplayName("Bậc lương")]

        public decimal? bacLuong { get; set; }
        [DisplayName("Hệ số lương")]

        public decimal? heSoLuong { get; set; }
        [DisplayName("Mức lương")]

        public decimal? mucLuong { get; set; }
        [DisplayName("Phụ cấp")]

        public decimal? phuCap { get; set; }
        [DisplayName("Số ngày công")]

        public int? soNgayCong { get; set; }
        [DisplayName("Số nghỉ không phép")]

        public int? nghiKhongPhep { get; set; }
        [DisplayName("Ứng trước")]

        public decimal? luongUngTruoc { get; set; }
        [DisplayName("Lương nhận")]

        public decimal? luongNhan { get; set; }

        public virtual NhanVien NhanVien { get; set; }
    }
}
