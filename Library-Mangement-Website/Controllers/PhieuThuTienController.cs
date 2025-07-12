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
    public class PhieuThuTienController : Controller
    {
        private libraryEntities db = new libraryEntities();

        // GET: PhieuThuTien
        public async Task<ActionResult> Index()
        {
            var phieuThuTiens = db.PhieuThuTiens.Include(p => p.Phat);
            return View(await phieuThuTiens.ToListAsync());
        }

        // GET: PhieuThuTien/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhieuThuTien phieuThuTien = await db.PhieuThuTiens.FindAsync(id);
            if (phieuThuTien == null)
            {
                return HttpNotFound();
            }
            return View(phieuThuTien);
        }

        // GET: PhieuThuTien/Create
        public ActionResult Create()
        {
            ViewBag.phat_id = new SelectList(db.Phats, "phat_id", "phat_id");
            return View();
        }

        // POST: PhieuThuTien/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "thutien_id,NgayTra,SoTien,phat_id")] PhieuThuTien phieuThuTien)
        {
            if (ModelState.IsValid)
            {
                db.PhieuThuTiens.Add(phieuThuTien);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.phat_id = new SelectList(db.Phats, "phat_id", "phat_id", phieuThuTien.phat_id);
            return View(phieuThuTien);
        }

        // GET: PhieuThuTien/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhieuThuTien phieuThuTien = await db.PhieuThuTiens.FindAsync(id);
            if (phieuThuTien == null)
            {
                return HttpNotFound();
            }
            ViewBag.phat_id = new SelectList(db.Phats, "phat_id", "phat_id", phieuThuTien.phat_id);
            return View(phieuThuTien);
        }

        // POST: PhieuThuTien/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "thutien_id,NgayTra,SoTien,phat_id")] PhieuThuTien phieuThuTien)
        {
            if (ModelState.IsValid)
            {
                db.Entry(phieuThuTien).State = System.Data.Entity.EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.phat_id = new SelectList(db.Phats, "phat_id", "phat_id", phieuThuTien.phat_id);
            return View(phieuThuTien);
        }

        // GET: PhieuThuTien/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhieuThuTien phieuThuTien = await db.PhieuThuTiens.FindAsync(id);
            if (phieuThuTien == null)
            {
                return HttpNotFound();
            }
            return View(phieuThuTien);
        }

        // POST: PhieuThuTien/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            PhieuThuTien phieuThuTien = await db.PhieuThuTiens.FindAsync(id);
            db.PhieuThuTiens.Remove(phieuThuTien);
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
