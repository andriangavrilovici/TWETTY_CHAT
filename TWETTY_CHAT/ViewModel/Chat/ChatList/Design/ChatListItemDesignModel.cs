namespace TWETTY_CHAT
{
    /// <summary>
    /// The design-time data for a <see cref="ChatListItemViewModel"/>
    /// </summary>
    public class ChatListItemDesignModel : ChatListItemViewModel
    {
        #region Singleton

        /// <summary>
        /// A single instance of the design model
        /// </summary>
        public static ChatListItemDesignModel Instance => new ChatListItemDesignModel();


        #endregion  

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public ChatListItemDesignModel()
        {
            FirstName = "Ivan";
            LastName = "Biteikin";
            ProfilePictureRGB = "#18D995";
            Status = true;
        }

        #endregion


    }
}
