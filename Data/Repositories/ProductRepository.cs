using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Data;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class ProductRepository : Repository<Product>, IProductRepository
{
    public ProductRepository(TradeMarketDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Product>> GetAllWithDetailsAsync()
    {
        return await _entities
            .Include(p => p.Category)
            .Include(p => p.ReceiptDetails)
            .ThenInclude(c => c.Receipt)
            .ThenInclude(r => r.Customer)
            .ThenInclude(c => c.Person)
            .ToListAsync();
    }

    public async Task<Product> GetByIdWithDetailsAsync(int id)
    {
        if(await GetByIdAsync(id) == null)
        {
            return null;
        }

        return await _entities
            .Include(p => p.Category)
            .Include(p => p.ReceiptDetails)
            .ThenInclude(c => c.Receipt)
            .ThenInclude(r => r.Customer)
            .ThenInclude(c => c.Person)
            .FirstOrDefaultAsync(p => p.Id == id);
    }
}