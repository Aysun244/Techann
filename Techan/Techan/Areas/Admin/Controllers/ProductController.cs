using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Techan.DataAccessLayer;
using Techan.ViewModels.Brands;

namespace Techan.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController (TechanDbContext _context): Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult>Create()
        {
            var brands = await _context.Brands.Select(x => new BrandGetVM
            {
                Id = x.Id,
                Name = x.Name,
            }).ToListAsync();
            ViewBag.Brands = brands;
            return View(brands);
        }
    }
}
