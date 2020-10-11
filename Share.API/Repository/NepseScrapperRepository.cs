using System.Diagnostics.Contracts;
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

namespace Share.API.Repository
{
    public class NepseScrapperRepository : INepseScrapperRepository
    {
        private readonly DatabaseContext _context;
        public NepseScrapperRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<DataResult> SeedAllCompanies()
        {
            // Load default configuration
            var config = Configuration.Default.WithDefaultLoader();
            // Create a new browsing context
            var context = BrowsingContext.New(config);
            // This is where the HTTP request happens, returns <IDocument> that // we can query later
            var companyHTML = await context.OpenAsync("http://www.nepalstock.com/company/index/1/?stock-name=&stock-symbol=&sector-id=&_limit=50000");
            var companyData = companyHTML.QuerySelectorAll("#company-list .my-table tr").ToArray();
            var companyHTML2 = await context.OpenAsync("http://www.nepalstock.com/promoter-share/index/1/?stock-name=&stock-symbol=&_limit=500000");
            var companyData2 = companyHTML2.QuerySelectorAll("#company-list .my-table tr").ToArray();
            DateTime currentDateTime = DateTime.Now;
            List<Company> companies = new List<Company>();

            using (var db = _context.Database.BeginTransaction())
            {
                try
                {
                    foreach (var item in companyData)
                    {
                        var attr = item.GetAttribute("align");
                        if(item.ClassName == null && attr == null && item.Children.Count() == 6)
                        {
                            var childItem = item.Children[5].Children[0].GetAttribute("href");
                            var id = childItem.Split('/').LastOrDefault();
                            var prevCompany = (await _context.Company.Where(x => x.Symbol.ToLower().Equals(item.Children[3].TextContent.ToLower())).FirstOrDefaultAsync()) ?? new Company();
                            prevCompany.Id = Int32.Parse(id);
                            prevCompany.Symbol = item.Children[3].TextContent;
                            prevCompany.Name = item.Children[2].TextContent.Trim( new Char[] { ' ', '\n' } );
                            prevCompany.Sector = item.Children[4].TextContent;
                            prevCompany.LiveTradingData = null;
                            if(!await _context.Company.AnyAsync(x => x.Id == prevCompany.Id && x.Symbol.ToLower() == prevCompany.Symbol.ToLower()))
                            {
                                companies.Add(prevCompany);
                            }
                        }
                    }

                    foreach (var item in companyData2)
                    {
                        var attr = item.GetAttribute("align");
                        if(item.ClassName == null && attr == null && item.Children.Count() == 5)
                        {
                            var childItem = item.Children[4].Children[0].GetAttribute("href");
                            var id = childItem.Split('/').LastOrDefault();
                            var prevCompany = (await _context.Company.Where(x => x.Symbol.ToLower().Equals(item.Children[3].TextContent.ToLower())).FirstOrDefaultAsync()) ?? new Company();
                            prevCompany.Id = Int32.Parse(id);
                            prevCompany.Symbol = item.Children[3].TextContent;
                            prevCompany.Name = item.Children[2].TextContent.Trim( new Char[] { ' ', '\n' } );
                            prevCompany.LiveTradingData = null;
                            if(!await _context.Company.AnyAsync(x => x.Id == prevCompany.Id && x.Symbol.ToLower() == prevCompany.Symbol.ToLower()))
                            {
                                companies.Add(prevCompany);
                            }
                        }
                    }
                    
                    await _context.Company.AddRangeAsync(companies);
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
            return new DataResult { ResultType = ResultTypeOption.Success, Message = "Successfully Scraped Nepse Companies" };
        }

        public async Task<DataResult> SeedFloorSheet()
        {
            // Load default configuration
            var config = Configuration.Default.WithDefaultLoader();
            // Create a new browsing context
            var context = BrowsingContext.New(config);
            // This is where the HTTP request happens, returns <IDocument> that // we can query later
            var floorSheetHTML = await context.OpenAsync("http://www.nepalstock.com/main/floorsheet/index/1/?contract-no=&stock-symbol=&buyer=&seller=&_limit=30000");
            var floorSheetData = floorSheetHTML.QuerySelectorAll("#home-contents .my-table tr").ToArray();
            DateTime currentDateTime = DateTime.Now;
            List<FloorSheet> floorSheets = new List<FloorSheet>();
            using (var db = _context.Database.BeginTransaction())
            {
                try
                {
                    var floorList = _context.FloorSheets.AsQueryable();
                    foreach (var item in floorSheetData)
                    {
                        FloorSheet floorSheet = new FloorSheet();
                        var attr = item.GetAttribute("align");
                        if(item.ClassName == null && attr == null && item.Children.Count() == 8)
                        {
                            ulong ContractNo = ulong.Parse(item.Children[1].TextContent);
                            if(!await floorList.AnyAsync(x => x.ContractNo == ContractNo)) 
                            {
                                floorSheet.CompanyId = (await _context.Company.Where(x => x.Symbol.ToLower().Equals(item.Children[2].TextContent.ToLower())).FirstOrDefaultAsync()).Id;
                                floorSheet.ContractNo = ContractNo;
                                floorSheet.Symbol = (item.Children[2].TextContent);
                                floorSheet.BuyerBroker = int.Parse(item.Children[3].TextContent);
                                floorSheet.SellerBroker = int.Parse(item.Children[4].TextContent);
                                floorSheet.Quantity = int.Parse(item.Children[5].TextContent);
                                floorSheet.Rate = decimal.Parse(item.Children[6].TextContent);
                                floorSheet.Amount = decimal.Parse(item.Children[7].TextContent);
                                floorSheet.EntryDate = currentDateTime;
                                floorSheets.Add(floorSheet);
                            }
                        }
                    }
                    
                    await _context.FloorSheets.AddRangeAsync(floorSheets);
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
            return new DataResult { ResultType = ResultTypeOption.Success, Message = "Successfully Scraped Nepse Floor Sheet" };
        }
    }
}