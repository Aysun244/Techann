using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Techan.DataAccessLayer;
using Techan.Models;
using Techan.ViewModels.Brands;
using static System.Net.Mime.MediaTypeNames;

namespace Techan.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BrandController (TechanDbContext _context) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var datas = await _context.Brands.Select(x => new BrandGetVM
            {
                Id = x.Id,
                Name = x.Name,
                ImageUrl = x.ImageUrl,
            }).ToListAsync();

            return View(datas);
        }

       
        public async Task<IActionResult>Create()
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Create(BrandCreateVM vm)
        {
            if (vm.ImageFile != null)
            {
                if(!vm.ImageFile.ContentType.StartsWith("image"))
                {
                    string ext = Path.GetExtension(vm.ImageFile.FileName);
                    ModelState.AddModelError("ImageFile", "Sadece sekil formatinda fayl qebul olunandir" + "olmaz!");
                }
                if (vm.ImageFile.Length / 1024 > 200)
                    ModelState.AddModelError("ImageFile", "200kb'dan cox olmamalidir");
                
            }
            if (!ModelState.IsValid)
                return View(vm);


            string newImageName=Path.GetRandomFileName() +  Path.GetExtension(vm.ImageFile!.FileName);
            string path = Path.Combine("wwwroot","imgs", "brands",newImageName);
            await using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                await vm.ImageFile.CopyToAsync(fs);
            }
            await _context.Brands.AddAsync(new Brand
            {
                ImageUrl = newImageName,
                Name = vm.Name,
            });
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if(!id.HasValue || id<1) 
                return BadRequest();
           var brand = await _context.Brands
                .Where(x => x.Id == id)
                .Select(x=> new BrandUpdateVM
           {
               ImageUrl=x.ImageUrl,
               Name = x.Name,
           }).FirstOrDefaultAsync();
            return View(brand);
        }

        [HttpPost]

        public async Task<IActionResult>Update(int? id,BrandUpdateVM vm)
        {
            if (!id.HasValue || id < 1)
                return BadRequest();

            if ( vm.ImageFile!= null)
            {
                if (!vm.ImageFile.ContentType.StartsWith("image"))
                {
                    string ext = Path.GetExtension(vm.ImageFile.FileName);
                    ModelState.AddModelError("ImageFile", "Sadece sekil formatinda fayl qebul olunandir" + "olmaz!");
                }
                if (vm.ImageFile.Length / 1024 > 200)
                    ModelState.AddModelError("ImageFile", "200kb'dan cox olmamalidir");
               
            }
            if (!ModelState.IsValid)           
                return View(vm);
            var brand = await _context.Brands.FindAsync(id);
            if (brand is null)
                return NotFound();
            if (vm.ImageFile != null)
            {
                string path = Path.Combine("wwwroot", "imgs", "brands", brand.ImageUrl!);
                await using (FileStream fs = new FileStream(path, FileMode.Create))
                {
                    await vm.ImageFile.CopyToAsync(fs);
                }
            }
                brand.Name = vm.Name;
            await _context.SaveChangesAsync();
               return RedirectToAction(nameof(Index));
        }
    }
}
