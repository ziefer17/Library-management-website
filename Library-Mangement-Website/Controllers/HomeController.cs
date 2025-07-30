using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using PagedList;
using System.Web.Razor.Tokenizer.Symbols;
using Library_Mangement_Website.Models;


namespace Library_Mangement_Website.Controllers
{
    public class HomeController : Controller
    {
        private libraryEntities db = new libraryEntities();
        private string defaultImage = "https://raw.githubusercontent.com/ziefer17/PDFStorage/main/Images/Default_book_cover.png";
        public ActionResult HomeMain(string keyword, int? page)
        {
            int pageSize = 8;
            int pageNumber = page ?? 1;

            var sachs = db.Saches.Include(s => s.TheLoai).Include(s => s.TacGias);

            if (!string.IsNullOrEmpty(keyword))
            {
                keyword = keyword.ToLower();
                sachs = sachs.Where(s =>
                    s.TenSach.ToLower().Contains(keyword) ||
                    s.TheLoai.Ten.ToLower().Contains(keyword) ||
                    s.TacGias.Any(tg => tg.Ten.ToLower().Contains(keyword))
                );
            }

            var pagedSachs = sachs.OrderBy(s => s.TenSach).ToPagedList(pageNumber, pageSize);

            ViewBag.Keyword = keyword;
            return View(pagedSachs);
        }

        public JsonResult Suggest(string term)
        {
            var result = db.Saches
                .Where(s => s.TenSach.Contains(term))
                .OrderBy(s => s.TenSach.StartsWith(term) ? 0 : 1) // Ưu tiên khớp đầu
                .Take(10)
                .Select(s => new
                {
                    label = s.TenSach,
                    value = s.TenSach,
                    id = s.sach_id,
                    theloai = s.TheLoai.Ten,
                    tacgia = s.TacGias.Select(t => t.Ten).FirstOrDefault(),
                })
                .ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        
        public ActionResult ReaderMenu()
        {
            return View();
        }
        public ActionResult ReportMenu()
        {
            return View();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        //Chi tiết sách
        public ActionResult ChiTiet(int? id)
        {
            if (id == null)
                return HttpNotFound();

            var sachCopy = db.Sach_copy
                .Include(s => s.Sach.TacGias)
                .Include(s => s.Sach.TheLoai)
                .Include(s => s.Sach)
                .Include(s => s.NhaXuatBan)
                .FirstOrDefault(s=>s.sach_id ==id);

            if (sachCopy == null)
                return HttpNotFound();

            string imageUrl = string.IsNullOrEmpty(sachCopy.image)
                ? defaultImage
                : sachCopy.image;

            var sachCopies = db.Sach_copy
                .Include(s => s.NhaXuatBan)
                .Where(s => s.sach_id == sachCopy.sach_id)
                .ToList();

            foreach (var copy in sachCopies)
            {           
                if (string.IsNullOrEmpty(copy.pdf_link))
                {
                    copy.pdf_link = "#";
                }
            }

            var deXuatSachs = db.Sach_copy
                .Include(s => s.Sach)
                .Where(s => s.sach_id != sachCopy.sach_id)
                .OrderByDescending(s => Guid.NewGuid()) // Random sách
                .Take(10)
                .ToList();

            foreach (var deXuat in deXuatSachs)
            {
                if (string.IsNullOrEmpty(deXuat.image))
                {
                    deXuat.image = defaultImage;
                }
            }

            string pdfUrl = string.IsNullOrEmpty(sachCopy.pdf_link)
                ? "#"
                : sachCopy.pdf_link;

            var viewModel = new SachChiTietViewModel
            {
                image = imageUrl,
                Sach_Copy = sachCopy,
                DeXuatSachs = deXuatSachs,
                SachCopies = sachCopies
            };

            ViewBag.PdfUrl = pdfUrl;
            return View(viewModel);
        }
    }
}