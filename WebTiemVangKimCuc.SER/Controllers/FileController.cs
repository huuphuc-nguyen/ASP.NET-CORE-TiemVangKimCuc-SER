using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebTiemVangKimCuc.SER.Domain.SeedWork;
using WebTiemVangKimCuc.SER.ViewModel;

namespace WebTiemVangKimCuc.SER.Controllers
{
    [ApiController]
    [Route("/api/[controller]/[action]")]
    public class FileController : ControllerBase
    {
        private readonly ISeedService _service;
        public FileController(ISeedService service)
        {
            _service = service;
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Upload(IFormFile UploadFile)
        {
            try
            {
                var result = await _service.FileService.UploadAsync(UploadFile);
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
                var result = await _service.FileService.ListAsync();
                return CreatedAtAction(null, new ResultApi(result));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResultApi(ex.Message));
            }
        }

        [Authorize]
        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteBlob (string fileName)
        {
            try
            {
                var result = await _service.FileService.DeleteAsync(fileName);
                return Ok(result);
            }
            catch (FileNotFoundException ex)
            {
                return NoContent();
            }
            catch(Exception ex)
            {
                return BadRequest(new ResultApi(ex.Message));
            }
        }
    }
}
