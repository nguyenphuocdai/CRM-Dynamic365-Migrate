using System.ComponentModel.DataAnnotations;

namespace CRM.Data
{
    public class ContactData
    {
        [Key]
        public string contactid { get; set; }
        public string fullname { get; set; }
        public string gendercode { get; set; }
        public string fso_diachi { get; set; }
        public string fso_didong { get; set; }
        public string fso_namsinh { get; set; }
        public string fso_nganhnghe { get; set; }
        public string fso_ngaycap { get; set; }
        public string fso_ngaysinh { get; set; }
        public string fso_noicapcmndpassport { get; set; }
        public string fso_phuongxa  { get; set; }
        public string fso_quanhuyen { get; set; }
        public string fso_quocgia { get; set; }
        public string fso_quoctich { get; set; }
        public string fso_socmndpassport { get; set; }
        public string fso_sonha { get; set; }
        public string fso_tinhthanh { get; set; }
        public string description { get; set; }
    }
}