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
using Data.Entities;

namespace Business.Services
{
    public class ReceiptService : BaseService, IReceiptService
    {
        public ReceiptService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        public async Task AddAsync(ReceiptModel model)
        {
            ValidationHelper.ValidateReceiptModel(model);
            await UnitOfWork.ReceiptRepository.AddAsync(Mapper.Map<Receipt>(model));
            await UnitOfWork.SaveAsync();
        }

        public async Task AddProductAsync(int productId, int receiptId, int quantity)
        {
            var receipt = await UnitOfWork.ReceiptRepository.GetByIdWithDetailsAsync(receiptId);
            if (receipt == null)
            {
                throw new MarketException("Receipt does not exists");
            }

            if (receipt.ReceiptDetails != null &&
                receipt.ReceiptDetails.Any(r => r.ProductId == productId))
            {
                var receiptDetails = receipt.ReceiptDetails.First(r => r.ProductId == productId);
                receiptDetails.Quantity += quantity;
                UnitOfWork.ReceiptDetailRepository.Update(receiptDetails);
            }
            else
            {
                var product = await UnitOfWork.ProductRepository.GetByIdAsync(productId);
                if (product == null)
                {
                    throw new MarketException("Product does not exists");
                }

                await UnitOfWork.ReceiptDetailRepository.AddAsync(new ReceiptDetail
                {
                    ReceiptId = receiptId,
                    ProductId = productId,
                    UnitPrice = product.Price,
                    DiscountUnitPrice = product.Price - product.Price * receipt.Customer.DiscountValue / 100,
                    Quantity = quantity
                });
            }

            await UnitOfWork.SaveAsync();
        }

        public async Task CheckOutAsync(int receiptId)
        {
            var receipt = await UnitOfWork.ReceiptRepository.GetByIdAsync(receiptId);
            receipt.IsCheckedOut = true;
            UnitOfWork.ReceiptRepository.Update(receipt);
            await UnitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(int modelId)
        {
            var receipt = await UnitOfWork.ReceiptRepository.GetByIdWithDetailsAsync(modelId);
            if (receipt == null)
            {
                throw new MarketException("Receipt does not exists");
            }

            foreach (var details in receipt.ReceiptDetails)
            {
                UnitOfWork.ReceiptDetailRepository.Delete(details);
            }

            await UnitOfWork.ReceiptRepository.DeleteByIdAsync(modelId);
            await UnitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<ReceiptModel>> GetAllAsync()
        {
            var allReceipts = await UnitOfWork.ReceiptRepository.GetAllWithDetailsAsync();
            return Mapper.Map<IEnumerable<ReceiptModel>>(allReceipts);
        }

        public async Task<ReceiptModel> GetByIdAsync(int id)
        {
            var receipt = await UnitOfWork.ReceiptRepository.GetByIdWithDetailsAsync(id);
            return Mapper.Map<ReceiptModel>(receipt);
        }

        public async Task<IEnumerable<ReceiptDetailModel>> GetReceiptDetailsAsync(int receiptId)
        {
            var receipt = await UnitOfWork.ReceiptRepository.GetByIdWithDetailsAsync(receiptId);
            var receiptDetails = receipt.ReceiptDetails;
            return Mapper.Map<IEnumerable<ReceiptDetailModel>>(receiptDetails);
        }

        public async Task<IEnumerable<ReceiptModel>> GetReceiptsByPeriodAsync(DateTime startDate, DateTime endDate)
        {
            var receipts = await UnitOfWork.ReceiptRepository.GetAllWithDetailsAsync();
            
            var receiptsByPeriod = receipts
                .Where(r => r.OperationDate >= startDate && r.OperationDate <= endDate);
            return Mapper.Map<IEnumerable<ReceiptModel>>(receiptsByPeriod);
        }

        public async Task RemoveProductAsync(int productId, int receiptId, int quantity)
        {
            var receipt = await UnitOfWork.ReceiptRepository.GetByIdWithDetailsAsync(receiptId);
            if (receipt.ReceiptDetails == null || !receipt.ReceiptDetails.Any(r => r.ProductId == productId))
                throw new MarketException("Receipt does not have this product");

            var receiptDetails = receipt.ReceiptDetails.First(r => r.ProductId == productId);
            receiptDetails.Quantity -= quantity;
            if (receiptDetails.Quantity == 0)
            {
                UnitOfWork.ReceiptDetailRepository.Delete(receiptDetails);
            }
            else
            {
                UnitOfWork.ReceiptRepository.Update(receipt);
            }

            await UnitOfWork.SaveAsync();
        }

        public async Task<decimal> ToPayAsync(int receiptId)
        {
            var receipt = await UnitOfWork.ReceiptRepository.GetByIdWithDetailsAsync(receiptId);
            return receipt.ReceiptDetails.Sum(r => r.Quantity * r.DiscountUnitPrice);
        }

        public async Task UpdateAsync(ReceiptModel model)
        {
            ValidationHelper.ValidateReceiptModel(model);
            UnitOfWork.ReceiptRepository.Update(Mapper.Map<Receipt>(model));
            await UnitOfWork.SaveAsync();
        }
    }
}