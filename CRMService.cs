using CRM.Data;
using javax.management;
using Microsoft.Data.SqlClient;
using Microsoft.Identity.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.Protocols;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using MessageBox = System.Windows.Forms.MessageBox;
using SearchScope = System.DirectoryServices.Protocols.SearchScope;

namespace CRM
{
    public class CRMService
    {
        //private static string ServiceUrl = "https://ttcland.crm5.dynamics.com/";   // CRM Online Prod
        private static string ServiceUrl = "https://ttclanduat.crm5.dynamics.com/";   // CRM Online UAT
        private static string Domain = "ttcland";  //CRM server domain
        private static string Username = "dev1@ttcland.vn";  //CRM user account
        private static string Password = "Ttc@123Land";


        public Task<HttpMessageHandler> OpenConnect()
        {
            HttpMessageHandler messageHandler;
            if (ServiceUrl.StartsWith("https://"))
            {
                NetworkCredential credentials = new NetworkCredential(Username, Password, Domain);
                messageHandler = new OAuthMessageHandler(new HttpClientHandler { Credentials = credentials });
            }
            else
            {
                NetworkCredential credentials = new NetworkCredential(Username, Password, Domain);
                messageHandler = new HttpClientHandler { Credentials = credentials };
            }

            return Task.FromResult(messageHandler);
        }


        public async void GetContacts()
        {
            string entityName = "contacts";
            HttpMessageHandler messageHandler = await OpenConnect();
            JArray array = GetAll(messageHandler, entityName);
            if (array == null)
            {
                return;
            }

            CRMContext context = new CRMContext();
            foreach (JObject data in array)
            {
                ContactData contact = data.ToObject<ContactData>();
                context.Contacts.Add(contact);
            }

            MessageBox.Show("Begin Sync Database");
            context.SaveChanges();
        }

        public async void GetAccounts()
        {
            string entityName = "accounts";
            HttpMessageHandler messageHandler = await OpenConnect();
            JArray array = GetAll(messageHandler, entityName);
            if (array == null)
            {
                return;
            }

            CRMContext context = new CRMContext();
            foreach (JObject data in array)
            {
                AccountData account = data.ToObject<AccountData>();
                context.Accounts.Add(account);
            }

            MessageBox.Show("Begin Sync Database");
            context.SaveChanges();
        }

        public async void GetSaleContacts()
        {
            string entityName = "fso_hdmuabans";
            HttpMessageHandler messageHandler = await OpenConnect();
            JArray array = GetAll(messageHandler, entityName);
            if (array == null)
            {
                return;
            }

            CRMContext context = new CRMContext();
            foreach (JObject data in array)
            {
                SaleContactData contact = data.ToObject<SaleContactData>();
                context.SaleContacts.Add(contact);
            }

            MessageBox.Show("Begin Sync Database");
            context.SaveChanges();
        }

        public async void GetAllURLEntities()
        {
            HttpMessageHandler messageHandler = await OpenConnect();
            JArray array = GetAllURLEntities(messageHandler);
            if (array == null)
            {
                return;
            }

            int countSuccess = 0;
            int countSkipTable = 0;
            foreach (JObject data in array)
            {
                string url = data["url"].ToString();
                string lastCharacter = url.Substring(url.Length - 1);
                if (lastCharacter == "s")
                {
                    url = url.Remove(url.Length - 1);
                }

                JArray arrayAttributes = GetAllAttributeEntities(messageHandler, url);
                if (arrayAttributes == null)
                {
                    countSkipTable++;
                    continue;
                }

                List<SQLModelData> listObjectModelData = new List<SQLModelData>();


                foreach (JObject attribute in arrayAttributes)
                {
                    SQLModelData sqlModel = new SQLModelData();
                    int maxLength = 250;
                    bool isPrimary = false;

                    string schemaName = attribute["SchemaName"].ToString();
                    string dataType = attribute["AttributeType"].ToString();

                    if (attribute["MaxLength"] != null)
                    {
                        string length = attribute["MaxLength"].ToString();
                        maxLength = int.Parse(length);
                    }
                    if (attribute["IsPrimaryId"] != null)
                    {
                        string primaryKey = attribute["IsPrimaryId"].ToString();
                        isPrimary = primaryKey == "True";
                    }

                    if (maxLength >= 4000)
                    {
                        maxLength = 4000;
                    }
                    switch (dataType)
                    {
                        case "String":
                        case "":
                        case "Virtual":
                        case "Lookup":
                        case "Double":
                        case "Memo":
                        case "Owner":
                        case "EntityName":
                        case "PartyList":
                        case "ManagedProperty":
                        case "Customer":

                            dataType = $"nvarchar({maxLength})";
                            break;
                        case "Picklist":
                            dataType = $"nvarchar({maxLength})";
                            break;
                        case "Boolean":
                            dataType = "Bit";
                            break;
                        case "Status":
                        case "State":
                            dataType = "Int";
                            break;
                    }

                    sqlModel.SchemnaName = schemaName;
                    sqlModel.MaxLength = maxLength;
                    sqlModel.DataType = dataType;
                    sqlModel.IsPrimaryKey = isPrimary;

                    listObjectModelData.Add(sqlModel);
                }

                string queryTableCreate = @"
                          IF NOT EXISTS (select * from sysobjects where name='{0}')
                            BEGIN  
                                CREATE TABLE [dbo].[{0}](
	                                {1} {2}
                            END
                            
                ";
                string queryField = @"[{0}] {1} {2}";

                string queryPrimaryKey = @"CONSTRAINT [PK_{0}] PRIMARY KEY CLUSTERED 
                            (
	                            {1} DESC
                            ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) 
                            ON [PRIMARY]
                            ";

                List<string> listPrimaryKey = new List<string>();
                string script = string.Empty;
                string joined = string.Empty;
                string queryListField = string.Empty;
                string queryAddPrimaryKey = string.Empty;

                foreach (SQLModelData objecModelData in listObjectModelData)
                {
                    queryListField += string.Format(queryField, objecModelData.SchemnaName, objecModelData.DataType,
                        objecModelData.IsPrimaryKey ? "NOT NULL," : "NULL,");

                    if (objecModelData.IsPrimaryKey)
                    {
                        listPrimaryKey.Add(objecModelData.SchemnaName);
                    }
                    joined = string.Join<string>(" DESC ,", listPrimaryKey);
                    queryAddPrimaryKey = string.Format(queryPrimaryKey, url, joined);
                }

                if (listPrimaryKey.Count == 0)
                {
                    script = string.Format(queryTableCreate, url, queryListField, ")");
                }
                else
                {
                    script = string.Format(queryTableCreate, url, queryListField, queryAddPrimaryKey);
                }


                using (SqlConnection connection = new SqlConnection(
                    CRMContext.connectionString))
                {
                    SqlCommand command = new SqlCommand(script, connection);
                    command.Connection.Open();
                    command.ExecuteNonQuery();
                }
                countSuccess++;
            }
            var count = countSuccess + countSkipTable;
            MessageBox.Show($"Total Database {count} Success: {countSuccess}. Not Attribute: {countSkipTable}");
        }

        private JArray GetAllURLEntities(HttpMessageHandler messageHandler)
        {
            HttpClient httpClient = new HttpClient(messageHandler);
            //Specify the Web API address of the service and the period of time each request 
            // has to execute.
            httpClient.BaseAddress = new Uri(ServiceUrl);
            httpClient.Timeout = new TimeSpan(0, 2, 0);  //2 minutes

            //Send the WhoAmI request to the Web API using a GET request. 
            var response = httpClient.GetAsync(
                    $"api/data/v9.0",
                    HttpCompletionOption.ResponseHeadersRead)
                .Result;

            if (response.IsSuccessStatusCode)
            {
                //Get the response content and parse it.
                JObject body = JObject.Parse(response.Content.ReadAsStringAsync().Result);
                return body.GetValue("value") as JArray;
            }
            //else
            //{
            //    MessageBox.Show($"The request failed with a status of '{response.ReasonPhrase}'");
            //    return null;
            //}
            return null;
        }

        private JArray GetAllAttributeEntities(HttpMessageHandler messageHandler, string logicalName)
        {
            HttpClient httpClient = new HttpClient(messageHandler);
            httpClient.BaseAddress = new Uri(ServiceUrl);
            httpClient.Timeout = new TimeSpan(0, 2, 0);  //2 minutes

            var response = httpClient.GetAsync(
                    $"api/data/v9.0/EntityDefinitions(LogicalName='{logicalName}')/Attributes",
                    HttpCompletionOption.ResponseHeadersRead)
                .Result;

            if (response.IsSuccessStatusCode)
            {
                JObject body = JObject.Parse(response.Content.ReadAsStringAsync().Result);
                return body.GetValue("value") as JArray;
            }
            //else
            //{
            //    MessageBox.Show($"The request failed with a status of '{response.ReasonPhrase}'");
            //    return null;
            //}
            return null;
        }

        private JArray GetAll(HttpMessageHandler messageHandler, string entityName)
        {
            using (HttpClient httpClient = new HttpClient(messageHandler))
            {
                //Specify the Web API address of the service and the period of time each request 
                // has to execute.
                httpClient.BaseAddress = new Uri(ServiceUrl);
                httpClient.Timeout = new TimeSpan(0, 2, 0);  //2 minutes

                //Send the WhoAmI request to the Web API using a GET request. 
                var response = httpClient.GetAsync(
                        $"api/data/v9.0/{entityName}",
                        HttpCompletionOption.ResponseHeadersRead)
                    .Result;

                if (response.IsSuccessStatusCode)
                {
                    //Get the response content and parse it.
                    JObject body = JObject.Parse(response.Content.ReadAsStringAsync().Result);
                    return body.GetValue("value") as JArray;
                }
                //else
                //{
                //    MessageBox.Show($"The request failed with a status of '{response.ReasonPhrase}'");
                //    return null;
                //}
                return null;
            }
        }

        //private bool ValidateCredentials(string username, string password, string domain)
        //{
        //    NetworkCredential credentials
        //        = new NetworkCredential(username, password, domain);

        //    LdapDirectoryIdentifier id = new LdapDirectoryIdentifier(domain);

        //    using (LdapConnection connection = new LdapConnection(id, credentials, AuthType.Kerberos))
        //    {
        //        connection.SessionOptions.Sealing = true;
        //        connection.SessionOptions.Signing = true;

        //        try
        //        {
        //            connection.Bind();
        //        }
        //        catch (LdapException lEx)
        //        {
        //            if (ERROR_LOGON_FAILURE == lEx.ErrorCode)
        //            {
        //                return false;
        //            }
        //            throw;
        //        }
        //    }
        //    return true;
        //}

        public void IsAuthenticated(string username, string password)
        {
            try
            {
                string myADSPath = "LDAP://172.17.0.2:389/OU=TTCLAND,DC=sacomreal,DC=net";
                DirectoryEntry ldapConnection = new DirectoryEntry(myADSPath);
                ldapConnection.Username = username;
                ldapConnection.Password = password;

                DirectorySearcher search = new DirectorySearcher(ldapConnection);
                search.Filter = "(sAMAccountName=" + username + ")";
                var result = search.FindOne();
                MessageBox.Show(result.Path);
            }
            catch (LdapException lexc)
            {
                String error = lexc.ServerErrorMessage;
                MessageBox.Show(lexc.Message);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }
    }

    public class OAuthMessageHandler : DelegatingHandler
    {
        private static readonly string ClientID = "2dc5c0cf-8382-4cf9-9bba-986658eee339";
        private static readonly string TenantID = "245d5b97-4f12-41c1-ac47-9bac8614f813";
        private static readonly string SecretKey = "CRqrZrQ493HKgd@VEE4zhgpr]HM@l.O-";

        private static string ServiceUrl = "https://ttcland.crm5.dynamics.com/";
        private static readonly string RedirectUrl = "msal2dc5c0cf-8382-4cf9-9bba-986658eee339://auth";
        private static readonly string Tenant = "organizations";

        private static readonly string Username = "dev1@ttcland.vn";  //CRM user account
        private static readonly string Password = "Ttc@123Land";
        private static readonly SecureString SecurePassword;

        private static IPublicClientApplication App;
        private AuthenticationHeaderValue AuthHeader;

        private static string AccessToken;

        static OAuthMessageHandler()
        {
            App = PublicClientApplicationBuilder.Create(ClientID)
                .WithAuthority(AzureCloudInstance.AzurePublic, Tenant)
                .WithDefaultRedirectUri()
                .Build();

            SecurePassword = new SecureString();
            foreach (char c in Password)
            {
                SecurePassword.AppendChar(c);
            }
        }

        public static async void AcquireToken()
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new
                    Uri("https://login.microsoftonline.com/common/oauth2/token");

                HttpRequestMessage request = new HttpRequestMessage();
                request.Method = HttpMethod.Post;

                var keysValues = new List<KeyValuePair<string, string>>();
                keysValues.Add(new KeyValuePair<string, string>("client_id", ClientID));
                keysValues.Add(new KeyValuePair<string, string>("resource", ServiceUrl));
                keysValues.Add(new KeyValuePair<string, string>("username", Username));
                keysValues.Add(new KeyValuePair<string, string>("password", Password));
                keysValues.Add(new KeyValuePair<string, string>("grant_type", "password"));

                request.Content = new FormUrlEncodedContent(keysValues);

                HttpResponseMessage response = client.SendAsync(request).Result;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string result = response.Content.ReadAsStringAsync().Result;
                    JObject json = JObject.Parse(result);

                    AccessToken = json["access_token"].ToString();
                }
            }


            //string[] scopes = { "User.Read" };
            //AuthenticationResult authResult = null;
            //try
            //{
            //    authResult = await App.AcquireTokenInteractive(scopes)
            //        .WithPrompt(Prompt.SelectAccount)
            //        .ExecuteAsync();
            //}
            //catch (Exception exception)
            //{
            //    MessageBox.Show(exception.ToString());
            //}

            //if (authResult != null)
            //{
            //    AccessToken = authResult.AccessToken;
            //}
        }

        private void AddToken()
        {
            if (string.IsNullOrWhiteSpace(AccessToken))
            {
                return;
            }

            AuthHeader = new AuthenticationHeaderValue("Bearer", AccessToken);
        }

        public OAuthMessageHandler() : base()
        {
            AddToken();
        }

        public OAuthMessageHandler(HttpMessageHandler innerHandler)
            : base(innerHandler)
        {
            AddToken();
        }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Authorization = AuthHeader;
            return base.SendAsync(request, cancellationToken);
        }
    }
}