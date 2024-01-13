using System.Windows;
using System.Windows.Input;

namespace MintyLauncher3._0.View.Windows
{
    public partial class FaQ : Window
    {
        public FaQ()
        {
            InitializeComponent();
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
