using AutoMapper;
using Microsoft.Extensions.Logging;
using WebTiemVangKimCuc.SER.Domain.Services;
using WebTiemVangKimCuc.SER.Infrastructure;

namespace WebTiemVangKimCuc.SER.Domain.SeedWork
{
    public interface ISeedService
    {
        IFileService FileService { get; }
        ISanPhamService SanPhamService { get; }
        IDanhMucService DanhMucService { get; }
        IAuthService AuthService { get; }
        public class SeedService : ISeedService
        {
            private readonly ILogger _logger;
            private readonly IMapper _mapper;
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly ApplicationDbContext _context;

            public SeedService(ILoggerFactory loggerFactory, IMapper mapper, IHttpContextAccessor httpContextAccessor, ApplicationDbContext context)
            {
                _logger = loggerFactory.CreateLogger("logs");
                _mapper = mapper;
                _httpContextAccessor = httpContextAccessor;
                _context = context;
            }

            private IFileService? _fileService { get; set; }
            public IFileService FileService => _fileService ?? new FileService(_logger);
            private ISanPhamService? _sanPhamService { get; set; }
            public ISanPhamService SanPhamService => _sanPhamService ?? new SanPhamService(_context, _logger, _mapper, FileService);
            private IDanhMucService? _danhMucService { get; set; }
            public IDanhMucService DanhMucService => _danhMucService ?? new DanhMucService(_context, _logger, _mapper);
            private IAuthService _authService { get; set; }
            public IAuthService AuthService => _authService ?? new AuthService(_logger, _context, _httpContextAccessor, _mapper);
        }
    }
}
