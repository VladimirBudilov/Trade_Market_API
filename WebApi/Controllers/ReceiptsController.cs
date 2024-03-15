using System;
using System.Linq;
using System.Threading.Tasks;
using Business.Interfaces;
using Business.Models;
using Microsoft.AspNetCore.Mvc;
using WebApi.Filters;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[ExceptionsFilter]
public class ReceiptsController : ControllerBase
{
    private readonly IReceiptService _receiptService;

    public ReceiptsController(IReceiptService receiptService)
    {
        _receiptService = receiptService;
    }

    [HttpGet]
    public async Task<ActionResult> GetAllReceipts()
    {
        var receipts = await _receiptService.GetAllAsync();
        return Ok(receipts);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetReceiptById(int id)
    {
        var receipt = await _receiptService.GetByIdAsync(id);
        if (receipt == null)
        {
            return NotFound();
        }

        return Ok(receipt);
    }

    [HttpGet("{id}/details")]
    public async Task<ActionResult> GetReceiptDetails(int id)
    {
        var details = await _receiptService.GetReceiptDetailsAsync(id);
        return Ok(details);
    }

    [HttpGet("period")]
    public async Task<ActionResult> GetReceiptsByPeriod(DateTime startDate, DateTime endDate)
    {
        var receipts = await _receiptService.GetReceiptsByPeriodAsync(startDate, endDate);
        return Ok(receipts);
    }

    [HttpPost]
    public async Task<ActionResult> AddReceipt([FromBody] ReceiptModel value)
    {
        await _receiptService.AddAsync(value);
        return Ok(value);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateReceipt(int id, [FromBody] ReceiptModel value)
    {
        if (value == null || id != value.Id)
        {
            return BadRequest();
        }

        await _receiptService.UpdateAsync(value);
        return Ok(value);
    }

    [HttpPut("{id}/products/add/{productId}/{quantity}")]
    public async Task<ActionResult> AddProductToReceipt(int id, int productId, int quantity)
    {
        await _receiptService.AddProductAsync(productId, id, quantity);
        return Ok();
    }

    [HttpPut("{id}/products/remove/{productId}/{quantity}")]
    public async Task<ActionResult> RemoveProductFromReceipt(int id, int productId, int quantity)
    {
        await _receiptService.RemoveProductAsync(productId, id, quantity);
        return Ok();
    }

    [HttpPut("{id}/checkout")]
    public async Task<ActionResult> CheckoutReceipt(int id)
    {
        await _receiptService.CheckOutAsync(id);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteReceipt(int id)
    {
        await _receiptService.DeleteAsync(id);
        return Ok();
    }

    [HttpGet("{id}/sum")]
    public async Task<ActionResult> GetReceiptSum(int id)
    {
        var receiptDetails = await _receiptService.GetReceiptDetailsAsync(id);
        if (receiptDetails == null)
        {
            return NotFound();
        }

        return Ok(receiptDetails.Sum(x => x.DiscountUnitPrice * x.Quantity));
    }
}