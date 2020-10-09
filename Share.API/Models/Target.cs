using System;
using System.ComponentModel.DataAnnotations;
using Share.API.Enums;

namespace Share.API.Models
{
    public class Target
    {
        [Key]
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string Name { get; set; }
        public Level Level { get; set; }
        public decimal? BuyPercentage { get; set; }  // Defines probablity of buying a company share
        public decimal Amount { get; set; }
        public DateTime TargetDate { get; set; }
        public Company Company { get; set; }
    }
}