using Microsoft.AspNetCore.Mvc;
using Dion.Api.Repositories.Contracts;
using Dion.Models.Dtos.Dtos.CustomerParticipant;
using Dion.Api.Entities;
using Dion.Api.Extensions;
using Dion.Api.UnitOfWork;

namespace Dion.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerParticipantController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public CustomerParticipantController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpPost]
        public async Task<ActionResult<CustomerParticipantGetDto>> AddItem([FromBody] CustomerParticipantAddDto customerParticipantAddDto)
        {
            try
            {
                CustomerParticipant customerParticipant = new CustomerParticipant
                {
                    // Map properties from the DTO
                    Id = customerParticipantAddDto.Id,
                   Name = customerParticipantAddDto.Name,
                    EnteredDate = DateTime.Now.ToString(),
                    CustomerId = customerParticipantAddDto.CustomerId,
                    IsActive = customerParticipantAddDto.IsActive



                };

                unitOfWork.BeginTransaction();
                var addedItem = await this.unitOfWork.CustomerParticipantRepository.AddItem(customerParticipant);
                unitOfWork.Commit();

                if (addedItem == null)
                {
                    return NoContent();
                }

                var addedItemDto = addedItem.ConvertToDto();

                return Ok(addedItemDto);
            }
            catch (Exception ex)
            {
                this.unitOfWork.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("changeActive/{id:int}")]
        public async Task<ActionResult<bool>> ChangeActive(int id, [FromBody] bool activeValue)
        {
            try
            {
                unitOfWork.BeginTransaction();
                var success = await this.unitOfWork.CustomerParticipantRepository.ChangeActive(id, activeValue);
                unitOfWork.Commit();

                if (!success)
                {
                    return NotFound();
                }

                return Ok(success);
            }
            catch (Exception ex)
            {
                this.unitOfWork.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("getByCustomerId/{customerId:int}")]
        public async Task<ActionResult<IEnumerable<CustomerParticipantGetDto>>> GetItemsByCustomerId(int customerId)
        {
            try
            {
                var items = await this.unitOfWork.CustomerParticipantRepository.GetItemsByCustomerId(customerId);

                if (items == null)
                {
                    return NoContent();
                }

                var itemsDto = items.Select(item => item.ConvertToDto());

                return Ok(itemsDto);
            }
            catch (Exception ex)
            {
                this.unitOfWork.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}