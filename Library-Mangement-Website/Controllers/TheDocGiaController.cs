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
    public class TheDocGiaController : Controller
    {
        private libraryEntities db = new libraryEntities();

        // GET: TheDocGia
        public async Task<ActionResult> Index()
        {
            return View(await db.TheDocGias.ToListAsync());
        }

        // GET: TheDocGia/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TheDocGia theDocGia = await db.TheDocGias.FindAsync(id);
            if (theDocGia == null)
            {
                return HttpNotFound();
            }
            return View(theDocGia);
        }

        // GET: TheDocGia/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TheDocGia/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "docgia_id,Ten,NgayLapThe,NgaySinh,Email,Status,Password,LoaiDG")] TheDocGia theDocGia)
        {
            if (ModelState.IsValid)
            {
                db.TheDocGias.Add(theDocGia);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(theDocGia);
        }

        // GET: TheDocGia/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TheDocGia theDocGia = await db.TheDocGias.FindAsync(id);
            if (theDocGia == null)
            {
                return HttpNotFound();
            }
            return View(theDocGia);
        }

        // POST: TheDocGia/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "docgia_id,Ten,NgayLapThe,NgaySinh,Email,Status,Password,LoaiDG")] TheDocGia theDocGia)
        {
            if (ModelState.IsValid)
            {
                db.Entry(theDocGia).State = System.Data.Entity.EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(theDocGia);
        }

        // GET: TheDocGia/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TheDocGia theDocGia = await db.TheDocGias.FindAsync(id);
            if (theDocGia == null)
            {
                return HttpNotFound();
            }
            return View(theDocGia);
        }

        // POST: TheDocGia/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            TheDocGia theDocGia = await db.TheDocGias.FindAsync(id);
            db.TheDocGias.Remove(theDocGia);
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
