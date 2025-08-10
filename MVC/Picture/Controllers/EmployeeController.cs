using Microsoft.AspNetCore.Mvc;
using PictureU.Models;

namespace PictureU.Controllers
{
    public class EmployeeController : Controller
    {
        public readonly EDbContext _context;
        public EmployeeController(EDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var employees = _context.Employees.ToList();
            return View(employees);
        }

        public IActionResult Create()
        {


            return View();
        }
        [HttpPost]
        public IActionResult Create(Employee eMployee)
        {
            if (ModelState.IsValid)
            {
                if (eMployee.Picture != null)
                {
                    string fileName = Path.GetFileName(eMployee.Picture.FileName);
                    string extension = Path.GetExtension(fileName);

                    if (extension != ".jpg" && extension != ".png" && extension != ".jpeg")
                    {
                        ModelState.AddModelError("Picture", "Only .jpg, .png, and .jpeg files are allowed.");
                        return View(eMployee);
                    }

                    string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Pictures");
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }

                    string filePath = Path.Combine(directoryPath, eMployee.Name + extension);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        eMployee.Picture.CopyTo(stream);
                    }

                    eMployee.PicturePath = "/Pictures/" + eMployee.Name + extension;
                }

                _context.Employees.Add(eMployee);

                if (_context.SaveChanges() > 0)
                {
                    return RedirectToAction("Index");
                }
            }
            else
            {
                var message = string.Join(" | ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                ModelState.AddModelError(" ", message);
            }

            return View(eMployee);
        }

        // GET: Employee/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var employee = _context.Employees.Find(id);
            if (employee == null)
                return NotFound();

            return View(employee);
        }

        [HttpPost]
        public IActionResult Edit(int id, Employee eMployee)
        {
            if (id != eMployee.EmployeeId)
                return NotFound();

            if (ModelState.IsValid)
            {
                var existingEmployee = _context.Employees.Find(id);
                if (existingEmployee == null)
                    return NotFound();

                existingEmployee.Name = eMployee.Name;
                existingEmployee.Email = eMployee.Email;

                if (eMployee.Picture != null)
                {
                    string fileName = Path.GetFileName(eMployee.Picture.FileName);
                    string extension = Path.GetExtension(fileName);

                    if (extension != ".jpg" && extension != ".png" && extension != ".jpeg")
                    {
                        ModelState.AddModelError("Picture", "Only .jpg, .png, and .jpeg files are allowed.");
                        return View(eMployee);
                    }

                    string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Pictures");
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }

                    string filePath = Path.Combine(directoryPath, eMployee.Name + extension);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        eMployee.Picture.CopyTo(stream);
                    }

                    existingEmployee.PicturePath = "/Pictures/" + eMployee.Name + extension;
                }

                _context.Employees.Update(existingEmployee);

                if (_context.SaveChanges() > 0)
                {
                    return RedirectToAction("Index");
                }
            }
            else
            {
                var message = string.Join(" | ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                ModelState.AddModelError(" ", message);
            }

            return View(eMployee);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var employee = _context.Employees.Find(id);
            if (employee == null)
                return NotFound();

            return View(employee);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var employee = _context.Employees.Find(id);
            if (employee == null)
                return NotFound();

            if (!string.IsNullOrEmpty(employee.PicturePath))
            {
                string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", employee.PicturePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
            }

            _context.Employees.Remove(employee);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }


    }
}