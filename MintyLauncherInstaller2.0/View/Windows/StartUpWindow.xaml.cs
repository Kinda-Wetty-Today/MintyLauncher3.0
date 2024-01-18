namespace MintyLauncherInstaller2._0.View.Windows
{
    public partial class StartUpWindow : Window
    {
        public StartUpWindow()
        {
            InitializeComponent();
            InitializeWindowAsync();
        }
        private async void InitializeWindowAsync()
        {
            bool isInternetAvailable = await IsInternetAvailable();

            if (isInternetAvailable)
            {
                Loaded();
            }
            else
            {
                InternetError InternetError = new InternetError();
                InternetError.Show();
                this.Close();
            }
        }

        public async Task<bool> IsInternetAvailable()
        {
            return await Task.Run(() => IsInternetAvailableBool());
        }

        public static bool IsInternetAvailableBool()
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
        //Metods
        #region
        //CheckVerMetods
        #region
        private new async void Loaded()
        {
            Random random = new Random();
            int token = random.Next(1, 3);
            string? accessToken = null;
            if (token == 1) { accessToken = "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"; }
            else if (token == 2) { accessToken = "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"; }
            string owner = "Kinda-Wetty-Today";
            string repositoryName = "Minty-Launcher3.0-Releases";
            var client = new GitHubClient(new Octokit.ProductHeaderValue("Launcher"));
            var tokenAuth = new Credentials(accessToken);
            client.Credentials = tokenAuth;

            var releases = await client.Repository.Release.GetAll(owner, repositoryName);
            Release? latestRelease = releases[0];
            string MainFolderPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory);
            string LauncherFolderPath = System.IO.Path.Combine(MainFolderPath, "Launcher3.0");
            string LauncherFilePath = System.IO.Path.Combine(LauncherFolderPath, "MintyLauncher3.0.exe");
            string verFilePath = Path.Combine(LauncherFolderPath, "LauncherVer");


            if (latestRelease == null)
            {
                new MessageBox("Unable to fetch the latest release.", MessageType.Error, MessageButtons.Ok).ShowDialog();
                return;
            }


            if (latestRelease.Assets.Count == 0)
            {
                new MessageBox("Minty.zip not found. The file name may not match.", MessageType.Error, MessageButtons.Ok).ShowDialog();
                return;
            }
            if (!File.Exists(verFilePath))
            {
                mainFrame.Navigate(new Install());
            }
            else
            {
                string verText = await File.ReadAllTextAsync(verFilePath);
                Version? localVersion;

                if (!Version.TryParse(verText, out localVersion))
                {
                    new MessageBox($"Incorrect version format in local file: {verText}", MessageType.Error, MessageButtons.Ok).ShowDialog();
                    return;
                }

                string githubVersionTag = latestRelease.TagName;
                Version? githubVersion;

                if (!Version.TryParse(githubVersionTag, out githubVersion))
                {
                    new MessageBox($"Incorrect version format on GitHub: {githubVersionTag}", MessageType.Error, MessageButtons.Ok).ShowDialog();
                    return;
                }

                if (localVersion >= githubVersion)
                {
                    LaunchExecutable(LauncherFilePath);
                    Environment.Exit(0);
                    return;
                }

                if (latestRelease.Assets.Count == 0)
                {
                    new MessageBox("Minty.zip not found. The file name may not match.", MessageType.Error, MessageButtons.Ok).ShowDialog();
                    return;
                }
                mainFrame.Navigate(new Update());
            }
        }
        #endregion
        //Launch
        #region
        private void LaunchExecutable(string exePath)
        {
            try
            {
                Process.Start(exePath);
            }
            catch (Exception ex)
            {
                new MessageBox($"Error launching executable: {ex.Message}", MessageType.Error, MessageButtons.Ok).ShowDialog();
            }
        }
        #endregion
        #endregion
    }
}
