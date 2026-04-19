using Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ViewModel
{
   public class MessagesManager:BaseDB
    {
        protected override BaseEntity NewEntity()
        {
            return new Message() as BaseEntity;
        }
        protected override BaseEntity CreateModel(BaseEntity entity)
        {
            Message message = (Message)entity;
            message.Id = int.Parse(reader["Id"].ToString());
            message.Text = reader["text"].ToString();
            message.Timestamp = DateTime.Parse(reader["Timestamp"].ToString());
            UserManager user_manager = new UserManager();
            // הבאת נתוני המשתמש ששלח את ההודעה לפי מזהה משתמש
            int userId = int.Parse(reader["Sender"].ToString());
            message.Sender = user_manager.SelectById(userId);
             userId = int.Parse(reader["Receiver"].ToString());
            message.Receiver = user_manager.SelectById(userId);

            return message;
        }
        #region Select Queires
        public List<Message> SelectAll()
        {
            command.CommandText = "SELECT * FROM MessageTable";
            return base.ExecuteCommand().Cast<Message>().ToList();
        }
        public Message SelectById(int id)
        {
            command.Parameters.Clear();
            command.CommandText = "SELECT * FROM MessageTable WHERE (ID = @Id)";
            command.Parameters.AddWithValue("@Id", id);
            var list = base.ExecuteCommand().Cast<Message>().ToList();
            return list.Count == 1 ? list[0] : null;
        }
        public List<Message> SelectByUser(User user1, User user2)
        {
            command.Parameters.Clear();
            command.CommandText = @"SELECT * FROM MessageTable
                            WHERE
                            (
                                (Sender = @User1 AND Receiver = @User2) OR
                                (Sender = @User2 AND Receiver = @User1)
                            )";
            command.Parameters.AddWithValue("@User1", user1.Id);
            command.Parameters.AddWithValue("@User2", user2.Id);
            return base.ExecuteCommand().Cast<Message>().ToList();
        }
        public List<Message> SearchTextInChat(User user1, User user2, string text)
        {
            command.Parameters.Clear();
            command.CommandText = @"SELECT * FROM MessageTable 
                            WHERE 
                            (
                                (Sender=@User1 AND Receiver=@User2) OR 
                                (Sender=@User2 AND Receiver=@User1)
                            )
                            AND Text LIKE @text";
            command.Parameters.AddWithValue("@User1", user1.Id);
            command.Parameters.AddWithValue("@User2", user2.Id);
            command.Parameters.AddWithValue("@text", "%" + text + "%");
            return base.ExecuteCommand().Cast<Message>().ToList();
        }
        #endregion
        #region Actions
        public int Insert(Message message)
        {
            command.Parameters.Clear();
            command.CommandText = "INSERT INTO [dbo].[MessageTable] ([Sender],[Receiver],[Text],[Timestamp])" +
                " VALUES (@Sender,@Receiver, @Text,@Timestamp);";
            command.Parameters.AddWithValue("@Sender", message.Sender.Id);
            command.Parameters.AddWithValue("@Receiver", message.Receiver.Id);
            command.Parameters.AddWithValue("@Text", message.Text);
            command.Parameters.AddWithValue("@Timestamp", message.Timestamp);
            return base.ExecuteChanges();
        }
        public int Delete(Message message)
        {
            command.Parameters.Clear();
            command.CommandText = "DELETE FROM tblMessages WHERE ([Id] = @Original_Id) ";
            command.Parameters.AddWithValue("@Original_Id", message.Id);
            return base.ExecuteChanges();
        }
        #endregion
    }
}
