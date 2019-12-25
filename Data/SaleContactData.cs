using System.ComponentModel.DataAnnotations;

namespace CRM.Data
{
    public class SaleContactData
    {
        [Key]
        public string fso_hdmuabanid { get; set; }
        public string fso_ngaykyket { get; set; }
        public string fso_sohd { get; set; }
        public string _fso_khachhang_value { get; set; }
        public string _fso_duan_value { get; set; }
        public string _fso_batdongsan_value { get; set; }
    }
}