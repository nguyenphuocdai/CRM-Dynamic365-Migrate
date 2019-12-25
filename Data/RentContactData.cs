using System.ComponentModel.DataAnnotations;

namespace CRM.Data
{
    public class RentContactData
    {
        [Key]
        public string fso_hdthueid { get; set; }
        public string fso_diachi { get; set; }
        public string fso_ngaykyket { get; set; }
        public string fso_nguoidungten { get; set; }
        public string fso_nguoidungten2 { get; set; }
        public string fso_sohd { get; set; }
        public string _fso_khachhang_value { get; set; }
        public string _fso_duan_value { get; set; }
        public string _fso_batdongsan_value { get; set; }
        public string createdon { get; set; }
        public string statuscode { get; set; }
        public string statecode { get; set; }
    }
}