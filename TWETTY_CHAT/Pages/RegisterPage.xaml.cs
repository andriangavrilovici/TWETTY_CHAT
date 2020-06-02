using System.Security;
namespace TWETTY_CHAT
{
    /// <summary>
    /// Interaction logic for RegisterPage.xaml
    /// </summary>
    public partial class RegisterPage : BasePage<RegisterViewModel>, IHavePassword
    {
        public RegisterPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor with specific view model
        /// </summary>
        public RegisterPage(RegisterViewModel specificViewModel) : base(specificViewModel)
        {
            InitializeComponent();
        }

        /// <summary>
        /// The secure password for this register page
        /// </summary>
        public SecureString SecurePassword => PasswordText.SecurePassword;

    }
}
