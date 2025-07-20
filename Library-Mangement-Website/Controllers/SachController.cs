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

namespace Library_Mangement_Website.Controllers
{
    public class SachController : Controller
    {
        private libraryEntities db = new libraryEntities();

        public ActionResult RemainingBooks(string searchTerm)
        {
            var sachesData = db.Saches
                .Include(s => s.TheLoai)
                .Include(s => s.TacGias)
                .Include(s => s.Sach_copy)
                .ToList();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                sachesData = sachesData
                    .Where(s => s.TenSach.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0)
                    .ToList();
            }


            var saches = sachesData.Select(s => new SachViewModel
            {
                SachId = s.sach_id,
                TenSach = s.TenSach,
                ThongTin = s.ThongTin,
                TheLoaiTen = s.TheLoai != null ? s.TheLoai.Ten : "N/A",
                Authors = s.TacGias.Any() ? string.Join(", ", s.TacGias.Select(t => t.Ten)) : "No Authors",
                TotalCopies = s.Sach_copy.Sum(sc => sc.SoLuong ?? 0)
            }).ToList();

            ViewBag.SearchTerm = searchTerm;
            return View(saches);
        }

        public ActionResult ExportToExcelRemaining(string searchTerm)
        {
            
            var sachesData = db.Saches
                .Include(s => s.TheLoai)
                .Include(s => s.TacGias)
                .Include(s => s.Sach_copy)
                .ToList();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                sachesData = sachesData
                    .Where(s => s.TenSach.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0)
                    .ToList();
            }

            var saches = sachesData.Select(s => new SachViewModel
            {
                SachId = s.sach_id,
                TenSach = s.TenSach,
                ThongTin = s.ThongTin,
                TheLoaiTen = s.TheLoai != null ? s.TheLoai.Ten : "N/A",
                Authors = s.TacGias.Any() ? string.Join(", ", s.TacGias.Select(t => t.Ten)) : "No Authors",
                TotalCopies = s.Sach_copy.Sum(sc => sc.SoLuong ?? 0)
            }).ToList();

            // Create Excel file
            ExcelPackage.License.SetNonCommercialPersonal("My Name");
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Remaining in");

                var currentDateTime = DateTime.Now;
                worksheet.Cells[1, 1].Value = $"Cập nhật lần cuối: {currentDateTime.ToString("hh:mm tt dd/MM/yyyy")} (GMT+7)";
                worksheet.Cells[1, 1].Style.Font.Bold = true;

                
                worksheet.Cells[3, 1].Value = "Book Name";
                worksheet.Cells[3, 2].Value = "Genre";
                worksheet.Cells[3, 3].Value = "Authors";
                worksheet.Cells[3, 4].Value = "Total Copies";

                
                for (int i = 0; i < saches.Count; i++)
                {
                    worksheet.Cells[i + 4, 1].Value = saches[i].TenSach;
                    worksheet.Cells[i + 4, 2].Value = saches[i].TheLoaiTen;
                    worksheet.Cells[i + 4, 3].Value = saches[i].Authors;
                    worksheet.Cells[i + 4, 4].Value = saches[i].TotalCopies;
                }

                
                worksheet.Cells.AutoFitColumns();

                
                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;
                string fileName = $"RemainingBooks_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }



        // GET: Sach
        public ActionResult Index(string searchTerm)
        {
            var sachesData = db.Saches
                .Include(s => s.TheLoai)
                .Include(s => s.TacGias)
                .Include(s => s.Sach_copy)
                .ToList();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                sachesData = sachesData
                    .Where(s => s.TenSach.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0)
                    .ToList();
            }


            var saches = sachesData.Select(s => new SachViewModel
            {
                SachId = s.sach_id,
                TenSach = s.TenSach,
                ThongTin = s.ThongTin,
                TheLoaiTen = s.TheLoai != null ? s.TheLoai.Ten : "N/A",
                Authors = s.TacGias.Any() ? string.Join(", ", s.TacGias.Select(t => t.Ten)) : "No Authors",
                TotalCopies = s.Sach_copy.Sum(sc => sc.SoLuong ?? 0)
            }).ToList();

            ViewBag.SearchTerm = searchTerm;
            return View(saches);
        }

        public ActionResult ExportToExcel(string searchTerm)
        {
            
            var sachesData = db.Saches
                .Include(s => s.TheLoai)
                .Include(s => s.TacGias)
                .Include(s => s.Sach_copy)
                .ToList();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                sachesData = sachesData
                    .Where(s => s.TenSach.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0)
                    .ToList();
            }

            var saches = sachesData.Select(s => new SachViewModel
            {
                SachId = s.sach_id,
                TenSach = s.TenSach,
                ThongTin = s.ThongTin,
                TheLoaiTen = s.TheLoai != null ? s.TheLoai.Ten : "N/A",
                Authors = s.TacGias.Any() ? string.Join(", ", s.TacGias.Select(t => t.Ten)) : "No Authors",
                TotalCopies = s.Sach_copy.Sum(sc => sc.SoLuong ?? 0)
            }).ToList();

            // Create Excel file
            ExcelPackage.License.SetNonCommercialPersonal("My Name");
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Books");
                worksheet.Cells[1, 1].Value = "Book Name";
                worksheet.Cells[1, 2].Value = "Genre";
                worksheet.Cells[1, 3].Value = "Authors";
                worksheet.Cells[1, 4].Value = "Total Copies";

                // Add data 
                for (int i = 0; i < saches.Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = saches[i].TenSach;
                    worksheet.Cells[i + 2, 2].Value = saches[i].TheLoaiTen;
                    worksheet.Cells[i + 2, 3].Value = saches[i].Authors;
                    worksheet.Cells[i + 2, 4].Value = saches[i].TotalCopies;
                }

                
                worksheet.Cells.AutoFitColumns();

                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;
                string fileName = $"Books_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }

        public ActionResult RemovedBooks(string searchTerm)
        {
            // Get current date (date only, no time)
            var currentDate = DateTime.Today;


            var sachesData = db.Saches
                .Include(s => s.TheLoai)
                .Include(s => s.TacGias)
                .Include(s => s.Sach_copy)
                .Where(s => s.Sach_copy.Any(sc =>
                    sc.is_removed == true))
                .ToList();


            if (!string.IsNullOrEmpty(searchTerm))
            {
                sachesData = sachesData
                    .Where(s => s.TenSach.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0)
                    .ToList();
            }


            var saches = sachesData.Select(s => new SachViewModel
            {
                SachId = s.sach_id,
                TenSach = s.TenSach,
                ThongTin = s.ThongTin,
                TheLoaiTen = s.TheLoai != null ? s.TheLoai.Ten : "N/A",
                Authors = s.TacGias.Any() ? string.Join(", ", s.TacGias.Select(t => t.Ten)) : "No Authors",
                Publisher = s.Sach_copy.Any() ? s.Sach_copy.First().NhaXuatBan?.Ten : "N/A",
                YearPublished = s.Sach_copy.Any(sc => sc.NamXuatBan != null)
                    ? s.Sach_copy.Where(sc => sc.NamXuatBan != null)
                        .Min(sc => sc.NamXuatBan.Value).ToString("yyyy")
                    : "N/A",
            }).ToList();

            // Store search term in ViewBag for the view
            ViewBag.SearchTerm = searchTerm;

            return View(saches);
        }

        public ActionResult ExportToExcelRemovedBooks(string searchTerm)
        {

            var currentDate = DateTime.Today;


            var sachesData = db.Saches
                .Include(s => s.TheLoai)
                .Include(s => s.TacGias)
                .Include(s => s.Sach_copy)
                .Where(s => s.Sach_copy.Any(sc =>
                    sc.is_removed == true))
                .ToList();

            // Apply search filter if provided
            if (!string.IsNullOrEmpty(searchTerm))
            {
                sachesData = sachesData
                    .Where(s => s.TenSach.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0)
                    .ToList();
            }

            // Transform to SachViewModel
            var saches = sachesData.Select(s => new SachViewModel
            {
                SachId = s.sach_id,
                TenSach = s.TenSach,
                ThongTin = s.ThongTin,
                TheLoaiTen = s.TheLoai != null ? s.TheLoai.Ten : "N/A",
                Authors = s.TacGias.Any() ? string.Join(", ", s.TacGias.Select(t => t.Ten)) : "No Authors",
                Publisher = s.Sach_copy.Any() ? s.Sach_copy.First().NhaXuatBan?.Ten : "N/A",
                YearPublished = s.Sach_copy.Any(sc => sc.NamXuatBan != null)
                    ? s.Sach_copy.Where(sc => sc.NamXuatBan != null)
                        .Min(sc => sc.NamXuatBan.Value).ToString("yyyy")
                    : "N/A",
            }).ToList();

            ExcelPackage.License.SetNonCommercialPersonal("My Name");
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("ReturnedBooks");

                // Add CurrentDateTime
                var currentDateTime = DateTime.Now;
                worksheet.Cells[1, 1].Value = $"Cập nhật lần cuối: {currentDateTime.ToString("hh:mm tt dd/MM/yyyy")} (GMT+7)";
                worksheet.Cells[1, 1].Style.Font.Bold = true;

                // Add table headers
                worksheet.Cells[3, 1].Value = "Book Name";
                worksheet.Cells[3, 2].Value = "Genre";
                worksheet.Cells[3, 3].Value = "Authors";
                worksheet.Cells[3, 4].Value = "Publisher";
                worksheet.Cells[3, 5].Value = "Year published";

                // Add data
                for (int i = 0; i < saches.Count; i++)
                {
                    worksheet.Cells[i + 4, 1].Value = saches[i].TenSach;
                    worksheet.Cells[i + 4, 2].Value = saches[i].TheLoaiTen;
                    worksheet.Cells[i + 4, 3].Value = saches[i].Authors;
                    worksheet.Cells[i + 4, 4].Value = saches[i].Publisher;
                    worksheet.Cells[i + 4, 5].Value = saches[i].YearPublished;
                }

                // Auto-fit columns
                worksheet.Cells.AutoFitColumns();

                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;
                string fileName = $"RemovedBooks_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }

        public ActionResult ReportReturnedBooks(string searchTerm)
        {
            // Get current date (date only, no time)
            var currentDate = DateTime.Today; 

            
            var sachesData = db.Saches
                .Include(s => s.TheLoai)
                .Include(s => s.TacGias)
                .Include(s => s.Sach_copy)
                .Where(s => s.Sach_copy.Any(sc =>
                    sc.PhieuMuons.Any(pm =>
                        pm.NgayTra != null &&
                        DbFunctions.TruncateTime(pm.NgayTra) == currentDate)))
                .ToList();

            
            if (!string.IsNullOrEmpty(searchTerm))
            {
                sachesData = sachesData
                    .Where(s => s.TenSach.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0)
                    .ToList();
            }

            
            var saches = sachesData.Select(s => new SachViewModel
            {
                SachId = s.sach_id,
                TenSach = s.TenSach,
                ThongTin = s.ThongTin,
                TheLoaiTen = s.TheLoai != null ? s.TheLoai.Ten : "N/A",
                Authors = s.TacGias.Any() ? string.Join(", ", s.TacGias.Select(t => t.Ten)) : "No Authors",
                Publisher = s.Sach_copy.Any() ? s.Sach_copy.First().NhaXuatBan?.Ten : "N/A",
                YearPublished = s.Sach_copy.Any(sc => sc.NamXuatBan != null)
                    ? s.Sach_copy.Where(sc => sc.NamXuatBan != null)
                        .Min(sc => sc.NamXuatBan.Value).ToString("yyyy")
                    : "N/A",
            }).ToList();

            // Store search term in ViewBag for the view
            ViewBag.SearchTerm = searchTerm;

            return View(saches);
        }

        

        public ActionResult ExportToExcelReturnedBooks(string searchTerm)
        {
            
            var currentDate = DateTime.Today;

            
            var sachesData = db.Saches
                .Include(s => s.TheLoai)
                .Include(s => s.TacGias)
                .Include(s => s.Sach_copy)
                .Where(s => s.Sach_copy.Any(sc =>
                    sc.PhieuMuons.Any(pm =>
                        pm.NgayTra != null &&
                        DbFunctions.TruncateTime(pm.NgayTra) == currentDate)))
                .ToList();

            // Apply search filter if provided
            if (!string.IsNullOrEmpty(searchTerm))
            {
                sachesData = sachesData
                    .Where(s => s.TenSach.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0)
                    .ToList();
            }

            // Transform to SachViewModel
            var saches = sachesData.Select(s => new SachViewModel
            {
                SachId = s.sach_id,
                TenSach = s.TenSach,
                ThongTin = s.ThongTin,
                TheLoaiTen = s.TheLoai != null ? s.TheLoai.Ten : "N/A",
                Authors = s.TacGias.Any() ? string.Join(", ", s.TacGias.Select(t => t.Ten)) : "No Authors",
                Publisher = s.Sach_copy.Any() ? s.Sach_copy.First().NhaXuatBan?.Ten : "N/A",
                YearPublished = s.Sach_copy.Any(sc => sc.NamXuatBan != null)
                    ? s.Sach_copy.Where(sc => sc.NamXuatBan != null)
                        .Min(sc => sc.NamXuatBan.Value).ToString("yyyy")
                    : "N/A",
            }).ToList();

            ExcelPackage.License.SetNonCommercialPersonal("My Name");
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("ReturnedBooks");

                // Add CurrentDateTime
                var currentDateTime = DateTime.Now;
                worksheet.Cells[1, 1].Value = $"Cập nhật lần cuối: {currentDateTime.ToString("hh:mm tt dd/MM/yyyy")} (GMT+7)";
                worksheet.Cells[1, 1].Style.Font.Bold = true;

                // Add table headers
                worksheet.Cells[3, 1].Value = "Book Name";
                worksheet.Cells[3, 2].Value = "Genre";
                worksheet.Cells[3, 3].Value = "Authors";
                worksheet.Cells[3, 4].Value = "Publisher";
                worksheet.Cells[3, 5].Value = "Year published";

                // Add data
                for (int i = 0; i < saches.Count; i++)
                {
                    worksheet.Cells[i + 4, 1].Value = saches[i].TenSach;
                    worksheet.Cells[i + 4, 2].Value = saches[i].TheLoaiTen;
                    worksheet.Cells[i + 4, 3].Value = saches[i].Authors;
                    worksheet.Cells[i + 4, 4].Value = saches[i].Publisher;
                    worksheet.Cells[i + 4, 5].Value = saches[i].YearPublished;
                }

                // Auto-fit columns
                worksheet.Cells.AutoFitColumns();

                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;
                string fileName = $"ReturnedBooks_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
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
