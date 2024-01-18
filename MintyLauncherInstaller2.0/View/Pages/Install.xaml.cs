namespace MintyLauncherInstaller2._0.View.Pages
{
    public partial class Install : Page
    {
        public Install()
        {
            InitializeComponent();
        }
        //Launch
        #region
        private async void Install_Click(object sender, RoutedEventArgs e)
        {
            Button.IsEnabled = false;
            Button.Content = "Downloading";
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
            string LauncherZipFilePath = System.IO.Path.Combine(LauncherFolderPath, "Launcher.zip");
            string verFilePath = Path.Combine(LauncherFolderPath, "LauncherVer");

            if (latestRelease == null)
            {
                new MessageBox("Unable to fetch the latest release.", MessageType.Error, MessageButtons.Ok).ShowDialog();
                return;
            }
            string latestReleaseTag = latestRelease.TagName;

            if (latestRelease.Assets.Count == 0)
            {
                new MessageBox("Minty.zip not found. The file name may not match.", MessageType.Error, MessageButtons.Ok).ShowDialog();
                return;
            }
            var asset = latestRelease.Assets[0];
            string downloadUrl = asset.BrowserDownloadUrl;
            Directory.CreateDirectory(LauncherFolderPath);
            using (StreamWriter writer = new StreamWriter(verFilePath))
            {
                await writer.WriteLineAsync(latestReleaseTag);
            }

            bool downloadSuccess = await DownloadFilesAsync(downloadUrl, LauncherZipFilePath, LauncherFolderPath);
            using (StreamWriter writer = new StreamWriter(verFilePath))
            {
                await writer.WriteLineAsync(latestReleaseTag);
            }
            if (downloadSuccess)
            {
                LaunchExecutable(LauncherFilePath);
                Environment.Exit(0);
            }
            else
            {
                new MessageBox("Failed to download Minty.zip.", MessageType.Error, MessageButtons.Ok).ShowDialog();
            }
        }
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
        //Download and Extract
        #region
        public async Task<bool> DownloadFilesAsync(string downloadUrl, string zipFilePath, string assetsFolderPath)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    async Task WriteDownloadedBytesToDisk(byte[] content, string filePath)
                    {
                        await File.WriteAllBytesAsync(filePath, content);
                    }
                    using (var downloadTask = httpClient.GetByteArrayAsync(downloadUrl))
                    {
                        var tasks = new[] { downloadTask };
                        await Task.WhenAll(tasks);
                        foreach (var task in tasks)
                        {
                            task.Dispose();
                        }
                        await WriteDownloadedBytesToDisk(downloadTask.Result, zipFilePath);
                    }
                }

                await ExtractZipFile(zipFilePath, assetsFolderPath);
                File.Delete(zipFilePath);
                return true;
            }
            catch (HttpRequestException ex)
            {
                new MessageBox($"Error downloading file: {ex.Message}", MessageType.Error, MessageButtons.Ok).ShowDialog();
            }
            catch (IOException ex)
            {
                new MessageBox($"Error saving file: {ex.Message}", MessageType.Error, MessageButtons.Ok).ShowDialog();
            }
            catch (Exception ex)
            {
                new MessageBox($"An unexpected error occurred: {ex.Message}", MessageType.Error, MessageButtons.Ok).ShowDialog();
            }

            return false;
        }

        private async Task ExtractZipFile(string zipFilePath, string extractionPath)
        {
            try
            {
                await Task.Run(() =>
                {
                    ZipFile.ExtractToDirectory(zipFilePath, extractionPath);
                });

            }
            catch (Exception ex)
            {
                new MessageBox($"Error while extracting the archive: {ex.Message}", MessageType.Error, MessageButtons.Ok).ShowDialog();
            }
        }
        #endregion
    }
}

