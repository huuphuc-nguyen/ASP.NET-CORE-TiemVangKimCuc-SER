using System.Text.Json.Serialization;

namespace WebTiemVangKimCuc.SER.Domain.Entities.DanhMuc
{
    public class DmChatLieu
    {
        public int? Id { get; set; }
        public string? ChatLieu { get; set; }
        public string? MoTa { get; set; }

        [JsonIgnore]
        public virtual ICollection<SanPham>? SanPhams { get; set; }
    }
}
