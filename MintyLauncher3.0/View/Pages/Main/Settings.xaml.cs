using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using System.Globalization;
using System.Threading;

namespace MintyLauncher3._0.View.Pages.Main
{
    public partial class Settings : Page
    {
        public Settings()
        {
            InitializeComponent();
        }


        //Metods
        #region
        //DeleteGI
        #region
        private void DeleteGI(object sender, RoutedEventArgs e)
        {
            try
            {
                string appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                string mintyFolderPath = System.IO.Path.Combine(appDataFolder, "Minty");
                string assetsFolderPath = System.IO.Path.Combine(mintyFolderPath, "MintyGI");
                Directory.Delete(assetsFolderPath, true);
                new MessageBox("MintyGI Has been deleted.", MessageType.Info, MessageButtons.Ok).ShowDialog();
            }
            catch (Exception ex)
            {
                new MessageBox($"An error occurred while deleting a folder:{ex.Message}", MessageType.Error, MessageButtons.Ok).ShowDialog();
            }
        }
        #endregion
        //DeleteSR
        #region
        private void DeleteSR(object sender, RoutedEventArgs e)
        {
            try
            {
                string appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                string mintyFolderPath = System.IO.Path.Combine(appDataFolder, "minty");
                string assetsFolderPath = System.IO.Path.Combine(mintyFolderPath, "MintySR");
                Directory.Delete(assetsFolderPath, true);
                new MessageBox("MintySR Has been deleted.", MessageType.Info, MessageButtons.Ok).ShowDialog();
            }
            catch (Exception ex)
            {
                new MessageBox($"An error occurred while deleting a folder:{ex.Message}", MessageType.Error, MessageButtons.Ok).ShowDialog();
            }
        }
        #endregion
        //GI_Background
        #region
        private void SelectFileAndMoveGI_Background(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                string selectedFilePath = openFileDialog.FileName;
                string newFileName = "Gi_Background.png";

                if (!string.IsNullOrEmpty(newFileName))
                {
                    string MainFolderPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory);
                    string Images1FolderPath = System.IO.Path.Combine(MainFolderPath, "View");
                    string Images2FolderPath = System.IO.Path.Combine(Images1FolderPath, "Images");
                    string destinationFolderPath = System.IO.Path.Combine(Images2FolderPath, "Background");
                    string newFilePath = System.IO.Path.Combine(destinationFolderPath, newFileName);
                    Directory.CreateDirectory(destinationFolderPath);
                    try
                    {
                        File.Move(selectedFilePath, newFilePath);
                    }
                    catch (Exception ex)
                    {
                        new MessageBox($"Произошла ошибка: {ex.Message}", MessageType.Error, MessageButtons.Ok).ShowDialog();
                    }
                }
                else
                {
                    new MessageBox("Введите новое имя файла.", MessageType.Error, MessageButtons.Ok).ShowDialog();
                }
            }
        }
        #endregion
        //HSR_Background
        #region
        private void SelectFileAndMoveHsr_Background(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                string selectedFilePath = openFileDialog.FileName;
                string newFileName = "Hsr_Background.png";

                if (!string.IsNullOrEmpty(newFileName))
                {
                    string MainFolderPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory);
                    string Images1FolderPath = System.IO.Path.Combine(MainFolderPath, "View");
                    string Images2FolderPath = System.IO.Path.Combine(Images1FolderPath, "Images");
                    string destinationFolderPath = System.IO.Path.Combine(Images2FolderPath, "Background");
                    string newFilePath = System.IO.Path.Combine(destinationFolderPath, newFileName);
                    Directory.CreateDirectory(destinationFolderPath);
                    try
                    {
                        File.Move(selectedFilePath, newFilePath);
                    }
                    catch (Exception ex)
                    {
                        new MessageBox($"Произошла ошибка: {ex.Message}", MessageType.Error, MessageButtons.Ok).ShowDialog();
                    }
                }
                else
                {
                    new MessageBox("Введите новое имя файла.", MessageType.Error, MessageButtons.Ok).ShowDialog();
                }
            }
        }
        #endregion
        #endregion
    }
}
