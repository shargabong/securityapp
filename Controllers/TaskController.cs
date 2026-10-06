using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using securityapp.Data;
using securityapp.Models;

namespace securityapp.Controllers;

[Authorize]
public class TaskController : Controller
{
    private readonly AppDbContext _db;
    private readonly IAuthorizationService _auth;

    public TaskController(AppDbContext db, IAuthorizationService auth)
    {
        _db = db;
        _auth = auth;
    }

    public IActionResult Index()
    {
        var tasks = _db.Tasks.ToList();
        return View(tasks);
    }

    [Authorize(Roles = "Admin,Author")]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Author")]
    public async Task<IActionResult> Create(TaskItem task)
    {
        if (!ModelState.IsValid) return View(task);

        task.UserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value;
        _db.Tasks.Add(task);
        await _db.SaveChangesAsync();

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Edit(int id)
    {
        var task = await _db.Tasks.FindAsync(id);
        if (task == null) return NotFound();

        var result = await _auth.AuthorizeAsync(User, task, "CanEditTask");
        if (!result.Succeeded) return Forbid();

        return View(task);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, TaskItem task)
    {
        if (id != task.Id) return BadRequest();

        var existing = await _db.Tasks.FindAsync(id);
        if (existing == null) return NotFound();

        var result = await _auth.AuthorizeAsync(User, existing, "CanEditTask");
        if (!result.Succeeded) return Forbid();

        existing.Title = task.Title;
        await _db.SaveChangesAsync();

        return RedirectToAction("Index");
    }
}