﻿using System;
using System.Diagnostics;
using System.IO.Compression;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Path = System.IO.Path;
using Octokit;
using DiscordRPC;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace MintyLauncher3._0.View.Pages.Main
{
    public partial class Genshin_Impact : System.Windows.Controls.Page
    {
        public Genshin_Impact()
        {
            InitializeComponent();
        }
        //metods
        #region
        //launch metod and click
        #region
        public async void Launch(object sender, RoutedEventArgs e)
        {
            Random random = new Random();
            int token = random.Next(1, 3);
            string? accessToken = null;
            if (token == 1) { accessToken = "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"; }
            else if (token == 2) { accessToken = "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"; }
            string owner = "kindawindytoday";
            string repositoryName = "Minty-Releases";
            var client = new GitHubClient(new Octokit.ProductHeaderValue("Launcher"));
            var tokenAuth = new Credentials(accessToken);
            client.Credentials = tokenAuth;

            var releases = await client.Repository.Release.GetAll(owner, repositoryName);
            Release? latestRelease = releases[0];


            if (latestRelease == null)
            {
                new MessageBox("Unable to fetch the latest release.", MessageType.Error, MessageButtons.Ok).ShowDialog();
                return;
            }

            string appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string mintyFolderPath = System.IO.Path.Combine(appDataFolder, "Minty");
            string assetsFolderPath = Path.Combine(mintyFolderPath, "MintyGI");
            string launcherFilePath = Path.Combine(assetsFolderPath, "Launcher.exe");
            string dllFilePath = Path.Combine(assetsFolderPath, "minty.dll");
            string zipFilePath = Path.Combine(assetsFolderPath, "minty.zip");
            string verFilePath = Path.Combine(assetsFolderPath, "VerGI");
            string latestReleaseTag = latestRelease.TagName;
            GI_button.IsEnabled = false;

            if (!File.Exists(verFilePath))
            {
                if (latestRelease.Assets.Count == 0)
                {
                    new MessageBox("Minty.zip not found.The file name may not match.", MessageType.Error, MessageButtons.Ok).ShowDialog();
                    return;
                }
                var asset = latestRelease.Assets[0];
                string downloadUrl = asset.BrowserDownloadUrl;

                GI_button.Content = "Downloading";

                Directory.CreateDirectory(assetsFolderPath);
                Directory.CreateDirectory(mintyFolderPath);

                bool downloadSuccess = await DownloadFilesAsync(downloadUrl, zipFilePath, assetsFolderPath, launcherFilePath);
                using (StreamWriter writer = new StreamWriter(verFilePath))
                {
                    await writer.WriteLineAsync(latestReleaseTag);
                }
                if (downloadSuccess)
                {
                    GI_button.Content = "Launch";
                    LaunchExecutable(launcherFilePath);
                }
                else
                {
                    new MessageBox("Failed to download Minty.zip.", MessageType.Error, MessageButtons.Ok).ShowDialog();
                }
            }
            else
            {
                string verText = await File.ReadAllTextAsync(verFilePath);
                Version? localVersion;

                if (!Version.TryParse(verText, out localVersion))
                {
                    new MessageBox($"Incorrect version format in local file: {verText}", MessageType.Error, MessageButtons.Ok).ShowDialog();
                    File.Delete(verFilePath);
                    File.Delete(launcherFilePath);
                    File.Delete(dllFilePath);
                    GI_button.IsEnabled = true;
                    return;
                }

                string githubVersionTag = latestRelease.TagName;
                Version? githubVersion;

                if (!Version.TryParse(githubVersionTag, out githubVersion))
                {
                    new MessageBox($"Incorrect version format on GitHub: {githubVersionTag}", MessageType.Error, MessageButtons.Ok).ShowDialog();
                    return;
                }
                if (latestRelease.Assets.Count == 0)
                {
                    new MessageBox("Minty.zip not found. The file name may not match.", MessageType.Error, MessageButtons.Ok).ShowDialog();
                    return;
                }

                if (localVersion >= githubVersion)
                {
                    GI_button.Content = "Launch";
                    LaunchExecutable(launcherFilePath);
                    return;
                }
                var asset = latestRelease.Assets[0];
                string downloadUrl = asset.BrowserDownloadUrl;

                GI_button.Content = "Downloading";

                File.Delete(verFilePath);
                File.Delete(launcherFilePath);
                File.Delete(dllFilePath);

                bool downloadSuccess = await DownloadFilesAsync(downloadUrl, zipFilePath, assetsFolderPath, launcherFilePath);
                using (StreamWriter writer = new StreamWriter(verFilePath))
                {
                    await writer.WriteLineAsync(latestReleaseTag);
                }
                if (downloadSuccess)
                {
                    GI_button.Content = "Launch";
                    new MessageBox($"Minty updated to version: {await File.ReadAllTextAsync(verFilePath)}", MessageType.Info, MessageButtons.Ok).ShowDialog();
                    LaunchExecutable(launcherFilePath);
                }

            }
        }

        public void LaunchExecutable(string exePath)
        {
            try
            {
                GI_button.IsEnabled = true;
                UpdateRPC("Minty", "Hacking MHY <333");
                Process process = new Process();
                process.StartInfo.FileName = exePath;
                process.StartInfo.UseShellExecute = true;
                process.StartInfo.Verb = "runas";
                process.EnableRaisingEvents = true;
                process.Start();
            }
            catch (Exception ex)
            {
                new MessageBox($"Error launching executable: {ex.Message}", MessageType.Error, MessageButtons.Ok).ShowDialog();
            }
        }
        #endregion
        //Download and Extract
        #region
        public async Task<bool> DownloadFilesAsync(string downloadUrl, string zipFilePath, string assetsFolderPath, string launcherFilePath)
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
        //Rcp
        #region
        private static readonly DiscordRpcClient client = new DiscordRpcClient("1112360491847778344");

        public static void InitializeDiscordRPC()
        {
            if (!client.IsInitialized)
            {
                client.OnReady += (sender, e) => { };

                client.OnPresenceUpdate += (sender, e) => { };

                client.OnError += (sender, e) => { };

                client.Initialize();
            }
        }

        public static void UpdateRPC(string state, string details)
        {
            InitializeDiscordRPC();

            var presence = new RichPresence()
            {
                State = state,
                Details = details,
                Assets = new Assets()
                {
                    LargeImageKey = "idol",
                    SmallImageKey = "gensh",
                    SmallImageText = "Genshin Impact"
                },
                Buttons = new DiscordRPC.Button[]
                {
            new DiscordRPC.Button()
            {
                Label = "Join",
                Url = "https://discord.gg/kindawindytoday"
            }
                }
            };

            client.SetPresence(presence);
        }
        #endregion
        #endregion
    }
}
