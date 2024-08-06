using Ecommerce.Data;
using Ecommerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ecommerce.ViewModel;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using Microsoft.AspNetCore.Authorization;
using Ecommerce.Utalitis;
namespace Ecommerce.Controllers
{
    [Authorize]
    [Authorize(Roles = RL.RoleAdmin)]
    public class Dashboard : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IHostingEnvironment hosting;

        public Dashboard(ApplicationDbContext db, IHostingEnvironment hosting)
        {
            _db=db;
            this.hosting = hosting;
        }


        public IActionResult Index()
        {
            return View();
        }

        #region get
        public IActionResult GetProducts()
        {
            var product= _db.products.Include(x=>x.Company).ToList();
            return View(product);

        }
        #endregion


        #region add
        public IActionResult AddProduct()
        {

            var product = new ProductViewModel();
            product.companies = _db.companies.ToList();
                return View(product);
        }

        [HttpPost]
        public IActionResult AddProduct(Product product)
        {
            if (product.ProductImage != null)
            {
                string ImageFolder = Path.Combine(hosting.WebRootPath, "Images");
                String ImagePath = Path.Combine(ImageFolder, product.ProductImage.FileName);
                product.ProductImage.CopyTo(new FileStream(ImagePath, FileMode.Create));
                product.ImagePath = product.ProductImage.FileName;
            }
           

            _db.products.Add(product);
            _db.SaveChanges();
            return RedirectToAction("GetProducts");
        }
        #endregion


        #region edit
        public IActionResult EditProduct(int Id)
        {
            ProductViewModel model = new ProductViewModel();
            model.product = _db.products.FirstOrDefault(x => x.Id == Id);
            model.companies= _db.companies.ToList();

            return View(model);
        }

        [HttpPost]
        public IActionResult EditProduct(ProductViewModel model)
        {
        
            Product product = _db.products.FirstOrDefault(x=>x.Id==model.product.Id);


            if (product.ProductImage != null)
            {
                string ImageFolder = Path.Combine(hosting.WebRootPath, "Images");
                String ImagePath = Path.Combine(ImageFolder, product.ProductImage.FileName);
                model.product.ProductImage.CopyTo(new FileStream(ImagePath, FileMode.Create));
                product.ImagePath = model.product.ProductImage.FileName;
            }

            product.Name= model.product.Name;
            product.Description= model.product.Description;
            product.Price= model.product.Price;
            product.CompanyId= model.product.CompanyId;
            _db.Update(product);
            _db.SaveChanges();
            return RedirectToAction("GetProducts");
        }
        #endregion

        #region delete
        public IActionResult DeleteProduct(int Id)
        {
            Product? p = _db.products.FirstOrDefault(x => x.Id == Id);
            if (p.Id != null)
            {
                _db.Remove(p);
                _db.SaveChanges();
                return RedirectToAction("GetProducts");
            }   
                return RedirectToAction("GetProducts");
        }
        #endregion

    }
}
