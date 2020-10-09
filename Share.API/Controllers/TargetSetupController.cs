using System.Net;
using System.Security.Cryptography.X509Certificates;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Share.API.Common.Results;
using Share.API.Dtos;
using Share.API.Enums;
using Share.API.IRepository;
using Share.API.Models;

namespace Share.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TargetSetupController: ControllerBase
    {
        private readonly ITargetSetupRepository _repo;
        public TargetSetupController(ITargetSetupRepository repo)
        {
            _repo = repo;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]TargetDto targetDto) 
        {
            // validate request
            var targetToCreate = new Target {
                CompanyId = targetDto.CompanyId,
                Name = targetDto.Name,
                Level = targetDto.Level,
                BuyPercentage =targetDto.BuyPercentage,
                Amount = targetDto.Amount,
                TargetDate = DateTime.Now
            };

            var result = await _repo.CreateAsync(targetToCreate);

            if(result.ResultType != ResultTypeOption.Success)
            {
                return BadRequest(result);
            }

            return CreatedAtRoute ("Get", 
                new { controller = "TargetSetup", id = result.Data.Id }, result.Data);
        }

        [HttpGet]
        public async Task<DataResult<List<TargetDto>>> Get()
        {
            try
            {
                var data = await _repo.GetDataDescAsync();
                if(data == null)
                {
                    return new DataResult<List<TargetDto>> { ResultType = ResultTypeOption.Success, Message = "No Data Found" };
                }
                List<TargetDto> result = new List<TargetDto>();
                foreach (var item in data)
                {
                    result.Add(new TargetDto {
                        Id = item.Id,
                        CompanyId = item.CompanyId,
                        Name = item.Name,
                        Level = item.Level,
                        BuyPercentage =item.BuyPercentage,
                        Amount = item.Amount,
                        TargetDate = item.TargetDate,
                        CompanyName = item.Company.Name,
                        CompanySymbol = item.Company.Symbol
                    });
                }
                return new DataResult<List<TargetDto>> { ResultType = ResultTypeOption.Success, Data = result};
            }
            catch (Exception ex)
            {
                return new DataResult<List<TargetDto>> { ResultType = ResultTypeOption.Failed, Message = ex.Message };
            }
        }

        [HttpGet("{id}")]
        public async Task<DataResult<TargetDto>> Get(int id)
        {
            try
            {
                var data = await _repo.GetDataAsync(id);
                if(data == null)
                {
                    return new DataResult<TargetDto> { ResultType = ResultTypeOption.Success, Message = "No Data Found" };
                }
                TargetDto result = new TargetDto {
                    Id = data.Id,
                    CompanyId = data.CompanyId,
                    Name = data.Name,
                    Level = data.Level,
                    BuyPercentage =data.BuyPercentage,
                    Amount = data.Amount,
                    TargetDate = data.TargetDate,
                    CompanyName = data.Company.Name,
                    CompanySymbol = data.Company.Symbol
                };
                return new DataResult<TargetDto> { ResultType = ResultTypeOption.Success, Data = result};
            }
            catch (Exception ex)
            {
                return new DataResult<TargetDto> { ResultType = ResultTypeOption.Failed, Message = ex.Message };
            }
        }
    }
}