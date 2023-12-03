using AutoMapper;
using Azure;
using Azure.Core;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Text.Json;
using WebTiemVangKimCuc.SER.Domain.Entities;
using WebTiemVangKimCuc.SER.Domain.Ultilities;
using WebTiemVangKimCuc.SER.Infrastructure;
using WebTiemVangKimCuc.SER.ViewModel.SanPhamVM.Request;
using WebTiemVangKimCuc.SER.ViewModel.SanPhamVM.Response;
using WebTiemVangKimCuc.SER.Domain.Entities.DanhMuc;
using System.Web;
using Newtonsoft.Json;
using WebTiemVangKimCuc.SER.ViewModel;

namespace WebTiemVangKimCuc.SER.Domain.Services
{
    public interface ISanPhamService
    {
        object? CreateEntity(SanPhamCreateRequest request);
        Task<object?> GetAll();
        object? GetBySearch(string filter, string pagination);
        Task<SanPhamResponse?> GetEntity(Guid id);
        Task<object?> SoftDeleteEntity(Guid id);
        Task<object?> UpdateEntity(SanPhamUpdateRequest request);
    }
    public class SanPhamService : ISanPhamService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IFileService _fileService;
        public SanPhamService(ApplicationDbContext context, ILogger logger, IMapper mapper, IFileService fileService)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
            _fileService = fileService;
        }
        public object? CreateEntity(SanPhamCreateRequest request)
        {
            try
            {
                var entity = _mapper.Map<SanPhamCreateRequest, SanPham>(request);

                if (entity == null)
                    return entity;

                if (!IsValidSanPham(entity))
                {
                    _logger.LogWarning("Invalid data provided for adding a new product.");
                    throw new ValidationException("Dữ liệu sản phẩm không hợp lệ.");
                }

                _context.SanPhams.Add(entity);
                _context.SaveChanges();

                var resEntity = _context
                                        .SanPhams
                                        .Include (s => s.ChatLieu)
                                        .Include (s => s.LoaiTrangSuc)
                                        .Where(x => x.Id == entity.Id).FirstOrDefault();

                var response = _mapper.Map<SanPham, SanPhamResponse>(resEntity);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, $"{nameof(CreateEntity)} function error on {nameof(SanPhamService)}");
                throw;
            }
        }
        private bool IsValidSanPham(SanPham sanPham)
        {
            // Use data annotations to validate the SanPham object
            var validationContext = new ValidationContext(sanPham);
            var validationResults = new List<ValidationResult>();
            return Validator.TryValidateObject(sanPham, validationContext, validationResults, true);
        }
        public async Task<object?> GetAll()
        {
            try
            {
                var result = await _context
                                    .SanPhams
                                    .Include(s => s.ChatLieu)
                                    .Include(s => s.LoaiTrangSuc)
                                    .ToListAsync();
                        
                var response = _mapper.Map<List<SanPham>, List<SanPhamResponse>>(result);

                return response; 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, $"{nameof(CreateEntity)} function error on {nameof(SanPhamService)}");
                throw;
            }
        }

        public async Task<SanPhamResponse?> GetEntity(Guid id)
        {
            try
            {
                var entity = await _context
                                    .SanPhams
                                    .Include(x => x.ChatLieu)
                                    .Include(x => x.LoaiTrangSuc)
                                    .Where(x => x.Id == id)
                                    .FirstOrDefaultAsync();

                var response = _mapper.Map<SanPham, SanPhamResponse>(entity);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, $"{nameof(CreateEntity)} function error on {nameof(SanPhamService)}");
                throw;
            }
        }

        public async Task<object?> SoftDeleteEntity(Guid id)
        {
            try
            {
                var entity = _context
                                    .SanPhams
                                    .Include(x => x.ChatLieu)
                                    .Include(x => x.LoaiTrangSuc)
                                    .Where(x => x.Id == id)
                                    .FirstOrDefault();

                // Delete image
                if (!String.IsNullOrEmpty(entity.ImgUrl))
                {
                    string fileName = StringUltil.getFileNameByUrl(entity.ImgUrl);

                    await _fileService.DeleteAsync(fileName);
                }

                // Delete product
                if (entity != null)
                {
                    entity.IsDeleted = true;
                    entity.ImgUrl = string.Empty;
                    _context.SanPhams.Update(entity);
                    await _context.SaveChangesAsync();
                }

                var response = _mapper.Map<SanPham, SanPhamResponse>(entity);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, $"{nameof(SoftDeleteEntity)} function error on {nameof(SanPhamService)}");
                throw;
            }
        }

        public object? GetBySearch(string filter, string pagination)
         {
            try
            {
                // Decode request
                string JsonSearchRequest = HttpUtility.UrlDecode(filter);
                string JsonPagination = HttpUtility.UrlDecode(pagination);

                // Convert Json to Object
                SanPhamSearchRequest searchRequest = JsonConvert.DeserializeObject<SanPhamSearchRequest>(JsonSearchRequest);
                PaginationRequest paginationRequest = JsonConvert.DeserializeObject<PaginationRequest>(JsonPagination);
                int pageSize = paginationRequest.PageSize;
                int pageIndex = paginationRequest.PageIndex-1;

                // Searching
                string[] searchWords = null;

                if (searchRequest.SearchKey != null)
                {
                    string searchValue = StringUltil.convertToUnSign(searchRequest.SearchKey).ToLower();
                    searchWords = searchValue.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                }

                var entities = _context
                                    .SanPhams
                                    .Include(s => s.ChatLieu)
                                    .Include(s => s.LoaiTrangSuc);

                Func<SanPham, bool> filterCondition = (sanPham) =>
                {
                    bool matches = true;

                    if (searchWords != null)
                    {
                        matches = matches && searchWords.All(word =>
                                                    StringUltil.convertToUnSign(sanPham.TenSanPham).ToLower().Contains(word)
                                                 || StringUltil.convertToUnSign(sanPham.TrongLuongSanPham).ToLower().Contains(word)
                                                 || StringUltil.convertToUnSign(sanPham.ChatLieu.ChatLieu).ToLower().Contains(word)
                                                 || StringUltil.convertToUnSign(sanPham.LoaiTrangSuc.LoaiTrangSuc).ToLower().Contains(word));
                    }

                    if (searchRequest.ChatLieus != null)
                    {
                        matches = matches && searchRequest.ChatLieus.Any(id => sanPham.ChatLieuId == id);
                    }

                    if (searchRequest.LoaiTrangSucs != null)
                    {
                        matches = matches && searchRequest.LoaiTrangSucs.Any(id => sanPham.LoaiTrangSucId == id);
                    }

                    return matches;
                };

                var filteredEntities = entities
                                            .Where(filterCondition)
                                            .OrderByDescending(s => s.CreatedDate);

                var paginationEntities = filteredEntities
                                            .Skip(pageSize * pageIndex)
                                            .Take(pageSize)
                                            .ToList();

                var totalRows = filteredEntities.Count();

                var responseDTO = new SanPhamSearchResponse<SanPhamResponse>
                {
                    Data = _mapper.Map<List<SanPham>, List<SanPhamResponse>>(paginationEntities),
                    TotalRows = totalRows,
                    PageIndex = pageIndex + 1,
                    PageSize = pageSize,
                    TotalPages = totalRows % pageSize == 0 ? totalRows / pageSize : totalRows / pageSize + 1
                };

                return responseDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, $"{nameof(GetBySearch)} function error on {nameof(SanPhamService)}");
                throw;
            }
        }
        
        public async Task<object?> UpdateEntity (SanPhamUpdateRequest request)
        {
            try
            {
                var entity = _context
                                .SanPhams
                                .Include(s => s.ChatLieu) 
                                .Include(s => s.LoaiTrangSuc)
                                .Where(x => x.Id == request.Id)
                                .FirstOrDefault();

                if (entity != null)
                {
                    if (request.Img != null)
                    {
                        // Delete old image
                        var oldImgUrl = entity.ImgUrl;

                        if (!String.IsNullOrEmpty(oldImgUrl))
                        {
                            var fileName = StringUltil.getFileNameByUrl(oldImgUrl);

                            await _fileService.DeleteAsync(fileName);
                        }

                        // Upload new image
                        var file = request.Img;
                        var BlobDto = await _fileService.UploadAsync(file);
                        request.ImgUrl = BlobDto.Uri;
                    }
                    else
                    {
                        request.ImgUrl = entity.ImgUrl;
                    }

                    _mapper.Map<SanPhamUpdateRequest, SanPham>(request, entity);

                    _context.Update(entity);
                    await _context.SaveChangesAsync();

                    var result = await _context
                                        .SanPhams
                                        .Include(s => s.ChatLieu)
                                        .Include(s => s.LoaiTrangSuc)
                                        .Where(s => s.Id == entity.Id)
                                        .FirstOrDefaultAsync();

                    var response = _mapper.Map<SanPham, SanPhamResponse>(result);

                    return response;
                }
                else
                {
                    throw new Exception("Product does not exist.");
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message, $"{nameof(UpdateEntity)} function error on {nameof(SanPhamService)}");
                throw;
            }
        }
    }
}
