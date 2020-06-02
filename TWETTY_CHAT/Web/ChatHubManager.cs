using TWETTY_CHAT.Core;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace TWETTY_CHAT
{
    public static class ChatHubManager
    {

        #region Public Properties

        /// <summary>
        /// Connection property
        /// </summary>
        public static HubConnection Connection { get; set; }

        #endregion

        #region Connection Settings

        /// <summary>
        /// Initialize the settings for connecting to the chat hub
        /// </summary>
        /// <param name="token">The token is used to access the connection</param>
        public static void ChatHubManagerConnection(string token)
        {
            Connection = new HubConnectionBuilder()
                .WithUrl(appsettings.HostUrl + "/chat", options =>
                {
                    options.AccessTokenProvider = () => Task.FromResult(token);
                })
                .WithAutomaticReconnect()
                .Build();

            RegisterEvents();
        }
        #endregion

        #region Check and Receive

        public static void RegisterEvents()
        {
            LocalDB localDB = new LocalDB();

            #region Receive Notification

            Connection.On<string>("Notify", notify =>
            {
                DialogWindow.Show(notify, "Notificare");
            });

            #endregion

            #region Receive Messages

            Connection.On<string>("Error", (message) =>
            {
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                      new Action(() =>
                      {
                          DialogWindow.Show(message, "A esuat");
                      }));
            });

            Connection.On<MessageApiModel>("SendBy", (message) =>
            {
                int index = 0;
                for (int i = 0; i < ChatListViewModel.Items.Count; i++)
                    if (ChatListViewModel.Items[i].Email == message.SendBy_Email)
                        index = i;

                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                    new Action(() =>
                    {
                        localDB.InsertMessage(message.Message, message.SendBy_Email, message.SendTo_Email, message.MessageSentTime);

                        if (ChatListViewModel.Items[index].IsSelected)
                        {

                            ChatMessageListViewModel.StaticList.Items.Add(new ChatMessageListItemViewModel
                            {
                                SenderFirstName = ChatListViewModel.Items[index].FirstName,
                                SenderLastName = ChatListViewModel.Items[index].LastName,
                                Message = message.Message,
                                ProfilePictureRGB = GenerateRGBValues.Generate(ChatListViewModel.Items[index].FirstName[0]),
                                MessageSentTime = message.MessageSentTime,
                                SentByMe = false
                            });
                        }
                        else
                        {
                            ChatListViewModel.Items[index].NewContentAvailable = true;
                        }
                    }));
            });
            #endregion

            #region Receive Friend Requests and Response

            Connection.On<FriendApiModel>("FriendRequest", (request) =>
            {
                MessageBoxResult result = MessageBoxResultCustom.Show($"Confirmati cererea de prietenie?\n" +
                            $"Email: {request.Email}\nNume: {request.FirstName}\nPrenume: {request.LastName}",
                                "Cerere de prietenie");
                switch (result)
                {
                    case MessageBoxResult.Yes:

                        SendFriendResponse(request.Email, true).ConfigureAwait(true);

                        localDB.InsertFriends(request);

                        Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                            new Action(() =>
                            {
                                ChatListViewModel.Items.Add(new ChatListItemViewModel
                                {
                                    Email = request.Email,
                                    FirstName = request.FirstName,
                                    LastName = request.LastName,
                                    ProfilePictureRGB = GenerateRGBValues.Generate(request.FirstName[0]),
                                    Status = request.Status,
                                    IsSelected = false,
                                    NewContentAvailable = false
                                });
                            }));
                        break;

                    case MessageBoxResult.No:
                        SendFriendResponse(request.Email, false).ConfigureAwait(true);
                        break;
                }
            });

            Connection.On<FriendApiModel>("FriendResponse", (response) =>
            {
                localDB.InsertFriends(response);

                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                    new Action(() =>
                    {
                        ChatListViewModel.Items.Add(new ChatListItemViewModel
                        {
                            Email = response.Email,
                            FirstName = response.FirstName,
                            LastName = response.LastName,
                            ProfilePictureRGB = GenerateRGBValues.Generate(response.FirstName[0]),
                            IsSelected = false,
                            NewContentAvailable = false,
                            Status = true
                        });
                    }));
            });

            #endregion

            #region Check Users is Online or Offline

            // Check if user is online
            Connection.On<string>("UserOnline", (email) =>
            {
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                      new Action(() =>
                      {
                          for (int i = 0; i < ChatListViewModel.Items.Count; i++)
                          {
                              if (email == ChatListViewModel.Items[i].Email)
                              {
                                  ChatListViewModel.Items[i].Status = true;
                                  localDB.UpdateClientStatus(email, true);
                                  break;
                              }
                          }
                      }));
            });

            // Check if user is offline
            Connection.On<string>("UserOffline", (email) =>
            {
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                      new Action(() =>
                      {
                          for (int i = 0; i < ChatListViewModel.Items.Count; i++)
                          {
                              if (email == ChatListViewModel.Items[i].Email)
                              {
                                  ChatListViewModel.Items[i].Status = false;
                                  localDB.UpdateClientStatus(email, false);
                                  break;
                              }
                          }
                      }));
            });
            #endregion

            #region Receive Update for Friend

            // Receive update profile for friend
            Connection.On<UpdateUserProfileApiModel>("UpdateFriend", (friend) =>
            {
                for (int index = 0; index < ChatListViewModel.Items.Count; index++)
                {
                    if (friend.CurrentEmail.Equals(ChatListViewModel.Items[index].Email))
                    {
                        Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                          new Action(() =>
                          {
                              if (!string.IsNullOrWhiteSpace(friend.FirstName))
                              {
                                  ChatListViewModel.Items[index].FirstName = friend.FirstName;
                                  ChatListViewModel.Items[index].ProfilePictureRGB = GenerateRGBValues.Generate(friend.FirstName[0]);
                                  localDB.UpdateFriendFirstName(friend.CurrentEmail, friend.FirstName);
                              }

                              if (!string.IsNullOrWhiteSpace(friend.LastName))
                              {
                                  ChatListViewModel.Items[index].LastName = friend.LastName;
                                  localDB.UpdateFriendLastName(friend.CurrentEmail, friend.LastName);
                              }

                              if (!string.IsNullOrWhiteSpace(friend.NewEmail))
                              {
                                  ChatListViewModel.Items[index].Email = friend.NewEmail;
                                  localDB.UpdateFriendEmail(friend.CurrentEmail, friend.NewEmail);
                              }
                          }));

                        break;
                    }
                }
            });

            #endregion

        }
        #endregion

        #region Start/Stop Connection

        public static async Task<bool> Start()
        {
            try
            {
                await Connection.StartAsync();
                return true;
            }
            catch (Exception ex)
            {
                DialogWindow.Show(ex.Message, "Conectare esuata");
                return false;
            }
        }

        public static async Task Stop()
        {
            await Connection.StopAsync();
        }

        #endregion

        #region Calling Sending Methods

        public static async Task SendBy(MessageApiModel message)
        {
            await Connection.InvokeAsync("SendBy", message);
        }

        public static async Task SendFriendRequest(string Email)
        {
            await Connection.InvokeAsync("SendFriendRequest", Email);
        }

        public static async Task SendFriendResponse(string Email, bool Response)
        {
            await Connection.InvokeAsync("SendFriendResponse", Email + " " + Response.ToString());
        }

        public static async Task UpdateProfile(UpdateUserProfileApiModel user)
        {
            await Connection.InvokeAsync("UpdateUserProfile", user);
        }

        #endregion

    }
}
