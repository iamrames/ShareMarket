using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Share.API.Models;

namespace Share.API.Repository
{
    public class DatabaseContext: DbContext
    {
        // This is the table we are querying
        public DbSet<Company> Company { get; set; }
        public DbSet<LiveTradingData> LiveTradingData { get; set; }
        public DbSet<LiveTradingDataHistory> LiveTradingDataHistory { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Target> Targets { get; set; }
        public DbSet<FloorSheet> FloorSheets { get; set; }
        private readonly IConfiguration _configuration;
        
        public DatabaseContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This is what .NET calls when creating a new connection
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {            
            // This tells .NET what provider to use
            options.UseNpgsql(_configuration.GetConnectionString("DefaultConnection"));
        }
    }
}