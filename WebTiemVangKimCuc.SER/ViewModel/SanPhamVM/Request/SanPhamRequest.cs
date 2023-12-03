using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebTiemVangKimCuc.SER.ViewModel.SanPhamVM.Request
{
    public class SanPhamBaseRequest
    {
        public string? ImgUrl { get; set; }

        public string? TenSanPham { get; set; }

        public string? TrongLuongSanPham { get; set; }

        public string? MoTa { get; set; }
        public int ChatLieuId { get; set; } 
        public int LoaiTrangSucId { get; set; }
    }
    public class SanPhamCreateRequest : SanPhamBaseRequest
    {
        [JsonIgnore]
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        public IFormFile? Img { get; set; }
    }
    public class SanPhamSearchRequest 
    {
        public string? SearchKey { get; set; }
        public int[] ChatLieus { get; set; } 
        public int[] LoaiTrangSucs { get; set; } 
    }

    public class SanPhamUpdateRequest : SanPhamBaseRequest
    {
        [JsonIgnore]
        public DateTime? UpdatedDate { get; set; } = DateTime.Now;
        public IFormFile? Img { get; set; }
        public Guid Id { get; set; }
    }
}
