using System.Collections.Generic;
using System.Threading.Tasks;
using Share.API.Common.Results;
using Share.API.Models;

namespace Share.API.IRepository
{
    public interface ITargetSetupRepository
    {
        Task<DataResult<Target>> CreateAsync(Target data);
        Task<List<Target>> GetDataDescAsync();
        Task<Target> GetDataAsync(int id);
    }
}