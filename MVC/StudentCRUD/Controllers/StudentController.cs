using Microsoft.AspNetCore.Mvc;
using StudentCRUD.Data;
using StudentCRUD.Models;
using StudentCRUD.DTOs;

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
            List<Student> students = _db.Students.ToList();
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
                var student = new Student
                {
                    Name = dto.Name,
                    Age = (int)dto.Age,
                    Address = dto.Address
                };

                _db.Students.Add(student);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(dto);
        }

        /*
        [HttpPost]
        public IActionResult Create(Student student)
        {
            if (ModelState.IsValid)
            {
                _db.Students.Add(student);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(student);
        }
        */

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
                Address = student.Address
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

                student.Name = dto.Name;
                student.Age = dto.Age;
                student.Address = dto.Address;

                _db.Students.Update(student);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(dto);
        }

        /*
        [HttpPost]
        public IActionResult Edit(Student student)
        {
            if (ModelState.IsValid)
            {
                _db.Students.Update(student);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(student);
        }
        */
    }
}
