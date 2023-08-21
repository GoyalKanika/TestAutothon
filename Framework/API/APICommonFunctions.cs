using Framework.UI;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using File = System.IO.File;

namespace Framework.API
{
    public class APICommonFunctions
    {

        public String Response { get; set; }
        public HttpClient httpClient { get; set; }
        public HttpResponseMessage httpResponse { get; set; }
        public HttpContent httpContent { get; set; }
        public static string BaseUrl;
        public static string ContentType { get; set; }
        public Dictionary<string, string> RequestHeader = new Dictionary<string, string>();
        //public APICommonFunctions()
        //{
        //    this = this;
        //}


        public void GetContentType()
        {
            bool flag = false;
            foreach (KeyValuePair<string, string> eachHeader in RequestHeader)
            {
                if (eachHeader.Key.Equals("ContentType"))
                {
                    ContentType = eachHeader.Value;
                    flag = true;
                    break;
                }
            }
            if (!flag)
            {
                BaseClass.Reporter.Report("Setting default ContentType as application/json","info",false);
                ContentType = "application/json";
            }
        }
        public void InithttpClient()
        {
          
                this.httpClient = new HttpClient();
           
        }
        public void SetHeaders()
        {
            InithttpClient();
            this.httpClient.DefaultRequestHeaders.Clear();

            foreach (KeyValuePair<string, string> header in RequestHeader)
            {
                try
                {
                    this.httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
                catch (Exception ex)
                {
                    BaseClass.Reporter.Report("Error in adding the header: " + ex.Message,"fail", false);
                }
            }
        }

        public string GetPayload(string FilePath)
        {
            string Payload = Json.Read(FilePath).ToString();
            return Payload;
        }


        public void GET(string EndPoint, bool DownloadFile = false, string DownloadFileAs = null)
        {
            BaseClass.Reporter.Report("Initialising Headers for EndPoint :" + EndPoint,"info",false);
            SetHeaders();
            GetContentType();

            BaseClass.Reporter.Report("Requesting EndPoint :" + EndPoint, "info", false);
            //if (DownloadFile == true)
            //{
            //    try
            //    {

            //        byte[] fileBytes = this.httpClient.GetByteArrayAsync(EndPoint).Result;

            //        if (DownloadFileAs == null)
            //        {
            //            try
            //            {
            //                DownloadFileAs = this.httpResponse.Content.Headers.ContentDisposition.FileName;
            //            }

            //            catch (NullReferenceException e)
            //            {
            //                BaseClass.Reporter.Report("Please Specify the DownloadFileAs", "info", false);
            //                Environment.Exit(1);
            //            }
            //        }

            //        EnsureDirectory(APIBaseClass.DownloadPath);
            //        File.WriteAllBytes(Path.Combine(this.data.TestStep.EndPoint, DownloadFileAs), fileBytes);
            //        BaseClass.Reporter.Report("File downloaded at location : " + Path.Combine(APIBaseClass.DownloadPath, DownloadFileAs, "pass"));
            //    }
            //    catch (Exception e)
            //    {
            //        BaseClass.Reporter.Report(e.Message,"fail",false);
            //        BaseClass.Reporter.Report(e.StackTrace,"fail",false);
            //    }
            //}

            //else
            {
                try
                {

                    this.httpResponse = this.httpClient.GetAsync(EndPoint).Result;
                }
                catch (Exception e)
                {
                    BaseClass.Reporter.Report(e.Message,"fail", false);
                    BaseClass.Reporter.Report(e.StackTrace,"fail",false);
                }

            }
            if (Convert.ToInt32(this.httpResponse.StatusCode) == 200)
            {
                this.Response = this.httpResponse.Content.ReadAsStringAsync().Result;
            }

        }

        public void PUT(string EndPoint, String RequestPayload)
        {
            BaseClass.Reporter.Report("Initialising Headers for EndPoint :" + EndPoint,"info",false);
            SetHeaders();
            GetContentType();

            if (ContentType.Equals("multipart/form-data"))
                this.httpContent = GenerateMultipartFormPayload(RequestPayload);
            else if (ContentType.Equals("application/x-www-form-urlencoded"))
                this.httpContent = GenerateFormUrlEncodedPayload(RequestPayload);
            else
                this.httpContent = GenerateAppJsonPayload(RequestPayload);

            BaseClass.Reporter.Report("Requesting EndPoint :" + EndPoint,"Info",false);
            try
            {
                this.httpResponse = this.httpClient.PutAsync(EndPoint, this.httpContent).Result;
            }
            catch (Exception e)
            {
                BaseClass.Reporter.Report(e.Message,"fail",false);
                BaseClass.Reporter.Report(e.StackTrace,"fail",false);
            }
            if (Convert.ToInt32(this.httpResponse.StatusCode) == 200)
            {
                this.Response = this.httpResponse.Content.ReadAsStringAsync().Result;
            }
        }

        public void POST(string EndPoint, string RequestPayload, bool DownloadFile = false, string DownloadFileAs = null)
        {

            BaseClass.Reporter.Report("Initialising Headers for EndPoint :" + EndPoint,"Info",false);
            SetHeaders();
            GetContentType();

            if (ContentType.Equals("multipart/form-data"))
                this.httpContent = GenerateMultipartFormPayload(RequestPayload);
            else if (ContentType.Equals("application/x-www-form-urlencoded"))
                this.httpContent = GenerateFormUrlEncodedPayload(RequestPayload);
            else
                this.httpContent = GenerateAppJsonPayload(RequestPayload);

            BaseClass.Reporter.Report("Requesting EndPoint :" + EndPoint, "Info", false);
            //if (DownloadFile == true)
            //{
            //    try
            //    {

            //        byte[] fileBytes = this.httpClient.GetByteArrayAsync(EndPoint).Result;

            //        if (DownloadFileAs == null)
            //        {
            //            try
            //            {
            //                DownloadFileAs = this.httpResponse.Content.Headers.ContentDisposition.FileName;
            //            }

            //            catch (NullReferenceException e)
            //            {

            //                BaseClass.Reporter.Report("Please Specify the DownloadFileAs","fail",false);
            //                Environment.Exit(1);
            //            }
            //        }

            //        Data.EnsureDirectory(APIBaseClass.DownloadPath);
            //        File.WriteAllBytes(Path.Combine(this.data.TestStep.EndPoint, DownloadFileAs), fileBytes);
            //        BaseClass.Reporter.Report(Report.Status.Pass, "File downloaded at location : " + Path.Combine(APIBaseClass.DownloadPath, DownloadFileAs));
            //    }
            //    catch (Exception e)
            //    {
            //        BaseClass.Reporter.Report(e.Message,"fail",false);
            //        BaseClass.Reporter.Report(e.StackTrace,"fail",false);
            //    }
            //}

            //else
            {
                try
                {
                    this.httpResponse = this.httpClient.PostAsync(EndPoint, this.httpContent).Result;
                }
                catch (Exception e)
                {
                    BaseClass.Reporter.Report(e.Message,"fail",false);
                    BaseClass.Reporter.Report(e.StackTrace, "fail", false);
                }

            }

            if (Convert.ToInt32(this.httpResponse.StatusCode) == 200)
            {
                this.Response = this.httpResponse.Content.ReadAsStringAsync().Result;
            }
        }


        public void POST(string EndPoint, string UploadFilePath, string RequestPayload)
        {

            BaseClass.Reporter.Report("Initialising Headers for EndPoint :" + EndPoint,"Info",false);
            SetHeaders();

            GetContentType();

            if (ContentType.Equals("multipart/form-data"))
                this.httpContent = GenerateMultipartFormPayload(RequestPayload,UploadFilePath);
            else if (ContentType.Equals("application/x-www-form-urlencoded"))
                this.httpContent = GenerateFormUrlEncodedPayload(RequestPayload);
            else
                this.httpContent = GenerateAppJsonPayload(RequestPayload);

            BaseClass.Reporter.Report("Requesting EndPoint :" + EndPoint,"Info",false);

            try
            {
                this.httpResponse = this.httpClient.PostAsync(EndPoint, this.httpContent).Result;
            }
            catch (Exception e)
            {
                BaseClass.Reporter.Report(e.Message,"fail",false);
                BaseClass.Reporter.Report(e.StackTrace,"fail",false);
            }

            if (Convert.ToInt32(this.httpResponse.StatusCode) == 200)
            {
                this.Response = this.httpResponse.Content.ReadAsStringAsync().Result;
            }
        }

        public void DELETE(string EndPoint, string RequestPayload)
        {
            BaseClass.Reporter.Report("Initialising Headers for EndPoint :" + EndPoint,"Info",false);
            SetHeaders();
            GetContentType();
            var request = new HttpRequestMessage(HttpMethod.Delete,
                       EndPoint);
            if (ContentType.Equals("multipart/form-data"))
                request.Content = GenerateMultipartFormPayload(RequestPayload);
            else if (ContentType.Equals("application/x-www-form-urlencoded"))
                request.Content = GenerateFormUrlEncodedPayload(RequestPayload);
            else
                request.Content = GenerateAppJsonPayload(RequestPayload);
            BaseClass.Reporter.Report("Requesting EndPoint :" + EndPoint,"Info",false);
            try
            {

                this.Response = this.httpClient.Send(request).Content.ReadAsStringAsync().Result;

            }
            catch (Exception e)
            {
                BaseClass.Reporter.Report(e.Message,"fail",false);
                BaseClass.Reporter.Report(e.StackTrace,"fail",false);
            }
        }
        public void DELETE(string EndPoint)
        {


            BaseClass.Reporter.Report("Initialising Headers for EndPoint :" + EndPoint,"Info",false);
            SetHeaders();

            BaseClass.Reporter.Report("Requesting EndPoint :" + EndPoint,"Info",false);
            try
            {
                this.httpResponse = this.httpClient.DeleteAsync(EndPoint).Result;
            }
            catch (Exception e)
            {
                BaseClass.Reporter.Report(e.Message, "Fail", false);
                BaseClass.Reporter.Report(e.StackTrace, "Fail", false);
            }
            if (Convert.ToInt32(this.httpResponse.StatusCode) == 200)
            {
                this.Response = this.httpResponse.Content.ReadAsStringAsync().Result;
            }


        }

        public void PATCH(string EndPoint, String RequestPayload)
        {
            BaseClass.Reporter.Report("Initialising Headers for EndPoint :" + EndPoint, "info",false);
            SetHeaders();
            GetContentType();
            var request = new HttpRequestMessage(HttpMethod.Patch,
                      EndPoint);
            if (ContentType.Equals("multipart/form-data"))
                request.Content = GenerateMultipartFormPayload(RequestPayload);
            else if (ContentType.Equals("application/x-www-form-urlencoded"))
                request.Content = GenerateFormUrlEncodedPayload(RequestPayload);
            else
                request.Content = GenerateAppJsonPayload(RequestPayload);


            BaseClass.Reporter.Report("Requesting EndPoint :" + EndPoint,"info",false);
            try
            {
                this.httpResponse = this.httpClient.PatchAsync(EndPoint, this.httpContent).Result;
            }
            catch (Exception e)
            {
                BaseClass.Reporter.Report(e.Message,"Fail",false );
                BaseClass.Reporter.Report(e.StackTrace,"Fail",false);
            }
        }

        public static void EnsureFile(string FilePath)
        {

            try
            {
                SearchFile(FilePath);
                BaseClass.Reporter.Report( "Validated downloaded file at location : " + FilePath, "pass",false);
            }
            catch
            {
                BaseClass.Reporter.Report("Failed to validate downloaded file at location : " + FilePath,"fail",false);
            }

        }

        public void Dispose()
        {
            this.httpClient.Dispose();
            this.httpResponse.Dispose();
            if (this.httpContent != null)
            {
                this.httpContent.Dispose();
            }
            this.RequestHeader.Clear();
        }



        public string GetValueFromJsonResponse(string Key)
        {
            JObject obj;
            string value = null;
            try
            {
                obj = Json.Read(this.Response.ToString(), true);
                value = obj.SelectToken(Key).ToString();
            }
            catch (Exception ex)
            {
                BaseClass.Reporter.Report( "Error in reading the key : " + Key + " from the Json Response", "fail",false);
            }
            return value;
        }
       
        public string GetJObjectFromJArray(string array, string key, string value)
        {
            JArray obj = JArray.Parse(array);
            try
            {
                JToken returnVal = obj
                    .Where(jt => (((string)jt[key]).ToLower() == value.ToLower()))        // Find the object(s) containing the asset you want
                    .FirstOrDefault();
                return returnVal.ToString();
            }
            catch
            {
                JToken returnVal = obj
                  .Where(jt => (jt[key].ToString().ToLower().Contains(value.ToLower()) == true))       // Find the object(s) containing the asset you want
                    .FirstOrDefault();
                return returnVal.ToString();
            }

        }

        public string GetValueFromJObject(JObject obj,string Key)
        {
           
            string value = null;
            try
            {
               
                value = obj.SelectToken(Key).ToString();
            }
            catch (Exception ex)
            {
                BaseClass.Reporter.Report("Error in reading the key : " + Key + " from the Json Response","fail" ,false);
            }
            return value;
        }
        public List<string> GetMultipleValuesFromJsonResponse(string Key1, string key2)
        {
            //JObject obj;
            List<string> list = new List<string>();
            try
            {
                JObject obj = JObject.Parse(this.Response.ToString());
                var collection =
    from p in obj[Key1]
    select (string)p[key2];

                foreach (var item in collection)
                {
                    list.Add(item);
                }

            }
            catch (Exception ex)
            {
                BaseClass.Reporter.Report("Error in reading the key : " + Key1 + "and" + key2 + " from the Json Response" + ex.ToString(),"fail",false);
            }
            return list;
        }
        public List<string> GetMultipleValuesFromJsonResponse(string Key1)
        {
            //JObject obj;
            List<string> list = new List<string>();
            try
            {
                JArray array = JArray.Parse(this.Response.ToString());
                var collection =
    from p in array
    select (string)p[Key1];

                foreach (var item in collection)
                {
                    list.Add((string)item);
                }




            }
            catch (Exception ex)
            {
                BaseClass.Reporter.Report( "Error in reading the key : " + Key1 + "from the Json Response" + ex.ToString(),"fail",false);
            }
            return list;
        }


        public List<string> GetMultipleValuesFromJArray(string array, string keyToBeFetched, string conditionalKey, string conditionalValue)
        {

            if (array != null && array != "")
            {
                JArray obj = JArray.Parse(array);
                try
                {
                    List<string> returnVal = obj
                          .Where(jt => ((string)jt[conditionalKey] == conditionalValue)) // Find the object(s) containing the asset you want
                          .Select(jt => (string)jt[keyToBeFetched])  // From those get the  value
                          .ToList();
                    return returnVal;
                }
                catch (Exception ex)
                {
                    List<string> returnVal = obj
                                           .Where(jt => (jt[conditionalKey].ToString().Contains(conditionalValue) == true))       // Find the object(s) containing the asset you want
                                           .Select(jt => (string)jt[keyToBeFetched])    // From those get the  value
                                           .ToList();
                    return returnVal;
                }
            }
            return null;
        }
        public string GetConditionalValueFromJArray(string array,string keyToBeFetched,string conditionalKey,string conditionalValue)
        {

            if (array != null && array !="")
            {
                JArray obj = JArray.Parse(array);
                try
                {
                    string returnVal = obj
                        .Where(jt => ((string)jt[conditionalKey] == conditionalValue))        // Find the object(s) containing the asset you want
                        .Select(jt => (string)jt[keyToBeFetched])    // From those get the  value
                        .FirstOrDefault();
                    return returnVal;
                }
                catch
                {
                    string returnVal = obj
                        .Where(jt => (jt[conditionalKey].ToString().Contains(conditionalValue) == true))       // Find the object(s) containing the asset you want
                        .Select(jt => (string)jt[keyToBeFetched])    // From those get the  value
                        .FirstOrDefault();
                    return returnVal;
                }
            }
            return null;
        }

       

        public string GetValueFromJArrayResponse(string Key, int index)
        {

            JArray obj;
            string value = null;
            try
            {
                obj = JArray.Parse(this.Response.ToString());
                value = obj.SelectToken("$[" + index + "]." + Key).ToString();
            }
            catch
            {
                BaseClass.Reporter.Report( "Error in reading the key : " + Key + " at the index : " + index + " from the Json Response","fail" ,false);
            }
            return value;
        }

        public void ReportResponse()
        {
            BaseClass.Reporter.Report(this.Response, "info", false);
        }

        public StringContent GenerateAppJsonPayload(String Payload)
        {
            return new StringContent(Payload, Encoding.UTF8, ContentType);
        }
        public FormUrlEncodedContent GenerateFormUrlEncodedPayload(String RequestPayload)
        {
            Dictionary<String, String> formData = new Dictionary<String, String>();
            JObject json_RequestPayload = null;
            try
            {
                json_RequestPayload = JObject.Parse(RequestPayload);
            }
            catch(Exception ex)
            {
                BaseClass.Reporter.Report( "Ensure Request Palyoad is a valid JSON","info",false);
                BaseClass.Reporter.Report("Error in parsing the Request Payload"+ ex.Message, "fail", false);

            }
            List<String> keys = json_RequestPayload.Properties().Select(p => p.Name).ToList();
            foreach (String eachKey in keys)
            {
                formData.Add(eachKey, json_RequestPayload.SelectToken("$." + eachKey).ToString());
            }

            return new FormUrlEncodedContent(formData);

        }

        public MultipartFormDataContent GenerateMultipartFormPayload(String RequestPayload = null, string UploadFilePath= null)
        {
            MultipartFormDataContent multipartFormDataContent = new MultipartFormDataContent();

            if (UploadFilePath != null)
            {
                String FilePath =  UploadFilePath;
                Byte[] FileBytes = File.ReadAllBytes(FilePath);
                 multipartFormDataContent.Add(new ByteArrayContent(FileBytes, 0, FileBytes.Length), "files", Path.GetFileName(FilePath));
            }
            if(RequestPayload != null)
            {
                JObject json_RequestPayload = null;
                try
                {
                    json_RequestPayload = JObject.Parse(RequestPayload);
                }
                catch (Exception ex)
                {
                    BaseClass.Reporter.Report("Ensure Request Palyoad is a valid JSON","info",false);
                    BaseClass.Reporter.Report("Error in parsing the Request Payload" + ex.Message,"fail", false);

                }
                List<String> keys = json_RequestPayload.Properties().Select(p => p.Name).ToList();
                foreach (String eachKey in keys)
                {
                    multipartFormDataContent.Add(new StringContent(eachKey), json_RequestPayload.SelectToken("$." + eachKey).ToString());
                }
            }

            return multipartFormDataContent;
        }

        public static Boolean IsFile(String FilePath)
        {
            try
            {
                if (File.Exists(FilePath))
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        public static String SearchFile(String RelativeFilePath, Boolean ToReturnType = false)
        {
            String CurDir = Directory.GetCurrentDirectory();
            Int32 count = 0;
            while (count != 5)
            {
                if (IsFile(Path.Combine(CurDir, RelativeFilePath)))
                {
                    if (!ToReturnType)
                        return Path.Combine(CurDir, RelativeFilePath);
                    else
                        return "file:" + Path.Combine(CurDir, RelativeFilePath);
                }
                else if (IsDirectory(Path.Combine(CurDir, RelativeFilePath)))
                {
                    if (!ToReturnType)
                        return Path.Combine(CurDir, RelativeFilePath);
                    else
                        return "directory:" + Path.Combine(CurDir, RelativeFilePath);
                }
                else
                {
                    CurDir = Directory.GetParent(CurDir).FullName;
                    count++;
                }
                if (count == 5)
                    break;
            }
            if (count == 5)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(RelativeFilePath + " directory not found at project root" +
                    ". Please provide the config at the project root path.");
                Console.ResetColor();
                Environment.Exit(1);
                return null;
            }
            else
                return null;
        }

        public static Boolean IsDirectory(String FilePath)
        {
            try
            {
                if (Directory.Exists(FilePath))
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }
        public static void EnsureDirectory(String path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }
    }
}

