using Microsoft.AspNetCore.Mvc;
using Dion.Api.Repositories.Contracts;
using Dion.Api.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dion.Models.Dtos.Dtos.Currency;
using Dion.Api.Extensions;

namespace Dion.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CurrencyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public async Task<ActionResult<CurrencyGetDto>> PostItem([FromBody] CurrencyAddDto currency)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                Currency item = new Currency();
                item.Name = currency.Name;
                item.EnteredDate = currency.EnteredDate;
                var addedCurrency = await _unitOfWork.CurrencyRepository.AddItem(item);
                _unitOfWork.Commit();

                var newcurrencyItemDto = addedCurrency.ConvertToDto();

                return Ok(newcurrencyItemDto);
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<bool>> DeleteItem(int id)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                var result = await _unitOfWork.CurrencyRepository.DeleteItem(id);
                _unitOfWork.Commit();

                if (!result)
                {
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CurrencyGetDto>>> GetItems()
        {
            try
            {
                _unitOfWork.BeginTransaction();
                var currencies = await _unitOfWork.CurrencyRepository.GetItems();
                _unitOfWork.Commit();

                if (currencies == null || !currencies.Any())
                {
                    return NoContent();
                }
                var currenciesDto= currencies.ConvertToDto();
                return Ok(currenciesDto);
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult<CurrencyGetDto>> UpdateItem(int id, CurrencyAddDto currency)
        {
            try
            {
                Currency item = new Currency();
                item.Id = id;
                item.Name = currency.Name;
                item.EnteredDate = currency.EnteredDate;
                _unitOfWork.BeginTransaction();
                var updatedCurrency = await _unitOfWork.CurrencyRepository.UpdateItem(id, item);
                _unitOfWork.Commit();

                if (updatedCurrency == null)
                {
                    return NotFound();
                }
                var newcurrencyItemDto = updatedCurrency.ConvertToDto();

                return Ok(newcurrencyItemDto);
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}