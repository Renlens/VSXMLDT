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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace XmlTranslation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Button_Click_1(null, null);
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown(0);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.WindowState = this.WindowState switch
            {
                WindowState.Normal => WindowState.Maximized,
                WindowState.Maximized => WindowState.Normal,
                _ => WindowState.Maximized
            };
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
    }
}
