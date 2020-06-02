using System.Windows;

namespace TWETTY_CHAT
{
    /// <summary>
    /// Interaction logic for MessageBoxResultCustom.xaml
    /// </summary>
    public partial class MessageBoxResultCustom : Window
    {
        private static MessageBoxResultCustom messageBox { get; set; }
        private static MessageBoxResult result = MessageBoxResult.Cancel;
        public static MessageBoxResult Show(string message, string titleMessage = "Confirmare")
        {
            if (string.IsNullOrWhiteSpace(message))
                return MessageBoxResult.Cancel;

            var formatText = FormattedString.Measure(message, "Trebuchet MS", 20);

            messageBox = new MessageBoxResultCustom
            {
                MessageTxt = { Text = message },
                TitleTxt = { Text = titleMessage }
            };
            messageBox.Width = 70 + formatText.Width; ;
            messageBox.Height = 150 + formatText.Height;

            if (messageBox.Width < 290)
                messageBox.Width = 290;

            messageBox.ShowDialog();

            return result;
        }
        private MessageBoxResultCustom()
        {
            InitializeComponent();
            this.Owner = Application.Current.MainWindow;
            this.WindowStartupLocation = WindowStartupLocation.CenterOwner;
        }
        private void ButtonYes_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            result = MessageBoxResult.Yes;
        }
        private void ButtonNo_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            result = MessageBoxResult.No;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
