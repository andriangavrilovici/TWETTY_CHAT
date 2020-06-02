using TWETTY_CHAT.Core;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using static TWETTY_CHAT.DI;

namespace TWETTY_CHAT
{
    /// <summary>
    /// The application state as a view model
    /// </summary>
    public class ApplicationViewModel : BaseViewModel
    {
        /// <summary>
        /// The current page of the application
        /// </summary>
        public ApplicationPage CurrentPage { get; set; } = ApplicationPage.Login;
        /// <summary>
        /// The view model to use for the current page when the CurrentPage changes
        /// NOTE: This is not a live up-to-date view model of the current page
        ///       it is simply used to set the view model of the current page 
        ///       at the time it changes
        /// </summary>
        public BaseViewModel CurrentPageViewModel { get; set; }

        /// <summary>
        /// Determines the currently visible side menu content
        /// </summary>
        public SideMenuContent CurrentSideMenuContent { get; set; } = SideMenuContent.Contacts;

        #region Side Menu Search Properties

        /// <summary>
        /// The email of the user
        /// </summary>
        public static string SearchField { get; set; }

        /// <summary>
        /// A flag indicating if the login command is running
        /// </summary>
        public static bool SearchIsRunning { get; set; } = false;

        #endregion

        #region Public Commands

        /// <summary>
        /// The command to change the side menu to the Chat
        /// </summary>
        public ICommand OpenChatCommand { get; set; }

        /// <summary>
        /// The command to change the side menu to the Contacts
        /// </summary>
        public ICommand OpenContactsCommand { get; set; }

        /// <summary>
        /// The command to change the side menu to the Contacts
        /// </summary>
        public ICommand SearchCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// The default constructor
        /// </summary>
        public ApplicationViewModel()
        {
            // Create the commands
            OpenChatCommand = new RelayCommand(OpenChat);
            OpenContactsCommand = new RelayCommand(OpenContacts);
            SearchCommand = new RelayCommand(async () => await SearchAsync());
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Changes the current side menu to the Chat
        /// </summary>
        public void OpenChat()
        {
            // Set the current side menu to Chat
            CurrentSideMenuContent = SideMenuContent.Chat;
        }

        /// <summary>
        /// Changes the current side menu to the Contacts
        /// </summary>
        public void OpenContacts()
        {
            // Set the current side menu to Chat
            CurrentSideMenuContent = SideMenuContent.Contacts;
        }

        public async Task SearchAsync()
        {
            if (string.IsNullOrWhiteSpace(SearchField) || SearchIsRunning == false)
            {
                SearchIsRunning = false;
                ChatListViewModel.chatList();
                
                return;
            }

            bool fHasSpace = SearchField.Contains(" ");
            string email = "";
            string firstname = "";
            string lastname = "";

            if (!fHasSpace)
            {
                email = firstname = lastname = SearchField;
            }
            else
            {
                string[] words = SearchField.Split(new char[] { ' ' });
                firstname = words[0];
                lastname = words[1];
            }

            var result = await WebRequests.PostAsync<ApiResponse<SearchUsersResultsApiModel>>(
                appsettings.HostUrl + $"/{ApiRoutes.SearchUsers}",
                new SearchUserApiModel
                {
                    Email = email,
                    FirstName = firstname,
                    LastName = lastname
                });

            // If the response has an error...
            if (await result.ErrorIfFailedAsync("Cautarea a esuat"))
                return;

            SearchIsRunning = true;

            ChatListViewModel.chatList(result.ServerResponse.Response);
        }

        #endregion

        /// <summary>
        /// Navigates to the specified page
        /// </summary>
        /// <param name="page">The page to go to</param>
        /// <param name="viewModel">The view model, if any, to set explicitly to the new page</param>
        public void GoToPage(ApplicationPage page, BaseViewModel viewModel = null)
        {
            // Set the view model
            CurrentPageViewModel = viewModel;

            // See if page has changed
            var different = CurrentPage != page;

            // Set the current page
            CurrentPage = page;

            // If the page hasn't changed, fire off notification
            // So pages still update if just the view model has changed
            if (!different)
                OnPropertyChanged(nameof(CurrentPage));
        }


        public async Task LoginSuccessfulAsync(UserProfileDetails loginResult)
        {
            await ClientDataBase.SaveLoginCredentialsAsync(loginResult.ToLoginCredentialsDataModel());

            await ViewModelSetting.LoadAsync();

            OpenContacts();

            OpenChat();

            ViewModelApplication.GoToPage(ApplicationPage.Chat);

            // If preloader is run then close preloader
            if (DialogWindow.RunPreloader)
            {
                Application.Current.MainWindow.Focus();
                Application.Current.MainWindow.Activate();
                DialogWindow.RunPreloader = false;
                DialogWindow.ClosePreloader();
            }
        }

    }
}
