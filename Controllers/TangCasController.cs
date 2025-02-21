﻿using System;
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
    public class TangCasController : Controller
    {
        private Model1 db = new Model1();
        ManageController manageController = new ManageController();
        // GET: TangCas
        List<SelectListItem> loaiTCList = new List<SelectListItem>
        {
            new SelectListItem { Value = "Lễ", Text = "Ngày lễ" },
            new SelectListItem { Value = "Thường", Text = "Ngày thường" },
        };
        public ActionResult Index()
        {
            ViewBag.maNV = new SelectList(db.NhanViens, "maNV", "hoTen");
            ViewBag.loaiTC = new SelectList(loaiTCList, "Value", "Text");

            var tangCas = db.TangCas.Include(t => t.NhanVien);
            return View(tangCas.ToList());
        }

        // GET: TangCas/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TangCa tangCa = db.TangCas.Find(id);
            if (tangCa == null)
            {
                return HttpNotFound();
            }
            return View(tangCa);
        }

        // GET: TangCas/Create
        public ActionResult Create()
        {
            ViewBag.maNV = new SelectList(db.NhanViens, "maNV", "hoTen");
            return View();
        }

        // POST: TangCas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "maNV,soGio,ngayTangCa,loaiTangCa")] TangCa tangCa)
        {
            if (ModelState.IsValid)
            {
                tangCa.maTangCa = manageController.tuSinhMa(db.TangCas.Max(x => x.maTangCa));
                db.TangCas.Add(tangCa);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.maNV = new SelectList(db.NhanViens, "maNV", "hoTen", tangCa.maNV);
            return View(tangCa);
        }

        // GET: TangCas/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TangCa tangCa = db.TangCas.Find(id);
            if (tangCa == null)
            {
                return HttpNotFound();
            }
            ViewBag.maNV = new SelectList(db.NhanViens, "maNV", "hoTen", tangCa.maNV);
            return View(tangCa);
        }

        // POST: TangCas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "maNV,soGio,ngayTangCa,loaiTangCa")] TangCa tangCa)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tangCa).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.maNV = new SelectList(db.NhanViens, "maNV", "hoTen", tangCa.maNV);
            return View(tangCa);
        }

        // GET: TangCas/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TangCa tangCa = db.TangCas.Find(id);
            if (tangCa == null)
            {
                return HttpNotFound();
            }
            return View(tangCa);
        }

        //// POST: TangCas/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            TangCa tangCa = db.TangCas.Find(id);
            db.TangCas.Remove(tangCa);
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
