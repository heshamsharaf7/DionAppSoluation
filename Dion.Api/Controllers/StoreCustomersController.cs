using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dion.Api.Entities;
using Dion.Api.Repositories.Contracts;
using Dion.Models.Dtos.Dtos.User;
using Dion.Api.Extensions;
using Dion.Models.Dtos.Dtos.StoreType;
using Dion.Api.UnitOfWork;

namespace Dion.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreCustomersController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public StoreCustomersController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpGet("GetCustomer/{id}")]
        public async Task<ActionResult<StoreCustomerGetDto>> GetCustomer(int id)
        {
            try
            {
                var custoemr = await this.unitOfWork.StoreCustomersRepository.GetItem(id);
                var customerDebt = await this.unitOfWork.TransactionDetailsRepository.GetTotalDebtCreditDiscrepancyStoreCustomer(custoemr.Id, custoemr.StoreId);
                
                if (custoemr == null)
                {
                    return NotFound();
                }
                var customerDto= custoemr.ConvertToDto();
                if (custoemr.UserId != 0)
                {

                    var userInfo = await this.unitOfWork.UserRepository.GetItem(customerDto.UserId);
                    customerDto.UserPhoneNo = userInfo.PhoneNo;
                }
                customerDto.TotalDebt = customerDebt;

                return customerDto;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("{storeId}")]
        public async Task<ActionResult<IEnumerable<StoreCustomerGetDto>>> GetItemsByStore(int storeId)
        {
            var storeCustomers = await unitOfWork.StoreCustomersRepository.GetItemsByStore(storeId);

            if (storeCustomers == null)
            {
                return NotFound();
            }

            var storeCustomerDtos = new List<StoreCustomerGetDto>();
            foreach (var sc in storeCustomers)
            {
                var customerDto = sc.ConvertToDto();
                if(customerDto.UserId!=0)
                {

                    var userInfo = await this.unitOfWork.UserRepository.GetItem(customerDto.UserId);
                    customerDto.UserPhoneNo = userInfo.PhoneNo;
                }
                else
                {
                    customerDto.UserPhoneNo = 0;

                }
                var customerDebt = await this.unitOfWork.TransactionDetailsRepository.GetTotalDebtCreditDiscrepancyStoreCustomer(customerDto.Id,customerDto.StoreId);
                customerDto.TotalDebt=customerDebt;

                storeCustomerDtos.Add(customerDto);
            }

            return Ok(storeCustomerDtos);
        }

        [HttpGet("getItemsByUserId/{userId}")]
        public async Task<ActionResult<IEnumerable<StoreCustomerGetDto>>> GetItemsByUserId(int userId)
        {
            var storeCustomers = await unitOfWork.StoreCustomersRepository.GetItemsByUserId(userId);

            if (storeCustomers == null)
            {
                return NotFound();
            }

            var storeCustomerDtos = storeCustomers.Select(sc => sc.ConvertToDto());

            return Ok(storeCustomerDtos);
        }

        [HttpGet("getStoreTypesByUserId/{userId}")]
        public async Task<ActionResult<IEnumerable<StroreTypeGetDto>>> GetStoreTypesByUserId(int userId)
        {
            // Fetch store types from the repository
            var storeTypes = await unitOfWork.StoreCustomersRepository.GetStoreTypesByUserId(userId);

            // Check if no store types are found
            if (storeTypes == null || !storeTypes.Any())
            {
                return NotFound(new { Message = "No store types found for the given user ID." });
            }

            // Convert to DTO and update IconPath
            var storeTypesDtos = storeTypes.Select(sc =>
            {
                var dto = sc.ConvertToDto();
                if (!string.IsNullOrEmpty(dto.IconPath))
                {
                    dto.IconPath = $"{Request.Scheme}://{Request.Host}/images/{dto.IconPath}";
                }
                return dto;
            }).ToList();

            // Return the result
            return Ok(storeTypesDtos);
        }


        [HttpGet("getStoresByUserIdStoreAndTypeId/{userId}/{storeTypeId}")]
        public async Task<ActionResult<IEnumerable<StoreGetDto>>> GetStoresByUserIdStoreAndTypeId(int userId, int storeTypeId)
        {
            var store = await unitOfWork.StoreCustomersRepository.GetStoresByUserIdStoreAndTypeId(userId, storeTypeId);

            if (store == null)
            {
                return NotFound();
            }

            var storeDtos = store.Select(sc => sc.ConvertToDto());

            return Ok(storeDtos);
        }

        [HttpGet("isUserExist/{userId}/{storeId}")]
        public async Task<ActionResult<bool>> isUserExist(int userId,int storeId)
        {
            try
            {
                var isExist = await this.unitOfWork.StoreCustomersRepository.IsUserExist(userId,storeId);

                return isExist;

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                "Error retrieving data from the database");

            }
        }

        [HttpPost]
        public async Task<ActionResult<StoreCustomerGetDto>> PostItem([FromBody] StoreCustomersAddDto storeCustomersAddDto)
        {
            try
            {
                StoreCustomers storeCustomers = new StoreCustomers
                {
                    AccountCapacity = storeCustomersAddDto.AccountCapacity,
                    IsLock = storeCustomersAddDto.IsLock,
                    EnteredDate = storeCustomersAddDto.EnteredDate,
                    PayNotification = storeCustomersAddDto.PayNotification,
                    CuName = storeCustomersAddDto.CuName,
                    CuAddress = storeCustomersAddDto.CuAddress,
                    IsAccepted = storeCustomersAddDto.IsAccepted,
                    StoreTypeId = storeCustomersAddDto.StoreTypeId,
                    UserId = storeCustomersAddDto.UserId,
                    StoreId = storeCustomersAddDto.StoreId
                };

                unitOfWork.BeginTransaction();
                var addedStoreCustomers = await unitOfWork.StoreCustomersRepository.AddItem(storeCustomers);
                unitOfWork.Commit();

                if (addedStoreCustomers == null)
                {
                    return NoContent();
                }

                var addedStoreCustomerDto = addedStoreCustomers.ConvertToDto();

                return Ok(addedStoreCustomerDto);
            }
            catch (Exception ex)
            {
                unitOfWork.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<StoreCustomerGetDto>> UpdateItem(int id, StoreCustomersAddDto storeCustomersAddDto)
        {
            try
            {
                unitOfWork.BeginTransaction();

                var existingItem = await unitOfWork.StoreCustomersRepository.GetItem(id);

                if (existingItem == null)
                {
                    unitOfWork.Rollback();
                    return NotFound();
                }

                existingItem.AccountCapacity = storeCustomersAddDto.AccountCapacity;
                existingItem.IsLock = storeCustomersAddDto.IsLock;
                existingItem.EnteredDate = storeCustomersAddDto.EnteredDate;
                existingItem.PayNotification = storeCustomersAddDto.PayNotification;
                existingItem.CuName = storeCustomersAddDto.CuName;
                existingItem.CuAddress = storeCustomersAddDto.CuAddress;
                existingItem.IsAccepted = storeCustomersAddDto.IsAccepted;
                existingItem.StoreTypeId = storeCustomersAddDto.StoreTypeId;
                existingItem.UserId = storeCustomersAddDto.UserId;
                existingItem.StoreId = storeCustomersAddDto.StoreId;

                var updatedItem = await unitOfWork.StoreCustomersRepository.UpdateItem(id, existingItem);

                unitOfWork.Commit();

                // Assuming ConvertToDto is an extension method for StoreCustomers
                var updatedItemDto = updatedItem.ConvertToDto();

                return Ok(updatedItemDto);
            }
            catch (Exception ex)
            {
                unitOfWork.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    
        [HttpGet("GetOrdersByUserId/{userId}")]
        public async Task<ActionResult<IEnumerable<StoreCustomerOrderGetDto>>> GetOrdersByUserId(int userId)
        {
            try
            {
                var userOrders = await this.unitOfWork.StoreCustomersRepository.GetItemsByUserId(userId);

                IEnumerable<StoreCustomerOrderGetDto> orderDto = userOrders.ConvertToOrderDto();


                foreach (var order in orderDto)
                {
                    var storeType = await this.unitOfWork.StoreTypeRepository.GetItem(order.StoreTypeId);
                    order.StoreTypeName = storeType.Name;
                }
                foreach (var order in orderDto)
                {
                    var store = await this.unitOfWork.StoreRepository.GetItem(order.StoreId);
                    order.StoreName = store.Name;
                }

                return Ok( orderDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }

        [HttpPost("AcceptedStore/{id}")]
        public async Task<ActionResult<bool>> AcceptedStore(int id)
        {
            try
            {
                unitOfWork.BeginTransaction();

                var existingItem = await unitOfWork.StoreCustomersRepository.GetItem(id);

                if (existingItem == null)
                {
                    unitOfWork.Rollback();
                    return NotFound();
                }

                existingItem.IsAccepted = true;

                var updatedItem = await unitOfWork.StoreCustomersRepository.UpdateItem(id, existingItem);

                unitOfWork.Commit();

                return Ok(true);
            }
            catch (Exception ex)
            {
                unitOfWork.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpGet("getItemByStoreAdUser/{userId}/{storeId}")]
        public async Task<ActionResult<StoreCustomerGetDto>> GetStoreTypesByUserId(int userId, int storeId)
        {
            var customer = await unitOfWork.StoreCustomersRepository.GetItemByStoreAndUser(userId,storeId);

            if (customer == null)
            {
                return NotFound();
            }

            var storeCustomerDtos =  customer.ConvertToDto();
            var customerDebt = await this.unitOfWork.TransactionDetailsRepository.GetTotalDebtCreditDiscrepancyStoreCustomer(storeCustomerDtos.Id, storeCustomerDtos.StoreId);
            storeCustomerDtos.TotalDebt = customerDebt;

            return Ok(storeCustomerDtos);
        }

        [HttpPost("ChangeCustomerLock/{customerId}")]
        public async Task<ActionResult<bool>> ChangeCustomerLock(int customerId, [FromBody] bool status)
        {
            try
            {
                var done = await this.unitOfWork.StoreCustomersRepository.ChangeCustomerLock(customerId, status);
                return Ok(done);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        [HttpPost("ChangeCustomerPayNotification/{customerId}")]
        public async Task<ActionResult<bool>> ChangeCustomerPayNotification(int customerId, [FromBody] bool status)
        {
            try
            {
                var done = await this.unitOfWork.StoreCustomersRepository.ChangeCustomerPayNotification(customerId, status);
                return Ok(done);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        [HttpPut("UpdateItem/{id}")]
        public async Task<ActionResult<StoreCustomerGetDto>> UpdateItem(int id, StoreCustomerGetDto storeCustomers)
        {

            StoreCustomers customer =new StoreCustomers { CuName= storeCustomers.CuName,CuAddress=storeCustomers.CuAddress,AccountCapacity=storeCustomers.AccountCapacity};
            var updatedItem = await this.unitOfWork.StoreCustomersRepository.UpdateItem(id, customer);


            if (updatedItem == null)
            {
                return NotFound();
            }

            return updatedItem.ConvertToDto();
        }
        [HttpPut("ConnectUser/{id}/{phoneNo}")]
        public async Task<ActionResult<StoreCustomerGetDto>> ConnectCustomerToUser(int id, int phoneNo)
        {
           

            //StoreCustomers customer = new StoreCustomers { CuName = storeCustomers.CuName, CuAddress = storeCustomers.CuAddress, AccountCapacity = storeCustomers.AccountCapacity };
            var user = await this.unitOfWork.UserRepository.GetUserByPhoneNo( phoneNo);

            var chekUser = await this.unitOfWork.StoreCustomersRepository.CheckUserIdExisits(user.Id);
            if (chekUser)
            {
                return NotFound();


            }
            StoreCustomerGetDto customerDto ;
            if (user!=null)
            {
                this.unitOfWork.BeginTransaction();
                var customer = await this.unitOfWork.StoreCustomersRepository.ConnectUserToCustomer(id, user.Id);
                customerDto = customer.ConvertToDto();
                customerDto.UserPhoneNo= phoneNo;
                this.unitOfWork.Commit();

            }
            else
            {
                return NotFound();

            }

            

            return customerDto;
        }



    }
}