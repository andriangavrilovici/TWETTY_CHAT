namespace TWETTY_CHAT
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : BasePage<SettingsViewModel>
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public SettingsPage() : base()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor with specific view model
        /// </summary>
        /// <param name="specificViewModel">The specific view model to use for this page</param>
        public SettingsPage(SettingsViewModel specificViewModel) : base(specificViewModel)
        {
            InitializeComponent();
        }

        private void NewPasswordText_PasswordChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            // Update view model
            if (DataContext is SettingsViewModel viewModel)
                viewModel.NewPassword = NewPasswordText.SecurePassword;
        }

        private void CurrentPasswordText_PasswordChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            // Update view model
            if (DataContext is SettingsViewModel viewModel)
                viewModel.CurrentPassword = CurrentPasswordText.SecurePassword;
        }
    }
}
