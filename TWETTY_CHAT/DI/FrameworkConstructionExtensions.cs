using Dna;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace TWETTY_CHAT
{
    /// <summary>
    /// Extension methods for the <see cref="FrameworkConstruction"/>
    /// </summary>
    public static class FrameworkConstructionExtensions
    {
        /// <summary>
        /// Injects the view models needed for ChatApp application
        /// </summary>
        /// <param name="construction"></param>
        /// <returns></returns>
        public static FrameworkConstruction AddChatAppViewModels(this FrameworkConstruction construction)
        {
            // Bind to a single instance of Application view model
            construction.Services.AddSingleton<ApplicationViewModel>();

            // Bind to a single instance of Settings view model
            construction.Services.AddSingleton<SettingsViewModel>();

            // Return the construction for chaining
            return construction;
        }

        public static FrameworkConstruction AddClientDataBase(this FrameworkConstruction construction)
        {
            // Inject our SQLite EF data base
            construction.Services.AddDbContext<ClientDataBaseDbContext>(options =>
            {
                // Setup connection string
                options.UseSqlite(appsettings.ConnectionClientDataBase);
            }, contextLifetime: ServiceLifetime.Transient);

            // Add client data base for easy access/use of the backing data base
            // Make it scoped so we can inject the scoped DbContext
            construction.Services.AddTransient<IClientDataBase>(
                provider => new BaseClientDataBase(provider.GetService<ClientDataBaseDbContext>()));

            // Return framework for chaining
            return construction;
        }
    }
}
