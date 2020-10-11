using System.Security.Cryptography.X509Certificates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AngleSharp;
using Microsoft.EntityFrameworkCore;
using Share.API.Common.Results;
using Share.API.Enums;
using Share.API.IRepository;
using Share.API.Models;
using Share.API.Dtos;

namespace Share.API.Repository
{
    public class MeroLaganiScrapperRepository : IMeroLaganiScrapperRepository
    {
        private readonly DatabaseContext _context;
        public MeroLaganiScrapperRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<List<LiveTradingDataDto>> GetAllLiveTradingDataAsync()
        {
            var data = await _context.LiveTradingData.Where(x => x.Source == ScrapingSource.MeroLagani).AsNoTracking().OrderBy(x => x.Symbol).ToListAsync();
            List<LiveTradingDataDto> result = new List<LiveTradingDataDto>();
            foreach (var item in data)
            {
                var targetLevel = await _context.Targets
                    .Where(x => x.CompanyId == item.CompanyId && x.Amount < item.LTP)
                    .OrderByDescending(x => x.TargetDate)
                    .FirstOrDefaultAsync();

                result.Add(new LiveTradingDataDto {
                    CompanyId = item.Id,
                    Symbol = item.Symbol,
                    LTP = item.LTP,
                    LTV = item.LTV,
                    ChangePercentage = item.ChangePercentage,
                    High = item.High,
                    Low = item.Low,
                    Open = item.Open,
                    Qty = item.Qty,
                    EntryDate = item.EntryDate,
                    UpdatedDate = item.UpdatedDate,
                    Source = item.Source,
                    TargetLevel = targetLevel?.Level
                });
            }
            return result;
        }

        public async Task<DataResult> SeedMeroLaganiLiveTradingData()
        {
            // Load default configuration
            var config = Configuration.Default.WithDefaultLoader();
            // Create a new browsing context
            var context = BrowsingContext.New(config);
            // This is where the HTTP request happens, returns <IDocument> that // we can query later
            var document = await context.OpenAsync("https://merolagani.com/LatestMarket.aspx");
            var heading = document.QuerySelectorAll("#ctl00_ContentPlaceHolder1_LiveTrading thead th").Where(x => x.TextContent.Length > 0).ToArray();
            var shareData = document.QuerySelectorAll("#ctl00_ContentPlaceHolder1_LiveTrading tbody tr").ToArray();
            DateTime currentDateTime = DateTime.Now;
            List<LiveTradingData> liveTradingData = new List<LiveTradingData>();
            List<LiveTradingDataHistory> liveTradingDataHistory = new List<LiveTradingDataHistory>();

            using (var db = _context.Database.BeginTransaction())
            {
                try
                {
                    foreach (var item in shareData)
                    {
                        #region LiveTradingData
                        var prevLiveTradingData = (await _context.LiveTradingData.Where(x => x.Symbol.ToLower().Equals(item.ChildNodes[(int)ShareDataOptions.Symbol].TextContent.ToLower())).FirstOrDefaultAsync());
                        if(prevLiveTradingData == null)
                        {
                            prevLiveTradingData = new LiveTradingData();
                            prevLiveTradingData.EntryDate = currentDateTime;
                        }
                        prevLiveTradingData.CompanyId = (await _context.Company.Where(x => x.Symbol.ToLower().Equals(item.ChildNodes[(int)ShareDataOptions.Symbol].TextContent.ToLower())).FirstOrDefaultAsync()).Id;
                        prevLiveTradingData.Symbol = item.ChildNodes[(int)ShareDataOptions.Symbol].TextContent;
                        prevLiveTradingData.LTP = Decimal.Parse(item.ChildNodes[(int)ShareDataOptions.LTP].TextContent);
                        prevLiveTradingData.LTV = Decimal.Parse(item.ChildNodes[(int)ShareDataOptions.LTV].TextContent);
                        prevLiveTradingData.ChangePercentage = Decimal.Parse(item.ChildNodes[(int)ShareDataOptions.ChangePercentage].TextContent);
                        prevLiveTradingData.High = Decimal.Parse(item.ChildNodes[(int)ShareDataOptions.High].TextContent);
                        prevLiveTradingData.Low = Decimal.Parse(item.ChildNodes[(int)ShareDataOptions.Low].TextContent);
                        prevLiveTradingData.Open = Decimal.Parse(item.ChildNodes[(int)ShareDataOptions.Open].TextContent);
                        prevLiveTradingData.Qty = Decimal.Parse(item.ChildNodes[(int)ShareDataOptions.Qty].TextContent);
                        prevLiveTradingData.UpdatedDate = currentDateTime;
                        prevLiveTradingData.Source = ScrapingSource.MeroLagani;
                        liveTradingData.Add(prevLiveTradingData);
                        #endregion

                        #region LiveTradingDataHistory
                        // Checking if the same data is repeated today or not.
                        var prevLiveTradingDataHistory = await _context.LiveTradingDataHistory
                            .Where(x => x.Symbol.ToLower().Equals(prevLiveTradingData.Symbol.ToLower()) && x.LTP == prevLiveTradingData.LTP && x.LTV == prevLiveTradingData.LTV
                                && x.ChangePercentage == prevLiveTradingData.ChangePercentage && x.High == prevLiveTradingData.High && x.Low == prevLiveTradingData.Low
                                && x.Qty == prevLiveTradingData.Qty && x.Source == ScrapingSource.MeroLagani 
                                && x.UpdatedDate.Date == currentDateTime.Date)
                            .OrderByDescending(x => x.UpdatedDate).FirstOrDefaultAsync();

                        if(prevLiveTradingDataHistory == null)
                        {
                            prevLiveTradingDataHistory = new LiveTradingDataHistory();
                            prevLiveTradingDataHistory.CompanyId = prevLiveTradingData.CompanyId;
                            prevLiveTradingDataHistory.Symbol = prevLiveTradingData.Symbol;
                            prevLiveTradingDataHistory.LTP = prevLiveTradingData.LTP;
                            prevLiveTradingDataHistory.LTV = prevLiveTradingData.LTV;
                            prevLiveTradingDataHistory.ChangePercentage = prevLiveTradingData.ChangePercentage;
                            prevLiveTradingDataHistory.High = prevLiveTradingData.High;
                            prevLiveTradingDataHistory.Low = prevLiveTradingData.Low;
                            prevLiveTradingDataHistory.Open = prevLiveTradingData.Open;
                            prevLiveTradingDataHistory.Qty = prevLiveTradingData.Qty;
                            prevLiveTradingDataHistory.EntryDate = currentDateTime;
                            prevLiveTradingDataHistory.UpdatedDate = currentDateTime;
                            prevLiveTradingDataHistory.Source = ScrapingSource.MeroLagani;
                        }
                        else
                        {
                            DateTime latestDate = prevLiveTradingDataHistory.UpdatedDate;
                            // This condition is used to find if the given value is the latest updated value or not, if there exist other value after 'prevLiveTradingDataHistory'
                            // then new data is added else the date is updated to current date
                            if(await _context.LiveTradingDataHistory.AnyAsync(x => x.UpdatedDate > latestDate && x.Source == ScrapingSource.MeroLagani))
                            {
                                prevLiveTradingDataHistory.UpdatedDate = currentDateTime;
                            }
                            else 
                            {
                                prevLiveTradingDataHistory.CompanyId = prevLiveTradingData.CompanyId;
                                prevLiveTradingDataHistory.Symbol = prevLiveTradingData.Symbol;
                                prevLiveTradingDataHistory.LTP = prevLiveTradingData.LTP;
                                prevLiveTradingDataHistory.LTV = prevLiveTradingData.LTV;
                                prevLiveTradingDataHistory.ChangePercentage = prevLiveTradingData.ChangePercentage;
                                prevLiveTradingDataHistory.High = prevLiveTradingData.High;
                                prevLiveTradingDataHistory.Low = prevLiveTradingData.Low;
                                prevLiveTradingDataHistory.Open = prevLiveTradingData.Open;
                                prevLiveTradingDataHistory.Qty = prevLiveTradingData.Qty;
                                prevLiveTradingDataHistory.UpdatedDate = currentDateTime;
                                prevLiveTradingDataHistory.Source = ScrapingSource.MeroLagani;
                            }
                        }

                        liveTradingDataHistory.Add(prevLiveTradingDataHistory);
                        #endregion
                    }

                    _context.LiveTradingData.UpdateRange(liveTradingData);
                    await _context.SaveChangesAsync();

                    _context.LiveTradingDataHistory.UpdateRange(liveTradingDataHistory);
                    await _context.SaveChangesAsync();
                    await db.CommitAsync();
                } 
                catch(DbUpdateException ex) 
                {
                    db.Rollback();
                    return new DataResult { ResultType = ResultTypeOption.Failed, Message = ex.Message };
                }
                catch(Exception ex)
                {
                    db.Rollback();
                    return new DataResult { ResultType = ResultTypeOption.Failed, Message = ex.Message };
                }
            }
            return new DataResult { ResultType = ResultTypeOption.Success, Message = "Successfully Scraped Mero Lagani" };
        }
    }
}