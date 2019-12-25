using CRM.Data;
using Microsoft.EntityFrameworkCore;

namespace CRM
{
    public class CRMContext : DbContext
    {
        public static string connectionString = "Data Source=DAINP\\SQLEXPRESS;Initial Catalog=CRM_Online;User ID=sa;Password=qqq111!!!";
        public DbSet<ContactData> Contacts { get; set; }
        public DbSet<AccountData> Accounts { get; set; }
        public DbSet<SaleContactData> SaleContacts { get; set; }
        public DbSet<RentContactData> RentContacts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //string connectionString = "Data Source=DAINP\\SQLEXPRESS;Initial Catalog=CRM_Online;User ID=sa;Password=qqq111!!!";
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}