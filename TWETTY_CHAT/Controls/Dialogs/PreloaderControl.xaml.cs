using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace TWETTY_CHAT
{
    /// <summary>
    /// Interaction logic for PreloaderDialog.xaml
    /// </summary>
    public partial class PreloaderControl : UserControl
    {
        public PreloaderControl()
        {
            InitializeComponent();
            ((Storyboard)FindResource("WaitStoryboard")).Begin();
        }
    }
}
