using TWETTY_CHAT.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TWETTY_CHAT
{
    public interface IClientDataBase
    {
        /// <summary>
        /// Determines if the current user has logged in credentials
        /// </summary>
        Task<bool> HasCredentialsAsync();

        /// <summary>
        /// Makes sure the client data store is correctly set up
        /// </summary>
        /// <returns>Returns a task that will finish once setup is complete</returns>
        Task EnsureDataStoreAsync();

        /// <summary>
        /// Gets the stored login credentials for this client
        /// </summary>
        /// <returns>Returns the login credentials if they exist, or null if none exist</returns>
        Task<LoginCredentialsDataModel> GetLoginCredentialsAsync();

        /// <summary>
        /// Gets the stored friends for this client
        /// </summary>
        /// <returns>Returns the client friends if they exist, or null if none exist</returns>
        Task<List<FriendApiModel>> GetClientFriendsAsync();

        /// <summary>
        /// Stores the given login credentials to the backing data store
        /// </summary>
        /// <param name="loginCredentials">The login credentials to save</param>
        /// <returns>Returns a task that will finish once the save is complete</returns>
        Task SaveLoginCredentialsAsync(LoginCredentialsDataModel loginCredentials);

        /// <summary>
        /// Stores the given client friends to the backing data store
        /// </summary>
        /// <param name="loginCredentials">The friend to save</param>
        /// <returns>Returns a task that will finish once the save is complete</returns>
        Task SaveClientFriendsAsync(FriendsResultsApiModel friendCredentials);

        /// <summary>
        /// Removes all login credentials stored in the data store
        /// </summary>
        /// <returns></returns>
        Task ClearAllLoginCredentialsAsync();

    }
}
