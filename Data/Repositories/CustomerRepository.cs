using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Data;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class CustomerRepository : Repository<Customer>, ICustomerRepository
{
    public CustomerRepository(TradeMarketDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Customer>> GetAllWithDetailsAsync()
    {
        return await _entities
            .Include(c => c.Person)
            .Include(c => c.Receipts)
            .ThenInclude(r => r.ReceiptDetails)
            .ThenInclude(rd => rd.Product)
            .ThenInclude(p => p.Category)
            .ToListAsync();
    }

    public async Task<Customer> GetByIdWithDetailsAsync(int id)
    {
        if (await GetByIdAsync(id) == null)
        {
            return null;
        }
        return await _entities
            .Include(c => c.Person)
            .Include(c => c.Receipts)
            .ThenInclude(r => r.ReceiptDetails)
            .ThenInclude(rd => rd.Product)
            .ThenInclude(p => p.Category)
            .FirstOrDefaultAsync(c => c.Id == id);
    }
    
    public override void Update(Customer entity)
    {
        var currentCustomer = GetByIdWithDetailsAsync(entity.Id).Result;
        currentCustomer.Receipts = entity.Receipts;
        currentCustomer.DiscountValue = entity.DiscountValue;
    }
}