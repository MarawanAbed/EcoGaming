

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
            CreateMap<ProductDto, Product>();
            CreateMap<Product, ProductDto>();

            CreateMap<Product, GetAllProductsDto>();

            CreateMap<CartDto, Cart>();
            CreateMap<Cart, CartDto>();

            // Mapping from Order entity to OrderDto
            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.UserName, opt => opt.Ignore()); // Ignore userName property

            // Mapping from OrderDto to Order entity
            CreateMap<OrderDto, Order>();

            // Mapping from OrderDetail entity to OrderDetailDto
            CreateMap<OrderDetailDto, OrderDetail>();


            // Mapping from OrderDetailDto to OrderDetail entity
            CreateMap<OrderDetail, OrderDetailDto>();

            // Mapping from CartDetails entity to CartDetailsDto
            CreateMap<CartDetails, CartDetailsDto>();

            // Mapping from CartDetailsDto to CartDetails entity
            CreateMap<CartDetailsDto, CartDetails>();
        }
    }
}
