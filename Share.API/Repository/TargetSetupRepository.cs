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
    public class TargetSetupRepository : ITargetSetupRepository
    {
        private readonly DatabaseContext _context;
        public TargetSetupRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<DataResult<Target>> CreateAsync(Target data)
        {
            try
            {
                _context.Targets.Add(data);
                await _context.SaveChangesAsync();
                return new DataResult<Target> { ResultType = ResultTypeOption.Success, Message = "Successfully created", Data = data};
            }
            catch(Exception ex)
            {
                return new DataResult<Target> { ResultType = ResultTypeOption.Failed, Message = ex.Message, Data = null};
            }
        }

        public async Task<Target> GetDataAsync(int id)
        {
            return await _context.Targets.Where(x => x.Id == id)
                .Include(x => x.Company)
                .OrderByDescending(x => x.TargetDate).AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<List<Target>> GetDataDescAsync()
        {
            return await _context.Targets
                .Include(x => x.Company)
                .OrderByDescending(x => x.TargetDate).AsNoTracking().ToListAsync();
        }
    }
}