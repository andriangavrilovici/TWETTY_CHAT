using System.Windows;

namespace TWETTY_CHAT
{
    /// <summary>
    /// Interaction logic for DialogWindow.xaml
    /// </summary>
    public partial class DialogWindow : Window
    {
        #region Private Members

        private static DialogWindow _dialogWindow { get; set; }
        private static DialogWindow _dialogWindowPreloader { get; set; }

        #endregion
        public static bool RunPreloader { get; set; } = false;

        public static void Show(string message, string title = "Caseta de mesaje")
        {
            if (string.IsNullOrWhiteSpace(message))
                return;

            var formatText = FormattedString.Measure(message, "Trebuchet MS", 24);

            _dialogWindow = new DialogWindow
            {
                ControlContent = { Content = new MessageBoxControl(message, formatText.Width) },
                TitleTxt = { Text = title }
            };


            _dialogWindow.Width = 70 + formatText.Width;
            _dialogWindow.Height = 150 + formatText.Height;
            _dialogWindow.ShowDialog();
        }

        public static void ShowPreloader()
        {
            _dialogWindowPreloader = new DialogWindow
            {
                ControlContent = { Content = new PreloaderControl() },
                TitleTxt = { Text = "Incarcare" }
            };
            RunPreloader = true;
            _dialogWindowPreloader.Show();
        }
        public static void ClosePreloader()
        {
            RunPreloader = false;
            _dialogWindowPreloader.Hide();
            _dialogWindowPreloader.Close();
        }
        private DialogWindow()
        {
            InitializeComponent();

            if (Application.Current.MainWindow.IsActive)
                this.Owner = Application.Current.MainWindow;
            this.WindowStartupLocation = WindowStartupLocation.CenterOwner;
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            if (RunPreloader)
                RunPreloader = false;
            this.Close();
        }
    }
}
