﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_QLNS_VTH.Models;

namespace Web_QLNS_VTH.Controllers
{
    public class ClientController : Controller
    {
        Model1 db = new Model1();
        ManageController m = new ManageController();
        public TaiKhoan getTK()
        {
            if (Session["taikhoan"] != null)
            {
                return Session["taikhoan"] as TaiKhoan;
            }
            return null;
        }
        
        // GET: Client
        public ActionResult Index()
        {
            ManageController x = new ManageController();
            TaiKhoan tk;
            if (Session["taikhoan"] != null)
            {
                tk = Session["taikhoan"] as TaiKhoan;
                NhanVien nv = db.NhanViens.Find(tk.maNV);
                ViewBag.dt=x.tinhKCNgaySinh(DateTime.Now,(DateTime)nv.ngaySinh);
                return View(nv);
            }
            return HttpNotFound();
        }
        public ActionResult doiMK()
        {
            if (Session["taikhoan"] != null)
            {
                TaiKhoan tk = Session["taikhoan"] as TaiKhoan;
                NhanVien nv = db.NhanViens.Find(tk.maNV);
                ViewBag.anh = tk.NhanVien.anh;
            }
            return View(); }
        [HttpPost]
        public ActionResult doiMK(string oldMK, string newMK)
        {
            TaiKhoan tk = Session["taikhoan"] as TaiKhoan;

            if (tk != null && tk.matKhau == oldMK)
            {
                if (m.checkMK(newMK))
                {
                    // Cập nhật mật khẩu  
                    TaiKhoan tkc = db.TaiKhoans.Find(tk.maTK);
                    db.TaiKhoans.Attach(tkc);
                    tkc.matKhau = newMK;
                    // Lưu các thay đổi vào cơ sở dữ liệu
                    db.Entry(tkc).State = EntityState.Modified;
                    db.SaveChanges();

                    // Chuyển hướng đến trang đăng nhập
                    return RedirectToAction("login", "Manage", null);
                }
                Session["danger"] = m.danger;
            } else Session["danger"] = "Mật khẩu không đúng!";
            if (Session["taikhoan"] != null)
            {
                tk = Session["taikhoan"] as TaiKhoan;
                NhanVien nv = db.NhanViens.Find(tk.maNV);
                ViewBag.anh = tk.NhanVien.anh;
            }

            // Nếu không thành công, trả về view hiện tại
            return View();
        }
        public ActionResult showTC(string id)
        {
            var tangCa = db.TangCas.Where(x => x.maNV == id).ToList();
            return View(tangCa);
        }
        public ActionResult showLuong(string id)
        {
            var luong= db.Luongs.Where(x => x.maNV == id).ToList();
            return View(luong);
        }
        public ActionResult showNghiPhep(string id)
        {
            var np = db.NghiPheps.Where(x => x.maNV == id).ToList();
            return View(np);
        }
        public ActionResult showYeuCauNP(string id)
        {
            var yc = db.YeuCauNPs.Where(x => x.maNV == id).ToList();
            return View(yc);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult showYeuCauNP([Bind(Include = "tuNgay,denNgay")] YeuCauNP yeuCauNP)
        { 
            if (ModelState.IsValid)
            {
                ManageController x = new ManageController();
                TaiKhoan tk;
                if (Session["taikhoan"] != null)
                {
                    tk = Session["taikhoan"] as TaiKhoan;
                    if (m.checkTimePhuHop((DateTime)yeuCauNP.tuNgay, (DateTime)yeuCauNP.denNgay, tk.maNV))
                    {
                        yeuCauNP.maNV = tk.maNV;
                        yeuCauNP.maYeuCauNP = x.tuSinhMa(db.YeuCauNPs.Max(m => m.maYeuCauNP));
                        yeuCauNP.tinhTrang = "Chưa duyệt";
                        db.YeuCauNPs.Add(yeuCauNP);
                        db.SaveChanges();
                        var yc = db.YeuCauNPs.Where(n => n.maNV == tk.maNV).ToList();
                        return View(yc);
                    }
                    Session["danger"] = m.danger;
                    return RedirectToAction("showYeuCauNP","Client",new {id=tk.maNV});
                }
            }
            return View();
        }
    }
}