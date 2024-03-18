using AutoMapper;
using ProductAPI.Data.Dtos.Product;
using ProductAPI.Data.Models;

namespace ProductAPI.Data.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<CreateProductDto, Product>();
            CreateMap<Product, ReadProductDto>();
        }
    }
}
