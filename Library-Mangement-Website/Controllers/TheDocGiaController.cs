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
            db.Database.CommandTimeout = 120;
            var docGias = await db.TheDocGias.ToListAsync();
            return View(docGias);
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "docgia_id,Ten,NgaySinh,Email,Status,LoaiDG")] TheDocGia docGia)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Check NgaySinh có nằm trong phạm vi cho phép SQL Server
                    if (docGia.NgaySinh < new DateTime(1753, 1, 1))
                    {
                        ModelState.AddModelError("NgaySinh", "Ngày sinh không hợp lệ.");
                        return View(docGia);
                    }

                    docGia.NgayLapThe = DateTime.Now;
                    docGia.docgia_id = db.TheDocGias.Any()
                            ? db.TheDocGias.Max(d => d.docgia_id) + 1 : 1;

                    db.TheDocGias.Add(docGia);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException ex)
                {
                    foreach (var validationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            // Ghi log lỗi ra Debug Output
                            System.Diagnostics.Debug.WriteLine($"Property: {validationError.PropertyName} Error: {validationError.ErrorMessage}");
                            // Hiển thị lỗi trên view
                            ModelState.AddModelError(validationError.PropertyName, validationError.ErrorMessage);
                        }
                    }
                }
            }

            return View(docGia);
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
        public async Task<ActionResult> Edit([Bind(Include = "docgia_id,Ten,NgaySinh,Email,Status,LoaiDG")] TheDocGia docGia)
        {
            System.Diagnostics.Debug.WriteLine("Ngày sinh mới: " + docGia.NgaySinh);
            if (ModelState.IsValid)
            {
                docGia.NgayLapThe = DateTime.Now;
                db.Entry(docGia).State = System.Data.Entity.EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(docGia);
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
            TheDocGia docGia = await db.TheDocGias.FindAsync(id);
            db.TheDocGias.Remove(docGia);
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
        // AJAX: Kiểm tra mã độc giả và lấy tên độc giả
        [HttpGet]
        public JsonResult GetDocGiaInfo(int id)
        {
            var docGia = db.TheDocGias.FirstOrDefault(d => d.docgia_id == id);
            if (docGia == null)
            {
                return Json(new { exists = false }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                exists = true,
                ten = docGia.Ten,
                loai = docGia.LoaiDG,
                soPhieuMuon = docGia.PhieuMuons.Count
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
