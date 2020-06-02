using TWETTY_CHAT.Core;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TWETTY_CHAT
{
    /// <summary>
    /// A view model for a chat message thread list 
    /// </summary>
    public class ChatMessageListViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// The chat thread items for the list
        /// NOTE: Do not call Items.Add to add messages to this list
        /// </summary>
        public ObservableCollection<ChatMessageListItemViewModel> Items { get; set; }

        /// <summary>
        /// The text for the current message being written
        /// </summary>
        public string PendingMessageText { get; set; }

        public string Display { get; set; }
        public string ProfilePictureRGB { get; set; }
        public string Initials { get; set; }
        public bool Status { get; set; }

        public bool OpenMessages { get; set; } = false;


        public static ChatMessageListViewModel StaticList;

        #endregion

        #region Public Commands
        /// <summary>
        /// The command for when the user clicks the send button
        /// </summary>
        public ICommand SendCommand { get; set; }
        /// <summary>
        /// The command for when the user clicks the send file
        /// </summary>
        public ICommand AttachedCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public ChatMessageListViewModel()
        {
            StaticList = this;

            // Create commands
            SendCommand = new RelayCommand(async () => await SendAsync());
            AttachedCommand = new RelayCommand(Attached);
        }

        #endregion

        /// <summary>
        /// Get items from local data base
        /// </summary>
        /// <param name="Email">Friend's email address to receive messages</param>
        /// <returns>Messages extracted from the database</returns>
        public ObservableCollection<ChatMessageListItemViewModel> GetItems(string Email = null)
        {
            Items = new ObservableCollection<ChatMessageListItemViewModel>();

            // 10,5,2 ms exec
            LocalDB localDB = new LocalDB();
            localDB.GetMessageDetails(Email: Email);

            while (localDB.sqlite_datareader.Read())
            {
                if (string.Equals(localDB.sqlite_datareader["SendBy_Email"].ToString(), UserProfileDetails.Instance.Email))
                {
                    Items.Add(new ChatMessageListItemViewModel
                    {
                        SenderFirstName = UserProfileDetails.Instance.FirstName,
                        SenderLastName = UserProfileDetails.Instance.LastName,
                        Message = localDB.sqlite_datareader["Message"].ToString(),
                        ProfilePictureRGB = UserProfileDetails.Instance.ProfilePictureRGB,
                        MessageSentTime = DateTimeOffset.Parse(localDB.sqlite_datareader["MessageSentTime"].ToString()),
                        SentByMe = true
                    });
                }
                else
                    Items.Add(new ChatMessageListItemViewModel
                    {
                        SenderFirstName = localDB.sqlite_datareader["FirstName"].ToString(),
                        SenderLastName = localDB.sqlite_datareader["LastName"].ToString(),
                        Message = localDB.sqlite_datareader["Message"].ToString(),
                        ProfilePictureRGB = GenerateRGBValues.Generate(localDB.sqlite_datareader["FirstName"].ToString()[0]),
                        MessageSentTime = DateTimeOffset.Parse(localDB.sqlite_datareader["MessageSentTime"].ToString()),
                        SentByMe = false

                    });
            }

            // Sort messages by date
            Items = new ObservableCollection<ChatMessageListItemViewModel>(Items.OrderBy(i => i.MessageSentTime));

            return Items;
        }

        #region Command Methods

        /// <summary>
        /// When the user clicks the send button, sends the message
        /// </summary>
        private async Task SendAsync()
        {
            if (Items == null)
                Items = new ObservableCollection<ChatMessageListItemViewModel>();


            // Don't send a blank message
            if (string.IsNullOrWhiteSpace(PendingMessageText))
                return;

            // Send a new message
            var message = new ChatMessageListItemViewModel
            {
                Message = PendingMessageText,
                MessageSentTime = DateTime.UtcNow,
                SenderFirstName = UserProfileDetails.Instance.FirstName,
                SenderLastName = UserProfileDetails.Instance.LastName,
                ProfilePictureRGB = UserProfileDetails.Instance.ProfilePictureRGB,
                SentByMe = true,
                NewItem = true
            };

            // Add message to both lists
            Items.Add(message);
            string SendTo_Email = null;
            for (int i = 0; i < ChatListViewModel.Items.Count; i++)
            {
                if (ChatListViewModel.Items[i].IsSelected)
                    SendTo_Email = ChatListViewModel.Items[i].Email;
            }
            if (SendTo_Email != null)
            {
                LocalDB localDB = new LocalDB();
                DateTimeOffset dateTime = DateTimeOffset.Now;
                localDB.InsertMessage(PendingMessageText, UserProfileDetails.Instance.Email, SendTo_Email, dateTime);

                await ChatHubManager.SendBy(new MessageApiModel
                {
                    SendBy_Email = UserProfileDetails.Instance.Email,
                    SendTo_Email = SendTo_Email,
                    Message = PendingMessageText,
                    MessageSentTime = dateTime
                });

            }
            // Clear the pending message text
            PendingMessageText = string.Empty;

        }

        private void Attached()
        {
            DialogWindow.Show("Ne pare rau.\nAcest buton la moment nu functioneaza.", "Nu functioneaza");
        }

        #endregion

    }
}
