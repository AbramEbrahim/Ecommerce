using Ecommerce.Data;
using Ecommerce.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Controllers
{
    [Authorize]
    public class ItemController(ApplicationDbContext _db) : Controller
    {
        public IActionResult Index()
        {

            var product = _db.products.Include(x => x.Company).ToList();
            return View(product);
        }
    }
}
