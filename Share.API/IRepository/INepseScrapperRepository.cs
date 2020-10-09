using System.Threading.Tasks;
using Share.API.Common.Results;

namespace Share.API.IRepository
{
    public interface INepseScrapperRepository
    {
        Task<DataResult> SeedAllCompanies();
    }
}