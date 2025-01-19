using Microsoft.AspNetCore.Mvc;
using Dion.Api.Repositories.Contracts;
using Dion.Models.Dtos.Dtos.Accounting;
using Dion.Api.Entities;
using Dion.Api.Extensions;
using Dion.Api.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dion.Models.Dtos.Dtos.Transactions;

namespace Dion.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public InvoiceController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public async Task<ActionResult<bool>> PostItem([FromBody] InvoiceAddDto invoiceAddDto)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                double debt = 0;
                string statement = "";
                var invoice = new Invoice
                {
                    EnteredDate = invoiceAddDto.EnteredDate,
                    //Details = invoiceAddDto.Details,
                    CustomerId = invoiceAddDto.CustomerId,
                    CurrencyId = invoiceAddDto.CurrencyId,
                    ParticipantId=invoiceAddDto.ParticipantId,
                    //IsCustomerBuyer = invoiceAddDto.IsCustomerBuyer,
                    LockStatus=false,
                    LockDate=""
                };
                var addedInvoice = await _unitOfWork.InvoiceRepository.AddItem(invoice);

                foreach (var invoiceItemDto in invoiceAddDto.InvoiceItems)
                {
                    var invoiceItem = new InvoiceDetails
                    {
                        InvoiceId = addedInvoice.Id,
                        Statement = invoiceItemDto.Statement,
                        UnitPrice = invoiceItemDto.UnitPrice,
                        Quantity = invoiceItemDto.Quantity
                    };
                    double total = invoiceItemDto.UnitPrice * invoiceItemDto.Quantity;
                    debt += total;
                    statement += ","+invoiceItemDto.Statement;

                    var addedInvoiceDetails = await _unitOfWork.InvoiceDetailsRepository.AddItem(invoiceItem);

                }
                var transaction = new TransactionDetails
                {
                    Credit=0,
                    CurrencyId=1,
                    CustomerId=invoiceAddDto.CustomerId,
                    Debit=debt,
                    EnteredDate= invoiceAddDto.EnteredDate,
                    InvoiceId= addedInvoice.Id,
                    LockDate="",
                    LockStatus =false,
                    Statement=statement,
                    StoreId=invoiceAddDto.StoreId,
                };
                var addedTransactionDetails = await _unitOfWork.TransactionDetailsRepository.AddItem(transaction);

                _unitOfWork.Commit();

                if (addedInvoice == null)
                {
                    return NoContent();
                }

                return Ok(true);
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("GetInvoiceDetailsByCustomerId/{customerId}")]
        public async Task<ActionResult<IEnumerable<InvoiceDetailsGetDto>>> GetInvoiceDetailsByCustomerId(int customerId)
        {
            try
            {
                var invoiceDetails = await _unitOfWork.InvoiceDetailsRepository.GetInvoiceDetailsByCustomerId(customerId);

                if (invoiceDetails == null || !invoiceDetails.Any())
                {
                    return NotFound();
                }

                var itemsDto = new List<InvoiceDetailsGetDto>();

                foreach (var item in invoiceDetails)
                {
                    String participantName =await _unitOfWork.CustomerParticipantRepository.GetParticipantNameByInvoiceId(item.InvoiceId);  

                    itemsDto.Add(new InvoiceDetailsGetDto
                    {
                        Id = item.Id,
                        Statement = item.Statement,
                        UnitPrice = item.UnitPrice,
                        Quantity = item.Quantity,
                        InvoiceId = item.InvoiceId,
                        ParticipantName = participantName,
                        EnteredDate = item.Invoice.EnteredDate,
                        ParticipantId=item.Invoice.ParticipantId


                    });
                }

                return Ok(itemsDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
       
        [HttpDelete("DeleteInvoiceDetail/{invoiceDetailId}")]
        public async Task<ActionResult<TransactionGetDto>> DeleteInvoiceDetail(int invoiceDetailId)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                var invoiceDetail = await _unitOfWork.InvoiceDetailsRepository.GetItem(invoiceDetailId);

                if (invoiceDetail == null)
                {
                    return NotFound();
                }

                // Perform any additional business logic or validation here before deleting the invoice detail
              var invoiceItems=  await _unitOfWork.InvoiceDetailsRepository.GetItemsByInvoiceId(invoiceDetail.InvoiceId);
                string totalStatement = "";
                double totalUnitPrice = 0;
                foreach (var item in invoiceItems)
                {
                    if(item.Id!=invoiceDetailId)

                    {
                        totalStatement += item.Statement;
                        totalUnitPrice += (item.UnitPrice*item.Quantity);
                    }
                }
                
                await _unitOfWork.InvoiceDetailsRepository.DeleteItem(invoiceDetail.Id);
            var transaction=    await _unitOfWork.TransactionDetailsRepository.GetItemByInvoiceId(invoiceDetail.InvoiceId);
                transaction.Statement = totalStatement;
                transaction.Debit = totalUnitPrice;
                if(transaction.Debit != 0)
                {
                    await _unitOfWork.TransactionDetailsRepository.UpdateItem(transaction.Id, transaction);

                }
                else
                {
                    await _unitOfWork.TransactionDetailsRepository.DeleteItem(transaction.Id);
                    await _unitOfWork.InvoiceRepository.DeleteItem(invoiceDetail.InvoiceId);

                }

                _unitOfWork.Commit();

                return transaction.ConvertToDto();
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return StatusCode(500, ex.Message);

            }
        }
        [HttpPost("UpdateInvoiceDetail")]
        public async Task<ActionResult<TransactionGetDto>> UpdateInvoiceDetail([FromBody] InvoiceDetailsGetDto  invoiceGetDetail)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                var invoiceDetail = await _unitOfWork.InvoiceDetailsRepository.GetItem(invoiceGetDetail.Id);

                if (invoiceDetail == null)
                {
                    return NotFound();
                }

                // Perform any additional business logic or validation here before deleting the invoice detail
                var invoiceItems = await _unitOfWork.InvoiceDetailsRepository.GetItemsByInvoiceId(invoiceDetail.InvoiceId);
                string totalStatement = invoiceGetDetail.Statement;
                double totalUnitPrice = (invoiceGetDetail.UnitPrice* invoiceGetDetail.Quantity);
                foreach (var item in invoiceItems)
                {
                    if (item.Id != invoiceDetail.Id)
                    {
                        totalStatement += ","+ item.Statement;
                        totalUnitPrice += (item.UnitPrice * item.Quantity);

                    }
                }
                var invoiceDetailsForInsert = new InvoiceDetails { Id= invoiceGetDetail.Id,InvoiceId= invoiceGetDetail.InvoiceId,Quantity= invoiceGetDetail.Quantity,Statement= invoiceGetDetail.Statement,UnitPrice= invoiceGetDetail.UnitPrice};
                await _unitOfWork.InvoiceDetailsRepository.UpdateItem(invoiceDetail.Id, invoiceDetailsForInsert);
                var transaction = await _unitOfWork.TransactionDetailsRepository.GetItemByInvoiceId(invoiceDetail.InvoiceId);
                transaction.Statement = totalStatement;
                transaction.Debit = totalUnitPrice;
                if (transaction.Debit != 0)
                {
                    await _unitOfWork.TransactionDetailsRepository.UpdateItem(transaction.Id, transaction);

                }
                else
                {
                    await _unitOfWork.TransactionDetailsRepository.DeleteItem(transaction.Id);
                    await _unitOfWork.InvoiceRepository.DeleteItem(invoiceDetail.InvoiceId);

                }

                _unitOfWork.Commit();

                return transaction.ConvertToDto();
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return StatusCode(500, ex.Message);

            }
        }
        [HttpGet("GetInvoiceDetailsByInvoiceId/{invoiceId}")]
        public async Task<ActionResult<IEnumerable<InvoiceDetailsGetDto>>> GetInvoiceDetailsByInvoiceId(int invoiceId)
        {
            try
            {
                var invoiceDetails = await _unitOfWork.InvoiceDetailsRepository.GetItemsByInvoiceId(invoiceId);

                if (invoiceDetails == null || !invoiceDetails.Any())
                {
                    return NotFound();
                }

                var itemsDto = new List<InvoiceDetailsGetDto>();

                foreach (var item in invoiceDetails)
                {
                    String participantName = await _unitOfWork.CustomerParticipantRepository.GetParticipantNameByInvoiceId(item.InvoiceId);

                    itemsDto.Add(new InvoiceDetailsGetDto
                    {
                        Id = item.Id,
                        Statement = item.Statement,
                        UnitPrice = item.UnitPrice,
                        Quantity = item.Quantity,
                        InvoiceId = item.InvoiceId,
                        ParticipantName = participantName,
                        EnteredDate = item.Invoice.EnteredDate

                    });
                }

                return Ok(itemsDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }




    }
}