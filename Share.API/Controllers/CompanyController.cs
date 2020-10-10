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
using Microsoft.AspNetCore.Authorization;

namespace Share.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController: ControllerBase
    {
        private readonly ICompanyRepository _repo;
        public CompanyController(ICompanyRepository repo)
        {
            _repo = repo;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CompanyDto CompanyDto) 
        {
            // validate request
            var CompanyToCreate = new Company {
                Name = CompanyDto.Name,
                Symbol = CompanyDto.Symbol,
                Address = CompanyDto.Address,
                Sector = CompanyDto.Sector
            };

            var result = await _repo.CreateAsync(CompanyToCreate);

            if(result.ResultType != ResultTypeOption.Success)
            {
                return BadRequest(result);
            }

            return CreatedAtRoute ("Get", 
                new { controller = "Company", id = result.Data.Id }, result.Data);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Get()
        {
            try
            {
                var data = await _repo.GetDataDescAsync();
                if(data == null)
                {
                    return Ok(new DataResult<List<CompanyDto>> { ResultType = ResultTypeOption.Success, Message = "No Data Found" });
                }
                List<CompanyDto> result = new List<CompanyDto>();
                foreach (var item in data)
                {
                    result.Add(new CompanyDto {
                        Id = item.Id,
                        Name = item.Name,
                        Symbol = item.Symbol,
                        Address = item.Address,
                        Sector = item.Sector
                    });
                }
                return Ok(new DataResult<List<CompanyDto>> { ResultType = ResultTypeOption.Success, Data = result});
            }
            catch (Exception ex)
            {
                return Ok(new DataResult<List<CompanyDto>> { ResultType = ResultTypeOption.Failed, Message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var data = await _repo.GetDataAsync(id);
                if(data == null)
                {
                    return Ok(new DataResult<CompanyDto> { ResultType = ResultTypeOption.Success, Message = "No Data Found" });
                }
                CompanyDto result = new CompanyDto {
                    Id = data.Id,
                    Name = data.Name,
                    Symbol = data.Symbol,
                    Address = data.Address,
                    Sector = data.Sector
                };
                return Ok(new DataResult<CompanyDto> { ResultType = ResultTypeOption.Success, Data = result});
            }
            catch (Exception ex)
            {
                return Ok(new DataResult<CompanyDto> { ResultType = ResultTypeOption.Failed, Message = ex.Message });
            }
        }
    }
}