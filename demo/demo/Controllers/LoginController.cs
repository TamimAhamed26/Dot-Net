using demo.DTO;
using demo.EF;
using System;
using System.Linq;
using System.Web.Mvc;

namespace demo.Controllers
{
    public class LoginController : Controller
    {
        private readonly DemoEntities1 rw = new DemoEntities1();

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Index(UserDTO log, int id)
        {
            // Check user credentials
            var user = rw.Users.SingleOrDefault(u => u.UName == log.Uname && u.Pass == log.Password);
            if (user != null)
            {
                Session["user"] = user; // Store user information in session
                return RedirectToAction("Edit", "Program", new { id = id });
            }

            TempData["Msg"] = "Invalid username or password.";
            return RedirectToAction("List", "Program"); // Redirect back to the list page if login fails
        }




    }
}
