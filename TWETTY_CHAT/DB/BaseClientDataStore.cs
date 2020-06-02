using TWETTY_CHAT.Core;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TWETTY_CHAT
{
    public class BaseClientDataBase : IClientDataBase
    {
        #region Protected Members

        /// <summary>
        /// The database context for the client data store
        /// </summary>
        protected ClientDataBaseDbContext mDbContext;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="dbContext">The database to use</param>
        public BaseClientDataBase(ClientDataBaseDbContext dbContext)
        {
            // Set local member
            mDbContext = dbContext;
        }

        #endregion

        #region Interface Implementation

        /// <summary>
        /// Determines if the current user has logged in credentials
        /// </summary>
        public async Task<bool> HasCredentialsAsync()
        {
            return await GetLoginCredentialsAsync() != null;
        }

        /// <summary>
        /// Makes sure the client data store is correctly set up
        /// </summary>
        /// <returns>Returns a task that will finish once setup is complete</returns>
        public async Task EnsureDataStoreAsync()
        {
            // Make sure the database exists and is created
            await mDbContext.Database.EnsureCreatedAsync();
        }

        /// <summary>
        /// Gets the stored login credentials for this client
        /// </summary>
        /// <returns>Returns the login credentials if they exist, or null if none exist</returns>
        public Task<LoginCredentialsDataModel> GetLoginCredentialsAsync()
        {
            // Get the first column in the login credentials table, or null if none exist
            return Task.FromResult(mDbContext.LoginCredentials.FirstOrDefault());
        }

        /// <summary>
        /// Gets the stored friend for this client
        /// </summary>
        /// <returns>Returns the friend if they exist, or null if none exist</returns>
        public Task<List<FriendApiModel>> GetClientFriendsAsync()
        {
            // Get the first column in the client friend table, or null if none exist
            return Task.FromResult(mDbContext.ClientFriends.ToList());
        }


        /// <summary>
        /// Stores the given login credentials to the backing data store
        /// </summary>
        /// <param name="loginCredentials">The login credentials to save</param>
        /// <returns>Returns a task that will finish once the save is complete</returns>
        public async Task SaveLoginCredentialsAsync(LoginCredentialsDataModel loginCredentials)
        {
            // Clear all entries
            mDbContext.LoginCredentials.RemoveRange(mDbContext.LoginCredentials);

            // Add new one
            mDbContext.LoginCredentials.Add(loginCredentials);

            // Save changes
            await mDbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Stores the given friend to the backing data store
        /// </summary>
        /// <param name="friendCredentials">The friend to save</param>
        /// <returns>Returns a task that will finish once the save is complete</returns>
        public async Task SaveClientFriendsAsync(FriendsResultsApiModel friends)
        {
            // Clear all entries
            mDbContext.ClientFriends.RemoveRange(mDbContext.ClientFriends);

            // Add new one
            for (int index = 0; index < friends.Count; index++)
            {
                mDbContext.ClientFriends.Add(friends[index]);
            }

            // Save changes
            await mDbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Removes all login credentials stored in the data store
        /// </summary>
        /// <returns></returns>
        public async Task ClearAllLoginCredentialsAsync()
        {
            // Clear all entries
            mDbContext.LoginCredentials.RemoveRange(mDbContext.LoginCredentials);

            // Save changes
            await mDbContext.SaveChangesAsync();
        }

        #endregion
    }
}
