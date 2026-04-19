using System.ServiceModel;
using ChatServiceReference;
using Microsoft.AspNet.SignalR.Client; // שימוש בלקוח הישן
using Message = ChatServiceReference.Message;

namespace MauiChatUser.Services
{
    internal class ServiceHelper
    {
        private static readonly Lazy<ServiceHelper> _instance = new Lazy<ServiceHelper>(() => new ServiceHelper());
        public static ServiceHelper Instance => _instance.Value;

        private IMauiChatService _chatService;
        public event Action<Message> OnMessageReceived;

        // הגדרות SignalR (גרסה ישנה)
        private HubConnection _hubConnection;
        private IHubProxy _chatHubProxy;

        private readonly string _signalrAddress = "https://l3ghdvzd-8080.euw.devtunnels.ms/signalr";
        private readonly string _wcfAddress = "https://l3ghdvzd-8733.euw.devtunnels.ms/Design_Time_Addresses/WcfService/ChatService/maui";

        private ServiceHelper()
        {
            InitializeSignalR();
        }

        public IMauiChatService ChatService
        {
            get
            {
                if (_chatService == null)
                    _chatService = CreateChatServiceClient();
                return _chatService;
            }
        }

        private IMauiChatService CreateChatServiceClient()
        {
            var binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
            var endpoint = new EndpointAddress(_wcfAddress);
            var factory = new ChannelFactory<IMauiChatService>(binding, endpoint);
            return factory.CreateChannel();
        }

        private async void InitializeSignalR()
        {
            if (_hubConnection != null) return;

            // בגרסה הישנה, הכתובת צריכה להיות ה-URL של השרת בלבד (הסיומת /signalr מתווספת אוטומטית לעיתים, תלוי בקינפוג)
            // אם לא עובד, נסה להוריד את ה- /signalr מהכתובת
            _hubConnection = new HubConnection("https://l3ghdvzd-8080.euw.devtunnels.ms/");

            // יצירת Proxy ל-Hub (השם חייב להתאים בדיוק ל-ChatHub בשרת)
            _chatHubProxy = _hubConnection.CreateHubProxy("ChatHub");

            // רישום לאירוע קבלת הודעה
            _chatHubProxy.On<Message>("receiveMessage", (message) =>
            {
                ReceiveMessage(message);
            });

            try
            {
                await _hubConnection.Start();
                Console.WriteLine("SignalR Connected Successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SignalR Connection Error: {ex.Message}");
            }
        }

        public void ReceiveMessage(Message message)
        {
            if (message == null) return;

            MainThread.BeginInvokeOnMainThread(() =>
            {
                OnMessageReceived?.Invoke(message);
                SharedData.Instance.AddMessage(message);
            });
        }
    }
}