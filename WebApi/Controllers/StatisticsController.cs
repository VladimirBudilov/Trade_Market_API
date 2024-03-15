using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Interfaces;
using Business.Models;
using Microsoft.AspNetCore.Mvc;
using WebApi.Filters;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[ExceptionsFilter]
public class StatisticsController : ControllerBase
{
    private readonly IStatisticService _statisticService;
    
    public StatisticsController(IStatisticService statisticService)
    {
        _statisticService = statisticService;
    }
    
    [HttpGet("popularProducts")]
    public async Task<ActionResult<IEnumerable<ProductModel>>> GetPopularProducts([FromQuery]int productCount)
    {
        var products = await _statisticService.GetMostPopularProductsAsync(productCount);
        return Ok(products);
    }

    [HttpGet("customer/{id}/{productCount}")]
    public async Task<ActionResult<IEnumerable<ProductModel>>> GetCustomerFavouriteProducts(int id, int productCount)
    {
        var products =
            await _statisticService.GetCustomersMostPopularProductsAsync(id, productCount);
        return Ok(products);
    }
    
    [HttpGet("activity/{customerCount}")]
    public async Task<ActionResult<IEnumerable<CustomerModel>>>
        GetMostActiveCustomers(int customerCount, DateTime startDate, DateTime endDate)
    {
        var customers = await _statisticService.GetMostValuableCustomersAsync(customerCount, startDate, endDate);
        return Ok(customers);
    }
    
    [HttpGet("income/{categoryId}")]
    public async Task<ActionResult<decimal>> GetIncomeOfCategory(int categoryId, DateTime startDate, DateTime endDate)
    {
        var income = await _statisticService.GetIncomeOfCategoryInPeriod(categoryId, startDate, endDate);
        return Ok(income);
    }

}