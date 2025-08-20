using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Data;
using TaskManagement.Models;

namespace TaskManagement.Controllers;

public class TaskAssignedController : Controller
{
    private readonly TaskDbContext _dbContext;

    public TaskAssignedController(TaskDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public IActionResult Index()
    {
        var assigned = _dbContext.AssignedTasks
            .Include(a => a.User)
            .Include(a => a.Task)
            .ToList();

        ViewBag.Tasks = _dbContext.Tasks.ToList();
        ViewBag.Users = _dbContext.Users.ToList();

        return View(assigned);
    }

    [HttpPost]
    public IActionResult Save(AssignedTask task)
    {
        if (task.Id == 0)
        {
            _dbContext.AssignedTasks.Add(task);
        }
        else
        {
            _dbContext.AssignedTasks.Update(task);
        }
        _dbContext.SaveChanges();
        return Json(new { msg = "Saved successfully" });
    }

    [HttpPost]
    public IActionResult Delete(int id)
    {
        var a = _dbContext.AssignedTasks.Find(id);
        if (a == null) return NotFound();

        _dbContext.AssignedTasks.Remove(a);
        _dbContext.SaveChanges();
        return Json(new { msg = "Deleted successfully" });
    }

    [HttpGet]
    public IActionResult GetById(int id)
    {
        var a = _dbContext.AssignedTasks.Find(id);
        if (a == null) return NotFound();
        return Json(a);
    }
}
