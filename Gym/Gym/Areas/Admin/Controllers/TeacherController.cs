using Gym.DataAccessLayer;
using Gym.Models;
using Gym.ViewModels.CourseVM;
using Gym.ViewModels.TeacherVM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gym.Areas.Admin.Controllers;
[Area("Admin")]
public class TeacherController(AppDbContext _context) : Controller
{
    public async Task<IActionResult> Index()
    {
        var courses = await _context.Teachers.Select(x => new TeacherGetVM()
        {
            Id = x.Id,
            Name = x.Name,        
            ImagePath = x.ImagePath,
        }).ToListAsync();
        return View(courses);
    }

    public async Task<IActionResult>Create()
    {
        return View();
    }

    [HttpPost]
    public async Task <IActionResult>Create(TeacherCreateVM model)
    {
        ModelState.Remove("Id");
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        if (!model.ImageFile.ContentType.StartsWith("image"))
        {
            ModelState.AddModelError("ImageFile", "Yalniz sekil elave olunmalidi");
        }
        if (model.ImageFile.Length / 1024 > 200)
        {
            ModelState.AddModelError("ImageFile", "Seklin olcusu 2mb'dan boyuk olmamalidi");
        }
        string newFileName = Guid.NewGuid().ToString() + model.ImageFile.FileName;
        string path = Path.Combine("wwwroot", "images", "teacher", newFileName);
        using FileStream stream = new(path, FileMode.OpenOrCreate);
        await model.ImageFile.CopyToAsync(stream);
        Teacher teacher = new Teacher()
        {
            Name = model.Name,         
            ImagePath = newFileName,          
        };
        await _context.Teachers.AddAsync(teacher);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (!id.HasValue || id < 1)
            return BadRequest();

        var teacher = await _context.Teachers.Where(x => x.Id == id).ExecuteDeleteAsync();
        if (teacher == 0) return NotFound();
        return RedirectToAction("Index");
    }
    public async Task<IActionResult> Update(int? id)
    {
        if (!id.HasValue || id < 1) return BadRequest();
        var teacher = await _context.Teachers.FirstOrDefaultAsync(x => x.Id == id);
        if (teacher == null) return NotFound();
        TeacherUpdateVM model = new TeacherUpdateVM()
        {
            Id = teacher.Id,
            Name = teacher.Name,       
            ImagePath = teacher.ImagePath,
    
        };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Update(TeacherUpdateVM model, int? id)
    {

        if (!id.HasValue || id < 1)
            return BadRequest();
        var teacher = await _context.Teachers.FirstOrDefaultAsync(x => x.Id == id);
        if (teacher == null) return BadRequest();


        if (model.ImageFile is not null)
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
            teacher.ImagePath = newFileName;
        }
        teacher.Id=model.Id;
        teacher.Name = model.Name;
     
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }
}


