using Microsoft.AspNetCore.Mvc;
using Dion.Api.Repositories.Contracts;
using Dion.Models.Dtos.Dtos.StoreType;
using Dion.Api.Entities;
using Dion.Api.Extensions;
using Dion.Api.UnitOfWork;
using Dion.Models.Dtos.Dtos.Wallets;

namespace Dion.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController] 
    public class StoreTypeController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public StoreTypeController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpPost]
        public async Task<ActionResult<StroreTypeGetDto>> PostItem([FromForm] StroreTypeAddDto stroreTypeAddDto, IFormFile iconFile)
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

                var newStoreType = new StroreType
                {
                    Id = 0,

                    Name = stroreTypeAddDto.Name,
                    IconPath = uniqueFileName,
                    EnteredDate = stroreTypeAddDto.EnteredDate,
                    Details= stroreTypeAddDto.Details,
                    
                };

                var createdStoreType = await unitOfWork.StoreTypeRepository.AddItem(newStoreType);

                var createdStoreTypeDto = new StroreTypeGetDto
                {
                    Id = createdStoreType.Id,
                    Name = createdStoreType.Name,
                    IconPath = createdStoreType.IconPath,
                    EnteredDate = createdStoreType.EnteredDate,
                    Details= createdStoreType.Details,
                };

                return Ok(createdStoreTypeDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<bool>> DeleteItem(int id)
        {
            try
            {
                unitOfWork.BeginTransaction();

                var storeTypeItem = await this.unitOfWork.StoreTypeRepository.DeleteItem(id);
                unitOfWork.Commit();


                if (storeTypeItem !=true)
                {
                    return NotFound();
                }




                return Ok(storeTypeItem);

            }
            catch (Exception ex)
            {
                this.unitOfWork.Rollback();

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StroreTypeGetDto>>> GetItems()
        {
            try
            {
                unitOfWork.BeginTransaction();
                var storeTypesItems = await this.unitOfWork.StoreTypeRepository.GetItems();
                unitOfWork.Commit();

                if (storeTypesItems == null)
                {
                    return NoContent();
                }

                var newstoreTypeItemDtos = new List<StroreTypeGetDto>();

                foreach (var item in storeTypesItems)
                {
                    var dto = item.ConvertToDto();

                    // Add the file path to the DTO if available
                    if (!string.IsNullOrEmpty(item.IconPath))
                    {
                        dto.IconPath = $"{Request.Scheme}://{Request.Host}/images/{item.IconPath}";
                    }

                    newstoreTypeItemDtos.Add(dto);
                }

                return Ok(newstoreTypeItemDtos);
            }
            catch (Exception ex)
            {
                this.unitOfWork.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult<StroreTypeGetDto>> UpdateItem(int id, [FromForm] StroreTypeGetDto storeTypeItemDto, IFormFile file)
        {
            try
            {
                StroreType t = new StroreType();
                t.Id = id;
                t.Name = storeTypeItemDto.Name;
                t.EnteredDate = storeTypeItemDto.EnteredDate;
                t.Details = storeTypeItemDto.Details;

                if (file != null && file.Length > 0)
                {
                    var filePath = Path.Combine("wwwroot", "images", Guid.NewGuid().ToString() + Path.GetExtension(file.FileName));

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    t.IconPath = filePath; // Update the file path in the entity
                }

                unitOfWork.BeginTransaction();
                var storeTypeItem = await this.unitOfWork.StoreTypeRepository.UpdateItem(id, t);
                unitOfWork.Commit();

                if (storeTypeItem == null)
                {
                    return NotFound();
                }

                var storeTypeDto = storeTypeItem.ConvertToDto();

                return Ok(storeTypeDto);
            }
            catch (Exception ex)
            {
                this.unitOfWork.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

    }
}
