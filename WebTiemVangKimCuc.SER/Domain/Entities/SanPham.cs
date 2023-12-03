    using System.ComponentModel.DataAnnotations;
    using System.Text.Json.Serialization;
    using WebTiemVangKimCuc.SER.Domain.Entities.DanhMuc;

    namespace WebTiemVangKimCuc.SER.Domain.Entities
    {
        public class SanPham
        {
            public Guid Id { get; set; } = Guid.NewGuid();
            public string? ImgUrl { get; set; }

            [Required(ErrorMessage = "Tên sản phẩm không được để trống.")]
            public string? TenSanPham { get; set; }
            public string? TrongLuongSanPham { get; set; }
            public int ChatLieuId { get; set; }
            public int LoaiTrangSucId { get; set; }
            public string? MoTa { get; set; }
            public DateTime? CreatedDate { get; set; }
            public DateTime? UpdatedDate { get; set; }
            public bool IsDeleted { get; set; } = false;

            [JsonIgnore]
            public virtual DmChatLieu ChatLieu { get; set; }
            [JsonIgnore]
            public virtual DmTrangSuc LoaiTrangSuc { get; set; }
        }
    }
