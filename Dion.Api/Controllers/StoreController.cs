using Dion.Api.Entities;
using Dion.Api.Extensions;
using Dion.Api.Repositories.Contracts;
using Dion.Models.Dtos.Dtos.StoreType;
using Dion.Models.Dtos.Dtos.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dion.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public StoreController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpPost]
        public async Task<ActionResult<StoreGetDto>> PostItem([FromBody] StoreAddDto storeAddDto)
        {
            try
            {
                unitOfWork.BeginTransaction();
                User user = new User();
                user.Name = storeAddDto.UserName;
                user.EnteredDate = storeAddDto.EnteredDate;
                user.Address = storeAddDto.UserAddress;
                user.PhoneNo = storeAddDto.PhoneNo;
                user.Password = storeAddDto.UserPassword;
                user.Type = storeAddDto.UserType;

                var addedUser = await this.unitOfWork.UserRepository.AddItem(user);

                Store store = new Store();
                //store.Id = 0;
                store.Name = storeAddDto.StoreName;
                store.EnteredDate = storeAddDto.EnteredDate;
                store.Latitude = storeAddDto.Latitude;
                store.Longitude = storeAddDto.Longitude;
                store.Verified = storeAddDto.StoreVerified;
                store.StoreTypeId = storeAddDto.StoreTypeId;
                store.UserId = user.Id;
                store.StorePhoneNo = storeAddDto.StorePhoneNo;

                var addesStore = await this.unitOfWork.StoreRepository.AddItem(store);

                this.unitOfWork.Commit();

                if (addedUser == null)
                {
                    return NoContent();
                }

                //var newstoreTypeItemDto = storeTypeItem.ConvertToDto();

                StoreGetDto storeGetDto = new StoreGetDto();

                storeGetDto.Id = addesStore.Id;
                storeGetDto.Name = addesStore.Name;
                storeGetDto.EnteredDate = addesStore.EnteredDate;
                storeGetDto.Verified = addesStore.Verified;
                storeGetDto.Latitude = addesStore.Latitude;
                storeGetDto.Longitude = addesStore.Longitude;
                storeGetDto.StoreTypeId = addesStore.StoreTypeId;
                storeGetDto.UserId = addesStore.UserId;

                return Ok(storeGetDto);
            }
            catch (Exception ex)
            {
                this.unitOfWork.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<StoreGetDto>> GetUserByPhoneNo(int id)
        {
            try
            {
                var store = await this.unitOfWork.StoreRepository.GetItem(id);

                var userDto = store.ConvertToDto();

                return userDto;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                "Error retrieving data from the database");

            }
        }
        [HttpGet("GetStore/{id}")]
        public async Task<ActionResult<StoreGetDto>> GetStoreItem(int id)
        {
            try
            {
                var store = await this.unitOfWork.StoreRepository.GetItem(id);

                if (store == null)
                {
                    return NotFound();
                }

                return store.ConvertToDto();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }

    }
