using ChatServiceReference;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MauiChatUser.Services
{
    internal class SharedData
    {
        private static readonly Lazy<SharedData> _instance = new Lazy<SharedData>(() => new SharedData());
        public static SharedData Instance => _instance.Value;

        public User CurrentUser { get; set; }

        // מילון ששומר הודעות לפי ה-ID של איש הקשר (שיחות)
        public Dictionary<int, ObservableCollection<Message>> ChatHistory { get; set; }

        private SharedData()
        {
            ChatHistory = new Dictionary<int, ObservableCollection<Message>>();
        }

        public void AddMessage(Message msg)
        {
            // קביעת המפתח (הצד השני בשיחה)
            int contactId = (msg.Sender.Id == CurrentUser.Id) ? msg.Receiver.Id : msg.Sender.Id;

            if (!ChatHistory.ContainsKey(contactId))
                ChatHistory[contactId] = new ObservableCollection<Message>();

            ChatHistory[contactId].Add(msg);
        }

        public void Clear()
        {
            CurrentUser = null;
            ChatHistory.Clear();
        }
    }
}
