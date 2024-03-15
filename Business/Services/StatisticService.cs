using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Validation;
using FluentAssertions;

namespace Business.Services
{
    public class StatisticService : BaseService, IStatisticService
    {
        public StatisticService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        public async Task<IEnumerable<ProductModel>> GetCustomersMostPopularProductsAsync(int productCount,
            int customerId)
        {
            var receipts = await UnitOfWork.ReceiptRepository.GetAllWithDetailsAsync();
            var customerReceipts = receipts.Where(r => r.CustomerId == customerId);
            
            var popularProducts = customerReceipts
                .SelectMany(r => r.ReceiptDetails)
                .GroupBy(r => r.Product)
                .OrderByDescending(g =>
                    g.Sum(rd => rd.Quantity))
                .Select(g => g.Key)
                .Take(productCount);

            return Mapper.Map<IEnumerable<ProductModel>>(popularProducts);
        }

        public async Task<decimal> GetIncomeOfCategoryInPeriod(int categoryId, DateTime startDate, DateTime endDate)
        {
            var receipts = await UnitOfWork.ReceiptRepository.GetAllWithDetailsAsync();
            var receiptsInPeriod = receipts.Where(r => r.OperationDate >= startDate && r.OperationDate <= endDate);
            
            var categoryReceipts = receiptsInPeriod
                .SelectMany(r => r.ReceiptDetails)
                .Where(rd => rd.Product.ProductCategoryId == categoryId);
            var income = categoryReceipts.Sum(rd => rd.DiscountUnitPrice * rd.Quantity);
            return income;
        }

        public async Task<IEnumerable<ProductModel>> GetMostPopularProductsAsync(int productCount)
        {
            var receiptsDetails = await UnitOfWork.ReceiptDetailRepository.GetAllWithDetailsAsync();
            
            var popularProducts = receiptsDetails
                .GroupBy(r => r.Product)
                .OrderByDescending(g =>
                    g.Sum(rd => rd.Quantity))
                .Select(g => g.Key)
                .Take(productCount);
            return Mapper.Map<IEnumerable<ProductModel>>(popularProducts);

        }

        public async Task<IEnumerable<CustomerActivityModel>> GetMostValuableCustomersAsync(int customerCount,
            DateTime startDate, DateTime endDate)
        {
            var receipts = await UnitOfWork.ReceiptRepository.GetAllWithDetailsAsync();
            var receiptsInPeriod = receipts.Where(r => r.OperationDate >= startDate && r.OperationDate <= endDate);
            
            var customerActivities = receiptsInPeriod
                .GroupBy(r => r.Customer)
                .Select(g => new CustomerActivityModel
                {
                    CustomerId = g.Key.Id,
                    CustomerName = g.Key.Person.Name + " " + g.Key.Person.Surname,
                    ReceiptSum = g.Sum(r => r.ReceiptDetails.Sum(rd => rd.DiscountUnitPrice * rd.Quantity))
                })
                .OrderByDescending(ca => ca.ReceiptSum)
                .Take(customerCount);
            
            return customerActivities;
        }
    }
}