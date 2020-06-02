using static TWETTY_CHAT.DI;

namespace TWETTY_CHAT
{
    /// <summary>
    /// Locates view models from the IoC for user in binding in Xaml files
    /// </summary>
    public class ViewModelLocator
    {
        #region Public Property

        /// <summary>
        /// Singleton instance of the locator
        /// </summary>
        public static ViewModelLocator Instance { get; private set; } = new ViewModelLocator();

        /// <summary>
        /// The application view model
        /// </summary>
        public ApplicationViewModel ApplicationViewModel => ViewModelApplication;

        /// <summary>
        /// The setting view model
        /// </summary>
        public SettingsViewModel SettingsViewModel => ViewModelSetting;

        #endregion
    }
}
