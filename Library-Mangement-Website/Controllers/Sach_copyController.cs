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
    public class Sach_copyController : Controller
    {
        private libraryEntities db = new libraryEntities();

        // GET: Sach_copy
        public async Task<ActionResult> Index()
        {
            var sach_copy = db.Sach_copy.Include(s => s.NhaXuatBan).Include(s => s.Sach);
            return View(await sach_copy.ToListAsync());
        }

        // GET: Sach_copy/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sach_copy sach_copy = await db.Sach_copy.FindAsync(id);
            if (sach_copy == null)
            {
                return HttpNotFound();
            }
            return View(sach_copy);
        }

        // GET: Sach_copy/Create
        public ActionResult Create()
        {
            ViewBag.nxb_id = new SelectList(db.NhaXuatBans, "nxb_id", "Ten");
            ViewBag.sach_id = new SelectList(db.Saches, "sach_id", "TenSach");
            return View();
        }

        // POST: Sach_copy/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "copy_id,NamXuatBan,SoLuong,pdf_link,is_removed,sach_id,nxb_id,image")] Sach_copy sach_copy)
        {
            if (ModelState.IsValid)
            {
                db.Sach_copy.Add(sach_copy);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.nxb_id = new SelectList(db.NhaXuatBans, "nxb_id", "Ten", sach_copy.nxb_id);
            ViewBag.sach_id = new SelectList(db.Saches, "sach_id", "TenSach", sach_copy.sach_id);
            return View(sach_copy);
        }

        // GET: Sach_copy/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sach_copy sach_copy = await db.Sach_copy.FindAsync(id);
            if (sach_copy == null)
            {
                return HttpNotFound();
            }
            ViewBag.nxb_id = new SelectList(db.NhaXuatBans, "nxb_id", "Ten", sach_copy.nxb_id);
            ViewBag.sach_id = new SelectList(db.Saches, "sach_id", "TenSach", sach_copy.sach_id);
            return View(sach_copy);
        }

        // POST: Sach_copy/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "copy_id,NamXuatBan,SoLuong,pdf_link,is_removed,sach_id,nxb_id,image")] Sach_copy sach_copy)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sach_copy).State = System.Data.Entity.EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.nxb_id = new SelectList(db.NhaXuatBans, "nxb_id", "Ten", sach_copy.nxb_id);
            ViewBag.sach_id = new SelectList(db.Saches, "sach_id", "TenSach", sach_copy.sach_id);
            return View(sach_copy);
        }

        // GET: Sach_copy/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sach_copy sach_copy = await db.Sach_copy.FindAsync(id);
            if (sach_copy == null)
            {
                return HttpNotFound();
            }
            return View(sach_copy);
        }

        // POST: Sach_copy/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Sach_copy sach_copy = await db.Sach_copy.FindAsync(id);
            db.Sach_copy.Remove(sach_copy);
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
