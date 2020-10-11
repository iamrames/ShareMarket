using System;
using System.Threading;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Share.API.IRepository;

namespace Share.API.Repository
{
    public class Seed
    {
        private readonly INepseScrapperRepository _nepseRepo;
        private readonly IMeroLaganiScrapperRepository _meroLaganiRepo;
        private readonly IBackgroundJobClient _backgroundJobs;
        public Seed(IMeroLaganiScrapperRepository meroLaganiRepo, INepseScrapperRepository nepseRepo, IBackgroundJobClient backgroundJobs)
        {
            _meroLaganiRepo = meroLaganiRepo;
            _backgroundJobs = backgroundJobs;
            _nepseRepo = nepseRepo;
        }

        public void PeriodicSeedLiveTradingData()
        {
            RecurringJob.AddOrUpdate(() =>_nepseRepo.SeedAllCompanies(), Cron.Monthly);
            RecurringJob.AddOrUpdate(() => _meroLaganiRepo.SeedMeroLaganiLiveTradingData(), Cron.Minutely);
            RecurringJob.AddOrUpdate(() => _nepseRepo.SeedFloorSheet(), Cron.Hourly);
        }
    }
}