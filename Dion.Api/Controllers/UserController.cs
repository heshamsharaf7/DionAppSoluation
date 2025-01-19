using Dion.Api.Entities;
using Dion.Api.Extensions;
using Dion.Api.Repositories.Contracts;
using Dion.Models.Dtos.Dtos.User;
using Microsoft.AspNetCore.Mvc;

namespace Dion.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public UserController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        [HttpGet("{phoneNo}")]
        public async Task<ActionResult<bool>> isPhoneNoExist(int phoneNo)
        {
            try
            {
                var isExist = await this.unitOfWork.UserRepository.isPhoneNoExist(phoneNo);

                return isExist;

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                "Error retrieving data from the database");

            }
        }

        [HttpGet("GetUserByPhoneNo/{phoneNo}")]
        public async Task<ActionResult<UserGetDto>> GetUserByPhoneNo(int phoneNo)
        {
            try
            {
                var user = await this.unitOfWork.UserRepository.GetUserByPhoneNo(phoneNo);

                var userDto = user.ConvertToDto();

                return userDto;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                "Error retrieving data from the database");

            }
        }
        [HttpPost("merchantLogin")]
        public async Task<ActionResult<IEnumerable<StoreGetDto>>> MerchantLogin([FromBody] LoginDto merchantLoginDto)
        {
            try
            {

                User user = new User();
            
                user.PhoneNo = merchantLoginDto.PhoneNo;
                user.Password = merchantLoginDto.UserPassword;
                user.Type = merchantLoginDto.UserType;

                var getUser = await this.unitOfWork.UserRepository.Login(user);

                if (getUser == null)
                {
                    return NoContent();
                }

                IEnumerable<Store> store = await this.unitOfWork.StoreRepository.GetByUserId(getUser.Id);


                List<StoreGetDto> storeGetDtos = store.Select(addesStore => new StoreGetDto
                {
                    Id = addesStore.Id,
                    Name = addesStore.Name,
                    EnteredDate = addesStore.EnteredDate,
                    Verified = addesStore.Verified,
                    Latitude = addesStore.Latitude,
                    Longitude = addesStore.Longitude,
                    StoreTypeId = addesStore.StoreTypeId,
                    UserId = addesStore.UserId
                }).ToList();

                return Ok(storeGetDtos);


            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("userLogin")]
        public async Task<ActionResult<UserGetDto>> UserLogin([FromBody] LoginDto merchantLoginDto)
        {
            try
            {

                User user = new User();

                user.PhoneNo = merchantLoginDto.PhoneNo;
                user.Password = merchantLoginDto.UserPassword;
                user.Type = merchantLoginDto.UserType;

                var getUser = await this.unitOfWork.UserRepository.Login(user);

                if (getUser == null)
                {
                    return NoContent();
                }

              
                return Ok(getUser);


            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpPost("userAddAccount")]
        public async Task<ActionResult<UserGetDto>> UserCreatAccount([FromBody] UserAddDto userAddDto)
        {
            try
            {

                unitOfWork.BeginTransaction();

                User user = new User();
                user.Name = userAddDto.UserName;
                user.EnteredDate = userAddDto.EnteredDate;
                user.Address = userAddDto.UserAddress;
                user.PhoneNo = userAddDto.PhoneNo;
                user.Password = userAddDto.UserPassword;
                user.Type = userAddDto.UserType;

                var addedUser = await this.unitOfWork.UserRepository.AddItem(user);



                this.unitOfWork.Commit();

                if (addedUser == null)
                {
                    return NoContent();
                }

              

                return Ok(addedUser);


            }
            catch (Exception ex)
            {
                this.unitOfWork.Rollback();

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("GetUserById/{userId}")]
        public async Task<ActionResult<UserGetDto>> GetUserById(int userId)
        {
            try
            {
                var user = await this.unitOfWork.UserRepository.GetItem(userId);

                if (user == null)
                {
                    return NotFound();
                }

                var userDto = user.ConvertToDto();

                return userDto;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


    }
}
