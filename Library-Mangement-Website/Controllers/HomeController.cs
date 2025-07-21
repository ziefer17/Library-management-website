using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using PagedList;


namespace Library_Mangement_Website.Controllers
{
    public class HomeController : Controller
    {
        private libraryEntities db = new libraryEntities();
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
                    s.ThongTin.ToLower().Contains(keyword) ||
                    s.TheLoai.Ten.ToLower().Contains(keyword) ||
                    s.TacGias.Any(tg => tg.Ten.ToLower().Contains(keyword))
                );
            }

            var pagedSachs = sachs.OrderBy(s => s.TenSach).ToPagedList(pageNumber, pageSize);

            return View(pagedSachs);
        }

        public JsonResult Suggest(string keyword)
        {

            var result = db.Saches
                .Where(s => s.TenSach.Contains(keyword))
                .OrderBy(s => s.TenSach.StartsWith(keyword) ? 0 : 1) // Ưu tiên khớp đầu
                .Take(10)
                .Select(s => new
                {
                    anh = Url.Content("~/Content/images/default-book.jpg"),
                    label = s.TenSach,
                    value = s.TenSach,
                    id = s.sach_id,
                    theloai = s.TheLoai.Ten,
                    tacgia = s.TacGias.Select(t => t.Ten).FirstOrDefault(),
                })
                .ToList();
            ViewBag.Keyword = keyword;

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ChiTiet(int id)
        {
            var sach = db.Saches.Include(s => s.TheLoai)
                                .Include(s => s.TacGias)
                                .FirstOrDefault(s => s.sach_id == id);
            if (sach == null)
            {
                return HttpNotFound();
            }
            return View(sach);
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
    }
}