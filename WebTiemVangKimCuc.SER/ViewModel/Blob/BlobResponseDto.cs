﻿namespace WebTiemVangKimCuc.SER.ViewModel.Blob
{
    public class BlobResponseDto
    {
        public string? Status { get; set; }
        public bool Error { get; set; }
        public BlobDto Blob { get; set; }
        public BlobResponseDto()
        {
            Blob = new BlobDto();
        }
    }
}
