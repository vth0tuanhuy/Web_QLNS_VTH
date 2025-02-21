using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Web_QLNS_VTH.Models;

namespace Web_QLNS_VTH.Controllers
{
    public class NhanViensController : Controller
    {
        private ManageController manageController = new ManageController();
        private Model1 db = new Model1();
        //Controller manageC = new ManageController(); 
        // GET: NhanViens
        public List<ChucVu> getCVus()
        {
            TaiKhoan taiKhoan = Session["taikhoan"] as TaiKhoan;
            var cVu = db.ChucVus.ToList();
            if (taiKhoan.loaiTK == "QL")
            {
                cVu = cVu.Where(x => x.maPhongBan == taiKhoan.NhanVien.ChucVu.maPhongBan && x.maChucVu != taiKhoan.NhanVien.maChucVu).ToList();
            }
            return cVu;
        }
        public ActionResult Index()
        {
            TaiKhoan taiKhoan = Session["taikhoan"] as TaiKhoan;
            var nhanViens = db.NhanViens.Include(n => n.ChucVu);
            if (taiKhoan == null)
            {
                return View(nhanViens.ToList());
            }
            else
            {
                if (taiKhoan.loaiTK == "AD")
                {
                    return View(nhanViens.ToList());
                }
                else if (taiKhoan.loaiTK == "QL")
                {
                    nhanViens = nhanViens.Where(x => x.ChucVu.maPhongBan == taiKhoan.NhanVien.ChucVu.maPhongBan && x.maNV != taiKhoan.maNV);
                    return View(nhanViens.ToList());
                }
            }
            return View(nhanViens.ToList());
        }

        // GET: NhanViens/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhanVien nhanVien = db.NhanViens.Find(id);
            if (nhanVien == null)
            {
                return HttpNotFound();
            }
            return View(nhanVien);
        }

        // GET: NhanViens/Create
        public ActionResult Create()
        {
            ViewBag.maChucVu = new SelectList(getCVus(), "maChucVu", "tenChucVu");
            return View();
        }

        // POST: NhanViens/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "hoTen,diaChi,sdt,cccd,mail,ngaySinh,ngayVaoCT,gioiTinh,trinhDo,maChucVu,anh")] NhanVien nhanVien)
        {

            if (ModelState.IsValid)
            {
                nhanVien.anh = "logoft.png";
                var f = Request.Files["FileName"];
                if (f != null && f.ContentLength > 0)
                {
                    var tenfile = System.IO.Path.GetFileName(f.FileName);
                    var duongdan = Path.Combine(Server.MapPath("~/Img"), tenfile);
                    f.SaveAs(duongdan);
                    nhanVien.anh = tenfile;
                }

                bool check = db.NhanViens.FirstOrDefault(x => x.sdt == nhanVien.sdt) == null && db.NhanViens.FirstOrDefault(x => x.cccd == nhanVien.cccd) == null && db.NhanViens.FirstOrDefault(x => x.mail == nhanVien.mail) == null;
                if (check)
                {
                    var maNV = manageController.tuSinhMa(db.NhanViens.Max(x => x.maNV));
                    nhanVien.maNV = maNV.ToString();
                    if (manageController.checkNgaySinh(nhanVien.ngaySinh.Value))
                    {
                        if (manageController.tinhKCNgaySinh(nhanVien.ngayVaoCT.Value, nhanVien.ngaySinh.Value) >= 18)
                        {
                            db.NhanViens.Add(nhanVien);
                            db.SaveChanges();
                            Session["success"] = "Thêm nhân viên thành công";
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            Session["danger"] = "Vui lòng nhập ngày vào công ty phù hợp!";
                            ViewBag.maChucVu = new SelectList(db.ChucVus, "maChucVu", "tenChucVu", nhanVien.maChucVu);
                            return View(nhanVien);
                        }
                    }
                    else
                    {
                        Session["danger"] = manageController.danger;
                        ViewBag.maChucVu = new SelectList(db.ChucVus, "maChucVu", "tenChucVu", nhanVien.maChucVu);
                        return View(nhanVien);
                    }
                }
                else
                {
                    Session["danger"] = "Thông tin về số điện thoại, gmail, căn cước công dân bị trùng!!!!";
                    ViewBag.maChucVu = new SelectList(db.ChucVus, "maChucVu", "tenChucVu", nhanVien.maChucVu);
                    return View(nhanVien);
                }
            }
            ViewBag.maChucVu = new SelectList(getCVus(), "maChucVu", "tenChucVu", nhanVien.maChucVu);
            return View(nhanVien);
        }

        // GET: NhanViens/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhanVien nhanVien = db.NhanViens.Find(id);
            if (nhanVien == null)
            {
                return HttpNotFound();
            }
            ViewBag.maChucVu = new SelectList(getCVus(), "maChucVu", "tenChucVu", nhanVien.maChucVu);
            return View(nhanVien);
        }

        // POST: NhanViens/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "maNV,hoTen,diaChi,sdt,cccd,mail,ngaySinh,ngayVaoCT,gioiTinh,trinhDo,maChucVu,anh")] NhanVien nhanVien)
        {

            if (ModelState.IsValid)
            {
                var d = db.NhanViens.AsNoTracking().SingleOrDefault(x => x.maNV == nhanVien.maNV);
                var f = Request.Files["FileName"];
                if (f != null && f.ContentLength > 0)
                {
                    var tenfile = System.IO.Path.GetFileName(f.FileName);
                    var duongdan = Path.Combine(Server.MapPath("~/Img"), tenfile);
                    f.SaveAs(duongdan);
                    nhanVien.anh = tenfile;
                }
                else
                {
                    nhanVien.anh = d.anh;
                }
                var check = db.NhanViens.Where(x => x.sdt == nhanVien.sdt || x.cccd == nhanVien.cccd || x.mail == nhanVien.mail).ToList();
                if (check.Count() > 1)
                {
                    Session["danger"] = "Thông tin về số điện thoại, gmail, căn cước công dân bị trùng!!!!";
                    ViewBag.maChucVu = new SelectList(db.ChucVus, "maChucVu", "tenChucVu", nhanVien.maChucVu);
                    return View(nhanVien);
                }
                if (manageController.checkNgaySinh(nhanVien.ngaySinh.Value))
                {
                    if (manageController.tinhKCNgaySinh(nhanVien.ngayVaoCT.Value, nhanVien.ngaySinh.Value) >= 18)
                    {
                        var existingNhanVien = db.NhanViens.Find(nhanVien.maNV); // Hoặc sử dụng một phương thức khác để tìm kiếm
                        if (existingNhanVien != null)
                        {
                            var properties = typeof(NhanVien).GetProperties();
                            foreach (var property in properties)
                            {
                                if (property.Name != "maNV") // Bỏ qua thuộc tính Id
                                {
                                    var value = property.GetValue(nhanVien);
                                    property.SetValue(existingNhanVien, value);
                                }
                            }

                            db.Entry(existingNhanVien).State = EntityState.Modified;
                            db.SaveChanges();
                            Session["success"] = "Cập nhật nhân viên thành công";
                            return RedirectToAction("Index");
                        }
                    }
                    else
                    {
                        Session["danger"] = "Vui lòng nhập ngày vào công ty phù hợp!";
                        ViewBag.maChucVu = new SelectList(db.ChucVus, "maChucVu", "tenChucVu", nhanVien.maChucVu);
                        return View(nhanVien);
                    }
                }
                else
                {
                    Session["danger"] = manageController.danger;
                    ViewBag.maChucVu = new SelectList(db.ChucVus, "maChucVu", "tenChucVu", nhanVien.maChucVu);
                    return View(nhanVien);
                }

            }
            ViewBag.maChucVu = new SelectList(getCVus(), "maChucVu", "tenChucVu", nhanVien.maChucVu);
            return View(nhanVien);
        }
        //[HttpPost]
        //public ActionResult Delete(string id)
        //{
        //    var nhanVien = db.NhanViens.Find(id);
        //    if (nhanVien != null)
        //    {
        //        db.NhanViens.Remove(nhanVien);
        //        db.SaveChanges();
        //        return Json(new { success = true });
        //    }
        //    return Json(new { success = false });
        //}
        //GET: NhanViens/Delete/5
        //public ActionResult Delete(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    NhanVien nhanVien = db.NhanViens.Find(id);
        //    if (nhanVien == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(nhanVien);
        //}

        // POST: NhanViens/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            NhanVien nhanVien = db.NhanViens.Find(id);
            db.NhanViens.Remove(nhanVien);
            db.SaveChanges();
            Session["success"] = "Xóa thành công nhân viên với mã: " + nhanVien.maNV;
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
