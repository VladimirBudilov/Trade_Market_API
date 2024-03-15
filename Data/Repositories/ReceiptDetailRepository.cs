using Data.Data;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class ReceiptDetailRepository : Repository<ReceiptDetail>, IReceiptDetailRepository
    {
        public ReceiptDetailRepository(TradeMarketDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<ReceiptDetail>> GetAllWithDetailsAsync()
        {
            return await _entities
                .Include(r => r.Product).ThenInclude(p => p.Category)
                .Include(r => r.Receipt)
                .ThenInclude(r => r.Customer)
                .ThenInclude(c => c.Person)
                .ToListAsync();
        }
    }
}
