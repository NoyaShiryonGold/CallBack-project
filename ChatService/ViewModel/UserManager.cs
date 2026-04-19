using Model;
using System.Collections.Generic;
using System.Linq;

namespace ViewModel
{
    public class UserManager : BaseDB
    {
        protected override BaseEntity NewEntity()
        {
            return new User() as BaseEntity;
        }
        protected override BaseEntity CreateModel(BaseEntity entity)
        {
            User user = (User)entity;
            user.Id = int.Parse(reader["Id"].ToString());
            user.Username = reader["Username"].ToString();
            return user;
        }
        #region SELECT Queries
        public List<User> SelectAll()
        {
            command.CommandText = "SELECT * FROM UserTable";
            return base.ExecuteCommand().Cast<User>().ToList();
        }
        public User SelectById(int id)
        {
            command.Parameters.Clear();
            command.CommandText =string.Format("SELECT * FROM UserTable WHERE (ID = {0})", id) ;
            var list = base.ExecuteCommand().Cast<User>().ToList();
            return list.Count == 1 ? list[0] : null;
        }
        public List<User> SearchByText(string text)
        {
            command.Parameters.Clear();
            command.CommandText =string.Format(" SELECT * FROM UserTable WHERE (Username LIKE Concat('%'))");
            command.Parameters.AddWithValue("@text",text);
            return base.ExecuteCommand().Cast<User>().ToList();
        }
        #endregion
        #region Actions
        public int Insert(User user)
        {
            command.Parameters.Clear();
            command.CommandText = "INSERT INTO [dbo].[UserTable] ([Username]) VALUES (@Username);";
            command.Parameters.AddWithValue("@Username", user.Username);
            return base.ExecuteChanges();
        }
        public int Update(User user)
        {
            command.Parameters.Clear();
            command.CommandText = "UPDATE [dbo].[UserTable] SET [Username] = @Username WHERE ([Id] = @Original_Id) ";
            command.Parameters.AddWithValue("@Username", user.Username);
            command.Parameters.AddWithValue("@Original_Id", user.Id);
            return base.ExecuteChanges();
        }
        public int Delete(User user)
        {
            command.Parameters.Clear();
            command.CommandText = "DELETE FROM [dbo].[UserTable] WHERE ([Id] = @Original_Id)";
            command.Parameters.AddWithValue("@Original_Id", user.Id);
            return base.ExecuteChanges();
        }
        #endregion
    }
}
