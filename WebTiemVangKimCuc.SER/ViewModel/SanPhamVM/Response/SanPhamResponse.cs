namespace WebTiemVangKimCuc.SER.ViewModel.SanPhamVM.Response
{
    public class SanPhamResponse
    {
        public Guid id { get; set; }
        public string? ImgUrl { get; set; }
        public string? TenSanPham { get; set; }
        public string? TrongLuongSanPham { get; set; }
        public string? ChatLieu { get; set; }
        public string? LoaiTrangSuc { get; set; }
        public int ChatLieuId { get; set; }
        public int LoaiTrangSucId { get; set; }
        public string? MoTa { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
    public class SanPhamSearchResponse<T> : PaginationRequest
    {
        public List<T> Data { get; set; } = new List<T>();
        public int TotalRows { get; set; }
        public int TotalPages { get; set; }
    }
}
