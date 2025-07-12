using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Library_Mangement_Website;

namespace Library_Mangement_Website.Controllers
{
    public class PhieuMuonsController : Controller
    {
        private libraryEntities db = new libraryEntities();

        // GET: PhieuMuons
        public async Task<ActionResult> Index()
        {
            var phieuMuons = db.PhieuMuons.Include(p => p.Sach_copy).Include(p => p.TheDocGia);
            return View(await phieuMuons.ToListAsync());
        }

        // GET: PhieuMuons/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhieuMuon phieuMuon = await db.PhieuMuons.FindAsync(id);
            if (phieuMuon == null)
            {
                return HttpNotFound();
            }
            return View(phieuMuon);
        }

        // GET: PhieuMuons/Create
        public ActionResult Create()
        {
            ViewBag.copy_id = new SelectList(db.Sach_copy, "copy_id", "pdf_link");
            ViewBag.docgia_id = new SelectList(db.TheDocGias, "docgia_id", "Ten");
            return View();
        }

        // POST: PhieuMuons/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "pm_id,NgayMuon,NgayTra,docgia_id,copy_id")] PhieuMuon phieuMuon)
        {
            if (ModelState.IsValid)
            {
                db.PhieuMuons.Add(phieuMuon);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.copy_id = new SelectList(db.Sach_copy, "copy_id", "pdf_link", phieuMuon.copy_id);
            ViewBag.docgia_id = new SelectList(db.TheDocGias, "docgia_id", "Ten", phieuMuon.docgia_id);
            return View(phieuMuon);
        }

        // GET: PhieuMuons/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhieuMuon phieuMuon = await db.PhieuMuons.FindAsync(id);
            if (phieuMuon == null)
            {
                return HttpNotFound();
            }
            ViewBag.copy_id = new SelectList(db.Sach_copy, "copy_id", "pdf_link", phieuMuon.copy_id);
            ViewBag.docgia_id = new SelectList(db.TheDocGias, "docgia_id", "Ten", phieuMuon.docgia_id);
            return View(phieuMuon);
        }

        // POST: PhieuMuons/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "pm_id,NgayMuon,NgayTra,docgia_id,copy_id")] PhieuMuon phieuMuon)
        {
            if (ModelState.IsValid)
            {
                db.Entry(phieuMuon).State = System.Data.Entity.EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.copy_id = new SelectList(db.Sach_copy, "copy_id", "pdf_link", phieuMuon.copy_id);
            ViewBag.docgia_id = new SelectList(db.TheDocGias, "docgia_id", "Ten", phieuMuon.docgia_id);
            return View(phieuMuon);
        }

        // GET: PhieuMuons/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhieuMuon phieuMuon = await db.PhieuMuons.FindAsync(id);
            if (phieuMuon == null)
            {
                return HttpNotFound();
            }
            return View(phieuMuon);
        }

        // POST: PhieuMuons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            PhieuMuon phieuMuon = await db.PhieuMuons.FindAsync(id);
            db.PhieuMuons.Remove(phieuMuon);
            await db.SaveChangesAsync();
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
