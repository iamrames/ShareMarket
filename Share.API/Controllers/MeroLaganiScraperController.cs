using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Share.API.IRepository;

namespace Share.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeroLaganiScraperController : ControllerBase
    {
        private readonly ILogger<MeroLaganiScraperController> _logger;
        private readonly IMeroLaganiScrapperRepository _repo;
        // Constructor
        public MeroLaganiScraperController(ILogger<MeroLaganiScraperController> logger, IMeroLaganiScrapperRepository repo)
        {
            _logger = logger;
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _repo.GetAllLiveTradingDataAsync());
        }
        
    }
}