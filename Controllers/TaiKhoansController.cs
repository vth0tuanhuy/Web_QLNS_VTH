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
    public class TaiKhoansController : Controller
    {
        List<SelectListItem> loaiTKList = new List<SelectListItem>
        {
            new SelectListItem { Value = "AD", Text = "Admin" },
            new SelectListItem { Value = "QL", Text = "Quản lý" },
            new SelectListItem { Value = "NV", Text = "Nhân viên" }
        };
        private Model1 db = new Model1();
        private ManageController manageController = new ManageController();
        // GET: TaiKhoans
        public ActionResult Index()
        {
            ViewBag.maNV = new SelectList(db.NhanViens, "maNV", "hoTen");
            ViewBag.loaiTK = new SelectList(loaiTKList, "Value", "Text");
            var taiKhoans = db.TaiKhoans.Include(t => t.NhanVien);
            return View(taiKhoans.ToList());
        }

        // GET: TaiKhoans/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaiKhoan taiKhoan = db.TaiKhoans.Find(id);
            if (taiKhoan == null)
            {
                return HttpNotFound();
            }
            return View(taiKhoan);
        }

        // GET: TaiKhoans/Create
        public ActionResult Create()
        {
            
            ViewBag.maNV = new SelectList(db.NhanViens, "maNV", "hoTen");
            return View();
        }

        // POST: TaiKhoans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "tenDangNhap,matKhau,loaiTK,maNV")] TaiKhoan taiKhoan)
        {
            if (ModelState.IsValid)
            {
                taiKhoan.maTK = manageController.tuSinhMa(db.TaiKhoans.Max(x => x.maTK));
                db.TaiKhoans.Add(taiKhoan);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.loaiTK = new SelectList(loaiTKList, "Value", "Text", taiKhoan.loaiTK);
            ViewBag.maNV = new SelectList(db.NhanViens, "maNV", "hoTen", taiKhoan.maNV);
            return View(taiKhoan);
        }

        // GET: TaiKhoans/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaiKhoan taiKhoan = db.TaiKhoans.Find(id);
            if (taiKhoan == null)
            {
                return HttpNotFound();
            }
            ViewBag.maNV = new SelectList(db.NhanViens, "maNV", "hoTen", taiKhoan.maNV);
            return View(taiKhoan);
        }

        // POST: TaiKhoans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "tenDangNhap,matKhau,loaiTK,maNV")] TaiKhoan taiKhoan)
        {
            if (ModelState.IsValid)
            {
                
                db.Entry(taiKhoan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.maNV = new SelectList(db.NhanViens, "maNV", "hoTen", taiKhoan.maNV);
            return View(taiKhoan);
        }

        // GET: TaiKhoans/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaiKhoan taiKhoan = db.TaiKhoans.Find(id);
            if (taiKhoan == null)
            {
                return HttpNotFound();
            }
            return View(taiKhoan);
        }

        // POST: TaiKhoans/Delete/5

        public ActionResult DeleteConfirmed(string id)
        {
            TaiKhoan taiKhoan = db.TaiKhoans.Find(id);
            db.TaiKhoans.Remove(taiKhoan);
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
