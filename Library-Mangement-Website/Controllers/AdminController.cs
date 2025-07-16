using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Library_Mangement_Website.Controllers
{
    public class AdminController : Controller
    {
        public ActionResult Dashboard()
        {
            // Nếu chưa đăng nhập hoặc không phải Admin → quay về trang đăng nhập
            if (Session["Role"]?.ToString() != "Admin")
                return RedirectToAction("Login", "Account");

            // Nếu là Admin thì cho vào trang Dashboard
            return View();
        }
    }
}