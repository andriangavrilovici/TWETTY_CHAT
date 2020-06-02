using TWETTY_CHAT.Core;
using System;
using System.Data.SQLite;

namespace TWETTY_CHAT
{
    public class LocalDB
    {
        public SQLiteConnection Connect { get; set; } = new SQLiteConnection(appsettings.ConnectionClientDataBase);
        public SQLiteDataReader sqlite_datareader { get; set; }
        public SQLiteCommand sqlite_cmd { get; set; }

        public LocalDB()
        {
            Connect = new SQLiteConnection(appsettings.ConnectionClientDataBase);
            Connect.Open();
        }

        public void GetFriendsDetails()
        {
            sqlite_cmd = Connect.CreateCommand();

            sqlite_cmd.CommandText = "SELECT * FROM [ClientFriends]";

            sqlite_datareader = sqlite_cmd.ExecuteReader();
        }

        public void UpdateClientStatus(string Email, bool Status)
        {
            try
            {
                sqlite_cmd = Connect.CreateCommand();

                sqlite_cmd.CommandText = "UPDATE [ClientFriends] SET [Status] = @Status " +
                    "WHERE [Email] = @Email";

                sqlite_cmd.Parameters.AddWithValue("@Email", Email);

                sqlite_cmd.Parameters.Add("@Status", System.Data.DbType.Boolean).Value = Status;

                sqlite_datareader = sqlite_cmd.ExecuteReader();
            }
            catch (Exception ex)
            {
                DialogWindow.Show(ex.Message, "Actualizare locala esuata");
            }

        }

        public void UpdateFriendEmail(string CurrentEmail, string NewEmail)
        {
            try
            {
                sqlite_cmd = Connect.CreateCommand();

                sqlite_cmd.CommandText = "UPDATE [ClientFriends] SET [Email] = @NewEmail " +
                    "WHERE [Email] = @CurrentEmail";

                sqlite_cmd.Parameters.AddWithValue("@CurrentEmail", CurrentEmail);

                sqlite_cmd.Parameters.AddWithValue("@NewEmail", NewEmail);

                sqlite_datareader = sqlite_cmd.ExecuteReader();

                UpdateEmailInMessages(CurrentEmail, NewEmail);
            }
            catch (Exception ex)
            {
                DialogWindow.Show(ex.Message, "Actualizare locala esuata");
            }
        }
        public void UpdateFriendFirstName(string Email, string FirstName)
        {
            try
            {
                sqlite_cmd = Connect.CreateCommand();

                sqlite_cmd.CommandText = "UPDATE [ClientFriends] SET [FirstName] = @FirstName " +
                    "WHERE [Email] = @Email";

                sqlite_cmd.Parameters.AddWithValue("@Email", Email);

                sqlite_cmd.Parameters.AddWithValue("@FirstName", FirstName);

                sqlite_datareader = sqlite_cmd.ExecuteReader();
            }
            catch (Exception ex)
            {
                DialogWindow.Show(ex.Message, "Actualizare locala esuata");
            }
        }
        public void UpdateFriendLastName(string Email, string LastName)
        {
            try
            {
                sqlite_cmd = Connect.CreateCommand();

                sqlite_cmd.CommandText = "UPDATE [ClientFriends] SET [LastName] = @LastName " +
                    "WHERE [Email] = @Email";

                sqlite_cmd.Parameters.AddWithValue("@Email", Email);

                sqlite_cmd.Parameters.AddWithValue("@LastName", LastName);

                sqlite_datareader = sqlite_cmd.ExecuteReader();
            }
            catch (Exception ex)
            {
                DialogWindow.Show(ex.Message, "Actualizare locala esuata");
            }
        }

        public void UpdateEmailInMessages(string CurrentEmail, String NewEmail)
        {
            try
            {
                sqlite_cmd = Connect.CreateCommand();

                sqlite_cmd.CommandText = "UPDATE [ClientMessages] SET [SendBy_Email] = @NewEmail " +
                    "WHERE [SendBy_Email] = @CurrentEmail";

                sqlite_cmd.Parameters.AddWithValue("@CurrentEmail", CurrentEmail);

                sqlite_cmd.Parameters.AddWithValue("@NewEmail", NewEmail);

                sqlite_datareader = sqlite_cmd.ExecuteReader();

                sqlite_cmd = Connect.CreateCommand();

                sqlite_cmd.CommandText = "UPDATE [ClientMessages] SET [SendTo_Email] = @NewEmail " +
                    "WHERE [SendTo_Email] = @CurrentEmail";

                sqlite_cmd.Parameters.AddWithValue("@CurrentEmail", CurrentEmail);

                sqlite_cmd.Parameters.AddWithValue("@NewEmail", NewEmail);

                sqlite_datareader = sqlite_cmd.ExecuteReader();

            }
            catch (Exception ex)
            {
                DialogWindow.Show(ex.Message, "Actualizare locala esuata");
            }

        }

        public void GetMessageDetails(string Email = null)
        {
            sqlite_cmd = Connect.CreateCommand();

            sqlite_cmd.CommandText = "SELECT [ClientFriends].FirstName, " +
                                    "[ClientFriends].LastName, " +
                                    "[ClientMessages].Message, " +
                                    "[ClientMessages].SendBy_Email, " +
                                    "[ClientMessages].SendTo_Email, " +
                                    "[ClientMessages].MessageSentTime " +
                                    "FROM [ClientMessages] INNER JOIN [ClientFriends] ON ([ClientFriends].Email = [ClientMessages].SendBy_Email OR [ClientFriends].Email = [ClientMessages].SendTo_Email) ";

            if (!string.IsNullOrWhiteSpace(Email))
            {
                sqlite_cmd.Parameters.AddWithValue("@Email", Email);
                sqlite_cmd.CommandText += " WHERE @Email = [ClientMessages].SendBy_Email OR @Email = [ClientMessages].SendTo_Email ";
            }
            sqlite_cmd.CommandText += "ORDER BY [ClientMessages].MessageSentTime ASC;";

            sqlite_datareader = sqlite_cmd.ExecuteReader();
        }

        public void InsertMessage(string PendingMessageText, string SendBy_Email, string SendTo_Email, DateTimeOffset dateTime)
        {
            string InsertNewMessage = "INSERT INTO [ClientMessages] ([Message],[SendBy_Email], [SendTo_Email], [MessageSentTime]) " +
                        "VALUES(@message, @SendBy_Email, @SendTo_Email, @dateTime);";

            SQLiteCommand Command = new SQLiteCommand(InsertNewMessage, Connect);

            Command.Parameters.AddWithValue("@message", PendingMessageText);
            Command.Parameters.AddWithValue("@SendBy_Email", SendBy_Email);
            Command.Parameters.AddWithValue("@SendTo_Email", SendTo_Email);
            Command.Parameters.AddWithValue("@dateTime", dateTime);

            Command.ExecuteNonQuery();
        }

        public void InsertFriends(FriendApiModel friend)
        {
            string InsertNewMessage = "INSERT INTO [ClientFriends] ([Email], [FirstName], [LastName], [Status]) " +
                        "VALUES(@Email, @FirstName, @LastName, @Status);";

            SQLiteCommand Command = new SQLiteCommand(InsertNewMessage, Connect);

            Command.Parameters.AddWithValue("@Email", friend.Email);
            Command.Parameters.AddWithValue("@FirstName", friend.FirstName);
            Command.Parameters.AddWithValue("@LastName", friend.LastName);
            Command.Parameters.AddWithValue("@Status", friend.Status);

            Command.ExecuteNonQuery();
        }

        public void DeleteLoginCredentials()
        {
            SQLiteCommand Command = new SQLiteCommand("DELETE FROM [LoginCredentials];", Connect);
            Command.ExecuteNonQuery();
        }
        public void DeleteClientFriends()
        {
            SQLiteCommand Command = new SQLiteCommand("DELETE FROM [ClientFriends];", Connect);
            Command.ExecuteNonQuery();
        }
        public void DeleteClientMessages()
        {
            SQLiteCommand Command = new SQLiteCommand("DELETE FROM [ClientMessages];", Connect);
            Command.ExecuteNonQuery();
        }

    }
}
