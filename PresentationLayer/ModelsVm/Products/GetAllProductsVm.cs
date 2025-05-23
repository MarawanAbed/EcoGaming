﻿using BusinessLogicLayer.DTOs;

namespace PresentationLayer.ModelsVm.Products
{
    public class GetAllProductsVm
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string ImageUrl { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        public string AddedByUserId { get; set; } // Store the user's ID

        public string AddedByRole { get; set; }   // Store the role (Admin or Buyer)

        public static GetAllProductsVm FromProduct(GetAllProductsDto product)
        {
            return new GetAllProductsVm
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                ImageUrl = product.ImageUrl,
                CategoryId = product.CategoryId,
                CategoryName = product.CategoryName,
                AddedByRole= product.AddedByRole,
                AddedByUserId = product.AddedByUserId
            };
        }
    }
}
