using BusinessLogicLayer.DTOs;
using PresentationLayer.ModelsVm.Products;

namespace PresentationLayer.ModelsVm.Browse
{
    public class BrowseVm
    {
        public CategoryDto Category { get; set; }
        public List<GetAllProductsVm> Products { get; set; }

        public string Search { get; set; }  

    }
}
