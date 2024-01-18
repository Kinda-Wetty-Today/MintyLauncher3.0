namespace MintyLauncherInstaller2._0.View.Windows
{
    public partial class InternetError : Window
    {
        public InternetError()
        {
            InitializeComponent();
        }
        private void Retry_Click(object sender, RoutedEventArgs e)
        {
            if (IsInternetAvailable())
            {
                StartUpWindow StartUpWindow = new StartUpWindow();
                StartUpWindow.Show();
                this.Close();
            }
            else
            {
                new MessageBox("Internet is still not available. Please check your connection.", MessageType.Error, MessageButtons.Ok).ShowDialog();
            }
        }
        public static bool IsInternetAvailable()
        {
            try
            {
                using (Ping ping = new Ping())
                {
                    PingReply reply = ping.Send("github.com", 3000);
                    return reply != null && reply.Status == IPStatus.Success;
                }
            }
            catch (PingException)
            {
                return false;
            }
        }
    }
}
