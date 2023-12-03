using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using WebTiemVangKimCuc.SER.Domain.SeedWork;
using WebTiemVangKimCuc.SER.Domain.Services;
using WebTiemVangKimCuc.SER.ViewModel;
using WebTiemVangKimCuc.SER.ViewModel.User.Request;

namespace WebTiemVangKimCuc.SER.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ISeedService _service;

        public AuthController(ISeedService service)
        {
            _service = service;
        }

        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Login(UserLoginRequest request)
        {
            try
            {
                var result = await _service.AuthService.ValidateUser(request);

                return Ok(new ResultApi(result));
            }
            catch (AuthenticationException aex)
            {
                return Unauthorized(new ResultApi(aex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResultApi(ex.Message));
            }
        }

        [Authorize]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> LoadUser() 
        {
            try
            {
                var result = await _service.AuthService.LoadUser();

                return Ok(new ResultApi(result));
            }
            catch (AuthenticationException aex)
            {
                return Unauthorized(new ResultApi(aex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResultApi(ex.Message));
            }
        }

        [Authorize]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public IActionResult RefreshToken()
        {
            try
            {
                var result = _service.AuthService.GetNewAccessToken();

                return Ok(new ResultApi(result));
            }
            catch (AuthenticationException aex)
            {
                return Unauthorized(new ResultApi(aex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResultApi(ex.Message));
            }
        }
    }
}
