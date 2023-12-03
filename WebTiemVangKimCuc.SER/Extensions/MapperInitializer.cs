using AutoMapper;
using WebTiemVangKimCuc.SER.Domain.Entities;
using WebTiemVangKimCuc.SER.Domain.Entities.DanhMuc;
using WebTiemVangKimCuc.SER.ViewModel.SanPhamVM.Request;
using WebTiemVangKimCuc.SER.ViewModel.SanPhamVM.Response;
using WebTiemVangKimCuc.SER.ViewModel.User.Response;

namespace WebTiemVangKimCuc.SER.Extensions
{
    public class MapperInitializer : Profile
    {
        public MapperInitializer()
        {
            //CreateSanPham
            CreateMap<SanPhamCreateRequest, SanPham>()
                .ForMember(dest => dest.ImgUrl, opt => opt.MapFrom(src => src.ImgUrl))
                .ForMember(dest => dest.TenSanPham, opt => opt.MapFrom(src => src.TenSanPham))
                .ForMember(dest => dest.TrongLuongSanPham, opt => opt.MapFrom(src => src.TrongLuongSanPham))
                .ForMember(dest => dest.ChatLieuId, opt => opt.MapFrom(src => src.ChatLieuId))
                .ForMember(dest => dest.LoaiTrangSucId, opt => opt.MapFrom(src => src.LoaiTrangSucId))
                .ForMember(dest => dest.MoTa, opt => opt.MapFrom(src => src.MoTa))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false)); // Set default value

            // SanPhamResponse
            CreateMap<SanPham, SanPhamResponse>()
                .ForMember(dest => dest.ImgUrl, opt => opt.MapFrom(src => src.ImgUrl))
                .ForMember(dest => dest.TenSanPham, opt => opt.MapFrom(src => src.TenSanPham))
                .ForMember(dest => dest.TrongLuongSanPham, opt => opt.MapFrom(src => src.TrongLuongSanPham))
                .ForMember(dest => dest.ChatLieu, opt => opt.MapFrom(src => src.ChatLieu.ChatLieu))
                .ForMember(dest => dest.LoaiTrangSuc, opt => opt.MapFrom(src => src.LoaiTrangSuc.LoaiTrangSuc))
                .ForMember(dest => dest.MoTa, opt => opt.MapFrom(src => src.MoTa))
                .ForMember(dest => dest.ChatLieuId, opt => opt.MapFrom(src => src.ChatLieuId))
                .ForMember(dest => dest.LoaiTrangSucId, opt => opt.MapFrom(src => src.LoaiTrangSucId))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate))
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => src.UpdatedDate));

            // UpdateSanPham
            CreateMap<SanPhamUpdateRequest, SanPham>()
                .ForMember(dest => dest.ImgUrl, opt => opt.MapFrom(src => src.ImgUrl))
                .ForMember(dest => dest.TenSanPham, opt => opt.MapFrom(src => src.TenSanPham))
                .ForMember(dest => dest.TrongLuongSanPham, opt => opt.MapFrom(src => src.TrongLuongSanPham))
                .ForMember(dest => dest.ChatLieuId, opt => opt.MapFrom(src => src.ChatLieuId))
                .ForMember(dest => dest.LoaiTrangSucId, opt => opt.MapFrom(src => src.LoaiTrangSucId))
                .ForMember(dest => dest.MoTa, opt => opt.MapFrom(src => src.MoTa))
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => src.UpdatedDate));

            // UpdateChatLieu
            CreateMap<DmChatLieu, DmChatLieu>()
                .ForMember(dest => dest.ChatLieu, opt => opt.MapFrom(src => src.ChatLieu))
                .ForMember(dest => dest.MoTa, opt => opt.MapFrom(src => src.MoTa));

            // LoadUser
            CreateMap<User, UserLoadResponse>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName));
        }
    }
}
