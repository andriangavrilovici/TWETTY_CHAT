namespace TWETTY_CHAT.Core
{
    public class FriendApiModel
    {
        /// <summary>
        /// The friend email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The friend first name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// The friend last name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// The friend status
        /// </summary>
        public bool Status { get; set; } = false;
    }
}
