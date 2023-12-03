using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebTiemVangKimCuc.SER.Domain.Entities;
using WebTiemVangKimCuc.SER.Domain.Entities.DanhMuc;
using WebTiemVangKimCuc.SER.Domain.Ultilities;
using WebTiemVangKimCuc.SER.Infrastructure;

namespace WebTiemVangKimCuc.SER.Domain.Services
{
    public interface IDanhMucService
    {
        // ChatLieuService
        Task<object?> GetAllChatLieu();
        object CreateEntity(DmChatLieu request);
        object UpdateEntity(DmChatLieu request);

        // LoaiTrangSucService
        Task<object?> GetAllLoaiTrangSuc();
    }
    public class DanhMucService : IDanhMucService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        public DanhMucService(ApplicationDbContext context, ILogger logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<object?> GetAllChatLieu()
        {
            try
            {
                var result = await _context.PhanLoaiChatLieus.ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, $"{nameof(GetAllChatLieu)} function error on {nameof(DanhMucService)}");
                throw;
            }
        }

        public object CreateEntity(DmChatLieu request)
        {
            try
            {
                string[] searchWords = null;
                string searchValue = StringUltil.convertToUnSign(request.ChatLieu).ToLower();
                searchWords = searchValue.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                

                Func<DmChatLieu, bool> filterCondition = (DmChatLieu) =>
                {
                    bool matches = true;

                    if (searchWords != null)
                    {
                        matches = matches && searchWords.All(word =>
                                                    StringUltil.convertToUnSign(DmChatLieu.ChatLieu).ToLower().Contains(word));
                    }

                    return matches;
                };

                var filteredEntities = _context
                                            .PhanLoaiChatLieus
                                            .Where(filterCondition)
                                            .FirstOrDefault();

                if (filteredEntities != null)
                    throw new InvalidOperationException("The resource already exists.");

                _context.PhanLoaiChatLieus.Add(request);
                _context.SaveChanges();

                return request;
            }
            catch (DbUpdateException dbex)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, $"{nameof(CreateEntity)} function error on {nameof(DanhMucService)}");
                throw;
            }
        }

        public object UpdateEntity(DmChatLieu request)
        {
            try
            {
                var entity = _context.PhanLoaiChatLieus.Where(x => x.Id == request.Id).FirstOrDefault();

                _mapper.Map<DmChatLieu, DmChatLieu>(request, entity);

                _context.Update(entity);
                _context.SaveChanges();

                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, $"{nameof(UpdateEntity)} function error on {nameof(DanhMucService)}");
                throw;
            }
        }

        public async Task<object?> GetAllLoaiTrangSuc()
        {
            try
            {
                var result = await _context.PhanLoaiTrangSucs.Select(entity => new
                {
                    Id = entity.Id,
                    IdForSEO = StringUltil.convertToSEOId(entity.LoaiTrangSuc),
                    LoaiTrangSuc = entity.LoaiTrangSuc,
                    MoTa = entity.MoTa,
                }).ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, $"{nameof(GetAllLoaiTrangSuc)} function error on {nameof(DanhMucService)}");
                throw;
            }
        }
    }
}
