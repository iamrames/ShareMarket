using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Share.API.Common.Results;
using Share.API.Enums;
using Share.API.IRepository;
using Share.API.Models;

namespace Share.API.Repository
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly DatabaseContext _context;
        public CompanyRepository(DatabaseContext context)
        {
            _context = context;
        }
        public async Task<DataResult<Company>> CreateAsync(Company data)
        {
            try
            {
                await _context.Company.AddAsync(data);
                await _context.SaveChangesAsync();
                return new DataResult<Company> { ResultType = ResultTypeOption.Success, Message = "Successfully Created", Data = data };
            }
            catch (Exception ex)
            {
                return new DataResult<Company> { ResultType = ResultTypeOption.Failed, Message = ex.Message , Data = data };
            }
        }

        public async Task<Company> GetDataAsync(int id)
        {
            return await _context.Company.Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<Company>> GetDataDescAsync()
        {
            return await _context.Company.OrderBy(x => x.Symbol).ToListAsync();
        }
    }
}