using labexam.DTO;
using labexam.EF;
using labexam.Auth;
using System;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;

namespace labexam.Controllers


{
    [Logged]
    public class LoginController : Controller
    {
        private readonly LabEntities rw = new LabEntities(); // Your DbContext

        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(UserDTO log) 
        {
            if (!ModelState.IsValid) 
            {
                return View(log); // Return the view with validation messages
            }


            var user = (from u in rw.Users
                        where u.Name.Equals(log.Name) &&
                              u.Password.Equals(log.Password)
                        select u).SingleOrDefault();


            if (user != null)
            {
                var userDto = new UserDTO
                {
                    UserID = user.UserID,
                    Name = user.Name,
                    Email = user.Email,
                    Password = user.Password,
                    Role = user.Role
                };

                // Store user info in the session
                Session["user"] = userDto;
                /*
                // Redirect based on role
                if (user.Role == "Admin")
                {
                    return RedirectToAction("ManageUser", "User"); // Redirect to Admin dashboard
                }
                else if (user.Role == "User")
                {
                    return RedirectToAction("Index", "User"); 
                }
                */
                return RedirectToAction("Index", "LogIn"); // Default action if no role match
            }

            TempData["Msg"] = "User not found or invalid credentials";
            return View();
        }
    }
}
