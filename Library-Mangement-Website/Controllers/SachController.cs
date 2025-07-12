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
    public class SachController : Controller
    {
        private libraryEntities db = new libraryEntities();

        // GET: Sach
        public async Task<ActionResult> Index()
        {
            var saches = db.Saches.Include(s => s.TheLoai);
            return View(await saches.ToListAsync());
        }

        // GET: Sach/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sach sach = await db.Saches.FindAsync(id);
            if (sach == null)
            {
                return HttpNotFound();
            }
            return View(sach);
        }

        // GET: Sach/Create
        public ActionResult Create()
        {
            ViewBag.theloai_id = new SelectList(db.TheLoais, "theloai_id", "Ten");
            return View();
        }

        // POST: Sach/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "sach_id,TenSach,ThongTin,theloai_id")] Sach sach)
        {
            if (ModelState.IsValid)
            {
                db.Saches.Add(sach);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.theloai_id = new SelectList(db.TheLoais, "theloai_id", "Ten", sach.theloai_id);
            return View(sach);
        }

        // GET: Sach/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sach sach = await db.Saches.FindAsync(id);
            if (sach == null)
            {
                return HttpNotFound();
            }
            ViewBag.theloai_id = new SelectList(db.TheLoais, "theloai_id", "Ten", sach.theloai_id);
            return View(sach);
        }

        // POST: Sach/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "sach_id,TenSach,ThongTin,theloai_id")] Sach sach)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sach).State = System.Data.Entity.EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.theloai_id = new SelectList(db.TheLoais, "theloai_id", "Ten", sach.theloai_id);
            return View(sach);
        }

        // GET: Sach/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sach sach = await db.Saches.FindAsync(id);
            if (sach == null)
            {
                return HttpNotFound();
            }
            return View(sach);
        }

        // POST: Sach/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Sach sach = await db.Saches.FindAsync(id);
            db.Saches.Remove(sach);
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
