using Dion.Api.Entities;
using Dion.Api.Extensions;
using Dion.Api.Repositories.Contracts;
using Dion.Models.Dtos.Dtos.StoreType;
using Dion.Models.Dtos.Dtos.StoreWallets;
using Dion.Models.Dtos.Dtos.User;
using Dion.Models.Dtos.Dtos.Wallets;
using Microsoft.AspNetCore.Mvc;

namespace Dion.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreWalletsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public StoreWalletsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<StoreWalletsGetDto>> GetStoreWalletById(int id)
        {
            try
            {
                var wallet = await _unitOfWork.StoreWalletsRepository.GetItem(id);
                if (wallet == null)
                {
                    return NotFound();
                }

                var walletDto = new StoreWalletsGetDto
                {
                    Id = wallet.Id,
                    AccountNo = wallet.AccountNo,
                    Details = wallet.Details,
                    EnteredDate = wallet.EnteredDate,
                    StoreId=wallet.Id,
                };

              

                return Ok(walletDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<StoreWalletsGetDto>> CreateStoreWallet([FromBody] StoreWalletsAddDto storeWalletDto)
        {
            try
            {
                // Map the DTO to your entity model
                var newWallet = new StoreWallets
                {
                    AccountNo = storeWalletDto.AccountNo,
                    Details = storeWalletDto.Details,
                    EnteredDate = storeWalletDto.EnteredDate,
                    StoreId = storeWalletDto.StoreId,
                    WalletId = storeWalletDto.WalletId,
                    
                    // You may need to map other properties as needed
                };

                // Add the new wallet to the repository
                var createdWallet = await _unitOfWork.StoreWalletsRepository.AddItem(newWallet);

                // Map the created entity back to DTO
                var createdWalletDto = new StoreWalletsGetDto
                {
                    Id = createdWallet.Id,
                    AccountNo = createdWallet.AccountNo,
                    Details = createdWallet.Details,
                    EnteredDate = createdWallet.EnteredDate,
                    StoreId = createdWallet.StoreId,
                    
                };

                // Return created status with the DTO
                return Ok(createdWalletDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("GetByStoreId/{storeId}")]
        public async Task<ActionResult<IEnumerable<StoreWalletsGetDto>>> GetByStoreId(int storeId)
        {
            try
            {
                var allStoreWallet = await _unitOfWork.StoreWalletsRepository.GetItemsByStoreId(storeId);
                var newItemDtos = new List<StoreWalletsGetDto>();


               foreach (var item in allStoreWallet)
                {
                    var dto = item.ConvertToDto();

                    var walletType = await _unitOfWork.WalletsRepository.GetItem(dto.WalletId);
                    if (string.IsNullOrEmpty(dto.IconPath))
                    {
                        dto.IconPath = $"{Request.Scheme}://{Request.Host}/images/{walletType.IconPath}";
                    }

                    newItemDtos.Add(dto);
                }
                    
                    
                 

                return Ok(newItemDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteWallet(int id)
        {
            try
            {
                var existingWallet = await _unitOfWork.StoreWalletsRepository.GetItem(id);
                if (existingWallet == null)
                {
                    return NotFound();
                }

                await _unitOfWork.StoreWalletsRepository.DeleteItem(id);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
