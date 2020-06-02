namespace TWETTY_CHAT
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class InformationPage : BasePage<SettingsViewModel>
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public InformationPage() : base()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor with specific view model
        /// </summary>
        /// <param name="specificViewModel">The specific view model to use for this page</param>
        public InformationPage(SettingsViewModel specificViewModel) : base(specificViewModel)
        {
            InitializeComponent();
        }
    }
}
