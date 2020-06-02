namespace TWETTY_CHAT.Core
{
    /// <summary>
    /// The details to change for a User Profile from an API client call
    /// </summary>
    public class UpdateUserProfileApiModel
    {
        /// <summary>
        /// The current email
        /// </summary>
        public string CurrentEmail { get; set; }

        /// <summary>
        /// The new email
        /// </summary>
        public string NewEmail { get; set; }

        /// <summary>
        /// The new first name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// The new last name
        /// </summary>
        public string LastName { get; set; }
        
    }
}
