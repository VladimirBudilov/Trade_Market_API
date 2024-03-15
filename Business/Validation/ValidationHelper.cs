using System.Linq;
using Business.Models;

namespace Business.Validation;

public static class ValidationHelper
{
    public static void ValidateCustomerModel(CustomerModel model)
    {
        if (model == null)
        {
            throw new MarketException("Customer model is null");
        }

        if (model.DiscountValue < 0 || model.DiscountValue > 100)
        {
            throw new MarketException("Discount value is not valid. It should be between 0 and 100.");
        }

        if (string.IsNullOrEmpty(model.Name) || string.IsNullOrWhiteSpace(model.Name) ||
            string.IsNullOrEmpty(model.Surname) || string.IsNullOrWhiteSpace(model.Surname)
           )
        {
            throw new MarketException("Name and surname cannot be empty or null.");
        }

        if (model.BirthDate == default || model.BirthDate > System.DateTime.Now ||
            model.BirthDate < System.DateTime.Now.AddYears(-150))
        {
            throw new MarketException("Birth date is not valid. It should be between 150 years ago and today.");
        }
    }

    public static void ValidateProductModel(ProductModel model)
    {
        if (model == null)
        {
            throw new MarketException("Product model is null");
        }

        if (model.Price < 0)
        {
            throw new MarketException("Price cannot be less than 0.");
        }

        if (string.IsNullOrEmpty(model.ProductName) || string.IsNullOrWhiteSpace(model.ProductName))
        {
            throw new MarketException("Product name cannot be empty or null.");
        }
    }

    public static void ValidateCategoryModel(ProductCategoryModel categoryModel)
    {
        if (categoryModel == null)
        {
            throw new MarketException("Category model is null");
        }

        if (string.IsNullOrEmpty(categoryModel.CategoryName)
            || string.IsNullOrWhiteSpace(categoryModel.CategoryName))
        {
            throw new MarketException("Category name cannot be empty or null.");
        }
    }

    public static void ValidateReceiptModel(ReceiptModel model)
    {
        if (model == null)
        {
            throw new MarketException("Receipt model is null");
        }

        if (model.CustomerId <= 0)
        {
            throw new MarketException("Customer id is not valid.");
        }

        if (model.OperationDate == default || model.OperationDate > System.DateTime.Now
           || model.OperationDate < System.DateTime.Now.AddYears(-150))
        {
            throw new MarketException("Operation date is not valid. It should be in the past.");
        }
    }
}