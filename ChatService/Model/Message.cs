using System;
using System.Runtime.Serialization;

namespace Model
{
    [DataContract]
    public class Message:BaseEntity
    {
        private User sender;
        private User receiver;
        private string text;
        private DateTime timestamp;

        [DataMember]
        public User Sender { get=>sender; set { sender=value; } }
        [DataMember]
        public User Receiver { get => receiver; set { receiver=value; } }

        [DataMember]
        public string Text { get => text; set { text=value; } }

        [DataMember]
        public DateTime Timestamp { get => timestamp; set { timestamp=value; } }
    }
}
