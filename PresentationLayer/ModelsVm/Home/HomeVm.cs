using PresentationLayer.ModelsVm.Categories;
using PresentationLayer.ModelsVm.Popular;

namespace PresentationLayer.ModelsVm.Home
{
    public class HomeVm
    {
        public List<GetAllCategoriesVm> Categories { get; set; }
        public List<PopularVm> PopularProducts { get; set; }
    }
}
