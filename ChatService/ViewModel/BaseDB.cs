using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ViewModel
{
    public abstract class BaseDB
    {

        protected string connectionString;
        protected SqlConnection connection;
        protected SqlCommand command;
        protected SqlDataReader reader;

        protected abstract BaseEntity NewEntity();
        protected abstract BaseEntity CreateModel(BaseEntity entity);

        public BaseDB()
        {
            if (connectionString == string.Empty || connectionString==null)
                connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" +
                    Path() + @"\MyChatDataBase.mdf;Integrated Security=True";
            connection = new SqlConnection(connectionString);
            command = new SqlCommand();
            command.Connection = connection;
        }

        public List<BaseEntity> ExecuteCommand()
        {
            List<BaseEntity> list = new List<BaseEntity>();
            try
            {
                connection.Open();
                reader = command.ExecuteReader(); 
                while (reader.Read()) 
                {
                    BaseEntity entity = NewEntity();
                    list.Add(CreateModel(entity)); 
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
            return list;
        }
        public int ExecuteChanges() //ביצוע שינויים במסד
        {
            int records = 0;
            try
            {
                connection.Open(); //פתיחת תקשורת עם המסד
                records = command.ExecuteNonQuery(); //ביצוע שאילתת פעולה
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message + "\n" + command.CommandText);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
            return records;
        }
        private static string Path()
        {
            string s = Environment.CurrentDirectory; //המיקום שבו רץ הפרויקט
            string[] sub = s.Split('\\'); //פירוק מחרוזת הכתובת למערך לפי תיקיות

            int index = sub.Length - 3; //חזרה אחורה 3 תיקיות
            sub[index] = "ViewModel";     //שינוי התיקיה לתיקיה המתאימה
            Array.Resize(ref sub, index + 1); //תיקון של אורך המערך, לאורך המתאים לתיקייה

            s = String.Join("\\", sub);  //חיבור מחדש של המערך עם / מפריד אישי 
            return s;
        }
    }
}
