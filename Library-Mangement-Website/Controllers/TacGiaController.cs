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
    public class TacGiaController : Controller
    {
        private libraryEntities db = new libraryEntities();

        // GET: TacGia
        public async Task<ActionResult> Index()
        {
            return View(await db.TacGias.ToListAsync());
        }

        // GET: TacGia/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TacGia tacGia = await db.TacGias.FindAsync(id);
            if (tacGia == null)
            {
                return HttpNotFound();
            }
            return View(tacGia);
        }

        // GET: TacGia/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TacGia/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "tacgia_id,Ten")] TacGia tacGia)
        {
            if (ModelState.IsValid)
            {
                db.TacGias.Add(tacGia);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(tacGia);
        }

        // GET: TacGia/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TacGia tacGia = await db.TacGias.FindAsync(id);
            if (tacGia == null)
            {
                return HttpNotFound();
            }
            return View(tacGia);
        }

        // POST: TacGia/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "tacgia_id,Ten")] TacGia tacGia)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tacGia).State = System.Data.Entity.EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(tacGia);
        }

        // GET: TacGia/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TacGia tacGia = await db.TacGias.FindAsync(id);
            if (tacGia == null)
            {
                return HttpNotFound();
            }
            return View(tacGia);
        }

        // POST: TacGia/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            TacGia tacGia = await db.TacGias.FindAsync(id);
            db.TacGias.Remove(tacGia);
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
