using System.Collections.Generic;
using System.Threading.Tasks;
using Share.API.Common.Results;
using Share.API.Models;

namespace Share.API.IRepository
{
    public interface ICompanyRepository
    {
        Task<DataResult<Company>> CreateAsync(Company data);
        Task<List<Company>> GetDataDescAsync();
        Task<Company> GetDataAsync(int id);
    }
}