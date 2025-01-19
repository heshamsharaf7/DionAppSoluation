using Dion.Api.Entities;
using Dion.Api.Extensions;
using Dion.Api.Repositories.Contracts;
using Dion.Api.UnitOfWork;
using Dion.Models.Dtos.Dtos.Accounting;
using Dion.Models.Dtos.Dtos.Currency;
using Dion.Models.Dtos.Dtos.Transactions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace Dion.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public TransactionController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpPost]
        public async Task<ActionResult<TransactionGetDto>> PostItem([FromBody] TransactionAddtDto transaction)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                //Currency item = new Currency();
                //item.Name = currency.Name;
                //item.EnteredDate = currency.EnteredDate;

                TransactionDetails item =  new TransactionDetails
                {
                    Statement = transaction.Statement,
                    StoreId = transaction.StoreId,
                    CustomerId = transaction.CustomerId,
                    Credit=transaction.Credit,
                    CurrencyId = transaction.CurrencyId,
                    Debit = transaction.Debit,
                    EnteredDate = transaction.EnteredDate,
                    InvoiceId= 0,
                    LockDate= "",
                    LockStatus = false,

                };

                var addedTrans = await _unitOfWork.TransactionDetailsRepository.AddItem(item);
                _unitOfWork.Commit();

                var newcurrencyItemDto = addedTrans.ConvertToDto();

                return Ok(newcurrencyItemDto);
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpGet("GetAllStoreTransactions/{storeId}")]
        public async Task<ActionResult<IEnumerable<TransactionGetDto>>> GetAllStoreTransactions(int storeId)
        {
            try
            {
                var allTransactions = await _unitOfWork.TransactionDetailsRepository.GetAllStoreTransactions(storeId);

                var itemsDto = allTransactions.Select(item => item.ConvertToDto()).ToList();

                foreach (var item in itemsDto)
                {
                    var customerName = await _unitOfWork.StoreCustomersRepository.GetItem(item.CustomerId);
                    item.CustomerName = customerName?.CuName; // Ensure customerName is not null before accessing CuName
                }

                return Ok(itemsDto);
            }
            catch (Exception ex)
            {
                // Log the exception for further investigation
                // Log.Error(ex, "An error occurred in GetAllStoreTransactions for storeId: {storeId}", storeId);

                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }
        }
        [HttpGet("GetAllCustomerTransactions/{customerId}")]
        public async Task<ActionResult<IEnumerable<TransactionGetDto>>> GetAllCustomerTransactions(int customerId)
        {
            try
            {
                var allTransactions = await _unitOfWork.TransactionDetailsRepository.GetAllCustomerTransactions(customerId);
                var itemsDto = allTransactions.Select(item => item.ConvertToDto());

                return Ok(itemsDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }
        }
        [HttpDelete("DeleteTransaction/{transactionId}")]
        public async Task<ActionResult<IEnumerable<InvoiceDetailsGetDto>>> DeleteTransaction(int transactionId)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                var transaction = await _unitOfWork.TransactionDetailsRepository.GetItem(transactionId);
                await _unitOfWork.TransactionDetailsRepository.DeleteItem(transactionId);
                var invoiceItems = await _unitOfWork.InvoiceDetailsRepository.GetItemsByInvoiceId(transaction.InvoiceId);

                await _unitOfWork.InvoiceRepository.DeleteItem(transaction.InvoiceId);
                //foreach (var item in invoiceItems)
                //{
                //    await _unitOfWork.InvoiceDetailsRepository.DeleteItem(item.Id);
                //}
                _unitOfWork.Commit();
                var returnIttems= invoiceItems.Select(s=> s.ConvertToDto()).ToList();   
                return Ok(returnIttems);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }
        }



        [HttpGet("GetAllStoreCustomerTransactions/{customerId}/{storeId}")]
        public async Task<ActionResult<IEnumerable<TransactionGetDto>>> GetAllStoreCustomerTransactions(int customerId, int storeId)
        {
            try
            {
                var allTransactions = await _unitOfWork.TransactionDetailsRepository.GetAllStoreCustomerTransactions(customerId,storeId);
                var itemsDto = allTransactions.Select(item => item.ConvertToDto());

                return Ok(itemsDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }
        }


        [HttpGet("GetTotalDebtCreditDiscrepancyForCurrentDayStore/{storeId}")]
        public async Task<ActionResult<double>> GetTotalDebtCreditDiscrepancyForCurrentDayStore(int storeId)
        {
            try
            {
                var totalDebt = await _unitOfWork.TransactionDetailsRepository.GetTotalDebtCreditDiscrepancyForCurrentDayStore(storeId);
                return Ok(totalDebt);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }
        }

        [HttpGet("GetTotalDebtCreditDiscrepancyStore/{storeId}")]
        public async Task<ActionResult<double>> GetTotalDebtCreditDiscrepancyStore(int storeId)
        {
            try
            {
                var totalDebt = await _unitOfWork.TransactionDetailsRepository.GetTotalDebtCreditDiscrepancyStore(storeId);
                return Ok(totalDebt);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }
        }
        [HttpGet("GetTotalDebtCreditDiscrepancyCustomer/{customerId}")]
        public async Task<ActionResult<double>> GetTotalDebtCreditDiscrepancyCustomer(int customerId)
        {
            try
            {
                var totalDebt = await _unitOfWork.TransactionDetailsRepository.GetTotalDebtCreditDiscrepancyCustomer(customerId);
                return Ok(totalDebt);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }
        }
        [HttpGet("GetTotalDebtCustomer/{customerId}")]

        public async Task<ActionResult<double>> GetTotalDebtCustomer(int customerId)
        {
            try
            {
                var totalDebt = await _unitOfWork.TransactionDetailsRepository.GetTotalDebtCustomer(customerId);
                return Ok(totalDebt);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }
        }
        [HttpGet("GetTotalCreditCustomer/{customerId}")]
        public async Task<ActionResult<double>> GetTotalCreditCustomer(int customerId)
        {
            try
            {
                var totalDebt = await _unitOfWork.TransactionDetailsRepository.GetTotalCreditCustomer(customerId);
                return Ok(totalDebt);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }
        }
        [HttpGet("GetTotalDebtCreditDiscrepancyByUserId/{userId}")]
        public async Task<ActionResult<double>> GetTotalDebtCreditDiscrepancyByUserId(int userId)
        {
            try
            {
                var totalDebt = await _unitOfWork.TransactionDetailsRepository.GetTotalDebtCreditDiscrepancyByUserId(userId);
                return Ok(totalDebt);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }
        }


    }
}
