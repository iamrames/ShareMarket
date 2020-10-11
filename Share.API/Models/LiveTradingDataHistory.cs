using System;
using System.ComponentModel.DataAnnotations;
using Share.API.Enums;

namespace Share.API.Models
{
    public class LiveTradingDataHistory
    {
        [Key]
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
        public Company Company { get; set; }
    }
}