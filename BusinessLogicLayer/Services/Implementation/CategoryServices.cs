using AutoMapper;
using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Services.Abstraction;
using DataAccessLayer.Enitites;
using DataAccessLayer.Repo.Abstraction;

namespace BusinessLogicLayer.Services.Implementation
{
    public class CategoryServices : ICategoryServices
    {
        private readonly ICategoryRepo categoryRepo;
        private readonly IMapper _mapper;

        public CategoryServices(ICategoryRepo categoryRepo,IMapper mapper)
        {
            this.categoryRepo = categoryRepo;
            _mapper = mapper;
        }

        public async Task AddCategory(CategoryDto category)
        {
            var categoryEntity = _mapper.Map<Category>(category);
            await categoryRepo.AddCategory(categoryEntity);
        }

        public async Task DeleteCategory(int id)
        {
            await categoryRepo.DeleteCategory(id);
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategories()
        {
            var categories = await categoryRepo.GetAllCategories();
            return _mapper.Map<IEnumerable<CategoryDto>>(categories);
        }

        public async Task<CategoryDto> GetCategoryById(int id)
        {
            var category = await categoryRepo.GetCategoryById(id);
            return _mapper.Map<CategoryDto>(category);
        }
        public async Task UpdateCategory(CategoryDto category)
        {
            var categoryEntity = _mapper.Map<Category>(category);
            await categoryRepo.UpdateCategory(categoryEntity);
        }
    }
}
