using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Share.API.Models
{
    public class Company
    {
        [Key]
        public int Id { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Sector { get; set; }
        public IEnumerable<LiveTradingData> LiveTradingData { get; set; }
        public IEnumerable<LiveTradingDataHistory> LiveTradingDataHistory { get; set; }
    }
}