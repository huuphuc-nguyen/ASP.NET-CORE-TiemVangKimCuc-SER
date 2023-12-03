namespace WebTiemVangKimCuc.SER.ViewModel
{
    public class ResultApi
    {
        public bool Result { get; set; } = true;
        public string? ErrorMessage { get; set; } = String.Empty;
        public object? DataResult { get; set; } = null!;
        public ResultApi()
        {
        }
        public ResultApi(string msg)
        {
            Result = false;
            ErrorMessage = msg;
        }
        public ResultApi(object? Data)
        {
            DataResult = Data;
        }
    }
}
