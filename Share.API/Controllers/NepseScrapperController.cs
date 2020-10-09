using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Share.API.Common.Results;
using Share.API.IRepository;

namespace Share.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NepseScrapperController: ControllerBase
    {
        private readonly ILogger<MeroLaganiScraperController> _logger;
        private readonly INepseScrapperRepository _repo;
        // Constructor
        public NepseScrapperController(ILogger<MeroLaganiScraperController> logger, INepseScrapperRepository repo)
        {
            _logger = logger;
            _repo = repo;
        }

        // [HttpGet]
        // public async Task<IActionResult> Get()
        // {
        //     return Ok(await _repo.GetAllLiveTradingDataAsync());
        // }

        [HttpGet("[action]")]
        public async Task<DataResult> SeedCompanies()
        {
            return await _repo.SeedAllCompanies();
        }
    }
}