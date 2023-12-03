namespace WebTiemVangKimCuc.SER.Domain.Entities.DanhMuc
{
    public class DmTrangSuc
    {
        public int Id { get; set; }
        public string? LoaiTrangSuc { get; set; }
        public string? MoTa { get; set; }
        public ICollection<SanPham> SanPhams { get; set; }
    }
}
