using Microsoft.Owin.Hosting;
using Owin;
using System;
using System.ServiceModel;
using System.Windows;
using WcfService;

namespace WpfHost
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ServiceHost host;
        IDisposable signalrServer;
        public MainWindow()
        {
            InitializeComponent();
            host = new ServiceHost(typeof(ChatService));
            host.Open();

            string signalrUrl = "http://localhost:8080";
            signalrServer = WebApp.Start<Startup>(signalrUrl);

            Console.WriteLine($"SignalR Server running on {signalrUrl}");
        }

        protected override void OnClosed(EventArgs e)
        {
            signalrServer?.Dispose();
            host?.Close();
            base.OnClosed(e);
        }
    }
}
