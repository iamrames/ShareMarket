using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Share.API.Common.Results;
using Share.API.Dtos;
using Share.API.Enums;
using Share.API.IRepository;

namespace Share.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LiveTradingDataHistoryController: ControllerBase
    {
        private readonly ILiveTradingDataHistoryRepository _repo;
        public LiveTradingDataHistoryController(ILiveTradingDataHistoryRepository repo)
        {
            _repo = repo;
        }

        [HttpGet("[action]")]
        public async Task<DataResult<List<LiveTradingDataHistoryDto>>> Get()
        {
            try
            {
                var data = await _repo.GetDataDescAsync();
                if(data == null)
                {
                    return new DataResult<List<LiveTradingDataHistoryDto>> { ResultType = ResultTypeOption.Success, Message = "No Data Found" };
                }
                List<LiveTradingDataHistoryDto> result = new List<LiveTradingDataHistoryDto>();
                foreach (var item in data)
                {
                    var targetLevel = await _repo.GetTargetLevel(item.CompanyId, item.LTP);
                    
                    result.Add(new LiveTradingDataHistoryDto {
                        Id = item.Id,
                        CompanyId = item.CompanyId,
                        Symbol = item.Symbol,
                        LTP = item.LTP,
                        LTV =item.LTV,
                        ChangePercentage = item.ChangePercentage,
                        High = item.High,
                        Low = item.Low,
                        Qty = item.Qty,
                        Source = item.Source,
                        EntryDate = item.EntryDate,
                        UpdatedDate = item.UpdatedDate,
                        TargetLevel = targetLevel?.Level,
                        CompanyName = item.Company.Name,
                    });
                }
                return new DataResult<List<LiveTradingDataHistoryDto>> { ResultType = ResultTypeOption.Success, Data = result};
            }
            catch (Exception ex)
            {
                return new DataResult<List<LiveTradingDataHistoryDto>> { ResultType = ResultTypeOption.Failed, Message = ex.Message };
            }
        }

        [HttpGet("[action]/{companyId}")]
        public async Task<DataResult<LiveTradingDataHistoryDto>> GetDataByCompanyId(int companyId)
        {
            try
            {
                var data = await _repo.GetDataByCompanyIdAsync(companyId);
                if(data == null)
                {
                    return new DataResult<LiveTradingDataHistoryDto> { ResultType = ResultTypeOption.Success, Message = "No Data Found" };
                }
                var targetLevel = await _repo.GetTargetLevel(data.CompanyId, data.LTP);
                LiveTradingDataHistoryDto result = new LiveTradingDataHistoryDto {
                    Id = data.Id,
                    CompanyId = data.CompanyId,
                    Symbol = data.Symbol,
                    LTP = data.LTP,
                    LTV =data.LTV,
                    ChangePercentage = data.ChangePercentage,
                    High = data.High,
                    Low = data.Low,
                    Qty = data.Qty,
                    Source = data.Source,
                    EntryDate = data.EntryDate,
                    UpdatedDate = data.UpdatedDate,
                    TargetLevel = targetLevel?.Level,
                    CompanyName = data.Company.Name,
                };
                
                return new DataResult<LiveTradingDataHistoryDto> { ResultType = ResultTypeOption.Success, Data = result};
            }
            catch (Exception ex)
            {
                return new DataResult<LiveTradingDataHistoryDto> { ResultType = ResultTypeOption.Failed, Message = ex.Message };
            }
        }

        [HttpGet("[action]/{symbol}")]
        public async Task<DataResult<LiveTradingDataHistoryDto>> GetDataByCompanySymbol(string symbol)
        {
            try
            {
                var data = await _repo.GetDataByCompanySymbolAsync(symbol);
                if(data == null)
                {
                    return new DataResult<LiveTradingDataHistoryDto> { ResultType = ResultTypeOption.Success, Message = "No Data Found" };
                }
                var targetLevel = await _repo.GetTargetLevel(data.CompanyId, data.LTP);
                LiveTradingDataHistoryDto result = new LiveTradingDataHistoryDto {
                    Id = data.Id,
                    CompanyId = data.CompanyId,
                    Symbol = data.Symbol,
                    LTP = data.LTP,
                    LTV =data.LTV,
                    ChangePercentage = data.ChangePercentage,
                    High = data.High,
                    Low = data.Low,
                    Qty = data.Qty,
                    Source = data.Source,
                    EntryDate = data.EntryDate,
                    UpdatedDate = data.UpdatedDate,
                    TargetLevel = targetLevel?.Level,
                    CompanyName = data.Company.Name,
                };
                
                return new DataResult<LiveTradingDataHistoryDto> { ResultType = ResultTypeOption.Success, Data = result};
            }
            catch (Exception ex)
            {
                return new DataResult<LiveTradingDataHistoryDto> { ResultType = ResultTypeOption.Failed, Message = ex.Message };
            }
        }
    }
}