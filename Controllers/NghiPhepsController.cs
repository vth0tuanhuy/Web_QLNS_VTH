using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Web_QLNS_VTH.Models;

namespace Web_QLNS_VTH.Controllers
{
    public class NghiPhepsController : Controller
    {
        ManageController manageController = new ManageController();
        private Model1 db = new Model1();
        // GET: NghiPheps
        public List<NhanVien> getNVs()
        {
            TaiKhoan taiKhoan = Session["taikhoan"] as TaiKhoan;
            var nv = db.NhanViens.ToList();
            if (taiKhoan.loaiTK == "QL")
            {
                nv = nv.Where(x => x.ChucVu.maPhongBan == taiKhoan.NhanVien.ChucVu.maPhongBan && x.maChucVu != taiKhoan.NhanVien.maChucVu).ToList();
            }
            return nv;
        }
        public ActionResult Index()
        {
            // Sử dụng biến trong controller khác
            TaiKhoan taiKhoan = Session["taikhoan"] as TaiKhoan;
            var nghiPheps = db.NghiPheps.Include(n => n.NhanVien);
            if (taiKhoan != null)
            {
                if (taiKhoan.loaiTK == "AD")
                {
                    ViewBag.maNV = new SelectList(getNVs(), "maNV", "hoTen");
                    ViewBag.NV = db.NhanViens.ToList();
                    return View(nghiPheps.ToList());
                }
                else if (taiKhoan.loaiTK == "QL")
                {
                    ViewBag.maNV = new SelectList(getNVs(), "maNV", "hoTen");
                    ViewBag.NV = db.NhanViens.Where(x => x.ChucVu.maPhongBan == taiKhoan.NhanVien.ChucVu.maPhongBan && x.maNV != taiKhoan.maNV).ToList();
                    nghiPheps = nghiPheps.Where(x => x.NhanVien.ChucVu.maPhongBan == taiKhoan.NhanVien.ChucVu.maPhongBan && x.maNV != taiKhoan.maNV);
                    return View(nghiPheps.ToList());
                }
            }
            return HttpNotFound();
            //return View(nghiPheps.ToList());
        }
        public ActionResult NghiPhepNV(string id)
        {
            var nghiPhep = db.NghiPheps.Where(n => n.maNV == id).ToList();
            ViewBag.maNV = new SelectList(getNVs(), "maNV", "hoTen");
            ViewBag.NVChon = db.NhanViens.Find(id);
            return View(nghiPhep);
        }
        // GET: NghiPheps/Details/

        // POST: NghiPheps/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "maNV,tuNgay,denNgay")] NghiPhep nghiPhep)
        {
            if (ModelState.IsValid)
            {
                if(manageController.checkTimePhuHop((DateTime)nghiPhep.tuNgay, (DateTime)nghiPhep.denNgay, nghiPhep.maNV))
                {
                    nghiPhep.maNghiPhep = manageController.tuSinhMa(db.NghiPheps.Max(x => x.maNghiPhep));
                    db.NghiPheps.Add(nghiPhep);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                Session["danger"] = manageController.danger;
            }

            ViewBag.maNV = new SelectList(db.NhanViens, "maNV", "hoTen", nghiPhep.maNV);
            return View(nghiPhep);
        }

        // GET: NghiPheps/Edit/5
        // POST: NghiPheps/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "maNghiPhep,maNV,tuNgay,denNgay")] NghiPhep nghiPhep)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nghiPhep).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.maNV = new SelectList(db.NhanViens, "maNV", "hoTen", nghiPhep.maNV);
            return View(nghiPhep);
        }

        // GET: NghiPheps/Delete/5
        // POST: NghiPheps/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            NghiPhep nghiPhep = db.NghiPheps.Find(id);
            db.NghiPheps.Remove(nghiPhep);
            db.SaveChanges();
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
