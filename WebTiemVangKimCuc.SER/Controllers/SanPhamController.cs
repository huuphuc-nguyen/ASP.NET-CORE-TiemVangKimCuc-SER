using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using WebTiemVangKimCuc.SER.Domain.SeedWork;
using WebTiemVangKimCuc.SER.Domain.Services;
using WebTiemVangKimCuc.SER.Domain.Ultilities;
using WebTiemVangKimCuc.SER.ViewModel;
using WebTiemVangKimCuc.SER.ViewModel.SanPhamVM.Request;

namespace WebTiemVangKimCuc.SER.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class SanPhamController : ControllerBase
    {
        //private readonly ISanPhamService _sanPhamService;
        //private readonly IFileService _fileService;
        private readonly ISeedService _service;
        public SanPhamController(ISeedService service)
        {
            _service = service;
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ThemSanPham([FromForm]SanPhamCreateRequest request)
        {
            try
            {
                // Upload file to Storage account
                if (request.Img != null)
                {
                    IFormFile file = request.Img;

                    var BlobDto =  await _service.FileService.UploadAsync(file);

                    // Get the Uri of file in Storage account
                    request.ImgUrl = BlobDto.Uri;
                }
                
                var result = _service.SanPhamService.CreateEntity(request);

                return CreatedAtAction(null, new ResultApi(result));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResultApi(ex.Message));
            }
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _service.SanPhamService.GetAll();
                return Ok(new ResultApi(result));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResultApi(ex.Message));
            }
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetSanPham(Guid id)
        {
            try
            {
                var result = await _service.SanPhamService.GetEntity(id);
                return Ok(new ResultApi(result));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResultApi(ex.Message));
            }
        }

        [Authorize]
        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> XoaSanPham(Guid id)
        {
            try
            {
                var result =  await _service.SanPhamService.GetEntity(id);

                // Delete image
                if (!String.IsNullOrEmpty(result.ImgUrl))
                {
                    string fileName = StringUltil.getFileNameByUrl(result.ImgUrl);

                    await _service.FileService.DeleteAsync(fileName);
                }

                // Delete entity
                await _service.SanPhamService.SoftDeleteEntity(id);

                return Ok(new ResultApi(result));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResultApi(ex.Message));
            }
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public IActionResult SearchSanPham(string filter, string pagination)
        {
            try
            {
                var result = _service.SanPhamService.GetBySearch(filter, pagination);

                return Ok(new ResultApi(result));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResultApi(ex.Message));
            }
        }

        [Authorize]
        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateSanPham([FromForm]SanPhamUpdateRequest request)
        {
            try
            {
                var result = await _service.SanPhamService.UpdateEntity(request);

                return Ok(new ResultApi(result));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResultApi(ex.Message));
            }
        }
    }
}
