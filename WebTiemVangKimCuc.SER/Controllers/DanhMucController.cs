using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using WebTiemVangKimCuc.SER.Domain.Entities.DanhMuc;
using WebTiemVangKimCuc.SER.Domain.SeedWork;
using WebTiemVangKimCuc.SER.ViewModel;

namespace WebTiemVangKimCuc.SER.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class DanhMucController : ControllerBase
    {
        private readonly ISeedService _service;
        public DanhMucController(ISeedService service)
        {
            _service = service;
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetAllChatLieu()
        {
            try
            {
                var result = await _service.DanhMucService.GetAllChatLieu();
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
        public async Task<IActionResult> GetAllLoaiTrangSuc()
        {
            try
            {
                var result = await _service.DanhMucService.GetAllLoaiTrangSuc();
                return Ok(new ResultApi(result));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResultApi(ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        public IActionResult ThemChatLieu(DmChatLieu request)
        { 
            try
            {
                var result = _service.DanhMucService.CreateEntity(request);
               
                return CreatedAtAction(null, new ResultApi(result));
            }
            catch (InvalidOperationException iex)
            {
                return Conflict(new ResultApi(iex.Message));
            }
            catch (DbUpdateException dbex)
            {
                return Conflict(new ResultApi("Database conflict. ID field should be null or empty."));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResultApi(ex.Message));
            }
        }

        [Authorize]
        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public IActionResult SuaChatLieu(DmChatLieu request)
        {
            try 
            {
                var result = _service.DanhMucService.UpdateEntity(request);

                return Ok(new ResultApi(result));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResultApi(ex.Message));
            }
        }
    }
}
