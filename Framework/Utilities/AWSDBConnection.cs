using Amazon;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Linq;
using System;
using System.Data;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Utilities
{
    public class AWSDBConnection
    {
       // private const string DEFAULT_REGION = "eu-west-1";
        private static string _connectionString = "";
        
        //public static string GetConnectionString()
        //{
        //    return _connectionString;
        //    BaseClass.Reporter.Report("Returned Connection String" + _connectionString, "pass", false);
        //}
        public string GetConnectionString(string ConnectionString,string AWSKey,string DEFAULT_REGION)
        {
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == Environments.Development)
            {
                _connectionString = ConnectionString;
                return ConnectionString;
               
            }

            var secret = GetSecret(AWSKey, DEFAULT_REGION);
            dynamic jObject = JObject.Parse(secret);
            var sqlConnectionStringBuilder = new SqlConnectionStringBuilder(ConnectionString)
            {
                DataSource = jObject.host,
                UserID = jObject.username,
                Password = jObject.password,
                MultipleActiveResultSets = true,
                TrustServerCertificate = true
            };
            _connectionString = sqlConnectionStringBuilder.ConnectionString;
            return _connectionString;
            BaseClass.Reporter.Report("Returned Connection String" + _connectionString, "pass", false);
           
        }
        private static string GetSecret(string secretName, string region)
        {
            IAmazonSecretsManager client = new AmazonSecretsManagerClient(RegionEndpoint.GetBySystemName(region));
            GetSecretValueRequest request = new()
            {
                SecretId = secretName,
                VersionStage = "AWSCURRENT" // VersionStage defaults to AWSCURRENT if unspecified.,
            };
            string secret = string.Empty;
            try
            {
                GetSecretValueResponse response = client.GetSecretValueAsync(request).Result;
                if (response.SecretString != null)
                {
                    secret = response.SecretString;
                }
                else
                {
                    MemoryStream memoryStream = response.SecretBinary;
                    StreamReader reader = new(memoryStream);
                    secret = Encoding.UTF8.GetString(Convert.FromBase64String(reader.ReadToEnd()));
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return secret;
        }

        public async Task<DataTable> ExecuteQuery(string Query, string ConnectionString, string AWSKey, string Region)
        {
            try
            {
               string AWSConnectionString = GetConnectionString(ConnectionString, AWSKey, Region);
                SqlConnection Connection = new(AWSConnectionString);
                Connection.Open();
                if (Connection.State != ConnectionState.Open)
                {
                    BaseClass.Reporter.Report("DB Connection is not Opened", "fail", false);
                    Console.WriteLine("DB Connection is not Opened");
                }
                else
                {
                    BaseClass.Reporter.Report("DB Connection is Opened", "pass", false);
                    Console.WriteLine("DB Connection is not Opened");
                }
                DataSet ds = new DataSet();
                SqlDataAdapter adp = new SqlDataAdapter(Query, Connection);  // Execute query on database 
                BaseClass.Reporter.Report("Query executed: " + Query, "pass", false);
                Console.WriteLine("Query executed: " + Query);
                adp.Fill(ds);  // Store query result into DataSet object   
                Connection.Close();  // Close connection 

                if (Connection.State == ConnectionState.Closed)
                {
                    BaseClass.Reporter.Report("DB Connection is Closed", "pass", false);
                    
                }
                Connection.Dispose();
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                BaseClass.Reporter.Report("Error in getting result of query: " + ex.Message, "fail", false);
                BaseClass.ExitTest(ex.Message);
               
                return new DataTable();
            }
        }
    }
}
