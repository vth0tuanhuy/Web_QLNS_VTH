using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Web_QLNS_VTH.Models;

namespace Web_QLNS_VTH.Controllers
{
    public class PhongBansController : Controller
    {
        private Model1 db = new Model1();
        ManageController m = new ManageController();
        
        // GET: PhongBans
        public ActionResult Index()
        {
            var PhongB = db.PhongBans.ToList();
            var ChucV = db.ChucVus.ToList();
            var NhanV = db.NhanViens.ToList();
            var phongBans = db.PhongBans.Select(pb => new
            {
                PhongBanId = pb.maPhongBan,
                TenPhong = pb.tenPhongBan,
                SoLuongNhanVien = pb.ChucVus.SelectMany(cv => cv.NhanViens).Count() // Đếm số lượng nhân viên theo chức vụ
            }).ToList();

            List<int> SLNV = new List<int>();
            //return View(phongBans);
            return View(db.PhongBans.ToList());
        }

        // GET: PhongBans/Details/5

        // POST: PhongBans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "tenPhongBan")] PhongBan phongBan)
        {
            
            if (ModelState.IsValid)
            {
                if (!m.CheckProperties(phongBan, "maPhongBan"))
                {
                    Session["danger"] = m.danger;
                    return RedirectToAction("Index");
                }
                if (db.PhongBans.FirstOrDefault(x => x.tenPhongBan == phongBan.tenPhongBan) == null)
                { 
                ManageController manageController = new ManageController();
                phongBan.maPhongBan = manageController.tuSinhMa(db.PhongBans.Max(x=>x.maPhongBan));
                db.PhongBans.Add(phongBan);
                db.SaveChanges();
                return RedirectToAction("Index");}
                Session["danger"] = "Tên phòng ban đã tồn tại";
                return RedirectToAction("Index");
            }

            return View();
        }

        // GET: PhongBans/Edit/5

        // POST: PhongBans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "maPhongBan,tenPhongBan")] PhongBan phongBan)
        {
            if (ModelState.IsValid)
            {
                db.Entry(phongBan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        // GET: PhongBans/Delete/5
        public ActionResult DeleteConfirmed(string id)
        {
            PhongBan phongBan = db.PhongBans.Find(id);
            db.PhongBans.Remove(phongBan);
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
