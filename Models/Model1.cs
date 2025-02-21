using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace Web_QLNS_VTH.Models
{
    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=Model1")
        {
        }

        public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet<ChucVu> ChucVus { get; set; }
        public virtual DbSet<Luong> Luongs { get; set; }
        public virtual DbSet<NghiPhep> NghiPheps { get; set; }
        public virtual DbSet<NhanVien> NhanViens { get; set; }
        public virtual DbSet<PhongBan> PhongBans { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<TaiKhoan> TaiKhoans { get; set; }
        public virtual DbSet<TangCa> TangCas { get; set; }
        public virtual DbSet<YeuCauNP> YeuCauNPs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admin>()
                .Property(e => e.maTK)
                .IsUnicode(false);

            modelBuilder.Entity<Admin>()
                .Property(e => e.tenDangNhap)
                .IsUnicode(false);

            modelBuilder.Entity<Admin>()
                .Property(e => e.matKhau)
                .IsUnicode(false);

            modelBuilder.Entity<ChucVu>()
                .Property(e => e.maChucVu)
                .IsUnicode(false);

            modelBuilder.Entity<ChucVu>()
                .Property(e => e.maPhongBan)
                .IsUnicode(false);

            modelBuilder.Entity<Luong>()
                .Property(e => e.maLuong)
                .IsUnicode(false);

            modelBuilder.Entity<Luong>()
                .Property(e => e.maNV)
                .IsUnicode(false);

            modelBuilder.Entity<Luong>()
                .Property(e => e.thangNam)
                .IsUnicode(false);

            modelBuilder.Entity<Luong>()
                .Property(e => e.mucLuong)
                .HasPrecision(20, 0);

            modelBuilder.Entity<Luong>()
                .Property(e => e.phuCap)
                .HasPrecision(20, 0);

            modelBuilder.Entity<Luong>()
                .Property(e => e.luongUngTruoc)
                .HasPrecision(20, 0);

            modelBuilder.Entity<Luong>()
                .Property(e => e.luongNhan)
                .HasPrecision(20, 0);

            modelBuilder.Entity<NghiPhep>()
                .Property(e => e.maNghiPhep)
                .IsUnicode(false);

            modelBuilder.Entity<NghiPhep>()
                .Property(e => e.maNV)
                .IsUnicode(false);

            modelBuilder.Entity<NhanVien>()
                .Property(e => e.maNV)
                .IsUnicode(false);

            modelBuilder.Entity<NhanVien>()
                .Property(e => e.sdt)
                .IsUnicode(false);

            modelBuilder.Entity<NhanVien>()
                .Property(e => e.cccd)
                .IsUnicode(false);

            modelBuilder.Entity<NhanVien>()
                .Property(e => e.mail)
                .IsUnicode(false);

            modelBuilder.Entity<NhanVien>()
                .Property(e => e.maChucVu)
                .IsUnicode(false);

            modelBuilder.Entity<NhanVien>()
                .Property(e => e.anh)
                .IsUnicode(false);

            modelBuilder.Entity<PhongBan>()
                .Property(e => e.maPhongBan)
                .IsUnicode(false);

            modelBuilder.Entity<TaiKhoan>()
                .Property(e => e.maTK)
                .IsUnicode(false);

            modelBuilder.Entity<TaiKhoan>()
                .Property(e => e.tenDangNhap)
                .IsUnicode(false);

            modelBuilder.Entity<TaiKhoan>()
                .Property(e => e.matKhau)
                .IsUnicode(false);

            modelBuilder.Entity<TaiKhoan>()
                .Property(e => e.loaiTK)
                .IsUnicode(false);

            modelBuilder.Entity<TaiKhoan>()
                .Property(e => e.maNV)
                .IsUnicode(false);

            modelBuilder.Entity<TangCa>()
                .Property(e => e.maTangCa)
                .IsUnicode(false);

            modelBuilder.Entity<TangCa>()
                .Property(e => e.maNV)
                .IsUnicode(false);

            modelBuilder.Entity<YeuCauNP>()
                .Property(e => e.maYeuCauNP)
                .IsUnicode(false);

            modelBuilder.Entity<YeuCauNP>()
                .Property(e => e.maNV)
                .IsUnicode(false);
        }
    }
}
