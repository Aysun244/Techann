using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Techan.DataAccessLayer;
using Techan.Models;
using Techan.ViewModels.Sliders;

namespace Techan.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SliderController (TechanDbContext _context): Controller
    {
        public async Task<IActionResult> Index()
        {
            List<Slider> datas = [];
            //using (var context = new TechanDbContext())
            //{
            //    datas = await context.Sliders.ToListAsync();
            //}
            datas = await _context.Sliders.ToListAsync();
            List<SliderGetVM> slider = [];
            foreach (var item in datas)
            {
                slider.Add(new SliderGetVM()
                {
                    BigTitle = item.BigTitle,
                    Id = item.Id,
                    Title = item.Title,
                    LittleTitle = item.LittleTitle,
                    Link = item.Link,
                    Offer = item.Offer,
                    ImageUrl = item.ImageUrl,
                });
            }
            return View(slider);
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SliderCreateVM model)
        {
            if (!ModelState.IsValid)
                return View(model);
            Slider slider = new();
            slider.LittleTitle = model.LittleTitle;
            slider.Title = model.Title;
            slider.BigTitle = model.BigTitle;
            slider.ImageUrl = model.ImageUrl;
            slider.Link = model.Link;
            slider.Offer = model.Offer;
            //using (var context = new TechanDbContext())
            //{
            //    await context.Sliders.AddAsync(slider);
            //    await context.SaveChangesAsync();
            //}
            await _context.Sliders.AddAsync(slider);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult>Delete(int? id)
        {
            if (!id.HasValue || id.Value<1) return BadRequest();
            //using (var context = new TechanDbContext())
            //{
            //int result = await context.Sliders.Where(x => x.Id == id).ExecuteDeleteAsync();
            //if (result == 0)
            //    return NotFound();
            //}
            int result = await _context.Sliders.Where(x => x.Id == id).ExecuteDeleteAsync();
            if (result == 0)
                return NotFound();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]

        public async Task<IActionResult>Update(int? id)
        {
            if (id.HasValue && id < 1) return BadRequest();
            var entity = await _context.Sliders
                .Select(x => new SliderUpdateVM
                {
                    Id = x.Id,
                    Title = x.Title,
                    BigTitle = x.BigTitle,
                    ImageUrl = x.ImageUrl,
                    Link = x.Link,
                    Offer = x.Offer,
                    LittleTitle = x.LittleTitle,
                })
                .FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null) return NotFound();  

            return View(entity);
        }

        [HttpPost]
        public async Task<IActionResult>Update(int? id,SliderUpdateVM vm)
        {
            if (id.HasValue && id < 1) 
                return BadRequest();
            if (!ModelState.IsValid) 
                return View(vm);
            var entity = await _context.Sliders.FirstOrDefaultAsync(x => x.Id == id);
            if (entity is null)
                return BadRequest();
            entity.LittleTitle = vm.LittleTitle;
            entity.Title = vm.Title;
            entity.Offer = vm.Offer;
            entity.BigTitle = vm.BigTitle;
            entity.Link = vm.Link;
            entity.ImageUrl = vm.ImageUrl;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
    }
}
    





