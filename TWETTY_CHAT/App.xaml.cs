using TWETTY_CHAT.Core;
using Dna;
using System.Threading.Tasks;
using System.Windows;
using static TWETTY_CHAT.DI;
using static Dna.FrameworkDI;

namespace TWETTY_CHAT
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Custom startup so we load our IoC immediately before anything else
        /// </summary>
        /// <param name="e"></param>
        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            // Setup the main application
            await ApplicationSetupAsync();

            // Log it
            Logger.LogDebugSource("Application starting...");

            
            if (await ClientDataBase.HasCredentialsAsync())
            {
                await ViewModelSetting.LoadAsync();
                ViewModelApplication.OpenChat();
                ViewModelApplication.GoToPage(ApplicationPage.Chat);
            }
            else
            ViewModelApplication.GoToPage(ApplicationPage.Login);

            // Show the main window
            Current.MainWindow = new MainWindow();
            Current.MainWindow.Show();
        }
        /// <summary>
        /// Configures our application ready for use
        /// </summary>
        private async Task ApplicationSetupAsync()
        {
            // Setup the Dna Framework
            Framework.Construct<DefaultFrameworkConstruction>()
                .AddFileLogger()
                .AddClientDataBase()
                .AddChatAppViewModels()
                .Build();

            // Ensure the client data store 
            await ClientDataBase.EnsureDataStoreAsync();
        }

    }
}
