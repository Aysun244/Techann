using System.Threading.Tasks;
using Gym.DataAccessLayer;
using Gym.Models;
using Gym.ViewModels.CourseVM;
using Gym.ViewModels.TeacherVM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace Gym.Areas.Admin.Controllers;
[Area("Admin")]
public class CourseController(AppDbContext _context) : Controller
{
   
    public async Task<IActionResult> Index()
    {
        
        var courses = await _context.Courses.Select(x=>new CourseGetVM()
        {
            Id=x.Id,
            Name = x.Name,
            Description = x.Description,
            ImagePath = x.ImagePath,
            Time = x.Time,
        }).ToListAsync();
        await ViewBags();
        return View(courses);
    }
    public async Task<IActionResult> Create()
    {
        await ViewBags();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult>Create(CourseCreateVM model)
    {
        await ViewBags();
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        if (!model.ImageFile.ContentType.StartsWith("image"))
        {
            ModelState.AddModelError("ImageFile", "Yalniz sekil elave olunmalidi");
        }
        if(model.ImageFile.Length/1024>200)
        {
            ModelState.AddModelError("ImageFile", "Seklin olcusu 2mb'dan boyuk olmamalidi");
        }
        string newFileName = Guid.NewGuid().ToString() + model.ImageFile.FileName;
        string path = Path.Combine("wwwroot", "images", "course", newFileName);
        using FileStream stream = new(path, FileMode.OpenOrCreate);
        await model.ImageFile.CopyToAsync(stream);
        Course course = new Course()
        {
             Name = model.Name,
            Description = model.Description,
            ImagePath = newFileName,
            Time = model.Time,
            TeacherId = model.TeacherId,
        };
        await _context.Courses.AddAsync(course);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }

    public async Task<IActionResult>Delete(int? id)
    {
        if (!id.HasValue || id < 1)
            return BadRequest();

        var course = await _context.Courses.Where(x => x.Id == id).ExecuteDeleteAsync(); 
        if(course==0) return NotFound();
        return RedirectToAction("Index");
    }
    public async Task<IActionResult> Update(int? id)
    {
        await ViewBags();
        if (!id.HasValue || id < 1) return BadRequest();
        var course = await _context.Courses.FirstOrDefaultAsync(x => x.Id == id);
        if (course == null) return NotFound();
        CourseUpdateVM model = new CourseUpdateVM()
        {
            Id = course.Id,
            Name = course.Name,
            Description = course.Description,
            ImagePath = course.ImagePath,
            Time = course.Time,
            TeacherId=course.TeacherId,
        };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult>Update(CourseUpdateVM model,int? id)
    {

        await ViewBags();
        if (!id.HasValue || id < 1)
            return BadRequest();
        var course = await _context.Courses.FirstOrDefaultAsync(x => x.Id == id);
        if (course == null) return BadRequest();


        if(model.ImageFile is not null)
        {
        if (!model.ImageFile.ContentType.StartsWith("image"))
        {
            ModelState.AddModelError("ImageFile", "Yalniz sekil elave olunmalidi");
        }
        if (model.ImageFile.Length / 1024 > 200)
        {
            ModelState.AddModelError("ImageFile", "Seklin olcusu 2mb'dan boyuk olmamalidi");
        }

        string newFileName = Guid.NewGuid().ToString() + model.ImageFile.FileName;
        string path = Path.Combine("wwwroot", "images", "course", newFileName);
        using FileStream stream = new(path, FileMode.OpenOrCreate);
        await model.ImageFile.CopyToAsync(stream);
            course.ImagePath = newFileName;
        }
        course.Name = model.Name;
        course.Description = model.Description;
        course.Time = model.Time;
        course.TeacherId = model.TeacherId;

        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }

    private async Task ViewBags()
    {
        var teachers = await _context.Teachers.Select(x => new TeacherGetVM()
        {
            Id = x.Id,
            Name = x.Name,
            ImagePath = x.ImagePath,
        }).ToListAsync();
        ViewBag.Teachers = teachers;   
    }
}