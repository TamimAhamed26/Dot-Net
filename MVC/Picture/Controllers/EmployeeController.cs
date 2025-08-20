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
        public async Task<IActionResult> Edit(int id, Employee employee)
        {
           

            var existingEmployee = await _context.Employees.FindAsync(id);
            if (existingEmployee == null)
                return NotFound();

            if (!ModelState.IsValid)
            {
                employee.PicturePath = existingEmployee.PicturePath;
                return View(employee);
            }

            existingEmployee.Name = employee.Name.Trim();
            existingEmployee.Email = employee.Email.Trim();

            if (employee.Picture != null && employee.Picture.Length > 0)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                string extension = Path.GetExtension(employee.Picture.FileName).ToLowerInvariant();

                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("Picture", "Only JPG, JPEG, and PNG formats are allowed.");
                    employee.PicturePath = existingEmployee.PicturePath;
                    return View(employee);
                }

                if (employee.Picture.Length > 5 * 1024 * 1024)
                {
                    ModelState.AddModelError("Picture", "File size cannot exceed 5 MB.");
                    employee.PicturePath = existingEmployee.PicturePath;
                    return View(employee);
                }

                string uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Pictures");
                if (!Directory.Exists(uploadDir))
                    Directory.CreateDirectory(uploadDir);

                string safeFileName = $"{Guid.NewGuid()}{extension}";
                string filePath = Path.Combine(uploadDir, safeFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await employee.Picture.CopyToAsync(stream);
                }

                if (!string.IsNullOrEmpty(existingEmployee.PicturePath))
                {
                    string oldFilePath = Path.Combine(uploadDir, Path.GetFileName(existingEmployee.PicturePath));
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        try { System.IO.File.Delete(oldFilePath); }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error deleting old file: {ex.Message}");
                        }
                    }
                }

                existingEmployee.PicturePath = $"/Pictures/{safeFileName}";
            }

            try
            {
                _context.Employees.Update(existingEmployee);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Employee updated successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database update failed: {ex.Message}");
                ModelState.AddModelError("", "Something went wrong while saving. Please try again.");
            }

            return View(employee);
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