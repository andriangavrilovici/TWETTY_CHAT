using System;

namespace TWETTY_CHAT
{
    /// <summary>
    /// A view model for each chat message thread item in a chat thread
    /// </summary>
    public class ChatMessageListItemViewModel : BaseViewModel
    {
        /// <summary>
        /// The display first name of the sender of the message
        /// </summary>
        public string SenderFirstName { get; set; }

        /// <summary>
        /// The display last name of the sender of the message
        /// </summary>
        public string SenderLastName { get; set; }

        /// <summary>
        /// The latest message from this chat
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// The initials to show for the profile picture background
        /// </summary>
        public string Initials
        {
            get
            {
                return string.Format("{0}{1}", string.IsNullOrWhiteSpace(SenderFirstName) ? ' ' : SenderFirstName[0], string.IsNullOrWhiteSpace(SenderLastName) ? ' ' : SenderLastName[0]).ToUpper();
            }
        }

        /// <summary>
        /// The RGB values (in hex) for the background color of the profile picture
        /// For example FF00FF for Red and Blue mixed
        /// </summary>
        public string ProfilePictureRGB { get; set; }

        /// <summary>
        /// True if this message was sent by the signed in user
        /// </summary>
        public bool SentByMe { get; set; }

        /// <summary>
        /// The time the message was sent
        /// </summary>
        public DateTimeOffset MessageSentTime { get; set; }

        /// <summary>
        /// A flag indicating if this item was added since the first main list of items was created
        /// Used as a flag for animating in
        /// </summary>
        public bool NewItem { get; set; }
    }
}
