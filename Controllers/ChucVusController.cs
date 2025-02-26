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
    public class ChucVusController : Controller
    {
        private Model1 db = new Model1();

        // GET: ChucVus
        public ActionResult Index()
        {
            //Session["Msg"] = null;
            ViewBag.PBan = db.PhongBans.ToList();
            //ViewBag.PBan1 = db.PhongBans.ToList();
            ViewBag.maPhongBan = new SelectList(db.PhongBans, "maPhongBan", "tenPhongBan");
            var chucVus = db.ChucVus.Include(c => c.PhongBan);
            return View(chucVus.ToList());
        }

        // GET: ChucVus/Details/5

        // POST: ChucVus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "tenChucVu,maPhongBan")] ChucVu chucVu)
        {
            if (ModelState.IsValid)
            {
                if(db.ChucVus.FirstOrDefault(x=>x.tenChucVu == chucVu.tenChucVu) == null) { 
                var manageController = new ManageController();
                var maNV = manageController.tuSinhMa(db.ChucVus.Max(x => x.maChucVu));
                Debug.WriteLine(maNV.ToString());
                chucVu.maChucVu = maNV.ToString();
                db.ChucVus.Add(chucVu);
                db.SaveChanges();
                Session["Msg"] = "Thêm mới phúc";
                return RedirectToAction("Index");
                }
                Session["danger"] = "Tên chức vụ bị trùng!!";
                ViewBag.maPhongBan = new SelectList(db.PhongBans, "maPhongBan", "tenPhongBan", chucVu.maPhongBan);
                return View(chucVu);
            }
            Session["danger"] = "Error";
            ViewBag.maPhongBan = new SelectList(db.PhongBans, "maPhongBan", "tenPhongBan", chucVu.maPhongBan);
            return View(chucVu);
        }

        // GET: ChucVus/Edit/5
        // POST: ChucVus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "maChucVu,tenChucVu,maPhongBan")] ChucVu chucVu)
        {
            if (ModelState.IsValid)
            {
                if (db.ChucVus.FirstOrDefault(x => x.tenChucVu == chucVu.tenChucVu) == null)
                {
                    Session["Msg"] = "Thêm mới phúc";
                    db.Entry(chucVu).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                Session["danger"] = "Tên chức vụ bị trùng!!";
                ViewBag.maPhongBan = new SelectList(db.PhongBans, "maPhongBan", "tenPhongBan", chucVu.maPhongBan);
                return View(chucVu);
            }
            ViewBag.maPhongBan = new SelectList(db.PhongBans, "maPhongBan", "tenPhongBan", chucVu.maPhongBan);
            return View(chucVu);
        }

        // GET: ChucVus/Delete/5
        // POST: ChucVus/Delete/5

        public ActionResult DeleteConfirmed(string id)
        {
            ChucVu chucVu = db.ChucVus.Find(id);
            db.ChucVus.Remove(chucVu);
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
