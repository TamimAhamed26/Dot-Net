using Microsoft.AspNetCore.Mvc;
using TaskManagement.Data;
using TaskManagement.Models;

namespace TaskManagement.Controllers;

public class UserController : Controller
{
    private readonly TaskDbContext _dbContext;

    public UserController(TaskDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IActionResult Index()
    {
        return View(); 
    }

    [HttpGet]
    public IActionResult Get()
    {
        var users = _dbContext.Users.ToList();
        return Json(users);
    }

    [HttpGet]
    public IActionResult GetById(int id)
    {
        var user = _dbContext.Users.Find(id);
        if (user == null) return NotFound();
        return Json(user);
    }

    [HttpPost]
    public IActionResult Save(User user)
    {
        _dbContext.Users.Add(user);
        _dbContext.SaveChanges();
        return Json(new { data = user, msg = "User created successfully" });
    }

    [HttpPost]
    public IActionResult Update(User user)
    {
        _dbContext.Users.Update(user);
        _dbContext.SaveChanges();
        return Json(new { data = user, msg = "User updated successfully" });
    }

    [HttpGet]
    public IActionResult Delete(int id)
    {
        var user = _dbContext.Users.Find(id);
        if (user == null) return NotFound();

        _dbContext.Users.Remove(user);
        _dbContext.SaveChanges();
        return Json(new { msg = "User deleted successfully" });
    }
}
