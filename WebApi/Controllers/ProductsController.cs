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
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductModel>>> GetProducts([FromQuery] FilterSearchModel request)
    {
        var products = await _productService.GetByFilterAsync(request);
        if (products == null)
        {
            return NotFound();
        }

        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductModel>> GetProductById(int id)
    {
        var product = await _productService.GetByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        return Ok(product);
    }

    [HttpGet("categories")]
    public async Task<ActionResult<IEnumerable<ProductCategoryModel>>> GetCategories()
    {
        var categories = await _productService.GetAllProductCategoriesAsync();
        return Ok(categories);
    }

    [HttpPost]
    public async Task<ActionResult> AddProduct([FromBody] ProductModel value)
    {
        await _productService.AddAsync(value);
        return Ok(value);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        await _productService.DeleteAsync(id);
        return Ok();
    }

    [HttpPost("categories")]
    public async Task<ActionResult> AddCategory([FromBody] ProductCategoryModel value)
    {
        await _productService.AddCategoryAsync(value);
        return Ok(value);
    }


    [HttpDelete("categories/{id}")]
    public async Task<ActionResult> DeleteCategory(int id)
    {
        await _productService.RemoveCategoryAsync(id);
        return Ok();
    }

    [HttpPut("categories/{id}")]
    public async Task<ActionResult> UpdateCategory(int Id, [FromBody] ProductCategoryModel value)
    {
        if (value == null)
        {
            return BadRequest();
        }
        value.Id = Id;
        await _productService.UpdateCategoryAsync(value);
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateProduct(int id, [FromBody] ProductModel value)
    {
        if(value == null)
        {
            return BadRequest();
        }
        value.Id = id;
        await _productService.UpdateAsync(value);
        return Ok();
    }
}