using labexam.DTO;
using labexam.EF;
using labexam.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Web;
using System.Web.Mvc;

namespace labexam.Controllers
{
    public class UsersController : Controller
    {
        private readonly LabEntities rw = new LabEntities();

        public static User Convert(UserDTO d)
        {
            return new User()
            {
                UserID = d.UserID,
                Name = d.Name,
                Email = d.Email,
                Password = d.Password,
                Role = d.Role
            };
        }

        // Convert User entity to UserDTO
        public static UserDTO Convert(User d)
        {
            return new UserDTO()
            {
                UserID = d.UserID,
                Name = d.Name,
                Email = d.Email,
                Password = d.Password,
                Role = d.Role
            };
        }


        public List<UserDTO> Convert(List<User> data)
        {
            var list = new List<UserDTO>();
            foreach (var d in data)
            {
                list.Add(Convert(d));
            }
            return list;
        }
        //to show the list
        
        
     
        public ActionResult List()
        {
            var data = rw.Users.ToList();//rw.Channel.ToList() is wrong
            return View(Convert(data));
        }


        [HttpGet]
        public ActionResult Edit(int id)
        {
            var exobj = rw.Users.Find(id);  // Fix: Use Users instead of Channels
            if (exobj == null) return HttpNotFound();
            return View(Convert(exobj));
        }

        // POST: Users/Edit/{id}
        [HttpPost]
        public ActionResult Edit(UserDTO userDTO)
        {
            if (ModelState.IsValid)
            {
                // Check if a user with the same email already exists (excluding the current user)
                var duplicateUser = rw.Users
                    .FirstOrDefault(u => u.Email == userDTO.Email && u.UserID != userDTO.UserID);

                if (duplicateUser != null)
                {
                    ModelState.AddModelError("Email", "Email must be unique.");
                    return View(userDTO);
                }

                var existingUser = rw.Users.Find(userDTO.UserID);
                if (existingUser == null)
                {
                    return RedirectToAction("List");
                }

                // Update the user details
                existingUser.Name = userDTO.Name;
                existingUser.Email = userDTO.Email;
                existingUser.Password = userDTO.Password;
                existingUser.Role = userDTO.Role;

                rw.SaveChanges();

                // Success feedback
                TempData["Success"] = $"User '{userDTO.Name}' has been updated successfully.";
                return RedirectToAction("List");
            }

            // Validation failure feedback
            TempData["Error"] = "Failed to update the user. Please correct the errors and try again.";
            return View(userDTO);
        }

     
          

        // GET: Users/Details/{id}
        [HttpGet]
        public ActionResult Details(int id)
        {
            var user = rw.Users.Find(id);  // Fetch user by UserID, not ChannelId
            if (user == null)
            {
                TempData["Msg"] = "User with ID " + id + " not found";
                return RedirectToAction("List");
            }

            var userDTO = new UserDTO
            {
                UserID = user.UserID,
                Name = user.Name,
                Email = user.Email,
                Password = user.Password,
                Role = user.Role
            };

            return View(userDTO);
        }

        // GET: Users/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View(new UserDTO());
        }

        // POST: Users/Create
        [HttpPost]
        public ActionResult Create(UserDTO userDTO)
        {
            if (ModelState.IsValid)
            {
                // Check for duplicate email
                var duplicateUser = rw.Users.FirstOrDefault(u => u.Email == userDTO.Email);
                if (duplicateUser != null)
                {
                    ModelState.AddModelError("Email", "Email already exists.");
                    return View(userDTO);  // Stay on the page for correction
                }

                // Create a new user
                var newUser = new User
                {
                    Name = userDTO.Name,
                    Email = userDTO.Email,
                    Password = userDTO.Password,
                    Role = userDTO.Role
                };
                rw.Users.Add(newUser);
                rw.SaveChanges();

                // Provide success feedback
                TempData["Success"] = $"User '{userDTO.Name}' has been added successfully.";
                return RedirectToAction("List");
            }

            // Provide validation failure feedback
            TempData["Error"] = "Failed to add the user. Please correct the errors and try again.";
            return View(userDTO);
        }
        public ActionResult Manage()
        {
        
            var posts = rw.Posts.ToList(); 
            return View(posts);
        }

        // GET: Post/Delete/{id}
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var post = rw.Posts.Find(id);  // Find the post by ID
            if (post == null)
            {
                TempData["Error"] = "Post not found.";
                return RedirectToAction("Manage");
            }

            // Pass the post to the view for confirmation
            return View(post);
        }

        // POST: Post/Delete/{id}
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var post = rw.Posts.Find(id);  // Find the post by ID
            if (post == null)
            {
                TempData["Error"] = "Post not found.";
                return RedirectToAction("Manage");
            }

            // Delete the post from the database
            rw.Posts.Remove(post);
            rw.SaveChanges();

            TempData["Success"] = "Post has been deleted successfully.";
            return RedirectToAction("Manage");
        }
    }








}
