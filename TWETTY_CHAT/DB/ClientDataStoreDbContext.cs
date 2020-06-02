using TWETTY_CHAT.Core;
using Microsoft.EntityFrameworkCore;

namespace TWETTY_CHAT
{
    public class ClientDataBaseDbContext : DbContext
    {
        #region DbSets 

        /// <summary>
        /// The client login credentials
        /// </summary>
        public DbSet<LoginCredentialsDataModel> LoginCredentials { get; set; }

        /// <summary>
        /// The client friends
        /// </summary>
        public DbSet<FriendApiModel> ClientFriends { get; set; }

        /// <summary>
        /// The client messages
        /// </summary>
        public DbSet<MessageApiModel> ClientMessages { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public ClientDataBaseDbContext(DbContextOptions<ClientDataBaseDbContext> options) : base(options) { }

        #endregion

        #region Model Creating

        /// <summary>
        /// Configures the database structure and relationships
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Fluent API

            // Set Id as primary key
            modelBuilder.Entity<LoginCredentialsDataModel>().HasKey(a => a.Id);

            modelBuilder.Entity<FriendApiModel>().HasKey(b => b.Email);

            modelBuilder.Entity<MessageApiModel>().HasKey(c => c.Id);

        }

        #endregion
    }
}
