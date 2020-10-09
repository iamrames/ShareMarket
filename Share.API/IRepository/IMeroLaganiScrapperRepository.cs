using System.Collections.Generic;
using System.Threading.Tasks;
using Share.API.Common.Results;
using Share.API.Dtos;
using Share.API.Models;

namespace Share.API.IRepository
{
    public interface IMeroLaganiScrapperRepository
    {
        Task<DataResult> SeedMeroLaganiLiveTradingData();
        Task<List<LiveTradingDataDto>> GetAllLiveTradingDataAsync();
    }
}