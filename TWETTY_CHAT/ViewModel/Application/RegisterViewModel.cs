using TWETTY_CHAT.Core;
using System.Security;
using System.Threading.Tasks;
using System.Windows.Input;
using static TWETTY_CHAT.DI;

namespace TWETTY_CHAT
{
    /// <summary>
    /// The View Model for a register screen
    /// </summary>
    public class RegisterViewModel : BaseViewModel
    {
        #region Public Properties

        // The email of the user
        public string Email { get; set; }

        // The first name of the user
        public string FirstName { get; set; }

        // The last name of the user
        public string LastName { get; set; }

        /// <summary>
        /// A flag indicating if the Register command is running
        /// </summary>
        public bool RegisterIsRunning { get; set; }

        #endregion

        #region Commands

        /// <summary>
        /// The command to register for a new account
        /// </summary>
        public ICommand RegisterCommand { get; set; }

        /// <summary>
        /// The command to login
        /// </summary>
        public ICommand LoginCommand { get; set; }



        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public RegisterViewModel()
        {
            // Create commands
            RegisterCommand = new RelayParameterizedCommand(async (parameter) => await RegisterAsync(parameter));
            LoginCommand = new RelayCommand(async () => await LoginAsync());
        }

        #endregion

        /// <summary>
        /// Attempts to register a new user
        /// </summary>
        /// <param name="parameter">The <see cref="SecureString"/> passed in from the view for the users password</param>
        /// <returns></returns>
        public async Task RegisterAsync(object parameter)
        {
            await RunCommandAsync(() => RegisterIsRunning, async () =>
            {
                if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(FirstName) ||
                string.IsNullOrWhiteSpace(LastName) || string.IsNullOrWhiteSpace((parameter as IHavePassword).SecurePassword.Unsecure()))
                {
                    DialogWindow.Show("Nu ati introdus toate datele", "Inregistrare esuata");
                    return;
                }

                if (!EmailValidation.IsValidEmail(Email))
                {
                    DialogWindow.Show("Acest email nu este valid.", "Inregistrare esuata");
                    return;
                }

                // Preloader register
                DialogWindow.ShowPreloader();

                var result = await WebRequests.PostAsync<ApiResponse<UserProfileDetails>>(
                    appsettings.HostUrl + $"/{ApiRoutes.Register}",
                    new RegisterApiModel
                    {
                        FirstName = FirstName,
                        LastName = LastName,
                        Email = Email,
                        Password = (parameter as IHavePassword).SecurePassword.Unsecure()
                    });

                // If the response has an error...
                if (await result.ErrorIfFailedAsync("Inregistrare esuata"))
                    return;

                // Let the application view model handle what happens
                // with the successful login
                await ViewModelApplication.LoginSuccessfulAsync(result.ServerResponse.Response);
            });
        }

        /// <summary>
        /// Takes the user to the login page
        /// </summary>
        /// <returns></returns>
        public async Task LoginAsync()
        {
            // Go to login page
            ViewModelApplication.GoToPage(ApplicationPage.Login);

            await Task.Delay(1);
        }
    }
}
