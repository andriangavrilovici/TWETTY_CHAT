using System.Windows.Controls;

namespace TWETTY_CHAT
{
    /// <summary>
    /// Interaction logic for SlideMenuControl.xaml
    /// </summary>
    public partial class SideMenuControl : UserControl
    {
        public SideMenuControl()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(ApplicationViewModel.SearchField))
            {
                ApplicationViewModel.SearchIsRunning = true;
            } else 
                ApplicationViewModel.SearchIsRunning = false;

            new ApplicationViewModel().SearchAsync().ConfigureAwait(true);

        }
    }
}
