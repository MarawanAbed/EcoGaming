using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Helper;
using BusinessLogicLayer.Services.Abstraction;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PresentationLayer.ActionRequests.Products;
using PresentationLayer.ModelsVm.Products;

namespace PresentationLayer.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductServices _productServices;
        private readonly ICategoryServices _categoryServices;
        public ProductsController(IProductServices productServices, ICategoryServices categoryServices)
        {
            _productServices = productServices;
            _categoryServices = categoryServices;
        }
        public async Task<IActionResult> Index()
        {
            var products = await _productServices.GetAllProducts();
            var model = products.Select(GetAllProductsVm.FromProduct).ToList();
            return View(model);
        }

        [HttpGet]
        public  IActionResult AddProducts()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddProducts(AddProductsActionRequest addProductsAction)
        {
            if (ModelState.IsValid)
            {
                var imageUrl = UploadFile.UploadImage(addProductsAction.Image, "Images");
                if (imageUrl != null)
                {
                    var productDto = new ProductDto
                    {
                        Name = addProductsAction.Name,
                        Description = addProductsAction.Description,
                        Price = addProductsAction.Price,
                        Stock = addProductsAction.Stock,
                        ImageUrl = imageUrl,
                        CategoryId = addProductsAction.CategoryId
                    };
                    await _productServices.AddProduct(productDto);
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Image upload failed.");
                }
            }
            else
            {
                ModelState.AddModelError("", "Invalid form data.");
            }
          return View(addProductsAction);
        }


        [HttpPost]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            await _productServices.DeleteProduct(id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> EditProduct(int id)
        {
            var product = await _productServices.GetProductById(id);
            var model=new EditProductsActionRequest
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                CategoryId = product.CategoryId,
                Stock = product.Stock,
                ExistingImagePath = product.ImageUrl
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditProduct(EditProductsActionRequest editProductRequest)
        {
            if (ModelState.IsValid)
            {
                var productDto = new ProductDto
                {
                    Name = editProductRequest.Name,
                    Description = editProductRequest.Description,
                    Price = editProductRequest.Price,
                    CategoryId = editProductRequest.CategoryId,
                    Stock = editProductRequest.Stock,
                    ProductId = editProductRequest.ProductId,
                    ImageUrl = editProductRequest.ExistingImagePath
                };
                await _productServices.UpdateProduct(productDto);
                return RedirectToAction("Index");
            }
            return View(editProductRequest);
        }
    }
}
