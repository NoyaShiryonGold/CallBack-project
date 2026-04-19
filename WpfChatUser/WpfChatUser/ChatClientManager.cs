using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows;
using WpfChatUser.ChatServiceReference;

namespace WpfChatUser
{
    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public class ChatClientManager : IChatServiceCallback
    {
        private static readonly object _lock = new object();
        private static ChatClientManager _instance;
        private ChatServiceClient _client;

        public event Action<Message> OnMessageReceived;
        public event Action<string> OnUserJoined;
        public event Action<string> OnError;
        public event Action OnReconnected;

        private ChatClientManager() => InitializeClient();

        public static ChatClientManager Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                        _instance = new ChatClientManager();
                    return _instance;
                }
            }
        }

        private void InitializeClient()
        {
            InstanceContext context = new InstanceContext(this);
            _client = new ChatServiceClient(context);
            _client.InnerChannel.Faulted += (s, e) =>
            {
                _client.Abort();
                Application.Current.Dispatcher.Invoke(() =>
                {
                    InitializeClient();
                    OnReconnected?.Invoke();
                });
            };
        }

        #region מימוש IChatServiceCallback

        public void ReceiveMessage(Message message)
        {
            Application.Current.Dispatcher.Invoke(() => OnMessageReceived?.Invoke(message));
        }

        public void UserJoined(string username)
        {
            Application.Current.Dispatcher.Invoke(() => OnUserJoined?.Invoke(username));
        }

        #endregion

        #region Proxy Methods

        public void Join(string username)
        {
            try { _client.Join(username); }
            catch (Exception ex) { OnError?.Invoke($"שגיאה בחיבור: {ex.Message}"); }
        }
        public void Leave(string username)
        {
            try
            {
                _client.Leave(username);
                _client.Close();
            }
            catch { _client.Abort(); }
        }

        public async void SendMessage(Message msg)
        {
            try
            { // שליחה ברקע כדי לא לחסום את ה-UI
                await Task.Run(() => _client.SendMessage(msg));
            }
            catch (Exception ex) { OnError?.Invoke($"שגיאה בשליחה: {ex.Message}"); }
        }

        public List<User> GetUsers()
        {
            try { return new List<User>(_client.GetUsers()); }
            catch (Exception ex) { OnError?.Invoke(ex.Message); return new List<User>(); }
        }

        public List<Message> GetMessages(User currentUser, User user)
        {
            try { return new List<Message>(_client.GetMessages(currentUser, user)); }
            catch (Exception ex) { OnError?.Invoke(ex.Message); return new List<Message>(); }
        }

        #endregion
    }
}