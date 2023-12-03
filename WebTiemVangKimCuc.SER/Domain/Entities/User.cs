using System.ComponentModel.DataAnnotations;

namespace WebTiemVangKimCuc.SER.Domain.Entities
{
    public class User
    {
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public string? Email { get; set; }
        public string FullName { get; set; }
    }
}
