using PresentationLayer.ModelsVm.Products;

namespace PresentationLayer.ModelsVm.Popular
{
    public class PopularVm
    {
        public string CategoryName { get; set; }
        public List<GetAllProductsVm> getAllProductsVms { get; set; }

    }
}
