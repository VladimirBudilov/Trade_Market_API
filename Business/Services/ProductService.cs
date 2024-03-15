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
    public class ProductService : BaseService, IProductService
    {
        public ProductService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        public async Task AddAsync(ProductModel model)
        {
            ValidationHelper.ValidateProductModel(model);

            await UnitOfWork.ProductRepository.AddAsync(Mapper.Map<Product>(model));
            await UnitOfWork.SaveAsync();
        }

        public async Task AddCategoryAsync(ProductCategoryModel categoryModel)
        {
            ValidationHelper.ValidateCategoryModel(categoryModel);

            await UnitOfWork.ProductCategoryRepository.AddAsync(Mapper.Map<ProductCategory>(categoryModel));
            await UnitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(int modelId)
        {
            await UnitOfWork.ProductRepository.DeleteByIdAsync(modelId);
            await UnitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<ProductModel>> GetAllAsync()
        {
            var allProducts = await UnitOfWork.ProductRepository.GetAllWithDetailsAsync();
            return Mapper.Map<IEnumerable<ProductModel>>(allProducts);
        }

        public async Task<IEnumerable<ProductCategoryModel>> GetAllProductCategoriesAsync()
        {
            var allCategories = await UnitOfWork.ProductCategoryRepository.GetAllAsync();
            return Mapper.Map<IEnumerable<ProductCategoryModel>>(allCategories);
        }

        public async Task<IEnumerable<ProductModel>> GetByFilterAsync(FilterSearchModel filterSearch)
        {
            var products = Mapper.Map<IEnumerable<ProductModel>>(await UnitOfWork.ProductRepository.GetAllWithDetailsAsync());

            if (filterSearch == null)
            {
                return products;
            }

            var filteredProducts =
                products
                    .Where(p =>
                        (filterSearch.CategoryId == null || p.ProductCategoryId == filterSearch.CategoryId) &&
                        (filterSearch.MinPrice == null || p.Price >= filterSearch.MinPrice) &&
                        (filterSearch.MaxPrice == null || p.Price <= filterSearch.MaxPrice));
            return filteredProducts;
        }

        public async Task<ProductModel> GetByIdAsync(int id)
        {
            var product = await UnitOfWork.ProductRepository.GetByIdWithDetailsAsync(id);
            return Mapper.Map<ProductModel>(product);
        }

        public async Task<ProductCategoryModel> GetCategoryByIdAsync(int categoryId)
        {
            var category = await UnitOfWork.ProductCategoryRepository.GetByIdAsync(categoryId);
            return Mapper.Map<ProductCategoryModel>(category);
        }

        public async Task RemoveCategoryAsync(int categoryId)
        {
            await UnitOfWork.ProductCategoryRepository.DeleteByIdAsync(categoryId);
            await UnitOfWork.SaveAsync();
        }

        public async Task UpdateAsync(ProductModel model)
        {
            ValidationHelper.ValidateProductModel(model);
            UnitOfWork.ProductRepository.Update(Mapper.Map<Product>(model));
            await UnitOfWork.SaveAsync();
        }

        public async Task UpdateCategoryAsync(ProductCategoryModel categoryModel)
        {
            ValidationHelper.ValidateCategoryModel(categoryModel);
            UnitOfWork.ProductCategoryRepository.Update(Mapper.Map<ProductCategory>(categoryModel));
            await UnitOfWork.SaveAsync();
        }
    }
}