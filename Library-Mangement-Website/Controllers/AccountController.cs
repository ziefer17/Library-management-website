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
using System.Web.Security;

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

                if (existingUser.Password != null)
                {
                    ViewBag.Message = "Email này đã có mật khẩu";
                    return View(model);
                }

                existingUser.Password = model.Password;
                db.SaveChanges();

                ViewBag.AlertMessage = "Đăng ký thành công. Vui lòng đăng nhập.";

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
                db.Database.CommandTimeout = 120;
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
            FormsAuthentication.SignOut();
            Session.Clear();
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public ActionResult Profile()
        {
            if (Session["UserId"] == null)
                return RedirectToAction("Login");

            int userId = Convert.ToInt32(Session["UserId"]);
            var reader = db.TheDocGias.FirstOrDefault(r => r.docgia_id == userId);
            if (reader == null)
                return HttpNotFound();

            var model = new ReaderViewModel
            {
                ReaderId = reader.docgia_id,
                ReaderEmail = reader.Email,
                DateOfBirth = reader.NgaySinh.ToString("yyyy-MM-dd"),
                DateCreated = reader.NgayLapThe.ToString("dd/MM/yyyy"),
                ReaderType = reader.LoaiDG,
                Status = reader.Status,
                Password = reader.Password
            };

            return View(model);
        }

        // GET: /Account/EditProfile
        [HttpGet]
        public ActionResult EditProfile()
        {
            int userId = Convert.ToInt32(Session["UserId"]);
            var reader = db.TheDocGias.FirstOrDefault(r => r.docgia_id == userId);
            if (reader == null)
                return RedirectToAction("Login");

            var model = new ReaderViewModel
            {
                ReaderId = reader.docgia_id,
                ReaderName = reader.Ten,
                ReaderEmail = reader.Email,
                DateOfBirth = reader.NgaySinh.ToString("yyyy-MM-dd"),
                ReaderType = reader.LoaiDG,
                Status = reader.Status,
                Password = reader.Password,
                DateCreated = reader.NgayLapThe.ToString("dd/MM/yyyy")
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProfile(ReaderViewModel model)
        {
            if (ModelState.IsValid)
            {
                var reader = db.TheDocGias.FirstOrDefault(r => r.docgia_id == model.ReaderId);
                if (reader != null)
                {
                    reader.Ten = model.ReaderName;
                    reader.Email = model.ReaderEmail;

                    // Chỉ cập nhật mật khẩu nếu có nhập
                    if (!string.IsNullOrEmpty(model.Password))
                    {
                        if (model.Password == model.ConfirmPassword)
                        {
                            reader.Password = model.Password;
                        }
                        else
                        {
                            ModelState.AddModelError("ConfirmPassword", "Mật khẩu xác nhận không khớp.");
                            return View(model);
                        }
                    }

                    db.SaveChanges();
                    TempData["Message"] = "Cập nhật thành công!";
                    return RedirectToAction("Profile");
                }
            }

            return View(model);
        }
    }
}
