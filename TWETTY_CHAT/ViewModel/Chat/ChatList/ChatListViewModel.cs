using TWETTY_CHAT.Core;
using System.Collections.ObjectModel;
using static TWETTY_CHAT.DI;

namespace TWETTY_CHAT
{
    /// <summary>
    /// A view model for the overview chat list 
    /// </summary>
    public class ChatListViewModel : BaseViewModel
    {
        /// <summary>
        /// The chat list items for the list
        /// </summary>  
        public static ObservableCollection<ChatListItemViewModel> Items { get; set; }

        public static async void chatList(SearchUsersResultsApiModel searchUsers = null)
        {
            if (!ApplicationViewModel.SearchIsRunning || searchUsers == null)
            {
                Items.Clear();

                var friends = await ClientDataBase.GetClientFriendsAsync();

                foreach (var friend in friends)
                {
                    Items.Add(new ChatListItemViewModel
                    {
                        Email = friend.Email,
                        FirstName = friend.FirstName,
                        LastName = friend.LastName,
                        ProfilePictureRGB = GenerateRGBValues.Generate(friend.FirstName[0]),
                        NewContentAvailable = false,
                        Status = friend.Status,
                    });
                }
                return;
            }

            if (searchUsers != null)
            {
                Items.Clear();
                for (int i = 0; i < searchUsers.Count; i++)
                {
                    Items.Add(new ChatListItemViewModel
                    {
                        Email = searchUsers[i].Email,
                        FirstName = searchUsers[i].FirstName,
                        LastName = searchUsers[i].LastName,
                        ProfilePictureRGB = GenerateRGBValues.Generate(searchUsers[i].FirstName[0]),
                        NewContentAvailable = false,
                        Status = false,
                    });
                }
            }
        }
    }
}
