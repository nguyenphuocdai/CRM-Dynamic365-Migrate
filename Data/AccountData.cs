using System.ComponentModel.DataAnnotations;

namespace CRM.Data
{
    public class AccountData
    {
        [Key]
        public string accountid { get; set; }
        public string fso_mast { get; set; }
        public string fso_nguoidd { get; set; }
        public string fso_nguoilh { get; set; }
        public string fso_diachi { get; set; }
        public string fso_phuongxa { get; set; }
        public string fso_quanhuyen { get; set; }
        public string fso_quocgia { get; set; }
        public string fso_sonha { get; set; }
        public string telephone1 { get; set; }
    }
}