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
    public class YeuCauNPsController : Controller
    {
        private Model1 db = new Model1();

        // GET: YeuCauNPs
        public ActionResult Index()
        {
            var yeuCauNPs = db.YeuCauNPs.Include(y => y.NhanVien);
            return View(yeuCauNPs.ToList());
        }
        public ActionResult XacNhan(string id, string xn)
        {
            var yeucau = db.YeuCauNPs.Find(id);
            var manageController = new ManageController();
            NghiPhep np = new NghiPhep();
            np.maNghiPhep = manageController.tuSinhMa(db.NghiPheps.Max(x => x.maNghiPhep));
            np.maNV = yeucau.maNV;
            np.tuNgay = yeucau.tuNgay;
            np.denNgay = yeucau.denNgay;
            if (xn == "Y")
            {
                db.YeuCauNPs.Find(id).tinhTrang = "Đã chấp nhận";
                db.NghiPheps.Add(np);
                db.SaveChanges();
            }
            else
            {
                db.YeuCauNPs.Find(id).tinhTrang = "Đã từ chối";
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        // GET: YeuCauNPs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            YeuCauNP yeuCauNP = db.YeuCauNPs.Find(id);
            if (yeuCauNP == null)
            {
                return HttpNotFound();
            }
            return View(yeuCauNP);
        }

        // GET: YeuCauNPs/Create
        public ActionResult Create()
        {
            ViewBag.maNV = new SelectList(db.NhanViens, "maNV", "hoTen");
            return View();
        }

        // POST: YeuCauNPs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "maYeuCauNP,maNV,tuNgay,denNgay,tinhTrang")] YeuCauNP yeuCauNP)
        {
            if (ModelState.IsValid)
            {
                db.YeuCauNPs.Add(yeuCauNP);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.maNV = new SelectList(db.NhanViens, "maNV", "hoTen", yeuCauNP.maNV);
            return View(yeuCauNP);
        }

        // GET: YeuCauNPs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            YeuCauNP yeuCauNP = db.YeuCauNPs.Find(id);
            if (yeuCauNP == null)
            {
                return HttpNotFound();
            }
            ViewBag.maNV = new SelectList(db.NhanViens, "maNV", "hoTen", yeuCauNP.maNV);
            return View(yeuCauNP);
        }

        // POST: YeuCauNPs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "maYeuCauNP,maNV,tuNgay,denNgay,tinhTrang")] YeuCauNP yeuCauNP)
        {
            if (ModelState.IsValid)
            {
                db.Entry(yeuCauNP).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.maNV = new SelectList(db.NhanViens, "maNV", "hoTen", yeuCauNP.maNV);
            return View(yeuCauNP);
        }

        // GET: YeuCauNPs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            YeuCauNP yeuCauNP = db.YeuCauNPs.Find(id);
            if (yeuCauNP == null)
            {
                return HttpNotFound();
            }
            return View(yeuCauNP);
        }

        // POST: YeuCauNPs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            YeuCauNP yeuCauNP = db.YeuCauNPs.Find(id);
            db.YeuCauNPs.Remove(yeuCauNP);
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
