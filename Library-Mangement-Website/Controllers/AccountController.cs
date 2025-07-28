using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Library_Mangement_Website;
using Library_Mangement_Website.Models;
using System.Data;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Library_Mangement_Website.Controllers
{
    public class AccountController : Controller
    {
        private libraryEntities db = new libraryEntities();

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Check email
                var existingUser = db.TheDocGias.FirstOrDefault(d => d.Email == model.Email);
                if (existingUser == null)
                {
                    ViewBag.Message = "Người dùng chưa tạo thẻ độc giả";
                    return View(model);
                }


                //TheDocGia.Password = model.Password;

                db.SaveChanges();

                return RedirectToAction("Login", "Account");
            }

            return View(model);
        }

        // GET: /Account/Login
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                
                var user = db.TheDocGias.FirstOrDefault(d => d.Email == model.Email && d.Password == model.Password);
                if (user == null)
                {
                    ViewBag.Message = "Email hoặc mật khẩu không đúng.";
                    return View(model);
                }
               
                Session["UserId"] = user.docgia_id;
                Session["UserName"] = user.Ten;
                Session["LoaiDG"] = user.LoaiDG;

                // Redirect to Sach/Index
                return RedirectToAction("HomeMain", "Home");
            }

            return View(model);
        }

        // GET: /Account/Logout
        [HttpGet]
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login", "Account");
        }
    }
}
