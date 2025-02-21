using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Web_QLNS_VTH.Models;

namespace Web_QLNS_VTH.Controllers
{
    public class LuongsController : Controller
    {
        public Model1 db = new Model1();
        ManageController manageController = new ManageController();

        // GET: Luongs
        public JsonResult GetEmployeeData(string maNV)
        {
            var employee = db.NhanViens.FirstOrDefault(e => e.maNV == maNV);
            if (employee != null)
            {
                return Json(employee, JsonRequestBehavior.AllowGet);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Index()
        {
            var luongs = db.Luongs.Include(l => l.NhanVien);
            return View(luongs.ToList());
        }

        // GET: Luongs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Luong luong = db.Luongs.Find(id);
            ViewBag.PCCV = manageController.tinhPhuCapCV(luong.maNV);
            var parts = luong.thangNam.Split('-');
            int year = int.Parse(parts[0]); // Năm
            int month = int.Parse(parts[1]); // Tháng
            ViewBag.PCTN = manageController.tinhPhuCapThamNien(luong.maNV, month, year);
            if (luong == null)
            {
                return HttpNotFound();
            }
            return View(luong);
        }

        // GET: Luongs/Create
        public ActionResult Create()
        {
            var list = db.NhanViens.ToList();
            ViewBag.NV = list;
            ViewBag.maNV = new SelectList(db.NhanViens, "maNV", "maNV");
            return View();
        }

        // POST: Luongs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "maNV,thangNam,phuCap,nghiKhongPhep,luongUngTruoc")] Luong luong)
        {
            if (ModelState.IsValid)
            {
                luong.maLuong = manageController.tuSinhMa(db.Luongs.Max(x => x.maLuong)).ToString();
                luong.maNV = luong.maNV.ToString();
                var parts = luong.thangNam.Split('-');
                int year = int.Parse(parts[0]); // Năm
                int month = int.Parse(parts[1]); // Tháng
                luong.bacLuong = manageController.xetBLTrinhDo(luong.maNV).Item1;
                Debug.WriteLine(manageController.xetBLTrinhDo(luong.maNV).Item1.ToString());
                Debug.WriteLine(manageController.layHeSoLuong(luong.maNV, month, year).ToString());
                luong.heSoLuong = manageController.layHeSoLuong(luong.maNV, month, year);
                luong.mucLuong = manageController.mucLuong(luong.maNV, month, year);
                luong.phuCap = manageController.tinhPhuCap(luong.maNV, month, year);
                luong.soNgayCong = manageController.soNgayCong(luong.maNV, month, year, (int)luong.nghiKhongPhep);
                luong.luongNhan = manageController.luongNhan(luong.maNV, month, year, (int)luong.nghiKhongPhep, (decimal)luong.luongUngTruoc);
                //luong.mucLuong = Decimal.Truncate(manageController.mucLuong(luong.maNV, month, year));
                //luong.phuCap = Decimal.Truncate(manageController.tinhPhuCap(luong.maNV, month, year));
                //luong.soNgayCong = manageController.soNgayCong(luong.maNV, month, year, (int)luong.nghiKhongPhep);
                //luong.luongNhan = Decimal.Truncate(manageController.luongNhan(luong.maNV, month, year, (int)luong.nghiKhongPhep, (decimal)luong.luongUngTruoc));
                db.Luongs.Add(luong);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.maNV = new SelectList(db.NhanViens, "maNV", "hoTen", luong.maNV);
            return View(luong);
        }
        //public ActionResult Create([Bind(Include = "maLuong,maNV,thangNam,bacLuong,heSoLuong,mucLuong,phuCap,soNgayCong,nghiKhongPhep,luongUngTruoc,luongNhan")] Luong luong)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Luongs.Add(luong);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.maNV = new SelectList(db.NhanViens, "maNV", "hoTen", luong.maNV);
        //    return View(luong);
        //}
        // GET: Luongs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Luong luong = db.Luongs.Find(id);
            if (luong == null)
            {
                return HttpNotFound();
            }
            //ViewBag.maNV = new SelectList(db.NhanViens, "maNV", "hoTen", luong.maNV);
            return View(luong);
        }

        // POST: Luongs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "maLuong,maNV,thangNam,phuCap,nghiKhongPhep,luongUngTruoc")] Luong luong)
        {
            if (ModelState.IsValid)
            {
                luong.maLuong = luong.maLuong;
                var parts = luong.thangNam.Split('-');
                int year = int.Parse(parts[0]); // Năm
                int month = int.Parse(parts[1]); // Tháng
                luong.bacLuong = manageController.xetBLTrinhDo(luong.maNV).Item1;
                luong.heSoLuong = manageController.layHeSoLuong(luong.maNV, month, year);
                luong.mucLuong = manageController.mucLuong(luong.maNV, month, year);
                luong.phuCap = manageController.tinhPhuCap(luong.maNV, month, year);
                luong.soNgayCong = manageController.soNgayCong(luong.maNV, month, year, (int)luong.nghiKhongPhep);
                luong.luongNhan = manageController.luongNhan(luong.maNV, month, year, (int)luong.nghiKhongPhep, (decimal)luong.luongUngTruoc);
                var existing = db.Luongs.Find(luong.maLuong);
                if (existing != null)
                {
                    var properties = typeof(Luong).GetProperties();
                    foreach (var property in properties)
                    {
                        if (property.Name != "maLuong") // Bỏ qua thuộc tính Id
                        {
                            var value = property.GetValue(luong);
                            property.SetValue(existing, value);
                        }
                    }

                    db.Entry(existing).State = EntityState.Modified;
                    db.SaveChanges();
                    Session["success"] = "Cập nhật nhân viên thành công";
                    return RedirectToAction("Index");
                }
                //db.Entry(luong).State = EntityState.Modified;
                //db.SaveChanges();
                //return RedirectToAction("Index");
            }
            return HttpNotFound();
        }

        // GET: Luongs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Luong luong = db.Luongs.Find(id);
            if (luong == null)
            {
                return HttpNotFound();
            }
            return View(luong);
        }

        // POST: Luongs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Luong luong = db.Luongs.Find(id);
            db.Luongs.Remove(luong);
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
