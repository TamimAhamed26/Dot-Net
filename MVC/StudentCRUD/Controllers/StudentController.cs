using Microsoft.AspNetCore.Mvc;
using StudentCRUD.Data;
using StudentCRUD.Models;
using StudentCRUD.DTOs;
using System.IO;

namespace StudentCRUD.Controllers
{
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _db;

        public StudentController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var students = _db.Students.ToList();
            return View(students);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var student = _db.Students.FirstOrDefault(s => s.Id == id);
            if (student == null)
                return NotFound();

            return View(student);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateStudentDto dto)
        {
            if (ModelState.IsValid)
            {
                string? picturePath = null;

                if (dto.PictureFile != null)
                {
                    string fileName = Path.GetFileName(dto.PictureFile.FileName);
                    string extension = Path.GetExtension(fileName).ToLower();

                    if (extension != ".jpg" && extension != ".png" && extension != ".jpeg")
                    {
                        ModelState.AddModelError("PictureFile", "Only .jpg, .png, and .jpeg files are allowed.");
                        return View(dto);
                    }

                    string uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Pictures");
                    if (!Directory.Exists(uploadsDir))
                        Directory.CreateDirectory(uploadsDir);

                    string savedFileName = dto.Name.Replace(" ", "_") + extension;
                    string filePath = Path.Combine(uploadsDir, savedFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        dto.PictureFile.CopyTo(stream);
                    }

                    picturePath = "/Pictures/" + savedFileName;
                }

                var student = new Student
                {
                    Name = dto.Name,
                    Age = dto.Age ?? 0,
                    Address = dto.Address,
                    PicturePath = picturePath
                };

                _db.Students.Add(student);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(dto);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var student = _db.Students.FirstOrDefault(s => s.Id == id);
            if (student == null)
                return NotFound();

            var dto = new EditStudentDto
            {
                Id = student.Id,
                Name = student.Name,
                Age = student.Age,
                Address = student.Address,
                ExistingPicturePath = student.PicturePath
            };

            return View(dto);
        }

        [HttpPost]
        public IActionResult Edit(EditStudentDto dto)
        {
            if (ModelState.IsValid)
            {
                var student = _db.Students.FirstOrDefault(s => s.Id == dto.Id);
                if (student == null)
                    return NotFound();

                if (dto.PictureFile != null)
                {
                    string fileName = Path.GetFileName(dto.PictureFile.FileName);
                    string extension = Path.GetExtension(fileName).ToLower();

                    if (extension != ".jpg" && extension != ".png" && extension != ".jpeg")
                    {
                        ModelState.AddModelError("PictureFile", "Only .jpg, .png, and .jpeg files are allowed.");
                        return View(dto);
                    }

                    string uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Pictures");
                    if (!Directory.Exists(uploadsDir))
                        Directory.CreateDirectory(uploadsDir);

                    string savedFileName = dto.Name.Replace(" ", "_") + extension;
                    string filePath = Path.Combine(uploadsDir, savedFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        dto.PictureFile.CopyTo(stream);
                    }

                    student.PicturePath = "/Pictures/" + savedFileName;
                }

                student.Name = dto.Name;
                student.Age = dto.Age;
                student.Address = dto.Address;

                _db.Students.Update(student);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(dto);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var student = _db.Students.FirstOrDefault(s => s.Id == id);
            if (student == null)
                return NotFound();

            return View(student);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var student = _db.Students.FirstOrDefault(s => s.Id == id);
            if (student == null)
                return NotFound();

            _db.Students.Remove(student);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
