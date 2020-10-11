using System;
using Share.API.Enums;

namespace Share.API.Dtos
{
    public class LiveTradingDataDto
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string Symbol { get; set; }
        public decimal LTP { get; set; }
        public decimal LTV { get; set; }
        public decimal ChangePercentage { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Open { get; set; }
        public decimal Qty { get; set; }
        public DateTime EntryDate { get; set; }
        public ScrapingSource Source { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string CompanyName { get; set; }
        public Level? TargetLevel { get; set; }
    }
}