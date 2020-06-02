using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace TWETTY_CHAT
{
    public static class WebRequests
    {
        private static string SerializerJson { get; } = "application/json";
        /// <summary>
        /// GETs a web request to an URL and returns the raw http web response
        /// </summary>
        /// <remarks>IMPORTANT: Remember to close the returned <see cref="HttpWebResponse"/> stream once done</remarks>
        /// <param name="url">The URL</param>
        /// <param name="configureRequest">Allows caller to customize and configure the request prior to the request being sent</param>
        /// <param name="bearerToken">If specified, provides the Authorization header with `bearer token-here` for things like JWT bearer tokens</param>
        /// <returns></returns>
        public static async Task<HttpWebResponse> GetAsync(string url, Action<HttpWebRequest> configureRequest = null, string bearerToken = null)
        {
            #region Setup

            // Create the web request
            var request = WebRequest.CreateHttp(url);

            // Make it a GET request method
            request.Method = HttpMethod.Get.ToString();

            // If we have a bearer token...
            if (bearerToken != null)
                // Add bearer token to header
                request.Headers.Add(HttpRequestHeader.Authorization, $"Bearer {bearerToken}");

            // Any custom work
            configureRequest?.Invoke(request);

            #endregion

            // Wrap call...
            try
            {
                // Return the raw server response
                return await request.GetResponseAsync() as HttpWebResponse;
            }
            // Catch Web Exceptions (which throw for things like 401)
            catch (WebException ex)
            {
                // If we got a response...
                if (ex.Response is HttpWebResponse httpResponse)
                    // Return the response
                    return httpResponse;

                // Otherwise, we don't have any information to be able to return
                // So re-throw
                throw;
            }
        }

        public static async Task<HttpWebResponse> PostAsync(string url, object content = null, string bearerToken = null)
        {
            #region Setup
            // Create the web request
            var request = WebRequest.CreateHttp(url);

            // Make it a POST request method
            request.Method = HttpMethod.Post.ToString();

            // Set the content type
            request.ContentType = SerializerJson;

            // If we have a bearer token...
            if (bearerToken != null)
                // Add bearer token to header
                request.Headers.Add(HttpRequestHeader.Authorization, $"Bearer {bearerToken}");

            #endregion

            #region Write Content

            // Set the content length
            if (content == null)
            {
                // Set content length to 0
                request.ContentLength = 0;
            }
            // Otherwise...
            else
            {
                // Serialize content to Json string
                var contentString = JsonConvert.SerializeObject(content);
                // Get body stream...
                using (var requestStream = await request.GetRequestStreamAsync())
                // Create a stream writer from the body stream...
                using (var streamWriter = new StreamWriter(requestStream))
                    // Write content to HTTP body stream
                    await streamWriter.WriteAsync(contentString);
            }

            #endregion
            // Wrap call...
            try
            {
                // Return the raw server response
                return await request.GetResponseAsync() as HttpWebResponse;
            }
            // Catch Web Exceptions (which throw for things like 401)
            catch (WebException ex)
            {
                // If we got a response...
                if (ex.Response is HttpWebResponse httpResponse)
                    // Return the response
                    return httpResponse;

                // Otherwise, we don't have any information to be able to return
                // So re-throw
                throw;
            }
        }

        /// <summary>
        /// Posts a web request to an URL and returns a response of the expected data type
        /// </summary>
        /// <param name="url">The URL to post to</param>
        /// <param name="content">The content to post</param>
        /// <param name="sendType">The format to serialize the content into</param>
        /// <param name="returnType">The expected type of content to be returned from the server</param>
        /// <returns></returns>
        public static async Task<WebRequestResult<TResponse>> PostAsync<TResponse>(string url, object content = null, string bearerToken = null)
        {
            var serverResponse = default(HttpWebResponse);
            try
            {
                // Make the standard Post call first
                serverResponse = await PostAsync(url, content, bearerToken);
            }
            catch (Exception ex)
            {
                // If we got unexpected error, return that
                return new WebRequestResult<TResponse>
                {
                    // Include exception message
                    ErrorMessage = ex.Message
                };
            }

            // Create a result
            var result = serverResponse.CreateWebRequestResult<TResponse>();

            // If the response status code is not 200...
            if (result.StatusCode != HttpStatusCode.OK)
            {
                // Call failed
                // TODO: Localize string
                result.ErrorMessage = $"Server returned unsuccessful status code. {serverResponse.StatusCode} {serverResponse.StatusDescription}";

                // Done
                return result;
            }
            // If we have no content to deserialize...
            if (string.IsNullOrWhiteSpace(result.RawServerResponse))
                // Done
                return result;

            // Deserialize raw response
            try
            {
                // Deserialize Json string
                result.ServerResponse = JsonConvert.DeserializeObject<TResponse>(result.RawServerResponse);
            }
            catch (Exception)
            {
                // If deserialize failed then set error message
                result.ErrorMessage = "Failed to deserialize server response to the expected type";

                // Done
                return result;
            }

            // Return result
            return result;
        }
    }
}
