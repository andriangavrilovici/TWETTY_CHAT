using TWETTY_CHAT.Core;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using static TWETTY_CHAT.DI;

namespace TWETTY_CHAT
{
    /// <summary>
    /// A view model for each chat list item in the overview chat list
    /// </summary>
    public class ChatListItemViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// This is user email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The display first name of this chat list
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// The display last name of this chat list
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// The display fist name and last name of the chat list
        /// </summary>
        public string FullName
        {
            get { return string.Format("{0} {1}", FirstName, LastName); }
        }

        /// <summary>
        /// The initials to show for the profile picture background
        /// </summary>
        public string Initials
        {
            get
            {
                return string.Format("{0}{1}", string.IsNullOrWhiteSpace(FirstName) ? ' ' : FirstName[0], string.IsNullOrWhiteSpace(LastName) ? ' ' : LastName[0]).ToUpper();
            }
        }

        /// <summary>
        /// The RGB values (in hex) for the background color of the profile picture
        /// For example FF00FF for Red and Blue mixed
        /// </summary>
        public string ProfilePictureRGB { get; set; }

        /// <summary>
        /// True if there are unread message in this chat
        /// </summary>
        public bool NewContentAvailable { get; set; }

        /// <summary>
        /// True if this item is currently selected
        /// </summary>
        public bool IsSelected { get; set; }

        /// <summary>
        /// True if this user is online
        /// </summary>
        public bool Status { get; set; }



        #endregion

        #region Public Commands

        /// <summary>
        /// Opens the current message thread
        /// </summary>
        public ICommand OpenMessageCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public ChatListItemViewModel()
        {
            // Create commands
            OpenMessageCommand = new RelayCommand(async () => await OpenMessage());
        }

        #endregion

        #region Command Methods

        public async Task OpenMessage()
        {

            for (int i = 0; i < ChatListViewModel.Items.Count; i++)
                if (ChatListViewModel.Items[i].IsSelected)
                    ChatListViewModel.Items[i].IsSelected = false;

            if (ApplicationViewModel.SearchIsRunning)
            {
                var friends = await ClientDataBase.GetClientFriendsAsync();
                bool isFriend = false;

                for (int i = 0; i < friends.Count; i++)
                    if (this.Email.Equals(friends[i].Email))
                    {
                        isFriend = true;
                        break;
                    }

                if (!isFriend)
                {

                    var result = MessageBoxResultCustom.Show("Aceasta persoana nu este in lista de prieteni.\nAdaugati aceasta persoana in prieteni?",
                                "Cautare");

                    if (result == MessageBoxResult.Yes)
                        await ChatHubManager.SendFriendRequest(this.Email);
                    
                    return;
                }
            }

            IsSelected = true;

            NewContentAvailable = false;

            ViewModelApplication.GoToPage(ApplicationPage.Chat, new ChatMessageListViewModel
            {
                Items = new ChatMessageListViewModel().GetItems(Email),
                Display = FullName,
                Initials = Initials,
                ProfilePictureRGB = ProfilePictureRGB,
                Status = Status,
                OpenMessages = true
            });


        }

        #endregion

    }
}
