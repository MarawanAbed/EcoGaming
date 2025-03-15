using BusinessLogicLayer.DTOs;
using PresentationLayer.ModelsVm.Products;

namespace PresentationLayer.ModelsVm.Browse
{
    public class BrowseVm
    {
        public CategoryDto Category { get; set; }
        public List<GetAllProductsVm> Products { get; set; }
        public string PrimaryColor { get; set; }
        public string SecondaryColor { get; set; }
        public string BackgroundColor { get; set; }
        public string TextColor { get; set; }
    }
}
