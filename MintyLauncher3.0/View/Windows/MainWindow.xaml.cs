using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MintyLauncher3._0.View.Windows
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void FandQ(object sender, RoutedEventArgs e)
        {
            FaQ faqWindow = new FaQ(); 
            faqWindow.Show();
        }
        private void DragMove(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
        private void X(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
