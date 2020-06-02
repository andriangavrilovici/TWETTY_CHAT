using TWETTY_CHAT.Core;
using System.Security;
using System.Threading.Tasks;
using System.Windows.Input;
using static TWETTY_CHAT.DI;

namespace TWETTY_CHAT
{
    /// <summary>
    /// The View Model for a login screen
    /// </summary>
    public class LoginViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// The email of the user
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// A flag indicating if the login command is running
        /// </summary>
        public bool LoginIsRunning { get; set; }

        #endregion

        #region Commands

        /// <summary>
        /// The command to login
        /// </summary>
        public ICommand LoginCommand { get; set; }

        /// <summary>
        /// The command to register for a new account
        /// </summary>
        public ICommand RegisterCommand { get; set; }

        /// <summary>
        /// The command to forget for a account
        /// </summary>
        public ICommand ForgotCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public LoginViewModel()
        {
            // Create commands
            LoginCommand = new RelayParameterizedCommand(async (parameter) => await LoginAsync(parameter));
            RegisterCommand = new RelayCommand(Register);
            ForgotCommand = new RelayCommand(Forgot);
        }

        #endregion


        /// <summary>
        /// Attempts to log the user in
        /// </summary>
        /// <param name="parameter">The <see cref="SecureString"/> passed in from the view for the users password</param>
        /// <returns></returns>
        private async Task LoginAsync(object parameter)
        {
            await RunCommandAsync(() => LoginIsRunning, async () =>
            {
                // Email verification
                if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace((parameter as IHavePassword).SecurePassword.Unsecure()))
                {
                    DialogWindow.Show("Nu ati introdus email-ul sau parola", "Autentificare esuata");
                    return;
                }
                
                if (!EmailValidation.IsValidEmail(Email))
                {
                    DialogWindow.Show("Acest email nu este valid.", "Autentificare esuata");
                    return;
                }
                // Preloader login
                DialogWindow.ShowPreloader();

                var result = await WebRequests.PostAsync<ApiResponse<UserProfileDetails>>(
                    appsettings.HostUrl + $"/{ApiRoutes.Login}",
                    new LoginApiModel
                    {
                        Email = Email,
                        Password = (parameter as IHavePassword).SecurePassword.Unsecure()
                    });

                // If the response has an error...
                if (await result.ErrorIfFailedAsync("Autentificare esuata"))
                    return;

                // Let the application view model handle what happens
                // with the successful login
                await ViewModelApplication.LoginSuccessfulAsync(result.ServerResponse.Response);

            });
        }

        /// <summary>
        /// Takes the user to the register page
        /// </summary>
        private void Register()
        {
            // Go to register page?
            ViewModelApplication.GoToPage(ApplicationPage.Register);
        }

        /// <summary>
        /// Brings the user to the contact details
        /// </summary>
        private void Forgot()
        {
            DialogWindow.Show("Date de legatura:\nAndrian Gavrilovici\nemail:andrian.gav@gmail.com\nTelefon:+37360965765");
        }
    }
}
