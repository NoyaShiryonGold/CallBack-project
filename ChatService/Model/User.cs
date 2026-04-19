using System.Runtime.Serialization;

namespace Model
{
    [DataContract]
    public class User:BaseEntity
    {
        private string username;

        [DataMember]
        public string Username
        {
            get { return username; }
            set { username = value; }
        }
    }
}
