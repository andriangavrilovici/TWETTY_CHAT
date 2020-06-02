using TWETTY_CHAT.Core;
using System.Security;
using System.Threading.Tasks;
using System.Windows.Input;
using static TWETTY_CHAT.DI;

namespace TWETTY_CHAT
{
    public class SettingsViewModel : BaseViewModel
    {

        #region Public Properties

        /// <summary>
        /// Indicates if the user is currently logging out
        /// </summary>
        public bool LoggingOut { get; set; }

        #endregion

        #region Changes User Password Properties

        public SecureString CurrentPassword { get; set; }
        public SecureString NewPassword { get; set; }

        #endregion

        #region Public Commands

        /// <summary>
        /// The command to open the settings menu
        /// </summary>
        public ICommand OpenCommand { get; set; }

        /// <summary>
        /// The command to close the settings menu
        /// </summary>
        public ICommand CloseCommand { get; set; }

        /// <summary>
        /// The command to open the information message box
        /// </summary>
        public ICommand OpenInfoCommand { get; set; }

        /// <summary>
        /// Loads the settings data from the client data store
        /// </summary>
        public ICommand LoadCommand { get; set; }

        /// <summary>
        /// The command to logout of the application
        /// </summary>
        public ICommand LogoutCommand { get; set; }

        /// <summary>
        /// Saved change settings
        /// </summary>
        public ICommand SaveCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public SettingsViewModel()
        {
            // Create command
            OpenCommand = new RelayCommand(Open);
            CloseCommand = new RelayCommand(Close);
            OpenInfoCommand = new RelayCommand(OpenInfo);
            LoadCommand = new RelayCommand(async () => await LoadAsync());
            LogoutCommand = new RelayCommand(async () => await LogoutAsync());
            SaveCommand = new RelayCommand(async () => await SaveAsync());
        }

        #endregion

        #region Open / Close (Settings / Information)

        /// <summary>
        /// Open the settings menu
        /// </summary>
        private void Open()
        {
            // Go to settings page
            ViewModelApplication.GoToPage(ApplicationPage.Settings);
        }

        /// <summary>
        /// Open the information page
        /// </summary>
        private void OpenInfo()
        {
            // Go to information page
            ViewModelApplication.GoToPage(ApplicationPage.Information);
        }

        /// <summary>
        /// Closed the settings page and information page
        /// </summary>
        private async void Close()
        {
            // Set the default user data
            if (ViewModelApplication.CurrentPage != ApplicationPage.Information)
                await UpdateValuesFromLocalStoreAsync(ClientDataBase);

            // Go to chat page
            ViewModelApplication.GoToPage(ApplicationPage.Chat);
        }

        #endregion

        #region Save Settings

        /// <summary>
        /// Save changes to settings
        /// </summary>
        private async Task SaveAsync()
        {
            string ErrorMessage = "Actualizare esuata";

            var user = await ClientDataBase.GetLoginCredentialsAsync();

            if (!EmailValidation.IsValidEmail(UserProfileDetails.Instance.Email))
            {
                DialogWindow.Show("Acest email nu este valid.", ErrorMessage);
                return;
            }

            bool isChangesInProfile = !(UserProfileDetails.Instance.FirstName.Equals(user.FirstName) &&
                UserProfileDetails.Instance.LastName.Equals(user.LastName) &&
                UserProfileDetails.Instance.Email.Equals(user.Email));

            bool isChangesInPassword = !string.IsNullOrWhiteSpace(CurrentPassword.Unsecure()) && !string.IsNullOrWhiteSpace(NewPassword.Unsecure());

            if (string.IsNullOrWhiteSpace(CurrentPassword.Unsecure()) && !string.IsNullOrWhiteSpace(NewPassword.Unsecure()))
            {
                DialogWindow.Show("Introduceti parola curenta", ErrorMessage);
                return;
            }

            if (!isChangesInProfile && !isChangesInPassword)
                return;

            #region Change User Password

            if (isChangesInPassword)
            {
                // Update the server with the details
                var updatePassword = await WebRequests.PostAsync<ApiResponse>(
                    // Set URL
                    appsettings.HostUrl + $"/{ApiRoutes.UpdateUserPassword}",
                    // Pass the Api model
                    new UpdateUserPasswordApiModel
                    {
                        CurrentPassword = CurrentPassword.Unsecure(),
                        NewPassword = NewPassword.Unsecure()
                    },
                    // Pass in user Token
                    bearerToken: user.Token);

                // If the response has an error...
                if (await updatePassword.ErrorIfFailedAsync("Modificarea parolei nu a reușit"))
                    return;

                DialogWindow.Show("Parola a fost modificata cu succes", "Modificarea parolei");

                CurrentPassword = new SecureString();
                NewPassword = new SecureString();
            }

            #endregion

            #region Changes In User Profile

            // Make sure there are changes in your profile
            if (isChangesInProfile)
            {

                if (string.IsNullOrWhiteSpace(UserProfileDetails.Instance.FirstName) ||
                    string.IsNullOrWhiteSpace(UserProfileDetails.Instance.LastName) ||
                    string.IsNullOrWhiteSpace(UserProfileDetails.Instance.Email))
                {
                    DialogWindow.Show("Nu ati introdus!\nNume, Prenume sau Email.", ErrorMessage);
                    return;
                }

                var apiUpdateUser = new UpdateUserProfileApiModel();

                apiUpdateUser.CurrentEmail = user.Email;

                if (!UserProfileDetails.Instance.Email.Equals(user.Email))
                    apiUpdateUser.NewEmail = UserProfileDetails.Instance.Email;

                if (!UserProfileDetails.Instance.FirstName.Equals(user.FirstName))
                    apiUpdateUser.FirstName = UserProfileDetails.Instance.FirstName;

                if (!UserProfileDetails.Instance.LastName.Equals(user.LastName))
                    apiUpdateUser.LastName = UserProfileDetails.Instance.LastName;


                // Update the server with the details
                var updateProfile = await WebRequests.PostAsync<ApiResponse<string>>(
                    // Set URL
                    appsettings.HostUrl + $"/{ApiRoutes.UpdateUserProfile}",
                    // Pass the Api model
                    apiUpdateUser,
                    // Pass in user Token
                    bearerToken: user.Token);

                // If the response has an error...
                if (await updateProfile.ErrorIfFailedAsync(ErrorMessage))
                    return;

                // Send to all my new data
                await ChatHubManager.UpdateProfile(apiUpdateUser);

                // Update new user data from local database ClientMessages
                if (!string.IsNullOrWhiteSpace(apiUpdateUser.NewEmail))
                    new LocalDB().UpdateEmailInMessages(apiUpdateUser.CurrentEmail, apiUpdateUser.NewEmail);

                // Update new user data from local database LoginCredentials
                await ClientDataBase.SaveLoginCredentialsAsync(new LoginCredentialsDataModel
                {
                    Email = UserProfileDetails.Instance.Email,
                    FirstName = UserProfileDetails.Instance.FirstName,
                    LastName = UserProfileDetails.Instance.LastName,
                    Token = updateProfile.ServerResponse.Response
                });

                // Stop chat
                await ChatHubManager.Stop();

                // We start the new chat with the new data
                // Set token for connect to chathub
                ChatHubManager.ChatHubManagerConnection(updateProfile.ServerResponse.Response);

                // Check if you have connected to the chathub
                if (!await ChatHubManager.Start())
                    // We return if the connection has not been made
                    return;
            }

            #endregion

        }

        #endregion

        #region Logout Method

        /// <summary>
        /// Logs the user out
        /// </summary>
        public async Task LogoutAsync()
        {
            // Lock this command to ignore any other requests while processing
            await RunCommandAsync(() => LoggingOut, async () =>
            {
                // Confirm the user wants to logout
                var result = MessageBoxResultCustom.Show("Doriti sa iesiti?");

                if (result == System.Windows.MessageBoxResult.No ||
                    result == System.Windows.MessageBoxResult.Cancel)
                    return;

                // Clear all list friends
                ChatListViewModel.Items =
                new System.Collections.ObjectModel.ObservableCollection<ChatListItemViewModel>();

                // Clear all data from messages
                ChatMessageListViewModel.StaticList.Items =
                new System.Collections.ObjectModel.ObservableCollection<ChatMessageListItemViewModel>();
                ChatMessageListViewModel.StaticList.PendingMessageText = "";
                ChatMessageListViewModel.StaticList.ProfilePictureRGB = "";
                ChatMessageListViewModel.StaticList.Display = "";
                ChatMessageListViewModel.StaticList.OpenMessages = false;
                ChatMessageListViewModel.StaticList.Status = false;

                // Stop chat
                await ChatHubManager.Stop();

                // Go to login page
                ViewModelApplication.GoToPage(ApplicationPage.Login);

                ApplicationViewModel.SearchField = "";
                ApplicationViewModel.SearchIsRunning = false;

                // Clear any user data/cache
                LocalDB localDB = new LocalDB();
                localDB.DeleteLoginCredentials();
                localDB.DeleteClientFriends();
                localDB.DeleteClientMessages();

                ClearUserData();

            });
        }

        #endregion

        #region Load Method

        public async Task LoadAsync()
        {
            var scopedClientDataBase = ClientDataBase;

            var user = await scopedClientDataBase.GetLoginCredentialsAsync();

            // If we don't have a token (so we are not logged in...)
            if (string.IsNullOrWhiteSpace(user.Token))
                // Then do nothing more
                return;

            #region Get And Update User Profile Details

            var result = await WebRequests.PostAsync<ApiResponse<UserProfileDetails>>(
                    // Set URL
                    appsettings.HostUrl + $"/{ApiRoutes.GetUserProfile}",
                    // Pass in user Token
                    bearerToken: user.Token);

            if (await result.ErrorIfFailedAsync("Incarcarea detaliilor utilizatorului a esuat"))
                return;

            // Should we check if the values are different before saving
            var response = result.ServerResponse.Response;

            if (!(user.Email.Equals(response.Email) &&
                user.FirstName.Equals(response.FirstName) &&
                user.LastName.Equals(response.LastName)))
            {
                DialogWindow.Show("Datele nu coincid!", "Autentificare esuata");
                return;
            }

            // Create data model from the response
            var dataModel = result.ServerResponse.Response.ToLoginCredentialsDataModel();
            // Re-add our known token
            dataModel.Token = user.Token;

            // Save the new information in the data store
            await scopedClientDataBase.SaveLoginCredentialsAsync(dataModel);

            // Update values from local cache
            await UpdateValuesFromLocalStoreAsync(scopedClientDataBase);
            #endregion

            #region Get And Save Client Friends

            // Load the client's friends from the server
            var resultFriends = await WebRequests.PostAsync<ApiResponse<FriendsResultsApiModel>>(
                    // Set URL
                    appsettings.HostUrl + $"/{ApiRoutes.GetFriends}",
                    // Pass in user Token
                    bearerToken: user.Token);

            if (await resultFriends.ErrorIfFailedAsync("Incarcarea prietenilor a esuat"))
                return;

            // Save the information in the data store
            await ClientDataBase.SaveClientFriendsAsync(resultFriends.ServerResponse.Response);

            #endregion

            #region Get And Save Client Messages

            var resultMessages = await WebRequests.PostAsync<ApiResponse<GetMessagesApiModels>>(
                 // Set URL
                 appsettings.HostUrl + $"/{ApiRoutes.GetMessage}",
                 // Pass in user Token
                 bearerToken: user.Token);

            if (await resultMessages.ErrorIfFailedAsync("Incarcarea mesajelor a esuat"))
                return;

            // Save in data store
            LocalDB localDB = new LocalDB();

            localDB.DeleteClientMessages();

            for (int i = 0; i < resultMessages.ServerResponse.Response.Count; i++)
                localDB.InsertMessage(resultMessages.ServerResponse.Response[i].Message, resultMessages.ServerResponse.Response[i].SendBy_Email,
                    resultMessages.ServerResponse.Response[i].SendTo_Email, resultMessages.ServerResponse.Response[i].MessageSentTime);
            #endregion

            // Set token for connect to chathub
            ChatHubManager.ChatHubManagerConnection(user.Token);

            // Check if you have connected to the chathub
            if (!await ChatHubManager.Start())
                return;
        }

        #endregion

        #region Private Helper Methods

        /// <summary>
        /// Loads the settings from the local database and binds 
        /// them to user profile details
        /// </summary>
        private async Task UpdateValuesFromLocalStoreAsync(IClientDataBase ClientDataBase)
        {
            // Get the stored credentials
            var storedCredentials = await ClientDataBase.GetLoginCredentialsAsync();

            // Set email
            UserProfileDetails.Instance.Email = storedCredentials.Email;
            // Set first name
            UserProfileDetails.Instance.FirstName = storedCredentials.FirstName;
            // Set last name
            UserProfileDetails.Instance.LastName = storedCredentials.LastName;
            // Set profile picture
            UserProfileDetails.Instance.ProfilePictureRGB = GenerateRGBValues.Generate(storedCredentials.FirstName[0]);
            //Set status
            UserProfileDetails.Instance.Status = true;
        }

        /// <summary>
        /// Clears any data specific to the current user
        /// </summary>
        public void ClearUserData()
        {
            // Clear all view models containing the users info
            UserProfileDetails.Instance.Email = "";
            UserProfileDetails.Instance.FirstName = "";
            UserProfileDetails.Instance.LastName = "";
            UserProfileDetails.Instance.ProfilePictureRGB = "";
            UserProfileDetails.Instance.Token = "";
            UserProfileDetails.Instance.Status = false;

            CurrentPassword = new SecureString();
            NewPassword = new SecureString();
        }

        #endregion
    }
}
