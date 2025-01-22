using AutoMapper;
using BAL.DTOs;
using DAL.EF.Entities;
using DAL.Interfaces;
using DAL.Repos;
using System.Collections.Generic;

namespace BAL.Services
{
    public class ProductService
    {
        private static readonly IRepo<Product, int, string> _productRepo = new ProductRepo();

        private static IMapper InitializeMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Product, ProductDTO>().ReverseMap();
                cfg.CreateMap<Customer, CustomerDTO>().ReverseMap();
            });
            return new Mapper(config);
        }

        public static List<ProductDTO> GetAllProducts()
        {
            var products = _productRepo.Get();
            var mapper = InitializeMapper();
            return mapper.Map<List<ProductDTO>>(products);
        }

        public static ProductDTO GetProductById(int id)
        {
            var product = _productRepo.Get(id);
            var mapper = InitializeMapper();
            return product != null ? mapper.Map<ProductDTO>(product) : null;
        }

        public static string CreateProduct(ProductDTO productDto)
        {
            var mapper = InitializeMapper();
            var product = mapper.Map<Product>(productDto);
            return _productRepo.Create(product);
        }

        public static string UpdateProduct(ProductDTO productDto)
        {
            var mapper = InitializeMapper();
            var product = mapper.Map<Product>(productDto);
            return _productRepo.Update(product);
        }

        public static bool DeleteProduct(int id)
        {
            return _productRepo.Delete(id);
        }
    }
}
