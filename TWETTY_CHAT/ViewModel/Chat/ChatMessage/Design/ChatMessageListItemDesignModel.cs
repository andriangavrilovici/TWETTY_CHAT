using System;

namespace TWETTY_CHAT
{
    /// <summary>
    /// The design-time data for a <see cref="ChatMessageListItemViewModel"/>
    /// </summary>
    public class ChatMessageListItemDesignModel : ChatMessageListItemViewModel
    {
        #region Singleton

        /// <summary>
        /// A single instance of the design model
        /// </summary>
        public static ChatMessageListItemDesignModel Instance => new ChatMessageListItemDesignModel();


        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public ChatMessageListItemDesignModel()
        {
            SenderFirstName = "Ivan";
            SenderLastName = "Biteikin";
            Message = "This new chat app is awesome! I bet it will be fast too";
            ProfilePictureRGB = "#18D995";
            MessageSentTime = DateTimeOffset.UtcNow;
            SentByMe = false;
        }

        #endregion
    }
}
