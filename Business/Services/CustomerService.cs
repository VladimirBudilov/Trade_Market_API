using System;
using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Validation;
using Data.Data;

namespace Business.Services
{
    public class CustomerService : BaseService, ICustomerService
    {
        public CustomerService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        public async Task AddAsync(CustomerModel model)
        {
            ValidationHelper.ValidateCustomerModel(model);
            await UnitOfWork.CustomerRepository.AddAsync(Mapper.Map<Customer>(model));
            await UnitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(int modelId)
        {
            await UnitOfWork.CustomerRepository.DeleteByIdAsync(modelId);
            await UnitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<CustomerModel>> GetAllAsync()
        {
            var allCustomers = await UnitOfWork.CustomerRepository.GetAllWithDetailsAsync();
            return Mapper.Map<IEnumerable<CustomerModel>>(allCustomers);
        }

        public async Task<CustomerModel> GetByIdAsync(int id)
        {
            var customer = await UnitOfWork.CustomerRepository.GetByIdWithDetailsAsync(id);
            return Mapper.Map<CustomerModel>(customer);
        }

        public async Task<IEnumerable<CustomerModel>> GetCustomersByProductIdAsync(int productId)
        {
            var allCustomers = await UnitOfWork.CustomerRepository.GetAllWithDetailsAsync();

            return allCustomers
                .Where(c => c.Receipts
                    .Any(r => r.ReceiptDetails
                        .Any(rd => rd.ProductId == productId)))
                .Select(c => Mapper.Map<CustomerModel>(c));
        }

        public async Task UpdateAsync(CustomerModel model)
        {
            ValidationHelper.ValidateCustomerModel(model);
            var customer = Mapper.Map<Customer>(model);
            var person = customer.Person;
            UnitOfWork.CustomerRepository.Update(customer);
            customer = await UnitOfWork.CustomerRepository.GetByIdWithDetailsAsync(model.Id);
            person.Id = customer.PersonId;
            UnitOfWork.PersonRepository.Update(person);
            await UnitOfWork.SaveAsync();
        }
    }
}