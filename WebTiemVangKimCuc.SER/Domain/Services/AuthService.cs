using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WebTiemVangKimCuc.SER.Domain.Entities;
using WebTiemVangKimCuc.SER.Infrastructure;
using WebTiemVangKimCuc.SER.ViewModel.User.Request;
using WebTiemVangKimCuc.SER.ViewModel.User.Response;

namespace WebTiemVangKimCuc.SER.Domain.Services
{
    public interface IAuthService
    {
        Task<object?> ValidateUser(UserLoginRequest request);
        Task<object?> LoadUser();
        object GetNewAccessToken();
    }
    public class AuthService : IAuthService
    {
        private readonly ILogger _logger;
        private readonly ApplicationDbContext _context; 
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public AuthService(ILogger logger, ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        public async Task<object?> ValidateUser(UserLoginRequest request)
        {
            try
            {
                var user = await _context
                                   .Users
                                   .Where(x => (x.UserName.Equals(request.UserName) && x.PassWord.Equals(request.PassWord)))
                                   .FirstOrDefaultAsync();

                if (user == null)
                {
                    throw new AuthenticationException("Invalid UserName Or Password");
                }
                else
                {
                    var accessToken = GenerateAccessToken(request.UserName);
                    var refreshToken = GenerateRefreshToken(request.UserName);

                    return new
                    {
                        AccessToken = accessToken,
                        RefreshToken = refreshToken
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, $"{nameof(ValidateUser)} function error on {nameof(AuthService)}");
                throw;
            }
        }

        private string GenerateAccessToken(string username)
        {
            DotNetEnv.Env.Load();

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("KEY")));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


            // Create access token
            var accessToken = new JwtSecurityToken(
                Environment.GetEnvironmentVariable("ISSUER"),
                Environment.GetEnvironmentVariable("AUDIENCE"),
                claims,
                expires: DateTime.Now.AddHours(5),
                signingCredentials: credentials
            );

            var accessTokenString = new JwtSecurityTokenHandler().WriteToken(accessToken);

            return accessTokenString;
        }

        private string GenerateRefreshToken(string username)
        {
            var claims = new[]
            {   
                new Claim(ClaimTypes.Name, username),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("KEY")));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Create access token
            var refreshToken = new JwtSecurityToken(
                Environment.GetEnvironmentVariable("ISSUER"),
                Environment.GetEnvironmentVariable("AUDIENCE"),
                claims,
                expires: DateTime.Now.AddMonths(1),
                signingCredentials: credentials
            );

            var refreshTokenString = new JwtSecurityTokenHandler().WriteToken(refreshToken);

            return refreshTokenString;
        }

        public async Task<object?> LoadUser()
        {
            try
            {
                HttpContext httpContext = _httpContextAccessor.HttpContext;

                var usernameClaim = httpContext?.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);

                if (usernameClaim != null)
                {
                    string username = usernameClaim.Value;

                    var user = await _context.Users.Where(x => x.UserName.Equals(username)).FirstOrDefaultAsync();

                    if (user != null)
                    {
                        var response = _mapper.Map<User, UserLoadResponse>(user);

                        return response;
                    }
                    else
                    {
                        // Username get from JWT must exist in database
                        // Because the username is put into jwt after validating with database
                        // The only case can happen is that usernam is deleted from db while jwt still valid.
                        throw new AuthenticationException("User does not exist in database. Please check database again.");
                    }
                }
                else
                {
                    throw new AuthenticationException("UserName in jwt is null");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, $"{nameof(LoadUser)} function error on {nameof(AuthService)}");
                throw;
            }
        }

        public object GetNewAccessToken()
        {
            try
            {
                var usernameClaim = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);

                if (usernameClaim != null)
                {
                    string username = usernameClaim.Value;
                    var response = GenerateAccessToken(username);

                    return response;
                }
                else
                {
                    // Username get from JWT must exist in database
                    // Because the username is put into jwt after validating with database
                    // The only case can happen is that usernam is deleted from db while jwt still valid.
                    throw new AuthenticationException("User does not exist in database. Please check database again.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, $"{nameof(GetNewAccessToken)} function error on {nameof(AuthService)}");
                throw;
            }
        }
    }

}
