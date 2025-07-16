using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Library_Mangement_Website.Controllers
{
    public class AccountControllerr : Controller
    {
        private libraryEntities db = new libraryEntities();

        // GET: /Account/Register
        public ActionResult Register() => View();

        [HttpPost]
        public ActionResult Register(TheDocGia model)
        {
            if (db.TheDocGias.Any(d => d.Email == model.Email))
            {
                ModelState.AddModelError("", "Email đã tồn tại");
                return View();
            }

            model.NgayLapThe = DateTime.Now;
            db.TheDocGias.Add(model);
            db.SaveChanges();
            return RedirectToAction("Login");
        }

        public ActionResult Login() => View();

        [HttpPost]
        public ActionResult Login(TheDocGia model)
        {
            var user = db.TheDocGias.FirstOrDefault(d => d.Email == model.Email && d.Password == model.Password);
            if (user == null)
            {
                ModelState.AddModelError("", "Sai email hoặc mật khẩu");
                return View();
            }

            Session["User"] = user;
            Session["Role"] = user.LoaiDG;

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }
    }
}