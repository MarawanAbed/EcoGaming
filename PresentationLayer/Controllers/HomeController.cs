using System.Diagnostics;
using BusinessLogicLayer.Services.Abstraction;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.ModelsVm;
using PresentationLayer.ModelsVm.Categories;
using PresentationLayer.ModelsVm.Home;
using PresentationLayer.ModelsVm.Popular;
using PresentationLayer.ModelsVm.Products;

namespace PresentationLayer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICategoryServices _category;
        private readonly IProductServices _product; 
        public HomeController(ILogger<HomeController> logger, ICategoryServices category,IProductServices productServices)
        {
            _logger = logger;
            _category = category;
            _product = productServices;
        }
        public async Task<IActionResult> Index()
        {
            var categories = await _category.GetAllCategories();
            var categoryModels = categories.Select(GetAllCategoriesVm.FromCategory).ToList();

            var popularProducts = new List<PopularVm>();
            foreach (var category in categories)
            {
                var products = await _product.GetPopularProducts(category.CategoryId);
                var popularVm = new PopularVm
                {
                    CategoryName = category.Name,
                    getAllProductsVms = products.Select(p => new GetAllProductsVm
                    {
                        ProductId = p.ProductId,
                        Name = p.Name,
                        Description = p.Description,
                        Price = p.Price,
                        ImageUrl = p.ImageUrl,
                        Stock = p.Stock,
                        CategoryId = p.CategoryId,
                        CategoryName = p.CategoryName
                    }).ToList()
                };
                popularProducts.Add(popularVm);
            }

            var model = new HomeVm
            {
                Categories = categoryModels,
                PopularProducts = popularProducts
            };

            return View(model);
        }
        public async Task<IActionResult> Popular(int id)
        {
            var category = await _category.GetCategoryById(id);
            if (category == null)
            {
                return NotFound();
            }

            var popularProducts = await _product.GetPopularProducts(id);
            var model = new PopularVm
            {
                CategoryName = category.Name,
                getAllProductsVms = popularProducts.Select(p => new GetAllProductsVm
                {
                    ProductId = p.ProductId,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    ImageUrl = p.ImageUrl,
                    Stock = p.Stock,
                    CategoryId = p.CategoryId,
                    CategoryName = p.CategoryName
                }).ToList()
            };

            return View(model);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
