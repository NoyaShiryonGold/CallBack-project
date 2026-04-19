using ChatServiceReference;
using MauiChatUser.Services;
using Microsoft.Maui.Controls.Shapes;

namespace MauiChatUser
{
    public partial class MainPage : ContentPage
    {
        private User _selectedUser;
        private User _currentUser;
        private List<User> _users;

        public MainPage()
        {
            InitializeComponent();

            ServiceHelper.Instance.OnMessageReceived += (msg) =>
            {
                MainThread.BeginInvokeOnMainThread(() => AddMessageToUI(msg));
            };
        }

        // --- לוגיקת כניסה (LOGIN) ---
        private async void OnLoginClicked(object sender, EventArgs e)
        {
            string typedName = txtUsername.Text?.Trim();

            if (string.IsNullOrEmpty(typedName))
            {
                lblError.Text = "Please enter a username";
                lblError.IsVisible = true;
                return;
            }

            try
            {
                // 1. ננסה להביא את רשימת המשתמשים מהשרת
                var allUsers = await ServiceHelper.Instance.ChatService.MauiGetUsersAsync();

                // 2. נחפש את המשתמש שהקלידו
                _currentUser = allUsers.FirstOrDefault(u => u.Username.Equals(typedName, StringComparison.OrdinalIgnoreCase));

                if (_currentUser == null)
                {
                    lblError.Text = "User not found on server";
                    lblError.IsVisible = true;
                    return;
                }

                // 3. שמירת המשתמש ב-SharedData (כפי שמופיע בקוד השליחה שלך)
                SharedData.Instance.CurrentUser = _currentUser;

                // 4. הכנת רשימת המשתמשים האחרים לצ'אט
                _users = allUsers;
                _users.Remove(_currentUser);
                lstUsers.ItemsSource = _users;

                // 5. מעבר ויזואלי למסך הצ'אט
                gridLogin.IsVisible = false;
                gridMainChat.IsVisible = true;
            }
            catch (Exception ex)
            {
                await DisplayAlertAsync("Error", "Connection failed: " + ex.Message, "OK");
            }
        }

        private async void OnUserSelected(object sender, SelectionChangedEventArgs e)
        {
            _selectedUser = e.CurrentSelection.FirstOrDefault() as User;
            if (_selectedUser == null) return;

            // החלפת תצוגה פנימית (מרשימה לצ'אט ספציפי)
            lstUsers.IsVisible = false;
            scrollChat.IsVisible = true;
            gridInput.IsVisible = true;
            btnBack.IsVisible = true;
            lblChatTitle.Text = $"Chat with {_selectedUser.Username}";

            // טעינת היסטוריה
            stackMessages.Children.Clear();
            var history = await ServiceHelper.Instance.ChatService.MauiGetMessagesAsync(_currentUser, _selectedUser);
            if (history != null)
            {
                foreach (var msg in history) AddMessageToUI(msg);
            }
        }

        private void OnSendClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMessage.Text) || _selectedUser == null) return;

            var msg = new Message
            {
                Sender = SharedData.Instance.CurrentUser,
                Receiver = _selectedUser,
                Text = txtMessage.Text,
                Timestamp = DateTime.Now
            };

            ServiceHelper.Instance.ChatService.MauiSendMessageAsync(msg);
            txtMessage.Text = string.Empty;
        }

        private void AddMessageToUI(Message msg)
        {
            // בדיקה אם ההודעה שייכת לצ'אט הפתוח כרגע
            if (_selectedUser == null) return;

            bool isFromSelected = msg.Sender.Id == _selectedUser.Id;
            bool isFromMe = msg.Sender.Id == SharedData.Instance.CurrentUser.Id;

            if (!isFromSelected && !isFromMe) return;

            Border border = new Border
            {
                Content = new Label { Text = msg.Text, TextColor = Colors.Black },
                Background = isFromMe ? Color.FromArgb("#DCF8C6") : Colors.White,
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(10) },
                HorizontalOptions = isFromMe ? LayoutOptions.End : LayoutOptions.Start,
                Padding = 8,
                Margin = new Thickness(isFromMe ? 50 : 0, 2, isFromMe ? 0 : 50, 2)
            };

            stackMessages.Children.Add(border);

            // גלילה לסוף
            MainThread.BeginInvokeOnMainThread(async () => {
                await Task.Delay(100); // עיכוב קטן כדי שה-Layout יתעדכן
                await scrollChat.ScrollToAsync(0, stackMessages.Height, true);
            });
        }

        private void OnBackClicked(object sender, EventArgs e)
        {
            lstUsers.IsVisible = true;
            scrollChat.IsVisible = false;
            gridInput.IsVisible = false;
            btnBack.IsVisible = false;
            lblChatTitle.Text = "Select a user";
            _selectedUser = null;
            lstUsers.SelectedItem = null;
        }
    }
}