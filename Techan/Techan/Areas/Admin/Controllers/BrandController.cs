using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Techan.DataAccessLayer;
using Techan.ViewModels.Brands;


namespace Techan.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BrandController (TechanDbContext _context): Controller
    {
        public async Task< IActionResult> Index()
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
    }
}
