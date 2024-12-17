using AutoMapper;
using BAL.DTOs;

using DAL.EF.Entities;
using DAL.Interfaces;
using DAL.Repos;
using System.Collections.Generic;

namespace BAL.Services
{
    public class CustomerService
    {
        private static readonly IRepo<Customer, int, string> _customerRepo = new CustomerRepo();

        private static IMapper InitializeMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Customer, CustomerDTO>().ReverseMap();
                cfg.CreateMap<Product, ProductDTO>().ReverseMap();
            });
            return new Mapper(config);
        }

        public static List<CustomerDTO> GetAllCustomers()
        {
            var customers = _customerRepo.Get();
            var mapper = InitializeMapper();
            return mapper.Map<List<CustomerDTO>>(customers);
        }

        public static CustomerDTO GetCustomerById(int id)
        {
            var customer = _customerRepo.Get(id);
            var mapper = InitializeMapper();
            return customer != null ? mapper.Map<CustomerDTO>(customer) : null;
        }

        public static string CreateCustomer(CustomerDTO customerDto)
        {
            var mapper = InitializeMapper();
            var customer = mapper.Map<Customer>(customerDto);
            return _customerRepo.Create(customer);
        }

        public static string UpdateCustomer(CustomerDTO customerDto)
        {
            var mapper = InitializeMapper();
            var customer = mapper.Map<Customer>(customerDto);
            return _customerRepo.Update(customer);
        }

        public static bool DeleteCustomer(int id)
        {
            return _customerRepo.Delete(id);
        }
    }
}
