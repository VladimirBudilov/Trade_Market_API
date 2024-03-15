using AutoMapper;
using Business.Models;
using Data.Entities;
using System.Linq;

namespace Business
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<Receipt, ReceiptModel>()
                .ForMember(rm => rm.Id, r =>
                                                  r.MapFrom(x => x.Id))
                .ForMember(rm => rm.CustomerId, r =>
                                                  r.MapFrom(x => x.CustomerId))
                .ForMember(rm => rm.OperationDate, r =>
                                                  r.MapFrom(x => x.OperationDate))
                .ForMember(rm => rm.IsCheckedOut, r =>
                                                  r.MapFrom(x => x.IsCheckedOut))
                .ForMember(rm => rm.ReceiptDetailsIds, r =>
                                                  r.MapFrom(x => x.ReceiptDetails.Select(rd => rd.Id)))
                .ReverseMap();

            CreateMap<Product, ProductModel>()
                .ForMember(pm => pm.Id, p =>
                                   p.MapFrom(x => x.Id))
                .ForMember(pm => pm.ProductCategoryId, p =>
                                   p.MapFrom(x => x.ProductCategoryId))
                .ForMember(pm => pm.CategoryName, p =>
                                   p.MapFrom(x => x.Equals(null) ? string.Empty : x.Category.CategoryName))
                .ForMember(pm => pm.ProductName, p =>
                                   p.MapFrom(x => x.ProductName))
                .ForMember(pm => pm.Price, p =>
                                   p.MapFrom(x => x.Price))
                .ForMember(pm => pm.ReceiptDetailIds, p =>
                                   p.MapFrom(x => x.ReceiptDetails.Select(rd => rd.Id)))
                .ReverseMap();

            CreateMap<ReceiptDetail, ReceiptDetailModel>()
                .ForMember(rdm => rdm.Id, rd =>
                                                  rd.MapFrom(x => x.Id))
                .ForMember(rdm => rdm.ProductId, rd =>
                                                  rd.MapFrom(x => x.ProductId))
                .ForMember(rdm => rdm.ReceiptId, rd =>
                                                  rd.MapFrom(x => x.ReceiptId))
                .ForMember(rdm => rdm.UnitPrice, rd =>
                                                  rd.MapFrom(x => x.UnitPrice))
                .ForMember(rdm => rdm.DiscountUnitPrice, rd =>
                                                  rd.MapFrom(x => x.DiscountUnitPrice))
                .ForMember(rdm => rdm.Quantity, rd => 
                                                  rd.MapFrom(x => x.Quantity))
                .ReverseMap();

            CreateMap<Customer, CustomerModel>()
                .ForMember(cm => cm.Id, c =>
                                                     c.MapFrom(x => x.Id))
                .ForMember(cm => cm.Name, c =>
                                                     c.MapFrom(x => x.Person.Name))
                .ForMember(cm => cm.Surname, c =>
                                                     c.MapFrom(x => x.Person.Surname))
                .ForMember(cm => cm.DiscountValue, c =>
                                                     c.MapFrom(x => x.DiscountValue))
                .ForMember(cm => cm.BirthDate, c => 
                                                     c.MapFrom(x => x.Person.BirthDate))
                .ForMember(cm => cm.ReceiptsIds, c => 
                                                     c.MapFrom(x => x.Receipts.Select(r => r.Id)))
                .ReverseMap();

            CreateMap<ProductCategory, ProductCategoryModel>()
                .ForMember(pcm => pcm.Id, pc =>
                                                       pc.MapFrom(x => x.Id))
                .ForMember(pcm => pcm.CategoryName, pc =>
                                                       pc.MapFrom(x => x.CategoryName))
                .ForMember(pcm => pcm.ProductIds, pc =>
                                                       pc.MapFrom(x => x.Products.Select(p => p.Id)))
                .ReverseMap();

        }
    }
}