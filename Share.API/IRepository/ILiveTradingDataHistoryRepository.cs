using System.Collections.Generic;
using System.Threading.Tasks;
using Share.API.Dtos;
using Share.API.Models;

namespace Share.API.IRepository
{
    public interface ILiveTradingDataHistoryRepository
    {
        Task<List<LiveTradingDataHistory>> GetDataDescAsync();
        Task<LiveTradingDataHistory> GetDataByCompanyIdAsync(int companyId);
        Task<LiveTradingDataHistory> GetDataByCompanySymbolAsync(string symbol);
        Task<Target> GetTargetLevel(int companyId, decimal ltp);
    }
}