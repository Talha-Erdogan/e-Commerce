using e_Commerce.Web.Business.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace e_Commerce.Web.Business.Helpers
{

    //httpClient.PostAsJsonAsync(string.Format("v1/Company/GetAll"), portalApiRequestModel).Result; -> kullanamadığımız ve hata aldıgımız için böyle bir sınıf oluşturduk.
    //konu ile ilgili araştırma;
    //https://stackoverflow.com/questions/40027299/where-is-postasjsonasync-method-in-asp-net-core
    public static class HttpClientExtensions
    {
        public static Task<HttpResponseMessage> PostAsJsonAsync<T>(this HttpClient httpClient, string url, T data)
        {
            var dataAsString = JsonConvert.SerializeObject(data);
            var content = new StringContent(dataAsString);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return httpClient.PostAsync(url, content);
        }

        public static Task<HttpResponseMessage> PostAsJsonAsync(this HttpClient httpClient, string url)
        {
            //var dataAsString = JsonConvert.SerializeObject(data);
            var content = new StringContent("");
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return httpClient.PostAsync(url, content);
        }

        public static Task<HttpResponseMessage> PostAsImageFiles(this HttpClient httpClient, string url, ImageInformation imageInformation)
        {

            //resim gönderim işlemleri
            MultipartFormDataContent multiContent = new MultipartFormDataContent();
            List<ByteArrayContent> arrayList = new List<ByteArrayContent>();

            ByteArrayContent bytes = new ByteArrayContent(imageInformation.ImageByteData);
            multiContent.Add(bytes, "imageFile", imageInformation.FileName);
            return httpClient.PostAsync(url, multiContent);
        }

        public static Task<HttpResponseMessage> PutAsJsonAsync<T>(this HttpClient httpClient, string url, T data)
        {
            var dataAsString = JsonConvert.SerializeObject(data);
            var content = new StringContent(dataAsString);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return httpClient.PutAsync(url, content);
        }


        public static async Task<T> ReadAsJsonAsync<T>(this HttpContent content)
        {
            var dataAsString = await content.ReadAsStringAsync().ConfigureAwait(false);

            return JsonConvert.DeserializeObject<T>(dataAsString);
        }
    }
}
