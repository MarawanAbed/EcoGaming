using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Helper;
using BusinessLogicLayer.Services.Abstraction;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.ActionRequests.Categories;
using PresentationLayer.ModelsVm.Browse;
using PresentationLayer.ModelsVm.Categories;
using PresentationLayer.ModelsVm.Products;

namespace PresentationLayer.Controllers.Categories
{
    public class CategoriesController : Controller
    {
        private readonly ICategoryServices _categoryServices;
        private readonly IProductServices _productServices;

        public CategoriesController(ICategoryServices categoryServices,IProductServices productServices)
        {
            _categoryServices = categoryServices;
            _productServices = productServices;
        }
        public async Task<IActionResult> Browse(int id,string search=null)
        {
            var category = await _categoryServices.GetCategoryById(id);
            if (category == null)
            {
                return NotFound();
            }
            IEnumerable<GetAllProductsDto> searchProducts;
            if(!string.IsNullOrEmpty(search))
            {
                searchProducts = await _productServices.SearchProductsByName(search,id);
            }
            else
            {
                searchProducts = await _productServices.GetProductsByCategory(id);
            }
            var products = await _productServices.GetProductsByCategory(id);
            var model = new BrowseVm
            {
                Category = category,
                Products = searchProducts.Select(p => new GetAllProductsVm
                {
                    ProductId = p.ProductId,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    ImageUrl = p.ImageUrl,
                    Stock = p.Stock,
                    CategoryId = p.CategoryId,
                    CategoryName = category.Name
                }).ToList(),
                Search = search
            };

            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _categoryServices.GetAllCategories();
            return Json(categories);
        }
        public async Task<IActionResult> Index()
        {

            var categories = await _categoryServices.GetAllCategories();
            var model = categories.Select(GetAllCategoriesVm.FromCategory).ToList();
            return View(model);
        }

        [HttpGet]
        public IActionResult AddCategory()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory(AddCategoriesActionRequest categoriesAction)
        {

            if (ModelState.IsValid)
            {
                var imageUrl = UploadFile.UploadImage(categoriesAction.Image, "Images");

                var categoryDto = new CategoryDto
                {
                    Name = categoriesAction.Name,
                    ImageUrl = imageUrl

                };
                await _categoryServices.AddCategory(categoryDto);
                return RedirectToAction("Index");
            }
            return View(categoriesAction);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            await _categoryServices.DeleteCategory(id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> EditCategory(int id)
        {
            var category = await _categoryServices.GetCategoryById(id);
            var categoryAction = new EditCategoriesActionRequest
            {
                CategoryId = category.CategoryId,
                CategoryName = category.Name,
                ExistingImagePath = category.ImageUrl
            };
            return View(categoryAction);
        }

        [HttpPost]
        public async Task<IActionResult> EditCategory(EditCategoriesActionRequest categoriesAction)
        {
            string imageUrl = categoriesAction.ExistingImagePath;
            if (categoriesAction.Image != null)
            {
                // ✅ Delete the old image if it exists
                if (!string.IsNullOrEmpty(categoriesAction.ExistingImagePath))
                {
                    string oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", categoriesAction.ExistingImagePath);
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }
                // ✅ Upload the new image using the helper
                imageUrl = UploadFile.UploadImage(categoriesAction.Image, "Images");
            }
            var category = await _categoryServices.GetCategoryById(categoriesAction.CategoryId);
            if (category != null)
            {
                category.Name = categoriesAction.CategoryName;
                category.ImageUrl = imageUrl;
                await _categoryServices.UpdateCategory(category);
                return RedirectToAction("Index");
            }
            return View(categoriesAction);
        }
    }
}
