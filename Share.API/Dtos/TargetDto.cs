using System;
using Share.API.Enums;

namespace Share.API.Dtos
{
    public class TargetDto
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string Name { get; set; }
        public Level Level { get; set; }
        public decimal? BuyPercentage { get; set; }  // Defines probablity of buying a company share
        public decimal Amount { get; set; }
        public DateTime TargetDate { get; set; }
        public string CompanyName { get; set; }
        public string CompanySymbol { get; set; }
    }
}