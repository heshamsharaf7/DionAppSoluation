using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dion.Api.Entities;
using Dion.Api.Repositories.Contracts;
using Dion.Api.Repositories;
using Dion.Models.Dtos.Dtos.Wallets;

namespace Dion.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public WalletController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<WalletsGetDto>>> GetWallets()
        {
            try
            {
                var wallets = await _unitOfWork.WalletsRepository.GetItems();
                if (wallets == null || !wallets.Any())
                {
                    return NoContent();
                }

                var walletDtos = wallets.Select(w => new WalletsGetDto
                {
                    Id = w.Id,
                    Name = w.Name,
                    IconPath = !string.IsNullOrEmpty(w.IconPath)
                        ? $"{Request.Scheme}://{Request.Host}/images/{w.IconPath}"
                        : null,
                    EnteredDate = w.EnteredDate
                });

                return Ok(walletDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<WalletsGetDto>> GetWalletById(int id)
        {
            try
            {
                var wallet = await _unitOfWork.WalletsRepository.GetItem(id);
                if (wallet == null)
                {
                    return NotFound();
                }

                var walletDto = new WalletsGetDto
                {
                    Id = wallet.Id,
                    Name = wallet.Name,
                    IconPath = wallet.IconPath,
                    EnteredDate = wallet.EnteredDate
                };

                // Append the base URL to the IconPath to get the complete image URL
                if (!string.IsNullOrEmpty(walletDto.IconPath))
                {
                    walletDto.IconPath = $"{Request.Scheme}://{Request.Host}/images/{walletDto.IconPath}";
                }

                return Ok(walletDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpPost]
        public async Task<ActionResult<WalletsGetDto>> CreateWallet([FromForm] WalletsAddDto walletDto, IFormFile iconFile)
        {
            try
            {
                string uniqueFileName = null;

                if (iconFile != null)
                {
                    string uploadsFolder = Path.Combine("wwwroot", "images");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + iconFile.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await iconFile.CopyToAsync(fileStream);
                    }
                }

                var newWallet = new Wallets
                {
                    Id = 0,

                    Name = walletDto.Name,
                    IconPath = uniqueFileName,
                    EnteredDate = walletDto.EnteredDate
                };

                var createdWallet = await _unitOfWork.WalletsRepository.AddItem(newWallet);

                var createdWalletDto = new WalletsGetDto
                {
                    Id = createdWallet.Id,
                    Name = createdWallet.Name,
                    IconPath = createdWallet.IconPath,
                    EnteredDate = createdWallet.EnteredDate
                };

                return CreatedAtAction(nameof(GetWalletById), new { id = createdWallet.Id }, createdWalletDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<WalletsGetDto>> UpdateWallet(int id, [FromBody] WalletsGetDto walletDto)
        {
            try
            {
                var existingWallet = await _unitOfWork.WalletsRepository.GetItem(id);
                if (existingWallet == null)
                {
                    return NotFound();
                }

                existingWallet.Name = walletDto.Name;
                existingWallet.IconPath = walletDto.IconPath;
                existingWallet.EnteredDate = walletDto.EnteredDate;

                var updatedWallet = await _unitOfWork. WalletsRepository.UpdateItem(existingWallet.Id,existingWallet);

                var updatedWalletDto = new WalletsGetDto
                {
                    Id = updatedWallet.Id,
                    Name = updatedWallet.Name,
                    IconPath = updatedWallet.IconPath,
                    EnteredDate = updatedWallet.EnteredDate
                };

                return Ok(updatedWalletDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}/icon-path")]
        public async Task<ActionResult<string>> GetIconPathById(int id)
        {
            try
            {
                var wallet = await _unitOfWork.WalletsRepository.GetItem(id);
                if (wallet == null)
                {
                    return NotFound($"Wallet with ID {id} not found.");
                }

                var fullIconPath = string.IsNullOrEmpty(wallet.IconPath)
                    ? null
                    : $"{Request.Scheme}://{Request.Host}/images/{wallet.IconPath}";

                return Ok(fullIconPath);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteWallet(int id)
        {
            try
            {
                var existingWallet = await _unitOfWork.WalletsRepository.GetItem(id);
                if (existingWallet == null)
                {
                    return NotFound();
                }

                await _unitOfWork.WalletsRepository.DeleteItem(id);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}