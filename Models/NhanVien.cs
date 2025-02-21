namespace Web_QLNS_VTH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("NhanVien")]
    public partial class NhanVien
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public NhanVien()
        {
            Luongs = new HashSet<Luong>();
            NghiPheps = new HashSet<NghiPhep>();
            TaiKhoans = new HashSet<TaiKhoan>();
            TangCas = new HashSet<TangCa>();
            YeuCauNPs = new HashSet<YeuCauNP>();
        }

        [Key]
        [StringLength(50)]
        public string maNV { get; set; }
        [DisplayName("Họ và tên")]
        public string hoTen { get; set; }

        [StringLength(300)]
        [DisplayName("Địa chỉ")]
        public string diaChi { get; set; }

        [StringLength(11)]
        [DisplayName("Số điện thoại")]
        public string sdt { get; set; }

        [StringLength(16)]
        [DisplayName("CCCD")]

        public string cccd { get; set; }

        [StringLength(100)]
        [DisplayName("Mail")]

        public string mail { get; set; }

        [Column(TypeName = "date")]
        [DisplayName("Ngày sinh")]
        //[DataType(DataType.Date)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? ngaySinh { get; set; }

        [Column(TypeName = "date")]
        [DisplayName("Ngày vào công ty")]

        public DateTime? ngayVaoCT { get; set; }

        [StringLength(5)]
        [DisplayName("Giới tính")]

        public string gioiTinh { get; set; }

        [StringLength(50)]
        [DisplayName("Trình độ")]

        public string trinhDo { get; set; }

        [StringLength(50)]
        [DisplayName("Chức vụ")]

        public string maChucVu { get; set; }

        [StringLength(200)]
        [DisplayName("Ảnh")]

        public string anh { get; set; }

        public virtual ChucVu ChucVu { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Luong> Luongs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NghiPhep> NghiPheps { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TaiKhoan> TaiKhoans { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TangCa> TangCas { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<YeuCauNP> YeuCauNPs { get; set; }
    }
}
