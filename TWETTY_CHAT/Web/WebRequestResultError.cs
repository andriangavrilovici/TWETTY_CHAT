using TWETTY_CHAT.Core;
using System.Threading.Tasks;

namespace TWETTY_CHAT
{
    public static class WebRequestResultError
    {
        /// <summary>
        /// Checks the web request result for any errors, displaying them if there are any,
        /// or if we are unauthorized automatically logging us out
        /// </summary>
        /// <typeparam name="T">The type of Api Response</typeparam>
        /// <param name="response">The response to check</param>
        /// <param name="title">The title of the error dialog if there is an error</param>
        /// <returns>Returns true if there was an error, or false if all was OK</returns>
        public static async Task<bool> ErrorIfFailedAsync(this WebRequestResult response, string title)
        {
            // If there was no response, bad data, or a response with a error message...
            if (response == null || response.ServerResponse == null || (response.ServerResponse as ApiResponse).Successful == false)
            {
                // Default error message
                // TODO: Localize strings
                var message = "Unknown error from server call";

                // If we got a response from the server...
                if (response.ServerResponse is ApiResponse apiResponse)
                    // Set message to servers response
                    message = apiResponse.ErrorMessage;
                // If we have a result but deserialize failed...
                else if (!string.IsNullOrWhiteSpace(response.RawServerResponse))
                    // Set error message
                    message = $"Unexpected response from server. {response.RawServerResponse}";
                // If we have a result but no server response details at all...
                else if (response != null)
                    // Set message to standard HTTP server response details
                    message = response.ErrorMessage ?? $"Server responded with {response.StatusDescription} ({response.StatusCode})";

                // If this is an unauthorized response...
                if (response?.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    // Automatically log the user out
                    // await ViewModelSettings.LogoutAsync();
                    await Task.Delay(1);
                }
                else
                {
                    // Display error
                    if (DialogWindow.RunPreloader)
                        DialogWindow.ClosePreloader();
                    DialogWindow.Show(message, title);

                }

                // Return that we had an error
                return true;
            }

            // All was OK, so return false for no error
            return false;
        }
    }
}
