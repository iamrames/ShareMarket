using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Share.API.IRepository;
using Share.API.Models;

namespace Share.API.Repository
{
    public class LiveTradingDataHistoryRepository : ILiveTradingDataHistoryRepository
    {
        private readonly DatabaseContext _context;
        public LiveTradingDataHistoryRepository(DatabaseContext context)
        {
            _context = context;
        }
        public async Task<LiveTradingDataHistory> GetDataByCompanyIdAsync(int companyId)
        {
            return await _context.LiveTradingDataHistory.Where(x => x.CompanyId == companyId).Include(x => x.Company).OrderByDescending(x => x.UpdatedDate).FirstOrDefaultAsync();
        }

        public async Task<LiveTradingDataHistory> GetDataByCompanySymbolAsync(string symbol)
        {
            return await _context.LiveTradingDataHistory.Where(x => x.Symbol.ToLower() == symbol.ToLower()).Include(x => x.Company).OrderByDescending(x => x.UpdatedDate).FirstOrDefaultAsync();
        }

        public async Task<List<LiveTradingDataHistory>> GetDataDescAsync()
        {
            return await _context.LiveTradingDataHistory.Include(x => x.Company).OrderByDescending(x => x.UpdatedDate).ToListAsync();
        }

        public async Task<Target> GetTargetLevel(int companyId, decimal ltp)
        {
            return await _context.Targets
                        .Where(x => x.CompanyId == companyId && x.Amount < ltp)
                        .OrderByDescending(x => x.TargetDate)
                        .FirstOrDefaultAsync();
        }
    }
}