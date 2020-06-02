using System.Windows.Controls;

namespace TWETTY_CHAT
{
    /// <summary>
    /// Interaction logic for MessageBoxControl.xaml
    /// </summary>
    public partial class MessageBoxControl : UserControl
    {
        public MessageBoxControl(string message, double btnWidth)
        {
            InitializeComponent();
            MessageTxt.Text = message;
            OkBtn.Width = btnWidth;
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        }
    }
}
