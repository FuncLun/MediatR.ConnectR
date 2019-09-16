//// Decompiled with JetBrains decompiler
//// Type: Microsoft.AspNetCore.Blazor.HttpClientJsonExtensions
//// Assembly: Microsoft.AspNetCore.Blazor, Version=0.7.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60
//// MVID: 7CCDD79B-D655-4313-8799-1356CC8BCE62
//// Assembly location: C:\Users\Chrisw\.nuget\packages\microsoft.aspnetcore.blazor\0.7.0\lib\netstandard2.0\Microsoft.AspNetCore.Blazor.dll

//using Microsoft.JSInterop;
//using System.Net.Http;
//using System.Text;
//using System.Threading.Tasks;
//using Newtonsoft;

//namespace Microsoft.AspNetCore.Blazor
//{
//    /// <summary>Extension methods for working with JSON APIs.</summary>
//    public static class HttpClientJsonExtensions
//    {
//        /// <summary>
//        /// Sends a GET request to the specified URI, and parses the JSON response body
//        /// to create an object of the generic type.
//        /// </summary>
//        /// <typeparam name="T">A type into which the response body can be JSON-deserialized.</typeparam>
//        /// <param name="httpClient">The <see cref="T:System.Net.Http.HttpClient" />.</param>
//        /// <param name="requestUri">The URI that the request will be sent to.</param>
//        /// <returns>The response parsed as an object of the generic type.</returns>
//        public static async Task<T> GetJsonAsync<T>(this HttpClient httpClient, string requestUri)
//        {
//            return Json.Deserialize<T>(await httpClient.GetStringAsync(requestUri));
//        }

//        /// <summary>
//        /// Sends a POST request to the specified URI, including the specified <paramref name="content" />
//        /// in JSON-encoded format, and parses the JSON response body to create an object of the generic type.
//        /// </summary>
//        /// <param name="httpClient">The <see cref="T:System.Net.Http.HttpClient" />.</param>
//        /// <param name="requestUri">The URI that the request will be sent to.</param>
//        /// <param name="content">Content for the request body. This will be JSON-encoded and sent as a string.</param>
//        /// <returns>The response parsed as an object of the generic type.</returns>
//        public static Task PostJsonAsync(
//          this HttpClient httpClient,
//          string requestUri,
//          object content)
//        {
//            return httpClient.SendJsonAsync(HttpMethod.Post, requestUri, content);
//        }

//        /// <summary>
//        /// Sends a POST request to the specified URI, including the specified <paramref name="content" />
//        /// in JSON-encoded format, and parses the JSON response body to create an object of the generic type.
//        /// </summary>
//        /// <typeparam name="T">A type into which the response body can be JSON-deserialized.</typeparam>
//        /// <param name="httpClient">The <see cref="T:System.Net.Http.HttpClient" />.</param>
//        /// <param name="requestUri">The URI that the request will be sent to.</param>
//        /// <param name="content">Content for the request body. This will be JSON-encoded and sent as a string.</param>
//        /// <returns>The response parsed as an object of the generic type.</returns>
//        public static Task<T> PostJsonAsync<T>(
//          this HttpClient httpClient,
//          string requestUri,
//          object content)
//        {
//            return httpClient.SendJsonAsync<T>(HttpMethod.Post, requestUri, content);
//        }

//        /// <summary>
//        /// Sends a PUT request to the specified URI, including the specified <paramref name="content" />
//        /// in JSON-encoded format.
//        /// </summary>
//        /// <param name="httpClient">The <see cref="T:System.Net.Http.HttpClient" />.</param>
//        /// <param name="requestUri">The URI that the request will be sent to.</param>
//        /// <param name="content">Content for the request body. This will be JSON-encoded and sent as a string.</param>
//        public static Task PutJsonAsync(
//          this HttpClient httpClient,
//          string requestUri,
//          object content)
//        {
//            return httpClient.SendJsonAsync(HttpMethod.Put, requestUri, content);
//        }

//        /// <summary>
//        /// Sends a PUT request to the specified URI, including the specified <paramref name="content" />
//        /// in JSON-encoded format, and parses the JSON response body to create an object of the generic type.
//        /// </summary>
//        /// <typeparam name="T">A type into which the response body can be JSON-deserialized.</typeparam>
//        /// <param name="httpClient">The <see cref="T:System.Net.Http.HttpClient" />.</param>
//        /// <param name="requestUri">The URI that the request will be sent to.</param>
//        /// <param name="content">Content for the request body. This will be JSON-encoded and sent as a string.</param>
//        /// <returns>The response parsed as an object of the generic type.</returns>
//        public static Task<T> PutJsonAsync<T>(
//          this HttpClient httpClient,
//          string requestUri,
//          object content)
//        {
//            return httpClient.SendJsonAsync<T>(HttpMethod.Put, requestUri, content);
//        }

//        /// <summary>
//        /// Sends an HTTP request to the specified URI, including the specified <paramref name="content" />
//        /// in JSON-encoded format.
//        /// </summary>
//        /// <param name="httpClient">The <see cref="T:System.Net.Http.HttpClient" />.</param>
//        /// <param name="method">The HTTP method.</param>
//        /// <param name="requestUri">The URI that the request will be sent to.</param>
//        /// <param name="content">Content for the request body. This will be JSON-encoded and sent as a string.</param>
//        public static Task SendJsonAsync(
//          this HttpClient httpClient,
//          HttpMethod method,
//          string requestUri,
//          object content)
//        {
//            return (Task)httpClient.SendJsonAsync<HttpClientJsonExtensions.IgnoreResponse>(method, requestUri, content);
//        }

//        /// <summary>
//        /// Sends an HTTP request to the specified URI, including the specified <paramref name="content" />
//        /// in JSON-encoded format, and parses the JSON response body to create an object of the generic type.
//        /// </summary>
//        /// <typeparam name="T">A type into which the response body can be JSON-deserialized.</typeparam>
//        /// <param name="httpClient">The <see cref="T:System.Net.Http.HttpClient" />.</param>
//        /// <param name="method">The HTTP method.</param>
//        /// <param name="requestUri">The URI that the request will be sent to.</param>
//        /// <param name="content">Content for the request body. This will be JSON-encoded and sent as a string.</param>
//        /// <returns>The response parsed as an object of the generic type.</returns>
//        public static async Task<T> SendJsonAsync<T>(
//          this HttpClient httpClient,
//          HttpMethod method,
//          string requestUri,
//          object content)
//        {
//            string content1 = Json.Serialize(content);
//            HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(new HttpRequestMessage(method, requestUri)
//            {
//                Content = (HttpContent)new StringContent(content1, Encoding.UTF8, "application/json")
//            });
//            if (typeof(T) == typeof(HttpClientJsonExtensions.IgnoreResponse))
//                return default(T);
//            return Json.Deserialize<T>(await httpResponseMessage.Content.ReadAsStringAsync());
//        }

//        private class IgnoreResponse
//        {
//        }
//    }
//}