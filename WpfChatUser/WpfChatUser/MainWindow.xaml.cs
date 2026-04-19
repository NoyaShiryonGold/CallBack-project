using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WpfChatUser.ChatServiceReference;

namespace WpfChatUser
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<Message> ChatMessages { get; set; } = new ObservableCollection<Message>();
        public User _currentUser { get; set; }
        private User _selectedRecipient;

        public MainWindow()
        {
            InitializeComponent();
            lstMessages.ItemsSource = ChatMessages;
            ChatClientManager.Instance.OnMessageReceived += OnMessageReceived;
            ChatClientManager.Instance.OnUserJoined += OnUserJoined;
            ChatClientManager.Instance.OnError += OnError;
            ChatClientManager.Instance.OnReconnected += OnReconnected;
        }

        // ===== לוגיקת כניסה =====

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            TryLogin();
        }

        private void txtUsername_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) TryLogin();
        }

        private async void TryLogin()
        {
            string username = txtUsername.Text.Trim();

            if (string.IsNullOrEmpty(username))
            {
                ShowLoginError("נא להזין שם משתמש.");
                return;
            }

            var users = ChatClientManager.Instance.GetUsers();
            _currentUser = users.Find(u => u.Username == username);

            if (_currentUser == null)
            {
                ShowLoginError($"המשתמש '{username}' לא נמצא.");
                return;
            }
            btnLogin.IsEnabled = false;
            btnLogin.Content = "מתחבר...";

            await Task.Run(() => ChatClientManager.Instance.Join(_currentUser.Username));

            users.Remove(_currentUser);
            lstUsers.ItemsSource = users;

            gridLogin.Visibility = Visibility.Collapsed;
            gridChat.Visibility = Visibility.Visible;
            Title = $"Chat — {_currentUser.Username}";

            this.DataContext = _currentUser;
        }

        private void ShowLoginError(string message)
        {
            lblError.Text = message;
            lblError.Visibility = Visibility.Visible;
        }

        // ===== לוגיקת צ'אט =====

        private void RefreshUserList()
        {
            var users = ChatClientManager.Instance.GetUsers();
            users.Remove(users.Find(u => u.Id == _currentUser.Id));
            lstUsers.ItemsSource = users;
        }

        private void OnMessageReceived(Message msg)
        {
            if (_selectedRecipient == null) return;

            bool isRelevant =
                (msg.Sender.Id == _currentUser.Id && msg.Receiver.Id == _selectedRecipient.Id) ||
                (msg.Sender.Id == _selectedRecipient.Id && msg.Receiver.Id == _currentUser.Id);

            if (isRelevant)
            {
                ChatMessages.Add(msg);
                lstMessages.ScrollIntoView(msg);
            }
        }

        private void OnUserJoined(string username)
        {
            RefreshUserList();
        }

        private void OnError(string errorMessage)
        {
            MessageBox.Show(errorMessage, "שגיאה", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void OnReconnected()
        {
            if (_currentUser != null)
                ChatClientManager.Instance.Join(_currentUser.Username);
        }

        private void lstUsers_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            _selectedRecipient = lstUsers.SelectedItem as User;
            if (_selectedRecipient == null) return;

            lblChatWith.Text = $"Chatting with: {_selectedRecipient.Username}";
            lblChatWith.Foreground = System.Windows.Media.Brushes.Black;

            var history = ChatClientManager.Instance.GetMessages(_currentUser,_selectedRecipient);
            ChatMessages.Clear();
            foreach (var m in history)
                ChatMessages.Add(m);
        }

        private void btnSend_Click(object sender, RoutedEventArgs e) => SendMessage();

        private void txtMessageInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) SendMessage();
        }

        private void SendMessage()
        {
            if (_selectedRecipient == null || string.IsNullOrWhiteSpace(txtMessageInput.Text)) return;

            var msg = new Message
            {
                Sender = _currentUser,
                Receiver = _selectedRecipient,
                Text = txtMessageInput.Text,
                Timestamp = System.DateTime.Now
            };

            ChatClientManager.Instance.SendMessage(msg);
            txtMessageInput.Clear();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (_currentUser != null)
                ChatClientManager.Instance.Leave(_currentUser.Username);
            base.OnClosing(e);
        }
    }
}