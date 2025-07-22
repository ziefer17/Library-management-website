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
using Library_Mangement_Website.Models;
using OfficeOpenXml;
using System.IO;
using System.Web.UI.WebControls;

namespace Library_Mangement_Website.Controllers
{
    public class PhieuMuonsController : Controller
    {
        private libraryEntities db = new libraryEntities();

        public ActionResult Debt(string searchTerm)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            int id = (int)Session["UserId"];
            var currentDate = DateTime.Today;

            var phatData = db.Phats
                .Include(p => p.PhieuMuon.Sach_copy.Sach.TheLoai)
                .Include(p => p.PhieuMuon.Sach_copy.Sach.TacGias)
                .Include(p => p.PhieuMuon.Sach_copy)
                .Include(p => p.PhieuMuon.TheDocGia)
                .Where(p => p.PhieuMuon.NgayTra != null)
                .Where(p => p.PhieuMuon.TheDocGia.docgia_id == id)
                .ToList();

            

            if (!string.IsNullOrEmpty(searchTerm))
            {
                phatData = phatData
                    .Where(s => s.PhieuMuon.Sach_copy.Sach.TenSach.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0)
                    .ToList();
            }


            var saches = phatData.Select(s => new SachViewModel
            {
                SachId = s.PhieuMuon.Sach_copy.Sach.sach_id,
                TenSach = s.PhieuMuon.Sach_copy.Sach.TenSach,
                Authors = s.PhieuMuon.Sach_copy.Sach.TacGias.Any() ? string.Join(", ", s.PhieuMuon.Sach_copy.Sach.TacGias.Select(t => t.Ten)) : "No Authors",
                Publisher = s.PhieuMuon.Sach_copy.NhaXuatBan != null ? s.PhieuMuon.Sach_copy.NhaXuatBan.Ten : "N/A",
                YearPublished = s.PhieuMuon.Sach_copy.NamXuatBan.HasValue ? s.PhieuMuon.Sach_copy.NamXuatBan.Value.ToString("yyyy") : "N/A",
                DayOvertime = s.PhieuMuon.NgayMuon.HasValue
                    ? (currentDate - s.PhieuMuon.NgayMuon.Value).Days + 12 + " days"
                    : "N/A",
                Debt = (decimal)s.SoTienPhat,
                DateReturned = s.PhieuMuon.NgayTra.HasValue ? s.PhieuMuon.NgayMuon.Value.ToString("dd/MM/yyyy") : "N/A",
            }).ToList();

            var totalDebt = saches.Sum(g => g.Debt);

            ViewBag.TotalDebt = totalDebt;
            ViewBag.SearchTerm = searchTerm;
            

            return View(saches);
        }

        public ActionResult ReportOvertimeBooks(string searchTerm)
        {
            var currentDate = DateTime.Today;


            var sachesData = db.PhieuMuons
                .Include(s => s.Sach_copy.Sach.TheLoai)
                .Include(s => s.Sach_copy.Sach.TacGias)
                .Include(s => s.Sach_copy)
                .Where(s => s.NgayTra == null)
                .ToList();


            if (!string.IsNullOrEmpty(searchTerm))
            {
                sachesData = sachesData
                    .Where(s => s.Sach_copy.Sach.TenSach.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0)
                    .ToList();
            }


            var saches = sachesData.Select(s => new SachViewModel
            {
                SachId = s.Sach_copy.Sach.sach_id,
                TenSach = s.Sach_copy.Sach.TenSach,
                Authors = s.Sach_copy.Sach.TacGias.Any() ? string.Join(", ", s.Sach_copy.Sach.TacGias.Select(t => t.Ten)) : "No Authors",
                Publisher = s.Sach_copy.NhaXuatBan != null ? s.Sach_copy.NhaXuatBan.Ten : "N/A",
                YearPublished = s.Sach_copy.NamXuatBan.HasValue ? s.Sach_copy.NamXuatBan.Value.ToString("yyyy") : "N/A",
                TenDocGia = s.TheDocGia != null ? s.TheDocGia.Ten : "N/A",
                EmailDocGia = s.TheDocGia != null ? s.TheDocGia.Email : "N/A",
                DayOvertime = s.NgayMuon.HasValue
                    ? (currentDate - s.NgayMuon.Value).Days + 12 + " days"
                    : "N/A"
            }).ToList();

            ViewBag.SearchTerm = searchTerm;

            return View(saches);
        }

        public ActionResult ExportToExcelOvertime(string searchTerm)
        {
            var currentDate = DateTime.Today;


            var sachesData = db.PhieuMuons
                .Include(s => s.Sach_copy.Sach.TheLoai)
                .Include(s => s.Sach_copy.Sach.TacGias)
                .Include(s => s.Sach_copy)
                .Where(s => s.NgayTra == null)
                .ToList();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                sachesData = sachesData
                    .Where(s => s.Sach_copy.Sach.TenSach.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0)
                    .ToList();
            }

            var saches = sachesData.Select(s => new SachViewModel
            {
                SachId = s.Sach_copy.Sach.sach_id,
                TenSach = s.Sach_copy.Sach.TenSach,
                Authors = s.Sach_copy.Sach.TacGias.Any() ? string.Join(", ", s.Sach_copy.Sach.TacGias.Select(t => t.Ten)) : "No Authors",
                Publisher = s.Sach_copy.NhaXuatBan != null ? s.Sach_copy.NhaXuatBan.Ten : "N/A",
                YearPublished = s.Sach_copy.NamXuatBan.HasValue ? s.Sach_copy.NamXuatBan.Value.ToString("yyyy") : "N/A",
                TenDocGia = s.TheDocGia != null ? s.TheDocGia.Ten : "N/A",
                EmailDocGia = s.TheDocGia != null ? s.TheDocGia.Email : "N/A",
                DayOvertime = s.NgayMuon.HasValue
                    ? (currentDate - s.NgayMuon.Value).Days + 12 + " days"
                    : "N/A"
            }).ToList();

            ExcelPackage.License.SetNonCommercialPersonal("My Name");
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Overtime rent");


                var currentDateTime = DateTime.Now;
                worksheet.Cells[1, 1].Value = $"Cập nhật lần cuối: {currentDateTime.ToString("hh:mm tt dd/MM/yyyy")} (GMT+7)";
                worksheet.Cells[1, 1].Style.Font.Bold = true;


                worksheet.Cells[3, 1].Value = "Book Name";
                worksheet.Cells[3, 2].Value = "Authors";
                worksheet.Cells[3, 3].Value = "Publisher";
                worksheet.Cells[3, 4].Value = "Year published";
                worksheet.Cells[3, 5].Value = "Reader name";
                worksheet.Cells[3, 6].Value = "Reader email";
                worksheet.Cells[3, 7].Value = "Overtime(days)";


                for (int i = 0; i < saches.Count; i++)
                {
                    worksheet.Cells[i + 4, 1].Value = saches[i].TenSach;
                    worksheet.Cells[i + 4, 2].Value = saches[i].Authors;
                    worksheet.Cells[i + 4, 3].Value = saches[i].Publisher;
                    worksheet.Cells[i + 4, 4].Value = saches[i].YearPublished;
                    worksheet.Cells[i + 4, 5].Value = saches[i].TenDocGia;
                    worksheet.Cells[i + 4, 6].Value = saches[i].EmailDocGia;
                    worksheet.Cells[i + 4, 7].Value = saches[i].DayOvertime;
                }


                worksheet.Cells.AutoFitColumns();

                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;
                string fileName = $"Overtime_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }

        public ActionResult History(string searchTerm)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            int userId = (int)Session["UserId"];
            var currentDate = DateTime.Today;


            var pm = db.PhieuMuons.Include(p => p.Sach_copy).Include(p => p.TheDocGia)
                            .Where(p => p.TheDocGia.docgia_id == userId)
                            .OrderBy(p => p.NgayMuon)
                            .ToList();

            var saches = pm.Select(s => new SachViewModel
            {
                SachId = s.Sach_copy.Sach.sach_id,
                TenSach = s.Sach_copy.Sach.TenSach,
                TheLoaiTen = s.Sach_copy.Sach.TheLoai != null ? s.Sach_copy.Sach.TheLoai.Ten : "N/A",
                Publisher = s.Sach_copy.NhaXuatBan.Ten,
                YearPublished = s.Sach_copy.NamXuatBan.HasValue ? s.Sach_copy.NamXuatBan.Value.ToString("yyyy") : "N/A",
                Authors = s.Sach_copy.Sach.TacGias.Any() ? string.Join(", ", s.Sach_copy.Sach.TacGias.Select(t => t.Ten)) : "No Authors",
                DayBorrowed = s.NgayMuon.HasValue ? s.NgayMuon.Value.ToString("dd/MM/yyyy") : "N/A",
                DateReturned = s.NgayTra.HasValue ? s.NgayMuon.Value.ToString("dd/MM/yyyy") : "N/A",
            }).ToList();

            var totalBorrowed = pm.Count;

            if (!string.IsNullOrEmpty(searchTerm))
            {
                pm = pm
                    .Where(g => g.Sach_copy.Sach.TenSach.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0)
                    .ToList();
            }

            ViewBag.SearchTerm = searchTerm;
            ViewBag.TotalBorrowed = totalBorrowed;
            ViewBag.CurrentDate = currentDate;
            return View(saches);
        }
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
