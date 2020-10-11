using System;
using System.ComponentModel.DataAnnotations;

namespace Share.API.Models
{
    public class FloorSheet
    {
        [Key]
        public ulong ContractNo { get; set; }
        public string Symbol { get; set; }
        public int BuyerBroker { get; set; }
        public int SellerBroker { get; set; }
        public int Quantity { get; set; }
        public decimal Rate { get; set; }
        public decimal Amount { get; set; }
        public int CompanyId { get; set; }
        public DateTime EntryDate { get; set; }
        public Company Company { get; set; }
    }
}