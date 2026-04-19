using Model;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using ViewModel;
using Microsoft.AspNet.SignalR;

namespace WcfService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class ChatService : IChatService, IMauiChatService
    {
        private readonly object _syncRoot = new object();
        private Dictionary<string, IChatCallback> clients = new Dictionary<string, IChatCallback>();
        private MessagesManager msgManager = new MessagesManager();
        private UserManager userManager = new UserManager();

        
        public void Join(string user)
        {
            var callback = OperationContext.Current.GetCallbackChannel<IChatCallback>();
            lock (_syncRoot)
            {
                if (clients.ContainsKey(user))
                    clients.Remove(user);
                clients.Add(user, callback);
            }
            GetSignalRContext().Clients.All.userJoined(user);

            BroadcastUserJoined(user);
        }

        public void Leave(string user)
        {
            lock (_syncRoot)
            {
                if (clients.ContainsKey(user))
                    clients.Remove(user);
            }
            BroadcastUserJoined(user);
        }

        public void SendMessage(Message msg)
        {
            msgManager.Insert(msg);
            SendToWcfClients(msg);
            try
            {
                var context = GetSignalRContext();
                context.Clients.All.receiveMessage(msg);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("SignalR Broadcast failed: " + ex.Message);
            }
        }
        private void SendToWcfClients(Message msg)
        {
            List<KeyValuePair<string, IChatCallback>> snapshot;
            lock (_syncRoot)
            {
                snapshot = new List<KeyValuePair<string, IChatCallback>>(clients);
            }

            List<string> disconnected = new List<string>();
            foreach (var client in snapshot)
            {
                try
                {
                    if (((ICommunicationObject)client.Value).State == CommunicationState.Opened)
                        client.Value.ReceiveMessage(msg);
                    else
                        disconnected.Add(client.Key);
                }
                catch
                {
                    disconnected.Add(client.Key);
                }
            }

            if (disconnected.Count > 0)
            {
                lock (_syncRoot)
                {
                    foreach (var user in disconnected)
                        clients.Remove(user);
                }
            }
        }

        private void BroadcastUserJoined(string user)
        {
            List<IChatCallback> snapshot;
            lock (_syncRoot)
            {
                snapshot = new List<IChatCallback>(clients.Values);
            }

            foreach (var client in snapshot)
            {
                try { client.UserJoined(user); } catch { }
            }
        }

        public List<User> GetUsers()
        {
            return userManager.SelectAll();
        }

        public List<Message> GetMessages(User user1, User user2) => msgManager.SelectByUser(user1, user2);

        void IMauiChatService.Join(string user) => this.Join(user);
        void IMauiChatService.SendMessage(Message msg) => this.SendMessage(msg);
        void IMauiChatService.Leave(string user) => this.Leave(user);
        List<User> IMauiChatService.GetUsers() => this.GetUsers();
        List<Message> IMauiChatService.GetMessages(User user1, User user2) => this.GetMessages(user1, user2);
        private IHubContext GetSignalRContext()
        {
            return GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
        }
    }
}