

using AutoMapper;
using BusinessLogicLayer.DTOs;
using DataAccessLayer.Enitites;

namespace BusinessLogicLayer.Mapper
{
    public class DomainProfile : Profile
    {
        public DomainProfile() 
        {
            //Category Entity to CategoryDto

            CreateMap<Category, CategoryDto>();

            //CategoryDto to Category Entity

            CreateMap<CategoryDto, Category>();

            //Product Entity to ProductDto
            CreateMap<CategoryDto, Category>();
            CreateMap<Product, ProductDto>();
        }
    }
}
