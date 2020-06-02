using Dna;

namespace TWETTY_CHAT
{
    public static class DI
    {
        /// <summary>
        /// A shortcut to access the <see cref="ApplicationViewModel"/>
        /// </summary>
        public static ApplicationViewModel ViewModelApplication => Framework.Service<ApplicationViewModel>();

        /// <summary>
        /// A shortcut to access the <see cref="SettingsViewModel"/>
        /// </summary>
        public static SettingsViewModel ViewModelSetting => Framework.Service<SettingsViewModel>();

        /// <summary>
        /// A shortcut to access toe <see cref="IClientDataBase"/> service
        /// </summary>
        public static IClientDataBase ClientDataBase => Framework.Service<IClientDataBase>();
    }
}
