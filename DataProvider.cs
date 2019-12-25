using Microsoft.EntityFrameworkCore;

namespace CRM
{
    public static class DataProvider
    {
        public static void Initialize()
        {
            CRMContext context = new CRMContext();
            context.Database.EnsureCreated();

            OAuthMessageHandler.AcquireToken();
        }

        public static void GetContacts()
        {
            CRMService service = new CRMService();
            service.GetContacts();
        }

        public static void GetAccounts()
        {
            CRMService service = new CRMService();
            service.GetAccounts();
        }

        public static void GetSaleContacts()
        {
            CRMService service = new CRMService();
            service.GetSaleContacts();
        }

        public static void GetAllURLEntities()
        {
            CRMService service = new CRMService();
            service.GetAllURLEntities();
        }
        
        public static void IsAuthenticated(string username,string password)
        {
            CRMService service = new CRMService();
            service.IsAuthenticated(username, password);
        }
    }
}